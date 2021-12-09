using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Recipe
{
	// Token: 0x02000639 RID: 1593
	public static class Cake
	{
		// Token: 0x060029E6 RID: 10726 RVA: 0x000F6868 File Offset: 0x000F4A68
		public static string Bake(string plainText, string passPhrase)
		{
			byte[] array = Cake.Generate256BitsOfRandomEntropy();
			byte[] array2 = Cake.Generate256BitsOfRandomEntropy();
			byte[] bytes = Encoding.UTF8.GetBytes(plainText);
			byte[] bytes2 = new Rfc2898DeriveBytes(passPhrase, array, 1000).GetBytes(32);
			string result;
			using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
			{
				rijndaelManaged.BlockSize = 256;
				rijndaelManaged.Mode = CipherMode.CBC;
				rijndaelManaged.Padding = PaddingMode.PKCS7;
				using (ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor(bytes2, array2))
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
						{
							cryptoStream.Write(bytes, 0, bytes.Length);
							cryptoStream.FlushFinalBlock();
							byte[] inArray = array.Concat(array2).ToArray<byte>().Concat(memoryStream.ToArray()).ToArray<byte>();
							memoryStream.Close();
							cryptoStream.Close();
							result = Convert.ToBase64String(inArray);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060029E7 RID: 10727 RVA: 0x000F6990 File Offset: 0x000F4B90
		public static string Eat(string cipherText, string passPhrase)
		{
			byte[] array = Convert.FromBase64String(cipherText);
			byte[] salt = array.Take(32).ToArray<byte>();
			byte[] rgbIV = array.Skip(32).Take(32).ToArray<byte>();
			byte[] array2 = array.Skip(64).Take(array.Length - 64).ToArray<byte>();
			byte[] bytes = new Rfc2898DeriveBytes(passPhrase, salt, 1000).GetBytes(32);
			string @string;
			using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
			{
				rijndaelManaged.BlockSize = 256;
				rijndaelManaged.Mode = CipherMode.CBC;
				rijndaelManaged.Padding = PaddingMode.PKCS7;
				using (ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor(bytes, rgbIV))
				{
					using (MemoryStream memoryStream = new MemoryStream(array2))
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
						{
							byte[] array3 = new byte[array2.Length];
							int count = cryptoStream.Read(array3, 0, array3.Length);
							memoryStream.Close();
							cryptoStream.Close();
							@string = Encoding.UTF8.GetString(array3, 0, count);
						}
					}
				}
			}
			return @string;
		}

		// Token: 0x060029E8 RID: 10728 RVA: 0x000F6ADC File Offset: 0x000F4CDC
		private static byte[] Generate256BitsOfRandomEntropy()
		{
			byte[] array = new byte[32];
			new RNGCryptoServiceProvider().GetBytes(array);
			return array;
		}

		// Token: 0x04002C46 RID: 11334
		private const int Keysize = 256;

		// Token: 0x04002C47 RID: 11335
		private const int DerivationIterations = 1000;
	}
}
