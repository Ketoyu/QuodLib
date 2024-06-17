//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using QuodLib.WinForms.Drawing;
using QuodLib.WinForms.Objects.Enums;
using System.Drawing;
using System.Windows.Media;
using static QuodLib.Objects.Area;

namespace QuodLib.WinForms.Objects
{
	public partial class GButton : CHoverable
	{
		protected Image Normal { get; set;}
		protected Image Hovered { get; set; }
		protected Image Pressed { get; set; }
        public bool Calibrate = false;

		private GButton(int width, int height)
		{
			Width = width;
			Height = height;
		}
		public GButton(Image sheet, bool vertical, int width, int height) : this(width, height)
		{
			Normal = new Bitmap(Width, Height);
			Hovered = new Bitmap(Width, Height);
			Pressed = new Bitmap(Width, Height);
			Restyle(sheet, vertical);
		}
		public GButton(Image normal, Image hovered, Image pressed, int width, int height) : this(width, height)
		{
			Restyle(normal, hovered, pressed);
		}

		public void Restyle(Image sheet, bool vertical)
		{
			Image[] phases = new[] {Normal, Hovered, Pressed};
            int w = sheet.Width / (vertical ? 1 : 3),
				h = sheet.Height / (vertical ? 3 : 1);

	        Rectangle dest = new Rectangle(0, 0, Width, Height);
                        
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
		public void Resize(int width, int height)
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

		/// <summary>
		/// Returns a new <see cref="GButton"/> with just the same images and size.
		/// </summary>
		/// <returns></returns>
        public GButton CloneByImage()
			=> new(Normal, Hovered, Pressed, this.Width, this.Height);
    } // </class>
}
