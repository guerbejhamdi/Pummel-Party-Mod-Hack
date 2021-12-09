using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200054F RID: 1359
public class UIMinigameButton : MonoBehaviour
{
	// Token: 0x060023D8 RID: 9176 RVA: 0x000D85D4 File Offset: 0x000D67D4
	public void OnToggle()
	{
		if (this.m_active && GameManager.GetNumberOfActiveMinigames() <= 1)
		{
			return;
		}
		this.m_active = !this.m_active;
		if (!this.m_onBoard)
		{
			this.m_toggle.isOn = this.m_active;
		}
		this.m_minigame.SetIsActive(this.m_active);
	}

	// Token: 0x060023D9 RID: 9177 RVA: 0x000D862C File Offset: 0x000D682C
	public void SetMinigame(MinigameDefinition minigame)
	{
		this.m_minigame = minigame;
		this.m_nameTxt.text = LocalizationManager.GetTranslation(minigame.minigameNameToken, true, 0, true, false, null, null, true);
		if (this.m_minigame.screenshot != null)
		{
			this.m_iconImg.sprite = this.m_minigame.screenshot;
		}
		this.m_active = this.m_minigame.GetIsActive();
		if (!this.m_onBoard)
		{
			this.m_toggle.isOn = this.m_active;
		}
	}

	// Token: 0x040026CF RID: 9935
	[SerializeField]
	protected Text m_nameTxt;

	// Token: 0x040026D0 RID: 9936
	[SerializeField]
	protected Image m_iconImg;

	// Token: 0x040026D1 RID: 9937
	[SerializeField]
	protected Toggle m_toggle;

	// Token: 0x040026D2 RID: 9938
	[SerializeField]
	protected bool m_onBoard;

	// Token: 0x040026D3 RID: 9939
	private bool m_active;

	// Token: 0x040026D4 RID: 9940
	private MinigameDefinition m_minigame;
}
