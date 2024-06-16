using System;
using System.Threading.Channels;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace QuodLib.WinForms.Objects
{
	public abstract class CHoverable : CObject, ICHoverable
	{
		public enum MouseState {
			/// <summary>
			/// The cursor is outside.
			/// </summary>
			Normal,

			/// <summary>
			/// The curspr is inside.
			/// </summary>
			Hovered,

			/// <summary>
			/// The cursor is inside and pressing down.
			/// </summary>
			Pressed,

			/// <summary>
			/// A child <see cref="CHoverable"/> has changed its <see cref="MouseState"/>.
			/// </summary>
			Dirty,

			/// <summary>
			/// A <see cref="MouseState.Dirty"/> has been addressed.
			/// </summary>
			Clean
		}

		protected MouseState State;

		public MouseState State_Previous { get; private set; }

		/// <summary>
		/// If <see cref="true"/>, the <see cref="CHoverable"/> uses only <see cref="MouseState.Dirty"/> / <see cref="MouseState.Clean"/> and
		/// a change in <see cref="State"/> will not automatically invoke <see cref="Redraw"/>.
		/// </summary>
		public virtual bool IsContainer => false;


		/// <summary>
		/// Whether the State has changed since the last interaction (left, entered, pressed, unpressed).
		/// </summary>
		public bool State_HasChanged {
			get {
				return State != State_Previous;
			}
		}
		/// <summary>
		/// The method(s) that activate(s) upon the user interacting with [this] object via mouse movement.
		/// </summary>
		public event EmptyHandler? MouseEnter, MouseLeave, MouseMove, MouseDrag;

		/// <summary>
		/// The method(s) that activate(s) upon the user interacting with [this] object via mouse-click.
		/// </summary>
		public override event EmptyHandler? MouseDown, MouseUp;

        /// <summary>
        /// The method(s) that activate(s) upon a change in <see cref="MouseState"/>.
        /// </summary>
        public event EmptyHandler? StateChange;

        /// <summary>
        /// The internal method that [this] object runs upon the user interacting with [this] object via mouse-click.
        /// </summary>
        public override void OnClick()
		{
			OnMouseDown();
			OnMouseUp();
		}
		/// <summary>
		/// The internal method that [this] object runs upon the user interacting with [this] object via mouse movement.
		/// </summary>
		public virtual void OnMouseMove()
		{
			MouseState old = State;
            bool changed = false;
            if (!IsContainer)
				State = Enabled && IsHovered ? MouseState.Hovered : MouseState.Normal;

			if (State != old)
			{
				State_Previous = old;
                changed = true;

                if (!IsContainer)
                    Redraw();

				if (old == MouseState.Normal && State == MouseState.Hovered)
					MouseEnter?.Invoke();
				if (State == MouseState.Normal)
					MouseLeave?.Invoke();

            } else {
                if (State == MouseState.Pressed)
					MouseDrag?.Invoke();
				else if (State == MouseState.Hovered)
					MouseMove?.Invoke();
			}

			if (changed)
				StateChange?.Invoke();
		}

		/// <summary>
		/// Invokes <see cref="StateChange"/> if <see cref="State"/> is <see cref="MouseState.Dirty"/>.
		/// </summary>
		protected virtual void OnDirty() {
			if (State == MouseState.Dirty)
				StateChange?.Invoke();
		}

		/// <summary>
		/// The internal method that [this] object runs upon the user interacting with [this] object via mouse-down.
		/// </summary>
		public virtual void OnMouseDown()
		{
			MouseState old = State;
			bool changed = false;
			if (Enabled && IsHovered)
			{
				if (!IsContainer)
					State = MouseState.Pressed;

				if (old != State) {
					State_Previous = old;
                    changed = true;

                    if (!IsContainer)
						Redraw();
				}
                MouseDown?.Invoke();
			}

            if (changed)
                StateChange?.Invoke();
        }
        /// <summary>
        /// The internal method that [this] object runs upon the user interacting with [this] object via mouse-up.
        /// </summary>
        public virtual void OnMouseUp()
		{
			MouseState old = State;
            bool changed = false;
            if (Enabled && IsHovered)
			{
				if (!IsContainer)
					State = MouseState.Hovered;

				if (old != State) {
					State_Previous = old;
                    changed = true;

                    if (!IsContainer)
						Redraw();
				}

                MouseUp?.Invoke();
			}

            if (changed)
                StateChange?.Invoke();
        }
        /// <summary>
        /// The internal method that [this] object runs for its draw-call.
        /// </summary>
        public virtual void Redraw()
		{
			throw new NotImplementedException("Error: Derived CHoverable sub-class does not contain a Redraw() override.");
		}
	}
}
