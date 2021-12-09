using System;
using Rewired.UI.ControlMapper;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200052A RID: 1322
public class RewiredButtonRestyler : MonoBehaviour
{
	// Token: 0x060022C0 RID: 8896 RVA: 0x000190F6 File Offset: 0x000172F6
	public void Update()
	{
		if (this.m_lastChildCount != base.transform.childCount)
		{
			this.RestyleButtons();
			this.m_lastChildCount = base.transform.childCount;
		}
	}

	// Token: 0x060022C1 RID: 8897 RVA: 0x000D3AEC File Offset: 0x000D1CEC
	private void RestyleButtons()
	{
		foreach (CustomButton customButton in base.GetComponentsInChildren<CustomButton>())
		{
			ColorBlock colors = customButton.colors;
			colors.disabledColor = this.m_disabledColor;
			customButton.colors = colors;
			customButton.GetComponent<Image>().sprite = this.m_buttonSprite;
			Text componentInChildren = customButton.GetComponentInChildren<Text>();
			componentInChildren.fontStyle = FontStyle.Bold;
			componentInChildren.color = new Color(1f, 1f, 1f, 0.65f);
			componentInChildren.gameObject.AddComponent<Shadow>().effectColor = Color.black;
		}
	}

	// Token: 0x0400259E RID: 9630
	[SerializeField]
	private Color m_disabledColor;

	// Token: 0x0400259F RID: 9631
	[SerializeField]
	private Sprite m_buttonSprite;

	// Token: 0x040025A0 RID: 9632
	private int m_lastChildCount = -1;
}
