using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Bitwise {
    public static class Rectangles {
        public struct RectByte {
            #region Properties
            public static byte X, Y, W, H;
            public static byte Width {
                get
                {
                    return W;
                }
                set
                {
                    W = value;
                }
            }
            public static byte Height {
                get
                {
                    return H;
                }
                set
                {
                    H = value;
                }
            }
            #endregion //Properties
            public RectByte(byte x_, byte y_, byte w_, byte h_) {
                X = x_; Y = y_; W = w_; H = h_;
            }
            public bool IsInside(byte x_, byte y_) {
                return (x_ >= X && y_ >= Y && x_ < X + W && y_ < Y + H);
            }
            public bool IsInside(System.Drawing.Point pt) {
                return (pt.X >= X && pt.Y >= Y && pt.X < X + W && pt.Y < Y + H);
            }
        }
        public struct RectSByte {
            #region Properties
            public static sbyte X, Y, W, H;
            public static sbyte Width {
                get
                {
                    return W;
                }
                set
                {
                    W = value;
                }
            }
            public static sbyte Height {
                get
                {
                    return H;
                }
                set
                {
                    H = value;
                }
            }
            #endregion //Properties
            public RectSByte(sbyte x_, sbyte y_, sbyte w_, sbyte h_) {
                X = x_; Y = y_; W = w_; H = h_;
            }
            public bool IsInside(sbyte x_, sbyte y_) {
                return (x_ >= X && y_ >= Y && x_ < X + W && y_ < Y + H);
            }
            public bool IsInside(System.Drawing.Point pt) {
                return (pt.X >= X && pt.Y >= Y && pt.X < X + W && pt.Y < Y + H);
            }
        }
        public struct RectUShort {
            #region Properties
            public static ushort X, Y, W, H;
            public static ushort Width {
                get
                {
                    return W;
                }
                set
                {
                    W = value;
                }
            }
            public static ushort Height {
                get
                {
                    return H;
                }
                set
                {
                    H = value;
                }
            }
            #endregion //Properties
            public RectUShort(ushort x_, ushort y_, ushort w_, ushort h_) {
                X = x_; Y = y_; W = w_; H = h_;
            }
            public bool IsInside(ushort x_, ushort y_) {
                return (x_ >= X && y_ >= Y && x_ < X + W && y_ < Y + H);
            }
            public bool IsInside(System.Drawing.Point pt) {
                return (pt.X >= X && pt.Y >= Y && pt.X < X + W && pt.Y < Y + H);
            }

        }
        public struct RectShort {
            #region Properties
            public static short X, Y, W, H;
            public static short Width {
                get
                {
                    return W;
                }
                set
                {
                    W = value;
                }
            }
            public static short Height {
                get
                {
                    return H;
                }
                set
                {
                    H = value;
                }
            }
            #endregion //Properties
            public RectShort(short x_, short y_, short w_, short h_) {
                X = x_; Y = y_; W = w_; H = h_;
            }
            public bool IsInside(short x_, short y_) {
                return (x_ >= X && y_ >= Y && x_ < X + W && y_ < Y + H);
            }
            public bool IsInside(System.Drawing.Point pt) {
                return (pt.X >= X && pt.Y >= Y && pt.X < X + W && pt.Y < Y + H);
            }
        }
        public struct RectUInt {
            #region Properties
            public static uint X, Y, W, H;
            public static uint Width {
                get
                {
                    return W;
                }
                set
                {
                    W = value;
                }
            }
            public static uint Height {
                get
                {
                    return H;
                }
                set
                {
                    H = value;
                }
            }
            #endregion //Properties
            public RectUInt(uint x_, uint y_, uint w_, uint h_) {
                X = x_; Y = y_; W = w_; H = h_;
            }
            public bool IsInside(uint x_, uint y_) {
                return (x_ >= X && y_ >= Y && x_ < X + W && y_ < Y + H);
            }
            public bool IsInside(System.Drawing.Point pt) {
                return (pt.X >= X && pt.Y >= Y && pt.X < X + W && pt.Y < Y + H);
            }
        }
        public struct RectInt {
            #region Properties
            public static int X, Y, W, H;
            public static int Width {
                get
                {
                    return W;
                }
                set
                {
                    W = value;
                }
            }
            public static int Height {
                get
                {
                    return H;
                }
                set
                {
                    H = value;
                }
            }
            #endregion //Properties
            public RectInt(int x_, int y_, int w_, int h_) {
                X = x_; Y = y_; W = w_; H = h_;
            }
            public bool IsInside(int x_, int y_) {
                return (x_ >= X && y_ >= Y && x_ < X + W && y_ < Y + H);
            }
            public bool IsInside(System.Drawing.Point pt) {
                return (pt.X >= X && pt.Y >= Y && pt.X < X + W && pt.Y < Y + H);
            }
        }
        public struct RectULong {
            #region Properties
            public static ulong X, Y, W, H;
            public static ulong Width {
                get
                {
                    return W;
                }
                set
                {
                    W = value;
                }
            }
            public static ulong Height {
                get
                {
                    return H;
                }
                set
                {
                    H = value;
                }
            }
            #endregion //Properties
            public RectULong(ulong x_, ulong y_, ulong w_, ulong h_) {
                X = x_; Y = y_; W = w_; H = h_;
            }
            public bool IsInside(ulong x_, ulong y_) {
                return (x_ >= X && y_ >= Y && x_ < X + W && y_ < Y + H);
            }
            public bool IsInside(System.Drawing.Point pt) {
                return ((ulong)pt.X >= X && (ulong)pt.Y >= Y && (ulong)pt.X < X + W && (ulong)pt.Y < Y + H);
            }
        }
        public struct RectLong {
            #region Properties
            public static long X, Y, W, H;
            public static long Width {
                get
                {
                    return W;
                }
                set
                {
                    W = value;
                }
            }
            public static long Height {
                get
                {
                    return H;
                }
                set
                {
                    H = value;
                }
            }
            #endregion //Properties
            public RectLong(long x_, long y_, long w_, long h_) {
                X = x_; Y = y_; W = w_; H = h_;
            }
            public bool IsInside(long x_, long y_) {
                return (x_ >= X && y_ >= Y && x_ < X + W && y_ < Y + H);
            }
            public bool IsInside(System.Drawing.Point pt) {
                return (pt.X >= X && pt.Y >= Y && pt.X < X + W && pt.Y < Y + H);
            }
        }
        public struct RectFloat {
            #region Properties
            public static float X, Y, W, H;
            public static float Width {
                get
                {
                    return W;
                }
                set
                {
                    W = value;
                }
            }
            public static float Height {
                get
                {
                    return H;
                }
                set
                {
                    H = value;
                }
            }
            #endregion //Properties
            public RectFloat(float x_, float y_, float w_, float h_) {
                X = x_; Y = y_; W = w_; H = h_;
            }
            public bool IsInside(float x_, float y_) {
                return (x_ >= X && y_ >= Y && x_ < X + W && y_ < Y + H);
            }
            public bool IsInside(System.Drawing.Point pt) {
                return (pt.X >= X && pt.Y >= Y && pt.X < X + W && pt.Y < Y + H);
            }
        }
        public struct RectDouble {
            #region Properties
            public static double X, Y, W, H;
            public static double Width {
                get
                {
                    return W;
                }
                set
                {
                    W = value;
                }
            }
            public static double Height {
                get
                {
                    return H;
                }
                set
                {
                    H = value;
                }
            }
            #endregion //Properties
            public RectDouble(double x_, double y_, double w_, double h_) {
                X = x_; Y = y_; W = w_; H = h_;
            }
            public bool IsInside(double x_, double y_) {
                return (x_ >= X && y_ >= Y && x_ < X + W && y_ < Y + H);
            }
            public bool IsInside(System.Drawing.Point pt) {
                return (pt.X >= X && pt.Y >= Y && pt.X < X + W && pt.Y < Y + H);
            }
        }
    }
}
