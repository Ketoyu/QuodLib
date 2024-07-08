namespace QuodLib.IO.Symbolic {
    /// <summary>
    /// Contains information about a symbolic link to another directory.
    /// </summary>
    public class SymbolicDirectoryLink : SymbolicLink
    {
        /// <summary>
        /// Info about the symbolic link.
        /// </summary>
        public DirectoryInfo Info => (DirectoryInfo)_info;

        internal SymbolicDirectoryLink(string path) : this(new DirectoryInfo(path))
        { }

        internal SymbolicDirectoryLink(DirectoryInfo info) {
            _info = info;
            Destination = Info.LinkTarget!;
        }

        /// <summary>
        /// Checks whether the <see cref="Destination"/> exists.
        /// </summary>
        /// <returns></returns>
        public override bool IsBroken()
            => Directory.Exists(Destination);
    }
}
