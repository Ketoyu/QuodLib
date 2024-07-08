using QuodLib.IO;

namespace IOCL {
    public readonly record struct IOErrorModel(PathType PathType, string Path, Exception Error);
}
