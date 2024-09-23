namespace QuodLib.IO.Models.Sync {

    /// <summary>
    /// Information about what should be done when a <see cref="FileDuplicate"/> is found.
    /// </summary>
    public enum FileDuplicateBehavior {
        /// <summary>
        /// Move the source to a new "~\.Duplicate\{Newer|Older|Same}\" directory.
        /// </summary>
        Reserve,

        /// <summary>
        /// Replace the target file by moving the source file.
        /// </summary>
        Move,

        /// <summary>
        /// Replace the target file and <see cref="Reserve"/> the source file.
        /// </summary>
        CopyAndReserve,

        /// <summary>
        /// Replace the target file and keep the source file where it was.
        /// </summary>
        CopyAndKeep,

        /// <summary>
        /// Do nothing.
        /// </summary>
        Skip
    }
}
