using ILGPU;
using ILGPU.Runtime;
using QuodLib.ILGPU.Kernels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.ILGPU.Functions
{
    public abstract class AcceleratedFunction<TDelegateNative, TDelegateAccellerated, TValue> 
        where TDelegateNative : Delegate 
        where TDelegateAccellerated : Delegate
        where TValue : unmanaged
    {
        protected TDelegateNative? Function { get; set; }
        protected TValue? Constant { get; set; }
        protected IterationType IterationType { get; set; }
        protected Func<Accelerator, TDelegateAccellerated>? KernelFetcher { get; set; }
        private Accelerator? _currentDevice;
        protected Accelerator? CurrentDevice
        {
            get => _currentDevice;
            private set
            {
                CurrentKernel = null;
                _currentDevice = value;
            }
        }
        protected TDelegateAccellerated? CurrentKernel { get; set; }

        protected void EnsureKernel(Accelerator device) {
            if (KernelFetcher == null)
                throw new InvalidOperationException($"{nameof(KernelFetcher)} is null; could not fetch a kernel to work with!");

            TryEnsureKernel(device);
        }

        /// <summary>
        /// If there is no <see cref="KernelFetcher"/>, quietly doesn't change the <see cref="CurrentKernel"/>.
        /// </summary>
        /// <param name="device"></param>
        protected void TryEnsureKernel(Accelerator device) {
            if (!device.Equals(CurrentDevice)) {
                CurrentDevice = device;
                CurrentKernel = null;
            }

            CurrentKernel ??= KernelFetcher?.Invoke(device);
        }

        private (Accelerator Device, Action<Index1D, TValue, ArrayView1D<TValue, Stride1D.Dense>> Kernel)? AcceleratedConstant1D_K;
        protected MemoryBuffer1D<TValue, Stride1D.Dense> AcceleratedConstant1D(Accelerator device, Index1D length) {
            if (Constant == null)
                throw new InvalidOperationException($"Null {Constant}!");

            if (!(AcceleratedConstant1D_K?.Kernel?.Equals(device) ?? false))
                AcceleratedConstant1D_K = null;

            AcceleratedConstant1D_K ??= new(device, device.LoadAutoGroupedStreamKernel<Index1D, TValue, ArrayView1D<TValue, Stride1D.Dense>>(Matrix.Fill));

            return AcceleratedConstant1D_K?.Kernel.GpuGenerate(device, (TValue)Constant!, length);
        }

        protected AcceleratedFunction(TValue constant) {
            Constant = constant;
            IterationType = IterationType.Sequential;
        }

        protected AcceleratedFunction(TDelegateNative native, IterationType iterationType) {
            Function = native;
            IterationType = iterationType;
        }
        protected AcceleratedFunction(Func<Accelerator, TDelegateAccellerated> kernelFetcher) {
            KernelFetcher = kernelFetcher;
        }

        protected AcceleratedFunction(TDelegateNative native, IterationType iterationType, Func<Accelerator, TDelegateAccellerated> kernelFetcher) {
            Function = native;
            IterationType = iterationType;
            KernelFetcher = kernelFetcher;
        }

        protected AcceleratedFunction(IterationType iterationType) {
            IterationType = iterationType;
        }
    }
}
