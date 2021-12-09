using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x0200009B RID: 155
public class RulesetUIWindow : MonoBehaviour
{
	// Token: 0x0600033E RID: 830 RVA: 0x00038644 File Offset: 0x00036844
	public void Awake()
	{
		this.m_window = base.GetComponent<MainMenuWindow>();
		this.m_uiElements.Add(this.m_rulesetElement.GetComponent<RulesetUIElement>());
		for (int i = 0; i < 15; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_rulesetElement, this.m_container);
			gameObject.name = "Ruleset Element " + (i + 1).ToString();
			RulesetUIElement component = gameObject.GetComponent<RulesetUIElement>();
			this.m_uiElements.Add(component);
		}
		this.m_window.UpdateSelectables();
	}

	// Token: 0x0600033F RID: 831 RVA: 0x00005B5C File Offset: 0x00003D5C
	public void Reset()
	{
		this.OpenMainWindow();
	}

	// Token: 0x06000340 RID: 832 RVA: 0x00005B5C File Offset: 0x00003D5C
	public void OnEnable()
	{
		this.OpenMainWindow();
	}

	// Token: 0x06000341 RID: 833 RVA: 0x000386CC File Offset: 0x000368CC
	private void CreateMainWindow()
	{
		this.m_mainWindow.Clear();
		string translation = LocalizationManager.GetTranslation("Ruleset_New", true, 0, true, false, null, null, true);
		string translation2 = LocalizationManager.GetTranslation("Ruleset_Host", true, 0, true, false, null, null, true);
		string translation3 = LocalizationManager.GetTranslation("Exit", true, 0, true, false, null, null, true);
		if (!NetSystem.IsConnected || NetSystem.IsServer)
		{
			foreach (GameRuleset ruleset in GameManager.RulesetManager.GetRulesets())
			{
				this.AddRulesetButton(this.m_mainWindow, ruleset);
			}
			List<GameRuleset> rulesets = GameManager.RulesetManager.GetRulesets();
			this.m_mainWindow.Add(new RulesetButtonElement(translation, new RulesetEventDelegate(this.OnNewRuleset), null, RulesetUIStyles.NewRulesetStyle, rulesets.Count < 8, -1));
		}
		else
		{
			GameRuleset activeRuleset = GameManager.RulesetManager.ActiveRuleset;
			activeRuleset.Name = translation2;
			if (activeRuleset != null)
			{
				this.AddRulesetButton(this.m_mainWindow, activeRuleset);
			}
		}
		this.m_mainWindow.Add(new RulesetButtonElement(translation3, new RulesetEventDelegate(this.OnExitGameRulesets), null, RulesetUIStyles.BackStyle, true, InputActions.Cancel));
	}

	// Token: 0x06000342 RID: 834 RVA: 0x00038804 File Offset: 0x00036A04
	private void AddRulesetButton(List<RulesetElementDefinition> window, GameRuleset ruleset)
	{
		RulesetElementStyle style = RulesetUIStyles.DefaultStyle;
		bool flag = ruleset == GameManager.RulesetManager.ActiveRuleset;
		if (ruleset.IsDefault)
		{
			style = (flag ? RulesetUIStyles.DefaultRulesetStyleActive : RulesetUIStyles.DefaultRulesetStyle);
		}
		else
		{
			style = (flag ? RulesetUIStyles.DefaultStyleActive : RulesetUIStyles.DefaultStyle);
		}
		this.m_mainWindow.Add(new RulesetButtonElement(ruleset.Name, new RulesetEventDelegate(this.OnSelectRuleset), ruleset, style, true, -1));
	}

	// Token: 0x06000343 RID: 835 RVA: 0x00038878 File Offset: 0x00036A78
	public void SetWindow(string header, List<RulesetElementDefinition> elements)
	{
		this.m_headerText.text = header;
		for (int i = 0; i < elements.Count; i++)
		{
			RulesetUIElement rulesetUIElement = this.m_uiElements[i];
			RulesetElementDefinition definition = elements[i];
			rulesetUIElement.gameObject.SetActive(true);
			rulesetUIElement.Setup(definition);
			rulesetUIElement.Show((float)i * 0.025f, 0.15f);
		}
		for (int j = elements.Count; j < this.m_uiElements.Count; j++)
		{
			this.m_uiElements[j].ResetElement();
			this.m_uiElements[j].gameObject.SetActive(false);
		}
		this.m_window.UpdateSelectables();
	}

	// Token: 0x06000344 RID: 836 RVA: 0x00005B64 File Offset: 0x00003D64
	private void OnSelectRuleset(object obj)
	{
		new List<RulesetElementDefinition>();
		GameRuleset gameRuleset = (GameRuleset)obj;
		this.OpenRulesetWindow((GameRuleset)obj);
	}

	// Token: 0x06000345 RID: 837 RVA: 0x0003892C File Offset: 0x00036B2C
	public void OnSelectRulesetGroup(object obj)
	{
		GameRulesetGroup gameRulesetGroup = (GameRulesetGroup)obj;
		string translation = LocalizationManager.GetTranslation("Ruleset_Name", true, 0, true, false, null, null, true);
		gameRulesetGroup.ShowUIWindow(string.Concat(new string[]
		{
			translation,
			" / ",
			this.m_curRuleset.Name,
			" / ",
			gameRulesetGroup.GetRulesetGroupName()
		}), this);
	}

	// Token: 0x06000346 RID: 838 RVA: 0x00005B5C File Offset: 0x00003D5C
	private void OnExitRuleset(object obj)
	{
		this.OpenMainWindow();
	}

	// Token: 0x06000347 RID: 839 RVA: 0x00005B7F File Offset: 0x00003D7F
	private void OnExitGameRulesets(object obj)
	{
		GameManager.MainMenuUIController.DoButtonEvent(MainMenuButtonEventType.ExitGameRulesets);
	}

	// Token: 0x06000348 RID: 840 RVA: 0x00038990 File Offset: 0x00036B90
	private void OnNewRuleset(object obj)
	{
		GameRuleset gameRuleset = GameManager.RulesetManager.NewRuleset();
		if (gameRuleset != null)
		{
			GameManager.RulesetManager.SetActiveRuleset(gameRuleset);
			this.OpenRulesetWindow(gameRuleset);
			return;
		}
		this.OpenMainWindow();
	}

	// Token: 0x06000349 RID: 841 RVA: 0x000389C4 File Offset: 0x00036BC4
	private void OnRequestDeleteRuleset(object obj)
	{
		string translation = LocalizationManager.GetTranslation("Ruleset_ConfirmDeletion", true, 0, true, false, null, null, true);
		this.ShowConfirmation(translation, obj, new RulesetEventDelegate(this.OnCancel), new RulesetEventDelegate(this.OnDeleteRuleset));
	}

	// Token: 0x0600034A RID: 842 RVA: 0x00038A04 File Offset: 0x00036C04
	private void OnRequestRenameRuleset(object obj)
	{
		GameRuleset gameRuleset = (GameRuleset)obj;
		string translation = LocalizationManager.GetTranslation("Name", true, 0, true, false, null, null, true);
		this.ShowTextInput(translation, gameRuleset.Name, obj, new RulesetStringEventDelegate(this.OnTextChanged), new RulesetEventDelegate(this.OnCancel), new RulesetEventDelegate(this.OnRenameRuleset));
	}

	// Token: 0x0600034B RID: 843 RVA: 0x00005B8D File Offset: 0x00003D8D
	private void OnDeleteRuleset(object obj)
	{
		Debug.LogWarning("Deleting : " + ((GameRuleset)obj).Name);
		GameManager.RulesetManager.DeleteRuleset((GameRuleset)obj);
		this.OpenMainWindow();
	}

	// Token: 0x0600034C RID: 844 RVA: 0x00038A5C File Offset: 0x00036C5C
	private void OnRenameRuleset(object obj)
	{
		if (this.m_curText != "" && this.m_curText != null)
		{
			Debug.LogWarning("Renaming : " + ((GameRuleset)obj).Name + " to " + this.m_curText);
			GameManager.RulesetManager.RenameRuleset((GameRuleset)obj, this.m_curText);
			this.OpenMainWindow();
		}
	}

	// Token: 0x0600034D RID: 845 RVA: 0x00005BBF File Offset: 0x00003DBF
	private void OnTextChanged(object obj, string text)
	{
		this.m_curText = text;
	}

	// Token: 0x0600034E RID: 846 RVA: 0x00005BC8 File Offset: 0x00003DC8
	private void OnCancel(object obj)
	{
		this.OpenRulesetWindow((GameRuleset)obj);
	}

	// Token: 0x0600034F RID: 847 RVA: 0x00038AC4 File Offset: 0x00036CC4
	private void OpenMainWindow()
	{
		this.CreateMainWindow();
		string translation = LocalizationManager.GetTranslation("Ruleset_Name", true, 0, true, false, null, null, true);
		this.SetWindow(translation, this.m_mainWindow);
	}

	// Token: 0x06000350 RID: 848 RVA: 0x00038AF8 File Offset: 0x00036CF8
	private void OpenRulesetWindow(GameRuleset ruleset)
	{
		this.m_curRuleset = ruleset;
		string translation = LocalizationManager.GetTranslation("Ruleset_SetActive", true, 0, true, false, null, null, true);
		string translation2 = LocalizationManager.GetTranslation("Rename", true, 0, true, false, null, null, true);
		string translation3 = LocalizationManager.GetTranslation("Delete", true, 0, true, false, null, null, true);
		string translation4 = LocalizationManager.GetTranslation("Back", true, 0, true, false, null, null, true);
		List<RulesetElementDefinition> list = new List<RulesetElementDefinition>();
		foreach (GameRulesetGroup gameRulesetGroup in ruleset.GetRulesetGroups())
		{
			list.Add(new RulesetButtonElement(gameRulesetGroup.GetRulesetGroupName(), new RulesetEventDelegate(this.OnSelectRulesetGroup), gameRulesetGroup, new RulesetElementStyle
			{
				elementIcon = gameRulesetGroup.GetRulesetGroupIcon()
			}, true, -1));
		}
		list.Add(new RulesetEmptyElement(RulesetUIStyles.DefaultStyle, true));
		if (NetSystem.IsServer)
		{
			list.Add(new RulesetButtonElement(translation, new RulesetEventDelegate(this.OnSelectActiveRuleset), ruleset, RulesetUIStyles.SelectActiveRulesetStyle, true, -1));
		}
		if (NetSystem.IsServer && !ruleset.IsDefault)
		{
			list.Add(new RulesetButtonElement(translation2, new RulesetEventDelegate(this.OnRequestRenameRuleset), ruleset, RulesetUIStyles.Rename, true, -1));
			list.Add(new RulesetButtonElement(translation3, new RulesetEventDelegate(this.OnRequestDeleteRuleset), ruleset, RulesetUIStyles.DeleteRulesetStyle, true, -1));
		}
		list.Add(new RulesetButtonElement(translation4, new RulesetEventDelegate(this.OnExitRuleset), null, RulesetUIStyles.BackStyle, true, InputActions.Cancel));
		string translation5 = LocalizationManager.GetTranslation("Ruleset_Name", true, 0, true, false, null, null, true);
		this.SetWindow(translation5 + " / " + ruleset.Name, list);
	}

	// Token: 0x06000351 RID: 849 RVA: 0x00005BD6 File Offset: 0x00003DD6
	public void OnExitRulesetGroup(object obj)
	{
		this.OpenRulesetWindow(this.m_curRuleset);
	}

	// Token: 0x06000352 RID: 850 RVA: 0x00005BE4 File Offset: 0x00003DE4
	public void OnSelectActiveRuleset(object obj)
	{
		GameManager.RulesetManager.SetActiveRuleset(this.m_curRuleset);
		this.OpenMainWindow();
	}

	// Token: 0x06000353 RID: 851 RVA: 0x00038CB0 File Offset: 0x00036EB0
	public void ShowConfirmation(string text, object obj, RulesetEventDelegate cancel, RulesetEventDelegate confirm)
	{
		List<RulesetElementDefinition> list = new List<RulesetElementDefinition>();
		string translation = LocalizationManager.GetTranslation("Delete", true, 0, true, false, null, null, true);
		string translation2 = LocalizationManager.GetTranslation("Cancel", true, 0, true, false, null, null, true);
		string translation3 = LocalizationManager.GetTranslation("Confirm", true, 0, true, false, null, null, true);
		list.Add(new RulesetLabelElement(text, RulesetUIStyles.DefaultStyle, true));
		list.Add(new RulesetButtonElement(translation, confirm, obj, RulesetUIStyles.DeleteRulesetStyle, true, -1));
		list.Add(new RulesetButtonElement(translation2, cancel, obj, RulesetUIStyles.BackStyle, true, InputActions.Cancel));
		this.SetWindow(translation3, list);
	}

	// Token: 0x06000354 RID: 852 RVA: 0x00038D44 File Offset: 0x00036F44
	public void ShowTextInput(string label, string text, object obj, RulesetStringEventDelegate textChanged, RulesetEventDelegate cancel, RulesetEventDelegate confirm)
	{
		List<RulesetElementDefinition> list = new List<RulesetElementDefinition>();
		this.m_curText = text;
		string translation = LocalizationManager.GetTranslation("Cancel", true, 0, true, false, null, null, true);
		string translation2 = LocalizationManager.GetTranslation("Confirm", true, 0, true, false, null, null, true);
		string translation3 = LocalizationManager.GetTranslation("Rename", true, 0, true, false, null, null, true);
		list.Add(new RulesetLabelElement(label, RulesetUIStyles.DefaultStyle, true));
		list.Add(new RulesetTextInputElement(text, textChanged, obj, RulesetUIStyles.DefaultStyle, true));
		list.Add(new RulesetButtonElement(translation2, confirm, obj, RulesetUIStyles.SelectActiveRulesetStyle, true, -1));
		list.Add(new RulesetButtonElement(translation, cancel, obj, RulesetUIStyles.BackStyle, true, InputActions.Cancel));
		this.SetWindow(translation3, list);
	}

	// Token: 0x0400035D RID: 861
	[SerializeField]
	private GameObject m_rulesetElement;

	// Token: 0x0400035E RID: 862
	[SerializeField]
	private RectTransform m_container;

	// Token: 0x0400035F RID: 863
	[SerializeField]
	private Text m_headerText;

	// Token: 0x04000360 RID: 864
	private List<RulesetElementDefinition> m_mainWindow = new List<RulesetElementDefinition>();

	// Token: 0x04000361 RID: 865
	private List<RulesetUIElement> m_uiElements = new List<RulesetUIElement>();

	// Token: 0x04000362 RID: 866
	private const int m_pooledElements = 15;

	// Token: 0x04000363 RID: 867
	private MainMenuWindow m_window;

	// Token: 0x04000364 RID: 868
	private GameRuleset m_curRuleset;

	// Token: 0x04000365 RID: 869
	private string m_curText = "";
}
