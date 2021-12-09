using System;
using UnityEngine;

// Token: 0x02000149 RID: 329
public class BarnBrawlSpawnPoint : MonoBehaviour
{
	// Token: 0x06000976 RID: 2422 RVA: 0x0005537C File Offset: 0x0005357C
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(base.transform.position + new Vector3(0f, -1.1f, 0f), new Vector3(1f, 0.05f, 1f));
	}
}
