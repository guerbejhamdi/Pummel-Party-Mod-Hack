using System;
using UnityEngine;

// Token: 0x02000168 RID: 360
public class BoundingBlocksTile : MonoBehaviour
{
	// Token: 0x04000931 RID: 2353
	public BoxCollider trigger;

	// Token: 0x04000932 RID: 2354
	public BoxCollider blocker;

	// Token: 0x04000933 RID: 2355
	public MeshRenderer glowVisual;

	// Token: 0x04000934 RID: 2356
	public MeshRenderer visual;

	// Token: 0x04000935 RID: 2357
	public GameObject disabledVisual;

	// Token: 0x04000936 RID: 2358
	public int id;
}
