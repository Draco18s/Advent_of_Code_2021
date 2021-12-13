using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code_2021 {
	internal static class DayTen {
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			long result = 0;
			Dictionary<char,int> points = new Dictionary<char,int>();
			points.Add(')',3);
			points.Add(']',57);
			points.Add('}',1197);
			points.Add('>',25137);

			foreach(string lin in lines) {
				if(lin == string.Empty) continue;
				Queue<char> output = new Queue<char>();
				Stack<char> trainyard = new Stack<char>();
				char last = ' ';
				char c;
				foreach(char exp in lin) {
					if(exp == ' ') continue;
					switch(exp) {
						case '<':
						case '{':
						case '[':
						case '(':
							trainyard.Push(exp);
							break;
						case ']':
							c = trainyard.Pop();
							if(c == '[') break;
							last = exp;
							goto invalidFound;
						case '}':
							c = trainyard.Pop();
							if(c == '{') break;
							last = exp;
							goto invalidFound;
						case '>':
							c = trainyard.Pop();
							if(c == '<') break;
							last = exp;
							goto invalidFound;
						case ')':
							c = trainyard.Pop();
							if(c == '(') break;
							last = exp;
							goto invalidFound;
						default:
							break;
					}
				}
				continue;
				invalidFound:
				if(last != ' ') {
					result += points[last];
				}
			}
			return result;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			Dictionary<char,int> points = new Dictionary<char,int>();
			points.Add(')',1);
			points.Add(']',2);
			points.Add('}',3);
			points.Add('>',4);
			Dictionary<char,char> lookup = new Dictionary<char,char>();
			lookup.Add('(',')');
			lookup.Add('[',']');
			lookup.Add('{','}');
			lookup.Add('<','>');
			List<long> scores = new List<long>();
			foreach(string lin in lines) {
				if(lin == string.Empty) continue;
				Queue<char> output = new Queue<char>();
				Stack<char> trainyard = new Stack<char>();
				char last = ' ';
				char c;
				foreach(char exp in lin) {
					if(exp == ' ') continue;
					switch(exp) {
						case '<':
						case '{':
						case '[':
						case '(':
							trainyard.Push(exp);
							break;
						case ']':
							c = trainyard.Pop();
							if(c == '[') break;
							last = exp;
							goto invalidFound;
						case '}':
							c = trainyard.Pop();
							if(c == '{') break;
							last = exp;
							goto invalidFound;
						case '>':
							c = trainyard.Pop();
							if(c == '<') break;
							last = exp;
							goto invalidFound;
						case ')':
							c = trainyard.Pop();
							if(c == '(') break;
							last = exp;
							goto invalidFound;
						default:
							break;
					}
				}
				string append = "";
				foreach(char c2 in trainyard) {
					append += lookup[c2];
				}
				long score = 0;
				foreach(char c2 in append) {
					score *= 5;
					score += points[c2];
				}
				scores.Add(score);
				continue;
				invalidFound:
				continue;
			}
			scores.Sort();
			int ind = (scores.Count / 2);
			return scores[ind];
		}
	}
}