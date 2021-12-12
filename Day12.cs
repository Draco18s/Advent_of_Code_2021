using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code_2021 {
	internal static class DayTwelve {
		public class Cave {
			public string name = "";
			public bool isLarge = false;
			public bool isEnd = false;
			public List<Cave> connections = new List<Cave>();

			public void AddConnection(Cave c) {
				this.connections.Add(c);
				c.connections.Add(this);
			}

			public void PlotRoute(ref List<List<Cave>> routes) {
				List<List<Cave>> ourRoutes = routes.Where(l => l.Last() == this).ToList();
				if(this.isEnd) {
					foreach(List<Cave> rout in ourRoutes) {
						rout.Add(this);
					}
					return;
				}

				foreach(List<Cave> rout in ourRoutes) {
					routes.Remove(rout);
					foreach(Cave c in connections) {
						if(c.name == "start") continue;
						if(c == this) continue;
						if(c.isLarge || !rout.Contains(c)) {
							List<Cave> clonedList = new List<Cave>(rout);
							clonedList.Add(c);
							routes.Add(clonedList);
							c.PlotRoute(ref routes);
						}
					}
				}
			}
		}

		public class Cave2 {
			public string name = "";
			public bool isLarge = false;
			public bool isEnd = false;
			public List<Cave2> connections = new List<Cave2>();

			public override string ToString() {
				return "Cave " + name;
			}

			public void AddConnection(Cave2 c) {
				this.connections.Add(c);
				c.connections.Add(this);
			}

			public void PlotRoute(ref List<List<Cave2>> routes) {
				List<List<Cave2>> ourRoutes = routes.Where(l => l.Last() == this).ToList();
				if(this.isEnd) {
					/*foreach(List<Cave2> rout in ourRoutes) {
						rout.Add(this);
					}*/
					return;
				}

				foreach(List<Cave2> rout in ourRoutes) {
					int a = routes.Count;
					routes.Remove(rout);
					foreach(Cave2 c in connections) {
						if(c.name == "start") continue;
						if(c == this) continue;
						List<Cave2> ls = rout.Where(x => x.name != "start" && char.ToUpper(x.name[0]) != x.name[0]).ToList();
						bool allowed = true;//ls.Count == 0;

						var g = ls.GroupBy( i => i );
						foreach(var grp in g) {
							//Console.WriteLine( "{0} {1}", grp.Key, grp.Count() );
							if(grp.Count() > 1) {
								allowed = false;
							}
						}

						if(c.isLarge || allowed || !rout.Contains(c)) {
							List<Cave2> clonedList = new List<Cave2>(rout);
							clonedList.Add(c);
							routes.Add(clonedList);
							c.PlotRoute(ref routes);
						}
					}
					if(a > routes.Count) {
						//Console.WriteLine("AAA");
					}
				}
			}
		}

		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			//long result = 0;

			Cave start = null;
			Cave end = null;
			Dictionary<string,Cave> caveMap = new Dictionary<string,Cave>();
			foreach(string lin in lines) {
				string[] bits = lin.Split('-');
				if(bits[0] == "start" && start == null){
					start = new Cave();
					start.name = "start";
					caveMap.Add("start", start);
				}
				else if(bits[0] == "end" && end == null){
					end = new Cave();
					end.name = "end";
					end.isEnd = true;
					caveMap.Add("end", end);
				}
				else {
					Cave c = new Cave();
					c.name = bits[0];
					caveMap.TryAdd(bits[0], c);
				}
				if(bits[1] == "end" && end == null){
					end = new Cave();
					end.name = "end";
					end.isEnd = true;
					caveMap.Add("end", end);
				}
				else if(bits[1] == "start" && start == null){
					start = new Cave();
					start.name = "start";
					caveMap.Add("start", start);
				}
				else {
					Cave c = new Cave();
					c.name = bits[1];
					caveMap.TryAdd(bits[1], c);
				}
				Cave one = caveMap[bits[0]];
				Cave two = caveMap[bits[1]];
				one.AddConnection(two);
				if(bits[0][0] == char.ToUpper(bits[0][0])) {
					caveMap[bits[0]].isLarge = true;
				}
				if(bits[1][0] == char.ToUpper(bits[0][0])) {
					caveMap[bits[1]].isLarge = true;
				}
			}
			caveMap = caveMap
			.Where(kv => kv.Value.isLarge || kv.Value.connections.Count > 1)
			.ToDictionary(kv => kv.Key, kv => kv.Value);

			List<List<Cave>> allRoutes = new List<List<Cave>>();
			List<Cave> r = new List<Cave>();
			r.Add(start);
			allRoutes.Add(r);
			start.PlotRoute(ref allRoutes);
			
			return allRoutes.Count;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			//long result = 0;

			Cave2 start = null;
			Cave2 end = null;
			Dictionary<string,Cave2> caveMap = new Dictionary<string,Cave2>();
			foreach(string lin in lines) {
				string[] bits = lin.Split('-');
				if(bits[0] == "start" && start == null){
					start = new Cave2();
					start.name = "start";
					caveMap.Add("start", start);
				}
				else if(bits[0] == "end" && end == null){
					end = new Cave2();
					end.name = "end";
					end.isEnd = true;
					caveMap.Add("end", end);
				}
				else {
					Cave2 c = new Cave2();
					c.name = bits[0];
					caveMap.TryAdd(bits[0], c);
				}
				if(bits[1] == "end" && end == null){
					end = new Cave2();
					end.name = "end";
					end.isEnd = true;
					caveMap.Add("end", end);
				}
				else if(bits[1] == "start" && start == null){
					start = new Cave2();
					start.name = "start";
					caveMap.Add("start", start);
				}
				else {
					Cave2 c = new Cave2();
					c.name = bits[1];
					caveMap.TryAdd(bits[1], c);
				}
				Cave2 one = caveMap[bits[0]];
				Cave2 two = caveMap[bits[1]];
				one.AddConnection(two);
				if(bits[0][0] == char.ToUpper(bits[0][0])) {
					caveMap[bits[0]].isLarge = true;
				}
				if(bits[1][0] == char.ToUpper(bits[1][0])) {
					caveMap[bits[1]].isLarge = true;
				}
			}

			caveMap = caveMap
			.Where(kv => {
				bool b = kv.Value.isLarge || kv.Value.connections.Count > 1 || kv.Value.connections[0].isLarge;
				return b;
			})
			.ToDictionary(kv => kv.Key, kv => kv.Value);

			List<List<Cave2>> allRoutes = new List<List<Cave2>>();
			List<Cave2> r = new List<Cave2>();
			r.Add(start);
			allRoutes.Add(r);
			start.PlotRoute(ref allRoutes);
			
			return allRoutes.Count;
		}
	}
}