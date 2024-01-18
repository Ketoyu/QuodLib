using ILGPU.Runtime;
using ILGPU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.ILGPU.DelegateKernels._2x1D_1D {
    // \/ ---- Someday, ILGPU will support lambda expressions. The below are not supported (Func<> was ILGPU...IsPassedViaPtr() ) ---- \/

    //public class Kernel2x1D_1D<T> where T : unmanaged {
    //    protected Action<Index1D, 
    //        Func<T, T, T>, 
    //        ArrayView1D<T, Stride1D.Dense>, ArrayView1D<T, Stride1D.Dense>, 
    //        ArrayView1D<T, Stride1D.Dense>
    //    > Kernel { get; set; }

    //    public Func<T, T, T> Operation { get; private set; }

    //    public Kernel2x1D_1D(Func<T, T, T> operation) {
    //        Operation = operation;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="device"></param>
    //    /// <param name="x">[indexOf_Value1]</param>
    //    /// <param name="y">[indexOf_Value2]</param>
    //    /// <returns><c>returnValues[i] = <see cref="Operation"/>(x[i], y[i])</c></returns>
    //    public T[] Calculate(Accelerator device, T[] x, T[] y) {
    //        if (x.Length != y.Length)
    //            throw new ArgumentException($"Sizes of {nameof(x)} and {nameof(y)} must match!");

    //        int length = x.Length; //that is, n.
    //        using var gX = device.Allocate1D<T>(length);
    //        using var gY = device.Allocate1D<T>(length);
    //        using var target = device.Allocate1D<T>(length);
    //        gX.CopyFromCPU(x);
    //        gY.CopyFromCPU(y);

    //        Kernel = Extensions.LoadAutoGroupedDelegateStreamKernel<Index1D, T,
    //            ArrayView1D<T, Stride1D.Dense>, ArrayView1D<T, Stride1D.Dense>, ArrayView1D<T, Stride1D.Dense>
    //            >(device, Calculate);
    //        Kernel.Invoke(length, Operation, gX, gY, target);
    //        //device.Synchronize();

    //        return target.GetAsArray1D();
    //    }

    //    private static void Calculate(Index1D index, Func<T, T, T> operation, 
    //        ArrayView1D<T, Stride1D.Dense> x, ArrayView1D<T, Stride1D.Dense> y, ArrayView1D<T, Stride1D.Dense> target) {

    //        target[index] = operation(x[index], y[index]);
    //    }
    //}
}
