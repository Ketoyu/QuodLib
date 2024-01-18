using ILGPU.Runtime;
using ILGPU;

namespace QuodLib.ILGPU.DelegateKernels {
    public abstract class Kernel3D_2D<T> where T : unmanaged {
        protected Action<Index2D, ArrayView3D<T, Stride3D.DenseZY>, ArrayView2D<T, Stride2D.DenseX>>? Kernel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="composite">[indexOfTarget,indexOfValue]</param>
        /// <returns></returns>
        public T[,] Calculate(Accelerator device, T[,,] composite) {
            Kernel ??= device.LoadAutoGroupedStreamKernel<Index2D, ArrayView3D<T, Stride3D.DenseZY>, ArrayView2D<T, Stride2D.DenseX>>(Calculate);

            return Kernel!.Calculate(device, composite);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">indexOfValue</param>
        /// <param name="composite">[indexOfTarget,indexOfValue]</param>
        /// <param name="target">[indexOfValue]</param>
        protected abstract void Calculate(Index2D index, ArrayView3D<T, Stride3D.DenseZY> composite, ArrayView2D<T, Stride2D.DenseX> target);
    }
}