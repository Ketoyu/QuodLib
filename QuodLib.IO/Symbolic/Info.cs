using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
