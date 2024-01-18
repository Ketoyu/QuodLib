using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static QuodLib.Strings.Query;

namespace QuodLib.Tests.strings {
    public static class QueryTests {
        /// <summary>
        /// The source string, to query on.
        /// </summary>
        private const string CONTENT = "xxxx0000 1 4 0000 22 33 22";

        private static StringQuery AsQuery(this string content, bool asInclusive)
            => (asInclusive ? content.AsQuery().AsInclusive() : content.AsQuery());

        [Theory]
        [InlineData(CONTENT, false, " 1 4 0000 22 33 22", 
            "0000")]
        [InlineData(CONTENT, false, " 22 33 22", 
            "0000", "0000")]
        [InlineData(CONTENT, true, "0000 1 4 0000 22 33 22", 
            "0000")]
        [InlineData(CONTENT, true, "0000 1 4 0000 22 33 22",
            "0000", "0000")]
        public static void After_Success(string content, bool asInclusive, string expected, params string[] terms) {
            var query = content.AsQuery(asInclusive);

            string actual = query.After(terms).ToString();

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(CONTENT, false, "xxxx", "0000")]
        [InlineData(CONTENT, true, "xxxx0000", "0000")]
        public static void Before_Success(string content, bool asInclusive, string expected, string term) {
            string actual = content.AsQuery(asInclusive).Before(term).ToString();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(CONTENT, false, "xxx",
            "0000", "x")]
        [InlineData(CONTENT, true, "xxxx0000",
            "0000", "x")]
        public static void Before_After_Success(string content, bool asInclusive, string expected, string term_before, string term_after) {
            string actual = content.AsQuery(asInclusive).Before(term_before).After(term_after).ToString();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(CONTENT, false, " 1 4 ",
            "0000", "0000")]
        [InlineData(CONTENT, false, "0000",
            "xxxx", " ")]
        [InlineData(CONTENT, true, "0000",
            "0000", "0000")]
        [InlineData(CONTENT, true, "xxxx0000 ",
            "xxxx", " ")]
        public static void After_Before_Success(string content, bool asInclusive, string expected, string term_after, string term_before) {
            string actual = content.AsQuery(asInclusive).After(term_after).Before(term_before).ToString();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(CONTENT, false, " 22", "33")]
        [InlineData(CONTENT, true, "33 22", "33")]
        public static void AfterLast_Success(string content, bool asInclusive, string expected, string term) {
            string actual = content.AsQuery(asInclusive).AfterLast(term).ToString();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(CONTENT, false, "xxxx0000 1 4 0000 22 33 ", "22")]
        [InlineData(CONTENT, true, "xxxx0000 1 4 0000 22 33 22", "22")]
        public static void BeforeLast_Success(string content, bool asInclusive, string expected, string term) {
            string actual = content.AsQuery(asInclusive).BeforeLast(term).ToString();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(CONTENT, false, " ", 
            "33", "22")]
        [InlineData(CONTENT, true, "22 33", 
            "33", "22")]
        public static void Before_AfterLast_Success(string content, bool asInclusive, string expected, string term_before, string term_afterLast) {
            string actual = content.AsQuery(asInclusive).Before(term_before).AfterLast(term_afterLast).ToString();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(CONTENT, 1,
            "xx0000 1 4 0000 22 33 2")]
        public static void RelativeFrom_Success(string content, int relativeIndex, string expected) {
            string actual = content.AsQuery()
                .From(1).Until(content.Length - 2)
                .RelativeFrom(relativeIndex).ToString();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(CONTENT, 3,
            "xxx0")]
        public static void RelativeUntil_Success(string content, int relativeIndex, string expected) {
            string actual = content.AsQuery()
                .From(1).Until(content.Length - 2)
                .RelativeUntil(relativeIndex).ToString();

            Assert.Equal(expected, actual);
        }

    } //</class>
}
