using System;

// Token: 0x020004B4 RID: 1204
public class TrackedStat
{
	// Token: 0x06002011 RID: 8209 RVA: 0x00017726 File Offset: 0x00015926
	public TrackedStat(StatType type)
	{
		this.Type = type;
		this.m_value = 0.0;
	}

	// Token: 0x170003D0 RID: 976
	// (get) Token: 0x06002012 RID: 8210 RVA: 0x00017744 File Offset: 0x00015944
	// (set) Token: 0x06002013 RID: 8211 RVA: 0x0001774C File Offset: 0x0001594C
	public double Value
	{
		get
		{
			return this.m_value;
		}
		set
		{
			this.m_value = value;
		}
	}

	// Token: 0x170003D1 RID: 977
	// (get) Token: 0x06002014 RID: 8212 RVA: 0x00017755 File Offset: 0x00015955
	// (set) Token: 0x06002015 RID: 8213 RVA: 0x0001775D File Offset: 0x0001595D
	public StatType Type { get; set; }

	// Token: 0x06002016 RID: 8214 RVA: 0x00017766 File Offset: 0x00015966
	public void Reset()
	{
		this.Value = 0.0;
	}

	// Token: 0x040022EE RID: 8942
	private double m_value;
}
