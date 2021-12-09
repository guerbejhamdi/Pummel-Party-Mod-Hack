using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000549 RID: 1353
public class UIItemSelection : MonoBehaviour
{
	// Token: 0x060023C7 RID: 9159 RVA: 0x000D822C File Offset: 0x000D642C
	public void Start()
	{
		this.m_itemBtnPfb = Resources.Load<GameObject>("Prefabs/UI/ItemBtn");
		this.m_items = GameManager.ItemList.items;
		this.m_numPages = (this.m_items.Length + this.m_itemsPerPage - 1) / this.m_itemsPerPage;
		for (int i = 0; i < this.m_itemsPerPage; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_itemBtnPfb);
			gameObject.transform.SetParent(this.m_itemsContainer, false);
			UIItemButton component = gameObject.GetComponent<UIItemButton>();
			this.m_buttons.Add(component);
		}
		this.SetPage(0);
	}

	// Token: 0x060023C8 RID: 9160 RVA: 0x000D82C0 File Offset: 0x000D64C0
	private void SetPage(int page)
	{
		if (page < 0 || page >= this.m_numPages)
		{
			return;
		}
		this.m_curPage = page;
		int num = page * this.m_itemsPerPage;
		for (int i = 0; i < this.m_itemsPerPage; i++)
		{
			if (num + i < this.m_items.Length)
			{
				this.m_buttons[i].gameObject.SetActive(true);
				this.m_buttons[i].SetItem(this.m_items[num + i]);
			}
			else
			{
				this.m_buttons[i].gameObject.SetActive(false);
			}
		}
		this.m_pageTxt.text = (this.m_curPage + 1).ToString() + "/" + this.m_numPages.ToString();
	}

	// Token: 0x060023C9 RID: 9161 RVA: 0x00019CC3 File Offset: 0x00017EC3
	public void NextPage()
	{
		if (this.m_curPage >= this.m_numPages - 1)
		{
			return;
		}
		this.SetPage(this.m_curPage + 1);
	}

	// Token: 0x060023CA RID: 9162 RVA: 0x00019CE4 File Offset: 0x00017EE4
	public void PreviousPage()
	{
		if (this.m_curPage <= 0)
		{
			return;
		}
		this.SetPage(this.m_curPage - 1);
	}

	// Token: 0x040026B1 RID: 9905
	[SerializeField]
	protected int m_itemsPerPage = 6;

	// Token: 0x040026B2 RID: 9906
	[SerializeField]
	protected Transform m_itemsContainer;

	// Token: 0x040026B3 RID: 9907
	[SerializeField]
	protected Text m_pageTxt;

	// Token: 0x040026B4 RID: 9908
	private List<UIItemButton> m_buttons = new List<UIItemButton>();

	// Token: 0x040026B5 RID: 9909
	private ItemDetails[] m_items;

	// Token: 0x040026B6 RID: 9910
	private GameObject m_itemBtnPfb;

	// Token: 0x040026B7 RID: 9911
	private int m_numPages;

	// Token: 0x040026B8 RID: 9912
	private int m_curPage;
}
