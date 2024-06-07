using Timer = System.Windows.Forms.Timer;

namespace QuodLib.WinForms.Screens {

    /// <summary>
    /// Used for managing multiple <see cref="Watcher"/>s feeding multiple <see cref="Target"/>s.
    /// </summary>
    internal class Watchmaster {
        protected Dictionary<string, Watcher> Watchers = new();
        protected Timer Timer;
        public Watchmaster(Timer timer) {
            Timer = timer;
            timer.Tick += Timer_Tick;
        }

        public void Start()
            => Timer.Start();

        public void Stop()
            => Timer.Stop();

        private double _fps;
        /// <summary>
        /// Set the frame-captures per second (fast).
        /// </summary>
        public double FPS {
            get => _fps;
            set => _fps = (Timer.Interval = (int)(1000D / value));
        }

        /// <summary>
        /// Set the seconds between each frame-capture (slow).
        /// </summary>
        public double SecondsPerFrame {
            get => 1 / _fps;
            set => _fps = 1 / (Timer.Interval = (int)(value * 1000D));
        }

        public void Enlist(string key, Watcher watcher) {
            Watchers[key] = watcher;
        }

        public bool TryGetValue(string key, out Watcher? watcher)
            => Watchers.TryGetValue(key, out watcher);

        public Watcher? Discharge(string key) {
            if (Watchers.TryGetValue(key, out Watcher? watcher)) {
                Watchers.Remove(key);
                return watcher;
            }

            return null;
        }

        public delegate void WatcherFrameHandler(string key, Image? frame);
        public event WatcherFrameHandler? WatcherFrame;

        public delegate void WatchmasterFinishedHandler();
        public event WatchmasterFinishedHandler? WatcherFinished;

        private void Timer_Tick(object? sender, EventArgs e) {
            foreach (var pair in Watchers)
                WatcherFrame?.Invoke(pair.Key, pair.Value.Peek());

            WatcherFinished?.Invoke();
        }

    }
}
