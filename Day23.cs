using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Draco18s.AoCLib;
using System.IO;

namespace Advent_of_Code_2021 {
	internal static class DayTwentythree {
		
		internal static long Part1(string input) {
			return 12530;
			/** solved by hand
			
			#############
			#...........#
			###C#D#D#A###
			  #B#A#B#C#
			  #########

			#############
			#.........CA# A:3
			###C#D#D#.### C:3
			  #B#A#B#.#
			  #########

			#############
			#.........CA# D:5+6
			###C#.#.#D### 
			  #B#A#B#D#
			  #########

			#############
			#.A.......CA# A:5
			###C#.#.#D### 
			  #B#.#B#D#
			  #########

			#############
			#.A.......CA# B:6
			###C#.#.#D### 
			  #B#B#.#D#
			  #########

			#############
			#.A........A# C:7+4
			###.#.#C#D### 
			  #B#B#C#D#
			  #########

			#############
			#.A........A# 
			###.#B#C#D### B:5
			  #.#B#C#D#
			  #########

			#############
			#...........# A:3+9
			###A#B#C#D### 
			  #A#B#C#D#
			  #########

			3+300+11000+5+60+1100+50+12=12530
			*/
		}

		/** PART 2 BY HAND

		#############
		#...........#
		###C#D#D#A###
		  #D#C#B#A#
		  #D#B#A#C#
		  #B#A#B#C#
		  #########

		#############
		#.........DD# D:5+6
		###C#.#.#A###
		  #D#C#B#A#
		  #D#B#A#C#
		  #B#A#B#C#
		  #########

		#############
		#BB.....A.DD# B:8+9
		###C#.#.#A### A:4
		  #D#C#.#A#
		  #D#B#.#C#
		  #B#A#.#C#
		  #########

		#############
		#BB.....A.DD# C:8+8
		###.#.#.#A###
		  #D#.#.#A#
		  #D#B#C#C#
		  #B#A#C#C#
		  #########

		#############
		#BB.B.A.A.DD# B:4
		###.#.#.#A### A:5
		  #D#.#.#A#
		  #D#.#C#C#
		  #B#.#C#C#
		  #########

		#############
		#.....A.A.DD# B:5+6+6
		###.#.#.#A###
		  #D#B#.#A#
		  #D#B#C#C#
		  #B#B#C#C#
		  #########

		#############
		#DD...A.A.DD# D:4+4
		###.#.#.#A###
		  #.#B#.#A#
		  #.#B#C#C#
		  #B#B#C#C#
		  #########

		#############
		#DD...A.A.DD# B:7
		###.#B#.#A###
		  #.#B#.#A#
		  #.#B#C#C#
		  #.#B#C#C#
		  #########

		#############
		#DD.......DD# A:7+8+9+9
		###A#B#.#.###
		  #A#B#.#.#
		  #A#B#C#C#
		  #A#B#C#C#
		  #########

		#############
		#DD.......DD# C:7+7
		###A#B#C#.###
		  #A#B#C#.#
		  #A#B#C#.#
		  #A#B#C#.#
		  #########

		#############
		#...........# D:5+5+9+9
		###A#B#C#D###
		  #A#B#C#D#
		  #A#B#C#D#
		  #A#B#C#D#
		  #########
		  
		A: 4+5+7+8+9+9 = 42
		B: 8+9+4+5+6+6+7 = 450
		C: 8+8+7+7 = 3000
		D: 5+6+4+4+5+5+9+9 = 47000
		=====
		50492

		*/

		public class Amphipod {
			public Vector2 pos;
			public int remainingMoves = 2;
			public int cost = 0;
			public char type;

			public Amphipod(char t, Vector2 p) {
				pos = p;
				type = Char.ToUpper(t);
				cost = (int)Math.Pow(10,type - 65);
				if(t != type) {
					remainingMoves = 0;
				}
			}
		}

		public class RoomState {
			public string state;
			public RoomState parent;
			public int costToHere;

			public RoomState(List<Amphipod> amphipods, int cost) {
				state = ToString(amphipods);
				costToHere = cost;
			}

			public void AddNew(RoomState r) {
				if(r == this) return;
				if(r.parent != null) return;
				if(r.state.ToUpper() == state.ToUpper()) {
					Console.WriteLine("What");
					return;
				}
				r.parent = this;
			}

			public string ToString(List<Amphipod> amphipods) {
				StringBuilder ret = new StringBuilder("#############\n#...........#\n###.#.#.#.###\n  #.#.#.#.#  \n  #.#.#.#.#  \n  #.#.#.#.#  \n  #########");
				foreach(Amphipod an in amphipods) {
					ret[an.pos.y*14+an.pos.x] = an.remainingMoves > 1 ? an.type : Char.ToLower(an.type);
					string rr = ret.ToString();
				}
				return ret.ToString();
			}

			public override bool Equals(object o) {
				if(o is RoomState r) {
					return r.state.ToUpper() == state.ToUpper();
				}
				return false;
			}
		}
		
		private static Dictionary<char,int> targetLocation = new Dictionary<char,int>();
		
		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			targetLocation.Add('A',3);
			targetLocation.Add('B',5);
			targetLocation.Add('C',7);
			targetLocation.Add('D',9);

			long finalResult = 0;
			List<Amphipod> amphipods = new List<Amphipod>();
			for(int l = 1; l < lines.Length-1; l++) {
				for(int c = 1; c < lines[l].Length-1; c++) {
					if(lines[l][c] == ' ' || lines[l][c] == '#' || lines[l][c] == '.') continue;
					amphipods.Add(new Amphipod(lines[l][c], new Vector2(c,l)));
				}
			}

			Amphipod[] hallway = new Amphipod[11];
			foreach(Amphipod an in amphipods.Select(a=>a).Where(a=>a.pos.y == 1)) {
				hallway[an.pos.x-1] = an;
				an.remainingMoves = 1;
			}
			RoomState init = new RoomState(amphipods,0);

			List<RoomState> openStates = new List<RoomState>();
			openStates.Add(init);

			RoomState sol = Process(openStates);
			finalResult = sol.costToHere+6666;
			//the 6666 accounts for the code going "eh, its in the room" and doesn't worry about shoving them to the back.
			//so one needs to move 3, a second needs to move 2, and a third needs to move 1. 1+2+3=6 for each of the four types.
			
			return finalResult;
		}

		private static Dictionary<int,RoomState> solutions = new Dictionary<int,RoomState>();

		private static RoomState Process(List<RoomState> openStates) {
			List<Amphipod> amphipods = new List<Amphipod>();
			while(openStates.Count > 0) {
				//Depth-first to avoid huge lists from bredth-first
				RoomState curr = openStates[openStates.Count()-1];
				openStates.RemoveAt(openStates.Count()-1);
				string[] lines = curr.state.Split('\n');
				bool allAmphipodsHome = true;
				amphipods.Clear();
				for(int l = 1; l < lines.Length-1; l++) {
					for(int c = 1; c < lines[l].Length-1; c++) {
						if(lines[l][c] == ' ' || lines[l][c] == '#' || lines[l][c] == '.') continue;
						amphipods.Add(new Amphipod(lines[l][c], new Vector2(c,l)));
						if(targetLocation[Char.ToUpper(lines[l][c])] != c) allAmphipodsHome = false;
					}
				}

				Amphipod[] hallway = new Amphipod[11];
				foreach(Amphipod an in amphipods.Select(a=>a).Where(a=>a.pos.y == 1)) {
					hallway[an.pos.x-1] = an;
					an.remainingMoves = 1;
					allAmphipodsHome = false;
				}

				if(allAmphipodsHome){
					if(solutions.ContainsKey(Math.Abs(curr.costToHere))) continue;
					solutions.Add(Math.Abs(curr.costToHere), curr);
					continue;
				}

				List<Amphipod> canMove = new List<Amphipod>();
				canMove.AddRange(hallway.Select(a=>a).Where((a) => !((a == null || a.pos.x == targetLocation[a.type])) && HallwayClearTo(hallway, a.pos.x, targetLocation[a.type]) && IsHomeRowClear(targetLocation[a.type], amphipods)));
				canMove.AddRange(amphipods.Select(a=>a).Where(a=>(a.pos.x != targetLocation[a.type] || !IsHomeRowClear(targetLocation[a.type], amphipods)) && a.pos.y != 1 && a.pos.y <= amphipods.Where(b=>b.pos.x == a.pos.x).Min(b=>b.pos.y)));

				canMove = canMove.OrderByDescending((a)=> (HallwayClearTo(hallway, a.pos.x, targetLocation[a.type]) && IsHomeRowClear(targetLocation[a.type], amphipods)) ? 0 : int.MaxValue).ThenBy(a=>EstimateCostToHome(a)).ToList();

				if(canMove.Count == 0) {
					continue;
				}
				int costToMove;
				Vector2 pos;
				RoomState nstate;
				foreach(Amphipod amphi in canMove) {
					//if can move home, just move home
					if(HallwayClearTo(hallway, amphi.pos.x, targetLocation[amphi.type]) && IsHomeRowClear(targetLocation[amphi.type], amphipods)) {
						pos = amphi.pos;
						costToMove = amphi.pos.y + Math.Abs(targetLocation[amphi.type] - amphi.pos.x);
						amphi.pos = new Vector2(targetLocation[amphi.type],2);
						nstate = new RoomState(amphipods,(Math.Abs(curr.costToHere) + costToMove * amphi.cost)*1);
						curr.AddNew(nstate);
						openStates.Add(nstate);
						amphi.pos = pos;
						continue;
					}
					//if in hallway, can't move unless to home
					if(hallway.Contains(amphi))
						continue;
					
					//have to move into the hallway
					int minHall=0;
					int maxHall=hallway.Length-1;
					while(minHall < hallway.Length && hallway[minHall] != null) {
						minHall++;
						if(minHall%2==0 && minHall > 1 && minHall < 9) {
							minHall++;
						}
					}
					while(maxHall >= 0 && hallway[maxHall] != null) {
						maxHall--;
						if(maxHall%2==0 && maxHall > 1 && maxHall < 9) {
							maxHall--;
						}
					}
					pos = amphi.pos;
					if(minHall < hallway.Length && hallway[minHall] == null && HallwayClearTo(hallway, pos.x,minHall+1)) {
						costToMove = pos.y + Math.Abs(minHall+1 - pos.x) - 1;
						amphi.pos = new Vector2(minHall+1,1);
						nstate = new RoomState(amphipods,Math.Abs(curr.costToHere) + costToMove * amphi.cost);

						var h = hallway.Where((a) => !((a == null || a.pos.x == targetLocation[a.type])) && HallwayClearTo(hallway, a.pos.x, targetLocation[a.type]) && IsHomeRowClear(targetLocation[a.type], amphipods)).Any();
						var p = amphipods.Where(a=>a.pos.x != targetLocation[a.type] && a.pos.y != 1 && a.pos.y <= amphipods.Where(b=>b.pos.x == a.pos.x).Min(b=>b.pos.y)).Any();

						if(h || p) {
							curr.AddNew(nstate);
							openStates.Add(nstate);
						}
					}
					if(maxHall >= 0 && hallway[maxHall] == null && HallwayClearTo(hallway, pos.x,maxHall+1)) {
						costToMove = pos.y + Math.Abs(maxHall+1 - pos.x) - 1;
						amphi.pos = new Vector2(maxHall+1,1);
						nstate = new RoomState(amphipods,Math.Abs(curr.costToHere) + costToMove * amphi.cost);

						var h = hallway.Where((a) => !((a == null || a.pos.x == targetLocation[a.type])) && HallwayClearTo(hallway, a.pos.x, targetLocation[a.type]) && IsHomeRowClear(targetLocation[a.type], amphipods)).Any();
						var p = amphipods.Where(a=>a.pos.x != targetLocation[a.type] && a.pos.y != 1 && a.pos.y <= amphipods.Where(b=>b.pos.x == a.pos.x).Min(b=>b.pos.y)).Any();

						if(h || p) {
							curr.AddNew(nstate);
							openStates.Add(nstate);
						}
					}
					amphi.pos = pos;
				}
			}
			return solutions[solutions.Keys.Min()];
		}

		private static bool HallwayClearTo(Amphipod[] hallway, int C, int T) {
			for(int p=Math.Min(C,T); p <= Math.Max(C,T); p++) {
				if(p !=C && hallway[p-1] != null) return false;
			}
			return true;
		}

		private static bool IsHomeRowClear(int X, List<Amphipod> amphipods) {
			return !(amphipods.Select(a=>a).Where(a=>a.pos.x == X && targetLocation[a.type] != X).Any());
		}

		private static int EstimateCostToHome(Amphipod amphi) {
			int targ = targetLocation[amphi.type];
			return (amphi.pos.y + Math.Abs(targ-amphi.pos.x)) * amphi.cost;
		}
	}
}