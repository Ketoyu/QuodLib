namespace QuodLib.WinForms.Linq.Extensions.Collections
{
    public static class Convert
    {
        /// <summary>
        /// Performs an <see cref="Enumerable.Cast{TResult}(System.Collections.IEnumerable)"/> on the <paramref name="source"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<ListViewItem> AsEnumerable(this ListView.ListViewItemCollection source)
            => source.Cast<ListViewItem>();
    }
}
