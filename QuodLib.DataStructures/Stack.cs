using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuodLib.DataStructures
{
	/// <summary>
	/// A from-scratch implementation of a Stack.
	/// </summary>
	/// <typeparam name="T"></typeparam>
    public class Stack<T> //where T : IComparable<T>
    {
        public class Node
        {
            public T Value;
            public Node Next;
            public Node(T val, Node next)
            {
                Value = val;
                Next = next;
            }
            public bool Equals(Node node)
            {
                return Value.Equals(node.Value) && (Next == node.Next); //.CompareTo(...) == 0
            }
            public Node Copy()
            {
                return new Node(Value, Next);
            }
        } // </Node>
            
        private Node Head;
        public uint Size{ get; private set; }
        public Stack()
        {
            Head = null;
            Size = 0;
        }
        /// <summary>
        /// Adds the provided Node to the top of the stack.
        /// </summary>
        /// <param name="value"></param>
        public void Push(T value)
        {
            Head = new Node(value, new Node(Head.Value, Head.Next));
            Size++;
        }
        /// <summary>
        /// Returns and removes the Node at the top of the Stack.
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            Node rtn = Head.Copy();
            Head = Head.Next.Copy();
            Size--;
            return rtn.Value;
        }
        /// <summary>
        /// Returns a copy of the Node at the top of the stack without removing it.
        /// </summary>
        /// <returns></returns>
        public Node Peek()
        {
            return Head.Copy();
        }
        /// <summary>
        /// Set every item in the Stack to null.
        /// </summary>
        public void Clear()
        {
            while (Head.Next != null)
                Head = Head.Next;

            Head = null;
        }
		/// <summary>
		/// Whether [this] Stack is empty.
		/// </summary>
		/// <returns></returns>
        public bool IsEmpty()
        {
            return Size == 0;
        }
		/// <summary>
		/// Reverses the item order in [this] Stack and returns [this] Stack.
		/// </summary>
		/// <returns></returns>
		public Stack<T> Reverse()
		{
			Queue<T> temp = new Queue<T>();
			while (!this.IsEmpty()) temp.Enqueue(this.Pop());
			while (!temp.IsEmpty()) this.Push(temp.Dequeue());
			return this;
		}
    }
}
