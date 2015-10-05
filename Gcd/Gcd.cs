
// Flaviu Pasca
// flaviup @ gmail.com
// (C) 2015

using System;

namespace SharpAlgorithms
{
	public class Gcd
	{
		public Gcd (long a, long b)
		{
			Value = Compute (a, b);
		}

		public long Value { get; private set; }

		public static long Compute (long a, long b)
		{
			while (b != 0)
			{
				Assign (ref a, ref b, b, a % b);
				// Alternative implementation:
				/*var t = Tuple.Create (b, a % b);
				a = t.Item1;
				b = t.Item2;*/
			}
			return a;
		}

		private static void Assign (ref long a, ref long b, long a2, long b2)
		{
			a = a2;
			b = b2;
		}
	}
}
