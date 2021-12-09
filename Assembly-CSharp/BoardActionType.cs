using System;

// Token: 0x0200039A RID: 922
public enum BoardActionType
{
	// Token: 0x04001A6D RID: 6765
	NoAction,
	// Token: 0x04001A6E RID: 6766
	Simple,
	// Token: 0x04001A6F RID: 6767
	Wait,
	// Token: 0x04001A70 RID: 6768
	ChangeGameState,
	// Token: 0x04001A71 RID: 6769
	SpawnGoal,
	// Token: 0x04001A72 RID: 6770
	MovePlayer,
	// Token: 0x04001A73 RID: 6771
	ChooseDirection,
	// Token: 0x04001A74 RID: 6772
	HitDice,
	// Token: 0x04001A75 RID: 6773
	StartTurn,
	// Token: 0x04001A76 RID: 6774
	LoadMinigame,
	// Token: 0x04001A77 RID: 6775
	DoNodeEvent,
	// Token: 0x04001A78 RID: 6776
	KillPlayer,
	// Token: 0x04001A79 RID: 6777
	ShowMinigameResults,
	// Token: 0x04001A7A RID: 6778
	EquipItem,
	// Token: 0x04001A7B RID: 6779
	UseItem,
	// Token: 0x04001A7C RID: 6780
	UnEquipItem,
	// Token: 0x04001A7D RID: 6781
	InteractionChoice,
	// Token: 0x04001A7E RID: 6782
	ShowWinner,
	// Token: 0x04001A7F RID: 6783
	SetupMinigameOnlyLobby,
	// Token: 0x04001A80 RID: 6784
	KillerMove,
	// Token: 0x04001A81 RID: 6785
	KillerAttack,
	// Token: 0x04001A82 RID: 6786
	StartPummelAwards,
	// Token: 0x04001A83 RID: 6787
	EndPummelAwards,
	// Token: 0x04001A84 RID: 6788
	SendStatistics,
	// Token: 0x04001A85 RID: 6789
	SpawnWeapon,
	// Token: 0x04001A86 RID: 6790
	PersistentItemEvent,
	// Token: 0x04001A87 RID: 6791
	ShellGame
}
