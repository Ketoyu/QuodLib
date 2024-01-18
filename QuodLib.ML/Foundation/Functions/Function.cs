using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.ML.Foundation.Functions {
	public abstract class Function<TValue> where TValue : unmanaged {
        public delegate double Delegate2x1D_1D(TValue x, TValue y);
        public delegate double Delegate1D_1D(TValue input);
    }
}
