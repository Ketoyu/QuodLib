using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Objects {
    public class Dirty<TData> : IDirty<TData> {
        private bool _dirty;
        public bool IsDirty {
            get => _dirty;
            set {
                if (_dirty == value)
                    return;

                _dirty = value;
                if (value)
                    Soil?.Invoke(this, EventArgs.Empty);
                else
                    Clean?.Invoke(this, EventArgs.Empty);
            }
        }

        public TData Data { get; init; }

        public event EventHandler? Clean;

        public event EventHandler? Soil;
        public Dirty(TData data) {
            Data = data;
        }
    }
}
