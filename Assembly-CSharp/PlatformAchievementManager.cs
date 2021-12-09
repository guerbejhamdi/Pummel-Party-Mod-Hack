using System;
using Steamworks;

// Token: 0x020002CA RID: 714
public class PlatformAchievementManager
{
	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x0600145D RID: 5213 RVA: 0x0000FEB3 File Offset: 0x0000E0B3
	public static PlatformAchievementManager Instance
	{
		get
		{
			if (PlatformAchievementManager.instance == null)
			{
				PlatformAchievementManager.instance = new PlatformAchievementManager();
			}
			return PlatformAchievementManager.instance;
		}
	}

	// Token: 0x0600145E RID: 5214 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Initialize()
	{
	}

	// Token: 0x0600145F RID: 5215 RVA: 0x0000FECB File Offset: 0x0000E0CB
	public virtual void TriggerAchievement(string achievementID)
	{
		if (SteamManager.Initialized)
		{
			SteamUserStats.SetAchievement(achievementID);
			SteamUserStats.StoreStats();
		}
	}

	// Token: 0x040015AA RID: 5546
	private static PlatformAchievementManager instance;
}
