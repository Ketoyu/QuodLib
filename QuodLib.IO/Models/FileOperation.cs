namespace QuodLib.IO.Models {
    /// <summary>
    /// Pending instruction to copy or move a file.
    /// </summary>
    public class FileOperation : IOOperation {
        /// <summary>
        /// The source path to copy from.
        /// </summary>
        public required string SourcePath { get; init; }

        /// <summary>
        /// Copy or Move.
        /// </summary>
        public required FileOperationType Operation { get; init; }

        /// <summary>
        /// Whether or when to overwrite the destination file.
        /// </summary>
        public required OverwriteOption Overwrite { get; init; }

        /// <summary>
        /// Run the planned file operation.
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        public override void Run() {
            switch (Operation) {
                case FileOperationType.Copy:
                    if (Overwrite == OverwriteOption.IfNewer)
                        Files.CopyIfNewer(SourcePath, TargetPath);
                    else
                        File.Copy(SourcePath, TargetPath, Overwrite == OverwriteOption.Yes);
                    return;

                case FileOperationType.Move:
                    if (Overwrite == OverwriteOption.IfNewer)
                        Files.MoveIfNewer(SourcePath, TargetPath);
                    else
                        File.Move(SourcePath, TargetPath, Overwrite == OverwriteOption.Yes);
                    return;

                default:
                    throw new NotSupportedException($"{nameof(FileOperationType.Delete)} not yet supported");
            }
        }
    }
}
