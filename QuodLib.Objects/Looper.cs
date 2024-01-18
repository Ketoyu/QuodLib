using QuodLib.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Objects {

    /// <summary>
    /// A wrapper for walking an <see cref="IList{T}"/> from start to end, then seamlessly over again.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Looper<T> {
        private readonly Func<IList<T>?> GetList;
        private int _index;
        public int Index {
            get => _index;
            private set {
                _index = value % (GetList()?.Count ?? 0);
            }
        }
        public Looper(Func<IList<T>?> getList) {
            GetList = getList;
        }
        public T? GetNext() {
            var list = GetList();
            if (list != null)
                return list[Index++];
            
            return default;
        }
        public void Skip(int count) {
            Index += count;
        }
        public void MoveFirst() {
            Index = 0;
        }
        public void MoveLast() {
            Index = GetList()?.LastIndex() ?? 0;
        }
        public void Jump(int index) {
            Index = index;
        }
    }
}
