using ILGPU.Runtime;
using ILGPU;
using QuodLib.ILGPU.Functions;
using QuodLib.ILGPU;

namespace QuodLib.ML.Foundation.Functions.Standard.Costs {
    public static class Square {
        internal static double Primary(double actual, double expected)
            => Math.Pow(actual - expected, 2);

        internal static double Derivative(double actual, double expected)
            => 2 * (actual - expected);

        internal static void GpuPrimary(Index1D index, ArrayView1D<double, Stride1D.Dense> x, ArrayView1D<double, Stride1D.Dense> y, ArrayView1D<double, Stride1D.Dense> target) {
            target[index] = Math.Pow(x[index] - y[index], 2);
        }

        internal static void GpuDerivative(Index1D index, ArrayView1D<double, Stride1D.Dense> x, ArrayView1D<double, Stride1D.Dense> y, ArrayView1D<double, Stride1D.Dense> target) {
            target[index] = 2 * (x[index] - y[index]);
        }

        public static Cost GetCost()
            => new(
                Primary,
                IterationType.Parallel,
                GpuPrimary,

                Derivative,
                IterationType.Parallel,
                GpuDerivative);
    }
}