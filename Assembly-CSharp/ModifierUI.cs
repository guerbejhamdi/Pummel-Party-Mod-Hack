using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000298 RID: 664
public class ModifierUI : MonoBehaviour
{
	// Token: 0x0600138B RID: 5003 RVA: 0x00096588 File Offset: 0x00094788
	public void Awake()
	{
		this.m_curColor = this.m_defaultOutline;
		this.m_panelRT = this.m_panel.GetComponent<RectTransform>();
		this.m_panelRT.anchoredPosition = new Vector2(0f, 64f);
		this.m_window.SetActive(false);
	}

	// Token: 0x0600138C RID: 5004 RVA: 0x0000F8B0 File Offset: 0x0000DAB0
	public void Show()
	{
		this.m_window.SetActive(true);
	}

	// Token: 0x0600138D RID: 5005 RVA: 0x0000F8BE File Offset: 0x0000DABE
	public void Hide()
	{
		this.m_window.SetActive(false);
	}

	// Token: 0x0600138E RID: 5006 RVA: 0x000965D8 File Offset: 0x000947D8
	public void SetModifiers(List<GameModifierDefinition> modifiers)
	{
		Debug.LogError("Set Modifiers UI : " + modifiers.Count.ToString());
		this.m_curModifiers = modifiers;
		if (this.m_curModifiers.Count <= 0 || this.m_curModifiers == null)
		{
			return;
		}
		this.m_window.SetActive(true);
		this.CreateMiniIcons(modifiers.Count);
		foreach (ModifierUIMiniIcon modifierUIMiniIcon in this.m_miniIconList)
		{
			modifierUIMiniIcon.gameObject.SetActive(false);
		}
		for (int i = 0; i < modifiers.Count; i++)
		{
			this.m_miniIconList[i].SetIcon(modifiers[i].icon);
			this.m_miniIconList[i].SetColor(modifiers[i].color);
			this.m_miniIconList[i].gameObject.SetActive(true);
		}
		if (this.m_curModifiers.Count > 0)
		{
			this.SetCurrentModifier(this.m_curModifiers[0]);
		}
	}

	// Token: 0x0600138F RID: 5007 RVA: 0x00096704 File Offset: 0x00094904
	private void CreateMiniIcons(int num)
	{
		if (num > this.m_miniIconList.Count)
		{
			int num2 = num - this.m_miniIconList.Count;
			for (int i = 0; i < num2; i++)
			{
				int count = this.m_miniIconList.Count;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_miniIcon, this.m_hideGroup.transform);
				gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-4f - (float)count * 26f, -4f);
				ModifierUIMiniIcon component = gameObject.GetComponent<ModifierUIMiniIcon>();
				this.m_miniIconList.Add(component);
			}
		}
	}

	// Token: 0x06001390 RID: 5008 RVA: 0x00096790 File Offset: 0x00094990
	public void ShowAllModifiers(float totalTime)
	{
		if (this.m_curModifiers == null || this.m_curModifiers.Count <= 0)
		{
			return;
		}
		Debug.LogError("Show All Modifiers");
		if (this.m_showModifiersRoutine != null)
		{
			base.StopCoroutine(this.m_showModifiersRoutine);
			this.m_showModifiersRoutine = null;
			this.m_isShowingAllModifiers = false;
		}
		this.m_isShowingAllModifiers = true;
		base.StartCoroutine(this.ShowAllModifiersRoutine(totalTime));
	}

	// Token: 0x06001391 RID: 5009 RVA: 0x0000F8CC File Offset: 0x0000DACC
	private IEnumerator ShowAllModifiersRoutine(float totalTime)
	{
		float stepTime = totalTime / (float)this.m_curModifiers.Count;
		this.m_curModifierIndex = -1;
		foreach (GameModifierDefinition gameModifierDefinition in this.m_curModifiers)
		{
			this.NextModifier();
			yield return new WaitForSeconds(stepTime);
		}
		List<GameModifierDefinition>.Enumerator enumerator = default(List<GameModifierDefinition>.Enumerator);
		this.m_isShowingAllModifiers = false;
		yield break;
		yield break;
	}

	// Token: 0x06001392 RID: 5010 RVA: 0x000967F8 File Offset: 0x000949F8
	public void Update()
	{
		if (!this.m_window.activeSelf)
		{
			return;
		}
		if (!this.m_isShowingAllModifiers)
		{
			foreach (Player player in ReInput.players.AllPlayers)
			{
				if (player != ReInput.players.SystemPlayer && (player.GetButtonDown(InputActions.OpenStatistics) || (player.controllers.GetLastActiveController() != null && player.controllers.GetLastActiveController().type == ControllerType.Joystick && player.GetButtonDown(InputActions.UITabLeft))))
				{
					this.NextModifier();
				}
			}
			if (!this.m_windowHidden && Time.unscaledTime > this.m_hideWindowTime)
			{
				this.m_windowRoutine = base.StartCoroutine(this.HideWindow());
			}
		}
		Rect uvRect = this.m_stripeImage.uvRect;
		uvRect.x -= Time.unscaledDeltaTime * 2f;
		this.m_stripeImage.uvRect = uvRect;
	}

	// Token: 0x06001393 RID: 5011 RVA: 0x00096900 File Offset: 0x00094B00
	private void NextModifier()
	{
		if (this.m_curModifiers == null || this.m_curModifiers.Count <= 0)
		{
			return;
		}
		this.m_curModifierIndex++;
		if (this.m_curModifierIndex >= this.m_curModifiers.Count)
		{
			this.m_curModifierIndex = 0;
		}
		if (this.m_inProgress)
		{
			LeanTween.cancel(this.m_panel.gameObject);
		}
		if (this.m_coroutine != null)
		{
			base.StopCoroutine(this.m_coroutine);
			this.m_coroutine = null;
		}
		this.m_inProgress = true;
		this.m_coroutine = base.StartCoroutine(this.SetCurrentModifier(this.m_curModifiers[this.m_curModifierIndex]));
	}

	// Token: 0x06001394 RID: 5012 RVA: 0x0000F8E2 File Offset: 0x0000DAE2
	private void ClearWindowAnimation()
	{
		if (this.m_windowRoutine != null)
		{
			base.StopCoroutine(this.m_windowRoutine);
		}
		LeanTween.cancel(this.m_showGroup.gameObject);
		LeanTween.cancel(this.m_hideGroup.gameObject);
	}

	// Token: 0x06001395 RID: 5013 RVA: 0x0000F918 File Offset: 0x0000DB18
	private IEnumerator HideWindow()
	{
		this.m_windowHidden = true;
		this.ClearWindowAnimation();
		if (!this.m_panelHidden)
		{
			yield return base.StartCoroutine(this.HidePanel());
		}
		LeanTween.value(this.m_showGroup.gameObject, new Action<float, object>(this.OnGroupAlpha), this.m_showGroup.alpha, 0f, 0.25f).setOnUpdateParam(this.m_showGroup);
		yield return new WaitForSeconds(0.25f);
		this.m_showGroup.gameObject.SetActive(false);
		this.m_hideGroup.gameObject.SetActive(true);
		LeanTween.value(this.m_hideGroup.gameObject, new Action<float, object>(this.OnGroupAlpha), this.m_hideGroup.alpha, 1f, 0.25f).setOnUpdateParam(this.m_hideGroup);
		yield break;
	}

	// Token: 0x06001396 RID: 5014 RVA: 0x0000F927 File Offset: 0x0000DB27
	private IEnumerator ShowWindow()
	{
		this.m_hideWindowTime = Time.unscaledTime + this.m_windowShowTime;
		this.m_windowHidden = false;
		this.ClearWindowAnimation();
		LeanTween.value(this.m_hideGroup.gameObject, new Action<float, object>(this.OnGroupAlpha), this.m_hideGroup.alpha, 0f, 0.125f).setOnUpdateParam(this.m_hideGroup);
		yield return new WaitForSeconds(0.125f);
		this.m_hideGroup.gameObject.SetActive(false);
		this.m_showGroup.gameObject.SetActive(true);
		LeanTween.value(this.m_showGroup.gameObject, new Action<float, object>(this.OnGroupAlpha), this.m_showGroup.alpha, 1f, 0.125f).setOnUpdateParam(this.m_showGroup);
		yield break;
	}

	// Token: 0x06001397 RID: 5015 RVA: 0x0000F936 File Offset: 0x0000DB36
	private void OnGroupAlpha(float value, object obj)
	{
		((CanvasGroup)obj).alpha = value;
	}

	// Token: 0x06001398 RID: 5016 RVA: 0x0000F944 File Offset: 0x0000DB44
	private IEnumerator SetCurrentModifier(GameModifierDefinition modifier)
	{
		this.m_hideWindowTime = Time.unscaledTime + this.m_windowShowTime;
		string translation = LocalizationManager.GetTranslation(modifier.nameToken, true, 0, true, false, null, null, true);
		string translation2 = LocalizationManager.GetTranslation("Modifier", true, 0, true, false, null, null, true);
		this.m_modifierCountTxt.text = string.Concat(new string[]
		{
			translation2,
			" ",
			(this.m_curModifierIndex + 1).ToString(),
			"/",
			this.m_curModifiers.Count.ToString()
		});
		this.m_modifierNameTxt.text = translation;
		if (this.m_windowHidden)
		{
			this.m_windowRoutine = base.StartCoroutine(this.ShowWindow());
			yield return this.m_windowRoutine;
		}
		if (!this.m_panelHidden)
		{
			yield return base.StartCoroutine(this.HidePanel());
		}
		string translation3 = LocalizationManager.GetTranslation(modifier.descriptionToken, true, 0, true, false, null, null, true);
		this.m_panel.SetDescription(translation3);
		this.m_panel.SetIcon(modifier.icon);
		LeanTween.moveLocalY(this.m_panel.gameObject, 0f, 0.5f).setEaseOutBounce();
		LeanTween.value(this.m_panel.gameObject, new Action<Color, object>(this.OnSetColor), this.m_curColor, modifier.color, 0.5f).setEaseOutBack();
		Color color = modifier.color;
		color.a = 0.5f;
		this.m_stripeImage.color = color;
		this.m_panelHidden = false;
		yield return new WaitForSeconds(0.6f);
		this.m_inProgress = false;
		yield break;
	}

	// Token: 0x06001399 RID: 5017 RVA: 0x0000F95A File Offset: 0x0000DB5A
	private void OnSetColor(Color c, object o)
	{
		c.a = this.m_defaultOutline.a;
		this.m_curColor = c;
	}

	// Token: 0x0600139A RID: 5018 RVA: 0x0000F975 File Offset: 0x0000DB75
	private IEnumerator HidePanel()
	{
		Debug.Log("HidePanel");
		LeanTween.moveLocalY(this.m_panel.gameObject, 64f, 0.25f).setEaseOutExpo();
		LeanTween.value(this.m_panel.gameObject, new Action<Color, object>(this.OnSetColor), this.m_curColor, this.m_defaultOutline, 0.25f).setEaseOutBack();
		yield return new WaitForSeconds(0.3f);
		this.m_panelHidden = true;
		yield break;
	}

	// Token: 0x040014DD RID: 5341
	[Header("Panel")]
	[SerializeField]
	private ModifierUIPanel m_panel;

	// Token: 0x040014DE RID: 5342
	[Header("References")]
	[SerializeField]
	private GameObject m_window;

	// Token: 0x040014DF RID: 5343
	[SerializeField]
	private Text m_modifierNameTxt;

	// Token: 0x040014E0 RID: 5344
	[SerializeField]
	private Text m_modifierCountTxt;

	// Token: 0x040014E1 RID: 5345
	[SerializeField]
	private Image m_border;

	// Token: 0x040014E2 RID: 5346
	[SerializeField]
	private CanvasGroup m_showGroup;

	// Token: 0x040014E3 RID: 5347
	[SerializeField]
	private CanvasGroup m_hideGroup;

	// Token: 0x040014E4 RID: 5348
	[SerializeField]
	private RawImage m_stripeImage;

	// Token: 0x040014E5 RID: 5349
	[SerializeField]
	private GameObject m_miniIcon;

	// Token: 0x040014E6 RID: 5350
	[Header("Properties")]
	[SerializeField]
	private Color m_defaultOutline;

	// Token: 0x040014E7 RID: 5351
	[Header("Test")]
	[SerializeField]
	private GameModifierDefinition[] m_testModifiers;

	// Token: 0x040014E8 RID: 5352
	private RectTransform m_panelRT;

	// Token: 0x040014E9 RID: 5353
	private int m_curModifierIndex;

	// Token: 0x040014EA RID: 5354
	private bool m_panelHidden = true;

	// Token: 0x040014EB RID: 5355
	private bool m_windowHidden = true;

	// Token: 0x040014EC RID: 5356
	private Color m_curColor;

	// Token: 0x040014ED RID: 5357
	private float m_hideWindowTime;

	// Token: 0x040014EE RID: 5358
	private float m_windowShowTime = 4f;

	// Token: 0x040014EF RID: 5359
	private List<GameModifierDefinition> m_curModifiers = new List<GameModifierDefinition>();

	// Token: 0x040014F0 RID: 5360
	private List<ModifierUIMiniIcon> m_miniIconList = new List<ModifierUIMiniIcon>();

	// Token: 0x040014F1 RID: 5361
	private bool m_isShowingAllModifiers;

	// Token: 0x040014F2 RID: 5362
	private Coroutine m_showModifiersRoutine;

	// Token: 0x040014F3 RID: 5363
	private bool m_inProgress;

	// Token: 0x040014F4 RID: 5364
	private Coroutine m_coroutine;

	// Token: 0x040014F5 RID: 5365
	private Coroutine m_windowRoutine;
}
