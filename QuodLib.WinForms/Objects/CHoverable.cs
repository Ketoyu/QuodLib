using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace QuodLib.WinForms.Objects
{
	public abstract class CHoverable : CObject, ICHoverable
	{
		public enum MouseState {
			Normal,
			Hovered,
			Pressed
		}

		protected MouseState State;

		public MouseState State_Previous { get; private set; }

		/// <summary>
		/// Whether click/move should always redraw, regardless of <see cref="State"/>.
		/// </summary>
		protected virtual bool StatelessRedraw => false;

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
		public event DClick? Enter, Leave, Move, Drag;
		/// <summary>
		/// The method(s) that activate(s) upon the user interacting with [this] object via mouse-click.
		/// </summary>
		public override event DClick? MouseDown, MouseUp;
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
		public virtual void OnMouseOver()
		{
			MouseState old = State;
			State = Enabled && IsHovered ? MouseState.Hovered : MouseState.Normal;
			if (State != old)
			{
				State_Previous = old;
				Redraw();

				if (old == MouseState.Normal && State == MouseState.Hovered)
					Enter?.Invoke();
				if (State == MouseState.Normal)
					Leave?.Invoke();
			} else {
				if (StatelessRedraw)
					Redraw();

				if (State == MouseState.Pressed)
					Drag?.Invoke();
				if (State == MouseState.Hovered && Move != null)
					Move?.Invoke();
			}
		}
		/// <summary>
		/// The internal method that [this] object runs upon the user interacting with [this] object via mouse-down.
		/// </summary>
		public virtual void OnMouseDown()
		{
			MouseState old = State;
			if (Enabled && IsHovered)
			{
				State = MouseState.Pressed;
				if (old != State) {
					State_Previous = old;
					Redraw();
				} else if (StatelessRedraw)
                    Redraw();

                MouseDown?.Invoke();
			}
		}
		/// <summary>
		/// The internal method that [this] object runs upon the user interacting with [this] object via mouse-up.
		/// </summary>
		public virtual void OnMouseUp()
		{
			MouseState old = State;
			if (Enabled && IsHovered)
			{
				State = MouseState.Hovered;
				if (old != State) {
					State_Previous = old;
					Redraw();
				} else if (StatelessRedraw)
                    Redraw();

                MouseUp?.Invoke();
			}
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
