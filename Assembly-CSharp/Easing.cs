using System;
using UnityEngine;

// Token: 0x02000454 RID: 1108
public static class Easing
{
	// Token: 0x06001E64 RID: 7780 RVA: 0x000166AE File Offset: 0x000148AE
	public static float SineEaseIn(float a)
	{
		return Mathf.Sin((a - 1f) * 1.5707964f) + 1f;
	}

	// Token: 0x06001E65 RID: 7781 RVA: 0x000166C8 File Offset: 0x000148C8
	public static float BackEaseIn(float a)
	{
		return a * a * a - a * Mathf.Sin(a * 3.1415927f);
	}

	// Token: 0x06001E66 RID: 7782 RVA: 0x000C41E0 File Offset: 0x000C23E0
	public static float BackEaseOut(float a)
	{
		float num = 1f - a;
		return 1f - (num * num * num - num * Mathf.Sin(num * 3.1415927f));
	}

	// Token: 0x06001E67 RID: 7783 RVA: 0x000166DE File Offset: 0x000148DE
	public static float ElasticEaseIn(float a)
	{
		return Mathf.Sin(20.420353f * a) * Mathf.Pow(2f, 10f * (a - 1f));
	}

	// Token: 0x06001E68 RID: 7784 RVA: 0x00016704 File Offset: 0x00014904
	public static float ElasticEaseOut(float a)
	{
		return Mathf.Sin(-20.420353f * (a + 1f)) * Mathf.Pow(2f, -10f * a) + 1f;
	}

	// Token: 0x04002150 RID: 8528
	private const float HALFPI = 1.5707964f;
}
