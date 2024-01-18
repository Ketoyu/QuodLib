using ILGPU.Runtime;
using ILGPU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.ILGPU.DelegateKernels._1D_1D {
    // \/ ---- Someday, ILGPU will support lambda expressions. The below are not supported (Func<> was ILGPU...IsPassedViaPtr() ) ---- \/

    
    
    //public class Kernel1D_1D<T> where T : unmanaged {
    //    protected Action<Index1D, 
    //        Func<T, T>, 
    //        ArrayView1D<T, Stride1D.Dense>, ArrayView1D<T, Stride1D.Dense>
    //    > Kernel { get; set; }

    //    public Func<T, T> Operation { get; private set; }

    //    public Kernel1D_1D(Func<T, T> operation) {
    //        Operation = operation;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="device"></param>
    //    /// <param name="input">[indexOf_Value1]</param>
    //    /// <param name="y">[indexOf_Value2]</param>
    //    /// <returns><c>returnValues[i] = <see cref="Operation"/>(x[i], y[i])</c></returns>
    //    public T[] Calculate(Accelerator device, T[] input) {
    //        Kernel ??= Extensions.LoadAutoGroupedDelegateStreamKernel<Index1D, T,
    //            ArrayView1D<T, Stride1D.Dense>, ArrayView1D<T, Stride1D.Dense>
    //            >(device, Calculate);
    //        return Kernel.Invoke(length, Operation, gInput, target);
    //    }

    //    private static void Calculate(Index1D index, Func<T, T> operation, 
    //        ArrayView1D<T, Stride1D.Dense> input, ArrayView1D<T, Stride1D.Dense> target) {

    //        target[index] = operation(input[index]);
    //    }
    //}
}
