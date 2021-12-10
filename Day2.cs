using System;

namespace Advent_of_Code_2021 {
	internal static class DayTwo {
		enum Direction {
			forward,down,up
		}
		internal static long Part1(string input) {
			int h = 0;
			int d = 0;
			string[] lines = input.Split('\n');

			foreach(string l in lines) {
				string[] parts = l.Split(' ');
				Enum.TryParse<Direction>(parts[0], out Direction r);
				int.TryParse(parts[1], out int v);
				switch(r) {
					case Direction.forward:
						h += v;
						break;
					case Direction.up:
						d -= v;
						break;
					case Direction.down:
						d += v;
						break;
				}
			}

			return h*d;
		}

		internal static long Part2(string input) {
			int h = 0;
			int a = 0;
			int d = 0;
			string[] lines = input.Split('\n');

			foreach(string l in lines) {
				string[] parts = l.Split(' ');
				Enum.TryParse<Direction>(parts[0], out Direction r);
				int.TryParse(parts[1], out int v);
				switch(r) {
					case Direction.forward:
						h += v;
						d += a * v;
						break;
					case Direction.up:
						a -= v;
						break;
					case Direction.down:
						a += v;
						break;
				}
			}

			return h * d;
		}
	}
}