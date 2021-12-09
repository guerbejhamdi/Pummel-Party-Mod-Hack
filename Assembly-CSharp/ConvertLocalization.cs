using System;
using System.IO;
using System.Text;
using UnityEngine;

// Token: 0x0200003D RID: 61
public class ConvertLocalization : MonoBehaviour
{
	// Token: 0x06000101 RID: 257 RVA: 0x0002FD34 File Offset: 0x0002DF34
	[ContextMenu("Convert")]
	public void Convert()
	{
		string text = File.ReadAllText(this.inputFile);
		string text2 = "\"Identifier\",\"English\",\"Description\"\n";
		char c = ';';
		Delimiter delimiter = this.delimiter;
		if (delimiter != Delimiter.SemiColon)
		{
			if (delimiter == Delimiter.Tab)
			{
				c = '\t';
			}
		}
		else
		{
			c = ';';
		}
		string[] array = text.Split(new char[]
		{
			'\n'
		});
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(new char[]
			{
				c
			});
			if (array2.Length >= 4)
			{
				text2 = string.Concat(new string[]
				{
					text2,
					"\"",
					array2[0],
					"\",\"",
					array2[3],
					"\",\"",
					array2[2],
					"\"\n"
				});
			}
			else
			{
				Debug.LogError("Not enough columns");
			}
		}
		File.WriteAllText(this.outputFile, text2, Encoding.UTF8);
	}

	// Token: 0x0400015B RID: 347
	public Delimiter delimiter;

	// Token: 0x0400015C RID: 348
	public string inputFile = "D:/Localization.csv";

	// Token: 0x0400015D RID: 349
	public string outputFile = "D:/Localization_Converted.csv";
}
