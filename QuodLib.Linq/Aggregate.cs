using System.Collections;
using System.Numerics;
using QuodLib.Linq.Comparers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QuodLib.Linq
{
    public static class Aggregate {
        public static IEnumerable<TItem> DistinctBy<TItem, TDistinct>(this IEnumerable<TItem> source, Func<TItem, TDistinct> distinctBy) where TDistinct : IEqualityComparer<TDistinct> {
            EqualitySelectComparer<TItem, TDistinct> equator = new(distinctBy);

            return source.Distinct(equator);
        }

        public static IEnumerable<KeyValuePair<T, int>> GroupCount<T>(this IEnumerable<T> source)
            => source.GroupBy(t => t).Select(t => new KeyValuePair<T, int>(t.Key, t.Count()));

        public static IEnumerable<KeyValuePair<TItem, TNumber>> GroupCount<TItem, TNumber>(this IEnumerable<TItem> source, Func<TItem, TNumber> sumBy) where TNumber : INumber<TNumber>
            => source.GroupBy(t => t).Select(t => new KeyValuePair<TItem, TNumber>(t.Key, t.Select(sumBy).Sum()));

        public static TNumber Sum<TNumber>(this IEnumerable<TNumber> source) where TNumber : INumber<TNumber>
            => source.Aggregate((sum, n) => sum += n);
    }
}