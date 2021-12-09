using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000083 RID: 131
public class RulesetGroupMinigames : GameRulesetGroup
{
	// Token: 0x060002C1 RID: 705 RVA: 0x00036C8C File Offset: 0x00034E8C
	public RulesetGroupMinigames()
	{
		foreach (MinigameDefinition minigameDefinition in GameManager.GetMinigameList())
		{
			if (this.m_minigameEnabledMap.ContainsKey(minigameDefinition.minigameID) || this.m_minigameIDMap.ContainsKey(minigameDefinition.minigameID))
			{
				Debug.LogError("loading minigame ruleset error : minigame id already present in map " + minigameDefinition.name + " : " + minigameDefinition.minigameID.ToString());
			}
			else
			{
				this.m_minigameEnabledMap.Add(minigameDefinition.minigameID, true);
				this.m_minigameIDMap.Add(minigameDefinition.minigameID, minigameDefinition);
			}
		}
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x00036D98 File Offset: 0x00034F98
	public void Serialize(ZPBitStream stream, ushort version)
	{
		stream.Write((ushort)this.m_minigameEnabledMap.Count);
		foreach (KeyValuePair<ushort, bool> keyValuePair in this.m_minigameEnabledMap)
		{
			stream.Write(keyValuePair.Key);
			stream.Write(keyValuePair.Value);
		}
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x00036E10 File Offset: 0x00035010
	public void Deserialize(ZPBitStream stream, ushort version)
	{
		ushort num = stream.ReadUShort();
		for (int i = 0; i < (int)num; i++)
		{
			ushort key = stream.ReadUShort();
			if (this.m_minigameEnabledMap.ContainsKey(key))
			{
				bool value = stream.ReadBool();
				this.m_minigameEnabledMap[key] = value;
			}
		}
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Apply()
	{
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x0000563E File Offset: 0x0000383E
	public string GetRulesetGroupName()
	{
		return LocalizationManager.GetTranslation("Minigames", true, 0, true, false, null, null, true);
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x00005651 File Offset: 0x00003851
	public RulesetGroupType GetRulesetType()
	{
		return RulesetGroupType.Minigames;
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x00005654 File Offset: 0x00003854
	public Sprite GetRulesetGroupIcon()
	{
		return RulesetUIStyles.Minigame.elementIcon;
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x000053AE File Offset: 0x000035AE
	public List<RulesetElementDefinition> GetUIWindow(RulesetUIWindow window)
	{
		return null;
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x00036E5C File Offset: 0x0003505C
	public void ShowUIWindow(string header, RulesetUIWindow window)
	{
		this.m_header = header;
		this.m_window = window;
		List<RulesetElementDefinition> list = new List<RulesetElementDefinition>();
		int num = 0;
		int num2 = 0;
		this.m_enabledDisabledElements[0] = "<color=#a2d66d>" + LocalizationManager.GetTranslation("Options_Enabled", true, 0, true, false, null, null, true) + "</color>";
		this.m_enabledDisabledElements[1] = "<color=#d17564>" + LocalizationManager.GetTranslation("Options_Disabled", true, 0, true, false, null, null, true) + "</color>";
		foreach (MinigameDefinition minigameDefinition in GameManager.GetMinigameList())
		{
			if (this.m_minigameEnabledMap.ContainsKey(minigameDefinition.minigameID) && num >= this.curPage * this.minigamesPerPage)
			{
				int startIndex = this.m_minigameEnabledMap[minigameDefinition.minigameID] ? 0 : 1;
				RulesetElementStyle rulesetElementStyle = RulesetUIStyles.Minigame.Clone();
				rulesetElementStyle.elementIcon = minigameDefinition.screenshot;
				bool isServer = NetSystem.IsServer;
				string translation = LocalizationManager.GetTranslation(minigameDefinition.minigameNameToken, true, 0, true, false, null, null, true);
				list.Add(new RulesetListElement(translation, startIndex, this.m_enabledDisabledElements, minigameDefinition, new RulesetListValueChanged(this.OnMinigameEnabledChanged), rulesetElementStyle, isServer, new RulesetListAllowValueChange(this.AllowMinigameEnabledChange)));
				num2++;
			}
			num++;
			if (num2 >= this.minigamesPerPage)
			{
				break;
			}
		}
		int num3 = 7 - list.Count;
		if (num3 > 0)
		{
			for (int i = 0; i < num3; i++)
			{
				list.Add(new RulesetEmptyElement(RulesetUIStyles.DefaultStyle, false));
			}
		}
		bool flag = this.curPage < this.GetMaxPages() - 1;
		RulesetElementStyle rulesetElementStyle2 = RulesetUIStyles.NextPageStyle;
		if (!flag)
		{
			rulesetElementStyle2 = RulesetUIStyles.NextPageStyle.Clone();
			rulesetElementStyle2.fontColor.a = 0.25f;
			rulesetElementStyle2.iconBackgroundColor.a = 0.25f;
			rulesetElementStyle2.iconColor.a = 0.25f;
		}
		string translation2 = LocalizationManager.GetTranslation("Page", true, 0, true, false, null, null, true);
		string translation3 = LocalizationManager.GetTranslation("Page_Next", true, 0, true, false, null, null, true);
		string translation4 = LocalizationManager.GetTranslation("Page_Previous", true, 0, true, false, null, null, true);
		string translation5 = LocalizationManager.GetTranslation("Back", true, 0, true, false, null, null, true);
		list.Add(new RulesetButtonElement(translation3, new RulesetEventDelegate(this.OnNextPage), null, rulesetElementStyle2, flag, InputActions.UINext));
		flag = (this.curPage != 0);
		rulesetElementStyle2 = RulesetUIStyles.PreviousPageStyle;
		if (!flag)
		{
			rulesetElementStyle2 = RulesetUIStyles.PreviousPageStyle.Clone();
			rulesetElementStyle2.fontColor.a = 0.25f;
			rulesetElementStyle2.iconBackgroundColor.a = 0.25f;
			rulesetElementStyle2.iconColor.a = 0.25f;
		}
		list.Add(new RulesetButtonElement(translation4, new RulesetEventDelegate(this.OnPreviousPage), null, rulesetElementStyle2, flag, InputActions.UIPrevious));
		list.Add(new RulesetButtonElement(translation5, new RulesetEventDelegate(window.OnExitRulesetGroup), null, RulesetUIStyles.BackStyle, true, InputActions.Cancel));
		this.m_window.SetWindow(string.Concat(new string[]
		{
			header,
			" - ",
			translation2,
			" ",
			(this.curPage + 1).ToString(),
			" / ",
			this.GetMaxPages().ToString()
		}), list);
	}

	// Token: 0x060002CA RID: 714 RVA: 0x00005660 File Offset: 0x00003860
	public bool IsMinigameEnabled(MinigameDefinition minigame)
	{
		if (this.m_minigameEnabledMap.ContainsKey(minigame.minigameID))
		{
			return this.m_minigameEnabledMap[minigame.minigameID];
		}
		Debug.LogError("IsMinigameEnabled - Minigame could not be found in ruleset group. this shouldn't happen");
		return false;
	}

	// Token: 0x060002CB RID: 715 RVA: 0x00005692 File Offset: 0x00003892
	private void OnNextPage(object obj)
	{
		if (this.curPage + 1 < this.GetMaxPages())
		{
			this.curPage++;
		}
		this.ShowUIWindow(this.m_header, this.m_window);
	}

	// Token: 0x060002CC RID: 716 RVA: 0x000056C4 File Offset: 0x000038C4
	private void OnPreviousPage(object obj)
	{
		if (this.curPage - 1 >= 0)
		{
			this.curPage--;
		}
		this.ShowUIWindow(this.m_header, this.m_window);
	}

	// Token: 0x060002CD RID: 717 RVA: 0x000056F1 File Offset: 0x000038F1
	private int GetMaxPages()
	{
		return this.m_minigameEnabledMap.Count / this.minigamesPerPage + ((this.m_minigameEnabledMap.Count % this.minigamesPerPage > 0) ? 1 : 0);
	}

	// Token: 0x060002CE RID: 718 RVA: 0x000371CC File Offset: 0x000353CC
	private void OnMinigameEnabledChanged(int index, object obj)
	{
		MinigameDefinition minigameDefinition = (MinigameDefinition)obj;
		if (this.m_minigameEnabledMap.ContainsKey(minigameDefinition.minigameID))
		{
			this.m_minigameEnabledMap[minigameDefinition.minigameID] = (index == 0);
			GameManager.RulesetManager.RulesetDataChanged = true;
			return;
		}
		Debug.LogError("OnMinigameEnabledChanged - Minigame could not be found in ruleset group. this shouldn't happen");
	}

	// Token: 0x060002CF RID: 719 RVA: 0x0000571F File Offset: 0x0000391F
	private bool AllowMinigameEnabledChange(int index, object obj)
	{
		return index != 0 || this.GetNumberOfActiveMinigames() > 1;
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x00037224 File Offset: 0x00035424
	private int GetNumberOfActiveMinigames()
	{
		int num = 0;
		foreach (KeyValuePair<ushort, bool> keyValuePair in this.m_minigameEnabledMap)
		{
			if (keyValuePair.Value)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x040002E8 RID: 744
	private Dictionary<ushort, bool> m_minigameEnabledMap = new Dictionary<ushort, bool>();

	// Token: 0x040002E9 RID: 745
	private Dictionary<ushort, MinigameDefinition> m_minigameIDMap = new Dictionary<ushort, MinigameDefinition>();

	// Token: 0x040002EA RID: 746
	private string[] m_enabledDisabledElements = new string[]
	{
		"<color=#a2d66d>Enabled</color>",
		"<color=#d17564>Disabled</color>"
	};

	// Token: 0x040002EB RID: 747
	private RulesetUIWindow m_window;

	// Token: 0x040002EC RID: 748
	private string m_header = "";

	// Token: 0x040002ED RID: 749
	private int curPage;

	// Token: 0x040002EE RID: 750
	private int minigamesPerPage = 7;
}
