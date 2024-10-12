using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Linq {
    public static class Ask {
        /// <summary>
        /// The maximum index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="col"></param>
        /// <returns></returns>
        public static int LastIndex<T>(this IList<T> col)
            => col.Count - 1;

        /*
		/// <summary>
		/// Returns the value at the maximum index.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="col"></param>
		/// <returns></returns>
		public static T Last<T>(this IList<T> col)
            => col[col.LastIndex()];*/

        /// <summary>
        /// Whether the collection has zero items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="que"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this ICollection<T> que)
            => que.Count == 0;

        /// <summary>
        /// Whether the collection has zero items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stk"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this Stack<T> stk)
            => stk.Count == 0;

        /// <summary>
        /// Whether the collection has zero items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="que"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this Queue<T> que)
            => que.Count == 0;

        /// <summary>
        /// Checks whether <paramref name="source"/> contains MORE THAN <paramref name="count"/> items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool CountExceeds<T>(this IEnumerable<T> source, int count) {
            if (source is IList<T> list)
                return list.Count > count;

            int count_ = 0;
            foreach (T item in source) {
                count_++;
                if (count_ > count)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the first <typeparamref name="TSource"/> whose <typeparamref name="TKey"/> is found
        /// within <param name="search"></param>, or null if it is not found.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="search"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public static TSource? FindByOrDefault<TSource, TKey>(this IList<TSource> source, Func<TSource, TKey> keySelector,
            IList<TKey> search)
            where TSource : notnull
            where TKey : notnull {

            if (source.Count < search.Count) //find by converting source to a dictionary (for constant lookup times)
                return ((IEnumerable<TSource>)source).FindByOrDefault(keySelector, search);

            //find by converting search to a HashSet (for constant lookup times)
            return source.FindByOrDefault(keySelector, search.ToHashSet());
        }

        /// <summary>
        /// Returns the first <typeparamref name="TSource"/> whose <typeparamref name="TKey"/> is found
        /// within <param name="search"></param>, or null if it is not found.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="search"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public static TSource? FindByOrDefault<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,
            IEnumerable<TKey> search)
            where TSource : notnull
            where TKey : notnull {

            //find by converting source to a dictionary (for constant lookup times)
            Dictionary<TKey, TSource> dic = source.ToDictionary(keySelector, i => i);
            foreach (TKey item in search) {
                if (dic.TryGetValue(item, out TSource? found))
                    return found;
            }

            return default;
        }

        /// <summary>
        /// Returns the first <typeparamref name="TSource"/> whose <typeparamref name="TKey"/> is found
        /// within <param name="search"></param>, or null if it is not found.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="search"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public static TSource? FindByOrDefault<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            HashSet<TKey> search)
            where TSource : notnull
            where TKey : notnull
            //check using the hash key (for constant lookup times)
            => source.FirstOrDefault(item => search.Contains(keySelector(item)));

        /// <summary>
        /// Determines whether an <paramref name="item"/> is in the <paramref name="source"/>, using the provided <paramref name="comparisonType"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="item"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public static bool Contains(this IEnumerable<string> source, string item, StringComparison comparisonType)
            => source.Any(s => s.Equals(item, comparisonType));

        #region ArrayContains
        public static bool Contains<T>(this T[] array, T item) {
            for (int i = 0; i < array.Length; i++)
                if (array[i].Equals(item)) return true;

            return false;
        }
        public static bool Contains<T>(this T[][] array, T item) {
            for (int i = 0; i < array.Length; i++)
                if (Contains<T>(array[i], item)) return true; //Calls T[]

            return false;
        }
        public static bool Contains<T>(this T[][][] array, T item) {
            for (int i = 0; i < array.Length; i++)
                if (Contains<T>(array[i], item)) return true; //calls T[][]

            return false;
        }
        public static bool Contains<T>(this T[][][][] array, T item) {
            for (int i = 0; i < array.Length; i++)
                if (Contains<T>(array[i], item)) return true; //calls T[][][]

            return false;
        }
        public static bool Contains<T>(this T[,] array, T item) {
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    if (array[i, j].Equals(item)) return true;

            return false;
        }
        public static bool Contains<T>(this T[,,] array, T item) {
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    for (int k = 0; k < array.GetLength(2); k++)
                        if (array[i, j, k].Equals(item)) return true;

            return false;
        }
        public static bool Contains<T>(this T[,,,] array, T item) {
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    for (int k = 0; k < array.GetLength(2); k++)
                        for (int l = 0; l < array.GetLength(3); l++)
                            if (array[i, j, k, l].Equals(item)) return true;

            return false;
        }
        #endregion //ArrayContains
    }
}
