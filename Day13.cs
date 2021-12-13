using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code_2021 {
	internal static class DayThirteen {
		public struct Vec2 {
			public int x;
			public int y;

			public override bool Equals(object o) {
				if(o is Vec2 v) {
					return v.x == x && v.y == y;
				}
				return false;
			}

			public override int GetHashCode() {
				return x.GetHashCode() + y.GetHashCode();
			}
		}
		public struct Fold {
			public bool isHor;
			public int val;
		}

		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			long result = 0;
			bool dots = true;
			List<Vec2> points = new List<Vec2>();
			List<Fold> folds = new List<Fold>();
			foreach(string lin in lines) {
				if(lin == string.Empty) {
					dots = false;
					continue;
				}
				if(dots) {
					string[] parts = lin.Split(',');
					Vec2 p = new Vec2() {
						x = int.Parse(parts[0]),
						y = int.Parse(parts[1])
					};
					points.Add(p);
				}
				else {
					string[] parts = lin.Split('=');
					Fold f = new Fold() {
						isHor = parts[0] == "fold along y",
						val = int.Parse(parts[1])
					};
					folds.Add(f);
				}
			}

			points = DoFold(points, folds[0]);

			return points.Count;
		}

		public static List<Vec2> DoFold(List<Vec2> points, Fold f) {
			List<Vec2> altered = new List<Vec2>();
			foreach(Vec2 p in points) {
				if(f.isHor) {
					altered.Add(new Vec2() {
						x = p.x,
						y = p.y > f.val ? f.val - (p.y - f.val) : p.y
					});
				}
				else {
					altered.Add(new Vec2() {
						y = p.y,
						x = p.x > f.val ? f.val - (p.x - f.val) : p.x
					});
				}
			}
			Vec2[] arr = altered.Distinct().ToArray();
			return arr.ToList();
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			bool dots = true;
			List<Vec2> points = new List<Vec2>();
			List<Fold> folds = new List<Fold>();
			foreach(string lin in lines) {
				if(lin == string.Empty) {
					dots = false;
					continue;
				}
				if(dots) {
					string[] parts = lin.Split(',');
					Vec2 p = new Vec2() {
						x = int.Parse(parts[0]),
						y = int.Parse(parts[1])
					};
					points.Add(p);
				}
				else {
					string[] parts = lin.Split('=');
					Fold f = new Fold() {
						isHor = parts[0] == "fold along y",
						val = int.Parse(parts[1])
					};
					folds.Add(f);
				}
			}
			foreach(Fold f in folds) {
				points = DoFold(points, f);
			}
			int xm = 2+points.Max(i => i.x);
			int ym = 2+points.Max(i => i.y);
			for(int y = 0; y < ym; y++) {
				for(int x = 0; x < xm; x++) {
					Console.Write(points.Contains(new Vec2(){x=x,y=y}) ? "#" : " ");
				}
				Console.Write("\n");
			}
			Console.WriteLine("ARHZPCUH\n");
			return 0;
		}
	}
}