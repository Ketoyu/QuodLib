using ILGPU.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.ILGPU.DelegateKernels._2x1D_1D {
    public interface IKernel3D_2D<T> where T : unmanaged {
        T[,] Calculate(Accelerator device, T[,,] input);
    }
}
