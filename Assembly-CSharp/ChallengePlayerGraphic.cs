using System;
using UnityEngine;

// Token: 0x0200031B RID: 795
public class ChallengePlayerGraphic : MonoBehaviour
{
	// Token: 0x060015C0 RID: 5568 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Awake()
	{
	}

	// Token: 0x060015C1 RID: 5569 RVA: 0x00010748 File Offset: 0x0000E948
	public void SetPlayer(GamePlayer player)
	{
		this.player = player;
	}

	// Token: 0x060015C2 RID: 5570 RVA: 0x0009C814 File Offset: 0x0009AA14
	public void SetGame(ChallengeItemGameType game)
	{
		this.curGameType = game;
		this.challengeGameData[(int)this.curGameType].graphic.SetActive(true);
		this.gameTypeSet = true;
		this.challengeGameData[(int)this.curGameType].image.color = this.player.Color.skinColor1;
		ChallengeItemGameType challengeItemGameType = this.curGameType;
		if (challengeItemGameType != ChallengeItemGameType.RoadDodge)
		{
		}
	}

	// Token: 0x040016E7 RID: 5863
	public ChallengeGamePlayerData[] challengeGameData;

	// Token: 0x040016E8 RID: 5864
	private ChallengeItemGameType curGameType;

	// Token: 0x040016E9 RID: 5865
	private Vector3 lastPosition = Vector3.zero;

	// Token: 0x040016EA RID: 5866
	private bool gameTypeSet;

	// Token: 0x040016EB RID: 5867
	private GamePlayer player;
}
