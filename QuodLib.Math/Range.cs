//using System.Text;
//using System.Threading.Tasks;

using System.Text;

namespace QuodLib.Math {

    /// <summary>
    /// A simple range, defining low- and high-ends (i.e. 0 or 100) and inclusiveness (i.e. ( or [ {&lt; vs &lt;=}, ) or ] {&gt; vs &gt;=}).
    /// </summary>
    public struct Range<T> where T : IComparable
	{
		public T Low;
		public T High;
		public bool LowInclusive;
		public bool HighInclusive;
		#region Constructors
		/// <summary>
		/// Creates a new instance of Range, with "sbyte" as its primary data type.
		/// </summary>
		/// <param name="low">The range's minumum.</param>
		/// <param name="lowinc">Whether the range includes its minimum ( (min, max} vs [min, max} ).</param>
		/// <param name="high">The range's maximum.</param>
		/// <param name="highinc">Whether the range includes its minimum ( {min, max) vs {min, max] ).</param>
		public Range(T low, bool lowinc, T high, bool highinc)
		{
			if (!Types.IsNumeric(low.GetType())) throw new Exception("Generic T must be a number.");
			Low = low;
			High = high;
			LowInclusive = lowinc;
			HighInclusive = highinc;
		}
		/// <summary>
		/// Creates a new instance of Range, with "sbyte" as its primary data type.
		/// </summary>
		/// <param name="low">The range's minumum.</param>
		/// <param name="high">The range's maximum.</param>
		public Range(T low, T high) : this(low, true, high, true) { }
		/// <summary>
		/// Creates a new instance of Range, with "sbyte" as its primary data type.
		/// </summary>
		/// <param name="low">The range's minumum.</param>
		/// <param name="high">The range's maximum.</param>
		/// <param name="inclusive">Whether the range includes its extremes ( (min, max) vs [min, max] ).</param>
		public Range(T low, T high, bool inclusive) : this(low, inclusive, high, inclusive) { }
		/// <summary>
		/// Creates a new instance of range, with only one number as its bounds [m,m].
		/// </summary>
		/// <param name="exact">Both the minimum and maximum of the range (inclusive).</param>
		public Range(T exact) : this(exact, true, exact, true) { }
		#endregion //Constructors
		#region Functions
		/// <summary>
		/// Outputs a mathematical range (i.e. (x,y] ) depending on the range's defined bounds and inclusiveness.
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			StringBuilder sb = new();
			sb.Append(LowInclusive ? '[' : '(');
			sb.Append($"{Low},{High}");
            sb.Append(HighInclusive ? ']' : ')');

			return sb.ToString();
		}
		/// <summary>
		/// Returns whether the provided Value falls within the Range.
		/// </summary>
		/// <param name="Value">The value being tested for.</param>
		/// <returns></returns>
		public bool Contains(T Value)
		{
			bool rtn = true;
			if (Value.CompareTo(Low) == -1) rtn = false;
			if (Value.CompareTo(Low ) == 0 && !LowInclusive) rtn = false;
			if (Value.CompareTo(High) == 1) rtn = false;
			if (Value.CompareTo(High) == 0 && !HighInclusive) rtn = false;
			return rtn;
		}
		/// <summary>
		/// Detects whether the value is within a given range's bounds.
		/// </summary>
		/// <param name="rg"></param>
		/// <param name="Value"></param>
		/// <returns></returns>
		private static bool Contains(Range<T> rg, T Value)
		{
			bool rtn = true;
			if (Value.CompareTo(rg.Low) == -1) rtn = false;
			if (Value.CompareTo(rg.Low) == -1 && !rg.LowInclusive) rtn = false;
			if (Value.CompareTo(rg.High) == 1) rtn = false;
			if (Value.CompareTo(rg.High) == 0 && !rg.HighInclusive) rtn = false;
			return rtn;
		}
		/// <summary>
		/// Accepts a Range[] collection and outputs an int based on the indexed location of the first one containing the provided value. Returns -1 if the value is not contained at all.
		/// </summary>
		/// <param name="rgs">A given array of ranges.</param>
		/// <param name="Value">The value being tested for.</param>
		/// <returns></returns>
		public static int Ranges_FallsWithin(Range<T>[] rgs, T Value)
		{
			int rtn = -1;
			for (uint i = 0; i < rgs.Count() && rtn == -1; i++)
				if (Contains(rgs[i], Value)) rtn = (int)i;

			return rtn;
		}
		#endregion //Functions
	}
}
