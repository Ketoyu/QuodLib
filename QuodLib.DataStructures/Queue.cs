using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.DataStructures
{
	/// <summary>
	/// A from-scratch implementation of a Queue.
	/// </summary>
	/// <typeparam name="T"></typeparam>
    public class Queue<T> //where T : IComparable<T>
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
            
        private Node Head, Tail;
        public uint Size { get; private set; }
        public Queue()
        {
            Head = null;
            Tail = null;
            Size = 0;
        }
        /// <summary>
        /// Adds the provided Node to the front of the Queue.
        /// </summary>
        /// <param name="value"></param>
        public void Enqueue(T value)
        {
            switch (Size)
            {
                case 0:
                    Head = new Node(value, null); // Head --> null
                    Tail = Head; // Head == Tail
                    break;
                case 1:
                    Tail = new Node(Head.Value, null);
                    Head = new Node(value, null);
                    Tail.Next = Head; // Head <-- Tail
                    Head.Next = Tail; // Head --> Tail
                    break;
                default:
                    Head = new Node(value, Head); // new --> previous head --> ... --> Tail.Next --> Tail
                    break;
            }
            Size++;
        }
        /// <summary>
        /// Removes the Node at the end of the Queue and returns that Node's value.
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            T rtn = Tail.Copy().Value;
            Tail = Tail.Next; // Head --> ... --> Tail.Next <-- Tail
            Size--;
            return rtn;
        }
        /// <summary>
        /// Returns the value at the top of the stack without removing it.
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            return Head.Copy().Value;
        }
        /// <summary>
        /// Set every item in the Queue to null.
        /// </summary>
        public void Clear()
        {
            while (Head.Next != null)
                Head = Head.Next;

            Head = null;
            Tail = null;
            Size = 0;
        }
		/// <summary>
		/// Whether the Queue() is empty.
		/// </summary>
		/// <returns></returns>
        public bool IsEmpty()
        {
            return Size == 0;
        }
			
		/// <summary>
		/// Reverses the item order in [this] Queue and returns [this] Queue.
		/// </summary>
		/// <returns></returns>
		public Queue<T> Reverse()
		{
			Stack<T> temp = new Stack<T>();
			while(!this.IsEmpty()) temp.Push(this.Dequeue());
			while (!temp.IsEmpty()) this.Enqueue(temp.Pop());
			return this;
		}
    }
}
