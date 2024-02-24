using QuodLib.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Objects
{
    /*public*/ class Column<T> where T : IComparable
    {
        public T data;
        bool IsNumeric {
            get {
                return Types.IsNumeric(typeof(T));
            }
        }
        bool IsString {
            get {
                return typeof(T) == typeof(string);
            }
        }
    }
}
