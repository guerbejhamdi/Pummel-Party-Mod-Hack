using System;
using I2.Loc;
using TMPro;
using UnityEngine;

// Token: 0x020004E8 RID: 1256
public class TwitchMapTextSetter : MonoBehaviour
{
	// Token: 0x06002122 RID: 8482 RVA: 0x000CDC20 File Offset: 0x000CBE20
	private void Start()
	{
		string text = LocalizationManager.GetTranslation("TwitchMapEventPrompt", true, 0, true, false, null, null, true);
		text = text.Replace("%Number%", "4");
		text = text.Replace("%Number2%", "3");
		this.text.text = text;
	}

	// Token: 0x040023D2 RID: 9170
	public TextMeshPro text;
}
