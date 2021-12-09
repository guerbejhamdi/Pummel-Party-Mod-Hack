using System;
using UnityEngine;

// Token: 0x02000581 RID: 1409
[RequireComponent(typeof(VoxelGrid))]
public class VoxelFillerTool : MonoBehaviour
{
	// Token: 0x0400283B RID: 10299
	public FillType fill_type = FillType.FillAllButEdges;

	// Token: 0x0400283C RID: 10300
	public float fill_y = 0.5f;

	// Token: 0x0400283D RID: 10301
	public float perlin_noise_fill_scale = 10f;

	// Token: 0x0400283E RID: 10302
	public float ringRadius = 10f;

	// Token: 0x0400283F RID: 10303
	public float ringRadius2 = 2f;
}
