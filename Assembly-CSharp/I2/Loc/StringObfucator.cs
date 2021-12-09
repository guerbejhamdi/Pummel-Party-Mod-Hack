using System;
using System.Text;

namespace I2.Loc
{
	// Token: 0x02000851 RID: 2129
	public class StringObfucator
	{
		// Token: 0x06003C26 RID: 15398 RVA: 0x0012E94C File Offset: 0x0012CB4C
		public static string Encode(string NormalString)
		{
			string result;
			try
			{
				result = StringObfucator.ToBase64(StringObfucator.XoREncode(NormalString));
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003C27 RID: 15399 RVA: 0x0012E980 File Offset: 0x0012CB80
		public static string Decode(string ObfucatedString)
		{
			string result;
			try
			{
				result = StringObfucator.XoREncode(StringObfucator.FromBase64(ObfucatedString));
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003C28 RID: 15400 RVA: 0x00028432 File Offset: 0x00026632
		private static string ToBase64(string regularString)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(regularString));
		}

		// Token: 0x06003C29 RID: 15401 RVA: 0x0012E9B4 File Offset: 0x0012CBB4
		private static string FromBase64(string base64string)
		{
			byte[] array = Convert.FromBase64String(base64string);
			return Encoding.UTF8.GetString(array, 0, array.Length);
		}

		// Token: 0x06003C2A RID: 15402 RVA: 0x0012E9D8 File Offset: 0x0012CBD8
		private static string XoREncode(string NormalString)
		{
			string result;
			try
			{
				char[] stringObfuscatorPassword = StringObfucator.StringObfuscatorPassword;
				char[] array = NormalString.ToCharArray();
				int num = stringObfuscatorPassword.Length;
				int i = 0;
				int num2 = array.Length;
				while (i < num2)
				{
					array[i] = (array[i] ^ stringObfuscatorPassword[i % num] ^ (char)((byte)((i % 2 == 0) ? (i * 23) : (-i * 51))));
					i++;
				}
				result = new string(array);
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x04003998 RID: 14744
		public static char[] StringObfuscatorPassword = "ÝúbUu¸CÁÂ§*4PÚ©-á©¾@T6Dl±ÒWâuzÅm4GÐóØ$=Íg,¥Që®iKEßr¡×60Ít4öÃ~^«y:Èd1<QÛÝúbUu¸CÁÂ§*4PÚ©-á©¾@T6Dl±ÒWâuzÅm4GÐóØ$=Íg,¥Që®iKEßr¡×60Ít4öÃ~^«y:Èd".ToCharArray();
	}
}
