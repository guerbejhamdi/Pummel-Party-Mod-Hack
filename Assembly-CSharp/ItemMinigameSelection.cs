using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020000BF RID: 191
public class ItemMinigameSelection : MonoBehaviour
{
	// Token: 0x060003E0 RID: 992 RVA: 0x0003B748 File Offset: 0x00039948
	public void Show(PresentItem item, List<MinigameDefinition> minigames, GamePlayer player)
	{
		this.window.SetState(MainMenuWindowState.Visible);
		this.item = item;
		this.eventSystemGroup.EventSystemID = (int)player.LocalIDAndAI;
		this.possibleMinigames = minigames;
		for (int i = 0; i < this.possibleMinigames.Count; i++)
		{
			this.minigameButton[i].SetMinigame(this.possibleMinigames[i]);
		}
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x00006255 File Offset: 0x00004455
	public void Hide()
	{
		this.window.SetState(MainMenuWindowState.Hidden);
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x00006263 File Offset: 0x00004463
	public void OnToggle(int id)
	{
		this.item.SelectMinigame((byte)id);
	}

	// Token: 0x04000442 RID: 1090
	public MainMenuWindow window;

	// Token: 0x04000443 RID: 1091
	public UIMinigameButton[] minigameButton;

	// Token: 0x04000444 RID: 1092
	public EventSystemGroup eventSystemGroup;

	// Token: 0x04000445 RID: 1093
	private List<MinigameDefinition> possibleMinigames = new List<MinigameDefinition>();

	// Token: 0x04000446 RID: 1094
	private PresentItem item;
}
