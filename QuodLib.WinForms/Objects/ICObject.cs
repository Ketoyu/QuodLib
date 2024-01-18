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
		/// An Interface for Image-based controls.
		/// </summary>
        public interface ICObject
        {
            //static Type Type {get;}
            void OnClick();
            void Redraw();
        }
    }
}
