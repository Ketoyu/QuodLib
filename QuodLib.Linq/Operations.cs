using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuodLib.Linq {
    public static class Operations {
        public static void Counter<T>(IList<T> start, IList<(T Min, T Max)> bounds,
            Func<T, (T Min, T Max), (T, bool)> Increment,
            Func<IList<T>,
                IList<(T Min, T Max)>,
                bool> Finished) {
            IList<(T Min, T One, T Max)> bounds_ = bounds.ConvertAll(b => (b.Min, b.Min, b.Max));
            Counter(start, (list) => bounds_, Increment, Finished);
        }


        public static void Counter<T>(IList<T> start, IList<(T Min, T One, T Max)> bounds,
            Func<T, (T Min, T Max), (T, bool)> Increment,
            Func<IList<T>,
                IList<(T Min, T Max)>,
                bool> Finished)
            => Counter(start, (list) => bounds, Increment, Finished);

        public static void Counter<T>(IList<T> start,
            Func<IList<T>, IList<(T Min, T Max)>> GetBounds,
            Func<T, (T Min, T Max), (T, bool)> Increment,
            Func<IList<T>,
                IList<(T Min, T Max)>,
                bool> Finished)
            => Counter(start,
                (list) => GetBounds(list).ConvertAll(b => (b.Min, b.Min, b.Max)),
                Increment, Finished);

        public static void Counter<T>(IList<T> start,
            Func<IList<T>, IList<(T Min, T One, T Max)>> GetBounds,
            Func<T, (T Min, T Max), (T, bool)> Increment,
            Func<IList<T>,
                IList<(T Min, T Max)>,
                bool> Finished) {
            if (Finished(start,
                GetBounds(start).ConvertAll(b => (b.Min, b.Max)))) return; //Initial check.
            if (start.Count == 0) start.Add(default); //Initialize.

            int index;
            (T Min, T One, T Max) bounds_i;
            do { //Count.
                for (index = 0; ; index++) { //Increment; carry if necessary.

                    (T Value, bool Success) next;

                    bounds_i = GetBounds(start)[index];

                    if (index <= start.Count) {

                        //Try increment.
                        next = Increment(start[index], (bounds_i.Min, bounds_i.Max));
                    }
                    else {
                        next = (default(T), false);
                    }

                    if (next.Success) { //if (success), apply and break(for).
                        start[index] = next.Value;
                        break;
                    }
                    else { //else, carry.
                        start[index] = bounds_i.Min; //Reset to min.

                        //if !(space for carry), add new digit.
                        if (start.LastIndex() < index + 1) {
                            start.Add(default); //Add new
                            start[start.LastIndex()] = GetBounds(start)[index + 1].One; //Re-initialize
                        }
                    }
                } //for-loop: carry.
            } while (!Finished(start, GetBounds(start).ConvertAll(b => (b.Min, b.Max)))); //do-loop: increment again.
        }
    }
}
