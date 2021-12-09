using System;
using UnityEngine;

// Token: 0x020001D2 RID: 466
public class MemoryMenuItemSpawnPoint : MonoBehaviour
{
	// Token: 0x1700012F RID: 303
	// (get) Token: 0x06000D7E RID: 3454 RVA: 0x0000C44A File Offset: 0x0000A64A
	public int Index
	{
		get
		{
			return this.m_spawnIndex;
		}
	}

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06000D7F RID: 3455 RVA: 0x0000C452 File Offset: 0x0000A652
	// (set) Token: 0x06000D80 RID: 3456 RVA: 0x0000C45A File Offset: 0x0000A65A
	public short ItemID { get; set; }

	// Token: 0x04000CD3 RID: 3283
	[SerializeField]
	private int m_spawnIndex;
}
