using IOCL;
using QuodLib.IO.Symbolic;

namespace QuodLib.IO
{
    /// <summary>
    /// A class of methods for running System.IO methods asynchronously.
    /// </summary>
    public static class IOTasks {

        /// <summary>
        /// A fluid implementation of adding <paramref name="progressChanged"/> to <see cref="Progress{T}.ProgressChanged"/> of <paramref name="progress"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="progress">The <see cref="Progress{T}"/> to affect.</param>
        /// <param name="progressChanged">The <see cref="EventHandler{TEventArgs}"/> to attach.</param>
        /// <returns></returns>
        public static Progress<T> OnChange<T>(this Progress<T> progress, EventHandler<T> progressChanged) {
            progress.ProgressChanged += progressChanged;
            return progress;
        }

        /// <summary>
        /// Scans directories and sub-directories, reporting <see cref="FileInfo"/>s and directories that need copied.
        /// </summary>
        /// <param name="root">The directory to perform a nested scan on.</param>
        /// <param name="symbolicLink">Reports a <see cref="SymbolicLink"/></param>
        /// <param name="file">Reports a <see cref="FileInfo"/></param>
        /// <param name="leafDirectory">Reports a directory that may have files, but has no sub-directories</param>
        /// <param name="error">Reports an <see cref="IOErrorModel"/></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <remarks>Does not nest into <see cref="SymbolicLink"/>s.</remarks>
        public static Task TraverseFilesAsync(string root, IProgress<SymbolicLink>? symbolicLink, IProgress<FileInfo> file, IProgress<DirectoryInfo> leafDirectory, IProgress<IOErrorModel> error, CancellationToken cancel)
            => TraverseFilesAsync(new string[] { root }, (TraverseFilesAsyncSkipOptions?)null, symbolicLink, file, leafDirectory, error, cancel);

        /// <summary>
        /// Scans directories and sub-directories, reporting <see cref="FileInfo"/>s and directories that need copied.
        /// </summary>
        /// <param name="root">The directory to perform a nested scan on.</param>
        /// <param name="skipSources">List of (sub-)directories to ignore</param>
        /// <param name="symbolicLink">Reports a <see cref="SymbolicLink"/></param>
        /// <param name="file">Reports a <see cref="FileInfo"/></param>
        /// <param name="leafDirectory">Reports a directory that may have files, but has no sub-directories</param>
        /// <param name="error">Reports an <see cref="IOErrorModel"/></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <remarks>Does not nest into <see cref="SymbolicLink"/>s.</remarks>
        public static Task TraverseFilesAsync(string root, IList<string> skipSources, IProgress<SymbolicLink>? symbolicLink, IProgress<FileInfo> file, IProgress<DirectoryInfo> leafDirectory, IProgress<IOErrorModel> error, CancellationToken cancel)
            => TraverseFilesAsync(
                new string[] { root }, 
                new TraverseFilesAsyncSkipOptions { SkipSubdirectory = subdir => skipSources.Contains(subdir) }, 
                symbolicLink, file, leafDirectory, error, cancel);

        /// <summary>
        /// Scans directories and sub-directories, reporting <see cref="FileInfo"/>s and directories that need copied.
        /// </summary>
        /// <param name="sources">List of directories to scan</param>
        /// <param name="skipSources">List of (sub-)directories to ignore</param>
        /// <param name="symbolicLink">Reports a <see cref="SymbolicLink"/></param>
        /// <param name="file">Reports a <see cref="FileInfo"/></param>
        /// <param name="leafDirectory">Reports a directory that may have files, but has no sub-directories</param>
        /// <param name="error">Reports an <see cref="IOErrorModel"/></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <remarks>Does not nest into <see cref="SymbolicLink"/>s.</remarks>
        public static Task TraverseFilesAsync(IList<string> sources, IList<string> skipSources, IProgress<SymbolicLink>? symbolicLink, IProgress<FileInfo> file, IProgress<DirectoryInfo> leafDirectory, IProgress<IOErrorModel> error, CancellationToken cancel)
            => TraverseFilesAsync(sources, 
                new TraverseFilesAsyncSkipOptions { SkipSubdirectory = subdir => skipSources.Contains(subdir) }, 
                symbolicLink, file, leafDirectory, error, cancel);

        /// <summary>
        /// Options for controlling the way that <see cref="TraverseFilesAsync(IList{string}, TraverseFilesAsyncSkipOptions?, IProgress{SymbolicLink}?, IProgress{FileInfo}, IProgress{DirectoryInfo}, IProgress{IOErrorModel}, CancellationToken)"/> traverses through directories.
        /// </summary>
        public class TraverseFilesAsyncSkipOptions {
            /// <summary>
            /// Don't nest into this subdirectory.
            /// </summary>
            public Func<string, bool>? SkipSubdirectory { get; init; }

            /// <summary>
            /// Given the files and subdirectories of the <b><i>current</i></b> directory, don't scan this level or nest further.
            /// </summary>
            public Func<FileNest, bool>? SkipNest { get; init; }
        }

        /// <summary>
        /// Scans directories and sub-directories, reporting <see cref="FileInfo"/>s and directories that need copied.
        /// </summary>
        /// <param name="sources">List of directories to scan</param>
        /// <param name="skipOptions">Options for (sub-)directories to ignore or to not nest into</param>
        /// <param name="symbolicLink">Reports a <see cref="SymbolicLink"/></param>
        /// <param name="file">Reports a <see cref="FileInfo"/></param>
        /// <param name="leafDirectory">Reports a directory that may have files, but has no sub-directories</param>
        /// <param name="error">Reports an <see cref="IOErrorModel"/></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <remarks>Does not nest into <see cref="SymbolicLink"/>s.</remarks>
        public async static Task TraverseFilesAsync(IList<string> sources, TraverseFilesAsyncSkipOptions? skipOptions, IProgress<SymbolicLink>? symbolicLink, IProgress<FileInfo> file, IProgress<DirectoryInfo> leafDirectory, IProgress<IOErrorModel> error, CancellationToken cancel) {
            Stack<string> stkDirs_init = new();
            IProgress<string> pinit = new Progress<string>().OnChange((_, dir) => stkDirs_init.Push(dir));

            await Parallel.ForEachAsync(sources, cancel, (dir, _) => {
                try {
                    if (Info.TryGet(dir, out SymbolicLink? link) != SymbolicLinkType.None) {
                        symbolicLink?.Report(link!);
                        return ValueTask.CompletedTask;
                    }
                } catch (Exception ex) {
                    error.Report(new(PathType.Folder, dir, ex));
                    return ValueTask.CompletedTask;
                }

                pinit.Report(dir);
                return ValueTask.CompletedTask;
            });

            Stack<string> stkDir_root = new();
            IProgress<string> proot = new Progress<string>().OnChange((dir_, dir) => stkDir_root.Push(dir));

            //nested dir-list
            while (stkDirs_init.Any()) {
                if (cancel.IsCancellationRequested)
                    return;

                stkDir_root.Push(stkDirs_init.Pop()); //hold one root-dir.

                while (stkDir_root.Any()) //for (that root-dir)
                {
                    if (cancel.IsCancellationRequested)
                        return;

                    bool hasSubdirs = false;
                    string root = stkDir_root.Pop();
                    DirectoryInfo? rootInfo = null;

                    string[] files;
                    string[] subdirs;

                    //ignore
                    try {
                        if (skipOptions?.SkipSubdirectory?.Invoke(root) ?? false)
                            continue;

                        rootInfo = new DirectoryInfo(root);

                        if (Info.TryGet(rootInfo, out SymbolicLink? link)) {
                            symbolicLink?.Report(link!);
                            continue;
                        }

                        files = Directory.GetFiles(root);
                        subdirs = Directory.GetDirectories(root);

                    } catch (Exception ex) {
                        error.Report(new(PathType.Folder, root, ex));
                        continue;
                    }

                    if (!skipOptions?.SkipNest?.Invoke(new FileNest {
                        Directory = rootInfo!,
                        Filepaths = files,
                        Subdirectories = subdirs
                    }) ?? true) {
                        //all files
                        try {
                            await Parallel.ForEachAsync(files, cancel, (fl, _) => {
                                try {
                                    FileInfo fI = new(fl);

                                    if (Info.TryGet(fI, out SymbolicLink? link))
                                        symbolicLink?.Report(link!);
                                    else
                                        file.Report(fI);
                                } catch (Exception ex) {
                                    error.Report(new(PathType.File, fl, ex));
                                }

                                return ValueTask.CompletedTask;
                            });
                        } catch (Exception ex) {
                            error.Report(new(PathType.File, root, ex));
                        }

                        //subdirectories
                        try {
                            

                            await Parallel.ForEachAsync(subdirs, cancel, (subdir, _) => {
                                //Ignore symbolic
                                try {
                                    if (skipOptions?.SkipSubdirectory?.Invoke(subdir) ?? false)
                                        return ValueTask.CompletedTask;

                                    if (Info.TryGet(subdir, out SymbolicLink? link) != SymbolicLinkType.None) {
                                        symbolicLink?.Report(link!);
                                        return ValueTask.CompletedTask;
                                    } else {
                                        proot.Report(subdir);
                                        hasSubdirs = true;
                                    }
                                } catch (Exception ex) {
                                    error.Report(new(PathType.Folder, subdir, ex));
                                }

                                return ValueTask.CompletedTask;
                            });
                        } catch (Exception ex) {
                            error.Report(new(PathType.File, root, ex));
                        }
                    }

                    if (!hasSubdirs)
                        leafDirectory.Report(rootInfo!);
                }
            }
        }
    }

    /// <summary>
    /// Information on some level of nested subdirectory.
    /// </summary>
    public class FileNest {
        /// <summary>
        /// The current directory.
        /// </summary>
        public DirectoryInfo Directory {  get; internal init; }

        /// <summary>
        /// Filepaths contained within the <see cref="Directory"/>.
        /// </summary>
        public IReadOnlyList<string> Filepaths { get; internal init; }

        /// <summary>
        /// Subdirectories contained within the <see cref="Directory"/>.
        /// </summary>
        public IReadOnlyList<string> Subdirectories { get; internal init; }
    }
}
