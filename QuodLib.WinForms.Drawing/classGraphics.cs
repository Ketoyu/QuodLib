using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using classMath = QuodLib.Math.General;
using sMath = System.Math;

namespace QuodLib.WinForms.Drawing
{
	/// <summary>
	/// Contains many functions for working with colors, mostly, as well as pens, brushes, and images.
	/// </summary>
	public static class classGraphics
	{
		public static void FillCircle(this Graphics g, Brush brush, float x, float y, float radius)
			=> g.FillPie(brush, x, y, radius * 2F, radius * 2F, 0F, 360F);

        public static void DrawCircle(this Graphics g, Pen pen, float x, float y, float radius)
			=> g.DrawEllipse(pen, x - radius, y - radius, radius, radius);

        public static Point ShiftX(this Point pt, int ofs)
		{
			return new Point(pt.X + ofs, pt.Y);
		}
		public static Point ShiftY(this Point pt, int ofs)
		{
			return new Point(pt.X, pt.Y + ofs);
		}
		public static Point Shift(this Point pt, int ofsX, int ofsY)
		{
			return new Point(pt.X + ofsX, pt.Y + ofsY);
		}

		#region ColorsPensBrushes
			#region Color
		public static Color MColor(byte c)
		{
			return Color.FromArgb(c, c, c);
		}
		public static Color CColor(byte r, byte g, byte b)
		{
			return Color.FromArgb(r, g, b);
		}
		public static Color CColor(byte a, byte r, byte g, byte b)
		{
			return Color.FromArgb(a, r, g, b);
		}
		public static Color CColor(byte a, Color c)
		{
			return Color.FromArgb(a, c.R, c.G, c.B);
		}
        #endregion //Color
        #region Brush
        public static Brush MBrush(byte c)
			=> new SolidBrush(Color.FromArgb(c, c, c));

        public static Brush CBrush(byte r, byte g, byte b)
			=> new SolidBrush(Color.FromArgb(r, g, b));

        public static Brush CBrush(Color c)
			=> new SolidBrush(c);

        public static Brush CBrush(byte a, byte r, byte g, byte b)
			=> new SolidBrush(Color.FromArgb(a, r, g, b));

        public static Brush ToBrush(this Color c)
			=> CBrush(c);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hue">0=white || 1=red, 2=green, 3=blue ||
		///						 4=yellow, 5=cyan, 6=violet ||
		///						 7=orange, 8=lime, 9=sea, 10=teal, 11=purple, 12=pink</param>
		/// <param name="level">Pale[2-14], Pure[1-13], Light[]</param>
		/// <param name="colorDepth">0=pale, 1=pure, 2=light</param>
		/// <returns></returns>
		public static Brush GBrush(byte hue, byte level, byte colorDepth)
		{
			if ((level > 15) || (hue > 12) || (colorDepth > 2))
			{ //out of bounds
				string sLevel = "level must be between 0 and 12!";
				string sHue = "hue must be bewteen 0 and 12!";
				string sCD = "colorDepth must be between 0 and 2!";
				string err = "";
				if (level > 15) { err = sLevel; }
				if (hue > 12) { if (err != "") { err += "\r\n"; } err += sHue; }
				if (colorDepth > 12) { if (err != "") { err += "\r\n"; } err += sCD; }
				throw new Exception(err);
			} else {
				return CBrush(GColor(hue, level, colorDepth));
			}
		}
				#region GBrush[]
		public static Brush[] GBrush(byte hue, byte[] level, byte colorDepth)
		{
			Brush[] rtn = new Brush[level.Count()];
			for (byte i = 0; i < level.Count(); i++)
			{
				rtn[i] = GBrush(hue, level[i], colorDepth);
			}
			return rtn;
		}
		public static Brush[] GBrush(byte[] hue, byte level, byte colorDepth)
		{
			Brush[] rtn = new Brush[hue.Count()];
			for (byte i = 0; i < hue.Count(); i++)
			{
				rtn[i] = GBrush(hue[i], level, colorDepth);
			}
			return rtn;
		}
		public static Brush[] GBrush(byte hue, byte level, byte[] colorDepth)
		{
			Brush[] rtn = new Brush[colorDepth.Count()];
			for (byte i = 0; i < colorDepth.Count(); i++)
			{
				rtn[i] = GBrush(hue, level, colorDepth[i]);
			}
			return rtn;
		}
		public static Brush[][] GBrush(byte[] hue, byte[] level, byte colorDepth)
		{
			Brush[][] rtn = new Brush[hue.Count()][];
			for (byte h = 0; h < hue.Count(); h++)
			{
				rtn[h] = new Brush[level.Count()];
				for (byte l = 0; l < level.Count(); l++)
				{
					rtn[h][l] = GBrush(hue[h], level[l], colorDepth);
				}
			}
			return rtn;
		}
				#endregion //GBrush[]
			#endregion //Brush
			#region Pen
		public static Pen MPen(byte c)
			=> new Pen(MBrush(c));

		public static Pen CPen(Color c)
			=> new Pen(c);

        public static Pen ToPen(Color c)
			=> CPen(c);

        public static Pen CPen(byte alpha, Color c)
			=> new Pen(Color.FromArgb(alpha, c.R, c.G, c.B));

        public static Pen CPen(byte r, byte g, byte b)
			=> new Pen(Color.FromArgb(r, g, b));

        public static Pen CPen(byte a, byte r, byte g, byte b)
			=> new Pen(Color.FromArgb(a, r, g, b));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hue">0=white || 1=red, 2=green, 3=blue ||
        ///						 4=yellow, 5=cyan, 6=violet ||
        ///						 7=orange, 8=lime, 9=sea, 10=teal, 11=purple, 12=pink</param>
        /// <param name="level">Pale[2-14], Pure[1-13], Light[]</param>
        /// <param name="colorDepth">0=pale, 1=pure, 2=light</param>
        /// <returns></returns>
        public static Pen GPen(byte hue, byte level, byte colorDepth)
		{
			if ((level > 15) || (hue > 12) || (colorDepth > 2))
			{ //out of bounds
				string sLevel = "level must be between 0 and 12!";
				string sHue = "hue must be bewteen 0 and 12!";
				string sCD = "colorDepth must be between 0 and 2!";
				string err = "";
				if (level > 15) { err = sLevel; }
				if (hue > 12) { if (err != "") { err += "\r\n"; } err += sHue; }
				if (colorDepth > 12) { if (err != "") { err += "\r\n"; } err += sCD; }
				throw new Exception(err);
			} else {
				return CPen(GColor(hue, level, colorDepth));
			}
		}
				#region GPen[]
		public static Pen[] GPen(byte hue, byte[] level, byte colorDepth)
		{
			Pen[] rtn = new Pen[level.Count()];
			for (byte i = 0; i < level.Count(); i++)
			{
				rtn[i] = GPen(hue, level[i], colorDepth);
			}
			return rtn;
		}
		public static Pen[] GPen(byte[] hue, byte level, byte colorDepth)
		{
			Pen[] rtn = new Pen[hue.Count()];
			for (byte i = 0; i < hue.Count(); i++)
			{
				rtn[i] = GPen(hue[i], level, colorDepth);
			}
			return rtn;
		}
		public static Pen[] GPen(byte hue, byte level, byte[] colorDepth)
		{
			Pen[] rtn = new Pen[colorDepth.Count()];
			for (byte i = 0; i < colorDepth.Count(); i++)
			{
				rtn[i] = GPen(hue, level, colorDepth[i]);
			}
			return rtn;
		}
		public static Pen[][] GPen(byte[] hue, byte[] level, byte colorDepth)
		{
			Pen[][] rtn = new Pen[hue.Count()][];
			for (byte h = 0; h < hue.Count(); h++)
			{
				rtn[h] = new Pen[level.Count()];
				for (byte l = 0; l < level.Count(); l++)
				{
					rtn[h][l] = GPen(hue[h], level[l], colorDepth);
				}
			}
			return rtn;
		}
				#endregion //GPen[]
			#endregion //Pen
		#endregion //ColorsPensBrushes

		public static int Colors_PercentDiff(Color a, Color b)
		{
			return (int)sMath.Ceiling( (double)(sMath.Abs(a.R - b.R) + sMath.Abs(a.G - b.G) + sMath.Abs(a.B - b.B) ) / (3 * 255) );
		}

		#region GColor supports
		public static Color CAverage(Color c1, Color c2)
		{
			byte r, g, b;
			r = CAverage_byte(c1.R, c2.R);
			g = CAverage_byte(c1.G, c2.G);
			b = CAverage_byte(c1.B, c2.B);
			return Color.FromArgb(r, g, b);
		}

		private static ushort CAverage(byte c1, byte c2)
		{
			if (c1 == c2)
			{
				return c1;
			} else if (c1 < c2) {
				return (ushort)(c1 + (Math.General.FInt((double)(c2 - c1) / 2)));
			} else {
				return (ushort)(c2 + (Math.General.FInt((double)(c1 - c2) / 2)));
			}
		}
		private static byte CAverage_byte(byte c1, byte c2)
		{
			if (c1 == c2)
			{
				return c1;
			} else if (c1 < c2) {
				return (byte)(c1 + (Math.General.FInt((double)(c2 - c1) / 2)));
			} else {
				return (byte)(c2 + (Math.General.FInt((double)(c1 - c2) / 2)));
			}
		}
		private static byte CAverage_ushort(ushort c1, ushort c2)
		{
			if ((c1 > 256) || (c2 > 256))
			{
				throw new Exception("One of the colors exceeds 256!");
			} else {
				if (c1 == c2)
				{
					if (c1 > 0) { c1 -= 1; }
					return (byte)c1;
				} else if (c1 < c2) {
					ushort rtn = (ushort)(c1 + (Math.General.FInt((double)(c2 - c1) / 2)));
					if (rtn > 0) { c1 -= 1; }
					return (byte)rtn;
				} else {
					ushort rtn = (ushort)(c2 + (Math.General.FInt((double)(c1 - c2) / 2)));
					if (rtn > 0) { c1 -= 1; }
					return (byte)rtn;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1">0-13</param>
		/// <param name="c2">0-13</param>
		/// <returns></returns>
		private static byte CLevel_Average(byte c1, byte c2)
		{
			if ((c1 > 14) || (c2 > 14))
			{
				throw new Exception("One or both of the parameters exceeded 14!");
			} else {
				if (c1 == c2)
				{
					return c1;
				} else if (c1 < c2) {
					return (byte)(CAverage_ushort(CLevel_Literal(c1), CLevel_Literal(c2)) - 1);
				} else {
					return (byte)(CAverage_ushort(CLevel_Literal(c2), CLevel_Literal(c1)) - 1);
				}
			}
		}
		private static byte CLevel_m(float level_m)
		{
			if ((level_m < 0) || (level_m > 14))
			{
				throw new Exception("level_m cannot be negative or exceed 14!");
			} else if (level_m - (float)Math.General.FInt(level_m) != 0.5) {
				throw new Exception("Please keep level_m limited to a non-integer multiple of 0.5!");
			} else {
				return CLevel_Average((byte)((uint)Math.General.FInt(level_m)), (byte)((uint)Math.General.CInt(level_m)));
			}
		}
		private static byte CLevel(byte level)
		{
			if (level > 14)
			{
				throw new Exception("Level " + level + " exceeds the maximum of 14!");
			} else {
				if (level <= 1)
				{
					return (byte)CLevel_Literal(level);
				}
				else
				{
					return (byte)(CLevel_Literal(level) - 1);
				}
			}
		}
		private static ushort CLevel_Literal(byte level)
		{
			if (level > 14)
			{
				throw new Exception("Level " + level + " exceeds the maximum of 14!");
			} else {
				ushort round = (ushort)Math.General.FInt((double)level / 2);

				ushort calc;
				if (level <= 1)
				{
					calc = (ushort)sMath.Pow(2, round);
				} else {
					calc = (ushort)sMath.Pow(2, round + 1);
				}

				if ((Math.General.num_IsEven(level)) || (level <= 1)) //evens & 0 & 1
				{
					return calc;
				} else { //odds
					return (ushort)(calc * 1.5);
				}

			}
		}
		#endregion //GColor supports

		/// <summary>
		/// A list of the constants ready to use in place of GColor's "hue" parameter
		/// </summary>
		public enum GColors:byte
		{
			white = 0,
			red = 1,
			green = 2,
			blue = 3,
			yellow = 4,
			cyan = 5,
			violet = 6,
			
			orange = 7,
			lime = 8,
			sea = 9,
			teal = 10,
			purple = 11,
			pink = 12
		}

		/// <summary>
		/// [<paramref name="hue"/>: 0=white || 1=red, 2=green, 3=blue || 4=yellow, 5=cyan, 6=violet || =orange, 8=lime, 9=sea, 10=teal, 11=purple, 12=pink]<br />
		/// [<paramref name="level"/>: Pale{scaled along the lines of 'brightness = 2^level', with some minor shifts of level (+-)=2 or so]} Pale{high=255, [mid/low] scaled to (255/10) * [level,level/2]}<br />
		/// [<paramref name="colorDepth"/>: 0=pale,1=pure,2=light]
		/// </summary>
		/// <param name="hue">0=white || <br />
		/// 1=red, 2=green, 3=blue || <br />
		///	4=yellow, 5=cyan, 6=violet || <br />
		/// 7=orange, 8=lime, 9=sea, 10=teal, 11=purple, 12=pink</param>
		/// <param name="level">Pale[2-14]<br />
		/// Pure[1-13]<br />
		///	Light[]</param>
		/// <param name="colorDepth">0=pale, 1=pure, 2=light</param>
		/// <param name="allowFloats"></param>
		/// <returns></returns>
		public static Color GColor(byte hue, float level, byte colorDepth, bool allowFloats)
		{
			if (allowFloats)
			{
				float dec = level - Math.General.FInt(level);
				if (!(Math.General.IntIs(dec / 0.125)))
				{
					throw new Exception("level must be a multiple of one eigth (0.125)!");
				}
				Color low = GColor(hue, (byte)Math.General.FInt(level), colorDepth);
				Color high = GColor(hue, (byte)Math.General.CInt(level), colorDepth);
				switch ((ushort)(dec * 1000))
				{
					case 0:
						return low;
					case 125:
						return CAverage(low, //0
								CAverage(low, CAverage(low, high))); //250
					case 250:
						return CAverage(low, CAverage(low, high));
					case 375:
						return CAverage(CAverage(low, high), //500
									CAverage(low, CAverage(low, high))); //250
					case 500:
						return CAverage(low, high);
					case 625:
						return CAverage(CAverage(low, high), //500
									CAverage(high, CAverage(low, high))); //750
					case 750:
						return CAverage(high, //1,000
									CAverage(low, high)); //500
					case 875:
						return CAverage(high, //1,000
									CAverage(high, CAverage(low, high))); //750
					default:
						throw new Exception("level must be a multiple of one eigth (0.125)!");
				}
			} else {
				return GColor(hue, (byte)level, colorDepth);
			}
		} //<GColor(...)>
		/// <summary>
		/// [<paramref name="hue"/>: 0=white || 1=red, 2=green, 3=blue || 4=yellow, 5=cyan, 6=violet || =orange, 8=lime, 9=sea, 10=teal, 11=purple, 12=pink]<br />
		/// [<paramref name="level"/>: Pale{scaled along the lines of 'brightness = 2^level', with some minor shifts of level (+-)=2 or so]} Pale{high=255, [mid/low] scaled to (255/10) * [level,level/2]}<br />
		/// [<paramref name="colorDepth"/>: 0=pale,1=pure,2=light]
		/// </summary>
		/// <param name="hue">0=white || <br />
		/// 1=red, 2=green, 3=blue || <br />
		///	4=yellow, 5=cyan, 6=violet || <br />
		///	7=orange, 8=lime, 9=sea, 10=teal, 11=purple, 12=pink</param>
		/// <param name="level">Pale[2-14]<br />
		///	Pure[1-13] //revise: 2-14 (have user input 0-12)<br />
		///	Light[0-10]
		/// </param>
		/// <param name="colorDepth">0=pale, 1=pure, 2=light</param>
		/// <returns></returns>
		public static Color GColor(byte hue, byte level, byte colorDepth)
		{
			//dictionary(r, g, b) = 0, 1, 2 || 0, 2, 1 || etc.
			//list(0, 1, 2)
			//0 = high, 1 = med, 2 = low

			if ((level > 12) || (hue > 12) || (colorDepth > 2))
			{ //out of bounds
				string sLevel = "level must be between 0 and 12!";
				string sHue = "hue must be bewteen 0 and 12!";
				string sCD = "colorDepth must be between 0 and 2!";
				string err = "";
				if (level > 12) { err = sLevel; }
				if (hue > 12) { if (err != "") { err += "\r\n"; } err += sHue; }
				if (colorDepth > 12) { if (err != "") { err += "\r\n"; } err += sCD; }
				throw new Exception(err);
			} else {
				Dictionary<string, byte> Clr = new Dictionary<string, byte>() { { "r", 0 }, { "g", 0 }, { "b", 0 } };
				int[] Shd = new int[3]; //fill differently for each colorDepth
				byte typ; //0=white, 1=primary, 2=secondary, 3=trinary
				#region Get typ
				if (hue == 0)
				{
					typ = 0;
				} else if ((hue > 0) && (hue <= 3)) {
					typ = 1;
				} else if ((hue >= 4) && (hue <= 6)) {
					typ = 2;
				} else if ((hue >= 7) && (hue <= 12)) {
					typ = 3;
				} else { //out of bounds
					throw new Exception("hue must be between 0 and 12!");
				}
				#endregion //Get typ
				#region Get hue
				switch (hue)
				{
					//no cases are redundant!
					#region White
					case 0:
						//no change
						break;
					#endregion //White
					#region Primary
					case 1: //red
						Clr["r"] = 0; Clr["g"] = 2; Clr["b"] = 2;
						break;
					case 2: //green
						Clr["r"] = 2; Clr["g"] = 0; Clr["b"] = 2;
						break;
					case 3: //blue
						Clr["r"] = 2; Clr["g"] = 2; Clr["b"] = 0;
						break;
					#endregion //Primary
					#region Secondary
					case 4: //yellow
						Clr["r"] = 0; Clr["g"] = 0; Clr["b"] = 2;
						break;
					case 5: //cyan
						Clr["r"] = 2; Clr["g"] = 0; Clr["b"] = 0;
						break;
					case 6: //violet
						Clr["r"] = 0; Clr["g"] = 2; Clr["b"] = 0;
						break;
					#endregion //Secondary
					#region Trinary
					case 7: //orange
						Clr["r"] = 0; Clr["g"] = 1; Clr["b"] = 2;
						break;
					case 8: //lime
						Clr["r"] = 1; Clr["g"] = 0; Clr["b"] = 2;
						break;
					case 9: //sea
						Clr["r"] = 2; Clr["g"] = 0; Clr["b"] = 1;
						break;
					case 10: //teal
						Clr["r"] = 2; Clr["g"] = 1; Clr["b"] = 0;
						break;
					case 11: //purple
						Clr["r"] = 1; Clr["g"] = 2; Clr["b"] = 0;
						break;
					case 12: //pink
						Clr["r"] = 0; Clr["g"] = 2; Clr["b"] = 1;
						break;
					#endregion //Trinary
				}
				#endregion //Get hue
				switch (colorDepth)
				{
					#region Pale
					case 0: //pale
						if (level > 12)
						{ //out of bounds
							throw new Exception("Pale: level must be between 0 and 12!");
						} else {
							level += 2;
							Shd[0] = CLevel(level);
							switch (typ)
							{
								case 0:
									//no change
									break;
								case 1: case 2:
									Shd[2] = CLevel((byte)(level - 2));
									break;
								case 3:
									Shd[1] = CLevel_Average(level, (byte)(level - 2)); Shd[2] = CLevel((byte)(level - 2));
									break;
							}
							return Color.FromArgb(Shd[Clr["r"]], Shd[Clr["g"]], Shd[Clr["b"]]);
						}
					#endregion //Pale
					#region Pure
					case 1: //pure
						if (level > 12)
						{ //out of bounds
							throw new Exception("Pure: level must be between 0 and 12!");
						} else {
							level += 2;
							Shd[0] = CLevel(level);
							switch (typ)
							{
								case 0:
									//no change
									break;
								case 1: case 2:
									Shd[2] = 0;
									break;
								case 3:
									Shd[2] = 0;
									Shd[1] = CLevel((byte)(level - 2));
									break;
								default:
									throw new Exception("typ miscalculated and fell out of bounds [0, 3]: " + typ);
							}
							return Color.FromArgb(Shd[Clr["r"]], Shd[Clr["g"]], Shd[Clr["b"]]);
						}
					#endregion //Pure
					#region Light
					case 2: //light
						if (level > 10)
						{ //out of bounds
							throw new Exception("Pale: level must be between 0 and 10!");
						} else {
							Shd[0] = 255;
							switch (typ)
							{
								case 0:
									//no change
									break;
								case 1:
								case 2:
									Shd[2] = Math.General.FInt((255 / 10) * (level / 2));
									break;
								case 3:
									Shd[1] = Math.General.FInt((255 / 10) * level);
									break;
								default:
									throw new Exception("typ miscalculated and fell out of bounds [0, 3]: " + typ);
							}
							return Color.FromArgb(Shd[Clr["r"]], Shd[Clr["g"]], Shd[Clr["b"]]);
						}
					#endregion //Light
					default:
						throw new Exception("colorDepth must be between 0 and 2!");
				} //</Switch (colorDepth)>
			} //</if (level < 12)>

		} //</GColor(...)>
		public struct objGColor
		{
			#region Variables
			private byte _ColorDepth, _Brightness, _Hue; //subProperties

			public byte LBrightness {
				get {
					return 0;
				}
			}
			public byte HBrightness {
				get {
					switch (ColorDepth)
					{
						case 0:
						case 1:
							return 12;
						default:
							return 10;
					}
				}
			}
			public byte ColorDepth {
				get {
					return _ColorDepth;
				}
				set {
					this._ColorDepth = Math.General.Limit_H(value, (byte)2);
					ProcessColor();
				}
			}
			public byte Brightness {
				get {
					return _Brightness;
				}
				set {
					this._Brightness = Math.General.Limit(value, (byte)LBrightness, (byte)HBrightness);
					ProcessColor();
				}
			}
			public byte Hue {
				get {
					return _Hue;
				}
				set {
					this._Hue = Math.General.Limit_H(value, (byte)12);
					ProcessColor();
				}
			}
			private Color Color { get; set; }
			private void ProcessColor()
			{
				this.Color = classGraphics.GColor(Brightness, Hue, ColorDepth);
			}
			#endregion //Variables
			#region CreationOverlaods
			public objGColor(byte brightness, byte hue, byte colordepth) : this()
			{
				Brightness = brightness;
				Hue = hue;
				ColorDepth = colordepth;
				Color = classGraphics.GColor(Brightness, Hue, ColorDepth);
			}
			public objGColor(Color C) : this()
			{
				Brightness = 0;
				Hue = 0;
				ColorDepth = 0;
				Color = C;
			}
			#endregion //CreationOverloads
			#region Conversions
			/// <summary>
			/// Sets GColor.Color to the given color; Brightness, Hue, and colorDepth values are left at the default of zero.
			/// </summary>
			/// <param name="C">Custom Color(byte red, byte green, byte blue)</param>
			/// <returns></returns>
			public static implicit operator objGColor(Color C)
			{
				return new objGColor(C);
			}
			/// <summary>
			/// Returns a color processed from Brightness, Hue, and ColorDepth! To get a custom-set color, use GColor.Color!
			/// </summary>
			/// <param name="G">Processed from Brightness, Hue, and ColorDepth.</param>
			/// <returns></returns>
			public static explicit operator Color(objGColor G)
			{
				return G.Color;
			}
			#endregion //Conversions
			#region Recolor
			public void Recolor(byte brightness, byte hue, byte colordepth)
			{
				Brightness = brightness;
				Hue = hue;
				ColorDepth = colordepth;
			}
			public void Recolor(Color C)
			{
				Color = C;
			}
			#endregion //Recolor
			#region ModdedColor
			public Color ModdedColor(sbyte modBrightness, sbyte modHue)
			{
				byte[] Excp = new byte[2];
				switch (ColorDepth)
				{
					case 0:
						if (!Math.General.IsInBounds(Brightness + modBrightness, 0, 12) && !Math.General.IsInBounds(Hue + modHue, 0, 12))
						{
							Excp[0] = 0;
							Excp[1] = 12;
						}
						break;
					case 1:
						if (!Math.General.IsInBounds(Brightness + modBrightness, 0, 12) && !Math.General.IsInBounds(Hue + modHue, 0, 12))
						{
							Excp[0] = 0;
							Excp[1] = 12;
						}
						break;
				}
				if (Excp[2] > 12)
				{
					throw new Exception("[Brightness " + Brightness + "] + [" + modBrightness + "] must be between " + Excp[0] + " and " + Excp[1] +
						"!\r\n[Hue " + Hue + "] + [modHue " + modHue + "] must be between 0 and 12!");
				} else {
					return classGraphics.GColor((byte)(Brightness + modBrightness), (byte)(Hue + modHue), ColorDepth);
				}
			}
			public Color ModdedColor(sbyte modBrightness)
			{
				return ModdedColor(modBrightness, 0);
			}
			#endregion //ModdedColor
		} // </objGColor>
		#region StringFormats
		public static StringFormat strFmCenter()
		{
			StringFormat strFormat = new StringFormat();
			strFormat.Alignment = StringAlignment.Center;
			strFormat.LineAlignment = StringAlignment.Center;
			return strFormat;
		}
		public static StringFormat strFmCenterNear()
		{
			StringFormat strFormat = new StringFormat();
			strFormat.Alignment = StringAlignment.Near;
			strFormat.LineAlignment = StringAlignment.Center;
			return strFormat;
		}
		public static StringFormat strFmCenterFar()
		{
			StringFormat strFormat = new StringFormat();
			strFormat.Alignment = StringAlignment.Far;
			strFormat.LineAlignment = StringAlignment.Center;
			return strFormat;
		}
		#endregion //StringFormats

		/// <summary>
		/// Clones <paramref name="source"/> to a new <see cref="Image"/>, tinted as <paramref name="color"/>.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="color"></param>
		/// <returns>A new, cloned <see cref="Image"/>.</returns>
		public static Image Tint(this Image source, Color color)
		{
			System.Drawing.Imaging.ImageAttributes imageAttributes = new System.Drawing.Imaging.ImageAttributes();
			int width = source.Width;
			int height = source.Height;

			float rgb = (float)1.0 / (float)255.0;
			float[][] colorMatrixElements = { 
			   new float[] {0, 0, 0, 0, 0}, // red *= 0 
			   new float[] {0, 0, 0, 0, 0}, // green *= 0
			   new float[] {0, 0, 0, 0, 0}, // blue *= 0
			   new float[] {0, 0, 0, 1, 0}, // alpha *= 1
			   new float[] {rgb*color.R, rgb*color.G, rgb*color.B, 0, 1} // {red,green,blue} *= ( (1/255)*{clr.R,clr.G,clr.B} )
			};

			System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(colorMatrixElements);

			imageAttributes.SetColorMatrix(
			   colorMatrix,
			   System.Drawing.Imaging.ColorMatrixFlag.Default,
			   System.Drawing.Imaging.ColorAdjustType.Bitmap);


			Image rtn = new Bitmap(source.Width, source.Height);
			using Graphics e = Graphics.FromImage(rtn);
			//e.DrawImage(img, 0, 0);

			e.DrawImage(source,
			   new Rectangle(0, 0, width, height),  // destination rectangle 
			   0, 0,		// upper-left corner of source rectangle 
			   width,	   // width of source rectangle
			   height,	  // height of source rectangle
			   GraphicsUnit.Pixel, imageAttributes);
			return rtn;
		}
		/// <summary>
		/// Copies all pixels matching the specified color to equal places in a new bitmap.
		/// </summary>
		/// <param name="img"></param>
		/// <param name="c"></param>
		/// <param name="tolerence"></param>
		/// <returns></returns>
		public static Bitmap Bitmap_ScreenForColor(Bitmap img, Color c)
		{
			return Bitmap_ScreenForColor(img, c, 0);
		}
		/// <summary>
		/// Copies all pixels matching the specified color, within the specified tolerance, to equal places in a new bitmap.
		/// </summary>
		/// <param name="img"></param>
		/// <param name="c"></param>
		/// <param name="tolerence"></param>
		/// <returns></returns>
		public static Bitmap Bitmap_ScreenForColor(Bitmap img, Color c, int tolerence)
		{
			if (tolerence < 0 || tolerence > 100) throw new Exception("Tolerence should be between 0 and 100.");

			Bitmap rtn = new Bitmap(img.Width, img.Height);
			Graphics G = Graphics.FromImage(rtn);
			for (int y = 0; y < img.Height; y++)
				for (int x = 0; x < img.Width; x++)
					if (Colors_PercentDiff(c, img.GetPixel(x, y)) <= tolerence)
						G.DrawImage(img, new Rectangle(x, y, 1, 1), new Rectangle(x, y, 1, 1), GraphicsUnit.Pixel);
			
			G.Dispose();
			return rtn;
		}
		public static void Image_CopyPoint(Graphics g, Image source, Point p)
		{
			g.DrawImage(source, new Rectangle(p.X, p.Y, 1, 1), new Rectangle(p.X, p.Y, 1, 1), GraphicsUnit.Pixel);
		}
		public static void Image_CopyPoint(Graphics g, Image source, int x, int y)
		{
			Image_CopyPoint(g, source, new Point(x, y));
		}

		/// <summary>
		/// Acts like the wand-select tool in paint programs (using local/contiguous select).
		/// </summary>
		/// <param name="img"></param>
		/// <param name="start"></param>
		/// <param name="c"></param>
		/// <param name="tolerence"></param>
		/// <returns></returns>
		public static Image Bitmap_GetStructure(Bitmap img, Point start, Color c, int tolerence)
		{
			List<Point> visited = new List<Point>();

			recurse(start, start);

			Image rtn = new Bitmap(img.Width, img.Height);
			Graphics g = Graphics.FromImage(rtn);

			foreach (Point pt in visited)
				Image_CopyPoint(g, img, pt);

			g.Dispose();
			return rtn;

			//---- func defs ----

			// <recurse(...)>
			void recurse(Point start_, Point prev) {
				// <check(...)>
				void check(Point p) {
					if (p == prev) return;

					//if (!visited && !(out-of-bounds) && (==color)), log & follow.
					if (!visited.Contains(p) && Math.General.IsInRect(p, new Rectangle(0, 0, img.Width, img.Height)))
						if (Colors_PercentDiff(img.GetPixel(p.X, p.Y), c) <= tolerence) {
							visited.Add(p);
							recurse(p, start_);
						}
				} // </check(...)>

				/* 5 4 3
				 * 6 p 2
				 * 7 8 1 */
				for (int i = 1; i > -2; i -= 2)
					for (int j = -1; j < 3; j++)
						check(new Point(start.X + (j < 2 ? i : 0),
										start.Y + (j < 2 ? j * i : i)));
			} // </recurse(...)>.
		}
		public static Image StretchImage(Image source, Size size)
		{
			if (size.Width <= 0 || size.Height <= 0) throw new Exception("Dimensions must be greater than zero.");
			return StretchImage(source, size.Width, size.Height);
		}
		public static Image StretchImage(Image source, int width, int height)
		{
			if (width <= 0 || height <= 0) throw new Exception("Dimensions must be greater than zero.");
			Image rtn = new Bitmap(width, height);
			Graphics G = Graphics.FromImage(rtn);
			G.DrawImage(source, new Rectangle(0, 0, width, height), new Rectangle(0, 0, source.Width, source.Height), GraphicsUnit.Pixel);
			G.Dispose();
			return rtn;
		}
		public static Image ScaleImage(Image source, float scale)
		{
			if (scale <= 0) throw new Exception("Scale must be greater than zero.");
			return StretchImage(source, (int)(source.Width * scale), (int)(source.Height * scale));
		}
		public static Image ScaleImage(Image source, byte scale)
		{
			if (scale <= 0) throw new Exception("Scale must be greater than zero.");
			return StretchImage(source, source.Width * scale, source.Height * scale);
		}
		public static Image DrawRow(Objects.classFonts.GFont font, string[] headers, string[] values, bool[] rightAlign, string separator, ushort height)
		{
			if (headers.Length != values.Length || headers.Length != rightAlign.Length || values.Length != rightAlign.Length) throw new Exception("headers[], and values[], and rightAlign[] must be the same length!");
			if (height < font.CharHeight) throw new Exception("Height must be at least font's height!");
			Image rtn = new Bitmap(font.GetWidth(CompileRow(headers, separator)), height);
			Graphics G = Graphics.FromImage(rtn);
			ushort x = 0;
			for (byte i = 0; i < headers.Length; i++)
			{
				ushort w = (ushort)font.GetWidth(headers[i]);
				ushort w2 = (ushort)font.GetWidth(headers[i] + separator);
				Image text = (rightAlign[i] ? font.PrintRightCenter(values[i], w, height) : font.PrintCenterH(values[i], height));
				G.DrawImageUnscaled(text, x, 0);
				x += (ushort)(w + w2);
			}
			G.Dispose();
			return rtn;
		}
		private static string CompileRow(string[] values, string separator)
		{
			string rtn = "";
			bool first = true;
			foreach (string str in values)
			{
				if (first) rtn += separator; else first = false;
				rtn += str;
			}
			return rtn;
		}
		public static Image PrependRow(Image prepend, Image row, ushort padding)
		{
			bool preH = prepend.Height > row.Height;
			ushort height = (ushort)(preH ? row.Height : prepend.Height);
			ushort ofsY = (ushort)sMath.Abs( (prepend.Height - row.Height) / 2 );
			Image rtn = new Bitmap(prepend.Width + padding + row.Width, height);
			Graphics G = Graphics.FromImage(rtn);
			G.DrawImageUnscaled(prepend, 0, (preH ? 0 : ofsY));
			G.DrawImageUnscaled(row, prepend.Width + padding, (preH ? ofsY : 0));
			G.Dispose();
			return rtn;
		}
	} //END-ALL
}
