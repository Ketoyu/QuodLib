using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.ML.Foundation {
	public interface ISample {
		public int LayerIndex { get; init; }
		public int NodeThis { get; init; }

		public bool HasGradients { get; init; }
		public bool HasCost { get; init; }
		public double Cost { get; init; }
	}

	public interface IBiasSample : ISample {
		public double Bias { get; init; }
		public double BiasGradient { get; init; }
	}

	public interface IWeightSample : ISample {
		public int NodeIn { get; set; }
		public double Weight { get; set; }
		public double WeightGradient { get; set; }
	}

	internal class FullSample : ISample, IBiasSample, IWeightSample {
		public int LayerIndex { get; init; }
		public int NodeThis { get; init; }

		public bool HasGradients { get; init; }
		public bool HasCost { get; init; }
		public double Cost { get; init; }

		public double Bias { get; init; }
		public double BiasGradient { get; init; }

		public int NodeIn { get; set; }
		public double Weight { get; set; }
		public double WeightGradient { get; set; }
	}
}
