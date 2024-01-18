using ILGPU;
using ILGPU.Runtime;
using QuodLib.ILGPU;
using QuodLib.ILGPU.Functions;

namespace QuodLib.ML.Foundation.Functions.Standard.Activations {
    public static class Step {
        internal static double Primary(double input)
            => input > 0 ? 1 : 0;

        public static Activation GetActivation()
            => new(GetPrimary(), GetDerivative());

        public static Function1D_1D<double> GetPrimary()
            => new Function1D_1D<double>(Primary, IterationType.Sequential);

        public static Function1D_1D<double> GetDerivative()
            => new Function1D_1D<double>(0);
    }
}