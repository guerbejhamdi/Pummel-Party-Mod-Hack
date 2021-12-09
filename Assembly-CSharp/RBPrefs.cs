using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZP.Utility;

// Token: 0x02000442 RID: 1090
public class RBPrefs
{
	// Token: 0x06001E03 RID: 7683 RVA: 0x000162E8 File Offset: 0x000144E8
	public static bool HasKey(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	// Token: 0x06001E04 RID: 7684 RVA: 0x000162F0 File Offset: 0x000144F0
	public static void DeleteKey(string key)
	{
		PlayerPrefs.DeleteKey(key);
	}

	// Token: 0x06001E05 RID: 7685 RVA: 0x000162F8 File Offset: 0x000144F8
	public static float GetFloat(string key, float defaultValue = 0f)
	{
		return PlayerPrefs.GetFloat(key, defaultValue);
	}

	// Token: 0x06001E06 RID: 7686 RVA: 0x00016301 File Offset: 0x00014501
	public static int GetInt(string key, int defaultValue = 0)
	{
		return PlayerPrefs.GetInt(key, defaultValue);
	}

	// Token: 0x06001E07 RID: 7687 RVA: 0x0001630A File Offset: 0x0001450A
	public static string GetString(string key, string defaultValue = "")
	{
		return PlayerPrefs.GetString(key, defaultValue);
	}

	// Token: 0x06001E08 RID: 7688 RVA: 0x000C2BD4 File Offset: 0x000C0DD4
	private static T GetValue<T>(string key, T defaultValue)
	{
		object obj = defaultValue;
		if (RBPrefs.objMap.TryGetValue(key, out obj))
		{
			return (T)((object)obj);
		}
		return defaultValue;
	}

	// Token: 0x06001E09 RID: 7689 RVA: 0x00016313 File Offset: 0x00014513
	public static void SetFloat(string key, float value)
	{
		PlayerPrefs.SetFloat(key, value);
	}

	// Token: 0x06001E0A RID: 7690 RVA: 0x0001631C File Offset: 0x0001451C
	public static void SetInt(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
	}

	// Token: 0x06001E0B RID: 7691 RVA: 0x00016325 File Offset: 0x00014525
	public static void SetString(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
	}

	// Token: 0x06001E0C RID: 7692 RVA: 0x0001632E File Offset: 0x0001452E
	private static void SetValue<T>(string key, T value)
	{
		if (RBPrefs.objMap.ContainsKey(key))
		{
			RBPrefs.objMap[key] = value;
			return;
		}
		RBPrefs.objMap.Add(key, value);
	}

	// Token: 0x06001E0D RID: 7693 RVA: 0x0000398C File Offset: 0x00001B8C
	public static void Initialize()
	{
	}

	// Token: 0x06001E0E RID: 7694 RVA: 0x00016360 File Offset: 0x00014560
	public static void OnStorageLoaded()
	{
		RBPrefs.Load();
	}

	// Token: 0x06001E0F RID: 7695 RVA: 0x00016367 File Offset: 0x00014567
	public static void Save()
	{
		PlayerPrefs.Save();
	}

	// Token: 0x06001E10 RID: 7696 RVA: 0x0000398C File Offset: 0x00001B8C
	public static void Load()
	{
	}

	// Token: 0x06001E11 RID: 7697 RVA: 0x000C2C00 File Offset: 0x000C0E00
	public static bool LoadFromJSON(string data)
	{
		RBJSONPrefs rbjsonprefs = JsonUtility.FromJson<RBJSONPrefs>(data);
		if (rbjsonprefs != null)
		{
			RBPrefs.objMap.Clear();
			foreach (PrefValue prefValue in rbjsonprefs.values)
			{
				RBPrefs.objMap.Add(prefValue.key, prefValue.GetValue());
			}
			return true;
		}
		return false;
	}

	// Token: 0x06001E12 RID: 7698 RVA: 0x000C2C7C File Offset: 0x000C0E7C
	public static string SaveToJSON()
	{
		RBJSONPrefs rbjsonprefs = new RBJSONPrefs();
		foreach (KeyValuePair<string, object> keyValuePair in RBPrefs.objMap)
		{
			Type type = keyValuePair.Value.GetType();
			if (type == typeof(float))
			{
				rbjsonprefs.values.Add(new PrefValue(keyValuePair.Key, (float)keyValuePair.Value));
			}
			else if (type == typeof(int))
			{
				rbjsonprefs.values.Add(new PrefValue(keyValuePair.Key, (int)keyValuePair.Value));
			}
			else if (type == typeof(string))
			{
				rbjsonprefs.values.Add(new PrefValue(keyValuePair.Key, (string)keyValuePair.Value));
			}
		}
		string text = JsonUtility.ToJson(rbjsonprefs, true);
		Debug.LogError("JSON : " + text);
		return text;
	}

	// Token: 0x06001E13 RID: 7699 RVA: 0x000C2DA0 File Offset: 0x000C0FA0
	public static bool LoadFromBytes(byte[] data)
	{
		bool result;
		try
		{
			RBPrefs.objMap.Clear();
			ZPBitStream zpbitStream = new ZPBitStream(data, data.Length * 8);
			ushort num = zpbitStream.ReadUShort();
			int i = 0;
			while (i < (int)num)
			{
				PrefType prefType = (PrefType)zpbitStream.ReadByte();
				string key = zpbitStream.ReadString();
				switch (prefType)
				{
				case PrefType.Float:
				{
					object value = zpbitStream.ReadFloat();
					goto IL_6D;
				}
				case PrefType.Int:
				{
					object value = zpbitStream.ReadInt();
					goto IL_6D;
				}
				case PrefType.String:
				{
					object value = zpbitStream.ReadUnicodeString();
					goto IL_6D;
				}
				}
				IL_89:
				i++;
				continue;
				IL_6D:
				if (!RBPrefs.objMap.ContainsKey(key))
				{
					object value;
					RBPrefs.objMap.Add(key, value);
					goto IL_89;
				}
				goto IL_89;
			}
			result = true;
		}
		catch (Exception ex)
		{
			Debug.LogError("Error loading preferences from bytes : " + ex.Message);
			result = false;
		}
		return result;
	}

	// Token: 0x06001E14 RID: 7700 RVA: 0x000C2E74 File Offset: 0x000C1074
	public static byte[] SaveToBytes()
	{
		byte[] result;
		try
		{
			ZPBitStream zpbitStream = new ZPBitStream();
			zpbitStream.Write((ushort)RBPrefs.objMap.Count);
			foreach (KeyValuePair<string, object> keyValuePair in RBPrefs.objMap)
			{
				if (keyValuePair.Value == null)
				{
					Debug.LogError("RBPrefs : Error value is null = " + keyValuePair.Key);
				}
				else
				{
					Type type = keyValuePair.Value.GetType();
					if (type == typeof(float))
					{
						zpbitStream.Write(0);
						zpbitStream.Write(keyValuePair.Key);
						zpbitStream.Write((float)keyValuePair.Value);
					}
					else if (type == typeof(int))
					{
						zpbitStream.Write(1);
						zpbitStream.Write(keyValuePair.Key);
						zpbitStream.Write((int)keyValuePair.Value);
					}
					else if (type == typeof(string))
					{
						zpbitStream.Write(2);
						zpbitStream.Write(keyValuePair.Key);
						zpbitStream.WriteUnicode((string)keyValuePair.Value);
					}
				}
			}
			result = zpbitStream.GetDataCopy();
		}
		catch (Exception ex)
		{
			Debug.LogError("Error saving preferences to bytes : " + ex.Message);
			result = null;
		}
		return result;
	}

	// Token: 0x06001E15 RID: 7701 RVA: 0x000C3008 File Offset: 0x000C1208
	private static string GetSaveDirectory()
	{
		string text = Application.persistentDataPath + "/SaveData/";
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		return text;
	}

	// Token: 0x040020DB RID: 8411
	private static bool json = false;

	// Token: 0x040020DC RID: 8412
	private static Dictionary<string, object> objMap = new Dictionary<string, object>();
}
