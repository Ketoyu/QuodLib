using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace QuodLib.Objects
{
	/// <summary>
	/// A wrapper for a sequence of Images.
	/// </summary>
    public class ImageSequence
    {
		#region Fields
        private uint iterator;
        private ushort index;
        public Image[] Data {get; private set;}
        public byte Rate = 1;
		public bool Loop = false;
		#endregion //Fields
		#region Properties
        public bool IsReset {
            get {
                return iterator == 0;
            }
        }
        public bool HasBegun {
            get {
                return iterator > 0;
            }
        }
        public bool HasEnded {
            get {
                return index >= Data.Length && !Loop;
            }
        }
		#endregion //Properties
		#region Constructors
        public ImageSequence(Image[] data)
        {
            Data = data;
        }
        public ImageSequence(Image[] data, byte rate)
        {
            if (rate == 0) throw new Exception("Rate must be greater than zero.");
            Data = data;
            Rate = rate;
        }
        public ImageSequence(Image tilesheet1D, byte squares, bool horizontal) : this(tilesheet1D, (horizontal ? squares : (byte)1), (horizontal ? (byte)1 : squares)) {}
        public ImageSequence(Image tilesheet2D, byte hSquares, byte vSquares)
        {
            if (tilesheet2D.Width % hSquares > 0) throw new Exception("Tilesheet width \"" + tilesheet2D.Width + "\" indivisible by \"" + hSquares + ".");
            if (tilesheet2D.Height % vSquares > 0) throw new Exception("Tilesheet height \"" + tilesheet2D.Width + "\" indivisible by \"" + vSquares + ".");
            ushort width = (ushort)(tilesheet2D.Width / hSquares);
            ushort height = (ushort)(tilesheet2D.Height / vSquares);
            List<Image> data = new List<Image>();
            Graphics G = null;
            for (byte y = 0; y < vSquares; y++)
            {
                for (byte x = 0; x < vSquares; x++)
                {
                    Image img = new Bitmap(width, height);
                    G = Graphics.FromImage(img);
                    G.DrawImage(tilesheet2D, new Rectangle(0, 0, width, height), new Rectangle(x * hSquares * width, y * vSquares * height, width, height), GraphicsUnit.Pixel);
                }
            }
            Data = data.ToArray();
            G.Dispose();
        }
		#endregion //Constructors
		#region Methods
        public void Reset()
        {
            iterator = 0;
            index = 0;
        }
        public Image Next()
        {
            if (index >= Data.Length) {
				if (Loop) Reset();
					else return null;
			}
            Image rtn = Data[index];
            iterator++; //Method never exits as reset.
            if (iterator % Rate == 0) index++;
            return rtn;
        }
		#endregion //Methods
    } // </class>
}
