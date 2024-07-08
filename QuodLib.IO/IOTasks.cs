using IOCL;
using QuodLib.IO.Symbolic;

namespace QuodLib.IO
{
    /// <summary>
    /// A class of methods for running System.IO methods asynchronously.
    /// </summary>
    public static class IOTasks {

        public static Task<bool> File_ExistsAsync(string path)
            => RunWithRethrow(() => File.Exists(path));

        public static Task<bool> Dir_ExistsAsync(string path)
            => RunWithRethrow(() => Directory.Exists(path));

        public static Task<bool> Path_IsSymbolicAsync(string path)
            => RunWithRethrow(() => Info.IsSymbolic(path));

        public static Task<string[]> GetDirectoriesAsync(string path)
            => RunWithRethrow(() => Directory.GetDirectories(path));

        public static Task<string[]> GetFilesAsync(string path)
            => RunWithRethrow(() => Directory.GetFiles(path));

        public static Task<FileInfo> FileInfo_NewAsync(string fileName)
            => RunWithRethrow(() => new FileInfo(fileName));

        public static Task<DirectoryInfo> DirectoryInfo_NewAsync(string dirName)
            => RunWithRethrow(() => new DirectoryInfo(dirName));

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

        public static async Task<T> RunWithRethrow<T>(Func<T> getValue) {
            T rtn = default(T);
            Exception rethrow = null;
            await Task.Run(() => {
                try {
                    rtn = getValue();
                } catch (Exception ex) {
                    rethrow = ex;
                }
            });

            if (rethrow != null)
                throw rethrow;

            return rtn;
        }

        /// <summary>
        /// Scans directories and sub-directories, reporting <see cref="FileInfo"/>s and directories that need copied.
        /// </summary>
        /// <param name="sources">List of directories to scan</param>
        /// <param name="skipSources">List of (sub-)directories to ignore</param>
        /// <param name="symbolicLink">Reports a <see cref="SymbolicLink"/></param>
        /// <param name="file">Reports a <see cref="FileInfo"/></param>
        /// <param name="finalDirectory">Returns a directory that may have files, but has no sub-directories</param>
        /// <param name="error">Reports an <see cref="ErrorModel"/></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public async static Task TraverseFilesAsync(IList<string> sources, IList<string> skipSources, IProgress<SymbolicLink>? symbolicLink, IProgress<FileInfo> file, IProgress<string> finalDirectory, IProgress<IOErrorModel> error, CancellationToken cancel) {
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

                    //ignore
                    try {
                        if (skipSources.Contains(root))
                            continue;

                        if (Info.TryGet(root, out SymbolicLink? link) != SymbolicLinkType.None) {
                            symbolicLink?.Report(link!);
                            continue;
                        }
                    } catch (Exception ex) {
                        error.Report(new(PathType.Folder, root, ex));
                        continue;
                    }

                    //all files
                    try {
                        string[] files = Directory.GetFiles(root);

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
                        string[] subdirs = Directory.GetDirectories(root);

                        await Parallel.ForEachAsync(subdirs, cancel, (subdir, _) => {
                            //Ignore symbolic
                            try {
                                if (skipSources.Contains(subdir))
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

                    if (!hasSubdirs)
                        finalDirectory.Report(root);
                }
            }
        }
    }
}
