namespace QuodLib.IO.Models.Sync {
    /// <summary>
    /// Information about where a file is.
    /// </summary>
    public enum FileOrigin {
        /// <summary>
        /// The source to copy / move / compare from.
        /// </summary>
        Source,

        /// <summary>
        /// The destination to copy / move / compare to.
        /// </summary>
        Target,

        /// <summary>
        /// Indeterminnate between source or destination.
        /// </summary>
        None
    }
}
