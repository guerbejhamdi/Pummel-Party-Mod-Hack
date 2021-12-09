using System;

// Token: 0x020003C7 RID: 967
public enum BoardPlayerState
{
	// Token: 0x04001B37 RID: 6967
	Idle,
	// Token: 0x04001B38 RID: 6968
	GetTurnInput,
	// Token: 0x04001B39 RID: 6969
	Moving,
	// Token: 0x04001B3A RID: 6970
	WaitingIntersection,
	// Token: 0x04001B3B RID: 6971
	MoveOffset,
	// Token: 0x04001B3C RID: 6972
	MoveArc,
	// Token: 0x04001B3D RID: 6973
	ViewingMap,
	// Token: 0x04001B3E RID: 6974
	ItemEquipped,
	// Token: 0x04001B3F RID: 6975
	ItemUsing,
	// Token: 0x04001B40 RID: 6976
	MakingInteractionChoice,
	// Token: 0x04001B41 RID: 6977
	Interacting,
	// Token: 0x04001B42 RID: 6978
	CompletedInteraction,
	// Token: 0x04001B43 RID: 6979
	CompletedDirectionChoice,
	// Token: 0x04001B44 RID: 6980
	Dying,
	// Token: 0x04001B45 RID: 6981
	Dead,
	// Token: 0x04001B46 RID: 6982
	InventoryOpen,
	// Token: 0x04001B47 RID: 6983
	Ragdolling,
	// Token: 0x04001B48 RID: 6984
	MoveTeleport
}
