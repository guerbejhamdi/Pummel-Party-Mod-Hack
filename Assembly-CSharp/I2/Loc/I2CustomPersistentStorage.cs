using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020007EF RID: 2031
	public class I2CustomPersistentStorage : I2BasePersistentStorage
	{
		// Token: 0x060039AE RID: 14766 RVA: 0x00120C54 File Offset: 0x0011EE54
		public override void SetSetting_String(string key, string value)
		{
			try
			{
				int length = value.Length;
				int num = 8000;
				if (length <= num)
				{
					RBPrefs.SetString(key, value);
				}
				else
				{
					int num2 = Mathf.CeilToInt((float)length / (float)num);
					for (int i = 0; i < num2; i++)
					{
						int num3 = num * i;
						RBPrefs.SetString(string.Format("[I2split]{0}{1}", i, key), value.Substring(num3, Mathf.Min(num, length - num3)));
					}
					RBPrefs.SetString(key, "[$I2#@div$]" + num2.ToString());
				}
			}
			catch (Exception)
			{
				Debug.LogError("Error saving PlayerPrefs " + key);
			}
		}

		// Token: 0x060039AF RID: 14767 RVA: 0x00120CFC File Offset: 0x0011EEFC
		public override string GetSetting_String(string key, string defaultValue)
		{
			string result;
			try
			{
				string text = RBPrefs.GetString(key, defaultValue);
				if (!string.IsNullOrEmpty(text) && text.StartsWith("[I2split]"))
				{
					int num = int.Parse(text.Substring("[I2split]".Length));
					text = "";
					for (int i = 0; i < num; i++)
					{
						text += RBPrefs.GetString(string.Format("[I2split]{0}{1}", i, key), "");
					}
				}
				result = text;
			}
			catch (Exception)
			{
				Debug.LogError("Error loading PlayerPrefs " + key);
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x00120D9C File Offset: 0x0011EF9C
		public override void DeleteSetting(string key)
		{
			try
			{
				string @string = RBPrefs.GetString(key, null);
				if (!string.IsNullOrEmpty(@string) && @string.StartsWith("[I2split]"))
				{
					int num = int.Parse(@string.Substring("[I2split]".Length));
					for (int i = 0; i < num; i++)
					{
						RBPrefs.DeleteKey(string.Format("[I2split]{0}{1}", i, key));
					}
				}
				RBPrefs.DeleteKey(key);
			}
			catch (Exception)
			{
				Debug.LogError("Error deleting PlayerPrefs " + key);
			}
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x00005651 File Offset: 0x00003851
		public override bool SaveFile(PersistentStorage.eFileType fileType, string fileName, string data, bool logExceptions = true)
		{
			return true;
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x000272CF File Offset: 0x000254CF
		public override void ForceSaveSettings()
		{
			RBPrefs.Save();
		}

		// Token: 0x060039B3 RID: 14771 RVA: 0x000272D6 File Offset: 0x000254D6
		public override bool HasSetting(string key)
		{
			return RBPrefs.HasKey(key);
		}
	}
}
