using System;

// Token: 0x0200043F RID: 1087
[Serializable]
public class PrefValue
{
	// Token: 0x06001DFB RID: 7675 RVA: 0x0001626A File Offset: 0x0001446A
	public PrefValue(string key, float value)
	{
		this.type = 0;
		this.key = key;
		this.val = value.ToString();
	}

	// Token: 0x06001DFC RID: 7676 RVA: 0x0001628D File Offset: 0x0001448D
	public PrefValue(string key, int value)
	{
		this.type = 1;
		this.key = key;
		this.val = value.ToString();
	}

	// Token: 0x06001DFD RID: 7677 RVA: 0x000162B0 File Offset: 0x000144B0
	public PrefValue(string key, string value)
	{
		this.type = 2;
		this.key = key;
		this.val = value;
	}

	// Token: 0x06001DFE RID: 7678 RVA: 0x000162CD File Offset: 0x000144CD
	public PrefType GetPrefType()
	{
		return (PrefType)this.type;
	}

	// Token: 0x06001DFF RID: 7679 RVA: 0x000C2B48 File Offset: 0x000C0D48
	public float GetFloatValue()
	{
		float result = 0f;
		float.TryParse(this.val, out result);
		return result;
	}

	// Token: 0x06001E00 RID: 7680 RVA: 0x000C2B6C File Offset: 0x000C0D6C
	public int GetIntValue()
	{
		int result = 0;
		int.TryParse(this.val, out result);
		return result;
	}

	// Token: 0x06001E01 RID: 7681 RVA: 0x000C2B8C File Offset: 0x000C0D8C
	public object GetValue()
	{
		switch (this.type)
		{
		case 0:
			return this.GetFloatValue();
		case 1:
			return this.GetIntValue();
		case 2:
			return this.val;
		default:
			return null;
		}
	}

	// Token: 0x040020D3 RID: 8403
	public byte type;

	// Token: 0x040020D4 RID: 8404
	public string key;

	// Token: 0x040020D5 RID: 8405
	public string val;
}
