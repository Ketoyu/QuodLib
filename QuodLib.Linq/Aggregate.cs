using System.Collections;
using QuodLib.Linq.Comparers;

namespace QuodLib.Linq
{
    public static class Aggregate {
        public static IEnumerable<TItem> DistinctBy<TItem, TDistinct>(this IEnumerable<TItem> source, Func<TItem, TDistinct> distinctBy) where TDistinct : IEqualityComparer<TDistinct> {
            EqualitySelectComparer<TItem, TDistinct> equator = new(distinctBy);

            return source.Distinct(equator);
        }

        public static IEnumerable<KeyValuePair<T, int>> GroupCount<T>(this IEnumerable<T> source)
            => source.GroupBy(t => t).Select(t => new KeyValuePair<T, int>(t.Key, t.Count()));
    }
}