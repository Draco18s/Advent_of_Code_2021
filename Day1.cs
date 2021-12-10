using System;

namespace Advent_of_Code_2021 {
	internal static class DayOne {
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			int last = -1;
			long i = 0;
			foreach(string lin in lines) {
				int.TryParse(lin, out int a);
				if(last > 0 && a > last) {
					i++;
				}
				last = a;
			}
			return i;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int plast = 0;
			int last = 0;
			long i = 0;
			int a = 0;
			int b = 0;
			int c = 0;
			foreach(string lin in lines) {
				int.TryParse(lin, out int d);
				if(a == 0 || b == 0) {
					c = b;
					b = a;
					a = d;
					last += d;
					continue;
				}
				
				last -= c;
				c = b;
				b = a;
				a = d;
				last += d;
				if(plast > 0 && last > plast) {
					i++;
				}
				plast = last;
			}
			return i;
		}
	}
}