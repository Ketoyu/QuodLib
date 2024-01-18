using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.WinForms {
    public static class Drawing {
        public static void DrawStringRightAlign(this Graphics G, string s, Font font, Brush brush, Point point, int width) {
            G.DrawString(s, font, brush, new Point(point.X + width - GetTextPixelSize(font, s).Width, point.Y));
        }
        public static Size GetTextPixelSize(Font font, string text) {
            //source: https://msdn.microsoft.com/en-us/library/y4xdbe66.aspx
            return System.Windows.Forms.TextRenderer.MeasureText(text, font);
        }
    }
}
