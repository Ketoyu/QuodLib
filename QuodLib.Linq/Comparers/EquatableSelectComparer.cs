using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace QuodLib.Linq.Comparers
{
    public class EquatableSelectComparer<TItem, TProperty> : IEqualityComparer<TItem>
        where TProperty : IEquatable<TProperty> {

        public Func<TItem, TProperty> Selector { get; init; }
        public EquatableSelectComparer(Func<TItem, TProperty> selector)
        {
            Selector = selector;
        }

        public bool Equals(TItem? x, TItem? y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            TProperty x_ = Selector(x);
            TProperty y_ = Selector(y);

            if (x_ == null && y_ == null)
                return true;

            if (x_ == null || y_ == null)
                return false;

            return x_.Equals(y_);
        }

        public int GetHashCode([DisallowNull] TItem obj)
            => obj.GetHashCode();
    }
}
