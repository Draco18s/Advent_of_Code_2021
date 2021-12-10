using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code_2021 {
	internal static class DaySix {
		public class Fish {
			int timer;

			public Fish(int n) {
				timer = n;
			}

			public Fish() {
				timer = 8;
			}

			public bool Tick() {
				if(timer == 0) {
					timer = 6;
					return true;
				}
				else {
					timer--;
					return false;
				}
			}

			public override string ToString() {
				return "Fish (" + timer + ")";
			}
		}
		internal static long Part1(string input) {
			string[] vals = input.Split(',');
			List<Fish> fishes = new List<Fish>();
			foreach(string s in vals) {
				if(int.TryParse(s, out int v)) {
					fishes.Add(new Fish(v));
				}
			}
			List<Fish> nfishes = new List<Fish>();
			for(int i = 0; i < 80; i++) {
				foreach(Fish f in fishes) {
					bool b = f.Tick();
					if(b) {
						nfishes.Add(new Fish());
					}
				}
				fishes.AddRange(nfishes);
				nfishes.Clear();
			}
			return fishes.Count;
		}

		internal static long Part2(string input) {
			string[] vals = input.Split(',');
			Dictionary<int,long> fishes = new Dictionary<int,long>();
			foreach(string s in vals) {
				if(int.TryParse(s, out int v)) {
					if(fishes.ContainsKey(v)) {
						fishes[v]++;
					}
					else {
						fishes.Add(v, 1);
					}
				}
			}
			for(int i = 0; i < 256; i++) {
				Dictionary<int,long> nfishes = new Dictionary<int,long>();
				foreach(int k in fishes.Keys) {
					long vv = fishes[k];
					if(k == 0) {
						if(nfishes.ContainsKey(6)) {
							nfishes[6] += vv;
						}
						else {
							nfishes.Add(6,vv);
						}
						nfishes.Add(8,vv);
					}
					else {
						if(nfishes.ContainsKey(k-1)) {
							nfishes[k-1] += vv;
						}
						else {
							nfishes.Add(k-1,vv);
						}
					}
				}
				fishes = nfishes;
			}
			long r = 0;
			foreach(int k in fishes.Keys) {
				r += fishes[k];
			}
			return r;
		}
	}
}