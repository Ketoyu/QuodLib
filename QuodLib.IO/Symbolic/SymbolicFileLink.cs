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
            Destination = Info.LinkTarget!;
        }

        /// <summary>
        /// Checks whether the <see cref="Destination"/> exists.
        /// </summary>
        /// <returns></returns>
        public override bool IsBroken()
            => File.Exists(Destination);
    }
}
