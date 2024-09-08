namespace QuodLib.IO.Symbolic {
    /// <summary>
    /// Contains information about a symbolic link.
    /// </summary>
    public abstract class SymbolicLink
    {
        public enum LinkStatus {
            /// <summary>
            /// The link is currently operational.
            /// </summary>
            Connected,

            /// <summary>
            /// The link exists, but the <see cref="Target"/> does not exist.
            /// </summary>
            Broken,

            /// <summary>
            /// The link can be connected. The <see cref="Source"/> does not exist, but the <see cref="Target"/> does.
            /// The parent path of the <see cref="Source"/> and of the <see cref="Target"/> may or may not need to be created before connecting.
            /// </summary>
            ReadyToConnect,

            /// <summary>
            /// The <see cref="Source"/> must be moved to the <see cref="Target"/> in order to connect the link.
            /// </summary>
            MustMove,

            /// <summary>
            /// The <see cref="Target"/> must be created in order to connect the link. The <see cref="Source"/> does not exist.
            /// </summary>
            MustCreateTarget,

            /// <summary>
            /// Potentially conflicting data between the <see cref="Source"/> and <see cref="Target"/> must be resolved in order to connect the link.
            /// </summary>
            DataConflicts,

            /// <summary>
            /// The link exists, but points to a different <see cref="Target"/> than the supplied path.
            /// </summary>
            MismatchedTarget //External use only (because there exists one Source and two Targets { actual, supplied }.
        }

        /// <summary>
        /// Info about the symbolic link.
        /// </summary>
        protected FileSystemInfo _info { get; init; }

        /// <summary>
        /// Defines whether the symbolic link is to a file or to a directory.
        /// </summary>
        public SymbolicLinkType Type { get; protected init; } //** SymbolicType.None should not never occur here.

        /// <summary>
        /// Checks whether the <see cref="Target"/> exists.
        /// </summary>
        /// <returns></returns>
        public abstract LinkStatus GetStatus();

        /// <summary>
        /// The path being pointed to by the symbolic link.
        /// </summary>
        public string Target { get; protected init; }

        /// <summary>
        /// The path containing the symbolic link itself.
        /// </summary>
        public string Source => _info.FullName;

        /// <summary>
        /// Takes in some information and builds a <see cref="LinkStatus"/> from it.
        /// </summary>
        /// <param name="linkExists"></param>
        /// <param name="sourceExists"></param>
        /// <param name="targetExists"></param>
        /// <returns></returns>
        internal static LinkStatus BuildStatus(bool linkExists, bool sourceExists, bool targetExists) {
            if (linkExists)
                return targetExists
                    ? LinkStatus.Connected
                    : LinkStatus.Broken;

            if (sourceExists)
                return targetExists
                    ? LinkStatus.DataConflicts
                    : LinkStatus.MustMove;

            if (targetExists)
                return LinkStatus.ReadyToConnect;
            
            return LinkStatus.MustCreateTarget;
        }
    }
}
