namespace QuodLib.WinForms.Screens {

    /// <summary>
    /// Used to capture a partial screenshot from a specific area of the screen.
    /// </summary>
    public class Watcher {
        /// <summary>
        /// Upper-left position to watch.
        /// </summary>
        public Point UpperLeft = new(0, 0);

        /// <summary>
        /// Lower-right position to watch.
        /// </summary>
        public Point LowerRight = new(0, 0);

        /// <summary>
		/// The size of the screen-region to watch.
		/// </summary>
		public Size Size
            => new Size(Width, Height);

        /// <summary>
		/// The height of the screen-region to watch.
		/// </summary>
        public int Height => LowerRight.Y - UpperLeft.Y + 1;

        /// <summary>
		/// The width of the screen-region to watch.
		/// </summary>
        public int Width => LowerRight.X - UpperLeft.X + 1;

        /// <summary>
		/// Whether <see cref="Size"/> has positive dimensions.
		/// </summary>
		public bool Valid
            => (Size.Width > 0 && Size.Height > 0);

        /// <summary>
        /// Capture and return a partial screenshot of the display <i>(<see cref="UpperLeft"/> to <see cref="LowerRight"/>)</i>.
        /// </summary>
        /// <returns><c>null</c> if <c>!<see cref="Valid"/></c></returns>
        public Image? Peek() {
            if (!Valid)
                return null;

            Image rtn = new Bitmap(Size.Width, Size.Height);
            using (Graphics P = Graphics.FromImage(rtn)) {
                P.CopyFromScreen(UpperLeft, new Point(0, 0), Size);
            }
            return rtn;
        }

    }
}
