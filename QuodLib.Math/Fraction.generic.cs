//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Numerics;
//using System.Text;
//using System.Threading.Tasks;

//namespace QuodLib.Math {
//    public struct Fraction<TNumber> : IComparable
//        where TNumber : IBinaryInteger<TNumber>, ISignedNumber<TNumber> {
//        public TNumber Numerator { get; set; } = TNumber.AdditiveIdentity;
//        public TNumber Denominator { get; set; } = TNumber.AdditiveIdentity;

//        public Fraction(TNumber numerator, TNumber denominator) {
//            Numerator = numerator;
//            Denominator = denominator;
//        }

//        public TNumber[] ToArray()
//            => new[] { Numerator, Denominator };

//        public int CompareTo(object? obj) {
//            if (obj is Fraction<TNumber> f)
//                return (f < this ? -1
//                        : f == this ? 0
//                        : 1);
//            else
//                return -1;
//        }

//        private Fraction(TNumber[] values) {
//            Numerator = values[0];
//            Denominator = values[1];
//        }

//        public static Fraction<TNumber> operator *(Fraction<TNumber> f, TNumber value)
//            => (f with { Numerator = value * f.Numerator }).Reduce();

//        public static Fraction<TNumber> operator /(Fraction<TNumber> f, TNumber value)
//            => (f with { Denominator = value * f.Denominator }).Reduce();

//        public static Fraction<TNumber> operator +(Fraction<TNumber> f, TNumber value)
//            => (f with { Numerator = f.Numerator + (f.Denominator * value) }).Reduce();

//        public static Fraction<TNumber> operator -(Fraction<TNumber> f, TNumber value)
//            => f + (TNumber.NegativeOne * value);

//        public static Fraction<TNumber> operator *(Fraction<TNumber> a, Fraction<TNumber> b)
//            => new(General.Reduce(a.Numerator * b.Numerator, a.Denominator * b.Denominator));

//        public static Fraction<TNumber> operator /(Fraction<TNumber> a, Fraction<TNumber> b)
//            => a * b.Inverse();

//        public static Fraction<TNumber> operator +(Fraction<TNumber> a, Fraction<TNumber> b) {
//            var mult = General.LCM(a.Denominator, b.Denominator);
//            return new Fraction<TNumber>((a.Numerator * (mult / b.Denominator)) + (b.Numerator * (mult / a.Denominator)), mult).Reduce();
//        }

//        public static Fraction<TNumber> operator -(Fraction<TNumber> a, Fraction<TNumber> b)
//            => a * (b with { Numerator = TNumber.NegativeOne * b.Numerator });

//        public Fraction<TNumber> Inverse()
//            => new(Denominator, Numerator);

//        public Fraction<TNumber> Reduce()
//            => new(General.Reduce(Numerator, Denominator));

//        public static bool operator <(Fraction<TNumber> a, Fraction<TNumber> b)
//            => a.Value < b.Value;
//        public static bool operator >(Fraction<TNumber> a, Fraction<TNumber> b)
//            => a.Value > b.Value;

//        public static bool operator ==(Fraction<TNumber> a, Fraction<TNumber> b)
//            => a.Numerator == b.Numerator && a.Denominator == b.Denominator;

//        public static bool operator !=(Fraction<TNumber> a, Fraction<TNumber> b)
//            => a.Numerator != b.Numerator || a.Denominator != b.Denominator;

//        public override bool Equals(object? obj) {
//            if (obj is Fraction<TNumber> f)
//                return this == f;
//            else
//                return false;
//        }

//        public override string ToString()
//            => Numerator + "/" + Denominator;

//        public void Deconstruct(out TNumber numerator, out TNumber denominator) {
//            numerator = Numerator;
//            denominator = Denominator;
//        }
//    }
//}
