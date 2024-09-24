using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Objects
{
    /// <summary>
    /// An empty implementation of <see cref="IProgress{T}"/>; see <see cref="Value"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class EmptyProgress<T> : IProgress<T> {
        /// <summary>
        /// An empty <see cref="IProgress{T}"/>.
        /// </summary>
        public static readonly EmptyProgress<T> Value = new();

        private EmptyProgress()
        { }

        /// <summary>
        /// Empty implementation; does nothing.
        /// </summary>
        /// <param name="value"></param>
        public void Report(T value)
        { } //Do nothing
    }
}
