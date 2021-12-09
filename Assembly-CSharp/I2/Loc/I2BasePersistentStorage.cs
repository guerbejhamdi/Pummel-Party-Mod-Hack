using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020007EE RID: 2030
	public abstract class I2BasePersistentStorage
	{
		// Token: 0x060039A2 RID: 14754 RVA: 0x00120870 File Offset: 0x0011EA70
		public virtual void SetSetting_String(string key, string value)
		{
			try
			{
				int length = value.Length;
				int num = 8000;
				if (length <= num)
				{
					PlayerPrefs.SetString(key, value);
				}
				else
				{
					int num2 = Mathf.CeilToInt((float)length / (float)num);
					for (int i = 0; i < num2; i++)
					{
						int num3 = num * i;
						PlayerPrefs.SetString(string.Format("[I2split]{0}{1}", i, key), value.Substring(num3, Mathf.Min(num, length - num3)));
					}
					PlayerPrefs.SetString(key, "[$I2#@div$]" + num2.ToString());
				}
			}
			catch (Exception)
			{
				Debug.LogError("Error saving PlayerPrefs " + key);
			}
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x00120918 File Offset: 0x0011EB18
		public virtual string GetSetting_String(string key, string defaultValue)
		{
			string result;
			try
			{
				string text = PlayerPrefs.GetString(key, defaultValue);
				if (!string.IsNullOrEmpty(text) && text.StartsWith("[I2split]"))
				{
					int num = int.Parse(text.Substring("[I2split]".Length));
					text = "";
					for (int i = 0; i < num; i++)
					{
						text += PlayerPrefs.GetString(string.Format("[I2split]{0}{1}", i, key), "");
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

		// Token: 0x060039A4 RID: 14756 RVA: 0x001209B8 File Offset: 0x0011EBB8
		public virtual void DeleteSetting(string key)
		{
			try
			{
				string @string = PlayerPrefs.GetString(key, null);
				if (!string.IsNullOrEmpty(@string) && @string.StartsWith("[I2split]"))
				{
					int num = int.Parse(@string.Substring("[I2split]".Length));
					for (int i = 0; i < num; i++)
					{
						PlayerPrefs.DeleteKey(string.Format("[I2split]{0}{1}", i, key));
					}
				}
				PlayerPrefs.DeleteKey(key);
			}
			catch (Exception)
			{
				Debug.LogError("Error deleting PlayerPrefs " + key);
			}
		}

		// Token: 0x060039A5 RID: 14757 RVA: 0x00016367 File Offset: 0x00014567
		public virtual void ForceSaveSettings()
		{
			PlayerPrefs.Save();
		}

		// Token: 0x060039A6 RID: 14758 RVA: 0x000272C7 File Offset: 0x000254C7
		public virtual bool HasSetting(string key)
		{
			return PlayerPrefs.HasKey(key);
		}

		// Token: 0x060039A7 RID: 14759 RVA: 0x00005651 File Offset: 0x00003851
		public virtual bool CanAccessFiles()
		{
			return true;
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x00120A48 File Offset: 0x0011EC48
		private string UpdateFilename(PersistentStorage.eFileType fileType, string fileName)
		{
			switch (fileType)
			{
			case PersistentStorage.eFileType.Persistent:
				fileName = Application.persistentDataPath + "/" + fileName;
				break;
			case PersistentStorage.eFileType.Temporal:
				fileName = Application.temporaryCachePath + "/" + fileName;
				break;
			case PersistentStorage.eFileType.Streaming:
				fileName = Application.streamingAssetsPath + "/" + fileName;
				break;
			}
			return fileName;
		}

		// Token: 0x060039A9 RID: 14761 RVA: 0x00120AA8 File Offset: 0x0011ECA8
		public virtual bool SaveFile(PersistentStorage.eFileType fileType, string fileName, string data, bool logExceptions = true)
		{
			if (!this.CanAccessFiles())
			{
				return false;
			}
			bool result;
			try
			{
				fileName = this.UpdateFilename(fileType, fileName);
				File.WriteAllText(fileName, data, Encoding.UTF8);
				result = true;
			}
			catch (Exception ex)
			{
				if (logExceptions)
				{
					string str = "Error saving file '";
					string str2 = fileName;
					string str3 = "'\n";
					Exception ex2 = ex;
					Debug.LogError(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060039AA RID: 14762 RVA: 0x00120B18 File Offset: 0x0011ED18
		public virtual string LoadFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (!this.CanAccessFiles())
			{
				return null;
			}
			string result;
			try
			{
				fileName = this.UpdateFilename(fileType, fileName);
				result = File.ReadAllText(fileName, Encoding.UTF8);
			}
			catch (Exception ex)
			{
				if (logExceptions)
				{
					string str = "Error loading file '";
					string str2 = fileName;
					string str3 = "'\n";
					Exception ex2 = ex;
					Debug.LogError(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
				result = null;
			}
			return result;
		}

		// Token: 0x060039AB RID: 14763 RVA: 0x00120B84 File Offset: 0x0011ED84
		public virtual bool DeleteFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (!this.CanAccessFiles())
			{
				return false;
			}
			bool result;
			try
			{
				fileName = this.UpdateFilename(fileType, fileName);
				File.Delete(fileName);
				result = true;
			}
			catch (Exception ex)
			{
				if (logExceptions)
				{
					string str = "Error deleting file '";
					string str2 = fileName;
					string str3 = "'\n";
					Exception ex2 = ex;
					Debug.LogError(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x00120BEC File Offset: 0x0011EDEC
		public virtual bool HasFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (!this.CanAccessFiles())
			{
				return false;
			}
			bool result;
			try
			{
				fileName = this.UpdateFilename(fileType, fileName);
				result = File.Exists(fileName);
			}
			catch (Exception ex)
			{
				if (logExceptions)
				{
					string str = "Error requesting file '";
					string str2 = fileName;
					string str3 = "'\n";
					Exception ex2 = ex;
					Debug.LogError(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
				result = false;
			}
			return result;
		}
	}
}
