using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Linq {
    public static class Generate {
        /// <summary>
        /// Creates a rectangle from two Points.
        /// </summary>
        /// <param name="upperLeft"></param>
        /// <param name="lowerRight"></param>
        /// <returns></returns>
        public static Rectangle MakeRectangle(Point upperLeft, Point lowerRight)
            => new Rectangle(upperLeft.X, upperLeft.Y, lowerRight.X - upperLeft.X + 1, lowerRight.Y - upperLeft.Y + 1);

        /// <summary>
        /// Returns the upperl-left and lower-right vertices.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="upperLeft"></param>
        /// <param name="lowerRight"></param>
        public static void ExtractPoints(this Rectangle rect, out Point upperLeft, out Point lowerRight) {
            upperLeft = new Point(rect.X, rect.Y);
            lowerRight = new Point(rect.X + rect.Width - 1, rect.Y + rect.Height - 1);
        }

        public static T[] CombineArrays<T>(T[] a, T[] b) {
            T[] rtn = new T[a.Length + b.Length];
            for (uint i = 0; i < a.Length; i++)
                rtn[i] = a[i];

            if (a.Count() > 0)
                for (uint i = 0; i < b.Length; i++)
                    rtn[a.Length - 1 + i] = b[i];
            else
                rtn = b;

            return rtn;
        }

        public static Queue<T> Clone<T>(this Queue<T> original) {
            Queue<T> store = new();
            Queue<T> rtn = new();

            //Move
            while (original.Count > 0)
                store.Enqueue(original.Dequeue());

            //Populate & restore
            while (store.Count > 0) {
                T temp = store.Dequeue();
                rtn.Enqueue(temp);
                original.Enqueue(temp);
            }

            return rtn;
        }

        public static Stack<T> Clone<T>(this Stack<T> original) {
            Stack<T> store = new();
            Stack<T> rtn = new();

            //Move
            while (original.Count > 0)
                store.Push(original.Pop());

            //Populate & restore
            while (store.Count > 0) {
                T temp = store.Pop();
                rtn.Push(temp);
                original.Push(temp);
            }

            return rtn;
        }


        /// <summary>
        /// Converts an IList&lt;<typeparamref name="T"/>&gt; to a List&lt;<typeparamref name="L"/>&gt;.
        /// </summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <typeparam name="L">Destination type</typeparam>
        /// <param name="source"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<L> ConvertAll<T, L>(this IList<T> source, Converter<T, L> converter)
            => MakeList<L>((outp) => {
                foreach (T inp in source)
                    outp.Add(converter(inp));
            });

        /// <summary>
        /// Converts an array <typeparamref name="T"/>[] to an array <typeparamref name="L"/>[].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="L"></typeparam>
        /// <param name="source"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static L[] ConvertAll<T, L>(this T[] source, Converter<T, L> converter)
            => Array.ConvertAll(source, converter);

        /// <summary>
        /// Creates a List&lt;<typeparamref name="T"/>&gt; without having to declare and return it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="AppendAll"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> MakeList<T>(Action<List<T>> AppendAll) {
            List<T> rtn = new List<T>();
            AppendAll(rtn);
            return rtn;
        }

        /// <summary>
        /// Creates a List&lt;<typeparamref name="T"/>&gt; without having to declare and return it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="size"></param>
        /// <param name="Assign"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] MakeArray<T>(int size, Action<int, T[]> Assign) {
            T[] rtn = new T[size];
            for (int i = 0; i < size; i++)
                Assign(i, rtn);
            return rtn;
        }

        /// <summary>
        /// Creates a List&lt;<typeparamref name="T"/>&gt; without having to declare and return it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="size"></param>
        /// <param name="Get"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] MakeArray<T>(int size, Func<int, T> Get) {
            T[] rtn = new T[size];
            for (int i = 0; i < size; i++)
                rtn[i] = Get(i);
            return rtn;
        }
    }
}
