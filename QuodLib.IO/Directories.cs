using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.IO {
    public static class Directories {
        /// <summary>
        /// Gets all immediate and nested subdirectories within <paramref name="rootDir"/>.
        /// </summary>
        /// <param name="rootDir"></param>
        /// <returns></returns>
        public static List<string> GetAllSubdirectories(string rootDir)
            => GetAllSubdirectories(rootDir, true);

        /// <summary>
        /// Gets all immediate and nested subdirectories within <paramref name="rootDir"/>.
        /// </summary>
        /// <param name="rootDir"></param>
        /// <param name="outputFullPath"></param>
        /// <returns></returns>
        public static List<string> GetAllSubdirectories(string rootDir, bool outputFullPath) {
            List<string> rtn = new();
            DirScanRecursive(rootDir, rtn);
            if (!outputFullPath)
                for (byte i = 0; i < rtn.Count; i++)
                    rtn[i] = rtn[i].Replace(rootDir + "\\", string.Empty);

            return rtn;
        }

        /// <summary>
        /// Recursively scan a directory for subdirectories.
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private static void DirScanRecursive(string dir, List<string> output) {
            foreach (string subdir in Directory.GetDirectories(dir)) {
                output.Add(subdir);
                DirScanRecursive(subdir, output);
            }
        }

        public static class Special {
            public static string User 
                => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            public static string Documents 
                => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            public static string Music 
                => Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            public static string Pictures 
                => Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            public static string Videos 
                => Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

            /// <summary>
            /// Whether the ApplicationData path ends with "\AppData\Roaming".
            /// </summary>
            /// <remarks>
            /// Windows XP uses "Documents and Settings", Windows 7+ uses "AppData".
            /// </remarks>
            private static bool AppIsData
                => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Contains("AppData");
            public static string AppData
                => AppIsData
                    ? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("\\Roaming", "")
                    : Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            public static string AppRoaming
                => AppIsData
                    ? $@"{AppData}\Roaming"
                    : AppData;

            public static string AppLocal 
                => AppRoaming.Replace(@"\Roaming", @"\Local");
            public static string AppLocalLow
                => AppRoaming.Replace(@"\Roaming", @"\LocalLow");
            public static string ProgramFiles 
                => Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            public static string ProgramFiles86 
                => Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            public static string Windows 
                => Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            public static string Sysystem 
                => Environment.GetFolderPath(Environment.SpecialFolder.System);
            public static string SystemX86 
                => Environment.GetFolderPath(Environment.SpecialFolder.SystemX86);
        }

    }
}
