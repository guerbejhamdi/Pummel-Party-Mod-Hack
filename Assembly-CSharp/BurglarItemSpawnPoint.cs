using System;
using UnityEngine;

// Token: 0x02000186 RID: 390
public class BurglarItemSpawnPoint : MonoBehaviour
{
	// Token: 0x170000FD RID: 253
	// (get) Token: 0x06000B1C RID: 2844 RVA: 0x0000B1EE File Offset: 0x000093EE
	public int Index
	{
		get
		{
			return this.m_spawnIndex;
		}
	}

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x06000B1D RID: 2845 RVA: 0x0000B1F6 File Offset: 0x000093F6
	// (set) Token: 0x06000B1E RID: 2846 RVA: 0x0000B1FE File Offset: 0x000093FE
	public short ItemID { get; set; }

	// Token: 0x04000A2B RID: 2603
	[SerializeField]
	private int m_spawnIndex;
}
