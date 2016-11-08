using Diffie_Hellman.Tests;
using Numerics;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Diffie_Hellman
{
	class Program
	{
		internal static readonly uint[] Prime2048 = new uint[64] {  0xFFFFFFFF, 0xFFFFFFFF, 0xC90FDAA2, 0x2168C234, 0xC4C6628B, 0x80DC1CD1,
				0x29024E08, 0x8A67CC74, 0x020BBEA6, 0x3B139B22, 0x514A0879, 0x8E3404DD,
				0xEF9519B3, 0xCD3A431B, 0x302B0A6D, 0xF25F1437, 0x4FE1356D, 0x6D51C245,
				0xE485B576, 0x625E7EC6, 0xF44C42E9, 0xA637ED6B, 0x0BFF5CB6, 0xF406B7ED,
				0xEE386BFB, 0x5A899FA5, 0xAE9F2411, 0x7C4B1FE6, 0x49286651, 0xECE45B3D,
				0xC2007CB8, 0xA163BF05, 0x98DA4836, 0x1C55D39A, 0x69163FA8, 0xFD24CF5F,
				0x83655D23, 0xDCA3AD96, 0x1C62F356, 0x208552BB, 0x9ED52907, 0x7096966D,
				0x670C354E, 0x4ABC9804, 0xF1746C08, 0xCA18217C, 0x32905E46, 0x2E36CE3B,
				0xE39E772C, 0x180E8603, 0x9B2783A2, 0xEC07A28F, 0xB5C55DF0, 0x6F4C52C9,
				0xDE2BCBF6, 0x95581718, 0x3995497C, 0xEA956AE5, 0x15D22618, 0x98FA0510,
				0x15728E5A, 0x8AACAA68, 0xFFFFFFFF, 0xFFFFFFFF
																 };

		static BigInteger publicModulo = new BigInteger(Prime2048);
		static BigInteger publicBase = 9;

		static BigInteger aliceSecret =  BigInteger.GenerateRandom(128);
		static BigInteger bobSecret = BigInteger.GenerateRandom(128);

		static RingBuffer<int>  ringBuffer = new RingBuffer<int>(16);

		static int globalCounter = 123340;

		static void Main(string[] args)
		{
			//bool result = publicModulo.IsProbablePrime(ushort.MaxValue / 10);
			//Console.WriteLine(publicModulo.ToString() + " is a " + (result ? "prime" : "not a prime!"));

			int loopCountCopy = globalCounter / 2;
			loopCountCopy += 10;


			object obj = new object();

			new Thread(() =>
			{
				int loopCount = loopCountCopy * 10;
				int prevNumber = globalCounter + 1;

				while (true)
				{
					int number = 0;
					bool result = ringBuffer.Dequeue(out number);

					if (!result)
					{
						continue;
					}

#if DEBUG

					if (prevNumber - 1 != number)
					{
						Console.WriteLine("incorrect number read, probably an overflow : " + prevNumber + " vs " + number);

						if (number == default(int))
						{
							lock (obj)
							{
								Console.WriteLine("result was true, but number read was incorrect");
							}

							break;
						}

					}

#endif
					Console.WriteLine("correct number read: " + number);

					prevNumber = number;

					if (prevNumber == 0 && number == 0)
					{
						break;
					}


				}

				Console.WriteLine("consumer done 1");

			}).Start();

			new Thread(() =>
			{
				int loopCount = loopCountCopy;

				Console.WriteLine("start producer 1");

				while (loopCount > 0)
				{
					lock (obj)
					{
						int numberToEnqueue = 0;

						do
						{
							//Note: primitives are guaranteed to be atomic reads.
							numberToEnqueue = globalCounter;
						}
						while (Interlocked.CompareExchange(ref globalCounter, numberToEnqueue - 1, numberToEnqueue) != numberToEnqueue);

						ringBuffer.Enqueue(numberToEnqueue);
						loopCount--;

						if (numberToEnqueue <= 0)
						{
							break;
						}

						//Thread.Sleep(0);
					}
				}

				Console.WriteLine("producer done 1");


			}).Start();

			new Thread(() =>
			{
				int loopCount = loopCountCopy;

				Console.WriteLine("start producer 2");

				while (loopCount > 0)
				{
					lock (obj)
					{
						int numberToEnqueue = 0;

						do
						{
							//Note: primitives are guaranteed to be atomic reads.
							numberToEnqueue = globalCounter;
						}
						while (Interlocked.CompareExchange(ref globalCounter, numberToEnqueue - 1, numberToEnqueue) != numberToEnqueue);

						ringBuffer.Enqueue(numberToEnqueue);
						loopCount--;

						if (numberToEnqueue <= 0)
						{
							break;
						}

						//Thread.Sleep(0);
					}
				}

				Console.WriteLine("producer done 2");


			}).Start();

			new Thread(() =>
			{
				int loopCount = loopCountCopy;

				Console.WriteLine("start producer 3");

				while (loopCount > 0)
				{
					lock (obj)
					{
						int numberToEnqueue = 0;

						do
						{
							//Note: primitives are guaranteed to be atomic reads.
							numberToEnqueue = globalCounter;
						}
						while (Interlocked.CompareExchange(ref globalCounter, numberToEnqueue - 1, numberToEnqueue) != numberToEnqueue);

						ringBuffer.Enqueue(numberToEnqueue);
						loopCount--;

						if (numberToEnqueue <= 0)
						{
							break;
						}

						//Thread.Sleep(0);
					}
				}

				Console.WriteLine("producer done 3");


			}).Start();

			new Thread(() =>
			{
				int loopCount = loopCountCopy;

				Console.WriteLine("start producer 4");

				while (loopCount > 0)
				{
					lock (obj)
					{
						int numberToEnqueue = 0;

						do
						{
							//Note: primitives are guaranteed to be atomic reads.
							numberToEnqueue = globalCounter;
						}
						while (Interlocked.CompareExchange(ref globalCounter, numberToEnqueue - 1, numberToEnqueue) != numberToEnqueue);

						ringBuffer.Enqueue(numberToEnqueue);
						loopCount--;

						if (numberToEnqueue <= 0)
						{
							break;
						}

						//Thread.Sleep(0);
					}
				}

				Console.WriteLine("producer done 4");


			}).Start();

			new Thread(() =>
			{
				int loopCount = loopCountCopy;

				Console.WriteLine("start producer 5");

				while (loopCount > 0)
				{
					lock (obj)
					{
						int numberToEnqueue = 0;

						do
						{
							//Note: primitives are guaranteed to be atomic reads.
							numberToEnqueue = globalCounter;
						}
						while (Interlocked.CompareExchange(ref globalCounter, numberToEnqueue - 1, numberToEnqueue) != numberToEnqueue);

						ringBuffer.Enqueue(numberToEnqueue);
						loopCount--;

						if (numberToEnqueue <= 0)
						{
							break;
						}

						//Thread.Sleep(0);
					}
				}

				Console.WriteLine("producer done 5");


			}).Start();




			/*  Console.WriteLine("Alice secret: " + aliceSecret.ToString());
			    Console.WriteLine("Bob secret:   " + bobSecret.ToString());

			    Console.WriteLine("public modulo:   " + publicModulo.ToString());

			    BigInteger aliceSharedSecret = AlicePhase2(BobPhase1());
			    BigInteger bobSharedSecret = BobPhase2(AlicePhase1());

			    Console.WriteLine("Alice shared: " + aliceSharedSecret.ToString());
			    Console.WriteLine("Bob shared:   " + bobSharedSecret.ToString());

			    bool equal = aliceSharedSecret == bobSharedSecret;
			    Console.WriteLine("The shared secret is equal: " + equal.ToString());

			    byte[] hashedSharedKey = SHA256.Create().ComputeHash(aliceSharedSecret.GetBytes());

			    AESProfiler.Run(new BigInteger(hashedSharedKey));*/

			Console.ReadKey();

			int a = 0;

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
