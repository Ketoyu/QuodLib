using ILGPU;
using ILGPU.Runtime;

namespace QuodLib.ILGPU {
    public static class KernelExtensions {
        // Constant to 1D
        public static T[] Generate<T>(this Action<Index1D,
                T, ArrayView1D<T, Stride1D.Dense>
            > kernel, Accelerator device, T input, int length)
            where T : unmanaged {

            using var result = kernel.GpuGenerate(device, input, length);
            return result.GetAsArray1D();
        }

        public static MemoryBuffer1D<T, Stride1D.Dense> GpuGenerate<T>(this Action<Index1D,
                T, ArrayView1D<T, Stride1D.Dense>
            > kernel, Accelerator device, T input, int length)
            where T : unmanaged {

            var target = device.Allocate1D<T>(length);

            kernel.Invoke(target.IntExtent, input, target);

            return target;
        }

        public static void Fill<T>(this Action<Index1D,
                T, ArrayView1D<T, Stride1D.Dense>
            > kernel, T input, MemoryBuffer1D<T, Stride1D.Dense> target)
            where T : unmanaged {

            kernel.Invoke(target.IntExtent, input, target);
        }

        // 1D to 1D
        public static T[] Calculate<T>(this Action<Index1D, 
                ArrayView1D<T, Stride1D.Dense>, ArrayView1D<T, Stride1D.Dense>
            > kernel, Accelerator device, T[] input) 
            where T : unmanaged {

            using var mem = input.GpuAllocate(device);
            using var result = kernel.Calculate(mem);
            return result.GetAsArray1D();
        }

        public static MemoryBuffer1D<T, Stride1D.Dense> Calculate<T>(this Action<Index1D, 
                ArrayView1D<T, Stride1D.Dense>, ArrayView1D<T, Stride1D.Dense>
            > kernel, MemoryBuffer1D<T, Stride1D.Dense> input) 
            where T : unmanaged {

            var target = input.AllocateEqual();

            kernel.Invoke(input.IntExtent, input, target);

            return target;
        }

        // 2x1D to 1D
        public static T[] Calculate<T>(this Action<Index1D, 
                ArrayView1D<T, Stride1D.Dense>, ArrayView1D<T, Stride1D.Dense>, ArrayView1D<T, Stride1D.Dense>
            > kernel, Accelerator device, T[] x, T[] y) 
            where T : unmanaged {

            using var gX = x.GpuAllocate(device);
            using var gY = y.GpuAllocate(device);
            using var result = kernel.Calculate(gX, gY);
            return result.GetAsArray1D();
        }

        public static MemoryBuffer1D<T, Stride1D.Dense> Calculate<T>(this Action<Index1D, 
                ArrayView1D<T, Stride1D.Dense>, ArrayView1D<T, Stride1D.Dense>, ArrayView1D<T, Stride1D.Dense>
            > kernel, MemoryBuffer1D<T, Stride1D.Dense> x, MemoryBuffer1D<T, Stride1D.Dense> y) 
            where T : unmanaged {

            if (!x.Accelerator.Equals(y.Accelerator))
                throw new ArgumentException("Non-matching devices for x vs y!");

            var target = x.AllocateEqual();

            kernel.Invoke(x.IntExtent, x, y, target);

            return target;
        }

        // { 1D, 2D } to 1D

        //      DenseX
        public static T[] Calculate<T>(this Action<Index1D, 
                ArrayView1D<T, Stride1D.Dense>, ArrayView2D<T, Stride2D.DenseX>, ArrayView1D<T, Stride1D.Dense>
            > kernel, Accelerator device, T[] b, T[,] a) 
            where T : unmanaged {

            using var mem_b = b.GpuAllocate(device);
            using var mem_a = a.GpuAllocateDenseX(device);
            using var result = kernel.Calculate(device, mem_b, mem_a);
            return result.GetAsArray1D();
        }

        public static MemoryBuffer1D<T, Stride1D.Dense> Calculate<T>(this Action<Index1D, 
                ArrayView1D<T, Stride1D.Dense>, ArrayView2D<T, Stride2D.DenseX>, ArrayView1D<T, Stride1D.Dense>
            > kernel, Accelerator device, MemoryBuffer1D<T, Stride1D.Dense> b, MemoryBuffer2D<T, Stride2D.DenseX> a) 
            where T : unmanaged {

            var index1D = b.IntExtent;
            var target = device.Allocate1D<T>(index1D);

            kernel.Invoke(index1D, b, a, target);

            return target;
        }

        //      DenseY
        public static T[] Calculate<T>(this Action<Index1D,
                ArrayView1D<T, Stride1D.Dense>, ArrayView2D<T, Stride2D.DenseY>, ArrayView1D<T, Stride1D.Dense>
            > kernel, Accelerator device, T[] b, T[,] a)
            where T : unmanaged {

            using var mem_b = b.GpuAllocate(device);
            using var mem_a = a.GpuAllocateDenseY(device);
            using var result = kernel.Calculate(device, mem_b, mem_a);
            return result.GetAsArray1D();
        }

        public static MemoryBuffer1D<T, Stride1D.Dense> Calculate<T>(this Action<Index1D,
                ArrayView1D<T, Stride1D.Dense>, ArrayView2D<T, Stride2D.DenseY>, ArrayView1D<T, Stride1D.Dense>
            > kernel, Accelerator device, MemoryBuffer1D<T, Stride1D.Dense> b, MemoryBuffer2D<T, Stride2D.DenseY> a)
            where T : unmanaged {

            var index1D = b.IntExtent;
            var target = device.Allocate1D<T>(index1D);

            kernel.Invoke(index1D, b, a, target);

            return target;
        }

        // { 2D, 1D } to 1D

        //      DenseX
        public static T[] Calculate<T>(this Action<Index1D,
                ArrayView2D<T, Stride2D.DenseX>, ArrayView1D<T, Stride1D.Dense>, ArrayView1D<T, Stride1D.Dense>
            > kernel, Accelerator device, T[,] b, T[] a)
            where T : unmanaged {

            using var mem_b = b.GpuAllocateDenseX(device);
            using var mem_a = a.GpuAllocate(device);
            using var result = kernel.Calculate(device, mem_b, mem_a);
            return result.GetAsArray1D();
        }

        public static MemoryBuffer1D<T, Stride1D.Dense> Calculate<T>(this Action<Index1D,
                ArrayView2D<T, Stride2D.DenseX>, ArrayView1D<T, Stride1D.Dense>, ArrayView1D<T, Stride1D.Dense>
            > kernel, Accelerator device, MemoryBuffer2D<T, Stride2D.DenseX> b, MemoryBuffer1D<T, Stride1D.Dense> a)
            where T : unmanaged {

            var index1D = a.IntExtent;
            var target = device.Allocate1D<T>(index1D);

            kernel.Invoke(index1D, b, a, target);

            return target;
        }

        //      DenseY
        public static T[] Calculate<T>(this Action<Index1D, 
                ArrayView2D<T, Stride2D.DenseY>, ArrayView1D<T, Stride1D.Dense>, ArrayView1D<T, Stride1D.Dense>
            > kernel, Accelerator device, T[,] b, T[] a) 
            where T : unmanaged {

            using var mem_b = b.GpuAllocateDenseY(device);
            using var mem_a = a.GpuAllocate(device);
            using var result = kernel.Calculate(device, mem_b, mem_a);
            return result.GetAsArray1D();
        }

        public static MemoryBuffer1D<T, Stride1D.Dense> Calculate<T>(this Action<Index1D, 
                ArrayView2D<T, Stride2D.DenseY>, ArrayView1D<T, Stride1D.Dense>, ArrayView1D<T, Stride1D.Dense>
            > kernel, Accelerator device, MemoryBuffer2D<T, Stride2D.DenseY> b, MemoryBuffer1D<T, Stride1D.Dense> a) 
            where T : unmanaged {

            var index1D = a.IntExtent;
            var target = device.Allocate1D<T>(index1D);

            kernel.Invoke(index1D, b, a, target);

            return target;
        }

        // 2D to 1D
        public static T[] Calculate<T>(this Action<Index1D, 
                ArrayView2D<T, Stride2D.DenseY>, ArrayView1D<T, Stride1D.Dense>
            > kernel, Accelerator device, T[,] input) 
            where T : unmanaged {

            using var mem = input.GpuAllocateDenseY(device);
            using var result = kernel.Calculate(device, mem);
            return result.GetAsArray1D();
        }

        public static MemoryBuffer1D<T, Stride1D.Dense> Calculate<T>(this Action<Index1D, 
                ArrayView2D<T, Stride2D.DenseY>, ArrayView1D<T, Stride1D.Dense>
            > kernel, Accelerator device, MemoryBuffer2D<T, Stride2D.DenseY> input) 
            where T : unmanaged {

            var index1D = input.IntExtent.LowerY();
            var target = device.Allocate1D<T>(index1D);

            kernel.Invoke(index1D, input, target);

            return target;
        }

        // 3D to 2D
        public static T[,] Calculate<T>(this Action<Index2D, 
                ArrayView3D<T, Stride3D.DenseZY>, ArrayView2D<T, Stride2D.DenseX>
            > kernel, Accelerator device, T[,,] input) 
            where T : unmanaged {

            using var mem = input.GpuAllocateDenseZY(device);
            using var result = kernel.Calculate(device, mem);
            return result.GetAsArray2D();
        }

        public static MemoryBuffer2D<T, Stride2D.DenseX> Calculate<T>(this Action<Index2D, 
                ArrayView3D<T, Stride3D.DenseZY>, ArrayView2D<T, Stride2D.DenseX>
            > kernel, Accelerator device, MemoryBuffer3D<T, Stride3D.DenseZY> input) 
            where T : unmanaged {

            var index2D = input.IntExtent.LowerYZ();
            var target = device.Allocate2DDenseX<T>(index2D);

            kernel.Invoke(index2D, input, target);

            return target;
        }

        // \/ ---- Someday, ILGPU will support lambda expressions. The below are not supported (Func<> was ILGPU...IsPassedViaPtr() ) ---- \/

        ///// <summary>
        ///// Loads the given implicitly grouped kernel and returns a launcher delegate
        ///// that uses the default accelerator stream.
        ///// </summary>
        ///// <typeparam name="TIndex">The index type.</typeparam>
        ///// <typeparam name="TO">The type for operating parameter 1 and parameter 2 into parameter 3.</typeparam>
        ///// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        ///// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        ///// <typeparam name="T3">Parameter type of parameter 3.</typeparam>

        ///// <param name="accelerator">The current accelerator.</param>
        ///// <param name="action">The action to compile into a kernel.</param>
        ///// <returns>The loaded kernel-launcher delegate.</returns>
        //public static Action<TIndex, Func<TO, TO, TO>, T1, T2, T3> LoadAutoGroupedDelegateStreamKernel<TIndex, TO, T1, T2, T3>(
        //    this Accelerator accelerator,
        //    Action<TIndex, Func<TO, TO, TO>, T1, T2, T3> action)
        //    where TIndex : struct, IIndex
        //    where TO : struct
        //    where T1 : struct where T2 : struct where T3 : struct {

        //    var baseKernel = accelerator.LoadAutoGroupedDelegateKernel<TIndex, TO, T1, T2, T3>(action);
        //    return (TIndex index, Func<TO, TO, TO> operation, T1 param1, T2 param2, T3 param3) =>
        //        baseKernel(accelerator.DefaultStream, index, operation, param1, param2, param3);
        //}

        ///// <summary>
        ///// Loads the given implicitly grouped kernel and returns a launcher delegate
        ///// that uses the default accelerator stream.
        ///// </summary>
        ///// <typeparam name="TIndex">The index type.</typeparam>
        ///// <typeparam name="TO">The type for operating parameter 1 into parameter 2.</typeparam>
        ///// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        ///// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        ///// <typeparam name="T3">Parameter type of parameter 3.</typeparam>

        ///// <param name="accelerator">The current accelerator.</param>
        ///// <param name="action">The action to compile into a kernel.</param>
        ///// <returns>The loaded kernel-launcher delegate.</returns>
        //public static Action<TIndex, Func<TO, TO>, T1, T2> LoadAutoGroupedDelegateStreamKernel<TIndex, TO, T1, T2>(
        //    this Accelerator accelerator,
        //    Action<TIndex, Func<TO, TO>, T1, T2> action)
        //    where TIndex : struct, IIndex
        //    where TO : struct
        //    where T1 : struct where T2 : struct {

        //    var baseKernel = accelerator.LoadAutoGroupedDelegateKernel<TIndex, TO, T1, T2>(action);
        //    return (TIndex index, Func<TO, TO> operation, T1 param1, T2 param2) =>
        //        baseKernel(accelerator.DefaultStream, index, operation, param1, param2);
        //}

        ///// <summary>
        ///// Loads the given implicitly grouped kernel and returns a launcher delegate that 
        ///// can receive arbitrary accelerator streams (first parameter).
        ///// </summary>
        ///// <typeparam name="TIndex">The index type.</typeparam>
        ///// <typeparam name="TO">The type for operating parameter 1 and parameter 2 into parameter 3.</typeparam>
        ///// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        ///// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        ///// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        ///// <param name="accelerator">The current accelerator.</param>
        ///// <param name="action">The action to compile into a kernel.</param>
        ///// <returns>The loaded kernel-launcher delegate.</returns>
        ///// <exception cref="ArgumentNullException"></exception>
        //public static Action<AcceleratorStream, TIndex, Func<TO, TO, TO>, T1, T2, T3> LoadAutoGroupedDelegateKernel<TIndex, TO, T1, T2, T3>(
        //    this Accelerator accelerator, Action<TIndex, Func<TO, TO, TO>, T1, T2, T3> action)
        //    where TIndex : struct, IIndex 
        //    where TO : struct 
        //    where T1 : struct where T2 : struct where T3 : struct {

        //    if (action == null)
        //        throw new ArgumentNullException(nameof(action));


        //    return accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, Func<TO, TO, TO>, T1, T2, T3>>(action.Method);
        //}

        ///// <summary>
        ///// Loads the given implicitly grouped kernel and returns a launcher delegate that 
        ///// can receive arbitrary accelerator streams (first parameter).
        ///// </summary>
        ///// <typeparam name="TIndex">The index type.</typeparam>
        ///// <typeparam name="TO">The type for operating parameter 1 into parameter 2.</typeparam>
        ///// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        ///// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        ///// <param name="accelerator">The current accelerator.</param>
        ///// <param name="action">The action to compile into a kernel.</param>
        ///// <returns>The loaded kernel-launcher delegate.</returns>
        ///// <exception cref="ArgumentNullException"></exception>
        //public static Action<AcceleratorStream, TIndex, Func<TO, TO>, T1, T2> LoadAutoGroupedDelegateKernel<TIndex, TO, T1, T2>(
        //    this Accelerator accelerator, Action<TIndex, Func<TO, TO>, T1, T2> action)
        //    where TIndex : struct, IIndex
        //    where TO : struct
        //    where T1 : struct where T2 : struct {

        //    if (action == null)
        //        throw new ArgumentNullException(nameof(action));

        //    return accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, Func<TO, TO>, T1, T2>>(action.Method);
        //}
    }
}