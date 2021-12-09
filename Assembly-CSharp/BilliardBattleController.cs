using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000151 RID: 337
public class BilliardBattleController : MinigameController
{
	// Token: 0x060009A3 RID: 2467 RVA: 0x00056558 File Offset: 0x00054758
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("BilliardBattlePlayer", null);
			Transform transform = base.Root.transform.Find("BallSpawnPoints");
			int num = (GameManager.GetPlayerCount() > 4) ? transform.childCount : 15;
			for (int i = 0; i < num; i++)
			{
				this.balls.Add(base.NetSpawn("BilliardBall", transform.GetChild(i).position, 0, null).GetComponent<BilliardBall>());
			}
		}
		this.ballMaterials = new Material[GameManager.GetPlayerCount()];
		for (int j = 0; j < GameManager.GetPlayerCount(); j++)
		{
			this.ballMaterials[j] = new Material(this.ballMaterial);
			float h;
			float num2;
			float v;
			Color.RGBToHSV(GameManager.GetPlayerAt(j).Color.skinColor1, out h, out num2, out v);
			num2 = Mathf.Clamp01(num2 - 0.1f);
			this.ballMaterials[j].color = Color.HSVToRGB(h, num2, v);
		}
		this.pocketPositionsRoot = base.Root.transform.Find("PocketPositionsRoot");
		Transform transform2 = base.Root.transform.Find("AITargetPoints");
		this.aiTargetPoints = new Transform[transform2.childCount];
		for (int k = 0; k < this.aiTargetPoints.Length; k++)
		{
			this.aiTargetPoints[k] = transform2.GetChild(k);
		}
		this.startGravity = Physics.gravity;
		Physics.gravity = this.gravity;
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x000566D8 File Offset: 0x000548D8
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		this.bars = new BilliardBattleBar[GameManager.GetPlayerCount()];
		for (int i = 0; i < this.bars.Length; i++)
		{
			this.bars[i] = UnityEngine.Object.Instantiate<GameObject>(this.statusBar).GetComponent<BilliardBattleBar>();
			this.bars[i].transform.SetParent(GameManager.UIController.MinigameUIRoot, false);
			this.bars[i].minigameController = this;
			this.bars[i].player = (BilliardBattlePlayer)base.GetPlayer(i);
		}
		base.StartMinigame();
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x000567A4 File Offset: 0x000549A4
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer)
		{
			int num = 0;
			for (int i = 0; i < this.balls.Count; i++)
			{
				if (!this.balls[i].Pocketed)
				{
					num++;
				}
			}
			if (num == 0 || this.ui_timer.time_test <= 0f)
			{
				base.EndRound(1f, 1f, false);
			}
		}
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x0000A236 File Offset: 0x00008436
	public override void BuildResults()
	{
		base.BuildResults();
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x0000A68A File Offset: 0x0000888A
	public override void ReleaseMinigame()
	{
		Physics.gravity = this.startGravity;
		base.ReleaseMinigame();
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x00056818 File Offset: 0x00054A18
	public void Pocketed(byte player)
	{
		CharacterBase player2 = base.GetPlayer((int)player);
		short score = player2.Score;
		player2.Score = score + 1;
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x0005683C File Offset: 0x00054A3C
	public Vector3 GetFreePosition()
	{
		int num = 1000;
		int i = 0;
		Vector3 vector = Vector3.zero;
		while (i < num)
		{
			vector = ZPMath.RandomVec3(GameManager.rand, new Vector3(-20f, 0f, -7.5f), new Vector3(20f, 0f, 7.5f));
			bool flag = false;
			for (int j = 0; j < this.balls.Count; j++)
			{
				if ((vector - this.balls[j].transform.position).sqrMagnitude < 4f)
				{
					flag = true;
					break;
				}
			}
			for (int k = 0; k < base.GetPlayerCount(); k++)
			{
				if ((vector - base.GetPlayer(k).transform.position).sqrMagnitude < 4f)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return vector;
			}
			i++;
		}
		return vector;
	}

	// Token: 0x0400085A RID: 2138
	public Vector3 gravity = new Vector3(0f, -50f, 0f);

	// Token: 0x0400085B RID: 2139
	public GameObject statusBar;

	// Token: 0x0400085C RID: 2140
	public BilliardBattleBar[] bars;

	// Token: 0x0400085D RID: 2141
	public Material ballMaterial;

	// Token: 0x0400085E RID: 2142
	public Material[] ballMaterials;

	// Token: 0x0400085F RID: 2143
	public Transform pocketPositionsRoot;

	// Token: 0x04000860 RID: 2144
	public Transform[] aiTargetPoints;

	// Token: 0x04000861 RID: 2145
	public static readonly float minX = -23.3f;

	// Token: 0x04000862 RID: 2146
	public static readonly float maxX = 23.3f;

	// Token: 0x04000863 RID: 2147
	public static readonly float minY = -2.55f;

	// Token: 0x04000864 RID: 2148
	public static readonly float maxY = 0.05f;

	// Token: 0x04000865 RID: 2149
	public static readonly float minZ = -12.1f;

	// Token: 0x04000866 RID: 2150
	public static readonly float maxZ = 12.1f;

	// Token: 0x04000867 RID: 2151
	public static readonly float maxNetY = 1.5f;

	// Token: 0x04000868 RID: 2152
	private Vector3 startGravity = Vector3.zero;

	// Token: 0x04000869 RID: 2153
	public List<BilliardBall> balls = new List<BilliardBall>();
}
