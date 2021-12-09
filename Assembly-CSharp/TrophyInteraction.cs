using System;
using System.Collections;
using UnityEngine;
using ZP.Net;

// Token: 0x02000398 RID: 920
public class TrophyInteraction : Interaction
{
	// Token: 0x060018CB RID: 6347 RVA: 0x000A9554 File Offset: 0x000A7754
	public override void Setup(BoardPlayer player)
	{
		this.dialog = GameManager.UIController.InteractionDialog;
		base.Setup(player);
		((SimpleInteractionDialog)this.dialog).Activate(this.title, this.description, this.buttonSettings, player.GamePlayer, this.icon);
		Vector3 normalized = (player.transform.position - base.transform.position).normalized;
		player.PlayerAnimation.SetPlayerRotation(Mathf.Atan2(normalized.x, normalized.z) * 57.29578f - 180f);
	}

	// Token: 0x060018CC RID: 6348 RVA: 0x000124CF File Offset: 0x000106CF
	public override IEnumerator DoInteraction(byte choice)
	{
		bool gotTrophy = false;
		Debug.Log("Doing Interaction: " + choice.ToString());
		if (choice > 0)
		{
			int index = GameManager.Board.GetGoalIndex(this.player.CurrentNode);
			if (!this.isFakeChest)
			{
				GameManager.Board.GoalScript[index].Open();
				yield return new WaitForSeconds(GameManager.Board.isHalloweenMap ? 2f : 5f);
				gotTrophy = this.player.GiveTrophy(choice, GameManager.Board.goalCost, false);
				if (NetSystem.IsServer)
				{
					if (this.player.GoalScore >= GameManager.WinningRelics)
					{
						GameManager.Board.OnTurnsEnd();
					}
					else
					{
						GameManager.Board.SpawnNewGoal(index);
					}
				}
				yield return new WaitForSeconds(GameManager.Board.isHalloweenMap ? 1f : 2f);
			}
			else
			{
				GameManager.Board.GoalScript[index].Despawn();
				GameManager.Board.GoalNode[index].ResetNode();
				GameManager.Board.GoalNode[index] = null;
				Debug.LogError("Fake Chest Opened");
			}
		}
		if (!gotTrophy)
		{
			GameManager.Board.EndInteraction();
		}
		base.Finished = true;
		yield break;
	}

	// Token: 0x060018CD RID: 6349 RVA: 0x000124E5 File Offset: 0x000106E5
	public override void OnInteractionChoice(byte choice, int seed)
	{
		new System.Random(seed);
		this.dialog.window.SetState(MainMenuWindowState.Hidden);
		GameManager.UIController.SetInputStatus(false);
		base.StartCoroutine(this.DoInteraction(choice));
	}

	// Token: 0x060018CE RID: 6350 RVA: 0x00012518 File Offset: 0x00010718
	public override int GetAIChoice()
	{
		if (this.player.Gold < (int)GameManager.Board.goalCost)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x04001A65 RID: 6757
	public bool isFakeChest;
}
