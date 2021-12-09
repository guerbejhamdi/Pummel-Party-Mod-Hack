using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x02000166 RID: 358
public class BoundingBlocksController : MinigameController
{
	// Token: 0x06000A42 RID: 2626 RVA: 0x0005AA34 File Offset: 0x00058C34
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("BoundingBlocksPlayer", null);
		}
		this.cameraShake = base.Root.GetComponentInChildren<CameraShake>();
		this.floorMaterials = new Material[GameManager.GetPlayerCount()];
		for (int i = 0; i < this.floorMaterials.Length; i++)
		{
			this.floorMaterials[i] = new Material(this.floorColourMat);
			this.floorMaterials[i].color = (GameManager.GetPlayerAt(i).Color.skinColor1 + new Color(0.1f, 0.1f, 0.1f)) * 1.5f;
		}
		base.Root.transform.Find("Colliders/Floor").transform.localScale = new Vector3((this.gridSize + 1f) * this.gridDimension, 1f, (this.gridSize + 1f) * this.gridDimension);
		Transform transform = base.Root.transform.Find("Colliders/LeftWall").transform;
		transform.transform.localScale = new Vector3(1f, 5f, (this.gridSize + 1f) * this.gridDimension);
		transform.transform.position = new Vector3((-this.gridSize / 2f - 0.5f) * this.gridDimension, 0f, 0f);
		Transform transform2 = base.Root.transform.Find("Colliders/RightWall").transform;
		transform2.transform.localScale = new Vector3(1f, 5f, this.gridSize + 1f);
		transform2.transform.position = new Vector3((this.gridSize / 2f + 0.5f) * this.gridDimension, 0f, 0f);
		Transform transform3 = base.Root.transform.Find("Colliders/TopWall").transform;
		transform3.transform.localScale = new Vector3(this.gridSize + 1f, 5f, 1f);
		transform3.transform.position = new Vector3(0f, 0f, (this.gridSize / 2f + 0.5f) * this.gridDimension);
		Transform transform4 = base.Root.transform.Find("Colliders/BottomWall").transform;
		transform4.transform.localScale = new Vector3(this.gridSize + 1f, 5f, 1f);
		transform4.transform.position = new Vector3(0f, 0f, (-this.gridSize / 2f - 0.5f) * this.gridDimension);
		if (NetSystem.IsServer)
		{
			int num = GameManager.rand.Next();
			this.SpawnTiles(num);
			base.SendRPC("RPCSpawnTiles", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				num
			});
		}
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x0000AB13 File Offset: 0x00008D13
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSpawnTiles(NetPlayer sender, int seed)
	{
		this.SpawnTiles(seed);
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x0005AD38 File Offset: 0x00058F38
	public void SpawnTiles(int seed)
	{
		System.Random random = new System.Random(seed);
		Vector3 vector = new Vector3(-(this.gridSize / 2f) * this.gridDimension + this.gridDimension / 2f, 0f, -(this.gridSize / 2f) * this.gridDimension + this.gridDimension / 2f);
		Vector3 vector2 = vector;
		Transform parent = base.Root.transform.Find("Tiles");
		int num = 0;
		int num2 = 0;
		while ((float)num2 < this.gridSize)
		{
			int num3 = 0;
			while ((float)num3 < this.gridSize)
			{
				GameObject gameObject = base.Spawn(this.floorPrefab, vector2, Quaternion.identity);
				gameObject.name = "Tile_" + num.ToString();
				gameObject.transform.localScale = new Vector3(this.gridDimension, 1f, this.gridDimension);
				gameObject.transform.parent = parent;
				BoundingBlocksTile component = gameObject.GetComponent<BoundingBlocksTile>();
				component.id = num;
				this.floorTiles.Add(component);
				num++;
				if (Vector3.Distance(Vector3.zero, vector2) >= this.gridSize / 2f * this.gridDimension)
				{
					component.blocker.enabled = true;
					component.trigger.enabled = false;
					component.visual.enabled = false;
					component.disabledVisual.SetActive(true);
					component.glowVisual.enabled = false;
				}
				vector2 += new Vector3(this.gridDimension, 0f, 0f);
				num3++;
			}
			vector2.x = vector.x;
			vector2 += new Vector3(0f, 0f, this.gridDimension);
			num2++;
		}
		int num4 = random.Next(8, 11);
		int num5 = 1000;
		int num6 = 0;
		float num7 = 0f;
		float num8 = 16f;
		int i = 0;
		while (i < num4)
		{
			int num9 = random.Next(0, (int)this.gridSize);
			int num10 = random.Next(0, (int)this.gridSize);
			float num11 = Vector3.Distance(new Vector3(this.gridSize / 2f, 0f, this.gridSize / 2f) * this.gridDimension, new Vector3((float)num9, 0f, (float)num10) * this.gridDimension);
			BoundingBlocksTile boundingBlocksTile = this.floorTiles[(int)((float)num10 * this.gridSize) + num9];
			if (num11 > num7 && num11 < num8 && !boundingBlocksTile.blocker.enabled)
			{
				int num12 = num9;
				int num13 = num10;
				int num14 = random.Next(6, 11);
				int num15 = 1000;
				int num16 = 0;
				int j = 0;
				while (j < num14)
				{
					switch (random.Next(0, 4))
					{
					case 0:
						num12 = num9 + 1;
						break;
					case 1:
						num12 = num9 - 1;
						break;
					case 2:
						num13 = num10 + 1;
						break;
					case 3:
						num13 = num10 - 1;
						break;
					}
					float num17 = Vector3.Distance(new Vector3(this.gridSize / 2f, 0f, this.gridSize / 2f) * this.gridDimension, new Vector3((float)num12, 0f, (float)num13) * this.gridDimension);
					if (num12 >= 0 && (float)num12 < this.gridSize && num13 >= 0 && (float)num13 < this.gridSize && num17 > num7 && num17 < num8)
					{
						BoundingBlocksTile boundingBlocksTile2 = this.floorTiles[(int)((float)num13 * this.gridSize) + num12];
						if (!boundingBlocksTile2.blocker.enabled)
						{
							num9 = num12;
							num10 = num13;
							boundingBlocksTile2.blocker.enabled = true;
							boundingBlocksTile2.trigger.enabled = false;
							boundingBlocksTile2.visual.enabled = false;
							boundingBlocksTile2.disabledVisual.SetActive(true);
							boundingBlocksTile2.glowVisual.enabled = false;
							j++;
							num16 = 0;
						}
					}
					num16++;
					if (num16 > num15)
					{
						break;
					}
				}
				i++;
				num6 = 0;
			}
			num6++;
			if (num6 > num5)
			{
				break;
			}
		}
	}

	// Token: 0x06000A45 RID: 2629 RVA: 0x0005B184 File Offset: 0x00059384
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		for (int i = 0; i < this.ui_score.Length; i++)
		{
			if (!(this.ui_score[i] == null))
			{
				this.ui_score[i].scoreUpdateSpeed = 25;
			}
		}
		base.StartMinigame();
	}

	// Token: 0x06000A46 RID: 2630 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000A48 RID: 2632 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x0000AB1C File Offset: 0x00008D1C
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer && this.ui_timer.time_test <= 0f)
		{
			base.EndRound(1f, 3f, false);
		}
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000A4C RID: 2636 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x0005B204 File Offset: 0x00059404
	public void ClaimTile(int tileID, short playerSlot)
	{
		this.floorTiles[tileID].glowVisual.material = this.floorMaterials[(int)playerSlot];
		this.floorTiles[tileID].trigger.enabled = false;
		this.floorTiles[tileID].blocker.enabled = true;
		BoundingBlocksPlayer boundingBlocksPlayer = (BoundingBlocksPlayer)base.GetPlayerInSlot(playerSlot);
		Physics.IgnoreCollision(this.floorTiles[tileID].blocker, boundingBlocksPlayer.characterController);
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCClaim", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				tileID,
				playerSlot
			});
			BoundingBlocksPlayer boundingBlocksPlayer2 = boundingBlocksPlayer;
			short score = boundingBlocksPlayer2.Score;
			boundingBlocksPlayer2.Score = score + 1;
		}
	}

	// Token: 0x06000A4E RID: 2638 RVA: 0x0000AB51 File Offset: 0x00008D51
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCClaim(NetPlayer sender, int tileID, short playerSlot)
	{
		this.ClaimTile(tileID, playerSlot);
	}

	// Token: 0x04000922 RID: 2338
	[Header("Minigame specific attributes")]
	public GameObject floorPrefab;

	// Token: 0x04000923 RID: 2339
	public float gridSize = 21f;

	// Token: 0x04000924 RID: 2340
	public float gridDimension = 1f;

	// Token: 0x04000925 RID: 2341
	public Material floorColourMat;

	// Token: 0x04000926 RID: 2342
	private CameraShake cameraShake;

	// Token: 0x04000927 RID: 2343
	private List<BoundingBlocksTile> floorTiles = new List<BoundingBlocksTile>();

	// Token: 0x04000928 RID: 2344
	private Material[] floorMaterials;
}
