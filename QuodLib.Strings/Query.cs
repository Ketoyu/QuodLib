using System;
using SMath = System.Math;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using StringCollection = System.Collections.ObjectModel.Collection<string>;

namespace QuodLib.Strings
{
	/// <summary>
	/// A class of methods for analyzing data about strings.
	/// </summary>
	public static class Query
	{
		/// <summary>
		/// A fluid object for performing searches and substrings on one <see cref="Source"/> string, without constantly creating new copies.
		/// </summary>
		public class StringQuery {

			/// <summary>
			/// The source <see cref="string"/> to query on.
			/// </summary>
			public string Source { get; protected set; }

			/// <summary>
			/// The effective start-index of the current query.
			/// </summary>
			/// <remarks>Default: 0</remarks>
			public int StartIndex { get; protected set; }

			/// <summary>
			/// The effective end-index of the current query.
			/// </summary>
			/// <remarks>Default: <see cref="Source"/>'s <see cref="string.Length"/> - 1</remarks>
			public int EndIndex { get; protected set; }

			/// <summary>
			/// Whether to include the search-term in the result of GetBefore queries.
			/// </summary>
			/// <remarks>Default: off</remarks>
			public bool StartInclusive { get; protected set; } = false;

			/// <summary>
			/// Whether to include the search-term in the result of GetAfter queries.
			/// </summary>
			/// <remarks>Default: off</remarks>
			public bool EndInclusive { get; protected set; } = false;

			/// <summary>
			/// The effective length of the string, if this <see cref="StringQuery"/> were to be executed.
			/// </summary>
			public int Length
				=> EndIndex - StartIndex + 1;

			/// <summary>
			/// Creates a new <see cref="StringQuery"/>, using the provided <see cref="string"/>.
			/// </summary>
			/// <param name="source"></param>
			public StringQuery(string source) {
				Source = source;
				EndIndex = Source.Length - 1;
            }

			/// <summary>
			/// Returns a copy of the searcher, so that subsequent queries do not affect the current searcher.
			/// </summary>
			/// <param name="preserveData">Whether to preserve the original string and indexes.</param>
			/// <param name="preserveInclusive">Whether to preserve the StartInclusive/EndInclusive settings.</param>
			/// <returns></returns>
			public StringQuery Copy(bool preserveData = false, bool preserveInclusive = true)
			{
				StringQuery rtn = (preserveData
					? new StringQuery(Source) {
						StartIndex = this.StartIndex,
						EndIndex = this.EndIndex
					}
					: new StringQuery(this.ToString())
				);

				if (preserveInclusive) {
					rtn.StartInclusive = this.StartInclusive;
					rtn.EndInclusive = this.EndInclusive;
                }

				return rtn;
			}

			/// <summary>
			/// Clamps the <see cref="Source"/> to the current query and resets the Start-/End-Indexes.
			/// </summary>
			/// <returns></returns>
			public StringQuery Clamp() {
				Source = this.ToString();
				StartIndex = 0;
				EndIndex = Source.Length - 1;
				return this;
            }

			/// <summary>
			/// Resets the query.
			/// </summary>
			/// <param name="preserveInclusive">Whether to preserve the <see cref="StartInclusive"/>/<see cref="EndInclusive"/>settings.</param>
			/// <returns></returns>
			public StringQuery Reset(bool preserveInclusive = true) {
				StartIndex = 0;
				EndIndex = Source.Length - 1;

				if (!preserveInclusive) {
					StartInclusive = false;
					EndInclusive = false;
                }
				return this;
            }

			/// <summary>
			/// Executes the current query and returns the resulting string.
			/// </summary>
			/// <returns>The resulting string, using the current <see cref="StartIndex"/> and <see cref="EndIndex"/>.</returns>
			public override string ToString()
				=> Source.Substring(StartIndex, Length);

			#region Inclusive
			/// <summary>
			/// Switches the <see cref="StartInclusive"/> setting on.
			/// </summary>
			/// <returns>The updated query.</returns>
			public StringQuery AsStartInclusive() {
				StartInclusive = true;
				return this;
            }
			/// <summary>
			/// Switches the <see cref="EndInclusive"/> setting on.
			/// </summary>
			/// <returns>The updated query.</returns>
			public StringQuery AsEndInclusive() {
				EndInclusive = true;
				return this;
            }

			/// <summary>
			/// Switches the <see cref="StartInclusive"/> setting off.
			/// </summary>
			/// <returns>The updated query.</returns>
			public StringQuery AsStartDisclusive() {
				StartInclusive = false;
				return this;
			}

			/// <summary>
			/// Switches the <see cref="EndInclusive"/> setting off.
			/// </summary>
			/// <returns>The updated query.</returns>
			public StringQuery AsEndDisclusive() {
				EndInclusive = false;
				return this;
			}

			/// <summary>
			/// Switches the <see cref="StartInclusive"/> and <see cref="EndInclusive"/> settings both on.
			/// </summary>
			/// <returns>The updated query.</returns>
			public StringQuery AsInclusive() {
				StartInclusive = true;
				EndInclusive = true;
				return this;
            }

			/// <summary>
			/// Switches the <see cref="StartInclusive"/> and <see cref="EndInclusive"/> settings both off.
			/// </summary>
			/// <returns>The updated query.</returns>
			public StringQuery AsDisclusive() {
				StartInclusive = false;
				EndInclusive = false;
				return this;
			}
			#endregion //Inclusive

			#region Indexes
			/// <summary>
			/// Forces the <see cref="StartIndex"/> to the non-relative <paramref name="index"/>.
			/// </summary>
			/// <remarks>
			/// <see cref="StartInclusive"/> does not apply here; processes inclusively.
			/// </remarks>
			/// <param name="index">The new, non-relative index.</param>
			/// <returns>The updated query.</returns>
			public StringQuery From(int index) {
				if (index < 0 || index >= Source.Length) throw new ArgumentOutOfRangeException("Index out of range of the Source string!");
				StartIndex = index;
				return this;
			}
			/// <summary>
			/// Forces the <see cref="EndIndex"/> to the non-relative <paramref name="index"/>.
			/// </summary>
			/// <remarks>
			/// <see cref="EndInclusive"/> does not apply here; processes inclusively.
			/// </remarks>
			/// <param name="index">The new, non-relative index.</param>
			/// <returns>The updated query.</returns>
			public StringQuery Until(int index) {
				if (index < 0 || index >= Source.Length) throw new ArgumentOutOfRangeException("Index out of range of the Source string!");
				EndIndex = index;
				return this;
			}

			/// <summary>
			/// Updates the <see cref="EndIndex"/> to be <see cref="StartIndex"/> + <paramref name="index"/>.
			/// </summary>
			/// <remarks>
			/// <see cref="EndInclusive"/> does not apply here; processes inclusively.
			/// </remarks>
			/// <param name="index">The new, relative index.</param>
			/// <returns></returns>
			/// <exception cref="IndexOutOfRangeException"></exception>
			public StringQuery RelativeUntil(int index) {
				if (StartIndex + index > EndIndex)
					throw new IndexOutOfRangeException("Reached past the EndIndex!");

				EndIndex = StartIndex + index;
				return this;
			}

			/// <summary>
			/// Increases the <see cref="StartIndex"/> by <paramref name="index"/>.
			/// </summary>
			/// <remarks>
			/// <see cref="StartInclusive"/> does not apply here; processes inclusively.
			/// </remarks>
			/// <param name="index">The new, relative index.</param>
			/// <returns></returns>
			/// <exception cref="IndexOutOfRangeException"></exception>
			public StringQuery RelativeFrom(int index) {
				if (StartIndex + index > EndIndex)
					throw new IndexOutOfRangeException("Jumped past the EndIndex!");

				StartIndex += index;
				return this;
			}
			#endregion

			#region After
			/// <summary>
			/// Moves the <see cref="StartIndex"/> to the effective earliest instance of the <paramref name="term"/>.
			/// </summary>
			/// <param name="term">The search-term.</param>
			/// <returns>The updated query.</returns>
			public StringQuery After(string term) {
				StartIndex = Source.IndexOf(term, StartIndex) + (StartInclusive ? 0 : term.Length);
				return this;
			}

			/// <summary>
			/// Moves the <see cref="StartIndex"/> to the effective earliest instance of the <paramref name="term"/>.
			/// </summary>
			/// <param name="term">The search-term.</param>
			/// <returns>The updated query.</returns>
			public StringQuery After(char term) {
				StartIndex = Source.IndexOf(term, StartIndex) + (StartInclusive ? 0 : 1);
				return this;
			}

			/// <summary>
			/// Moves the <see cref="StartIndex"/> to the effective earliest instance of, sequentially, each item in the list of <paramref name="terms"/>.
			/// </summary>
			/// <param name="terms">The sequential list of search-terms.</param>
			/// <returns>The updated query.</returns>
			public StringQuery After(params string[] terms) {
				foreach (string term in terms) {
					After(term);
				}
				return this;
			}

			/// <summary>
			/// Moves the <see cref="StartIndex"/> to the effective earliest instance of, sequentially, each item in the list of <paramref name="terms"/>.
			/// </summary>
			/// <param name="terms">The sequential list of search-terms.</param>
			/// <returns>The updated query.</returns>
			public StringQuery After(params char[] terms) {
				foreach (char term in terms) {
					After(term);
				}
				return this;
			}

            /// <summary>
            /// Moves the <see cref="StartIndex"/> until <paramref name="until"/> or the end of the <see cref="Source"/>.
            /// </summary>
            /// <param name="until">The criteria when looping through the <see cref="char"/>s of <see cref="Source"/>.</param>
			/// <param name="found"></param>
            /// <returns>The updated query.</returns>
            public StringQuery Seek(Func<char, bool> until, out bool found) {
                char c;
                while (!(found = until(c = Source[StartIndex])) && StartIndex < Source.Length)
                    StartIndex++;

                if (StartIndex > Source.Length)
                    StartIndex = Source.Length - 1;

                return this;
            }

            /// <summary>
            /// Moves the <see cref="StartIndex"/> to the effective last instance of the <paramref name="term"/>.
            /// </summary>
            /// <param name="term">The search-term.</param>
            /// <returns>The updated query.</returns>
            public StringQuery AfterLast(string term) {
				int index_i = EffectiveLast(term);
					
				if (!StartInclusive)
					index_i += term.Length;

				StartIndex = index_i;

				return this;
			}

			/// <summary>
			/// Moves the <see cref="StartIndex"/> to the effective last instance of the <paramref name="term"/>.
			/// </summary>
			/// <param name="term">The search-term.</param>
			/// <returns>The updated query.</returns>
			public StringQuery AfterLast(char term) {
				int index_i = EffectiveLast(term);

				if (!StartInclusive)
					index_i++;

				StartIndex = index_i;

				return this;
			}
			#endregion //After

			#region Before
			/// <summary>
			/// Moves the <see cref="EndIndex"/> to the effective earliest instance of the <paramref name="term"/>.
			/// </summary>
			/// <param name="term">The search-term.</param>
			/// <returns>The updated query.</returns>
			public StringQuery Before(string term) {
				EndIndex = Source.IndexOf(term, StartIndex) - 1 + (EndInclusive ? term.Length : 0);
				return this;
			}

			/// <summary>
			/// Moves the <see cref="EndIndex"/> to the effective earliest instance of the <paramref name="term"/>.
			/// </summary>
			/// <param name="term">The search-term.</param>
			/// <returns>The updated query.</returns>
			public StringQuery Before(char term) {
				EndIndex = Source.IndexOf(term, StartIndex) - (EndInclusive ? 0 : 1);
				return this;
			}

			/// <summary>
			/// Moves the <see cref="EndIndex"/> to the effective last instance of the <paramref name="term"/>.
			/// </summary>
			/// <param name="term">The search-term.</param>
			/// <returns>The updated query.</returns>
			public StringQuery BeforeLast(string term) {
				int index_i = EffectiveLast(term) - 1;

				if (EndInclusive)
					index_i += term.Length;

				EndIndex = index_i;

				return this;
			}

			/// <summary>
			/// Moves the <see cref="EndIndex"/> to the effective last instance of the <paramref name="term"/>.
			/// </summary>
			/// <param name="term">The search-term.</param>
			/// <returns>The updated query.</returns>
			public StringQuery BeforeLast(char term) {
				int index_i = EffectiveLast(term);

				if (!EndInclusive)
					index_i--;

				EndIndex = index_i;

				return this;
			}
			#endregion //Before

			#region EffectiveLast
			/// <summary>
			/// Returns the effective index of the last instance of the <paramref name="term"/>.
			/// </summary>
			/// <param name="term">The search-term.</param>
			/// <returns>The updated query.</returns>
			protected int EffectiveLast(string term)
				=> Source.LastIndexOf(term, EndIndex);

			/// <summary>
			/// Returns the effective index of the last instance of the <paramref name="term"/>.
			/// </summary>
			/// <param name="term">The search-term.</param>
			/// <returns>The updated query.</returns>
			protected int EffectiveLast(char term)
				=> Source.LastIndexOf(term, EndIndex);
			#endregion //EffectiveLast

			#region Between
			/// <summary>
			/// Moves the <see cref="StartIndex"/> to the effective earliest instance of <paramref name="start"/>, then the <see cref="EndIndex"/> to the then-effective earliest instance of <paramref name="end"/>.
			/// </summary>
			/// <param name="start">The starting search-term.</param>
			/// <param name="end">The ending search-term.</param>
			/// <returns></returns>
			public StringQuery Between(string start, string end) {
				After(start);
				Before(end);
				return this;
			}

			/// <summary>
			/// Moves the <see cref="StartIndex"/> to the effective earliest instance of <paramref name="start"/>, then the <see cref="EndIndex"/> to the then-effective earliest instance of <paramref name="end"/>.
			/// </summary>
			/// <param name="start">The starting search-term.</param>
			/// <param name="end">The ending search-term.</param>
			/// <returns></returns>
			public StringQuery Between(char start, char end) {
				After(start);
				Before(end);
				return this;
			}
			#endregion //Between
		}

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
		{
			foreach (string str in Input)
				if ( (caseSensitive ? str : str.ToLower() ) == (caseSensitive ? term : term.ToLower() ) ) return true;

			return false;
		}

		/// <summary>
		/// Whether the string contains any of an array of terms.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="terms"></param>
		/// <returns></returns>
		public static bool Contains(this string Input, IList<char> terms)
		{
			foreach (char term in terms)
				if (Input.Contains(term)) return true;

			return false;
		}

		/// <summary>
		/// Whether the string contains any of an array of terms.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="terms"></param>
		/// <returns></returns>
		public static bool Contains(this string Input, params char[] terms) {
			foreach (char term in terms)
				if (Input.Contains(term)) return true;

			return false;
		}

		/// <summary>
		/// Whether the string contains any of an array of terms.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="terms"></param>
		/// <returns></returns>
		public static bool Contains(this string Input, IList<string> terms)
		{
			foreach (string term in terms)
				if (Input.Contains(term)) return true;

			return false;
		}

		/// <summary>
		/// Whether the string contains any of an array of terms.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="terms"></param>
		/// <returns></returns>
		public static bool Contains(this string Input, params string[] terms) {
			foreach (string term in terms)
				if (Input.Contains(term)) return true;

			return false;
		}
		/// <summary>
		/// Whether the string contains an array of terms.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="terms"></param>
		/// <param name="AND">Whether the string must contain all of the terms.</param>
		/// <returns></returns>
		public static bool Contains(this string Input, IList<char> terms, bool AND)
		{
			if (!AND) return Input.Contains(terms);

			bool rtn = true;
			foreach (char term in terms)
				rtn &= Input.Contains(term);

			return rtn;
		}
		/// <summary>
		/// Whether the string contains an array of terms.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="terms"></param>
		/// <param name="AND">Whether the string must contain all of the terms.</param>
		/// <returns></returns>
		public static bool Contains(this string Input, IList<string> terms, bool AND)
		{
			if (!AND) return Input.Contains(terms);

			bool rtn = true;
			foreach (string term in terms)
				rtn &= Input.Contains(term);

			return rtn;
		}
		/// <summary>
		/// Whether the string starts with the provided character.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="chr"></param>
		/// <returns></returns>
		public static bool StartsWith(this string Input, char chr)
		{
			if (Input == "") return chr == null;
				
			return Input[0] == chr;
		}
		/// <summary>
		/// Whether the string starts with one of an array of terms.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="terms"></param>
		/// <returns></returns>
		public static bool StartsWith(this string Input, IList<string> terms)
		{
			foreach (string term in terms)
				if (Input.StartsWith(term)) return true;

			return false;
		}
		/// <summary>
		/// Whether the string ends with the privided character.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="terms"></param>
		/// <returns></returns>
		public static bool EndsWith(this string Input, char chr)
		{
			return Input[Input.Length - 1] == chr;
		}
		/// <summary>
		/// Whether the string starts with one of an array of terms.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="terms"></param>
		/// <param name="caseSensitive"></param>
		/// <returns></returns>
		public static bool StartsWith(this string Input, IList<string> terms, bool caseSensitive)
		{
			foreach (string term in terms)
				if ((caseSensitive ? Input : Input.ToLower()).StartsWith( caseSensitive ? term : term.ToLower() )) return true;

			return false;
		}
		/// <summary>
		/// Whether the string ends with one of an an array of terms.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="terms"></param>
		/// <returns></returns>
		public static bool EndsWith(this string Input, IList<string> terms)
		{
			foreach (string term in terms)
				if (Input.EndsWith(term)) return true;

			return false;
		}
		/// <summary>
		/// Whether the string ends with one of an an array of terms.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="terms"></param>
		/// <param name="caseSensitive"></param>
		/// <returns></returns>
		public static bool EndsWith(this string Input, IList<string> terms, bool caseSensitive)
		{
			foreach (string term in terms)
				if ((caseSensitive ? Input : Input.ToLower()).EndsWith( caseSensitive ? term : term.ToLower() )) return true;

			return false;
		}

		/// <summary>
		/// Returns the closest index of something other than <paramref name="term"/>, relative to <paramref name="index"/>. If both directions have equally close results, the lower index is chosen.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="index"></param>
		/// <param name="term"></param>
		/// <returns></returns>
		public static int SeekNot(this string Input, int index, char term)
		{
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
			if (head == -1 && tail == -1) throw new Exception("Failed both directions: head { " + exH.ToString() + " } & tail {" + exT.ToString() + " }.");
			if (head == -1) return tail;
			if (tail == -1) return head;
			if (tail - index > index - head) return tail;
			return head;
		}
		/// <summary>
		/// Returns the closest index of something other than <paramref name="term"/>, relative to <paramref name="index"/>. If both directions have equally close results, the lower index is chosen.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="index"></param>
		/// <param name="term"></param>
		/// <returns></returns>
		public static int SeekNot(this string Input, int index, string term)
		{
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
			if (head == -1 && tail == -1) throw new Exception("Failed both directions: head { " + exH.ToString() + " } & tail {" + exT.ToString() + " }.");
			if (head == -1) return tail;
			if (tail == -1) return head;
			if (tail - index > index - head) return tail;
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
		public static int SeekNot(this string Input, int index, char term, bool forward)
		{
			if (forward) {
				while (Input[index] == term && index < Input.GetLastIndex()) index++;
			} else {
				while (Input[index] == term && index > 0) index--;
			}
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
		public static int SeekNot(this string Input, int index, string term, bool forward)
		{
			if (forward) {
				while (Input.Substring(index - term.Length, term.Length) == term && index >= term.Length) index -= term.Length;
			} else {
				while (Input.Substring(index, term.Length) == term && index < Input.GetLastIndex()) index += term.Length;
			}
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
			if (forward) return Input.FromIndex(index, false).IndexOf(term) + index;
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
		public static int Seek(this string Input, int index, string term, bool forward)
		{
			if (forward) return Input.FromIndex(index, false).IndexOf(term) + index;
			return Input.TowardIndex(index, false).LastIndexOf(term);
		}
		/// <summary>
		/// Returns the closest index of any <paramref name="chrs"/>, after <paramref name="index"/> if <paramref name="forward"/> or before <paramref name="index"/> if not.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="index"></param>
		/// <param name="forward"></param>
		/// <returns></returns>
		public static int Seek(this string Input, int index, char[] chrs, bool forward)
		{
			int rtn = index;
			foreach (char chr in chrs)
			{
				int idx = Input.Seek(index, chr, forward);
				if (forward ? idx < rtn : idx < rtn) rtn = idx;
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
		public static int Seek(this string Input, int index, string[] terms, bool forward)
		{
			int rtn = index;
			foreach (string term in terms) {
				int idx = Input.Seek(index, term, forward);
				if (forward ? idx < rtn : idx < rtn) rtn = idx;
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
			if (forward) return Input.FromIndex(index, false).IndexOf(term) + index + (startOfTerm ? term.Length : 0);
			return Input.TowardIndex(index, false).LastIndexOf(term) + (startOfTerm ? term.Length : 0);
		}

		/// <summary>
		/// Returns the first index at which any of the provided terms occurs.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="terms"></param>
		/// <returns></returns>
		public static int IndexOfAny(this string Input, IList<string> terms)
		{
			int rtn = -1;
			foreach (string str in terms) {
				if (!Input.Contains(str)) continue;
				int idx = Input.IndexOf(str);
				if (rtn == -1 || idx < rtn) rtn = idx;
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
		public static int IndexOfAny(this string Input, IList<string> terms, int startIndex)
		{
			int rtn = -1;
			string temp = Input.FromIndex(startIndex, true);
			foreach (string str in terms) {
				if (!temp.Contains(str)) continue;
				int idx = temp.IndexOf(str);
				if (rtn == -1 || idx < rtn) rtn = idx; //if (rtn hasn't been changed yet || idx is earlier than rtn) idx -> rtn
			}
			if (rtn == -1) return -1;
			return startIndex + rtn;
		}
		/// <summary>
		/// Returns the last valid index of the string.
		/// </summary>
		/// <param name="Input"></param>
		/// <returns></returns>
		public static int GetLastIndex(this string Input)
		{
			return Input.Length - 1;
		}
		/// <summary>
		/// Returns the (0-based) Nth occurrance of <paramref name="chr"/>.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="chr"></param>
		/// <param name="occurrance"></param>
		/// <returns></returns>
		public static int IndexOf_Nth(this string Input, char chr, int occurrance)
		{
			return Input.IndexOf_Nth("" + chr, occurrance);
		}
		/// <summary>
		/// Returns the (0-based) Nth occurrance of <paramref name="chr"/>.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="chr"></param>
		/// <param name="occurrance"></param>
		/// <returns></returns>
		public static int IndexOf_Nth(this string Input, string chr, int occurrance)
		{
			if (!Input.Contains(chr)) throw new Exception("Term \"" + chr + "\" not contained within [Input] string.");

			if (occurrance == 0) return Input.IndexOf(chr);
			if (occurrance == -1) return Input.LastIndexOf(chr);

			string temp = Input;
			int idx = 0;
			if (occurrance > 0) {
				for (int i = 0; i < occurrance + 1; i++)
				{
					if (!temp.Contains(chr)) throw new Exception("Erorr: Only found " + i + " occurrances of \"" + chr + "\".");
					idx += temp.IndexOf(chr) + chr.Length;

					temp = Input.FromIndex(idx, true);
				}
				return idx - chr.Length;
			}

			//if (occurrance < 0)
			idx = Input.GetLastIndex();
			for (int i = 0; i < (0 - occurrance); i++)
			{
				if (!temp.Contains(chr)) throw new Exception("Error: Only found " + i + " occurrances of \"" + chr + "\".");
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
		public static int NextIndexOf(this string Input, string chr, int startIndex)
		{
			if (startIndex >= Input.Length) throw new Exception("Index \"" + startIndex + "\" out of bounds of [Input] string.");
			string temp = Input.FromIndex(startIndex, true);
				
			if (Input.IndexOf(chr) == 0) temp = Input.FromIndex(chr.Length, true);

			return temp.IndexOf(chr);
		}
		/// <summary>
		/// Returns the index at which two strings diverge. Returns -1 if one of the strings is empty.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="compare"></param>
		/// <returns></returns>
		public static int IndexOfDivergence(this string Input, string compare)
		{
			if (Input == "" || compare == "") return -1;
			int rtn = 0;
			while (Input[rtn] == compare[rtn])
				rtn++;

			return rtn;
		}
		/// <summary>
		/// Returns the index at which any one of the strings differs from the rest. Ignores empty strings.
		/// </summary>
		/// <param name="inputs"></param>
		/// <returns></returns>
		public static int IndexOfDivergence(IList<string> inputs) {
			int rtn = int.MaxValue;
			for (int i = 0; i < inputs.Count - 1; i++) {
				if (inputs[i] == "" || inputs[i+1] == "") continue;
				rtn = SMath.Min(rtn, inputs[i].IndexOfDivergence(inputs[i+1]));
			}
			return rtn;
		}

		public static bool FollowsFormat(this string Input, string Format)
		{
			if (Input.Length == Format.Length)
			{
				//Generate array
				bool[][] chars = new bool[][] { new bool[Format.Length], new bool[Format.Length] };;
				for (byte i = 0; i < Format.Length; i++)
				{
					switch (Format[i])
					{
						case '0': case '1': case '2': case '3': case '4': case '5': case '6': case '7': case '8': case '9':
							chars[0][i] = true;
							break;
						default:
							chars[0][i] = false;
							break;
					}
					switch (Input[i])
					{
						case '0': case '1': case '2': case '3': case '4': case '5': case '6': case '7': case '8': case '9':
							chars[1][i] = true;
							break;
						default:
							chars[1][i] = false;
							break;
					}
				}

				//compare bools of chars[][]
				bool rtn = true;
				for (byte i = 0; i < Format.Length && rtn; i++)
					rtn = chars[0][i] == chars[1][i];

				if (rtn)
					//compare non-numeric chars
					for (byte i = 0; i < Format.Length && rtn; i++)
						if (!chars[0][i])
							rtn = Input[i] == Format[i];

				return rtn;
			} else
				return false;
		}
		/* public static bool FollowsFormat(this string Input, IList<string> Format, bool XOR)
		{
			bool rtn = false;
			bool brk = false;
			foreach (string term in Format)
			{
				rtn = FollowsFormat(Input, Format);
				if (XOR) brk = !rtn; // rtn==false: brk=true
			}
			return rtn;
		}
		public static bool FollowsFormat(this string Input, IList<string> Format)
		{
			return FollowsFormat(Input, Format, false);
		} */

		//FollowsFormat(string input, string format) //"a##:##b:##.##"
		//{
		//	  array of booleans (whether or not format[idx] is a number
		//	  1) input.Length == format.Length
		//	  2) array(input) == array(format)
		//	  3) array(input)[false] == array(format)[false] <--non-number chars match (as opposed to "*##:##_@##-##" (dont check if test 2 failed)
		//}
	} //</class>
}
