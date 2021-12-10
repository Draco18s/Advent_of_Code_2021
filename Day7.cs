using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code_2021 {
	internal static class DaySeven {
		internal static long Part1(string input) {
			string[] lines = input.Split(',');
			List<int> crabs = new List<int>();
			foreach(string n in lines) {
				crabs.Add(int.Parse(n));
			}
			int meetat = crabs.Min();
			long best = int.MaxValue;
			int max = crabs.Max();
			for(int i = meetat; i <= max; i++) {
				long fuel = crabs.Sum(c => Math.Abs(c - i));
				//Console.WriteLine(i + "	" + fuel);
				if(fuel < best) {
					meetat = i;
					best = fuel;
				}
			}
			//Console.WriteLine("Meet: "+meetat);
			return best;
			//return 0;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split(',');
			List<int> crabs = new List<int>();
			foreach(string n in lines) {
				crabs.Add(int.Parse(n));
			}
			int meetat = crabs.Min();
			long best = int.MaxValue;
			int max = crabs.Max();
			for(int i = meetat; i <= max; i++) {
				long fuel = crabs.Sum(c => {
					float j = Math.Abs(c-i);
					float rr = (j/2)*(j+1);
					return (int)rr;
				});
				//Console.WriteLine(i + "	" + fuel);
				if(fuel < best) {
					meetat = i;
					best = fuel;
				}
			}
			return best;
		}
	}
}