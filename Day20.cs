using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Draco18s.AoCLib;
using System.IO;

namespace Advent_of_Code_2021 {
	internal static class DayTwenty {

		private static Grid.EdgeHandler dots = () => '.';
		private static Grid.EdgeHandler dashes = () => '#';
		
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			string pattern = lines[0];

			string inputImage = string.Join("\n", lines.Skip(2).ToList());

			Grid grid = new Grid(inputImage, true);

			grid.IncreaseGridBy(-1,-1, dots);
			grid.IncreaseGridBy(1,1, dots);

			//Console.WriteLine(RunEnhancement(grid, pattern).ToString("char+0"));

			string final = RunEnhancement(RunEnhancement(grid, pattern, dots),pattern, dashes).ToString("char+0");

			//Console.WriteLine(final);
			return final.Select(a=>a).Where(a=>a=='#').Count();
		}

		public static Grid RunEnhancement(Grid input, string pattern, Grid.EdgeHandler edges) {
			input.IncreaseGridBy(-2,-2, edges);
			input.IncreaseGridBy(2,2, edges);

			Grid output = new Grid(input.Width, input.Height, input.MinX, input.MinY);

			for(int y = input.MinY; y < input.MaxY; y++) {
				for(int x = input.MinY; x < input.MaxY; x++) {
					List<char> l = new List<char>();
					foreach(int a in input.GetNeighbors(x, y, false, true, edges)) {
						l.Add(((char)a == '.' ? '0' : '1'));
					}
					int v = Convert.ToInt32(string.Join("",l),2);
					output[x,y] = (int)pattern[v];
				}
			}

			return output;
		}
		
		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			string pattern = lines[0];

			string inputImage = string.Join("\n", lines.Skip(2).ToList());

			Grid grid = new Grid(inputImage, true);

			grid.IncreaseGridBy(-1,-1, dots);
			grid.IncreaseGridBy(1,1, dots);

			for(int i = 0; i < 50; i++) {
				grid = RunEnhancement(grid, pattern, i%2 == 0 ? dots : dashes);
			}

			//Console.WriteLine(RunEnhancement(grid, pattern).ToString("char+0"));

			string final = grid.ToString("char+0");//RunEnhancement(RunEnhancement(grid, pattern, dots),pattern, dashes).ToString("char+0");

			//Console.WriteLine(final);
			return final.Select(a=>a).Where(a=>a=='#').Count();
		}
	}
}