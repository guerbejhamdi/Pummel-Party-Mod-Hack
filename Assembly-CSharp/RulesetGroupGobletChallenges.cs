using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000081 RID: 129
public class RulesetGroupGobletChallenges : GameRulesetGroup
{
	// Token: 0x0600029D RID: 669 RVA: 0x00035F68 File Offset: 0x00034168
	public RulesetGroupGobletChallenges()
	{
		GobletChallenge gobletChallenge = new GobletChallenge();
		gobletChallenge.Event = StatChallengeBoardEvent.EndGame;
		gobletChallenge.Extent = StatChallengeExtent.Most;
		gobletChallenge.NumPlayers = 1;
		gobletChallenge.Stat = StatType.DamageDealt;
		this.m_challenges.Add(gobletChallenge);
		GobletChallenge gobletChallenge2 = new GobletChallenge();
		gobletChallenge2.Event = StatChallengeBoardEvent.EndGame;
		gobletChallenge2.Extent = StatChallengeExtent.Most;
		gobletChallenge2.NumPlayers = 1;
		gobletChallenge2.Stat = StatType.KeysGained;
		this.m_challenges.Add(gobletChallenge2);
		GobletChallenge gobletChallenge3 = new GobletChallenge();
		gobletChallenge3.Event = StatChallengeBoardEvent.EndGame;
		gobletChallenge3.Extent = StatChallengeExtent.Most;
		gobletChallenge3.NumPlayers = 1;
		gobletChallenge3.Stat = StatType.MinigamesWon;
		this.m_challenges.Add(gobletChallenge3);
	}

	// Token: 0x0600029E RID: 670 RVA: 0x00036068 File Offset: 0x00034268
	public void Serialize(ZPBitStream stream, ushort version)
	{
		stream.Write((ushort)this.m_challenges.Count);
		foreach (GobletChallenge challenge in this.m_challenges)
		{
			this.WriteGobletChallenge(stream, challenge);
		}
	}

	// Token: 0x0600029F RID: 671 RVA: 0x000360D0 File Offset: 0x000342D0
	public void Deserialize(ZPBitStream stream, ushort version)
	{
		this.m_challenges.Clear();
		ushort num = stream.ReadUShort();
		for (int i = 0; i < (int)num; i++)
		{
			this.m_challenges.Add(this.ReadGobletChallenge(stream));
		}
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Apply()
	{
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x00036110 File Offset: 0x00034310
	public List<GobletChallenge> GetChallenges(StatChallengeBoardEvent ev)
	{
		List<GobletChallenge> list = new List<GobletChallenge>();
		foreach (GobletChallenge gobletChallenge in this.m_challenges)
		{
			if (gobletChallenge.Event == ev)
			{
				list.Add(gobletChallenge);
			}
		}
		return list;
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x0000540D File Offset: 0x0000360D
	public string GetRulesetGroupName()
	{
		return LocalizationManager.GetTranslation("Challenge_GroupName", true, 0, true, false, null, null, true);
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x00005420 File Offset: 0x00003620
	public Sprite GetRulesetGroupIcon()
	{
		return Resources.Load<Sprite>("GobletIcon");
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x00036174 File Offset: 0x00034374
	public void ShowUIWindow(string header, RulesetUIWindow window)
	{
		this.m_header = header;
		this.m_window = window;
		List<RulesetElementDefinition> list = new List<RulesetElementDefinition>();
		string translation = LocalizationManager.GetTranslation("Challenge_Add", true, 0, true, false, null, null, true);
		string translation2 = LocalizationManager.GetTranslation("Back", true, 0, true, false, null, null, true);
		string translation3 = LocalizationManager.GetTranslation("Challenge", true, 0, true, false, null, null, true);
		int num = 1;
		foreach (GobletChallenge gobletChallenge in this.m_challenges)
		{
			string text = "";
			if (LocalizationManager.CurrentLanguage == "English")
			{
				text += ((gobletChallenge.NumPlayers == 1) ? (this.TextColor("Player", "#FFD390FF") + " with the ") : (this.TextColor(gobletChallenge.NumPlayers.ToString() + " Players", "#FFD390FF") + " with the "));
				if (gobletChallenge.Extent == StatChallengeExtent.Least)
				{
					text = text + this.TextColor(gobletChallenge.Extent.ToString(), "#F07F74FF") + " ";
				}
				else
				{
					text = text + this.TextColor(gobletChallenge.Extent.ToString(), "#C3EA67FF") + " ";
				}
				text = text + this.TextColor(PlayerStats.statNames[(int)gobletChallenge.Stat], "#A5DAFFFF") + " ";
				text = text + "at " + this.TextColor(gobletChallenge.Event.ToString(), "#D9B4FFFF");
			}
			else
			{
				text = translation3 + " " + num.ToString();
				num++;
			}
			list.Add(new RulesetButtonElement(text, new RulesetEventDelegate(this.OnSelectChallenge), gobletChallenge, RulesetUIStyles.DefaultChallenge, true, -1));
		}
		if (NetSystem.IsServer)
		{
			list.Add(new RulesetButtonElement(translation, new RulesetEventDelegate(this.OnAddChallenge), null, RulesetUIStyles.NewChallenge, true, -1));
		}
		list.Add(new RulesetButtonElement(translation2, new RulesetEventDelegate(window.OnExitRulesetGroup), null, RulesetUIStyles.BackStyle, true, InputActions.Cancel));
		this.m_window.SetWindow(this.m_header, list);
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x000363F4 File Offset: 0x000345F4
	private void OnAddChallenge(object obj)
	{
		if (this.m_challenges.Count <= 7)
		{
			GobletChallenge gobletChallenge = new GobletChallenge();
			gobletChallenge.Event = StatChallengeBoardEvent.EndGame;
			gobletChallenge.Extent = StatChallengeExtent.Most;
			gobletChallenge.NumPlayers = 1;
			gobletChallenge.Stat = StatType.KeysGained;
			this.m_challenges.Add(gobletChallenge);
			this.ShowUIWindow(this.m_header, this.m_window);
		}
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x00036450 File Offset: 0x00034650
	private void OnSelectChallenge(object obj)
	{
		GobletChallenge gobletChallenge = (GobletChallenge)obj;
		List<RulesetElementDefinition> list = new List<RulesetElementDefinition>();
		bool isServer = NetSystem.IsServer;
		string translation = LocalizationManager.GetTranslation("Challenge_Edit", true, 0, true, false, null, null, true);
		string translation2 = LocalizationManager.GetTranslation("Challenge_NumPlayers", true, 0, true, false, null, null, true);
		string translation3 = LocalizationManager.GetTranslation("Challenge_Extents", true, 0, true, false, null, null, true);
		string translation4 = LocalizationManager.GetTranslation("Challenge_Statistic", true, 0, true, false, null, null, true);
		string translation5 = LocalizationManager.GetTranslation("Challenge_EndEvent", true, 0, true, false, null, null, true);
		string translation6 = LocalizationManager.GetTranslation("Back", true, 0, true, false, null, null, true);
		string translation7 = LocalizationManager.GetTranslation("Delete", true, 0, true, false, null, null, true);
		string[] names = Enum.GetNames(typeof(StatChallengeExtent));
		for (int i = 0; i < names.Length; i++)
		{
			names[i] = LocalizationManager.GetTranslation("Challenge_Order_" + names[i], true, 0, true, false, null, null, true);
		}
		string[] names2 = Enum.GetNames(typeof(StatType));
		for (int j = 0; j < names2.Length; j++)
		{
			names2[j] = LocalizationManager.GetTranslation("Challenge_Statistic_" + names2[j], true, 0, true, false, null, null, true);
		}
		string[] names3 = Enum.GetNames(typeof(StatChallengeBoardEvent));
		for (int k = 0; k < names3.Length; k++)
		{
			names3[k] = LocalizationManager.GetTranslation("Challenge_Event_" + names3[k], true, 0, true, false, null, null, true);
			Debug.LogError("Challenge_Event_" + names3[k]);
		}
		list.Add(new RulesetListElement(translation2, gobletChallenge.NumPlayers - 1, this.m_numPlayersElements, gobletChallenge, new RulesetListValueChanged(this.OnNumPlayersChanged), RulesetUIStyles.DefaultStyle, isServer, null));
		list.Add(new RulesetListElement(translation3, (int)gobletChallenge.Extent, names, gobletChallenge, new RulesetListValueChanged(this.OnExtentChanged), RulesetUIStyles.DefaultStyle, isServer, null));
		list.Add(new RulesetListElement(translation4, (int)gobletChallenge.Stat, names2, gobletChallenge, new RulesetListValueChanged(this.OnStatChanged), RulesetUIStyles.DefaultStyle, isServer, null));
		list.Add(new RulesetListElement(translation5, (int)gobletChallenge.Event, names3, gobletChallenge, new RulesetListValueChanged(this.OnEventChanged), RulesetUIStyles.DefaultStyle, isServer, null));
		if (NetSystem.IsServer)
		{
			list.Add(new RulesetButtonElement(translation7, new RulesetEventDelegate(this.OnDeleteChallenge), gobletChallenge, RulesetUIStyles.DeleteRulesetStyle, true, -1));
		}
		list.Add(new RulesetButtonElement(translation6, new RulesetEventDelegate(this.OnExitChallenge), null, RulesetUIStyles.BackStyle, true, InputActions.Cancel));
		this.m_window.SetWindow(translation, list);
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x0000542C File Offset: 0x0000362C
	private void OnNumPlayersChanged(int index, object obj)
	{
		((GobletChallenge)obj).NumPlayers = index + 1;
		GameManager.RulesetManager.RulesetDataChanged = true;
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x00005447 File Offset: 0x00003647
	private void OnExtentChanged(int index, object obj)
	{
		((GobletChallenge)obj).Extent = (StatChallengeExtent)index;
		GameManager.RulesetManager.RulesetDataChanged = true;
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x00005460 File Offset: 0x00003660
	private void OnStatChanged(int index, object obj)
	{
		((GobletChallenge)obj).Stat = (StatType)index;
		GameManager.RulesetManager.RulesetDataChanged = true;
	}

	// Token: 0x060002AA RID: 682 RVA: 0x00005479 File Offset: 0x00003679
	private void OnEventChanged(int index, object obj)
	{
		((GobletChallenge)obj).Event = (StatChallengeBoardEvent)index;
		GameManager.RulesetManager.RulesetDataChanged = true;
	}

	// Token: 0x060002AB RID: 683 RVA: 0x000366D8 File Offset: 0x000348D8
	private void OnDeleteChallenge(object obj)
	{
		GobletChallenge item = (GobletChallenge)obj;
		this.m_challenges.Remove(item);
		this.m_window.OnSelectRulesetGroup(this);
	}

	// Token: 0x060002AC RID: 684 RVA: 0x00005492 File Offset: 0x00003692
	private void OnExitChallenge(object obj)
	{
		this.m_window.OnSelectRulesetGroup(this);
	}

	// Token: 0x060002AD RID: 685 RVA: 0x000054A0 File Offset: 0x000036A0
	private string TextColor(string text, string color)
	{
		return string.Concat(new string[]
		{
			"<color=",
			color,
			">",
			text,
			"</color>"
		});
	}

	// Token: 0x060002AE RID: 686 RVA: 0x000054CD File Offset: 0x000036CD
	public RulesetGroupType GetRulesetType()
	{
		return RulesetGroupType.GobletChallenges;
	}

	// Token: 0x060002AF RID: 687 RVA: 0x000054D0 File Offset: 0x000036D0
	private void WriteGobletChallenge(ZPBitStream stream, GobletChallenge challenge)
	{
		stream.Write((byte)challenge.Event);
		stream.Write((byte)challenge.Extent);
		stream.Write((byte)challenge.NumPlayers);
		stream.Write((byte)challenge.Stat);
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x00005506 File Offset: 0x00003706
	private GobletChallenge ReadGobletChallenge(ZPBitStream stream)
	{
		return new GobletChallenge
		{
			Event = (StatChallengeBoardEvent)stream.ReadByte(),
			Extent = (StatChallengeExtent)stream.ReadByte(),
			NumPlayers = (int)stream.ReadByte(),
			Stat = (StatType)stream.ReadByte()
		};
	}

	// Token: 0x040002DD RID: 733
	private List<GobletChallenge> m_challenges = new List<GobletChallenge>();

	// Token: 0x040002DE RID: 734
	private string[] m_numPlayersElements = new string[]
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

	// Token: 0x040002DF RID: 735
	private string m_header = "";

	// Token: 0x040002E0 RID: 736
	private RulesetUIWindow m_window;
}
