using System;
using UnityEngine;

// Token: 0x02000205 RID: 517
public class ObjectSpawnPoint : MonoBehaviour
{
	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06000F38 RID: 3896 RVA: 0x0000D2C4 File Offset: 0x0000B4C4
	public int SpawnIndex
	{
		get
		{
			return this.m_spawnIndex;
		}
	}

	// Token: 0x04000F05 RID: 3845
	[SerializeField]
	private int m_spawnIndex;
}
