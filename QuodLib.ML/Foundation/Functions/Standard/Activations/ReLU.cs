using ILGPU.Runtime;
using ILGPU;
using QuodLib.ILGPU;
using QuodLib.ILGPU.Functions;

namespace QuodLib.ML.Foundation.Functions.Standard.Activations
{
    public static class ReLU
    {
        internal static double Primary(double input)
            => Math.Max(0, input);

        private static void GpuPrimary(Index1D index, ArrayView1D<double, Stride1D.Dense> input, ArrayView1D<double, Stride1D.Dense> target) {
            target[index] = Math.Max(0, input[index]);
        }

        public static Activation GetActivation()
            => new(GetPrimary(), GetDerivative());

        public static Function1D_1D<double> GetPrimary()
            => new(Primary, IterationType.Sequential, d => Main.GetKernel1D_1D(d, GpuPrimary));

        public static Function1D_1D<double> GetDerivative()
            => Step.GetPrimary();
   }
}