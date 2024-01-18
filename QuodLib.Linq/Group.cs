namespace QuodLib.Linq {
    public static class Group {
        /*
        public static IEnumerable<TItem> DistinctBy<TItem, TDistinct, TSorted>(this IEnumerable<TItem> source, Func<TItem, TDistinct> distinctBy, Func<TItem, TSorted> orderBy) 
            where TDistinct : IEqualityComparer
            where TSorted : IComparable
            => source.OrderBy(orderBy).DistinctBy(distinctBy);
        */

        /// <summary>
        /// Partitions <paramref name="source"/> into blocks of <paramref name="size"/>; if it's uneven, the final block will be the smallest.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        /// <remarks>
        /// <i>This method must enumerate the <paramref name="source"/>!</i><br />
        /// If you happen to already have the expected <see cref="Enumerable.Count{TSource}(IEnumerable{TSource})"/>, use <see cref="Partition{T}(IEnumerable{T}, int, int)"/> instead.
        /// </remarks>
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int size)
            => (source is IList<T> list
                    ? list
                    : source.ToArray()
                ).Partition<T>(size);

        /// <summary>
        /// Partitions <paramref name="source"/> into blocks of <paramref name="size"/>; if it's uneven, the final block will be the smallest.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int size, int sourceLength) {
            int groups = (int)Math.Ceiling(sourceLength / (double)size);

            int start = 0,
                end = size - 1;

            for (int i = 0; i < groups; i++, start += size, end += size)
                yield return source.GetRange(start..end);
        }

        /// <summary>
        /// Partitions <paramref name="source"/> into blocks of <paramref name="size"/>; if it's uneven, the final block will be the smallest.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IList<T> source, int size) {
            int groups = (int)Math.Ceiling(source.Count / (double)size);

            int start = 0,
                end = size - 1;

            for (int i = 0; i < groups; i++, start += size, end += size)
                yield return source.GetRange(start..end);
        }
    }
}