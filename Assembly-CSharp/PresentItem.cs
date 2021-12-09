using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x02000300 RID: 768
public class PresentItem : Item
{
	// Token: 0x06001552 RID: 5458 RVA: 0x0009AD70 File Offset: 0x00098F70
	public override void Setup()
	{
		base.Setup();
		base.GetAITarget();
		this.player.BoardObject.PlayerAnimation.Carrying = true;
		this.presentAnim = this.heldObject.GetComponentInChildren<PresentAnim>();
		this.presentAnimController = this.heldObject.GetComponentInChildren<PresentAnimController>();
		this.presentAnimController.item = this;
		this.possibleMinigames = new List<MinigameDefinition>();
		base.SetNetworkState(Item.ItemState.Setup);
	}

	// Token: 0x06001553 RID: 5459 RVA: 0x0009ADE0 File Offset: 0x00098FE0
	protected override void Use(int seed)
	{
		base.Use(seed);
		byte b = (byte)this.rand.Next(0, 3);
		base.SendRPC("RPCOpenPresent", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			b,
			seed
		});
		base.StartCoroutine(this.OpenPresent(b));
	}

	// Token: 0x06001554 RID: 5460 RVA: 0x000103C4 File Offset: 0x0000E5C4
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOpenPresent(NetPlayer sender, byte actionID, int seed)
	{
		base.Use(seed);
		base.StartCoroutine(this.OpenPresent(actionID));
	}

	// Token: 0x06001555 RID: 5461 RVA: 0x000103DB File Offset: 0x0000E5DB
	private IEnumerator OpenPresent(byte actionID)
	{
		if (!base.IsOwner)
		{
			this.rand.Next(0, 3);
		}
		if (actionID > 1)
		{
			this.presentAnimController.quadRenderer.sharedMaterial.SetColor("_EmissionColor", this.badColor);
		}
		else
		{
			this.presentAnimController.quadRenderer.sharedMaterial.SetColor("_EmissionColor", this.goodColor);
		}
		this.heldObject.transform.parent = null;
		this.heldObject.transform.rotation = Quaternion.identity;
		this.player.BoardObject.PlayerAnimation.Carrying = false;
		yield return base.StartCoroutine(this.presentAnim.DoOpenAnimation(this.presentAnim.transform.position));
		this.presentAnim.SetGoodPresent(actionID <= 1);
		yield return base.StartCoroutine(this.presentAnimController.Open());
		yield return new WaitForSeconds(0.4f);
		switch (actionID)
		{
		case 0:
			yield return base.StartCoroutine(this.ChooseMinigame());
			break;
		case 1:
			yield return base.StartCoroutine(this.TeleportToGoal());
			break;
		case 2:
			yield return base.StartCoroutine(this.Swallow());
			break;
		}
		base.Finish(false);
		yield break;
	}

	// Token: 0x06001556 RID: 5462 RVA: 0x000103F1 File Offset: 0x0000E5F1
	public void OnSwallowMaximum()
	{
		this.player.BoardObject.Visible = false;
	}

	// Token: 0x06001557 RID: 5463 RVA: 0x00010404 File Offset: 0x0000E604
	private IEnumerator Swallow()
	{
		Debug.Log("Present: Swallow");
		this.skipTurnAfterUse = true;
		this.heldObject.transform.SetParent(null);
		this.presentAnim.SetTarget(this.player.BoardObject.gameObject);
		yield return null;
		this.presentAnimController.anim.Play("PresentSwallow2", PlayMode.StopAll);
		yield return new WaitForSeconds(1f);
		this.heldObject.transform.parent = this.player.BoardObject.transform;
		this.heldObject = null;
		this.presentAnimController.anim.CrossFade("PresentIdle2", 0.2f);
		this.player.BoardObject.PresentScript = this.presentAnimController;
		yield break;
	}

	// Token: 0x06001558 RID: 5464 RVA: 0x0009AE38 File Offset: 0x00099038
	private List<MinigameDefinition> GetMinigames()
	{
		List<MinigameDefinition> activeMinigameList = GameManager.GetActiveMinigameList();
		for (int i = activeMinigameList.Count - 1; i >= 0; i--)
		{
			if (GameManager.localPlayedMinigameList.Contains(activeMinigameList[i]))
			{
				activeMinigameList.RemoveAt(i);
			}
		}
		for (int j = activeMinigameList.Count - 1; j >= 0; j--)
		{
			if (GameManager.GetPlayerCount() < activeMinigameList[j].minPlayers || GameManager.GetPlayerCount() > activeMinigameList[j].maxPlayers)
			{
				activeMinigameList.RemoveAt(j);
			}
		}
		return activeMinigameList;
	}

	// Token: 0x06001559 RID: 5465 RVA: 0x00010413 File Offset: 0x0000E613
	private IEnumerator ChooseMinigame()
	{
		Debug.Log("Present: Choose Minigame");
		yield return new WaitForSeconds(1f);
		List<MinigameDefinition> minigames = this.GetMinigames();
		if (minigames.Count < 3)
		{
			GameManager.localPlayedMinigameList.Clear();
			minigames = this.GetMinigames();
		}
		int num = 0;
		int num2 = 100;
		while (this.possibleMinigames.Count < 3)
		{
			MinigameDefinition minigameDefinition = minigames[this.rand.Next(0, minigames.Count)];
			bool flag = false;
			for (int i = 0; i < this.possibleMinigames.Count; i++)
			{
				if (this.possibleMinigames[i] == minigameDefinition)
				{
					flag = true;
					break;
				}
			}
			if (!flag || num > num2)
			{
				this.possibleMinigames.Add(minigameDefinition);
				num = 0;
			}
			else
			{
				num++;
			}
		}
		if (this.possibleMinigames.Count < 3)
		{
			num = 0;
			while (this.possibleMinigames.Count < 3 && num < num2)
			{
				for (int j = 0; j < minigames.Count; j++)
				{
					this.possibleMinigames.Add(minigames[j]);
					if (this.possibleMinigames.Count >= 3)
					{
						break;
					}
				}
			}
		}
		GameManager.UIController.minigameSelection.Show(this, this.possibleMinigames, this.player);
		float startTime = Time.time;
		yield return new WaitUntil(delegate()
		{
			if (this.IsOwner && this.player.IsAI && Time.time - startTime > 1f)
			{
				GameManager.UIController.minigameSelection.OnToggle(this.rand.Next(0, this.possibleMinigames.Count));
			}
			return this.selectedMinigame != -1;
		});
		GameManager.UIController.minigameSelection.Hide();
		if (NetSystem.IsServer)
		{
			GameManager.AddMinigame(this.possibleMinigames[this.selectedMinigame]);
		}
		yield return new WaitForSeconds(1f);
		yield break;
	}

	// Token: 0x0600155A RID: 5466 RVA: 0x00010422 File Offset: 0x0000E622
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCSelectMinigame(NetPlayer sender, byte minigameID)
	{
		this.SelectMinigame(minigameID);
	}

	// Token: 0x0600155B RID: 5467 RVA: 0x0001042B File Offset: 0x0000E62B
	public void SelectMinigame(byte minigameID)
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCSelectMinigame", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				minigameID
			});
		}
		this.selectedMinigame = (int)minigameID;
	}

	// Token: 0x0600155C RID: 5468 RVA: 0x00010457 File Offset: 0x0000E657
	private IEnumerator TeleportToGoal()
	{
		float portalSpawnWaitTime = 0.25f;
		GameManager.GetCamera().SetTrackedObject(null, Vector3.zero);
		BoardNode tarNode = GameManager.Board.GoalNode[GameManager.Board.ClosestGoalIndex(this.player.BoardObject)];
		for (int i = 0; i < tarNode.nodeConnections.Length; i++)
		{
			if (tarNode.nodeConnections[i].connection_type == BoardNodeConnectionDirection.Back)
			{
				tarNode = tarNode.nodeConnections[i].node;
				break;
			}
		}
		RaycastHit raycastHit;
		Physics.Raycast(this.player.BoardObject.MidPoint, Vector3.down, out raycastHit, 5f, 3072);
		PortalEffect portal = PortalEffect.Spawn(raycastHit.point, raycastHit.normal, PortalOrientation.Horizontal, this.player.BoardObject.gameObject, true);
		yield return new WaitForSeconds(portalSpawnWaitTime);
		BoardPlayerState preState = this.player.BoardObject.PlayerState;
		yield return base.StartCoroutine(this.SinkPlayers(new List<BoardPlayer>
		{
			this.player.BoardObject
		}));
		yield return new WaitForSeconds(0.75f);
		if (portal != null)
		{
			portal.Release(true);
		}
		yield return new WaitForSeconds(portalSpawnWaitTime + 0.75f);
		GameManager.GetCamera().MoveTo(tarNode.transform.position + GameManager.Board.PlayerCamOffset);
		yield return new WaitUntil(() => GameManager.GetCamera().WithinDistance(0.1f));
		this.player.BoardObject.CurrentNode.LeaveNode(this.player.BoardObject);
		this.player.BoardObject.CurrentNode = tarNode;
		float d = 5f;
		Vector3 position = tarNode.GetPlayersSlotPosition(this.player.BoardObject) + Vector3.up * d;
		portal = PortalEffect.Spawn(position, Vector3.down, PortalOrientation.Horizontal, this.player.BoardObject.gameObject, true);
		yield return new WaitForSeconds(portalSpawnWaitTime);
		yield return base.StartCoroutine(this.player.BoardObject.StartRagdoll(0f, 16f, false, 0.5f));
		this.player.BoardObject.SetState(preState);
		this.player.BoardObject.diceEffect.startPos = this.player.BoardObject.DicePosition();
		if (portal != null)
		{
			portal.Release(true);
		}
		yield return new WaitForSeconds(portalSpawnWaitTime);
		base.Finish(false);
		yield break;
	}

	// Token: 0x0600155D RID: 5469 RVA: 0x00010466 File Offset: 0x0000E666
	private IEnumerator SinkPlayers(List<BoardPlayer> players)
	{
		for (int i = 0; i < players.Count; i++)
		{
			players[i].SetState(BoardPlayerState.Ragdolling);
			players[i].PlayerAnimation.Grounded = false;
			players[i].PlayerAnimation.FireFallingTrigger();
		}
		float fallSpeed = 7f;
		float dropDistance = 3f;
		float droppedDistance = 0f;
		for (;;)
		{
			float num = fallSpeed * Time.deltaTime;
			droppedDistance += num;
			for (int j = 0; j < players.Count; j++)
			{
				players[j].transform.position -= new Vector3(0f, num, 0f);
			}
			if (droppedDistance >= dropDistance)
			{
				break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600155E RID: 5470 RVA: 0x0009AEBC File Offset: 0x000990BC
	public override void Unequip(bool endingTurn)
	{
		this.player.BoardObject.PlayerAnimation.Carrying = false;
		GameManager.Board.CameraTrackCurrentPlayer();
		if (this.heldObject != null)
		{
			this.heldObject = null;
			base.StartCoroutine(this.WaitAndDestroy());
		}
		base.Unequip(endingTurn);
	}

	// Token: 0x0600155F RID: 5471 RVA: 0x00010475 File Offset: 0x0000E675
	public override ItemAIUse GetTarget(BoardPlayer user)
	{
		return new ItemAIUse(user, 1f);
	}

	// Token: 0x06001560 RID: 5472 RVA: 0x00010482 File Offset: 0x0000E682
	private IEnumerator WaitAndDestroy()
	{
		yield return base.StartCoroutine(this.presentAnimController.DoDestroyPresentAnimation());
		yield break;
	}

	// Token: 0x04001660 RID: 5728
	[ColorUsage(true, true, 0f, 20f, 0.125f, 3f)]
	public Color goodColor;

	// Token: 0x04001661 RID: 5729
	[ColorUsage(true, true, 0f, 20f, 0.125f, 3f)]
	public Color badColor;

	// Token: 0x04001662 RID: 5730
	private short[] difficultyDistanceChange = new short[]
	{
		12,
		6,
		3
	};

	// Token: 0x04001663 RID: 5731
	private PresentAnimController presentAnimController;

	// Token: 0x04001664 RID: 5732
	private PresentAnim presentAnim;

	// Token: 0x04001665 RID: 5733
	private List<MinigameDefinition> possibleMinigames;

	// Token: 0x04001666 RID: 5734
	private int selectedMinigame = -1;
}
