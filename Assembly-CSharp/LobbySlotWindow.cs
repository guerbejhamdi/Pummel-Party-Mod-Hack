using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200050F RID: 1295
public class LobbySlotWindow : MonoBehaviour
{
	// Token: 0x060021CF RID: 8655 RVA: 0x000D08B0 File Offset: 0x000CEAB0
	public void Awake()
	{
		this.anim = base.GetComponent<Animator>();
		this.ui_elements = new List<Selectable>();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Selectable component = base.transform.GetChild(i).gameObject.GetComponent<Selectable>();
			if (component != null)
			{
				this.ui_elements.Add(component);
			}
		}
	}

	// Token: 0x060021D0 RID: 8656 RVA: 0x00018842 File Offset: 0x00016A42
	public void ShowWindow()
	{
		this.anim.SetTrigger("Show");
	}

	// Token: 0x060021D1 RID: 8657 RVA: 0x00018854 File Offset: 0x00016A54
	public void HideWindow()
	{
		this.anim.SetTrigger("Hide");
	}

	// Token: 0x040024C0 RID: 9408
	private Animator anim;

	// Token: 0x040024C1 RID: 9409
	private List<Selectable> ui_elements;
}
