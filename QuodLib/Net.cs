
using System.Drawing;

namespace QuodLib {
    using Strings;
    /// <summary>
    /// Contains functions for page requests and 'dissection, mostly.
    /// </summary>
    public static class Net {
        /// <summary>
        /// Accepts a String 'url' web-address;
        /// downloads and returns a String of the source-code text of the given web-address.
        /// </summary>
        /// <param name="url">web-address</param>
        /// <param name="timeout">Timeout</param>
        /// <returns>String of the source-code text of the given web-address</returns>
        /// <remarks></remarks>
        public static Task<string> Page_GetSourceAsync(string url, TimeSpan? timeout = null) {
            using HttpClient client = new();
            if (timeout != null)
                client.Timeout = (TimeSpan)timeout;

            return client.GetStringAsync(url);
        }
        /// <summary>
        /// Returns a portion of the string between <paramref name="locS"/> and <paramref name="locE"/>, using webpage data from <paramref name="sourceOrUrl"/>, using <paramref name="tag"/> as a rougher starting point.
        /// </summary>
        /// <param name="sourceOrUrl">URL or Source-Text</param>
        /// <param name="tag"></param>
        /// <param name="locS">Rough starting-point</param>
        /// <param name="locE">Rough end-point</param>
        /// <param name="IsTheSource">Whether <paramref name="sourceOrUrl"/> is the URL (false) or the source text (true)</param>
        /// <returns></returns>
        public static async Task<string> Page_GetDataPieceAsync(string sourceOrUrl, string tag, string locS, string locE, bool IsTheSource = false) {
            if (IsTheSource) {
                return sourceOrUrl.AsQuery().After(tag).Between(locS, locE).ToString();
            } else {
                string str = await Page_GetSourceAsync(sourceOrUrl);
                return str.AsQuery().After(tag).Between(locS, locE).ToString();
            }
        }

    } //END-ALL
}
