namespace QuodLib.IO.Models {
    /// <summary>
    /// File copy vs move.
    /// </summary>
    public enum FileOperationType {
        /// <summary>
        /// Copy the source file to a target path.
        /// </summary>
        Copy,

        /// <summary>
        /// Move the source file to a target path.
        /// </summary>
        Move,

        /// <summary>
        /// Delete the source file.
        /// </summary>
        Delete
    }
}
