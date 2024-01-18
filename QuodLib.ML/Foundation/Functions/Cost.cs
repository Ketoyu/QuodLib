using ILGPU.Runtime;
using ILGPU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuodLib.ILGPU.Functions;
using QuodLib.ILGPU;
using QuodLib.ML.Foundation.Functions.Standard.Costs;
using QuodLib.ML.Foundation.Functions.Standard;
using QuodLib.ILGPU.DelegateKernels._2x1D_1D;

namespace QuodLib.ML.Foundation.Functions {
	public class Cost : Function<double>, IKernel2x1D_1D<double> {
		public Function2x1D_1D<double> Primary;
		public Function2x1D_1D<double> Derivative;

        public Cost(Function2x1D_1D<double> primary, Function2x1D_1D<double> derivative) {
            Primary = primary;
            Derivative = derivative;
        }

        public Cost(Delegate2x1D_1D primary, IterationType iterationPrimary, Kernel2x1D_1D_Double kernelPrimary,
            Delegate2x1D_1D derivative, IterationType iterationDerivative, Kernel2x1D_1D_Double kernelDerivative
        ) {
            Primary = new((x, y) => primary(x, y), iterationPrimary, d => Main.GetKernel2x1D_1D(d, kernelPrimary));
            Derivative = new((x, y) => derivative(x, y), iterationDerivative, d => Main.GetKernel2x1D_1D(d, kernelDerivative));
        }

        public Cost(Delegate2x1D_1D primary, IterationType iterationPrimary, Kernel2x1D_1D_Double kernelPrimary, double derivativeConstant) {
            Primary = new((x, y) => primary(x, y), iterationPrimary, d => Main.GetKernel2x1D_1D(d, kernelPrimary));
            Derivative = new(derivativeConstant);
        }

        public double[] Calculate(Accelerator device, double[] x, double[] y)
            => Primary.Calculate(device, x, y);

        public MemoryBuffer1D<double, Stride1D.Dense> Calculate(MemoryBuffer1D<double, Stride1D.Dense> x, MemoryBuffer1D<double, Stride1D.Dense> y)
            => Primary.Calculate(x, y);

        public double[] CalculateDerivative(Accelerator device, double[] x, double[] y)
            => Derivative.Calculate(device, x, y);

        public MemoryBuffer1D<double, Stride1D.Dense> CalculateDerivative(MemoryBuffer1D<double, Stride1D.Dense> x, MemoryBuffer1D<double, Stride1D.Dense> y)
            => Derivative.Calculate(x, y);
    }
}
