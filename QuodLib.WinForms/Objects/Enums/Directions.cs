using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.WinForms.Objects.Enums
{
    public static class Directions
    {
        public static readonly IList<Direction> Orthogonal = new[] {
            Direction.Up,
            Direction.Right,
            Direction.Down,
            Direction.Left
        };

        public static readonly IList<Direction> Diagonal = new[] {
            Direction.UpRight,
            Direction.DownRight,
            Direction.DownLeft,
            Direction.UpLeft
        };

        internal static readonly IReadOnlyDictionary<Direction, RotateFlipType> DrawRotate =
            new Dictionary<Direction, RotateFlipType> {
                { Direction.Up, RotateFlipType.RotateNoneFlipNone },
                { Direction.Right, RotateFlipType.Rotate90FlipNone },
                { Direction.Down, RotateFlipType.RotateNoneFlipY },
                { Direction.Left, RotateFlipType.Rotate90FlipX }
            };
    }
}
