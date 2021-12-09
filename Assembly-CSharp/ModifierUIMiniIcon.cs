using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200029E RID: 670
public class ModifierUIMiniIcon : MonoBehaviour
{
	// Token: 0x060013BB RID: 5051 RVA: 0x0000FA48 File Offset: 0x0000DC48
	public void SetIcon(Sprite sprite)
	{
		this.m_icon.sprite = sprite;
	}

	// Token: 0x060013BC RID: 5052 RVA: 0x0000FA56 File Offset: 0x0000DC56
	public void SetColor(Color c)
	{
		this.m_iconOutline.color = c;
	}

	// Token: 0x04001509 RID: 5385
	[Header("References")]
	[SerializeField]
	private Image m_icon;

	// Token: 0x0400150A RID: 5386
	[SerializeField]
	private Image m_iconOutline;
}
