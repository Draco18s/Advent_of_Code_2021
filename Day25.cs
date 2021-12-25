using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Draco18s.AoCLib;
using System.IO;

namespace Advent_of_Code_2021 {
	internal static class DayTwentyfive {


		internal static long Part1(string input) {
			Grid floor = new Grid(input,true);
			int step = 0;
			while(true) {
				step++;
				floor = TickEast(floor, out bool movedE);
				floor = TickSouth(floor, out bool movedS);
				if(!movedE && !movedS) break;
			}
			//Console.WriteLine(floor.ToString("char+0"));
			return step;
		}

		private static Grid TickEast(Grid gin, out bool didMove) {
			didMove = false;
			Grid gout = new Grid(gin.Width, gin.Height);
			for(int y=gin.MinY; y < gin.MaxY; y++){
				for(int x=gin.MinX; x < gin.MaxX; x++) {
					if((char)gin[x,y] == '>' && (char)gin[(x+1)%gin.MaxX,y] == '.') {
						gout[(x+1)%gin.MaxX,y] = gin[x,y];
						gout[x,y] = '.';
						didMove = true;
					}
					else if((char)gin[x,y] != '.') {
						gout[x,y] = gin[x,y];
					}
					else if(gout[x,y] == 0){
						gout[x,y] = '.';
					}
				}
			}
			return gout;
		}

		private static Grid TickSouth(Grid gin, out bool didMove) {
			didMove = false;
			Grid gout = new Grid(gin.Width, gin.Height);
			for(int y=gin.MinY; y < gin.MaxY; y++){
				for(int x=gin.MinX; x < gin.MaxX; x++) {
					if((char)gin[x,y] == 'v' && (char)gin[x,(y+1)%gin.MaxY] == '.') {
						gout[x,(y+1)%gin.MaxY] = gin[x,y];
						gout[x,y] = '.';
						didMove = true;
					}
					else if((char)gin[x,y] != '.') {
						gout[x,y] = gin[x,y];
					}
					else if(gout[x,y] == 0){
						gout[x,y] = '.';
					}
				}
			}
			return gout;
		}

		internal static long Part2(string input) {
			return 0;
		}
	}
}