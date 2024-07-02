using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
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
    }
}
