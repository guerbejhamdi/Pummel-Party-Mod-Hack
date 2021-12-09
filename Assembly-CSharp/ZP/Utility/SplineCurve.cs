using System;
using UnityEngine;

namespace ZP.Utility
{
	// Token: 0x020005EF RID: 1519
	public class SplineCurve
	{
		// Token: 0x060026F1 RID: 9969 RVA: 0x0001BB75 File Offset: 0x00019D75
		public SplineCurve(int _start, int _end, int _cp1, int _cp2)
		{
			this.start = _start;
			this.end = _end;
			this.cp1 = _cp1;
			this.cp2 = _cp2;
			this.type = SplineCurveType.Cubic;
		}

		// Token: 0x060026F2 RID: 9970 RVA: 0x0001BBA1 File Offset: 0x00019DA1
		public SplineCurve(int _start, int _end, int _cp)
		{
			this.start = _start;
			this.end = _end;
			this.cp1 = _cp;
			this.type = SplineCurveType.Quadratic;
		}

		// Token: 0x060026F3 RID: 9971 RVA: 0x000EACBC File Offset: 0x000E8EBC
		public Vector3 GetCurvePoint(float t, Spline sp)
		{
			Vector3 result = Vector3.zero;
			if (this.type == SplineCurveType.Cubic)
			{
				result = CurveUtility.GetCubicBezierPoint(t, sp.points[this.start], sp.points[this.end], sp.control_points[this.cp1], sp.control_points[this.cp2]);
			}
			else
			{
				result = CurveUtility.GetQuadraticBezierPoint(t, sp.points[this.start], sp.points[this.end], sp.control_points[this.cp1]);
			}
			return result;
		}

		// Token: 0x04002A85 RID: 10885
		public SplineCurveType type;

		// Token: 0x04002A86 RID: 10886
		public int start;

		// Token: 0x04002A87 RID: 10887
		public int end;

		// Token: 0x04002A88 RID: 10888
		public int cp1;

		// Token: 0x04002A89 RID: 10889
		public int cp2;

		// Token: 0x04002A8A RID: 10890
		public float start_distance;

		// Token: 0x04002A8B RID: 10891
		public float end_distance;

		// Token: 0x04002A8C RID: 10892
		public float length;
	}
}
