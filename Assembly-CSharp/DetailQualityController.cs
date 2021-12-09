using System;
using UnityEngine;

// Token: 0x02000462 RID: 1122
public class DetailQualityController : MonoBehaviour
{
	// Token: 0x06001E8B RID: 7819 RVA: 0x000C5B6C File Offset: 0x000C3D6C
	public void Awake()
	{
		foreach (DetailGroup detailGroup in this.m_groups)
		{
			bool active = (GameManager.GetLocalPlayerCount() <= detailGroup.maxPlayers || detailGroup.maxPlayers == 0) && Settings.DetailQuality >= detailGroup.minDetailQuality;
			GameObject[] objects = detailGroup.objects;
			for (int j = 0; j < objects.Length; j++)
			{
				objects[j].SetActive(active);
			}
		}
	}

	// Token: 0x0400218D RID: 8589
	[SerializeField]
	private DetailGroup[] m_groups;
}
