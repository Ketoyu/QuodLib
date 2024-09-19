namespace QuodLib.WinForms.Linq.Extensions.Collections {
    public static class Insert {
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

        /// <summary>
        /// Extension for adding several <see cref="ListViewGroup"/>s to a <see cref="ListView"/>.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="groups"></param>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>See also <see cref="ListView"/></item>
        ///     <item>See also <see cref="ListViewGroup"/></item>
        ///     <item>See also <see cref="ListView.Groups"/></item>
        ///     <item>See also <see cref="ListViewGroupCollection.Add(ListViewGroup)"/></item>
        ///     <item>See also <see cref="ListViewGroupCollection.AddRange(ListViewGroup[])"/></item>
        /// </list>
        /// </remarks>
        /// <returns></returns>
        public static void AddGroups(this ListView parent, IEnumerable<ListViewGroup> groups) {
            foreach (var group in groups)
                parent.Groups.Add(group);
        }

        /// <summary>
        /// A fluid method for adding items to a <see cref="ListViewGroup"/>.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="items"></param>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>See also <see cref="ListViewItem"/></item>
        ///     <item>See also <see cref="ListViewGroup"/></item>
        ///     <item>See also <see cref="ListView.ListViewItemCollection.Add(ListViewItem)"/></item>
        ///     <item>See also <see cref="ListView.ListViewItemCollection.AddRange(ListViewItem[])"/></item>
        /// </list>
        /// </remarks>
        /// <returns></returns>
        public static ListViewGroup AddItems(this ListViewGroup group, IEnumerable<ListViewItem> items) {
            foreach (var item in items)
                group.Items.Add(item);
            return group;
        }
    }
}
