using System.Diagnostics;

namespace QuodLib.IO.Models {
    /// <summary>
    /// Pending instruction to create a directory.
    /// </summary>
    public class DirectoryCreateOperation : IOOperation {
        /// <summary>
        /// Run the planned operation.
        /// </summary>
        public override void Run()
            => Directory.CreateDirectory(base.TargetPath);
    }
}
