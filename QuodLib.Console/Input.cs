using QuodLib.Strings;

namespace QuodLib.Console {
    using Console = System.Console;
    public static class Input {
        public static T Get<T>(Func<string, (bool Success, T Value)> parse, string consoleColor = "<>") {
            (bool Success, T Value) rtn = (false, default(T));
            while (!rtn.Success) {
                string inp = Get(consoleColor);
                rtn = parse(inp);

                if (!rtn.Success) {
                    Console.CursorTop--;
                    for (byte i = 1; i <= inp.Length; i++)
                        Console.Write(" ");
                    for (byte i = 1; i <= inp.Length; i++)
                        Console.Write("\b");
                }
            }

            return rtn.Value;
        }

        public static string Get(string cnsClr1 = "<>", string cnsClr2 = "<>") {
            //Normal
            if (cnsClr1 == "<>" && cnsClr2 == "<>")
                return Console.ReadLine();

            // (else), custom.
            ConsoleColor cBG = Console.BackgroundColor;
            ConsoleColor cFG = Console.ForegroundColor;
            Colors.Set(cnsClr1);
            Colors.Set(cnsClr2);
            string rtn = Console.ReadLine();
            Console.BackgroundColor = cBG;
            Console.ForegroundColor = cFG;
            return rtn;
        }

        /// <summary>
        /// Restricts console user-input to a byte, or alternative inputs if provided.
        /// </summary>
        /// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
        /// <param name="bounds">Minimum/Maximum of numeric input (default 0 to 255, inclusive)</param>
        /// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
        /// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
        /// <returns>String user-input</returns>
        public static string Byte(string consoleColor = "<>", (byte min, byte max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true) {
            return Get((inp) => {
                if (alternates != null) {
                    (bool s, string v) rtn = parseAlt(inp, alternates, caseSensitive);
                    if (rtn.s) return rtn;
                }

                bool success = byte.TryParse(inp, out byte value);

                if (bounds != null)
                    success &= (value >= bounds?.min && value <= bounds?.max);

                return (success, "" + value);
            }, consoleColor);
        }

        /// <summary>
        /// Restricts console user-input to a decimal, or alternative inputs if provided.
        /// </summary>
        /// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
        /// <param name="bounds">Minimum/Maximum of numeric input (default -7.9E+28 to 7.9E+28, inclusive)</param>
        /// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
        /// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
        /// <returns>String user-input</returns>
        public static string Decimal(string consoleColor = "<>", (decimal min, decimal max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true) {
            return Get((inp) => {
                if (alternates != null) {
                    (bool s, string v) rtn = parseAlt(inp, alternates, caseSensitive);
                    if (rtn.s) return rtn;
                }

                bool success = decimal.TryParse(inp, out decimal value);

                if (bounds != null)
                    success &= (value >= bounds?.min && value <= bounds?.max);

                return (success, "" + value);
            }, consoleColor);
        }

        /// <summary>
        /// Restricts console user-input to a double, or alternative inputs if provided.
        /// </summary>
        /// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
        /// <param name="bounds">Minimum/Maximum of numeric input (default -4.9E-324 to 1.7E+308, inclusive)</param>
        /// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
        /// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
        /// <returns>String user-input</returns>
        public static string Double(string consoleColor = "<>", (double min, double max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true) {
            return Get((inp) => {
                if (alternates != null) {
                    (bool s, string v) rtn = parseAlt(inp, alternates, caseSensitive);
                    if (rtn.s) return rtn;
                }

                bool success = double.TryParse(inp, out double value);

                if (bounds != null)
                    success &= (value >= bounds?.min && value <= bounds?.max);

                return (success, "" + value);
            }, consoleColor);
        }

        /// <summary>
        /// Restricts console user-input to a float (default -3.4E-38 to 3.4E+38, inclusive) or a List&lt;string&gt; of alternative inputs (default case-sensitive).
        /// </summary>
        /// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
        /// <param name="bounds">Minimum/Maximum of numeric input (default -3.4E-38 to 3.4E+38, inclusive)</param>
        /// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
        /// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
        /// <returns>String user-input</returns>
        public static string Float(string consoleColor = "<>", (float min, float max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true) {
            return Get((inp) => {
                if (alternates != null) {
                    (bool s, string v) rtn = parseAlt(inp, alternates, caseSensitive);
                    if (rtn.s) return rtn;
                }

                bool success = float.TryParse(inp, out float value);

                if (bounds != null)
                    success &= (value >= bounds?.min && value <= bounds?.max);

                return (success, "" + value);
            }, consoleColor);
        }

        /// <summary>
        /// Restricts console user-input to an integer, or alternative inputs if provided.
        /// </summary>
        /// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
        /// <param name="bounds">Minimum/Maximum of numeric input (default -2,147,483,648 to 2,147,483,647, inclusive)</param>
        /// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
        /// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
        /// <returns>String user-input</returns>
        public static string Int(string consoleColor = "<>", (int min, int max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true) {
            return Get((inp) => {
                if (alternates != null) {
                    (bool s, string v) rtn = parseAlt(inp, alternates, caseSensitive);
                    if (rtn.s) return rtn;
                }

                bool success = int.TryParse(inp, out int value);

                if (bounds != null)
                    success &= (value >= bounds?.min && value <= bounds?.max);

                return (success, "" + value);
            }, consoleColor);
        }

        /// <summary>
        /// Restricts console user-input to an long-integer, or alternative inputs if provided.
        /// </summary>
        /// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
        /// <param name="bounds">Minimum/Maximum of numeric input (default -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807, inclusive)</param>
        /// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
        /// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
        /// <returns>String user-input</returns>
        public static string Long(string consoleColor = "<>", (long min, long max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true) {
            return Get((inp) => {
                if (alternates != null) {
                    (bool s, string v) rtn = parseAlt(inp, alternates, caseSensitive);
                    if (rtn.s) return rtn;
                }

                bool success = long.TryParse(inp, out long value);

                if (bounds != null)
                    success &= (value >= bounds?.min && value <= bounds?.max);

                return (success, "" + value);
            }, consoleColor);
        }

        /// <summary>
        /// Restricts console user-input to a signed byte, or alternative inputs if provided.
        /// </summary>
        /// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
        /// <param name="bounds">Minimum/Maximum of numeric input (default -128 to 127, inclusive)</param>
        /// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
        /// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
        /// <returns>String user-input</returns>
        public static string SByte(string consoleColor = "<>", (sbyte min, sbyte max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true) {
            return Get((inp) => {
                if (alternates != null) {
                    (bool s, string v) rtn = parseAlt(inp, alternates, caseSensitive);
                    if (rtn.s) return rtn;
                }

                bool success = sbyte.TryParse(inp, out sbyte value);

                if (bounds != null)
                    success &= (value >= bounds?.min && value <= bounds?.max);

                return (success, "" + value);
            }, consoleColor);
        }

        /// <summary>
        /// Restricts console user-input to a short, or alternative inputs if provided.
        /// </summary>
        /// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
        /// <param name="bounds">Minimum/Maximum of numeric input (default -32,768 to 32,767, inclusive)</param>
        /// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
        /// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
        /// <returns>String user-input</returns>
        public static string Short(string consoleColor = "<>", (short min, short max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true) {
            return Get((inp) => {
                if (alternates != null) {
                    (bool s, string v) rtn = parseAlt(inp, alternates, caseSensitive);
                    if (rtn.s) return rtn;
                }

                bool success = short.TryParse(inp, out short value);

                if (bounds != null)
                    success &= (value >= bounds?.min && value <= bounds?.max);

                return (success, "" + value);
            }, consoleColor);
        }

        /// <summary>
        /// Restricts console user-input to an unsigned uinteger, or alternative inputs if provided.
        /// </summary>
        /// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
        /// <param name="bounds">Minimum/Maximum of numeric input (default 0 to 4,294,967,295, inclusive, inclusive)</param>
        /// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
        /// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
        /// <returns>String user-input</returns>
        public static string UInt(string consoleColor = "<>", (uint min, uint max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true) {
            return Get((inp) => {
                if (alternates != null) {
                    (bool s, string v) rtn = parseAlt(inp, alternates, caseSensitive);
                    if (rtn.s) return rtn;
                }

                bool success = uint.TryParse(inp, out uint value);

                if (bounds != null)
                    success &= (value >= bounds?.min && value <= bounds?.max);

                return (success, "" + value);
            }, consoleColor);
        }

        /// <summary>
        /// Restricts console user-input to an unsigned long-integer, or alternative inputs if provided.
        /// </summary>
        /// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
        /// <param name="bounds">Minimum/Maximum of numeric input (default 0 to 18,446,744,073,709,551,616, inclusive, inclusive)</param>
        /// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
        /// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
        /// <returns>String user-input</returns>
        public static string ULong(string consoleColor = "<>", (ulong min, ulong max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true) {
            return Get((inp) => {
                if (alternates != null) {
                    (bool s, string v) rtn = parseAlt(inp, alternates, caseSensitive);
                    if (rtn.s) return rtn;
                }

                bool success = ulong.TryParse(inp, out ulong value);

                if (bounds != null)
                    success &= (value >= bounds?.min && value <= bounds?.max);

                return (success, "" + value);
            }, consoleColor);
        }

        /// <summary>
        /// Restricts console user-input to an unsigned short, or alternative inputs if provided.
        /// </summary>
        /// <param name="consoleColor">Color &lt;_=__&gt; of user-input</param>
        /// <param name="bounds">Minimum/Maximum of numeric input (default 0 to 65,535, inclusive)</param>
        /// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
        /// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
        /// <returns>String user-input</returns>
        public static string UShort(string consoleColor = "<>", (ushort min, ushort max)? bounds = null, List<string>? alternates = null, bool caseSensitive = true) {
            return Get((inp) => {
                if (alternates != null) {
                    (bool s, string v) rtn = parseAlt(inp, alternates, caseSensitive);
                    if (rtn.s) return rtn;
                }

                bool success = ushort.TryParse(inp, out ushort value);

                if (bounds != null)
                    success &= (value >= bounds?.min && value <= bounds?.max);

                return (success, "" + value);
            }, consoleColor);
        }
        public static void PauseKey() {
            int[] cPos = new int[] { Console.CursorLeft, Console.CursorTop };
            string In = Console.ReadKey().KeyChar.ToString();
            if (In == "\r") {
                Console.CursorTop = cPos[1];
                Console.CursorLeft = cPos[0];
            } else {
                if (In.Length == 1) {
                    if (Console.CursorLeft > 0)
                        Console.CursorLeft -= 1;
                    Console.Write(" ");
                    Console.Write("\b");
                } else
                    throw new Exception("Unplanned KeyChar occurred!");
            }
        }
        /// <summary>
        /// Returns the input and whether it was in the list of alternates.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="alternates">Alternative String inputs (input is returned in all-caps if case-sensitivity is False)</param>
        /// <param name="caseSensitive">Case-sensitivity of alternate String inputs (input is returned in all-caps if False)</param>
        /// <returns></returns>
        private static (bool, string) parseAlt(string input, List<string> alternates, bool caseSensitive = true) {
            if (caseSensitive)
                return ((alternates.Contains(input, false)), input.ToUpper());
            else
                return ((alternates.Contains(input)), input);
        }
    }
}