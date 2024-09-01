using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using Rectangle = System.Drawing.Rectangle;
using Point = System.Drawing.Point;

namespace QuodLib.Math {
    /// <summary>
    /// A class 'classMath' of helpful mathematical functions not found in the list of built-in mathematical functions in C#;
    /// Boolean IntIs(Double 'num'), Integer IntRound(Double 'num'), Boolean IntIs(Double 'num'), Dictionary(Of Integer, Integer) IntDivis(Integer 'num'),
    /// Double ABS(Double 'num'), Single numNegIs_Sng(Double 'num), Boolean numNegIs_Bool(Double 'num'), Integer IntRandom(Integer 'num', Integer 'num2'),
    /// Double CDouble(String 'str'), and String Ad0(Integer 'num', Integer 'num0s').
    /// </summary>
    /// <remarks></remarks>
    public static class General
	{
		#region Trig
		/// <summary>
		/// Converts Cartesian coordinates (x,y) to polar (magnitude, angle), using the pythagorean theorem for the magnitude (hypoteneus) and arctan for the angle (in radians).
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static double[] Coords_ToPolar(double x, double y)
		{
			return new double[] {
                System.Math.Sqrt(System.Math.Pow(x, 2) + System.Math.Pow(y, 2)), //Magnitude (hypoteneus)
				System.Math.Atan(y/ x) //angle
			};
		}
		/// <summary>
		/// Converts polar coordinates (magnitude, angle) to Cartesian (x, y) by multiplying the magnitude by cosine(th) and sine(th) respectively.
		/// </summary>
		/// <param name="m">The magnitude o the polar coordinates.</param>
		/// <param name="th">The angle of the polar coordinates, in radians.</param>
		/// <returns></returns>
		public static double[] Coords_FromPolar(double m, double th)
		{
			return new double[] {
                System.Math.Cos(th) * m,
                System.Math.Sin(th) * m
			};
		}
		/// <summary>
		/// Converts degrees to radians by multiplying by (pi / 180).
		/// </summary>
		/// <param name="deg">The angle, in degrees.</param>
		/// <returns></returns>
		public static double DegToRad(double deg)
		{
			return deg * System.Math.PI / 180;
		}
		/// <summary>
		/// Converts radians to degrees by multiplying by (180 / pi).
		/// </summary>
		/// <param name="rad">The andlge, in radians.</param>
		/// <returns></returns>
		public static double RadToDeg(double rad)
		{
			return rad * 180 / System.Math.PI;
		}
		#endregion //Trig

		public static double Square(double scalar)
			=> System.Math.Pow(scalar, 2);

        public static double Cube(double scalar)
            => System.Math.Pow(scalar, 3);

        /// <summary>
        /// Processes a small unit and returns it as part itself, part higher unit.
        /// </summary>
        /// <param name="value">The value to process.</param>
        /// <param name="limit">The value at which to carry.</param>
        /// <returns></returns>
        public static (int SmallValue, int LargeValue) RollOver(int value, int limit) {
			int smaller = value % limit;
			int larger = (value - smaller) / limit;
			return (smaller, larger);
        }

		/// <summary>
		/// The percent-error of <paramref name="value"/>, compared to <paramref name="target"/>.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static double PercentError(this double value, double target)
			=> (System.Math.Abs(target - value) / System.Math.Abs(target)) * 100;

		/// <summary>
		/// Whether <paramref name="num"/> is a whole number.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		/// <remarks></remarks>
		public static bool IsWhole(this double num)
			=> System.Math.Floor(num) == num;

        /// <summary>
		/// Whether <paramref name="num"/> is a whole number.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsWhole(this float num)
            => System.Math.Floor(num) == num;

        /// <summary>
		/// Whether <paramref name="num"/> is a whole number.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsWhole(this decimal num)
			=> System.Math.Floor(num) == num;

        /// <summary>Returns a random integer that is within a specified <see cref="Range"/>.</summary>
        /// <param name="range">The bounds of the random number returned.</param>
		/// <param name="endInclusive">Whether to include the <see cref="Range.End"/> <i>(default <see cref="false"/>)</i>.</param>
        /// <returns>
        /// A 32-bit signed integer within the provided <paramref name="range"/>. If the <see cref="Range.Start"/> euqals the <see cref="Range.End"/> are equal, returns the <see cref="Range.Start"/>.
        /// </returns>
		public static int Next(this Random rand, Range range, bool endInclusive = false)
			=> range.Start.Value != range.End.Value
				? rand.Next(range.Start.Value, range.End.Value + (endInclusive ? 1 : 0))
				: range.Start.Value;

        /// <summary>
        /// Returns a rounded Integer value of the Double 'num'.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int RInt(double num)
			=> (num - System.Math.Floor(num)) >= 0.5
				? (int)System.Math.Floor(num)
				: (int)System.Math.Ceiling(num);

        /// <summary>
        /// Returns a Integer value, rounded up from the Double 'num'.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int CInt(double num)
			=> (int)System.Math.Ceiling(num);

        /// <summary>
        /// Returns a Integer value, rounded down from the Double 'num'.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int FInt(double num)
			=> (int)System.Math.Floor(num);

        /// <summary>
        /// Returns a Long value, rounded down from the Decimal 'num'.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static long FLng(decimal num)
			=> (long)System.Math.Floor(num);

        //TODO: remove all Divis methods in favor of # % div. ~~~~~~~~

        /// <summary>
        /// Returns a Boolean value of whether the Long 'num' is evenly divisible by the Long 'qt'.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="qt"></param>
        /// <returns></returns>
        public static bool Divis(long num, long qt)
		{
			//return (num % qt != qt);
			return (num % qt == 0);
		}

		/// <summary>
		/// Returns a Boolean value of whether the ULong 'num' is evenly divisible by the ULong 'qt'.
		/// </summary>
		/// <param name="num"></param>
		/// <param name="qt"></param>
		/// <returns></returns>
		public static bool Divis(ulong num, ulong qt)
		{
			return (num % qt == 0);
		}
		#region Divis_overloads
		/// <summary>
		/// Returns a Boolean value of whether the SByte 'num' is evenly divisible by the SByte 'qt'.
		/// </summary>
		/// <param name="num"></param>
		/// <param name="qt"></param>
		/// <returns></returns>
		public static bool Divis(sbyte num, sbyte qt)
		{
			return (num % qt == 0);
		}

		/// <summary>
		/// Returns a Boolean value of whether the Byte 'num' is evenly divisible by the Byte 'qt'.
		/// </summary>
		/// <param name="num"></param>
		/// <param name="qt"></param>
		/// <returns></returns>
		public static bool Divis(byte num, byte qt)
		{
			return (num % qt == 0);
		}

		/// <summary>
		/// Returns a Boolean value of whether the Short 'num' is evenly divisible by the Short 'qt'.
		/// </summary>
		/// <param name="num"></param>
		/// <param name="qt"></param>
		/// <returns></returns>
		public static bool Divis(short num, short qt)
		{
			return (num % qt == 0);
		}

		/// <summary>
		/// Returns a Boolean value of whether the UShort 'num' is evenly divisible by the UShort 'qt'.
		/// </summary>
		/// <param name="num"></param>
		/// <param name="qt"></param>
		/// <returns></returns>
		public static bool Divis(ushort num, ushort qt)
		{
			return (num % qt == 0);
		}

		/// <summary>
		/// Returns a Boolean value of whether the Integer 'num' is evenly divisible by the Integer 'qt'.
		/// </summary>
		/// <param name="num"></param>
		/// <param name="qt"></param>
		/// <returns></returns>
		public static bool Divis(int num, int qt)
		{
			return (num % qt == 0);
		}

		/// <summary>
		/// Returns a Boolean value of whether the UInteger 'num' is evenly divisible by the UInteger 'qt'.
		/// </summary>
		/// <param name="num"></param>
		/// <param name="qt"></param>
		/// <returns></returns>
		public static bool Divis(uint num, uint qt)
		{
			return (num % qt == 0);
		}

		/// <summary>
		/// Returns a Boolean value of whether the Double 'num' is evenly divisible by the Double 'qt' and holds no more than the higher number decimal places.
		/// </summary>
		/// <param name="num"></param>
		/// <param name="qt"></param>
		/// <returns></returns>
		public static bool Divis(float num, float qt)
		{
			return ((num % qt != qt) && (("" + (num % qt)).Length == System.Math.Max(Flt_CountDecimals(num), Flt_CountDecimals(qt))));
		}

		/// <summary>
		/// Returns a Boolean value of whether the Double 'num' is evenly divisible by the Double 'qt' and holds no more than the higher number decimal places.
		/// </summary>
		/// <param name="num"></param>
		/// <param name="qt"></param>
		/// <returns></returns>
		public static bool Divis(double num, double qt)
		{
			return ( (num % qt != qt) && ( ("" + (num % qt)).Length == System.Math.Max(Dbl_CountDecimals(num), Dbl_CountDecimals(qt)) ) );
		}

		#endregion //Divis_overloads

		/// <summary>
		/// Counts the number of digits past the decimal point of <paramref name="num"/>.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static uint Flt_CountDecimals(float num)
		{
			string sFlt = "" + num;
			uint whole = 0;
			if (!sFlt.Contains("."))
			{
				sFlt += ".0";
				whole = 1;
			}
			return (uint)(sFlt.Split(".", 1).Length - whole);
		}
		
		public static uint Dbl_CountDecimals(double num)
		{
			string sDbl = "" + num;
			uint whole = 0;
			if (!sDbl.Contains("."))
			{
				sDbl += ".0";
				whole = 1;
			}
			return (uint)(sDbl.Split(".", 1).Length - whole);
		}

		/// <summary>
		/// Returns a Boolean value of whether the given number consists of only "1" followed by several "0"s.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool PowerTenIs(ulong num)
		{
			bool not = false;
			for (int i = 0; (i < ("" + num).Length) && !(not == true); i++) {
				if (i == 0)
					if (num == 0) return false;
				else
					if ( ("" + num)[i] != '0' ) not = true;
			}
			return !not;
		}

		/// <summary>
		/// Returns a Boolean value of whether the given number consists of only "1" followed by several "0"s.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool PowerTenIs(long num)
		{
			return PowerTenIs((ulong)System.Math.Abs(num));
		}
		#region PowerTenIs_overloads
		/// <summary>
		/// Returns a Boolean value of whether the given number consists of only "1" followed by several "0"s.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool PowerTenIs(sbyte num)
		{
			return PowerTenIs((long)num);
		}
		/// <summary>
		/// Returns a Boolean value of whether the given number consists of only "1" followed by several "0"s.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool PowerTenIs(byte num)
		{
			return PowerTenIs((ulong)num);
		}
		/// <summary>
		/// Returns a Boolean value of whether the given number consists of only "1" followed by several "0"s.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool PowerTenIs(short num)
		{
			return PowerTenIs((long)num);
		}
		/// <summary>
		/// Returns a Boolean value of whether the given number consists of only "1" followed by several "0"s.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool PowerTenIs(ushort num)
		{
			return PowerTenIs((ulong)num);
		}
		/// <summary>
		/// Returns a Boolean value of whether the given number consists of only "1" followed by several "0"s.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool PowerTenIs(int num)
		{
			return PowerTenIs((long)num);
		}
		/// <summary>
		/// Returns a Boolean value of whether the given number consists of only "1" followed by several "0"s.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool PowerTenIs(uint num)
		{
			return PowerTenIs((ulong)num);
		}
		#endregion //PowerTenIs_overloads

		/// <summary>
		/// Returns an int[1] {top, bottom} of the reduced fraction 'top' / 'bottom', or {top, bottom, -1} if the fraction could not be reduced.
		/// </summary>
		/// <param name="top"></param>
		/// <param name="bottom"></param>
		/// <returns></returns>
		public static long[] Reduce(long top, long bottom)
		{
			ulong[] rtn = Reduce((ulong)System.Math.Abs(top), (ulong)System.Math.Abs(bottom));
			return new long[] { (long)rtn[0] * NegIs_SByte(top), (long)rtn[1] * NegIs_SByte(bottom) };
		}

		/// <summary>
		/// Returns an int[1] {top, bottom} of the reduced fraction 'top' / 'bottom', or {top, bottom, -1} if the fraction could not be reduced.
		/// </summary>
		/// <param name="top"></param>
		/// <param name="bottom"></param>
		/// <returns></returns>
		public static ulong[] Reduce(ulong top, ulong bottom)
		{
			ulong gcd = GCD(top, bottom);
			return new ulong[] {top / gcd, bottom / gcd };
		}

		/*public static ulong[] Reduce(ulong top, ulong bottom)
		{
			ulong[] rtn = new ulong[2];
			rtn[0] = top; rtn[1] = bottom;
			if (bottom == 1)
				return rtn;
			else if (top == bottom) {
				rtn = new ulong[] { 1, 1 };
				return rtn;
			} else {
				if (Divis(top, bottom))
					rtn = new ulong[] { (ulong)(top / bottom), 1};
				else {
					if (Divis(bottom, top))
						return new ulong[] { 1, (ulong)(bottom / top) };
					else {
						//<Non-divisible; shared factors>
						ulong gfc = 0;
						bool gfc_set = false;
						ulong top_ = top;
						ulong bottom_ = bottom;
						if (top > bottom) {
							for (ulong i = 2; i < top_; i++) {
								//If the denominator is a power of ten and the numerator is not divisible by 2 and 5,
								//	the fraction cannot be reduced. ;_;
								//If it ain't divisible by anything between 1 and 10 (inclusive), it ain't gonna be divisible by a multiple of
								//	anything between 1 and 10 (inclusive). Try running through a list of prime numbers after 1-10 haven't worked.
								//Try to write an algorithm that increments an inputted number by something that doesn't lead to a multiple of an
								//	inputted array of factors?
								//		- keep a variable for the index of the array to increment relative to (if 2 is a listed factor
								//			  and 3 isn't, increment by 3)
								// Try testing higher powers of a number. Ex: Test {2, 4, 8, etc., then 3, 9, 27, etc., ...}.
								if (Divis(top_, i) && Divis(bottom_, i)) {
									if (!gfc_set) {
										gfc = i;
										gfc_set = true;   
									} else
										gfc *= i;

									top_ /= i;
									bottom_ /= i;
									i = 1;
								}
							}
						} else { // top < bottom
							for (ulong i = 2; i < bottom_; i++) {
								if (Divis(bottom_, i) && Divis(top_, i)) {
									if (gfc_set) {
										gfc = i;
										gfc_set = true;
									} else
										gfc *= i;
									
									top_ /= i;
									bottom_ /= i;
								}
							}
						}
						if (gfc_set)
							rtn = new ulong[] { (ulong)(top / gfc), (ulong)(bottom / gfc) };
						//</Non-divisible; shared factors>
					}
				}
			}
			return rtn;
		}*/

		#region Reduce_overloads
		/// <summary>
		/// Returns an int[1] {top, bottom} of the reduced fraction 'top' / 'bottom', or {top, bottom, -1} if the fraction could not be reduced.
		/// </summary>
		/// <param name="top"></param>
		/// <param name="bottom"></param>
		/// <returns></returns>
		public static sbyte[] Reduce(sbyte top, sbyte bottom)
		{
			long[] red = Reduce((long)top, (long)bottom);
			return new sbyte[] { (sbyte)(red[0]), (sbyte)(red[1]) };
		}
		
		/// <summary>
		/// Returns an int[1] {top, bottom} of the reduced fraction 'top' / 'bottom', or {top, bottom, -1} if the fraction could not be reduced.
		/// </summary>
		/// <param name="top"></param>
		/// <param name="bottom"></param>
		/// <returns></returns>
		public static byte[] Reduce(byte top, byte bottom)
		{
			ulong[] red = Reduce((ulong)top, (ulong)bottom);
			return new byte[] { (byte)(red[0]), (byte)(red[1]) };
		}

		/// <summary>
		/// Returns an int[1] {top, bottom} of the reduced fraction 'top' / 'bottom', or {top, bottom, -1} if the fraction could not be reduced.
		/// </summary>
		/// <param name="top"></param>
		/// <param name="bottom"></param>
		/// <returns></returns>
		public static short[] Reduce(short top, short bottom)
		{
			long[] red = Reduce((long)top, (long)bottom);
			return new short[] { (short)(red[0]), (short)(red[1]) };
		}

		/// <summary>
		/// Returns an int[1] {top, bottom} of the reduced fraction 'top' / 'bottom', or {top, bottom, -1} if the fraction could not be reduced.
		/// </summary>
		/// <param name="top"></param>
		/// <param name="bottom"></param>
		/// <returns></returns>
		public static ushort[] Reduce(ushort top, ushort bottom)
		{
			ulong[] red = Reduce((ulong)top, (ulong)bottom);
			return new ushort[] { (ushort)(red[0]), (ushort)(red[1]) };
		}
		
		public static int[] Reduce(int top, int bottom)
		{
			long[] red = Reduce((long)top, (long)bottom);
			return new int[] { (int)(red[0]), (int)(red[1]) };
		}

		/// <summary>
		/// Returns an int[1] {top, bottom} of the reduced fraction 'top' / 'bottom', or {top, bottom, -1} if the fraction could not be reduced.
		/// </summary>
		/// <param name="top"></param>
		/// <param name="bottom"></param>
		/// <returns></returns>
		public static uint[] Reduce(uint top, uint bottom)
		{
			ulong[] red = Reduce((ulong)top, (ulong)bottom);
			return new uint[] { (uint)(red[0]), (uint)(red[1]) };
		}
		#endregion //Reduce_overloads

		/// <summary>
		/// Returns a Dictionary&lt;int, int&gt; of the factors of the Integer 'num' and their pairs.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static Dictionary<int, int> Int_Factors(int num)
		{
			Dictionary<int, int> rtn = new Dictionary<int, int>();
			for (int i = 0; i <= System.Math.Abs(num); i++)
				if (Divis(num, i)) rtn.Add(i, num / i);

			return rtn;
		}

		/// <summary>
		/// Returns a Boolean value of whether the Integer 'num' is a negative number.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool Int_NegIs(int num)
		{
			return (num < 0);
		}

		/// <summary>
		/// -#, # } Returns "-" if the Integer 'num' is a negative number.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static string Int_NegIs_Str(int num)
		{
			return (num < 0 ? "-" : "");
		}

		/// <summary>
		/// -#, +# } Returns "-" if the Integer 'num' is a negative number or a '+' if it's positive.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static string Int_NegIs_Str_Plus(int num)
		{
			return (num < 0 ? "-" : "+");
		}

		/// <summary>
		/// -1, 1 } Returns -1 if the Long 'num' is a negative number or a 1 if it's positive.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static sbyte NegIs_SByte(long num)
		{
			if (num < 0) return -1;
				else return 1;
		}
		#region NegIs_SByte
		/// <summary>
		/// -1, 1 } Returns -1 if the SByte 'num' is a negative number or a 1 if it's positive.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static sbyte NegIs_SByte(sbyte num)
		{
			if (num < 0) { return -1; } else { return 1; }
		}

		/// <summary>
		/// -1, 1 } Returns -1 if the Short 'num' is a negative number or a 1 if it's positive.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static sbyte NegIs_SByte(short num)
		{
			if (num < 0) { return -1; } else { return 1; }
		}

		/// <summary>
		/// -1, 1 } Returns -1 if the Integer 'num' is a negative number or a 1 if it's positive.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static sbyte NegIs_SByte(int num)
		{
			if (num < 0) { return -1; } else { return 1; }
		}
		#endregion //NegIs_SByte
		
		/// <summary>
		/// Returns a Boolean value of whether the Long 'num' is evenly divisible by two.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool num_IsEven(long num)
		{
			return Divis(num, 2);
		}

		/// <summary>
		/// Returns a Boolean value of whether the ULong 'num' is evenly divisible by two.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool num_IsEven(ulong num)
		{
			return Divis(num, 2);
		}
		#region num_IsEven_overloads

		/// <summary>
		/// Returns a Boolean value of whether the Byte 'num' is evenly divisible by two.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool num_IsEven(sbyte num)
		{
			return Divis(num, (sbyte)2);
		}

		/// <summary>
		/// Returns a Boolean value of whether the SByte 'num' is evenly divisible by two.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool num_IsEven(byte num)
		{
			return Divis(num, (byte)2);
		}

		/// <summary>
		/// Returns a Boolean value of whether the Short 'num' is evenly divisible by two.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool num_IsEven(short num)
		{
			return Divis(num, (short)2);
		}

		/// <summary>
		/// Returns a Boolean value of whether the UShort 'num' is evenly divisible by two.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool num_IsEven(ushort num)
		{
			return Divis(num, (ushort)2);
		}

		/// <summary>
		/// Returns a Boolean value of whether the Integer 'num' is evenly divisible by two.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool num_IsEven(int num)
		{
			return Divis(num, 2);
		}

		/// <summary>
		/// Returns a Boolean value of whether the UInteger 'num' is evenly divisible by two.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool num_IsEven(uint num)
		{
			return Divis(num, 2);
		}

		/// <summary>
		/// Returns a Boolean value of whether the Float 'num' is evenly divisible by two.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool num_IsEven(float num)
		{
			return Divis(num, (float)2);
		}

		/// <summary>
		/// Returns a Boolean value of whether the Double 'num' is evenly divisible by two.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool num_IsEven(double num)
		{
			return Divis(num, 2);
		}
		#endregion

		private static bool IsInBounds_(double num, double low, double high)
		{
			return !((num < low) || (num > high));
		}
		#region IsInBounds_overloads
		public static bool IsInBounds(int num, int low, int high)
		{
			return !((num < low) || (num > high));
		}
		public static bool IsInBounds(uint num, uint low, uint high)
		{
			return !((num < low) || (num > high));
		}
		public static bool IsInBounds(byte num, byte low, byte high)
		{
			return !((num < low) || (num > high));
		}
		public static bool IsInBounds(sbyte num, sbyte low, sbyte high)
		{
			return !((num < low) || (num > high));
		}
		public static bool IsInBounds(long num, long low, long high)
		{
			return !((num < low) || (num > high));
		}
		public static bool IsInBounds(ulong num, ulong low, ulong high)
		{
			return !((num < low) || (num > high));
		}
		public static bool IsInBounds(float num, float low, float high)
		{
			return !((num < low) || (num > high));
		}
		public static bool IsInBounds(double num, double low, double high)
		{
			return !((num < low) || (num > high));
		}
		#endregion //IsInBounds_overloads

		private static double Limit_(double num, double low, double high)
		{
			if (num < low) return low;
				else if (num > high) return high;
				else return num;	
		}
		public static double Limit(double num, double low, double high)
		{
			return Limit_(num, low, high);
		}
		#region Limit overloads ( (s)byte, uint, double )
		public static sbyte Limit(sbyte num, sbyte low, sbyte high)
		{
			return (sbyte)(Limit_(num, low, high));
		}
		public static byte Limit(byte num, byte low, byte high)
		{
			return (byte)(Limit_(num, low, high));
		}
		public static uint Limit(uint num, uint low, uint high)
		{
			return (uint)(Limit_(num, low, high));
		}
		public static int Limit(int num, int low, int high)
		{
			return (int)(Limit_(num, low, high));
		}
		#endregion //Limit overloads

		public static double Limit_L(double num, double low)
		{
			return Limit_(num, low, num);
		}
		#region Limit_L overloads ( (s)byte, uint, double )
		public static sbyte Limit_L(sbyte num, sbyte low)
		{
			return (sbyte)(Limit_(num, low, num));
		}
		public static byte Limit_L(byte num, byte low)
		{
			return (byte)(Limit_(num, low, num));
		}
		public static uint Limit_L(uint num, uint low)
		{
			return (uint)(Limit_(num, low, num));
		}
		public static int Limit_L(int num, int low)
		{
			return (int)(Limit_(num, low, num));
		}
		#endregion //Limit overloads

		public static double Limit_H(double num, double high)
		{
			return Limit_(num, num, high);
		}
		#region Limit_H overloads ( (s)byte, uint, double )
		public static sbyte Limit_H(sbyte num, sbyte high)
		{
			return (sbyte)(Limit_(num, num, high));
		}
		public static byte Limit_H(byte num, byte high)
		{
			return (byte)(Limit_(num, num, high));
		}
		public static uint Limit_H(uint num, uint high)
		{
			return (uint)(Limit_(num, num, high));
		}
		public static int Limit_H(int num, int high)
		{
			return (int)(Limit_(num, num, high));
		}
		#endregion //Limit overloads

		/// <summary>
		/// Returns the Double sum of a List(string) 'nums'.
		/// </summary>
		/// <param name="nums"></param>
		/// <returns></returns>
		public static double listSum(List<string> nums)
		{
			double rtn = 0;
			foreach (string ln in nums) {
				string dbl_;
				dbl_ = ln.Replace(" ", "");
				dbl_ = dbl_.Replace(",", "");
				rtn += double.Parse(dbl_);
			}
			return rtn;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="accuracy"></param>
		/// <returns></returns>
		public static (Fraction Fraction, bool Exact) ToFraction(this double value, byte accuracy)
		{
			value = System.Math.Round(value, accuracy);
			if ((value == 0) || (value == 1)) {
				if (value == 0) return (new Fraction(0, 1), true);
					else return (new Fraction(1, 1), true);
			} else {
				Fraction rtn = new();

				sbyte neg = (value >= 0
					? (sbyte) 1
					: (sbyte)-1);

				double rounded_out;
				for (ulong i = 0; (i < System.Math.Pow(2, 64)) && !(System.Math.Round(neg * rtn.Value, accuracy) == value); i++) {
					rounded_out = System.Math.Round(rtn.Value, accuracy);
					if (rounded_out > value) rtn.Denominator ++;
						else rtn.Numerator++;
				}

				return (rtn,
                    System.Math.Round(rtn.Value, accuracy) == value);
			}
		}

		/// <summary>
		/// Approximates the <paramref name="value"/> as a combination of the <paramref name="options"/> provided.
		/// </summary>
		/// <param name="value">The decimal value, to approximate.</param>
		/// <param name="options">The <see cref="List{Fraction}"/> of available fractions to approximate the <paramref name="value"/>.</param>
		/// <returns></returns>
		/// <remarks>Currently, this function is intended for positive values/fractions only.</remarks>
		/// <exception cref="NotImplementedException"></exception>
		public static (List<Fraction> Fractions, double PercentError) ToFractions(this double value, List<Fraction> options) {
			//Biggest first (by copying)
			options = options.OrderByDescending(a => a.Value).ToList();
			double val_todo = value;

			//Perform selection
			List<Fraction> rtn = new();
			while (options.Count > 0) { //(while instead of for, to empty the list as it goes)
				var f_i = options[0];
				var v_i = f_i.Value; //to avoid re-computing

				int test = (int)System.Math.Floor(val_todo / v_i); //X times (floored) that options[0] fits into value.
				if (test > 1) { //if (more than 1)

					//Use a multiple of f_i.
					var f_mult = f_i with { Numerator = f_i.Numerator * test };
					var f_mult_v = f_mult.Value;

					//if (last option) and (1 more would be closer), use 1 more & break.
					if (options.Count == 1 && chooseBreak(f_mult, f_mult_v, f_mult with { Numerator = f_mult.Numerator + f_i.Numerator }, null))
						break;

					//(else), add to list.
					rtn.Add(f_mult);
					val_todo -= f_mult_v;
				//e'if (execatly 1), add to list.
                } else if (test == 1) {
					//if (last option) and (1 more would be closer), use 1 more & break.
					if (options.Count == 1 && chooseBreak(f_i, v_i, f_i with { Numerator = f_i.Numerator * 2 }, null))
						break;
					//else,
					rtn.Add(f_i);
					val_todo -= v_i;

					//e'if (f doesn't fit) && (penultimate f) && (2*f_j >= f_i) && ((re-use f) is closer than (use next)), use f instead & break.
				} else if (test == 0 && options.Count == 2 && (2 * options[1].Value) >= v_i
						&& chooseBreak(options[1], null, f_i, v_i))
					break;
				

				//Move to next option.
				options.RemoveAt(0);

                #region localFunc
				//If alt is closer than pri_, adds alt and prepares to end the loop.
                bool chooseBreak(Fraction pri_, double? pri_v, Fraction alt, double? alt_v) {
					pri_v ??= pri_.Value;
					alt_v ??= alt.Value;

					if (alt_v?.PercentError(val_todo) < pri_v?.PercentError(val_todo)) {
						//re-use f & break.
						rtn.Add(alt);
						options.Clear();
						return true;
					}

					return false;
				}
				#endregion //localFunc
			}

			return (rtn, 
				rtn.Sum(f => f.Value)
					.PercentError(value)
				);
        }

		#region Pascal's Triangle
			#region Factorial
			/// <summary>
			/// Returns a Integer factoral (n!). FTL(4) = 4 * 3 * 2 * 1, FTL(8) = 8 * 7 * 6 * 5 * 4 * 3 * 2 * 1, etc.
			/// </summary>
			/// <param name="n"></param>
			/// <returns></returns>
			/// <remarks></remarks>
			public static int FTL(int n)
			{
				int rtn = 1;
				if (n != 0)
				{
					for (int i = 1; i <= n; i++)
					{
						rtn *= i;
					}
				}
				#region Data type overflow
				if (rtn < 0)
				{
					//classStrings.Int_AddCommas(___)
					List<int> lFct = FTL_L(n);
					string strRtn = $"{rtn}";
					string sFct = "";
					for (int t = 0; t < lFct.Count; t++)
					{
						if (t != 0)
						{
							sFct = "" + sFct + ", ";
						}
						sFct = "" + sFct + lFct[t];
					}
					string excp = "" + (char)34 + n + "!" + (char)34 + " returned a negative result " + (char)39 + strRtn + (char)39 +
						", accompanying factors: \n[" + sFct + "]. \nThe product of these factors may be past the Integer maximum of 2,147,473,647.";
					throw new Exception(excp);
				}
				#endregion //Data type overflow
				return rtn;
			}

			/// <summary>
			/// Returns a List(Integer) of factors in factoral (n!). FTL(4) returns (4, 3, 2, and 1); FTL(8) returns (8, 7, 6, 5, 4, 3, 2, and 1); etc.
			/// </summary>
			/// <param name="n"></param>
			/// <returns></returns>
			/// <remarks></remarks>
			public static List<int> FTL_L(int n)
			{
				List<int> rtn = new List<int>();
				if (!(n == 0))
				{
					for (int i = 1; i <= n; i++)
					{
						rtn.Add(i);
					}
				} else {
					rtn.Add(1);
				}
				return rtn;
			}
			//public static List<long> FTL_L(long n)
			//{
			//	List<long> rtn = new List<long>();
			//}

			/// <summary>
			/// Returns a Integer factorial (n!), ending after iteration 'n_'. FTL(4, 2) = 4 * 3 * 2, FTL(8, 5) = 8 * 7 * 6 * 5, etc.
			/// </summary>
			/// <param name="n"></param>
			/// <param name="n_"></param>
			/// <returns></returns>
			/// <remarks></remarks>
			public static int FTL_cut(int n, int n_)
			{
				int rtn = 1;
				if ((!(n == 0)) && (!(n_ == 0)))
				{
					if (n == n_)
					{
						rtn = n;
					} else {
						for (int i = n_; i <= n; i++)
						{
							rtn *= i;
						}
					}
				}
				#region Data Type overflow
				if (rtn < 0)
				{
					//classStrings.Int_AddCommas(___)
					List<int> lFct = FTL_cut_L(n, n_);
					string strRtn = $"{rtn}";
					string sFct = "";
					for (int t = 0; t < lFct.Count; t++)
					{
						if (t != 0)
						{
							sFct = "" + sFct + ", ";
						}
						sFct = "" + sFct + lFct[t];
					}
					string excp = $"{(char)34}{n}! stop({n_}) {(char)34} returned a negative result {(char)39}{strRtn}{(char)39}" +
						$", accompanying factors: \n[{sFct}]. \nThe product of these factors may be past the Integer maximum of {int.MaxValue}.";
					throw new Exception(excp);
				}
				#endregion //Data type overflow
				return rtn;
			}

			/// <summary>
			/// Returns a List(Integer) of factors in factoral (n!), ending after iteration 'n_'. FTL(4, 2) returns (4, 3, and 2);
			/// FTL(8, 5) returns (8, 7, 6, and 5); etc.
			/// </summary>
			/// <param name="n"></param>
			/// <param name="n_"></param>
			/// <returns></returns>
			/// <remarks></remarks>
			public static List<int> FTL_cut_L(int n, int n_)
			{
				List<int> rtn = new List<int>();
				if ((!(n == 0)) && (!(n_ == 0)))
				{
					if (n == n_)
					{
						rtn.Add(n);
					} else {
						for (int i = n_; i <= n; i++)
						{
							rtn.Add(i);
						}
					}
				} else {
					rtn.Add(1);
				}
				return rtn;
			}
			#endregion //Factorial

			#region Choose
			/// <summary>
			/// Returns an Integer "n choose r". CHS(n, r) = n! / ((n - r)! * r!)
			/// </summary>
			/// <param name="n"></param>
			/// <param name="r"></param>
			/// <returns></returns>
			/// <remarks></remarks>
			public static int CHS(int n, int r) //Re-write function to implement simplification!
			{
				int rtn = 0;
				int top, bottom;

				if (r > (n - r))
				{
					top = FTL_cut(n, r);
					bottom = FTL(n - r) * r;
				} else {
					top = FTL_cut(n, n - r);
					bottom = FTL(r) * (n - r);
				}
				if (bottom == 0)
				{
					rtn = 1;
				} else {
					rtn = top / bottom;
				}
				return rtn;
			}

			//<requires tesitng>
			/// <summary>
			/// Returns an Integer "n choose r" after simplifying repeating factors; has increased size limit from "CHS". CHS(n, r) = n! / ((n - r)! * r!)
			/// </summary>
			/// <param name="n"></param>
			/// <param name="r"></param>
			/// <returns></returns>
			/// <remarks></remarks>
			public static int CHS_simp(int n, int r) //re-write of CHS
			{
				//(copied from QuodLib.Linq.Aggregate.cs, to avoid the project-dependency)
				Dictionary<int, int> groupCount(IEnumerable<int> source)
					=> source
						.GroupBy(t => t)
						.Select(t => new KeyValuePair<int, int>(t.Key, t.Count()))
						.ToDictionary(t => t.Key, t => t.Value);

                //int foo1 = 1; int foo2 = 9; foo1 -= 1; int foo3 = foo2 / foo1; //Throw an error for debugging purposes
                if (r == 0)
				{
					return 1;
				} else {
					//Get List(int) of factors from ( FTL_cut(n, r) and FTL(n - r) ) or ( FTL_cut(n, n - r) and FTL(r) )
					//	  [x] re-write List(int) variants of FTL and FTL_cut
					int rtn = 0;
					Dictionary<int, int> lTop, lBottom;
					List<int> lTop_, lBottom_;
					int top = 0, bottom = 0;

					#region getLists
					if (r > (n - r))
					{
						lTop = groupCount(FTL_cut_L(n, r));
						lBottom = groupCount(FTL_L(n - r));
						ValueCounts.IncrementOrAdd(lBottom, r);
					} else {
						lTop = groupCount(FTL_cut_L(n, n - r));
						lBottom = groupCount(FTL_L(r));
                    ValueCounts.IncrementOrAdd(lBottom, n - r);
					}
					lTop_ = ValueCounts.MultipleToList(lTop);
					lBottom_ = ValueCounts.MultipleToList(lBottom);
					#region Cancel factors
					foreach (int numT in lTop_)
					{
						if (lBottom.ContainsKey(numT))
						{
							lTop = ValueCounts.DecrementOrRemove(lTop, numT); if (lTop.Count == 0) { lTop.Add(1, 1); }
							lBottom = ValueCounts.DecrementOrRemove(lBottom, numT); if (lBottom.Count == 0) { lBottom.Add(1, 1); }
						}
					}
					#endregion //Cancel factors

					#region Reduce factors
					lTop_ = ValueCounts.MultipleToList(lTop);
					lBottom_ = ValueCounts.MultipleToList(lBottom);
					for (int numT = 0; numT < lTop_.Count; numT++)
					{
						for (int numB = 0; numB < lBottom_.Count; numB++)
						{
							int[] fs = Reduce(lTop_[numT], lBottom_[numB]);
							lTop_[numT] = fs[0]; lBottom_[numB] = fs[1];
						}
					}
					#endregion //Reduce factors

					#region Clear 1s
					lTop = groupCount(lTop_);
					lBottom = groupCount(lBottom_);
					for (int numT = 0; numT < lTop_.Count; numT++)
					{
						if (lTop_[numT] == 1)
						{
							if (lTop.ContainsKey(1))
							{
								if (lTop.Keys.Count > 1)
								{
									lTop.Remove(1);
								} else if (lTop.Count == 1) {
									if (lTop[1] > 1)
									{
										lTop.Clear();
										lTop.Add(1, 1);
									}
								}
							}
						}
					}
					for (int numB = 0; numB < lBottom_.Count; numB++)
					{
						if (lBottom_[numB] == 1)
						{
							if (lBottom.ContainsKey(1))
							{
								if (lBottom.Keys.Count > 1)
								{
									lBottom.Remove(1);
								} else if (lBottom.Count == 1) {
									if (lBottom[1] > 1)
									{
										lBottom.Clear();
										lBottom.Add(1, 1);
									}
								}
							}
						}
					}
					lTop_ = ValueCounts.MultipleToList(lTop);
					lBottom_ = ValueCounts.MultipleToList(lBottom);
					#endregion //Clear 1s
					#endregion //getLists

					#region multiply
					bool topSet = false, bottomSet = false;
					//lTop_ = classObjects.Dic_keys_toList(lTop);
					for (int numT = 0; numT < lTop_.Count; numT++)
					{
						if (topSet == false)
						{
							top = lTop_[numT]; topSet = true;
						} else {
							top *= lTop_[numT];
						}
					}
					//lBottom_ = classObjects.Dic_keys_toList(lBottom);
					for (int numB = 0; numB < lBottom_.Count; numB++)
					{
						if (bottomSet == false)
						{
							bottom = lBottom_[numB]; bottomSet = true;
						} else {
							bottom *= lBottom_[numB];
						}
					}
					#endregion //multiply

					if (bottom == 0) //handle bottom==0
					{
						rtn = 1;
					} else {
						rtn = top / bottom;
					}

					#region Detect type overflow
					if (rtn < 0)
					{
						//classStrings.Int_AddCommas(___)
						string strTop = $"{top}";
						string strBottom = $"{bottom}";
						string strRtn = $"{rtn}";
						string sTop = string.Empty;
						for (int t = 0; t < lTop_.Count; t++)
						{
							if (t != 0)
							{
								sTop = $"{sTop}, ";
							}
							sTop = $"{sTop}{lTop_[t]}";
						}
						string sBottom = "";
						for (int b = 0; b < lBottom_.Count; b++)
						{
							if (b != 0)
							{
								sBottom = "" + sBottom + ", ";
							}
							sBottom = "" + sBottom + lBottom_[b];
						}
						string excp = $"{(char)34}{n} choose {r}{(char)34} returned a negative result {(char)39}{strRtn}{(char)39} ({strTop} / {strBottom}"
							+ $"), accompanying factors: \n[{sTop}] / [{sBottom}]. \nThe product of these factors may be past the Integer maximum of {int.MaxValue}.";
						throw new Exception(excp);
					}
					#endregion //Detect type overflow

					return rtn;
				}
			}
			//</requires testing>
			#endregion //Choose

			#region Binomial Theorem
			public static string PascalsTriangle_GetLine(int n)
			{
				string rtn = "";
				//classStrings.Int_AddCommas(___)
				for (int i = 0; i <= n; i++)
				{
					if (rtn == "")
					{
						rtn = $"{CHS_simp(n, i)}";
					} else {
						rtn += " " + $"{CHS_simp(n, i)}";
					}
				}
				return rtn;
			}
			public static List<int> PascalsTriangle_GetCoefficients(int n)
			{
				List<int> rtn = new List<int>();
				for (int i = 0; i <= n; i++)
				{
					rtn.Add(CHS_simp(n, i));
				}
				return rtn;
			}
			public static string PascalsTriangle_ExpandBinomial(int n, int x, int x_, int y, int y_)
			{
				string rtn = "";
				if (n == 0)
				{
					rtn = "1";
				} else {
					for (int i = 0; i <= n; i++)
					{
						int xc = (int)System.Math.Pow(System.Math.Pow(x, x_), (n - i));
						int yc = (int)System.Math.Pow(System.Math.Pow(y, y_), i);

						int chs_ = CHS_simp(n, i);
						int coef = chs_ * xc * yc;

						string x_str = "x^" + ((n - i) * x_);
						if ((n - i) == 0) { x_str = ""; }
						if ((n - i) == 1) { x_str = "x"; }
						string y_str = "y^" + (i * y_);
						if (i == 0) { y_str = ""; }
						if (i == 1) { y_str = "y"; }
						//classStrings.Int_AddCommas(___)
						if (rtn == "")
						{
							if (System.Math.Abs(coef) == 1)
							{
								rtn = Int_NegIs_Str(coef) + x_str; //+ y_str
							} else {
								rtn = "" + coef + x_str; //+ y_str
							}
						} else {
							if (System.Math.Abs(coef) == 1)
							{
								rtn = $"{rtn} {Int_NegIs_Str_Plus(coef)} {x_str}{y_str}";
							} else {
								rtn = $"{rtn} {Int_NegIs_Str_Plus(coef)} {System.Math.Abs(coef)}{x_str}{y_str}";
							}
						}
					}
					if (rtn == "") { rtn = "1"; }
				}
				return rtn;
			}
			public static string PascalsTriangle_ExpandBinomial_GetTerm(int n, int r, int x, int x_, int y, int y_)
			{
				string rtn = "";
				int xc = (int)System.Math.Pow(System.Math.Pow(x, x_), (n - r));
				int yc = (int)System.Math.Pow(System.Math.Pow(y, y_), r);

				int chs_ = CHS_simp(n, r);
				int coef = chs_ * xc * yc;
				//classStrings.Int_AddCommas(___)
				string x_str = "x^" + ((n - r) * x_);
				if ((n - r) == 0) { x_str = ""; }
				if ((n - r) == 1) { x_str = "x"; }
				string y_str = "y^" + (r * y_);
				if (r == 0) { y_str = ""; }
				if (r == 1) { y_str = "y"; }
				rtn = $"{coef}{x_str}{y_str}";
				if (System.Math.Abs(coef) == 1) { rtn = Int_NegIs_Str(coef) + x_str + y_str; }
				if (rtn == "") { rtn = "1"; }
				return rtn;
			}
			public static int PascalsTriangle_ExpandBinomial_GetTermCoefficient(int n, int r, int x, int x_, int y, int y_)
			{
				int chs_ = CHS_simp(n, r); int a = (int)System.Math.Pow(System.Math.Pow(x, x_), (n - r)); int b = (int)System.Math.Pow(System.Math.Pow(y, y_), n);
				return (chs_ * a * b);
			}
			#endregion Binomial Theorem
		#endregion //Pascal's Triangle

		/// <summary>
		/// Returns a String value format of the Integer 'num' which takes up the number of spaces 'digits' by adding 0s to the start of the string of 'num'.
		/// For example, the parameters (128, 8) will return the String value "00000128".
		/// </summary>
		/// <param name="num"></param>
		/// <param name="digits"></param>
		/// <returns></returns>
		/// <remarks></remarks>
		public static string Ad0(int num, int digits)
		{
			string rtn = "" + System.Math.Abs(num);
			if (rtn.Length != digits) rtn = rtn.PadLeft(digits, '0');
			return Int_NegIs_Str(num) + rtn;
		}

		public static Point Subtract(this Point p, Point offset)
		{
			return new Point(p.X - offset.X, p.Y - offset.Y);
		}
        public static Point Add(this Point p, Point offset) {
            return new Point(p.X + offset.X, p.Y + offset.Y);
        }
        #region IsInRect
        public static bool IsInRect(Point pt, Rectangle rc)
		{
			return IsInRect(pt.X, pt.Y, rc.X, rc.Y, rc.Width, rc.Height);
		}
		public static bool IsInRect(Point pt, int x, int y, int w, int h)
		{
			return IsInRect(pt.X, pt.Y, x, y, w, h);
		}
		public static bool IsInRect(int x, int y, Rectangle rc)
		{
			return IsInRect(x, y, rc.X, rc.Y, rc.Width, rc.Height);
		}
		public static bool IsInRect(int x_, int y_, int x, int y, int w, int h)
		{
			return (x_ >= x && y_ >= y && x_ < x + w && y_ < y + h);
		}
		public static bool ContainsPoint(this Rectangle rect, Point p)
		{
			return p.X >= rect.X && p.Y >= rect.Y && p.X < (rect.X + rect.Width) && p.Y < (rect.Y + rect.Height);
		}
		public static bool ContainsPoint(this Rectangle rect, Point p, bool allowEdges)
		{
			if (allowEdges) return ContainsPoint(rect, p);

			return p.X > rect.X && p.Y > rect.Y && p.X < (rect.X + rect.Width - 1) && p.Y < (rect.Y + rect.Height - 1);
		}
        #endregion //IsInRect
        private static bool IsNumericType(dynamic num)
		{
			return Types.IsNumeric(typeof(NotFiniteNumberException));
		}
		/// <summary>
		/// Performs modulus-[mod] on <paramref name="input"/>. If the [result] &lt; 0, also increments by <paramref name="mod"/>.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="mod"></param>
		/// <returns></returns>
		public static int Mod_n(this int input, int mod)
		{
			if (mod <= 0) throw new Exception("[mod] should only be positive!");
			int rtn = input % mod;
			if (rtn < 0) rtn += mod;
			return rtn;
		}
		/// <summary>
		/// Performs modulus-[mod] on <paramref name="input"/>. If the [result] &lt; 0, also increments by <paramref name="mod"/>.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="mod"></param>
		/// <returns></returns>
		public static sbyte Mod_n(this sbyte input, sbyte mod)
		{
			if (mod <= 0) throw new Exception("[mod] should only be positive!");
			sbyte rtn = (sbyte)(input % mod);
			if (rtn < 0) rtn += mod;
			return rtn;
		}
		/// <summary>
		/// Performs modulus-[mod] on <paramref name="input"/>. If the [result] &lt; 0, also increments by <paramref name="mod"/>.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="mod"></param>
		/// <returns></returns>
		public static short Mod_n(this short input, short mod)
		{
			if (mod <= 0) throw new Exception("[mod] should only be positive!");
			short rtn = (short)(input % mod);
			if (rtn < 0) rtn += mod;
			return rtn;
		}
		/// <summary>
		/// Returns whether <paramref name="greater"/> is evenly divisible by <paramref name="input"/>.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="greater"></param>
		/// <returns></returns>
		public static bool Divides(this int input, int greater)
		{
			if (greater < input) return false;
			return greater % input == 0;
		}
		/// <summary>
		/// Returns whether <paramref name="greater"/> is evenly divisible by <paramref name="input"/>.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="greater"></param>
		/// <returns></returns>
		public static bool Divides(this sbyte input, int greater)
		{
			if (greater < input) return false;
			return greater % input == 0;
		}
		/// <summary>
		/// Returns whether <paramref name="greater"/> is evenly divisible by <paramref name="input"/>.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="greater"></param>
		/// <returns></returns>
		public static bool Divides(this short input, int greater)
		{
			if (greater < input) return false;
			return greater % input == 0;
		}
		/// <summary>
		/// Returns whether <paramref name="input"/> is a multiple of <paramref name="lower"/>.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="lower"></param>
		/// <returns></returns>
		public static bool IsMultipleOf(this int input, int lower)
		{
			return lower.Divides(input);
		}
		/// <summary>
		/// Returns whether <paramref name="input"/> is a multiple of <paramref name="lower"/>.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="lower"></param>
		/// <returns></returns>
		public static bool IsMultipleOf(this sbyte input, int lower)
		{
			return lower.Divides(input);
		}
		/// <summary>
		/// Returns whether <paramref name="input"/> is a multiple of <paramref name="lower"/>.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="lower"></param>
		/// <returns></returns>
		public static bool IsMultipleOf(this short input, int lower)
		{
			return lower.Divides(input);
		}
		/// <summary>
		/// Returns whether the Greatest Common Factor of <paramref name="input"/> and <paramref name="partner"/> is equal to one, via the Euclidian Algorithm.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="partner"></param>
		/// <returns></returns>
		public static bool IsCoprimeTo(this short input, int partner)
		{
			return GCD(input, partner) == 1;
		}
		/// <summary>
		/// Finds the Least Common Multiple of two Integers, using their Greatest Common Divisor via the Euclidian Algorithm.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static int LCM(int a, int b)
		{
			return (a * b) / GCD(a, b);
		}

		/// <summary>
		/// Finds the Least Common Multiple of two Integers, using their Greatest Common Divisor via the Euclidian Algorithm.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static long LCM(long a, long b) {
			return (a * b) / GCD(a, b);
		}

		/// <summary>
		/// Finds the Greatest Common Divisor of two Integers, using the Euclidian Algorithm.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static int GCD(int a, int b)
		{
			if (b > a) Swap(ref a, ref b);
			int r = -1, r2 = -1;
			while (r != 0)
			{
				Swap(ref r, ref r2);
				Rotate(ref a, ref b, ref r2);
				r2 = a % b;
			}
			return r;
		}

		/// <summary>
		/// Finds the greatest common divisor of two Long integers, using the Euclidian Algorithm.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static long GCD(long a, long b) {
			ulong result = GCD((ulong)System.Math.Abs(a), (ulong)System.Math.Abs(b));
			if (a > 0 || b > 0)
				return (long)result;

			return -1 * (long)result;
		}


        /// <summary>
        /// Finds the greatest common divisor of two Long integers, using the Euclidian Algorithm.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ulong GCD(IEnumerable<ulong> source) {
			using var s = source.GetEnumerator();

			ulong result = s.Current;

			while (s.MoveNext()) {
				result = GCD(s.Current, result);
				if (result == 1)
					break;
			}

			return result;
        }

        /// <summary>
        /// Finds the greatest common divisor of two Long integers, using the Euclidian Algorithm.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ulong GCD(ulong a, ulong b)
		{
			if (b > a) Swap(ref a, ref b);
			ulong r = 0, r_old = 0;
			do
			{
				r_old = r;
				if (r_old != 0) //if (not the first run)
					Rotate(ref a, ref b, ref r_old);
				r = a % b;
			} while (r != 0);
			return b; //b (recently rotated from r_old)
		}
		/// <summary>
		/// Rotates the values of three Integers counter-clockwise, by reference. a->c, b->a, &amp; c->b.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		public static void Rotate(ref int a, ref int b, ref int c)
		{
			int temp = a + 0;
			a = b + 0;
			b = c + 0;
			c = temp;
		}
		/// <summary>
		/// Rotates the values of three Unsigned Long integers counter-clockwise, by reference. a->c, b->a, &amp; c->a.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		public static void Rotate(ref ulong a, ref ulong b, ref ulong c)
		{
			ulong temp = a + 0;
			a = b + 0;
			b = c + 0;
			c = temp;
		}
		/// <summary>
		/// Swaps the values of two Integers, by reference.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		public static void Swap(ref int a, ref int b)
		{
			int temp = b + 0;
			b = a + 0;
			a = temp;
		} 
		/// <summary>
		/// Swaps the values of two Unsigned Long integers, by reference.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		public static void Swap(ref ulong a, ref ulong b)
		{
			ulong temp = b + 0;
			b = a + 0;
			a = temp;
		} 
		/// <summary>
		/// Returns an array of digits within "num". Example: 100 = {1, 0, 0}.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static sbyte[] Digits(int num)
		{
			int length = Length(num);
			sbyte[] rtn = new sbyte[length];
			for (int i = 0; i < length; i++) {
				int temp = (int)(num / System.Math.Pow(10, length - i - 1)); //Digit at rightmost, left-padded with extra digits.
				rtn[i] = (sbyte)(temp % 10); //subtract left digits.
			}
			return rtn;
		}
		/// <summary>
		/// Returns an array of digits within "num". Example: 100.55 = (indicateDecimal: TRUE {1, 0, 0, -1, 5, 5} | FALSE {1, 0, 0, 5, 5})
		/// </summary>
		/// <param name="num"></param>
		/// <param name="indicateDecimal"></param>
		/// <returns></returns>
		public static sbyte[] Digits(double num, bool indicateDecimal)
		{
			sbyte[][] temp = SplitDigits(num);
			sbyte[] rtn = new sbyte[temp[0].Length + temp[1].Length + (indicateDecimal ? 1 : 0)];
			int digit = 0;
			for(byte i = 0; i < 2; i++) {
				if (i == 1) {
					rtn[digit] = -1;
					digit++;
				}
				for (byte j = 0; j < temp[i].Length; j++) {
					rtn[digit] = temp[i][j];
					digit++;
				}
			}
			return rtn;
		}
		/// <summary>
		/// [0]: whole-array | [1]: decimal-array | [#][i] the ith digit.
		/// If there are no decimals, an array of length 0 is sent to [1].
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static sbyte[][] SplitDigits(double num)
		{
			int[] temp = SplitDecimal(num);
			bool isDec = temp[1] > 0;
			return new sbyte[][] {Digits(temp[0]), (isDec ? Digits(temp[1]) : new sbyte[0])};
		}
		/// <summary>
		/// Returns the character length of <paramref name="num"/>.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static int Length(int num)
		{
			return ("" + num).Length;
		}
		/// <summary>
		/// Returns the character length of <paramref name="num"/>.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static int Length(double num)
		{
			return ("" + num).Length;
		}
		/// <summary>
		/// Returns the character lengths {left-of, right-of} the decimal point of the Double <paramref name="num"/>.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static int[] Lengths(double num)
		{
			string temp = "" + num;
			return (!temp.Contains(".") ? new int[] {temp.Length, 0} : new int[] {temp.Split('.')[0].Length, temp.Split('.')[1].Length});
		}
		public static int[] SplitDecimal(double num)
		{
			string temp = "" + num;
			bool isDec = temp.Contains('.');
			int whole = (int)System.Math.Floor(num);
			if (!isDec)
				return new int[] {whole, 0};
			else {
				double dec = num - whole;
				dec *= System.Math.Pow(10, temp.Split('.')[1].Length);
				return new int[] { whole, (int)System.Math.Round(dec) };
			}
		}
		public static double RoundF(double num, byte decimals)
		{
			int factor = (int)System.Math.Floor(System.Math.Pow(10, decimals));
			return ((double)FInt(num * factor) / factor);
		}
		/*

 		/// <summary>
		/// Outputs the instantaneous rates of change in the array of numbers.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static dynamic[] Derive(dynamic[] data)
		{
			if ( !IsNumericType(data[0]) ) throw new Exception("Type must be numeric.");
			Type T = (data.GetType().GetElementType());
			Type A = data.GetType();
			dynamic rtn = default(typeof(data));
			for (uint i = 1; i < data.Length; i++)
			{
				rtn[i] = data[i+1] - data[i];
			}
		}
		/// <summary>
		/// Outputs values integrated from an array of instantaneous rates of change.
		/// </summary>
		/// <param name="data"></param>
		public static void Integrate(dynamic[] data)
		{

		}

		*/

	} //END-ALL
}
