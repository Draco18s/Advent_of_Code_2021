using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code_2021 {
	internal static class DayEight {
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			long restult = 0;
			foreach(string lin in lines) {
				if(lin == string.Empty) continue;
				string[] parts = lin.Split('|');
				string output = parts[1];
				string[] digits = output.Split(' ');
				foreach(string d in digits) {
					if(d == string.Empty) continue;
					if(d.Length == 2 || d.Length == 4 || d.Length == 3 || d.Length == 7) {
						restult++;
					}
				}
			}
			return restult;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			long result = 0;
			foreach(string lin in lines) {
				Dictionary<string,int> collated = new Dictionary<string,int>();
				if(lin == string.Empty) continue;
				string[] parts = lin.Split('|');
				string intput = parts[0];
				string output = parts[1];
				string[] digits = intput.Split(' ');
				List<string> others = new List<string>();
				foreach(string d in digits) {
					if(d == string.Empty) continue;
					string d2 = MakeSort(d);
					if(collated.ContainsKey(d2)) continue;

					int l = d.Length;
					switch(l) {
						case 2:
							collated.Add(d2,1);
							break;
						case 3:
							collated.Add(d2,7);
							break;
						case 4:
							collated.Add(d2,4);
							break;
						case 7:
							collated.Add(d2,8);
							break;
						default:
							if(others.Contains(d2)) break;
							others.Add(d2);
							break;
					}
				}
				List<string> fivetwo = new List<string>();
				List<string> sixninezero = new List<string>();
				char[] onesegs = collated.Keys.First(x => collated[x] == 1).ToCharArray();
				foreach(string unknow in others) {
					if(unknow.Length == 5) {
						char[] segs = unknow.ToCharArray();
						//char[] onesegs = collated.Keys.First(x => collated[x] == 1).ToCharArray();
						if(segs.Contains(onesegs[0]) && segs.Contains(onesegs[1])) {
							collated.Add(MakeSort(unknow), 3);
						}
						else {
							fivetwo.Add(unknow);
						}
					}
					else {
						sixninezero.Add(unknow);
					}
				}
				char[] threesegs = collated.Keys.First(x => collated[x] == 3).ToCharArray();
				char[] foursegs = collated.Keys.First(x => collated[x] == 4).ToCharArray();

				List<char> fourTemp = new List<char>(foursegs);
				foreach(char c in threesegs) {
					fourTemp.Remove(c);
				}
				char upperLeft = fourTemp[0];

				foreach(string unknow in fivetwo) {
					if(unknow.ToCharArray().Contains(upperLeft)) {
						collated.Add(MakeSort(unknow), 5);
					}
					else {
						collated.Add(MakeSort(unknow), 2);
					}
				}
				char[] fivesegs = collated.Keys.First(x => collated[x] == 5).ToCharArray();
				fourTemp = new List<char>(foursegs);
				fourTemp.Remove(upperLeft);
				foreach(char c in onesegs) {
					fourTemp.Remove(c);
				}
				char centerBar = fourTemp[0];
				foreach(string unknow in sixninezero) {
					char[] unknowSegs = unknow.ToCharArray();
					foreach(char c in unknowSegs) {
						if(c == centerBar) {
							goto bad1;
						}
					}
					collated.Add(MakeSort(unknow), 0);
					break;
					bad1:
					;
				}
				sixninezero.Remove(collated.Keys.First(x => collated[x] == 0));
				foreach(string unknow in sixninezero) {
					char[] unknowSegs = unknow.ToCharArray();
					foreach(char c in threesegs) {
						if(!unknowSegs.Contains(c)) {
							goto bad2;
						}
					}
					collated.Add(MakeSort(unknow), 9);
					break;
					bad2:
					;
				}
				sixninezero.Remove(collated.Keys.First(x => collated[x] == 9));
				collated.Add(sixninezero[0], 6);
				result += Parse(output.Split(' '), collated);
			}
			return result;
		}

		public static string MakeSort(string input) {
			List<char> l = new List<char>(input.ToCharArray());
			l.Sort();
			return string.Join("", l.ToArray());
		}

		public static long Parse(string[] values, Dictionary<string,int> keys) {
			List<long> ll = new List<long>();
			string outp = "";
			foreach(string d in values) {
				if(d == string.Empty) continue;
				outp += keys[MakeSort(d)].ToString();
			}
			return long.Parse(outp);
		}
	}
}