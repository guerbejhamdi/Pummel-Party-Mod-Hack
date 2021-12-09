using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x02000098 RID: 152
public class RulesetUIElement : MonoBehaviour
{
	// Token: 0x06000319 RID: 793 RVA: 0x00037E98 File Offset: 0x00036098
	public void Update()
	{
		if (this.m_stripes.gameObject.activeSelf)
		{
			Rect uvRect = this.m_stripes.uvRect;
			uvRect.x -= Time.unscaledDeltaTime;
			this.m_stripes.uvRect = uvRect;
		}
	}

	// Token: 0x0600031A RID: 794 RVA: 0x00037EE4 File Offset: 0x000360E4
	public void Setup(RulesetElementDefinition definition)
	{
		this.ResetElement();
		this.m_definition = definition;
		this.m_label.text = definition.text;
		this.m_label.color = definition.style.fontColor;
		this.m_icon.SetActive(definition.style.elementIcon != null);
		this.m_iconImage.sprite = definition.style.elementIcon;
		this.m_iconImage.color = definition.style.iconColor;
		this.m_iconBackground.color = definition.style.iconBackgroundColor;
		this.m_borderImage.gameObject.SetActive(definition.style.border);
		this.m_borderImage.color = definition.style.borderColor;
		this.m_fitter.aspectRatio = definition.style.iconAspectRatio;
		float num = (float)definition.style.iconPadding;
		this.m_innerIconRect.offsetMax = new Vector2(-num, -num);
		this.m_innerIconRect.offsetMin = new Vector2(num, num);
		this.m_button.enabled = true;
		this.m_button.targetGraphic.gameObject.SetActive(true);
		this.m_mappedActionBtn.gameObject.SetActive(false);
		this.m_inputField.transform.parent.gameObject.SetActive(false);
		this.m_buttonElement.enabled = this.m_definition.enabled;
		this.m_buttonElement.interactable = this.m_definition.enabled;
		this.m_group.interactable = this.m_definition.enabled;
		this.m_stripes.gameObject.SetActive(this.m_definition.style.backgroundStripes);
		this.m_stripes.color = this.m_definition.style.backgroundStripesColor;
		this.m_hoverBackground.SetActive(this.m_definition.enabled);
		if (!NetSystem.IsServer)
		{
			RulesetListElementButton[] listButtons = this.m_listButtons;
			for (int i = 0; i < listButtons.Length; i++)
			{
				listButtons[i].SetState(this.m_definition.enabled ? BasicButtonBase.BasicButtonState.Enabled : BasicButtonBase.BasicButtonState.Disabled);
			}
			this.m_inputField.interactable = false;
		}
		this.m_listElement = null;
		this.m_numericElement = null;
		switch (definition.elementType)
		{
		case RulesetElementType.Button:
		{
			RulesetButtonElement rulesetButtonElement = (RulesetButtonElement)definition;
			if (rulesetButtonElement.mappedActionID != -1)
			{
				this.m_mappedActionBtn.ActionID = rulesetButtonElement.mappedActionID;
				this.m_mappedActionBtn.gameObject.SetActive(true);
			}
			this.m_buttonElement.Clear();
			return;
		}
		case RulesetElementType.Toggle:
			break;
		case RulesetElementType.Numeric:
			this.m_listObject.SetActive(true);
			this.m_numericElement = (RulesetNumericElement)this.m_definition;
			this.UpdateNumericElement();
			this.m_buttonElement.Clear();
			return;
		case RulesetElementType.List:
			this.m_listObject.SetActive(true);
			this.m_listElement = (RulesetListElement)this.m_definition;
			this.UpdateListElement();
			this.m_buttonElement.Clear();
			return;
		case RulesetElementType.Label:
			this.m_buttonElement.enabled = false;
			this.m_hoverBackground.SetActive(false);
			this.m_icon.SetActive(false);
			this.m_buttonElement.interactable = false;
			this.m_group.interactable = false;
			return;
		case RulesetElementType.TextInput:
		{
			RulesetTextInputElement rulesetTextInputElement = (RulesetTextInputElement)definition;
			this.m_button.enabled = false;
			this.m_button.interactable = false;
			this.m_hoverBackground.SetActive(false);
			this.m_label.text = "";
			this.m_icon.SetActive(false);
			this.m_inputField.transform.parent.gameObject.SetActive(true);
			this.m_inputField.interactable = true;
			this.m_inputField.text = rulesetTextInputElement.text;
			break;
		}
		case RulesetElementType.Empty:
			this.m_icon.SetActive(false);
			this.m_buttonElement.enabled = false;
			this.m_hoverBackground.SetActive(false);
			this.m_label.text = "";
			this.m_button.interactable = false;
			this.m_group.interactable = false;
			return;
		default:
			return;
		}
	}

	// Token: 0x0600031B RID: 795 RVA: 0x0000598B File Offset: 0x00003B8B
	public void OnEnable()
	{
		this.m_button.interactable = true;
	}

	// Token: 0x0600031C RID: 796 RVA: 0x00005999 File Offset: 0x00003B99
	public void OnDisable()
	{
		this.m_button.interactable = false;
	}

	// Token: 0x0600031D RID: 797 RVA: 0x000059A7 File Offset: 0x00003BA7
	public void ResetElement()
	{
		this.m_icon.SetActive(false);
		this.m_label.text = "";
		this.m_definition = null;
		this.m_listObject.SetActive(false);
	}

	// Token: 0x0600031E RID: 798 RVA: 0x00038300 File Offset: 0x00036500
	public void OnButtonPress()
	{
		if (this.m_definition != null && this.m_definition.elementType == RulesetElementType.Button)
		{
			RulesetButtonElement rulesetButtonElement = (RulesetButtonElement)this.m_definition;
			if (rulesetButtonElement.OnButtonPressed != null)
			{
				AudioSystem.PlayOneShot("ButtonPress01_SFXR", 1f, 0f);
				rulesetButtonElement.OnButtonPressed(rulesetButtonElement.obj);
			}
		}
	}

	// Token: 0x0600031F RID: 799 RVA: 0x0003835C File Offset: 0x0003655C
	public void OnTextChanged(string text)
	{
		if (this.m_definition != null && this.m_definition.elementType == RulesetElementType.TextInput)
		{
			RulesetTextInputElement rulesetTextInputElement = (RulesetTextInputElement)this.m_definition;
			if (rulesetTextInputElement.OnTextChanged != null)
			{
				rulesetTextInputElement.OnTextChanged(rulesetTextInputElement.obj, text);
			}
		}
	}

	// Token: 0x06000320 RID: 800 RVA: 0x000383A8 File Offset: 0x000365A8
	public void IncrementList(bool right)
	{
		if (this.m_listElement == null)
		{
			if (this.m_numericElement != null)
			{
				float num = this.m_numericElement.numericValue + (right ? this.m_numericElement.numericStep : (-this.m_numericElement.numericStep));
				if (num > this.m_numericElement.numericMax || num < this.m_numericElement.numericMin)
				{
					return;
				}
				if (!NetSystem.IsServer || (this.m_numericElement.AllowValueChange != null && !this.m_numericElement.AllowValueChange(num, this.m_numericElement.obj)))
				{
					return;
				}
				this.m_numericElement.numericValue = num;
				if (this.m_numericElement.OnValueChanged != null)
				{
					this.m_numericElement.OnValueChanged(num, this.m_numericElement.obj);
				}
				this.UpdateNumericElement();
			}
			return;
		}
		if (!NetSystem.IsServer || (this.m_listElement.AllowValueChange != null && !this.m_listElement.AllowValueChange(this.m_listElement.curIndex, this.m_listElement.obj)))
		{
			return;
		}
		if (right)
		{
			this.m_listElement.curIndex = (this.m_listElement.curIndex + 1) % this.m_listElement.elements.Length;
		}
		else
		{
			this.m_listElement.curIndex--;
			if (this.m_listElement.curIndex < 0)
			{
				this.m_listElement.curIndex = this.m_listElement.elements.Length - 1;
			}
		}
		if (this.m_listElement.OnValueChanged != null)
		{
			this.m_listElement.OnValueChanged(this.m_listElement.curIndex, this.m_listElement.obj);
		}
		this.UpdateListElement();
	}

	// Token: 0x06000321 RID: 801 RVA: 0x000059D8 File Offset: 0x00003BD8
	private void UpdateListElement()
	{
		this.m_listText.text = this.m_listElement.elements[this.m_listElement.curIndex];
	}

	// Token: 0x06000322 RID: 802 RVA: 0x000059FC File Offset: 0x00003BFC
	private void UpdateNumericElement()
	{
		this.m_listText.text = this.m_numericElement.GetCurValue();
	}

	// Token: 0x06000323 RID: 803 RVA: 0x00005A14 File Offset: 0x00003C14
	public void Show(float delay, float animTime)
	{
		if (this.showRoutine != null)
		{
			base.StopCoroutine(this.showRoutine);
		}
		this.showRoutine = base.StartCoroutine(this.ShowRoutine(delay, animTime));
	}

	// Token: 0x06000324 RID: 804 RVA: 0x00005A3E File Offset: 0x00003C3E
	public IEnumerator ShowRoutine(float delay, float animTime)
	{
		this.m_group.alpha = 0f;
		yield return new WaitForSeconds(delay);
		float startTime = Time.time;
		float elapsed = 0f;
		while (elapsed < animTime)
		{
			elapsed = Time.time - startTime;
			float alpha = elapsed / animTime;
			this.m_group.alpha = alpha;
			yield return null;
		}
		this.m_group.alpha = 1f;
		this.showRoutine = null;
		yield break;
	}

	// Token: 0x0400032F RID: 815
	[SerializeField]
	private Button m_button;

	// Token: 0x04000330 RID: 816
	[SerializeField]
	private Text m_label;

	// Token: 0x04000331 RID: 817
	[SerializeField]
	private GameObject m_icon;

	// Token: 0x04000332 RID: 818
	[SerializeField]
	private Image m_iconImage;

	// Token: 0x04000333 RID: 819
	[SerializeField]
	private Image m_iconBackground;

	// Token: 0x04000334 RID: 820
	[SerializeField]
	private Image m_borderImage;

	// Token: 0x04000335 RID: 821
	[SerializeField]
	private CanvasGroup m_group;

	// Token: 0x04000336 RID: 822
	[SerializeField]
	private AspectRatioFitter m_fitter;

	// Token: 0x04000337 RID: 823
	[SerializeField]
	private RectTransform m_innerIconRect;

	// Token: 0x04000338 RID: 824
	[Header("List Element")]
	[SerializeField]
	private GameObject m_listObject;

	// Token: 0x04000339 RID: 825
	[SerializeField]
	private Text m_listText;

	// Token: 0x0400033A RID: 826
	[SerializeField]
	private RulesetSelectable m_buttonElement;

	// Token: 0x0400033B RID: 827
	[SerializeField]
	private CanvasGroup m_contentGroup;

	// Token: 0x0400033C RID: 828
	[SerializeField]
	private RulesetListElementButton[] m_listButtons;

	// Token: 0x0400033D RID: 829
	[SerializeField]
	private GameObject m_hoverBackground;

	// Token: 0x0400033E RID: 830
	[SerializeField]
	private RulesetListElementButton m_mappedActionBtn;

	// Token: 0x0400033F RID: 831
	[SerializeField]
	private InputField m_inputField;

	// Token: 0x04000340 RID: 832
	[SerializeField]
	private RawImage m_stripes;

	// Token: 0x04000341 RID: 833
	private RulesetElementDefinition m_definition;

	// Token: 0x04000342 RID: 834
	private RulesetListElement m_listElement;

	// Token: 0x04000343 RID: 835
	private RulesetNumericElement m_numericElement;

	// Token: 0x04000344 RID: 836
	private Coroutine showRoutine;
}
