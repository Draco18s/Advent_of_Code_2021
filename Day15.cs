using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Draco18s.AoCLib;
using System.IO;

namespace Advent_of_Code_2021 {
	internal static class DayFifteen {

		public class Cell {
			public int cost;
			public int costToHere = int.MaxValue;
			List<Vector2> neighbors = new List<Vector2>();
			long beenHere = -1;

			public Cell(int v) {
				cost = v;
				neighbors.Add(new Vector2(1,0));
				neighbors.Add(new Vector2(0,1));
			}

			public override string ToString() {
				return cost.ToString();
			}

			public long PathCost(int x, int y, Cell[,] grid) {
				if(beenHere > 0) 
					return beenHere;
				if(x+1 == grid.GetLength(0) && y+1 == grid.GetLength(1)) {
					return cost;
				}
				else {
					long best = long.MaxValue;
					foreach(Vector2 n in neighbors) {
						if(!(x+n.x < grid.GetLength(0) && y+n.y < grid.GetLength(1))) {
							continue;
						}
						long costtotal = grid[x+n.x, y+n.y].PathCost(x+n.x, y+n.y, grid);
						if(costtotal < best) {
							best = costtotal;
						}
					}
					beenHere = best+cost;
				}
				return beenHere;
			}
		}

		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			Cell[,] grid = new Cell[lines[0].Length,lines.Length];
			for(int y = 0; y < lines.Length; y++) {
				for(int x = 0; x < lines[0].Length; x++) {
					grid[x,y] = new Cell(int.Parse(lines[y][x].ToString()));
				}
			}

			long lowCost = grid[0,0].PathCost(0,0,grid) - grid[0,0].cost;
			return lowCost;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			Cell[,] grid = new Cell[lines[0].Length,lines.Length];
			Cell[,] lgrid = new Cell[lines[0].Length*5,lines.Length*5];
			for(int y = 0; y < lines.Length; y++) {
				for(int x = 0; x < lines[0].Length; x++) {
					grid[x,y] = new Cell(int.Parse(lines[y][x].ToString()));
				}
			}

			for(int ty=0; ty < 5; ty++) {
				for(int tx=0; tx < 5; tx++) {
					for(int x=0; x < lines.Length; x++) {
						for(int y=0; y < lines[0].Length; y++) {
							int px = lines[0].Length*tx+x;
							int py = lines.Length*ty+y;
							int bonus = tx+ty;
							int cc = grid[x,y].cost+bonus;
							while(cc > 9)
								cc -= 9;
							lgrid[px,py] = new Cell(cc);
						}
					}
				}
			}

			List<Vector2> open = new List<Vector2>();
			open.Add(new Vector2(0,0));
			lgrid[0,0].costToHere = 0;

			while(open.Count > 0) {
				open = open.OrderBy(p => p.x+p.y).ToList();
				Vector2 current = open.First();
				open.Remove(current);

				int toCurrent = lgrid[current.x,current.y].costToHere;
				if(current.x+1 < lgrid.GetLength(0)) {
					Vector2 p = new Vector2(current.x+1,current.y);
					int orig = lgrid[p.x,p.y].costToHere;
					lgrid[p.x,p.y].costToHere = Math.Min(lgrid[p.x,p.y].costToHere,lgrid[p.x,p.y].cost+toCurrent);
					if(!open.Contains(p) && orig > lgrid[p.x,p.y].costToHere)
						open.Add(p);
				}
				if(current.y+1 < lgrid.GetLength(1)) {
					Vector2 p = new Vector2(current.x,current.y+1);
					int orig = lgrid[p.x,p.y].costToHere;
					lgrid[p.x,p.y].costToHere = Math.Min(lgrid[p.x,p.y].costToHere,lgrid[p.x,p.y].cost+toCurrent);
					if(!open.Contains(p) && orig > lgrid[p.x,p.y].costToHere)
						open.Add(p);
				}
				if(current.x-1 >= 0) {
					Vector2 p = new Vector2(current.x-1,current.y);
					int orig = lgrid[p.x,p.y].costToHere;
					lgrid[p.x,p.y].costToHere = Math.Min(lgrid[p.x,p.y].costToHere,lgrid[p.x,p.y].cost+toCurrent);
					if(!open.Contains(p) && orig > lgrid[p.x,p.y].costToHere)
						open.Add(p);
				}
				if(current.y-1 >= 0) {
					Vector2 p = new Vector2(current.x,current.y-1);
					int orig = lgrid[p.x,p.y].costToHere;
					lgrid[p.x,p.y].costToHere = Math.Min(lgrid[p.x,p.y].costToHere,lgrid[p.x,p.y].cost+toCurrent);
					if(!open.Contains(p) && orig > lgrid[p.x,p.y].costToHere)
						open.Add(p);
				}
			}
			
			return lgrid[lines[0].Length*5-1,lines.Length*5-1].costToHere;
		}
	}
}