using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Draco18s.AoCLib;
using System.IO;

namespace Advent_of_Code_2021 {
	internal static class DaySixteen {
		public class Packet {
			public int version;
			public int ID;
			public long literalValue=-1;

			public List<Packet> subPackets = new List<Packet>();

			public long GetTotalVersion() {
				long total = version;
				foreach(Packet p in subPackets) {
					total += p.GetTotalVersion();
				}
				return total;
			}

			public long Evaluate() {
				long total = 0;
				switch(ID) {
					case 4:
						return literalValue;
					case 0:
						foreach(Packet p in subPackets) {
							total += p.Evaluate();
						}
						return total;
					case 1:
						total = 1;
						foreach(Packet p in subPackets) {
							total *= p.Evaluate();
						}
						return total;
					case 2:
						total = subPackets[0].Evaluate();
						foreach(Packet p in subPackets) {
							total = Math.Min(total, p.Evaluate());
						}
						return total;
					case 3:
						total = subPackets[0].Evaluate();
						foreach(Packet p in subPackets) {
							total = Math.Max(total, p.Evaluate());
						}
						return total;
					case 5:
						return subPackets[0].Evaluate() > subPackets[1].Evaluate() ? 1 : 0;
					case 6:
						return subPackets[0].Evaluate() < subPackets[1].Evaluate() ? 1 : 0;
					case 7:
						return subPackets[0].Evaluate() == subPackets[1].Evaluate() ? 1 : 0;
				}
				return total;
			}
		}

		internal static long Part1(string input) {
			Packet p = ParseBITS(HexToBinary(input), 0).First();
			
			return p.GetTotalVersion();
		}

		private static IEnumerable<Packet> ParseBITS(string input, int index) {
			while(index < input.Length) {
				yield return ParseOnePacket(input, ref index);
			}
		}

		private static Packet ParseOnePacket(string input, ref int index) {
			Packet p = new Packet();
			Packet r = p;
			p.version = Convert.ToInt32(input.Substring(index, 3), 2);
			index+=3;
			string sss = input.Substring(index, 3);
			p.ID = Convert.ToInt32(sss, 2);
			index+=3;
			if(p.ID == 4) {
				string valueStr="";
				while(true) {
					string header = input.Substring(index, 1);
					index+=1;
					string segment = input.Substring(index, 4);
					index+=4;
					valueStr += segment;
					if(header == "0") {
						p.literalValue = Convert.ToInt64(valueStr, 2);
						break;
					}
				}
				Packet p2 = new Packet();
				p.subPackets.Add(p2);
				p = p2;
			}
			else {
				int lengthID = Convert.ToInt32(input.Substring(index, 1), 2);
				index+=1;
				if(lengthID == 0) {
					int bitsConsumed = Convert.ToInt32(input.Substring(index, 15), 2);
					index+=15;
					Packet[] arr = ParseBITS(input.Substring(index, bitsConsumed), 0).ToArray();
					index+=bitsConsumed;
					p.subPackets.AddRange(arr);
				}
				else {
					int numSubPackets = Convert.ToInt32(input.Substring(index, 11), 2);
					index+=11;
					for(int s=0; s < numSubPackets; s++) {
						Packet np = ParseOnePacket(input, ref index);
						p.subPackets.Add(np);
					}
				}
			}
			return r;
		}

		private static string HexToBinary(string input) {
			StringBuilder sb = new StringBuilder();
			foreach(char c in input) {
				sb.Append(Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4,'0'));
			}
			return sb.ToString();
		}

		internal static long Part2(string input) {
			Packet p = ParseBITS(HexToBinary(input), 0).First();
			
			return p.Evaluate();
		}
	}
}