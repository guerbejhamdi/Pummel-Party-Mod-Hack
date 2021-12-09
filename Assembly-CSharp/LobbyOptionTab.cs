using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200050A RID: 1290
public class LobbyOptionTab : MonoBehaviour
{
	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x060021BF RID: 8639 RVA: 0x000187E1 File Offset: 0x000169E1
	// (set) Token: 0x060021C0 RID: 8640 RVA: 0x000187E9 File Offset: 0x000169E9
	public int curIndex { get; set; }

	// Token: 0x060021C1 RID: 8641 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x060021C2 RID: 8642 RVA: 0x000CFD28 File Offset: 0x000CDF28
	private void Initialize()
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
		this.tabBar = base.gameObject.GetComponent<TabBar>();
		switch (this.lobbyOption)
		{
		case LobbyOption.GameMode:
			this.options = GameManager.GameModeStrings;
			return;
		case LobbyOption.Map:
		{
			MapDetails[] maps = GameManager.GetMaps();
			this.options = new string[maps.Length];
			for (int i = 0; i < maps.Length; i++)
			{
				this.options[i] = maps[i].name;
			}
			return;
		}
		case LobbyOption.TurnCount:
			this.options = GameManager.TurnCountStrings;
			return;
		case LobbyOption.WinningRelics:
			this.options = GameManager.WinningRelicsStrings;
			return;
		case LobbyOption.MaxTurnLength:
			this.options = GameManager.MaxTurnLengthStrings;
			return;
		case LobbyOption.MinigameCount:
			this.options = GameManager.MaxMinigameCountStrings;
			return;
		case LobbyOption.MaxPlayers:
			this.options = GameManager.MaxPlayers;
			return;
		default:
			return;
		}
	}

	// Token: 0x060021C3 RID: 8643 RVA: 0x000CFDFC File Offset: 0x000CDFFC
	public void IncrementOption(bool right)
	{
		int num = this.curIndex;
		if (!right && num == 0)
		{
			num = (int)((ushort)(this.options.Length - 1));
		}
		else if (right && num == this.options.Length - 1)
		{
			num = 0;
		}
		else if (right)
		{
			num++;
		}
		else
		{
			num--;
		}
		this.curIndex = num;
		GameManager.LobbyController.SetLobbyOption(this.lobbyOption, this.curIndex);
		this.UpdateVisual();
	}

	// Token: 0x060021C4 RID: 8644 RVA: 0x000CFE68 File Offset: 0x000CE068
	private void UpdateVisual()
	{
		if (this.curIndex >= this.options.Length)
		{
			this.curIndex = 0;
		}
		int num = 0;
		if (int.TryParse(this.options[this.curIndex], out num))
		{
			this.text.text = this.options[this.curIndex];
			return;
		}
		this.localizeText.SetTerm(this.options[this.curIndex]);
	}

	// Token: 0x060021C5 RID: 8645 RVA: 0x000187F2 File Offset: 0x000169F2
	public void SetOption(int index)
	{
		this.Initialize();
		this.curIndex = index;
		this.UpdateVisual();
	}

	// Token: 0x060021C6 RID: 8646 RVA: 0x00018807 File Offset: 0x00016A07
	public void SetInteractable(bool state)
	{
		this.Initialize();
		this.tabBar.SetInteractable(state);
	}

	// Token: 0x0400248C RID: 9356
	public LobbyOption lobbyOption;

	// Token: 0x0400248E RID: 9358
	public Text text;

	// Token: 0x0400248F RID: 9359
	public Localize localizeText;

	// Token: 0x04002490 RID: 9360
	private string[] options;

	// Token: 0x04002491 RID: 9361
	private bool initialized;

	// Token: 0x04002492 RID: 9362
	private TabBar tabBar;
}
