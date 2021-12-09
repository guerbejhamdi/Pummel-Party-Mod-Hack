using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004E4 RID: 1252
public class TwitchMapEvent : MainBoardEvent
{
	// Token: 0x06002109 RID: 8457 RVA: 0x00017FB3 File Offset: 0x000161B3
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode boardNode, int seed)
	{
		GameManager.Board.curMainBoardEvent = this;
		this.rand = new System.Random(seed);
		yield return new WaitForSeconds(0.5f);
		AudioSystem.PlayOneShot(this.staticSound, 0.8f, 0f, 1f);
		this.glitchAnimation.SetTrigger("Glitch");
		yield return new WaitForSeconds(1.5f);
		for (int i = 0; i < GameManager.GetPlayerCount(); i++)
		{
			GamePlayer playerAt = GameManager.GetPlayerAt(i);
			playerAt.BoardObject.TwitchMapEvent = true;
			playerAt.BoardObject.TwitchMapEventFailRolls = 0;
			playerAt.BoardObject.transform.position = this.spawnPoints[i].position;
		}
		GameManager.Board.boardCamera.MoveToInstant(this.lookPoint.position, GameManager.Board.PlayerCamOffset);
		yield return new WaitForSeconds(2f);
		yield break;
	}

	// Token: 0x0600210A RID: 8458 RVA: 0x00017FC9 File Offset: 0x000161C9
	public override IEnumerator DoTurnStartEvent(BoardPlayer player)
	{
		base.Finished = true;
		yield return null;
		yield break;
	}

	// Token: 0x0600210B RID: 8459 RVA: 0x00017FD8 File Offset: 0x000161D8
	public override IEnumerator DoFirstTurnEvent(BoardPlayer player)
	{
		base.Finished = true;
		yield return null;
		yield break;
	}

	// Token: 0x0600210C RID: 8460 RVA: 0x000CD92C File Offset: 0x000CBB2C
	public override int GetEventValue1()
	{
		int num = 0;
		for (int i = 0; i < GameManager.GetPlayerCount(); i++)
		{
			num |= (GameManager.GetPlayerAt(i).BoardObject.TwitchMapEvent ? 1 : 0) << i;
		}
		return num;
	}

	// Token: 0x0600210D RID: 8461 RVA: 0x000CD96C File Offset: 0x000CBB6C
	public override int GetEventValue2()
	{
		uint num = 0U;
		for (int i = 0; i < GameManager.GetPlayerCount(); i++)
		{
			num |= (uint)((uint)GameManager.GetPlayerAt(i).BoardObject.TwitchMapEventFailRolls << i * 3);
		}
		return (int)num;
	}

	// Token: 0x0600210E RID: 8462 RVA: 0x000CD9A8 File Offset: 0x000CBBA8
	public override void SetupFromLoad(int val1, int val2)
	{
		GameManager.Board.curMainBoardEvent = this;
		for (int i = 0; i < GameManager.GetPlayerCount(); i++)
		{
			if ((val1 & 1 << i) != 0)
			{
				GamePlayer playerAt = GameManager.GetPlayerAt(i);
				playerAt.BoardObject.TwitchMapEvent = true;
				int num = 29 - i * 3;
				int num2 = num + i * 3;
				playerAt.BoardObject.TwitchMapEventFailRolls = (int)((uint)((uint)val2 << num) >> num2);
				playerAt.BoardObject.transform.position = this.spawnPoints[i].position;
			}
		}
		base.SetupFromLoad(val1, val2);
	}

	// Token: 0x040023C4 RID: 9156
	public Animator glitchAnimation;

	// Token: 0x040023C5 RID: 9157
	public AudioClip staticSound;

	// Token: 0x040023C6 RID: 9158
	public Transform[] spawnPoints;

	// Token: 0x040023C7 RID: 9159
	public Transform lookPoint;
}
