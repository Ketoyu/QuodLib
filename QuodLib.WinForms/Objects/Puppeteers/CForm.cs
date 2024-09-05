using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;
using QuodLib.WinForms.Drawing;
using QuodLib.Objects;

namespace QuodLib.WinForms.Objects.Puppeteers
{
    public class CForm : Puppeteer<Form>
    {
        #region Properties
        #region Fields
        #region Constants
        /// <summary>
        /// The height of the toolbar.
        /// </summary>
        private const byte TOOLBAR_HEIGHT = 30;
        /// <summary>
        /// The width of a single toolbar-button.
        /// </summary>
        private const byte BUTTON_WIDTH = 45;
        #endregion //Constants

        private MouseStatus MS;
        private bool closeable = true;

        #region Config
        /// <summary>
        /// 0=BG, 1=edgeOutside, 2=edgeFocused
        /// </summary>
        private Color[] clrForm = new Color[3];
        /// <summary>
        /// 0=MenuBG, 1=Normal_Hover, 2=Normal_Pressed, 3=close_Hover, 4=close_Pressed
        /// </summary>
        private Color[] clrButtons = new Color[5];
        /// <summary>
        /// 0=Normal, 1=close_NormalBlend, 2=close_Hover, 3=close_HoverBlend, 4=closed_PressedBlend
        /// </summary>
        private Color[] clrFonts = new Color[5];
        #endregion //Config

        #region Info
        /// <summary>
        /// 
        /// </summary>
        public Point Log_FormPos;
        /// <summary>
        /// 
        /// </summary>
        public Point Log_MousePos;
        /// <summary>
        /// 
        /// </summary>
        public Point Log_MouseRel;
        /// <summary>
        /// 
        /// </summary>
        public Size Log_FormSize;
        #endregion //Info
        #endregion //Fields
        #region GetSet
        public bool Maximized
        {
            get
            {
                return Puppet.WindowState == FormWindowState.Maximized;
            }
            set
            {
                Puppet.WindowState = value ? FormWindowState.Maximized : FormWindowState.Normal;
            }
        }
        public bool Maximizable
        {
            get
            {
                return Puppet.MaximizeBox;
            }
            set
            {
                Puppet.MaximizeBox = value;
                updateBG();
            }
        }
        public bool Minimizable
        {
            get
            {
                return Puppet.MinimizeBox;
            }
            set
            {
                Puppet.MinimizeBox = value;
            }
        }
        public bool Closable
        {
            get
            {
                return closeable;
            }
            set
            {
                closeable = value;
                Draw();
            }
        }
        #endregion //GetSet
        #region ReadOnly
        private Point MousePosition { get { return Control.MousePosition; } }
        /// <summary>
        /// The x coordinate where the button-group is drawn.
        /// </summary>
        /// <returns></returns>
        private int startBtns //assumes mousePos() has been performed.
        {
            get
            {
                int numBtns = (Minimizable ? 1 : 0) + (Maximizable ? 1 : 0) + (Closable ? 1 : 0);
                return Puppet.Size.Width - BUTTON_WIDTH * numBtns - 1;
            }
        }
        #endregion //ReadOnly
        #endregion //Properties

        #region Constructors
        public CForm(Form puppet) : base(puppet)
        {
            AttachPuppet(puppet);

            Recolor(0, 0, classGraphics.CColor(191, 127, 11));
            updateBG();
        }
        public CForm(Form puppet, byte hue, byte brightness, Color edge) : base(puppet)
        {
            AttachPuppet(puppet);

            Recolor(hue, brightness, edge);
            updateBG();
        }
        #endregion //Constructors

        #region Subroutines
        protected override void AttachPuppet(Form puppet)
        {
            base.AttachPuppet(puppet);
            MS = new MouseStatus(puppet);

            Puppet.SizeChanged += CForm_SizeChanged;
            Puppet.Load += CForm_Load;
            Puppet.MouseDown += CForm_MouseDown;
            Puppet.MouseUp += CForm_MouseUp;
            Puppet.MouseEnter += CForm_MouseEnter;
            Puppet.MouseLeave += CForm_MouseLeave;
            Puppet.DoubleClick += CForm_DoubleClick;
            Puppet.MouseMove += CForm_MouseMove;

            Puppet.FormBorderStyle = FormBorderStyle.None;
        }
        #region Form_EventHandlers
        private void CForm_SizeChanged(object sender, EventArgs e)
        {
            updateBG();
        }
        private void CForm_Load(object sender, EventArgs e)
        {
            updateBG();
        }
        private void CForm_MouseDown(object sender, MouseEventArgs e)
        {
            MS.OnMouseDown();
            mouseDown();
        }
        private void CForm_MouseUp(object sender, MouseEventArgs e)
        {
            MS.OnMouseUp();
            mouseUp();
        }
        private void CForm_MouseEnter(object sender, EventArgs e)
        {
            updateBG();
        }
        private void CForm_MouseLeave(object sender, EventArgs e)
        {
            updateBG();
        }
        private void CForm_DoubleClick(object sender, EventArgs e)
        {
            doubleClickBG();
        }
        private void CForm_MouseMove(object sender, EventArgs e)
        {
            MS.OnMouseMove();
            mouseMove();
        }
        #endregion //Form_EventHandlers
        #region Event_Assistors
        protected void updateBG()
        {
            updateFormLogs();
            if (!Puppet.IsDisposed)
            {
                Graphics T = Puppet.CreateGraphics();
                Image BG_ = Draw();
                T.DrawImageUnscaled(BG_, 0, 0);
                BG_.Dispose();
                T.Dispose();
            }
        }
        protected void clickBG()
        {
            byte action = mouseClick();
            switch (action)
            {
                case 1:
                    Puppet.WindowState = FormWindowState.Minimized;
                    break;
                case 2:
                    toggleMaximized();
                    break;
                case 3:
                    Puppet.Close();
                    break;
            }
            updateBG();
        }
        protected void doubleClickBG()
        {
            byte action = mouseClick();
            if (action == 4) toggleMaximized();

            updateBG();
        }
        protected void mouseDown()
        {
            updateMouseLogs();
            updateBG();
        }
        protected void mouseUp()
        {
            if (MS.IsDragging)
                if (!Maximized)
                {
                    if (MousePosition.Y == 0)
                        toggleMaximized();
                    else if (Puppet.Location.Y < 0)
                        Puppet.Location = new Point(Puppet.Location.X, 0);
                }

            updateMouseLogs();
            clickBG();
        }
        protected void mouseMove()
        {
            if (!MS.IsDragging) updateBG();
        }
        private void toggleMaximized()
        {
            //Handle taskbar
            //  - https://winsharp93.wordpress.com/2009/06/29/find-out-size-and-position-of-the-taskbar/
            if (Maximized)
            {
                MS.AllowDragging = true;
                Maximized = false;
                bool logged = false;
                do
                {
                    if (Puppet.WindowState == FormWindowState.Normal)
                    {
                        Puppet.Location = Log_FormPos;
                        logged = true;
                    }
                } while (logged == false);
                Puppet.Size = Log_FormSize;
            }
            else
            {
                updateMouseLogs();
                Maximized = true;
                MS.AllowDragging = false;
            }
        }
        /// <summary>
        /// Updates the logs for the Mouse literal- and relative-position. Used for toggling <see cref="Maximized"/>. Also calls <see cref="updateFormLogs()"/>.
        /// </summary>
        private void updateMouseLogs()
        {
            Log_MousePos = MousePosition;
            Log_MouseRel = MS.RelativePosition;
            updateFormLogs();
        }
        /// <summary>
        /// Updates the logs for Form position and size. Used for toggling <see cref="Maximized"/>.
        /// </summary>
        private void updateFormLogs()
        {
            if (!Maximized)
            {
                Log_FormPos = Puppet.Location;
                Log_FormSize = Puppet.Size;
            }
        }
        #endregion //Event_Assistors
        #region Recolor
        public void Recolor(byte hue, byte brightness, Color edge)
        {
            if (brightness <= 4)
            {
                Recolor(hue, brightness);
                clrForm[2] = edge; //edgeFocused
            }
        }
        public void Recolor(byte hue, byte brightness)
        {
            Recolor(hue, brightness, 2);
        }
        public void Recolor(byte hue_BG, byte brightness_BG, byte hue_Trim, byte brightness_Trim)
        {
            Recolor(hue_BG, brightness_BG, 0);
            Recolor(hue_Trim, brightness_Trim, 1);
        }
        public void Recolor(byte hue_BG, byte brightness_BG, byte hue_Trim, byte brightness_Trim, Color edge)
        {
            Recolor(hue_BG, brightness_BG, 0);
            Recolor(hue_Trim, brightness_Trim, 1);
            clrForm[2] = edge; //edgeFocused
        }
        public void Recolor(byte hue_BG, byte brighness_BG, Color trim, bool edgeFocused_IsTrim)
        {
            Recolor(hue_BG, brighness_BG);
            if (edgeFocused_IsTrim)
            {
                clrForm[2] = trim; //edgeFocused
            }
            clrFonts[0] = trim; //Normal
            clrFonts[1] = classGraphics.CAverage(clrFonts[0], clrForm[0]); //close_NormalBlend
        }
        public void Recolor(byte hue_BG, byte brighness_BG, Color trim, Color edge)
        {
            Recolor(hue_BG, brighness_BG);
            clrForm[2] = trim; //edgeFocused
            clrFonts[0] = trim; //Normal
            clrFonts[1] = classGraphics.CAverage(clrFonts[0], clrForm[0]); //close_NormalBlend
        }
        public void RecolorBG(byte hue, byte brightness)
        {
            clrForm[0] = classGraphics.GColor(hue, (byte)(3 + brightness), 0); //BG
            clrForm[1] = classGraphics.GColor(hue, (byte)(3 + brightness), 0); //edgeOutside

            clrButtons[0] = classGraphics.GColor(hue, (byte)(4 + brightness), 0); //MenuBG
            clrButtons[1] = classGraphics.GColor(hue, (byte)(6 + brightness), 0); //Normal_Hover
            clrButtons[2] = classGraphics.GColor(hue, (byte)(5 + brightness), 0); //Normal_Pressed

            clrFonts[1] = classGraphics.CAverage(clrFonts[0], clrForm[0]); //close_NormalBlend
        }
        public void RecolorTrim(byte hue, byte brightness)
        {
            if (brightness <= 2)
            {
                clrForm[2] = classGraphics.GColor(hue, (byte)(7 + brightness), 0); //edgeFocused
                clrFonts[0] = classGraphics.GColor(hue, (byte)(8 + brightness), 0); //Normal
            }
            else
            {
                clrForm[2] = classGraphics.GColor(0, 10, 0); //edgeFocused

                clrFonts[0] = classGraphics.GColor(0, 6, 0); //Normal
            }

            clrFonts[1] = classGraphics.CAverage(clrFonts[0], clrForm[0]); //close_NormalBlend
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hue"></param>
        /// <param name="brightness"></param>
        /// <param name="colorType">0=BG, 1=Trim, 2=BG+Trim</param>
        private void Recolor(byte hue, byte brightness, byte colorType)
        {
            if (brightness <= 4)
            {
                if (colorType == 0 || colorType == 2)
                {
                    RecolorBG(hue, brightness);
                }
                if (colorType == 1 || colorType == 2)
                {
                    RecolorTrim(hue, brightness);
                }

                #region close_Hover,_Pressed(Blend)
                clrButtons[3] = classGraphics.CColor(232, 17, 35); //close_Hover
                clrButtons[4] = classGraphics.CColor(122, 14, 23); //close_Pressed
                clrFonts[2] = classGraphics.GColor(0, 12, 0); //close_Hover
                clrFonts[3] = classGraphics.CAverage(classGraphics.GColor(0, 12, 0), classGraphics.CColor(232, 17, 35)); //close_HoverBlend
                clrFonts[4] = classGraphics.CAverage(classGraphics.GColor(0, 12, 0), classGraphics.CColor(122, 14, 23)); //close_PressedBlend
                #endregion //close_Hover,_Pressed(Blend)
            }
        }
        #endregion //Recolor
        #endregion //Subroutines
        #region Functions
        #region Modifying
        /// <summary>
        /// Moves the form as the user drags the mouse. WARNING: Make sure to use updateMouse beforehand!
        /// </summary>
        private Point DragForm()
        {
            Point pos = MousePosition;
            byte itm = InItem();
            if (IsIn_Toolbar() && MS.IsDown || MS.IsDragging)
            {
                //if (!Dragging) { Dragging = true; }
                if (Maximized)
                {
                    int mDiff = MousePosition.Y - Log_MousePos.Y;
                    int cfThresh = TOOLBAR_HEIGHT / 2;
                    if (mDiff > cfThresh)
                    {
                        int x = MousePosition.X - Log_FormSize.Width / 2;
                        if (x + Log_FormSize.Width > Screen.PrimaryScreen.WorkingArea.Width)
                            x = Screen.PrimaryScreen.WorkingArea.Width - Log_FormSize.Width;

                        toggleMaximized();
                        Puppet.Location = new Point(x, Puppet.Location.Y);
                        Log_MousePos = new Point(MousePosition.X, Log_MousePos.Y);
                        Log_MouseRel = new Point(MS.RelativePosition.X, Log_MouseRel.Y);
                        return new Point(x, MousePosition.Y + TOOLBAR_HEIGHT / 2);
                    }
                    else
                    {
                        //throw new Exception("");
                        return Puppet.Location;
                    }
                    //get previous size, assume mouse to equivalent ratio of previous size
                }
                else
                {
                    pos.X -= Log_MouseRel.X;
                    pos.Y -= Log_MouseRel.Y;
                    return pos;
                }
            }
            else
                return Puppet.Location;
        }

        //public Size MouseSize()
        //{

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Image Draw()
        {
            //Add Form title and icon!

            //Handle Form focusing

            //Handle sizing cursor!

            //Check for MinimizedState!

            //Handle Minimizable, Maximizable, Closable!

            byte Inside = InItem();
            Image rtn = new Bitmap(Puppet.Size.Width, Puppet.Size.Height);
            Graphics F = Graphics.FromImage(rtn);
            #region cEdge
            Color cEdge;
            switch (Inside)
            {
                case 0:
                    cEdge = clrForm[1];
                    break;
                default:
                    cEdge = clrForm[2];
                    break;
            }
            #endregion //cEedge
            #region cBtns
            Color[] cBtns = new Color[2] { clrButtons[0], clrButtons[0] }; //0=Min, 1=Max
            Color fBtns = clrFonts[0];
            switch (Inside)
            {
                case 1:
                    if (MS.IsDown) cBtns[0] = clrButtons[2];
                    else cBtns[0] = clrButtons[1];
                    break;
                case 2:
                    if (MS.IsDown) cBtns[1] = clrButtons[2];
                    else cBtns[1] = clrButtons[1];
                    break;
                default:
                    cBtns[0] = clrButtons[0];
                    cBtns[1] = clrButtons[0];
                    break;
            }
            #endregion //cBtns
            #region cBtnClose
            Color cBtnClose;
            Color fBtnClose;
            Color fBtnClose_blend;
            switch (Inside)
            {
                case 3:
                    fBtnClose = clrFonts[2];
                    if (MS.IsDown)
                    {
                        cBtnClose = clrButtons[4];
                        fBtnClose_blend = clrFonts[4];
                    }
                    else
                    {
                        cBtnClose = clrButtons[3];
                        fBtnClose_blend = clrFonts[3];
                    }
                    break;
                default:
                    fBtnClose = clrFonts[0];
                    fBtnClose_blend = clrFonts[1];
                    cBtnClose = clrButtons[0];
                    break;
            }
            #endregion //cBtnClose

            int btnSt = startBtns;
            F.FillRectangle(classGraphics.CBrush(clrForm[0]), 1, TOOLBAR_HEIGHT + 1, Puppet.Size.Width - 2, Puppet.Size.Height - TOOLBAR_HEIGHT - 2); //BG
            F.FillRectangle(classGraphics.CBrush(clrButtons[0]), 1, 1, Puppet.Size.Width - 2 - BUTTON_WIDTH * 3, TOOLBAR_HEIGHT); //Toolbar
            F.DrawRectangle(classGraphics.CPen(cEdge), 0, 0, Puppet.Size.Width - 1, Puppet.Size.Height - 1); //edge

            #region Buttons_BG
            for (int i = 0; i < 2; i++)
                F.FillRectangle(classGraphics.CBrush(cBtns[i]), btnSt + i * BUTTON_WIDTH, 1, BUTTON_WIDTH, TOOLBAR_HEIGHT);

            F.FillRectangle(classGraphics.CBrush(cBtnClose), btnSt + 2 * BUTTON_WIDTH, 1, BUTTON_WIDTH, TOOLBAR_HEIGHT);
            #endregion //Buttons_BG

            //button text
            #region Buttons_Text
            int btn2 = btnSt + BUTTON_WIDTH;
            int btn3 = btn2 + BUTTON_WIDTH;
            F.DrawLine(classGraphics.CPen(fBtns), new Point(btnSt + 18, 14 + 1), new Point(btnSt + 27, 14 + 1)); //Minimize
            #region Maximize
            if (Maximized)
            {
                F.DrawRectangle(classGraphics.CPen(fBtns), btn2 + 16, 11 + 1, 8, 8); //Foreground Rectangle

                //<Background Rectangle>
                F.DrawLine(classGraphics.CPen(fBtns), new Point(btn2 + 18, 9 + 1), new Point(btn2 + 26, 9 + 1));
                F.DrawLine(classGraphics.CPen(fBtns), new Point(btn2 + 26, 10 + 1), new Point(btn2 + 26, 16 + 1));

                F.DrawLine(classGraphics.CPen(fBtns), new Point(btn2 + 18, 9 + 1), new Point(btn2 + 18, 10 + 1));
                F.DrawLine(classGraphics.CPen(fBtns), new Point(btn2 + 25, 16 + 1), new Point(btn2 + 26, 16 + 1));
                //</Background Rectangle>
            }
            else
                F.DrawRectangle(classGraphics.CPen(fBtns), btn2 + 16, 9 + 1, 10, 10);
            #endregion //Maximize
            //close-button text_blend
            #region Close
            #region close_textBlend
            F.DrawLine(classGraphics.CPen(fBtnClose_blend), new Point(btn3 + 17, 10 + 1), new Point(btn3 + 25, 18 + 1));
            F.DrawLine(classGraphics.CPen(fBtnClose_blend), new Point(btn3 + 18, 9 + 1), new Point(btn3 + 26, 17 + 1));

            F.DrawLine(classGraphics.CPen(fBtnClose_blend), new Point(btn3 + 17, 17 + 1), new Point(btn3 + 25, 9 + 1));
            F.DrawLine(classGraphics.CPen(fBtnClose_blend), new Point(btn3 + 18, 18 + 1), new Point(btn3 + 26, 10 + 1));
            #endregion //close_textBlend
            //close-button text
            #region close_text
            F.DrawLine(classGraphics.CPen(fBtnClose), new Point(btn3 + 17, 9 + 1), new Point(btn3 + 26, 18 + 1));
            F.DrawLine(classGraphics.CPen(fBtnClose), new Point(btn3 + 17, 18 + 1), new Point(btn3 + 26, 9 + 1));
            #endregion //close_text
            #endregion //Close
            #endregion //Buttons_Text
            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Near;
            strFormat.LineAlignment = StringAlignment.Center;

            if (Puppet.Text != "") F.DrawString(Puppet.Text, SystemFonts.DefaultFont, classGraphics.CBrush(fBtns),
                                        new Rectangle(Math.General.FInt((double)TOOLBAR_HEIGHT / 4) + (9 + 14 - 3), 0, Puppet.Size.Width, TOOLBAR_HEIGHT), strFormat);

            F.DrawImage(Puppet.Icon.ToBitmap(), 9, 7, 16, 16);
            F.Dispose();
            return rtn;
        }
        #endregion //Modifying
        #region ReturnOnly
        /// <summary>
        /// Return of {0=NOP} {1=Minimize} {2=ToggleMax} {3=Close} {4=ToggleMax_Toolbar}.
        /// </summary>
        /// <returns></returns>
        public byte mouseClick()
        {
            byte itm = InItem();

            if (itm > 0 && itm < 5) return itm;
            //implicit else
            return 0; //having clicked the form, the mouse cannot be outside of it
        }

        /// <summary>
        /// Return of {0=Outside} {1=Minimize} {2=ToggleMax} {3=Close} {4=Toolbar} {5=Form} {6=FormEdge}.
        /// </summary>
        /// <returns></returns>
        private byte InItem()
        {
            //Handle Minimizable, Maximizable, Closable!

            Point pos = MS.RelativePosition;
            if (IsOutsideForm())
                return 0;
            //implicit else
            if (pos.Y > 0 && pos.Y < TOOLBAR_HEIGHT + 1 && pos.Y < Puppet.Size.Height - 1 && pos.X < Puppet.Size.Width - 1)
            {
                if (IsIn_Toolbar())
                    return 4;
                //implicit else (InToolbar && !OutsideButtons)
                byte btn = (byte)(Math.General.FInt((double)(pos.X - startBtns) / BUTTON_WIDTH) + 1);
                if (btn == 4) return 3;
                //implicit else
                return btn;
            }
            else
            {
                if (pos.X == Puppet.Location.X || pos.Y == Puppet.Location.Y || pos.X == Puppet.Size.Width - 1 || pos.Y == Puppet.Size.Height - 1)
                    return 6;
                //implicit else
                return 5;
            }
        }
        private bool IsOutsideForm()
        {
            Point pos = MS.RelativePosition;
            if (pos.X > Puppet.Size.Width || pos.Y > Puppet.Size.Height || pos.X < 0 || pos.Y < 0)
                return true;
            //implicit else
            return false;
        }
        private bool IsIn_Toolbar()
        {
            //Handle Minimizable, Maximizable, Closable!

            Point pos = MS.RelativePosition;
            if (pos.X < startBtns)
                return true;
            //implicit else
            return false;
        }
        #endregion //ReturnOnly
        #endregion //Functions
    } // </CForm>
}
