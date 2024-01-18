using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace QuodLib
{
    namespace Objects
    {
        /*public*/ class objButtonG
        {
            public bool Sizeable {get; private set;}
            private Image imageOriginal
            {
                get {
                    return null; //Replace later
                }
                set {
                    //  5  4  3
                    //   \ | /
                    //6 ---8--- 2
                    //   / | \
                    //  7  0  1
                    imageOriginal = value;
                    List<Image> iCorners = new List<Image>();
                    List<Image> iSides = new List<Image>();
                    Bitmap iMiddle;
                    //center
                    int crnW = Math.General.FInt((double)imageOriginal.Width / 3);
                    int crnH = Math.General.FInt((double)imageOriginal.Height / 3);
                    //left & right
                    int lrW = imageOriginal.Width - (2 * crnW);
                    int lrH = crnH;
                    //top & bottom
                    int udW = lrH;
                    int udH = imageOriginal.Height - (2 * crnH);

                    int midW = udW;
                    int midH = lrH;
                    Graphics G;
                    #region iCorners
                        for (byte i = 1; i <= 4; i++)
                        {
                            iCorners.Add(new Bitmap(crnW, crnH));
                        }
                        G = Graphics.FromImage(iCorners[0]);
                        G.DrawImage(imageOriginal, crnW + udW, crnH + udH, crnW, crnH);
                        G = Graphics.FromImage(iCorners[1]);
                        G.DrawImage(imageOriginal, crnW + udW, 0, crnW, crnH);
                        G = Graphics.FromImage(iCorners[2]);
                        G.DrawImage(imageOriginal, 0, 0, crnW, crnH);
                        G = Graphics.FromImage(iCorners[3]);
                        G.DrawImage(imageOriginal, 0, crnH + udH, crnW, crnH);                    
                    #endregion //iCorners
                    #region iSides
                        for (byte i = 1; i <= 2; i++)
                        {
                            iSides.Add(new Bitmap((int)lrW, (int)lrH));
                            iSides.Add(new Bitmap((int)udW, (int)udH));
                        }
                        G = Graphics.FromImage(iSides[0]);
                        G.DrawImage(imageOriginal, (int)crnW, (int)(udH + midH), udW, udH);
                        G = Graphics.FromImage(iSides[1]);
                        G.DrawImage(imageOriginal, (int)(crnW + udW), (int)crnH, lrW, lrH);
                        G = Graphics.FromImage(iSides[2]);
                        G.DrawImage(imageOriginal, (int)crnW, 0, udW, udH);
                        G = Graphics.FromImage(iSides[3]);
                        G.DrawImage(imageOriginal, 0, (int)crnH, lrW, lrH);
                    #endregion //iSides
                    #region iMiddle
                        iMiddle = new Bitmap((int)midW, (int)midH);
                        G = Graphics.FromImage(iMiddle);
                        G.DrawImage(iMiddle, (int)crnW, (int)udH, midW, midH);
                    #endregion //iMiddle
                    Image = new Bitmap((int)Width, (int)Height);
                    G = Graphics.FromImage(Image);
                    /*uint lrW_, lrH_, udW_, udH_;
                    uint lrOfs, udOfs;*/

                }
            }
            private List<Image> imagePieces = new List<Image>();
            public uint Height
            {
                get {
                    return Height;
                }
                set {

                }
            }
            public uint Width
            {
                get {
                    return Width;
                }
                set {

                }
            }
            public Image Image
            {
                get {
                    if (imageOriginal.Size != Image.Size)
                    {
                        //Compile Image
                    }
                    return Image;
                }
                set {

                }
            }
            public objButtonG(Image img)
            {

            }
            public objButtonG(uint width, uint height, Image img)
            {

            }
            /*
            /// <summary>
            /// Fragment a square image into an array of scaleable pieces.
            /// </summary>
            /// <param name="img"></param>
            /// <returns></returns>
            private Image[] FragmentImage(Image img)
            {
                Image[][] rtn = new Image[3][];
                
                //Center
                rtn[0] = new Image[1];

                //Sides
                rtn[1] = new Image[4];

                //Corners
                rtn[2] = new Image[4];

            }*/

            /// <summary>
            /// Divide a multi-phase image (ex: {normal, hover, pressed}) into its phases.
            /// </summary>
            /// <param name="sheet">The source image that will be split into distinct button states.</param>
            /// <param name="phaseType">0: static image | 1: Default &amp; pressed | 2: Default, hovered, &amp; pressed</param>
            /// <param name="vertical">Whether the image phases go top-down (true) or left to right (false).</param>
            /// <returns>An Image[] of the button states.</returns>
            private Image[] SplitSheet(Image sheet, byte phaseType, bool vertical)
            {
                if (phaseType > 2) throw new Exception("Invalid phaseType: must be between 0 and 2 (inclusive).");
                Image[] phases;

                if (phaseType == 0) {
                    phases = new Image[1];
                    phases[0] = sheet;
                } else {
                    byte parts = (phaseType == 1 ? (byte)2 : (byte)3);
                    phases = new Image[2];
                    int w = (vertical ? sheet.Width : sheet.Width / parts);
                    int h = (vertical ? sheet.Height / parts : sheet.Height);
                    Rectangle dest = new Rectangle(0, 0, w, h);
                        
                    for (byte i = 0; i < parts; i++)
                    {
                        phases[i] = new Bitmap(2, sheet.Height);
                        Graphics G = Graphics.FromImage(phases[i]);
                        G.DrawImage(sheet, dest, (vertical ? new Rectangle(0, h*i, w, h) : new Rectangle(w*i, 0, w, h)), GraphicsUnit.Pixel);
                    }
                }

                return phases;
            }
        } //END-ALL
    }
}
