using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using ZP.Net;

// Token: 0x0200027C RID: 636
public class TreasureHuntController : MinigameController
{
	// Token: 0x06001295 RID: 4757 RVA: 0x0008F8A8 File Offset: 0x0008DAA8
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("TreasureHuntPlayer", null);
			TreasureHuntVoxelGrid component = base.NetSpawn("TreasureHuntVoxel", new Vector3(0f, 2.5f, 0f), Quaternion.identity, 0, null).GetComponent<TreasureHuntVoxelGrid>();
			component.seed.Value = new System.Random().Next();
			component.Setup();
		}
		this.intro = base.Root.GetComponentInChildren<TreasureHuntIntro>();
	}

	// Token: 0x06001296 RID: 4758 RVA: 0x00009BEC File Offset: 0x00007DEC
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.StartMinigame();
	}

	// Token: 0x06001297 RID: 4759 RVA: 0x0000A3A2 File Offset: 0x000085A2
	public override void OnPlayersReady()
	{
		base.OnPlayersReady();
		GameManager.CaptureInput = true;
	}

	// Token: 0x06001298 RID: 4760 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06001299 RID: 4761 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x0600129A RID: 4762 RVA: 0x0000EF12 File Offset: 0x0000D112
	public override void UpdateBase()
	{
		if (this.controllerState == MinigameControllerState.EnablePlayers && !this.introFinished)
		{
			if (!this.introStarted)
			{
				this.introStarted = true;
				this.intro.StartIntro(this);
				return;
			}
		}
		else
		{
			base.UpdateBase();
		}
	}

	// Token: 0x0600129B RID: 4763 RVA: 0x0008F924 File Offset: 0x0008DB24
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing)
		{
			switch (this.curTreasureHuntState)
			{
			case TreasureHuntController.TreasureHuntState.Loading:
				this.stateTimer.SetInterval(35f, true);
				this.curTreasureHuntState = TreasureHuntController.TreasureHuntState.Waiting;
				return;
			case TreasureHuntController.TreasureHuntState.Waiting:
				if (this.stateTimer.Elapsed(false))
				{
					this.stateTimer.SetInterval(10f, true);
					if (this.treasure != null && this.treasure.interactable)
					{
						this.treasure.Outline = true;
						this.treasure.transform.Find("Root").gameObject.SetActive(true);
					}
					this.curTreasureHuntState = TreasureHuntController.TreasureHuntState.ShowingOutline;
					return;
				}
				break;
			case TreasureHuntController.TreasureHuntState.ShowingOutline:
				if (this.stateTimer.Elapsed(false))
				{
					this.stateTimer.SetInterval(15f, true);
					this.digTimer.SetInterval(0.2f, true);
					this.curTreasureHuntState = TreasureHuntController.TreasureHuntState.Digging;
					return;
				}
				break;
			case TreasureHuntController.TreasureHuntState.Digging:
				if (NetSystem.IsServer)
				{
					if (this.digTimer.Elapsed(true) && this.treasure != null)
					{
						this.grid.Vacuum(this.treasure.transform.position, false);
					}
					if (this.stateTimer.Elapsed(true))
					{
						base.EndRound(1f, 1f, false);
						this.curTreasureHuntState = TreasureHuntController.TreasureHuntState.Ending;
					}
				}
				break;
			case TreasureHuntController.TreasureHuntState.Ending:
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x0600129C RID: 4764 RVA: 0x0000A3B0 File Offset: 0x000085B0
	public override void ReleaseMinigame()
	{
		GameManager.CaptureInput = false;
		base.ReleaseMinigame();
	}

	// Token: 0x0600129D RID: 4765 RVA: 0x0000EF47 File Offset: 0x0000D147
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void SetWinnerRPC(NetPlayer sender, short playerSlot)
	{
		this.SetWinner(playerSlot);
	}

	// Token: 0x0600129E RID: 4766 RVA: 0x0008FA8C File Offset: 0x0008DC8C
	public void SetWinner(short playerSlot)
	{
		TreasureHuntPlayer treasureHuntPlayer = (TreasureHuntPlayer)base.GetPlayerInSlot(playerSlot);
		if (treasureHuntPlayer.IsOwner && !treasureHuntPlayer.GamePlayer.IsAI)
		{
			PlatformAchievementManager.Instance.TriggerAchievement("ACH_SANDY_SEARCH");
		}
		string newValue = string.Concat(new string[]
		{
			"<color=#",
			ColorUtility.ToHtmlStringRGBA(treasureHuntPlayer.GamePlayer.Color.uiColor),
			"> ",
			treasureHuntPlayer.GamePlayer.Name,
			"</color>"
		});
		string text = "<color=#4FF051FF>" + LocalizationManager.GetTranslation("FoundTreasure", true, 0, true, false, null, null, true).Replace("%Player%", newValue) + "</color>";
		GameManager.UIController.ShowLargeText(text, LargeTextType.PlayerWins, 10f, true, false);
		AudioSystem.PlayOneShot(this.winClip, 0.25f, 0f, 1f);
		if (NetSystem.IsServer)
		{
			TreasureHuntPlayer treasureHuntPlayer2 = treasureHuntPlayer;
			treasureHuntPlayer2.Score += 5;
			base.SendRPC("SetWinnerRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				playerSlot
			});
		}
	}

	// Token: 0x0600129F RID: 4767 RVA: 0x0008FBA0 File Offset: 0x0008DDA0
	[NetRPC(true, NetRPCSecurity.ALL, NetRPCSecurity.ALL)]
	public void InteractRPC(NetPlayer sender, short id, short player_slot)
	{
		for (int i = 0; i < this.objects.Count; i++)
		{
			if (this.objects[i].ObjectID == id)
			{
				this.objects[i].Interact(player_slot, true);
				return;
			}
		}
	}

	// Token: 0x060012A0 RID: 4768 RVA: 0x0000EF50 File Offset: 0x0000D150
	public void Interact(short id, short player_slot)
	{
		base.SendRPC("InteractRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			id,
			player_slot
		});
	}

	// Token: 0x060012A1 RID: 4769 RVA: 0x0008FBEC File Offset: 0x0008DDEC
	public void ChunkUpdated(Chunk c)
	{
		if (base.State != MinigameControllerState.Playing)
		{
			return;
		}
		for (int i = 0; i < this.objects.Count; i++)
		{
			if (this.objects[i].Buried)
			{
				if (this.CheckPosition(c.meshCollider.bounds, this.objects[i].transform.position, this.objects[i].uncover_radius))
				{
					this.objects[i].Buried = false;
				}
			}
			else if (this.objects[i].unstuckable && this.objects[i].Stuck && !Physics.CheckSphere(this.objects[i].transform.position, this.objects[i].unstuck_radius, 1024))
			{
				this.objects[i].Stuck = false;
			}
		}
	}

	// Token: 0x060012A2 RID: 4770 RVA: 0x0000EF76 File Offset: 0x0000D176
	private bool CheckPosition(Bounds b, Vector3 pos, float r)
	{
		return b.SqrDistance(pos) <= r * r && Physics.CheckSphere(pos, r, 1024);
	}

	// Token: 0x060012A3 RID: 4771 RVA: 0x0008FCEC File Offset: 0x0008DEEC
	public override bool HasLoadedLocally()
	{
		if (this.players.Count >= GameManager.GetPlayerCount())
		{
			return this.grid != null && this.grid.voxelGrid != null && this.grid.voxelGrid.data != null && this.grid.voxelGrid.buildCompleted;
		}
		Debug.Log("Not Loaded : " + this.players.Count.ToString() + " < " + GameManager.GetPlayerCount().ToString());
		return false;
	}

	// Token: 0x040013A6 RID: 5030
	[HideInInspector]
	public TreasureHuntVoxelGrid grid;

	// Token: 0x040013A7 RID: 5031
	[HideInInspector]
	public TreasureHuntTreasure treasure;

	// Token: 0x040013A8 RID: 5032
	[HideInInspector]
	public List<TreasureHuntObject> objects = new List<TreasureHuntObject>();

	// Token: 0x040013A9 RID: 5033
	public bool introStarted;

	// Token: 0x040013AA RID: 5034
	public bool introFinished;

	// Token: 0x040013AB RID: 5035
	public GameObject camPrefab;

	// Token: 0x040013AC RID: 5036
	public AudioClip winClip;

	// Token: 0x040013AD RID: 5037
	private TreasureHuntController.TreasureHuntState curTreasureHuntState;

	// Token: 0x040013AE RID: 5038
	private ActionTimer stateTimer = new ActionTimer(60f);

	// Token: 0x040013AF RID: 5039
	private ActionTimer digTimer = new ActionTimer(60f);

	// Token: 0x040013B0 RID: 5040
	private bool cur_setting;

	// Token: 0x040013B1 RID: 5041
	private TreasureHuntIntro intro;

	// Token: 0x0200027D RID: 637
	private enum TreasureHuntState
	{
		// Token: 0x040013B3 RID: 5043
		Loading,
		// Token: 0x040013B4 RID: 5044
		Waiting,
		// Token: 0x040013B5 RID: 5045
		ShowingOutline,
		// Token: 0x040013B6 RID: 5046
		Digging,
		// Token: 0x040013B7 RID: 5047
		Ending
	}
}
