﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.Assertions;

namespace Assets.Encrypter
{
	class DiffieHellman
	{
		//source: https://www.ietf.org/rfc/rfc3526.txt
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

		private BigInteger publicModulo = new BigInteger(Prime2048);
		private BigInteger publicBase = 7;

		private BigInteger secretKey = new BigInteger();
		private BigInteger phase1SecretKey = new BigInteger();
		private BigInteger sharedSecretKey = new BigInteger();
		private byte[] hashedSharedSecretKey;

		public DiffieHellman()
		{


			secretKey = BigInteger.GenerateRandom(16);
			phase1SecretKey = publicBase.ModPow(secretKey, publicModulo);

			BigInteger test = new BigInteger(2);
			BigInteger test2 = test.ModPow(7, 16);
			BigInteger test3 = test.ModPow(9, 16);
			BigInteger test4 = test.ModPow(21, 123123);
			Assert.IsTrue(test2 == 14);
			Assert.IsTrue(test3 == 2);
			Assert.IsTrue(test4 == 42);


			Combine(test2);
		}

		public void Combine(BigInteger otherPhase1Secret)
		{
			sharedSecretKey = otherPhase1Secret.ModPow(secretKey, publicModulo);

			byte[] hashedSharedKey = SHA256.Create().ComputeHash(sharedSecretKey.GetBytes());
		}


	}
}
