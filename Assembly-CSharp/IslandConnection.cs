using System;

// Token: 0x020000B8 RID: 184
[Serializable]
public struct IslandConnection
{
	// Token: 0x04000412 RID: 1042
	public BoardNode start;

	// Token: 0x04000413 RID: 1043
	public BoardNode end;

	// Token: 0x04000414 RID: 1044
	public BoardNodeTransition transition;

	// Token: 0x04000415 RID: 1045
	public float startOffset;

	// Token: 0x04000416 RID: 1046
	public float endOffset;

	// Token: 0x04000417 RID: 1047
	public float startHeightOffset;

	// Token: 0x04000418 RID: 1048
	public float endHeightOffset;
}
