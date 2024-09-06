namespace QuodLib.IO.Models {
    /// <summary>
    /// A collection of <see cref="IOOperation"/>s to be run.
    /// </summary>
    public sealed class IOBulkOperation {
        /// <summary>
        /// The list of operations to be performed.
        /// </summary>
        public required IList<IOOperation> Operations { get; init; }

        /// <summary>
        /// The total size of source files to be operated on.
        /// </summary>
        public required long Size { get; init; }

        /// <summary>
        /// The total count of source files to be operated on.
        /// </summary>
        public required long Count { get; init; }

        /// <summary>
        /// Run the full list of <see cref="Operations"/>.
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="error"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public async Task RunParallelAsync(IProgress<IOProgressModel> progress, IProgress<IOErrorModel> error, CancellationToken cancel) {
            long sizeDestination = 0;
            long countDestination = 0;
            IProgress<long> pDest = new Progress<long>().OnChange((_, add) => {
                sizeDestination += add;
                countDestination++;
            });

            IProgress<bool> pProg = new Progress<bool>().OnChange((_, success) => {
                progress.Report(new IOProgressModel {
                    SourceSize = Size,
                    SourceCount = Count,
                    CurrentSize = sizeDestination,
                    CurrentCount = countDestination,
                    Success = success
                });
            });

            //Copy folders & files
            await Parallel.ForEachAsync(Operations.ToArray(), cancel, (itm, pcancel) => {
                try {
                    itm.Run();
                    if (itm is FileOperation opFile) {
                        pDest.Report(new FileInfo(opFile.SourcePath).Length);
                        //status.Report(new($"Copying: {cF.Filename_GetPath(itm)}", true));
                    }

                    pProg.Report(true);
                } catch (Exception ex) {
                    switch (itm) {
                        case DirectoryMapOperation opDirMap:
                            error.Report(new(PathType.Folder, opDirMap.SourcePath, ex));
                            break;
                        case FileOperation opFile:
                            error.Report(new(PathType.File, opFile.SourcePath, ex));
                            break;
                        case DirectoryCreateOperation opDirCreate:
                            error.Report(new(PathType.Folder, opDirCreate.TargetPath, ex));
                            break;
                        default:
                            throw new NotSupportedException(itm.GetType().Name);
                    }

                    pProg.Report(false);
                }

                return ValueTask.CompletedTask;
            });
        }
    }
}
