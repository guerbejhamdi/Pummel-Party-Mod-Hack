using System;
using System.IO;
using UnityEngine;

// Token: 0x02000496 RID: 1174
public static class TextureUtility
{
	// Token: 0x06001F75 RID: 8053 RVA: 0x000C7E4C File Offset: 0x000C604C
	public static void SaveTextureToFile(Texture2D texture, string file_name)
	{
		byte[] buffer = texture.EncodeToPNG();
		FileStream fileStream = File.Open(file_name, FileMode.Create);
		new BinaryWriter(fileStream).Write(buffer);
		fileStream.Close();
	}

	// Token: 0x06001F76 RID: 8054 RVA: 0x000C7E78 File Offset: 0x000C6078
	public static Texture2D CreateTextureFromValues(float[][] values)
	{
		int num = values.Length;
		int num2 = (values.Length != 0) ? values[0].Length : num;
		Texture2D texture2D = new Texture2D(num, num2);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				texture2D.SetPixel(i, j, new Color(values[i][j], values[i][j], values[i][j], 1f));
			}
		}
		return texture2D;
	}
}
