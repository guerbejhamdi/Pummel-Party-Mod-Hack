using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000105 RID: 261
[Serializable]
public class LTSpline
{
	// Token: 0x06000771 RID: 1905 RVA: 0x00009221 File Offset: 0x00007421
	public LTSpline(Vector3[] pts)
	{
		this.init(pts, true);
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x00009238 File Offset: 0x00007438
	public LTSpline(Vector3[] pts, bool constantSpeed)
	{
		this.constantSpeed = constantSpeed;
		this.init(pts, constantSpeed);
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x0004AD84 File Offset: 0x00048F84
	private void init(Vector3[] pts, bool constantSpeed)
	{
		if (pts.Length < 4)
		{
			LeanTween.logError("LeanTween - When passing values for a spline path, you must pass four or more values!");
			return;
		}
		this.pts = new Vector3[pts.Length];
		Array.Copy(pts, this.pts, pts.Length);
		this.numSections = pts.Length - 3;
		float num = float.PositiveInfinity;
		Vector3 vector = this.pts[1];
		float num2 = 0f;
		for (int i = 1; i < this.pts.Length - 1; i++)
		{
			float num3 = Vector3.Distance(this.pts[i], vector);
			if (num3 < num)
			{
				num = num3;
			}
			num2 += num3;
		}
		if (constantSpeed)
		{
			num = num2 / (float)(this.numSections * LTSpline.SUBLINE_COUNT);
			float num4 = num / (float)LTSpline.SUBLINE_COUNT;
			int num5 = (int)Mathf.Ceil(num2 / num4) * LTSpline.DISTANCE_COUNT;
			if (num5 <= 1)
			{
				num5 = 2;
			}
			this.ptsAdj = new Vector3[num5];
			vector = this.interp(0f);
			int num6 = 1;
			this.ptsAdj[0] = vector;
			this.distance = 0f;
			for (int j = 0; j < num5 + 1; j++)
			{
				float num7 = (float)j / (float)num5;
				Vector3 vector2 = this.interp(num7);
				float num8 = Vector3.Distance(vector2, vector);
				if (num8 >= num4 || num7 >= 1f)
				{
					this.ptsAdj[num6] = vector2;
					this.distance += num8;
					vector = vector2;
					num6++;
				}
			}
			this.ptsAdjLength = num6;
		}
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x0004AEF8 File Offset: 0x000490F8
	public Vector3 map(float u)
	{
		if (u >= 1f)
		{
			return this.pts[this.pts.Length - 2];
		}
		float num = u * (float)(this.ptsAdjLength - 1);
		int num2 = (int)Mathf.Floor(num);
		int num3 = (int)Mathf.Ceil(num);
		if (num2 < 0)
		{
			num2 = 0;
		}
		Vector3 vector = this.ptsAdj[num2];
		Vector3 a = this.ptsAdj[num3];
		float d = num - (float)num2;
		return vector + (a - vector) * d;
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x0004AF7C File Offset: 0x0004917C
	public Vector3 interp(float t)
	{
		this.currPt = Mathf.Min(Mathf.FloorToInt(t * (float)this.numSections), this.numSections - 1);
		float num = t * (float)this.numSections - (float)this.currPt;
		Vector3 a = this.pts[this.currPt];
		Vector3 a2 = this.pts[this.currPt + 1];
		Vector3 vector = this.pts[this.currPt + 2];
		Vector3 b = this.pts[this.currPt + 3];
		return 0.5f * ((-a + 3f * a2 - 3f * vector + b) * (num * num * num) + (2f * a - 5f * a2 + 4f * vector - b) * (num * num) + (-a + vector) * num + 2f * a2);
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x0004B0B4 File Offset: 0x000492B4
	public float ratioAtPoint(Vector3 pt)
	{
		float num = float.MaxValue;
		int num2 = 0;
		for (int i = 0; i < this.ptsAdjLength; i++)
		{
			float num3 = Vector3.Distance(pt, this.ptsAdj[i]);
			if (num3 < num)
			{
				num = num3;
				num2 = i;
			}
		}
		return (float)num2 / (float)(this.ptsAdjLength - 1);
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x0004B104 File Offset: 0x00049304
	public Vector3 point(float ratio)
	{
		float num = (ratio > 1f) ? 1f : ratio;
		if (!this.constantSpeed)
		{
			return this.interp(num);
		}
		return this.map(num);
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x0004B13C File Offset: 0x0004933C
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

	// Token: 0x06000779 RID: 1913 RVA: 0x0004B1A8 File Offset: 0x000493A8
	public void placeLocal2d(Transform transform, float ratio)
	{
		if (transform.parent == null)
		{
			this.place2d(transform, ratio);
			return;
		}
		transform.localPosition = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			Vector3 vector = this.point(ratio) - transform.localPosition;
			float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			transform.localEulerAngles = new Vector3(0f, 0f, z);
		}
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x00009256 File Offset: 0x00007456
	public void place(Transform transform, float ratio)
	{
		this.place(transform, ratio, Vector3.up);
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x00009265 File Offset: 0x00007465
	public void place(Transform transform, float ratio, Vector3 worldUp)
	{
		transform.position = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			transform.LookAt(this.point(ratio), worldUp);
		}
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x00009293 File Offset: 0x00007493
	public void placeLocal(Transform transform, float ratio)
	{
		this.placeLocal(transform, ratio, Vector3.up);
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x000092A2 File Offset: 0x000074A2
	public void placeLocal(Transform transform, float ratio, Vector3 worldUp)
	{
		transform.localPosition = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			transform.LookAt(transform.parent.TransformPoint(this.point(ratio)), worldUp);
		}
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x0004B22C File Offset: 0x0004942C
	public void gizmoDraw(float t = -1f)
	{
		if (this.ptsAdj == null || this.ptsAdj.Length == 0)
		{
			return;
		}
		Vector3 from = this.ptsAdj[0];
		for (int i = 0; i < this.ptsAdjLength; i++)
		{
			Vector3 vector = this.ptsAdj[i];
			Gizmos.DrawLine(from, vector);
			from = vector;
		}
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x0004B280 File Offset: 0x00049480
	public void drawGizmo(Color color)
	{
		if (this.ptsAdjLength >= 4)
		{
			Vector3 from = this.ptsAdj[0];
			Color color2 = Gizmos.color;
			Gizmos.color = color;
			for (int i = 0; i < this.ptsAdjLength; i++)
			{
				Vector3 vector = this.ptsAdj[i];
				Gizmos.DrawLine(from, vector);
				from = vector;
			}
			Gizmos.color = color2;
		}
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x0004B2DC File Offset: 0x000494DC
	public static void drawGizmo(Transform[] arr, Color color)
	{
		if (arr.Length >= 4)
		{
			Vector3[] array = new Vector3[arr.Length];
			for (int i = 0; i < arr.Length; i++)
			{
				array[i] = arr[i].position;
			}
			LTSpline ltspline = new LTSpline(array);
			Vector3 from = ltspline.ptsAdj[0];
			Color color2 = Gizmos.color;
			Gizmos.color = color;
			for (int j = 0; j < ltspline.ptsAdjLength; j++)
			{
				Vector3 vector = ltspline.ptsAdj[j];
				Gizmos.DrawLine(from, vector);
				from = vector;
			}
			Gizmos.color = color2;
		}
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x000092DB File Offset: 0x000074DB
	public static void drawLine(Transform[] arr, float width, Color color)
	{
		int num = arr.Length;
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x0004B374 File Offset: 0x00049574
	public void drawLinesGLLines(Material outlineMaterial, Color color, float width)
	{
		GL.PushMatrix();
		outlineMaterial.SetPass(0);
		GL.LoadPixelMatrix();
		GL.Begin(1);
		GL.Color(color);
		if (this.constantSpeed)
		{
			if (this.ptsAdjLength >= 4)
			{
				Vector3 v = this.ptsAdj[0];
				for (int i = 0; i < this.ptsAdjLength; i++)
				{
					Vector3 vector = this.ptsAdj[i];
					GL.Vertex(v);
					GL.Vertex(vector);
					v = vector;
				}
			}
		}
		else if (this.pts.Length >= 4)
		{
			Vector3 v2 = this.pts[0];
			float num = 1f / ((float)this.pts.Length * 10f);
			for (float num2 = 0f; num2 < 1f; num2 += num)
			{
				float t = num2 / 1f;
				Vector3 vector2 = this.interp(t);
				GL.Vertex(v2);
				GL.Vertex(vector2);
				v2 = vector2;
			}
		}
		GL.End();
		GL.PopMatrix();
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x0004B45C File Offset: 0x0004965C
	public Vector3[] generateVectors()
	{
		if (this.pts.Length >= 4)
		{
			List<Vector3> list = new List<Vector3>();
			Vector3 item = this.pts[0];
			list.Add(item);
			float num = 1f / ((float)this.pts.Length * 10f);
			for (float num2 = 0f; num2 < 1f; num2 += num)
			{
				float t = num2 / 1f;
				Vector3 item2 = this.interp(t);
				list.Add(item2);
			}
			list.ToArray();
		}
		return null;
	}

	// Token: 0x04000630 RID: 1584
	public static int DISTANCE_COUNT = 3;

	// Token: 0x04000631 RID: 1585
	public static int SUBLINE_COUNT = 20;

	// Token: 0x04000632 RID: 1586
	public float distance;

	// Token: 0x04000633 RID: 1587
	public bool constantSpeed = true;

	// Token: 0x04000634 RID: 1588
	public Vector3[] pts;

	// Token: 0x04000635 RID: 1589
	[NonSerialized]
	public Vector3[] ptsAdj;

	// Token: 0x04000636 RID: 1590
	public int ptsAdjLength;

	// Token: 0x04000637 RID: 1591
	public bool orientToPath;

	// Token: 0x04000638 RID: 1592
	public bool orientToPath2d;

	// Token: 0x04000639 RID: 1593
	private int numSections;

	// Token: 0x0400063A RID: 1594
	private int currPt;
}
