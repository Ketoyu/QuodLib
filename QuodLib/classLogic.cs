using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuodLib
{
    class classLogic
    {
        public static T Switch<T>(T default_, params (bool Condition, T Value)[] cases)
        {
            (bool Condition, T Value)? found = cases.FirstOrDefault(p => p.Condition);
			if (found != null)
				return found.Value.Value;

			return default_;
        }
        public static T Switch<T>(params (bool Condition, T Value)[] cases)
            => cases.First(p => p.Condition).Value;
    } //END-ALL
}
