using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000A7 RID: 167
public class HedgeMazeEvent : MainBoardEvent
{
	// Token: 0x0600037F RID: 895 RVA: 0x00005E81 File Offset: 0x00004081
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode boardNode, int seed)
	{
		yield return new WaitForSeconds(0.15f);
		base.DoGenericBoardEventActions();
		this.rand = new System.Random(seed);
		GameManager.Board.boardCamera.MoveTo(base.transform, Vector3.zero, GameManager.Board.boardCamera.targetDistScale);
		yield return new WaitUntil(() => GameManager.Board.boardCamera.WithinDistance(0.15f));
		yield return base.StartCoroutine(this.generator.UpdateMaze(seed, false));
		yield return new WaitForSeconds(0.35f);
		yield break;
	}

	// Token: 0x0400038D RID: 909
	public HedgeMazeGenerator generator;
}
