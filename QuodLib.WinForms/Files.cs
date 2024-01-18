using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;
using DialogResult = System.Windows.Forms.DialogResult;
//using WinMusicProperties = Windows.Storage.FileProperties.MusicProperties;
//using StorageFile = Windows.Storage.StorageFile;
//using Windows.Foundation;

namespace QuodLib
{
	public static class Files
	{
		public static Image Image_FromFileSafely(string fl)
		{
			FileStream fs = new FileStream(fl, FileMode.Open);
			Image img = Image.FromStream(fs);
			fs.Close();
			fs.Dispose();
			return img;
		}

		public static Icon Exe_ToIcon(string fl)
			=> Icon.ExtractAssociatedIcon(fl);
		public static void Exe_ExtractIconTo(string flExe, string flIco)
		{
			Exe_ToIcon(flExe).Save(new System.IO.FileStream(flIco, System.IO.FileMode.CreateNew));
		}
		public static Bitmap Exe_ToBitmap(string fl)
			=> Icon.ExtractAssociatedIcon(fl).ToBitmap();

		public static void Exe_ExtractBitmapTo(string flExe, string flBMP)
		{
			Exe_ToBitmap(flExe).Save(flBMP);
		}

		#region openFile
		public static string OpenFile()
			=> OpenFile("ALL files", "*.*", "Open file");

		public static string OpenFile(string typeName, string[] extensions, string title)
			=> OpenFile(typeName, extListToExt(extensions), title);

		public static string OpenFile(string typeName, string[] extensions, string title, string initDir)
			=> OpenFile(typeName, extListToExt(extensions), title, initDir);

		public static string OpenFile(string typeName, string extension, string title)
			=> OpenFile(typeName, extension, title, "");

		public static string OpenFile(string typeName, string extension, string title, string initDir)
		{
			System.Windows.Forms.OpenFileDialog opn = new System.Windows.Forms.OpenFileDialog();
			opn.Title = title;

			if (extension == "") extension = "*.*";
			if (!extension.Contains("*")) extension = "*." + extension;
			opn.Filter = typeName + " (" + extension + ") | " + extension;
			
			if (initDir != "") opn.InitialDirectory = initDir;
			opn.ShowDialog();
			return opn.FileName;
		}
		public static string OpenFolder(string initDir = "")
		{
			FolderBrowserDialog fld = new FolderBrowserDialog();
			if (initDir != "") fld.SelectedPath = initDir;
			DialogResult res = fld.ShowDialog();
			if (res == DialogResult.Yes || res == DialogResult.OK) return fld.SelectedPath;

			fld.Dispose();
			return null;
		}
		public static string SaveFile(string typeName, string extension, string title, string initDir = "")
		{
			System.Windows.Forms.SaveFileDialog sv = new System.Windows.Forms.SaveFileDialog();
			sv.Title = title;

			if (extension == "") extension = "*.*";
			if (!extension.Contains("*")) extension = "*." + extension;
			sv.Filter = typeName + " (" + extension + ") | " + extension;
			
			if (initDir != "") sv.InitialDirectory = initDir;
			sv.ShowDialog();
			return sv.FileName;
		}

		private static string extListToExt(string[] extensions)
		{
			string rtn = "*.";
			for (byte i = 0; i < extensions.Length; i++) {
				if (i > 0) rtn += ";*.";
			   rtn += extensions[i];
			}
			return rtn;
		}
		#endregion //openFile
	}
}
