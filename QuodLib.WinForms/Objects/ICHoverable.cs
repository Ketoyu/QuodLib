using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.WinForms.Objects
{
	/// <summary>
	/// An interface for Image-based controls which have hover actions.
	/// </summary>
    public interface ICHoverable
    {
        void OnMouseMove();
        void OnMouseDown();
        void OnMouseUp();
    }
}
