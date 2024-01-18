//using System.Text;
//using System.Threading.Tasks;

namespace QuodLib.Math {
    public struct Fraction : IComparable {
		public long Numerator { get; set; } = 0;
		public long Denominator { get; set; } = 0;
		public double Value
			=> (double)Numerator / Denominator;

        public Fraction(long numerator, long denominator) {
			Numerator = numerator;
			Denominator = denominator;
        }

		public long[] ToArray()
			=> new[] { Numerator, Denominator };
			
		public int CompareTo(object? obj) {
            if (obj is Fraction f)
				return (f < this ? -1
						: f == this ? 0
						: 1);
			else
				return -1;
        }

		private Fraction(long[] values) {
			Numerator = values[0];
			Denominator = values[1];
        }

		public static Fraction operator *(Fraction f, long value)
			=> (f with { Numerator = value * f.Numerator }).Reduce();

		public static Fraction operator /(Fraction f, long value)
			=> (f with { Denominator = value * f.Denominator }).Reduce();

		public static Fraction operator +(Fraction f, long value)
			=> (f with { Numerator = f.Numerator + (f.Denominator * value) }).Reduce();

		public static Fraction operator -(Fraction f, long value)
			=> f + (-1 * value);

		public static Fraction operator *(Fraction a, Fraction b)
			=> new(General.Reduce(a.Numerator * b.Numerator, a.Denominator * b.Denominator));

		public static Fraction operator /(Fraction a, Fraction b)
			=> a * b.Inverse();

		public static Fraction operator +(Fraction a, Fraction b) {
			var mult = General.LCM(a.Denominator, b.Denominator);
			return new Fraction((a.Numerator * (mult / b.Denominator)) + (b.Numerator * (mult / a.Denominator)), mult).Reduce();
		}

		public static Fraction operator -(Fraction a, Fraction b)
			=> a * (b with { Numerator = -1 * b.Numerator});

		public Fraction Inverse()
			=> new(Denominator, Numerator);

		public Fraction Reduce()
			=> new(General.Reduce(Numerator, Denominator));

		public static bool operator <(Fraction a, Fraction b)
			=> a.Value < b.Value;
		public static bool operator >(Fraction a, Fraction b)
			=> a.Value > b.Value;

		public static bool operator ==(Fraction a, Fraction b)
			=> a.Numerator == b.Numerator && a.Denominator == b.Denominator;

		public static bool operator !=(Fraction a, Fraction b)
			=> a.Numerator != b.Numerator || a.Denominator != b.Denominator;

		public override bool Equals(object? obj) {
			if (obj is Fraction f)
				return this == f;
			else
				return false;
		}

        public override string ToString()
            => Numerator + "/" + Denominator;

        public void Deconstruct(out long numerator, out long denominator) {
            numerator = Numerator;
			denominator = Denominator;
        }
    }
}
