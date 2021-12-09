using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x020001B5 RID: 437
public class HorsesController : MinigameController
{
	// Token: 0x06000C9A RID: 3226 RVA: 0x0006A058 File Offset: 0x00068258
	public override void InitializeMinigame()
	{
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("HorsesPlayer", null);
			List<int> list = new List<int>
			{
				0,
				1,
				2,
				3
			};
			int[] array = new int[4];
			for (int i = 0; i < array.Length; i++)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				array[i] = list[index];
				list.RemoveAt(index);
			}
			for (int j = 0; j < 2; j++)
			{
				int num = array[j * 2];
				HorseMover component = base.NetSpawn("HorseMover", base.SpawnPoints[j].position, base.SpawnPoints[j].rotation, (ushort)num, GameManager.GetPlayerAt(j).NetOwner).GetComponent<HorseMover>();
				component.RecieveMoverID(num);
				component.RecieveJumperID(array[j * 2 + 1]);
			}
		}
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x00009BEC File Offset: 0x00007DEC
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.StartMinigame();
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x0000BCDB File Offset: 0x00009EDB
	public override void OnPlayersReady()
	{
		base.OnPlayersReady();
	}

	// Token: 0x06000C9D RID: 3229 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000C9E RID: 3230 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000C9F RID: 3231 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000CA0 RID: 3232 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x06000CA1 RID: 3233 RVA: 0x0000AB1C File Offset: 0x00008D1C
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer && this.ui_timer.time_test <= 0f)
		{
			base.EndRound(1f, 3f, false);
		}
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x06000CA4 RID: 3236 RVA: 0x0000A236 File Offset: 0x00008436
	public override void BuildResults()
	{
		base.BuildResults();
	}
}
