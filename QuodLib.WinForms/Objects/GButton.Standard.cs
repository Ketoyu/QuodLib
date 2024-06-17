using QuodLib.WinForms.Drawing;
using QuodLib.WinForms.Objects.Enums;
using QuodLib.WinForms.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ILGPU.IR.Transformations.CodePlacement;
using static System.Windows.Forms.DataFormats;

namespace QuodLib.WinForms.Objects {
    public partial class GButton {
        public static class Standard {
            public static class Arrow {
                public static IReadOnlyDictionary<Direction, GButton> Orthogonal()
                    => Orthogonal(null, Directions.Orthogonal);

                public static IReadOnlyDictionary<Direction, GButton> Orthogonal(Color foreColor, Color hoverColor, Color pressedColor)
                    => Orthogonal((foreColor, hoverColor, pressedColor), Directions.Orthogonal);

                public static IReadOnlyDictionary<Direction, GButton> Orthogonal((Color Forecolor, Color HoverColor, Color PressedColor)? colors = null, params Direction[] directions)
                    => Orthogonal(colors, (IList<Direction>)directions);

                public static IReadOnlyDictionary<Direction, GButton> Orthogonal((Color Forecolor, Color HoverColor, Color PressedColor)? colors, IList<Direction> directions) {
                    var unsupported = directions.Except(Directions.Orthogonal);
                    if (unsupported.Any())
                        throw new NotSupportedException($"{nameof(directions)}: {string.Join(", ", unsupported)}");

                    Dictionary<Direction, GButton> rtn = new();

                    using Image arrowUp = Resources.ArrowUp_x18;

                    colors ??= (classGraphics.MColor(12), classGraphics.MColor(127), classGraphics.MColor(255));
                    var (foreColor, hoverColor, pressedColor) = ((Color Forecolor, Color HoverColor, Color PressedColor))colors!;

                    foreach (Direction d in Directions.Orthogonal) {
                        if (!directions.Contains(d))
                            continue;

                        Image normal = arrowUp.Tint(foreColor);
                        Image hover = arrowUp.Tint(hoverColor);
                        Image pressed = arrowUp.Tint(pressedColor);

                        if (d != Directions.Orthogonal.First()) {
                            normal.RotateFlip(Directions.DrawRotate[d]);
                            hover.RotateFlip(Directions.DrawRotate[d]);
                            pressed.RotateFlip(Directions.DrawRotate[d]);
                        }

                        rtn.Add(d, new(normal, hover, pressed, 18, 18));
                    }

                    return rtn;
                }

                public static GButton Single(Direction direction) {
                    if (!Directions.Orthogonal.Contains(direction))
                        throw new NotSupportedException($"{nameof(direction)}: {direction}");

                    return Orthogonal(null, direction).Single().Value;
                }
            }

            public static GButton Dot()
                => Dot(classGraphics.MColor(12), classGraphics.MColor(127), classGraphics.MColor(255));
            public static GButton Dot(Color foreColor, Color hoverColor, Color pressedColor) {
                using Image dot = Resources.Dot_x18;

                Image normal = dot.Tint(foreColor);
                Image hover = dot.Tint(hoverColor);
                Image pressed = dot.Tint(pressedColor);

                return new(normal, hover, pressed, 18, 18);
            }
        }
    }
}
