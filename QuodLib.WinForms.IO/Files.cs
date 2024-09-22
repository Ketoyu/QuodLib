//using System.Text;
//using System.Threading.Tasks;

using DialogResult = System.Windows.Forms.DialogResult;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;
//using WinMusicProperties = Windows.Storage.FileProperties.MusicProperties;
//using StorageFile = Windows.Storage.StorageFile;
//using Windows.Foundation;

namespace QuodLib.WinForms.IO {
    public static class Files {
        public const string ExtensionFilter_AllFiles = "All files (*.*)|*.*";

        public static Image Image_FromFileSafely(string filepath) {
            using FileStream fs = new(filepath, FileMode.Open);
            Image img = Image.FromStream(fs);
            return img;
        }

        public static Icon? Exe_ToIcon(string filepath)
            => Icon.ExtractAssociatedIcon(filepath);
        public static bool Exe_ExtractIconTo(string filepathExe, string filepathIco) {
            Icon? icon = Exe_ToIcon(filepathExe);
            if (icon == null)
                return false;

            using FileStream file = new(filepathIco, FileMode.CreateNew);
            icon.Save(file);
            return true;
        }

        public static Bitmap? Exe_ToBitmap(string filepath)
            => Icon.ExtractAssociatedIcon(filepath)?.ToBitmap();

        public static bool Exe_ExtractBitmapTo(string filepathExe, string filepathBmp) {
            Bitmap? bitmap = Exe_ToBitmap(filepathExe);
            bitmap?.Save(filepathBmp);
            return bitmap != null;
        }

        #region openFile
        /// <summary>
        /// Builds a <see cref="FileDialog.Filter"/>.
        /// </summary>
        /// <param name="filters">
        /// <code>
        /// {
        ///     { "Description1", [ ".ext1, ".ext2" ] }, 
        ///     { "Description2", [ ".ext3", ".ext4" ] }
        /// }
        /// </code>
        /// </param>
        public static string BuildExtensionFilter(Dictionary<string, string[]> filters)
            => string.Join('|', filters.Select(pair => BuildExtensionFilter(pair.Key, pair.Value)));

        /// <summary>
        /// Builds a <see cref="FileDialog.Filter"/>.
        /// </summary>
        /// <param name="filters">
        /// <code>
        /// {
        ///     { "Description1", { ".ext1, ".ext2" } }, 
        ///     { "Description2", { ".ext3", ".ext4" } }
        /// }
        /// </code>
        /// </param>
        public static string BuildExtensionFilter(IEnumerable<KeyValuePair<string, IEnumerable<string>>> filters)
            => string.Join('|', filters.Select(pair => BuildExtensionFilter(pair.Key, pair.Value)));

        /// <summary>
        /// Builds a <see cref="FileDialog.Filter"/>.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="extensions"><code>{ ".ext1", ".ext2" }</code></param>
        /// <returns></returns>
        public static string BuildExtensionFilter(string description, IEnumerable<string> extensions) {
            string list = JoinExtensions(extensions, true);
            return $"{description} ({list})|{list}";
        }

        /// <summary>
        /// Builds a <see cref="FileDialog.Filter"/>.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="extension"><code>".ext"</code></param>
        /// <returns></returns>
        public static string BuildExtensionFilter(string description, string extension) {
            return $"{description} (*{extension})|*{extension}";
        }

        private static string JoinExtensions(IEnumerable<string> extensions, bool hasDot) {
            if (hasDot)
                return $"*{string.Join(";*", extensions)}";

            return $"*.{string.Join(";*.", extensions)}";
        }

        /// <summary>
        /// Runs the <paramref name="dialog"/> and outputs the result.
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        /// <remarks>
        ///     <list type="bullet">
        ///         <item>See also <see cref="BuildExtensionFilter(string, IEnumerable{string})"/></item>
        ///         <item>See also <see cref="BuildExtensionFilter(Dictionary{string, string[]})"/></item>
        ///         <item>See also <see cref="BuildExtensionFilter(IEnumerable{KeyValuePair{string, IEnumerable{string}}})"/></item>
        ///         <item>See also <see cref="ExtensionFilter_AllFiles"/></item>
        ///     </list>
        /// </remarks>
        public static bool TryBrowse(this FileDialog dialog, out string? filepath) {
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK) {
                filepath = dialog.FileName;
                return true;
            }

            filepath = null;
            return false;
        }

        /// <summary>
        /// Runs the <paramref name="dialog"/> and outputs the result.
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        /// <remarks>
        ///     <list type="bullet">
        ///         <item>See also <see cref="BuildExtensionFilter(string, IEnumerable{string})"/></item>
        ///         <item>See also <see cref="BuildExtensionFilter(Dictionary{string, string[]})"/></item>
        ///         <item>See also <see cref="BuildExtensionFilter(IEnumerable{KeyValuePair{string, IEnumerable{string}}})"/></item>
        ///         <item>See also <see cref="ExtensionFilter_AllFiles"/></item>
        ///     </list>
        /// </remarks>
        public static bool TryBrowse(this FolderBrowserDialog dialog, out string? filepath) {
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK) {
                filepath = dialog.SelectedPath;
                return true;
            }

            filepath = null;
            return false;
        }

        #endregion //openFile
    }
}
