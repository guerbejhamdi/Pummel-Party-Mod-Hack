using System;
using System.Collections.Generic;

// Token: 0x020004B5 RID: 1205
public class PlayerStats
{
	// Token: 0x06002017 RID: 8215 RVA: 0x000C9B90 File Offset: 0x000C7D90
	public PlayerStats(short playerID)
	{
		this.m_playerID = playerID;
		foreach (object obj in Enum.GetValues(typeof(StatType)))
		{
			int type = (int)obj;
			this.m_stats.Add(new TrackedStat((StatType)type));
		}
	}

	// Token: 0x06002018 RID: 8216 RVA: 0x000C9C14 File Offset: 0x000C7E14
	public TrackedStat GetStat(StatType type)
	{
		if (type >= StatType.MinigamesWon && type < (StatType)this.m_stats.Count)
		{
			return this.m_stats[(int)type];
		}
		return null;
	}

	// Token: 0x06002019 RID: 8217 RVA: 0x000C9C44 File Offset: 0x000C7E44
	public void Reset()
	{
		foreach (TrackedStat trackedStat in this.m_stats)
		{
			trackedStat.Reset();
		}
	}

	// Token: 0x170003D2 RID: 978
	// (get) Token: 0x0600201A RID: 8218 RVA: 0x00017777 File Offset: 0x00015977
	public short PlayerID
	{
		get
		{
			return this.m_playerID;
		}
	}

	// Token: 0x040022F0 RID: 8944
	public static string[] statNames = new string[]
	{
		"Minigames Won",
		"Minigames Lost",
		"Damage Dealt",
		"Damage Recieved",
		"Keys Gained",
		"Keys Lost"
	};

	// Token: 0x040022F1 RID: 8945
	private short m_playerID;

	// Token: 0x040022F2 RID: 8946
	private List<TrackedStat> m_stats = new List<TrackedStat>();
}
