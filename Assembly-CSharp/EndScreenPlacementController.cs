using System;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class EndScreenPlacementController : MonoBehaviour
{
	// Token: 0x17000030 RID: 48
	// (get) Token: 0x06000166 RID: 358 RVA: 0x000047A0 File Offset: 0x000029A0
	public Transform[] SpawnPoints
	{
		get
		{
			return this.m_spawnPoints;
		}
	}

	// Token: 0x06000167 RID: 359 RVA: 0x000316C4 File Offset: 0x0002F8C4
	public void Awake()
	{
		for (int i = 0; i < this.m_placementUIs.Length; i++)
		{
			this.m_placementUIs[i].SetActive(false);
		}
	}

	// Token: 0x06000168 RID: 360 RVA: 0x000047A8 File Offset: 0x000029A8
	public void SetPlacementActive(int index, bool active)
	{
		if (index < 0 || index >= this.m_placementObjects.Length || index >= this.m_placementUIs.Length)
		{
			return;
		}
		this.m_placementUIs[index].SetActive(active);
	}

	// Token: 0x040001C1 RID: 449
	[SerializeField]
	private GameObject[] m_placementObjects;

	// Token: 0x040001C2 RID: 450
	[SerializeField]
	private GameObject[] m_placementUIs;

	// Token: 0x040001C3 RID: 451
	[SerializeField]
	private Transform[] m_spawnPoints;
}
