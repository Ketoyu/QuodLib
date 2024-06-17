using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using System.Drawing;
using QuodLib.WinForms.Drawing;
//using System.Windows.Forms;

namespace QuodLib.WinForms.Objects
{
    using Strings;
    public class CButton : CHoverable, ICObject//, ICHoverable
    {
		#region Fields
        public bool Calibrate = false;
        private byte textBrightness = 12;
        public Font font = SystemFonts.DefaultFont;
        public int fSize { get; private set; }
        public byte Brightness = 11;
        public byte Hue = 0;
        private string b_text = "Button";

		#endregion //Fields

        #region Properties
        public static Type Type { get { return typeof(CButton); } }
        public string Text {
            get {
                return b_text;
            }
            set {
                b_text = value;
                Redraw();
            }
        }
        //public bool color = true;
		#endregion //Properties
            
        #region Constructors
        public CButton()
        {
            //color = false;
            fSize = 12;
            Resize(50, 20);
            Recolor(10, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="brightness">0-15</param>
        /// <param name="hue">0=white || 1=red, 2=green, 3=blue ||
        ///                         4=yellow, 5=cyan, 6=violet ||
        ///                         7=orange, 8=lime, 9=sea, 10=teal, 11=purple, 12=pink</param>
        public CButton(int width, int height, byte brightness, byte hue)
        {
            fSize = 12;
            Resize(width, height);
            Recolor(brightness, hue);
        }
        public CButton(int width, int height, byte brightness, byte hue, string text)
        {
            fSize = 12;
            b_text = text;
            Resize(width, height);
            Recolor(brightness, hue);
        }
        public CButton(int width, int height, byte brightness, byte hue, string text, Font font_)
        {
            fSize = 12;
            font = font_;
            b_text = text;
            Resize(width, height);
            Recolor(brightness, hue);
        }
        public CButton(int width, int height, byte brightness, byte hue, string text, FontFamily fontFam)
        {
            fSize = 12;
            font = new Font(fontFam, 12);
            b_text = text;
            Resize(width, height);
            Recolor(brightness, hue);
        }
        public CButton(int width, int height)
        {
            fSize = 12;
            Resize(width, height);
        }
        public CButton(int width, int height, string text, Font font_)
        {
            fSize = 12;
            b_text = text;
            font = font_;
            Resize(width, height, text);
        }
        public CButton(int width, int height, string text, FontFamily fontFam)
        {
            fSize = 12;
            b_text = text;
            font = new Font(fontFam, 12);
            Resize(width, height, text);
        }
        #endregion //Constructors

        #region Subroutines
			#region Graphics
        public override void Redraw()
        {
            #region Colors
            sbyte mod = 0;
            switch (State)
            {
                case MouseState.Hovered:
                    mod = 1;
                    break;
                case MouseState.Pressed:
                    mod = -1;
                    break;
                default: //Normal
                    mod = 0;
                    break;
            }
            byte bri = (byte)(Brightness + mod);
            Brush B;
            Pen D1, D2, L1, L2;
            Color b, d1, l1;
            byte hu = Hue;
            b = classGraphics.GColor((Enabled ? hu : (byte)0), bri, 0); B = classGraphics.CBrush(b);
            d1 = classGraphics.GColor((Enabled ? hu : (byte)0), (byte)(bri - 1), 0); D1 = classGraphics.CPen(d1);
            l1 = classGraphics.GColor((Enabled ? hu : (byte)0), (byte)(bri + 1), 0); L1 = classGraphics.CPen(l1);

            D2 = classGraphics.CPen(classGraphics.CAverage(b, d1));
            L2 = classGraphics.CPen(classGraphics.CAverage(b, l1));
            #endregion //Colors

            #region Draw
            Image img = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(img);
                #region Background
            G.FillRectangle(B, 2, 2, Width - 2 - 2, Height - 2 - 2);

            switch (State)
            {
                case MouseState.Normal: case MouseState.Hovered:
                    #region Normal+Hover
                    //<outer>
                    G.DrawLine(D1, new Point(0, 0), new Point(0, Height - 1));
                    G.DrawLine(D1, new Point(0, Height - 1), new Point(Width, Height - 1));

                    G.DrawLine(L1, new Point(1, 0), new Point(Width - 1, 0));
                    G.DrawLine(L1, new Point(Width - 1, 0), new Point(Width - 1, Height));
                    //</outer>

                    //<inner>
                    G.DrawLine(D2, new Point(1, 1), new Point(1, Height - 1 - 1));
                    G.DrawLine(D2, new Point(1, Height - 1 - 1), new Point(Width - 1 - 1, Height - 1 - 1));

                    G.DrawLine(L2, new Point(2, 1), new Point(Width - 1 - 1, 1));
                    G.DrawLine(L2, new Point(Width - 1 - 1, 1), new Point(Width - 1 - 1, Height - 1 - 1));
                    //</outer>
                    #endregion //Normal
                    break;
                case MouseState.Pressed:
                    #region Pressed
                    //<outer>
                    G.DrawLine(L1, new Point(0, 0), new Point(0, Height - 1));
                    G.DrawLine(L1, new Point(0, Height - 1), new Point(Width, Height - 1));

                    G.DrawLine(D1, new Point(1, 0), new Point(Width - 1, 0));
                    G.DrawLine(D1, new Point(Width - 1, 0), new Point(Width - 1, Height));
                    //</outer>

                    //<inner>
                    G.DrawLine(L2, new Point(1, 1), new Point(1, Height - 1 - 1));
                    G.DrawLine(L2, new Point(1, Height - 1 - 1), new Point(Width - 1 - 1, Height - 1 - 1));

                    G.DrawLine(D2, new Point(2, 1), new Point(Width - 1 - 1, 1));
                    G.DrawLine(D2, new Point(Width - 1 - 1, 1), new Point(Width - 1 - 1, Height - 1 - 1));
                    //</outer>

                    #endregion
                    break;
            }
                #endregion //Background
                #region Text
            //uint strW = Graphics.MeasureString(string, font);

            G.DrawString(Text, font, classGraphics.GBrush(0, this.textBrightness, 0), new Rectangle(0, 0, Width, Height), classGraphics.strFmCenter());
                #endregion //Text
            #endregion //Draw
            Image = img;
        }
        public void Recolor(byte brightness, byte hue)
        {
            if ((brightness > 0) && (brightness < 11)) {
                Brightness = brightness;
                Hue = hue;
                textBrightness = (byte)(0 - (brightness - 12));
                Redraw();
            } else {
                throw new Exception("Brightness must be between 1 and 10!"); //+1 for light edging, then +1 for hover-state
            }
        }
		public void SetTextBrightness(byte brightness)
		{
            if (brightness < 12) {
                textBrightness = brightness;
                Redraw();
            } else {
                throw new Exception("Text brightness must be between 0 and 11!"); //+1 for light edging, then +1 for hover-state
            }
		}
        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;
            if (Text != string.Empty) {
                ////Handle multi-line text
                if (Text.Contains("\n")) {
                    int length = 0;
                    string spl;
                    if (Text.Contains("\r\n")) {
                        spl = "\r\n";
                    } else {
                        spl = "\n";
                    }
                    int lines = Text.SplitCount(spl);
                    for (int i = 0; i <= lines; i++) {
                        string splT = Text.Split(spl)[i];
                        if (splT.Length > length)
                            length = splT.Length;
                    }
                    fSize = (width / length);
                    if ((fSize) > (Height / 2))
                        fSize = (int)((height * (3 / 4f)) / Math.General.CInt((double)lines));
                } else {
                    fSize = width / (Text.Length);
                    if ((fSize) > (Height / 2))
                        fSize = (int)(height * (3 / 4f));
                }
                font = new Font(font.FontFamily, fSize);
            }
            Redraw();
        }
        public void Resize(int width, int height, string text)
        {
            b_text = text;
            Resize(width, height);
        }
			#endregion //Graphics
			#region EventHelpers
        public override void OnMouseUp()
        {
            base.OnMouseUp();
            if (Calibrate) System.Diagnostics.Debug.WriteLine("Control-relative MouseClick registered at (" + MousePosition.X + ", " + MousePosition.Y + "). Is " + (IsHovered ? "" : " not") + " hovered.");
        }
			#endregion //EventHelpers
        #endregion //Subroutines
    } //END-ALL
}
