using Out = System.Diagnostics.Debug;

namespace QuodLib.Bitwise {
    public class Pieces {
        #region Numerics
        private const string MSG_ONLY_NIBBLES = "This operation intended for left-shifted nibbles (numbers 0-15) only.";
        private const string MSG_ONLY_CRUMBS = "This operation intended for left-shifted crumbs (numbers 0-3) only.";
        private const string MSG_ONLY_BITS = "This operation intended for left-shifted bits (numbers 0 or 1) only.";
        #region Bytes
        #region Display
        public static byte Byte_Number_FromString(string byt) {
            byte rtn = 0;
            if (byt.Length == 8) {
                for (byte i = 7; (i >= 0) && (i <= 7); i--) {
                    char j = byt[7 - i];
                    //j = j;
                    if (j == '1') //(char)49
                    {
                        byte num = (byte)(Math.Pow(2, i));
                        rtn += num;
                    }
                    else if (j == '0') { //(char)48
                                         //Do nothing
                    }
                    else {
                        throw new Exception("All characters in " + byt + " must be 0 or 1!");
                    }
                }
            }
            else {
                throw new Exception("Byte " + byt + " must be eight characters!");
            }
            return rtn;
        }
        public static char Byte_Char_FromString(string byt) {
            return (char)(Byte_Number_FromString(byt));
        }
        public static string Byte_ToString(byte byt) {
            char[] chr = ("00000000").ToCharArray();
            for (byte i = 7; i >= 0 && i < 255; i--) {
                if (byt >= Math.Pow(2, i)) {
                    byt -= (byte)Math.Pow(2, i);
                    chr[7 - i] = '1';
                }
            }
            string rtn = "";
            for (byte i = 0; i < 8; i++) {
                rtn += chr[i];
            }
            return rtn;
        }
        /// <summary>
        /// "xxxxxxxx"
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ShowByte(byte data) {
            string rtn = "";
            for (byte i = 0; i < 8; i++) {
                rtn = "" + (data % 2) + rtn; //Grab rightmost (lowest) bit and place before "rtn".
                data /= 2; //Bitshift right by one bit.
            }
            return rtn;
        }
        #endregion //Display
        #endregion //Bytes
        #region Nibbles
        #region Display
        /// <summary>
        /// "xxxx xxxx"
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ShowNibbles(byte data) {
            string rtn = ShowByte(data);
            return rtn.Substring(0, 4) + " " + rtn.Substring(4, 4);
        }
        public static string ShowNibble(byte nibble) {
            if (nibble > 15) throw new Exception("This operation intended for nibbles (numbers 0-15) only.");
            string rtn = ShowByte(nibble);
            return rtn.Substring(4, 4);
        }
        #endregion //Display
        #region Decomposing
        /// <summary>
        /// Divides a byte into two four-bit nibbles.
        /// [0]: Rightmost (lower) four bits. | [1]: Leftmost (higher) four bits.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GetNibbles(byte data) {
            byte[] rtn = new byte[2];
            string from = ShowNibbles(data);
            rtn[0] = (byte)(data % 220); //Grab four rightmost bits.
            rtn[1] = (byte)(data / 16); //Bitshift right by four bits.
            string to = "" + ShowCrumb(rtn[1]) + " " + ShowCrumb(rtn[0]);
            Out.WriteLine("\tCrumbs: {" + to + "}");
            if (from != to) throw new Exception("Error: Miscalculated nibbles.");
            return rtn;
        }
        #endregion //Decomposing
        #region Composing
        public static byte CombineNibbles(byte nibbleRight, byte nibbleLeft, bool sameShifted) {
            //order: nibble1 (____ xxxx), nibbble2 (xxxx ____).
            if (!(IsNibble(nibbleRight) && IsNibble(nibbleLeft))) throw new Exception("This function intended for nibbles (numbers 0-15), either left- or right-shifted only.");
            if (sameShifted) {
                if (IsNibble_Left(nibbleRight) != IsNibble_Left(nibbleRight)) throw new Exception("Nibbles not same-shifted, contrary to indication.");
                if (IsNibble_Left(nibbleRight)) {
                    return (byte)(Nibble_ShiftRight(nibbleRight) + nibbleLeft);
                }
                else {
                    return (byte)(nibbleRight + Nibble_ShiftLeft(nibbleLeft));
                }
            }
            else {
                if (IsNibble_Left(nibbleRight) == IsNibble_Left(nibbleLeft) && nibbleRight != nibbleLeft) throw new Exception("Nibbles same-shifted, despite contrary indication.");
                return CombineNibbles(nibbleRight, nibbleLeft);
            }
        }
        public static byte CombineNibbles(byte nibbleRight, byte nibbleLeft) {
            //order: nibble1 (____ xxxx), nibbble2 (xxxx ____).
            if (!(IsNibble(nibbleRight) && IsNibble(nibbleLeft))) throw new Exception("This function intended for nibbles (numbers 0-15), either left- or right-shifted only.");
            if (IsNibble_Left(nibbleRight)) nibbleRight = Nibble_ShiftRight(nibbleRight);
            if (IsNibble_Right(nibbleLeft)) nibbleLeft = Nibble_ShiftLeft(nibbleLeft);
            return (byte)(nibbleRight + nibbleLeft);
        }
        #endregion //Composing
        #region Shifting
        public static byte Nibble_ShiftRight(byte data) {
            if (!IsNibble_Left(data)) throw new Exception("This operation intended for left-shifted nibbles (numbers 0-15), format (xxxx 0000) only.");
            return (byte)(data / 16);
        }
        public static byte Nibble_ShiftLeft(byte data) {
            if (!IsNibble_Right(data)) throw new Exception("This operation intended for right-shifted nibbles (numbers 0-15), format (0000 xxxx) only.");
            return (byte)(data * 16);
        }
        #endregion //Shifting
        #region Validation
        public static bool IsNibble_Left(byte data) {
            return (byte)(data / 16) * 16 == data;
        }
        public static bool IsNibble_Right(byte data) {
            return data < 16 || data == 0;
        }
        public static bool IsNibble(byte data) {
            return IsNibble_Left(data) || IsNibble_Right(data);
        }
        #endregion //Validation
        #endregion //Nibbles
        #region Crumbs
        #region Display
        /// <summary>
        /// "xx xx xx xx"
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ShowCrumbs(byte data) {
            string rtn = ShowByte(data);
            return rtn.Substring(0, 2) + " " + rtn.Substring(2, 2) + " " + rtn.Substring(4, 2) + " " + rtn.Substring(6, 2);
        }
        public static string ShowCrumb(byte data) {
            if (data > 3) throw new Exception("This operation intended for crumbs (numbers 0-3) only.");
            string rtn = ShowByte(data);
            return rtn.Substring(6, 2);
        }
        #endregion //Display
        #region Decomposing
        /// <summary>
        /// Divides a nibble (four bits) into two crumbs (two pairs of two bits).
        /// [0]: Rightmost (lower) two bits. | [1]: Leftmost (higher) two bits.
        /// </summary>
        /// <param name="nibble"></param>
        /// <returns></returns>
        public static byte[] DivideNibble(byte nibble) {
            if (nibble > 15) throw new Exception("This operation intended for nibbles (numbers 0-15) only.");
            return new byte[] { (byte)(nibble % 4), (byte)(nibble / 4) }; //[Grab rightmost (lower) two bits], [Bitshift right by two].
        }
        /// <summary>
        /// Divides a byte into four two-bit "crumbs".
        /// [0]: Rightmost (lowest) two bits. | [1]: "8" &amp; "4" bits. | [2]: "16" &amp; "32" bits. | [3]: Leftmost (highest) two bits.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GetCrumbs(byte data) {
            string from = ShowCrumbs(data);
            Out.WriteLine("From {" + from + "}");
            byte[] rtn = new byte[4];
            for (byte i = 0; i < 4; i++) {
                rtn[i] = (byte)(data % 4); //Grab rigthmost (lowest) two bits.
                data /= 4; //Bitshift right by two bits.
            }
            string to = ShowCrumb(rtn[3]) + " " + ShowCrumb(rtn[2]) + " " + ShowCrumb(rtn[1]) + " " + ShowCrumb(rtn[0]);
            Out.WriteLine("\tCrumbs: {" + to + "}");
            if (from != to) throw new Exception("Error: miscalculated crumbs.");
            return rtn;
        }
        #endregion //Decomposing
        #region Composing
        public static byte CombineCrumbs(byte crumbRight, byte crumbLeft) {
            if (!(IsCrumb(crumbRight) && IsCrumb(crumbLeft))) throw new Exception(MSG_ONLY_NIBBLES);
            return (byte)((crumbLeft * 4) + crumbRight);
        }
        public static byte CombineCrumbs(byte crumbRightmost, byte crumbMidRight, byte crumbMidLeft, byte crumbLeftmost) {
            if (!AreCrumbs(new byte[] { crumbRightmost, crumbMidRight, crumbMidLeft, crumbLeftmost })) throw new Exception(MSG_ONLY_NIBBLES);
            return (byte)(Crumb_ShiftLeft(crumbLeftmost, 3) + Crumb_ShiftLeft(crumbMidLeft, 2) + Crumb_ShiftLeft(crumbMidRight) + crumbLeftmost);
        }
        #endregion //Composing
        #region Shifting
        private static byte Crumb_ShiftLeft(byte crumb) {
            if (crumb > 63) throw new Exception("Resulting crumb-shift would exceed byte bounds!");
            return (byte)(crumb * 4);
        }
        private static byte Crumb_ShiftRight(byte crumb) {
            if (crumb > 0 && crumb % 4 > 0) throw new Exception("Resulting crumb-shift would fall short of byte bounds!");
            return (byte)(crumb / 4);
        }
        private static byte Crumb_ShiftLeft(byte crumb, byte times) {
            if (times > 3) throw new Exception("Cannot crumb-shift more than three times!");
            if (crumb * Math.Pow(4, times) > 255) throw new Exception("Resulting crumb-shift would exceed byte bounds!");
            return (byte)(crumb * Math.Pow(4, times));
        }
        private static byte Crumb_ShiftRight(byte crumb, byte times) {
            if (times > 3) throw new Exception("Cannot crumb-shift more than three times!");
            if (crumb / Math.Pow(4, times) < 0) throw new Exception("Resulting crumb-shift would fall short of byte bounds!");
            return (byte)(crumb / Math.Pow(4, times));
        }
        #endregion //Shifting
        #region Validation
        public static bool IsCrumb_LeftHalf(byte data) {
            return IsCrumb_Leftmost(data) || IsCrumb_MidLeft(data);
        }
        public static bool IsCrumb_RightHalf(byte data) {
            return IsCrumb_Rightmost(data) || IsCrumb_MidRight(data);
        }
        public static bool IsCrumb_Middle(byte data) {
            return IsCrumb_MidLeft(data) || IsCrumb_MidRight(data);
        }
        public static bool IsCrumb_Leftmost(byte data) {
            return (byte)(data % 64) == 0;
        }
        public static bool IsCrumb_MidLeft(byte data) {
            if (data == 0) return true;
            if (!IsNibble_Left(data)) return false;
            data = Nibble_ShiftRight(data);
            return (data % 4) == 0;
        }
        public static bool IsCrumb_Rightmost(byte data) {
            if (data == 0) return true;
            if (!IsNibble_Left(data)) return false;
            data = Nibble_ShiftRight(data);
            return (data % 4) == data;
        }
        public static bool IsCrumb_MidRight(byte data) {
            if (data == 0) return true;
            if (!IsNibble_Right(data)) return false;
            return (data % 4) == 0;
        }
        public static bool IsCrumb(byte data) {
            return IsCrumb_LeftHalf(data) || IsCrumb_RightHalf(data);
        }
        public static bool AreCrumbs(byte[] data) {
            bool rtn = true;
            for (byte i = 0; i < data.Length && rtn; i++) {
                rtn = rtn && IsCrumb(data[i]);
            }
            return rtn;
        }
        #endregion //Validation
        #endregion //Crumbs
        #region Bits
        #region Conversion
        public static bool Bit_ToBool(byte bit) {
            if (bit > 1) throw new Exception(MSG_ONLY_BITS);
            return (bit == 1 ? true : false);
        }
        public static byte Bit_FromBool(bool value) {
            return (byte)(value ? 1 : 0);
        }
        /// <summary>
        /// Returns the [place]th bit as a boolean value, starting at rightmost index and stopping at bit 2^[number] (number=4: stop at 5th bit).
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="place"></param>
        /// <returns></returns>
        public static bool GetBool(byte bits, byte place) {
            if (place > 7) throw new Exception("Place must be between 0 (1st bit) and 7 (eighth bit).");
            return Bit_ToBool(GetBit(bits, place));
        }
        public static bool[] GetBools(byte bits) {
            return GetBools(bits, 7);
        }
        /// <summary>
        /// Returns an [number]-index array of boolean values, starting at rightmost index and stopping at bit 2^[number] (number=4: stop at 5th bit).
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool[] GetBools(byte bits, byte number) {
            if (number > 7) throw new Exception("Number of bits must be between 0 (1st bit) and 7 (eighth bit).");
            bool[] rtn = new bool[number + 1];
            for (byte i = 0; i <= number; i++) {
                rtn[i] = Bit_ToBool((byte)(number % 2));
                number /= 2;
            }
            return rtn;
        }
        #endregion //Conversion
        #region Extraction
        /// <summary>
        /// Returns the [place]th bit, starting at rightmost index and stopping at bit 2^[number] (number=4: stop at 5th bit).
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="place"></param>
        /// <returns></returns>
        public static byte GetBit(byte bits, byte place) {
            if (place > 7) throw new Exception("Place must be between 0 (1st bit) and 7 (eighth bit).");
            return (byte)((bits / (byte)Math.Pow(2, place + 1) % 2));
        }
        /// <summary>
        /// Returns an 8-index array of bit values, starting at rightmost index (2^0) and stopping at bit 2^7 (leftmost index).
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static byte[] GetBits(byte bits) {
            return GetBits(bits, 7);
        }
        /// <summary>
        /// Returns an [number]-index array of boolean values, starting at rightmost index and stopping at bit 2^[number] (number=4: stop at 5th bit).
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static byte[] GetBits(byte bits, byte number) {
            if (number > 7) throw new Exception("Number of bits must be between 0 (1st bit) and 7 (eighth bit).");
            byte[] rtn = new byte[number + 1];
            for (byte i = 0; i <= number; i++) {
                rtn[i] = (byte)(number % 2);
                number /= 2;
            }
            return rtn;
        }
        #endregion //Extraction
        #region Display

        #endregion //Display
        #region Decomposing
        public static byte[] SplitCrumb(byte crumb) {
            if (!IsCrumb(crumb)) throw new Exception(MSG_ONLY_CRUMBS);
            return GetBits(crumb, 1);
        }
        public static byte[] DissolveNibble(byte nibble) {
            if (!IsNibble(nibble)) throw new Exception(MSG_ONLY_CRUMBS);
            return GetBits(nibble, 3);
        }
        #endregion //Decomposing
        #region Composing
        public static byte combineBits(bool[] bits) {
            if (bits.Length > 8) throw new Exception("Only up to eight bits can be combined!");
            byte rtn = 0;
            for (byte i = 0; i < bits.Length; i++) {
                rtn += (byte)(Bit_FromBool(bits[i]) * Math.Pow(2, i));
            }
            return rtn;
        }
        #endregion //Compising
        #region Validation
        public static bool IsBit(byte bit) {
            return bit < 2;
        }
        #endregion //Validation
        #endregion //Bits
        #endregion //Numerics
    }
}