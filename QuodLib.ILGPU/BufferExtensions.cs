using ILGPU.Runtime;
using ILGPU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.ILGPU {
    public static class BufferExtensions {
        public static MemoryBuffer1D<T, Stride1D.Dense> AllocateEqual<T>(this MemoryBuffer1D<T, Stride1D.Dense> buffer) where T : unmanaged
            => buffer.Accelerator.Allocate1D<T>(buffer.IntExtent);

        public static MemoryBuffer1D<T, Stride1D.Dense> Clone<T>(this MemoryBuffer1D<T, Stride1D.Dense> buffer) where T : unmanaged {
            var clone = buffer.Accelerator.Allocate1D<T>(buffer.IntExtent);
            buffer.CopyTo(clone);
            return clone;
        }

    }
}
