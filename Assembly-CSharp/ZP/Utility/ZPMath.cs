using System;
using UnityEngine;

namespace ZP.Utility
{
	// Token: 0x020005F2 RID: 1522
	public static class ZPMath
	{
		// Token: 0x06002749 RID: 10057 RVA: 0x0001BEEC File Offset: 0x0001A0EC
		public static uint NextPowerOfTwo(uint v)
		{
			v -= 1U;
			v |= v >> 1;
			v |= v >> 2;
			v |= v >> 4;
			v |= v >> 8;
			v |= v >> 16;
			v += 1U;
			return v;
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x0001BF1D File Offset: 0x0001A11D
		public static float UShortToFloat(ushort value, float maxValue)
		{
			return (float)value * maxValue / 65535f;
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x0001BF29 File Offset: 0x0001A129
		public static float UShortToFloat(ushort value, float min, float max)
		{
			return ZPMath.UShortToFloat(value, max - min) + min;
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x0001BF36 File Offset: 0x0001A136
		public static ushort FloatToUShort(float sample, float max_value)
		{
			return (ushort)(sample / max_value * 65535f);
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x0001BF42 File Offset: 0x0001A142
		public static ushort FloatToUShort(float sample, float min, float max)
		{
			return ZPMath.FloatToUShort(sample - min, max - min);
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x0001BF4F File Offset: 0x0001A14F
		public static float ByteToFloat(byte value, float maxValue)
		{
			return (float)value * maxValue / 255f;
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x0001BF5B File Offset: 0x0001A15B
		public static float ByteToFloat(byte value, float min, float max)
		{
			return ZPMath.ByteToFloat(value, max - min) + min;
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x0001BF68 File Offset: 0x0001A168
		public static byte FloatToByte(float sample, float max_value)
		{
			return (byte)(sample / max_value * 255f);
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x0001BF74 File Offset: 0x0001A174
		public static byte FloatToByte(float sample, float min, float max)
		{
			return ZPMath.FloatToByte(sample - min, max - min);
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x000EC358 File Offset: 0x000EA558
		public static byte CompressFloatToByte(float x, float min, float max)
		{
			double num = (double)Mathf.Abs(min - max);
			double num2 = (double)Mathf.Abs(min - Mathf.Clamp(x, min, max));
			return (byte)(255.0 * (num2 / num));
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x000EC390 File Offset: 0x000EA590
		public static float DecompressByteToFloat(byte x, float min, float max)
		{
			double num = (double)Mathf.Abs(min - max);
			return (float)((double)x / 255.0 * num - (double)Mathf.Abs(min));
		}

		// Token: 0x06002754 RID: 10068 RVA: 0x000EC3C0 File Offset: 0x000EA5C0
		public static short CompressFloatToShort(float x, float min, float max)
		{
			double num = (double)Mathf.Abs(min - max);
			double num2 = (double)Mathf.Abs(min - Mathf.Clamp(x, min, max));
			return (short)(32767.0 * (num2 / num));
		}

		// Token: 0x06002755 RID: 10069 RVA: 0x000EC3F8 File Offset: 0x000EA5F8
		public static float DecompressShortToFloat(short x, float min, float max)
		{
			double num = (double)Mathf.Abs(min - max);
			return (float)((double)x / 32767.0 * num - (double)Mathf.Abs(min));
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x000EC428 File Offset: 0x000EA628
		public static ushort CompressFloatToUShort(float x, float min, float max)
		{
			double num = (double)Mathf.Abs(min - max);
			double num2 = (double)Mathf.Abs(min - Mathf.Clamp(x, min, max));
			return (ushort)(65535.0 * (num2 / num));
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x000EC460 File Offset: 0x000EA660
		public static float DecompressUShortToFloat(ushort x, float min, float max)
		{
			double num = (double)Mathf.Abs(min - max);
			return (float)((double)x / 65535.0 * num - (double)Mathf.Abs(min));
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x000EC490 File Offset: 0x000EA690
		public static byte CompressFloat(float x, float min, float max)
		{
			float num = Mathf.Abs(min - max);
			float num2 = Mathf.Abs(min - Mathf.Clamp(x, min, max));
			return (byte)(255f * (num2 / num));
		}

		// Token: 0x06002759 RID: 10073 RVA: 0x000EC4C0 File Offset: 0x000EA6C0
		public static float DecompressFloat(byte x, float min, float max)
		{
			float num = Mathf.Abs(min - max);
			return (float)x / 255f * num - Mathf.Abs(min);
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x000EC4E8 File Offset: 0x000EA6E8
		public static Rect GetAtlasRect(int pos, int texture_size, int rows, int columns)
		{
			float num = (float)(texture_size / rows) / (float)texture_size;
			float num2 = (float)(pos % columns) * num;
			float num3 = (float)(pos / columns) * num;
			float num4 = 0.0001f;
			return new Rect(num2 + num4, 1f - num3 - num + num4, num - num4, num - num4);
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x000EC528 File Offset: 0x000EA728
		public static float PackColor(Color c)
		{
			Vector4 a = new Vector4(c.r, c.g, c.b, c.a);
			Vector4 b = new Vector4(1f, 0.003921569f, 1.53787E-05f, 6.030863E-08f);
			return Vector4.Dot(a, b);
		}

		// Token: 0x0600275C RID: 10076 RVA: 0x000EC574 File Offset: 0x000EA774
		public static Vector3 ProjectPointOnLine(Vector3 linePoint, Vector3 lineVec, Vector3 point)
		{
			float d = Vector3.Dot(point - linePoint, lineVec);
			return linePoint + lineVec * d;
		}

		// Token: 0x0600275D RID: 10077 RVA: 0x000EC59C File Offset: 0x000EA79C
		public static Vector3 ProjectPointOnLineSegment(Vector3 linePoint1, Vector3 linePoint2, Vector3 point)
		{
			Vector3 vector = ZPMath.ProjectPointOnLine(linePoint1, (linePoint2 - linePoint1).normalized, point);
			int num = ZPMath.PointOnWhichSideOfLineSegment(linePoint1, linePoint2, vector);
			if (num == 0)
			{
				return vector;
			}
			if (num == 1)
			{
				return linePoint1;
			}
			if (num == 2)
			{
				return linePoint2;
			}
			return Vector3.zero;
		}

		// Token: 0x0600275E RID: 10078 RVA: 0x000EC5E0 File Offset: 0x000EA7E0
		public static int PointOnWhichSideOfLineSegment(Vector3 linePoint1, Vector3 linePoint2, Vector3 point)
		{
			Vector3 rhs = linePoint2 - linePoint1;
			Vector3 lhs = point - linePoint1;
			if (Vector3.Dot(lhs, rhs) <= 0f)
			{
				return 1;
			}
			if (lhs.magnitude <= rhs.magnitude)
			{
				return 0;
			}
			return 2;
		}

		// Token: 0x0600275F RID: 10079 RVA: 0x000EC620 File Offset: 0x000EA820
		public static float SqrDistanceToLine(Vector3 line, Vector3 lineOrigin, Vector3 point)
		{
			return Vector3.Cross(line, point - lineOrigin).sqrMagnitude;
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x000EC644 File Offset: 0x000EA844
		public static float DistanceToLine(Vector3 line, Vector3 lineOrigin, Vector3 point)
		{
			return Vector3.Cross(line, point - lineOrigin).magnitude;
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x000EC668 File Offset: 0x000EA868
		public static Vector3 ClosestPointOnLine(Vector3 vA, Vector3 vB, Vector3 vPoint)
		{
			Vector3 rhs = vPoint - vA;
			Vector3 normalized = (vB - vA).normalized;
			float num = Vector3.Distance(vA, vB);
			float num2 = Vector3.Dot(normalized, rhs);
			if (num2 <= 0f)
			{
				return vA;
			}
			if (num2 >= num)
			{
				return vB;
			}
			Vector3 b = normalized * num2;
			return vA + b;
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x000EC6C0 File Offset: 0x000EA8C0
		public static Vector2 ClosestPointOnLine(Vector2 vA, Vector2 vB, Vector2 vPoint)
		{
			Vector2 rhs = vPoint - vA;
			Vector2 normalized = (vB - vA).normalized;
			float num = Vector2.Distance(vA, vB);
			float num2 = Vector2.Dot(normalized, rhs);
			if (num2 <= 0f)
			{
				return vA;
			}
			if (num2 >= num)
			{
				return vB;
			}
			Vector2 b = normalized * num2;
			return vA + b;
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x000EC718 File Offset: 0x000EA918
		public static Vector2 GetClosestPointOnLineSegment(Vector2 A, Vector2 B, Vector2 P)
		{
			Vector2 lhs = P - A;
			Vector2 vector = B - A;
			float sqrMagnitude = vector.sqrMagnitude;
			float num = Vector2.Dot(lhs, vector) / sqrMagnitude;
			if (num < 0f)
			{
				return A;
			}
			if (num > 1f)
			{
				return B;
			}
			return A + vector * num;
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x000EC768 File Offset: 0x000EA968
		public static float CalcShortestRot(float from, float to)
		{
			if (from < 0f)
			{
				from += 360f;
			}
			if (to < 0f)
			{
				to += 360f;
			}
			if (from == to || (from == 0f && to == 360f) || (from == 360f && to == 0f))
			{
				return 0f;
			}
			float num = 360f - from + to;
			float num2 = from - to;
			if (from < to)
			{
				if (to > 0f)
				{
					num = to - from;
					num2 = 360f - to + from;
				}
				else
				{
					num = 360f - to + from;
					num2 = to - from;
				}
			}
			if (num > num2)
			{
				return num2 * -1f;
			}
			return num;
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x0001BF81 File Offset: 0x0001A181
		public static bool CalcShortestRotDirection(float from, float to)
		{
			return ZPMath.CalcShortestRot(from, to) >= 0f;
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x0001BF94 File Offset: 0x0001A194
		public static float ClampRotation(float r)
		{
			if (r > 360f)
			{
				return r - (float)((int)r / 360) * 360f;
			}
			if (r < 0f)
			{
				return r + (float)((int)r / -360 + 1) * 360f;
			}
			return r;
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x000EC808 File Offset: 0x000EAA08
		public static Vector3 RandomPointInUnitSphere(System.Random rand)
		{
			int i = 0;
			int num = 1000;
			while (i < num)
			{
				float num2 = ZPMath.RandomFloat(rand, -1f, 1f);
				float num3 = ZPMath.RandomFloat(rand, -1f, 1f);
				float num4 = ZPMath.RandomFloat(rand, -1f, 1f);
				if (num2 * num2 + num3 * num3 + num4 * num4 < 1f)
				{
					return new Vector3(num2, num3, num4);
				}
			}
			return Vector3.zero;
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x000EC87C File Offset: 0x000EAA7C
		public static float RandomFloat(System.Random r, float min, float max)
		{
			float num = max - min;
			return (float)((double)min + r.NextDouble() * (double)num);
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x0001BFCD File Offset: 0x0001A1CD
		public static Vector3 RandomVec3(System.Random r, Vector3 min, Vector3 max)
		{
			return new Vector3(ZPMath.RandomFloat(r, min.x, max.x), ZPMath.RandomFloat(r, min.y, max.y), ZPMath.RandomFloat(r, min.z, max.z));
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x0001C00A File Offset: 0x0001A20A
		public static Vector3 RandomVec3(System.Random r, float min, float max)
		{
			return new Vector3(ZPMath.RandomFloat(r, min, max), ZPMath.RandomFloat(r, min, max), ZPMath.RandomFloat(r, min, max));
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x000EC89C File Offset: 0x000EAA9C
		public static Vector3 RandomTrianglePoint(Vector3 p1, Vector3 p2, Vector3 p3, System.Random r)
		{
			double num = r.NextDouble();
			double num2 = r.NextDouble();
			double num3 = (double)p1.x;
			double num4 = (double)p2.x;
			double num5 = (double)p3.x;
			double num6 = (double)p1.y;
			double num7 = (double)p2.y;
			double num8 = (double)p3.y;
			double num9 = (double)p1.z;
			double num10 = (double)p2.z;
			double num11 = (double)p3.z;
			if (num + num2 > 1.0)
			{
				num = 1.0 - num;
				num2 = 1.0 - num2;
			}
			double num12 = 1.0 - num - num2;
			double num13 = num * num3 + num2 * num4 + num12 * num5;
			double num14 = num * num6 + num2 * num7 + num12 * num8;
			double num15 = num * num9 + num2 * num10 + num12 * num11;
			return new Vector3((float)num13, (float)num14, (float)num15);
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x0001C029 File Offset: 0x0001A229
		public static float GetVolume(SphereCollider s)
		{
			return 4.1887903f * (s.radius * s.radius * s.radius);
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x0001C045 File Offset: 0x0001A245
		public static float GetVolume(CapsuleCollider c)
		{
			return 3.1415927f * (c.radius * c.radius) * (1.3333334f * c.radius + c.height);
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x0001C06E File Offset: 0x0001A26E
		public static float GetVolume(BoxCollider b)
		{
			return b.size.x * b.size.y * b.size.z;
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x000EC988 File Offset: 0x000EAB88
		public static float GetVolume(Collider c)
		{
			Type type = c.GetType();
			if (type == typeof(SphereCollider))
			{
				return ZPMath.GetVolume((SphereCollider)c);
			}
			if (type == typeof(CapsuleCollider))
			{
				return ZPMath.GetVolume((CapsuleCollider)c);
			}
			if (type == typeof(BoxCollider))
			{
				return ZPMath.GetVolume((BoxCollider)c);
			}
			return 0f;
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x0001C093 File Offset: 0x0001A293
		public static Vector3 CirclePosition(float radius, float angle)
		{
			return ZPMath.CirclePosition(Vector3.zero, radius, angle);
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x000EC9FC File Offset: 0x000EABFC
		public static Vector3 CirclePosition(Vector3 center, float radius, float angle)
		{
			float f = angle * 3.1415927f / 180f;
			return center + new Vector3(radius * Mathf.Cos(f), 0f, radius * Mathf.Sin(f));
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x000ECA38 File Offset: 0x000EAC38
		public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
		{
			Vector3 lhs = linePoint2 - linePoint1;
			Vector3 rhs = Vector3.Cross(lineVec1, lineVec2);
			Vector3 lhs2 = Vector3.Cross(lhs, lineVec2);
			if (Mathf.Abs(Vector3.Dot(lhs, rhs)) < 0.0001f && rhs.sqrMagnitude > 0.0001f)
			{
				float d = Vector3.Dot(lhs2, rhs) / rhs.sqrMagnitude;
				intersection = linePoint1 + lineVec1 * d;
				return true;
			}
			intersection = Vector3.zero;
			return false;
		}
	}
}
