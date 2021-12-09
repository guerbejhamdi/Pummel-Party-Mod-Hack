using System;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x02000426 RID: 1062
public class MinigameDefinition : ScriptableObject
{
	// Token: 0x06001D84 RID: 7556 RVA: 0x000C0AE0 File Offset: 0x000BECE0
	public int GetRandomAlternateIndex()
	{
		if (this.alternates == null || this.alternates.Length == 0)
		{
			return 0;
		}
		string key = "MINIGAME_CUR_ALTERNATE_" + this.minigameName;
		int num = this.alternates.Length + 1;
		int num2 = RBPrefs.GetInt(key, 0);
		num2++;
		if (num2 >= num)
		{
			num2 = 0;
		}
		RBPrefs.SetInt(key, num2);
		return num2;
	}

	// Token: 0x06001D85 RID: 7557 RVA: 0x00015C52 File Offset: 0x00013E52
	public bool GetIsActive()
	{
		return GameManager.RulesetManager.ActiveRuleset.Minigames.IsMinigameEnabled(this);
	}

	// Token: 0x06001D86 RID: 7558 RVA: 0x0000398C File Offset: 0x00001B8C
	public void SetIsActive(bool active)
	{
	}

	// Token: 0x04002017 RID: 8215
	public ushort minigameID;

	// Token: 0x04002018 RID: 8216
	public bool enabled = true;

	// Token: 0x04002019 RID: 8217
	public string minigameName = "DefaultMinigame";

	// Token: 0x0400201A RID: 8218
	public string minigameNameToken = "DefaultMinigame";

	// Token: 0x0400201B RID: 8219
	public string sceneName = "DefaultMinigame_Scene";

	// Token: 0x0400201C RID: 8220
	public string description;

	// Token: 0x0400201D RID: 8221
	public string descriptionToken = "DefaultMinigameDescription";

	// Token: 0x0400201E RID: 8222
	public VideoClip videoClip;

	// Token: 0x0400201F RID: 8223
	public string videoClipPath;

	// Token: 0x04002020 RID: 8224
	public GameObject minigameControllerPfb;

	// Token: 0x04002021 RID: 8225
	public string minigameControllerPfbPath;

	// Token: 0x04002022 RID: 8226
	public InputHelp inputHelp;

	// Token: 0x04002023 RID: 8227
	public bool controlsChanged;

	// Token: 0x04002024 RID: 8228
	public Sprite screenshot;

	// Token: 0x04002025 RID: 8229
	public int minPlayers = 2;

	// Token: 0x04002026 RID: 8230
	public int maxPlayers = 8;

	// Token: 0x04002027 RID: 8231
	public MinigameAlternate[] alternates;
}
