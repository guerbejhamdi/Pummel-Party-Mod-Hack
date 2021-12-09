using System;

// Token: 0x020004BB RID: 1211
public class StatInfo
{
	// Token: 0x0600202C RID: 8236 RVA: 0x000177F5 File Offset: 0x000159F5
	public StatInfo(StatType type, float value)
	{
		this.type = type;
		this.value = value;
	}

	// Token: 0x040022FD RID: 8957
	public StatType type;

	// Token: 0x040022FE RID: 8958
	public float value;
}
