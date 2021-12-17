using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Draco18s.AoCLib;
using System.IO;

namespace Advent_of_Code_2021 {
	internal static class DaySeventeen {
		

		internal static long Part1(string input) {
			Regex rx = new Regex("(\\d+)");
			var matches = Regex.Matches(input, "(-?\\d+)");
			List<int> list = new List<int>();
			foreach(Match m in matches) {
				list.Add(int.Parse(m.Value));
			}
			Vector2 a = new Vector2(Math.Min(list[0], list[1]),Math.Max(list[2], list[3]));
			Vector2 b = new Vector2(Math.Max(list[0], list[1]),Math.Min(list[2], list[3]));
			
			int velX;
			int velY = 0;
			if(a.x > 0) {
				velX = 1;
			}
			else {
				velX = -1;
			}
			int maxY = -1;
			int result = -1;
			int lastVelX = velX;
			while(result < 0) {
				result = ComputeTrajectory(velX, velY, a, b);
				if(result >= 0)
					break;
				if(result == -1 || result == -4) {
					if(velX >= a.x) {
						break;
					}
					velX += Math.Sign(velX);
				}
				if(result == -2 || Math.Abs(velX) >= Math.Abs(b.x)){
					velX /= Math.Abs(velX);
					velY++;
				}
				if(velX >= 500 || velY > 500) {
					return -1;
				}
				if(lastVelX == velX) {
					return -1;
				}
				lastVelX = velX;
			}
			while(result >= 0) {
				result = ComputeTrajectory(velX, velY, a, b);
				if(result > maxY)
					maxY = result;
				if(result >= 0)
					velX += Math.Sign(velX);
				else if(result == -2 || result == -3 || Math.Abs(velX) >= Math.Abs(b.x)){
					velX /= Math.Abs(velX);
					velY++;
					result = 0;
				}
				else if(velX >= 500 || velY > 500) {
					break;
				}
				else if (result < 0) {
					velX += Math.Sign(velX);
					result = 0;
				}
			}
			return maxY;
		}

		private static int ComputeTrajectory(int xv, int yv, Vector2 min, Vector2 max) {
			Vector2 pos = new Vector2(0,0);
			int maxY = 0;
			while(true) {
				bool wasAboveTarget = pos.y > min.y;
				bool wasNearerMin = Math.Abs(pos.x) < Math.Abs(min.x);
				pos = new Vector2(pos.x + xv, pos.y+yv);
				if(maxY < pos.y)
					maxY = pos.y;
				xv -= 1 * Math.Sign(xv);
				yv--;
				if(xv == 0 && Math.Abs(pos.x) < Math.Abs(min.x)) {
					//undershot
					return -1;
				}
				if(wasNearerMin && Math.Abs(pos.x) > Math.Abs(max.x)) {
					//overshot
					return -2;
				}
				if(pos.x >= min.x && pos.x <= max.x && pos.y <= min.y && pos.y >= max.y) {
					//in the hole
					return maxY;
				}
				if(pos.y < max.y) {
					if(wasAboveTarget) {
						//dropped past
						return -4;
					}
					//did not enter, fell past
					return -1;
				}
			}
		}

		private static int ComputeTrajectory2(int xv, int yv, Vector2 min, Vector2 max) {
			Vector2 pos = new Vector2(0,0);
			while(true) {
				bool wasAboveTarget = pos.y > min.y;
				bool wasNearerMin = Math.Abs(pos.x) < Math.Abs(min.x);
				pos = new Vector2(pos.x + xv, pos.y+yv);
				xv -= 1 * Math.Sign(xv);
				yv--;
				if(pos.x >= min.x && pos.x <= max.x && pos.y <= min.y && pos.y >= max.y) {
					//in the hole
					return 1;
				}
				if(xv == 0 && Math.Abs(pos.x) < Math.Abs(min.x)) {
					//undershot
					return -1;
				}
				if(wasNearerMin && Math.Abs(pos.x) > Math.Abs(max.x)) {
					//overshot
					return -2;
				}
				if(pos.y < max.y) {
					if(wasAboveTarget) {
						//dropped past
						return -4;
					}
					//did not enter, fell past
					return -1;
				}
			}
		}


		internal static long Part2(string input) {
		Regex rx = new Regex("(\\d+)");
			//Regex ry = new Regex("y=(\\d+)\\.\\.(\\d+)");
			var matches = Regex.Matches(input, "(-?\\d+)");
			List<int> list = new List<int>();
			foreach(Match m in matches) {
				list.Add(int.Parse(m.Value));
			}
			Vector2 a = new Vector2(Math.Min(list[0], list[1]),Math.Max(list[2], list[3]));
			Vector2 b = new Vector2(Math.Max(list[0], list[1]),Math.Min(list[2], list[3]));
			int velX;
			int velY = b.y;
			if(a.x > 0) {
				velX = 1;
			}
			else {
				velX = -1;
			}
			int result = -1;
			int lastVelX = velX;
			long count = 0;
			while(true) {
				result = ComputeTrajectory2(velX, velY, a, b);
				if(result > 0) {
					count++;
				}
				velX++;
				if(velX > b.x) {
					velX /= Math.Abs(velX);
					velY++;
				}
				if(velY > 2000) {
					break;
				}
				if(lastVelX == velX) {
					break;
				}
				lastVelX = velX;
			}
			return count;
		}
	}
}