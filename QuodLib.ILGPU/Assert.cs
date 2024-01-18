using ILGPU;
using ILGPU.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.ILGPU {
    public static class Assert {
        public static Accelerator GetDevice(params AcceleratorObject[] buffers) {
            Accelerator device = buffers[0].Accelerator;
            if (buffers.Any(b => !device.Equals(b.Accelerator)))
                throw new ArgumentException("Differing accellerators!", nameof(buffers));

            return device;
        }
    }
}
