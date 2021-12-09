using System;
using UnityEngine;

// Token: 0x0200046D RID: 1133
[Serializable]
public class SimpleAnimationLayer
{
	// Token: 0x17000399 RID: 921
	// (get) Token: 0x06001E9B RID: 7835 RVA: 0x000168AB File Offset: 0x00014AAB
	// (set) Token: 0x06001E9C RID: 7836 RVA: 0x000168B3 File Offset: 0x00014AB3
	public float Time
	{
		get
		{
			return this.time;
		}
		set
		{
			this.time = value;
		}
	}

	// Token: 0x040021AB RID: 8619
	public SimpleAnimationType animation_type;

	// Token: 0x040021AC RID: 8620
	public float animation_speed = 1f;

	// Token: 0x040021AD RID: 8621
	public bool loop = true;

	// Token: 0x040021AE RID: 8622
	public bool randomOffset = true;

	// Token: 0x040021AF RID: 8623
	public Vector3 rotation_axis = Vector3.up;

	// Token: 0x040021B0 RID: 8624
	public float rotation_speed;

	// Token: 0x040021B1 RID: 8625
	public Vector3 movement_offset = Vector3.up;

	// Token: 0x040021B2 RID: 8626
	public AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	// Token: 0x040021B3 RID: 8627
	private float time;

	// Token: 0x040021B4 RID: 8628
	private bool active;
}
