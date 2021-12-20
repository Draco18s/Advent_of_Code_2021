using System;

namespace Draco18s.AoCLib {
	public struct Vector3 {
		public readonly int x;
		public readonly int y;
		public readonly int z;
		public Vector3(int _x, int _y, int _z) {
			x = _x;
			y = _y;
			z = _z;
		}

		public static Vector3 operator -(Vector3 a, Vector3 b) {
			return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		public static Vector3 operator +(Vector3 a, Vector3 b) {
			return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		public static bool operator ==(Vector3 a, Vector3 b) {
			return a.Equals(b);
		}

		public static bool operator !=(Vector3 a, Vector3 b) {
			return !a.Equals(b);
		}

		public override bool Equals(object obj) {
			if(obj is Vector3 o) {
				return o.x == x && o.y == y && o.z == z;
			}
			return false;
		}

		public override string ToString() {
			return string.Format("({0},{1},{2})", x, y, z);
		}
	}
}