using System;
using UnityEngine;

// Token: 0x02000312 RID: 786
public class RaycastAlign : MonoBehaviour
{
	// Token: 0x060015A7 RID: 5543 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x060015A8 RID: 5544 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x040016A9 RID: 5801
	public Vector3 offset = Vector3.zero;

	// Token: 0x040016AA RID: 5802
	public LayerMask mask;

	// Token: 0x040016AB RID: 5803
	public float maxDistance;

	// Token: 0x040016AC RID: 5804
	private Vector3 startPos;
}
