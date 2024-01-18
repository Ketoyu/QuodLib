using ILGPU;
using ILGPU.Runtime;
using QuodLib.ILGPU;
using QuodLib.ILGPU.DelegateKernels._2x1D_1D;
using QuodLib.ILGPU.Functions;
using QuodLib.ML.Foundation.Functions.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.ML.Foundation.Functions {
	public class Activation : Function<double>, IKernel1D_1D<double> {
        private Function1D_1D<double> Primary;
        private Function1D_1D<double> Derivative;

        public Activation(Function1D_1D<double> primary, Function1D_1D<double> derivative) {
            Primary = primary;
            Derivative = derivative;
        }

        public Activation(Delegate1D_1D primary, IterationType iterationPrimary, Kernel1D_1D_Double kernelPrimary,
            Delegate1D_1D derivative, IterationType iterationDerivative, Kernel1D_1D_Double kernelDerivative
        ) {
            Primary = new(x => primary(x), iterationPrimary, d => Main.GetKernel1D_1D(d, kernelPrimary));
            Derivative = new(x => derivative(x), iterationDerivative, d => Main.GetKernel1D_1D(d, kernelDerivative));
        }
        
        public Activation(Delegate1D_1D primary, IterationType iterationPrimary, Kernel1D_1D_Double kernelPrimary, double derivativeConstant) {
            Primary = new(x => primary(x), iterationPrimary, d => Main.GetKernel1D_1D(d, kernelPrimary));
            Derivative = new(derivativeConstant);
		}

        public double[] Calculate(Accelerator device, double[] input)
            => Primary.Calculate(device, input);

        public MemoryBuffer1D<double, Stride1D.Dense> Calculate(MemoryBuffer1D<double, Stride1D.Dense> sizedBuffer)
            => Primary.Calculate(sizedBuffer);

        public double[] CalculateDerivative(Accelerator device, double[] input)
            => Derivative.Calculate(device, input);

        public MemoryBuffer1D<double, Stride1D.Dense> CalculateDerivative(MemoryBuffer1D<double, Stride1D.Dense> sizedBuffer)
            => Derivative.Calculate(sizedBuffer);
    }
}
