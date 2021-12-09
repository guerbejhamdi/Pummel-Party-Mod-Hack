using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200029F RID: 671
public class ModifierUIPanel : MonoBehaviour
{
	// Token: 0x060013BE RID: 5054 RVA: 0x0000FA64 File Offset: 0x0000DC64
	public void SetIcon(Sprite sprite)
	{
		this.m_icon.sprite = sprite;
	}

	// Token: 0x060013BF RID: 5055 RVA: 0x0000FA72 File Offset: 0x0000DC72
	public void SetDescription(string text)
	{
		this.m_descriptionText.text = text;
	}

	// Token: 0x060013C0 RID: 5056 RVA: 0x0000FA80 File Offset: 0x0000DC80
	public void SetColor(Color c)
	{
		this.m_iconOutline.color = c;
		this.m_descriptionOutline.color = c;
	}

	// Token: 0x0400150B RID: 5387
	[Header("References")]
	[SerializeField]
	private Text m_descriptionText;

	// Token: 0x0400150C RID: 5388
	[SerializeField]
	private Image m_icon;

	// Token: 0x0400150D RID: 5389
	[SerializeField]
	private Image m_iconOutline;

	// Token: 0x0400150E RID: 5390
	[SerializeField]
	private Image m_descriptionOutline;
}
