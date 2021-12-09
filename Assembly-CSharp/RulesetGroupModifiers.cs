using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000085 RID: 133
public class RulesetGroupModifiers : GameRulesetGroup
{
	// Token: 0x060002D1 RID: 721 RVA: 0x00037280 File Offset: 0x00035480
	public RulesetGroupModifiers()
	{
		foreach (GameModifierDefinition gameModifierDefinition in GameManager.GetModifierList())
		{
			ushort key = (ushort)GameModifierDefinition.GetGameModifierID(gameModifierDefinition);
			Dictionary<ushort, ModifierActivationState> dictionary = null;
			if (gameModifierDefinition.modifierType == GameModifierType.BoardModifier)
			{
				dictionary = this.m_boardModifierStateMap;
			}
			else if (gameModifierDefinition.modifierType == GameModifierType.BoardModifier)
			{
				dictionary = this.m_minigameModifierStateMap;
			}
			else if (gameModifierDefinition.modifierType == GameModifierType.BoardModifier)
			{
				dictionary = this.m_globalMinigameModifierStateMap;
			}
			if (dictionary.ContainsKey(key) || this.m_modifierIDMap.ContainsKey(key))
			{
				Debug.LogError("loading minigame ruleset error : minigame id already present in map " + gameModifierDefinition.name + " : " + key.ToString());
			}
			else
			{
				dictionary.Add(key, gameModifierDefinition.defaultActivationState);
				this.m_modifierIDMap.Add(key, gameModifierDefinition);
			}
		}
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x000373D0 File Offset: 0x000355D0
	public void Serialize(ZPBitStream stream, ushort version)
	{
		ushort val = (ushort)this.m_modifierIDMap.Count;
		stream.Write(val);
		foreach (KeyValuePair<ushort, GameModifierDefinition> keyValuePair in this.m_modifierIDMap)
		{
			stream.Write(keyValuePair.Key);
			stream.Write((byte)this.GetModifierState(keyValuePair.Key));
		}
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x00037454 File Offset: 0x00035654
	public void Deserialize(ZPBitStream stream, ushort version)
	{
		if (version < 2)
		{
			return;
		}
		int num = (int)stream.ReadUShort();
		for (int i = 0; i < num; i++)
		{
			ushort key = stream.ReadUShort();
			ModifierActivationState value = (ModifierActivationState)stream.ReadByte();
			if (this.m_modifierIDMap.ContainsKey(key))
			{
				switch (this.m_modifierIDMap[key].modifierType)
				{
				case GameModifierType.BoardModifier:
					this.m_boardModifierStateMap[key] = value;
					break;
				case GameModifierType.MinigameModifier:
					this.m_globalMinigameModifierStateMap[key] = value;
					break;
				case GameModifierType.GlobalMinigameModifier:
					this.m_minigameModifierStateMap[key] = value;
					break;
				}
			}
		}
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x000374E8 File Offset: 0x000356E8
	private ModifierActivationState GetModifierState(ushort id)
	{
		if (this.m_boardModifierStateMap.ContainsKey(id))
		{
			return this.m_boardModifierStateMap[id];
		}
		if (this.m_minigameModifierStateMap.ContainsKey(id))
		{
			return this.m_minigameModifierStateMap[id];
		}
		if (this.m_globalMinigameModifierStateMap.ContainsKey(id))
		{
			return this.m_globalMinigameModifierStateMap[id];
		}
		return ModifierActivationState.Disabled;
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Apply()
	{
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x00005730 File Offset: 0x00003930
	public string GetRulesetGroupName()
	{
		return LocalizationManager.GetTranslation("Modifiers", true, 0, true, false, null, null, true);
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x00005743 File Offset: 0x00003943
	public RulesetGroupType GetRulesetType()
	{
		return RulesetGroupType.Modifiers;
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x00005746 File Offset: 0x00003946
	public Sprite GetRulesetGroupIcon()
	{
		return RulesetUIStyles.Modifier.elementIcon;
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x000053AE File Offset: 0x000035AE
	public List<RulesetElementDefinition> GetUIWindow(RulesetUIWindow window)
	{
		return null;
	}

	// Token: 0x060002DA RID: 730 RVA: 0x00037548 File Offset: 0x00035748
	public List<BoardModifier> GenerateBoardModifiers(string sceneName)
	{
		List<BoardModifier> list = new List<BoardModifier>();
		foreach (KeyValuePair<ushort, ModifierActivationState> keyValuePair in this.m_boardModifierStateMap)
		{
			bool flag = keyValuePair.Value == ModifierActivationState.Enabled;
			if (sceneName != null && sceneName == "TempleMap_Scene" && this.m_modifierIDMap[keyValuePair.Key].boardModifierID == BoardModifierID.FakeChests && keyValuePair.Value == ModifierActivationState.Map)
			{
				flag = true;
			}
			if (flag)
			{
				BoardModifier boardModifier = (BoardModifier)this.CreateModifierInstance(keyValuePair.Key);
				if (boardModifier != null)
				{
					list.Add(boardModifier);
				}
			}
		}
		return list;
	}

	// Token: 0x060002DB RID: 731 RVA: 0x00037600 File Offset: 0x00035800
	public GameModifier CreateModifierInstance(ushort id)
	{
		GameModifierDefinition gameModifierDefinition = null;
		if (this.m_modifierIDMap.TryGetValue(id, out gameModifierDefinition) && gameModifierDefinition.scriptTypeName != "")
		{
			return Activator.CreateInstance(Type.GetType(gameModifierDefinition.scriptTypeName + ", Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")) as GameModifier;
		}
		return null;
	}

	// Token: 0x060002DC RID: 732 RVA: 0x00005752 File Offset: 0x00003952
	public void ShowUIWindow(string header, RulesetUIWindow window)
	{
		this.m_header = header;
		this.m_window = window;
		this.OpenBoardModifiers(null);
	}

	// Token: 0x060002DD RID: 733 RVA: 0x0000398C File Offset: 0x00001B8C
	private void OpenMinigameModifiers(object obj)
	{
	}

	// Token: 0x060002DE RID: 734 RVA: 0x0000398C File Offset: 0x00001B8C
	private void OpenGlobalMinigameModifiers(object obj)
	{
	}

	// Token: 0x060002DF RID: 735 RVA: 0x00037654 File Offset: 0x00035854
	private void OpenBoardModifiers(object obj)
	{
		List<RulesetElementDefinition> list = new List<RulesetElementDefinition>();
		bool isServer = NetSystem.IsServer;
		LocalizationManager.GetTranslation("Modifiers_Board", true, 0, true, false, null, null, true);
		string translation = LocalizationManager.GetTranslation("Back", true, 0, true, false, null, null, true);
		int num = 0;
		int num2 = 0;
		this.m_activationElements[0] = "<color=#d17564>" + LocalizationManager.GetTranslation("Options_Disabled", true, 0, true, false, null, null, true) + "</color>";
		this.m_activationElements[1] = "<color=#a2d66d>" + LocalizationManager.GetTranslation("Options_Enabled", true, 0, true, false, null, null, true) + "</color>";
		this.m_activationElements[2] = "<color=#6fc0e8>" + LocalizationManager.GetTranslation("Map", true, 0, true, false, null, null, true) + "</color>";
		foreach (GameModifierDefinition gameModifierDefinition in GameManager.GetModifierList())
		{
			ushort key = (ushort)GameModifierDefinition.GetGameModifierID(gameModifierDefinition);
			if (this.m_boardModifierStateMap.ContainsKey(key) && num >= this.curPage * this.boardModifiersPerPage)
			{
				int startIndex = (int)this.m_boardModifierStateMap[key];
				RulesetElementStyle rulesetElementStyle = RulesetUIStyles.DefaultStyle.Clone();
				rulesetElementStyle.backgroundStripes = true;
				rulesetElementStyle.backgroundStripesColor = new Color(gameModifierDefinition.color.r, gameModifierDefinition.color.g, gameModifierDefinition.color.b, 0.4f);
				rulesetElementStyle.elementIcon = gameModifierDefinition.icon;
				rulesetElementStyle.iconColor = new Color(1f, 1f, 1f, 0.8f);
				string translation2 = LocalizationManager.GetTranslation(gameModifierDefinition.nameToken, true, 0, true, false, null, null, true);
				list.Add(new RulesetListElement(translation2, startIndex, this.m_activationElements, gameModifierDefinition, new RulesetListValueChanged(this.OnBoardModifierActivationStateChanged), rulesetElementStyle, isServer, null));
				num2++;
			}
			num++;
			if (num2 >= this.boardModifiersPerPage)
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
		string translation3 = LocalizationManager.GetTranslation("Page", true, 0, true, false, null, null, true);
		string translation4 = LocalizationManager.GetTranslation("Page_Next", true, 0, true, false, null, null, true);
		string translation5 = LocalizationManager.GetTranslation("Page_Previous", true, 0, true, false, null, null, true);
		list.Add(new RulesetButtonElement(translation4, new RulesetEventDelegate(this.OnNextPage), null, rulesetElementStyle2, flag, InputActions.UINext));
		flag = (this.curPage != 0);
		rulesetElementStyle2 = RulesetUIStyles.PreviousPageStyle;
		if (!flag)
		{
			rulesetElementStyle2 = RulesetUIStyles.PreviousPageStyle.Clone();
			rulesetElementStyle2.fontColor.a = 0.25f;
			rulesetElementStyle2.iconBackgroundColor.a = 0.25f;
			rulesetElementStyle2.iconColor.a = 0.25f;
		}
		list.Add(new RulesetButtonElement(translation5, new RulesetEventDelegate(this.OnPreviousPage), null, rulesetElementStyle2, flag, InputActions.UIPrevious));
		list.Add(new RulesetButtonElement(translation, new RulesetEventDelegate(this.m_window.OnExitRulesetGroup), null, RulesetUIStyles.BackStyle, true, InputActions.Cancel));
		this.m_window.SetWindow(string.Concat(new string[]
		{
			this.m_header,
			" - ",
			translation3,
			" ",
			(this.curPage + 1).ToString(),
			" / ",
			this.GetMaxPages().ToString()
		}), list);
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x00005769 File Offset: 0x00003969
	private void OnNextPage(object obj)
	{
		if (this.curPage + 1 < this.GetMaxPages())
		{
			this.curPage++;
		}
		this.ShowUIWindow(this.m_header, this.m_window);
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x0000579B File Offset: 0x0000399B
	private void OnPreviousPage(object obj)
	{
		if (this.curPage - 1 >= 0)
		{
			this.curPage--;
		}
		this.ShowUIWindow(this.m_header, this.m_window);
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x000057C8 File Offset: 0x000039C8
	private int GetMaxPages()
	{
		return this.m_boardModifierStateMap.Count / this.boardModifiersPerPage + ((this.m_boardModifierStateMap.Count % this.boardModifiersPerPage > 0) ? 1 : 0);
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x00037A54 File Offset: 0x00035C54
	private void OnBoardModifierActivationStateChanged(int index, object obj)
	{
		ushort key = (ushort)GameModifierDefinition.GetGameModifierID((GameModifierDefinition)obj);
		if (this.m_boardModifierStateMap.ContainsKey(key))
		{
			this.m_boardModifierStateMap[key] = (ModifierActivationState)index;
			GameManager.RulesetManager.RulesetDataChanged = true;
			return;
		}
		Debug.LogError("OnModifierEnabledChanged - Modifier could not be found in ruleset group. this shouldn't happen");
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x000057F6 File Offset: 0x000039F6
	private void OnExitModifierGroup(object obj)
	{
		this.ShowUIWindow(this.m_header, this.m_window);
	}

	// Token: 0x040002F3 RID: 755
	private Dictionary<ushort, ModifierActivationState> m_boardModifierStateMap = new Dictionary<ushort, ModifierActivationState>();

	// Token: 0x040002F4 RID: 756
	private Dictionary<ushort, ModifierActivationState> m_minigameModifierStateMap = new Dictionary<ushort, ModifierActivationState>();

	// Token: 0x040002F5 RID: 757
	private Dictionary<ushort, ModifierActivationState> m_globalMinigameModifierStateMap = new Dictionary<ushort, ModifierActivationState>();

	// Token: 0x040002F6 RID: 758
	private Dictionary<ushort, GameModifierDefinition> m_modifierIDMap = new Dictionary<ushort, GameModifierDefinition>();

	// Token: 0x040002F7 RID: 759
	private string[] m_activationElements = new string[]
	{
		"<color=#a2d66d>Disabled</color>",
		"<color=#d17564>Enabled</color>",
		"<color=#6fc0e8>Map</color>"
	};

	// Token: 0x040002F8 RID: 760
	private RulesetUIWindow m_window;

	// Token: 0x040002F9 RID: 761
	private string m_header = "";

	// Token: 0x040002FA RID: 762
	private int curPage;

	// Token: 0x040002FB RID: 763
	private int itemsPerPage = 7;

	// Token: 0x040002FC RID: 764
	private int boardModifiersPerPage = 7;
}
