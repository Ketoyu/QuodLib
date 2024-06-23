//using System.Text;
//using System.Threading.Tasks;

//using StringCollection = System.Collections.ObjectModel.Collection<string>;

namespace QuodLib.Strings {
    public static partial class Query {
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
            public StringQuery Copy(bool preserveData = false, bool preserveInclusive = true) {
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

    }
}
