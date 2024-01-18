using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using classMath = QuodLib.Math.General;

namespace QuodLib
{
    using QuodLib.Proto;
    using ProtoBuf;
    using QuodLib.Strings;
	public static class classDatetime
	{
		public static class SimpleTimes
		{
			public class TimeFrame
			{
				public Time Start;
				public Time End;
				/// <summary>
				/// The duration, in fractional hours, contained between the Start and End times.
				/// </summary>
				public float HourLength
				{
					get
					{
						return (End - Start < 0 ? 24f - (Start - new Time(0, 0)) + (End - new Time(0, 0)) : (End - Start == 0 ? 24f : End - Start));
					}
				}
				public bool Crosses_12hr {
					get {
						return Start.Hour12 >= End.Hour12;
					}
				}
				public bool Crosses_Day {
					get {
						return Start.Hour24 >= End.Hour24;
					}
				}
				public TimeFrame(Time start, Time end)
				{
					Start = start;
					End = end;
				}
				public TimeFrame Copy()
				{
					return new TimeFrame(Start.Copy(), End.Copy());
				}
			} // </TimeFrame>

			[ProtoContract]
			public class Duration : IProto {
				#region Properties

				[ProtoMember(1)]
				private int _hours;

				[ProtoMember(2)]
				private int _minutes;

				[ProtoMember(3)]
				private int _seconds;

				[ProtoMember(4)]
				private int _milliseconds;

                public int Days { get; set; }

                public int Hours {
					get => _hours;
					set {
						if (value < 0) throw new ArgumentOutOfRangeException("Must be positive.");
						(int val, int carry) = Math.General.RollOver(value, 24);
						_hours = val;
						Days += carry;
					}
				}
				public int Minutes {
					get => _minutes;
					set {
						if (value < 0) throw new ArgumentOutOfRangeException("Must be positive.");
						(int val, int carry) = Math.General.RollOver(value, 60);
						_minutes = val;
						Hours += carry;
					}
				}
				public int Seconds {
					get => _seconds;
					set {
						if (value < 0) throw new ArgumentOutOfRangeException("Must be positive.");
						(int val, int carry) = Math.General.RollOver(value, 60);
						_seconds = val;
						Minutes += carry;
					}
				}
                public int Milliseconds {
					get => _milliseconds;
					set {
						if (value < 0) throw new ArgumentOutOfRangeException("Must be positive.");
						(int val, int carry) = Math.General.RollOver(value, 60);
						_milliseconds = val;
						Seconds += carry;
					}
				}
                #endregion //Properties
                public Duration(int days, int hours, int minutes, int seconds, int milliseconds) {
					if (days < 0 || hours < 0 || minutes < 0 || seconds < 0 || milliseconds < 0
							|| hours > 24 || minutes > 59 || seconds > 59 || milliseconds > 999)
                        throw new ArgumentOutOfRangeException();

					Days = days;
					_hours = hours;
					_minutes = minutes;
					_seconds = seconds;
				}

				public static bool operator !=(Duration a, Duration b)
					=> a.Days != b.Days || a.Hours != b.Hours || a.Minutes != b.Minutes || a.Seconds != b.Seconds || a.Milliseconds != b.Milliseconds;

				public static bool operator <(Duration a, Duration b)
					=> a.Days < b.Days || a.Hours < b.Hours || a.Minutes < b.Minutes || a.Seconds < b.Seconds || a.Milliseconds < b.Milliseconds;

				public static bool operator >(Duration a, Duration b)
					=> b.Days < a.Days || b.Hours < a.Hours || b.Minutes < a.Minutes || b.Seconds < a.Seconds || b.Milliseconds < a.Milliseconds;

				public static bool operator ==(Duration a, Duration b)
					=> b.Days == a.Days && b.Hours == a.Hours && b.Minutes == a.Minutes && b.Seconds == a.Seconds && b.Milliseconds == a.Milliseconds;
			}

			public class Time
			{
				#region Properties
				private int hour;
				private int minute;
				private int second;
				/// <summary>
				/// 12-hour format for the hour value. 12:00 is used for midnight and noon.
				/// </summary>
				public int Hour12 {
					get {
						return (hour == 0 ? 12 : (hour > 12 ? hour - 12 : hour));
					}
				}
				/// <summary>
				/// 24-hour format for the hour value. 0:00 is used for midnight.
				/// </summary>
				public int Hour24 {
					get {
						return hour;
					}
					set {
						if (value < 0 || value > 24) throw new Exception("Hour " + value + " outside 24-hour bounds.");
						hour = value;
					}
				}
				public int Minute {
					get {
						return minute;
					}
					set {
						if (value < 0 || value > 60) throw new Exception("Minute " + value + " oustide 60-minute bounds.");
						minute = value;
					}
				}
				public int Second {
					get {
						return Second;
					}
					set {
						if (value < 0 || value > 60) throw new Exception("Second " + value + " oustide 60-second bounds.");
						second = value;
					}
				}
				/// <summary>
				/// Boolean value for if the hour value is between noon (inclusive) and midnight (non-inclusive).
				/// </summary>
				public bool Afternoon {
					get {
						return (hour >= 12);
					}
					set {
						if (value) { if (hour < 12) hour += 12; } else { if (hour > 12) hour -= 12; }
					}
				}
				/// <summary>
				/// Boolean value for whether the hour value is between midnight (inclusive) and noon (non-inclusive).
				/// </summary>
				public bool Morning {
					get {
						return (hour < 12);
					}
					set {
						if (value) { if (hour > 12) hour -= 12; } else { if (hour < 12) hour += 12; }
					}
				}
				#endregion //Properties

				public Time(int hour, int minute, int second)
				{
					if (hour < 0 || minute < 0 || second < 0 || hour > 24 || minute > 59 || second > 59) throw new Exception("Args outside time bounds.");
					this.hour = hour;
					this.minute = minute;
					this.second = second;
				}
				public Time(int hour, int minute) : this(hour, minute, 0) {}
				public Time(int hour, int minute, bool afternoon) : this(hour_Make24(hour, afternoon), minute, 0) {}
				public Time(int hour, int minute, int second, bool afternoon) : this(hour_Make24(hour, afternoon), minute, second) {}
				
				public static int hour_Make24(int hour, bool afternoon)
				{
					if (hour < 0 || hour > 12) throw new Exception("Hour " + hour + " outside 24-hour bounds.");
					return (hour == 12 ? (afternoon ? 12 : 0) : (afternoon ? hour + 12 : hour) );
				}
				public Time Copy()
				{
					return new Time(hour, minute, second);
				}
				public static explicit operator Time(DateTime dt)
				{
					return new Time(dt.Hour, dt.Minute, dt.Second);
				}
				public override string ToString() {
					return "" + Hour12 + ':' + ("" + Minute).PadLeft(2, '0') + ':' + ("" + second).PadLeft(2, '0') + (Afternoon ? "pm" : "am");
				}
				/// <summary>
				/// Returns a duration difference, in fractional hours, between two Time objects.
				/// </summary>
				/// <param name="before"></param>
				/// <param name="after"></param>
				/// <returns></returns>
				public static float operator -(Time after, Time before)
				{
					return after.ToFractionalHours() - before.ToFractionalHours();
				}
				private float ToFractionalHours()
				{
					return Hour24 + (minute / 60f) + (second / 3600f);
				}
			} // </Time>
			/// <summary>
			/// Simple value, with conversions to and from "int" and "byte" types as well as strings, defining a day of the week.
			/// </summary>
			public class Weekday {
				private byte weekNum;
				private Weekday(byte wNum)
				{
					weekNum = wNum;
				}
				public static implicit operator string(Weekday wd)
				{
					switch (wd.weekNum)
					{
						case 0:
							return "Monday";
						case 1:
							return "Tuesday";
						case 2:
							return "Wednesday";
						case 3:
							return "Thursday";
						case 4:
							return "Friday";
						case 5:
							return "Saturday";
						case 6:
							return "Sunday";
						default:
							throw new Exception("Weekday's day value " + wd.weekNum + " undefined.");
					}
				}
				public static implicit operator int(Weekday wd)
				{
					return wd.weekNum;
				}
				public static implicit operator byte(Weekday wd)
				{
					return wd.weekNum;
				}
				public static explicit operator Weekday(string str)
				{
					switch (str.ToLower())
					{
						case "monday": case "mon":
							return (Weekday)0;
						case "tuesday": case "tue":
							return (Weekday)1;
						case "wednesday": case "wed":
							return (Weekday)2;
						case "thursday": case "thu":
							return (Weekday)3;
						case "friday": case "fri":
							return (Weekday)4;
						case "saturday": case "sat":
							return (Weekday)5;
						case "sunday": case "sun":
							return (Weekday)6;
						default:
							throw new Exception("Day \"" + str + "\" undefined.");
					}
				}
				public static explicit operator Weekday(int wNum)
				{
					if (wNum < 0 || wNum > 6) throw new Exception("Day " + wNum + " undefined.");
					return new Weekday((byte)wNum);
				}
				public static explicit operator Weekday(byte wNum)
				{
					if (wNum < 0 || wNum > 6) throw new Exception("Day " + wNum + " undefined.");
					return new Weekday(wNum);
				}
				public static explicit operator Weekday(DateTime dt)
				{
					return (Weekday)(dt.DayOfWeek.ToString());
				}
				public Weekday Copy()
				{
					return new Weekday(weekNum);
				}
			} // </Weekday>
			/// <summary>
			/// Simplified object for defining a date without defining a time of day.
			/// </summary>
			public class Date
			{
				#region Fields
				byte day, month;
				public ushort Year;
				#endregion //Fields
				#region Properties
				/// <summary>
				/// The numeric day of the month.
				/// </summary>
				public byte Day {
					get {
						return day;
					}
					set {
						byte max = (byte)DateTime.DaysInMonth(Year, month);
						if (max < day) throw new Exception("Day \"" + value + "\" exceeds month's " + Year + " limit of " + max);
							else day = value;
					}
				}
				/// <summary>
				/// The numeric month.
				/// </summary>
				public byte Month {
					get {
						return month;
					}
					set {
						if (value > 12) throw new Exception("Month \'" + value + "\" nonexistent.");
							else month = value;
					}
				}
				/// <summary>
				/// An object instance of the day of week.
				/// </summary>
				public Weekday Weekday {
					get {
						return (Weekday)(DateTime)this;
					}
				}
				#endregion //Properties
				#region Constructors
				public Date(ushort year, byte month, byte day)
				{
					Year = year; Month = month; Day = day;
				}
				public Date(byte month, byte day) : this((ushort)DateTime.Now.Year, month, day) {}
				#endregion //Constructors
				#region Methods
				/// <summary>
				/// Adds the provided number of days to the date.
				/// </summary>
				/// <param name="days"></param>
				/// <returns></returns>
				public Date AddDays(int days)
				{
					return (Date)((DateTime)this).AddDays(days);
				}
				/// <summary>
				/// Adds the number of months to the date.
				/// </summary>
				/// <param name="months"></param>
				/// <returns></returns>
				public Date AddMonths(int months)
				{
					return (Date)((DateTime)this).AddMonths(months);
				}
				public bool Equals(Date dt)
				{
					return Year == dt.Year && Month == dt.Month && Day == dt.Day;
				}
				#endregion //Methods
				#region Operators
				public static implicit operator DateTime(Date dt)
				{
					return new DateTime(dt.Year, dt.Month, dt.Day);
				}
				public static explicit operator Date(DateTime dt)
				{
					return new Date((ushort)dt.Year, (byte)dt.Month, (byte)dt.Day);
				}
				#endregion //Operators
			} // </Date>
		}
		/// <summary>
		/// Format: YYYY---MM.DD-(am|pm)hh.mm.ss.lll
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="Accuracy">1: year | 2: month | 3: day | 4: hour | 5: minute | 6: second | 7: millisecond</param>
		/// <param name="NoonMidnightIsUnderscored">Replaces am00.00 with am_12.00</param>
		/// <returns></returns>
		public static string Date_ToString(DateTime dt, byte Accuracy, bool NoonMidnightIsUnderscored)
		{
			string rtn = (Accuracy == 0 ? "NULL DATE" : "");
			if (Accuracy >= 1) { rtn += dt.Year; }
			if (Accuracy >= 2) { rtn += "---" + Math.General.Ad0(dt.Month, 2); }
			if (Accuracy >= 3) { rtn += "." + Math.General.Ad0(dt.Day, 2); }
			if (Accuracy >= 4) { rtn += "-" + Hour_ToString((byte)dt.Hour, NoonMidnightIsUnderscored); }
			if (Accuracy >= 5) { rtn += "." + Math.General.Ad0(dt.Minute, 2); }
			if (Accuracy >= 6) { rtn += "." + Math.General.Ad0(dt.Second, 2); }
			if (Accuracy >= 7) { rtn += "." + Math.General.Ad0(dt.Millisecond, 3); }
			return rtn;
		}
		public static string Hour_ToString(byte hour, bool NoonMidnightIsUnderscored)
		{
			string apm = "am";
			if (hour >= 12)
			{
				hour -= 12;
				apm = "pm";
			}
			if (NoonMidnightIsUnderscored)
			{
				if (hour == 0) { hour = 12; }
				if (hour == 12)
				{
					return apm + "_12";
				} else {
					return apm + Math.General.Ad0(hour, 2);
				}
			} else {
				return apm + Math.General.Ad0(hour, 2);
			}
		}
		public static DateTime HourMinute_FromString(string hr)
		{
			uint[] times = new uint[2];
			string exc = "Given time doesn not follow the format \"##:##<am/pm>\", \"#:##<am/pm>\", \"<am/pm>##:##\", or \"<am/pm>#:##\" (ex. am09.53).";
			if ( (hr.Length == 7 || hr.Length == 6 || hr.Length == 5 || hr.Length == 4) && hr.Contains(":") )
			{
				hr = hr.ToLower();
				string[] apm_arr  = new string[] {"am", "pm"};
				string apm = hr.RemoveTerms(apm_arr);
				apm = hr.RemoveTerm(apm);
				if ( string.IsNullOrEmpty(apm) )
				{
					if (hr.StartsWith(apm_arr) || hr.EndsWith(apm_arr))
					{
						hr = hr.RemoveTerm(apm);
						times[0] = uint.Parse(hr.Split(':')[0]);
						if (apm == "pm" && times[0] != 12)
						{
							times[0] += 12;
						}
						times[1] = uint.Parse(hr.Split(':')[1]);
					} else if (!hr.Contains(apm_arr, false) && (hr.Length == 5 || hr.Length == 4) ) {
						hr = hr.RemoveTerm(apm);
						times[0] = uint.Parse(hr.Split(':')[0]);
						if (apm == "pm" && times[0] != 12)
						{
							times[0] += 12;
						}
						times[1] = uint.Parse(hr.Split(':')[1]);
					} else {
						throw new Exception(exc);
					}
				} else { //IsMilitaryTime
					hr = hr.RemoveTerm(apm);
					times[0] = uint.Parse(hr.Split(':')[0]);
					times[1] = uint.Parse(hr.Split(':')[1]);
				}
			} else {
				throw new Exception(exc);
			}
			return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, (int)times[0], (int)times[1], 0);
		}
		public static string HourMinute_ToString(DateTime dt, bool ApmBefore, bool caps)
		{
			byte hr = (byte)dt.Hour;
			string Apm = "";
			if (dt.Hour >= 12)
			{
				hr -= 12;
				Apm = "pm";
			} else {
				Apm = "am";
			}
			string tm = "" + Math.General.Ad0(hr, 2) + ":" + Math.General.Ad0(dt.Minute, 2);
			if (caps)
			{
				Apm = Apm.ToUpper();
			}
			if (ApmBefore)
			{
				return tm + Apm;
			} else {
				return Apm + tm;
			}
		}
		public static DateTime Earliest(DateTime[] dts)
		{
			DateTime rtn = dts[0];
			for (uint i = 1; i < dts.Count(); i++)
			{
				if ( IsBefore(dts[i], dts[i-1]) )
				{
					rtn = dts[i];
				}
			}
			return rtn;
		}
		public static DateTime Latest(DateTime[] dts)
		{
			DateTime rtn = dts[0];
			for (uint i = 1; i < dts.Count(); i++)
			{
				if ( IsAfter(dts[i], dts[i-1]) )
				{
					rtn = dts[i];
				}
			}
			return rtn;
		}
		public static bool IsAfter(DateTime Primary, DateTime Established)
		{
			return IsAfter(Primary, Established, false);
		}
		public static bool IsAfter(DateTime Primary, DateTime Established, bool AllowEqual)
		{
			bool rtn = Primary.CompareTo(Established) >= 0;
				if (AllowEqual)
			{
				return rtn;
			} else {
				return rtn && !IsEqual(Primary, Established);
			}
		}
		public static bool IsBefore(DateTime Primary, DateTime Established)
		{
			return IsBefore(Primary, Established, false);
		}
		public static bool IsBefore(DateTime Primary, DateTime Established, bool AllowEqual)
		{
			bool rtn = Primary.CompareTo(Established) <= 0;
			if (AllowEqual)
			{
				return rtn;
			} else {
				return rtn && !IsEqual(Primary, Established);
			}
		}
		public static bool IsEqual(DateTime Primary, DateTime Secondary)
		{
			return Primary.CompareTo(Secondary) == 0;
		}
		public static bool Overlaps(DateTime[] Primary, DateTime[][] Established) //[0]=start, [1]=end | Established[i][2]
		{
			bool rtn = false;
			for (uint i = 0; i < Established.Count() && !rtn; i++)
			{
				rtn = Overlaps(Primary, Established[i]);
			}
			return rtn;
		}
		public static bool Overlaps(DateTime[] Primary, DateTime[] Established) //[0]=start, [1]=end
		{
			bool Encloses = ( IsBefore(Primary[0], Established[0], true) && IsAfter(Primary[1], Established[1], true) ) || ( IsBefore(Established[0], Primary[0], true) && IsAfter(Established[1], Primary[1], true) );
			
			bool ShiftLeftP = IsBefore(Primary[0], Established[0], true) && IsAfter(Primary[1], Established[0], true);
			bool ShiftRightP = IsAfter(Primary[1], Established[1], true) && IsBefore(Primary[0], Established[1], true);

			bool ShiftLeftE = IsBefore(Established[0], Primary[0], true) && IsAfter(Established[1], Primary[0], true);
			bool ShiftRightE = IsAfter(Established[1], Primary[1], true) && IsBefore(Established[0], Primary[1], true);

			return Encloses || (ShiftLeftP || ShiftLeftE) || (ShiftRightP || ShiftRightE);
		}
		public static byte[] DateTime_ToArray(DateTime dt)
		{
			return new byte[] { (byte)dt.Year, (byte)dt.Month, (byte)dt.Day, (byte)dt.Hour, (byte)dt.Minute, (byte)dt.Second, (byte)dt.Millisecond };
		}
		public static string Weekday_FromShortened(string wd)
		{
			switch (wd.ToUpper())
			{
				case "MON":
					return "Monday";
				case "TUE":
					return "Tuesday";
				case "WED":
					return "Wednesday";
				case "THU":
					return "Thursday";
				case "FRI":
					return "Friday";
				case "SAT":
					return "Saturday";
				case "SUN":
					return "Sunday";
				default:
					throw new Exception("Weedkay \"" + wd + "\" not recognized");
			}
		}
		public static int Month_ToNum(string month, bool zeroBased)
		{
			string temp = month.Substring(0, 3).ToLower();
			int rtn = "jan_feb_mar_apr_may_jun_jul_aug_sep_oct_nov_dec".IndexOf(temp) / 4;
			return (zeroBased ? 0 : 1) + rtn;
		}
		public static string[] Weekday_FromShortened(string[] wd)
		{
			string[] rtn = new string[wd.Count()];
			for (byte i = 0; i < wd.Count(); i++)
			{
				rtn[i] = Weekday_FromShortened(wd[i]);
			}
			return rtn;
		}
		public static string Weekday_Shorten(string wd)
		{
			return wd.TowardIndex(2, true);
		}
		public static string[] Weekday_Shorten(string[] wd)
		{
			string[] rtn = new string[wd.Count()];
			for (byte i = 0; i < wd.Count(); i++)
			{
				rtn[i] = Weekday_Shorten(wd[i]);
			}
			return rtn;
		}
		//DateTime NextOccurrance(<signed parameters>, iteration)
		//{
		//	  While (itr < iteration)
		//	  {
		//		  While (test if matches)
		//		  {
		//			  functionality
		//		  }
		//	  }
		//}
	}
}
