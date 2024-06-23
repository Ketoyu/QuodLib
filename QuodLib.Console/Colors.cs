using QuodLib.Strings;

namespace QuodLib.Console {
    using Console = System.Console;
    public static class Colors {
        public static string Get() {
            string BG = GetBG();
            string FG = GetFG();
            List<string> clr = new() { BG.GetBetween("=", ">"), FG.GetBetween("=", ">") };
            return "<b,f=" + clr[0] + "," + clr[1] + ">"; //<b,f=cc,cc>
        }
        public static string GetBG() {
            return Colors.ConsoleColor_ToString(Console.BackgroundColor, true);
        }
        public static string GetFG() {
            return Colors.ConsoleColor_ToString(Console.ForegroundColor, false);
        }
        public static void Set(string color) {
            Colors.Set(color, Console.BackgroundColor, Console.ForegroundColor);
        }
        private static string ConsoleColor_ToString(ConsoleColor clr, bool BG) {
            string sClr;
            if (BG) sClr = Console.BackgroundColor.ToString().ToLower();
            else sClr = Console.ForegroundColor.ToString().ToLower();
            string _G = "b";
            string dark = "";
            if (sClr.StartsWith("dark")) {
                dark = "d";
                sClr = sClr.Replace("dark", "");
            }
            if (!BG) _G = "f";
            switch (sClr.ToLower()) {
                case "blue": //blue
                    sClr = "b";
                    break;
                case "green": //green
                    sClr = "g";
                    break;
                case "cyan": //cyan
                    sClr = "c";
                    break;
                case "red": //red
                    sClr = "r";
                    break;
                case "magenta": //magenta
                    sClr = "m";
                    break;
                case "yellow": //yellow
                    sClr = "y";
                    break;
                case "white": //white
                    sClr = "w";
                    break;
                case "gray": //Gray
                    sClr = "G";
                    break;
                case "black": //Black
                    sClr = "B";
                    break;
                default:
                    throw new System.Exception("Invalid color string " + sClr + " in " + clr.ToString() + ".");
            }
            return "<" + _G + "=" + dark + sClr + ">";
        }

        internal static void Set(string color, ConsoleColor cBG, ConsoleColor cFG) {
            string color_ = color;
            if ((color_.Length == 6 || color_.Length == 5 || color_.Length == 2 || (color_.Length == 3 && color_.Contains("p")) || ((color.Length == 7 || color.Length == 8
                        || color_.Length == 9 || color_.Length == 10 || color.Length == 11) && color_.Contains(",")))
                    && (color_.Contains('<') && color_.Contains('>'))
                    && (color.Length == 2 || color_.Contains("=") || (color_.Length == 3 && color_.Contains("p")))) {
                if (color_.Contains(",")) {
                    //allow <b,f=_,_> and <f,b=_,_> (just reconstruct it into <b=__><f=__> or <f=__><b=__>and use recursion)
                    color_ = color_.GetBetween("<", ">");
                    string[] parts = color_.Split('=');
                    if (parts[0].Contains(",")) {
                        string[] grnd = parts[0].Split(',');
                        if (parts[1].Contains(",")) {
                            string[] clrs = parts[1].Split(',');
                            Set("<" + grnd[0] + "=" + clrs[0] + ">", cBG, cFG);
                            Set("<" + grnd[1] + "=" + clrs[1] + ">", cBG, cFG);
                        } else {
                            //f and b will be the same color (let the user decide whether to fix that)
                            Set("<" + grnd[0] + "=" + parts[1] + ">", cBG, cFG);
                            Set("<" + grnd[1] + "=" + parts[1] + ">", cBG, cFG);
                        }
                    } else throw new Exception("Color format does not match <p>, <_=_>, <_=__>, <_,_=_,_>, <_,_=__,_>, <_,_=_,__>, <_,_=__,__>, <_,_=_>, <_,_=__>, or <> (one -ground cannot have two colors <f=_,_>, <b=_,_>) :" + color);
                } else if (color_.Contains("p")) {
                    if (color_ == "<p>") Input.PauseKey();
                    else throw new Exception("Color format does not match <p>, <_=_>, <_=__>, <_,_=_,_>, <_,_=__,_>, <_,_=_,__>, <_,_=__,__>, <_,_=_>, <_,_=__>, or <> (one -ground cannot have two colors <f=_,_>, <b=_,_>) :" + color);
                } else {
                    if (color_ == "<>") {
                        Console.BackgroundColor = cBG;
                        Console.ForegroundColor = cFG;
                    } else {
                        color_ = color_.GetBetween("<", ">");
                        bool Background = (color_[0] == 'b');
                        bool Dark = false;
                        color_ = color_.GetAfter("=");
                        char clr;
                        if (color_.Length == 2) {
                            if (color_.Contains("d")) {
                                Dark = true;
                                byte idxD = (byte)color_.IndexOf('d');
                                if (idxD == 0) clr = color_[1];
                                else clr = color_[0];
                                if ((clr == 'w') || (clr == 'W'))
                                    throw new Exception("Color White cannot be dark! @ " + color);
                                else if (clr == 'B')
                                    throw new Exception("Color Black cannot be dark! @ " + color);
                            } else
                                throw new Exception("Unrecognized color character pair " + color_ + " in " + color + ".");
                        } else
                            clr = color_[0];
                        switch (clr) {
                            case 'b': //blue
                                Set_ConsoleColor("Blue", Dark, Background);
                                break;
                            case 'g': //green
                                Set_ConsoleColor("Green", Dark, Background);
                                break;
                            case 'c': //cyan
                                Set_ConsoleColor("Cyan", Dark, Background);
                                break;
                            case 'r': //red
                                Set_ConsoleColor("Red", Dark, Background);
                                break;
                            case 'm': //magenta
                                Set_ConsoleColor("Magenta", Dark, Background);
                                break;
                            case 'y': //yellow
                                Set_ConsoleColor("Yellow", Dark, Background);
                                break;
                            case 'w': //white
                                Set_ConsoleColor("White", Dark, Background);
                                break;
                            case 'G': //Gray
                                Set_ConsoleColor("Gray", Dark, Background);
                                break;
                            case 'B': //Black
                                Set_ConsoleColor("Black", Dark, Background);
                                break;
                            default:
                                throw new System.Exception("Invalid color character " + clr + " in " + color + ". Char must be lower-case except in the case of G, g, B, or b, where G is Gray, g is green, B is Black, and b is blue.");
                        }
                    }
                }
            } else
                throw new Exception("Color format does not match <p>, <_=_>, <_=__>, <_,_=_,_>, <_,_=__,_>, <_,_=_,__>, <_,_=__,__>, <_,_=_>, <_,_=__>, or <> :" + color);
        }
        private static void Set_ConsoleColor(string color, bool dark, bool background) {
            ConsoleColor final;
            if (dark) final = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), "Dark" + color);
            else final = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color);
            if (background) Console.BackgroundColor = final;
            else Console.ForegroundColor = final;
        }
    }
}