using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Objects {
    public static class PointExtensions {
        public static Point Left(this Point point, int by = 1) {
            point.Y -= by;
            return point;
        }
        public static Point Right(this Point point, int by = 1) {
            point.Y += by;
            return point;
        }
        public static Point Up(this Point point, int by = 1, bool upIsNegative = true) {
            if (!upIsNegative)
                return point.Up(by);

            point.Y -= by;
            return point;
        }
        public static Point Down(this Point point, int by = 1, bool upIsNegative = true) {
            if (!upIsNegative)
                return point.Down(by);

            point.Y += by;
            return point;
        }
    }
}
