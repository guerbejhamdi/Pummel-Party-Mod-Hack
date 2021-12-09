using System;
using UnityEngine;

// Token: 0x0200018B RID: 395
public class BurglarVehicleSpawn : MonoBehaviour
{
	// Token: 0x17000101 RID: 257
	// (get) Token: 0x06000B42 RID: 2882 RVA: 0x0000B2D3 File Offset: 0x000094D3
	public int SpawnIndex
	{
		get
		{
			return this.m_spawnIndex;
		}
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x06000B43 RID: 2883 RVA: 0x0000B2DB File Offset: 0x000094DB
	// (set) Token: 0x06000B44 RID: 2884 RVA: 0x0000B2E3 File Offset: 0x000094E3
	public float NextSpawnTime { get; set; }

	// Token: 0x04000A5E RID: 2654
	[SerializeField]
	private int m_spawnIndex;
}
