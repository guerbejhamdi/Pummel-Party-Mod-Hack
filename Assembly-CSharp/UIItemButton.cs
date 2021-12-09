using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000548 RID: 1352
public class UIItemButton : MonoBehaviour
{
	// Token: 0x060023C4 RID: 9156 RVA: 0x000D8128 File Offset: 0x000D6328
	public void OnToggle()
	{
		if ((this.m_active && GameManager.GetNumberOfActiveItems() <= 1) || this.m_item == null)
		{
			return;
		}
		this.m_active = !this.m_active;
		this.m_toggle.isOn = this.m_active;
		this.m_item.SetIsActive(this.m_active);
	}

	// Token: 0x060023C5 RID: 9157 RVA: 0x000D8188 File Offset: 0x000D6388
	public void SetItem(ItemDetails item)
	{
		this.m_item = item;
		this.m_nameTxt.text = LocalizationManager.GetTranslation(this.m_item.itemNameToken, true, 0, true, false, null, null, true);
		this.m_descTxt.text = LocalizationManager.GetTranslation(this.m_item.descriptionToken, true, 0, true, false, null, null, true);
		if (this.m_item.icon != null)
		{
			this.m_iconImg.sprite = this.m_item.icon;
		}
		this.m_active = this.m_item.GetIsActive();
		this.m_toggle.isOn = this.m_active;
	}

	// Token: 0x040026AB RID: 9899
	[SerializeField]
	protected Text m_nameTxt;

	// Token: 0x040026AC RID: 9900
	[SerializeField]
	protected Text m_descTxt;

	// Token: 0x040026AD RID: 9901
	[SerializeField]
	protected Image m_iconImg;

	// Token: 0x040026AE RID: 9902
	[SerializeField]
	protected Toggle m_toggle;

	// Token: 0x040026AF RID: 9903
	private bool m_active;

	// Token: 0x040026B0 RID: 9904
	private ItemDetails m_item;
}
