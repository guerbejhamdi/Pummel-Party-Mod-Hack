using System;
using UnityEngine;

// Token: 0x02000210 RID: 528
[Serializable]
public class RespawnPoint
{
	// Token: 0x06000F90 RID: 3984 RVA: 0x0000D527 File Offset: 0x0000B727
	public RespawnPoint(int triangle_index1, int triangle_index2, Vector3 spawn_point, float spawn_y_rotation)
	{
		this.triangle_index1 = triangle_index1;
		this.triangle_index2 = triangle_index2;
		this.spawn_point = spawn_point;
		this.spawn_y_rotation = spawn_y_rotation;
	}

	// Token: 0x04000F9C RID: 3996
	public int triangle_index1;

	// Token: 0x04000F9D RID: 3997
	public int triangle_index2;

	// Token: 0x04000F9E RID: 3998
	public Vector3 spawn_point;

	// Token: 0x04000F9F RID: 3999
	public float spawn_y_rotation;
}
