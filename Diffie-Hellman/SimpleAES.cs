using System;
using System.IO;
using System.Security.Cryptography;

namespace Diffie_Hellman
{
	public class SimpleAES
	{
		private ICryptoTransform encryptorTransform;
		private ICryptoTransform decryptorTransform;

		private System.Text.UTF8Encoding UTFEncoder;

		public SimpleAES(byte[] key, byte[] vector)
		{
			//This is our encryption method
			RijndaelManaged rm = new RijndaelManaged();
			rm.Padding = PaddingMode.Zeros;

			encryptorTransform = rm.CreateEncryptor(key, vector);
			decryptorTransform = rm.CreateDecryptor(key, vector);

			//Used to translate bytes to text and vice versa
			UTFEncoder = new System.Text.UTF8Encoding();
		}

		/// Generates a unique encryption vector
		static public byte[] GenerateEncryptionVector()
		{
			//Generate a Vector
			RijndaelManaged rm = new RijndaelManaged();
			rm.GenerateIV();
			return rm.IV;
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