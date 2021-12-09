using System;
using UnityEngine;

// Token: 0x0200032F RID: 815
public class DelayedClip
{
	// Token: 0x06001628 RID: 5672 RVA: 0x00010B33 File Offset: 0x0000ED33
	public DelayedClip(AudioClip _clip, float _delay_time)
	{
		this.clip = _clip;
		this.delay_time = _delay_time;
	}

	// Token: 0x0400176A RID: 5994
	public AudioClip clip;

	// Token: 0x0400176B RID: 5995
	public float delay_time;

	// Token: 0x0400176C RID: 5996
	public float time_counter;
}
