using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuodLib.WinForms {
    public static class classObjects {
        public static string GetColumn(this ListViewItem item, int columnIndex)
            => item.SubItems[columnIndex].Text;

        public static IEnumerable<string> AsEnumerable(this ListViewItem item) {
            for (int i = 0; i < item.SubItems.Count; i++)
                yield return item.SubItems[i].Text;
        }

        public static string GetColumn(this ListView listView, int columnIndex)
            => listView.SelectedItems[0].GetColumn(columnIndex);

        /// <summary>
        /// Copies the string to the system's Clipboard.
        /// </summary>
        /// <param name="Input"></param>
        public static void CopyToClipboard(this string Input) {
            System.Windows.Forms.Clipboard.SetText(Input);
        }
    }
}
