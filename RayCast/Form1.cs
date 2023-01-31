using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace RayCast {
	public partial class Form1 : Form {
		//Graphics RaycastViewCanvas;
		//Graphics MinimapCanvas;
		Level CurrentLevel;
		Camera mainCamera;
		public bool Debug = false;
		public bool RayLock = false;

		float scalingFactor;
		int xOffset;
		int yOffset;

		public List<WallImpact> wallImpacts = new List<WallImpact>();

		public readonly int[][][] WallColors = {        // Color int as found in level maps to 2 RGB values = NS and EW
				new int[][] { new int[] {0, 0, 0 }, new int[] {0, 0, 0 } }, // ZERO - not used
				new int[][] { new int[] {0, 0, 200 }, new int[] {0, 200, 0 } }
		};

		public Form1() {
			InitializeComponent();
			//RaycastViewCanvas = raycastView.CreateGraphics();
			//MinimapCanvas = minimap.CreateGraphics();
			Level myBasicLevel = new Level(new int[][] {
				new int[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
				new int[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
				new int[] {1, 0, 1, 1, 1, 1, 0, 1, 0, 1},
				new int[] {1, 0, 1, 0, 0, 0, 0, 0, 0, 1},
				new int[] {1, 0, 1, 0, 1, 1, 1, 1, 0, 1},
				new int[] {1, 0, 1, 0, 1, 0, 0, 0, 0, 1},
				new int[] {1, 0, 1, 0, 1, 1, 1, 1, 0, 1},
				new int[] {1, 0, 1, 0, 0, 0, 0, 1, 0, 1},
				new int[] {1, 0, 1, 1, 1, 1, 1, 1, 0, 1},
				new int[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
				new int[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
			});
			CurrentLevel = myBasicLevel;
			mainCamera = new Camera(70, 16, 0.05f, new JT.Pect(5.5f, 5.5f));
			//WindowState = FormWindowState.Maximized;
			//for (double x = 0; x < 5; x += 0.25) {
			//	for (double y = 0; y < 5; y += 0.25) {
			//		Console.WriteLine((Math.Sqrt(Math.Pow(Math.Abs((int)(x + 0.5) - x), 2) + Math.Pow(Math.Abs((int)(y + 0.5) - y), 2))).ToString());
			//	}
			//}
			//Console.WriteLine("Math Test: " + Math.Abs((int)(0.25 + 0.5) - 0.25).ToString());
			//Console.WriteLine("Math Test: " + Math.Abs((int)(0.75 + 0.5) - 0.75).ToString());
		}

		public class Level {
			public int[][] Data;
			public int Width;
			public int Height;
			public Level(int[][] data) {
				this.Data = data;
				this.Height = data.Length;
				this.Width = data[0].Length;
			}
		}
		public class Camera {
			public int FOV;
			public int Res;
			public float DepthRes;
			public JT.Pect pect;
			public bool Smoothing;


			public Camera(int FOV, int Res, float DepthRes, JT.Pect pect, bool Smoothing = false) {
				this.FOV = FOV;
				this.Res = Res;
				this.DepthRes = DepthRes;
				this.pect = pect;
				this.Smoothing = Smoothing;
			}
		}
		public void DrawMinimap(Level lv, Graphics CanvasToDraw) {
			// Gets which of the ratios between level and output are best for not cropping (The scaling factor that is closer to 0 will produce the
			// smaller picture, so it's the largest it can possibly be while still fitting)
			scalingFactor = new float[] { minimap.Width / lv.Width, minimap.Height / lv.Height }.Min();
			// Offesets to center the minimap within the control
			xOffset = (int)((minimap.Width - (lv.Width * scalingFactor)) / 2);
			yOffset = (int)((minimap.Height - (lv.Height * scalingFactor)) / 2);
			Brush MMBrush0 = Brushes.DarkGray;
			Brush MMBrush1 = Brushes.Black;
			Brush MMBrush1Else = Brushes.Red;
			for (int y = 0; y < lv.Height; y++) {
				for (int x = 0; x < lv.Width; x++) {
					if (lv.Data[y][x] == 0) {
						CanvasToDraw.FillRectangle(MMBrush0, (x * scalingFactor) + xOffset, (y * scalingFactor) + yOffset, scalingFactor, scalingFactor);
					} else if (lv.Data[y][x] == 1) {
						CanvasToDraw.FillRectangle(MMBrush1, (x * scalingFactor) + xOffset, (y * scalingFactor) + yOffset, scalingFactor, scalingFactor);
					} else {
						CanvasToDraw.FillRectangle(MMBrush1Else, (x * scalingFactor) + xOffset, (y * scalingFactor) + yOffset, scalingFactor, scalingFactor);
					}
				}
			}
		}
		public void AddPlayerToMiniMap(Graphics CanvasToDraw) {
			double triSize = 0.6;
			int ax = (int)(mainCamera.pect.x * scalingFactor) + xOffset;
			int ay = (int)(mainCamera.pect.y * scalingFactor) + yOffset;
			int bx = (int)((mainCamera.pect.x + (JT.SinD(mainCamera.pect.Direction + 162) * triSize)) * scalingFactor) + xOffset;
			int by = (int)((mainCamera.pect.y - (JT.CosD(mainCamera.pect.Direction + 162) * triSize)) * scalingFactor) + yOffset;
			int cx = (int)((mainCamera.pect.x + (JT.SinD(mainCamera.pect.Direction + 198) * triSize)) * scalingFactor) + xOffset;
			int cy = (int)((mainCamera.pect.y - (JT.CosD(mainCamera.pect.Direction + 198) * triSize)) * scalingFactor) + yOffset;
			CanvasToDraw.FillPolygon(Brushes.Yellow, new PointF[] {
				new Point(ax, ay),
				new Point(bx, by),
				new Point(cx, cy)
			});
		}
		public class WallImpact {
			public int TileNumber = -1;
			public JT.Pect pect = new JT.Pect();
			public float impactDirection = 0;
			public WallImpact(int TileNumber, JT.Pect pect, float impactDirection) {
				this.TileNumber = TileNumber;
				this.pect = pect;
				this.impactDirection = impactDirection;
			}
			public string ToDebugString() {
				return "Pect: (" + pect.ToDebugString() + "), impactDirection: " + impactDirection.ToString() + ", TileNumber: " + TileNumber.ToString();
			}
		}
		//private WallImpact ShootRay(Graphics CanvasToDraw, float RelativeDirection, double Distance, bool drawRays = true, bool DistanceSpeeder = true, bool BackupSpeeder = true) {
		//	return ShootRay(CanvasToDraw, mainCamera.pect, RelativeDirection, Distance, DistanceSpeeder, BackupSpeeder);
		//}
		private WallImpact ShootRay(Graphics CanvasToDraw, float RelativeDirection, float DepthRes, bool drawRays = true, bool FancyStuff = false) {
			JT.Pect StartPect = mainCamera.pect.Clone();
			StartPect.Direction += RelativeDirection;
			Brush Bcolor;
			float Distance = 0;
			while (CurrentLevel.Data[StartPect.yI][StartPect.xI] == 0) {
				Bcolor = Brushes.Blue;
				//if (FancyStuff) {
				//	double distanceScaler = 8*(new double[] { Math.Abs((int)(StartPect.x + 0.5) - StartPect.x), Math.Abs((int)(StartPect.y + 0.5) - StartPect.y) }.Min());
				//	if (distanceScaler < distanceScalerMinimum) { distanceScaler = distanceScalerMinimum; Bcolor = Brushes.Yellow; }
				//	NewDistance = Distance * distanceScaler;
				//}
				StartPect.Move(DepthRes);
				Distance += DepthRes;

				if (drawRays) {
					int ax = (int)(StartPect.x * scalingFactor) + xOffset;
					int ay = (int)(StartPect.y * scalingFactor) + yOffset;
					CanvasToDraw.FillEllipse(Bcolor, ax - 3, ay - 3, 6, 6);
				}
			}
			while (CurrentLevel.Data[StartPect.yI][StartPect.xI] != 0) {
				StartPect.Move(-DepthRes / 4);
				//StartPect.Move(-DepthRes / (float)(Math.Pow(2, 4 - (0.5 * Distance)) + 1));
				Bcolor = Brushes.Yellow;
				if (drawRays) {
					int ax = (int)(StartPect.x * scalingFactor) + xOffset;
					int ay = (int)(StartPect.y * scalingFactor) + yOffset;
					CanvasToDraw.FillEllipse(Bcolor, ax - 3, ay - 3, 6, 6);
				}
			}
			while (CurrentLevel.Data[StartPect.yI][StartPect.xI] == 0) {
				StartPect.Move(DepthRes / 8);
				//StartPect.Move(-DepthRes / (float)(Math.Pow(2, 4 - (0.5 * Distance)) + 1));
				Bcolor = Brushes.Green;
				if (drawRays) {
					int ax = (int)(StartPect.x * scalingFactor) + xOffset;
					int ay = (int)(StartPect.y * scalingFactor) + yOffset;
					CanvasToDraw.FillEllipse(Bcolor, ax - 3, ay - 3, 6, 6);
				}
			}
			// Test directions, right now blah[][] should != 0
			float oldDir = StartPect.Direction;
			float impactDirection = 0;
			int[] directions = new int[] { 0, 90, 180, 270 }; // Tests the impact direction
			for (int i = 0; i < directions.Length; i++) {
				StartPect.Direction = directions[i];
				StartPect.Move(DepthRes / 8);
				if (CurrentLevel.Data[StartPect.yI][StartPect.xI] != 0) {
					StartPect.Move(-DepthRes / 8);
				} else {
					Bcolor = Brushes.Red;
					if (drawRays) {
						int ax = (int)(StartPect.x * scalingFactor) + xOffset;
						int ay = (int)(StartPect.y * scalingFactor) + yOffset;
						CanvasToDraw.FillEllipse(Bcolor, ax - 3, ay - 3, 6, 6);
					}
					StartPect.Move(-DepthRes / 8);
					impactDirection = directions[i];
					break;
				}
			}
			StartPect.Direction = oldDir; // Return direction to it's original value
			return new WallImpact(CurrentLevel.Data[StartPect.yI][StartPect.xI], StartPect, impactDirection);
		}
		private void ShootRays(Graphics CanvasToDraw, bool drawRays = true) {
			RayLock = true;
			wallImpacts.Clear();
			int lines = raycastView.Widt / mainCamera.Res; // lines to draw
			float degreesPerTurn = (float)mainCamera.FOV / lines;
			for (int i = -lines / 2; i <= lines / 2 + 1; i++) {
				WallImpact impact = ShootRay(CanvasToDraw, i * degreesPerTurn, mainCamera.DepthRes, drawRays: drawRays);
				wallImpacts.Add(impact);
			}
			RayLock = false;
		}

		private float LensCorrect(float theta, float distance) {
			// Adjacent = cos(theta) * hypotenuse
			//                         +---@ impact
			// lens corrected distance |  /
			//                         | / ray
			//                         A/
			return JT.CosD(theta) * distance;
		}

		private void Minimap_Paint(object sender, PaintEventArgs e) {
			Graphics canvas = e.Graphics;
			canvas.Clear(Color.Gray);
			DrawMinimap(CurrentLevel, canvas);
			AddPlayerToMiniMap(canvas);
			ShootRays(canvas, true);
		}
		private void RaycastView_Paint(object sender, PaintEventArgs e) {
			Graphics canvas = e.Graphics;
			canvas.Clear(Color.Gray);
			if (!RayLock) {

				for (int i = 0; i < wallImpacts.Count - 1; i++) {
					float width = mainCamera.Res;
					float x = i * width;


					float distance = mainCamera.pect.Distance(wallImpacts[i].pect);
					distance = LensCorrect(mainCamera.pect.Direction - wallImpacts[i].pect.Direction, distance);
					distance = (distance < 0.2f) ? 0.2f : distance;
					float height = (0.4f / distance) * raycastView.Height;
					float y = (raycastView.Height - height) / 2;

					// Color?
					int ColorNS = (JT.CompassDirection(wallImpacts[i].impactDirection) == JT.NSEW.N || JT.CompassDirection(wallImpacts[i].impactDirection) == JT.NSEW.S) ? 1 : 0;
					int[] color = (int[])(WallColors[wallImpacts[i].TileNumber][ColorNS]).Clone();

					for (int c = 0; c < color.Length; c++) { // Loop through each of r, g, and b
						color[c] = (int)(color[c] * (1 / distance));    // Use distance to darken the color
						color[c] = (color[c] > 255) ? 255 : color[c];   // Bound to 255
					}

					canvas.FillRectangle(new SolidBrush(JT.FromRGB(color)), x, y, width, height);
				}
			} else {
				Console.WriteLine("Raylock on");
			}
		}
		private void GameTimer_Tick(object sender, EventArgs e) {
			if (Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.A)) {
				mainCamera.pect.Direction -= 3;
			}
			if (Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.D)) {
				mainCamera.pect.Direction += 3;
			}
			if (Keyboard.IsKeyDown(Key.Q)) {
				TryMove(270);
			}
			if (Keyboard.IsKeyDown(Key.E)) {
				TryMove(90);
			}
			if (Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.W)) {
				TryMove(0);
			}
			if (Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.S)) {
				TryMove(180);
			}
			minimap.Invalidate();
			raycastView.Invalidate();
		}
		private void TryMove(float RelativeRotation, float Distance1 = 0.05f, float Distance2 = 0.025f) {
			mainCamera.pect.MoveRR(Distance1, RelativeRotation, JT.Pect.CoordFilter.X); // Move only one direction, 0=x, 1=y
			if (CurrentLevel.Data[mainCamera.pect.yI][mainCamera.pect.xI] != 0) {
				mainCamera.pect.MoveRR(-Distance1, RelativeRotation, JT.Pect.CoordFilter.X);
				mainCamera.pect.MoveRR(Distance2, RelativeRotation, JT.Pect.CoordFilter.X);
				if (CurrentLevel.Data[mainCamera.pect.yI][mainCamera.pect.xI] != 0) {
					mainCamera.pect.MoveRR(-Distance2, RelativeRotation, JT.Pect.CoordFilter.X);
				}
			}
			mainCamera.pect.MoveRR(Distance1, RelativeRotation, JT.Pect.CoordFilter.Y); // Move only one direction, 0=x, 1=y
			if (CurrentLevel.Data[mainCamera.pect.yI][mainCamera.pect.xI] != 0) {
				mainCamera.pect.MoveRR(-Distance1, RelativeRotation, JT.Pect.CoordFilter.Y);
				mainCamera.pect.MoveRR(Distance2, RelativeRotation, JT.Pect.CoordFilter.Y);
				if (CurrentLevel.Data[mainCamera.pect.yI][mainCamera.pect.xI] != 0) {
					mainCamera.pect.MoveRR(-Distance2, RelativeRotation, JT.Pect.CoordFilter.Y);
				}
			}
		}
		private void Form1_Layout(object sender, LayoutEventArgs e) {
			raycastView.Size = new Size(((this.Width * 2) / 3) - 6, this.Height - 6);
			raycastView.Location = new Point(3, 3);
			minimap.Size = new Size(((this.Width * 1) / 3) - 6, this.Height - 6);
			minimap.Location = new Point(((this.Width * 2) / 3) + 3, 3);
		}

		private void Form1_KeyPress(object sender, KeyPressEventArgs e) {
			if (e.KeyChar == 'n') {
				if (mainCamera.Res > 1) {
					mainCamera.Res -= 1;
				}
				Console.WriteLine("-Res: " + mainCamera.Res);
			}
			if (e.KeyChar == 'm') {
				mainCamera.Res += 1;
				Console.WriteLine("+Res: " + mainCamera.Res);
			}
			if (e.KeyChar == 'v') {
				mainCamera.FOV -= 1;
				Console.WriteLine("-FOV: " + mainCamera.FOV);
			}
			if (e.KeyChar == 'b') {
				mainCamera.FOV += 1;
				Console.WriteLine("+FOV: " + mainCamera.FOV);
			}
			if (e.KeyChar == 'x') {
				if (mainCamera.DepthRes > 0.1f) {
					mainCamera.DepthRes -= 0.05f;
				} else if (mainCamera.DepthRes > 0.005f) {
					mainCamera.DepthRes -= 0.005f;
				}
				mainCamera.DepthRes = (float)Math.Round(mainCamera.DepthRes, 3);
				Console.WriteLine("-DpR: " + mainCamera.DepthRes);
			}
			if (e.KeyChar == 'c') {
				if (mainCamera.DepthRes > 0.1f) {
					mainCamera.DepthRes += 0.05f;
				} else {
					mainCamera.DepthRes += 0.005f;
				}
				mainCamera.DepthRes = (float)Math.Round(mainCamera.DepthRes, 3);
				Console.WriteLine("+DpR: " + mainCamera.DepthRes);
			}
			if (e.KeyChar == 'z') {
				mainCamera.Smoothing = !mainCamera.Smoothing;
				Console.WriteLine("!Smo: " + mainCamera.Smoothing);
			}
		}
	}

	//public class MazeGenerator {
	//	public class cell {
	//		int x = 0;
	//		int y = 0;
	//		public cell(int x, int y) {
	//			this.x = x;
	//			this.y = y;
	//		}
	//	}
	//	public int[][] genMaze(int width, int height) {
	//		return new int[][] { new int[] { 0 } };
	//	}
	//	public int[][] genMaze() { return genMaze(10, 10); }
	//	public int[][] genMaze(int[] size) { return genMaze(size[0], size[1]); }
	//}
}