using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Diffie_Hellman
{
	public class SimpleAES
	{
		private ICryptoTransform encryptorTransform;
		private ICryptoTransform decryptorTransform;

		private MemoryStream encryptionStream;
		private CryptoStream encrypt;

		private MemoryStream decryptionStream;
		private CryptoStream decrypt;



		private System.Text.UTF8Encoding UTFEncoder;

		public SimpleAES(byte[] key, byte[] vector)
		{
			//This is our encryption method
			RijndaelManaged rm = new RijndaelManaged();

			//Create an encryptor and a decryptor using our encryption method, key, and vector.
			encryptorTransform = rm.CreateEncryptor(key, vector);
			decryptorTransform = rm.CreateDecryptor(key, vector);

			encryptionStream = new MemoryStream();
			encrypt = new CryptoStream(encryptionStream, encryptorTransform, CryptoStreamMode.Write);

			decryptionStream = new MemoryStream();
			decrypt = new CryptoStream(decryptionStream, decryptorTransform, CryptoStreamMode.Write);

			//Used to translate bytes to text and vice versa
			UTFEncoder = new System.Text.UTF8Encoding();
		}

		public SimpleAES(byte[] key) : this(key, GenerateEncryptionVector())
		{
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
			encrypt.Write(data, 0, length);
			encrypt.FlushFinalBlock();

			encryptionStream.Position = 0;
			byte[] encrypted = new byte[encryptionStream.Length];
			encryptionStream.Read(encrypted, 0, encrypted.Length);

			return encrypted;
		}

		/// Decryption when working with byte arrays.
		public byte[] Decrypt(byte[] EncryptedValue, int length)
		{
			decrypt.Write(EncryptedValue, 0, length);
			decrypt.FlushFinalBlock();

			decryptionStream.Position = 0;
			Byte[] decryptedBytes = new Byte[length];
			decryptionStream.Read(decryptedBytes, 0, decryptedBytes.Length);

			return decryptedBytes;

		}

	}
}
