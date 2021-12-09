using System;

// Token: 0x0200026C RID: 620
public class TargetHitResult
{
	// Token: 0x06001221 RID: 4641 RVA: 0x0000EA77 File Offset: 0x0000CC77
	public TargetHitResult(int _score, TargetHitResultType _type, float _time)
	{
		this.score = _score;
		this.type = _type;
		this.time = _time;
	}

	// Token: 0x040012EC RID: 4844
	public int score;

	// Token: 0x040012ED RID: 4845
	public TargetHitResultType type;

	// Token: 0x040012EE RID: 4846
	public float time;
}
