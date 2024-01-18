using ILGPU;
using ILGPU.Runtime;
using QuodLib.ILGPU.DelegateKernels._2x1D_1D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.ILGPU.Functions {
    public class Function2x1D_1D<TValue> : AcceleratedFunction<
        Func<TValue, TValue, TValue>,
        Action<Index1D, ArrayView1D<TValue, Stride1D.Dense>, ArrayView1D<TValue, Stride1D.Dense>, ArrayView1D<TValue, Stride1D.Dense>>,
        TValue
        >, IKernel2x1D_1D<TValue> where TValue : unmanaged {

        public Function2x1D_1D(TValue constant) : base(constant) { }
        public Function2x1D_1D(Func<TValue, TValue, TValue> function, IterationType iterationType) : base(function, iterationType) { }
        public Function2x1D_1D(Func<Accelerator, Action<Index1D, ArrayView1D<TValue, Stride1D.Dense>, ArrayView1D<TValue, Stride1D.Dense>, ArrayView1D<TValue, Stride1D.Dense>>> kernelFetcher) : base(kernelFetcher) { }
        public Function2x1D_1D(Func<TValue, TValue, TValue> function, IterationType iterationType, Func<Accelerator, Action<Index1D, ArrayView1D<TValue, Stride1D.Dense>, ArrayView1D<TValue, Stride1D.Dense>, ArrayView1D<TValue, Stride1D.Dense>>> kernelFetcher) : base(function, iterationType, kernelFetcher) { }

        public TValue[] Calculate(Accelerator device, TValue[] x, TValue[] y) {
            //Constant value
            if (base.Constant != null) {
                TValue[] result = new TValue[x.Length];
                for (int i = 0; i < result.Length; i++)
                    result[i] = (TValue)base.Constant!;

                return result;
            }

            //Accellerated
            base.EnsureKernel(device);
            if (base.CurrentKernel != null)
                return base.CurrentKernel!.Calculate(base.CurrentDevice!, x, y);

            //Native
            TValue[] resultN = new TValue[x.Length];
            Parallel.For(0, resultN.Length, i =>
                resultN[i] = base.Function!.Invoke(x[i], y[i])
            );

            return resultN;
        }

        public MemoryBuffer1D<TValue, Stride1D.Dense> Calculate(MemoryBuffer1D<TValue, Stride1D.Dense> x, MemoryBuffer1D<TValue, Stride1D.Dense> y) {
            var device = Assert.GetDevice(x, y);
            
            //Constant value
            if (base.Constant != null)
                return base.AcceleratedConstant1D(device, x.IntExtent);

            //Accellerated
            base.EnsureKernel(device);
            if (base.CurrentKernel == null)
                throw new InvalidOperationException($"Could not fetch {nameof(CurrentKernel)}!");

            return base.CurrentKernel!.Calculate(x, y);
        }

    }
}
