using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code_2021 {
	internal static class DayEleven {
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			long result = 0;

			int xm = lines[0].Length;
			int ym = lines.Length;
			int[,] grid = new int[xm,ym];
			for(int y =0; y < ym; y++) {
				for(int x =0; x < xm; x++) {
					grid[x,y] = int.Parse(lines[y][x].ToString());
				}
			}
			for(int step = 0; step < 100; step++) {
				for(int y =0; y < ym; y++) {
					for(int x =0; x < xm; x++) {
						grid[x,y]++;
					}
				}
				result += ComputeFlashes(grid, xm, ym);

				/*for(int y =0; y < ym; y++) {
					for(int x =0; x < xm; x++) {
						Console.Write(grid[x,y].ToString("X"));
					}
					Console.Write("\n");
				}
				Console.Write("\n");
				Console.Write("\n");*/
			}
			return result;
		}

		internal static long ComputeFlashes(int[,] grid, int xm, int ym) {
			long ret = 0;
			bool changed = true;
			while(changed) {
				changed = false;
				for(int y =0; y < ym; y++) {
					for(int x =0; x < xm; x++) {
						if(grid[x,y] > 9) {
							changed = true;
							ret++;
							grid[x,y] = -1;

							if (y>0 && grid[x,y-1] >= 0) {
								grid[x,y-1]++;
							}
							if(y+1<ym && grid[x,y+1] >= 0) {
								grid[x,y+1]++;
							} 
							if(x+1<xm && grid[x+1,y] >= 0) {
								grid[x+1,y]++;
							} 
							if(x>0 && grid[x-1,y] >= 0) {
								grid[x-1,y]++;
							} 
							
							if (x>0 &&y>0 && grid[x-1,y-1] >= 0) {
								grid[x-1,y-1]++;
							}
							if(y+1<ym&&x+1<xm && grid[x+1,y+1] >= 0) {
								grid[x+1,y+1]++;
							} 
							if(y>0 &&x+1<xm && grid[x+1,y-1] >= 0) {
								grid[x+1,y-1]++;
							} 
							if(y+1<ym&&x>0 && grid[x-1,y+1] >= 0) {
								grid[x-1,y+1]++;
							}
						}
					}
				}
			}
			for(int y =0; y < ym; y++) {
				for(int x =0; x < xm; x++) {
					if(grid[x,y] < 0)
						grid[x,y]=0;
				}
			}
			return ret;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			long result = 0;

			int xm = lines[0].Length;
			int ym = lines.Length;
			int[,] grid = new int[xm,ym];
			for(int y =0; y < ym; y++) {
				for(int x =0; x < xm; x++) {
					grid[x,y] = int.Parse(lines[y][x].ToString());
				}
			}
			for(int step = 0; step < 1000; step++) {
				for(int y =0; y < ym; y++) {
					for(int x =0; x < xm; x++) {
						grid[x,y]++;
					}
				}
				result = ComputeFlashes(grid, xm, ym);
				if(result == xm*ym) {
					return step+1;
				}
			}
			return -1;
		}
	}
}