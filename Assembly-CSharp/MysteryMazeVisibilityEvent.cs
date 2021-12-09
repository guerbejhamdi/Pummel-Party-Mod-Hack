using System;

// Token: 0x020001E4 RID: 484
public class MysteryMazeVisibilityEvent
{
	// Token: 0x06000E15 RID: 3605 RVA: 0x0000C981 File Offset: 0x0000AB81
	public MysteryMazeVisibilityEvent(float s, float t)
	{
		this.startTime = s;
		this.endTime = t;
	}

	// Token: 0x04000D87 RID: 3463
	public float startTime;

	// Token: 0x04000D88 RID: 3464
	public float endTime;

	// Token: 0x04000D89 RID: 3465
	public bool started;

	// Token: 0x04000D8A RID: 3466
	public bool completed;
}
