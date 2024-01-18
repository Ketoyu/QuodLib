using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

namespace QuodLib
{
	namespace Objects
	{
		/// <summary>
		/// A Timer which calls a delegate function on each tick.
		/// </summary>
		public class DelegatedTimer
		{
			public delegate void DRelease();
			private event DRelease ERelease;
			private Timer tmr;

			public delegate bool DReleaseCondition();
			private DReleaseCondition checkRelease;
			public bool HasReleaseCondition {
				get {
					return checkRelease == null;
				}
			}
			public void SetReleaseCondition(DReleaseCondition checkBeforeRelease)
			{
				checkRelease = checkBeforeRelease;
			}
			
			public DelegatedTimer(int milliseconds, DRelease doOnRelease)
			{
				ERelease += doOnRelease;
				tmr = new Timer();
				tmr.Interval = milliseconds;
				this.tmr.Tick += new EventHandler(this.Tick);
			}
			public DelegatedTimer(int milliseconds, DRelease doOnRelease, DReleaseCondition checkBeforeRelease) : this(milliseconds, doOnRelease)
			{
				checkRelease = checkBeforeRelease;
			}
			public void Start()
			{
				tmr.Start();
			}
			private void Tick(object sender, EventArgs e)
			{
				if (HasReleaseCondition ? checkRelease() : true) {
					tmr.Stop();
					ERelease();
				}
			}
		}
	}
}
