namespace QuodLib.IO.Models {
    /// <summary>
    /// Options for whether to overwrite when copying/moving from a source to a target.
    /// </summary>
    public enum OverwriteOption {
        /// <summary>
        /// Overwrite.
        /// </summary>
        Yes,

        /// <summary>
        /// Don't overwrite.
        /// </summary>
        No,

        /// <summary>
        /// Overwrite only if the source is newer than the target.
        /// </summary>
        IfNewer
    }
}
