using QuodLib.Linq.Comparers;

namespace QuodLib.DataStructures.NTree {
    public abstract class NTreeBase<TData>
        where TData : notnull {

        protected Dictionary<int, NTreeNode<TData>> Nodes { get; set; }
        public IEnumerable<NTreeNode<TData>> Roots
            => Nodes.Values
                .Where(n => n.ParentID == null);
        protected HashSet<int> _changes { get; private set; } = new HashSet<int>();
        public IEnumerable<TData> Changes
            => _changes
                .Select(id => Nodes[id].Data);

        public void Update(IEnumerable<TData> data) {
            foreach (var item in data) {
                int id = GetID(item);
                Nodes[id].Data = item;
                _changes.Remove(id);
            }
        }

        protected HashSet<TData> _removed { get; private set; }
        public IEnumerable<TData> Removed
            => _removed;

        protected NTreeBase(IEnumerable<TData> nodes) {
            _removed = new(new EquatableSelectComparer<TData, int>(GetID));
            Nodes = nodes
                .ToDictionary(GetID, d => new NTreeNode<TData>(this, d));
        }

        public void Changed(TData data)
            => _changes.Add(GetID(data));

        public NTreeNode<TData> AddNode(TData data) {
            NTreeNode<TData> node = new NTreeNode<TData>(this, data);
            Nodes.Add(node.ID, node);
            return node;
        }

        public NTreeNode<TData> Find(int id)
            => Nodes[id];

        public IEnumerable<NTreeNode<TData>> FindChildren(int? parentID) {
            if (parentID == null)
                return Enumerable.Empty<NTreeNode<TData>>();

            return Nodes.Values.Where(n => n.ParentID == parentID);
        }

        public TData Remove(int id) {
            var node = Find(id);
            _changes.Remove(id);
            Nodes.Remove(id);
            _removed.Add(node.Data);
            return node.Data;
        }

        public abstract int GetID(TData data);
        public abstract int? GetParentID(TData data);
        public abstract void SetParentID(TData data, int? parentID);
    }
}
