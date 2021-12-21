using System;

namespace Draco18s.AoCLib {
	public struct Vector2 {
		public readonly int x;
		public readonly int y;
		public Vector2(int _x, int _y) {
			x = _x;
			y = _y;
		}

		public override string ToString() {
			return string.Format("({0},{1})", x, y);
		}
	}
}