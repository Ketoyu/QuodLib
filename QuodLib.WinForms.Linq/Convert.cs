namespace QuodLib.WinForms.Linq
{
    public static class Convert
    {
        /// <summary>
        /// Performs an <see cref="Enumerable.Cast{TResult}(System.Collections.IEnumerable)"/> on the <paramref name="source"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<ListViewItem> ToEnumerable(this ListView.ListViewItemCollection source)
            => source.Cast<ListViewItem>();

        /// <summary>
        /// Casts the <paramref name="source"/>'s <see cref="ComboBox.SelectedItem"/> to the provided <typeparamref name="T"/>?
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T? SelectedItem<T>(this ComboBox source)
            => (T?)source.SelectedItem;
    }
}
