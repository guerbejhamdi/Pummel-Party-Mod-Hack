using System;

// Token: 0x02000408 RID: 1032
public class RagdollSettings
{
	// Token: 0x06001CBE RID: 7358 RVA: 0x00015364 File Offset: 0x00013564
	public RagdollSettings()
	{
		this.despawn_effect = RagdollDespawnEffect.Sink;
		this.despawn_type = RagdollDespawnType.MaxRagdolls;
		this.max_ragdolls = 60;
		this.max_inactive_time = 8f;
		this.max_total_time = 16f;
	}

	// Token: 0x1700035F RID: 863
	// (get) Token: 0x06001CBF RID: 7359 RVA: 0x00015398 File Offset: 0x00013598
	// (set) Token: 0x06001CC0 RID: 7360 RVA: 0x000153A0 File Offset: 0x000135A0
	public RagdollDespawnEffect DespawnEffect
	{
		get
		{
			return this.despawn_effect;
		}
		set
		{
			this.despawn_effect = value;
		}
	}

	// Token: 0x17000360 RID: 864
	// (get) Token: 0x06001CC1 RID: 7361 RVA: 0x000153A9 File Offset: 0x000135A9
	// (set) Token: 0x06001CC2 RID: 7362 RVA: 0x000153B1 File Offset: 0x000135B1
	public RagdollDespawnType DespawnType
	{
		get
		{
			return this.despawn_type;
		}
		set
		{
			this.despawn_type = value;
		}
	}

	// Token: 0x17000361 RID: 865
	// (get) Token: 0x06001CC3 RID: 7363 RVA: 0x000153BA File Offset: 0x000135BA
	// (set) Token: 0x06001CC4 RID: 7364 RVA: 0x000153C2 File Offset: 0x000135C2
	public int MaxRagdolls
	{
		get
		{
			return this.max_ragdolls;
		}
		set
		{
			this.max_ragdolls = value;
		}
	}

	// Token: 0x17000362 RID: 866
	// (get) Token: 0x06001CC5 RID: 7365 RVA: 0x000153CB File Offset: 0x000135CB
	// (set) Token: 0x06001CC6 RID: 7366 RVA: 0x000153D3 File Offset: 0x000135D3
	public float MaxInactiveTime
	{
		get
		{
			return this.max_inactive_time;
		}
		set
		{
			this.max_inactive_time = value;
		}
	}

	// Token: 0x17000363 RID: 867
	// (get) Token: 0x06001CC7 RID: 7367 RVA: 0x000153DC File Offset: 0x000135DC
	// (set) Token: 0x06001CC8 RID: 7368 RVA: 0x000153D3 File Offset: 0x000135D3
	public float MaxTotalTime
	{
		get
		{
			return this.max_total_time;
		}
		set
		{
			this.max_inactive_time = value;
		}
	}

	// Token: 0x04001F34 RID: 7988
	private RagdollDespawnEffect despawn_effect;

	// Token: 0x04001F35 RID: 7989
	private RagdollDespawnType despawn_type;

	// Token: 0x04001F36 RID: 7990
	private int max_ragdolls;

	// Token: 0x04001F37 RID: 7991
	private float max_inactive_time;

	// Token: 0x04001F38 RID: 7992
	private float max_total_time;
}
