using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Objects {
	/// <summary>
	/// Takes control of many attached objects, pulling their strings by running their functions and adding event-fires.
	/// </summary>
	public abstract class PuppetMaster<P> {
		public List<P> Puppets { get; protected set; }
		public PuppetMaster() {
			Puppets = new();
		}

		protected virtual void Possess(P puppet) {
			Puppets.Add(puppet);
		}

		protected virtual void Release(P puppet) {
			Puppets.Remove(puppet);
		}
	} // </class>
}
