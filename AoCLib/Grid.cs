using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Draco18s.AoCLib {
	public class Grid {
		int[,] cells;
		private int width;
		private int height;
		private int offsetx;
		private int offsety;

		public delegate int EdgeHandler();
		public static EdgeHandler returnZero = () => 0;
		public static EdgeHandler returnInf = () => int.MaxValue;
		public static EdgeHandler returnNegInf = () => int.MinValue;

		public Grid(int w, int h, int offx=0, int offy=0) {
			width = w;
			height = h;
			cells = new int[w,h];
			offsetx = offx;
			offsety = offy;
		}

		public Grid(string input, bool asChar, char separator=(char)0, int offx=0, int offy=0) {
			string[] lines = input.Split('\n');
			width = lines[0].Length;
			height = lines.Length;
			offsetx = offx;
			offsety = offy;

			cells = new int[width,height];
			int y = 0;
			foreach(string lin in lines) {
				int x = 0;
				string[] ca = null;
				
				if(separator == (char)0)
					ca = Regex.Split(lin, string.Empty);
				else
					ca = lin.Split(separator);

				foreach(string c in ca) {
					if(c == string.Empty) continue;
					cells[x,y] = (!asChar) ? int.Parse(c) : (int)c[0];
					x++;
				}
				y++;
			}
		}

		public int Width => width;
		public int Height => height;
		public int MinX => offsetx;
		public int MinY => offsety;
		public int MaxX => width+offsetx;
		public int MaxY => height+offsety;

		public int this[int X, int Y, bool useOffset=true]
		{
			get => cells[X-(useOffset?offsetx:0),Y-(useOffset?offsety:0)];
			set => cells[X-(useOffset?offsetx:0),Y-(useOffset?offsety:0)] = value;
		}

		public int this[Vector2 p, bool useOffset=true]
		{
			get => this[p.x,p.y,useOffset];
			set => this[p.x,p.y,useOffset] = value;
		}

		public int GetLength(int i) {
			return cells.GetLength(i);
		}

		///<summary>
		/// Increases the grid in the positive direction if parameters are positive and in the negative
		/// direction if parameters are negative. The value accessed at this[0,0] remains the same.
		///</summary>
		public void IncreaseGridBy(int x, int y, EdgeHandler edgeHandler) {
			int nwidth = width + Math.Abs(x);
			int nheight = height + Math.Abs(y);
			int[,] ncells = new int[nwidth,nheight];

			for(int ny = 0; ny < nheight; ny++) {
				for(int nx = 0; nx < nwidth; nx++) {
					int ox = nx;
					int oy = ny;
					if(x < 0) ox += x;
					if(y < 0) oy += y;
					ncells[nx,ny] = ox >= 0 && ox < width && oy >= 0 && oy < height ? cells[ox,oy] : edgeHandler();
				}
			}
			width = nwidth;
			height = nheight;
			cells = ncells;
			if(x < 0) offsetx += x;
			if(y < 0) offsety += y;
		}
		public void IncreaseGridBy(Vector2 amt, EdgeHandler edgeHandler) {
			IncreaseGridBy(amt.x, amt.y, edgeHandler);
		}

		///<summary>
		/// Decreases the grid in the positive direction if parameters are positive and in the negative
		/// direction if parameters are negative. The value accessed at this[0,0] remains the same.
		///</summary>
		public void DecreaseGridBy(int x, int y) {
			int nwidth = width - Math.Abs(x);
			int nheight = height - Math.Abs(y);
			int[,] ncells = new int[nwidth,nheight];

			for(int ny = 0; ny < nheight; ny++) {
				for(int nx = 0; nx < nwidth; nx++) {
					int ox = nx;
					int oy = ny;
					if(x < 0) ox -= x;
					if(y < 0) oy -= y;
					ncells[nx,ny] = ox >= 0 && ox < width && oy >= 0 && oy < height ? cells[ox,oy] : 0;
				}
			}
			width = nwidth;
			height = nheight;
			cells = ncells;
			if(x < 0) offsetx -= x;
			if(y < 0) offsety -= y;
		}

		public void DecreaseGridBy(Vector2 amt) {
			DecreaseGridBy(amt.x, amt.y);
		}

		public void TrimGrid(EdgeHandler edgeHandler) {
			DecreaseGridBy(MinX - GetMinimumX(edgeHandler), MinY - GetMinimumY(edgeHandler));
			DecreaseGridBy(MaxX - GetMaximumX(edgeHandler)-1, MaxY - GetMaximumY(edgeHandler)-1);
		}

		public int GetMinimumX(EdgeHandler edgeHandler) {
			int edgeVal = edgeHandler();
			for(int x=MinX;x<MaxX;x++) {
				for(int y=MinY;y<MaxY;y++) {
					if(this[x,y] == edgeVal) continue;
					return x;
				}
			}
			return MinX;
		}

		public int GetMinimumY(EdgeHandler edgeHandler) {
			int edgeVal = edgeHandler();
			for(int y=MinY;y<MaxY;y++) {
				for(int x=MinX;x<MaxX;x++) {
					if(this[x,y] == edgeVal) continue;
					return y;
				}
			}
			return MinY;
		}

		public int GetMaximumX(EdgeHandler edgeHandler) {
			int edgeVal = edgeHandler();
			for(int x=MaxX-1;x>=MinX;x--) {
				for(int y=MinY;y<MaxY;y++) {
					if(this[x,y] == edgeVal) continue;
					return x;
				}
			}
			return MaxX;
		}

		public int GetMaximumY(EdgeHandler edgeHandler) {
			int edgeVal = edgeHandler();
			for(int y=MaxY-1;y>=MinY;y--) {
				for(int x=MinX;x<MaxX;x++) {
					if(this[x,y] == edgeVal) continue;
					return y;
				}
			}
			return MaxY;
		}

		public IEnumerable<int> GetNeighbors(int x, int y, bool orthoOnly, bool includeSelf) {
			return GetNeighbors(x, y, orthoOnly, includeSelf, returnZero);
		}

		public IEnumerable<int> GetNeighbors(int x, int y, bool orthoOnly, bool includeSelf, EdgeHandler edgeHandler) {
			for(int oy=-1;oy<=1;oy++) {
				for(int ox=-1;ox<=1;ox++) {
					if(orthoOnly && ox != 0 && oy != 0) continue;
					if(!includeSelf && ox == 0 && oy == 0) continue;
					if(x+ox < MinX || x+ox>=MaxX || y+oy < MinY || y+oy>=MaxY) {
						yield return edgeHandler == null ? 0 : edgeHandler();
					}
					else
						yield return this[x+ox, y+oy];
				}
			}
		}

		public IEnumerable<int> GetNeighbors(Vector2 p, bool orthoOnly, bool includeSelf) {
			return GetNeighbors(p.x, p.y, orthoOnly, includeSelf, returnZero);
		}

		public IEnumerable<int> GetNeighbors(Vector2 p, bool orthoOnly, bool includeSelf, EdgeHandler edgeHandler) {
			return GetNeighbors(p.x, p.y, orthoOnly, includeSelf, edgeHandler);
		}

		public void Rotate(Orientation northBecomes) {
			int[,] ncells = null;
			switch(northBecomes) {
				case Orientation.NORTH:
					ncells = new int[width,height];
					for(int ny = 0; ny < height; ny++) {
						for(int nx = 0; nx < width; nx++) {
							ncells[nx,ny] = cells[nx,ny];
						}
					}
					break;
				case Orientation.SOUTH:
					ncells = new int[width,height];
					for(int ny = 0; ny < height; ny++) {
						for(int nx = 0; nx < width; nx++) {
							ncells[nx,ny] = cells[width-nx-1,height-ny-1];
						}
					}
					offsetx = width-offsetx;
					break;
				case Orientation.EAST:
					ncells = new int[height,width];
					for(int ny = 0; ny < width; ny++) {
						for(int nx = 0; nx < height; nx++) {
							ncells[nx,ny] = cells[ny,width-nx-1];
						}
					}
					int t = offsetx;
					offsetx = -offsety;
					offsety = t;
					break;
				case Orientation.WEST:
					ncells = new int[height,width];
					for(int ny = 0; ny < width; ny++) {
						for(int nx = 0; nx < height; nx++) {
							ncells[nx,ny] = cells[width-ny-1,nx];
						}
					}
					int tm = offsetx;
					offsetx = offsety;
					offsety = -tm;
					break;
			}
			cells = ncells;
			width = cells.GetLength(0);
			height = cells.GetLength(1);
		}

		public void Translate(int x, int y) {
			offsetx += x;
			offsety += y;
		}

		public void Translate(Vector2 amt) {
			Translate(amt.x, amt.y);
		}

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			int pad = 0;
			foreach(int y in cells) {
				pad = Math.Max(pad, y);
			}
			pad = (int)Math.Log(pad)+1;
			//(int)Math.Log(cells.Max())+1;
			for(int y = 0; y < height; y++) {
				for(int x = 0; x < width; x++) {
					sb.Append(this[x,y,false].ToString().PadLeft(pad,' '));
				}
				sb.Append('\n');
			}
			sb.Remove(sb.Length-1,1);
			return sb.ToString();
		}
		public string ToString(string format) {
			StringBuilder sb = new StringBuilder();
			int pad = 0;
			int max = int.MinValue;
			foreach(int y in cells) {
				max = Math.Max(max, y);
			}
			pad = (int)Math.Log(max)+1;
			string numFormat = "";
			//(int)Math.Log(cells.Max())+1;
			if(format.Substring(0,4) == "char") {
				if(format == "char") format += "+32";
				int.TryParse(format.Substring(4, format.Length-4), out int v);
				for(int y = 0; y < height; y++) {
					for(int x = 0; x < width; x++) {
						sb.Append(((char)(this[x,y,false]+v)).ToString());
					}
					sb.Append('\n');
				}
				sb.Remove(sb.Length-1,1);
				return sb.ToString();
			}
			else if(format[0] == 'P' && int.TryParse(format.Substring(1, format.Length-1), out int v)) {
				pad = v;
			}
			else {
				numFormat = format;
				pad = max.ToString(numFormat).Length+1;
			}
			for(int y = 0; y < height; y++) {
				for(int x = 0; x < width; x++) {
					sb.Append(this[x,y,false].ToString(numFormat).PadLeft(pad,' '));
				}
				sb.Append('\n');
			}
			sb.Remove(sb.Length-1,1);
			return sb.ToString();
		}

		public delegate bool ShouldFill(int neighborValue, int thisValueBeforeFill);
		public static ShouldFill equalVal = (n,t) => n==t;

		public long FloodFill(Vector2 pos, int fillValue, ShouldFill shouldFill, bool allowDiagonals=false) {
			return FloodFill(pos.x, pos.y, fillValue, shouldFill, returnNegInf,allowDiagonals);
		}

		public long FloodFill(Vector2 pos, int fillValue, ShouldFill shouldFill, EdgeHandler edgeHandler, bool allowDiagonals=false) {
			return FloodFill(pos.x, pos.y, fillValue, shouldFill, edgeHandler,allowDiagonals);
		}

		public long FloodFill(int x, int y, int fillValue, ShouldFill shouldFill, bool allowDiagonals=false) {
			return FloodFill(x, y, fillValue, shouldFill, returnNegInf,allowDiagonals);
		}

		public long FloodFill(int x, int y, int fillValue, ShouldFill shouldFill, EdgeHandler edgeHandler, bool allowDiagonals=false) {
			long size = 1;
			if(x >= MaxX || y >= MaxY || x < MinX || y < MinY) return 0;
			int L = cells[x,y];
			if(L == fillValue) return 0;

			int N = (y == MinY)?edgeHandler():cells[x,y-1];
			int W = (x == MinX)?edgeHandler():cells[x-1,y];

			int S = (y == MaxY-1)?edgeHandler():cells[x,y+1];
			int E = (x == MaxX-1)?edgeHandler():cells[x+1,y];

			cells[x,y] = fillValue;
			if(shouldFill(N,L)) {
				size += FloodFill(x, y-1, fillValue, shouldFill, edgeHandler, allowDiagonals);
			}
			if(shouldFill(S,L)) {
				size += FloodFill(x, y+1, fillValue, shouldFill, edgeHandler, allowDiagonals);
			}
			if(shouldFill(W,L)) {
				size += FloodFill(x-1, y, fillValue, shouldFill, edgeHandler, allowDiagonals);
			}
			if(shouldFill(E,L)) {
				size += FloodFill(x+1, y, fillValue, shouldFill, edgeHandler, allowDiagonals);
			}

			if(allowDiagonals) {
				int NE = (x == MaxX-1 && y == MinY)?edgeHandler():cells[x+1,y-1];
				int NW = (x == MinX && y == MinY)?edgeHandler():cells[x-1,y-1];

				int SW = (x == MinX && y == MaxY-1)?edgeHandler():cells[x-1,y+1];
				int SE = (x == MaxX-1 && y == MaxY-1)?edgeHandler():cells[x+1,y+1];

				if(shouldFill(NE,L)) {
					size += FloodFill(x+1, y-1, fillValue, shouldFill, edgeHandler, allowDiagonals);
				}
				if (shouldFill(NW,L)) {
					size += FloodFill(x-1, y-1, fillValue, shouldFill, edgeHandler, allowDiagonals);
				}
				if(shouldFill(SW,L)) {
					size += FloodFill(x-1, y+1, fillValue, shouldFill, edgeHandler, allowDiagonals);
				}
				if(shouldFill(SE,L)) {
					size += FloodFill(x+1, y+1, fillValue, shouldFill, edgeHandler, allowDiagonals);
				}
			}
			return size;
		}

		public delegate bool FeatureSpec(int[,] values);

		public List<Vector2> LocateFeature(FeatureSpec featureIdentifier, List<Vector2> offsets, EdgeHandler edgeHandler) {
			List<Vector2> ret = new List<Vector2>();
			int mx = offsets.Max(p => p.x);
			int Mx = offsets.Min(p => p.x);
			int my = offsets.Max(p => p.y);
			int My = offsets.Min(p => p.y);
			int w = Mx - mx + 1; 
			int h = My - my + 1;
			
			for(int y = MinY; y < MaxY; y++) {
				for(int x = MinX; x < MinX; x++) {
					int[,] vals = new int[w,h];
					foreach(Vector2 off in offsets) {
						if(x + off.x < MinX || x+off.x >= MaxX || y + off.y < MinY || y+off.y >= MaxY) {
							vals[off.x+mx,off.y+my] = edgeHandler();
						}
						else {
							vals[off.x+mx,off.y+my] = cells[x+off.x,y+off.y];
						}
					}
					if(featureIdentifier(vals)) {
						ret.Add(new Vector2(x,y));
					}
				}
			}
			return ret;
		}
	}
}