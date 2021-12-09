using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000458 RID: 1112
public class IDMapping
{
	// Token: 0x06001E72 RID: 7794 RVA: 0x0001677D File Offset: 0x0001497D
	public bool TryGetMapping(string key, ref int id)
	{
		return this.idMap.TryGetValue(key, out id);
	}

	// Token: 0x06001E73 RID: 7795 RVA: 0x000C5280 File Offset: 0x000C3480
	public int CreateMapping(string key)
	{
		int nextID = this.GetNextID();
		this.idMap.Add(key, nextID);
		return nextID;
	}

	// Token: 0x06001E74 RID: 7796 RVA: 0x000C52A4 File Offset: 0x000C34A4
	public bool SaveMapping(string file)
	{
		bool result;
		try
		{
			string text = "";
			foreach (KeyValuePair<string, int> keyValuePair in this.idMap)
			{
				text = string.Concat(new string[]
				{
					text,
					keyValuePair.Key,
					",",
					keyValuePair.Value.ToString(),
					"\n"
				});
			}
			File.WriteAllText(file, text);
			result = true;
		}
		catch (Exception)
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06001E75 RID: 7797 RVA: 0x000C5354 File Offset: 0x000C3554
	public bool LoadMapping(string file)
	{
		bool result;
		try
		{
			string[] array = File.ReadAllText(file).Split(new char[]
			{
				'\n'
			});
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[]
				{
					','
				});
				int value = 0;
				if (int.TryParse(array2[1], out value))
				{
					this.idMap.Add(array2[0], value);
				}
			}
			result = true;
		}
		catch (Exception)
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06001E76 RID: 7798 RVA: 0x000C53D4 File Offset: 0x000C35D4
	private int GetNextID()
	{
		int num = 0;
		foreach (KeyValuePair<string, int> keyValuePair in this.idMap)
		{
			num = Mathf.Max(keyValuePair.Value, num);
		}
		return num + 1;
	}

	// Token: 0x0400215B RID: 8539
	private Dictionary<string, int> idMap = new Dictionary<string, int>();
}
