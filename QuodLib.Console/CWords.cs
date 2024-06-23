using QuodLib.Strings;
namespace QuodLib.Console {
	using Console = System.Console;
    public struct CWords
	{
		List<string[]> Phrases; //<text, color>: don't reset BG before each color! (<setBG>phrase<setFG>phrase2	<-- <setBG> must be kept!)

		public CWords(string line)
		{
			bool coloring = false;
			List<string[]> myPhr = new List<string[]>();
			string[] texts = new string[2];
			//texts[0] and texts[1] are dumped at the end of every color declaraion.
			for (int i = 0; i < line.Length; i++) {
				char chr = line[i];
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
					Colors.Set(text[1], cBG, cFG);
				if (line.Length + text[0].Length > cnsW) {
					//bool overflow = false;
					if (text[0].Contains("\n") && !text[0].StartsWith("\n")) {
						throw new Exception("The phrase already contains a new-line character and that crap is complicated: " + text);
						//Just... have the constructor split new phrases at "\n" the same way it does at colors.

						#region discarded
						//for (int i = 0; i < text.SplitCount("\n"); i++)
						//{
						//	string phrs = text.SplitString("\n", (int)i);
						//	if (line.Length + phrs.Length > cnsW)
						//	{
						//		string text_ = (line + phrs).InsertBreaks(cnsW);
						//		text_ = text_.FromIndex(line.Length);
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
								for (int i = 1; i < breaks; i++)
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
			Colors.Set("<>", cBG, cFG);
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
}
