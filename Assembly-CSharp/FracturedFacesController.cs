using System;
using System.Collections;
using UnityEngine;
using ZP.Net;

// Token: 0x020001AE RID: 430
public class FracturedFacesController : MinigameController
{
	// Token: 0x06000C50 RID: 3152 RVA: 0x00067610 File Offset: 0x00065810
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			Transform transform = base.Root.transform.Find("PlayAreas");
			Transform[] array = new Transform[GameManager.GetPlayerCount()];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ((array.Length > 4) ? transform.Find("PlayArea" + (i + 1).ToString() + "/SpawnPoint") : transform.Find("PlayArea" + this.playAreaMap[i].ToString() + "/SpawnPoint"));
			}
			base.SpawnPlayers("FracturedFacesPlayer", array);
			int seed = GameManager.rand.Next();
			base.StartCoroutine(this.SpawnPuzzlePieces(seed));
		}
	}

	// Token: 0x06000C51 RID: 3153 RVA: 0x0000BBAD File Offset: 0x00009DAD
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSpawnPuzzlePieces(NetPlayer sender, int seed)
	{
		base.StartCoroutine(this.SpawnPuzzlePieces(seed));
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x0000BBBD File Offset: 0x00009DBD
	private IEnumerator SpawnPuzzlePieces(int seed)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSpawnPuzzlePieces", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				seed
			});
		}
		System.Random random = new System.Random(seed);
		this.mr = base.Root.transform.Find("Example").gameObject.GetComponent<MeshRenderer>();
		this.mr.material = this.puzzleMaterials[random.Next(0, this.puzzleMaterials.Length)];
		int i = 0;
		while (i < GameManager.GetPlayerCount())
		{
			FracturedFacesPlayer fracturedFacesPlayer = (FracturedFacesPlayer)base.GetPlayerInSlot((short)i);
			if (fracturedFacesPlayer != null)
			{
				fracturedFacesPlayer.SetupPuzzlePieces(seed);
				int num = i + 1;
				i = num;
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x0000ACF2 File Offset: 0x00008EF2
	public override void StartMinigame()
	{
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.StartMinigame();
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x000676D4 File Offset: 0x000658D4
	public override void RoundEnded()
	{
		if (NetSystem.IsServer)
		{
			for (int i = 0; i < GameManager.GetPlayerCount(); i++)
			{
				FracturedFacesPlayer fracturedFacesPlayer = (FracturedFacesPlayer)base.GetPlayerInSlot((short)i);
				if (!fracturedFacesPlayer.Finished)
				{
					int num = 0;
					for (int j = 0; j < fracturedFacesPlayer.puzzlePieces.Count; j++)
					{
						if (fracturedFacesPlayer.puzzlePieces[j].id == fracturedFacesPlayer.puzzlePiecePositions[j] && fracturedFacesPlayer.puzzlePieceRotations[j] == 0)
						{
							num++;
						}
					}
					fracturedFacesPlayer.Score = (short)num;
				}
			}
		}
		base.RoundEnded();
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000C56 RID: 3158 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x00067768 File Offset: 0x00065968
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer)
		{
			bool flag = true;
			for (int i = 0; i < GameManager.GetPlayerCount(); i++)
			{
				FracturedFacesPlayer fracturedFacesPlayer = (FracturedFacesPlayer)base.GetPlayerInSlot((short)i);
				if (!fracturedFacesPlayer.Finished)
				{
					flag = false;
					bool flag2 = true;
					for (int j = 0; j < fracturedFacesPlayer.puzzlePieces.Count; j++)
					{
						if (fracturedFacesPlayer.puzzlePieces[j].id != fracturedFacesPlayer.puzzlePiecePositions[j] || fracturedFacesPlayer.puzzlePieceRotations[j] != 0)
						{
							flag2 = false;
							break;
						}
					}
					if (flag2)
					{
						fracturedFacesPlayer.Finished = true;
						FracturedFacesPlayer fracturedFacesPlayer2 = fracturedFacesPlayer;
						fracturedFacesPlayer2.Score += (short)((GameManager.GetPlayerCount() - this.finishedCount) * 100);
						this.finishedCount++;
					}
				}
			}
			if (this.ui_timer.time_test <= 0f || flag)
			{
				base.EndRound(2.5f, 2f, false);
			}
		}
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x04000BAB RID: 2987
	public float gridSize = 5f;

	// Token: 0x04000BAC RID: 2988
	public float playAreaSize = 5f;

	// Token: 0x04000BAD RID: 2989
	public float gap = 0.05f;

	// Token: 0x04000BAE RID: 2990
	public Material[] puzzleMaterials;

	// Token: 0x04000BAF RID: 2991
	private int[] playAreaMap = new int[]
	{
		2,
		3,
		6,
		7
	};

	// Token: 0x04000BB0 RID: 2992
	private MeshRenderer mr;

	// Token: 0x04000BB1 RID: 2993
	private int finishedCount;
}
