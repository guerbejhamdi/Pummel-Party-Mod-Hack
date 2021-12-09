using System;
using UnityEngine;

// Token: 0x02000104 RID: 260
public class LTBezierPath
{
	// Token: 0x06000764 RID: 1892 RVA: 0x00004023 File Offset: 0x00002223
	public LTBezierPath()
	{
	}

	// Token: 0x06000765 RID: 1893 RVA: 0x000091BE File Offset: 0x000073BE
	public LTBezierPath(Vector3[] pts_)
	{
		this.setPoints(pts_);
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x0004AA10 File Offset: 0x00048C10
	public void setPoints(Vector3[] pts_)
	{
		if (pts_.Length < 4)
		{
			LeanTween.logError("LeanTween - When passing values for a vector path, you must pass four or more values!");
		}
		if (pts_.Length % 4 != 0)
		{
			LeanTween.logError("LeanTween - When passing values for a vector path, they must be in sets of four: controlPoint1, controlPoint2, endPoint2, controlPoint2, controlPoint2...");
		}
		this.pts = pts_;
		int num = 0;
		this.beziers = new LTBezier[this.pts.Length / 4];
		this.lengthRatio = new float[this.beziers.Length];
		this.length = 0f;
		for (int i = 0; i < this.pts.Length; i += 4)
		{
			this.beziers[num] = new LTBezier(this.pts[i], this.pts[i + 2], this.pts[i + 1], this.pts[i + 3], 0.05f);
			this.length += this.beziers[num].length;
			num++;
		}
		for (int i = 0; i < this.beziers.Length; i++)
		{
			this.lengthRatio[i] = this.beziers[i].length / this.length;
		}
	}

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06000767 RID: 1895 RVA: 0x000091CD File Offset: 0x000073CD
	public float distance
	{
		get
		{
			return this.length;
		}
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x0004AB24 File Offset: 0x00048D24
	public Vector3 point(float ratio)
	{
		float num = 0f;
		for (int i = 0; i < this.lengthRatio.Length; i++)
		{
			num += this.lengthRatio[i];
			if (num >= ratio)
			{
				return this.beziers[i].point((ratio - (num - this.lengthRatio[i])) / this.lengthRatio[i]);
			}
		}
		return this.beziers[this.lengthRatio.Length - 1].point(1f);
	}

	// Token: 0x06000769 RID: 1897 RVA: 0x0004AB98 File Offset: 0x00048D98
	public void place2d(Transform transform, float ratio)
	{
		transform.position = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			Vector3 vector = this.point(ratio) - transform.position;
			float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			transform.eulerAngles = new Vector3(0f, 0f, z);
		}
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x0004AC04 File Offset: 0x00048E04
	public void placeLocal2d(Transform transform, float ratio)
	{
		transform.localPosition = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			Vector3 vector = this.point(ratio) - transform.localPosition;
			float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			transform.localEulerAngles = new Vector3(0f, 0f, z);
		}
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x000091D5 File Offset: 0x000073D5
	public void place(Transform transform, float ratio)
	{
		this.place(transform, ratio, Vector3.up);
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x000091E4 File Offset: 0x000073E4
	public void place(Transform transform, float ratio, Vector3 worldUp)
	{
		transform.position = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			transform.LookAt(this.point(ratio), worldUp);
		}
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x00009212 File Offset: 0x00007412
	public void placeLocal(Transform transform, float ratio)
	{
		this.placeLocal(transform, ratio, Vector3.up);
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x0004AC70 File Offset: 0x00048E70
	public void placeLocal(Transform transform, float ratio, Vector3 worldUp)
	{
		ratio = Mathf.Clamp01(ratio);
		transform.localPosition = this.point(ratio);
		ratio = Mathf.Clamp01(ratio + 0.001f);
		if (ratio <= 1f)
		{
			transform.LookAt(transform.parent.TransformPoint(this.point(ratio)), worldUp);
		}
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x0004ACC4 File Offset: 0x00048EC4
	public void gizmoDraw(float t = -1f)
	{
		Vector3 to = this.point(0f);
		for (int i = 1; i <= 120; i++)
		{
			float ratio = (float)i / 120f;
			Vector3 vector = this.point(ratio);
			Gizmos.color = ((this.previousBezier == this.currentBezier) ? Color.magenta : Color.grey);
			Gizmos.DrawLine(vector, to);
			to = vector;
			this.previousBezier = this.currentBezier;
		}
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x0004AD30 File Offset: 0x00048F30
	public float ratioAtPoint(Vector3 pt, float precision = 0.01f)
	{
		float num = float.MaxValue;
		int num2 = 0;
		int num3 = Mathf.RoundToInt(1f / precision);
		for (int i = 0; i < num3; i++)
		{
			float ratio = (float)i / (float)num3;
			float num4 = Vector3.Distance(pt, this.point(ratio));
			if (num4 < num)
			{
				num = num4;
				num2 = i;
			}
		}
		return (float)num2 / (float)num3;
	}

	// Token: 0x04000628 RID: 1576
	public Vector3[] pts;

	// Token: 0x04000629 RID: 1577
	public float length;

	// Token: 0x0400062A RID: 1578
	public bool orientToPath;

	// Token: 0x0400062B RID: 1579
	public bool orientToPath2d;

	// Token: 0x0400062C RID: 1580
	private LTBezier[] beziers;

	// Token: 0x0400062D RID: 1581
	private float[] lengthRatio;

	// Token: 0x0400062E RID: 1582
	private int currentBezier;

	// Token: 0x0400062F RID: 1583
	private int previousBezier;
}
