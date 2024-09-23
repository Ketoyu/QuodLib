namespace QuodLib.IO.Models.Sync {
    /// <summary>
    /// Error when assigning a <see cref="FileDuplicate"/>'s <see cref="FileDuplicate.Source"/> or <see cref="FileDuplicate.Target"/> more than once.
    /// </summary>
    internal class FileDuplicateCollisionException : AlreadyAssignedException<FileDuplicate, FileInfo> {
        public FileDuplicateCollisionException(string message, FileDuplicate source, string propertyName, FileInfo? attemptedValue)
            : base(message, source, propertyName, attemptedValue) { }
    }
}
