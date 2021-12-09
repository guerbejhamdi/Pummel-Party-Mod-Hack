using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004BC RID: 1212
public class StatTracker_Test : MonoBehaviour
{
	// Token: 0x0600202D RID: 8237 RVA: 0x000CA1E4 File Offset: 0x000C83E4
	private void Start()
	{
		StatTracker.OnStatChanged.AddListener(new UnityAction<StatType>(this.OnStatChanged));
		for (int i = 0; i < 16; i++)
		{
			StatTracker.IncrementStat(StatType.DamageDealt, (short)i, (double)UnityEngine.Random.value * 10.0);
		}
		this.m_lastUpdate = Time.time;
	}

	// Token: 0x0600202E RID: 8238 RVA: 0x0001780B File Offset: 0x00015A0B
	private void OnDestroy()
	{
		StatTracker.OnStatChanged.RemoveListener(new UnityAction<StatType>(this.OnStatChanged));
	}

	// Token: 0x0600202F RID: 8239 RVA: 0x000CA238 File Offset: 0x000C8438
	private void Update()
	{
		if (Time.time - this.m_lastUpdate > 1f)
		{
			for (int i = 0; i < 16; i++)
			{
				StatTracker.IncrementStat(StatType.DamageDealt, (short)i, (double)UnityEngine.Random.value * 10.0);
			}
			this.m_lastUpdate = Time.time;
		}
		if (this.m_statsChanged)
		{
			this.PrintStats();
			this.m_statsChanged = false;
		}
	}

	// Token: 0x06002030 RID: 8240 RVA: 0x000CA2A0 File Offset: 0x000C84A0
	private void PrintStats()
	{
		StatType statType = StatType.DamageDealt;
		List<PlayerStats> playerStats = StatTracker.GetPlayerStats(16, 16, statType, false, StatSortType.Descending);
		Debug.Log("------------------------");
		foreach (PlayerStats playerStats2 in playerStats)
		{
			Debug.Log(playerStats2.PlayerID.ToString() + " = " + playerStats2.GetStat(statType).Value.ToString());
		}
		Debug.Log("------------------------");
	}

	// Token: 0x06002031 RID: 8241 RVA: 0x00017823 File Offset: 0x00015A23
	private void OnStatChanged(StatType type)
	{
		this.m_statsChanged = true;
	}

	// Token: 0x040022FF RID: 8959
	private float m_lastUpdate;

	// Token: 0x04002300 RID: 8960
	private bool m_statsChanged;
}
