namespace QuodLib.IO.Models {
    /// <summary>
    /// Pending instruction for a file or directory.
    /// </summary>
    public abstract class IOOperation {
        /// <summary>
        /// The destination path.
        /// </summary>
        public required string TargetPath { get; init; }

        /// <summary>
        /// Run the planned operation.
        /// </summary>
        public abstract void Run();
    }
}
