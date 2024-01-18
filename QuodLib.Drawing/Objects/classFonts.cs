using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using Resources = QuodLib.Drawing.Properties.Resources;
using QuodLib.Strings;

namespace QuodLib.Drawing.Objects
{
	/// <summary>
	/// Contains definitions for GFont objects, mostly.
	/// </summary>
	public static class classFonts
	{
		#region Objects
		/// <summary>
		/// An imaged imaged font (GFont) created from an information file and a character-image sheet.
		/// </summary>
		public class GFont
		{
			#region FieldsProperties
			public string Name {get; private set;}
			/// <summary>
			/// Displayed as a character when the printed text contains a character not found within the font's files.
			/// </summary>
			public Image ErrChar {get; private set;}
			/// <summary>
			/// Dictinary of character images created from the font's files.
			/// </summary>
			public Dictionary<char, Image> Characters {get; private set;}
			/// <summary>
			/// The standard character width defined by the font's information file.
			/// </summary>
			public int CharWidth {get; private set;}
			/// <summary>
			/// The standard character height defined by the font's information file.
			/// </summary>
			public int CharHeight {get; private set;}
			/// <summary>
			/// The greatest width of all contained numeric characters.
			/// </summary>
			public int NumericWidth {
				get {
					int rtn = Characters['0'].Width;
					for (byte i = 1; i < 10; i++)
					{
						int w = Characters[("" + i)[0]].Width;
						if (w > rtn) rtn = w;
					}
					return rtn;
				}
			}
			/// <summary>
			/// The predefined height, in pixels, of numbers 0-9.
			/// </summary>
			public int NumericHeight {get; private set;}
			/// <summary>
			/// The amount of vertical space, in pixels, between lines of text.
			/// </summary>
			public uint Padding {get; private set;}
			#endregion //FieldsProperties

			/// <summary>
			/// Creates a new font from the information file (specs) and the character-image sheet (charSheet).
			/// </summary>
			/// <param name="specs"></param>
			/// <param name="charSheet"></param>
			/// <param name="fontName"></param>
			public GFont(string specs, Image charSheet, string fontName)
			{
				Name = fontName;
				Characters = new Dictionary<char, Image>();
				Padding = 1;
				foreach (string line in specs.Replace("\r\n", "\n").GetLines())
				{
					if (line != "" && line != null)
					{
						if (line.StartsWith("Size="))
						{
							string sze_ = line.Split('=')[1];
							CharWidth = int.Parse(sze_.Split(',')[0]);
							CharHeight = int.Parse(sze_.Split(',')[1]);
						} else if (line.StartsWith("Padding=")) {
							Padding = uint.Parse(line.Split('=')[1]);
						} else if (line.StartsWith("NumHeight")) {
							NumericHeight = (int)uint.Parse(line.Split('=')[1]);
						} else {
							string name, sze;
							if (line.StartsWith("=="))
							{
								name = line.Replace("==", "_=");
								sze = name.Split('=')[1];
								name = "=";
							} else {
								name = line.Split('=')[0];
								sze = line.Split('=')[1];
							}

							int x = int.Parse(sze.Split(',')[0]);
							int y = int.Parse(sze.Split(',')[1]);
							int w = int.Parse(sze.Split(',')[2]);

							int locX = x * CharWidth;
							int locY = y * CharHeight;

							Image chrI = new Bitmap(w, CharHeight);
							Graphics C = Graphics.FromImage(chrI);
							C.DrawImage(charSheet, new Rectangle(0, 0, w, CharHeight), new Rectangle(locX, locY, w, CharHeight), GraphicsUnit.Pixel);
							C.Dispose();
							if (name == "<err>")
							{
								ErrChar = chrI;
							} else {
								if (name.Length > 1) throw new Exception("Character (except in the case of \"<err>\") must be a single character! (\'" + name + "\')");
								Characters.Add(name[0], chrI);
							}
						} // <(else: ValidCharacter)>
					} // </(if NotEmpty)>
				} // </loop>
				if (NumericHeight == 0) NumericHeight = CharHeight;
			} // </constructor>
			/// <summary>
			/// Returns the graphical width, in pixels, of the provided text.
			/// </summary>
			/// <param name="text"></param>
			/// <returns></returns>

			#region Functions
				#region Size
			public int GetWidth(string text)
			{
				return GetSize(text).Width;
			}
			/// <summary>
			/// Returns the graphical height, in pixels, of the provided text.
			/// </summary>
			/// <param name="text"></param>
			/// <returns></returns>
			public int GetHeight(string text)
			{
				return GetSize(text).Height;
			}
			/// <summary>
			/// Returns the graphical size, in pixels, of the provided text.
			/// </summary>
			/// <param name="text"></param>
			/// <returns></returns>
			public Size GetSize(string text)
			{
				int lineWidth = 0, width = 0, height = 0;
				string[] lines = text.GetLines();
				uint pad = (lines.Length > 1 ? Padding : 0);
				height = (lines.Length * (CharHeight + (int)pad)) - (int)pad; //if (multi-line), multiply with padding and then subtract last line's padding.
				for (int l = 0; l < lines.Length; l++) //Find greatest width.
				{
					string line = lines[l];
					lineWidth = 0;
					for (int c = 0; c < line.Length; c++) //if (line == ""), loop doesn't execute.
					{
						Image chrI = (Characters.ContainsKey(line[c]) ? Characters[line[c]] : ErrChar);
						lock (chrI) { lineWidth += chrI.Width; }
					}
					if (width < lineWidth) width = lineWidth; //update greatest width
				}
				return new Size(width, height);
			}
			/// <summary>
			/// Returns the graphical size, in pixels, of the provided colored text.
			/// </summary>
			/// <param name="cText"></param>
			/// <returns></returns>
			public Size GetSize(CString cText)
			{
				return GetSize(cText.Text);
			}
				#endregion //Size

				#region Print
			/// <summary>
			/// Creates a white graphical image from the provided text.
			/// </summary>
			/// <param name="text"></param>
			/// <returns></returns>
			public Image Print(string text)
			{
				return Print(text, Color.White);
			}
			/// <summary>
			/// Creates a graphical image from the provided text and color.
			/// </summary>
			/// <param name="text"></param>
			/// <param name="color"></param>
			/// <returns></returns>
			public Image Print(string text, Color color)
			{
				Size size = GetSize(text);
				if (size.Width == 0) return new Bitmap(1, size.Height);
				Image rtn = new Bitmap(size.Width, size.Height);
				Graphics I = Graphics.FromImage(rtn);

				string[] lines = text.GetLines();
				for (int l = 0; l < lines.Length; l++) //For (each line)
				{
					string line = lines[l];
					int linePos = 0;
					for (int c = 0; c < line.Length; c++) //if (line == ""), loop doesn't execute.
					{
						Image chrI = (Characters.ContainsKey(line[c]) ? Characters[line[c]] : ErrChar);
						lock (chrI)
						{
							I.DrawImage(classGraphics.Image_Tint(chrI, color), linePos, l * (CharHeight + Padding), chrI.Width, CharHeight);
							linePos += chrI.Width;
						}
					}
				}
			
				I.Dispose();
				return rtn;
			}
			/// <summary>
			/// Creates a graphical image from the provided colored text.
			/// </summary>
			/// <param name="text"></param>
			/// <returns></returns>
			public Image Print(CString text)
			{
				Size size = GetSize(text);
				if (size.Width == 0) return new Bitmap(1, size.Height);
				Image rtn = new Bitmap(size.Width, size.Height);
				Graphics I = Graphics.FromImage(rtn);

				CString[] lines = text.GetLines();
				for (int l = 0; l < lines.Length; l++) //For (each line)
				{
					string line = lines[l].Text;
					int linePos = 0;
					for (int c = 0; c < line.Length; c++) //if (line == ""), loop doesn't execute.
					{
						Image chrI = (Characters.ContainsKey(line[c]) ? Characters[line[c]] : ErrChar);
						lock (chrI)
						{
							I.DrawImage(classGraphics.Image_Tint(chrI, lines[l].GetColor(c)), linePos, l * (CharHeight + Padding), chrI.Width, CharHeight);
							linePos += chrI.Width;
						}
					}
				}
			
				I.Dispose();
				return rtn;
			}
			/// <summary>
			/// Creates a graphical image from the provided colored text, centered horizontally within the provided width.
			/// </summary>
			/// <param name="text"></param>
			/// <param name="width"></param>
			/// <returns></returns>
			public Image PrintCenterW(CString text, int width)
			{
				Image prt = Print(text);
				Image rtn = new Bitmap(width, prt.Height);
				Graphics G = Graphics.FromImage(rtn);
				G.DrawImageUnscaled(prt, (width - prt.Width) / 2, 0, prt.Width, prt.Height);
				G.Dispose();
				return rtn;
			}
			/// <summary>
			/// Creates a graphical image from the provided colored text, right-centered horizontally within the provided width.
			/// </summary>
			/// <param name="text"></param>
			/// <param name="width"></param>
			/// <returns></returns>
			public Image PrintRight(CString text, int width)
			{
				Image prt = Print(text);
				Image rtn = new Bitmap(width, prt.Height);
				Graphics G = Graphics.FromImage(rtn);
				G.DrawImageUnscaled(prt, width - prt.Width, 0, prt.Width, prt.Height);
				G.Dispose();
				return rtn;
			}
			/// <summary>
			/// Creates a graphical image from the provided colored text, right-centered horizontally within the provided width and centered vertically within the provied height.
			/// </summary>
			/// <param name="text"></param>
			/// <param name="width"></param>
			/// <param name="height"></param>
			/// <returns></returns>
			public Image PrintRightCenter(CString text, int width, int height)
			{
				Image right = PrintRight(text, width);
				Image rtn = new Bitmap(right.Width, height);
				Graphics G = Graphics.FromImage(rtn);
				G.DrawImage(right, 0, (height - right.Height) / 2);
				G.Dispose();
				return rtn;
			}
			/// <summary>
			/// Creates a graphical image from the provided colored text, centered vertically within the provided height.
			/// </summary>
			/// <param name="text"></param>
			/// <param name="height"></param>
			/// <returns></returns>
			public Image PrintCenterH(CString text, int height)
			{
				Image prt = Print(text);
				Image rtn = new Bitmap(prt.Width, height);
				Graphics G = Graphics.FromImage(rtn);
				G.DrawImageUnscaled(prt, 0, (height - prt.Height), prt.Width, prt.Height);
				G.Dispose();
				return rtn;
			}
			/// <summary>
			/// Creates a graphical image from the provided colored text, centered within the provided width and height.
			/// </summary>
			/// <param name="text"></param>
			/// <param name="width"></param>
			/// <param name="height"></param>
			/// <returns></returns>
			public Image PrintCenter(CString text, int width, int height)
			{
				Image prt = Print(text);
				Image rtn = new Bitmap(width, height);
				Graphics G = Graphics.FromImage(rtn);
				G.DrawImageUnscaled(prt, (width - prt.Width) / 2, (height - prt.Height) / 2, prt.Width, prt.Height);
				G.Dispose();
				return rtn;
			}
				#endregion //Print
			#endregion //Functions
			public override string ToString()
			{
				return Name;
			}
		} // </class>
		/// <summary>
		/// An object defining a string of colored text.
		/// </summary>
		public class CString
		{
			#region FieldsProperties
			private Dictionary<int[], Color> Mods;
			/// <summary>
			/// Returns the color of the specified string index, based on color tags in the object's text definition.
			/// </summary>
			/// <param name="index"></param>
			/// <returns></returns>
			public Color GetColor(int index)
			{
				foreach (int[] idx in Mods.Keys)
				{
					if (index >= idx[0] && index <= idx[1])
					{
						return Mods[idx];
					}
				}
				return DefaultColor;
			}
			/// <summary>
			/// The actual string of text, without color tags, contained within the object.
			/// </summary>
			public string Text {get; private set;}
			/// <summary>
			/// The actual string of text, with color tags, contained within the object.
			/// </summary>
			public string ModDefinition {get; private set;}
			/// <summary>
			/// The default text color.
			/// </summary>
			public Color DefaultColor {get; private set;}
			/// <summary>
			/// A dictionary of XTML-esque tags for color presets and the colors they represent.
			/// </summary>
			public Dictionary<string, Color> ColorPresets {get; private set;}
			#endregion //FieldsProperties

			#region Constructors
			private CString(string text) : this(text, Color.White, new Dictionary<string, Color>()) {}
			public CString(Color def) : this("", def, new Dictionary<string, Color>()) {}
			public CString(Color def, Dictionary<string, Color> presets) : this("", def, presets) {}
			public CString(string text, Color def) : this(text, def, new Dictionary<string, Color>()) {}
			public CString(string text, Color def, Dictionary<string, Color> presets)
			{
				ColorPresets = presets;
				Mods = new Dictionary<int[], Color>();
				ModDefinition = text;
				DefaultColor = def;

				string literal = "";
				bool definingColor = false;
				bool usingColor = false;
				string color = "";
				Color fin = Color.White;

				int[] span = new int[2];
				bool tagRight = false;
				bool moddedRight = false;
				bool moddedLeft = false;

				for (int i = 0; i < text.Length; i++)
				{
					int index = literal.Length - 1;
					if (definingColor)
					{
						//TODO: Allow << without modificatons, in the form of << and >> (if < or > directly after saved index? have second < or > cancel?)
						if (text[i] == '<')
						{
							if (tagRight) throw new Exception("Not currently defining a color! (near index " + i + ")");							
							if (text[i-1] == '<')
							{
								definingColor = false;
								literal += "<";
								if (!moddedLeft)
								{
									//span[0] += 1;
									moddedLeft = true;
								}
							} else {
								throw new Exception("Already defining a color! (near index " + i + ")");
							}
						} else if (text[i] == '>') {//End color.
							definingColor = false;
							if (color != "")
							{
									if ((presets == null ? false : ColorPresets.ContainsKey(color)))
									{
										fin = ColorPresets[color];
									} else {
										if (color.ReplaceNumeric('_').Replace("__", "_").Replace("__", "_") == "_,_,_")
										{
											fin = classGraphics.CColor(byte.Parse(color.Split(',')[0]), byte.Parse(color.Split(',')[1]), byte.Parse(color.Split(',')[2]));
										} else {
											throw new Exception("Color \"" + color + "\" not listed or not in \'___,___,___\' (red,green,blue) format: starting at index " + span[0]);
										}
								}
								usingColor = true;
								span = new int[2];
								span[0] = index;
								moddedRight = false;
							}
							color = "";
						} else {
							color += text[i]; //Progress color.
						}
					} else { //not defining a color
						if (text[i] == '>')
						{
							if (text[i-1] == '>') //consecutive >
							{
								if (tagRight)
								{
									literal += ">";
									tagRight = false;
									if (!moddedRight)
									{
										span[0] += 1;
										moddedRight = true;
									}
								} else {
									tagRight = true;
								}
							} else { //left-first >
								if (tagRight && text[i+1] != '>')
								{
									throw new Exception("Not currently defining a color! (near index " + i + ")");
								} else {
									if (tagRight) //text[i+1] != >
									{
										throw new Exception("Not currently defining a color! (near index " + (i+1) + ")");
									} else {
										if (text[i+1] == '>')
										{
											tagRight = true;
										} else {
											throw new Exception("Not currently defining a color! (near index " + (i+1) + ")");
										}
									}
								}
							}
						} else if (text[i] == '<') { //Begin color.
							if (tagRight && text[i-2] != '<') throw new Exception("Not currently defining a color! (near index " + (i+1) + ")");
							definingColor = true;
							if (usingColor)
							{
								if (moddedLeft) span[0] += 1;
								span[1] = index + (moddedLeft ? 1 : 0);
								Mods.Add(span, fin);
								usingColor = false;
								span = new int[2];
								moddedRight = false;
								moddedLeft = false;
							}
						} else {
							literal += text[i]; //Progress string.
						}
					}
				} // </loop>
				if (usingColor)
				{
					span[1] = text.Length - 1;
					Mods.Add(span, fin);
				}
				Text = literal;
			} // </constructor>
			#endregion //Constructors
				
			/// <summary>
			/// Splits the object's colored text into an array of colored strings based on locations of the new-line escape character.
			/// </summary>
			/// <returns></returns>
			public CString[] GetLines()
			{
				string[] lines = ModDefinition.GetLines();
				CString[] rtn = new CString[lines.Length];
				for (int i = 0; i < lines.Length; i++)
				{
					rtn[i] = new CString(lines[i], DefaultColor, ColorPresets);
				}
				return rtn;
			}
			/// <summary>
			/// Returns a copy of the object, using the provided new colored string.
			/// </summary>
			/// <param name="text"></param>
			/// <returns></returns>
			public CString Rewrite(string text)
			{
				return new CString(text, DefaultColor, ColorPresets);
			}
			public static implicit operator CString(string text)
			{
				return new CString(text);
			}
		}// </class>
		#endregion //Objects
		/// <summary>
		/// A static collection of imaged fonts (GFonts) custom made for this class library.
		/// </summary>
		public static class Fonts
		{
			private static GFont furDomus_6x9 = new GFont(Resources.Furdomus_6x9_df, Resources.Furdomus_6x9_ch, "Furdomus 6x9");
			/// <summary>
			/// A 6x9-pixel imaged font (GFont) custom made for this class library.
			/// </summary>
			public static GFont FurDomus_6x9 {get { return furDomus_6x9; }}

			private static GFont furDomus_8x12 = new GFont(Resources.Furdomus_8x12_df, Resources.Furdomus_8x12_ch, "Furdomus 8x12");
			/// <summary>
			/// An 8x12-pixel imaged font (GFont) custom made for this class library.
			/// </summary>
			public static GFont FurDomus_8x12 {get { return furDomus_8x12; }}

			private static GFont furDomus_12x18_bold = new GFont(Resources.Furdomus_12x18_bold_df, Resources.Furdomus_12x18_bold_ch, "Furdomus 12x18 Bold");
			/// <summary>
			/// A 12x18-pixel bold imaged font (GFont) custom made for this class library.
			/// </summary>
			public static GFont FurDomus_12x18_bold {get { return furDomus_12x18_bold; }}
		}
	}
}
