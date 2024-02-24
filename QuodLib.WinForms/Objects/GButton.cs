//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using System.Drawing;

namespace QuodLib.WinForms.Objects
{
	public class GButton : CHoverable
	{
		private Image normal, hovered, pressed;
        public bool Calibrate = false;

		private GButton(uint width, uint height)
		{
			Width = width;
			Height = height;
		}
		public GButton(Image sheet, bool vertical, uint width, uint height) : this(width, height)
		{
			normal = new Bitmap((int)Width, (int)Height);
			hovered = new Bitmap((int)Width, (int)Height);
			pressed = new Bitmap((int)Width, (int)Height);
			Restyle(sheet, vertical);
		}
		public GButton(Image normal, Image hovered, Image pressed, uint width, uint height) : this(width, height)
		{
			Restyle(normal, hovered, pressed);
		}
		public void Restyle(Image sheet, bool vertical)
		{
			Image[] phases = new[] {normal, hovered, pressed};
            int w = sheet.Width / (vertical ? 1 : 3),
				h = sheet.Height / (vertical ? 3 : 1);

	        Rectangle dest = new Rectangle(0, 0, (int)Width, (int)Height);
                        
            for (byte i = 0; i < 3; i++)
            {
                Graphics G = Graphics.FromImage(phases[i]);
                G.DrawImage(sheet, dest, new Rectangle ( (vertical ? 0 : w*i), (vertical ? h*i : 0), w, h), GraphicsUnit.Pixel);
				G.Dispose();
            }
			Redraw();
		}
		public void Restyle(Image normal, Image hovered, Image pressed)
		{
			this.normal = normal;
			this.hovered = hovered;
			this.pressed = pressed;
			Redraw();
		}
		public override void Redraw()
		{
			switch (State) {
				case 0:
					Image = normal;
					break;
				case 1:
					Image = hovered;
					break;
				case 3:
					Image = pressed;
					break;
			}
		}
		public void Resize(uint width, uint height)
		{
			Width = width;
			Height = height;
		}
        public override void OnMouseUp()
        {
            base.OnMouseUp();
            if (Calibrate) System.Diagnostics.Debug.WriteLine("Control-relative MouseClick registered at (" + MousePosition.X + ", " + MousePosition.Y + "). Is " + (IsHovered ? "" : " not") + " hovered.");
        }
	} // </class>
}
