using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZP.Net;

// Token: 0x02000079 RID: 121
public class GameRulesetManager
{
	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06000264 RID: 612 RVA: 0x00005161 File Offset: 0x00003361
	// (set) Token: 0x06000265 RID: 613 RVA: 0x00005169 File Offset: 0x00003369
	public bool RulesetDataChanged { get; set; }

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000266 RID: 614 RVA: 0x00005172 File Offset: 0x00003372
	public GameRuleset ActiveRuleset
	{
		get
		{
			if (!NetSystem.IsConnected || NetSystem.IsServer)
			{
				return this.m_activeRuleset;
			}
			return this.m_hostRuleset;
		}
	}

	// Token: 0x06000268 RID: 616 RVA: 0x000051BE File Offset: 0x000033BE
	public List<GameRuleset> GetRulesets()
	{
		return this.m_rulesets;
	}

	// Token: 0x06000269 RID: 617 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Initialize()
	{
	}

	// Token: 0x0600026A RID: 618 RVA: 0x000051C6 File Offset: 0x000033C6
	public void OnStorageLoaded()
	{
		this.Load();
	}

	// Token: 0x0600026B RID: 619 RVA: 0x00035868 File Offset: 0x00033A68
	public void Load()
	{
		try
		{
			string rulesetSaveDirectory = this.GetRulesetSaveDirectory();
			this.m_rulesets.Clear();
			foreach (string path in Directory.GetFiles(rulesetSaveDirectory, "*.rls", SearchOption.TopDirectoryOnly))
			{
				byte[] data = File.ReadAllBytes(path);
				GameRuleset gameRuleset = new GameRuleset("", false);
				if (gameRuleset.Load(data))
				{
					gameRuleset.HiddenName = Path.GetFileNameWithoutExtension(path);
					this.m_rulesets.Add(gameRuleset);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
		this.SetupRulesets();
	}

	// Token: 0x0600026C RID: 620 RVA: 0x00035900 File Offset: 0x00033B00
	private void SetupRulesets()
	{
		bool flag = false;
		using (List<GameRuleset>.Enumerator enumerator = this.m_rulesets.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.IsDefault)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			this.m_rulesets.Insert(0, new GameRuleset("Default Ruleset", true));
		}
		string @string = RBPrefs.GetString("_RULESET_ACTIVE_NAME", "");
		if (@string == "")
		{
			this.SetActiveRuleset(this.m_rulesets[0]);
			return;
		}
		bool flag2 = false;
		for (int i = 0; i < this.m_rulesets.Count; i++)
		{
			if (this.m_rulesets[i].Name == @string)
			{
				this.SetActiveRuleset(this.m_rulesets[i]);
				flag2 = true;
				break;
			}
		}
		if (!flag2)
		{
			this.SetActiveRuleset(this.m_rulesets[0]);
		}
	}

	// Token: 0x0600026D RID: 621 RVA: 0x00035A04 File Offset: 0x00033C04
	public void Save()
	{
		try
		{
			string rulesetSaveDirectory = this.GetRulesetSaveDirectory();
			foreach (GameRuleset gameRuleset in this.m_rulesets)
			{
				byte[] array = gameRuleset.Save(true);
				if (array != null)
				{
					File.WriteAllBytes(rulesetSaveDirectory + gameRuleset.HiddenName + ".rls", array);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
		if (this.m_activeRuleset != null)
		{
			RBPrefs.SetString("_RULESET_ACTIVE_NAME", this.m_activeRuleset.Name);
		}
	}

	// Token: 0x0600026E RID: 622 RVA: 0x00035AB0 File Offset: 0x00033CB0
	public GameRuleset NewRuleset()
	{
		if (this.m_rulesets.Count >= 9)
		{
			return null;
		}
		string text = "DEFAULT";
		for (int i = 1; i < 2048; i++)
		{
			text = "New Ruleset " + i.ToString();
			bool flag = false;
			using (List<GameRuleset>.Enumerator enumerator = this.m_rulesets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.HiddenName == text)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				break;
			}
		}
		GameRuleset gameRuleset = new GameRuleset(text, false);
		this.m_rulesets.Add(gameRuleset);
		return gameRuleset;
	}

	// Token: 0x0600026F RID: 623 RVA: 0x000051CE File Offset: 0x000033CE
	public void SetActiveRuleset(GameRuleset ruleset)
	{
		this.m_activeRuleset = ruleset;
		if (this.m_activeRuleset != null)
		{
			RBPrefs.SetString("_RULESET_ACTIVE_NAME", this.m_activeRuleset.Name);
		}
		GameManager.UpdatePsuedoRandomMinigameList();
		GameManager.RulesetManager.RulesetDataChanged = true;
	}

	// Token: 0x06000270 RID: 624 RVA: 0x00005204 File Offset: 0x00003404
	public void SetHostRuleset(byte[] rulesetData)
	{
		this.m_hostRuleset.Load(rulesetData);
		GameManager.UpdatePsuedoRandomMinigameList();
	}

	// Token: 0x06000271 RID: 625 RVA: 0x00035B60 File Offset: 0x00033D60
	public void RenameRuleset(GameRuleset ruleset, string name)
	{
		if (ruleset == null || ruleset.IsDefault)
		{
			return;
		}
		ruleset.Name = name;
		try
		{
			string rulesetSaveDirectory = this.GetRulesetSaveDirectory();
			byte[] array = ruleset.Save(true);
			if (array != null)
			{
				File.WriteAllBytes(rulesetSaveDirectory + ruleset.HiddenName + ".rls", array);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
	}

	// Token: 0x06000272 RID: 626 RVA: 0x00035BC8 File Offset: 0x00033DC8
	public void DeleteRuleset(GameRuleset ruleset)
	{
		if (ruleset.IsDefault)
		{
			return;
		}
		if (this.m_activeRuleset == ruleset)
		{
			this.m_activeRuleset = this.m_rulesets[0];
		}
		for (int i = this.m_rulesets.Count - 1; i >= 0; i--)
		{
			if (this.m_rulesets[i] == ruleset)
			{
				this.m_rulesets.RemoveAt(i);
				break;
			}
		}
		string path = this.GetRulesetSaveDirectory() + ruleset.Name + ".rls";
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}

	// Token: 0x06000273 RID: 627 RVA: 0x00035C54 File Offset: 0x00033E54
	private string GetRulesetSaveDirectory()
	{
		string text = Application.persistentDataPath + "/Rulesets/";
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		return text;
	}

	// Token: 0x040002B7 RID: 695
	private List<GameRuleset> m_rulesets = new List<GameRuleset>();

	// Token: 0x040002B8 RID: 696
	private GameRuleset m_activeRuleset;

	// Token: 0x040002B9 RID: 697
	private string m_activeRulesetName = "";

	// Token: 0x040002BA RID: 698
	private GameRuleset m_hostRuleset = new GameRuleset("", false);

	// Token: 0x040002BB RID: 699
	private const string c_activeRulesetKey = "_RULESET_ACTIVE_NAME";

	// Token: 0x040002BC RID: 700
	private const ushort c_consoleRulesetVersion = 32;
}
