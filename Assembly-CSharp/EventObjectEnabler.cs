using System;
using UnityEngine;

// Token: 0x0200009E RID: 158
public class EventObjectEnabler : MonoBehaviour
{
	// Token: 0x06000359 RID: 857 RVA: 0x00005C25 File Offset: 0x00003E25
	private void Start()
	{
		if (this.m_theme == GameManager.GetCurrentEventTheme())
		{
			if (this.m_targetObj != null)
			{
				this.m_targetObj.SetActive(true);
				return;
			}
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x04000369 RID: 873
	[SerializeField]
	public GameEventTheme m_theme;

	// Token: 0x0400036A RID: 874
	[SerializeField]
	public GameObject m_targetObj;
}
