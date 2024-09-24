namespace QuodLib.Linq {
    public static class Convert {

        public static IEnumerable<int> ToEnumerable(this Range range) {
            for (int i = range.Start.Value; i <= range.End.Value; i++)
                yield return i;
        }

        public static IEnumerable<IEnumerable<T>> ToEnumerable<T>(this T[,] source) {
            for (int i = 0; i < source.Length; i++)
                yield return source.At(i);
        }
    }
}