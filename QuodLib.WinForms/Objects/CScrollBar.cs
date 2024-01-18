using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib
{
    namespace Objects
    {
        class CScrollBar : CHoverable, ICObject, ICHoverable
        {
            private bool horizontal;
            private ushort index;

            public bool Horizontal {
                get { return horizontal; }
                set {
                    horizontal = value;
                    Redraw();
                }
            }
            public ushort Index {
                get { return index; }
                set {
                    index = value;
                    Redraw();
                }
            }

        } // </class>
    }
}
