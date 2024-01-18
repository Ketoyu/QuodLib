using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuodLib {
    public static class Sys {
        public static NotifyIcon CreateNotif(Icon? icon = null, EventHandler? onClick = null, EventHandler? onTipClick = null, ContextMenuStrip? menu = null, EventHandler? onShow = null, EventHandler? onClosed = null) {
            NotifyIcon notif = new() {
                Icon = icon
            };

            if (menu != null)
                notif.ContextMenuStrip = menu;

            if (onClick != null)
                notif.Click += onClick;

            if (onTipClick != null)
                notif.BalloonTipClicked += onTipClick;

            if (onShow != null)
                notif.BalloonTipShown += onShow;

            if (onClosed != null)
                notif.BalloonTipClosed += onClosed;

            return notif;
        }
        public static NotifyIcon WithSound(this NotifyIcon notif, string filePath)
            => notif.WithSound(new SoundPlayer(filePath));

        public static NotifyIcon WithSound(this NotifyIcon notif, Stream sound)
            => notif.WithSound(new SoundPlayer(sound));

        private static NotifyIcon WithSound(this NotifyIcon notif, SoundPlayer player) {
            notif.BalloonTipShown +=
                (_, _) => player.Play();

            notif.BalloonTipClosed += 
                (_, _) => player.Stop();

            return notif;
        }

    } // </class>
}
