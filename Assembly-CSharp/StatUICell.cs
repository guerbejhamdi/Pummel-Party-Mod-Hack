using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020004BD RID: 1213
public class StatUICell : MonoBehaviour
{
	// Token: 0x06002033 RID: 8243 RVA: 0x000CA33C File Offset: 0x000C853C
	public void Setup(bool isHeader, bool isAlt, int buttonAction)
	{
		if (isHeader)
		{
			this.m_background.color = this.m_backgroundHeaderColor;
			this.m_cellText.fontStyle = FontStyle.Bold;
		}
		if (isAlt)
		{
			this.m_background.color = this.m_altBackgroundColor;
		}
		if (buttonAction != 0)
		{
			this.m_window = base.GetComponentInParent<StatUIWindow>();
			this.m_button = this.m_background.gameObject.AddComponent<Button>();
			ColorBlock colors = this.m_button.colors;
			colors.normalColor = this.m_background.color;
			colors.highlightedColor = this.m_highlightedBackgroundColor;
			colors.disabledColor = this.m_SelectedBackgroundColor;
			this.m_button.colors = colors;
			this.m_background.color = Color.white;
			this.m_button.onClick.AddListener(new UnityAction(this.ButtonClick));
			this.m_buttonAction = buttonAction;
		}
	}

	// Token: 0x06002034 RID: 8244 RVA: 0x000CA41C File Offset: 0x000C861C
	public void SetSelected(StatType selected)
	{
		if (this.m_button == null)
		{
			return;
		}
		StatType statType = (StatType)(this.m_buttonAction - 1);
		this.m_button.interactable = (statType != selected);
	}

	// Token: 0x06002035 RID: 8245 RVA: 0x0001782C File Offset: 0x00015A2C
	private void ButtonClick()
	{
		if (this.m_buttonAction == 0)
		{
			return;
		}
		this.m_window.SetSortType((StatType)(this.m_buttonAction - 1));
	}

	// Token: 0x06002036 RID: 8246 RVA: 0x0001784A File Offset: 0x00015A4A
	public void SetValue(string text)
	{
		this.m_cellText.text = text;
	}

	// Token: 0x06002037 RID: 8247 RVA: 0x00017858 File Offset: 0x00015A58
	public void SetTextColor(Color col)
	{
		this.m_cellText.color = col;
	}

	// Token: 0x06002038 RID: 8248 RVA: 0x00017866 File Offset: 0x00015A66
	public void SetBackgroundColor(Color col)
	{
		this.m_background.color = col;
	}

	// Token: 0x06002039 RID: 8249 RVA: 0x00017874 File Offset: 0x00015A74
	public void SetFontStyle(FontStyle style)
	{
		this.m_cellText.fontStyle = style;
	}

	// Token: 0x0600203A RID: 8250 RVA: 0x00017882 File Offset: 0x00015A82
	public void AddShadow()
	{
		this.m_cellText.gameObject.AddComponent<Shadow>().effectDistance = new Vector2(2f, -2f);
	}

	// Token: 0x04002301 RID: 8961
	[SerializeField]
	private Text m_cellText;

	// Token: 0x04002302 RID: 8962
	[SerializeField]
	private Image m_background;

	// Token: 0x04002303 RID: 8963
	[SerializeField]
	private Color m_backgroundHeaderColor;

	// Token: 0x04002304 RID: 8964
	[SerializeField]
	private Color m_altBackgroundColor;

	// Token: 0x04002305 RID: 8965
	[SerializeField]
	private Color m_highlightedBackgroundColor;

	// Token: 0x04002306 RID: 8966
	[SerializeField]
	private Color m_SelectedBackgroundColor;

	// Token: 0x04002307 RID: 8967
	private int m_buttonAction;

	// Token: 0x04002308 RID: 8968
	private Button m_button;

	// Token: 0x04002309 RID: 8969
	private StatUIWindow m_window;
}
