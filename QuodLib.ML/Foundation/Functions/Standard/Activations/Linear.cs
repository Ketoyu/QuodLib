using ILGPU.Runtime;
using ILGPU;
using QuodLib.ILGPU.Functions;

namespace QuodLib.ML.Foundation.Functions.Standard.Activations
{
    public static class Linear
    {
        public static Activation GetActivation()
            => new(Function1D_1D<double>.Passthrough(), new Function1D_1D<double>(1));
        //TODO:	 HyperbolicTangent { e2w = Exp(2 * wi); return (e2w - 1) / (e2w + 1) }
    }
}