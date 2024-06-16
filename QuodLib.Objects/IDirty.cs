using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Objects {
    public interface IDirty<TData> {
        bool IsDirty { get; }
        TData Data { get; }
        event EventHandler? Soil;
        event EventHandler? Clean;
    }
}
