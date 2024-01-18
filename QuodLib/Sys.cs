using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib {
    public static class Sys {
        public static SoundPlayer PlaySound(string filePath) {
            SoundPlayer player = new(filePath);
            player.Play();
            return player;
        }
        public static SoundPlayer PlaySound(Stream sound) {
            SoundPlayer player = new(sound);
            player.Play();
            return player;
        }

    } // </class>
}
