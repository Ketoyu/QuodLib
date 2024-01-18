using ILGPU;
using ILGPU.Runtime;
using QuodLib.ILGPU.DelegateKernels._2x1D_1D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.ILGPU.Functions {
    public class Function1D_1D<TValue> : AcceleratedFunction<
        Func<TValue, TValue>,
        Action<Index1D, ArrayView1D<TValue, Stride1D.Dense>, ArrayView1D<TValue, Stride1D.Dense>>,
        TValue
        >, IKernel1D_1D<TValue>
        where TValue : unmanaged {

        public Function1D_1D(TValue constant) : base(constant) { }
        public Function1D_1D(Func<TValue, TValue> function, IterationType iterationType) : base(function, iterationType) { }
        public Function1D_1D(Func<Accelerator, Action<Index1D, ArrayView1D<TValue, Stride1D.Dense>, ArrayView1D<TValue, Stride1D.Dense>>> kernelFetcher) : base(kernelFetcher) { }
        public Function1D_1D(Func<TValue, TValue> function, IterationType iterationType, Func<Accelerator, Action<Index1D, ArrayView1D<TValue, Stride1D.Dense>, ArrayView1D<TValue, Stride1D.Dense>>> kernelFetcher) : base(function, iterationType, kernelFetcher) { }
        private Function1D_1D(IterationType iterationType) : base(iterationType) { }

        public MemoryBuffer1D<TValue, Stride1D.Dense> Calculate(MemoryBuffer1D<TValue, Stride1D.Dense> input) {
            //Accellerated
            base.EnsureKernel(input.Accelerator);
            return base.CurrentKernel!.Calculate(input);
        }

        public TValue[] Calculate(Accelerator device, TValue[] input) {
            if (IterationType == IterationType.Passthrough)
                return input;

            //Constant value
            if (base.Constant != null) {
                TValue[] result = new TValue[input.Length];
                for (int i = 0; i < result.Length; i++)
                    result[i] = (TValue)base.Constant!;

                return result;
            }

            //Accellerated
            base.TryEnsureKernel(device);
            if (base.CurrentKernel != null)
                return base.CurrentKernel!.Calculate(base.CurrentDevice!, input);

            //Native
            TValue[] resultN = new TValue[input.Length];
            switch (IterationType) {
                case IterationType.Copy:
                    Array.Copy(input, resultN, input.Length);
                    break;

                case IterationType.Sequential:
                    for (int i = 0; i < resultN.Length; i++)
                        resultN[i] = base.Function!.Invoke(input[i]);
                    break;

                case IterationType.Parallel:
                    Parallel.For(0, resultN.Length, i =>
                        resultN[i] = base.Function!.Invoke(input[i])
                    );
                    break;
            }

            return resultN;
        }

        public MemoryBuffer1D<TValue, Stride1D.Dense> Calculate(Accelerator device, MemoryBuffer1D<TValue, Stride1D.Dense> input) {
            //Constant value
            if (base.Constant != null)
                return base.AcceleratedConstant1D(device, input.IntExtent);

            //Accellerated
            base.EnsureKernel(device);
            if (base.CurrentKernel == null)
                throw new InvalidOperationException($"Could not fetch {nameof(CurrentKernel)}!");

            return base.CurrentKernel!.Calculate(input);
        }

        public static Function1D_1D<TValue> Passthrough() {
            return new Function1D_1D<TValue>(IterationType.Passthrough);
        }

        public static Function1D_1D<TValue> Cloned() {
            return new Function1D_1D<TValue>(IterationType.Copy);
        }
    }
}
