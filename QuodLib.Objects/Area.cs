using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Objects {
    /// <summary>
    /// Defines an area using two points.
    /// </summary>
    public class Area {
        /// <summary>
        /// Clarifies a <see cref="Point"/> as either <see cref="Origin"/> or <see cref="Extent"/>.
        /// </summary>
        public enum PointType {
            /// <summary>
            /// Upper-left point.
            /// </summary>
            Origin,

            /// <summary>
            /// Lower-right point.
            /// </summary>
            Extent
        }

        /// <summary>
        /// Upper-left point.
        /// </summary>
        public Point Origin { get; set; }

        /// <summary>
        /// Lower-right point.
        /// </summary>
        public Point Extent { get; set; }

        /// <summary>
		/// The size.
		/// </summary>
		public Size Size
            => new Size(Width, Height);

        /// <summary>
		/// The height.
		/// </summary>
        public int Height => Extent.Y - Origin.Y + 1;

        /// <summary>
		/// The width.
		/// </summary>
        public int Width => Extent.X - Origin.X + 1;

        /// <summary>
		/// Whether <see cref="Size"/> has positive dimensions.
		/// </summary>
		public bool Valid
            => (Size.Width > 0 && Size.Height > 0);
    }
}
