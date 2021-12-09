using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Utility;

// Token: 0x020004BA RID: 1210
public class StatTracker : MonoBehaviour
{
	// Token: 0x0600201F RID: 8223 RVA: 0x000C9CE0 File Offset: 0x000C7EE0
	static StatTracker()
	{
		if (!StatTracker.initialized)
		{
			for (int i = 0; i < 8; i++)
			{
				StatTracker.m_trackedPlayers.Add(new PlayerStats((short)i));
			}
			StatTracker.OnStatChanged = new OnStatChangedEvent();
			StatTracker.initialized = true;
		}
	}

	// Token: 0x06002020 RID: 8224 RVA: 0x000C9D34 File Offset: 0x000C7F34
	public static void SetStat(StatType type, short playerId, double value)
	{
		TrackedStat trackedStat = StatTracker.GetTrackedStat(type, playerId);
		if (trackedStat == null)
		{
			return;
		}
		trackedStat.Value = value;
		StatTracker.OnStatChanged.Invoke(type);
	}

	// Token: 0x06002021 RID: 8225 RVA: 0x000C9D60 File Offset: 0x000C7F60
	public static double GetStat(StatType type, short playerId)
	{
		TrackedStat trackedStat = StatTracker.GetTrackedStat(type, playerId);
		if (trackedStat == null)
		{
			return 0.0;
		}
		return trackedStat.Value;
	}

	// Token: 0x06002022 RID: 8226 RVA: 0x000C9D88 File Offset: 0x000C7F88
	public static void IncrementStat(StatType type, short playerId, double value)
	{
		TrackedStat trackedStat = StatTracker.GetTrackedStat(type, playerId);
		if (trackedStat == null)
		{
			return;
		}
		trackedStat.Value += value;
		StatTracker.OnStatChanged.Invoke(type);
	}

	// Token: 0x06002023 RID: 8227 RVA: 0x000C9DBC File Offset: 0x000C7FBC
	public static void CompareStat(StatType type, StatCompare comparison, short playerId, double value)
	{
		TrackedStat trackedStat = StatTracker.GetTrackedStat(type, playerId);
		if (trackedStat == null)
		{
			return;
		}
		if (comparison != StatCompare.Lesser)
		{
			if (comparison == StatCompare.Greater)
			{
				trackedStat.Value = ((value > trackedStat.Value) ? value : trackedStat.Value);
			}
		}
		else
		{
			trackedStat.Value = ((value < trackedStat.Value) ? value : trackedStat.Value);
		}
		StatTracker.OnStatChanged.Invoke(type);
	}

	// Token: 0x06002024 RID: 8228 RVA: 0x000C9E1C File Offset: 0x000C801C
	public static List<PlayerStats> GetPlayerStats(int numResults, int activePlayers, StatType orderedBy, bool returnTies = false, StatSortType sortType = StatSortType.Descending)
	{
		List<PlayerStats> list = new List<PlayerStats>();
		int num = 0;
		while (num < activePlayers && num < StatTracker.m_trackedPlayers.Count)
		{
			list.Add(StatTracker.m_trackedPlayers[num]);
			num++;
		}
		list.Sort(new PlayerStatsComparer(orderedBy));
		if (sortType == StatSortType.Ascending)
		{
			list.Reverse();
		}
		if (list.Count == numResults || list.Count < numResults)
		{
			return list;
		}
		if (returnTies)
		{
			int num2 = 0;
			double value = list[0].GetStat(orderedBy).Value;
			for (int i = 1; i < list.Count; i++)
			{
				if (value == list[i].GetStat(orderedBy).Value)
				{
					num2++;
				}
			}
			Debug.Log("There were " + num2.ToString() + " ties");
			int count = Mathf.Max(numResults, num2 + 1);
			return list.GetRange(0, count);
		}
		return list.GetRange(0, numResults);
	}

	// Token: 0x06002025 RID: 8229 RVA: 0x000177D3 File Offset: 0x000159D3
	private static TrackedStat GetTrackedStat(StatType type, short playerId)
	{
		if ((int)playerId < StatTracker.m_trackedPlayers.Count)
		{
			return StatTracker.m_trackedPlayers[(int)playerId].GetStat(type);
		}
		return null;
	}

	// Token: 0x06002026 RID: 8230 RVA: 0x000C9F08 File Offset: 0x000C8108
	public static void ResetStats()
	{
		foreach (PlayerStats playerStats in StatTracker.m_trackedPlayers)
		{
			playerStats.Reset();
		}
		StatTracker.OnStatChanged.Invoke(StatType.DamageDealt);
		StatTracker.OnStatChanged.RemoveAllListeners();
	}

	// Token: 0x06002027 RID: 8231 RVA: 0x000C9F6C File Offset: 0x000C816C
	public static Dictionary<short, List<StatInfo>> CollectStats()
	{
		List<GamePlayer> playerList = GameManager.PlayerList;
		Dictionary<short, List<StatInfo>> dictionary = new Dictionary<short, List<StatInfo>>();
		foreach (GamePlayer gamePlayer in playerList)
		{
			List<StatInfo> list = new List<StatInfo>();
			int length = Enum.GetValues(typeof(StatType)).Length;
			for (int i = 0; i < length; i++)
			{
				StatType type = (StatType)i;
				float value = (float)StatTracker.GetStat(type, gamePlayer.GlobalID);
				list.Add(new StatInfo(type, value));
			}
			dictionary.Add(gamePlayer.GlobalID, list);
		}
		return dictionary;
	}

	// Token: 0x06002028 RID: 8232 RVA: 0x000CA01C File Offset: 0x000C821C
	public static void SerializeStats(ZPBitStream bs, Dictionary<short, List<StatInfo>> m_stats)
	{
		bs.Write((byte)m_stats.Count);
		foreach (KeyValuePair<short, List<StatInfo>> keyValuePair in m_stats)
		{
			bs.Write(keyValuePair.Key);
			bs.Write((byte)keyValuePair.Value.Count);
			foreach (StatInfo statInfo in keyValuePair.Value)
			{
				bs.Write(statInfo.value);
			}
		}
	}

	// Token: 0x06002029 RID: 8233 RVA: 0x000CA0D8 File Offset: 0x000C82D8
	public static Dictionary<short, List<StatInfo>> DeserializeStats(ZPBitStream bs)
	{
		Dictionary<short, List<StatInfo>> dictionary = new Dictionary<short, List<StatInfo>>();
		int num = (int)bs.ReadByte();
		for (int i = 0; i < num; i++)
		{
			short key = bs.ReadShort();
			List<StatInfo> list = new List<StatInfo>();
			int num2 = (int)bs.ReadByte();
			for (int j = 0; j < num2; j++)
			{
				float value = bs.ReadFloat();
				list.Add(new StatInfo((StatType)j, value));
			}
			dictionary.Add(key, list);
		}
		return dictionary;
	}

	// Token: 0x0600202A RID: 8234 RVA: 0x000CA148 File Offset: 0x000C8348
	public static void ApplyStats(Dictionary<short, List<StatInfo>> m_stats)
	{
		foreach (KeyValuePair<short, List<StatInfo>> keyValuePair in m_stats)
		{
			foreach (StatInfo statInfo in keyValuePair.Value)
			{
				StatTracker.SetStat(statInfo.type, keyValuePair.Key, (double)statInfo.value);
			}
		}
	}

	// Token: 0x040022FA RID: 8954
	private static List<PlayerStats> m_trackedPlayers = new List<PlayerStats>();

	// Token: 0x040022FB RID: 8955
	private static bool initialized = false;

	// Token: 0x040022FC RID: 8956
	public static OnStatChangedEvent OnStatChanged;
}
