using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.IO {
    public static class Symbolic {
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
            => Get(path, out link, out _);

        /// <summary>
        /// Checks whether the <paramref name="path"/> is a symbolic link. If so, returns a non-null <paramref name="link"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="link"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static SymbolicLinkType Get(string path, out SymbolicLink? link, out FileAttributes attributes) {
            attributes = File.GetAttributes(path);
            link = null;

            bool symbolic = attributes.HasFlag(FileAttributes.ReparsePoint);
            if (!symbolic)
                return SymbolicLinkType.None;

            bool isDir = attributes.HasFlag(FileAttributes.Directory);
            if (isDir) {
                link = new SymbolicDirectoryLink(path);
                return SymbolicLinkType.Directory;
            }

            link = new SymbolicFileLink(path);
            return SymbolicLinkType.File;
        }

        /// <summary>
        /// Contains information about a symbolic link.
        /// </summary>
        public abstract class SymbolicLink {
            /// <summary>
            /// Info about the symbolic link.
            /// </summary>
            protected FileSystemInfo _info { get; init; }

            /// <summary>
            /// Defines whether the symbolic link is to a file or to a directory.
            /// </summary>
            public SymbolicLinkType Type { get; protected init; } //** SymbolicType.None should not never occur here.

            /// <summary>
            /// Checks whether the <see cref="Destination"/> exists.
            /// </summary>
            /// <returns></returns>
            public abstract bool IsBroken();

            /// <summary>
            /// The path being pointed to by the symbolic link.
            /// </summary>
            public string Destination { get; protected init; }

            /// <summary>
            /// The path containing the symbolic link itself.
            /// </summary>
            public string SourceLocation => _info.FullName;
        }

        /// <summary>
        /// Contains information about a symbolic link to another file.
        /// </summary>
        public class SymbolicFileLink : SymbolicLink {
            /// <summary>
            /// Info about the symbolic link.
            /// </summary>
            public FileInfo Info => (FileInfo)_info;
            internal SymbolicFileLink(string path) {
                _info = new FileInfo(path);
                Destination = Info.LinkTarget!;
            }

            /// <summary>
            /// Checks whether the <see cref="Destination"/> exists.
            /// </summary>
            /// <returns></returns>
            public override bool IsBroken()
                => File.Exists(Destination);
        }

        /// <summary>
        /// Contains information about a symbolic link to another directory.
        /// </summary>
        public class SymbolicDirectoryLink : SymbolicLink {
            /// <summary>
            /// Info about the symbolic link.
            /// </summary>
            public DirectoryInfo Info => (DirectoryInfo)_info;

            internal SymbolicDirectoryLink(string path) {
                _info = new DirectoryInfo(path);
                Destination = Info.LinkTarget!;
            }

            /// <summary>
            /// Checks whether the <see cref="Destination"/> exists.
            /// </summary>
            /// <returns></returns>
            public override bool IsBroken()
                => Directory.Exists(Destination);
        }

        /// <summary>
        /// Specifies a type of symbolic link.
        /// </summary>
        public enum SymbolicLinkType {
            /// <summary>
            /// Is not a symbolic link.
            /// </summary>
            None,

            /// <summary>
            /// Points to a directory.
            /// </summary>
            File,

            /// <summary>
            /// Points to a directory.
            /// </summary>
            Directory
        }
    }
}
