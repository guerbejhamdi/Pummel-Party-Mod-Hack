using System;
using System.Collections.Generic;
using UI.CustomSelectable;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000558 RID: 1368
public class UIMinigameSelection : MonoBehaviour
{
	// Token: 0x060023FD RID: 9213 RVA: 0x000D91D4 File Offset: 0x000D73D4
	private void Start()
	{
		this.m_minigameBtnPfb = Resources.Load<GameObject>("Prefabs/UI/MinigameBtn");
		this.m_minigames = GameManager.GetMinigameList();
		this.m_numPages = (this.m_minigames.Count + this.m_minigamesPerPage - 1) / this.m_minigamesPerPage;
		for (int i = 0; i < this.m_minigamesPerPage; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_minigameBtnPfb);
			gameObject.transform.SetParent(this.m_minigameListContainer, false);
			UIMinigameButton component = gameObject.GetComponent<UIMinigameButton>();
			this.m_buttons.Add(component);
		}
		this.m_prevBtn.rightSelectable = this.m_nextBtn;
		this.m_nextBtn.leftSelectable = this.m_prevBtn;
		this.SetPage(0);
	}

	// Token: 0x060023FE RID: 9214 RVA: 0x000D9288 File Offset: 0x000D7488
	private void SetPage(int page)
	{
		if (page < 0 || page >= this.m_numPages)
		{
			return;
		}
		this.m_curPage = page;
		int num = page * this.m_minigamesPerPage;
		for (int i = 0; i < this.m_minigamesPerPage; i++)
		{
			if (num + i < this.m_minigames.Count)
			{
				this.m_buttons[i].gameObject.SetActive(true);
				this.m_buttons[i].SetMinigame(this.m_minigames[num + i]);
			}
			else
			{
				this.m_buttons[i].gameObject.SetActive(false);
			}
		}
		this.m_pageTxt.text = (this.m_curPage + 1).ToString() + "/" + this.m_numPages.ToString();
	}

	// Token: 0x060023FF RID: 9215 RVA: 0x00019E85 File Offset: 0x00018085
	public void NextPage()
	{
		if (this.m_curPage >= this.m_numPages - 1)
		{
			return;
		}
		this.SetPage(this.m_curPage + 1);
	}

	// Token: 0x06002400 RID: 9216 RVA: 0x00019EA6 File Offset: 0x000180A6
	public void PreviousPage()
	{
		if (this.m_curPage <= 0)
		{
			return;
		}
		this.SetPage(this.m_curPage - 1);
	}

	// Token: 0x0400270F RID: 9999
	[SerializeField]
	protected int m_minigamesPerPage = 9;

	// Token: 0x04002710 RID: 10000
	[SerializeField]
	protected Transform m_minigameListContainer;

	// Token: 0x04002711 RID: 10001
	[SerializeField]
	protected CustomSelectableButton m_prevBtn;

	// Token: 0x04002712 RID: 10002
	[SerializeField]
	protected CustomSelectableButton m_nextBtn;

	// Token: 0x04002713 RID: 10003
	[SerializeField]
	protected Text m_pageTxt;

	// Token: 0x04002714 RID: 10004
	private GameObject m_minigameBtnPfb;

	// Token: 0x04002715 RID: 10005
	private List<UIMinigameButton> m_buttons = new List<UIMinigameButton>();

	// Token: 0x04002716 RID: 10006
	private List<MinigameDefinition> m_minigames;

	// Token: 0x04002717 RID: 10007
	private int m_numPages;

	// Token: 0x04002718 RID: 10008
	private int m_curPage;
}
