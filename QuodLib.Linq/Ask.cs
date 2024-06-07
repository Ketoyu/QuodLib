using System;
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
