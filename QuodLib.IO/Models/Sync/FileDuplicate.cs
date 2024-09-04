namespace QuodLib.IO.Models.Sync {
    /// <summary>
    /// Holds information about two files with the same name in differing locations.
    /// </summary>
    public class FileDuplicate {
        private FileInfo? _source;
        /// <summary>
        /// The source file to copy / move from.
        /// </summary>
        public FileInfo? Source {
            get => _source;
            set {
                if (value == null)
                    throw new ArgumentException("Assigned value cannot be null", nameof(Source));

                if (_source != null)
                    throw new FileDuplicateCollisionException("Property already assigned", this, nameof(Source), value);

                _source = value;
            }
        }

        private FileInfo? _target;
        /// <summary>
        /// The destination file to copy / move to.
        /// </summary>
        public FileInfo? Target {
            get => _target;
            set {
                if (value == null)
                    throw new ArgumentException("Assigned value cannot be null", nameof(Target));

                if (_target != null)
                    throw new FileDuplicateCollisionException("Property already assigned", this, nameof(Target), value);

                _target = value;
            }
        }

        FileOrigin? _newer;
        /// <summary>
        /// Information about which <i>(if either)</i> was newer between the <see cref="Source"/> or <see cref="Target"/>.
        /// </summary>
        public FileOrigin? Newer {
            get {
                if (_newer != null)
                    return _newer;

                if (Source == null || Target == null)
                    return null;

                if (Source.LastWriteTime > Target.LastWriteTime)
                    return _newer = FileOrigin.Source;

                if (Target.LastWriteTime > Source.LastWriteTime)
                    return _newer = FileOrigin.Target;

                return _newer = FileOrigin.None;
            }
        }
    }
}
