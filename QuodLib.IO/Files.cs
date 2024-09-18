using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
//using WinMusicProperties = Windows.Storage.FileProperties.MusicProperties;
//using StorageFile = Windows.Storage.StorageFile;
//using Windows.Foundation;

namespace QuodLib.IO
{
	public static class Files
	{
        /*public class MusicProperties
		{
			Windows.Storage.SystemMusicProperties properties;
			public string Artist, Album, AlbumArtist, TrackNumber, Album_TrackCount, Year;
			public MusicProperties(string filename)
			{
				try
				{
					
					StorageFile file = await StorageFile.GetFileFromPathAsync(filename);
					if (file != null)
					{
						StringBuilder outputText = new StringBuilder();

						// Get music properties
						WinMusicProperties musicProperties = await file.Properties.GetMusicPropertiesAsync();
						outputText.AppendLine("Album: " + musicProperties.Album);
						outputText.AppendLine("Rating: " + musicProperties.Rating);
					}
				}
				// Handle errors with catch blocks
				catch (FileNotFoundException)
				{
				 // For example, handle a file not found error
				}
			}
		}*/

        /// <summary>
        /// Resolves the <paramref name="sourcePath"/> filepath minus the root drive or <paramref name="relativeTo"/>, into the <paramref name="targetDirectory"/>.
        /// </summary>
        /// <param name="targetDirectory">The target directory</param>
        /// <param name="sourcePath">The source filepath</param>
        /// <param name="relativeTo">The path to trim from the <paramref name="sourcePath"/></param>
        /// <returns>The updated destination</returns>
        public static string ResolvePath(string targetDirectory, string sourcePath, string? relativeTo) {
            string partialSource = Path.GetRelativePath(
                    !string.IsNullOrEmpty(relativeTo) && sourcePath.StartsWith(relativeTo)
                        ? relativeTo
                        : Path.GetPathRoot(sourcePath)!,
                    sourcePath);

            return Path.Combine(targetDirectory, partialSource);
        }


        public static void File_CopyIfNewer(string source, string destination)
		{
			DateTime src = File.GetLastWriteTime(source),
				dest = File.GetLastWriteTime(destination);

			if (src > dest)
				File.Copy(source, destination, true);
		}
        public static void File_MoveIfNewer(string source, string destination) {
            DateTime src = File.GetLastWriteTime(source),
                dest = File.GetLastWriteTime(destination);

            if (src > dest)
                File.Move(source, destination, true);
        }
        public static void File_ForceNewer(string source1, string source2)
		{
			DateTime src1 = File.GetLastWriteTime(source1),
				src2 = File.GetLastWriteTime(source2);
			
			if (src1 > src2)
				File.Copy(source1, source2, true);
			else if (src1 < src2)
				File.Copy(source2, source1, true);

			//else: the date is the same, don't waste the write-times.
		}

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
		public static List<string> GetAllSubdirectories(string rootDir, bool outputFullPath)
		{
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
		private static void DirScanRecursive(string dir, List<string> output)
		{
			foreach (string subdir in Directory.GetDirectories(dir)) {
				output.Add(subdir);
				DirScanRecursive(subdir, output);
			}
		}

		public static FileStream FileStream_FromFile(string fl)
			=> new FileStream(fl, FileMode.Open);

		public static class GetDir
		{
			public static string User { get { return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); } }
			public static string Docs { get { return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); } }
			public static string Music { get { return Environment.GetFolderPath(Environment.SpecialFolder.MyMusic); } }
			public static string Pics { get { return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures); } }
			public static string Vids { get { return Environment.GetFolderPath(Environment.SpecialFolder.MyVideos); } }

			//Win-XP uses "Documents and Settings", Windows 7+ uses "AppData".
			private static bool AppIsData { get { return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Contains("AppData"); } }
			public static string AppData {
				get {
					if (AppIsData)
						return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("\\Roaming", "");

					return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				}
			}
			public static string AppRom {
				get {
					if (AppIsData)
						return AppData + "\\Roaming";

					return AppData;
				}
			}
			public static string AppLoc { get { return AppRom.Replace("\\Roaming", "\\Local"); } }
			public static string AppLocLow { get { return AppRom.Replace("\\Roaming", "\\LocalLow"); } }
			public static string PFiles { get { return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles); } }
			public static string PFiles86 { get { return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86); } }
			public static string Win { get { return Environment.GetFolderPath(Environment.SpecialFolder.Windows); } }
			public static string Sys { get { return Environment.GetFolderPath(Environment.SpecialFolder.System); } }
			public static string Sys86 { get { return Environment.GetFolderPath(Environment.SpecialFolder.SystemX86); } }
		}
		

		#region openFile
		/// <summary>
		/// Appends the text in a file.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="data">The text to send; it is recommended to prefix it with a new-line character</param>
		public static void TextFile_Append(string filename, string data)
		{
			string pdata = (File.Exists(filename) ? File.ReadAllText(filename) : "");
			using StreamWriter wrt = new(filename);
			wrt.Write(pdata + data);
		}
		#endregion //openFile
	}
}
