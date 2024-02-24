using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;
using QuodLib.Drawing;

namespace QuodLib.WinForms.Objects
{
	/// <summary>
	/// An Image-based control for changing the color-palette of a containing object.
	/// </summary>
    class objCPick : CObject, ICObject
    {
        #region Properties
            
        private byte _ColorDepth, _Brightness, _Hue; //subProperties
        private Point mPos
        {
            get {
                Point mp = System.Windows.Forms.Control.MousePosition;
                return new Point(mp.X - Location.X, mp.Y - Location.Y);

            }
        }
        private Color[][] Colors; //referenced upon 
        private byte[] selColor;
        /*public byte[] ControlPos
        {
            //0=Outside || 1=ColorDictionary || 1=Typ:: 0=Up, 1=Lf, 2=Dn || 3=Arrows:: 0=Up, 1=Dn || 4=Shade:: [0, {10,12}]
            get {
                List<List<classObjects.RectByte>> CtrlPos = new List<List<classObjects.RectByte>>();
                CtrlPos.Add(new List<classObjects.RectByte>()); //[0] ColorDictionary
                for (byte i = 0; i < Colors[0].Count(); i++)
                {

                }
            }
        }*/
        public byte LBrightness
        {
            get {
                return 0;
            }
        }
        public byte HBrightness
        {
            get {
                switch (ColorDepth)
                {
                    case 0:
                    case 1:
                        return 12;
                    default:
                        return 10;
                }
            }
        }
        public byte ColorDepth
        {
            get {
                return _ColorDepth;
            }
            set {
                this._ColorDepth = Math.General.Limit_H(value, (byte)2);
            }
        }
        public byte Brightness
        {
            get {
                return _Brightness;
            }
            set {
                this._Brightness = Math.General.Limit(value, (byte)LBrightness, (byte)HBrightness);
            }
        }
        public byte Hue
        {
            get {
                return _Hue;
            }
            set {
                this._Hue = Math.General.Limit_H(value, (byte)12);
            }
        }
            
        public Color Color {
            get {
                return classGraphics.GColor(Hue, Brightness, ColorDepth);
            }
        }
            #region Imaging
            /// <summary>
            /// [section][item] {Section [items]} | { <br />
            ///     - 0: [0: Overlay] | 1: RingInner[0: bars | 1: rightmost | 2-14: (clockwise)] | <br />
            ///     - 2: RingOuter[0-3: Home, Rightmost + 2 | 4-6: Bottom + 2 | 7-9: Leftmost + 2 | 10-12: Topmost + 2] | <br />
            ///     - 3: Arrows[0: Top outer | 2: Bottom outer | 3: Top shade | 4: Bottom shade] | <br />
            ///     - 4: TypeSwitch[0: Inner circle | 1: OuterTop | 2: OuterLeft | 3: OuterBottom | 4: InnerTop | 5: InnerLeft | 6: InnerBottom] | <br />
            ///     - 5: Shades[0: Selected | 1: Selected_dots | 2: Unselected | 4: Big divider | 5: Small divider] <br />
            ///     - 6: Transparency[0: Background image] <br />
            /// }
            /// </summary>
            private Rectangle[][] CPSrc = new Rectangle[][] {
                new Rectangle[] { //0: Overlay
                    new Rectangle(0, 0, 37, 36)
                },
                new Rectangle[] { //1: RingInner
                    new Rectangle(67, 16, 5, 5), //Bars
                    new Rectangle(73, 15, 1, 5), new Rectangle(71, 23, 2, 3), new Rectangle(66, 28, 3, 2), //Home, Rigtmost + 2
                    new Rectangle(58, 30, 5, 1), new Rectangle(53, 28, 3, 2), new Rectangle(49, 23, 2, 3), //Bottom + 2
                    new Rectangle(48, 15, 1, 5), new Rectangle(49, 10, 2, 3), new Rectangle(53, 6, 3, 2), //Leftmost + 2
                    new Rectangle(58, 5, 5, 1), new Rectangle(66, 6, 3, 2), new Rectangle(71, 10, 2, 3) //Topmost + 2
                },
                new Rectangle[] { //2: RingOuter
                    new Rectangle(110, 15, 1, 5), new Rectangle(107, 23, 3, 4), new Rectangle(102, 28, 4, 3), //Rightmost + 2
                    new Rectangle(94, 31, 5, 1), new Rectangle(88, 28, 4, 3), new Rectangle(84, 23, 3, 4), //Bottom + 2
                    new Rectangle(83, 15, 1, 5), new Rectangle(84, 9, 3, 4), new Rectangle(88, 5, 4, 3), //Leftmost + 2
                    new Rectangle(94, 4, 5, 1), new Rectangle(102, 5, 4, 3), new Rectangle(107, 9, 3, 4)
                },
                new Rectangle[] { //3: Arrows
                    new Rectangle(141, 2, 7, 12), new Rectangle(141, 22, 7, 12), //Outer
                    new Rectangle(178, 1, 7, 6), new Rectangle(178, 29, 7, 6) // Inner/shading
                },
                new Rectangle[] { //4: TypeSwitch
                    new Rectangle(5,5,10,10), new Rectangle(24,36,12,4), new Rectangle(40,40,4,12), new Rectangle(64,51,12,4), //Inner circle, outer crescents
                    new Rectangle(86,39,8,3), new Rectangle(103,42,3,8), new Rectangle(126,50,8,3), //Inner crescents
                },
                new Rectangle[] { //5: Shades
                    new Rectangle(0,56,5,16), new Rectangle(12,57,1,14), new Rectangle(20,61,5,6), //Selected, Selected_dots, Unselected
                    new Rectangle(5,72,3,10), new Rectangle(16,76,1,2), //Big divider, small divider
                },
                new Rectangle[] { //6: Transparency
                    new Rectangle(0, 82, 104, 18) //Background Image
                }
            };
            private Point[][] CPDest = new Point[][] {
                        new Point[] {

                        }
                    };
            public new Image Image {
                get {
                    //0=overlay, 1=ringInner, 2=ringOuter, 3=arrowMain, 4=arrowTrim, 6=circ, 7=circUp, 8=circLf, 9=circDn, 10=circSelUp, 11=circSelLf, 12=circSelDn, 11=shdSel, 12=shdSel2, 13=shd, 14=divBig, 15=div
                    #region BaseImages
                    Image[] CPImgs = new Image[] {}; //leave one-dimensional
                    #endregion //BaseImages

                    //System.Drawing.Image rtn = new Bitmap(_51_, 17);
                    //Graphics G = Graphics.FromImage(rtn);

                    //return rtn;
                    return new Bitmap(200,20); //Replace with above.
                }
            }
            #endregion //Imaging
        #endregion //Properties
        public objCPick(Point location, byte hue, byte brightness, byte colorDepth) //Constructor
        {
            this._ColorDepth = 0; this._Brightness = 0; this._Hue = 0;
            this.Location = location;
            this.Hue = hue;
            this.ColorDepth = colorDepth;
            this.Brightness = brightness;
        }
        #region Nudges
        public void HueLeft()
        {
            if (Hue == 0)
            {
                Hue = 12;
            } else {
                Hue--;
            }
        }
        public void HueRight()
        {
            if (Hue == 12)
            {
                Hue = 0;
            } else {
                Hue++;
            }
        }
        public void BrightnessUp()
        {
            switch (ColorDepth)
            {
                case 0: case 1: //Pale/Pure[0-12]
                    if (Brightness >= 12)
                    {
                        Brightness = 0;
                    } else {
                        Brightness++;
                    }
                    break;
                default: //Light[0-10]
                    if (Brightness >= 10)
                    {
                        Brightness = 0;
                    } else {
                        Brightness++;
                    }
                    break;
            }
        }
        public void BrightnessDown()
        {
            switch (ColorDepth)
            {
                case 0: case 1: //Pale/Pure[0-12]
                    if (Brightness <= 0)
                    {
                        Brightness = 12;
                    } else {
                        Brightness--;
                    }
                    break;
                default: //Light[0-10]
                    if (Brightness == 0)
                    {
                        Brightness = 10;
                    } else {
                        Brightness--;
                    }
                    break;
            }
        }
        public void ColorDepthClockwise()
        {
            if (ColorDepth == 0)
            {
                ColorDepth = 2;
            } else {
                ColorDepth--;
            }
        }
        #endregion //Nudges

        public void Redraw()
        {

        }
    }
}
