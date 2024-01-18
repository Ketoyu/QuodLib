using ILGPU;
using ILGPU.Runtime;

namespace QuodLib.ILGPU.DelegateKernels
{
    public sealed class Kernel2D_1D_AverageDouble : Kernel2D_1D<double> {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">indexOfValue</param>
        /// <param name="composite">[indexOfTarget,indexOfValue]</param>
        /// <param name="target">[indexOfValue]</param>
        protected override void Calculate(Index1D index, ArrayView2D<double, Stride2D.DenseY> composite, ArrayView1D<double, Stride1D.Dense> target)
        {
            for (int t = 0; t < composite.Length; t++)
                target[index] += composite[t, index];

            target[index] /= composite.Length;
        }
    }
}