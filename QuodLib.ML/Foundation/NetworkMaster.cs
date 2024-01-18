using ILGPU;
using ILGPU.Runtime;
using QuodLib.ILGPU;
using QuodLib.ILGPU.DelegateKernels;
using QuodLib.ILGPU.Kernels;
using QuodLib.ML.Foundation.Functions;
using QuodLib.ML.Foundation.Functions.Standard;
using static QuodLib.ML.Foundation.Network;

namespace QuodLib.ML.Foundation {
	public class NetworkMaster {
        private Accelerator _device;
        internal Accelerator Device {
			get => _device;
			set
			{
				if (_device.Equals(value))
					return;

				_device = value;
                Multiply_1Dx2D_K = null;
				Add_1Dx1D_K = null;
                Layer_GetConnections_K = null;

            }
		}
		internal Action<Index2D, int, bool, int, ArrayView1D<double, Stride1D.Dense>, ArrayView2D<double, Stride2D.DenseY>, ArrayView1D<Connection, Stride1D.Dense>>?
			Layer_GetConnections_K { get; set; }
		private Kernel2D_1D_AverageDouble AverageTo1D_K { get; set; }
		private Kernel3D_2D_AverageDouble AverageTo2D_K { get; set; }
		private Activation Activation { get; set; }
		private Cost Cost { get; set; }
		private Kernel1D_2DY_1D_Double? Multiply_1Dx2D_K { get; set; }
        private Kernel1D_1D_1D_Double? Add_1Dx1D_K { get; set; }


        public NetworkMaster(Accelerator device, Activation activation, Cost cost) {
			this.Activation = activation;
			this.Cost = cost;
			this._device = device;
		}

		internal double[] AverageTo1D(double[,] compositeBiases) {
			AverageTo1D_K ??= new();
			return AverageTo1D_K.Calculate(Device, compositeBiases);
		}

		internal double[,] AverageTo2D(double[,,] compositeWeights) {
			AverageTo2D_K ??= new();
			return AverageTo2D_K.Calculate(Device, compositeWeights);
		}

		internal double[] Activations(double[] weightedInputs)
			=> Activation.Calculate(Device, weightedInputs);

		internal double[] Costs(double[] actualOutputs, double[] expectedOutputs)
			=> Cost.Calculate(Device, expectedOutputs, actualOutputs);

        internal MemoryBuffer1D<double, Stride1D.Dense> Costs(MemoryBuffer1D<double, Stride1D.Dense> actualOutputs, MemoryBuffer1D<double, Stride1D.Dense> expectedOutputs)
            => Cost.Calculate(expectedOutputs, actualOutputs);
 
        internal MemoryBuffer1D<double, Stride1D.Dense> Evaluate(MemoryBuffer1D<double, Stride1D.Dense> inputs, MemoryBuffer2D<double, Stride2D.DenseY> weights, MemoryBuffer1D<double, Stride1D.Dense> recentWeightedInputs, Activation activation) {
            Device = Assert.GetDevice(inputs, weights, recentWeightedInputs);
			Multiply_1Dx2D_K ??= Main.GetKernel1D_2DY_1D(Device, Matrix.Multiply_1n_np);
			Multiply_1Dx2D_K!.Invoke(inputs.Extent.ToIntIndex(), inputs, weights, recentWeightedInputs);
			return activation.Calculate(recentWeightedInputs);
        }
    }
}