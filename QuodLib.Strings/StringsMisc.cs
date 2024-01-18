
namespace QuodLib.Strings
{
	/// <summary>
	/// A class of miscelaneous methods for operating on strings.
	/// </summary>
	public static class Misc
	{
		public static readonly string[] SizeNames_Bytes = new string[] {"  B", "KB", "MB", "GB", "TB"};
		public static readonly string[] SizeNames_Common = new string[] {"", "K", "M", "B", "T", "Qd", "Qi", "S", "H", "O", "N"};

		public static string Dbl_ToString(double num)
		{
			string sDbl = "" + num;
			if (!sDbl.Contains("."))
				sDbl += ".0";

			return sDbl;
		}
		#region AddCommas_functions
		public static string Num_AddCommas(int num)
		{
			return num.ToString("N1").Split('.')[0];
		}
		public static string Num_AddCommas(uint num)
		{
			return num.ToString("N1").Split('.')[0];
		}
		public static string Num_AddCommas(long num)
		{
			return num.ToString("N1").Split('.')[0];
		}
		public static string Num_AddCommas(ulong num)
		{
			return num.ToString("N1").Split('.')[0];
		}
		public static string Num_AddCommas(float num, byte decimals = 0)
		{
			if (decimals == 0) return num.ToString("N1").Split('.')[0];

			string rtn = Num_AddCommas(num);
			string dec = "" + num;
			if (dec.Contains("."))
			{
				dec = dec.Split('.')[1];
				if (dec.Length > decimals) dec = dec.Substring(0, decimals);
				rtn += "." + dec;
			}
			return rtn;
		}
		public static string Num_AddCommas(double num, byte decimals = 0)
		{
			if (decimals == 0) return num.ToString("N1").Split('.')[0];

			string rtn = Num_AddCommas(num);
			string dec = "" + num;
			if (dec.Contains("."))
			{
				dec = dec.Split('.')[1];
				if (dec.Length > decimals) dec = dec.Substring(0, decimals);
				rtn += "." + dec;
			}
			return rtn;
		}
		public static string Num_AddCommas(decimal num, byte decimals = 0)
		{
			if (decimals == 0) return num.ToString("N1").Split('.')[0];

			string rtn = Num_AddCommas(num);
			string dec = "" + num;
			if (dec.Contains("."))
			{
				dec = dec.Split('.')[1];
				if (dec.Length > decimals) dec = dec.Substring(0, decimals);
				rtn += "." + dec;
			}
			return rtn;
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
