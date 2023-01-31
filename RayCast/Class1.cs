using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast {
	public static class JT { // Jimmy Tools
		public static Color FromRGB(int[] color) {
			return Color.FromArgb(color[0], color[1], color[2]);
		}
		public static string PPColor(int[] color) { // Pretty Print
			return "(" + color[0].ToString() + ", " + color[1].ToString() + ", " + color[2].ToString() + ")";
		}
		#region Math Functions
		static public float SinD(float angle) { // Sine in degrees
			return (float)Math.Sin(DegToRad(angle));
		}
		static public float CosD(float angle) { // Cosine in degrees
			return (float)Math.Cos(DegToRad(angle));
		}
		static public float DegToRad(float angle) {
			return (float)Math.PI * angle / 180.0f;
		}
		#endregion

		public enum NSEW {
			N,
			S,
			E,
			W
		}
		static public NSEW CompassDirection(float Direction) {
			if (45 <= Direction && Direction < 135) {
				return NSEW.E;
			} else if (135 <= Direction && Direction < 225) {
				return NSEW.S;
			} else if (225 <= Direction && Direction < 315) {
				return NSEW.W;
			}
			return NSEW.N;
		}
		public class Pect { // Point with direction, I think?
			public float x = 0;
			public float y = 0;
			public int xI {
				get { return (int)x; }
				set { x = value; }
			}
			public int yI {
				get { return (int)y; }
				set { y = value; }
			}
			public float[] Position {
				get {
					return new float[] { x, y };
				}
				set {
					x = value[0];
					y = value[1];
				}
			}
			private float direction = 0;
			public float Direction {
				get { return direction; }
				set {
					direction = value;
					while (direction < 0) {
						direction += 360;
					}
					direction %= 360;
				}
			}
			public NSEW CompassDirection() {
				return JT.CompassDirection(Direction);
			}

			public enum CoordFilter {
				X,
				Y,
				Both
			}
			public void Move(float Distance, float Direction, CoordFilter only=CoordFilter.Both) {
				if (only == CoordFilter.X || only == CoordFilter.Both) {
					this.x += JT.SinD(Direction) * Distance;
				}
				if (only == CoordFilter.Y || only == CoordFilter.Both) {
					this.y -= JT.CosD(Direction) * Distance;
				}
			}
			public void Move(float Distance, CoordFilter only = CoordFilter.Both) { Move(Distance, this.Direction, only); }
			public void MoveRR(float Distance, float Direction, CoordFilter only = CoordFilter.Both) { // RR: Relative Rotation
				Move(Distance, this.Direction + Direction, only);
			}
			public float Distance(Pect PectToCompare) {
				return (float)Math.Sqrt(Math.Pow(PectToCompare.x - this.x, 2) + Math.Pow(PectToCompare.y - this.y, 2)); ;
			}
			public Pect Clone() {
				return new Pect(this);
			}
			public string ToDebugString() {
				return "(x, y): " + "(" + x + ", " + y + ")" + "(xI, yI): " + "(" + xI + ", " + yI + ")" + ", Direction: " + Direction;
			}
			#region Creators
			public Pect(int x = 0, int y = 0, float Direction = 0) {
				this.x = x;
				this.y = y;
				this.Direction = Direction;
			}
			public Pect(float x, float y, float Direction = 0) {
				this.x = (float)x;
				this.y = (float)y;
				this.Direction = Direction;
			}
			public Pect(float[] Position, float Direction = 0) {
				this.Position = Position;
				this.Direction = Direction;
			}
			public Pect(Pect pect) {
				this.Position = pect.Position;
				this.Direction = pect.Direction;
			}

			#endregion
		}
	}
}
