using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;
using GFont = QuodLib.WinForms.Drawing.Objects.classFonts.GFont;
using CString = QuodLib.WinForms.Drawing.Objects.classFonts.CString;

using Debug = System.Diagnostics.Debug;
using QuodLib.WinForms.Drawing;

using SMath = System.Math;
using QuodLib.WinForms.Drawing.Objects;

namespace QuodLib.WinForms.Objects
{
	public class CNumericUpDown : CObject, ICObject
	{

		#region Fields
		public bool PrintDiagnostics = false;
		public bool Calibrate = false;
		/// <summary>
		/// 0=BG, 1=border, 2=fontColor
		/// </summary>
		classGraphics.objGColor[] clrTxt = {classGraphics.GColor(0, 6, 0), classGraphics.GColor(0, 12, 0), classGraphics.MColor(255)};
		/// <summary>
		/// 0=buttonsFill, 1=buttonsEdge, 2=arrows
		/// </summary>
		classGraphics.objGColor[] clrBtn = {classGraphics.GColor(0, 2, 0), classGraphics.GColor(0, 10, 0), classGraphics.CColor(191, 127, 11)};
		public Color DisabledColor = classGraphics.MColor(127);
		public /*new*/ GFont Font = classFonts.Fonts.FurDomus_6x9;
		//public string Tag { get; set; }
		//private byte textBrightness = 14;
		public bool FlatStyle = true;
		private double value_ = 0;
		private double minimum = 0;
		private double maximum = 100;
		public byte Precision = 0;
		#endregion //Fields

		#region Properties
			#region ReadOnly
		public static Type Type {
			get {
				return (new CNumericUpDown()).GetType();
			}
		}
				#region Numerics
		public double Value {
			get {
				return value_;
			}
			set {
				if (value < Minimum) value = Minimum;
				if (value > Maximum) value = Maximum;
				value_ = value;
			}
		}
		public double Maximum {
			get {
				return maximum;
			}
			set {
				maximum = value;
				if (Value > Maximum) Value = maximum;
				if (Minimum < Maximum) Minimum = maximum;
			}
		}
		public double Minimum {
			get {
				return minimum;
			}
			set {
				minimum = value;
				if (Value < minimum) Value = minimum;
				if (Maximum < minimum) Maximum = minimum;
			}
		}
				#endregion //Numerics

				#region Graphics
		private int Spacing {
			get {   
				return (Font.CharWidth / 3);
			}
		}
		private Color ArrowColor {
			get {
				return(Enabled ? (FlatStyle ? (Color)clrTxt[2] : (Color)clrBtn[2]) : DisabledColor);
			}
		}
		private int ArrowWidth {
			get {
				return Font.NumericWidth + 1;
			}
		}
		private int ArrowHeight {
			get {
				return ArrowWidth / 2;
			}
		}
		private Image ArrowDown {
			get {
				Image rtn = new Bitmap(ArrowWidth, ArrowHeight);
				Graphics A = Graphics.FromImage(rtn);
				for (byte i = 0; i < ArrowHeight; i++)
				{
					A.DrawLine(classGraphics.CPen(ArrowColor), new Point(i, i), new Point(ArrowWidth - i - 1, i)); //x[i, arr - i], y[constant]
				}
				A.Dispose();
				return rtn;
			}
		}
		private Image ArrowUp {
			get {
				Image rtn = new Bitmap(ArrowWidth, ArrowHeight);
				Graphics A = Graphics.FromImage(rtn);
				for (byte i = 0; i < ArrowHeight; i++)
				{
					A.DrawLine(classGraphics.CPen(ArrowColor), new Point(i, ArrowHeight - i - 1), new Point(ArrowWidth - i - 1, ArrowHeight - i - 1)); //x[i, arr - i], y[constant]
				}
				A.Dispose();
				return rtn;
			}
		}
		/// <summary>
		/// [Digits] + [decimal separator] + [Decimals].
		/// </summary>
		public new uint Width {
			get {
				if (FlatStyle)
				{
					return (uint)(DecStart + 1 + (Precision * GWidth) - (Precision > 0 ? Spacing : 0));
				} else {
					return (uint)((Digits + (Precision == 0 ? 0 : 5 + Precision)) * 16);
				}
			}
		}
		public new uint Height {
			get {
				if (FlatStyle)
				{
					return (uint)( (ArrowHeight * 2) + Font.NumericHeight + (Padding_Vertical * 2) );
				} else {
					return (uint)(14 + Font.CharHeight);
				}
			}
		}
		private int FontY {
			get {
				return ArrowHeight + Padding_Vertical;
			}
		}
		private int Padding_Vertical {
			get {
				return Math.General.CInt(Font.NumericHeight * (3f / 14));
			}
		}
		private int GWidth {
			get {
				return Font.NumericWidth + Spacing;
			}
		}
		private int DecStart {
			get {
				return GWidth * Digits + Spacing;
			}
		}
					#region Areas
		Rectangle Area_ArrowsUp {
			get {
				return new Rectangle(0, 0, (int)Width, (int)ArrowHeight + 1);
			}
		}
		Rectangle Area_ArrowsDown {
			get {
				return new Rectangle(0, (int)Height - ArrowHeight, (int)Width, (int)ArrowHeight);
			}
		}
		Rectangle Area_Numbers {
			get {
				return new Rectangle(0, (int)ArrowHeight, (int)Width, (int)Height - (int)(ArrowHeight + ArrowHeight));
			}
		}
		/// <summary>
		/// Areas covering each digit, not including decimal point.
		/// </summary>
		Rectangle[] Area_Places {
			get {
				//Cover entire object height, including both arrows.
				Rectangle[] rtn = new Rectangle[Digits + Precision];
				for (byte w = 0; w < Digits; w++)
				{
					rtn[w] = new Rectangle(w * GWidth, 0, ArrowWidth, (int)Height);
				}
				for (byte d = 0; d < Precision; d++)
				{
					rtn[Digits + d] = new Rectangle(DecStart + (d * GWidth), 0, ArrowWidth, (int)Height);
				}
				return rtn;
			}
		}
					#endregion //Areas
				#endregion //Graphics

				#region Digits
		/// <summary>
		/// Returns an array of digits within <see cref="Value"/>. Example: 100.55 = {1, 0, 0, -1, 5, 5}.
		/// </summary>
		public sbyte[] VDigits {
			get {
				return Math.General.Digits(Value, true);
			}
		}
		/// <summary>
		/// Returns a 2D array of digits within <see cref="Value"/>. Example: 100.55 = { {1, 0, 0}, {5, 5} }.
		/// </summary>
		public sbyte[][] VHalves {
			get {
				return Math.General.SplitDigits(Value);
			}
		}
		/// <summary>
		/// Returns the number of whole-number digits in <see cref="Maximum"/>.
		/// </summary>
		public byte Digits {
			get {
				sbyte rtn = (sbyte)(SMath.Log10(Maximum) + 1);
				return (rtn < 0 ? (byte)1 : (byte)rtn);
			}
		}
				#endregion //Digits

				#region Forms
		public bool IsIn_UpArrows {
			get {
				return Mouse_IsIn(Area_ArrowsUp);
			}
		}
		public bool IsIn_DownArrows {
			get {
				return Mouse_IsIn(Area_ArrowsDown);
			}
		}
		public bool IsIn_Numbers {
			get {
				return Mouse_IsIn(Area_Numbers);
			}
		}
		/// <summary>
		/// {0: Is whole | 1: Is decimal}, {whole: right to left | decimal: left to right}.
		/// </summary>
		public sbyte[] In_Digit {
			get {
				//return -1 if not in any digit.

				//Area_Places.Length = Digits + Precision.
				for (sbyte i = 0; i < Area_Places.Length; i++)
				{
					if (Mouse_IsIn(Area_Places[i]))
					{
						if (i < Digits) return new sbyte[] {0, (sbyte)(Digits - (i + 1))};
						return new sbyte[] {1, (sbyte)(i - Digits)};
					}
				}
				return new sbyte[] {-1};
			}
		}
				#endregion //Forms
			#endregion //ReadOnly
		#endregion //Properties

		#region Constructors
		public CNumericUpDown()
		{
			Redraw();
		}
		public CNumericUpDown(byte brightness, byte hue)
		{
			Recolor(brightness, hue);
			Redraw();
		}
		public CNumericUpDown(byte brightness, byte hue, Color fColor)
		{
			Recolor(brightness, hue, fColor);
			Redraw();
		}
		public CNumericUpDown(byte brightness, byte hue, byte precision, Color fColor)
		{
			Recolor(brightness, hue, fColor);
			Precision = precision;
			Redraw();
		}
		public CNumericUpDown(byte brightness, byte hue, byte fBrightness, byte fHue)
		{
			Recolor(brightness, hue, fBrightness, fHue);
			Redraw();
		}
		public CNumericUpDown(byte brightness, byte hue, byte digits, byte precision, byte fBrightness, byte fHue)
		{
			Recolor(brightness, hue, fBrightness, fHue);
			Redraw();
		}
		public CNumericUpDown(GFont font) : this()
		{
			Font = font;
			Redraw();
		}
		#endregion //Constructors

		#region Subroutines
		public void SetBounds(float minimum, float maximum)
		{
			if (minimum > maximum || maximum < minimum) throw new Exception("Bounds conflict - maximum and minimum may be switched.");
			Minimum = minimum;
			Maximum = maximum;
		}
			#region Graphics
		public void Redraw()
		{
			Image img = new Bitmap((int)Width, (int)Height);
			base.Width = (uint)img.Width;
			base.Height = (uint)img.Height;
			/*Size = img.Size;
			MaximumSize = img.Size;
			MinimumSize = img.Size;*/
			Image arrUp = ArrowUp, arrDown = ArrowDown;
			Graphics G = Graphics.FromImage(img);
			sbyte[][] digits = VHalves;
			sbyte spacesW = (sbyte)(Digits - digits[0].Length); //Whole-Number max digits, minus Whole-Number actual digits.
			sbyte spacesD = (sbyte)(Precision - digits[1].Length); //max Decimal places, minus actual Decimal places.
			if (spacesW < 0) throw new Exception("Error: Digit overflow.");
			if (spacesD < 0) throw new Exception("Error: Precision underflow.");
				
			if (Calibrate)
			{
				G.FillRectangle(Brushes.Aqua, Area_ArrowsUp);
				G.FillRectangle(Brushes.Orange, Area_Numbers);
				G.FillRectangle(Brushes.Aqua, Area_ArrowsDown);
				for (byte i = 0; i < Digits; i++)
				{
					G.FillRectangle(Brushes.Blue, Area_Places[i]);
				}
				for (byte i = 0; i < Precision; i++)
				{
					G.FillRectangle(Brushes.Red, Area_Places[Digits + i]);
				}
			}

			if (FlatStyle)
			{
				if (Calibrate) G.DrawLine(Pens.Red, new Point(10, 10), new Point(11, 10));
				Color fColor = (Enabled ? (Color)clrTxt[2] : DisabledColor);
				//TODO: handle outer empty digits
				for (byte whole = 0; whole < Digits; whole++) //whole
				{
					double val = Value;
					//For 50, spacesW = 1. For 0, spacesW = 2.
					sbyte idx2 = (sbyte)(whole - spacesW); //For 50 result is [whole: result] {0: -1, 1: 0, 2: 1}. For 0, result is {0: -2, 1: -1, 2: 0}.
					int x = whole * GWidth;
						
					G.DrawImage(arrUp, x, 0);
					G.DrawImage(arrDown, x, Height - arrDown.Height);

					if (idx2 > -1) G.DrawImage(Font.PrintRight(new CString("" + digits[0][idx2], fColor), Font.CharWidth), x, FontY);
					//TODO: handle comma separators
				}
				if (Precision > 0) G.DrawImage(Font.PrintRight(new CString(".", fColor), Font.CharWidth), (GWidth * (Digits - 1) + (Spacing * 1.5f)), FontY); //decimal point
				bool hasDec = digits[1].Length > 0;
				for (byte dec = 0; dec < Precision; dec++) //decimal portion
				{
					sbyte idx2 = (sbyte)(dec - spacesD);
					int x = DecStart + (dec * GWidth);

					G.DrawImage(arrUp, x, 0);
					G.DrawImage(arrDown, x, Height - arrDown.Height);
						
					G.DrawImage(Font.PrintRight(new CString("" + (hasDec ? digits[1][idx2] : 0), fColor), Font.CharWidth), x, FontY);
				}
				//G.DrawImage(arrUp, 2, 2);
				//G.DrawImage(arrDown, 2, 12);
			} else { //    \/~~~~~~~~ BOX-BUTTON STYLE ~~~~~~~~\/
				#region Colors
				/*sbyte mod = 0; //Buttons
				switch (typ)
				{
					case 1:
						mod = 1;
						break;
					case 2:
						mod = -1;
						break;
					default:
						mod = 0;
						break;
				}*/
				Pen btnEdge, txtEdge;
				Brush btnFill, btnArr, txtFill;
				txtEdge = classGraphics.CPen((Color)clrTxt[1]);
				txtFill = classGraphics.CBrush((Color)clrTxt[0]);
			
				btnEdge = classGraphics.CPen((Color)clrBtn[1]);
				btnFill = classGraphics.CBrush((Color)clrBtn[0]);
				btnArr = classGraphics.CBrush((Color)clrBtn[2]);
				#endregion //Colors
				uint bWidth = Width / 3;
				uint bHeight = Height / 4;
				uint tHeight = Height / 2;
				#region Background
				G.DrawRectangle(txtEdge, 0, (int)bHeight + 1, (int)Width - 1, (int)tHeight - 1);
				G.FillRectangle(txtFill, 1, (int)bHeight + 2, (int)Width - 2, (int)Height - 2);
				#endregion //Background
				#region Buttons
					uint wLR = (uint)Math.General.FInt((double)Height / 3); //width (left || right)
					uint wM = Width - (wLR * 2); //width (middle)
					uint h = Height / 4; //button height
					//<Buttons>

					//Handle variable number of buttons!
					//TODO: Handle hover and press!
					for (uint i = 0; i <= Height; i += Height - h)
					{
						G.DrawRectangle(btnEdge, 0, i, wLR, h);
						G.DrawRectangle(btnEdge, Width - wLR, i, wLR, h);
						G.DrawRectangle(btnEdge, Width - wLR - wM, i, wM, h);

						G.FillRectangle(btnFill, 1, i + 1, wLR - 2, h - 2);
						G.FillRectangle(btnFill, Width - wLR + 1, i, wLR - 2, h - 2);
						G.FillRectangle(btnFill, Width - wLR - wM + 1, i, wM - 2, h - 2);
					}
					//</Buttons>
				#endregion //Buttons
				#region Text
					//<Textbox>
					G.DrawRectangle(txtEdge, 0, h, Width, Height - (2 * h));
					G.FillRectangle(txtFill, 1, h + 1, Width - 2, Height - (2 * h) - 2);
					//</Textbox>
				
					//TODO: Handle decimals!
					//classGraphics.strFmCenter()
					for (byte i = 0; i < Digits; i++)
					{
					
					}
				#endregion //Text
			}
			Image?.Dispose();
			Image = img;
		}

				#region Recolor
		public void Recolor(byte brightness, byte hue)
		{
			//border = brightness
			//bg = brightness - 4
			//btnBG = brightness + 4
			if ((brightness < 6) || (brightness > 10))
			{
				throw new Exception("brightness must be between 6 and 10!");
			} else {
				clrTxt[1] = classGraphics.GColor(brightness, hue, 0);
				clrTxt[2] = classGraphics.GColor((byte)(brightness - 4), hue, 0);
				clrTxt[3] = clrTxt[1];

				clrBtn[1] = classGraphics.GColor(0, (byte)(brightness + 4), 0);
				clrBtn[2] = clrTxt[3];
			}
			Redraw();
		}
		public void Recolor(byte brightness, byte hue, Color fColor)
		{
			Recolor(brightness, hue);
			clrTxt[2] = fColor;
			Redraw();
		}
		public void Recolor(byte brightness, byte hue, byte fBrightness, byte fHue)
		{
			Recolor(brightness, hue, classGraphics.GColor(fBrightness, fHue, 0));
			Redraw();
		}
			#endregion //Recolor
			#endregion //Graphics
			#region EventHelpers
		public override void OnClick()
		{
			if (PrintDiagnostics) Debug.WriteLine("objNumericUpDown: Click at " + MousePosition.X + ", " + MousePosition.Y);
			if (Calibrate) Debug.WriteLine("Recommend augment offset by (" + (MousePosition.X - 10)  + ", " + (MousePosition.Y - 10) + ").");
			if (Enabled && IsHovered)
			{
				sbyte[] digit = In_Digit;
				if (digit[0] != -1)
				{
					double difference = SMath.Pow(10, (digit[0] == 0 ? digit[1] : 0 - (digit[1] + 1)) );
					if (IsIn_UpArrows)
					{
						if (digit[0] == 0) //Whole
						{
							double newVal = Math.General.RoundF(Value + difference, Precision);
							if (newVal <= Maximum) Value = newVal;
							if (PrintDiagnostics) Debug.WriteLine("\tClick INCREMENT registered at Whole digit for " + difference + "s place, resulting in value " + Value + ".");
						} else { //Decimal
							double newVal = Math.General.RoundF(Value + difference, Precision);
							if (newVal <= Maximum) Value = newVal;
							if (PrintDiagnostics) Debug.WriteLine("\tClick INCREMENT registered at Decimal digit for " + difference + "s place, resulting in value " + Value + ".");
						}
					} else if (IsIn_DownArrows) {
						if (digit[0] == 0) //Whole
						{
							double newVal = Math.General.RoundF(Value - difference, Precision);
							if (newVal >= Minimum) Value = newVal;
							if (PrintDiagnostics) Debug.WriteLine("\tClick DECREMENT registered at Whole digit for " + difference + "s place, resulting in value " + Value + ".");
						} else { //Decimal
							double newVal = Math.General.RoundF(Value - difference, Precision);
							if (newVal >= Minimum) Value = newVal;
							if (PrintDiagnostics) Debug.WriteLine("\tClick DECREMENT registered at Decimal digit for " + difference + "s place, resulting in value " + Value + ".");
						}
					}
				}
				Redraw();
			} //</if>
		} //</OnClick()>
			#endregion //EventHelpers
		#endregion

		#region Functions
		#endregion //Functions

	} //END-ALL
}
