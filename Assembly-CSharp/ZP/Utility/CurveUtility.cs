using System;
using UnityEngine;

namespace ZP.Utility
{
	// Token: 0x020005EC RID: 1516
	public static class CurveUtility
	{
		// Token: 0x060026E7 RID: 9959 RVA: 0x000EABB4 File Offset: 0x000E8DB4
		public static Vector3 GetQuadraticBezierPoint(float t, Vector3 p1, Vector3 p2, Vector3 cp)
		{
			float num = 1f - t;
			float d = t * t;
			return num * num * p1 + 2f * num * t * cp + d * p2;
		}

		// Token: 0x060026E8 RID: 9960 RVA: 0x0001BB0A File Offset: 0x00019D0A
		public static Vector3 GetQuadraticBezierTangent(float t, Vector3 p1, Vector3 p2, Vector3 cp)
		{
			return 2f * (1f - t) * (cp - p1) + 2f * t * (p2 - cp);
		}

		// Token: 0x060026E9 RID: 9961 RVA: 0x000EABF8 File Offset: 0x000E8DF8
		public static Vector3 GetCubicBezierPoint(float t, Vector3 p1, Vector3 p2, Vector3 cp1, Vector3 cp2)
		{
			float num = 1f - t;
			float num2 = t * t;
			float d = num2 * t;
			float num3 = num * num;
			return num3 * num * p1 + 3f * num3 * t * cp1 + 3f * num * num2 * cp2 + d * p2;
		}

		// Token: 0x060026EA RID: 9962 RVA: 0x000EAC58 File Offset: 0x000E8E58
		public static Vector3 GetCubiceBezierTangent(float t, Vector3 p1, Vector3 p2, Vector3 cp1, Vector3 cp2)
		{
			float num = 1f - t;
			float num2 = num * num;
			return 3f * num2 * (cp1 - p1) + 6f * num * t * (cp2 - cp1) + 3f * (t * t) * (p2 - cp2);
		}
	}
}
