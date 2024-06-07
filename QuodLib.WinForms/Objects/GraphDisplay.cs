using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;
using cG = QuodLib.WinForms.Drawing.classGraphics;
using QuodLib.Objects;

namespace QuodLib.WinForms.Objects
{
	/// <summary>
	/// A wrapper, containing a compiled image, for (a) set(s) of data-points.
	/// </summary>
    public class GraphDisplay : CObject, ICObject
    {
        /*class GScale
        {
            public byte YBase, YPix, YStep, XPix, XStep;
        }*/

        /*TYPE CONVENTIONS
            * Data: ushort
            * DataField indexes: byte
            * Averages/Sums: float
            * */

        /*OBJECTIVES
            * ~ Bar graph: primary data
            * ~ ( transluscent bars || opaque lines ): average
            * ~ Average: running average, past-x average.
            * 
            * */

        #region Fields
        private List<GraphData> Data = new List<GraphData>();
        /// <summary>
		/// The number of datasets contained.
		/// </summary>
		public byte Datasets;
        private bool editing;
            #region Graphics
                #region Constants
        const byte CASING_LEFTWIDTH = 19;
        const byte CASING_RIGHTWIDTH = 6;
        const byte CONFIG_HEIGHT = 121;
        const byte CASING_BOTTOMHEIGHT = 15;
        const byte NOTCHHEIGHT_X = 1;
        const byte NOTCHHEIGHT_STEP = 3;
                #endregion //Constants
        /// <summary>
        /// 0: BG | 1: TopmostToggle main | 2: TopmostToggle secondary | 3: NumberArrows | 4: ConfigCog |
        /// 5: ConfigArrow | 6: XConfig | 7: YConfig | 8: RegionConfig
        /// </summary>
        public Color[] Colors;
        /// <summary>
        /// Whether to gradually draw (to the next point)/(the next bar).
        /// </summary>
        public bool Animated;
        /// <summary>
        /// True: Lines &amp; dots
        /// False: Bar graph
        /// </summary>
        public bool IsLine = true;
        /// <summary>
		/// The Image final Image of the GraphDisplay.
		/// </summary>
		public new Image Image { get; private set; }
        private Image ImageStatic;
        private Image ImageData;
            #endregion //Graphics
            #region Controls
        /// <summary>
        /// 0=XMax | 1=YMin, 2=YMax | 3=YScale, 4=YPix, 5=YMark | 6=XPix, 7=XMark | 8=Regions, 9=SelRegion | 10=SelMin, 11=SelMax.
        /// </summary>
        CNumericUpDown[] Specs {
            get {
                return (from ctl in Controls where ctl.GetType() == CNumericUpDown.Type select (CNumericUpDown)ctl).ToArray<CNumericUpDown>();
            }
        }
        /// <summary>
        /// 0: {0=XMax} | 1: {0=YMin, 1=YMax} | 2: {0=YScale, 1=YPix, 2=YMark} | 3: {0=XPix, 1=XMark} | 4: {0=Regions, 1=SelRegion} | 5: {0=SelMin, 1=SelMax}.
        /// </summary>
        CNumericUpDown[][] SpecsSplit {
            get {
                return new CNumericUpDown[][] {
                    new CNumericUpDown[] { Specs[0] },
                    new CNumericUpDown[] { Specs[1], Specs[2] },
                    new CNumericUpDown[] { Specs[3], Specs[4], Specs[5] },
                    new CNumericUpDown[] { Specs[6], Specs[7] },
                    new CNumericUpDown[] { Specs[8], Specs[9] },
                    new CNumericUpDown[] { Specs[10], Specs[11] }
                };
            }
        }
            #endregion //Controls
        #endregion //Fields

        #region Properties
        /// <summary>
        /// {0-11: CNums, 12-13: RadioTopmost, 14: ConfigToggle, 15: ColorPick}
        /// </summary>
        CObject[] Controls;
            #region ReadOnly
                #region CNums
        public byte XMax { get { return (byte)SpecsSplit[0][0].Value; } }
        public double YMin { get { return SpecsSplit[1][0].Value; } }
        public double YMax { get { return SpecsSplit[1][1].Value; } }
        public byte YScale { get { return (byte)SpecsSplit[2][0].Value; } }
        public byte YPix { get { return (byte)SpecsSplit[2][1].Value; } }
        public byte YStep { get { return (byte)SpecsSplit[2][2].Value; } }
        public byte XPix { get { return (byte)SpecsSplit[3][0].Value; } }
        public byte XStep { get { return (byte)SpecsSplit[3][1].Value; } }
        public byte RegionCount { get { return (byte)SpecsSplit[4][0].Value; } }
        public byte RegionSel { get { return (byte)SpecsSplit[4][1].Value; } }
        public double RegionSel_Min { get { return SpecsSplit[5][0].Value; } }
        public double RegionSel_Max { get { return SpecsSplit[5][1].Value; } }
                #endregion //CNums
        /// <summary>
        /// [#]: nth region | [#]{ 0: YMin | 1: YMax }.
        /// </summary>
        public List<double[]> RegionBounds { get; private set; }
                #region Graphics
        public new uint Width {
            get {
                return (uint)(CASING_LEFTWIDTH + GraphWidth + CASING_RIGHTWIDTH);
            }
        }
        public new uint Height {
            get {
                return (uint)(CASING_LEFTWIDTH + CASING_BOTTOMHEIGHT + (EditMode ? CONFIG_HEIGHT : 0));
            }
        }
        private ushort GraphHeight {
            get {
                return (ushort)(Pix_YScale * (YMax - YMin));
            }
        }
        private ushort GraphWidth {
            get {
                return (ushort)(XMax * XPix);
            }
        }
        private ushort Pix_XStep {
            get {
                return (ushort)(XStep * XPix);
            }
        }
        private ushort Pix_YScale {
            get {
                return (ushort)(YPix / YScale);
            }
        }
        private ushort Pix_YStep {
            get {
                return (ushort)((YPix * YStep) / YScale);
            }
        }
                #endregion //Graphics
            #endregion //ReadOnly
            #region Form
        public new Point Location {
            get {
                return base.Location;
            }
            set {
                base.Location = value;
                foreach (CObject ctl in Controls) ctl.ContainerLocation = value;
            }
        }
        public new Point MouseOffset {
            get {
                return base.MouseOffset;
            }
            set {
                base.MouseOffset = value;
                foreach (CObject ctl in Controls) ctl.MouseOffset = value;
            }
        }
        public bool EditMode {
            get {
                return editing;
            }
            set {
                editing = value;
                ImageStatic_Update();
                Image_Compose();
            }
        }
            #endregion //Form
        #endregion //Properties
            
        //~~~~ CONSTRUCTOR ~~~~
        public GraphDisplay()
        {
            Data.Add(new GraphData());
            Controls = new CObject[12];
            for (byte i = 0; i < 12; i++) Controls[i] = new CNumericUpDown(QuodLib.WinForms.Drawing.Objects.classFonts.Fonts.FurDomus_12x18_bold);
            SpecsSplit[0][0].SetBounds(4, 99);
            for (byte i = 0; i < 2; i++)
            {
                SpecsSplit[1][i].Precision = 1; //YMin, YMax
                SpecsSplit[5][i].Precision = 1; //SelMin, SelMax

                SpecsSplit[2][i+1].SetBounds(1, 99); //YPix, YMark
                SpecsSplit[3][i].SetBounds(1, 99); //XPix, XMark

                SpecsSplit[4][i].SetBounds(1, 9); //Regions, SelRegion
            }
            SpecsSplit[2][0].SetBounds(1, 9); //YScale
            Location = new Point(0, 0);
            ConstrainNumerics();

            //ConfigToggle and ColorPick handled dynamically.
        }

        #region Subroutines
        public override void OnClick()
        {
            ConstrainNumerics();
            foreach (CObject ctl in (from ctl in Controls where (ctl.IsHovered && ctl.Enabled) select ctl)) ctl.OnClick();
            Redraw();
        }
        /// <summary>
        /// Update Locations of all controls (CNumericUpDowns, config toggle, Color-Pick).
        /// </summary>
        private void MoveControls()
        {
            //TODO: Move controls based on text positions/dimensions.


        }
        private void ConstrainNumerics()
        {
            //Graph's Y-bounds
            ConstrainPair(SpecsSplit[1][0], SpecsSplit[1][1]);
            //All region bounds
            //      Go through each region
            //RegionSel bounds (based on [region bounds] list)
        }
        private void ConstrainPair(CNumericUpDown low, CNumericUpDown high)
        {
            if (low.Value > high.Value) //if (LowVal > HighVal) (data is invalid)
            {
                low.Maximum = high.Value; //LowMax = HighVal (LowVal auto-adjusts).
                high.Minimum = low.Maximum; //HighMin = LowMax (HighMin = auto-adjusted LowVal).
            } else { //low <= high (data is therefore valid)
                high.Minimum = low.Value; //HighMin = LowVal (HighVal auto-adjusts).
                low.Maximum = high.Minimum; //LowMax = HighMin (LowMax = auto-adjusted HighVal).
            }
        }
        /*public void PushDataValue(byte dataField, float value)
        {
            //TODO: create functionality for switching between sub-graph configs.
            //          (Encase Scale properties in a sub-class?)
            if (dataField >= Data.Length) throw new Exception("Error: DataField \"" + dataField + "\" ourside limit (" + (Data.length - 1) + ".");
            //TODO: create toggle for sub-graph visibility.
                
        }*/
        public void PushDataValue(float value)
        {
            Data[0].Push(value);
            Redraw();
        }
            #region Graphics
        public void Redraw()
        {
            MoveControls(); //TODO: Move to less-frequently accessed area.

            ImageStatic_Update();
            ImageData_Update();
            Image_Compose();
        }
        private void Image_Compose()
        {
            //Combine ImageData (back) and ImageStatic (front).
        }
        /// <summary>
        /// Update the casing, controls and data-background.
        /// </summary>
        private void ImageStatic_Update()
        {
            //Draw casing/controls here.
            Image img = new Bitmap((int)Width, GraphHeight + CASING_BOTTOMHEIGHT + (editing ? 0 : CONFIG_HEIGHT));
            base.Width = (uint)img.Width;
            base.Height = (uint)img.Height;
            Graphics G = Graphics.FromImage(img);


            #region DataBG
            //Regions

            //Lines

            #endregion //DataBG

            #region Casing

            G.FillRectangle(cG.MBrush(12), 0, 0, CASING_LEFTWIDTH, Height); //left
            G.DrawLine(cG.MPen(12), new Point(CASING_LEFTWIDTH - 1, 0), new Point(CASING_LEFTWIDTH + GraphWidth - 1, 0)); //top
            G.FillRectangle(cG.MBrush(12), CASING_LEFTWIDTH + GraphWidth, 0, CASING_RIGHTWIDTH, GraphHeight + CASING_BOTTOMHEIGHT); //right

            //Surround notches
            for (byte i = 0; i < XMax; i += 1)
            {
                int x = CASING_LEFTWIDTH + (i*XPix), y = GraphHeight + 1;
                G.FillRectangle(cG.MBrush(12), x, y, XPix - 1, CASING_BOTTOMHEIGHT); //Rectangle
                G.DrawLine(cG.MPen(12), new Point(x + XPix - 2, y + (XStep % i == 0 ? NOTCHHEIGHT_STEP : NOTCHHEIGHT_X)), new Point(x + XPix - 2, CASING_BOTTOMHEIGHT)); //Below notch
            }
                
            //Notch text

            //Config toggle

            #endregion

            #region Config
            //If (config), draw config-BG and controls. Else, draw X-marks.
                
            #endregion //Config

            G.Dispose();
            ImageData = img;
        }
        /// <summary>
        /// Update the graph image.
        /// </summary>
        private void ImageData_Update()
        {
            //Draw graph here.
        }
            #endregion //Graphics
        #endregion //Subroutines
    } //<end-class>
}
