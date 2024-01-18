using QuodLib.Linq.Comparers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.DataStructures.NTree {
    public class NTreeByDelegate<TData> : NTreeBase<TData>
        where TData : notnull {
        protected Func<TData, int> _getID { get; init; }
        protected Func<TData, int?> _getParentID { get; init; }
        protected Action<TData, int?> _setParentID { get; init; }

        public NTreeByDelegate(Func<TData, int> getID, Func<TData, int?> getParentID, Action<TData, int?> setParentID, IEnumerable<TData> nodes) : base(nodes) {
            _getID = getID;
            _getParentID = getParentID;
            _setParentID = setParentID;
        }

        public override int GetID(TData data)
            => _getID(data);

        public override int? GetParentID(TData data)
            => _getParentID(data);

        public override void SetParentID(TData data, int? parentID)
            => _setParentID(data, parentID);
    }
}
