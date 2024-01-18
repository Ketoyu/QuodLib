namespace QuodLib.Linq {
    public static class As {

        public static IEnumerable<int> AsEnumerable(this Range range) {
            for (int i = range.Start.Value; i <= range.End.Value; i++)
                yield return i;
        }

        public static IEnumerable<IEnumerable<T>> AsEnumerable<T>(this T[,] source) {
            for (int i = 0; i < source.Length; i++)
                yield return source.At(i);
        }
    }
}