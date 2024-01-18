using ILGPU;
using ILGPU.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.ILGPU.Kernels {
    public class Matrix {
        //TODO make instance
        //TODO make Interface or base-abstract: _device, Device { get; set; }

        public static void MultiplyPaired(Index1D index, ArrayView1D<double, Stride1D.Dense> x, ArrayView1D<double, Stride1D.Dense> y, ArrayView1D<double, Stride1D.Dense> target) {
            target[index] = x[index] * y[index];
        }

        public static void AddPaired(Index1D index, ArrayView1D<double, Stride1D.Dense> x, ArrayView1D<double, Stride1D.Dense> y, ArrayView1D<double, Stride1D.Dense> target) {
            target[index] = x[index] + y[index];
        }

        public static void AugmentPaired(Index1D index, ArrayView1D<double, Stride1D.Dense> target, ArrayView1D<double, Stride1D.Dense> amounts) {
            target[index] += amounts[index];
        }

        public static void Subtract(Index1D index, ArrayView1D<double, Stride1D.Dense> target, double scalar) {
            target[index] -= scalar;
        }

        public static void Add(Index1D index, ArrayView1D<double, Stride1D.Dense> target, double scalar) {
            target[index] += scalar;
        }

        public static void Multiply(Index1D index, ArrayView1D<double, Stride1D.Dense> target, double scalar) {
            target[index] *= scalar;
        }

        public static void Divide(Index1D index, ArrayView1D<double, Stride1D.Dense> target, double scalar) {
            target[index] /= scalar;
        }

        public static void Fill<T>(Index1D index, T value, ArrayView1D<T, Stride1D.Dense> target) where T : unmanaged {
            target[index] = value;
        }

        /// <summary>
        /// Matrix multiply, <b>B</b> * <b>A</b> = <b>T</b> where <b>B</b> is size <i>1</i> x <i>n</i>, <b>A</b> is <i>n</i> x <i>p</i>, <b>T</b> is size <i>1</i> x <i>p</i>.
        /// </summary>
        /// <param name="t_n">A range of length <i>n</i>.</param>
        /// <param name="b_1n"><b>B</b>: A horizontal matrix B[n] of <i>n</i> columns.</param>
        /// <param name="a_np"><b>A</b>: A 2D matrix[n,p] of <i>n</i> rows and <i>p</i> columns.</param>
        /// <param name="target_1n"><b>T</b>: A horizontal matrix T[n] of <i>n</i> columns.</param>
        /// <remarks>Matrix notation: 
        ///     <list type="bullet">
        ///         <item>an "<i>m</i> x <i>n</i>" 2D matrix is <i>m</i> rows and <i>n</i> columns</item>
        ///         <item>a <i>1</i> x <i>n</i> matrix is horizontal</item>
        ///         <item>an <i>m</i> x <i>1</i> matrix is vertical</item>
        ///     </list>
        /// </remarks>
        public static void Multiply_1n_np(Index1D t_n, ArrayView1D<double, Stride1D.Dense> b_1n, ArrayView2D<double, Stride2D.DenseY> a_np, ArrayView1D<double, Stride1D.Dense> target_1n) {
            for (int k = 0; k < t_n.Size; k++)
                target_1n[t_n] += b_1n[k] * a_np[k, t_n];
        }

        /// <summary>
        /// Matrix multiply, <b>B</b> * <b>A</b> = <b>T</b> where <b>B</b> is size <i>1</i> x <i>n</i>, <b>A</b> is <i>n</i> x <i>p</i>, <b>T</b> is size <i>1</i> x <i>p</i>.
        /// </summary>
        /// <param name="t_n">A range of length <i>m</i>.</param>
        /// <param name="b_mn"><b>B</b>: A 2D matrix B[m,n] of <i>m</i> rows and <i>n</i> columns.</param>
        /// <param name="a_n1"><b>A</b>: A vertical matrix A[n] of <i>n</i> rows.</param>
        /// <param name="target_n1"><b>T</b>: A vertical matrix T[n] of <i>n</i> rows.</param>
        /// <remarks>Matrix notation: 
        ///     <list type="bullet">
        ///         <item>an "<i>m</i> x <i>n</i>" 2D matrix is <i>m</i> rows and <i>n</i> columns</item>
        ///         <item>a <i>1</i> x <i>n</i> matrix is horizontal</item>
        ///         <item>an <i>m</i> x <i>1</i> matrix is vertical</item>
        ///     </list>
        /// </remarks>
        public static void Multiply_mn_m1(Index1D t_n, ArrayView2D<double, Stride2D.DenseX> b_mn, ArrayView1D<double, Stride1D.Dense> a_n1, ArrayView1D<double, Stride1D.Dense> target_n1) {
            for (int k = 0; k < t_n.Size; k++)
                target_n1[t_n] += b_mn[t_n, k] * a_n1[k];
        }
    }
}
