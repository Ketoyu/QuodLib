using QuodLib.Objects;

namespace QuodLib.WinForms.Screens {

    /// <summary>
    /// Used to capture a partial screenshot from a specific area of the screen.
    /// </summary>
    public class Watcher : Area {

        /// <summary>
        /// Capture and return a partial screenshot of the display <i>(<see cref="Origin"/> to <see cref="Extent"/>)</i>.
        /// </summary>
        /// <returns><c>null</c> if <c>!<see cref="Valid"/></c></returns>
        public Image? Peek() {
            if (!Valid)
                return null;

            Image rtn = new Bitmap(Size.Width, Size.Height);
            using (Graphics P = Graphics.FromImage(rtn)) {
                P.CopyFromScreen(Origin, new Point(0, 0), Size);
            }
            return rtn;
        }

    }
}
