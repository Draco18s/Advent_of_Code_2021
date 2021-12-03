using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code_2021 {
	internal static class DayThree {
		internal static long Part1(string input) {
			int gamma = 0;
			int epsilon = 0;
			string[] lines = input.Split('\n');
			for(int i = 0; i < lines[0].Length; i++){
				int count = 0;
				foreach(string l in lines) {
					count += l[i] == '1' ? 1 : 0;
				}
				if(count > lines.Length / 2) {
					gamma |= 1 << ((lines[0].Length-1) - i);
				}
				else {
					epsilon |= 1 << ((lines[0].Length - 1) - i);
				}
			}
			return gamma * epsilon;
		}

		internal static long Part2(string input) {
			int oxy = 0;
			int scrub = 0;
			List<string> lines = new List<string>(input.Split('\n'));
			List<int> values = new List<int>();
			foreach(string l in lines) {
				int r = Convert.ToInt32(l, 2);
				values.Add(r);
			}
			oxy = ReduceForOxy(values, lines[0].Length);
			scrub = ReduceForCO(values, lines[0].Length);
			return oxy * scrub;
		}

		private static int ReduceForOxy(List<int> values, int len) {
			for(int i=0; i < len; i++) {
				values = ReduceForOxy(values, len, i);
				if(values.Count == 1)
					return values[0];
			}
			return -1;
		}

		private static List<int> ReduceForOxy(List<int> values, int len, int i) {
			int count = values.Where(x => (x & (1 << ((len - 1) - i))) > 0).Count();
			if(count >= values.Count/2f) {
				values = values.Where(x => (x & (1 << ((len - 1) - i))) > 0).ToList();
			}
			else {
				values = values.Where(x => (x & (1 << ((len - 1) - i))) == 0).ToList();
			}
			return values;
		}

		private static int ReduceForCO(List<int> values, int len) {
			for(int i = 0; i < len; i++) {
				values = ReduceForCO(values, len, i);
				if(values.Count == 1)
					return values[0];
			}

			return -1;
		}

		private static List<int> ReduceForCO(List<int> values, int len, int i) {
			int count = values.Where(x => (x & (1 << ((len - 1) - i))) > 0).Count();
			if(count < values.Count/2f) {
				values = values.Where(x => (x & (1 << ((len - 1) - i))) > 0).ToList();
			}
			else {
				values = values.Where(x => (x & (1 << ((len - 1) - i))) == 0).ToList();
			}
			return values;
		}
	}
}