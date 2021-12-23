using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Draco18s.AoCLib {
	public class Grid3D {
		int[,,] cells;
		private int width;
		private int depth;
		private int height;
		private int offsetx;
		private int offsety;
		private int offsetz;

		public delegate int EdgeHandler();
		public static EdgeHandler returnZero = () => 0;
		public static EdgeHandler returnInf = () => int.MaxValue;
		public static EdgeHandler returnNegInf = () => int.MinValue;

		public Grid3D(int w, int  d, int h, int offx=0, int offy=0, int offz=0, bool isChar=false) {
			width = w;
			depth =  d;
			height = h;
			cells = new int[w,d,h];
			offsetx = offx;
			offsety = offy;
			offsetz = offz;
		}

		public int Width => width;
		public int Depth => depth;
		public int Height => height;
		public int MinX => offsetx;
		public int MinY => offsety;
		public int MinZ => offsetz;
		public int MaxX => width+offsetx;
		public int MaxY => depth+offsety;
		public int MaxZ => height+offsetz;

		public int this[int X, int Y, int Z, bool useOffset=true]
		{
			get => cells[X-(useOffset?offsetx:0),Y-(useOffset?offsety:0),Z-(useOffset?offsetz:0)];
			set => cells[X-(useOffset?offsetx:0),Y-(useOffset?offsety:0),Z-(useOffset?offsetz:0)] = value;
		}

		public int GetLength(int i) {
			return cells.GetLength(i);
		}

		///<summary>
		/// Increases the grid in the positive direction if parameters are positive and in the negative
		/// direction if parameters are negative. The value accessed at this[0,0] remains the same.
		///</summary>
		public void IncreaseGridBy(int x, int y, int z, EdgeHandler edgeHandler) {
			int nwidth = width + Math.Abs(x);
			int ndepth = depth + Math.Abs(y);
			int nheight = height + Math.Abs(z);
			int[,,] ncells = new int[nwidth,ndepth,nheight];

			if(edgeHandler != null) {
				for(int nz = 0; nz < nheight; nz++) {
					for(int ny = 0; ny < ndepth; ny++) {
						for(int nx = 0; nx < nwidth; nx++) {
							int ox = nx;
							int oy = ny;
							int oz = nz;
							if(x < 0) ox += x;
							if(y < 0) oy += y;
							if(z < 0) oz += z;
							ncells[nx,ny,nz] = ox >= 0 && ox < width && oy >= 0 && oy < depth && oz >= 0 && oz < height ? cells[ox,oy,oz] : edgeHandler();
						}
					}
				}
			}
			width = nwidth;
			depth = ndepth;
			height = nheight;
			cells = ncells;
			if(x < 0) offsetx += x;
			if(y < 0) offsety += y;
			if(z < 0) offsetz += z;
		}

		//public void IncreaseGridBy(Vector2 amt, EdgeHandler edgeHandler) {
		//	IncreaseGridBy(amt.x, amt.y, edgeHandler);
		//}

		///<summary>
		/// Decreases the grid in the positive direction if parameters are positive and in the negative
		/// direction if parameters are negative. The value accessed at this[0,0] remains the same.
		///</summary>
		public void DecreaseGridBy(int x, int y, int z) {
			int nwidth = width - Math.Abs(x);
			int ndepth = depth - Math.Abs(y);
			int nheight = height - Math.Abs(z);
			int[,,] ncells = new int[nwidth,ndepth,nheight];

			for(int nz = 0; nz < nheight; nz++) {
				for(int ny = 0; ny < ndepth; ny++) {
					for(int nx = 0; nx < nwidth; nx++) {
						int ox = nx;
						int oy = ny;
						int oz = nz;
						if(x < 0) ox -= x;
						if(y < 0) oy -= y;
						if(z < 0) oz -= z;
						ncells[nx,ny,nz] = ox >= 0 && ox < width && oy >= 0 && oy < depth && oz >= 0 && oz < height ? cells[ox,oy,oz] : 0;
					}
				}
			}
			width = nwidth;
			depth = ndepth;
			height = nheight;
			cells = ncells;
			if(x < 0) offsetx -= x;
			if(y < 0) offsety -= y;
			if(z < 0) offsetz -= z;
		}

		public void TrimGrid(EdgeHandler edgeHandler) {
			DecreaseGridBy(MinX - GetMinimumX(edgeHandler), MinY - GetMinimumY(edgeHandler), MinZ - GetMinimumZ(edgeHandler));
			DecreaseGridBy(MaxX - GetMaximumX(edgeHandler)-1, MaxY - GetMaximumY(edgeHandler)-1, MaxZ - GetMaximumZ(edgeHandler)-1);
		}

		public int GetMinimumX(EdgeHandler edgeHandler) {
			int edgeVal = edgeHandler();
			for(int x=MinX;x<MaxX;x++) {
				for(int y=MinY;y<MaxY;y++) {
					for(int z=MinZ;z<MaxZ;z++) {
						if(this[x,y,z] == edgeVal) continue;
						return x;
					}
				}
			}
			return MinX;
		}

		public int GetMinimumY(EdgeHandler edgeHandler) {
			int edgeVal = edgeHandler();
			for(int y=MinY;y<MaxY;y++) {
				for(int x=MinX;x<MaxX;x++) {
					for(int z=MinZ;z<MaxZ;z++) {
						if(this[x,y,z] == edgeVal) continue;
						return y;
					}
				}
			}
			return MinY;
		}

		public int GetMinimumZ(EdgeHandler edgeHandler) {
			int edgeVal = edgeHandler();
			for(int z=MinZ;z<MaxZ;z++) {
				for(int y=MinY;y<MaxY;y++) {
					for(int x=MinX;x<MaxX;x++) {
						if(this[x,y,z] == edgeVal) continue;
						return y;
					}
				}
			}
			return MinZ;
		}

		public int GetMaximumX(EdgeHandler edgeHandler) {
			int edgeVal = edgeHandler();
			for(int x=MaxX-1;x>=MinX;x--) {
				for(int y=MinY;y<MaxY;y++) {
					for(int z=MinZ;z<MaxZ;z++) {
						if(this[x,y,z] == edgeVal) continue;
						return x;
					}
				}
			}
			return MaxX;
		}

		public int GetMaximumY(EdgeHandler edgeHandler) {
			int edgeVal = edgeHandler();
			for(int z=MaxZ-1;z>=MinZ;z--) {
				for(int y=MaxY-1;y>=MinY;y--) {
					for(int x=MinX;x<MaxX;x++) {
						if(this[x,y,z] == edgeVal) continue;
						return y;
					}
				}
			}
			return MaxY;
		}

		public int GetMaximumZ(EdgeHandler edgeHandler) {
			int edgeVal = edgeHandler();
			for(int z=MinZ;z<MaxZ;z++) {
				for(int x=MaxX-1;x>=MinX;x--) {
					for(int y=MinY;y<MaxY;y++) {
						if(this[x,y,z] == edgeVal) continue;
						return x;
					}
				}
			}
			return MaxZ;
		}

		public void Rotate(Orientation northBecomes) {
			int[,,] ncells = null;
			switch(northBecomes) {
				case Orientation.NORTH:
					ncells = new int[width,depth,height];
					for(int nz = 0; nz < height; nz++) {
						for(int ny = 0; ny < depth; ny++) {
							for(int nx = 0; nx < width; nx++) {
								ncells[nx,ny,nz] = cells[nx,ny,nz];
							}
						}
					}
					break;
				case Orientation.SOUTH:
					ncells = new int[width,depth,height];
					for(int nz = 0; nz < height; nz++) {
						for(int ny = 0; ny < depth; ny++) {
							for(int nx = 0; nx < width; nx++) {
								ncells[nx,ny,nz] = cells[width-nx-1,depth-ny-1,nz];
							}
						}
					}
					offsetx = width-offsetx;
					break;
				case Orientation.EAST:
					ncells = new int[depth,width,height];
					for(int nz = 0; nz < height; nz++) {
						for(int ny = 0; ny < width; ny++) {
							for(int nx = 0; nx < depth; nx++) {
								ncells[nx,ny,nz] = cells[ny,width-nx-1,nz];
							}
						}
					}
					int t = offsetx;
					offsetx = -offsety;
					offsety = t;
					break;
				case Orientation.WEST:
					ncells = new int[depth,width,height];
					for(int nz = 0; nz < height; nz++) {
						for(int ny = 0; ny < width; ny++) {
							for(int nx = 0; nx < depth; nx++) {
								ncells[nx,ny,nz] = cells[width-ny-1,nx,nz];
							}
						}
					}
					int tm = offsetx;
					offsetx = offsety;
					offsety = -tm;
					break;
			}
			cells = ncells;
			width = cells.GetLength(0);
			depth = cells.GetLength(1);
		}

		public void Rotate(UpwiseOrientation upBecomes) {
			int[,,] ncells = null;
			switch(upBecomes) {
				case UpwiseOrientation.UP:
					ncells = new int[width,depth,height];
					for(int nz = 0; nz < height; nz++) {
						for(int ny = 0; ny < depth; ny++) {
							for(int nx = 0; nx < width; nx++) {
								ncells[nx,ny,nz] = cells[nx,ny,nz];
							}
						}
					}
					break;
				case UpwiseOrientation.DOWN:
					ncells = new int[width,depth,height];
					for(int nz = 0; nz < height; nz++) {
						for(int ny = 0; ny < depth; ny++) {
							for(int nx = 0; nx < width; nx++) {
								ncells[nx,ny,nz] = cells[width-nx-1,ny,height-nz-1];
							}
						}
					}
					offsetx = width-offsetx;
					break;
				case UpwiseOrientation.RIGHT:
					ncells = new int[height,depth,width];
					for(int nz = 0; nz < height; nz++) {
						for(int ny = 0; ny < width; ny++) {
							for(int nx = 0; nx < depth; nx++) {
								ncells[nx,ny,nz] = cells[nz,ny,width-nx-1];
							}
						}
					}
					int t = offsetx;
					offsetx = -offsetz;
					offsetz = t;
					break;
				case UpwiseOrientation.LEFT:
					ncells = new int[height,depth,width];
					for(int nz = 0; nz < height; nz++) {
						for(int ny = 0; ny < width; ny++) {
							for(int nx = 0; nx < depth; nx++) {
								ncells[nx,ny,nz] = cells[width-nz-1,ny,nx];
							}
						}
					}
					int tm = offsetx;
					offsetx = offsetz;
					offsetz = -tm;
					break;
			}
			cells = ncells;
			height = cells.GetLength(0);
			width = cells.GetLength(2);
		}

		public void Translate(int x, int y, int z) {
			offsetx += x;
			offsety += y;
			offsetz += z;
		}

		public void InsertValueAt(int val, int x, int y, int z, EdgeHandler edgeHandler) {
			int dx = 0;
			int dy = 0;
			int dz = 0;
			if(x < MinX) dx = x - MinX;
			if(y < MinY) dy = y - MinY;
			if(z < MinZ) dz = z - MinZ;

			IncreaseGridBy(dx, dy, dz, null);
			
			dx = 0;
			dy = 0;
			dz = 0;
			if(x >= MaxX) dx = (x+1) - MaxX;
			if(y >= MaxY) dy = (y+1) - MaxY;
			if(z >= MaxZ) dz = (z+1) - MaxZ;

			IncreaseGridBy(dx, dy, dz, null);
			this[x,y,z] = val;
		}
	}
}