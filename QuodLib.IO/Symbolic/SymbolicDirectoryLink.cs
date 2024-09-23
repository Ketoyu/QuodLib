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
            Target = Info.LinkTarget!;
        }

        /// <summary>
        /// Gets the <see cref="SymbolicLink.LinkStatus"/> of this link.
        /// </summary>
        /// <returns></returns>
        public override LinkStatus GetStatus()
            => SymbolicLink.BuildStatus(true, true, Directory.Exists(Target));
    }
}
