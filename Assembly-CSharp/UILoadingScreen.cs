using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200054D RID: 1357
public class UILoadingScreen : MonoBehaviour
{
	// Token: 0x060023D1 RID: 9169 RVA: 0x000D8524 File Offset: 0x000D6724
	public void Awake()
	{
		string translation = LocalizationManager.GetTranslation("Loading Players", true, 0, true, false, null, null, true);
		this.player_count_txt = base.transform.Find("PlayerCountTxt").GetComponent<Text>();
		this.player_count_txt.text = translation + " : 1 / 4";
	}

	// Token: 0x060023D2 RID: 9170 RVA: 0x000D8574 File Offset: 0x000D6774
	public void SetPlayerCount(int loaded, int total)
	{
		string translation = LocalizationManager.GetTranslation("Loading Players", true, 0, true, false, null, null, true);
		this.player_count_txt.text = string.Concat(new string[]
		{
			translation,
			" : ",
			loaded.ToString(),
			" / ",
			total.ToString()
		});
	}

	// Token: 0x040026CD RID: 9933
	private Text player_count_txt;
}
