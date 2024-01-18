using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.ILGPU {
    public enum IterationType {
        Sequential,
        Parallel,
        Copy,
        Passthrough
    }
}
