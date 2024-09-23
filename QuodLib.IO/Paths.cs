using System.IO;

namespace QuodLib.IO {
    public static class Paths {
        /// <summary>
        /// Resolves the <paramref name="sourcePath"/> filepath minus the root drive or <paramref name="relativeTo"/>, into the <paramref name="targetDirectory"/>.
        /// </summary>
        /// <param name="targetDirectory">The target directory</param>
        /// <param name="sourcePath">The source filepath</param>
        /// <param name="relativeTo">The path to trim from the <paramref name="sourcePath"/></param>
        /// <returns>The updated destination</returns>
        public static string Resolve(string targetDirectory, string sourcePath, string? relativeTo) {
            string partialSource = Path.GetRelativePath(
                    !string.IsNullOrEmpty(relativeTo) && sourcePath.StartsWith(relativeTo)
                        ? relativeTo
                        : Path.GetPathRoot(sourcePath)!,
                    sourcePath);

            return Path.Combine(targetDirectory, partialSource);
        }

        public static string ReplaceInvalidPathChars(string folderName, char replacement = '_')
            => string.Join(replacement, folderName.Split(Path.GetInvalidPathChars()));

        public static string ReplaceInvalidFileNameChars(string folderName, char replacement = '_')
            => string.Join(replacement, folderName.Split(Path.GetInvalidFileNameChars()));
    }
}
