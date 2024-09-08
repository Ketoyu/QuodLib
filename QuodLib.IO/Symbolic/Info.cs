using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuodLib.IO.Symbolic {
    public static class Info
    {
        /// <summary>
        /// Checks whether the <paramref name="path"/> defines a symbolic link.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsSymbolic(string path)
            => File.GetAttributes(path).IsSymbolic();

        /// <summary>
        /// Checks whether the <paramref name="attributes"/> contains the <see cref="FileAttributes.ReparsePoint"/> tag.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static bool IsSymbolic(this FileAttributes attributes)
            => attributes.HasFlag(FileAttributes.ReparsePoint);

        /// <summary>
        /// Checks whether the <paramref name="info"/> is a symbolic link.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool IsSymbolic(this FileInfo info)
            => info.Attributes.HasFlag(FileAttributes.ReparsePoint);

        /// <summary>
        /// Checks whether the <paramref name="info"/> is a symbolic link.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool IsSymbolic(this DirectoryInfo info)
            => info.Attributes.HasFlag(FileAttributes.ReparsePoint);

        /// <summary>
        /// Checks whether the <paramref name="path"/> is a symbolic link. If so, returns a non-null <paramref name="link"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public static SymbolicLinkType TryGet(string path, out SymbolicLink? link)
            => TryGet(path, out link, out _);

        /// <summary>
        /// Checks whether the <paramref name="info"/> defines a symbolic link. If so, returns a non-null <paramref name="link"/>.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public static bool TryGet(FileInfo info, out SymbolicLink? link) {
            if (!info.IsSymbolic()) {
                link = null;
                return false;
            }

            link = new SymbolicFileLink(info);
            return true;
        }

        /// <summary>
        /// Checks whether the <paramref name="info"/> defines a symbolic link. If so, returns a non-null <paramref name="link"/>.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public static bool TryGet(DirectoryInfo info, out SymbolicLink? link) {
            if (!info.IsSymbolic()) {
                link = null;
                return false;
            }

            link = new SymbolicDirectoryLink(info);
            return true;
        }

        /// <summary>
        /// Checks whether the <paramref name="path"/> is a symbolic link. If so, returns a non-null <paramref name="link"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="link"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static SymbolicLinkType TryGet(string path, out SymbolicLink? link, out FileAttributes attributes)
        {
            attributes = File.GetAttributes(path);
            link = null;

            bool symbolic = attributes.HasFlag(FileAttributes.ReparsePoint);
            if (!symbolic)
                return SymbolicLinkType.None;

            bool isDir = attributes.HasFlag(FileAttributes.Directory);
            if (isDir)
            {
                link = new SymbolicDirectoryLink(path);
                return SymbolicLinkType.Directory;
            }

            link = new SymbolicFileLink(path);
            return SymbolicLinkType.File;
        }

        /// <summary>
        /// Checks whether the <paramref name="sourcePath"/> is a symbolic link and the <see cref="SymbolicLink.LinkStatus"/> between the <paramref name="sourcePath"/> and <paramref name="expectedTarget"/>.
        /// </summary>
        /// <param name="sourcePath">The source path to link from.</param>
        /// <param name="expectedTarget">The source path to link to.</param>
        /// <param name="link">The source <see cref="SymbolicLink"/>, if it already exists.</param>
        /// <param name="status">The <see cref="SymbolicLink.LinkStatus"/> between the <paramref name="sourcePath"/> and <paramref name="expectedTarget"/>.</param>
        /// <param name="sourceAttributes">The <see cref="FileAttributes"/> of the <paramref name="sourcePath"/> if it exists.</param>
        /// <param name="targetAttributes">The <see cref="FileAttributes"/> of the <paramref name="expectedTarget"/> if it exists.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static SymbolicLinkType TryGet(string sourcePath, string expectedTarget, out SymbolicLink? link, out SymbolicLink.LinkStatus status, out FileAttributes? sourceAttributes, out FileAttributes? targetAttributes) {
            (bool isDir_source, bool sourceExists, sourceAttributes) = isDir(sourcePath);
            (bool isDir_target, bool targetExists, targetAttributes) = isDir(expectedTarget);

            if (isDir_source !=  isDir_target)
                throw new ArgumentException($"Expected {nameof(sourcePath)} and {nameof(expectedTarget)} to both be files or both be directories.");

            SymbolicLinkType linkType = TryGet(sourcePath, out link, out FileAttributes sourceFA);
            sourceAttributes = sourceFA;

            if (link != null) {
                status = link.Target != expectedTarget
                    ? SymbolicLink.LinkStatus.MismatchedTarget
                    : link.GetStatus();

                return linkType;
            }

            status = SymbolicLink.BuildStatus(false, sourceExists, targetExists);
            return SymbolicLinkType.None;

            //---- (local method) ----
            static (bool, bool, FileAttributes?) isDir(string path) {
                
                if (File.Exists(path) || Directory.Exists(path)) {
                    FileAttributes fa = File.GetAttributes(path);
                    return (fa.HasFlag(FileAttributes.Directory), true, fa);
                }

                return (string.IsNullOrEmpty(Path.GetExtension(path)), false, null);
            }
        }

    }
}
