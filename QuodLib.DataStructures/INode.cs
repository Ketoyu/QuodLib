namespace QuodLib.DataStructures {
    public interface INode<T> {
        public T Data { get; }
        public List<INode<T>>? Children { get; }
        public INode<T>? Parent { get; }
        public INode<T> Clone();

        public INode<T> FindRoot() {
            INode<T> node = this;
            while (node.Parent != null)
                node = node.Parent;
            return node;
        }

    }
}