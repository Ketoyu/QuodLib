using ILGPU;
using ILGPU.Runtime;
using QuodLib.ILGPU;
using QuodLib.ILGPU.Kernels;
using static QuodLib.ILGPU.BufferExtensions;
using QuodLib.ML.Foundation.Functions;
using QILGPU = QuodLib.ILGPU.Extensions;
using static QuodLib.ML.Foundation.Network;

namespace QuodLib.ML.Foundation {
	public class Layer {
		private Network Parent { get; set; }
		public Activation Activation { get; private set; }
		public Cost Cost { get; private set; }
		public int NodesIn { get; private set; }
		public int Size { get; private set; }
		internal double[,] Weights { get; private set; }
		internal double[] Biases { get; private set; }
		internal MemoryBuffer2D<double, Stride2D.DenseY> G_Weights { get; private set; }
        internal MemoryBuffer1D<double, Stride1D.Dense> G_Biases { get; private set; }

		#region Per batch
		public bool HasCost { get; private set; }
		public bool HasGradients { get; private set; }
		internal double[,] WeightGradients { get; private set; }
		internal double[] BiasGradients { get; private set; }
		#endregion //Per batch

		#region Per data-point
		private MemoryBuffer1D<double, Stride1D.Dense> G_RecentInputs { get; set; }
		private MemoryBuffer1D<double, Stride1D.Dense> G_RecentWeightedInputs { get; set; }
		internal MemoryBuffer1D<double, Stride1D.Dense> G_RecentActivations { get; set; }
		internal double RecentCost { get; private set; }
		#endregion //Per data-point

		internal Layer(Network parent, int nodesIn, int size, Activation activation, Cost cost) {
			Parent = parent;

			NodesIn = nodesIn;
			Size = size;
			Activation = activation;
			Cost = cost;

			Weights = new double[NodesIn, Size];
			Biases = new double[Size];

			RandomizeWeights();
		}

		private Layer(Network parent, int nodesIn, int size, Activation activation, Cost cost, double[,] weights, double[] biases) : this(parent, nodesIn, size, activation, cost) {
			NodesIn = nodesIn;
			Size = size;
			Activation = activation;
			Cost = cost;

			Weights = new double[NodesIn, Size];
			for (int i = 0; i < nodesIn; i++)
				for (int s = 0; s < size; s++)
					Weights[i, s] = weights[i, s];

			Biases = new double[Size];
			Array.Copy(biases, Biases, biases.Length);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="weights"></param>
		/// <param name="nodesIn"></param>
		/// <param name="layerSize"></param>
		/// <returns>ILGPU <i><b>(todo)</b></i></returns>
		internal static double[,] MergeWeights(double[][,] weights, int nodesIn, int layerSize) {
			double[,] newWeights = new double[nodesIn, layerSize];
			for (int w = 0; w < weights.Length; w++)
				for (int i = 0; i < nodesIn; i++)
					for (int s = 0; s < layerSize; s++)
						newWeights[i, s] += weights[w][i, s];

			for (int i = 0; i < nodesIn; i++)
				for (int s = 0; s < layerSize; s++)
					newWeights[i, s] /= layerSize;

			return newWeights;
		}

		internal void Merge(IList<Layer> siblings) {
			Weights = MergeWeights(Parent.Master, siblings);
			Biases = MergeBiases(Parent.Master, siblings);
		}

		private static double[] MergeBiases(NetworkMaster master, IList<Layer> siblings) {
			int biases = siblings[0].Biases.Length;
			double[,] compositeBiases = new double[siblings.Count, biases];
			Parallel.For(0, siblings.Count, s =>
				Parallel.For(0, biases, b => {
					compositeBiases[s, b] = siblings[(int)s].Biases[b];
				})
			);

			return master.AverageTo1D(compositeBiases);
		}

		private static double[,] MergeWeights(NetworkMaster master, IList<Layer> siblings) {
			int weightsX = siblings[0].Weights.GetLength(0), weightsY = siblings[0].Weights.GetLength(1);
			double[,,] compositeWeights = new double[siblings.Count, weightsX, weightsY];
			Parallel.For(0, siblings.Count, s =>
				Parallel.For(0, weightsX, x =>
					Parallel.For(0, weightsY, y => {
						compositeWeights[s, x, y] = siblings[(int)s].Weights[x, y];
					})
				)
			);
			return master.AverageTo2D(compositeWeights);
		}

		public Layer Clone()
			=> new(Parent, NodesIn, Size, Activation, Cost, Weights, Biases);

		public Layer[] Clone(int count) {
			Layer[] layers = new Layer[count];

			Parallel.For(0, count, i => {
				layers[i] = Clone();
			});

			return layers.ToArray();
		}

		internal int ConnectionCount
			=> NodesIn * Size;
		internal void FillConnections_EnsureKernel() {
            Parent.Master.Device = Assert.GetDevice(G_RecentActivations, G_Weights);
            var device = Parent.Master.Device;
            Parent.Master.Layer_GetConnections_K ??= device.LoadAutoGroupedStreamKernel<Index2D, int, bool, int, ArrayView1D<double, Stride1D.Dense>, ArrayView2D<double, Stride2D.DenseY>, ArrayView1D<Connection, Stride1D.Dense>>(FillConnectionsGPU);
        }
        internal void FillConnections(int layerIndex, bool isLast, int startIndex, MemoryBuffer1D<Connection, Stride1D.Dense> pool) {
			Index2D index = new(NodesIn, Size);
			Parent.Master.Layer_GetConnections_K!.Invoke(index, layerIndex, isLast, startIndex, G_RecentActivations, G_Weights, pool);
        }
		private static void FillConnectionsGPU(Index2D index, int layerIndex, bool isLastLayer, int startIndex, ArrayView1D<double, Stride1D.Dense> recentActivations, ArrayView2D<double, Stride2D.DenseY> weights, ArrayView1D<Connection, Stride1D.Dense> target) {
            //X: node in, Y: current node
			
			target[startIndex + (index.X * index.Y)] = new Connection() {
                LayerIn = layerIndex,
                IndexIn = index.Y,
                IndexNode = isLastLayer ? null : index.X,
                Weight = isLastLayer ? null : weights[index.X, index.Y],
                Activation = isLastLayer ? null : recentActivations[index.Y]
            };
		}

		/// <summary>
		/// Compares the expected- vs actual-values for <b><i>one</i></b> data-point across this <see cref="Layer"/>.
		/// </summary>
		/// <param name="actualOutputs"></param>
		/// <param name="expectedOutputs"></param>
		/// <returns>The <b><i>total</i></b> comparison.</returns>
		internal double DataPointCost(MemoryBuffer1D<double, Stride1D.Dense> actualOutputs, MemoryBuffer1D<double, Stride1D.Dense> expectedOutputs) {
			using var results = Parent.Master.Costs(actualOutputs, expectedOutputs);
            double[] costs = results.GetAsArray1D();

			double cost = 0;
			foreach(double c in costs)
				cost += c;

			RecentCost = cost;
			return cost;
		}

		/// <summary>
		/// Processes the <paramref name="inputs"/> <i>(<b>one</b> data-point)</i> into an output of this <see cref="Layer"/>.
		/// </summary>
		/// <param name="inputs"></param>
		/// <returns>The output for this <see cref="Layer"/>.</returns>
		internal MemoryBuffer1D<double, Stride1D.Dense> Evaluate(MemoryBuffer1D<double, Stride1D.Dense> inputs) {
            G_RecentInputs?.Dispose();
            G_RecentInputs = inputs;

            G_RecentWeightedInputs.CopyFrom(G_Biases);
            //Array.Copy(Biases, RecentWeightedInputs, G_Biases.Length);

            //Parallel.For(0, Size, s => {
            //	for (int i = 0; i < NodesIn; i++)
            //		RecentWeightedInputs[s] += inputs[i] * Weights[i, s];
            //});

            G_RecentActivations = Parent.Master.Evaluate(inputs, G_Weights, G_RecentWeightedInputs, Activation); //Dictionary<Activation, kernel>

            //double[] activations = Activation.Calculate(device, RecentWeightedInputs);
            
			//TODO kernel, weightedInputs: target[s] = Sum(inputs[i] * weights[i, s]);
            //TOTO kernel, activations: target[i] = Activation.Primary(weightedInputs[i]);

            //RecentActivations = activations;
			return G_RecentActivations.Clone();
		}

		/// <summary>
		/// <i><b>d</b></i>Activation * <i><b>d</b></i>Cost, for each node.
		/// </summary>
		/// <param name="expectedOutputs"></param>
		/// <returns></returns>
		internal MemoryBuffer1D<double, Stride1D.Dense> GetNodeValues(MemoryBuffer1D<double, Stride1D.Dense> expectedOutputs) {
			_ = Assert.GetDevice(expectedOutputs, G_RecentActivations, G_RecentWeightedInputs);
			MemoryBuffer1D<double, Stride1D.Dense> dActivations = Activation.CalculateDerivative(G_RecentWeightedInputs);
            MemoryBuffer1D<double, Stride1D.Dense> dCosts = Cost.CalculateDerivative(G_RecentActivations, expectedOutputs);

            MemoryBuffer1D<double, Stride1D.Dense> nodeValues = expectedOutputs.AllocateEqual();
			Matrix.MultiplyPaired(dActivations.IntExtent, dActivations, dCosts, nodeValues);

            //Parallel.For(0, nodeValues.Length, i => {
            //	nodeValues[i] = dActivations[i] * dCosts[i];
            //});

            //TODO kernel: either
            // - target[i] = delegate(rwi[i], ra[i], eo[i]);
            // or: {
            //		1. target[i] = Activation.Derivative(rwi[i]);
            //		2. target[i] = Cost.Derivative(ra[i], eo[i]);
            //		3. target[i] = {1: ad}[i] * {2: cd}[i];
            // }

            return nodeValues;
		}

		/// <summary>
		/// <i><b>d</b></i>Activation * <i><b>d</b></i>Cost, for each node.
		/// </summary>
		/// <param name="succesor"></param>
		/// <param name="itsNodeValues"></param>
		/// <returns></returns>
		internal MemoryBuffer1D<double, Stride1D.Dense> GetNodeValues(Accelerator device, Layer succesor, MemoryBuffer1D<double, Stride1D.Dense> itsNodeValues) {
			throw new NotImplementedException();

			var nodeValues = device.Allocate1D<double>(Size);
            MemoryBuffer1D<double, Stride1D.Dense> dActivations = Activation.CalculateDerivative(G_RecentWeightedInputs);

            //Parallel.For(0, nodeValues.Length, s => {
			//	for (int iSuccessor = 0; iSuccessor < itsNodeValues.Length; iSuccessor++)
			//		nodeValues[s] += succesor.Weights[s, iSuccessor] * itsNodeValues[iSuccessor];

			//	nodeValues[s] *= dActivations[s];
			//});

			//TOTO kernel, either:
			// - target[s] = Sum(weights[s, i] * nv[i]) * Activation.Derivative(rwi[s])
			// or {
			//		1. target[s] = Sum(weights[s, i] * nv[i]);
			//		2. target[i] = mv[i] * Activation.Derivative({2}[i])
			// }
			// or {
			//		1. target[s] = Sum(weights[s, i] * nv[i]);
			//		2. target[i] = delegate(mv[i], {2}[i]);
			// }
			// or {
			//		1. var weights_ = MultiplyY(weights, itsNodeValues);
			//		2. var nodeValues = SumZ(weights);
			//		3. return Sum(nodeValues, dActivations);
			// }

			return nodeValues;
		}

		/// <summary>
		/// Adds one data-point's worth of node-values to running weight- and bias-gradient totals.
		/// </summary>
		/// <param name="nodeValues">The node-values computed by this <see cref="Layer"/>.</param>
		/// <remarks><i>Later, you'll need to divide these by the length of the current data-set or -subset.</i></remarks>
		internal void AddGradiants(MemoryBuffer1D<double, Stride1D.Dense> nodeValues) {
			throw new NotImplementedException();

			//for (int s = 0; s < Size; s++) {
			//	for (int i = 0; i < NodesIn; i++)
			//		WeightGradients[i, s] += RecentInputs[i] * nodeValues[s];

			//	BiasGradients[s] += nodeValues[s]; //** (dInput:dBias is just 1 * bias)
			//}

			//TODO kernels: {
			//		1. var mul = MultiplyPerpendicular(RecentInputs, nodeValues);
			//		2. wg = Add(wg, mul);
			//		3; bg = AddPaired(bg, nodeValues);
			// }
		}

		/// <summary>
		/// Uses this <see cref="Layer"/>'s gradients to update its weights and biases, scaled by the <paramref name="learnRate"/>.
		/// </summary>
		/// <param name="learnRate"></param>
		/// <remarks><i>If the weight- and bias-gradients were computed using many data-points, you must divide <paramref name="learnRate"/> by the length of the data-set or -subset before passing it in.</i></remarks>
		internal void ApplyGradients(double learnRate) {
			throw new NotImplementedException();

			//for (int s = 0; s < Size; s++) {
			//	Biases[s] -= BiasGradients[s] * learnRate;
			//	for (int i = 0; i < NodesIn; i++)
			//		Weights[i, s] -= WeightGradients[i, s] * learnRate;
			//}

            // TODO kernels: {
            //		1. var mulB = bg.Scale(learnRate);
            //		2. B = B.Subtract(mulB);
            //		3. var mulW = wg.Scale(learnRate);
            //		4. W = W.Subtract(mulW);
            // }

            HasGradients = true;
		}

		/// <summary>
		/// Sets all weights to a random value.
		/// </summary>
		/// <remarks><i>Used during initialization of this <see cref="Layer"/> within a fresh <see cref="Network"/>.</i></remarks>
		private void RandomizeWeights() {
			Random rand = new();

			Parallel.For(0, NodesIn, i => 
				Parallel.For(0, Size, s =>
					Weights[i, s] = (rand.NextDouble() * 2 - 1) / Math.Sqrt(NodesIn)
				)
			);

			//(?) TODO attempt kernel, if it can use Random.
		}

		/// <summary>
		/// Resets all weight- and bias-gradients to zero, to prepare for a new batch of data-points.
		/// </summary>
		internal void ClearGradientsAndCost() {
			HasGradients = false;
			HasCost = false;
			RecentCost = 0;

			for (int s = 0; s < Size; s++) {
				for (int i = 0; i < NodesIn; i++)
					WeightGradients[i, s] = 0;

				BiasGradients[s] = 0;
			}

			//TODO (?) just = new[] and = new[,]
		}

	}
}