using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000343 RID: 835
public class CannonInteraction : Interaction
{
	// Token: 0x0600168E RID: 5774 RVA: 0x0009FA78 File Offset: 0x0009DC78
	public override void Setup(BoardPlayer player)
	{
		base.Setup(player);
		this.buttonSettings[0].interactable = (player.Gold >= this.cost);
		this.buttonSettings[0].cost = this.cost;
		((SimpleInteractionDialog)this.dialog).Activate(this.title, this.description, this.buttonSettings, player.GamePlayer, this.icon);
		Vector3 normalized = (player.transform.position - base.transform.position).normalized;
		player.PlayerAnimation.SetPlayerRotation(Mathf.Atan2(normalized.x, normalized.z) * 57.29578f - 180f);
	}

	// Token: 0x0600168F RID: 5775 RVA: 0x00010FE2 File Offset: 0x0000F1E2
	public override IEnumerator DoInteraction(byte choice)
	{
		if (choice == 0)
		{
			this.player.RemoveGold(this.cost, false, false);
			this.player.transform.position = this.cannonStart.transform.position + Vector3.up;
			yield return new WaitForSeconds(0.8f);
			UnityEngine.Object.Instantiate<GameObject>(this.canonExplodeParticle, this.particleSpawnTransform.position, this.particleSpawnTransform.rotation);
			AudioSystem.PlayOneShot(this.canonSound, 0.5f, 0f, 1f);
			this.player.MoveArc(this.target, 10f, 30f);
			GameManager.Board.boardCamera.SetTrackedObject(null, Vector3.zero);
			yield return new WaitForSeconds(0.5f);
			GameManager.Board.boardCamera.MoveTo(this.target.transform.position);
			yield return new WaitUntil(() => this.player.PlayerState == BoardPlayerState.Idle);
		}
		yield return new WaitForSeconds(0.25f);
		GameManager.Board.CameraTrackCurrentPlayer();
		yield return new WaitForSeconds(0.25f);
		GameManager.Board.EndInteraction();
		base.Finished = true;
		yield break;
	}

	// Token: 0x06001690 RID: 5776 RVA: 0x0009FB40 File Offset: 0x0009DD40
	public override int GetAIChoice()
	{
		if (this.player.Gold < this.cost)
		{
			return 1;
		}
		int num = GameManager.Board.ClosestGoalIndex(null);
		int num2 = GameManager.Board.CurrentMap.DistToNode(this.target, GameManager.Board.GoalNode[num], BoardNodeConnectionDirection.Forward);
		int num3 = GameManager.Board.CurrentMap.DistToNode(this.altNode, GameManager.Board.GoalNode[num], BoardNodeConnectionDirection.Forward);
		bool flag = num2 < num3;
		if (GameManager.rand.NextDouble() < (double)this.wrongChoiceChance[(int)this.player.GamePlayer.Difficulty])
		{
			flag = !flag;
		}
		if (num2 >= num3)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x040017C1 RID: 6081
	public Transform cannonStart;

	// Token: 0x040017C2 RID: 6082
	public Transform particleSpawnTransform;

	// Token: 0x040017C3 RID: 6083
	public GameObject canonExplodeParticle;

	// Token: 0x040017C4 RID: 6084
	public AudioClip canonSound;

	// Token: 0x040017C5 RID: 6085
	public BoardNode target;

	// Token: 0x040017C6 RID: 6086
	public BoardNode altNode;

	// Token: 0x040017C7 RID: 6087
	private int cost = 10;

	// Token: 0x040017C8 RID: 6088
	private float[] wrongChoiceChance = new float[]
	{
		0.4f,
		0.2f,
		-1f
	};
}
