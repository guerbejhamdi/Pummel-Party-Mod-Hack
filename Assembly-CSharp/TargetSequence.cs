using System;

// Token: 0x02000272 RID: 626
[Serializable]
public class TargetSequence
{
	// Token: 0x0400131C RID: 4892
	public int level;

	// Token: 0x0400131D RID: 4893
	public TargetAction[] activeTargets;

	// Token: 0x0400131E RID: 4894
	public float totalActiveTime = 1f;
}
