using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace QuodLib
{
	namespace Strings
	{
		/// <summary>
		/// A class of methods for modifying strings.
		/// </summary>
		public static class Alter
		{
			/// <summary>
			/// 
			/// </summary>
			/// <param name="num"></param>
			/// <returns></returns>
			public static string Integer_AddSuffix(int num) {
				string rtn = "" + num;
				int ones = System.Math.Abs(num % 10);
				if (ones > 3 || ones == 0) return rtn + "th";

				switch (ones) {
					case 1:
						return rtn + "st";
					case 2:
						return rtn + "nd";
					case 3:
						return rtn + "rd";
				}

				return null;
			}
			/// <summary>
			/// Inverts the order of the characters in a string.
			/// </summary>
			/// <param name="Input"></param>
			/// <returns></returns>
			public static string Reverse(this string Input)
			{
				return Input.Reverse();
			}
			/// <summary>
			/// Replaces an array of <paramref name="terms"/> with a single provided term <paramref name="with"/>.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="terms"></param>
			/// <param name="with"></param>
			/// <returns></returns>
			public static string Replace(this string Input, IList<string> terms, string with)
			{
				string rtn = Input;
				foreach (string rep in terms) rtn = rtn.Replace(rep, with);
				return rtn;
			}
			/// <summary>
			/// Replaces an array of <paramref name="terms"/> with a single provided term <paramref name="with"/>.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="terms"></param>
			/// <param name="with"></param>
			/// <returns></returns>
			public static string Replace(this string Input, IList<char> terms, string with)
			{
				string rtn = Input;
				foreach (char rep in terms) rtn = rtn.Replace("" + rep, with);
				return rtn;
			}
			/// <summary>
			/// Replaces an array of <paramref name="terms"/> with a single provided term <paramref name="with"/>.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="terms"></param>
			/// <param name="with"></param>
			/// <returns></returns>
			public static string Replace(this string Input, IList<char> terms, char with)
			{
				string rtn = Input;
				foreach (char rep in terms) rtn = rtn.Replace(rep, with);
				return rtn;
			}
			/// <summary>
			/// Inserts a <paramref name="term"/> into the <paramref name="Input"/> string at the specified <paramref name="index"/>.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="term"></param>
			/// <param name="index"></param>
			/// <returns></returns>
			public static string Insert(this string Input, string term, int index)
			{
				return Input.TowardIndex(index, false) + term + Input.FromIndex(index, true);
			}
			/// <summary>
			/// Deletes a number of characters starting at <paramref name="index"/>. If the end of the string is hit, the result is simply clipped before <paramref name="index"/>.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="index"></param>
			/// <param name="length"></param>
			/// <returns></returns>
			public static string Delete(this string Input, int index, int length)
			{
				if (index + length >= Input.Length) return Input.TowardIndex(index, false);
				return Input.TowardIndex(index, false) + Input.FromIndex(index + length, false);
			}
			/// <summary>
			/// Deletes a number of characters after <paramref name="startIndex"/> until the search <paramref name="term"/> is found.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="startIndex"></param>
			/// <param name="term"></param>
			/// <returns></returns>
			public static string DeleteTo(this string Input, int startIndex, char term)
			{
				Input.Break(startIndex, out string head, out string tail, true);
				return head + tail.FromIndex(tail.IndexOf(term), true);
			}
			/// <summary>
			/// Deletes a number of characters after <paramref name="startIndex"/> until the search <paramref name="term"/> is found.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="startIndex"></param>
			/// <param name="term"></param>
			/// <returns></returns>
			public static string DeleteTo(this string Input, int startIndex, string term)
			{
				Input.Break(startIndex, out string head, out string tail, true);
				return head + tail.FromIndex(tail.IndexOf(term), true);
			}
			/// <summary>
			/// Deletes a number of characters before <paramref name="index"/> (non-inclusive). If the start of the string is hit, the result is simply clipped at <paramref name="index"/>.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="index"></param>
			/// <param name="length"></param>
			/// <returns></returns>
			public static string Backspace(this string Input, int index, int length)
			{
				if (length >= index) return Input.FromIndex(index, true);
				return Input.TowardIndex(index - length, true) + Input.FromIndex(index, true);
			}
			/// <summary>
			/// Deletes a number of characters before <paramref name="index"/> until the search <paramref name="term"/> is hit.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="index"></param>
			/// <param name="term"></param>
			/// <returns></returns>
			public static string BackspaceTo(this string Input, int index, char term)
			{
				Input.Break(index, out string head, out string tail, false);
				return head.TowardIndex(head.LastIndexOf(term), true) + tail;
			}
			/// <summary>
			/// Deletes a number of characters before <paramref name="index"/> until the search <paramref name="term"/> is hit.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="index"></param>
			/// <param name="term"></param>
			/// <returns></returns>
			public static string BackspaceTo(this string Input, int index, string term)
			{
				Input.Break(index, out string head, out string tail, false);
				return head.TowardIndex(head.LastIndexOf(term) + head.Length, false) + tail;
			}
			/// <summary>
			/// Removes the provided <paramref name="term"/> from the string.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="term"></param>
			/// <returns></returns>
			public static string RemoveTerm(this string Input, char term)
			{
				return Input.Replace("" + term, "");
			}
			/// <summary>
			/// Removes the provided <paramref name="term"/> from the string.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="term"></param>
			/// <returns></returns>
			public static string RemoveTerm(this string Input, string term)
			{
				return Input.Replace(term, "");
			}
			/// <summary>
			/// Removes all <paramref name="terms"/> from the string.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="terms"></param>
			/// <returns></returns>
			public static string RemoveTerms(this string Input, IList<char> terms)
			{
				string rtn = Input;
				foreach (char term in terms) rtn = rtn.RemoveTerm(term);
				return rtn;
			}
			/// <summary>
			/// Removes all <paramref name="terms"/> from the string.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="terms"></param>
			/// <returns></returns>
			public static string RemoveTerms(this string Input, IList<string> terms)
			{
				string rtn = Input;
				foreach (string term in terms) rtn = rtn.RemoveTerm(term);
				return rtn;
			}
			/// <summary>
			/// Removes leading characters or repeated stings of characters.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="chr"></param>
			/// <returns></returns>
			public static string RemoveLeading(this string Input, string chr)
			{
				string rtn = Input;
				while (rtn.StartsWith(chr))
					rtn = rtn.FromIndex(chr.Length, true);

				return rtn;
			}
			/// <summary>
			/// Removes leading characters or repeated stings of characters.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="chr"></param>
			/// <returns></returns>
			public static string RemoveLeading(this string Input, char chr)
			{
				string rtn = Input;
				while (rtn.StartsWith(chr))
					rtn = rtn.FromIndex(1, true);

				return rtn;
			}
			/// <summary>
			/// Removes leading characters or repeated stings of characters.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="chrs"></param>
			/// <returns></returns>
			public static string RemoveLeading(this string Input, IList<string> chrs)
			{
				string rtn = Input;
				foreach (string chr in chrs) rtn = rtn.RemoveLeading(chr);

				return rtn;
			}
			/// <summary>
			/// Removes trailing characters or repeated strings of charachters.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="chr"></param>
			/// <returns></returns>
			public static string RemoveTrailing(this string Input, string chr)
			{
				string rtn = Input;
				while (rtn.EndsWith(chr))
					rtn = rtn.TowardIndex(rtn.Length - chr.Length - 1, true); 

				return rtn;
			}
			/// <summary>
			/// Removes trailing characters or repeated strings of charachters.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="chr"></param>
			/// <returns></returns>
			public static string RemoveTrailing(this string Input, char chr)
			{
				string rtn = Input;
				while (rtn.EndsWith(chr))
					rtn = rtn.TowardIndex(rtn.Length - 2, true);

				return rtn;
			}
			/// <summary>
			/// Removes trailing characters or repeated strings of charachters.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="chrs"></param>
			/// <returns></returns>
			public static string RemoveTrailing(this string Input, IList<string> chrs)
			{
				string rtn = Input;
				foreach (string chr in chrs) rtn = rtn.RemoveTrailing(chr);

				return rtn;
			}

			/// <summary>
			/// Removes leading and trailing characters or strings of characters.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="term"></param>
			/// <returns></returns>
			public static string RemoveEnclosing(this string Input, string term)
			{
				return Input.RemoveLeading(term).RemoveTrailing(term);
			}
			/// <summary>
			/// Removes leading and trailing characters or strings of characters.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="chrs"></param>
			/// <returns></returns>
			public static string RemoveEnclosing(this string Input, IList<string> chrs)
			{
				string rtn = Input;
				foreach (string chr in chrs) rtn = rtn.RemoveEnclosing(chr);
				return rtn;
			}
			/// <summary>
			/// Inserts line-breaks so that each line has no more than <paramref name="lineLimit"/> characters.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="lineLimit"></param>
			/// <returns></returns>
			public static string InsertBreaks(this string Input, int lineLimit)
			{
				if (lineLimit <= 0) throw new Exception("LineLimit mus be greater than zero.");
				return InsertBreaks(Input, lineLimit, 0, false, false);
			}
			/// <summary>
			/// Returns a string, broken into multiple lines so that each line has no more than <paramref name="lineLimit"/> characters, 
			/// and inserts indentations where applicable, to format the string into paragraphs at the start and where line breaks exist.
			/// </summary>
			/// <param name="Input">The string to be formatted.</param>
			/// <param name="lineLimit">The maximum number of characters per line.</param>
			/// <param name="indent">The number of spaces to places of indentation.</param>
			/// <param name="hanging">Whether the non-first lines shoud be indented past the first line, or vice versa.</param>
			/// <param name="startsLine">Whether the provided string is independent or is within a detached string or paragraph.</param>
			/// <returns></returns>
			public static string InsertBreaks(this string Input, int lineLimit, byte indent, bool hanging, bool startsLine)
			{
				if (lineLimit <= 0) throw new Exception("LineLimit mus be greater than zero.");
				List<string> words = Divide.GetWords(Input, indent, hanging, startsLine);
				List<string> Lines = new List<string>();
				string line = "";
				string id = "";
				if (indent > 0 && hanging)
					for (byte id_ = 0; id_ < indent; id_++)
						id += " ";

				string rtn = "";
				for (int i = 0; i < words.Count; i++)
					//bool TooLong;
					if ( (line.Length + 1 + words[i].Length > lineLimit) || (i == words.Count - 1) ) {
						if ( (i <= words.Count - 1) ) {
							if (i == words.Count - 1) {
								if (i != 0) {
									if (line.Length + 1 + words[i].Length > lineLimit) {
										line += "\n";
										if (indent > 0 && hanging) line += id;
									} else
										line += " ";
								}
								line += words[i];
							}
							Lines.Add(line);
							line = "";
							if (indent > 0 && hanging) line += id;
						}
						line += words[i];
					} else {
						if (i != 0) line += " ";
						line += words[i];
					}				

				for (int i = 0; i < Lines.Count; i++) {
					rtn += Lines[i];
					if (i != (Lines.Count - 1)) rtn += "\n";
				}
				return rtn;
			}
			/// <summary>
			/// Breaks a string's lines into paragraphs by imposing a limit on the number of characters per line.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="lineLimit"></param>
			/// <param name="initLimit"></param>
			/// <returns></returns>
			public static string InsertBreaks(this string Input, int lineLimit, int initLimit)
			{
				if (lineLimit <= 0) throw new Exception("lineLimit must be greater than zero.");
				if (initLimit <= 0) throw new Exception("initLimit must be greater than zero.");

				return InsertBreaks(Input, lineLimit, initLimit, 0, false, true);
			}
			/// <summary>
			/// Breaks a string's lines into indented paragraphs by imposing a limit on the number of characters per line.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="lineLimit"></param>
			/// <param name="initLimit"></param>
			/// <param name="indent"></param>
			/// <param name="hanging"></param>
			/// <param name="startsLine"></param>
			/// <returns></returns>
			public static string InsertBreaks(this string Input, int lineLimit, int initLimit, byte indent, bool hanging, bool startsLine)
			{
				if (lineLimit <= 0) throw new Exception("lineLimit must be greater than zero.");
				if (initLimit <= 0) throw new Exception("initLimit must be greater than zero.");

				bool startsNL = Input.StartsWith("\n");
				string text_i;
				if (startsNL) text_i = Divide.FromIndex(Input, 1);
					else text_i = Input;

				if (Divide.GetWords(Input)[0].Length > lineLimit)
					text_i = "";
				else
					text_i = InsertBreaks(text_i, initLimit, indent, hanging, startsLine);

				string text_;
				if (string.IsNullOrEmpty(text_i))
				{
					if (Input.StartsWith("\n"))
						text_ = Divide.FromIndex(Input, Input.IndexOf("\n"), false);
					else
						throw new Exception("Umm...! Ummm...! This was unplanned!");
				} else
					text_ = Divide.FromIndex(Input, text_i.IndexOf("\n"), false);

				if (startsNL)
					if (!string.IsNullOrEmpty(text_i))
						text_i = "\n" + text_i;

				text_ = InsertBreaks(text_, lineLimit, indent, hanging, startsLine);
				text_i = text_i.Split('\n')[0];
				if (string.IsNullOrEmpty(text_i))
					return text_;
				//implicit else
				return text_i + "\n" + text_;
			}
			/// <summary>
			/// Capitalizes each word in the string.
			/// </summary>
			/// <param name="Input"></param>
			/// <returns></returns>
			public static string Capitalize(this string Input)
			{
				string rtn = "";
				List<string> words = Divide.GetWords(Input);
				for (int i = 0; i < words.Count; i++) {
					string Mc = "";
					if (words[i].ToLower().StartsWith("mc")) {
						words[i] = Divide.FromIndex(words[i], 2, true);
						Mc = "Mc";
					}

					words[i] = words[i].ToLower();
					words[i] = words[i][0].ToString().ToUpper() + Divide.FromIndex(words[i], 1, true);
					words[i] = Mc + words[i];
					if (i > 0)
						rtn += " ";

					rtn += words[i];
				}
				return rtn;
			}
			/// <summary>
			/// Replaces numbers zero through 9 in the string with the provided character.
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="Replacement"></param>
			/// <returns></returns>
			public static string ReplaceNumeric(this string Input, char Replacement)
			{
				string rtn = Input;
				for (byte i = 0; i < 10; i++)
					rtn = rtn.Replace("" + i, "" + Replacement);

				return rtn;
			}
		} //</class>
	}
}
