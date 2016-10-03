using Numerics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Diffie_Hellman.Tests
{
	// Miller - Rabin primality test as an extension method on the BigInteger type.
// Based on the Ruby implementation on this page.
	public static class BigIntegerExtensions
	{
		public static int progress = 0;
		public static bool IsProbablePrime(this BigInteger source, int certainty)
		{
			if (source == 2 || source == 3)
			{
				return true;
			}

			if (source < 2 || source % 2 == 0)
			{
				return false;
			}

			BigInteger d = source - 1;
			int s = 0;

			while (d % 2 == 0)
			{
				d /= 2;
				s += 1;
			}

			// There is no built-in method for generating random BigInteger values.
			// Instead, random BigIntegers are constructed from randomly generated
			// byte arrays of the same length as the source.
			RandomNumberGenerator rng = RandomNumberGenerator.Create();
			byte[] bytes = new byte[source.GetBytes().LongLength];
			BigInteger a;

			int steps = certainty / 1000;
			double percent = 0;

			Stopwatch timer = new Stopwatch();

			timer.Start();

			for (int i = 0; i < certainty; i++)
			{
				if (i != 0 && i % steps == 0)
				{
					percent++;

					Console.WriteLine((percent / 10.0d) + "% took " + timer.ElapsedMilliseconds + "ms so far!");

				}



				do
				{
					// This may raise an exception in Mono 2.10.8 and earlier.
					// http://bugzilla.xamarin.com/show_bug.cgi?id=2761
					rng.GetBytes(bytes);
					a = new BigInteger(bytes);
				}
				while (a < 2 || a >= source - 2);

				BigInteger x = a.ModPow(d, source);

				if (x == 1 || x == source - 1)
				{
					continue;
				}

				for (int r = 1; r < s; r++)
				{
					x = x.ModPow(2, source);

					if (x == 1)
					{
						return false;
					}

					if (x == source - 1)
					{
						break;
					}
				}

				if (x != source - 1)
				{
					return false;
				}
			}



			return true;
		}
	}
}
