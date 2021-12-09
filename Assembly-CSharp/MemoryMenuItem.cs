using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001CE RID: 462
public class MemoryMenuItem : MonoBehaviour
{
	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000D5D RID: 3421 RVA: 0x0000C367 File Offset: 0x0000A567
	// (set) Token: 0x06000D5E RID: 3422 RVA: 0x0000C36F File Offset: 0x0000A56F
	public int ItemID { get; set; }

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x06000D5F RID: 3423 RVA: 0x0000C378 File Offset: 0x0000A578
	public int ItemTypeID
	{
		get
		{
			return this.m_itemTypeID;
		}
	}

	// Token: 0x06000D60 RID: 3424 RVA: 0x0006D598 File Offset: 0x0006B798
	private void Awake()
	{
		int num = UnityEngine.Random.Range(0, this.m_graphics.Length);
		this.m_graphics[num].SetActive(true);
		this.m_graphics[num].transform.localRotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
	}

	// Token: 0x06000D61 RID: 3425 RVA: 0x0000C380 File Offset: 0x0000A580
	private IEnumerator Spawn()
	{
		yield return null;
		yield break;
	}

	// Token: 0x06000D62 RID: 3426 RVA: 0x0000C388 File Offset: 0x0000A588
	private IEnumerator Despawn()
	{
		yield return null;
		yield break;
	}

	// Token: 0x04000CC6 RID: 3270
	[SerializeField]
	private GameObject[] m_graphics;

	// Token: 0x04000CC7 RID: 3271
	[SerializeField]
	private int m_itemTypeID = 1;
}
