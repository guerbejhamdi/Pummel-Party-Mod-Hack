using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001D6 RID: 470
public class MemoryMenuUI : MonoBehaviour
{
	// Token: 0x06000DA3 RID: 3491 RVA: 0x0000C50F File Offset: 0x0000A70F
	public void ShowMemoryPanel(List<Sprite> icons)
	{
		base.StartCoroutine(this.ShowMemoryPanelRoutine(icons));
	}

	// Token: 0x06000DA4 RID: 3492 RVA: 0x0000C51F File Offset: 0x0000A71F
	private IEnumerator ShowMemoryPanelRoutine(List<Sprite> icons)
	{
		for (int i = 0; i < this.m_iconImages.Length; i++)
		{
			if (i < icons.Count)
			{
				this.m_iconImages[i].gameObject.SetActive(true);
				this.m_iconImages[i].sprite = icons[i];
			}
			else
			{
				this.m_iconImages[i].gameObject.SetActive(false);
			}
		}
		this.m_memorisePanel.SetActive(true);
		this.m_memorisePanel.transform.localScale = Vector3.zero;
		yield return null;
		LeanTween.scale(this.m_memorisePanel, Vector3.one, 0.5f).setEaseOutBounce();
		yield return new WaitForSeconds(4f);
		LeanTween.scale(this.m_memorisePanel, Vector3.zero, 0.5f).setEaseInBack();
		yield return new WaitForSeconds(0.5f);
		this.m_memorisePanel.SetActive(false);
		yield break;
	}

	// Token: 0x04000D09 RID: 3337
	[SerializeField]
	private Image[] m_iconImages;

	// Token: 0x04000D0A RID: 3338
	[SerializeField]
	private GameObject m_memorisePanel;

	// Token: 0x04000D0B RID: 3339
	[SerializeField]
	private List<Sprite> m_sprites;
}
