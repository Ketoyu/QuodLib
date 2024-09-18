using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.WinForms.Linq.Extensions {
    public static class Nested {
        /// <summary>
        /// A fluid method for adding sub-items to a <see cref="ToolStripMenuItem"/>.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="items"></param>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>See also <see cref="ToolStripMenuItem"/></item>
        ///     <item>See also <see cref="ToolStripDropDownItem.DropDownItems"/></item>
        ///     <item>See also <see cref="ToolStripItemCollection.AddRange(ToolStripItem[])"/></item>
        /// </list>
        /// </remarks>
        /// <returns></returns>
        public static ToolStripMenuItem AddSubitems(this ToolStripMenuItem parent, params ToolStripMenuItem[] items) {
            parent.DropDownItems.AddRange(items);
            return parent;
        }
    }
}
