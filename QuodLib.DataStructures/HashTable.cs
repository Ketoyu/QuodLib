using QuodLib.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.DataStructures
{
	/// <summary>
	/// A simple Hash Table. The hashing algorithm is hard-coded {TODO: but can be overriden}. {TODO: Can} Cannot be resized.
	/// </summary>
	/// <typeparam name="K"></typeparam>
	/// <typeparam name="V"></typeparam>
	public class HashTable<K, V> //where K : IComparable<K>
	{
		protected V[] Data;
		public int ItemCount { get; protected set; }
		public bool IsNumeric { get; private set; }
		public bool IsFull {
			get {
				return ItemCount == Data.Length;
			}
		}
		/// <summary>
		/// Creates new HashTable of the provided size.
		/// </summary>
		/// <param name="size"></param>
		public HashTable(int size)
		{
			Data = new V[size];
			ItemCount = 0;
			IsNumeric = Types.IsNumericWhole(typeof(K));
		}
		/// <summary>
		/// Adds the <paramref name="item"/> to the HashTable using the provided <paramref name="key"/>. Probe()s if <paramref name="key"/> is taken.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="item"></param>
		public virtual int Add(K key, V item)
		{
			if (IsFull) Overflow(item);
			int rtn = Probe(key, default(V));
			Data[rtn] = item;
			ItemCount++;
			return rtn;
		}
		/// <summary>
		/// Action to take if an item tries to enter when the HashTable is full.
		/// </summary>
		protected virtual void Overflow(V item)
		{
			throw new Exception("Cannot add item \'" + item.ToString() + "\' because HashTable is full.");
		}
		/// <summary>
		/// Sets GetHash[key]) to the provided <paramref name="item"/> - overwrites <paramref name="key"/>'s item it not empty.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="item"></param>
		public void Set(K key, V item)
		{
			Data[GetHash(key)] = item;
		}
		/// <summary>
		/// Removes GetHash(<paramref name="key"/>)'s item from the Table.
		/// </summary>
		/// <param name="key"></param>
		public void Remove(K key)
		{
			Data[GetHash(key)] = default(V);
			ItemCount--;
		}
		/// <summary>
		/// Searches for the first occurrance of <paramref name="value"/> in the Table and removes it.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(V value)
		{
			Data[ProbeIndex(0, value)] = default(V);
			ItemCount--;
		}
		/// <summary>
		/// Gets GetHash([key])'s item and returns it.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public V Get(K key)
		{
			return Data[GetHash(key)];
		}
		/// <summary>
		/// Searches for <paramref name="item"/> in the Table and returns it.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int Search(V item)
		{
			return ProbeIndex(0, item);
		}
		/// <summary>
		/// Probes for and returns an open index in the Table holding <paramref name="value"/>, starting at <paramref name="index"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected int ProbeIndex(int index, V value)
		{
			int rtn = index;
			int original = rtn;
			bool looped = false, found = false;
			while (!(looped && rtn == original) && !found)
			{
				if (rtn == Data.Length) {
					looped = true;
					rtn = 0;
				} else {
					if (value == null) {
						if (Data[rtn] == null) found = true;
					} else if (Data[rtn].Equals(value)) found = true;
				}
				if (!found) rtn++;
			}
			return rtn;
		}
		/// <summary>
		/// Probes for and returns an index in the Table holding <paramref name="value"/>, starting at GetHash(<paramref name="key"/>).
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		private int Probe(K key, V value)
		{
			return ProbeIndex(GetHash(key), value);
		}
		/// <summary>
		/// Generates and returns an index from the provided <paramref name="key"/>.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public virtual int GetHash(K key)
		{
			if (IsNumeric)
				return System.Math.Abs(int.Parse(key.ToString()) % Data.Length);
			else {
				char[] c = key.ToString().ToCharArray();
				int rtn = 0;
				for (int i = 0; i < c.Length; i++)
					rtn += c[i];

				return rtn % Data.Length;
			}
		}
		/// <summary>
		/// Outputs the contents of the Table in text format - does not print the corresponding keys.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return ToString(false);
		}
		/// <summary>
		/// Outputs the contents of the Table in text format.
		/// </summary>
		/// <param name="includeKeys"></param>
		/// <returns></returns>
		public string ToString(bool includeKeys)
		{
			string rtn = "{\n" + (includeKeys ? " " : "");
			bool first = true;
			for (int i = 0; i < Data.Length; i++) {
				V value = Data[i];
				if (value != null) {
					if (first) first = false;
						else rtn += ", \n\n";
					rtn += (includeKeys ? "[" + i + ", " : "") + value.ToString() + (includeKeys ? "]" : "");
				}
			}
			return rtn + (includeKeys ? " " : "") + "\n}";
		}
	} // </class>
}
