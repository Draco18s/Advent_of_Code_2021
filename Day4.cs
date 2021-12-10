using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code_2021 {
	internal static class DayFour {
		private class Board {
			public int[,] numbers;
			public List<int> called;

			public bool hasWon() {
				for(int x = 0; x < 5; x++) {
					bool win1 = true;
					bool win2 = true;
					for(int y = 0; y < 5; y++) {
						win1 &= called.Contains(numbers[x,y]);
						win2 &= called.Contains(numbers[y,x]);
					}
					if(win1 || win2) 
						return true;
				}

				return false;
			}

			public long GetScore(int l) {
				long r = 0;
				for(int x = 0; x < 5; x++) {
					for(int y = 0; y < 5; y++) {
						r+=called.Contains(numbers[x,y])?0:numbers[x,y];
					}
				}
				return r * l;
			}
		}
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			string calledNums = lines[0];
			List<Board> allBoards = new List<Board>();
			for(int i = 2; i < lines.Length; i+=6) {
				string[] b = new string[] {
					lines[i+0],
					lines[i+1],
					lines[i+2],
					lines[i+3],
					lines[i+4]
				};
				int[,] n = new int[5,5];
				for(int y = 0; y < 5; y++) {
					string[] bb = b[y].Split(' ');
					bb = bb.Where(l => l != string.Empty).ToArray();
					for(int x = 0; x < 5; x++) {
						
						int.TryParse(bb[x], out int v);
						n[x,y]  = v;
					}
				}
				allBoards.Add(new Board() {
					numbers = n,
					called = new List<int>()
				});
			}
			string[] numStr = lines[0].Split(',');
			foreach(string num in numStr) {
				int.TryParse(num, out int v);
				foreach(Board b in allBoards) {
					b.called.Add(v);
					if(b.hasWon()) {
						return b.GetScore(v);
					}
				}
			}
			return 0;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			string calledNums = lines[0];
			List<Board> allBoards = new List<Board>();
			for(int i = 2; i < lines.Length; i+=6) {
				string[] b = new string[] {
					lines[i+0],
					lines[i+1],
					lines[i+2],
					lines[i+3],
					lines[i+4]
				};
				int[,] n = new int[5,5];
				for(int y = 0; y < 5; y++) {
					string[] bb = b[y].Split(' ');
					bb = bb.Where(l => l != string.Empty).ToArray();
					for(int x = 0; x < 5; x++) {
						
						int.TryParse(bb[x], out int v);
						n[x,y]  = v;
					}
				}
				allBoards.Add(new Board() {
					numbers = n,
					called = new List<int>()
				});
			}
			string[] numStr = lines[0].Split(',');
			for(int s = 0; s < numStr.Length; s++) {
				string num = numStr[s];
				int.TryParse(num, out int v);
				foreach(Board b in allBoards) {
					b.called.Add(v);
				}
				int numNon = allBoards.Where(br => !br.hasWon()).Count();
				if(numNon == 1) {
					Board loser = allBoards.Where(br => !br.hasWon()).First();
					for(int s2 = s+1; s2 < numStr.Length; s2++) {
						string num2 = numStr[s2];
						int.TryParse(num2, out int v2);
						loser.called.Add(v2);
						if(loser.hasWon())
							return loser.GetScore(v2);
					}
				}
			}
			return 0;
		}
	}
}