using QuodLib.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinTimer = System.Windows.Forms.Timer;

namespace QuodLib.WinForms.Objects.Puppeteers
{
    public class CustomNotifTray<F> : PuppetMaster<CustomNotifTray<F>.Notif> where F : Form
    {
        public class Notif
        {
            public F Form { get; internal set; }
            public int Index { get; internal set; }
            public Notif(F form, int index)
            {
                Form = form;
                Index = index;
            }
        }

        private Point _anchor;
        public Point Anchor
        {
            get => _anchor;
            set
            {
                _anchor = value;
                Shuffle();
            }
        }

        private bool _descending;
        public bool Descending
        {
            get => _descending;
            set
            {
                _descending = value;
                Shuffle();
            }
        }

        protected int IncrementLoc
            => Descending ? 1 : -1;

        public bool FadeIn { get; set; } = true;
        //public bool FadeOut { get; set; } // TODO (use in Paint() and Dismiss())
        private const int MOVE_FPS = 60;
        private const int MOVE_DURATION = 2;

        protected WinTimer Timer = new()
        {
            Interval = 1000 / MOVE_FPS
        };

        public CustomNotifTray() : base()
        {
            Timer.Tick +=
                (_, _) => Paint();
        }

        private void Paint()
        {
            Notif current = Puppets.Last();
            if (IsIn(current))
            {
                Timer.Stop();
                return;
            }

            int max_nudge = current.Form.Location.X - NewLocation.X;
            int nudge = current.Form.Width / MOVE_DURATION / MOVE_FPS;
            if (nudge > max_nudge)
                nudge = max_nudge;
            Point oldLoc = current.Form.Location;
            current.Form.Location = new(oldLoc.X - nudge, oldLoc.Y - current.Form.Height);
        }

        public void Show(F puppet)
        {
            Notif notif = new(puppet, Puppets.Count);
            puppet.FormClosed +=
                (_, _) => Dismiss(notif);
            base.Possess(new Notif(puppet, Puppets.Count));

            notif.Form.Location = NewLocation;

            if (FadeIn && Timer.Enabled)
                Timer.Start();
        }

        protected void Dismiss(Notif notif)
        {
            int removed_i = notif.Index;
            int y_init = Puppets.Take(removed_i).Sum(n => n.Form.Height); //Take([0] .. [removed - 1])
            base.Release(notif);
            Shuffle(removed_i, y_init);
        }

        protected void Shuffle(int startIndex = 0, int y_init = 0)
        {
            for (int i = startIndex; i < Puppets.Count; i++)
            {
                Notif puppet = Puppets[i];
                puppet.Index = i;
                Form frm = puppet.Form;
                y_init += IncrementLoc * frm.Height;
                frm.Location = new(frm.Location.X, y_init);
            }
        }

        //TODO: https://www.youtube.com/watch?v=QTWKUkiEqpQ
        protected Point NewLocation
            => new(Anchor.X, Anchor.Y + IncrementLoc * Puppets.Sum(p => p.Form.Height));

        /// <summary>
        /// Whether the <paramref name="notif"/> is finished appearing.
        /// </summary>
        /// <param name="notif"></param>
        /// <returns></returns>
        protected bool IsIn(Notif notif)
            => notif.Form.Location.X <= Anchor.X - notif.Form.Width;

        /// <summary>
        /// Whether the <paramref name="notif"/> is finished hiding.
        /// </summary>
        /// <param name="notif"></param>
        /// <returns></returns>
        protected bool IsOut(Notif notif)
            => notif.Form.Location.X >= Anchor.X;
    }
}
