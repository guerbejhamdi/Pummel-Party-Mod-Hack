using System;
using System.Linq;

namespace I2.Loc
{
	// Token: 0x0200083C RID: 2108
	public class HindiFixer
	{
		// Token: 0x06003BD8 RID: 15320 RVA: 0x0012C878 File Offset: 0x0012AA78
		internal static string Fix(string text)
		{
			char[] array = text.ToCharArray();
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == 'ि' && !char.IsWhiteSpace(array[i - 1]) && array[i - 1] != '\0')
				{
					array[i] = array[i - 1];
					array[i - 1] = 'ि';
					flag = true;
				}
				if (i != array.Length - 1)
				{
					if (array[i] == 'इ' && array[i + 1] == '़')
					{
						array[i] = 'ऌ';
						array[i + 1] = '\0';
						flag = true;
					}
					if (array[i] == 'ृ' && array[i + 1] == '़')
					{
						array[i] = 'ॄ';
						array[i + 1] = '\0';
						flag = true;
					}
					if (array[i] == 'ँ' && array[i + 1] == '़')
					{
						array[i] = 'ॐ';
						array[i + 1] = '\0';
						flag = true;
					}
					if (array[i] == 'ऋ' && array[i + 1] == '़')
					{
						array[i] = 'ॠ';
						array[i + 1] = '\0';
						flag = true;
					}
					if (array[i] == 'ई' && array[i + 1] == '़')
					{
						array[i] = 'ॡ';
						array[i + 1] = '\0';
						flag = true;
					}
					if (array[i] == 'ि' && array[i + 1] == '़')
					{
						array[i] = 'ॢ';
						array[i + 1] = '\0';
						flag = true;
					}
					if (array[i] == 'ी' && array[i + 1] == '़')
					{
						array[i] = 'ॣ';
						array[i + 1] = '\0';
						flag = true;
					}
					if (array[i] == '।' && array[i + 1] == '़')
					{
						array[i] = 'ऽ';
						array[i + 1] = '\0';
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return text;
			}
			string text2 = new string((from x in array
			where x > '\0'
			select x).ToArray<char>());
			if (text2 == text)
			{
				return text2;
			}
			text = text2;
			return text;
		}
	}
}
