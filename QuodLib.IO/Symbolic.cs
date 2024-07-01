using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.IO {
    public static class Symbolic {
        public static bool IsSymbolic(string path)
            => File.GetAttributes(path).IsSymbolic();

        public static bool IsSymbolic(this FileAttributes attributes)
            => attributes.HasFlag(FileAttributes.ReparsePoint);

        public static bool IsSymbolic(this FileInfo fileInfo)
            => fileInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);
        public static bool IsSymbolic(this DirectoryInfo dirInfo)
            => dirInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);

        public static SymbolicType Get(string path, out SymbolicLink? link)
            => Get(path, out link, out _);

        public static SymbolicType Get(string path, out SymbolicLink? link, out FileAttributes attributes) {
            attributes = File.GetAttributes(path);
            link = null;

            bool symbolic = attributes.HasFlag(FileAttributes.ReparsePoint);
            if (!symbolic)
                return SymbolicType.None;

            bool isDir = attributes.HasFlag(FileAttributes.Directory);
            if (isDir) {
                link = new SymbolicDirectoryLink(path);
                return SymbolicType.Directory;
            }

            link = new SymbolicFileLink(path);
            return SymbolicType.File;
        }

        public abstract class SymbolicLink {
            protected FileSystemInfo _info { get; init; }
            public SymbolicType Type { get; protected init; }

            public abstract bool IsBroken();
            public string Destination { get; protected init; }
            public string SourceLocation => _info.FullName;
        }

        public class SymbolicFileLink : SymbolicLink {
            public FileInfo Info => (FileInfo)_info;
            internal SymbolicFileLink(string path) {
                _info = new FileInfo(path);
            }

            public override bool IsBroken()
                => File.Exists(Destination);
        }

        public class SymbolicDirectoryLink : SymbolicLink {
            public DirectoryInfo Info => (DirectoryInfo)_info;

            internal SymbolicDirectoryLink(string path) {
                _info = new DirectoryInfo(path);
                Destination = Info.LinkTarget!;
            }

            public override bool IsBroken()
                => Directory.Exists(Destination);
        }

        public enum SymbolicType {
            None,
            File,
            Directory
        }
    }
}
