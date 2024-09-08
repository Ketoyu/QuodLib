namespace QuodLib.IO.Symbolic {
    /// <summary>
    /// Contains information about a symbolic link to another file.
    /// </summary>
    public class SymbolicFileLink : SymbolicLink
    {
        /// <summary>
        /// Info about the symbolic link.
        /// </summary>
        public FileInfo Info => (FileInfo)_info;
        internal SymbolicFileLink(string path) : this(new FileInfo(path))
        { }

        internal SymbolicFileLink(FileInfo info) {
            _info = info;
            Target = Info.LinkTarget!;
        }

        /// <summary>
        /// Gets the <see cref="SymbolicLink.LinkStatus"/> of this link.
        /// </summary>
        /// <returns></returns>
        public override LinkStatus GetStatus()
            => SymbolicLink.BuildStatus(true, true, File.Exists(Target));
    }
}
