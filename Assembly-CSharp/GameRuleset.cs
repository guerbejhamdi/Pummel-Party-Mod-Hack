using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using ZP.Utility;

// Token: 0x02000076 RID: 118
public class GameRuleset
{
	// Token: 0x17000055 RID: 85
	// (get) Token: 0x0600024C RID: 588 RVA: 0x000050D2 File Offset: 0x000032D2
	// (set) Token: 0x0600024D RID: 589 RVA: 0x000050F4 File Offset: 0x000032F4
	public string Name
	{
		get
		{
			if (this.m_isDefault)
			{
				return LocalizationManager.GetTranslation("Default", true, 0, true, false, null, null, true);
			}
			return this.m_rulesetName;
		}
		set
		{
			this.m_rulesetName = value;
		}
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x0600024E RID: 590 RVA: 0x000050FD File Offset: 0x000032FD
	// (set) Token: 0x0600024F RID: 591 RVA: 0x00005105 File Offset: 0x00003305
	public string HiddenName
	{
		get
		{
			return this.m_hiddenName;
		}
		set
		{
			this.m_hiddenName = value;
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x06000250 RID: 592 RVA: 0x0000510E File Offset: 0x0000330E
	public RulesetGroupGeneral General
	{
		get
		{
			return this.m_general;
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x06000251 RID: 593 RVA: 0x00005116 File Offset: 0x00003316
	public RulesetGroupMinigames Minigames
	{
		get
		{
			return this.m_minigames;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06000252 RID: 594 RVA: 0x0000511E File Offset: 0x0000331E
	public RulesetGroupItems Items
	{
		get
		{
			return this.m_items;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000253 RID: 595 RVA: 0x00005126 File Offset: 0x00003326
	public RulesetGroupGobletChallenges GobletChallenges
	{
		get
		{
			return this.m_gobletChallengeRules;
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x06000254 RID: 596 RVA: 0x0000512E File Offset: 0x0000332E
	public RulesetGroupModifiers Modifiers
	{
		get
		{
			return this.m_modifiers;
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x06000255 RID: 597 RVA: 0x00005136 File Offset: 0x00003336
	public bool IsDefault
	{
		get
		{
			return this.m_isDefault;
		}
	}

	// Token: 0x06000256 RID: 598 RVA: 0x000355C4 File Offset: 0x000337C4
	public GameRuleset(string name = "", bool isDefault = false)
	{
		this.m_rulesetName = name;
		this.m_hiddenName = name;
		this.m_isDefault = isDefault;
		this.m_general = new RulesetGroupGeneral();
		this.AddRulesetGroup(this.m_general);
		this.m_minigames = new RulesetGroupMinigames();
		this.AddRulesetGroup(this.m_minigames);
		this.m_items = new RulesetGroupItems();
		this.AddRulesetGroup(this.m_items);
		this.m_gobletChallengeRules = new RulesetGroupGobletChallenges();
		this.AddRulesetGroup(this.m_gobletChallengeRules);
		this.m_modifiers = new RulesetGroupModifiers();
		this.AddRulesetGroup(this.m_modifiers);
	}

	// Token: 0x06000257 RID: 599 RVA: 0x00035680 File Offset: 0x00033880
	public bool Load(byte[] data)
	{
		bool result;
		try
		{
			ZPBitStream zpbitStream = new ZPBitStream(data, 0, data.Length, data.Length * 8);
			if (zpbitStream.GetByteLength() <= 4)
			{
				result = false;
			}
			else if (zpbitStream.ReadUInt() != 1162630482U)
			{
				result = false;
			}
			else
			{
				ushort version = zpbitStream.ReadUShort();
				this.m_isDefault = zpbitStream.ReadBool();
				int num = (int)zpbitStream.ReadByte();
				for (int i = 0; i < num; i++)
				{
					RulesetGroupType type = (RulesetGroupType)zpbitStream.ReadByte();
					this.GetRulesetGroup(type).Deserialize(zpbitStream, version);
				}
				if (zpbitStream.ReadBool())
				{
					string rulesetName = zpbitStream.ReadUnicodeString();
					this.m_rulesetName = rulesetName;
				}
				result = true;
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
			result = false;
		}
		return result;
	}

	// Token: 0x06000258 RID: 600 RVA: 0x00035738 File Offset: 0x00033938
	public byte[] Save(bool disk)
	{
		byte[] result;
		try
		{
			ZPBitStream zpbitStream = new ZPBitStream();
			zpbitStream.Write(1162630482U);
			zpbitStream.Write(2);
			zpbitStream.Write(this.m_isDefault);
			zpbitStream.Write((byte)this.m_rulesetGroups.Count);
			foreach (GameRulesetGroup gameRulesetGroup in this.m_rulesetGroups)
			{
				zpbitStream.Write((byte)gameRulesetGroup.GetRulesetType());
				gameRulesetGroup.Serialize(zpbitStream, 2);
			}
			zpbitStream.Write(disk);
			if (disk)
			{
				zpbitStream.WriteUnicode(this.m_rulesetName);
			}
			result = zpbitStream.GetDataCopy();
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
			result = null;
		}
		return result;
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0000513E File Offset: 0x0000333E
	public int GetNumberRulesetGroups()
	{
		return this.m_rulesetGroups.Count;
	}

	// Token: 0x0600025A RID: 602 RVA: 0x0000514B File Offset: 0x0000334B
	private void AddRulesetGroup(GameRulesetGroup newGroup)
	{
		this.m_rulesetGroups.Add(newGroup);
	}

	// Token: 0x0600025B RID: 603 RVA: 0x0003580C File Offset: 0x00033A0C
	private GameRulesetGroup GetRulesetGroup(RulesetGroupType type)
	{
		foreach (GameRulesetGroup gameRulesetGroup in this.m_rulesetGroups)
		{
			if (gameRulesetGroup.GetRulesetType() == type)
			{
				return gameRulesetGroup;
			}
		}
		return null;
	}

	// Token: 0x0600025C RID: 604 RVA: 0x00005159 File Offset: 0x00003359
	public List<GameRulesetGroup> GetRulesetGroups()
	{
		return this.m_rulesetGroups;
	}

	// Token: 0x040002A6 RID: 678
	private string m_rulesetName = "Default Name";

	// Token: 0x040002A7 RID: 679
	private string m_hiddenName = "Ruleset";

	// Token: 0x040002A8 RID: 680
	private RulesetGroupGeneral m_general;

	// Token: 0x040002A9 RID: 681
	private RulesetGroupMinigames m_minigames;

	// Token: 0x040002AA RID: 682
	private RulesetGroupItems m_items;

	// Token: 0x040002AB RID: 683
	private RulesetGroupGobletChallenges m_gobletChallengeRules;

	// Token: 0x040002AC RID: 684
	private RulesetGroupModifiers m_modifiers;

	// Token: 0x040002AD RID: 685
	private bool m_isDefault;

	// Token: 0x040002AE RID: 686
	private const uint m_rulesetFileID = 1162630482U;

	// Token: 0x040002AF RID: 687
	private const ushort m_version = 2;

	// Token: 0x040002B0 RID: 688
	private List<GameRulesetGroup> m_rulesetGroups = new List<GameRulesetGroup>();
}
