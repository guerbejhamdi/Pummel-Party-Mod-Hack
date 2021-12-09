using System;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200014C RID: 332
public class BattyBatterController : MinigameController
{
	// Token: 0x0600097D RID: 2429 RVA: 0x00055448 File Offset: 0x00053648
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		float num = (float)GameManager.GetPlayerCount();
		Vector3 vector = new Vector3(-2f, 3.213f, 0f);
		vector -= this.spacing * ((num - 1f) / 2f);
		this.currentBalls = new BattyBattersBall[(int)num];
		this.ballSpawnPoints = new Vector3[(int)num];
		this.ballHitPoints = new Vector3[(int)num];
		this.hitMarkers = new GameObject[(int)num];
		int num2 = 0;
		while ((float)num2 < num)
		{
			base.Root.transform.Find("SpawnPoints/Spawn" + (num2 + 1).ToString()).position = vector;
			this.ballHitPoints[num2] = vector + this.hitPointOffset;
			this.ballSpawnPoints[num2] = this.ballHitPoints[num2] + this.ballSpawnPointOffet;
			this.hitMarkers[num2] = base.Spawn(this.hitMarker, this.ballHitPoints[num2], Quaternion.identity);
			vector += this.spacing;
			num2++;
		}
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("BattyBatterPlayer", null);
		}
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x0005558C File Offset: 0x0005378C
	public override void StartMinigame()
	{
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		this.timer.last = Time.time - 2f;
		base.StartMinigame();
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x000555F0 File Offset: 0x000537F0
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer)
		{
			if (this.ui_timer.time_test <= 0f)
			{
				base.EndRound(1f, 3f, false);
				return;
			}
			if (this.timer.Elapsed(true))
			{
				this.DropBalls(this.curGravity);
				this.curGravity *= 1.04f;
			}
		}
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x0000A52B File Offset: 0x0000872B
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCDropBalls(NetPlayer sender, float gravity)
	{
		this.DropBalls(gravity);
	}

	// Token: 0x06000981 RID: 2433 RVA: 0x00055660 File Offset: 0x00053860
	private void DropBalls(float gravity)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCDropBalls", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				gravity
			});
		}
		for (int i = 0; i < this.ballSpawnPoints.Length; i++)
		{
			this.currentBalls[i] = base.Spawn(this.baseBallPrefab, this.ballSpawnPoints[i], Quaternion.Euler(ZPMath.RandomVec3(GameManager.rand, -360f, 360f))).GetComponent<BattyBattersBall>();
			this.currentBalls[i].gravity = gravity;
			UnityEngine.Object.Destroy(this.currentBalls[i], 6f);
		}
		for (int j = 0; j < this.players.Count; j++)
		{
			BattyBatterPlayer battyBatterPlayer = (BattyBatterPlayer)this.players[j];
			if (battyBatterPlayer.IsOwner)
			{
				battyBatterPlayer.canHit = true;
			}
		}
	}

	// Token: 0x0400081D RID: 2077
	public GameObject baseBallPrefab;

	// Token: 0x0400081E RID: 2078
	public GameObject hitMarker;

	// Token: 0x0400081F RID: 2079
	private Vector3 spacing = new Vector3(0f, 0f, 1.5f);

	// Token: 0x04000820 RID: 2080
	private Vector3 hitPointOffset = new Vector3(-0.45f, 0.077f, 0.75f);

	// Token: 0x04000821 RID: 2081
	public Vector3 ballSpawnPointOffet = new Vector3(0f, 2.2f, 0f);

	// Token: 0x04000822 RID: 2082
	public Vector3[] ballSpawnPoints;

	// Token: 0x04000823 RID: 2083
	public Vector3[] ballHitPoints;

	// Token: 0x04000824 RID: 2084
	public BattyBattersBall[] currentBalls;

	// Token: 0x04000825 RID: 2085
	public GameObject[] hitMarkers;

	// Token: 0x04000826 RID: 2086
	private ActionTimer timer = new ActionTimer(2.5f);

	// Token: 0x04000827 RID: 2087
	private float curGravity = 9.8f;
}
