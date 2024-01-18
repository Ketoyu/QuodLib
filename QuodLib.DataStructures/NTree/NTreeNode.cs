using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.DataStructures.NTree {
    public class NTreeNode<TData> where TData : notnull {
        public NTreeBase<TData> Container { get; protected set; }
        public TData Data { get; set; }
        private int? _id;
        public int ID => _id ??= Container.GetID(Data);

        private int? _parentID;
        public int? ParentID {
            get => _parentID ??= Container.GetParentID(Data);
            internal set {
                Container.SetParentID(Data, value);
                _parent = null;
                _parentID = value;
                Container.Changed(Data);
            }
        }
        internal NTreeNode(NTreeBase<TData> container, TData data) {
            Container = container;
            Data = data;
        }

        private NTreeNode<TData>? _parent;
        public NTreeNode<TData>? Parent {
            get {
                if (_parent != null)
                    return _parent;

                int? id = ParentID;
                if (id == null)
                    return null;

                return _parent = Container.Find((int)id!);
            }
        }

        public IEnumerable<NTreeNode<TData>> Children
            => Container.FindChildren(ID);

        public void MoveUp() {
            if (ParentID == null)
                return;

            ParentID = Parent!.ParentID;
        }

        public void SwapUp() {
            if (ParentID == null)
                return;

            var parent = Parent;
            int? grandID = parent!.ParentID;

            parent!.ParentID = ID;
            ParentID = grandID;
        }

        public void SwapDown(NTreeNode<TData> child) {
            if (child.ParentID != ID)
                throw new ArgumentException($"{child} is not the child of this {nameof(NTreeNode<TData>)}", nameof(child));

            child.SwapUp();
        }

        public void Graduate()
            => ParentID = null;

        public void Unpack() {
            foreach (var child in Children)
                child.ParentID = ParentID;
        }

        public NTreeNode<TData> AddChild(TData data) {
            NTreeNode<TData> node = Container.AddNode(data);
            node.ParentID = ID;
            return node;
        }

    }
}
