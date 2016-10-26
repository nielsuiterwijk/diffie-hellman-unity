using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Assets.Encrypter
{
	public class AESHelper
	{
		RijndaelManaged rm = null;

		private ICryptoTransform encryptorTransform;
		private ICryptoTransform decryptorTransform;

		private System.Text.UTF8Encoding UTFEncoder;

		public AESHelper(byte[] key, byte[] vector)
		{
			//This is our encryption method
			rm = new RijndaelManaged();
			rm.Padding = PaddingMode.Zeros;

			encryptorTransform = rm.CreateEncryptor(key, vector);
			decryptorTransform = rm.CreateDecryptor(key, vector);

			//Used to translate bytes to text and vice versa
			UTFEncoder = new System.Text.UTF8Encoding();
		}

		public void SetIV(byte[] iv)
		{
			rm.IV = iv;
		}

		/// Encrypt some text and return an encrypted byte array.
		public byte[] Encrypt(byte[] data, int length)
		{
			return encryptorTransform.TransformFinalBlock(data, 0, length);
		}

		/// Decryption when working with byte arrays.
		public byte[] Decrypt(byte[] EncryptedValue, int length)
		{
			return decryptorTransform.TransformFinalBlock(EncryptedValue, 0, length);
		}
	}
}
