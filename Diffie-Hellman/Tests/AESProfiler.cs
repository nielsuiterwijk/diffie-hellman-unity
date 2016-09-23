using Numerics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Diffie_Hellman.Tests
{
	class AESProfiler
	{
		public static void Run(BigInteger secretPhrase)
		{
			//Have to use the same IV
			byte[] iv = SimpleAES.GenerateEncryptionVector();

			SimpleAES aes1 = new SimpleAES(secretPhrase.GetBytes(), iv);
			SimpleAES aes2 = new SimpleAES(secretPhrase.GetBytes(), iv);

			string test = "Lorem Ipsum Ad Finito";
			byte[] testArray = Encoding.ASCII.GetBytes(test);

			int numberOfRuns = 1000000;

			Stopwatch timer = new Stopwatch();
			timer.Start();

			for (int i = 0; i < numberOfRuns; i++)
			{
				byte[] encrypted = aes1.Encrypt(testArray, testArray.Length);
				byte[] decrtyped = aes2.Decrypt(encrypted, encrypted.Length);
			}

			for (int i = 0; i < numberOfRuns; i++)
			{
				byte[] encrypted = aes2.Encrypt(testArray, testArray.Length);
				byte[] decrtyped = aes1.Decrypt(encrypted, encrypted.Length);
			}


			timer.Stop();
			Console.WriteLine("AESProfiler took " + timer.ElapsedMilliseconds + " millseconds.");
		}
	}
}
