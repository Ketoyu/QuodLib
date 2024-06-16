using QuodLib.Math;
using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using System.Drawing;

namespace QuodLib.WinForms.Objects
{
    public abstract class CObject
    {
        //protected static Type Type {get {throw new Exception("CObject child error: static Type property not overridden.");}}
		/// <summary>
		/// The lcoation of this container, relative to its host container.
		/// </summary>
        public virtual Point Location { get; set; }

		/// <summary>
		/// The location of the container hosting [this] object.
		/// </summary>
        public virtual Point ContainerLocation { get; set; }

        /// <summary>
        /// Error-correcting calibration reference-point.
        /// </summary>
        public virtual Point MouseOffset { get; set; }

		public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The Image depicting [this] object.
        /// </summary>
        public Image Image { get; protected set; }

		/// <summary>
		/// Part of [this] object's dimensions.
		/// </summary>
        public uint Width, Height;

		/// <summary>
		/// Whether user-interaction with [this] object is recognized.
		/// </summary>
        public bool Enabled = true;

		/// <summary>
		/// String tag containing contextual information about [this] object.
		/// </summary>
        public string Tag = "";

		/// <summary>
		/// [This] object's dimensions.
		/// </summary>
		public Size Size {
			get {
				return new Size((int)Width, (int)Height);
			}
		}
		/// <summary>
		/// The global mouse-position.
		/// </summary>
        protected static Point GlobalMousePosition {
            get {
                return System.Windows.Forms.Control.MousePosition;
            }
        }

		/// <summary>
		/// The mouse-position, relative [this] object.
		/// </summary>
        public Point MousePosition {
            get {
				return GlobalMousePosition
					.Subtract(ContainerLocation)
					.Subtract(Location)
					.Subtract(MouseOffset);
            }
        }

		/// <summary>
		/// Whether the mouse-position is within [this] object.
		/// </summary>
        public bool IsHovered {
            get {
                return Mouse_IsIn(new Rectangle(0, 0, (int)Width, (int)Height));
            }
        }

		/// <summary>
		/// Whether the mouse-position is within [this] object.
		/// </summary>
		/// <returns></returns>
        protected bool Mouse_IsIn()
        {
			return Mouse_IsIn(new Rectangle(Location, Size));
        }

		/// <summary>
		/// Whether the mouse-position is within the provided Rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
        protected bool Mouse_IsIn(Rectangle rect)
        {
            if (rect.Width <= 0 || rect.Height <= 0) throw new Exception("Error: Size must be greater than 0.");
			return rect.ContainsPoint(MousePosition);
        }

		/// <summary>
		/// The void delegate definition for mouse-clicks.
		/// </summary>
        public delegate void EmptyHandler();

		/// <summary>
		/// The method(s) that activate(s) upon the user interacting with [this] object via mouse-click.
		/// </summary>
        public virtual event EmptyHandler MouseDown, MouseUp;

		/// <summary>
		/// The internal method that [this] object runs upon the user interacting with [this] object via mouse-click.
		/// </summary>
        public virtual void OnClick()
        {
            throw new Exception("Error: Derived CObject sub-class does not contain an OnClick() override.");
        }
    }
}
