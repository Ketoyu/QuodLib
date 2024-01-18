using ILGPU.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.ILGPU.DelegateKernels._2x1D_1D {
    public interface IKernel2x1D_1D<T> where T : unmanaged {
        T[] Calculate(Accelerator device, T[] x, T[] y);
    }
}
