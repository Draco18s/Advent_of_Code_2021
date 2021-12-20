using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Draco18s.AoCLib;
using System.IO;

namespace Advent_of_Code_2021 {
	internal static class DayNineteen {

		public class Sensor {
			public string name;
			public List<Vector3> pings = new List<Vector3>();
			public List<int> originalIds = new List<int>();

			public Sensor(string n) {
				name = n;
			}

			public override string ToString() {
				return name;
			}

			public void InsertPingAt(int x, int y, int z) {
				pings.Add(new Vector3(x, y, z));
			}

			public void InsertPingAt(Vector3 p) {
				if(pings.Contains(p)) return;
				pings.Add(p);
				if(pings.Count > 1100) {
					Console.WriteLine("Something has gone terribly terribly wrong");
				}
			}

			public void Rotate(UpwiseOrientation upBecomes) {
				List<Vector3> npings = new List<Vector3>();
				switch(upBecomes) {
					case UpwiseOrientation.UP:
						foreach(Vector3 v in pings) {
							npings.Add(v);
						}
						break;
					case UpwiseOrientation.RIGHT:
						foreach(Vector3 v in pings) {
							npings.Add(new Vector3(-v.x,-v.y,v.z));
						}
						break;
					case UpwiseOrientation.DOWN:
						foreach(Vector3 v in pings) {
							npings.Add(new Vector3(v.y,-v.x,v.z));
						}
						break;
					case UpwiseOrientation.LEFT:
						foreach(Vector3 v in pings) {
							npings.Add(new Vector3(-v.y,v.x,v.z));
						}
						break;
				}
				pings = npings;
			}

			public void Rotate(Orientation northBecomes) {
				List<Vector3> npings = new List<Vector3>();
				switch(northBecomes) {
					case Orientation.NORTH:
						foreach(Vector3 v in pings) {
							npings.Add(v);
						}
						break;
					case Orientation.SOUTH:
						foreach(Vector3 v in pings) {
							npings.Add(new Vector3(-v.x,v.y,-v.z));
						}
						break;
					case Orientation.EAST:
						foreach(Vector3 v in pings) {
							npings.Add(new Vector3(v.z,v.y,-v.x));
						}
						break;
					case Orientation.WEST:
						foreach(Vector3 v in pings) {
							npings.Add(new Vector3(-v.z,v.y,v.x));
						}
						break;
				}
				pings = npings;
			}
		}

		private static List<(UpwiseOrientation,Orientation,UpwiseOrientation)> possibleRots = new List<(UpwiseOrientation,Orientation,UpwiseOrientation)>();
		
		internal static long Part1(string input) {
			return 0; //handled in part 2
		}

		public static int CountMatches(List<Vector3> beacons, List<Vector3> curBeacons, int earlyStop, out Vector3 bestOffset) {
			bestOffset = new Vector3(int.MinValue, int.MinValue, int.MinValue);
			int bestMatch = 0;
			for(int sa = beacons.Count()-1; sa >= earlyStop; sa--) {
				for(int sb = 0; sb < curBeacons.Count(); sb++) {
					Vector3 offset = beacons[sa] - curBeacons[sb];
					int matches = 0;
					foreach(Vector3 A in curBeacons) {
						foreach(Vector3 B in beacons) {
							if(A+offset==B) {
								matches++;
								if(matches >= 12) {
									bestMatch = matches;
									bestOffset = offset;
									return matches;
								}
							}
						}
					}
					if(matches > bestMatch) {
						bestMatch = matches;
						bestOffset = offset;
					}
				}
			}
			return bestMatch;
		}

		private static List<Vector3> GetBeacons(Sensor sensor) {
			List<Vector3> beacons = new List<Vector3>();
			foreach(Vector3 v in sensor.pings) {
				beacons.Add(new Vector3(v.x, v.y, v.z));
			}
			return beacons;
		}

		private static Sensor ResetRotation(List<Vector3> originalBeacons, string n) {
			Sensor curSensor=new Sensor(n);
			foreach(Vector3 v in originalBeacons) {
				curSensor.InsertPingAt(v.x, v.y, v.z);
			}
			return curSensor;
		}
		
		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			List<Sensor> sensors = new List<Sensor>();
			Sensor curSensor = null;

			possibleRots.Add((UpwiseOrientation.UP, Orientation.NORTH, UpwiseOrientation.UP));
			possibleRots.Add((UpwiseOrientation.UP, Orientation.EAST, UpwiseOrientation.UP));
			possibleRots.Add((UpwiseOrientation.UP, Orientation.SOUTH, UpwiseOrientation.UP));
			possibleRots.Add((UpwiseOrientation.UP, Orientation.WEST, UpwiseOrientation.UP));

			possibleRots.Add((UpwiseOrientation.RIGHT, Orientation.NORTH, UpwiseOrientation.UP));
			possibleRots.Add((UpwiseOrientation.RIGHT, Orientation.EAST, UpwiseOrientation.UP));
			possibleRots.Add((UpwiseOrientation.RIGHT, Orientation.SOUTH, UpwiseOrientation.UP));
			possibleRots.Add((UpwiseOrientation.RIGHT, Orientation.WEST, UpwiseOrientation.UP));

			possibleRots.Add((UpwiseOrientation.LEFT, Orientation.NORTH, UpwiseOrientation.UP));
			possibleRots.Add((UpwiseOrientation.LEFT, Orientation.EAST, UpwiseOrientation.UP));
			possibleRots.Add((UpwiseOrientation.LEFT, Orientation.SOUTH, UpwiseOrientation.UP));
			possibleRots.Add((UpwiseOrientation.LEFT, Orientation.WEST, UpwiseOrientation.UP));

			possibleRots.Add((UpwiseOrientation.UP, Orientation.NORTH, UpwiseOrientation.DOWN));
			possibleRots.Add((UpwiseOrientation.UP, Orientation.EAST, UpwiseOrientation.DOWN));
			possibleRots.Add((UpwiseOrientation.UP, Orientation.WEST, UpwiseOrientation.DOWN));

			possibleRots.Add((UpwiseOrientation.RIGHT, Orientation.EAST, UpwiseOrientation.DOWN));
			possibleRots.Add((UpwiseOrientation.RIGHT, Orientation.SOUTH, UpwiseOrientation.DOWN));
			possibleRots.Add((UpwiseOrientation.RIGHT, Orientation.WEST, UpwiseOrientation.DOWN));

			possibleRots.Add((UpwiseOrientation.LEFT, Orientation.EAST, UpwiseOrientation.DOWN));
			possibleRots.Add((UpwiseOrientation.LEFT, Orientation.WEST, UpwiseOrientation.DOWN));

			possibleRots.Add((UpwiseOrientation.DOWN, Orientation.EAST, UpwiseOrientation.UP));
			possibleRots.Add((UpwiseOrientation.DOWN, Orientation.WEST, UpwiseOrientation.UP));

			possibleRots.Add((UpwiseOrientation.DOWN, Orientation.EAST, UpwiseOrientation.DOWN));
			possibleRots.Add((UpwiseOrientation.DOWN, Orientation.WEST, UpwiseOrientation.DOWN));
			int o = -1;
			foreach(string lin in lines) {
				if(lin == string.Empty)continue;
				if(lin[1] == '-') {
					curSensor=new Sensor(lin);
					curSensor.originalIds.Add(o);
					o++;
					sensors.Add(curSensor);
					continue;
				}
				var matches = Regex.Matches(lin, "(-?\\d+)");
				List<int> list = new List<int>();
				foreach(Match m in matches) {
					list.Add(int.Parse(m.Value));
				}
				curSensor.InsertPingAt(list[0],list[1],list[2]);
			}

			Sensor start = sensors[0];
			
			sensors.Remove(start);
			int earlyStop = 0;
			int seeThisIDAgain = -1;
			int pings = start.pings.Count;
			int listSize = -1;

			List<Vector3> beaconPositions = new List<Vector3>();
			beaconPositions.Add(new Vector3(0,0,0));

			while(sensors.Count > 0) {
				List<Vector3> startBeacons = GetBeacons(start);
				Sensor nextSensor = sensors[0];
				sensors.Remove(nextSensor);
				List<Vector3> originalBeacons = GetBeacons(nextSensor);
				if(nextSensor.originalIds.Contains(seeThisIDAgain)) {
					earlyStop+=(pings);
					seeThisIDAgain = -1;
					if(listSize == sensors.Count) {
						earlyStop = 0;
					}
					listSize = -1;
				}
				foreach((UpwiseOrientation,Orientation,UpwiseOrientation) rotation in possibleRots) {
					Sensor currentClone = ResetRotation(originalBeacons, nextSensor.name);
					currentClone.Rotate(rotation.Item1);
					currentClone.Rotate(rotation.Item2);
					currentClone.Rotate(rotation.Item3);
					List<Vector3> curBeacons = GetBeacons(currentClone);
					int m = CountMatches(startBeacons, curBeacons, earlyStop, out Vector3 bestOffset);
					if(m >= 12) {
						foreach(Vector3 v in curBeacons) {
							start.InsertPingAt(v+bestOffset);
						}
						start.originalIds.AddRange(nextSensor.originalIds);
						beaconPositions.Add(bestOffset);
						goto matchFound;
					}
				}
				if(seeThisIDAgain == -1) {
					seeThisIDAgain = nextSensor.originalIds[0];
					listSize = sensors.Count();
				}
				sensors.Add(nextSensor);
				matchFound:
				;
			}
			Console.WriteLine("Part 1: " + start.pings.Count);
			long bestDist = 0;
			for(int a = 0; a < beaconPositions.Count; a++) {
				for(int b = a+1; b < beaconPositions.Count; b++) {
					int dx = beaconPositions[a].x-beaconPositions[b].x;
					int dy = beaconPositions[a].y-beaconPositions[b].y;
					int dz = beaconPositions[a].z-beaconPositions[b].z;
					long dist = Math.Abs(dx) + Math.Abs(dy) + Math.Abs(dz);

					if(bestDist < dist) {
						bestDist = dist;
					}
				}
			}
			return bestDist;
		}
	}
}