using System;
using UnityEngine;

// Token: 0x02000471 RID: 1137
public static class Vector3Extensions
{
	// Token: 0x06001EC0 RID: 7872 RVA: 0x000C6250 File Offset: 0x000C4450
	public static Vector3 ZPMoveTowards(this Vector3 from, Vector3 to, float maxDelta)
	{
		if (from.Equals(to))
		{
			return from;
		}
		Vector3 vector = to - from;
		if (vector.sqrMagnitude < maxDelta * maxDelta)
		{
			return to;
		}
		return from + vector.normalized * maxDelta;
	}

	// Token: 0x06001EC1 RID: 7873 RVA: 0x00016B47 File Offset: 0x00014D47
	public static Vector3 Flattened(this Vector3 vector)
	{
		return new Vector3(vector.x, 0f, vector.z);
	}

	// Token: 0x06001EC2 RID: 7874 RVA: 0x00016B5F File Offset: 0x00014D5F
	public static float DistanceFlat(this Vector3 origin, Vector3 destination)
	{
		return Vector3.Distance(origin.Flattened(), destination.Flattened());
	}
}
