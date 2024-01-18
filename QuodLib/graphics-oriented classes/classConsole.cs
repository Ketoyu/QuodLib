using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuodLib
{
	using Strings;
	using static Strings.Divide;
	/// <summary>
	/// Color tag format: &lt;f=__&gt;&lt;b=__&gt;,&lt;f=__,b=__&gt;,&lt;f,b=_,_&gt; | __=(d){b,g,c,r,m,y,w,G,B} | 
	/// &lt;&gt; for reset, &lt;p&gt; for pause.
	/// </summary>
	public class classConsole
	{
		private static uint CLine_GetRealLength(string line)
		{
			return (uint)CLine_GetRealText(line)[0].Length;
		}
		private static uint CLine_GetColorLength(string line)
		{
			return (uint)CLine_GetRealText(line)[1].Length;
		}
		private static uint[] CLine_GetLengths_FromIndex(string line, uint index)
		{
			uint[] length = new uint[2];
			string line_ = "";
			for (uint i = index; i < line.Length; i++)
				line_ += line[(int)i];
			string[] texts = CLine_GetRealText(line_);
			length[1] = (uint)texts[1].Length;
			length[0] = (uint)texts[0].Length;
			return length;
		}
		private static string[] CLine_GetRealText(string line)
		{
			string[] rtn = new string[2];
			bool coloring = false;
			for (uint i = 0; i < line.Length; i++) {
				char chr = line[(int)i];
				switch (chr)
				{
					case '<':
						if (coloring)
							throw new Exception("Unexpected < in input string: Cannot open a new color when a color is already open.\n@ " + line);
						else {
							coloring = true;
							rtn[1] += chr;
						}
						break;
					case '>':
						if (coloring) {
							rtn[1] += chr;
							coloring = false;
						} else
							throw new Exception("Unexpected > in input string: A color was closed which never opened to begin with.\n@ " + line);
						break;
					default:
						if (coloring) rtn[1] += chr;
							else rtn[0] += chr;
						break;
				}
			}
			if (coloring)
				throw new Exception("> expected: A color which opened was never closed.\n@ " + line);
			else
				return rtn;
		}
		struct CWords
		{
			List<string[]> Phrases; //<text, color>: don't reset BG before each color! (<setBG>phrase<setFG>phrase2	<-- <setBG> must be kept!)

			public CWords(string line)
			{
				bool coloring = false;
				List<string[]> myPhr = new List<string[]>();
				string[] texts = new string[2];
				//texts[0] and texts[1] are dumped at the end of every color declaraion.
				for (uint i = 0; i < line.Length; i++) {
					char chr = line[(int)i];
					switch (chr)
					{
						case '\n':
							if (coloring)
								throw new Exception("Cannot break a line inside a color command!");
							else {
								if (!string.IsNullOrEmpty(texts[0] + texts[1]))
									myPhr.Add(new string[] { texts[0], texts[1] });
								texts[1] = "";
								texts[0] = "\n";
							}
							break;
						case '<':
							if (coloring)
								throw new Exception("Unexpected < in input string: Cannot open a new color when a color is already open.\n@ " + line);
							else {
								coloring = true;
								if (string.IsNullOrEmpty(texts[1])) {
									if (!string.IsNullOrEmpty(texts[0]))
									{ //there is text, but no color
										myPhr.Add(new string[] { texts[0], "" });
									}
								} else {
									if (string.IsNullOrEmpty(texts[0]))
									{ //there is color, but no text
										myPhr.Add(new string[] { "", texts[1] });
									} else { //there is color and text
										myPhr.Add(new string[] {texts[0], texts[1]});
									}
								}
								texts[1] = "";
								texts[0] = "";
								texts[1] += chr;
							}
							break;
						case '>':
							if (coloring) {
								texts[1] += chr;
								coloring = false;
							} else
								throw new Exception("Unexpected > in input string: A color was closed which never opened to begin with.\n@ " + line);
							break;
						default:
							if (coloring) texts[1] += chr;
								else texts[0] += chr;
							break;
					}
				}
				if (!string.IsNullOrEmpty(texts[0])) {
					if (string.IsNullOrEmpty(texts[1]))
					{ //there was a phrase after the last color
						myPhr.Add(new string[] { texts[0], "" });
					} else {
						myPhr.Add(new string[] { texts[0], texts[1] });
					}
				} else {
					if (!string.IsNullOrEmpty(texts[1]))
						myPhr.Add(new string[] { "", texts[1] });
				}
				if (coloring) throw new Exception("> expected: A color which opened was never closed.\n@ " + line);
					else Phrases = myPhr;
			}
			public void Print()
			{
				Print(0, false);
			}
			public void Print(byte indent, bool hanging)
			{
				ConsoleColor cBG = Console.BackgroundColor;
				ConsoleColor cFG = Console.ForegroundColor;
				string line = "";
				bool SimpWriting = false;
				foreach (string[] text in Phrases) {
					int cnsW = Console.BufferWidth - 1;
					//bool overflow = false;
					if (!string.IsNullOrEmpty(text[1]))
						setColor(text[1], cBG, cFG);
					if (line.Length + text[0].Length > cnsW) {
						//bool overflow = false;
						if (text[0].Contains("\n") && !text[0].StartsWith("\n")) {
							throw new Exception("The phrase already contains a new-line character and that crap is complicated: " + text);
							//Just... have the constructor split new phrases at "\n" the same way it does at colors.

							#region discarded
							//for (uint i = 0; i < text.SplitCount("\n"); i++)
							//{
							//	string phrs = text.SplitString("\n", (int)i);
							//	if (line.Length + phrs.Length > cnsW)
							//	{
							//		string text_ = (line + phrs).InsertBreaks(cnsW);
							//		text_ = text_.FromIndex((uint)line.Length);
							//		Console.Write(line);
							//		Console.Write(text_);
							//		line = text_.SplitString("\n", -1);
							//		//overflow = true; //double-check
							//	} else {
							//		Console.Write(line);
							//		line = phrs;
							//	}
							//}
							#endregion //discarded
						} else {
							if ( text[0].StartsWith("\n") || line.EndsWith("\n") )
								line = "";
							//sstring text_ = line + text[0];
							string text_ = line + text[0];
							text_ = text_.InsertBreaks(cnsW, indent, hanging, (line == ""));
							//text_ = text_.InsertBreaks(cnsW, indent, hanging, (line == ""));
							//text_ = text_InsertBreaks(cnsW, indent, hanging, (line == ""));
							if (text_.Contains("\n")) {
								text_ = text_.FromIndex(line.Length);
								int breaks = text_.SplitCount("\n");
								//if (text_.StartsWith("\n"))
								//{
									line = "";
								//}
								if (SimpWriting)
									SimpWriting = false;
								else {
									//Console.Write(line);
								}
								Console.WriteLine(text_.Split("\n", 0));
								if (breaks >= 1) {
									for (uint i = 1; i < breaks; i++)
										Console.WriteLine(text_.Split('\n')[i]);
									string temp = text_.Split('\n')[breaks]; //    <----   DEBUGGING LINE
									Console.Write(text_.Split('\n')[breaks]);
								}
								line = text_.Split('\n')[breaks];
							} else {
								Console.WriteLine(text_);
								line = text_;
							}
						}
						//line = text;
					} else {
						//if (!text[1].IsEmpty())
						//{
						//	setColor(text[1], cBG, cFG);
						//}
						SimpWriting = true;
						if (indent > 0 && !hanging && line == "") {
							string id = "";
							for (byte id_ = 0; id_ < indent; id_++)
								id += " ";
							Console.Write(id + text[0]);
							line += id + text[0];
						} else {
							Console.Write(text[0]);
							line += text[0];
						}
					}
					//if (!overflow) { Console.WriteLine(text); } //double-check
					//Console.Write(line);
				}
				setColor("<>", cBG, cFG);
			}
			public void PrintLine()
			{
				PrintLine(0, false);
			}
			public void PrintLine(byte indent, bool hanging)
			{
				Print(indent, hanging);
				Console.WriteLine("");
			}
		}
		/// <summary>
		/// Color tag format: &lt;f=__&gt;&lt;b=__&gt;,&lt;f=__,b=__&gt;,&lt;f,b=_,_&gt; | __=(d){b,g,c,r,m,y,w,G,B} | 
		/// &lt;&gt; for reset, &lt;p&gt; for pause.
		/// </summary>
		public static void CWrite(string line, byte indent)
		{
			CWrite(line, indent, false);
		}
		/// <summary>
		/// Color tag format: &lt;f=__&gt;&lt;b=__&gt;,&lt;f=__,b=__&gt;,&lt;f,b=_,_&gt; | __=(d){b,g,c,r,m,y,w,G,B} | 
		/// &lt;&gt; for reset, &lt;p&gt; for pause.
		/// </summary>
		public static void CWrite(string line, byte indent, bool hanging)
		{
			new CWords(line).Print(indent, hanging);
		}
		/// <summary>
		/// Color tag format: &lt;f=__&gt;&lt;b=__&gt;,&lt;f=__,b=__&gt;,&lt;f,b=_,_&gt; | __=(d){b,g,c,r,m,y,w,G,B} | 
		/// &lt;&gt; for reset, &lt;p&gt; for pause.
		/// </summary>
		public static void CWriteL(string line, byte indent)
		{
			CWriteL(line, indent, false);
		}
		/// <summary>
		/// Color tag format: &lt;f=__&gt;&lt;b=__&gt;,&lt;f=__,b=__&gt;,&lt;f,b=_,_&gt; | __=(d){b,g,c,r,m,y,w,G,B} | 
		/// &lt;&gt; for reset, &lt;p&gt; for pause.
		/// </summary>
		public static void CWriteL(string line, byte indent, bool hanging)
		{
			(new CWords(line)).PrintLine(indent, hanging);
		}
		/// <summary>
		/// Color tag format: &lt;f=__&gt;&lt;b=__&gt;,&lt;f=__,b=__&gt;,&lt;f,b=_,_&gt; | __=(d){b,g,c,r,m,y,w,G,B} | 
		/// &lt;&gt; for reset, &lt;p&gt; for pause.
		/// </summary>
		public static void CWrite(string line)
		{
			CWrite(line, 0, false);
		}
		/// <summary>
		/// Color tag format: &lt;f=__&gt;&lt;b=__&gt;,&lt;f=__,b=__&gt;,&lt;f,b=_,_&gt; | __=(d){b,g,c,r,m,y,w,G,B} | 
		/// &lt;&gt; for reset, &lt;p&gt; for pause.
		/// </summary>
		public static void CWriteL(string line)
		{
			CWriteL(line, 0, false);
		}
		#region Colors
		public enum CColor {
            Blue = 'b',
			Green = 'g',
			Cyan = 'c',
			Red = 'r',
			Magenta = 'm',
			Yellow = 'y',
			White = 'w',
			Gray = 'G',
            Black = 'B'
        }
		private static void setColor(string color, ConsoleColor cBG, ConsoleColor cFG)
		{
			string color_ = color;
			if (	(color_.Length == 6 || color_.Length == 5 || color_.Length == 2 || (color_.Length == 3 && color_.Contains("p")) || ((color.Length == 7 || color.Length == 8
						|| color_.Length == 9 || color_.Length == 10 || color.Length == 11) && color_.Contains(",")))
					&& (color_.Contains('<') && color_.Contains('>'))
					&& (color.Length == 2 || color_.Contains("=") || (color_.Length == 3 && color_.Contains("p")) ) ) {
				if (color_.Contains(",")) {
					//allow <b,f=_,_> and <f,b=_,_> (just reconstruct it into <b=__><f=__> or <f=__><b=__>and use recursion)
					color_ = color_.GetBetween("<", ">");
					string[] parts = color_.Split('=');
					if (parts[0].Contains(",")) {
						string[] grnd = parts[0].Split(',');
						if (parts[1].Contains(",")) {
							string[] clrs = parts[1].Split(',');
							setColor("<" + grnd[0] + "=" + clrs[0] + ">", cBG, cFG);
							setColor("<" + grnd[1] + "=" + clrs[1] + ">", cBG, cFG);
						} else {
							//f and b will be the same color (let the user decide whether to fix that)
							setColor("<" + grnd[0] + "=" + parts[1] + ">", cBG, cFG);
							setColor("<" + grnd[1] + "=" + parts[1] + ">", cBG, cFG);
						}
					} else throw new Exception("Color format does not match <p>, <_=_>, <_=__>, <_,_=_,_>, <_,_=__,_>, <_,_=_,__>, <_,_=__,__>, <_,_=_>, <_,_=__>, or <> (one -ground cannot have two colors <f=_,_>, <b=_,_>) :" + color);
				} else if (color_.Contains("p")) {
					if (color_ == "<p>") PauseKey();
						else throw new Exception("Color format does not match <p>, <_=_>, <_=__>, <_,_=_,_>, <_,_=__,_>, <_,_=_,__>, <_,_=__,__>, <_,_=_>, <_,_=__>, or <> (one -ground cannot have two colors <f=_,_>, <b=_,_>) :" + color);
				} else {
					if (color_ == "<>") {
						Console.BackgroundColor = cBG;
						Console.ForegroundColor = cFG;
					} else {
						color_ = color_.GetBetween("<", ">");
						bool Background = (color_[0] == 'b');
						bool Dark = false;
						color_ = color_.GetAfter("=");
						char clr;
						if (color_.Length == 2) {
							if (color_.Contains("d")) {
								Dark = true;
								byte idxD = (byte)color_.IndexOf('d');
								if (idxD == 0) clr = color_[1];
									else clr = color_[0];
								if ( (clr == 'w') || (clr == 'W') )
									throw new Exception("Color White cannot be dark! @ " + color);
								else if (clr == 'B')
									throw new Exception("Color Black cannot be dark! @ " + color);
							} else
								throw new Exception("Unrecognized color character pair " + color_ + " in " + color + ".");
						} else
							clr = color_[0];
						switch (clr)
						{
							case 'b': //blue
								setColor_ConsoleColor("Blue", Dark, Background);
								break;
							case 'g': //green
								setColor_ConsoleColor("Green", Dark, Background);
								break;
							case 'c': //cyan
								setColor_ConsoleColor("Cyan", Dark, Background);
								break;
							case 'r': //red
								setColor_ConsoleColor("Red", Dark, Background);
								break;
							case 'm': //magenta
								setColor_ConsoleColor("Magenta", Dark, Background);
								break;
							case 'y': //yellow
								setColor_ConsoleColor("Yellow", Dark, Background);
								break;
							case 'w': //white
								setColor_ConsoleColor("White", Dark, Background);
								break;
							case 'G': //Gray
								setColor_ConsoleColor("Gray", Dark, Background);
								break;
							case 'B': //Black
								setColor_ConsoleColor("Black", Dark, Background);
								break;
							default:
								throw new System.Exception("Invalid color character " + clr + " in " + color + ". Char must be lower-case except in the case of G, g, B, or b, where G is Gray, g is green, B is Black, and b is blue.");
						}
					}
				}
			} else
				throw new Exception("Color format does not match <p>, <_=_>, <_=__>, <_,_=_,_>, <_,_=__,_>, <_,_=_,__>, <_,_=__,__>, <_,_=_>, <_,_=__>, or <> :" + color);
		}
		public static void setColor(string color)
		{
			setColor(color, Console.BackgroundColor, Console.ForegroundColor);
		}
		private static void setColor_ConsoleColor(string color, bool dark, bool background)
		{
			ConsoleColor final;
			if (dark) final = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), "Dark" + color);
				else final = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color);
			if (background) Console.BackgroundColor = final;
				else Console.ForegroundColor = final;
		}
		public static string GetColor_BG()
		{
			return ConsoleColor_ToString(Console.BackgroundColor, true);
		}
		public static string GetColor_FG()
		{
			return ConsoleColor_ToString(Console.ForegroundColor, false);
		}
		public static string GetColor()
		{
			string BG = GetColor_BG();
			string FG = GetColor_FG();
			List<string> clr = new List<string> { BG.GetBetween("=", ">"), FG.GetBetween("=", ">") };
			return "<b,f=" + clr[0] + "," + clr[1] + ">"; //<b,f=cc,cc>
		}
		private static string ConsoleColor_ToString(ConsoleColor clr, bool BG)
		{
			string sClr;
			if (BG) sClr = Console.BackgroundColor.ToString().ToLower();
				else sClr = Console.ForegroundColor.ToString().ToLower();
			string _G = "b";
			string dark = "";
			if (sClr.StartsWith("dark")) {
				dark = "d";
				sClr = sClr.Replace("dark", "");
			}
			if (!BG) _G = "f";
			switch (sClr.ToLower())
			{
				case "blue": //blue
					sClr = "b";
					break;
				case "green": //green
					sClr = "g";
					break;
				case "cyan": //cyan
					sClr = "c";
					break;
				case "red": //red
					sClr = "r";
					break;
				case "magenta": //magenta
					sClr = "m";
					break;
				case "yellow": //yellow
					sClr = "y";
					break;
				case "white": //white
					sClr = "w";
					break;
				case "gray": //Gray
					sClr = "G";
					break;
				case "black": //Black
					sClr = "B";
					break;
				default:
					throw new System.Exception("Invalid color string " + sClr + " in " + clr.ToString() + ".");
			}
			return "<" + _G + "=" + dark + sClr + ">";
		}
		#endregion //Colors
		public static void EraseLines(uint goUp)
		{
			for (uint i = 0; i < goUp && Console.CursorTop > 0; i++) {
				Console.CursorLeft = 0;
				for (byte j = 0; j < Console.BufferWidth - 1; j++)
					Console.Write(" ");
				Console.CursorTop--;
			}
			Console.CursorLeft = 0;
			Console.Write("\b");
		}
		public static void PauseKey()
		{
			int[] cPos = new int[] { Console.CursorLeft, Console.CursorTop };
			string In = Console.ReadKey().KeyChar.ToString();
			if (In == "\r") {
				Console.CursorTop = cPos[1];
				Console.CursorLeft = cPos[0];
			} else {
			   if (In.Length == 1) {
					if (Console.CursorLeft > 0)
						Console.CursorLeft -= 1;
					Console.Write(" ");
					Console.Write("\b");
				} else
					throw new Exception("Unplanned KeyChar occurred!");
			}
		}
		#region inputDATATYPE
			/// <summary>
			/// Returns the input and whether it was in the list of alternates.
			/// </summary>
			/// <param name="input"></param>
			/// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
			/// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
			/// <returns></returns>
			private static (bool, string) input_parseAlt(string input, List<string> alternates, bool caseSensitive = true)
			{
				if (caseSensitive)
					return ((alternates.Contains(input, false)), input.ToUpper());
				else
					return ((alternates.Contains(input)), input);
			}

			/// <summary>
			/// Restricts console user-input to a float (default -3.4E-38 to 3.4E+38, inclusive) or a List&lt;string&gt; of alternative inputs (default case-sensitive).
			/// </summary>
			/// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
			/// <param name="bounds">Minimum/Maximum of numeric input (default -3.4E-38 to 3.4E+38, inclusive)</param>
			/// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
			/// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
			/// <returns>String user-input</returns>
			public static string InputFloat(string consoleColor = "<>", (float min, float max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true)
			{
				return GetInput((inp) => {
					if (alternates != null) {
						(bool s, string v) rtn = input_parseAlt(inp, alternates, caseSensitive);
						if (rtn.s) return rtn;
					}

					bool success = float.TryParse(inp, out float value);

					if (bounds != null)
						success &= (value >= bounds?.min && value <= bounds?.max);

					return (success, "" + value);
				}, consoleColor);
			}

			/// <summary>
			/// Restricts console user-input to a double, or alternative inputs if provided.
			/// </summary>
			/// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
			/// <param name="bounds">Minimum/Maximum of numeric input (default -4.9E-324 to 1.7E+308, inclusive)</param>
			/// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
			/// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
			/// <returns>String user-input</returns>
			public static string InputDouble(string consoleColor = "<>", (double min, double max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true)
			{
				return GetInput((inp) => {
					if (alternates != null) {
						(bool s, string v) rtn = input_parseAlt(inp, alternates, caseSensitive);
						if (rtn.s) return rtn;
					}

					bool success = double.TryParse(inp, out double value);

					if (bounds != null)
						success &= (value >= bounds?.min && value <= bounds?.max);

					return (success, "" + value);
				}, consoleColor);
			}

			/// <summary>
			/// Restricts console user-input to a decimal, or alternative inputs if provided.
			/// </summary>
			/// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
			/// <param name="bounds">Minimum/Maximum of numeric input (default -7.9E+28 to 7.9E+28, inclusive)</param>
			/// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
			/// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
			/// <returns>String user-input</returns>
			public static string InputDecimal(string consoleColor = "<>", (decimal min, decimal max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true)
			{
				return GetInput((inp) => {
					if (alternates != null) {
						(bool s, string v) rtn = input_parseAlt(inp, alternates, caseSensitive);
						if (rtn.s) return rtn;
					}

					bool success = decimal.TryParse(inp, out decimal value);

					if (bounds != null)
						success &= (value >= bounds?.min && value <= bounds?.max);

					return (success, "" + value);
				}, consoleColor);
			}

			/// <summary>
			/// Restricts console user-input to an long-integer, or alternative inputs if provided.
			/// </summary>
			/// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
			/// <param name="bounds">Minimum/Maximum of numeric input (default -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807, inclusive)</param>
			/// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
			/// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
			/// <returns>String user-input</returns>
			public static string InputLong(string consoleColor = "<>", (long min, long max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true)
			{
				return GetInput((inp) => {
					if (alternates != null) {
						(bool s, string v) rtn = input_parseAlt(inp, alternates, caseSensitive);
						if (rtn.s) return rtn;
					}

					bool success = long.TryParse(inp, out long value);

					if (bounds != null)
						success &= (value >= bounds?.min && value <= bounds?.max);

					return (success, "" + value);
				}, consoleColor);
			}

			/// <summary>
			/// Restricts console user-input to an unsigned long-integer, or alternative inputs if provided.
			/// </summary>
			/// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
			/// <param name="bounds">Minimum/Maximum of numeric input (default 0 to 18,446,744,073,709,551,616, inclusive, inclusive)</param>
			/// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
			/// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
			/// <returns>String user-input</returns>
			public static string InputULong(string consoleColor = "<>", (ulong min, ulong max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true)
			{
				return GetInput((inp) => {
					if (alternates != null) {
						(bool s, string v) rtn = input_parseAlt(inp, alternates, caseSensitive);
						if (rtn.s) return rtn;
					}

					bool success = ulong.TryParse(inp, out ulong value);

					if (bounds != null)
						success &= (value >= bounds?.min && value <= bounds?.max);

					return (success, "" + value);
				}, consoleColor);
			}

			/// <summary>
			/// Restricts console user-input to an integer, or alternative inputs if provided.
			/// </summary>
			/// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
			/// <param name="bounds">Minimum/Maximum of numeric input (default -2,147,483,648 to 2,147,483,647, inclusive)</param>
			/// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
			/// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
			/// <returns>String user-input</returns>
			public static string InputInt(string consoleColor = "<>", (int min, int max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true)
			{
				return GetInput((inp) => {
					if (alternates != null) {
						(bool s, string v) rtn = input_parseAlt(inp, alternates, caseSensitive);
						if (rtn.s) return rtn;
					}

					bool success = int.TryParse(inp, out int value);

					if (bounds != null)
						success &= (value >= bounds?.min && value <= bounds?.max);

					return (success, "" + value);
				}, consoleColor);
			}

			/// <summary>
			/// Restricts console user-input to an unsigned uinteger, or alternative inputs if provided.
			/// </summary>
			/// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
			/// <param name="bounds">Minimum/Maximum of numeric input (default 0 to 4,294,967,295, inclusive, inclusive)</param>
			/// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
			/// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
			/// <returns>String user-input</returns>
			public static string InputUInt(string consoleColor = "<>", (uint min, uint max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true)
			{
				return GetInput((inp) => {
					if (alternates != null) {
						(bool s, string v) rtn = input_parseAlt(inp, alternates, caseSensitive);
						if (rtn.s) return rtn;
					}

					bool success = uint.TryParse(inp, out uint value);

					if (bounds != null)
						success &= (value >= bounds?.min && value <= bounds?.max);

					return (success, "" + value);
				}, consoleColor);
			}

			/// <summary>
			/// Restricts console user-input to a short, or alternative inputs if provided.
			/// </summary>
			/// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
			/// <param name="bounds">Minimum/Maximum of numeric input (default -32,768 to 32,767, inclusive)</param>
			/// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
			/// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
			/// <returns>String user-input</returns>
			public static string InputShort(string consoleColor = "<>", (short min, short max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true)
			{
				return GetInput((inp) => {
					if (alternates != null) {
						(bool s, string v) rtn = input_parseAlt(inp, alternates, caseSensitive);
						if (rtn.s) return rtn;
					}

					bool success = short.TryParse(inp, out short value);

					if (bounds != null)
						success &= (value >= bounds?.min && value <= bounds?.max);

					return (success, "" + value);
				}, consoleColor);
			}

			/// <summary>
			/// Restricts console user-input to an unsigned short, or alternative inputs if provided.
			/// </summary>
			/// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
			/// <param name="bounds">Minimum/Maximum of numeric input (default 0 to 65,535, inclusive)</param>
			/// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
			/// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
			/// <returns>String user-input</returns>
			public static string InputUShort(string consoleColor = "<>", (ushort min, ushort max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true)
			{
				return GetInput((inp) => {
					if (alternates != null) {
						(bool s, string v) rtn = input_parseAlt(inp, alternates, caseSensitive);
						if (rtn.s) return rtn;
					}

					bool success = ushort.TryParse(inp, out ushort value);

					if (bounds != null)
						success &= (value >= bounds?.min && value <= bounds?.max);

					return (success, "" + value);
				}, consoleColor);
			}

			/// <summary>
			/// Restricts console user-input to a signed byte, or alternative inputs if provided.
			/// </summary>
			/// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
			/// <param name="bounds">Minimum/Maximum of numeric input (default -128 to 127, inclusive)</param>
			/// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
			/// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
			/// <returns>String user-input</returns>
			public static string InputSByte(string consoleColor = "<>", (sbyte min, sbyte max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true)
			{
				return GetInput((inp) => {
					if (alternates != null) {
						(bool s, string v) rtn = input_parseAlt(inp, alternates, caseSensitive);
						if (rtn.s) return rtn;
					}

					bool success = sbyte.TryParse(inp, out sbyte value);

					if (bounds != null)
						success &= (value >= bounds?.min && value <= bounds?.max);

					return (success, "" + value);
				}, consoleColor);
			}

			/// <summary>
			/// Restricts console user-input to a byte, or alternative inputs if provided.
			/// </summary>
			/// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
			/// <param name="bounds">Minimum/Maximum of numeric input (default 0 to 255, inclusive)</param>
			/// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
			/// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
			/// <returns>String user-input</returns>
			public static string InputByte(string consoleColor = "<>", (byte min, byte max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true)
			{
				return GetInput((inp) => {
					if (alternates != null) {
						(bool s, string v) rtn = input_parseAlt(inp, alternates, caseSensitive);
						if (rtn.s) return rtn;
					}

					bool success = byte.TryParse(inp, out byte value);

					if (bounds != null)
						success &= (value >= bounds?.min && value <= bounds?.max);

					return (success, "" + value);
				}, consoleColor);
			}

		public static T GetInput<T>(Func<string, (bool Success, T Value)> parse, string consoleColor = "<>")
        {
			(bool Success, T Value) rtn = (false, default(T));
			while (!rtn.Success) {
				string inp = GetInput(consoleColor);
				rtn = parse(inp);

				if (!rtn.Success) {
					Console.CursorTop--;
					for (byte i = 1; i <= inp.Length; i++)
						Console.Write(" ");
					for (byte i = 1; i <= inp.Length; i++)
						Console.Write("\b");
				}
			}
			
			return rtn.Value;
		}
		#endregion //inputDATATYPE

		public static string GetInput(string cnsClr1 = "<>", string cnsClr2 = "<>")
		{
			//Normal
			if (cnsClr1 == "<>" && cnsClr2 == "<>")
				return Console.ReadLine();

			// (else), custom.
			ConsoleColor cBG = Console.BackgroundColor;
			ConsoleColor cFG = Console.ForegroundColor;
			classConsole.setColor(cnsClr1);
			classConsole.setColor(cnsClr2);
			string rtn = Console.ReadLine();
			Console.BackgroundColor = cBG;
			Console.ForegroundColor = cFG;
			return rtn;
		}
	} //END-ALL
}
