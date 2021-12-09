using System;
using UnityEngine;

// Token: 0x02000103 RID: 259
public class LTBezier
{
	// Token: 0x06000760 RID: 1888 RVA: 0x0004A884 File Offset: 0x00048A84
	public LTBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float precision)
	{
		this.a = a;
		this.aa = -a + 3f * (b - c) + d;
		this.bb = 3f * (a + c) - 6f * b;
		this.cc = 3f * (b - a);
		this.len = 1f / precision;
		this.arcLengths = new float[(int)this.len + 1];
		this.arcLengths[0] = 0f;
		Vector3 vector = a;
		float num = 0f;
		int num2 = 1;
		while ((float)num2 <= this.len)
		{
			Vector3 vector2 = this.bezierPoint((float)num2 * precision);
			num += (vector - vector2).magnitude;
			this.arcLengths[num2] = num;
			vector = vector2;
			num2++;
		}
		this.length = num;
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x0004A980 File Offset: 0x00048B80
	private float map(float u)
	{
		float num = u * this.arcLengths[(int)this.len];
		int i = 0;
		int num2 = (int)this.len;
		int num3 = 0;
		while (i < num2)
		{
			num3 = i + ((int)((float)(num2 - i) / 2f) | 0);
			if (this.arcLengths[num3] < num)
			{
				i = num3 + 1;
			}
			else
			{
				num2 = num3;
			}
		}
		if (this.arcLengths[num3] > num)
		{
			num3--;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		return ((float)num3 + (num - this.arcLengths[num3]) / (this.arcLengths[num3 + 1] - this.arcLengths[num3])) / this.len;
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x00009174 File Offset: 0x00007374
	private Vector3 bezierPoint(float t)
	{
		return ((this.aa * t + this.bb) * t + this.cc) * t + this.a;
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x000091AF File Offset: 0x000073AF
	public Vector3 point(float t)
	{
		return this.bezierPoint(this.map(t));
	}

	// Token: 0x04000621 RID: 1569
	public float length;

	// Token: 0x04000622 RID: 1570
	private Vector3 a;

	// Token: 0x04000623 RID: 1571
	private Vector3 aa;

	// Token: 0x04000624 RID: 1572
	private Vector3 bb;

	// Token: 0x04000625 RID: 1573
	private Vector3 cc;

	// Token: 0x04000626 RID: 1574
	private float len;

	// Token: 0x04000627 RID: 1575
	private float[] arcLengths;
}
