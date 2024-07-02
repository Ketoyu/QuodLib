namespace QuodLib.IO.Symbolic {
    /// <summary>
    /// Specifies a type of symbolic link.
    /// </summary>
    public enum SymbolicLinkType
    {
        /// <summary>
        /// Is not a symbolic link.
        /// </summary>
        None,

        /// <summary>
        /// Points to a directory.
        /// </summary>
        File,

        /// <summary>
        /// Points to a directory.
        /// </summary>
        Directory
    }
}
