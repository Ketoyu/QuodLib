//using System.Text;
//using System.Threading.Tasks;

using System.Runtime.CompilerServices;
using Out = System.Diagnostics.Debug;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;

using sMath = System.Math;

namespace QuodLib {
    /// <summary>
    /// Contains general functions for many object types.
    /// </summary>
    public static class classObjects {

        #region IsType
        #region Type
        public static bool IsString(Type typ) {
            return typ == typeof(string);
        }
        #endregion //Type
        #region Object
        public static bool IsString(object obj) {
            return IsString(obj.GetType());
        }
        #endregion //Object
        #endregion //IsType
    } //END ALL
}
