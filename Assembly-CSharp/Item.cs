using System;
using System.Collections;
using I2.Loc;
using UnityEngine;
using ZP.Net;

// Token: 0x0200037B RID: 891
public class Item : NetBehaviour
{
	// Token: 0x1700024B RID: 587
	// (get) Token: 0x06001801 RID: 6145 RVA: 0x00011D64 File Offset: 0x0000FF64
	public System.Random Rand
	{
		get
		{
			return this.rand;
		}
	}

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x06001802 RID: 6146 RVA: 0x00011D6C File Offset: 0x0000FF6C
	public Item.ItemState CurState
	{
		get
		{
			return this.curState;
		}
	}

	// Token: 0x06001803 RID: 6147 RVA: 0x000A5ADC File Offset: 0x000A3CDC
	public override void OnNetInitialize()
	{
		this.curState = Item.ItemState.Setup;
		this.player = GameManager.GetPlayerAt((int)base.OwnerSlot);
		this.player.BoardObject.SetEquippedItem(this);
		if (NetSystem.IsServer)
		{
			GameManager.Board.QueueAction(new ActionSimple(SimpleBoardAction.SetupItem), true, true);
		}
		base.OnNetInitialize();
	}

	// Token: 0x06001804 RID: 6148 RVA: 0x000A5B34 File Offset: 0x000A3D34
	public virtual void Setup()
	{
		this.skipTurnAfterUse = this.details.skipTurnAfterUse;
		this.player.BoardObject.EquipItem(this);
		GameManager.UIController.InventoryUI.Hide();
		string str = " " + LocalizationManager.GetTranslation(this.details.itemNameToken, true, 0, true, false, null, null, true) + " - ";
		GameManager.UIController.SetMapViewUITitle(this.player.Name + str + LocalizationManager.GetTranslation(this.details.itemNameToken, true, 0, true, false, null, null, true));
		if (this.details.heldPrefab != null)
		{
			this.heldObject = UnityEngine.Object.Instantiate<GameObject>(this.details.heldPrefab);
			this.heldObject.transform.parent = this.player.BoardObject.PlayerAnimation.GetBone(this.details.heldBone);
			this.heldObject.transform.localScale = this.details.heldScale;
			this.heldObject.transform.localPosition = this.details.heldPosition;
			this.heldObject.transform.localRotation = Quaternion.Euler(this.details.heldRotation);
		}
		if (this.player.IsLocalPlayer && this.details.inputHelp != null)
		{
			GameManager.UIController.SetBoardInputHelp(this.details.inputHelp);
		}
	}

	// Token: 0x06001805 RID: 6149 RVA: 0x000A5CAC File Offset: 0x000A3EAC
	public virtual void Update()
	{
		if (this.player.IsLocalPlayer && !this.player.IsAI && this.curState == Item.ItemState.Aiming && !GameManager.IsGamePaused)
		{
			if (this.player.RewiredPlayer.GetButtonDown(this.details.ActionID))
			{
				this.Use(GameManager.rand.Next(int.MinValue, int.MaxValue));
				return;
			}
			if (this.player.RewiredPlayer.GetButtonDown(InputActions.Cancel))
			{
				this.CancelItem();
				return;
			}
		}
		else if (NetSystem.IsServer && this.curState == Item.ItemState.Finished && !this.didFinish)
		{
			if (this.CompareNetworkState(Item.ItemState.Finished))
			{
				this.didFinish = true;
				GameManager.Board.QueueAction(new ActionSimple(SimpleBoardAction.FinishUsingItem), true, true);
				return;
			}
		}
		else if (NetSystem.IsServer && this.CurState == Item.ItemState.Setup)
		{
			if (this.CompareNetworkState(Item.ItemState.Setup))
			{
				this.FinishedSetup();
				return;
			}
		}
		else if (NetSystem.IsServer && this.CurState == Item.ItemState.Cancelled && this.CompareNetworkState(Item.ItemState.Cancelled))
		{
			this.FinishedCancelling();
		}
	}

	// Token: 0x06001806 RID: 6150 RVA: 0x000A5DB8 File Offset: 0x000A3FB8
	private bool CompareNetworkState(Item.ItemState state)
	{
		for (int i = 0; i < this.networkStates.Length; i++)
		{
			if (GameManager.IsPlayerInSlot(i) && this.networkStates[i] != state)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001807 RID: 6151 RVA: 0x00011D74 File Offset: 0x0000FF74
	public void CancelItem()
	{
		Debug.Log("Cancelling Item Use");
		this.SetNetworkState(Item.ItemState.Cancelled);
		this.curState = Item.ItemState.Cancelled;
		this.Unequip(false);
		if (base.IsOwner)
		{
			base.SendRPC("RPCCancel", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x06001808 RID: 6152 RVA: 0x00011DAE File Offset: 0x0000FFAE
	public void Finish(bool relay = true)
	{
		this.curState = Item.ItemState.Finished;
		this.SetNetworkState(Item.ItemState.Finished);
	}

	// Token: 0x06001809 RID: 6153 RVA: 0x000A5DF0 File Offset: 0x000A3FF0
	public bool AIUseItem()
	{
		if (!GameManager.DEBUGGING || GameManager.rand.NextDouble() > 0.20000000298023224)
		{
			this.Use(GameManager.rand.Next(int.MinValue, int.MaxValue));
			return true;
		}
		this.CancelItem();
		return false;
	}

	// Token: 0x0600180A RID: 6154 RVA: 0x000A5E3C File Offset: 0x000A403C
	protected virtual void Use(int seed)
	{
		Debug.Log("Player " + this.player.GlobalID.ToString() + " Using Item: " + this.details.itemName);
		this.curState = Item.ItemState.Using;
		this.rand = new System.Random(seed);
		this.player.BoardObject.UseItem();
		if (this.player.IsLocalPlayer && this.details.usingInputHelp != null)
		{
			GameManager.UIController.SetBoardInputHelp(this.details.usingInputHelp);
		}
		if (base.IsOwner && this.relayUse)
		{
			base.SendRPC("RPCUse", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				seed
			});
		}
	}

	// Token: 0x0600180B RID: 6155 RVA: 0x00011DBE File Offset: 0x0000FFBE
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCUse(NetPlayer sender, int seed)
	{
		this.Use(seed);
	}

	// Token: 0x0600180C RID: 6156 RVA: 0x000A5EF8 File Offset: 0x000A40F8
	public virtual void Unequip(bool endingTurn)
	{
		if (this.curState != Item.ItemState.Cancelled)
		{
			this.curState = Item.ItemState.Finished;
			this.player.BoardObject.UnEquipItem(endingTurn);
			if (NetSystem.IsServer)
			{
				base.StartCoroutine(this.DestroyThis());
			}
		}
		this.player.BoardObject.SetEquippedItem(null);
		if (this.heldObject != null)
		{
			UnityEngine.Object.Destroy(this.heldObject);
		}
		if (this.player.IsLocalPlayer && !endingTurn)
		{
			GameManager.UIController.SetBoardInputHelpType(this.player.BoardObject.HasUsedItem ? BoardInputType.PlayerTurnUsedItem : BoardInputType.PlayerTurn);
		}
	}

	// Token: 0x0600180D RID: 6157 RVA: 0x00011DC7 File Offset: 0x0000FFC7
	private IEnumerator DestroyThis()
	{
		yield return new WaitForSeconds(60f);
		if (NetSystem.IsServer)
		{
			NetSystem.Kill(this);
		}
		yield break;
	}

	// Token: 0x0600180E RID: 6158 RVA: 0x00011DD6 File Offset: 0x0000FFD6
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCCancel(NetPlayer sender)
	{
		this.CancelItem();
	}

	// Token: 0x0600180F RID: 6159 RVA: 0x00011DDE File Offset: 0x0000FFDE
	public virtual ItemAIUse GetTarget(BoardPlayer user)
	{
		return new ItemAIUse();
	}

	// Token: 0x06001810 RID: 6160 RVA: 0x000A5F94 File Offset: 0x000A4194
	public void GetAITarget()
	{
		ItemAIUse target = this.GetTarget(this.player.BoardObject);
		if (target == null)
		{
			this.AITarget = ((this.player.BoardObject.ActorID == 0) ? GameManager.Board.GetActor(1) : GameManager.Board.GetActor(0));
			return;
		}
		this.AITarget = target.player;
	}

	// Token: 0x06001811 RID: 6161 RVA: 0x00011DE5 File Offset: 0x0000FFE5
	[NetRPC(true, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void SetNetworkState(NetPlayer sender, byte slot, byte state)
	{
		this.networkStates[(int)slot] = (Item.ItemState)state;
	}

	// Token: 0x06001812 RID: 6162 RVA: 0x000A5FF4 File Offset: 0x000A41F4
	protected void SetNetworkState(Item.ItemState state)
	{
		for (int i = 0; i < GameManager.GetPlayerCount(); i++)
		{
			if (GameManager.GetPlayerAt(i).IsLocalPlayer)
			{
				if (NetSystem.IsServer)
				{
					this.networkStates[i] = state;
				}
				else
				{
					base.SendRPC("SetNetworkState", NetRPCDelivery.RELIABLE_ORDERED, new object[]
					{
						(byte)i,
						(byte)state
					});
				}
			}
		}
	}

	// Token: 0x06001813 RID: 6163 RVA: 0x00011DF0 File Offset: 0x0000FFF0
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void FinishedSetupRPC(NetPlayer sender)
	{
		this.FinishedSetup();
	}

	// Token: 0x06001814 RID: 6164 RVA: 0x00011DF8 File Offset: 0x0000FFF8
	private void FinishedSetup()
	{
		if (this.curState == Item.ItemState.Setup)
		{
			this.curState = Item.ItemState.Aiming;
		}
		if (NetSystem.IsServer)
		{
			base.SendRPC("FinishedSetupRPC", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x06001815 RID: 6165 RVA: 0x00011E22 File Offset: 0x00010022
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void FinishedCancellingRPC(NetPlayer sender)
	{
		this.FinishedCancelling();
	}

	// Token: 0x06001816 RID: 6166 RVA: 0x00011E2A File Offset: 0x0001002A
	private void FinishedCancelling()
	{
		this.curState = Item.ItemState.Finished;
		this.player.BoardObject.UnEquipItem(false);
		if (NetSystem.IsServer)
		{
			base.SendRPC("FinishedCancellingRPC", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
			base.StartCoroutine(this.DestroyThis());
		}
	}

	// Token: 0x0400197B RID: 6523
	public ItemDetails details;

	// Token: 0x0400197C RID: 6524
	public bool relayUse = true;

	// Token: 0x0400197D RID: 6525
	public bool aiAutoUse;

	// Token: 0x0400197E RID: 6526
	protected GameObject heldObject;

	// Token: 0x0400197F RID: 6527
	protected GamePlayer player;

	// Token: 0x04001980 RID: 6528
	protected System.Random rand;

	// Token: 0x04001981 RID: 6529
	protected BoardActor AITarget;

	// Token: 0x04001982 RID: 6530
	protected ActionTimer actionTimer = new ActionTimer(0f);

	// Token: 0x04001983 RID: 6531
	private bool unequiping;

	// Token: 0x04001984 RID: 6532
	protected Item.ItemState curState = Item.ItemState.Setup;

	// Token: 0x04001985 RID: 6533
	private Item.ItemState[] networkStates = new Item.ItemState[8];

	// Token: 0x04001986 RID: 6534
	private bool didFinish;

	// Token: 0x04001987 RID: 6535
	public bool skipTurnAfterUse;

	// Token: 0x0200037C RID: 892
	public enum ItemState
	{
		// Token: 0x04001989 RID: 6537
		Initializing,
		// Token: 0x0400198A RID: 6538
		Setup,
		// Token: 0x0400198B RID: 6539
		Aiming,
		// Token: 0x0400198C RID: 6540
		Using,
		// Token: 0x0400198D RID: 6541
		Unequipped,
		// Token: 0x0400198E RID: 6542
		Finished,
		// Token: 0x0400198F RID: 6543
		Cancelled
	}
}
