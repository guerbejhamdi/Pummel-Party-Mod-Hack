using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Utility;

// Token: 0x020003B3 RID: 947
public class ActionSendStatistics : BoardAction
{
	// Token: 0x06001936 RID: 6454 RVA: 0x00012BDE File Offset: 0x00010DDE
	public ActionSendStatistics(bool collectStats)
	{
		this.action_type = BoardActionType.SendStatistics;
		if (collectStats)
		{
			this.m_stats = StatTracker.CollectStats();
		}
	}

	// Token: 0x06001937 RID: 6455 RVA: 0x000AA0DC File Offset: 0x000A82DC
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			StatTracker.SerializeStats(bs, this.m_stats);
			Debug.Log("Serialized statistics to " + bs.Length.ToString());
			return;
		}
		this.m_stats = StatTracker.DeserializeStats(bs);
	}

	// Token: 0x06001938 RID: 6456 RVA: 0x00012C07 File Offset: 0x00010E07
	public void ApplyStats()
	{
		StatTracker.ApplyStats(this.m_stats);
	}

	// Token: 0x04001AD3 RID: 6867
	public Dictionary<short, List<StatInfo>> m_stats = new Dictionary<short, List<StatInfo>>();
}
