using System;
using System.IO;
using UnityEngine;

// Token: 0x020004DE RID: 1246
public class TranslationTempDeleter : MonoBehaviour
{
	// Token: 0x060020E9 RID: 8425 RVA: 0x000CD490 File Offset: 0x000CB690
	private void Awake()
	{
		try
		{
			for (int i = 0; i < TranslationTempDeleter.FileNames.Length; i++)
			{
				string text = Application.temporaryCachePath + "/" + TranslationTempDeleter.FileNames[i];
				if (File.Exists(text))
				{
					Debug.Log("Deleted: " + text);
					File.Delete(text);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Attempted to delte temp Localization Files: " + ex.ToString());
		}
	}

	// Token: 0x040023AD RID: 9133
	private static readonly string[] FileNames = new string[]
	{
		"LangSource_I2Languages_Chinese (Simplified).loc",
		"LangSource_I2Languages_Chinese (Traditional).loc",
		"LangSource_I2Languages_English.loc",
		"LangSource_I2Languages_French.loc",
		"LangSource_I2Languages_German.loc",
		"LangSource_I2Languages_Italian.loc",
		"LangSource_I2Languages_Korean.loc",
		"LangSource_I2Languages_Russian.loc",
		"LangSource_I2Languages_Spanish (Spain).loc",
		"LangSource_I2Languages_日本語.loc"
	};
}
