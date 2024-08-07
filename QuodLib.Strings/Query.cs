﻿using SMath = System.Math;
//using System.Text;
//using System.Threading.Tasks;

//using StringCollection = System.Collections.ObjectModel.Collection<string>;

namespace QuodLib.Strings {
    /// <summary>
    /// A class of methods for analyzing data about strings.
    /// </summary>
    public static partial class Query {

        /// <summary>
        /// Begins a new <see cref="StringQuery"/>, using this string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static StringQuery AsQuery(this string input)
            => new(input);

        /// <summary>
        /// Returns whether any of the strings in the List contain the provided <paramref name="term"/>.
        /// </summary>
        /// <param name="Input">A List&lt;string&gt;</param>
        /// <param name="term"></param>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
        public static bool Contains(this IList<string> Input, string term, bool caseSensitive)
            => Input.Any(s => s.Equals(term, caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase));

        /// <summary>
        /// Whether the string contains any of an array of terms.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="terms"></param>
        /// <returns></returns>
        public static bool Contains(this string Input, IList<char> terms)
            => terms.Any(c => Input.Contains(c));

        /// <summary>
        /// Whether the string contains any of an array of terms.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="terms"></param>
        /// <returns></returns>
        public static bool Contains(this string Input, params char[] terms)
            => Input.Contains((IList<char>)terms);

        /// <summary>
        /// Whether the string contains any of an array of terms.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="terms"></param>
        /// <returns></returns>
        public static bool Contains(this string Input, IList<string> terms)
            => terms.Any(t => Input.Contains(t));

        /// <summary>
        /// Whether the string contains any of an array of terms.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="terms"></param>
        /// <returns></returns>
        public static bool Contains(this string Input, params string[] terms)
            => Input.Contains((IList<string>)terms);

        /// <summary>
        /// Whether the string contains an array of terms.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="terms"></param>
        /// <param name="AND">Whether the string must contain all of the terms.</param>
        /// <returns></returns>
        public static bool Contains(this string Input, IList<char> terms, bool AND) {
            if (!AND)
                return Input.Contains(terms);

            return terms.All(t => Input.Contains(t));
        }

        /// <summary>
        /// Whether the string contains an array of terms.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="terms"></param>
        /// <param name="AND">Whether the string must contain all of the terms.</param>
        /// <returns></returns>
        public static bool Contains(this string Input, IList<string> terms, bool AND) {
            if (!AND)
                return Input.Contains(terms);

            return terms.All(t => Input.Contains(t));
        }

        /// <summary>
        /// Whether the string starts with the provided character.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="chr"></param>
        /// <returns></returns>
        public static bool StartsWith(this string Input, char chr)
            => Input != string.Empty && Input[0] == chr;

        /// <summary>
        /// Whether the string starts with one of an array of terms.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="terms"></param>
        /// <returns></returns>
        public static bool StartsWith(this string Input, IList<string> terms)
            => terms.Any(t => Input.StartsWith(t));

        /// <summary>
        /// Whether the string ends with the privided character.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="terms"></param>
        /// <returns></returns>
        public static bool EndsWith(this string Input, char chr)
            => Input.Last() == chr;

        /// <summary>
        /// Whether the string starts with one of an array of terms.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="terms"></param>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
        public static bool StartsWith(this string Input, IList<string> terms, bool caseSensitive)
            => terms.Any(t => Input.StartsWith(t, caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase));

        /// <summary>
        /// Whether the string ends with one of an an array of terms.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="terms"></param>
        /// <returns></returns>
        public static bool EndsWith(this string Input, IList<string> terms)
            => terms.Any(t => Input.EndsWith(t));

        /// <summary>
        /// Whether the string ends with one of an an array of terms.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="terms"></param>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
        public static bool EndsWith(this string Input, IList<string> terms, bool caseSensitive)
            => terms.Any(t => Input.EndsWith(t, caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase));

        /// <summary>
        /// Returns the closest index of something other than <paramref name="term"/>, relative to <paramref name="index"/>. If both directions have equally close results, the lower index is chosen.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="index"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        public static int SeekNot(this string Input, int index, char term) {
            Exception exH = new Exception("head_nullException"), exT = new Exception("tail_nullException");
            int head = index, tail = index;

            try {
                head = Input.SeekNot(index, term, false);
            } catch (Exception ex) {
                exH = ex;
                head = -1;
            }
            try {
                tail = Input.SeekNot(index, term, true);
            } catch (Exception ex) {
                exT = ex;
                tail = -1;
            }
            if (head == -1 && tail == -1)
                throw new Exception("Failed both directions: head { " + exH.ToString() + " } & tail {" + exT.ToString() + " }.");

            if (head == -1)
                return tail;
            if (tail == -1)
                return head;
            if (tail - index > index - head)
                return tail;

            return head;
        }
        /// <summary>
        /// Returns the closest index of something other than <paramref name="term"/>, relative to <paramref name="index"/>. If both directions have equally close results, the lower index is chosen.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="index"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        public static int SeekNot(this string Input, int index, string term) {
            Exception exH = new Exception("head_nullException"), exT = new Exception("tail_nullException");
            int head = index, tail = index;
            try {
                head = Input.SeekNot(index, term, false);
            } catch (Exception ex) {
                exH = ex;
                head = -1;
            }
            try {
                tail = Input.SeekNot(index, term, true);
            } catch (Exception ex) {
                exT = ex;
                tail = -1;
            }
            if (head == -1 && tail == -1)
                throw new Exception("Failed both directions: head { " + exH.ToString() + " } & tail {" + exT.ToString() + " }.");

            if (head == -1)
                return tail;
            if (tail == -1)
                return head;
            if (tail - index > index - head)
                return tail;

            return head;
        }
        /// <summary>
        /// Returns the closest index of something other than <paramref name="term"/>, (starting at <paramref name="index"/>) if <paramref name="forward"/> or (before <paramref name="index"/>) if not.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="index"></param>
        /// <param name="term"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        public static int SeekNot(this string Input, int index, char term, bool forward) {
            if (forward)
                while (Input[index] == term && index < Input.GetLastIndex())
                    index++;
            else
                while (Input[index] == term && index > 0)
                    index--;
            
            return index;
        }
        /// <summary>
        /// Returns the closest index of something other than <paramref name="term"/>, (starting at <paramref name="index"/>) if <paramref name="forward"/> or (before <paramref name="index"/>) if not.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="index"></param>
        /// <param name="term"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        public static int SeekNot(this string Input, int index, string term, bool forward) {
            if (forward)
                while (Input.Substring(index - term.Length, term.Length) == term && index >= term.Length)
                    index -= term.Length;
            else
                while (Input.Substring(index, term.Length) == term && index < Input.GetLastIndex())
                    index += term.Length;
            
            return index;
        }
        /// <summary>
        /// Returns the closest index of <paramref name="term"/>, after <paramref name="index"/> if <paramref name="forward"/> or before <paramref name="index"/> if not.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="index"></param>
        /// <param name="term"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        public static int Seek(this string Input, int index, char term, bool forward) {
            if (forward)
                return Input.FromIndex(index, false).IndexOf(term) + index;

            return Input.TowardIndex(index, false).LastIndexOf(term);
        }
        /// <summary>
        /// Returns the closest index of <paramref name="term"/>, after <paramref name="index"/> if <paramref name="forward"/> or before <paramref name="index"/> if not.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="index"></param>
        /// <param name="term"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        public static int Seek(this string Input, int index, string term, bool forward) {
            if (forward)
                return Input.FromIndex(index, false).IndexOf(term) + index;

            return Input.TowardIndex(index, false).LastIndexOf(term);
        }
        /// <summary>
        /// Returns the closest index of any <paramref name="chrs"/>, after <paramref name="index"/> if <paramref name="forward"/> or before <paramref name="index"/> if not.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="index"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        public static int Seek(this string Input, int index, char[] chrs, bool forward) {
            int rtn = index;
            foreach (char chr in chrs) {
                int idx = Input.Seek(index, chr, forward);
                if (forward ? idx < rtn : idx < rtn)
                    rtn = idx;
            }

            return (rtn == index ? -1 : rtn);
        }
        /// <summary>
        /// Returns the closest index of any <paramref name="terms"/>, after <paramref name="index"/> if <paramref name="forward"/> or before <paramref name="index"/> if not.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="index"></param>
        /// <param name="terms"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        public static int Seek(this string Input, int index, string[] terms, bool forward) {
            int rtn = index;
            foreach (string term in terms) {
                int idx = Input.Seek(index, term, forward);
                if (forward ? idx < rtn : idx < rtn)
                    rtn = idx;
            }

            return (rtn == index ? -1 : rtn);
        }
        /// <summary>
        /// Returns the closest index of <paramref name="term"/>, after <paramref name="index"/> if <paramref name="forward"/>, or before <paramref name="index"/> if not. The retruned index is shifted after the term if ![startOfTerm].
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="index"></param>
        /// <param name="term"></param>
        /// <param name="forward"></param>
        /// <param name="startOfTerm"></param>
        /// <returns></returns>
        public static int Seek(this string Input, int index, string term, bool forward, bool startOfTerm) {
            if (forward)
                return Input.FromIndex(index, false).IndexOf(term) + index + (startOfTerm ? term.Length : 0);

            return Input.TowardIndex(index, false).LastIndexOf(term) + (startOfTerm ? term.Length : 0);
        }

        /// <summary>
        /// Returns the first index at which any of the provided terms occurs.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="terms"></param>
        /// <returns></returns>
        public static int IndexOfAny(this string Input, IList<string> terms) {
            int rtn = -1;
            foreach (string str in terms) {
                if (!Input.Contains(str))
                    continue;

                int idx = Input.IndexOf(str);
                if (rtn == -1 || idx < rtn)
                    rtn = idx;
            }

            return rtn;
        }
        /// <summary>
        /// Returns the first index after <paramref name="startIndex"/> (inclusive) that any of the provided terms occurs.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="terms"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static int IndexOfAny(this string Input, IList<string> terms, int startIndex) {
            int rtn = -1;
            string temp = Input.FromIndex(startIndex, true);
            foreach (string str in terms) {
                if (!temp.Contains(str))
                    continue;

                int idx = temp.IndexOf(str);
                if (rtn == -1 || idx < rtn)
                    rtn = idx; //if (rtn hasn't been changed yet || idx is earlier than rtn) idx -> rtn
            }

            if (rtn == -1)
                return -1;

            return startIndex + rtn;
        }
        /// <summary>
        /// Returns the last valid index of the string.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static int GetLastIndex(this string Input)
            => Input.Length - 1;

        /// <summary>
        /// Returns the (0-based) Nth occurrance of <paramref name="chr"/>.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="chr"></param>
        /// <param name="occurrance"></param>
        /// <returns></returns>
        public static int IndexOf_Nth(this string Input, char chr, int occurrance)
            => Input.IndexOf_Nth("" + chr, occurrance);

        /// <summary>
        /// Returns the (0-based) Nth occurrance of <paramref name="chr"/>.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="chr"></param>
        /// <param name="occurrance"></param>
        /// <returns></returns>
        public static int IndexOf_Nth(this string Input, string chr, int occurrance) {
            if (!Input.Contains(chr))
                throw new Exception("Term \"" + chr + "\" not contained within [Input] string.");

            if (occurrance == 0)
                return Input.IndexOf(chr);
            if (occurrance == -1)
                return Input.LastIndexOf(chr);

            string temp = Input;
            int idx = 0;
            if (occurrance > 0) {
                for (int i = 0; i < occurrance + 1; i++) {
                    if (!temp.Contains(chr))
                        throw new Exception("Erorr: Only found " + i + " occurrances of \"" + chr + "\".");

                    idx += temp.IndexOf(chr) + chr.Length;

                    temp = Input.FromIndex(idx, true);
                }

                return idx - chr.Length;
            }

            //if (occurrance < 0)
            idx = Input.GetLastIndex();
            for (int i = 0; i < (0 - occurrance); i++) {
                if (!temp.Contains(chr))
                    throw new Exception("Error: Only found " + i + " occurrances of \"" + chr + "\".");

                idx = temp.LastIndexOf(chr);
                temp = Input.TowardIndex(idx, false);
            }

            return idx;
        }
        /// <summary>
        /// Returns the next (0-based) occurrance of <paramref name="chr"/>.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="chr"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static int NextIndexOf(this string Input, string chr, int startIndex) {
            if (startIndex >= Input.Length)
                throw new Exception("Index \"" + startIndex + "\" out of bounds of [Input] string.");

            string temp = Input.FromIndex(startIndex, true);

            if (Input.IndexOf(chr) == 0)
                temp = Input.FromIndex(chr.Length, true);

            return temp.IndexOf(chr);
        }
        /// <summary>
        /// Returns the index at which two strings diverge. Returns -1 if one of the strings is empty.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="compare"></param>
        /// <returns><list type="bullet">
        ///     <item><c>-1</c> if the either string is empty</item>
        ///     <item>else, the farthest index toward which both strings are equal</itemm>
        /// </list>
        /// </returns>
        public static int IndexOfDivergence(this string Input, string compare) {
            if (Input == string.Empty || compare == string.Empty)
                return -1;
            
            int i;
            for (i = 0; i < Input.Length && i < compare.Length && Input[i] == compare[i]; i++);

            return i;
        }

        /// <summary>
        /// Returns the index at which any one of the strings differs from the rest. Ignores empty strings.
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns><list type="bullet">
        ///     <item><c>-1</c> if the any string is empty</item>
        ///     <item><see cref="null"/> if <paramref name="inputs"/> is single or empty</item>
        ///     <item>else, the farthest index toward which <paramref name="inputs"/> are equal</itemm>
        /// </list></returns>
        public static int? IndexOfDivergence(IList<string> inputs) {
            if (inputs.Count < 2)
                return null;

            int rtn = int.MaxValue;
            for (int i = 0; i < inputs.Count - 1; i++) {
                if (inputs[i] == string.Empty || inputs[i + 1] == string.Empty)
                    return -1;

                rtn = SMath.Min(rtn, inputs[i].IndexOfDivergence(inputs[i + 1]));
            }

            if (rtn == int.MaxValue)
                throw new Exception("Unexpected execution path");

            return rtn;
        }
    } //</class>
}
