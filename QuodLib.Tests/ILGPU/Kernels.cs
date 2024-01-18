using QuodLib.ILGPU;
using QuodLib.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace QuodLib.Tests.ILGPU {
    public static class Kernels {
        // \/ ---- Someday, ILGPU will support lambda expressions. The below are not supported (Func<> was ILGPU...IsPassedViaPtr() ) ---- \/

        //[Fact]
        //public static void DelegateKernel_Success() {
        //    double[] data = {0, 1, 2};
        //    double[] expected = { 1, 2, 3 };
        //    using var device = Extensions.DefaultContext().DefaultDevice();
        //    double[] actual = new Kernel1DTo1D<double>(d => d + 1).Calculate(device, data);

        //    Assert.Equal(expected, actual);
        //}
    }
}
