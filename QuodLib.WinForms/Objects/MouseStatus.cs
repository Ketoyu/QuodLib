using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Point = System.Drawing.Point;
using Form = System.Windows.Forms.Form;
using QuodLib.Objects;

namespace QuodLib.WinForms.Objects
{
    /// <summary>
    /// An instanced object for tracking how the mouse interacts with a Windows Form. Includes functions and flags that handle such things as mouse-dragging and relative position.
    /// </summary>
    public class MouseStatus : Puppeteer<Form>
    {
        /// <summary>
        /// The Windows Form that the MouseStatus is most concerned with.
        /// </summary>
        //public Form Puppet {get; private set;}
            
        private Point MousePosition
            => System.Windows.Forms.Control.MousePosition;

        #region Booleans
		/// <summary>
		/// Whether [this] object is allowed to change the parent Form's position.
		/// </summary>
		public bool AllowDragging = true;
		/// <summary>
		/// Whether the mouse is currently within the [parent] Form.
		/// </summary>
		public bool IsInForm
            => (RelativePosition.X >= 0 && RelativePosition.Y >= 0 && RelativePosition.X < Puppet.Width && RelativePosition.Y < Puppet.Height);
			
        /// <summary>
        /// Whether the mouse is currently held down.
        /// </summary>
        public bool IsDown {get; private set;}
        /// <summary>
        /// Whether the dragging process {is ready / has initiated}.
        /// </summary>
        public bool IsDragging {get; private set;}
        /// <summary>
        /// Whether the mouse has dragged since the most recent MouseDown() event. Lingers after the MouseUp() event.
        /// </summary>
        public bool HasDragged {get; private set;}
        /// <summary>
        /// Whether the relative mouse position has changed.
        /// </summary>
        public bool Relative_HasChanged
            => !( RelativePosition.X == RelativeOrigin.X || RelativePosition.Y == RelativeOrigin.Y );
            
        /// <summary>
        /// Set true by the MouseUp event if "Dragging" was true during its call.
        /// Set false by the MouseDown event regardless during its call.
        /// </summary>
        public bool Form_HasMoved {get; private set;}
        #endregion //Booleans

        #region Positions
        /// <summary>
        /// The literal origin of the mouse at the start of dragging.
        /// </summary>
        public Point MouseOrigin {get; private set;}
        /// <summary>
        /// The origin of the Form at the start of dragging.
        /// </summary>
        public Point FormOrigin {get; private set;}
        /// <summary>
        /// The relative origin of the mouse at the start of dragging.
        /// </summary>
        public Point RelativeOrigin
            => new Point(MouseOrigin.X - FormOrigin.X, MouseOrigin.Y - FormOrigin.Y);
            
        /// <summary>
        /// The position of the mouse, relative to the Form.
        /// </summary>
        public Point RelativePosition
            => new Point(MousePosition.X - Puppet.Location.X, MousePosition.Y - Puppet.Location.Y);
        #endregion //Positions

        /// <summary>
        /// Creates an instance of the MouseStatus object and sets its parent to the provided Windows Form.
        /// </summary>
        /// <param name="puppet"></param>
        public MouseStatus(Form puppet) : base(puppet) { }

        /// <summary>
        /// Make this call in the parent Form's "MouseDown" event handler.
        /// </summary>
        public void OnMouseDown()
        {
            IsDown = true;
            MouseOrigin = MousePosition;
            FormOrigin = Puppet.Location;
            HasDragged = false;
            Form_HasMoved = false;
        }
        /// <summary>
        /// Make this call in the parent Form's "MouseUp" event handler.
        /// Warning: Calls the parent Form's "Refresh()" method.
        /// </summary>
        public void OnMouseUp()
        {
            IsDown = false;
            IsDragging = false;
            Form_HasMoved = (Puppet.Location != FormOrigin);
            Puppet.Refresh();
        }
        /// <summary>
        /// Make this call in the Form's "MosueMove" event handler.
        /// Warning: If applicable, moves the parent Form.
        /// </summary>
        public void OnMouseMove()
        {
            if (!IsDown) return;

            if (IsDragging) {
                if (Relative_HasChanged) {
                    HasDragged = true;
                    Puppet.Location = new Point(MousePosition.X - RelativeOrigin.X, MousePosition.Y - RelativeOrigin.Y);
                }
            } else if (AllowDragging) {
				IsDragging = true;
				FormOrigin = Puppet.Location;
				MouseOrigin = MousePosition;
			}
        }
    } // </MouseStatus>
}
