using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.DataStructures
{
    /*
        * TYPE Node;
        * Node first;
        * Node Node Add(T);
        * Node Remove(location)
        * Node Remove(node)
        * Node Remove(node, removed || previous)
        * Node Insert(node, location<node>)
        * Node Insert(node, location<uint>)
        * Node SearchNode(node)
        * Node SearchNode(node, found || previous)
        * Node SearchNode(int)
        * Node SearchLoc(node)
        */
	/// <summary>
	/// A from-scratch implementation of a Singly-Linked List.
	/// </summary>
	/// <typeparam name="T"></typeparam>
    public class SLList<T> //where T : IComparable<T>
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

        public Node First;
        /// <summary>
        /// Appends a new Node containing the provided value to the end of the SLList. Returns the created Node.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Node Add(T value)
        {
            Node search = First;

            //Scroll to the end of the List.
            while (search.Next != null)
                search = search.Next;

            search.Next = new Node(value, null);
            return search.Next;
        }
        /// <summary>
        /// Removes the node at the provided location and returns the Node now at that location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public Node Remove(uint location)
        {
            Node search = First;
            Node searchPrev = null;

            //find location
            uint loc;
            for (loc = 0; loc < location && search.Next != null; loc++) {
                searchPrev = search;
                search = search.Next;
            }

            //return Node at location
            if (loc == location) {
                searchPrev.Next = search.Next;
                search.Next = null;
                return searchPrev.Next;
            } else
                return null;
        }
        /// <summary>
        /// Locates and removes the provided Node and returns the Node which was perviously pointed to by the removed node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public Node Remove(Node node)
        {
            return Remove(node, false);
        }
        public Node Remove(Node node, bool returnPrevious)
        {
            Node search = SearchNode(node, true);
            Node searchNext = search.Next;
            if (search != null) {
                search.Next = searchNext.Next;
                if (returnPrevious) {
                    searchNext = null;
                    return search.Next;
                } else {
                    Node rtn = searchNext.Copy();
                    searchNext = null;
                    return rtn;
                }
            } else
				return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="location"></param>
        public void Insert(Node node, uint location)
        {
            Node search = SearchNode((int)location);
            node.Next = search.Next;
            search.Next = node;
        }
        /// <summary>
        /// Inserts the provided node behind the provided location.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="location"></param>
        public void Insert(Node node, Node location)
        {
            Node search = SearchNode(location);
            node.Next = search.Next;
            search.Next = node;
        }
        public Node SearchNode(Node node)
        {
            return SearchNode(node, false);
        }
        public Node SearchNode(Node node, bool prev)
        {
            Node search = First;
            Node searchPrev = null;

            //find node
            while (search.Next != null && !search.Equals(node))
            {
                searchPrev = search;
                search = search.Next;
            }

            //return node or previous node
            if (search.Equals(node)) {
                if (prev) return searchPrev;
					else return search;
            } else
				return null;
        }
        public Node SearchNode(int location)
        {
            Node search = First;
            Node searchPrev = null;
            
            //find node at location
            uint loc;
            for (loc = 0; loc < location && search.Next != null; loc++) {
                searchPrev = search;
                search = search.Next;
            }

            //return node
            if (loc == location)
				return search;
            else
                return null;
        }
        public int SearchLoc(Node node)
        {
            Node search = First;
            Node searchPrev = null;

            //find location of node
            int loc;
            for (loc = 0; search.Next != null && !search.Equals(node); loc++) {
                searchPrev = search;
                search = search.Next;
            }
            
            //return location
            if (search.Next.Equals(node))
                return loc;
            else
                return -1;
        }

    } // </List>
}
