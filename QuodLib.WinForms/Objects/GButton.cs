﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using System.Drawing;

namespace QuodLib.WinForms.Objects
{
	public class GButton : CHoverable
	{
		protected Image Normal { get; set;}
		protected Image Hovered { get; set; }
		protected Image Pressed { get; set; }
        public bool Calibrate = false;

		private GButton(uint width, uint height)
		{
			Width = width;
			Height = height;
		}
		public GButton(Image sheet, bool vertical, uint width, uint height) : this(width, height)
		{
			Normal = new Bitmap((int)Width, (int)Height);
			Hovered = new Bitmap((int)Width, (int)Height);
			Pressed = new Bitmap((int)Width, (int)Height);
			Restyle(sheet, vertical);
		}
		public GButton(Image normal, Image hovered, Image pressed, uint width, uint height) : this(width, height)
		{
			Restyle(normal, hovered, pressed);
		}
		public void Restyle(Image sheet, bool vertical)
		{
			Image[] phases = new[] {Normal, Hovered, Pressed};
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
			this.Normal = normal;
			this.Hovered = hovered;
			this.Pressed = pressed;
			Redraw();
		}
		public override void Redraw()
		{
			switch (State) {
				case MouseState.Normal:
					Image = Normal;
					break;
				case MouseState.Hovered:
					Image = Hovered;
					break;
				case MouseState.Pressed:
					Image = Pressed;
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
            if (Calibrate) {
				string name = Name.Length > 0
					? name = $"for '{Name}'"
					: string.Empty;

                System.Diagnostics.Debug.WriteLine($"Control-relative MouseClick {name} registered at ({MousePosition.X}, {MousePosition.Y}). "
					+ "\tIs " + (IsHovered ? "" : " not") + " hovered.");
            }
				
        }
	} // </class>
}
