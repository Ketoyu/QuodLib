
using System.Drawing;

namespace QuodLib {
    using Strings;
    /// <summary>
    /// Contains functions for page requests and 'dissection, mostly.
    /// </summary>
    public static class Net {
        /// <summary>
        /// Return an image object from an image url ("http://... .../___.png").
        /// [ SOURCES: http://www.vcskicks.com/download_file_http.php &amp; http://www.vcskicks.com/image-from-url.php ]
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public static async Task<Image> Page_GetImageAsync(string imageUrl) {
            //source: + http://www.vcskicks.com/download_file_http.php
            //		+ http://www.vcskicks.com/image-from-url.php
            try {
                using (HttpClient client = new()) {
                    Stream stream = await client.GetStreamAsync(imageUrl);
                    return Image.FromStream(stream);
                }
            } catch (Exception ex) {
                throw new Exception("There was a problem downloading the file:\n" + ex.Message);
            }
        }
    } //END-ALL
}
