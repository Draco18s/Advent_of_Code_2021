using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Draco18s.AoCLib;
using System.IO;

namespace Advent_of_Code_2021 {
	internal static class DayTwentyone {
		static int p1pos;
		static int p2pos;
		static int p1score;
		static int p2score;
		static int hundredDie=0;
		static bool p1turn = true;
		static int numRolls=0;
		
		internal static long Part1(string input) {
			var matches = Regex.Matches(input, "(\\d)");
			p1pos = int.Parse(matches[1].Value);
			p2pos = int.Parse(matches[3].Value);


			do {
				int r1 = GetDieResultD100();
				int r2 = GetDieResultD100();
				int r3 = GetDieResultD100();
				if(p1turn) {
					p1pos += r1+r2+r3;
					p1pos = (p1pos-1)%10+1;
					p1score += p1pos;
				}
				else {
					p2pos += r1+r2+r3;
					p2pos = (p2pos-1)%10+1;
					p2score += p2pos;
				}
				if(p1score >= 1000) {
					return p2score * numRolls;
				}
				if(p2score >= 1000) {
					return p1score * numRolls;
				}
				p1turn = !p1turn;
			}while(true);
			return 0;
		}

		public static int GetDieResultD100() {
			numRolls++;
			return (hundredDie++%100)+1;
		}
		
		internal static long Part2(string input) {
			var matches = Regex.Matches(input, "(\\d)");
			p1pos = int.Parse(matches[1].Value);
			p2pos = int.Parse(matches[3].Value);

			long p1wins = 0;
			long p2wins = 0;

			//         pos                 score     count
			Dictionary<Vector2,Dictionary<Vector2,(long,long)>> alluniverses = new Dictionary<Vector2,Dictionary<Vector2,(long,long)>>();
			Dictionary<Vector2,Dictionary<Vector2,(long,long)>> alluniverses2 = new Dictionary<Vector2,Dictionary<Vector2,(long,long)>>();


			for(int i=1; i<=10;i++) {
				for(int j=1; j<=10;j++) {
					alluniverses.Add(new Vector2(i,j), new Dictionary<Vector2,(long,long)>());
					alluniverses2.Add(new Vector2(i,j), new Dictionary<Vector2,(long,long)>());
				}
			}
			
			foreach(Vector2 pospair in alluniverses.Keys) {
				for(int i=0; i<=30;i++) {
					for(int j=0; j<=30;j++) {
						alluniverses[pospair].Add(new Vector2(i,j), (0,0));
						alluniverses2[pospair].Add(new Vector2(i,j), (0,0));
					}
				}
			}
			long playersNow = 0;
			alluniverses[new Vector2(p1pos, p2pos)][new Vector2(0,0)] = (1,1);
			bool foundAvailablePlayers = true;

			while(foundAvailablePlayers) {
				foundAvailablePlayers = false;
				for(int roll = 0; roll < 3; roll++) {
					playersNow = 0;
					foreach(Vector2 pospair in alluniverses.Keys) {
						Dictionary<Vector2,(long,long)> scores = alluniverses[pospair];
						int[] p1poses = new int[] { (pospair.x-1+1)%10+1, (pospair.x-1+2)%10+1, (pospair.x-1+3)%10+1 };
						int[] p2poses = new int[] { (pospair.y-1+1)%10+1, (pospair.y-1+2)%10+1, (pospair.y-1+3)%10+1 };
						
						for(int a = 0; a < 3; a++) {
							for(int b = 0; b < 3; b++) {
								foreach(Vector2 scorepair in scores.Keys) {
									(long,long) playercounts = scores[scorepair];
									if(playercounts.Item1 == 0 && playercounts.Item2 == 0) continue;
									foundAvailablePlayers = true;

									if(roll < 2) {
										(long,long) lla = (playercounts.Item1,playercounts.Item2);
										(long,long) llb = alluniverses2[new Vector2(p1poses[a],p2poses[b])][scorepair];
										(long,long) llc = (lla.Item1+llb.Item1,lla.Item2+llb.Item2);
										Vector2 moveTo = new Vector2(p1poses[a],p2poses[b]);
										alluniverses2[moveTo][scorepair] = llc;
										playersNow++;
									}
									else {
										Dictionary<Vector2,(long,long)> newscores = alluniverses2[new Vector2(p1poses[a],p2poses[b])];
										
										if(scorepair.x+p1poses[a] >= 21) {
											p1wins+=playercounts.Item1;
											playercounts = (0,0);
											continue;
										}
										else if(scorepair.y+p2poses[b] >= 21) {
											p2wins+=playercounts.Item2;
											playercounts = (0,0);
											continue;
										}
										(long,long) vat = newscores[new Vector2(scorepair.x+p1poses[a],scorepair.y+p2poses[b])];
										(long,long) nat = (vat.Item1 + playercounts.Item1, vat.Item2 + playercounts.Item2);
										newscores[new Vector2(scorepair.x+p1poses[a],scorepair.y+p2poses[b])] = nat;
										playersNow++;
									}
								}
							}
						}
					}
					alluniverses = alluniverses2;

					alluniverses2 = new Dictionary<Vector2,Dictionary<Vector2,(long,long)>>();
					for(int i=1; i<=10;i++) {
						for(int j=1; j<=10;j++) {
							alluniverses2.Add(new Vector2(i,j), new Dictionary<Vector2,(long,long)>());
						}
					}
					
					foreach(Vector2 pospair in alluniverses2.Keys) {
						for(int i=0; i<=30;i++) {
							for(int j=0; j<=30;j++) {
								alluniverses2[pospair].Add(new Vector2(i,j), (0,0));
							}
						}
					}
				}
			}
			Console.WriteLine(Math.Min(p1wins, p2wins));
			return Math.Max(p1wins, p2wins)/27;
		}
	}
}