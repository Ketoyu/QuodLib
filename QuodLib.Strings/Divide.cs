namespace QuodLib.Strings
{
	/// <summary>
	/// A class of methods for picking apart strings.
	/// </summary>
	public static class Divide
	{
		/// <summary>
		/// Like [string].Split(char c).Count(), but accepts multi-character delimiters.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="chr"></param>
		/// <returns></returns>
		public static int SplitCount(this string Input, string chr)
		{
			if (!Input.Contains(chr)) throw new Exception("Term \"" + chr + "\" not contained within [Input] string.");

			string temp = Input;
			int count = 0;
			do {
				temp = temp.GetAfter(chr);
				count++;
			} while (temp.Contains(chr));

			return count;
		}
		/// <summary>
		/// Like [string].Split(char c), but accepts multi-character delimiters.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="chr"></param>
		/// <returns></returns>
		public static List<string> Split(this string Input, string chr)
		{
			if (!Input.Contains(chr)) throw new Exception("Term \"" + chr + "\" not contained within [Input] string.");

			string temp = Input;
			List<string> rtn = new List<string>();

			do {
				rtn.Add(temp.GetBefore(chr));
				temp = temp.GetAfter(chr);
			} while (temp.Contains(chr));

			rtn.Add(temp);

			return rtn;
		}

		/*
		/// <summary>
		/// Like [string].Split(char c)[int index], but accepts multi-character delimiters and the index -1 jumps to the last one.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="chr"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static string Split(this string Input, string chr, int index)
		{
			if (!Input.Contains(chr)) throw new Exception("Term \"" + chr + "\" not contained within [Input] string.");

			if (index == 0) return Input.GetBefore(chr);
			int idx = 0;
			string rtn;
			if (index > 0) {
				try {
					idx = Input.IndexOf_Nth(chr, index);
				} catch (Exception ex) {
					if (int.Parse(ex.ToString().GetBefore(" occurrances").GetAfter("found ")) == index) {
						idx = Input.LastIndexOf(chr);
						return Input.FromIndex(idx + chr.Length, true);
					}
				}
				rtn = Input.TowardIndex(idx, false);
				if (rtn.Contains(chr)) {
					idx = rtn.LastIndexOf(chr);
					return rtn.FromIndex(idx + chr.Length, true);
				}
				return rtn;
			}
			//implicit else
			idx = Input.IndexOf_Nth(chr, index);
			rtn = Input.FromIndex(idx + chr.Length, true);
			if (rtn.Contains(chr)) return rtn.GetBefore(chr);
			return rtn;
		}
		*/

		#region You're welcome. ~Rob
		// public static string SplitString(string mainString, string split, bool before)
		// {
		//	 string output = "";
		//	 if (before)
		//	 {
		//		 output = mainString.Substring(0, mainString.IndexOf(split));
		//	 }
		//	 else
		//	 {
		//		 output = mainString.Substring(mainString.IndexOf(split));
		//	 }

		//		 return output;
		//}
		#endregion

		/// <summary>
		/// Breaks one <paramref name="Input"/> string into two strings, on a specified <paramref name="index"/>.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="index"></param>
		/// <param name="head"></param>
		/// <param name="tail"></param>
		/// <param name="headIncludesIndex"></param>
		public static void Break(this string Input, int index, out string head, out string tail, bool headIncludesIndex)
		{
			head = Input.TowardIndex(index, headIncludesIndex);
			tail = Input.FromIndex(index, !headIncludesIndex);
		}

		/// <summary>
		/// Cuts off a string before a given character.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="chr"></param>
		/// <returns></returns>
		public static string GetBefore(this string Input, char chr)
		{
			if (Input.Contains(chr))
				return GetBefore_master(Input, Input.IndexOf(chr));
			//implicit else
			throw new Exception("Given string does not contain the search term \"" + chr + "\".\n\nString: " + Input);
		}
		/// <summary>
		/// Cuts off a string before a given term.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="chr"></param>
		/// <returns></returns>
		public static string GetBefore(this string Input, string chr)
		{
			if (Input.Contains(chr))
				return GetBefore_master(Input, Input.IndexOf(chr));
			//implicit else
			throw new Exception("Given string does not contain the search term \"" + chr + "\".\n\nString: " + Input);
		}
		/// <summary>
		/// Cuts off a sting before the nth iteration of a given character.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="chr"></param>
		/// <param name="occurrance"></param>
		/// <returns></returns>
		public static string GetBefore_Nth(this string Input, char chr, int occurrance)
		{
			if (Input.Contains(chr))
				return GetBefore_master(Input, Input.IndexOf_Nth(chr, occurrance));
			//implicit else
			throw new Exception("Given string does not contain the search term \"" + chr + "\".\n\nString: " + Input);
		}
		/// <summary>
		/// Cuts off a string before the nth iteration of a given term.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="chr"></param>
		/// <param name="occurrance"></param>
		/// <returns></returns>
		public static string GetBefore_Nth(this string Input, string chr, int occurrance)
		{
			if (Input.Contains(chr))
				return GetBefore_master(Input, Input.IndexOf_Nth(chr, occurrance));
			//implicit else
			throw new Exception("Given string does not contain the search term \"" + chr + "\".\n\nString: " + Input);
		}
		private static string GetBefore_master(string Input, int idx)
		{
			if (idx > 0)
				return Input.Substring(0, idx);

			//implicit else
			return "";
		}

		/// <summary>
		/// Cuts a string after a provided term.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="term"></param>
		/// <returns></returns>
		public static string GetAfter(this string Input, string term)
		{
			if (Input.Contains(term))
				return GetAfter_master(Input, Input.IndexOf(term), term.Length);
			//implicit else
			throw new Exception("Given string does not contain the search term \"" + term + "\".\n\nString: " + Input);
		}
		/// <summary>
		/// Cuts a string after a provided character.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="chr"></param>
		/// <returns></returns>
		public static string GetAfter(this string Input, char chr)
		{
			if (Input.Contains(chr))
				return GetAfter_master(Input, Input.IndexOf(chr), 1);

			//implicit else
			throw new Exception("Given string does not contain the search term \"" + chr + "\".\n\nString: " + Input);
		}
		private static string GetAfter_master(string Input, int idx, int offset)
		{
			if (idx + offset < Input.Length) return Input.Substring(idx + offset);

			//implicit else
			return "";
		}
		/// <summary>
		/// Cuts a sring between the first instance of <paramref name="chrStart"/> and <paramref name="chrEnd"/>.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="chrStart">The starting tag.</param>
		/// <param name="chrEnd">The ending tag.</param>
		/// <returns></returns>
		public static string GetBetween(this string Input, char chrStart, char chrEnd)
		{
			return Input.GetAfter(chrStart).GetBefore(chrEnd);
		}
		/// <summary>
		/// Cuts a sring between the first instance of <paramref name="chrStart"/> and <paramref name="chrEnd"/>.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="chrStart">The starting tag.</param>
		/// <param name="chrEnd">The ending tag.</param>
		/// <returns></returns>
		public static string GetBetween(this string Input, string chrStart, string chrEnd)
		{
			Input = Input.GetAfter(chrStart);
			Input = Input.GetBefore(chrEnd);
			return Input;
		}
		/// <summary>
		/// Returns a List of the words in a string.
		/// </summary>
		/// <param name="Input"></param>
		/// <returns></returns>
		public static List<string> GetWords(this string Input)
		{
			return GetWords(Input, 0, false, true);
		}
		/// <summary>
		/// Returns a list of words in a string and inserts indentations where applicable, to format the string into paragraphs at the start and where line breaks exist.
		/// </summary>
		/// <param name="Input">The string to be formatted.</param>
		/// <param name="indent">The number of spaces to places of indentation.</param>
		/// <param name="hanging">Whether the non-first lines shoud be indented past the first line, or vice versa.</param>
		/// <param name="startsLine">Whether the provided string is independent or is within a detached string or paragraph.</param>
		/// <returns></returns>
		public static List<string> GetWords(this string Input, byte indent, bool hanging, bool startsLine)
		{
			if (indent > 0 && !hanging)
			{
				string id = "";
				for (byte i = 0; i < indent; i++)
					id += " ";

				Input = Input.Replace("\n", "\n" + id);
				if (!Input.StartsWith("\n") && startsLine)
					Input = id + Input;

			}
			List<string> rtn = new List<string>();
			string wrd = "";
			bool isIndenting = false;
			string InputIdt = "";
			for (int i = 0; i <= Input.Length; i++)
				if (i < Input.Length) {
					if (Input[(int)i] == ' ') {
						if (isIndenting)
							InputIdt += ' ';
						else {
							rtn.Add(wrd);
							wrd = "";
						}
					} else {
						if (isIndenting) {
							isIndenting = false;
							if (InputIdt.Length > 1)
								rtn.Add( TowardIndex(InputIdt, InputIdt.Length - 1) );
						}
						InputIdt = "";
						wrd += Input[(int)i];
					}
				} else {
					rtn.Add(wrd);
					wrd = "";
				}

			return rtn;
		}
		/// <summary>
		/// Cuts a string at and past, or just past, the provided index.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="index"></param>
		/// <param name="inclusive"></param>
		/// <returns></returns>
		/// <returns></returns>
		public static string FromIndex(this string Input, int index, bool inclusive = true)
		{
			if (index < 0) throw new Exception("index must be non-negative!");
			if (inclusive || index + 1 >= Input.Length - 1) return Input.Substring(index);
			//implicit else
			return Input.Substring(index + 1);
		}
		/// <summary>
		/// Cuts a string before and at, or just before, the provided index.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="index"></param>
		/// <param name="inclusive"></param>
		/// <returns></returns>
		public static string TowardIndex(this string Input, int index, bool inclusive = true)
		{
			if (index < 0) throw new Exception("index must be non-negative!");
			if (inclusive && index + 1 < Input.Length)
				return Input.Substring(0, index + 1);
			//implicit else
			return Input.Substring(0, index);
		}
		/// <summary>
		/// Splits the given string into an array of single lines, using '\n' or "\r\n" as delimiters.
		/// </summary>
		/// <param name="Input"></param>
		/// <returns></returns>
		public static string[] GetLines(this string Input)
		{
			return Input.Replace("\r\n", "\n").Split('\n');
		}
	}
}
