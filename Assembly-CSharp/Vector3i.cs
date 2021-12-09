using System;
using UnityEngine;

// Token: 0x02000470 RID: 1136
public struct Vector3i
{
	// Token: 0x06001EA6 RID: 7846 RVA: 0x00016936 File Offset: 0x00014B36
	public Vector3i(int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	// Token: 0x06001EA7 RID: 7847 RVA: 0x0001694D File Offset: 0x00014B4D
	public Vector3i(Vector3i p1, Vector3i p2)
	{
		this.x = p1.x + p2.x;
		this.y = p1.y + p2.y;
		this.z = p1.z + p2.z;
	}

	// Token: 0x06001EA8 RID: 7848 RVA: 0x00016988 File Offset: 0x00014B88
	public Vector3i(Vector3 p)
	{
		this.x = Convert.ToInt32(p.x);
		this.y = Convert.ToInt32(p.y);
		this.z = Convert.ToInt32(p.z);
	}

	// Token: 0x06001EA9 RID: 7849 RVA: 0x000169BD File Offset: 0x00014BBD
	public Vector3i(Vector3i p)
	{
		this.x = p.x;
		this.y = p.y;
		this.z = p.z;
	}

	// Token: 0x06001EAA RID: 7850 RVA: 0x00016936 File Offset: 0x00014B36
	public void Set(int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	// Token: 0x06001EAB RID: 7851 RVA: 0x000169E3 File Offset: 0x00014BE3
	public static Vector3i Random(int minX, int maxX, int minY, int maxY, int minZ, int maxZ)
	{
		return new Vector3((float)UnityEngine.Random.Range(minX, maxX), (float)UnityEngine.Random.Range(minY, maxY), (float)UnityEngine.Random.Range(minZ, maxZ));
	}

	// Token: 0x06001EAC RID: 7852 RVA: 0x00016A09 File Offset: 0x00014C09
	public static float Distance(Vector3i p1, Vector3i p2)
	{
		return Mathf.Sqrt((float)Vector3i.SqrDistance(p1, p2));
	}

	// Token: 0x06001EAD RID: 7853 RVA: 0x000C6124 File Offset: 0x000C4324
	public static int SqrDistance(Vector3i p1, Vector3i p2)
	{
		int num = p1.x - p2.x;
		int num2 = p1.y - p2.y;
		int num3 = p1.z - p2.z;
		return num * num + num2 * num2 + num3 * num3;
	}

	// Token: 0x06001EAE RID: 7854 RVA: 0x000C6164 File Offset: 0x000C4364
	public override bool Equals(object o)
	{
		Vector3i vector3i = (Vector3i)o;
		return this.x == vector3i.x && this.y == vector3i.y && this.z == vector3i.z;
	}

	// Token: 0x06001EAF RID: 7855 RVA: 0x00016A18 File Offset: 0x00014C18
	public bool Equals(Vector3i v)
	{
		return this.x == v.x && this.y == v.y && this.z == v.z;
	}

	// Token: 0x06001EB0 RID: 7856 RVA: 0x000C61A4 File Offset: 0x000C43A4
	public override int GetHashCode()
	{
		return string.Concat(new string[]
		{
			this.x.ToString(),
			" ",
			this.y.ToString(),
			" ",
			this.z.ToString()
		}).GetHashCode();
	}

	// Token: 0x06001EB1 RID: 7857 RVA: 0x000C61FC File Offset: 0x000C43FC
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			this.x.ToString(),
			", ",
			this.y.ToString(),
			", ",
			this.z.ToString()
		});
	}

	// Token: 0x06001EB2 RID: 7858 RVA: 0x00016A18 File Offset: 0x00014C18
	public static bool operator ==(Vector3i p1, Vector3i p2)
	{
		return p1.x == p2.x && p1.y == p2.y && p1.z == p2.z;
	}

	// Token: 0x06001EB3 RID: 7859 RVA: 0x00016A46 File Offset: 0x00014C46
	public static bool operator !=(Vector3i p1, Vector3i p2)
	{
		return p1.x != p2.x || p1.y != p2.y || p1.z != p2.z;
	}

	// Token: 0x06001EB4 RID: 7860 RVA: 0x00016A77 File Offset: 0x00014C77
	public static Vector3i operator +(Vector3i p1, Vector3i p2)
	{
		return new Vector3i(p1.x + p2.x, p1.y + p2.y, p1.z + p2.z);
	}

	// Token: 0x06001EB5 RID: 7861 RVA: 0x00016AA5 File Offset: 0x00014CA5
	public static Vector3i operator -(Vector3i p1, Vector3i p2)
	{
		return new Vector3i(p1.x - p2.x, p1.y - p2.y, p1.z - p2.z);
	}

	// Token: 0x06001EB6 RID: 7862 RVA: 0x00016AD3 File Offset: 0x00014CD3
	public static implicit operator Vector3(Vector3i v)
	{
		return new Vector3((float)v.x, (float)v.y, (float)v.z);
	}

	// Token: 0x06001EB7 RID: 7863 RVA: 0x00016AEF File Offset: 0x00014CEF
	public static implicit operator Vector3i(Vector3 v)
	{
		return new Vector3i(v);
	}

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x06001EB8 RID: 7864 RVA: 0x00016AF7 File Offset: 0x00014CF7
	public static Vector3i up
	{
		get
		{
			return new Vector3i(0, 1, 0);
		}
	}

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x06001EB9 RID: 7865 RVA: 0x00016B01 File Offset: 0x00014D01
	public static Vector3i down
	{
		get
		{
			return new Vector3i(0, -1, 0);
		}
	}

	// Token: 0x1700039C RID: 924
	// (get) Token: 0x06001EBA RID: 7866 RVA: 0x00016B0B File Offset: 0x00014D0B
	public static Vector3i left
	{
		get
		{
			return new Vector3i(1, 0, 0);
		}
	}

	// Token: 0x1700039D RID: 925
	// (get) Token: 0x06001EBB RID: 7867 RVA: 0x00016B15 File Offset: 0x00014D15
	public static Vector3i right
	{
		get
		{
			return new Vector3i(-1, 0, 0);
		}
	}

	// Token: 0x1700039E RID: 926
	// (get) Token: 0x06001EBC RID: 7868 RVA: 0x00016B1F File Offset: 0x00014D1F
	public static Vector3i forward
	{
		get
		{
			return new Vector3i(0, 0, 1);
		}
	}

	// Token: 0x1700039F RID: 927
	// (get) Token: 0x06001EBD RID: 7869 RVA: 0x00016B29 File Offset: 0x00014D29
	public static Vector3i back
	{
		get
		{
			return new Vector3i(0, 0, -1);
		}
	}

	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x06001EBE RID: 7870 RVA: 0x00016B33 File Offset: 0x00014D33
	public static Vector3i one
	{
		get
		{
			return new Vector3i(1, 1, 1);
		}
	}

	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x06001EBF RID: 7871 RVA: 0x00016B3D File Offset: 0x00014D3D
	public static Vector3i zero
	{
		get
		{
			return new Vector3i(0, 0, 0);
		}
	}

	// Token: 0x040021C1 RID: 8641
	public int x;

	// Token: 0x040021C2 RID: 8642
	public int y;

	// Token: 0x040021C3 RID: 8643
	public int z;
}
