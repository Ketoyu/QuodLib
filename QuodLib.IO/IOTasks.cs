﻿using NickChapsas.ParallelForEachAsync;
using QuodLib.IO.Models;
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
        /// A fluid implementation of adding <paramref name="progressChanged"/> to <see cref="Progress{T}.ProgressChanged"/> of <paramref name="progress"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="progress">The <see cref="Progress{T}"/> to affect.</param>
        /// <param name="progressChanged">The <see cref="Action{TEventArgs}"/> to attach.</param>
        /// <returns></returns>
        public static Progress<T> OnChange<T>(this Progress<T> progress, Action<T> progressChanged) {
            progress.ProgressChanged += (_, data) => progressChanged(data);
            return progress;
        }

        /// <summary>
        /// Options for controlling the way that <see cref="TraverseFilesAsync(IList{string}, IProgress{FileInfo}, IProgress{IOErrorModel}, CancellationToken, TraverseFilesAsyncOptions?)"/> traverses through directories and reports information.
        /// </summary>
        public sealed class TraverseFilesAsyncOptions {
            public struct SkipNestModel {
                public required bool Skip { get; init; }

                /// <summary>
                /// Whether to still process the <see cref="FileNest"/>'s files when <see cref="Skip"/> is true.
                /// </summary>
                public required bool IncludeFiles { get; init; }
            }

            /// <summary>
            /// When to process <see cref="SkipNest"/>.
            /// </summary>
            public enum SkipNestStyle {
                /// <summary>
                /// If <see cref="SkipNest"/> returns true, abort the current <see cref="FileNest"/> immediately.
                /// </summary>
                IgnoreFiles,

                /// <summary>
                /// If <see cref="SkipNest"/> returns true, process any files from the current <see cref="FileNest"/> before aborting it.
                /// </summary>
                IncludeFiles
            }

            /// <summary>
            /// Reports a <see cref="Symbolic.SymbolicLink"/>.
            /// </summary>
            public IProgress<SymbolicLink>? SymbolicLink { get; init; }

            /// <summary>
            /// Reports a directory that may have files, but has no sub-directories.
            /// </summary>
            public IProgress<DirectoryInfo>? LeafDirectory { get; init; }

            private Func<string, bool>? _skipSubdirectory;
            /// <summary>
            /// Don't nest into this subdirectory.
            /// </summary>
            public Func<string, bool>? SkipSubdirectory {
                get => _skipSubdirectory;
                init => _skipSubdirectory = value;
            }

            /// <summary>
            /// Given the files and subdirectories of the <b><i>current</i></b> directory, don't scan this level or nest further.
            /// </summary>
            public Func<FileNest, SkipNestModel>? SkipNest { get; init; }

            /// <summary>
            /// Don't nest into these subdirectories.
            /// </summary>
            private HashSet<string>? SkipSources { get; set; }

            /// <summary>
            /// Sets or augments <see cref="SkipSubdirectory"/> to check <paramref name="sources"/>.
            /// </summary>
            /// <param name="sources"></param>
            public TraverseFilesAsyncOptions SkipSubdirectories(ICollection<string> sources) {
                if (SkipSources == null) {
                    SkipSources = new HashSet<string>(sources);
                    _skipSubdirectory = _skipSubdirectory == null
                        ? subdir => SkipSources.Contains(subdir)
                        : subdir => _skipSubdirectory(subdir) || SkipSources.Contains(subdir);
                } else
                    SkipSources.UnionWith(sources);

                return this;
            }
        }

        /// <summary>
        /// Scans directories and sub-directories, reporting <see cref="FileInfo"/>s and directories that need copied.
        /// </summary>
        /// <param name="root">The directory to perform a nested scan on.</param>
        /// <param name="file">Reports a <see cref="FileInfo"/></param>
        /// <param name="error">Reports an <see cref="IOErrorModel"/></param>
        /// <param name="cancel"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <remarks>Does not nest into <see cref="SymbolicLink"/>s.</remarks>
        public static Task TraverseFilesAsync(string root, IProgress<FileInfo> file, IProgress<IOErrorModel> error, CancellationToken cancel, TraverseFilesAsyncOptions? options = null)
            => TraverseFilesAsync(new string[] { root }, file, error, cancel, options);

        /// <summary>
        /// Scans directories and sub-directories, reporting <see cref="FileInfo"/>s and directories that need copied.
        /// </summary>
        /// <param name="sources">List of directories to scan</param>
        /// <param name="file">Reports a <see cref="FileInfo"/></param>
        /// <param name="error">Reports an <see cref="IOErrorModel"/></param>
        /// <param name="cancel"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <remarks>Does not nest into <see cref="SymbolicLink"/>s.</remarks>
        public async static Task TraverseFilesAsync(IList<string> sources, IProgress<FileInfo> file, IProgress<IOErrorModel> error, CancellationToken cancel, TraverseFilesAsyncOptions? options = null) {
            List<string?> lDirs_init = await sources.ParallelForEachAsync<string, string?>(Environment.ProcessorCount, dir => {
                try {
                    if (Info.TryGet(dir, out SymbolicLink? link) != SymbolicLinkType.None) {
                        options?.SymbolicLink?.Report(link!);
                        return Task.FromResult<string?>(null);
                    }
                }
                catch (Exception ex) {
                    error.Report(new(PathType.Folder, dir, ex));
                    return Task.FromResult<string?>(null);
                }

                return Task.FromResult<string?>(dir);
            });
            Stack<string> stkDirs_init = new(lDirs_init.OfType<string>());
            Stack<string> stkDir_root = new();

            //nested dir-list
            while (stkDirs_init.Count > 0) {
                if (cancel.IsCancellationRequested)
                    return;


                string nested = stkDirs_init.Pop(); 
                stkDir_root.Push(nested); //hold one root-dir.

                while (stkDir_root.Count > 0) //for (that root-dir)
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
                        if (options?.SkipSubdirectory?.Invoke(root) ?? false)
                            continue;

                        rootInfo = new DirectoryInfo(root);

                        if (Info.TryGet(rootInfo, out SymbolicLink? link)) {
                            options?.SymbolicLink?.Report(link!);
                            continue;
                        }

                        files = Directory.GetFiles(root);
                        subdirs = Directory.GetDirectories(root);

                    } catch (Exception ex) {
                        error.Report(new(PathType.Folder, root, ex));
                        continue;
                    }

                    TraverseFilesAsyncOptions.SkipNestModel skipNest = options?.SkipNest?.Invoke(new FileNest {
                        Directory = rootInfo!,
                        Filepaths = files,
                        Subdirectories = subdirs
                    }) ?? new TraverseFilesAsyncOptions.SkipNestModel() { 
                        Skip = false, 
                        IncludeFiles = false 
                    };

                    //all files
                    if (!skipNest.Skip || skipNest.IncludeFiles) {
                        try {
                            await Parallel.ForEachAsync(
                                files, 
                                new ParallelOptions() {
                                    MaxDegreeOfParallelism = Environment.ProcessorCount
                                },
                                (fl, _) => {
                                    try {
                                        FileInfo fI = new(fl);

                                        if (Info.TryGet(fI, out SymbolicLink? link))
                                            options?.SymbolicLink?.Report(link!);
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
                    }

                    //subdirectories
                    if (!skipNest.Skip) {
                        try {
                            List<string?> newRoots = await subdirs.ParallelForEachAsync(Environment.ProcessorCount, subdir => {
                                //Ignore symbolic
                                try {
                                    if (options?.SkipSubdirectory?.Invoke(subdir) ?? false)
                                        return Task.FromResult<string?>(null);

                                    if (Info.TryGet(subdir, out SymbolicLink? link) != SymbolicLinkType.None) {
                                        options?.SymbolicLink?.Report(link!);
                                        return Task.FromResult<string?>(null);
                                    }
                                    
                                    return Task.FromResult<string?>(subdir);
                                } catch (Exception ex) {
                                    error.Report(new(PathType.Folder, subdir, ex));
                                    return Task.FromResult<string?>(null);
                                }
                            });

                            var newRoots_real = newRoots.OfType<string>();

                            if (newRoots_real.Any()) {
                                hasSubdirs = true;
                                foreach (string subdir in newRoots_real)
                                    stkDir_root.Push(subdir);
                            }
                        } catch (Exception ex) {
                            error.Report(new(PathType.File, root, ex));
                        }
                    }

                    if (!hasSubdirs)
                        options?.LeafDirectory?.Report(rootInfo!);
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
