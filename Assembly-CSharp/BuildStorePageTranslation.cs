using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000021 RID: 33
public class BuildStorePageTranslation : MonoBehaviour
{
	// Token: 0x0600009D RID: 157 RVA: 0x00003F32 File Offset: 0x00002132
	private void Start()
	{
		this.Translate();
	}

	// Token: 0x0600009E RID: 158 RVA: 0x0002DCF4 File Offset: 0x0002BEF4
	private void Update()
	{
		if (this.redo)
		{
			this.redo = false;
			LocalizationManager.CurrentLanguageCode = this.languageCode;
			this.Translate();
		}
		if (this.redoAchieve)
		{
			this.redoAchieve = false;
			for (int i = 0; i < 31; i++)
			{
				string translation = LocalizationManager.GetTranslation("NEW_ACHIEVEMENT_1_" + i.ToString() + "_NAME", true, 0, true, false, null, "English", true);
				string translation2 = LocalizationManager.GetTranslation("NEW_ACHIEVEMENT_1_" + i.ToString() + "_DESC", true, 0, true, false, null, "English", true);
				string text = LocalizationManager.GetTranslation("NEW_ACHIEVEMENT_1_" + i.ToString() + "_NAME", true, 0, true, false, null, this.languageCode, true);
				string text2 = LocalizationManager.GetTranslation("NEW_ACHIEVEMENT_1_" + i.ToString() + "_DESC", true, 0, true, false, null, this.languageCode, true);
				text = text.Replace("\"", "\\\"");
				text2 = text2.Replace("\"", "\\\"");
				this.achievementInput.text = this.achievementInput.text.Replace(translation, text);
				this.achievementInput.text = this.achievementInput.text.Replace(translation2, text2);
			}
		}
	}

	// Token: 0x0600009F RID: 159 RVA: 0x0002DE44 File Offset: 0x0002C044
	private void Translate()
	{
		string text = "";
		text = string.Concat(new string[]
		{
			text,
			"[b]",
			LocalizationManager.GetTranslation("StorePageShortDescription", true, 0, true, false, null, null, true),
			"[/b]",
			this.n,
			this.n
		});
		text = string.Concat(new string[]
		{
			text,
			"[h2]",
			LocalizationManager.GetTranslation("StorePageHeaderMultiplayer", true, 0, true, false, null, null, true),
			"[/h2]",
			this.n
		});
		text = string.Concat(new string[]
		{
			text,
			LocalizationManager.GetTranslation("StorePageDescriptionMultiplayer", true, 0, true, false, null, null, true),
			this.n,
			this.n,
			this.n
		});
		text = text + "[img]{STEAM_APP_IMAGE}/extras/TemporalTrails.png[/img]" + this.n + this.n;
		text = string.Concat(new string[]
		{
			text,
			"[h2]",
			LocalizationManager.GetTranslation("StorePageHeaderBoardMode", true, 0, true, false, null, null, true),
			"[/h2]",
			this.n
		});
		text = string.Concat(new string[]
		{
			text,
			LocalizationManager.GetTranslation("StorePageDescriptionBoardMode", true, 0, true, false, null, null, true),
			this.n,
			this.n,
			this.n
		});
		text = text + "[img]{STEAM_APP_IMAGE}/extras/TrailerSnippet.png[/img]" + this.n + this.n;
		text = string.Concat(new string[]
		{
			text,
			"[h2]",
			LocalizationManager.GetTranslation("StorePageHeaderItems", true, 0, true, false, null, null, true),
			"[/h2]",
			this.n
		});
		text = text + LocalizationManager.GetTranslation("StorePageDescriptionItems", true, 0, true, false, null, null, true) + this.n + this.n;
		text = string.Concat(new string[]
		{
			text,
			"[h2]",
			LocalizationManager.GetTranslation("StorePageHeaderMinigames", true, 0, true, false, null, null, true),
			"[/h2]",
			this.n
		});
		text = text + LocalizationManager.GetTranslation("StorePageDescriptionMinigames", true, 0, true, false, null, null, true) + this.n + this.n;
		text = string.Concat(new string[]
		{
			text,
			"[h2]",
			LocalizationManager.GetTranslation("StorePageHeaderMinigameMode", true, 0, true, false, null, null, true),
			"[/h2]",
			this.n
		});
		text = text + LocalizationManager.GetTranslation("StorePageDescriptionMinigameMode", true, 0, true, false, null, null, true) + this.n + this.n;
		text = string.Concat(new string[]
		{
			text,
			"[h2]",
			LocalizationManager.GetTranslation("StorePageHeaderBots", true, 0, true, false, null, null, true),
			"[/h2]",
			this.n
		});
		text += LocalizationManager.GetTranslation("StorePageDescriptionBots", true, 0, true, false, null, null, true);
		this.input.text = text;
	}

	// Token: 0x040000B1 RID: 177
	public InputField input;

	// Token: 0x040000B2 RID: 178
	public InputField achievementInput;

	// Token: 0x040000B3 RID: 179
	public bool redo;

	// Token: 0x040000B4 RID: 180
	public bool redoAchieve;

	// Token: 0x040000B5 RID: 181
	public string languageCode;

	// Token: 0x040000B6 RID: 182
	private string n = "\n";

	// Token: 0x040000B7 RID: 183
	private string b = "[b]";

	// Token: 0x040000B8 RID: 184
	private string be = "[/b]";
}
