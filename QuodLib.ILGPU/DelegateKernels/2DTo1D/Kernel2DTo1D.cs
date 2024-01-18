using ILGPU;
using ILGPU.Runtime;

namespace QuodLib.ILGPU.DelegateKernels {
    public abstract class Kernel2D_1D<T> where T : unmanaged {
        protected Action<Index1D, ArrayView2D<T, Stride2D.DenseY>, ArrayView1D<T, Stride1D.Dense>> Kernel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="composite">[indexOfTarget,indexOfValue]</param>
        /// <returns></returns>
        public T[] Calculate(Accelerator device, T[,] composite) {
            Kernel ??= device.LoadAutoGroupedStreamKernel<Index1D, ArrayView2D<T, Stride2D.DenseY>, ArrayView1D<T, Stride1D.Dense>>(Calculate);
            return Kernel!.Calculate(device, composite);
        }

        protected abstract void Calculate(Index1D index, ArrayView2D<T, Stride2D.DenseY> composite, ArrayView1D<T, Stride1D.Dense> target);
    }
}