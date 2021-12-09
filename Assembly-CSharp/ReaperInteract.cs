using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200035E RID: 862
public class ReaperInteract : Interaction
{
	// Token: 0x06001734 RID: 5940 RVA: 0x000A179C File Offset: 0x0009F99C
	public override void Setup(BoardPlayer player)
	{
		base.Setup(player);
		((RecruitInteractionDialog)this.dialog).Setup(this.title, this.description, this.icon, player.GamePlayer);
		Vector3 normalized = (player.transform.position - this.eventManager.reaperScript.transform.position).normalized;
		player.PlayerAnimation.SetPlayerRotation(Mathf.Atan2(normalized.x, normalized.z) * 57.29578f - 195f);
	}

	// Token: 0x06001735 RID: 5941 RVA: 0x00011527 File Offset: 0x0000F727
	public override IEnumerator DoInteraction(byte choice)
	{
		this.eventManager.ClaimNode(this.player.CurrentNode, this.player, (ReaperHitType)choice);
		this.eventManager.ClaimNode(this.eventManager.GetNeighbour(this.player.CurrentNode), this.player, (ReaperHitType)choice);
		yield return base.StartCoroutine(this.eventManager.reaperScript.FadeReaper(false, this.player, this.eventManager.rand));
		base.Finished = true;
		this.player.PlayerState = BoardPlayerState.Idle;
		GameManager.Board.EndTurn();
		yield break;
	}

	// Token: 0x06001736 RID: 5942 RVA: 0x0001153D File Offset: 0x0000F73D
	public override int GetAIChoice()
	{
		return GameManager.rand.Next(0, 2);
	}

	// Token: 0x0400187F RID: 6271
	public RecruitEventManager eventManager;
}
