using ILGPU;
using ILGPU.Runtime;
using QuodLib.ILGPU;
using QuodLib.Linq;
using QuodLib.ML.Foundation.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.ML.Foundation {
	public class Network {
		public struct Connection {
			public int LayerIn { get; init; }
			public int IndexIn { get; init; }
			public int? IndexNode { get; init; }
			public double? Weight { get; init; }
			public double? Activation { get; init; }
		}

		internal NetworkMaster Master { get; private set; }

		public IList<Layer> Layers { get; private set; }

		public Network(Activation activation, Cost cost, params int[] layerSizes) {
			Layers = new Layer[layerSizes.Length - 1];
			Parallel.For(0, layerSizes.Length, i => {
				Layers[i] = new Layer(this, layerSizes[i],
					i == layerSizes.Length - 1
						? 0 
						: layerSizes[i + 1],
					activation, cost
				);
			});

			
		}

		private Network(Layer[] layers) {
			Layers = layers;
		}

		/// <summary>
		/// Compares the <see cref="DataPoint.ExpectedOutputs"/> vs actual-outputs for <i><b>one</b></i> <paramref name="dataPoint"/> across the output <see cref="Layer"/>.
		/// </summary>
		/// <param name="dataPoint"></param>
		/// <returns>The <b><i>total</i></b> comparison.</returns>
		public double Cost(Accelerator device, DataPoint dataPoint) {
            Layer outputLayer = Layers.Last();
            using var actual = Evaluate(device, dataPoint.Inputs);
			using var expected = dataPoint.ExpectedOutputs.GpuAllocate(device);
			return outputLayer.DataPointCost(actual, expected);
		}

		/// <summary>
		/// Sends the <paramref name="inputs"/> through this <see cref="Network"/>.
		/// </summary>
		/// <param name="inputs"></param>
		/// <returns>The final output-set of this <see cref="Network"/>.</returns>
		public MemoryBuffer1D<double, Stride1D.Dense> Evaluate(Accelerator device, double[] inputs) {
			var gInputs = inputs.GpuAllocate(device);

			return Evaluate(gInputs);
		}

        /// <summary>
        /// Sends the <paramref name="inputs"/> through this <see cref="Network"/>.
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns>The final output-set of this <see cref="Network"/>.</returns>
        public MemoryBuffer1D<double, Stride1D.Dense> Evaluate(MemoryBuffer1D<double, Stride1D.Dense> inputs) {
            foreach (Layer l in Layers)
                inputs = l.Evaluate(inputs);

            return inputs;
        }

        /// <summary>
        /// Compares the <see cref="DataPoint.ExpectedOutputs"/> vs actual-outputs for <i><b>all</b></i> <paramref name="dataPoints"/> across the output <see cref="Layer"/>.
        /// </summary>
        /// <param name="dataPoints"></param>
        /// <returns>The <b><i>average</i></b> comparison.</returns>
        public double AverageCost(Accelerator device, IList<DataPoint> dataPoints) {
			double[] costs = new double[dataPoints.Count];
			for (int i = 0; i < dataPoints.Count; i++)
				costs[i] = Cost(device, dataPoints[i]);

			return costs.Sum() / dataPoints.Count;
		}

		/// <summary>
		/// Locates the strongest output.
		/// </summary>
		/// <param name="inputs"></param>
		/// <returns>The index, within the output-<see cref="Layer"/>, of the node with the highest output.</returns>
		public int Classify(Accelerator device, double[] inputs)
			=> Evaluate(device, inputs)
				.GetAsArray1D()
				.IndexOfMax();

		public void Learn(Accelerator device, DataPoint[] trainingBatch, double learnRate) {
			ClearGradients(); //Fresh trainingBatch.

			foreach (DataPoint d in trainingBatch)
				AddGradients(device, d);

			ApplyGradients(learnRate / trainingBatch.Length);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="weight">(layerIndex, hasGradient, gradient, nodeIn, node, weight <c>=&gt;</c> newWeight)</param>
		/// <param name="bias">(layerIndex, hasGradient, gradient, node, bias <c>=&gt;</c> newBias)</param>
		public void Shake(Func<IWeightSample, double> weight, Func<IBiasSample, double> bias) {
			Parallel.For(0, Layers.Count, li => {
				Layer l = Layers[li];

				Parallel.For(0, l.Size, s => {
					FullSample sample = new() {
						 LayerIndex = li,
						 NodeThis = s,
						 Bias = l.Biases[s],
						 HasGradients = l.HasGradients,
						 HasCost = l.HasCost,
						 Cost = l.RecentCost,
						 BiasGradient = l.BiasGradients[s]
					};

					for (int i = 0; i < l.NodesIn; i++) {
						sample.Weight = l.Weights[i, s];
						sample.WeightGradient = l.WeightGradients[i, s];

						l.Weights[i, s] = weight(sample);
					}

					l.Biases[s] = bias(sample);
				});
			});
		}

		public Connection[] AllConnections()
		{
			int[] indexes = new int[Layers.Count];
			int offset = 0;

			for (int i = 0; i < indexes.Length; i++) {
				indexes[i] = offset;
				offset += Layers[i].ConnectionCount;
			}

            //each layer
            //return (0..Layers.Count)
            //  .AsEnumerable().ToArray()
            //  .AsParallel()
            //	.SelectMany(li => {
            //		bool isLast = li == Layers.Count - 1;

            //		int idx = (isLast ? Layers[li - 1].Size : Layers[li].Size) - 1;

            //                 Layer l = isLast ? Layers[li - 1] : Layers[li];

            //		//each node (TODO gpu-accelerate FROM HERE DOWN)
            //		return (0 .. idx)
            //			.AsEnumerable().ToArray()
            //			.AsParallel()
            //			.SelectMany(ni => {

            //				double[] recentActivations = l.G_RecentActivations.GetAsArray1D();

            //				//each weight to that node
            //				return (0 .. (l.NodesIn - 1))
            //					.AsEnumerable().ToArray()
            //					.AsParallel()
            //					.Select(nii => 
            //						new Connection() {
            //							LayerIn = li,
            //							IndexIn = nii,
            //							IndexNode = isLast ? null : ni,
            //							Weight = isLast ? null : l.Weights[nii, ni],
            //							Activation = isLast ? null : recentActivations[ni]
            //					});
            //			});
            //	})
            //	.ToArray();

            Layers[0].FillConnections_EnsureKernel();
            using var connections = Master.Device.Allocate1D<Connection>(offset);
			Parallel.For(0, indexes.Length, i =>
				Layers[i].FillConnections(i, i == Layers.Count - 1, indexes[i], connections)
			);
			return connections.GetAsArray1D();
		}

		public Network Clone() {
			Layer[] layers = new Layer[Layers.Count];
			for (int i = 0; i < Layers.Count; i++)
				layers[i] = Layers[i].Clone();

			return new Network(layers);
		}

		public Network[] Clone(int count) {
			Network[] networks = new Network[count];

			Parallel.For(0, count, i => {
				networks[i] = Clone();
			});

			return networks;
		}

		private void AddGradients(Accelerator device, DataPoint dataPoint) {
			Evaluate(device, dataPoint.Inputs); //This'll cause the layers to store the intermediate ("RecentXXXX") calculations.

			Layer outLayer = Layers.Last();
			var expectedOutputs = dataPoint.ExpectedOutputs.GpuAllocate(device);
            var nodeValues = outLayer.GetNodeValues(expectedOutputs);
			outLayer.AddGradiants(nodeValues);

			for (int i = Layers.Count - 2; i > 0; i--) {
				nodeValues = Layers[i].GetNodeValues(device, Layers[i+1], nodeValues);
				Layers[i].AddGradiants(nodeValues);
			}
		}

		private void ApplyGradients(double learnRate) {
			foreach (Layer l in Layers)
				l.ApplyGradients(learnRate);
		}
		private void ClearGradients() {
			foreach (Layer l in Layers)
				l.ClearGradientsAndCost();
		}
	}
}
