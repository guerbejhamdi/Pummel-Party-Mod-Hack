using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000278 RID: 632
public class UITankAimer : MonoBehaviour
{
	// Token: 0x06001274 RID: 4724 RVA: 0x0000EE0F File Offset: 0x0000D00F
	public void SetColor(Color c)
	{
		this.m_fill.color = c;
	}

	// Token: 0x06001275 RID: 4725 RVA: 0x0000EE1D File Offset: 0x0000D01D
	public void SetFill(float v)
	{
		this.m_fill.fillAmount = v;
	}

	// Token: 0x0400138A RID: 5002
	[SerializeField]
	protected Image m_fill;

	// Token: 0x0400138B RID: 5003
	[SerializeField]
	protected Image m_background;
}
