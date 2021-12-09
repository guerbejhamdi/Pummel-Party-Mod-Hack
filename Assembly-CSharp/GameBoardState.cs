using System;

// Token: 0x020003D8 RID: 984
public enum GameBoardState
{
	// Token: 0x04001C60 RID: 7264
	Loading,
	// Token: 0x04001C61 RID: 7265
	Initializing,
	// Token: 0x04001C62 RID: 7266
	DetermineTurnOrder,
	// Token: 0x04001C63 RID: 7267
	PlayTurns,
	// Token: 0x04001C64 RID: 7268
	SpawnGoal,
	// Token: 0x04001C65 RID: 7269
	Minigame,
	// Token: 0x04001C66 RID: 7270
	ShowMinigameResults,
	// Token: 0x04001C67 RID: 7271
	SelectingMinigame,
	// Token: 0x04001C68 RID: 7272
	EndingGame,
	// Token: 0x04001C69 RID: 7273
	LoadingSave,
	// Token: 0x04001C6A RID: 7274
	MinigamesOnlyPlay,
	// Token: 0x04001C6B RID: 7275
	ShowingKiller,
	// Token: 0x04001C6C RID: 7276
	KillersTurn,
	// Token: 0x04001C6D RID: 7277
	PummelAwards,
	// Token: 0x04001C6E RID: 7278
	RunPersistentItems
}
