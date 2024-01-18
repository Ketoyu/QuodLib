using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace QuodLib.Drawing {
    public static class Extensions {
        public static void WriteBlock(this WriteableBitmap target, ref byte[] rgbData, Int32Rect expectedArea) {
            target.Lock();
            Marshal.Copy(rgbData, 0, target.BackBuffer, rgbData.Length);
            target.AddDirtyRect(expectedArea);
            target.Unlock();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] To1D(this byte[,,] data) {
            int width = data.GetLength(0),
                height = data.GetLength(1);

            byte[] result = new byte[width * height * 3];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    result.WriteRGB(x, y, width, data[x,y,0], data[x,y,1], data[x,y,2]);

            return result;
        }

        public static void WriteRGB(this byte[] target, int x, int y, int width, byte r, byte g, byte b) {
            int baseIndex = (y * width) + x;
            target[baseIndex] = r;
            target[baseIndex + 1] = g;
            target[baseIndex + 2] = b;
        }

        public static void WriteGrayscale(this byte[] target, int x, int y, int width, byte grayscaleValue)
            => target.WriteRGB(x, y, width, grayscaleValue, grayscaleValue, grayscaleValue);
    }
}
