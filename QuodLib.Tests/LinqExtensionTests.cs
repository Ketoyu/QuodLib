using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace QuodLib.Tests {
    public static class LinqExtensionTests {
        public static readonly KeyValuePair<int, string>[]
            CONTENT_1S = new KeyValuePair<int, string>[] {
                new(0, "0"),
                new(1, "0"),
                new(2, "2"),
                new(3, "3")
            },
            CONTENT_1E = new KeyValuePair<int, string>[] {
                new(0, "0"),
                new(2, "2"),
                new(3, "3")
            };

        /*
        [Theory]
        [InlineData(
            CONTENT_1S,
            CONTENT_1E
        )]*/
        public static void DistinctBy_Success(KeyValuePair<int, string>[] source, KeyValuePair<int, string>[] expected) {
            KeyValuePair<int, string>[] actual = source.DistinctBy(item => item.Value).ToArray();

            Assert.Equal(expected, actual);
        }
    }
}
