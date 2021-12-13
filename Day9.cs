using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code_2021 {
	internal static class DayNine {
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			long result = 0;
			int[,] grid = new int[lines[0].Length,lines.Length];
			{
				int y = 0;
				foreach(string lin in lines) {
					int x = 0;
					char[] ca = lin.ToCharArray();
					foreach(char c in ca) {
						grid[x,y] = int.Parse(c.ToString());
						x++;
					}

					y++;
				}
			}
			{
				for(int y = 0; y < lines.Length; y++) {
					for(int x = 0; x < lines[0].Length; x++) {
						bool isLow = GetLow(x, y, grid, lines[0].Length, lines.Length);
						result += isLow ? 1 + grid[x,y] : 0;
					}
				}
			}
			return result;
		}

		private static bool GetLow(int x, int y, int[,] grid, int mx, int my) {
			int N = (y == 0)?99:grid[x,y-1];
			int W = (x == 0)?99:grid[x-1,y];

			int S = (y == my-1)?99:grid[x,y+1];
			int E = (x == mx-1)?99:grid[x+1,y];

			int L = grid[x,y];

			return (L < N && L < S && L < E && L < W);
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int[,] grid = new int[lines[0].Length,lines.Length];
			{
				int y = 0;
				foreach(string lin in lines) {
					int x = 0;
					char[] ca = lin.ToCharArray();
					foreach(char c in ca) {
						grid[x,y] = int.Parse(c.ToString());
						x++;
					}

					y++;
				}
			}
			List<Vec2> lows = new List<Vec2>();
			{
				for(int y = 0; y < lines.Length; y++) {
					for(int x = 0; x < lines[0].Length; x++) {
						bool isLow = GetLow(x, y, grid, lines[0].Length, lines.Length);
						//result += isLow ? 1 + grid[x,y] : 0;
						if(isLow) lows.Add(new Vec2() { x= x, y= y});
					}
				}
			}
			List<long> sizes = new List<long>();
			foreach(Vec2 p in lows) {
				long a = FindBasin(p, grid, lines[0].Length, lines.Length);
				sizes.Add(a);
			}
			sizes.Sort();
			int mm = sizes.Count-1;
			return sizes[mm-0] * sizes[mm-1] * sizes[mm-2];
		}
		
		private static long FindBasin(Vec2 lowPoint, int[,] grid, int mx, int my) {
			long size = 1;
			int x = lowPoint.x;
			int y = lowPoint.y;
			if(x >= mx) return 0;
			if(y >= my) return 0;
			int L = grid[x,y];
			if(L == 9) return 0;

			int N = (y == 0)?-99:grid[x,y-1];
			int W = (x == 0)?-99:grid[x-1,y];

			int S = (y == my-1)?99:grid[x,y+1];
			int E = (x == mx-1)?99:grid[x+1,y];
			grid[x,y] = 9;
			if(N >= L) {
				size += FindBasin(new Vec2(){x= x, y= y-1}, grid, mx, my);
			}
			if(S >= L) {
				size += FindBasin(new Vec2(){x= x, y= y+1}, grid, mx, my);
			}
			if(W >= L) {
				size += FindBasin(new Vec2(){x= x-1, y= y}, grid, mx, my);
			}
			if(E >= L) {
				size += FindBasin(new Vec2(){x= x+1, y= y}, grid, mx, my);
			}
			return size;
		}

		public struct Vec2 {
			public int x;
			public int y;
		}
	}
}