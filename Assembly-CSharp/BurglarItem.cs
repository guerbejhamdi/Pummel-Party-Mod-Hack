using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000182 RID: 386
public class BurglarItem : MonoBehaviour
{
	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x06000AFE RID: 2814 RVA: 0x0000B115 File Offset: 0x00009315
	// (set) Token: 0x06000AFF RID: 2815 RVA: 0x0000B11D File Offset: 0x0000931D
	public int ItemID { get; set; }

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x06000B00 RID: 2816 RVA: 0x0000B126 File Offset: 0x00009326
	public int PointValue
	{
		get
		{
			return this.m_pointValue;
		}
	}

	// Token: 0x06000B01 RID: 2817 RVA: 0x0005F7E8 File Offset: 0x0005D9E8
	private void Awake()
	{
		int num = UnityEngine.Random.Range(0, this.m_graphics.Length);
		this.m_graphics[num].SetActive(true);
		this.m_graphics[num].transform.localRotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
	}

	// Token: 0x06000B02 RID: 2818 RVA: 0x0000B12E File Offset: 0x0000932E
	private IEnumerator Spawn()
	{
		yield return null;
		yield break;
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x0000B136 File Offset: 0x00009336
	private IEnumerator Despawn()
	{
		yield return null;
		yield break;
	}

	// Token: 0x04000A1F RID: 2591
	[SerializeField]
	private GameObject[] m_graphics;

	// Token: 0x04000A20 RID: 2592
	[SerializeField]
	private int m_pointValue = 1;
}
