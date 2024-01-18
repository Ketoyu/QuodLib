using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Math {
    public static class Types {
        #region IsType
        private static Type[] NumWholePos { get { return new Type[] { typeof(byte), typeof(ushort), typeof(uint), typeof(ulong) }; } }
        private static Type[] NumWholeNeg { get { return new Type[] { typeof(sbyte), typeof(short), typeof(int), typeof(long) }; } }
        private static Type[] NumDec { get { return new Type[] { typeof(float), typeof(double), typeof(decimal) }; } }
        #region Type
        public static bool IsNumeric(Type typ) {
            return NumWholePos.Contains(typ) || NumWholeNeg.Contains(typ) || NumDec.Contains(typ);
        }
        public static bool IsNumericWhole(Type typ) {
            return NumWholePos.Contains(typ) || NumWholeNeg.Contains(typ);
        }
        public static bool IsNumericWholePositive(Type typ) {
            return NumWholePos.Contains(typ);
        }
        public static bool IsNumericWholeNegativeCapable(Type typ) {
            return NumWholeNeg.Contains(typ);
        }
        public static bool IsNumericFloating(Type typ) {
            return NumDec.Contains(typ);
        }
        public static bool IsString(Type typ) {
            return typ == typeof(string);
        }
        #endregion //Type

        public static bool IsNumeric(object obj) {
            return IsNumeric(obj.GetType());
        }
        public static bool IsNumericWhole(object obj) {
            return IsNumericWhole(obj.GetType());
        }
        public static bool IsNumericWholePositive(object obj) {
            return IsNumericWholePositive(obj.GetType());
        }
        public static bool IsNumericWholeNegativeCapable(object obj) {
            return IsNumericWholeNegativeCapable(obj.GetType());
        }
        public static bool IsNumericFloating(object obj) {
            return IsNumericFloating(obj.GetType());
        }
        #endregion //IsType
    }
}
