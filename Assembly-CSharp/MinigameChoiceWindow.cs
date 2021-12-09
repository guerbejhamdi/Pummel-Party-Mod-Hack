using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using ZP.Net;

// Token: 0x0200051B RID: 1307
public class MinigameChoiceWindow : MonoBehaviour
{
	// Token: 0x0600221A RID: 8730 RVA: 0x000D12DC File Offset: 0x000CF4DC
	public void Awake()
	{
		List<MinigameDefinition> minigameList = GameManager.GetMinigameList();
		for (int i = 0; i < minigameList.Count + 1; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.minigame_choice_btn_pfb);
			gameObject.transform.SetParent(base.transform, false);
			MinigameChoiceButton component = gameObject.GetComponent<MinigameChoiceButton>();
			if (i == minigameList.Count)
			{
				component.Initialize(this, "Skip");
			}
			else
			{
				component.Initialize(this, LocalizationManager.GetTranslation(minigameList[i].minigameNameToken, true, 0, true, false, null, null, true));
			}
		}
		if (!NetSystem.IsServer)
		{
			this.client_text.SetActive(true);
		}
	}

	// Token: 0x0600221B RID: 8731 RVA: 0x00018B70 File Offset: 0x00016D70
	public void ChooseMinigame(string minigame_name)
	{
		this.minigame_choice = minigame_name;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04002532 RID: 9522
	public GameObject minigame_choice_btn_pfb;

	// Token: 0x04002533 RID: 9523
	public GameObject client_text;

	// Token: 0x04002534 RID: 9524
	public string minigame_choice = "";
}
