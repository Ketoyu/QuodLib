namespace QuodLib.IO.Symbolic {
    /// <summary>
    /// Contains information about a symbolic link.
    /// </summary>
    public abstract class SymbolicLink
    {
        /// <summary>
        /// Info about the symbolic link.
        /// </summary>
        protected FileSystemInfo _info { get; init; }

        /// <summary>
        /// Defines whether the symbolic link is to a file or to a directory.
        /// </summary>
        public SymbolicLinkType Type { get; protected init; } //** SymbolicType.None should not never occur here.

        /// <summary>
        /// Checks whether the <see cref="Destination"/> exists.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsBroken();

        /// <summary>
        /// The path being pointed to by the symbolic link.
        /// </summary>
        public string Destination { get; protected init; }

        /// <summary>
        /// The path containing the symbolic link itself.
        /// </summary>
        public string SourceLocation => _info.FullName;
    }
}
