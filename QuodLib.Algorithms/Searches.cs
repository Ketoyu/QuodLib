using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;

namespace QuodLib.Algorithms
{
	/*static class Searches
	{
		public delegate bool Test(Point test);
		public delegate bool TolerantTest(Point test, float tolerence)			;
		List<Point> Fill_Detect(Point start, Rectangle bounds, Test criteria)
		{

		}
		List<Point> Fill_Detect(Point start, float tolerence, Rectangle bounds, TolerantTest criteria)
		{
			return Fill_Detect(new List<Point>(), start, start, bounds, criteria, tolerence);
		}
		private List<Point> Fill_Detect(List<Point> visited, Point start, Point prev, Rectangle bounds, TolerantTest criteria, float tolerence)
		{
			Queue<Point> start_, prev_;


			for (int i = 0; i < 2; i++) {
				int ofs = (int)Math.Pow(-1, i);
				Point x = new Point(start.X + ofs, start.Y),
					y = new Point(start.X, start.Y + ofs);
				if ( (x == prev ? false : !visited.Contains(x) ) && Math.General.IsInRect(x, bounds) )
					if (criteria(x, tolerence)) {
						visited.Add(x);
						Fill_Detect(visited, start, x, bounds, criteria, tolerence);
					}
				if ( (y == prev ? false : !visited.Contains(y) ) && Math.General.IsInRect(y, bounds) )
					if (criteria(y, tolerence)) {
						visited.Add(y);
						Fill_Detect(visited, start, y, bounds, criteria, tolerence);
					}
			}
		}
		private List<Point> Fill_Detect_iter(Point start, Rectangle bounds, TolerantTest criteria, float tolerence)
		{
			Queue<Point> start_, prev;
			Point pos = start;
			List<Point> rtn, visited;
			// ... ...

			return rtn;
		}
	} */
}
