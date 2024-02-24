using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.DataStructures
{
	public class Graph<T>
	{
		public delegate bool Predicate(Vertex test);
		#region Properties
		public List<Vertex> Verteces {get; private set; }
		public List<Edge> Edges {get; private set; }
		#endregion //Properties
		#region CustomObjects
		public class Edge {
			public Vertex Source, Dest;
			public bool Directed = false;
			public Edge(Vertex source, Vertex dest)
			{
				Source = source;
				Dest = dest;
			}
			public Edge(Vertex source, Vertex dest, bool directed) : this(source, dest)
			{
				Directed = directed;
			}
			/// <summary>
			/// Swaps the <see cref="Source"/> and <see cref="Dest"/> Verteces.
			/// </summary>
			public Edge Invert()
			{
				if (Directed) {
					Vertex temp = Source;
					Source = Dest;
					Dest = temp;
				}
				return this;
			}
			/// <summary>
			/// Returns true if this edge and the provided edge both connect the same pair of verteces, and either have the same direction or are both directionless.
			/// </summary>
			/// <param name="edge"></param>
			/// <returns></returns>
			public bool Equals(Edge edge)
			{
				if (Directed && edge.Directed) return Source == edge.Source && Dest == edge.Dest;
				if (!(Directed || edge.Directed)) return LooslyEquals(edge);
				return false;
			}
			/// <summary>
			/// Returns true if this edge and the provided edge connect the same pair of verteces.
			/// </summary>
			/// <param name="edge"></param>
			/// <returns></returns>
			public bool LooslyEquals(Edge edge)
			{
				return (Source == edge.Source && Dest == edge.Dest) || (Source == edge.Dest && Dest == edge.Source);
			}
		}
		#endregion//CustomObjects
		public class Vertex
		{
			public T Data;
			private Graph<T> parent;

			/// <summary>
			/// Queries <see cref="Edges"/> for Verteces that either connect directionlessly to or are destined from [this] Vertex.
			/// </summary>
			public List<Vertex> Neighbors {
				get {
					List<Vertex> rtn = new List<Vertex>();
					foreach (Edge e in Edges)
						if (e.Directed && e.Source == this) rtn.Add(e.Dest);
							else if (!e.Directed) rtn.Add(e.Source == this ? e.Dest : e.Source);
					return rtn;
				}
			}
			public List<Edge> DestEdges {
				get {
					List<Edge> rtn = new List<Edge>();
					foreach (Edge e in Edges)
						if (e.Directed && e.Source == this) rtn.Add(e);
							else if (!e.Directed) rtn.Add(e.Source == this ? e : e.Invert());
					return rtn;
				}
			}
			/// <summary>
			/// Queries the parent Graph for edges that contain [this] Vertex.
			/// </summary>
			public List<Edge> Edges {
				get {
					List<Edge> rtn = new List<Edge>();
					foreach (Edge e in parent.Edges)
						if (e.Source == this || e.Dest == this) rtn.Add(e);
					return rtn;
				}
			}
			/// <summary>
			/// Creates a new Vertex under the <paramref name="parent"/> Graph; should only be called directly by the Graph itself.
			/// </summary>
			public Vertex(Graph<T> parent, T data)
			{
				this.parent = parent;
				Data = data;
			}
		}
		#region Constructors
		public Graph()
		{
			Verteces = new List<Vertex>();
			Edges = new List<Edge>();
		}
		#endregion//Constructors
		#region Functions
			#region Modifying
		/// <summary>
		/// Creates and adds a disconnected Vertex to [this] Graph.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public Vertex Add(T data)
		{
			Vertex rtn = new Vertex(this, data);
			Verteces.Add(rtn);
			return rtn;
		}
		/// <summary>
		/// Creates an undirected edge between the two Verteces and adds it to [this] Graph.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="dest"></param>
		/// <returns></returns>
		public Edge Connect(Vertex source, Vertex dest)
		{
			return Connect(source, dest, false);
		}
		/// <summary>
		/// Creates an edge (directed if <paramref name="directed"/>) between the two Verteces and adds it to [this] Graph.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="dest"></param>
		/// <param name="directed"></param>
		/// <returns></returns>
		public Edge Connect(Vertex source, Vertex dest, bool directed)
		{
			Edge rtn = new Edge(source, dest, directed);
			Edges.Add(rtn);
			return rtn;
		}
		/// <summary>
		/// Disconnects and returns the provided edge only if it both exists and is undirected, or has the same [Source] and [Dest] Verteces (in that order); returns null otherwise.
		/// </summary>
		/// <param name="edge"></param>
		/// <returns></returns>
		public Edge Disconnect(Edge edge)
		{
			return Disconnect(edge.Source, edge.Dest, edge.Directed);
		}
		/// <summary>
		/// Disconnects the two verteces and returns their edge, only if the edge both exists and has the provided <paramref name="directed"/> value; returns null otherwise.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="dest"></param>
		/// <param name="directed"></param>
		/// <returns></returns>
		public Edge Disconnect(Vertex source, Vertex dest, bool directed)
		{
			Edge rtn = new Edge(source, dest, false);
			foreach (Edge e in Edges)
				if (e.Equals(rtn)) {
					Edges.Remove(e);
					return rtn;
				}
			return null;
		}
		/// <summary>
		/// Disconnects and returns the provided edge if it exists, regardless of whether it's directed; returns null otherwise.
		/// </summary>
		/// <param name="edge"></param>
		/// <returns></returns>
		public Edge DisconnectLoosly(Edge edge)
		{
			return DisconnectLoosly(edge.Source, edge.Dest);
		}
		/// <summary>
		/// Disconnects the two verteces and returns their edge, if it exists, whether or not it was directional; returns null otherwise.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="dest"></param>
		/// <returns></returns>
		public Edge DisconnectLoosly(Vertex source, Vertex dest)
		{
			Edge rtn = new Edge(source, dest, false);
			foreach (Edge e in Edges)
				if (e.LooslyEquals(rtn)) {
					Edges.Remove(e);
					return rtn;
				}
			return null;
		}
			#endregion //Modifying
			#region ReturnOnly
		/// <summary>
		/// Returns a Vertex matching the <paramref name="criteria"/>, but not the path leading to it.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="criteria"></param>
		/// <returns></returns>
		public Vertex Search_DepthFirst(Vertex start, Predicate criteria)
		{
			Queue<Edge> temp = new Queue<Edge>();
			return Search_DepthFirst(start, criteria, out temp);
		}
		/// <summary>
		/// Returns a Vertex matching the <paramref name="criteria"/> and stores the path leading to it in <paramref name="path"/>.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="criteria"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public Vertex Search_DepthFirst(Vertex start, Predicate criteria, out Queue<Edge> path)
		{
			Stack<Edge> temp = new Stack<Edge>();
			Vertex rtn = Search_DepthFirst(start, null, criteria, new List<Vertex>(), temp);

			temp.Reverse();
			path = new Queue<Edge>();
			while (!temp.IsEmpty()) path.Enqueue(temp.Pop());
			return rtn;
		}
		private Vertex Search_DepthFirst(Vertex current, Edge previous, Predicate criteria, List<Vertex> visited, Stack<Edge> path)
		{				
			if (visited.Contains(current)) {
				path.Pop();
				return null;
			}
			visited.Add(current);
			path.Push(previous);
			if (criteria(current)) return current;
				
			foreach (Edge neighbor in current.DestEdges)
				return Search_DepthFirst(neighbor.Dest, neighbor, criteria, visited, path);
				
			path.Pop();
			return null;
		}
			#endregion //ReturnOnly
		#endregion //Functions
	}
}
