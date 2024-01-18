using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

using System.Drawing;

namespace QuodLib
{
	/// <summary>
	/// Contains functions for getting information about the positions of multiple displays.
	/// </summary>
	public class classScreens
	{
        /*public static Rectangle[] MultiScreenAreas {
            get {
                
            }
        }*/
		public static Screen[] Screens {
			get {
				Screen[] Scns = Screen.AllScreens;
				uint sc = (uint)Scns.Count();
				bool PmF = false;

				//Orient by { Primary = 0 }
				Screen[] NwScn = new Screen[sc];
				for (uint i = 0; i < sc && !PmF; i++)
				{
					if (Scns[i] == Screen.PrimaryScreen)
					{
						NwScn[0] = Scns[i];
						PmF = true;
					}
				}
				for (uint[] i = new uint[] {0, 1}; i[0] < sc; i[0]++, i[1]++)
				{
					if (Scns[i[0]] == Screen.PrimaryScreen)
					{
						i[1]--;
					} else {
						NwScn[i[1]] = Scns[i[0]];
					}
				}
				return NwScn;
			}
		}

		public static Size PrimarySize
		{
			get {
				Screen pm = Screens[0];
				return new Size(pm.Bounds.Width, pm.Bounds.Height);
			}
		}
		public static bool IsOffScreen(Point location)
		{
			bool isOn = false;
			foreach (Screen scn in Screens)
			{
				/*if (scn.Primary)
				{
					Rectangle prim = scn.WorkingArea;
					isOn = isOn || Math.General.IsInRect(location, new Rectangle(prim.X, prim.Y, prim.Width, prim.Height - 42));
				} else*/
					isOn = isOn || Math.General.IsInRect(location, scn.WorkingArea);
			}
			return !isOn;
		}

		/// <summary>
		///  5	 4   3
		///   \  |  /
		/// 6 -- 8 -- 2
		///   /  |  \
		///  7	 0   1
		/// </summary>
		//public static float[,] Orientations
		//{
        //    //TODO: convert to polar coordinates, subtract pi/2, absolute value, % by 2pi, multiply by (8 / 2pi).
        //
		//	get {
		//		Screen[] Scns = Screens;
		//		float[,] rtn = new float[Scns.Count(),2];
		//		for (byte i = 1; i < Scns.Count(); i++)
		//		{
		//			
		//		}
		//	}
		//}
		//public static uint TotalWidth
		//{
        //  
		//}
		//public static uint TotalHeight //based on screen orientations
		//{
        //  
		//}
		//public static Rectangle[] StableSizes //based on orientations
		//{
        //  
		//}
	} //END-ALL
}
