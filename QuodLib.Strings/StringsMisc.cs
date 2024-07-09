
using System;

namespace QuodLib.Strings
{
	/// <summary>
	/// A class of miscelaneous methods for operating on strings.
	/// </summary>
	public static class Misc
	{
		public static readonly string[] SizeNames_Bytes = new string[] {"  B", "KB", "MB", "GB", "TB"};
		public static readonly string[] SizeNames_Common = new string[] {"", "K", "M", "B", "T", "Qd", "Qi", "S", "H", "O", "N"};

        #region AddCommas_functions
        public static string ToCommaString(this short num)
            => num.ToString("N1").Split('.')[0];

        public static string ToCommaString(this ushort num)
            => num.ToString("N1").Split('.')[0];

        public static string ToCommaString(this int num)
			=> num.ToString("N1").Split('.')[0];

        public static string ToCommaString(this uint num)
			=> num.ToString("N1").Split('.')[0];

        public static string ToCommaString(this long num)
			=> num.ToString("N1").Split('.')[0];

        public static string ToCommaString(this ulong num)
			=> num.ToString("N1").Split('.')[0];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <param name="decimals">The number of decimal-places to use (null for auto)</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string ToCommaString(this float num, int? decimals = null)
		{
			if (decimals != null && decimals < 0)
				throw new ArgumentOutOfRangeException(nameof(decimals), $"{nameof(decimals)} must be null or >= 0 ");

            return ToCommaString(num.ToString("N1"), (int)decimals!);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <param name="decimals">The number of decimal-places to use (null for auto)</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string ToCommaString(this double num, byte? decimals = null)
		{
            if (decimals != null && decimals < 0)
                throw new ArgumentOutOfRangeException(nameof(decimals), $"{nameof(decimals)} must be null or >= 0 ");

            return ToCommaString(num.ToString("N1"), (int)decimals!);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <param name="decimals">The number of decimal-places to use (null for auto)</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string ToCommaString(this decimal num, byte? decimals = 0)
		{
            if (decimals != null && decimals < 0)
                throw new ArgumentOutOfRangeException(nameof(decimals), $"{nameof(decimals)} must be null or >= 0 ");

            return ToCommaString(num.ToString("N1"), (int)decimals!);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatted">(numerictype)<c>.ToString("N1")</c></param>
        /// <param name="decimals">The number of decimal-places to use (null for auto)</param>
        /// <returns></returns>
        private static string ToCommaString(string formatted, int? decimals) {
			if (!formatted.Contains('.'))
				throw new ArgumentException("String has not been numerically formatted; expected to contain '.'", nameof(formatted));

            if (decimals == null) {
                string rtn = formatted.RemoveTrailing('0');
                if (rtn[^1] == '.')
                    return rtn.Substring(0, rtn.Length - 1);
            }

            string[] sides = formatted.Split('.');

            if (decimals == 0)
                return sides[0];

            if (sides[1].Length == decimals)
                return $"{sides[0]}.{sides[1]}";

            if (sides[1].Length > decimals)
                return $"{sides[0]}.{sides[1].Substring(0, (int)decimals!)}";

            return $"{sides[0]}.{sides[1].PadRight((int)decimals!, '0')}";
        }

        #endregion //AddCommas_functions
        public static string Size_Compress(long sze, short bs, byte len, string[] names, bool space) //todo: len[]
		{
			if (sze == 0) return "0" + (space ? " " : "") + names[0];
			if (len < 2) len = 2;

			decimal sze_ = sze;
			decimal sze_prev = -1;
			int i = 0;
			string temp = "" + (long)Math.Floor(sze_);
			while (temp.Length > len && temp != "0" && i <= (names.Length - 1)) {
				sze_prev = sze_;
				sze_ /= bs;
				i++;
				temp = "" + (long)Math.Floor(sze_prev);
			}

			string rtn;

			if (i == 0)
				rtn = "" + sze + (space ? " " : "") + names[0];
			else
				rtn = "" + System.Math.Round(sze_prev, 2) + (space ? " " : "") + names[i - 1];

			return rtn;
		}
		
		/// <summary>
		/// Outputs a List&lt;string&gt; to a single String, with line-breaks for each item.
		/// </summary>
		/// <param name="Input"></param>
		/// <returns></returns>
		public static string List_ToString(this List<string> Input)
		{
			return Input.List_ToString(true);
		}
		/// <summary>
		/// Outputs a List&lt;string&gt; to a single String, with line-breaks for each item.
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="useRet">Whether to use a carriage-return character in the line-break.</param>
		/// <returns></returns>
		public static string List_ToString(this List<string> Input, bool useRet)
		{
			string rtn = "";
			foreach (string str in Input)
				rtn += (rtn == "" ? "" : (useRet ? "\r" : "") + "\n") + str;
			return rtn;
		}
	}
}
