using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent_of_Code_2021 {
	internal static class DayTwentyfour {
		private static long w;
		private static long x;
		private static long y;
		private static long z;
		
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			lines = lines.Select(l=>l).Where(l=>!Regex.IsMatch(l,"div . 1")).ToArray();
			long result = 0;
			long modelNum = long.Parse("9999999");
			do {
				result = FindModelNum(modelNum.ToString().PadLeft(7,'0'));
				modelNum--;
			}while(result != 0 && modelNum >= 0);
			modelNum++;
			return modelNum; //WARNING returns only the significant digits.
		}

		public static long FindModelNum(string number) {
			if(number.Contains('0')) return -1;

			//All of these magic numbers were acquired by hand-reading the puzzle input:

			/*
			inp w // read; int[] newNum
			mul x 0
			add x z //push z to x
			mod x 26 //mod 26
			div z 26 //div by 1 or 26; int[] o
			add x -12 //add modifier; int n[]
			eql x w
			eql x 0 //check X != W -- we want this to be false in all positions we can control so x is 0
			mul y 0
			add y 25
			mul y x
			add y 1
			mul z y //multiply z by 1 or 26;
			mul y 0
			add y w //push digit to y
			add y 5 //add modifier; int[] m
			mul y x //multiply by 1 or 0
			add z y //add to z; ZYm
			*/

			// significant vs. insignificant digits are based on the modifier n applied to z%26. If the modifier is positive, it is
			// "significant" and is part of the solution value. If the modifier is negative, it is "insignificant" as the value can
			// be directly manipulated to be exactly the value we want it to be in order to produce x == w, resulting in z not growing.
			// As there are as many forced multiplications by 26 due to the significant digits as there are divisions by 26, we need all
			// of them to align to produce a 0 from the last division.

			// -48 to convert from char-'0' to int-0
			int[] newNum = new int[] { 	number[0]-48,number[1]-48,number[2]-48,number[3]-48,number[4]-48,        0-48,        0-48,
										number[5]-48,        0-48,number[6]-48,        0-48,        0-48,        0-48,        0-48 };
			//							\----------/                                                     \-----------/
			//                          significant                                                      insignificant
			
			int[]o = new int[] { 0,1,1,1,26,26,1,26,1,26,26,26,26,1 };
			int[]m = new int[] { 14,2,1,13,5,5,5,9,3,13,2,1,11,8 };
			int[]n = new int[] { 14,14,14,12,15,-12,-12,12,-7,13,-8,-5,-10,-7 };

			long row4 = m[0];
			long row5 = 1;
			long row6 = 0;
			long row8 = ComputeZYm(new long[] { newNum[0], row5, row6, m[0] } );
			long row10 = row8;
			for(int i=1; i < 14; i++) {
				row4 = row8 % 26 + n[i];
				if(newNum[i] <= 0) {		 // this valie is insignificant
					if(row4 < 1 || row4 > 9) // single digit values cannot exceed the range 1-9
						return -1;
					newNum[i] = (int)row4;	 // just set it equal to the check value
				}
				row5 = row4==newNum[i] ? 0:1;
				row6 = row10*(25*row5+1);
				row8 = ComputeZYm(new long[] { newNum[i], row5, row6, m[i] } );
				row10 = (long)Math.Floor((double)row8 / o[i]);
			}
			// actual answer in newNum now
			return 0;
		}

		private static long ComputeZYm(long[] invals) {
			return invals[2]+(invals[0]+invals[3])*invals[1];
		}

		public static long BruteForce(string[] lines, string number) {
			w = x = y = z = 0;
			if(number.Contains("0")) return -1;
			long val1=0;
			long val2=0;
			int index=0;
			foreach(string lin in lines) {
				if(lin == string.Empty) continue;
				string[] parts = lin.Split(' ');
				switch(parts[0]) {
					case "inp":
						int v = int.Parse(number[index].ToString());
						Store(parts[1],v);
						index++;
						break;
					case "add":
						val1 = Fetch(parts[1]);
						if(!long.TryParse(parts[2], out val2)) {
							val2 = Fetch(parts[2]);
						}
						Store(parts[1],val1+val2);
						break;
					case "mul":
						val1 = Fetch(parts[1]);
						if(!long.TryParse(parts[2], out val2)) {
							val2 = Fetch(parts[2]);
						}
						Store(parts[1],val1*val2);
						break;
					case "div":
						val1 = Fetch(parts[1]);
						if(!long.TryParse(parts[2], out val2)) {
							val2 = Fetch(parts[2]);
						}
						Store(parts[1],val1/val2);
						break;
					case "mod":
						val1 = Fetch(parts[1]);
						if(!long.TryParse(parts[2], out val2)) {
							val2 = Fetch(parts[2]);
						}
						Store(parts[1],val1%val2);
						break;
					case "eql":
						val1 = Fetch(parts[1]);
						if(!long.TryParse(parts[2], out val2)) {
							val2 = Fetch(parts[2]);
						}
						Store(parts[1],val1==val2?1:0);
						break;
				}
			}
			return z;
		}

		public static long Fetch(string reg) {
			switch(reg) {
				case "w":
					return w;
				case "x":
					return x;
				case "y":
					return y;
				case "z":
					return z;
			}
			throw new Exception("Fetch");
		}

		public static void Store(string reg, long val) {
			switch(reg) {
				case "w":
					w = val;
					return;
				case "x":
					x = val;
					return;
				case "y":
					y = val;
					return;
				case "z":
					z = val;
					return;
			}
			throw new Exception("Store");
		}

		internal static long Part2(string input) {
			long modelNum = long.Parse("1111111");
			long result = 0;
			do {
				result = FindModelNum(modelNum.ToString().PadLeft(7,'0'));
				modelNum++;
			}while(result != 0 && modelNum >= 0);
			modelNum--;
			return modelNum; //WARNING returns the significant digits.
		}
	}
}