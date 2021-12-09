using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200035A RID: 858
public class RecruitEventManager : BoardNodeEvent
{
	// Token: 0x0600171D RID: 5917 RVA: 0x00011415 File Offset: 0x0000F615
	public void Start()
	{
		RecruitEventManager.eventManager = this;
	}

	// Token: 0x0600171E RID: 5918 RVA: 0x0001141D File Offset: 0x0000F61D
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode node, int seed)
	{
		if (node.CurrentNodeType == BoardNodeType.Recruit)
		{
			this.rand = new System.Random(seed);
			if (this.owners.ContainsKey(node))
			{
				if (this.owners[node].owner != player)
				{
					RecruitEventManager.RecruitSpaceDetails entry = this.owners[node];
					yield return base.StartCoroutine(this.reaperScript.DoEvent(entry.owner, player, entry.hitType, this.rand));
					Debug.Log("Applying Reaper Heal");
					if (entry.hitType == ReaperHitType.Health)
					{
						entry.owner.ApplyHeal(this.reaperScript.CurHitDamage);
					}
					entry = default(RecruitEventManager.RecruitSpaceDetails);
				}
			}
			else
			{
				player.PlayerState = BoardPlayerState.MakingInteractionChoice;
				node.interactionScript = this.reaperInteraction;
				this.SetupNode(node);
				Coroutine coroutine = base.StartCoroutine(this.reaperScript.FadeReaper(true, player, this.rand));
				yield return coroutine;
				this.reaperInteraction.Setup(player);
			}
		}
		else
		{
			GameManager.Board.EndTurn();
		}
		yield break;
	}

	// Token: 0x0600171F RID: 5919 RVA: 0x00011441 File Offset: 0x0000F641
	public override bool EndTurnAfterEvent(BoardNode node)
	{
		return this.owners.ContainsKey(node);
	}

	// Token: 0x06001720 RID: 5920 RVA: 0x0001144F File Offset: 0x0000F64F
	public void ClaimNode(BoardNode node, BoardPlayer p, ReaperHitType type)
	{
		this.owners.Add(node, new RecruitEventManager.RecruitSpaceDetails(p, type));
		if (node.CurrentNodeType == BoardNodeType.Recruit)
		{
			node.SetRecruitColor();
		}
	}

	// Token: 0x06001721 RID: 5921 RVA: 0x000A1428 File Offset: 0x0009F628
	private void SetupNode(BoardNode node)
	{
		node.interactionScript = this.reaperInteraction;
		node.CurInteractionScript = this.reaperInteraction;
		BoardNode neighbour = this.GetNeighbour(node);
		if (neighbour == null)
		{
			Debug.Log("Neighbour Null: " + node.gameObject.name);
		}
		neighbour.interactionScript = this.reaperInteraction;
		if (neighbour.CurrentNodeType == BoardNodeType.Recruit)
		{
			neighbour.CurInteractionScript = this.reaperInteraction;
		}
	}

	// Token: 0x06001722 RID: 5922 RVA: 0x000A149C File Offset: 0x0009F69C
	public BoardNode GetNeighbour(BoardNode node)
	{
		foreach (BoardNodeConnection boardNodeConnection in node.nodeConnections)
		{
			if (boardNodeConnection.node.baseNodeType == BoardNodeType.Recruit)
			{
				return boardNodeConnection.node;
			}
		}
		return null;
	}

	// Token: 0x06001723 RID: 5923 RVA: 0x000A14DC File Offset: 0x0009F6DC
	public ReaperNode[] Save()
	{
		ReaperNode[] array = new ReaperNode[this.owners.Count];
		int num = 0;
		foreach (KeyValuePair<BoardNode, RecruitEventManager.RecruitSpaceDetails> keyValuePair in this.owners)
		{
			array[num] = new ReaperNode();
			array[num].nodeID = (short)keyValuePair.Key.NodeID;
			array[num].playerID = (byte)keyValuePair.Value.owner.OwnerSlot;
			array[num].choice = (byte)keyValuePair.Value.hitType;
			num++;
		}
		return array;
	}

	// Token: 0x06001724 RID: 5924 RVA: 0x000A158C File Offset: 0x0009F78C
	public void Load(ReaperNode[] nodes)
	{
		for (int i = 0; i < nodes.Length; i++)
		{
			BoardNode node = GameManager.Board.BoardNodes[(int)nodes[i].nodeID];
			BoardPlayer boardObject = GameManager.GetPlayerAt((int)nodes[i].playerID).BoardObject;
			ReaperHitType choice = (ReaperHitType)nodes[i].choice;
			this.SetupNode(node);
			this.ClaimNode(node, boardObject, choice);
		}
	}

	// Token: 0x0400186B RID: 6251
	public Dictionary<BoardNode, RecruitEventManager.RecruitSpaceDetails> owners = new Dictionary<BoardNode, RecruitEventManager.RecruitSpaceDetails>();

	// Token: 0x0400186C RID: 6252
	public Interaction reaperInteraction;

	// Token: 0x0400186D RID: 6253
	public Reaper reaperScript;

	// Token: 0x0400186E RID: 6254
	public static RecruitEventManager eventManager;

	// Token: 0x0200035B RID: 859
	public struct RecruitSpaceDetails
	{
		// Token: 0x06001726 RID: 5926 RVA: 0x00011487 File Offset: 0x0000F687
		public RecruitSpaceDetails(BoardPlayer owner, ReaperHitType hitType)
		{
			this.owner = owner;
			this.hitType = hitType;
		}

		// Token: 0x0400186F RID: 6255
		public BoardPlayer owner;

		// Token: 0x04001870 RID: 6256
		public ReaperHitType hitType;
	}
}
