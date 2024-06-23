using QuodLib.Strings;

namespace QuodLib.Console {
    using Console = System.Console;

    /// <summary>
    /// Color tag format: &lt;f=__&gt;&lt;b=__&gt;,&lt;f=__,b=__&gt;,&lt;f,b=_,_&gt; | __=(d){b,g,c,r,m,y,w,G,B} | 
    /// &lt;&gt; for reset, &lt;p&gt; for pause.
    /// </summary>
    public partial class Output
	{
        public static void EraseLines(uint goUp) {
            for (uint i = 0; i < goUp && Console.CursorTop > 0; i++) {
                Console.CursorLeft = 0;
                for (byte j = 0; j < Console.BufferWidth - 1; j++)
                    Console.Write(" ");
                Console.CursorTop--;
            }
            Console.CursorLeft = 0;
            Console.Write("\b");
        }

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
    } //END-ALL
}
