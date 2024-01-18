using ILGPU.Runtime;
using ILGPU;
using QuodLib.ILGPU;

namespace QuodLib.ML.Foundation.Functions.Standard.Activations
{
    public static class Sigmoid
    {

        internal static double Primary(double weightedInput)
            => 1 / (1 + Math.Exp(-weightedInput));

        private static void GpuPrimary(Index1D index, ArrayView1D<double, Stride1D.Dense> input, ArrayView1D<double, Stride1D.Dense> target) {
            target[index] = 1 / (1 + Math.Exp(-input[index]));
        }

        internal static double Derivative(double input) {
            double s = Primary(input);
            return s * (1 - s);
        }

        private static void GpuDerivative(Index1D index, ArrayView1D<double, Stride1D.Dense> input, ArrayView1D<double, Stride1D.Dense> target) {
            double s = 1 / (1 + Math.Exp(-input[index]));
            target[index] = s * (1 - s);
        }
 
        public static Activation GetActivation()
            => new(
                Primary,
                IterationType.Parallel,
                GpuPrimary,

                Derivative,
                IterationType.Parallel,
                GpuDerivative
            );
    }
}