using System;
using UnityEngine;
using ZP.Net;

// Token: 0x020001FE RID: 510
public class PlanesRaceController : MinigameController
{
	// Token: 0x17000151 RID: 337
	// (get) Token: 0x06000EF7 RID: 3831 RVA: 0x0000D042 File Offset: 0x0000B242
	public float CourseLength
	{
		get
		{
			return this.m_courseLength;
		}
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x0007791C File Offset: 0x00075B1C
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.CheckpointsRoot = base.Root.transform.Find("CheckPoints");
		this.CoursePathRoot = base.Root.transform.Find("CoursePath");
		this.m_courseLength = 0f;
		Transform coursePathRoot = this.CoursePathRoot;
		for (int i = 0; i < coursePathRoot.childCount; i++)
		{
			Transform child = coursePathRoot.GetChild(i);
			Transform child2 = coursePathRoot.GetChild((i + 1 >= coursePathRoot.childCount) ? 0 : (i + 1));
			float num = Vector3.Distance(child.position, child2.position);
			this.m_courseLength += num;
		}
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("PlanesRacePlayer", null);
		}
	}

	// Token: 0x06000EF9 RID: 3833 RVA: 0x00009BEC File Offset: 0x00007DEC
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.StartMinigame();
	}

	// Token: 0x06000EFA RID: 3834 RVA: 0x0000AB1C File Offset: 0x00008D1C
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer && this.ui_timer.time_test <= 0f)
		{
			base.EndRound(1f, 3f, false);
		}
	}

	// Token: 0x06000EFB RID: 3835 RVA: 0x000779DC File Offset: 0x00075BDC
	public override void BuildResults()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			PlanesRacePlayer planesRacePlayer = (PlanesRacePlayer)this.players[i];
			this.players[i].GamePlayer.MinigameScore = (int)(10000f * ((float)this.players[i].Score + planesRacePlayer.GetDistanceAlongCourse()));
		}
	}

	// Token: 0x04000EB9 RID: 3769
	public Transform CheckpointsRoot;

	// Token: 0x04000EBA RID: 3770
	public Transform CoursePathRoot;

	// Token: 0x04000EBB RID: 3771
	private float m_courseLength;
}
