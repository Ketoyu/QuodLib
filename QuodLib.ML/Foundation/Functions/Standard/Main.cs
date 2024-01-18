global using KernelConstant_1D_Double = System.Action<ILGPU.Index1D,
    double, 
    ILGPU.Runtime.ArrayView1D<double, ILGPU.Stride1D.Dense>>;

global using Kernel1D_1D_Double = System.Action<ILGPU.Index1D,
    ILGPU.Runtime.ArrayView1D<double, ILGPU.Stride1D.Dense>,
    ILGPU.Runtime.ArrayView1D<double, ILGPU.Stride1D.Dense>>;

global using Kernel1D_1D_1D_Double = System.Action<ILGPU.Index1D, 
    ILGPU.Runtime.ArrayView1D<double, ILGPU.Stride1D.Dense>,
    ILGPU.Runtime.ArrayView1D<double, ILGPU.Stride1D.Dense>,
    ILGPU.Runtime.ArrayView1D<double, ILGPU.Stride1D.Dense>>;

global using Kernel2x1D_1D_Double = System.Action<ILGPU.Index1D, 
    ILGPU.Runtime.ArrayView1D<double, ILGPU.Stride1D.Dense>,
    ILGPU.Runtime.ArrayView1D<double, ILGPU.Stride1D.Dense>,
    ILGPU.Runtime.ArrayView1D<double, ILGPU.Stride1D.Dense>>;

global using Kernel2DX_1D_1D_Double = System.Action<ILGPU.Index1D,
    ILGPU.Runtime.ArrayView2D<double, ILGPU.Stride2D.DenseX>,
    ILGPU.Runtime.ArrayView1D<double, ILGPU.Stride1D.Dense>,
    ILGPU.Runtime.ArrayView1D<double, ILGPU.Stride1D.Dense>>;

global using Kernel1D_2DY_1D_Double = System.Action<ILGPU.Index1D,
    ILGPU.Runtime.ArrayView1D<double, ILGPU.Stride1D.Dense>,
    ILGPU.Runtime.ArrayView2D<double, ILGPU.Stride2D.DenseY>,
    ILGPU.Runtime.ArrayView1D<double, ILGPU.Stride1D.Dense>>;

global using Kernel1D_2DX_1D_Double = System.Action<ILGPU.Index1D,
    ILGPU.Runtime.ArrayView1D<double, ILGPU.Stride1D.Dense>,
    ILGPU.Runtime.ArrayView2D<double, ILGPU.Stride2D.DenseX>,
    ILGPU.Runtime.ArrayView1D<double, ILGPU.Stride1D.Dense>>;

using ILGPU.Runtime;
using ILGPU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.ML.Foundation.Functions.Standard
{
	public static class Main
	{
        //TODO:	 SiLU => wi / (1 + exp(-wi));

        //public delegate void KernelDelegate1D_1D_Double(Index1D index, ArrayView1D<double, Stride1D.Dense> input, ArrayView1D<double, Stride1D.Dense> target);
        internal static KernelConstant_1D_Double GetKernel1D_1D(Accelerator device, KernelConstant_1D_Double method)
            => device.LoadAutoGroupedStreamKernel(method);

        internal static Kernel1D_1D_Double GetKernel1D_1D(Accelerator device, Kernel1D_1D_Double method)
            => device.LoadAutoGroupedStreamKernel(method);

        internal static Kernel1D_1D_1D_Double GetKernel1D_1D_1D(Accelerator device, Kernel1D_1D_1D_Double method)
            => device.LoadAutoGroupedStreamKernel(method);

        internal static Kernel2x1D_1D_Double GetKernel2x1D_1D(Accelerator device, Kernel2x1D_1D_Double method)
            => device.LoadAutoGroupedStreamKernel(method);

        internal static Kernel1D_2DY_1D_Double GetKernel1D_2DY_1D(Accelerator device, Kernel1D_2DY_1D_Double method)
            => device.LoadAutoGroupedStreamKernel(method);

        internal static Kernel1D_2DX_1D_Double GetKernel1D_2DX_1D(Accelerator device, Kernel1D_2DX_1D_Double method)
            => device.LoadAutoGroupedStreamKernel(method);
    }
}
