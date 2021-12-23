using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Draco18s.AoCLib;
using System.IO;

namespace Advent_of_Code_2021 {
	internal static class DayTwentytwo {
		
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			Grid3D reactor = new Grid3D(101,101,101,-50,-50,-50);
			int minimum = 9;
			int maximum = -9;
			foreach(string lin in lines) {
				if(lin == string.Empty) continue;
				var onoff = Regex.Matches(lin, "([fno]+) ");
				var numbers = Regex.Matches(lin, "(-?\\d+)");
				bool turnOn = false;
				List<int> list = new List<int>();
				foreach(Match m in numbers) {
					list.Add(int.Parse(m.Value));
				}
				foreach(Match m in onoff) {
					turnOn = m.Value == "on ";
				}
				minimum = Math.Min(minimum,list.Min());
				maximum = Math.Max(maximum,list.Max());
				if(list.Min() < -50) continue;
				if(list.Max() >  50) continue;
				// x   X   y   Y   z   Z
				(int,int,int,int,int,int) cube = (list[0],list[1],list[2],list[3],list[4],list[5]);

				for(int x = cube.Item1; x <= cube.Item2; x++) {
					for(int y = cube.Item3; y <= cube.Item4; y++) {
						for(int z = cube.Item5; z <= cube.Item6; z++) {
							reactor[x,y,z] = turnOn ? 1 : 0;
						}
					}
				}
			}
			long numOn = 0;
			for(int x = reactor.MinX; x < reactor.MaxX; x++) {
				for(int y = reactor.MinY; y < reactor.MaxX; y++) {
					for(int z = reactor.MinZ; z < reactor.MaxX; z++) {
						numOn += reactor[x,y,z];
					}
				}
			}
			return numOn;
		}

		/*internal static long Part2(string input) {
			(Vector3,Vector3) B = (new Vector3(0,0,0), new Vector3(10,10,10));
			(Vector3,Vector3) A = (new Vector3(2,8,-2), new Vector3(8,12,2));
			List<(Vector3,Vector3)> newRegions = Split(A, B, true);
			List<(Vector3,Vector3)> newRegionsAdj = new List<(Vector3,Vector3)>();
			foreach((Vector3,Vector3) a in newRegions) {
				newRegionsAdj.Add((
					new Vector3(Math.Min(a.Item1.x,a.Item2.x),Math.Min(a.Item1.y,a.Item2.y),Math.Min(a.Item1.z,a.Item2.z)),
					new Vector3(Math.Max(a.Item1.x,a.Item2.x),Math.Max(a.Item1.y,a.Item2.y),Math.Max(a.Item1.z,a.Item2.z))
				));
			}
			List<(Vector3,Vector3)> ff = new List<(Vector3,Vector3)>();
			ff.AddRange(newRegionsAdj.Select(a=>a).Where(a => newRegionsAdj.Any(b=> ShareFaceX(a, b))));
			ff.AddRange(newRegionsAdj.Select(a=>a).Where(a => newRegionsAdj.Any(b=> ShareFaceY(a, b))));
			ff.AddRange(newRegionsAdj.Select(a=>a).Where(a => newRegionsAdj.Any(b=> ShareFaceZ(a, b))));
			foreach((Vector3, Vector3) r1 in ff) {
				if(!newRegionsAdj.Contains(r1)) continue;
				if(newRegionsAdj.Any(b=> ShareFaceZ(r1, b))) {
					(Vector3, Vector3) r2 = newRegionsAdj.Select(a=>a).Where(b=> ShareFaceX(r1, b)).First();
					newRegionsAdj.Remove(r1);
					newRegionsAdj.Remove(r2);
					newRegionsAdj.Add((new Vector3(r1.Item1.x, r1.Item1.y, Math.Min(r1.Item1.z,r2.Item1.z)),new Vector3(r1.Item1.x, r1.Item1.y, Math.Max(r1.Item2.z,r2.Item2.z))));
				}
				if(newRegionsAdj.Any(b=> ShareFaceY(r1, b))) {
					(Vector3, Vector3) r2 = newRegionsAdj.Select(a=>a).Where(b=> ShareFaceY(r1, b)).First();
					newRegionsAdj.Remove(r1);
					newRegionsAdj.Remove(r2);
					newRegionsAdj.Add((new Vector3(r1.Item1.x, Math.Min(r1.Item1.y,r2.Item1.y), r1.Item1.z),new Vector3(r1.Item1.x, Math.Max(r1.Item2.y,r2.Item2.y), r1.Item1.z)));
				}
				if(newRegionsAdj.Any(b=> ShareFaceX(r1, b))) {
					(Vector3, Vector3) r2 = newRegionsAdj.Select(a=>a).Where(b=> ShareFaceX(r1, b)).First();
					newRegionsAdj.Remove(r1);
					newRegionsAdj.Remove(r2);
					newRegionsAdj.Add((new Vector3(Math.Min(r1.Item1.x,r2.Item1.x), r1.Item1.y, r1.Item1.z),new Vector3(Math.Max(r1.Item2.x,r2.Item2.x), r1.Item1.x, r1.Item1.z)));
				}
			}
			return 0;
		}*/
		
		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			long finalResult = 0;
			List<(bool,(Vector3,Vector3))> instructions = new List<(bool,(Vector3,Vector3))>();

			foreach(string lin in lines) {
				if(lin == string.Empty) continue;
				var onoff = Regex.Matches(lin, "([fno]+) ");
				var numbers = Regex.Matches(lin, "(-?\\d+)");
				bool turnOn = false;
				List<int> list = new List<int>();
				foreach(Match m in numbers) {
					list.Add(int.Parse(m.Value));
				}
				foreach(Match m in onoff) {
					turnOn = m.Value == "on ";
				}
				// x   X   y   Y   z   Z
				(Vector3,Vector3) cube = (new Vector3(list[0],list[2],list[4]), new Vector3(list[1],list[3],list[5]));
				instructions.Add((turnOn, cube));
			}
			List<(Vector3,Vector3)> onRegions = new List<(Vector3,Vector3)>();
			foreach((bool,(Vector3,Vector3)) instruc in instructions) {
				List<(Vector3,Vector3)> overlaps = onRegions.Select(a=>a).Where(a => ComputeDoesOverlap(a, instruc.Item2) > 0).ToList();
				if(overlaps.Count == 0 && instruc.Item1) {
					onRegions.Add(instruc.Item2);
					continue;
				}
				foreach((Vector3,Vector3) overl in overlaps) {
					onRegions.Remove(overl);
					List<(Vector3,Vector3)> newRegions = Split(overl, instruc.Item2, instruc.Item1);
					List<(Vector3,Vector3)> newRegionsAdj = new List<(Vector3,Vector3)>();
					foreach((Vector3,Vector3) a in newRegions) {
						newRegionsAdj.Add((
							new Vector3(Math.Min(a.Item1.x,a.Item2.x),Math.Min(a.Item1.y,a.Item2.y),Math.Min(a.Item1.z,a.Item2.z)),
							new Vector3(Math.Max(a.Item1.x,a.Item2.x),Math.Max(a.Item1.y,a.Item2.y),Math.Max(a.Item1.z,a.Item2.z))
						));
					}
					List<(Vector3,Vector3)> ff = new List<(Vector3,Vector3)>();
					ff.AddRange(newRegionsAdj.Select(a=>a).Where(a => newRegionsAdj.Any(b=> ShareFaceX(a, b))));
					ff.AddRange(newRegionsAdj.Select(a=>a).Where(a => newRegionsAdj.Any(b=> ShareFaceY(a, b))));
					ff.AddRange(newRegionsAdj.Select(a=>a).Where(a => newRegionsAdj.Any(b=> ShareFaceZ(a, b))));
					foreach((Vector3, Vector3) r1 in ff) {
						if(!newRegionsAdj.Contains(r1)) continue;
						if(newRegionsAdj.Any(b=> ShareFaceZ(r1, b))) {
							(Vector3, Vector3) r2 = newRegionsAdj.Select(a=>a).Where(b=> ShareFaceZ(r1, b)).First();
							newRegionsAdj.Remove(r1);
							newRegionsAdj.Remove(r2);
							newRegionsAdj.Add((new Vector3(r1.Item1.x, r1.Item1.y, Math.Min(r1.Item1.z,r2.Item1.z)),new Vector3(r1.Item1.x, r1.Item1.y, Math.Max(r1.Item2.z,r2.Item2.z))));
						}
						if(newRegionsAdj.Any(b=> ShareFaceY(r1, b))) {
							(Vector3, Vector3) r2 = newRegionsAdj.Select(a=>a).Where(b=> ShareFaceY(r1, b)).First();
							newRegionsAdj.Remove(r1);
							newRegionsAdj.Remove(r2);
							newRegionsAdj.Add((new Vector3(r1.Item1.x, Math.Min(r1.Item1.y,r2.Item1.y), r1.Item1.z),new Vector3(r1.Item1.x, Math.Max(r1.Item2.y,r2.Item2.y), r1.Item1.z)));
						}
						if(newRegionsAdj.Any(b=> ShareFaceX(r1, b))) {
							(Vector3, Vector3) r2 = newRegionsAdj.Select(a=>a).Where(b=> ShareFaceX(r1, b)).First();
							newRegionsAdj.Remove(r1);
							newRegionsAdj.Remove(r2);
							newRegionsAdj.Add((new Vector3(Math.Min(r1.Item1.x,r2.Item1.x), r1.Item1.y, r1.Item1.z),new Vector3(Math.Max(r1.Item2.x,r2.Item2.x), r1.Item1.x, r1.Item1.z)));
						}
					}
					onRegions.AddRange(newRegionsAdj);
				}
			}
			return finalResult;
		}

		private static bool ShareFaceX((Vector3,Vector3) a, (Vector3,Vector3) b) {
			bool part1 = a.Item1.z == b.Item1.z && a.Item1.y == b.Item1.y && a.Item2.z == b.Item2.z && a.Item2.y == b.Item2.y;

			return part1 && (a.Item2.x+1 == b.Item1.x || b.Item2.x+1 == a.Item1.x);
		}

		private static bool ShareFaceY((Vector3,Vector3) a, (Vector3,Vector3) b) {
			bool part1 = a.Item1.x == b.Item1.x && a.Item1.z == b.Item1.z && a.Item2.x == b.Item2.x && a.Item2.z == b.Item2.z;

			return part1 && (a.Item2.y+1 == b.Item1.y || b.Item2.z+1 == a.Item1.y);
		}

		private static bool ShareFaceZ((Vector3,Vector3) a, (Vector3,Vector3) b) {
			bool part1 = a.Item1.x == b.Item1.x && a.Item1.y == b.Item1.y && a.Item2.x == b.Item2.x && a.Item2.y == b.Item2.y;

			return part1 && (a.Item2.z+1 == b.Item1.z || b.Item2.z+1 == a.Item1.z);
		}

		private static List<(Vector3,Vector3)> Split((Vector3,Vector3) area, (Vector3,Vector3) instruc, bool turnOn) {
			List<(Vector3,Vector3)> newRegions = new List<(Vector3,Vector3)>();
			int overlaptype = 0;

			overlaptype |= (area.Item1.x <= instruc.Item1.x && area.Item2.x >= instruc.Item2.x) ? 1 : 0;
			overlaptype |= (area.Item1.y <= instruc.Item1.y && area.Item2.y >= instruc.Item2.y) ? 2 : 0;
			overlaptype |= (area.Item1.z <= instruc.Item1.z && area.Item2.z >= instruc.Item2.z) ? 4 : 0;

			overlaptype |= (area.Item1.x >= instruc.Item1.x && area.Item2.x <= instruc.Item2.x) ? 1<<3 : 0;//8
			overlaptype |= (area.Item1.y >= instruc.Item1.y && area.Item2.y <= instruc.Item2.y) ? 2<<3 : 0;//16
			overlaptype |= (area.Item1.z >= instruc.Item1.z && area.Item2.z <= instruc.Item2.z) ? 4<<3 : 0;//32
			//Console.WriteLine(overlaptype);
			if(turnOn) {
				if(overlaptype == 0) {
					// todo
					// corner overlaps
					// do as subtract?
					newRegions.Add(area);
					newRegions.Add(instruc);
					return newRegions;
				}
				if((overlaptype&7) == 7) { //area fully contains instruction
					newRegions.Add(area);
					return newRegions;
				}
				if(((overlaptype>>3)&7) == 7) { //instruction fully contains area
					newRegions.Add(instruc);
					return newRegions;
				}
				if((overlaptype&7) == 6) { // instruct sticks out on X
					newRegions.Add(area);
					if(area.Item1.x < instruc.Item1.x) { // positive direction
						newRegions.Add((new Vector3(area.Item2.x+1,instruc.Item1.y,instruc.Item1.z),instruc.Item2));
					}
					else { // negative direction
						newRegions.Add((instruc.Item1, new Vector3(area.Item1.x-1,instruc.Item2.y,instruc.Item2.z)));
					}
					return newRegions;
				}
				if((overlaptype&7) == 5) { // Y
					newRegions.Add(area);
					if(area.Item1.y < instruc.Item1.y) {
						newRegions.Add((new Vector3(instruc.Item1.x,area.Item2.y+1,instruc.Item1.z),instruc.Item2));
					}
					else {
						newRegions.Add((instruc.Item1, new Vector3(instruc.Item2.x,area.Item1.y-1,instruc.Item2.z)));
					}
					return newRegions;
				}
				if((overlaptype&7) == 3) { // Z
					newRegions.Add(area);
					if(area.Item1.z < instruc.Item1.z) {
						newRegions.Add((new Vector3(instruc.Item1.x,instruc.Item1.y,area.Item2.z+1),instruc.Item2));
					}
					else {
						newRegions.Add((instruc.Item1, new Vector3(instruc.Item2.x,instruc.Item2.y,area.Item1.z-1)));
					}
					return newRegions;
				}
				if(((overlaptype>>3)&7) == 6) { // area sticks out on X
					newRegions.Add(instruc);
					if(instruc.Item1.x < area.Item1.x) { // positive direction
						newRegions.Add((new Vector3(instruc.Item2.x+1,area.Item1.y,area.Item1.z),area.Item2));
					}
					else { // negative direction
						newRegions.Add((area.Item1, new Vector3(instruc.Item1.x-1,area.Item2.y,area.Item2.z)));
					}
					return newRegions;
				}
				if(((overlaptype>>3)&7) == 5) { // Y
					newRegions.Add(instruc);
					if(instruc.Item1.y < area.Item1.y) {
						newRegions.Add((new Vector3(area.Item1.x,instruc.Item2.y+1,area.Item1.z),area.Item2));
					}
					else {
						newRegions.Add((area.Item1, new Vector3(area.Item2.x,instruc.Item1.y-1,area.Item2.z)));
					}
					return newRegions;
				}
				if(((overlaptype>>3)&7) == 3) { // Z
					newRegions.Add(instruc);
					if(instruc.Item1.z < area.Item1.z) {
						newRegions.Add((new Vector3(area.Item1.x,area.Item1.y,instruc.Item2.z+1),area.Item2));
					}
					else {
						newRegions.Add((area.Item1, new Vector3(area.Item2.x,area.Item2.y,instruc.Item1.z-1)));
					}
					return newRegions;
				}
				if((overlaptype&7) == 4 && ((overlaptype>>3)&7) == 0) {
					int midpoint = (instruc.Item1.y > area.Item1.y ? area.Item2.y : area.Item1.y);
					if(midpoint == area.Item1.y) {
						newRegions.Add((new Vector3(instruc.Item1.x,instruc.Item1.y,instruc.Item1.z),new Vector3(instruc.Item2.x,midpoint-1,instruc.Item2.z)));
						newRegions.AddRange(Split(area, (new Vector3(instruc.Item1.x,midpoint,instruc.Item1.z),new Vector3(instruc.Item2.x,instruc.Item2.y,instruc.Item2.z)), turnOn));
					}
					else {
						newRegions.Add((new Vector3(instruc.Item1.x,midpoint+1,instruc.Item1.z),new Vector3(instruc.Item2.x,instruc.Item2.y,instruc.Item2.z)));
						newRegions.AddRange(Split(area, (new Vector3(instruc.Item1.x,instruc.Item1.y,instruc.Item1.z),new Vector3(instruc.Item2.x,midpoint,instruc.Item2.z)), turnOn));
					}
					return newRegions;
				}
				if(((overlaptype>>3)&7) == 4 && (overlaptype&7) == 0) {
					int midpoint = (area.Item1.y > instruc.Item1.y ? instruc.Item2.y : instruc.Item1.y);
					if(midpoint == instruc.Item1.y) {
						newRegions.Add((new Vector3(area.Item1.x,area.Item1.y,area.Item1.z),new Vector3(area.Item2.x,midpoint-1,area.Item2.z)));
						newRegions.AddRange(Split(instruc, (new Vector3(area.Item1.x,midpoint,area.Item1.z),new Vector3(area.Item2.x,area.Item2.y,area.Item2.z)), turnOn));
					}
					else {
						newRegions.Add((new Vector3(area.Item1.x,midpoint+1,area.Item1.z),new Vector3(area.Item2.x,area.Item2.y,area.Item2.z)));
						newRegions.AddRange(Split(instruc, (new Vector3(area.Item1.x,area.Item1.y,area.Item1.z),new Vector3(area.Item2.x,midpoint,area.Item2.z)), turnOn));
					}
					return newRegions;
				}
				if((overlaptype&7) == 2 && ((overlaptype>>3)&7) == 0) {
					int midpoint = (instruc.Item1.z > area.Item1.z ? area.Item2.z : area.Item1.z);
					if(midpoint == area.Item1.z) {
						newRegions.Add((new Vector3(instruc.Item1.x,instruc.Item1.y,instruc.Item1.z),new Vector3(instruc.Item2.x,instruc.Item2.y,midpoint-1)));
						newRegions.AddRange(Split(area, (new Vector3(instruc.Item1.x,instruc.Item1.y,midpoint),new Vector3(instruc.Item2.x,instruc.Item2.y,instruc.Item2.z)), turnOn));
					}
					else {
						newRegions.Add((new Vector3(instruc.Item1.x,instruc.Item1.y,midpoint+1),new Vector3(instruc.Item2.x,instruc.Item2.y,instruc.Item2.z)));
						newRegions.AddRange(Split(area, (new Vector3(instruc.Item1.x,instruc.Item1.y,instruc.Item1.z),new Vector3(instruc.Item2.x,instruc.Item2.y,midpoint)), turnOn));
					}
					return newRegions;
				}
				if(((overlaptype>>3)&7) == 2 && (overlaptype&7) == 0) {
					int midpoint = (area.Item1.z > instruc.Item1.z ? instruc.Item2.z : instruc.Item1.z);
					if(midpoint == instruc.Item1.z) {
						newRegions.Add((new Vector3(area.Item1.x,area.Item1.y,area.Item1.z),new Vector3(area.Item2.x,area.Item2.y,midpoint-1)));
						newRegions.AddRange(Split(instruc, (new Vector3(area.Item1.x,area.Item1.y,midpoint),new Vector3(area.Item2.x,area.Item2.y,area.Item2.z)), turnOn));
					}
					else {
						newRegions.Add((new Vector3(area.Item1.x,area.Item1.y,midpoint+1),new Vector3(area.Item2.x,area.Item2.y,area.Item2.z)));
						newRegions.AddRange(Split(instruc, (new Vector3(area.Item1.x,area.Item1.y,area.Item1.z),new Vector3(area.Item2.x,area.Item2.y,midpoint)), turnOn));
					}
					return newRegions;
				}
				if((overlaptype&7) == 1 && ((overlaptype>>3)&7) == 0) {
					int midpoint = (instruc.Item1.z > area.Item1.z ? area.Item2.z : area.Item1.z);
					if(midpoint == area.Item1.z) {
						newRegions.Add((new Vector3(instruc.Item1.x,instruc.Item1.y,instruc.Item1.z),new Vector3(instruc.Item2.x,instruc.Item2.y,midpoint-1)));
						newRegions.AddRange(Split(area, (new Vector3(instruc.Item1.x,instruc.Item1.y,midpoint),new Vector3(instruc.Item2.x,instruc.Item2.y,instruc.Item2.z)), turnOn));
					}
					else {
						newRegions.Add((new Vector3(instruc.Item1.x,instruc.Item1.y,midpoint+1),new Vector3(instruc.Item2.x,instruc.Item2.y,instruc.Item2.z)));
						newRegions.AddRange(Split(area, (new Vector3(instruc.Item1.x,instruc.Item1.y,instruc.Item1.z),new Vector3(instruc.Item2.x,instruc.Item2.y,midpoint)), turnOn));
					}
					return newRegions;
				}
				if(((overlaptype>>3)&7) == 1 && (overlaptype&7) == 0) {
					int midpoint = (area.Item1.z > instruc.Item1.z ? instruc.Item2.z : instruc.Item1.z);
					if(midpoint == instruc.Item1.z) {
						newRegions.Add((new Vector3(area.Item1.x,area.Item1.y,area.Item1.z),new Vector3(area.Item2.x,area.Item2.y,midpoint-1)));
						newRegions.AddRange(Split(instruc, (new Vector3(area.Item1.x,area.Item1.y,midpoint),new Vector3(area.Item2.x,area.Item2.y,area.Item2.z)), turnOn));
					}
					else {
						newRegions.Add((new Vector3(area.Item1.x,area.Item1.y,midpoint+1),new Vector3(area.Item2.x,area.Item2.y,area.Item2.z)));
						newRegions.AddRange(Split(instruc, (new Vector3(area.Item1.x,area.Item1.y,area.Item1.z),new Vector3(area.Item2.x,area.Item2.y,midpoint)), turnOn));
					}
					return newRegions;
				}
				if((overlaptype&7) == 1) {
					// split area into 3 pieces on X
					newRegions.Add((area.Item1,new Vector3(instruc.Item1.x-1,area.Item2.y,area.Item2.z)));
					(Vector3,Vector3) mid = (new Vector3(instruc.Item1.x,area.Item1.y,area.Item1.z),new Vector3(instruc.Item2.x,area.Item2.y,area.Item2.z));
					newRegions.Add((new Vector3(instruc.Item2.x+1,area.Item1.y,area.Item1.z),area.Item2));
					newRegions.AddRange(Split(mid,instruc,turnOn));
					return newRegions;
				}
				if((overlaptype&7) == 2) {
					// split area into 3 pieces on Y
					newRegions.Add((area.Item1,new Vector3(area.Item2.x,instruc.Item1.y-1,area.Item2.z)));
					(Vector3,Vector3) mid = (new Vector3(area.Item1.x,instruc.Item1.y,area.Item1.z),new Vector3(area.Item2.x,instruc.Item2.y,area.Item2.z));
					newRegions.Add((new Vector3(area.Item1.x,instruc.Item2.y+1,area.Item1.z),area.Item2));
					newRegions.AddRange(Split(mid,instruc,turnOn));
					return newRegions;
				}
				if((overlaptype&7) == 4) {
					// split area into 3 pieces on Z
					newRegions.Add((area.Item1,new Vector3(area.Item2.x,area.Item2.y,instruc.Item1.z-1)));
					(Vector3,Vector3) mid = (new Vector3(area.Item1.x,area.Item1.y,instruc.Item1.z),new Vector3(area.Item2.x,area.Item2.y,instruc.Item2.z));
					newRegions.Add((new Vector3(area.Item1.x,area.Item1.y,instruc.Item2.z+1),area.Item2));
					newRegions.AddRange(Split(mid,instruc,turnOn));
					return newRegions;
				}
				throw new Exception("Some other kind of additive " + overlaptype);
			}
			else {
				if(((overlaptype>>3)&7) == 7) { //obliterated
					return newRegions;
				}
				if(((overlaptype>>3)&7) == 6 && (overlaptype&7) == 0) { //truncated on X
					if(area.Item1.x < instruc.Item1.x && area.Item2.x <= instruc.Item2.x)
						newRegions.Add((area.Item1,new Vector3(instruc.Item1.x,area.Item1.y,area.Item1.z)));
					else if(instruc.Item1.x < area.Item1.x && instruc.Item2.x <= area.Item2.x)
						newRegions.Add((new Vector3(instruc.Item1.x,area.Item2.y,area.Item1.z),area.Item2));
					else
						throw new Exception("AAA1");
					return newRegions;
				}
				else {
					//split in 3
				}
				if(((overlaptype>>3)&7) == 5 && (overlaptype&7) == 0) { //truncated on Y
					if(area.Item1.y < instruc.Item1.y && area.Item2.y <= instruc.Item2.y)
						newRegions.Add((area.Item1,new Vector3(area.Item1.x,instruc.Item1.y,area.Item1.z)));
					else if(instruc.Item1.y < area.Item1.y && instruc.Item2.y <= area.Item2.y)
						newRegions.Add((new Vector3(area.Item2.x,instruc.Item1.y,area.Item1.z),area.Item2));
					else
						throw new Exception("AAA2");
					return newRegions;
				}
				else {
					//split in 3
				}
				if(((overlaptype>>3)&7) == 3 && (overlaptype&7) == 0) { //truncated on Z
					if(area.Item1.z < instruc.Item1.z && area.Item2.z <= instruc.Item2.z)
						newRegions.Add((area.Item1,new Vector3(area.Item1.x,area.Item1.y,instruc.Item1.z)));
					else if(instruc.Item1.z < area.Item1.z && instruc.Item2.z <= area.Item2.z)
						newRegions.Add((new Vector3(area.Item2.x,area.Item1.y,instruc.Item1.z),area.Item2));
					else
						throw new Exception("AAA3");
					return newRegions;
				}
				else {
					//split in 3
				}
				Console.WriteLine("Some other kind of subtractive " + overlaptype);
			}
			return newRegions;
		}

		private static long GetVolume((Vector3,Vector3) area) {
			int dx = area.Item2.x-area.Item1.x+1;
			int dy = area.Item2.y-area.Item1.y+1;
			int dz = area.Item2.z-area.Item1.z+1;
			return dx*dy*dz;
		}

		private static int ComputeDoesOverlap((Vector3,Vector3) area, (Vector3,Vector3) instruc) {
			if(area.Item1.x > instruc.Item2.x || area.Item2.x < instruc.Item1.x) return -1;
			if(area.Item1.y > instruc.Item2.y || area.Item2.y < instruc.Item1.y) return -1;
			if(area.Item1.z > instruc.Item2.z || area.Item2.z < instruc.Item1.z) return -1;

			if(area.Item1.x == instruc.Item2.x || area.Item2.x == instruc.Item1.x) return 0;
			if(area.Item1.y == instruc.Item2.y || area.Item2.y == instruc.Item1.y) return 0;
			if(area.Item1.z == instruc.Item2.z || area.Item2.z == instruc.Item1.z) return 0;
			
			int ret = 0;
			if(area.Item1.x < instruc.Item2.x && area.Item2.x > instruc.Item1.x) ret |= 1;
			if(area.Item1.y < instruc.Item2.y && area.Item2.y > instruc.Item1.y) ret |= 2;
			if(area.Item1.z < instruc.Item2.z && area.Item2.z > instruc.Item1.z) ret |= 4;

			return ret;
		}

		/*private static long ComputeSubgrid(Grid3D reactor, List<(bool,(int,int,int,int,int,int))> instructions) {
			(int,int,int,int,int,int) cube;
			bool turnOn;
			foreach((bool,(int,int,int,int,int,int)) instruc in instructions) {
				turnOn = instruc.Item1;
				cube = instruc.Item2;

				if(cube.Item1 < reactor.MinX && cube.Item2 < reactor.MinX) continue;
				if(cube.Item3 < reactor.MinY && cube.Item4 < reactor.MinY) continue;
				if(cube.Item5 < reactor.MinZ && cube.Item6 < reactor.MinZ) continue;

				if(cube.Item1 >= reactor.MaxX && cube.Item2 >= reactor.MaxX) continue;
				if(cube.Item3 >= reactor.MaxY && cube.Item4 >= reactor.MaxY) continue;
				if(cube.Item5 >= reactor.MaxZ && cube.Item6 >= reactor.MaxZ) continue;

				for(int x = cube.Item1; x <= cube.Item2; x++) {
					if(x < reactor.MinX || x >= reactor.MaxX) continue;
					for(int y = cube.Item3; y <= cube.Item4; y++) {
						if(y < reactor.MinY || y >= reactor.MaxY) continue;
						for(int z = cube.Item5; z <= cube.Item6; z++) {
							if(z < reactor.MinZ || z >= reactor.MaxZ) continue;
							reactor[x,y,z] = turnOn ? 1 : 0;
						}
					}
				}
			}
			long numOn = 0;
			for(int x = reactor.MinX; x < reactor.MaxX; x++) {
				for(int y = reactor.MinY; y < reactor.MaxY; y++) {
					for(int z = reactor.MinZ; z < reactor.MaxZ; z++) {
						numOn += reactor[x,y,z];
					}
				}
			}
			return numOn;
		}*/
	}
}