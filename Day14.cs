using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent_of_Code_2021 {
	internal static class DayFourteen {

		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			string template = lines[0];
			bool waitForBlank = true;
			Dictionary<string,string> rules = new Dictionary<string,string>();
			foreach(string lin in lines) {
				if(waitForBlank) {
					if(lin == string.Empty)
						waitForBlank = false;
					continue;
				}
				string[] parts = lin.Split(" -> ");
				rules.Add(parts[0],parts[1]);
			}
			string polymer = template;
			for(int i=0; i < 10; i++) {
				string newpoly = "";
				for(int l = 0; l < polymer.Length-1; l++) {
					string s = polymer.Substring(l,2);
					if(rules.TryGetValue(s, out string ins)) {
						newpoly += s[0] + ins;
					}
					else {
						newpoly += s[0];
					}
				}
				newpoly += polymer[polymer.Length-1];
				polymer = newpoly;
			}
			int max = polymer.GroupBy(x => x).Max(grp => grp.Count());
			int min = polymer.GroupBy(x => x).Min(grp => grp.Count());
			//Console.WriteLine(polymer);
			return max-min;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			string template = lines[0];
			bool waitForBlank = true;
			Dictionary<string,string> rules = new Dictionary<string,string>();
			foreach(string lin in lines) {
				if(waitForBlank) {
					if(lin == string.Empty)
						waitForBlank = false;
					continue;
				}
				string[] parts = lin.Split(" -> ");
				rules.Add(parts[0],parts[1]);
			}
			Dictionary<string,long> poly = new Dictionary<string,long>();
			for(int l = 0; l < template.Length-1; l++) {
				string s = template.Substring(l,2);
				if(poly.ContainsKey(s))
					poly[s]++;
				else
					poly.Add(s, 1);
			}
			for(int i=0; i < 40; i++) {
				Dictionary<string,long> poly2 = new Dictionary<string,long>();
				foreach(string k in poly.Keys) {
					if(rules.TryGetValue(k, out string ins)) {
						long c = poly[k];
						string nk = k[0] + ins;
						if(poly2.ContainsKey(nk))
							poly2[nk]+=c;
						else
							poly2.Add(nk, c);
						nk = ins + k[1];
						if(poly2.ContainsKey(nk))
							poly2[nk]+=c;
						else
							poly2.Add(nk, c);
					}
					else {
						if(poly2.ContainsKey(k))
							poly2[k]++;
						else
							poly2.Add(k, 1);
					}
				}
				poly = poly2;
			}
Dictionary<char,long> counts = new Dictionary<char,long>();
counts.Add(template[template.Length-1],1);
foreach(string k in poly.Keys) {
	char a = k[0];
	if(counts.ContainsKey(a))
		counts[a] += poly[k];
	else
		counts.Add(a, poly[k]);
}
			long max = counts.Values.Max();
			long min = counts.Values.Min();
			return max-min;
		}
	}
}