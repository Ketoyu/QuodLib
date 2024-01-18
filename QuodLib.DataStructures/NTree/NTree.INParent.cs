using QuodLib.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.DataStructures.NTree {
    public class NTree<TData> : NTreeBase<TData>
        where TData : notnull, INParentID {
        protected Func<TData, int> _getID { get; init; }
        public NTree(Func<TData, int> getID, IEnumerable<TData> nodes) : base(nodes) {
            _getID = getID;
        }

        public override int GetID(TData data)
            => _getID(data);

        public override int? GetParentID(TData data)
            => data.ParentID;

        public override void SetParentID(TData data, int? parentID)
            => data.ParentID = parentID;
    }
}
