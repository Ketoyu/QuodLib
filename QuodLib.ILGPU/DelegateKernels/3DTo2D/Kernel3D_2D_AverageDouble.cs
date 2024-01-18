using ILGPU;
using ILGPU.Runtime;

namespace QuodLib.ILGPU.DelegateKernels
{
    public sealed class Kernel3D_2D_AverageDouble : Kernel3D_2D<double> {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">indexOfValue</param>
        /// <param name="composite">[indexOfTarget,indexOfValue]</param>
        /// <param name="target">[indexOfValue]</param>
        protected override void Calculate(Index2D index, ArrayView3D<double, Stride3D.DenseZY> composite, ArrayView2D<double, Stride2D.DenseX> target) {
            for (int t = 0; t < composite.Length; t++)
                target[index] += composite[new Index3D(t, index.X, index.Y)];

            target[index] /= composite.Length;
        }
    }
}