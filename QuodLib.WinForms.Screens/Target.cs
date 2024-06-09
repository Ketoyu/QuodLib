namespace QuodLib.WinForms.Screens {

    /// <summary>
    /// A portion of a canvas, to draw on.
    /// </summary>
    public class Target {

        /// <summary>
        /// Upper-left point to paint.
        /// </summary>
        public Point Origin { get; set; }

        /// <summary>
        /// Whether the monitor/display uses a High-DPI setting.
        /// </summary>
        public virtual bool HighDPI { get; set; }

        /// <summary>
        /// The down-scaling factor for High-DPI, to compensate.
        /// </summary>
        public virtual int DPI_Downscale => 21;

        /// <summary>
        /// The target/destination size to paint.
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Auto-scales the <paramref name="source"/> to fit within <see cref="Size"/>.
        /// </summary>
        public Size AutoScale(Size source)
            => new Size(source.Width * Paint_Scale(source), source.Height * Paint_Scale(source));

        /// <summary>
        /// Returns an integer scale factor that <paramref name="source"/> can be stretched by to fit within <see cref="Size"/>.
        /// </summary>
        int Paint_Scale(Size source)
            => System.Math.Max(1, System.Math.Min(
                    (int)System.Math.Floor((double)(Size.Width / (HighDPI ? DPI_Downscale : 1)) / source.Width),
                    (int)System.Math.Floor((double)(Size.Height / (HighDPI ? DPI_Downscale : 1)) / source.Height)
                    )
                );

    }
}
