using ILGPU.IR;
using QuodLib.Math;
using QuodLib.Objects;
using QuodLib.WinForms.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static QuodLib.Objects.Area;
using PointType = QuodLib.Objects.Area.PointType;

using Resources = QuodLib.WinForms.Properties.Resources;

namespace QuodLib.WinForms.Objects {
    public class CAreaDefine : CHoverable {

        protected enum Direction {
            Up, Right, Down, Left
        }

        readonly IReadOnlyDictionary<Direction, RotateFlipType> DrawRotate =
            new Dictionary<Direction, RotateFlipType> {
                { Direction.Up, RotateFlipType.RotateNoneFlipNone },
                { Direction.Right, RotateFlipType.Rotate90FlipNone },
                { Direction.Down, RotateFlipType.RotateNoneFlipY },
                { Direction.Left, RotateFlipType.Rotate90FlipX }
            };

        public class PointChangedEventArgs : EventArgs {
            public PointType PointType { get; init; }
            public Point OldValue { get; init; }
        }

        public Color Backcolor { get; set; } = classGraphics.MColor(12);
        public Color Forecolor { get; set; } = classGraphics.MColor(127);
        public Color HoverColor { get; init; } = classGraphics.MColor(255);
        public Color PressedColor { get; init; } = classGraphics.MColor(63);

        /// <summary>
        /// The lcoation of this container, relative to its host container.
        /// </summary>
        public override Point Location {
            get => base.Location;
            set {
                base.Location = value;
                UpdateChildLocations();
            }
        }

        /// <summary>
        /// The location of the container hosting [this] object.
        /// </summary>
        public override Point ContainerLocation {
            get => base.ContainerLocation;
            set {
                base.ContainerLocation = value;
                UpdateChildLocations();
            }
        }

        /// <summary>
        /// Error-correcting calibration reference-point.
        /// </summary>
        public override Point MouseOffset {
            get => base.MouseOffset;
            set {
                base.MouseOffset = value;
                UpdateChildLocations();
            }
        }

        private bool _calibrate;
        public bool Calibrate {
            get => _calibrate;
            set {
                foreach (var btn in Controls)
                    btn.Calibrate = value;

                _calibrate = value;
            }
        }

        void UpdateChildLocations() {
            foreach (var btn in Controls) {
                btn.ContainerLocation = Location
                    .Add(ContainerLocation);

                btn.MouseOffset = MouseOffset;
            }
        }

        public Area Area { get; set; } = new();

        /// <summary>
		/// Whether <see cref="Size"/> has positive dimensions.
		/// </summary>
		public bool Valid
            => Area.Valid;

        protected GButton[,] ButtonsArrow { get; init; }

        protected GButton[] ButtonsDot { get; init; }

        public PointType? FocusedPoint { get; protected set; }

        public delegate void PointChangedHandler(object? sender, PointChangedEventArgs args);
        public event PointChangedHandler? Changed;

        public override bool IsContainer => true;
        public bool IsDirty => State == MouseState.Dirty;

        public CAreaDefine() {
            base.Width = 120;
            base.Height = 100;

            Brush f = Forecolor.ToBrush();
            Brush h = HoverColor.ToBrush();
            Brush p = PressedColor.ToBrush();

            using Image arrowUp = Resources.ArrowUp_x18;

            ButtonsArrow = new GButton[2, 4];

            for (int i = 0; i < 4; i++) {
                Direction d = (Direction)i;

                Image normal = arrowUp.Tint(Forecolor);
                Image hover = arrowUp.Tint(HoverColor);
                Image pressed = arrowUp.Tint(PressedColor);

                if (i > 0) {
                    normal.RotateFlip(DrawRotate[d]);
                    hover.RotateFlip(DrawRotate[d]);
                    pressed.RotateFlip(DrawRotate[d]);
                }

                GButton btn;
                ButtonsArrow[(int)PointType.Origin, i] = btn = new(normal, hover, pressed, 18, 18) {
                    ContainerLocation = this.Location,
                    Name = $"btnOrigin-{d}"
                };
                btn.StateChange += dirty;
                btn.MouseUp += () => {
                    FocusedPoint = PointType.Origin;
                    State = MouseState.Dirty;
                    Shift(PointType.Origin, d);
                };

                ButtonsArrow[(int)PointType.Extent, i] = btn = new(normal, hover, pressed, 18, 18) {
                    ContainerLocation = this.Location,
                    Name = $"btnExtent-{d}"
                };
                btn.StateChange += dirty;
                btn.MouseUp += () => {
                    FocusedPoint = PointType.Extent;
                    State = MouseState.Dirty;
                    Shift(PointType.Extent, d);
                };
            }

            using (Image dot = Resources.Dot_x18) {
                Image normal = dot.Tint(Forecolor);
                Image hover = dot.Tint(HoverColor);
                Image pressed = dot.Tint(PressedColor);

                ButtonsDot = new GButton[2];

                GButton btn;
                ButtonsDot[(int)PointType.Origin] = btn = new(normal, hover, pressed, 18, 18) {
                    ContainerLocation = this.Location,
                    Name = $"btnOrigin-Dot"
                };
                btn.StateChange += dirty;
                btn.MouseUp += () => {
                    FocusedPoint = PointType.Origin;
                    State = MouseState.Dirty;
                };

                ButtonsDot[(int)PointType.Extent] = btn = new(normal, hover, pressed, 18, 18) {
                    ContainerLocation = this.Location,
                    Name = $"btnExtent-Dot"
                };
                btn.StateChange += dirty;
                btn.MouseUp += () => {
                    FocusedPoint = PointType.Extent;
                    State = MouseState.Dirty;
                };
            }

            //origin [up, right, down, left, dot]
            ButtonsArrow[0, 0].Location = new Point(18, 0);
            ButtonsArrow[0, 1].Location = new Point(33, 51);
            ButtonsArrow[0, 2].Location = new Point(51, 33);
            ButtonsArrow[0, 3].Location = new Point(0, 18);
            ButtonsDot[0].Location = new Point(18, 18);

            //extent [up, tight, down, left, dot]
            ButtonsArrow[1, 0].Location = new Point(51, 69);
            ButtonsArrow[1, 1].Location = new Point(102, 84);
            ButtonsArrow[1, 2].Location = new Point(84, 102);
            ButtonsArrow[1, 3].Location = new Point(69, 51);
            ButtonsDot[1].Location = new Point(84, 84);
            
            Image = new Bitmap(120, 120);

            void dirty() {
                if (State != MouseState.Dirty)
                   State = MouseState.Dirty;
            }
        }

        public override void Redraw() {
            Image?.Dispose();

            Image = new Bitmap(120, 120);
            using Graphics g = Graphics.FromImage(Image);

            g.FillRectangle(Backcolor.ToBrush(), 0, 0, 120, 120);

            using (Image border = Resources.FlexibleBounds_x68.Tint(Forecolor)) {
                g.DrawImage(border, 26, 26);
            }

            GButton btn;
            for (int typ = 0; typ < 2; typ++) {
                for (int d = 0; d < 4; d++) {
                    btn = ButtonsArrow[typ, d];
                    g.DrawImage(btn.Image, btn.Location);
                }

                btn = ButtonsDot[typ];
                g.DrawImage(btn.Image, btn.Location);
            }

            State = MouseState.Clean;
        }

        public override void OnMouseMove() {
            foreach (var btn in Controls)
                btn.OnMouseMove();

            base.OnMouseMove();
        }

        public override void OnMouseDown() {
            foreach (var btn in Controls)
                btn.OnMouseDown();

            base.OnMouseDown();
        }

        public override void OnMouseUp() {
            foreach (var btn in Controls)
                btn.OnMouseUp();

            base.OnMouseUp();
            if (Calibrate) {
                string name = Name.Length > 0
                    ? name = $"for '{Name}'"
                    : string.Empty;

                System.Diagnostics.Debug.WriteLine($"Control-relative MouseClick {name} registered at ({MousePosition.X}, {MousePosition.Y}). "
                    + "\tIs " + (IsHovered ? "" : " not") + " hovered.");
            }
        }

        protected IEnumerable<GButton> Controls {
            get {
                for (int typ = 0; typ < 2; typ++) {
                    for (int d = 0; d < 4; d++) {
                        yield return ButtonsArrow[typ, d];
                    }

                    yield return ButtonsDot[typ];
                }
            }
        }

        protected void Shift(PointType pointType, Direction direction) {
            Point current = pointType == PointType.Origin ? Area.Origin : Area.Extent;
            Point old = current;

            switch (direction) {
                case Direction.Up:
                    current.Y--;
                    break;

                case Direction.Right:
                    current.X++;
                    break;

                case Direction.Down:
                    current.Y++;
                    break;

                case Direction.Left:
                    current.X--;
                    break;
            }

            if (pointType == PointType.Origin)
                Area.Origin = current;
            else
                Area.Extent = current;

            Changed?.Invoke(this, new() {
                PointType = pointType,
                OldValue = old
            });
        }

        public void KeyUp(Keys key) {
            if (FocusedPoint == null)
                return;

            PointType pointType = (PointType)FocusedPoint!;

            switch (key) {
                case Keys.Up:
                    Shift(pointType, Direction.Up);
                    break;

                case Keys.Right:
                    Shift(pointType, Direction.Right);
                    break;

                case Keys.Down:
                    Shift(pointType, Direction.Down);
                    break;

                case Keys.Left:
                    Shift(pointType, Direction.Left);
                    break;

                case Keys.Space: case Keys.Enter:
                    Point old;
                    if (FocusedPoint == PointType.Origin) {
                        old = Area.Origin;
                        Area.Origin = GlobalMousePosition;
                    } else {
                        old = Area.Extent;
                        Area.Extent = GlobalMousePosition;
                    }

                    Changed?.Invoke(this, new() {
                        PointType = pointType,
                        OldValue = old
                    });

                    break;
            }
        }

    }
}
