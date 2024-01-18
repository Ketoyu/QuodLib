using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Objects
{
	/// <summary>
	/// Takes control of an attached object, pulling its strings by running its functions and adding event-fires.
	/// </summary>
	public abstract class Puppeteer<P>
	{
		public P Puppet { get; protected set; }
		protected Puppeteer(P puppet)
		{
			AttachPuppet(puppet);
		}
		protected virtual void AttachPuppet(P puppet)
		{
			Puppet = puppet;
		}
	}
}

