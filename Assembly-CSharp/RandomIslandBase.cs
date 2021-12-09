using System;
using UnityEngine;

// Token: 0x0200030D RID: 781
public class RandomIslandBase : MonoBehaviour
{
	// Token: 0x0600159B RID: 5531 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x0600159C RID: 5532 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x0600159D RID: 5533 RVA: 0x000105F7 File Offset: 0x0000E7F7
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(base.transform.position, new Vector3(14f, 0.1f, 14f));
	}

	// Token: 0x0400169D RID: 5789
	private const float ISLAND_SIZE = 14f;
}
