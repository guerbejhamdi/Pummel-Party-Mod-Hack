using System;
using System.Collections.Generic;

// Token: 0x020004B6 RID: 1206
public class PlayerStatsComparer : IComparer<PlayerStats>
{
	// Token: 0x0600201C RID: 8220 RVA: 0x000177BC File Offset: 0x000159BC
	public PlayerStatsComparer(StatType orderedBy)
	{
		this.m_orderedBy = orderedBy;
	}

	// Token: 0x0600201D RID: 8221 RVA: 0x000C9C94 File Offset: 0x000C7E94
	public int Compare(PlayerStats x, PlayerStats y)
	{
		if (y == null)
		{
			return 1;
		}
		if (x == null && y == null)
		{
			return 0;
		}
		if (x == null)
		{
			return -1;
		}
		TrackedStat stat = x.GetStat(this.m_orderedBy);
		return y.GetStat(this.m_orderedBy).Value.CompareTo(stat.Value);
	}

	// Token: 0x040022F3 RID: 8947
	private StatType m_orderedBy;
}
