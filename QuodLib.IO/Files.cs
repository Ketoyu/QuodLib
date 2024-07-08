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

		public static void File_CopyIfNewer(string source, string destination)
		{
			DateTime src = File.GetLastWriteTime(source),
				dest = File.GetLastWriteTime(destination);

			if (src > dest)
				File.Copy(source, destination, true);
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
		public static class Serialization
		{
			/// <summary>
			/// Convert the object to a serialized data string.
			/// </summary>
			/// <param name="obj"></param>
			/// <returns></returns>
			public static string Do(object obj)
			{
				MemoryStream stream = new MemoryStream();
				BinaryFormatter format = new BinaryFormatter();

				format.Serialize(stream, obj);
				string serial = Convert.ToBase64String(stream.ToArray());

				stream.Close();
				return serial;
			}
			public static object Undo(string serial)
			{
				MemoryStream stream = new MemoryStream(Convert.FromBase64String(serial));
				BinaryFormatter format = new BinaryFormatter();

				object rtn = format.Deserialize(stream);

				stream.Close();
				return rtn;
			}
			public static object Undo(string serial, object example)
			{
				MemoryStream stream = new MemoryStream(Convert.FromBase64String(serial));
				BinaryFormatter format = new BinaryFormatter();

				object rtn = format.Deserialize(stream);
				if (rtn.GetType() != example.GetType()) throw new Exception("Deserialized object of type \"" + rtn.GetType() + "\" not of type \"" + example.GetType() + "\".");
				
				stream.Close();
				return rtn;
			}
		}

		//public static string Dir_GetName(string dir) {
		//	return dir.Split("\\")(dir.Split("\\"));
		//}

		#region openFile
		public static string TextFile_GetAllText(string filename)
		{
			StreamReader rd = new StreamReader(filename);
			string rtn = rd.ReadToEnd();
			rd.Close();
			return rtn;
		}
		public static void TextFile_Overwrite(string filename, string data)
		{
			StreamWriter wrt = new StreamWriter(filename);
			wrt.Write(data);
			wrt.Flush();
			wrt.Close();
		}
		/// <summary>
		/// Appends the text in a file.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="data">The text to send; it is recommended to prefix it with a new-line character</param>
		public static void TextFile_Append(string filename, string data)
		{
			string pdata = (File.Exists(filename) ? TextFile_GetAllText(filename) : "");
			StreamWriter wrt = new StreamWriter(filename);
			wrt.Write(pdata + data);
			wrt.Flush();
			wrt.Close();
		}
		public static Dictionary<string, string> IniFile_GetSettings(string filename)
		{
			string[] entries = TextFile_GetAllText(filename).Replace("\r\n", "\n").Split('\n');
			Dictionary<string, string> rtn = new Dictionary<string, string>();
			foreach (string entry in entries)
				if (entry != "") {
					string[] splEnt = entry.Split('=');
					rtn.Add(splEnt[0], splEnt[1]);
				}

			return rtn;
		}
		public static void IniFile_SendSettings(Dictionary<string, string> settings, string filename)
		{
			StreamWriter wrt = new StreamWriter(filename);
			foreach (string key in settings.Keys)
				wrt.WriteLine(key + "=" + settings[key]);

			wrt.Close();
		}
		public static void IniFile_UpdateSetting(string key, string value, string filename, bool WriteIfNotFound)
		{
			Dictionary<string, string> settings = IniFile_GetSettings(filename);
			if (settings.ContainsKey(key))
				settings[key] = value;
			else {
				if (WriteIfNotFound) settings.Add(key, value);
					else throw new Exception("Error: Key \"" + key + "\" not found in \"" + filename.Split("\n", -1) + "\".");
			}
			IniFile_SendSettings(settings, filename);
		}
		public static void IniFile_RemoveSetting(string key, string filename, bool ignoreAlreadyMissing)
		{
			Dictionary<string, string> settings = IniFile_GetSettings(filename);
			if (settings.ContainsKey(key))
				settings.Remove(key);
			else if (!ignoreAlreadyMissing) throw new Exception("Error: Key \"" + key + "\" not found in \"" + filename.Split("\n", -1) + "\".");
			
			IniFile_SendSettings(settings, filename);
		}
		#endregion //openFile
	}
}
