using Numerics;
using System;
using System.Security.Cryptography;

namespace Diffie_Hellman
{
	class Program
	{
		static void Main(string[] args)
		{
			BigInteger secretPhrase = BigInteger.GenerateRandom(1024);

			BigInteger publicModulo = BigInteger.GenerateRandom(1024);

			Console.ReadKey();

		}
	}
}
