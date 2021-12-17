using Advent_of_Code_2020.StatsBuilder;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Draco18s.AoCLib;

namespace Advent_of_Code_2021 {
	static class Program {
		private const string year = "2021";
		private static readonly Uri baseAddress = new Uri("https://adventofcode.com");
		private const string leaderboardURI = "{0}/leaderboard/private/view/{1}.json";
		private static Dictionary<string,List<string>> conf;
		
		private static string puzzleNum = "17";

		static void Main(string[] args) {
			string confj = File.ReadAllText(Path.GetFullPath("./inputs/config.json"));
			conf = JsonSerializer.Deserialize<Dictionary<string,List<string>>>(confj);

			string p = Path.GetFullPath(string.Format("./inputs/day{0}.txt", puzzleNum));
			if(!File.Exists(p)) {
				Task.Run(async () => {
					string puzzleInput = await GetInputFor(puzzleNum, conf.Keys.First());
					File.WriteAllText(p, puzzleInput);
					Main(args);
				});
			}
			else {
				string input = File.ReadAllText(p);
				input = input.Replace("\r", "");
				if(input[input.Length - 1] == '\n')
					input = input.Substring(0, input.Length - 1); //stupid trailing newline
				//string input = @"";
				DateTime s = DateTime.Now;
				long result = DaySeventeen.Part1(input);
				DateTime e = DateTime.Now;
				Console.WriteLine(result);
				Console.WriteLine("Time: " + (e - s).TotalMilliseconds);
				s = DateTime.Now;
				result = DaySeventeen.Part2(input);
				e = DateTime.Now;
				Console.WriteLine(result);
				Console.WriteLine("Time: " + (e - s).TotalMilliseconds);
				//Console.ReadKey();
				//BuildLeaderboard();
				//Console.ReadKey();
			}
			Console.Read();
		}

		static void BuildLeaderboard() {
			Task.Run(async () => {
				AoCLeaderboard obj;
				List<AoCUser> users = new List<AoCUser>();
				foreach(string k in conf.Keys) {
					List<string> boards = conf[k];
					foreach(string b in boards) {
						string input = await GetFromAsync(string.Format(leaderboardURI,year,b), k);
						obj = JsonSerializer.Deserialize<AoCLeaderboard>(input);
						foreach(AoCUser u in obj.members.Values) {
							if(u.name == null) {
								u.name = "(anonymous user #" + u.id + ")";
							}
							if(u.id.Equals("1081403") || users.Contains(u)) continue;
							users.Add(u);
						}
					}
				}

				string mainTable = "<table> <tbody> <tr> <td class=\"typeheader\" colspan=\"3\">(25 items)<span class=\"fixedextenser\">4</span></td></tr><tr> <th title=\"System.String\">day</th> <th title=\"System.String,System.DateTime,System.Int32[]\">silver_order</th> <th title=\"System.String,System.DateTime,System.Int32[]\">gold_order</th> </tr>{0}</tbody></table>";
				StringBuilder builder = new StringBuilder();
				for(int d = 1; d <= 25; d++) {
					if(Count(users, d.ToString(), "1") == 0) break;
					builder.Append(GetTableRow(users, d));
				}
				builder.Append(GetTableRowScores(users));
				string htmlTemplate = File.ReadAllText(Path.GetFullPath("inputs/leaderboard_html.txt"));
				htmlTemplate = htmlTemplate.Replace("{", "{{").Replace("}", "}}").Replace("{{0}}", "{0}");
				string outputFile = Path.GetFullPath("leaderboard.html");
				if(File.Exists(outputFile)) {
					File.Delete(outputFile);
				}
				File.WriteAllText(outputFile, string.Format(htmlTemplate, string.Format(mainTable, builder.ToString())));
			});
		}

		private static async Task<string> GetInputFor(string day, string sessionID) {
			//var url = new Uri(baseAddress + jsonurl);
			var jsonurl = string.Format("{0}{1}/day/{2}/input", baseAddress, year, day);
			var cookieContainer = new CookieContainer();
			cookieContainer.Add(baseAddress, new Cookie("session", sessionID));

			HttpClient httpClient = new HttpClient(
				new HttpClientHandler {
					CookieContainer = cookieContainer,
					AutomaticDecompression = DecompressionMethods.Deflate|DecompressionMethods.GZip,
				}) {
				BaseAddress = baseAddress,
			};
			var response = await httpClient.GetAsync(jsonurl);
			if(response.IsSuccessStatusCode) {
				return await response.Content.ReadAsStringAsync();
			}
			return string.Empty;
		}

		private static async Task<string> GetFromAsync(string jsonurl, string sessionID) {
			//var url = new Uri(baseAddress + jsonurl);
			var cookieContainer = new CookieContainer();
			cookieContainer.Add(baseAddress, new Cookie("session", sessionID));

			HttpClient httpClient = new HttpClient(
				new HttpClientHandler {
					CookieContainer = cookieContainer,
					AutomaticDecompression = DecompressionMethods.Deflate|DecompressionMethods.GZip,
				}) {
				BaseAddress = baseAddress,
			};
			var response = await httpClient.GetAsync(jsonurl);
			if(response.IsSuccessStatusCode) {
				return await response.Content.ReadAsStringAsync();
			}
			return string.Empty;
		}

		private static string GetTableRow(List<AoCUser> users, int d) {
			string tablerowtable = "<tr> <td>{4}</td><td> <table> <tbody> <tr> <td class=\"typeheader\" colspan=\"3\">ø[{2}]&nbsp;<span class=\"fixedextenser\">4</span></td></tr><tr> <th title=\"System.String\">name</th> <th title=\"System.DateTime\">get_star_ts</th> <th title=\"System.Int32\">value</th> </tr>{0}</tbody> </table> </td><td> <table> <tbody> <tr> <td class=\"typeheader\" colspan=\"3\">ø[{3}]&nbsp;<span class=\"fixedextenser\">4</span></td></tr><tr> <th title=\"System.String\">name</th> <th title=\"System.DateTime\">get_star_ts</th> <th title=\"System.Int32\">value</th> </tr>{1}</tbody> </table> </td></tr>";
			string day = d.ToString();
			string[] parts = new string[2];
			parts[0] = "";
			parts[1] = "";
			for(int p = 1; p <= 2; p++) {
				string part = p.ToString();
				SortUsers(ref users, day, part);
				foreach(AoCUser user in users) {
					int pts = GetPointsForUser(ref users, user.id, day, part);
					if(d > 0)
						user.locPoints += pts;
					parts[p - 1] += GetUserLineScore(user, day, part, pts);
				}
			}
			return string.Format(tablerowtable, parts[0], parts[1], Count(users, day, "1"), Count(users, day, "2"), day);
		}

		private static string GetTableRowScores(List<AoCUser> users) {
			users.Sort((x, y) => y.locPoints.CompareTo(x.locPoints));
			string tablerowtable = "<tr> <td>{2}</td><td> <table> <tbody> <tr> <td class=\"typeheader\" colspan=\"3\">ø[{1}]&nbsp;<span class=\"fixedextenser\">4</span></td></tr><tr> <th title=\"System.String\">name</th> <th title=\"System.Int32\">total</th> </tr>{0}</tbody> </table> </td> </table> </td></tr>";
			string row = "";
			foreach(AoCUser user in users) {
				int pts = user.locPoints;
				//user.locPoints += pts;
				row += GetUserTotalScore(user, pts);
			}
			return string.Format(tablerowtable, row, users.Count, "Total");
		}

		private static int Count(List<AoCUser> users, string day, string part) {
			return users.Count(x => {
				return x.completion_day_level.ContainsKey(day) && x.completion_day_level[day].ContainsKey(part);
			});
		}

		private static void SortUsers(ref List<AoCUser> users, string day, string part) {
			users.Sort((x, y) => {
				bool xhas = true;
				bool yhas = true;
				if(!y.completion_day_level.ContainsKey(day) || !y.completion_day_level[day].ContainsKey(part)) {
					yhas = false;
				}
				if(!x.completion_day_level.ContainsKey(day) || !x.completion_day_level[day].ContainsKey(part)) {
					xhas = false;
				}
				if(xhas && yhas)
					return x.completion_day_level[day][part].dateTime.CompareTo(y.completion_day_level[day][part].dateTime);
				else
					return yhas.CompareTo(xhas);
			});
		}

		private static int GetPointsForUser(ref List<AoCUser> users, string user, string day, string part) {
			int i = users.FindIndex(x => x.id == user);
			if(users[i].completion_day_level.ContainsKey(day) && users[i].completion_day_level[day].ContainsKey(part))
				return users.Count - i;
			return 0;
		}

		private static string GetUserLineScore(AoCUser user, string day, string part, int pts) {
			string p = "<tr><td>{0}</td><td>{1}</td><td class=\"n\">{2}</td></tr>";
			if(user.completion_day_level.ContainsKey(day) && user.completion_day_level[day].ContainsKey(part))
				return string.Format(p, user.name, user.completion_day_level[day][part].dateTime.ToString("M/d/yyyy h:mm:ss tt"), pts);
			long.TryParse(user.last_star_ts.ToString(), out long last);
			if(last == 0)
				return string.Empty;
			return string.Format(p, user.name, "----", "0");
		}

		private static string GetUserTotalScore(AoCUser user, int pts) {
			if(pts == 0) return string.Empty;
			string p = "<tr><td>{0}</td><td class=\"n\">{1}</td></tr>";
			return string.Format(p, user.name, pts.ToString());
		}
	}
}
