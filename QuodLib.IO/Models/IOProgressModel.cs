using QuodLib.Strings;

namespace QuodLib.IO.Models {
    /// <summary>
    /// A representation of the current progress of a bulk IO operation.
    /// </summary>
    public struct IOProgressModel {
        /// <summary>
        /// The total size, in bytes, of files that have been processed so far.
        /// </summary>
        public required long CurrentSize { get; init; }

        /// <summary>
        /// The total count of files that have been processed so far.
        /// </summary>
        public required long CurrentCount { get; init; }

        /// <summary>
        /// The total size of files to be processed.
        /// </summary>
        public required long SourceSize { get; init; }

        /// <summary>
        /// The total count of files to be processed.
        /// </summary>
        public required long SourceCount { get; init; }

        /// <summary>
        /// Whether the current IO operation was a success.
        /// </summary>
        public bool Success { get; init; }

        private decimal? _sizePercent;
        /// <summary>
        /// A percentage size of files processed so far.
        /// </summary>
        public decimal SizePercent
            => _sizePercent ??= Success
                    ? Math.Floor(CurrentSize * 100 / (decimal)SourceSize)
                    : (Math.Round(CurrentSize / (decimal)SourceSize, 2) * 100);

        private string? currentBytes;
        /// <summary>
        /// A text representation of the total size of files that have been processed so far.
        /// </summary>
        public string CurrentBytes
            => currentBytes ??= CompressSize(CurrentSize);

        private string? sourceBytes;
        /// <summary>
        /// A text representation of the total size of files to be processed.
        /// </summary>
        public string SourceBytes
            => sourceBytes ??= CompressSize(SourceSize);

        /// <summary>
        /// Convert bytes into to text representation of bytes or mega-/giga-/tera-bytes.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private static string CompressSize(long size)
            => Misc.Size_Compress(size, 1024, 3, Misc.SizeNames_Bytes, false);
    }
}
