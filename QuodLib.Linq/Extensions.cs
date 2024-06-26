﻿using QuodLib.Linq.Comparers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Linq
{
    public static class Extensions
    {

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> of <typeparamref name="T"/> for the provided <paramref name="range"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetRange<T>(this IEnumerable<T> source, Range range)
        {
            int i = range.Start.Value;
            source = source.Skip(i);

            foreach (T value in source)
            {
                if (i > range.End.Value)
                    break;

                yield return value;
                i++;
            }
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> of <typeparamref name="T"/> for the provided <paramref name="range"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetRange<T>(this IList<T> source, Range range)
        {
            int max = range.End.Value < source.Count
                ? range.End.Value
                : source.Count - 1
            ;

            for (int i = range.Start.Value; i <= max; i++)
                yield return source[i];
        }

        public static IEnumerable<T> At<T>(this T[,] source, int index) {
            int end = source.GetLength(index);
            for (int i = 0; i < end; i++)
                yield return source[index, i];
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source) where TKey : notnull
            => source.ToDictionary(p => p.Key, p => p.Value);

        public static int IndexOfMax<T>(this IList<T> source) where T : IComparable<T> {
            if (source.Count == 0)
                return -1;

            if (source.Count == 1)
                return 0;

            int m = 0;
            for (int i = 1; i < source.Count; i++)
                if (source[i].CompareTo(source[m]) == 1)
                    m = i;

            return m;
        }

        /// <summary>
        /// Filters out null values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IEnumerable<T> Real<T>(this IEnumerable<T?> input)
            => input.Cast<T>();

        /// <summary>
        /// Filters out null values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ParallelQuery<T> Real<T>(this ParallelQuery<T?> input)
            => input.Cast<T>();

        /// <summary>
        /// Adds an item to the list, unless that item is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="value"></param>
        public static void AddIfReal<T>(this List<T> input, T value) {
            if (value != null)
                input.Add(value);
        }

        /// <summary>
        /// A fluid implementation for setting <see cref="List{T}.Capacity"/>, if you're adding a mass but determinate amount of items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static List<T> WithCapacity<T>(this List<T> input, int size) {
            input.Capacity = size;
            return input;
        }

        /// <summary>
        /// A fluid implementation for setting <see cref="List{T}.Capacity"/>, if you're adding a mass but determinate amount of items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="input"></param>
        /// <param name="sameAs"></param>
        /// <returns></returns>
        public static List<T> SameCapacity<T, U>(this List<T> input, IList<U> sameAs)
            => input.WithCapacity(sameAs.Count);

        /// <summary>
        /// A predicate-based implementation of <see cref="List{T}.Remove(T)"/>, by using <see cref="System.Linq.Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="predicate"></param>
        public static void Remove<T>(this List<T> input, Func<T, bool> predicate)
            => input?.Remove(input.FirstOrDefault(i => predicate(i)));

        /// <summary>
        /// Returns <paramref name="source"/>[<paramref name="key"/>]. If it does not yet exist in <paramref name="source"/>, uses
        /// <paramref name="fetch"/> to set it, then returns the result.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="fetch"></param>
        /// <returns></returns>
        public static TValue Fetch<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, Func<TKey, TValue> fetch) {
            if (!source.ContainsKey(key))
                source[key] = fetch(key);

            return source[key];
        }

        public static IEnumerable<FieldInfo> Except<T>(this IEnumerable<FieldInfo> fields)
            => fields.Except(typeof(T).GetFields(), new EquatableSelectComparer<FieldInfo, string>(fi => fi.Name));

        public static IEnumerable<FieldInfo> ExceptAttribute<TAttribute>(this IEnumerable<FieldInfo> fields) where TAttribute : Attribute
            => fields.Where(f => f.GetCustomAttribute<TAttribute>() == null);

        public static IEnumerable<TItem> ToEnumerable<TItem>(this IList<TItem> source)
            => source.Select(item => item);

        public static IEnumerable<int> ToEnumerable(this Range source)
            => Enumerable.Range(source.Start.Value, source.End.Value);
    }
}
