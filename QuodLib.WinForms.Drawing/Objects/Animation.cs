using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace QuodLib.Objects
{
	/// <summary>
	/// A wrapper for a sequences of images to be played.
	/// </summary>
    public class Animation
    {
		#region Fields
        private ImageSequence Begin, Loop, End;
        public bool IsPlaying {get; private set;}
		#endregion //Fields
		#region Properties
        public bool IsInBegin {
            get {
                return !Begin.HasEnded;
            }
        }
        public bool IsInLoop {
            get {
                return !(IsInBegin || IsInStop) && IsPlaying;
            }
        }
        public bool IsInStop {
            get {
                return End.HasBegun && !End.HasEnded;
            }
        }
		#endregion //Properties
        public Animation(ImageSequence begin, ImageSequence loop, ImageSequence end)
        {
            Begin = begin;
            Loop = loop;
			Loop.Loop = true;
            End = end;
        }
		#region Methods
        public void Play()
        {
            IsPlaying = true;
        }
        public void Stop()
        {
			Loop.Loop = false;
        }
        public Image Next()
        {
            if (!IsPlaying || End.HasEnded) return null;
            if (IsInBegin) return Begin.Next();
            if (!IsInStop) return (Loop.HasEnded ? End.Next() : Loop.Next());
            return End.Next();
        }
		#endregion //Methods
    }
}
