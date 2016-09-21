using Numerics;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Diffie_Hellman
{
	class Program
	{
		static void Main(string[] args)
		{
			BigInteger secretPhrase = BigInteger.GenerateRandom(256);

			BigInteger publicModulo = BigInteger.GenerateRandom(64);

			SimpleAES aes1 = new SimpleAES(secretPhrase.GetBytes());
			SimpleAES aes2 = new SimpleAES(secretPhrase.GetBytes());

			string test = "Test";
			byte[] testArray = Encoding.ASCII.GetBytes(test);

			byte[] a = aes1.Encrypt(testArray, testArray.Length);

			byte[] b = aes2.Encrypt(testArray, testArray.Length);

			string testFromA = Encoding.ASCII.GetString(aes1.Decrypt(a, a.Length));
			string testFromB = Encoding.ASCII.GetString(aes2.Decrypt(b, b.Length));



			Console.ReadKey();

		}
	}
}
