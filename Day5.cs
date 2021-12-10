using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code_2021 {
	internal static class DayFive {
		public struct Vec2 {
			public int x;
			public int y;
		}
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			int[,] grid = new int[1000,1000];
			List<Tuple<Vec2,Vec2>> allLines = new List<Tuple<Vec2,Vec2>>();
			foreach(string lin in lines) {
				string[] parts = lin.Split(" -> ");
				//Console.WriteLine(lin);
				if(lin == string.Empty) break;
				int a = int.Parse(parts[0].Split(',')[0]);
				int b = int.Parse(parts[0].Split(',')[1]);
				int c = int.Parse(parts[1].Split(',')[0]);
				int d = int.Parse(parts[1].Split(',')[1]);

				if(a > c) {
					a ^= c;
					c ^= a;
					a ^= c;
				}
				if(b > d) {
					b ^= d;
					d ^= b;
					b ^= d;
				}
				Vec2 s = new Vec2() {
					x = a,
					y = b,
				};
				Vec2 e = new Vec2() {
					x = c,
					y = d,
				};
				if(s.x != e.x && s.y != e.y) {
					continue;
				}
				allLines.Add(new Tuple<Vec2,Vec2>(s,e));
			}
			Console.WriteLine(allLines.Count());
			foreach(Tuple<Vec2,Vec2> line in allLines) {
				for(int x = line.Item1.x; x <= line.Item2.x; x++) {
					for(int y = line.Item1.y; y <= line.Item2.y; y++) {
						grid[x,y]++;
					}
				}
			}
			int t = 0;
			for(int y = 0; y < 1000; y++) {
				for(int x = 0; x < 1000; x++) {
					t += grid[x,y] > 1 ? 1 : 0;
				}
			}
			return t;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int[,] grid = new int[1000,1000];
			List<Tuple<Vec2,Vec2>> allLines = new List<Tuple<Vec2,Vec2>>();
			List<Tuple<Vec2,Vec2>> diagonals = new List<Tuple<Vec2,Vec2>>();
			foreach(string lin in lines) {
				string[] parts = lin.Split(" -> ");
				//Console.WriteLine(lin);
				if(lin == string.Empty) break;
				int a = int.Parse(parts[0].Split(',')[0]);
				int b = int.Parse(parts[0].Split(',')[1]);
				int c = int.Parse(parts[1].Split(',')[0]);
				int d = int.Parse(parts[1].Split(',')[1]);

				if(a > c) {
					a ^= c;
					c ^= a;
					a ^= c;
				}
				if(b > d) {
					b ^= d;
					d ^= b;
					b ^= d;
				}
				Vec2 s = new Vec2() {
					x = a,
					y = b,
				};
				Vec2 e = new Vec2() {
					x = c,
					y = d,
				};
				if(s.x != e.x && s.y != e.y) {
					a = int.Parse(parts[0].Split(',')[0]);
					b = int.Parse(parts[0].Split(',')[1]);
					c = int.Parse(parts[1].Split(',')[0]);
					d = int.Parse(parts[1].Split(',')[1]);
					if(a > c) {
						a ^= c;
						c ^= a;
						a ^= c;
						b ^= d;
						d ^= b;
						b ^= d;
					}
					s = new Vec2() {
						x = a,
						y = b,
					};
					e = new Vec2() {
						x = c,
						y = d,
					};
					diagonals.Add(new Tuple<Vec2,Vec2>(s,e));
				}
				else {
					allLines.Add(new Tuple<Vec2,Vec2>(s,e));
				}
			}
			foreach(Tuple<Vec2,Vec2> line in allLines) {
				for(int x = line.Item1.x; x <= line.Item2.x; x++) {
					for(int y = line.Item1.y; y <= line.Item2.y; y++) {
						grid[x,y]++;
					}
				}
			}
			foreach(Tuple<Vec2,Vec2> line in diagonals) {
				int yy = line.Item1.y;
				for(int x = line.Item1.x; x <= line.Item2.x; x++) {
					grid[x,yy]++;
					if(line.Item2.y > line.Item1.y) {
						yy++;
					}
					else {
						yy--;
					}
				}
			}
			int t = 0;
			for(int y = 0; y < 1000; y++) {
				for(int x = 0; x < 1000; x++) {
					t += grid[x,y] > 1 ? 1 : 0;
				}
			}
			return t;
		}
	}
}