namespace QuodLib.IO.Models
{
    public readonly record struct IOErrorModel(PathType PathType, string Path, Exception Error);
}
