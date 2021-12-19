using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Draco18s.AoCLib;
using System.IO;

namespace Advent_of_Code_2021 {
	internal static class DayEightteen {
		public class SnailNum {
			public SnailNum parent;

			public SnailNum left;
			public SnailNum right;
			public int value = int.MinValue;

			public SnailNum(string s) {
				if(s[0] == '[') {
					int i;
					int opens = 0;
					for(i=0; i<s.Length;i++) {
						if(s[i] == '[')
							opens++;
						if(s[i] == ']')
							opens--;
						if(s[i] == ',' && opens==1) {
							break;
						}
					}

					left = new SnailNum(s.Substring(1,i-1));
					left.parent = this;
					right = new SnailNum(s.Substring(i+1, s.Length-(i+1)));
					right.parent = this;
				}
				else {
					value = int.Parse(s[0].ToString());
				}
			}

			public SnailNum(int v) {
				value = v;
			}

			public SnailNum(SnailNum a, SnailNum b) {
				left = a;
				right = b;
				left.parent = this;
				right.parent = this;
			}

			public SnailNum Add(SnailNum b) {
				SnailNum n = new SnailNum(this, b);
				this.parent = n;
				b.parent = n;
				return n.Reduce();
			}

			public SnailNum Reduce() {
				while(true) {
					if(FindDeep(0)) continue;
					if(CheckForSplit()) continue;
					break;
				}
				return this;
			}

			public bool CheckForSplit() {
				if(value >= 10) {
					left = new SnailNum((int)Math.Floor(value/2f));
					right = new SnailNum((int)Math.Ceiling(value/2f));
					value = int.MinValue;
					return true;
				}
				else if(value == int.MinValue) {
					return left.CheckForSplit() || right.CheckForSplit();
				}
				return false;
			}

			public bool FindDeep(int depth) {
				if(left != null && left.parent != this) left.parent = this;
				if(right != null  && right.parent != this) right.parent = this;
				bool exp = false;
				exp = left == null ? false : left.FindDeep(depth+1);
				if(exp) return true;
				exp = right == null ? false : right.FindDeep(depth+1);
				if(exp) return true;
				if(depth >= 4 && value == int.MinValue) {
					parent.Explode(this);
					return true;
				}
				return false;
			}

			public void Explode(SnailNum v) {
				if(left == v) {
					right.AddLeft(v.right.value);
					
					SnailNum t = this;
					SnailNum p = parent;
					while(p != null && p.left == t) {
						t = p;
						p = p.parent;
					}
					p?.left.AddRight(v.left.value);
					left = new SnailNum(0);
				}
				if(right == v) {
					left.AddRight(v.left.value);
					SnailNum t = this;
					SnailNum p = parent;
					while(p != null && p.right == t) {
						t = p;
						p = p.parent;
					}
					p?.right.AddLeft(v.left.value);
					right = new SnailNum(0);
				}
			}

			public void AddRight(int a) {
				if(value != int.MinValue)
					value += a;
				else
					right.AddRight(a);
			}

			public void AddLeft(int a) {
				if(value != int.MinValue)
					value += a;
				else
					left.AddLeft(a);
			}

			public override string ToString() {
				if(value != int.MinValue) return value.ToString();
				return "[" + left?.ToString() + "," + right?.ToString() + "]";
			}

			public long Magnitude() {
				if(value != int.MinValue) {
					return value;
				}
				long l = left.Magnitude();
				long r = right.Magnitude();
				return 3*l + 2*r;
			}
		}
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			SnailNum first = null;
			foreach(string lin in lines) {
				SnailNum a = new SnailNum(lin);
				SnailNum b = a.Reduce();
				if(first == null) {
					first = a;
					continue;
				}
				first = first.Add(a);
			}
			
			return first.Magnitude();
		}
		
		internal static long Part2(string input) {
			string[] lines = input.Split('\n');

			long biggest = 0;
			List<SnailNum> allNums = new List<SnailNum>();
			foreach(string lin in lines) {
				allNums.Add(new SnailNum(lin));
			}
			
			foreach(SnailNum A in allNums) {
				foreach(SnailNum B in allNums) {
					if(A == B) continue;
					SnailNum a1 = new SnailNum(A.ToString());
					SnailNum b1 = new SnailNum(B.ToString());
					long mag = (a1.Add(b1)).Magnitude();
					if(mag > biggest) {
						biggest = mag;
					}
				}
			}

			return biggest;
		}
	}
}