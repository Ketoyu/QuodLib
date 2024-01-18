using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.CPU;
using ILGPU.Runtime.Cuda;

namespace QuodLib.ILGPU {
    public static class Extensions {

        public static void WriteRGB(this ArrayView1D<byte, Stride1D.Dense> target, int x, int y, int width, byte r, byte g, byte b) {
            int baseIndex = (y * width) + x;
            target[baseIndex] = r;
            target[baseIndex + 1] = g;
            target[baseIndex + 2] = b;
        }

        public static void WriteGrayscale(this ArrayView1D<byte, Stride1D.Dense> target, int x, int y, int width, byte grayscaleValue)
            => target.WriteRGB(x, y, width, grayscaleValue, grayscaleValue, grayscaleValue);

        public static ArrayView1D<T, Stride1D.Dense>[] Allocate2DJagged<T>(this Accelerator device, int[] lengths) where T : unmanaged {
            ArrayView1D<T, Stride1D.Dense>[] result = new ArrayView1D<T, Stride1D.Dense>[lengths.Length];
            Parallel.For(0, lengths.Length, i => result[i] = device.Allocate1D<T>(lengths[i]));
            return result;
        }

        public static Context DefaultContext()
            => Context.Create(b => b.Cuda().CPU().EnableAlgorithms());

        public static Accelerator DefaultDevice(this Context context, bool forceCPU = false)
            => context.GetPreferredDevice(forceCPU).CreateAccelerator(context);

        public static MemoryBuffer1D<T, Stride1D.Dense> GpuAllocate<T>(this T[] array, Accelerator device) where T : unmanaged {
            var index = array.GpuIndex();
            var mem = device.Allocate1D<T>(index);
            mem.CopyFromCPU(array);
            return mem;
        }

        public static MemoryBuffer2D<T, Stride2D.DenseY> GpuAllocateDenseY<T>(this T[,] array, Accelerator device) where T : unmanaged {
            var index = array.GpuIndex();
            var mem = device.Allocate2DDenseY<T>(index);
            mem.CopyFromCPU(array);

            return mem;
        }

        public static MemoryBuffer2D<T, Stride2D.DenseX> GpuAllocateDenseX<T>(this T[,] array, Accelerator device) where T : unmanaged {
            var index = array.GpuIndex();
            var mem = device.Allocate2DDenseX<T>(index);
            mem.CopyFromCPU(array);

            return mem;
        }

        public static MemoryBuffer3D<T, Stride3D.DenseZY> GpuAllocateDenseZY<T>(this T[,,] array, Accelerator device) where T : unmanaged {
            var index = array.GpuIndex();
            var mem = device.Allocate3DDenseZY<T>(index);
            mem.CopyFromCPU(array);

            return mem;
        }

        internal static Index1D GpuIndex<T>(this T[] array) where T : unmanaged
            => new(array.Length);

        internal static Index2D GpuIndex<T>(this T[,] array) where T : unmanaged
            => new(array.GetLength(0), array.GetLength(1));

        internal static Index3D GpuIndex<T>(this T[,,] array) where T : unmanaged
            => new(array.GetLength(0), array.GetLength(1), array.GetLength(2));

        internal static Index1D LowerX(this Index2D index)
            => new(index.X);

        internal static Index1D LowerY(this Index2D index)
            => new(index.Y);

        internal static Index2D LowerYZ(this Index3D index)
            => new(index.Y, index.Z);
    }
}
