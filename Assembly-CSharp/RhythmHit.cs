using System;

// Token: 0x0200021C RID: 540
public class RhythmHit
{
	// Token: 0x06000FBF RID: 4031 RVA: 0x0000D78C File Offset: 0x0000B98C
	public RhythmHit(float start, float end, RhythmHitType type, RhythmHitButton button)
	{
		this.startTime = start;
		this.endTime = end;
		this.button = button;
		this.type = type;
	}

	// Token: 0x04000FEB RID: 4075
	public float startTime;

	// Token: 0x04000FEC RID: 4076
	public float endTime;

	// Token: 0x04000FED RID: 4077
	public RhythmHitType type;

	// Token: 0x04000FEE RID: 4078
	public RhythmHitButton button;
}
