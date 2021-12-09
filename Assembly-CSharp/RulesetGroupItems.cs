using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000082 RID: 130
public class RulesetGroupItems : GameRulesetGroup
{
	// Token: 0x060002B1 RID: 689 RVA: 0x00036708 File Offset: 0x00034908
	public RulesetGroupItems()
	{
		foreach (ItemDetails itemDetails in GameManager.ItemList.items)
		{
			this.m_itemsEnabledMap.Add(itemDetails.itemUniqueID, true);
			this.m_itemIDMap.Add(itemDetails.itemUniqueID, itemDetails);
		}
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x000367A0 File Offset: 0x000349A0
	public void Serialize(ZPBitStream stream, ushort version)
	{
		stream.Write((ushort)this.m_itemsEnabledMap.Count);
		foreach (KeyValuePair<ushort, bool> keyValuePair in this.m_itemsEnabledMap)
		{
			stream.Write(keyValuePair.Key);
			stream.Write(keyValuePair.Value);
		}
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x00036818 File Offset: 0x00034A18
	public void Deserialize(ZPBitStream stream, ushort version)
	{
		ushort num = stream.ReadUShort();
		for (int i = 0; i < (int)num; i++)
		{
			ushort key = stream.ReadUShort();
			if (this.m_itemsEnabledMap.ContainsKey(key))
			{
				bool value = stream.ReadBool();
				this.m_itemsEnabledMap[key] = value;
			}
		}
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Apply()
	{
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x0000553D File Offset: 0x0000373D
	public string GetRulesetGroupName()
	{
		return LocalizationManager.GetTranslation("Items", true, 0, true, false, null, null, true);
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x00005550 File Offset: 0x00003750
	public RulesetGroupType GetRulesetType()
	{
		return RulesetGroupType.Items;
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x00005553 File Offset: 0x00003753
	public Sprite GetRulesetGroupIcon()
	{
		return RulesetUIStyles.Item.elementIcon;
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x000053AE File Offset: 0x000035AE
	public List<RulesetElementDefinition> GetUIWindow(RulesetUIWindow window)
	{
		return null;
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x00036864 File Offset: 0x00034A64
	public void ShowUIWindow(string header, RulesetUIWindow window)
	{
		this.m_header = header;
		this.m_window = window;
		List<RulesetElementDefinition> list = new List<RulesetElementDefinition>();
		int num = 0;
		int num2 = 0;
		this.m_enabledDisabledElements[0] = "<color=#a2d66d>" + LocalizationManager.GetTranslation("Options_Enabled", true, 0, true, false, null, null, true) + "</color>";
		this.m_enabledDisabledElements[1] = "<color=#d17564>" + LocalizationManager.GetTranslation("Options_Disabled", true, 0, true, false, null, null, true) + "</color>";
		foreach (ItemDetails itemDetails in GameManager.ItemList.items)
		{
			if (this.m_itemsEnabledMap.ContainsKey(itemDetails.itemUniqueID) && num >= this.curPage * this.itemsPerPage)
			{
				int startIndex = this.m_itemsEnabledMap[itemDetails.itemUniqueID] ? 0 : 1;
				RulesetElementStyle rulesetElementStyle = RulesetUIStyles.Item.Clone();
				rulesetElementStyle.elementIcon = itemDetails.icon;
				bool isServer = NetSystem.IsServer;
				string translation = LocalizationManager.GetTranslation(itemDetails.itemNameToken, true, 0, true, false, null, null, true);
				list.Add(new RulesetListElement(translation, startIndex, this.m_enabledDisabledElements, itemDetails, new RulesetListValueChanged(this.OnItemEnabledChanged), rulesetElementStyle, isServer, new RulesetListAllowValueChange(this.AllowItemEnabledChange)));
				num2++;
			}
			num++;
			if (num2 >= this.itemsPerPage)
			{
				break;
			}
		}
		int num3 = 7 - list.Count;
		if (num3 > 0)
		{
			for (int j = 0; j < num3; j++)
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

	// Token: 0x060002BA RID: 698 RVA: 0x0000555F File Offset: 0x0000375F
	public bool IsItemEnabled(ItemDetails item)
	{
		if (this.m_itemsEnabledMap.ContainsKey(item.itemUniqueID))
		{
			return this.m_itemsEnabledMap[item.itemUniqueID];
		}
		Debug.LogError("IsItemEnabled - Item could not be found in ruleset group. this shouldn't happen");
		return false;
	}

	// Token: 0x060002BB RID: 699 RVA: 0x00005591 File Offset: 0x00003791
	private void OnNextPage(object obj)
	{
		if (this.curPage + 1 < this.GetMaxPages())
		{
			this.curPage++;
		}
		this.ShowUIWindow(this.m_header, this.m_window);
	}

	// Token: 0x060002BC RID: 700 RVA: 0x000055C3 File Offset: 0x000037C3
	private void OnPreviousPage(object obj)
	{
		if (this.curPage - 1 >= 0)
		{
			this.curPage--;
		}
		this.ShowUIWindow(this.m_header, this.m_window);
	}

	// Token: 0x060002BD RID: 701 RVA: 0x000055F0 File Offset: 0x000037F0
	private int GetMaxPages()
	{
		return this.m_itemsEnabledMap.Count / this.itemsPerPage + ((this.m_itemsEnabledMap.Count % this.itemsPerPage > 0) ? 1 : 0);
	}

	// Token: 0x060002BE RID: 702 RVA: 0x00036BB8 File Offset: 0x00034DB8
	private void OnItemEnabledChanged(int index, object obj)
	{
		ItemDetails itemDetails = (ItemDetails)obj;
		if (this.m_itemsEnabledMap.ContainsKey(itemDetails.itemUniqueID))
		{
			this.m_itemsEnabledMap[itemDetails.itemUniqueID] = (index == 0);
			GameManager.RulesetManager.RulesetDataChanged = true;
			return;
		}
		Debug.LogError("OnItemEnabledChanged - Item could not be found in ruleset group. this shouldn't happen");
	}

	// Token: 0x060002BF RID: 703 RVA: 0x0000561E File Offset: 0x0000381E
	private bool AllowItemEnabledChange(int index, object obj)
	{
		return ((ItemDetails)obj).weaponSpcaeItem || index != 0 || this.GetNumberOfActiveItems() > 1;
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x00036C10 File Offset: 0x00034E10
	private int GetNumberOfActiveItems()
	{
		int num = 0;
		foreach (KeyValuePair<ushort, bool> keyValuePair in this.m_itemsEnabledMap)
		{
			ItemDetails itemDetails;
			if (this.m_itemIDMap.TryGetValue(keyValuePair.Key, out itemDetails) && !itemDetails.weaponSpcaeItem && keyValuePair.Value)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x040002E1 RID: 737
	private Dictionary<ushort, bool> m_itemsEnabledMap = new Dictionary<ushort, bool>();

	// Token: 0x040002E2 RID: 738
	private Dictionary<ushort, ItemDetails> m_itemIDMap = new Dictionary<ushort, ItemDetails>();

	// Token: 0x040002E3 RID: 739
	private string[] m_enabledDisabledElements = new string[]
	{
		"<color=#a2d66d>Enabled</color>",
		"<color=#d17564>Disabled</color>"
	};

	// Token: 0x040002E4 RID: 740
	private RulesetUIWindow m_window;

	// Token: 0x040002E5 RID: 741
	private string m_header = "";

	// Token: 0x040002E6 RID: 742
	private int curPage;

	// Token: 0x040002E7 RID: 743
	private int itemsPerPage = 7;
}
