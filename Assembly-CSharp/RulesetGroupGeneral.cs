using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000080 RID: 128
public class RulesetGroupGeneral : GameRulesetGroup
{
	// Token: 0x17000063 RID: 99
	// (get) Token: 0x0600028A RID: 650 RVA: 0x000052FE File Offset: 0x000034FE
	public bool WeaponsCacheEnabled
	{
		get
		{
			return this.m_weaponsCacheEabled;
		}
	}

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x0600028B RID: 651 RVA: 0x00005306 File Offset: 0x00003506
	public int WeaponsCacheRespawnTurns
	{
		get
		{
			return (int)(this.m_weaponsCacheRespawnTurns + 1);
		}
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x0600028C RID: 652 RVA: 0x00005310 File Offset: 0x00003510
	public int MinDiceRoll
	{
		get
		{
			return (int)this.m_minDiceRoll;
		}
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x0600028D RID: 653 RVA: 0x00005318 File Offset: 0x00003518
	public int MaxDiceRoll
	{
		get
		{
			return (int)this.m_maxDiceRoll;
		}
	}

	// Token: 0x0600028F RID: 655 RVA: 0x00005320 File Offset: 0x00003520
	public void Serialize(ZPBitStream stream, ushort version)
	{
		stream.Write(this.m_weaponsCacheEabled);
		stream.Write(this.m_weaponsCacheRespawnTurns);
		if (version >= 2)
		{
			stream.Write(this.m_minDiceRoll);
			stream.Write(this.m_maxDiceRoll);
		}
	}

	// Token: 0x06000290 RID: 656 RVA: 0x00005356 File Offset: 0x00003556
	public void Deserialize(ZPBitStream stream, ushort version)
	{
		this.m_weaponsCacheEabled = stream.ReadBool();
		this.m_weaponsCacheRespawnTurns = stream.ReadByte();
		if (version >= 2)
		{
			this.m_minDiceRoll = stream.ReadByte();
			this.m_maxDiceRoll = stream.ReadByte();
		}
	}

	// Token: 0x06000291 RID: 657 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Apply()
	{
	}

	// Token: 0x06000292 RID: 658 RVA: 0x0000538C File Offset: 0x0000358C
	public string GetRulesetGroupName()
	{
		return LocalizationManager.GetTranslation("General", true, 0, true, false, null, null, true);
	}

	// Token: 0x06000293 RID: 659 RVA: 0x0000539F File Offset: 0x0000359F
	public RulesetGroupType GetRulesetType()
	{
		return RulesetGroupType.General;
	}

	// Token: 0x06000294 RID: 660 RVA: 0x000053A2 File Offset: 0x000035A2
	public Sprite GetRulesetGroupIcon()
	{
		return RulesetUIStyles.DefaultStyle.elementIcon;
	}

	// Token: 0x06000295 RID: 661 RVA: 0x000053AE File Offset: 0x000035AE
	public List<RulesetElementDefinition> GetUIWindow(RulesetUIWindow window)
	{
		return null;
	}

	// Token: 0x06000296 RID: 662 RVA: 0x00035DC0 File Offset: 0x00033FC0
	public void ShowUIWindow(string header, RulesetUIWindow window)
	{
		this.m_header = header;
		this.m_window = window;
		List<RulesetElementDefinition> list = new List<RulesetElementDefinition>();
		bool isServer = NetSystem.IsServer;
		string translation = LocalizationManager.GetTranslation("General_WeaponCacheEnabled", true, 0, true, false, null, null, true);
		string translation2 = LocalizationManager.GetTranslation("General_WeaponCacheRespawnTurns", true, 0, true, false, null, null, true);
		string translation3 = LocalizationManager.GetTranslation("General_MinDiceRoll", true, 0, true, false, null, null, true);
		string translation4 = LocalizationManager.GetTranslation("General_MaxDiceRoll", true, 0, true, false, null, null, true);
		string translation5 = LocalizationManager.GetTranslation("Back", true, 0, true, false, null, null, true);
		int startIndex = this.m_weaponsCacheEabled ? 0 : 1;
		list.Add(new RulesetNumericElement(translation3, RulesetUIStyles.DefaultStyle, (float)this.m_minDiceRoll, 0f, 32f, "N0", null, new RulesetNumericValueChanged(this.OnMinDiceRollChanged), 1f, true, new RulesetNumericAllowValueChange(this.AllowMinDiceRollChange)));
		list.Add(new RulesetNumericElement(translation4, RulesetUIStyles.DefaultStyle, (float)this.m_maxDiceRoll, 0f, 32f, "N0", null, new RulesetNumericValueChanged(this.OnMaxDiceRollChanged), 1f, true, new RulesetNumericAllowValueChange(this.AllowMaxDiceRollChange)));
		list.Add(new RulesetListElement(translation, startIndex, this.m_enabledDisabledElements, null, new RulesetListValueChanged(this.OnWeaponCacheEnabledChanged), RulesetUIStyles.DefaultStyle, isServer, null));
		list.Add(new RulesetListElement(translation2, (int)this.m_weaponsCacheRespawnTurns, this.m_weaponCacheRespawnElements, null, new RulesetListValueChanged(this.OnWeaponsCacheRespawnTurnsChanged), RulesetUIStyles.DefaultStyle, isServer, null));
		list.Add(new RulesetButtonElement(translation5, new RulesetEventDelegate(window.OnExitRulesetGroup), null, RulesetUIStyles.BackStyle, true, InputActions.Cancel));
		this.m_window.SetWindow(header, list);
	}

	// Token: 0x06000297 RID: 663 RVA: 0x000053B1 File Offset: 0x000035B1
	private void OnMinDiceRollChanged(float value, object obj)
	{
		if (value <= (float)this.m_maxDiceRoll)
		{
			this.m_minDiceRoll = (byte)value;
		}
	}

	// Token: 0x06000298 RID: 664 RVA: 0x000053C5 File Offset: 0x000035C5
	private bool AllowMinDiceRollChange(float value, object obj)
	{
		return value <= (float)this.m_maxDiceRoll;
	}

	// Token: 0x06000299 RID: 665 RVA: 0x000053D4 File Offset: 0x000035D4
	private void OnMaxDiceRollChanged(float value, object obj)
	{
		if (value >= (float)this.m_minDiceRoll)
		{
			this.m_maxDiceRoll = (byte)value;
		}
	}

	// Token: 0x0600029A RID: 666 RVA: 0x000053E8 File Offset: 0x000035E8
	private bool AllowMaxDiceRollChange(float value, object obj)
	{
		return value >= (float)this.m_minDiceRoll;
	}

	// Token: 0x0600029B RID: 667 RVA: 0x000053F7 File Offset: 0x000035F7
	private void OnWeaponCacheEnabledChanged(int index, object obj)
	{
		this.m_weaponsCacheEabled = (index == 0);
	}

	// Token: 0x0600029C RID: 668 RVA: 0x00005403 File Offset: 0x00003603
	private void OnWeaponsCacheRespawnTurnsChanged(int index, object obj)
	{
		this.m_weaponsCacheRespawnTurns = (byte)index;
	}

	// Token: 0x040002D1 RID: 721
	private bool m_weaponsCacheEabled = true;

	// Token: 0x040002D2 RID: 722
	private byte m_weaponsCacheRespawnTurns = 2;

	// Token: 0x040002D3 RID: 723
	private byte m_minDiceRoll = 2;

	// Token: 0x040002D4 RID: 724
	private byte m_maxDiceRoll = 9;

	// Token: 0x040002D5 RID: 725
	private string[] m_weaponCacheRespawnElements = new string[]
	{
		"1",
		"2",
		"3",
		"4",
		"5",
		"6",
		"7",
		"8"
	};

	// Token: 0x040002D6 RID: 726
	private string[] m_enabledDisabledElements = new string[]
	{
		"<color=#a2d66d>Enabled</color>",
		"<color=#d17564>Disabled</color>"
	};

	// Token: 0x040002D7 RID: 727
	private const float c_minAllowedDiceRoll = 0f;

	// Token: 0x040002D8 RID: 728
	private const float c_maxAllowedDiceRoll = 32f;

	// Token: 0x040002D9 RID: 729
	private RulesetUIWindow m_window;

	// Token: 0x040002DA RID: 730
	private string m_header = "";

	// Token: 0x040002DB RID: 731
	private int curPage;

	// Token: 0x040002DC RID: 732
	private int itemsPerPage = 7;
}
