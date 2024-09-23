//using System.Text;
//using System.Threading.Tasks;

//using WinMusicProperties = Windows.Storage.FileProperties.MusicProperties;
//using StorageFile = Windows.Storage.StorageFile;
//using Windows.Foundation;

namespace QuodLib.IO {
    public static class Files {
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

        public static void CopyIfNewer(string source, string destination) {
            DateTime src = File.GetLastWriteTime(source),
                dest = File.GetLastWriteTime(destination);

            if (src > dest)
                File.Copy(source, destination, true);
        }
        public static void MoveIfNewer(string source, string destination) {
            DateTime src = File.GetLastWriteTime(source),
                dest = File.GetLastWriteTime(destination);

            if (src > dest)
                File.Move(source, destination, true);
        }
        public static void ForceNewer(string source1, string source2) {
            DateTime src1 = File.GetLastWriteTime(source1),
                src2 = File.GetLastWriteTime(source2);

            if (src1 > src2)
                File.Copy(source1, source2, true);
            else if (src1 < src2)
                File.Copy(source2, source1, true);

            //else: the date is the same, don't waste the write-times.
        }

        #region openFile
        /// <summary>
        /// Appends the text in a file.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data">The text to send; it is recommended to prefix it with a new-line character</param>
        public static void TextFile_Append(string filename, string data) {
            string pdata = (File.Exists(filename) ? File.ReadAllText(filename) : "");
            using StreamWriter wrt = new(filename);
            wrt.Write(pdata + data);
        }
        #endregion //openFile
    }
}
