using Diffie_Hellman.Tests;
using Numerics;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Diffie_Hellman
{
	class Program
	{
		static BigInteger publicModulo = BigInteger.GenerateRandom(256);
		static BigInteger publicBase = 9;

		static BigInteger aliceSecret =  BigInteger.GenerateRandom(128);
		static BigInteger bobSecret = BigInteger.GenerateRandom(128);


		static void Main(string[] args)
		{
			Console.WriteLine("Alice secret: " + aliceSecret.ToString());
			Console.WriteLine("Bob secret:   " + bobSecret.ToString());

			Console.WriteLine("public modulo:   " + publicModulo.ToString());

			BigInteger aliceSharedSecret = AlicePhase2(BobPhase1());
			BigInteger bobSharedSecret = BobPhase2(AlicePhase1());

			Console.WriteLine("Alice shared: " + aliceSharedSecret.ToString());
			Console.WriteLine("Bob shared:   " + bobSharedSecret.ToString());

			bool equal = aliceSharedSecret == bobSharedSecret;
			Console.WriteLine("The shared secret is equal: " + equal.ToString());

			AESProfiler.Run(aliceSharedSecret);

			Console.ReadKey();

		}

		public static BigInteger AlicePhase1()
		{
			BigInteger phase1 = publicBase.ModPow(aliceSecret, publicModulo);
			Console.WriteLine("Alice 1: " + phase1.ToString());

			return phase1;
		}

		public static BigInteger AlicePhase2(BigInteger bobPhase1)
		{
			BigInteger phase2 = bobPhase1.ModPow(aliceSecret, publicModulo);
			Console.WriteLine("Alice 2: " + phase2.ToString());

			return phase2;
		}

		public static BigInteger BobPhase1()
		{
			BigInteger phase1 = publicBase.ModPow(bobSecret, publicModulo);
			Console.WriteLine("Bob 1: " + phase1.ToString());

			return phase1;
		}
		public static BigInteger BobPhase2(BigInteger alicePhase1)
		{
			BigInteger phase2 = alicePhase1.ModPow(bobSecret, publicModulo);
			Console.WriteLine("Bob 2: " + phase2.ToString());

			return phase2;
		}
	}
}
