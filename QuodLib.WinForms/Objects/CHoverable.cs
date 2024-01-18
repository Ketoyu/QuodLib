using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace QuodLib
{
	namespace Objects
	{
		public abstract class CHoverable : CObject, ICHoverable
		{
			/// <summary>
			/// 0: Normal | 1: Hovered | 2: Pressed
			/// </summary>
			protected byte State;
			/// <summary>
			/// 0: Normal | 1: Hovered | 2: Pressed
			/// </summary>
			public byte State_Previous {get; private set;}
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
			public event DClick EOnEnter, EOnLeave, EOnMove, EOnDrag;
			/// <summary>
			/// The method(s) that activate(s) upon the user interacting with [this] object via mouse-click.
			/// </summary>
			public override event DClick EOnMouseDown, EOnMouseUp;
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
				byte st = State;
				State = (byte)(Enabled && IsHovered ? 1 : 0);
				if (State != st)
				{
					State_Previous = st;
					Redraw();
					if (st == 0 && State == 1 && EOnEnter != null) EOnEnter.Invoke();
					if (State == 0 && EOnLeave != null) EOnLeave.Invoke();
				} else {
					if (State == 2 && EOnDrag != null) EOnDrag.Invoke();
					if (State == 1 && EOnMove != null) EOnMove.Invoke();
				}
			}
			/// <summary>
			/// The internal method that [this] object runs upon the user interacting with [this] object via mouse-down.
			/// </summary>
			public virtual void OnMouseDown()
			{
				byte st = State;
				if (Enabled && IsHovered)
				{
					State = 2;
					if (st != State) {
						State_Previous = st;
						Redraw();
					}
					if (EOnMouseDown != null) EOnMouseDown.Invoke();
				}
			}
			/// <summary>
			/// The internal method that [this] object runs upon the user interacting with [this] object via mouse-up.
			/// </summary>
			public virtual void OnMouseUp()
			{
				byte st = State;
				if (Enabled && IsHovered)
				{
					State = 1;
					if (st != State) {
						State_Previous = st;
						Redraw();
					}
					if (EOnMouseUp != null) EOnMouseUp.Invoke();
				}
			}
			/// <summary>
			/// The internal method that [this] object runs for its draw-call.
			/// </summary>
			public virtual void Redraw()
			{
				throw new Exception("Error: Derived CHoverable sub-class does not contain a Redraw() override.");
			}
		}
	}
}
