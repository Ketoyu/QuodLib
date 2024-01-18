using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib
{
    namespace Objects
    {
		/// <summary>
		/// An interface for Image-based controls which have hover actions.
		/// </summary>
        public interface ICHoverable
        {
            void OnMouseOver();
            void OnMouseDown();
            void OnMouseUp();
        }
    }
}
