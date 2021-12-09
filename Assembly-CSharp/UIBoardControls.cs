using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

// Token: 0x02000530 RID: 1328
public class UIBoardControls : MonoBehaviour
{
	// Token: 0x060022F3 RID: 8947 RVA: 0x000D4CB4 File Offset: 0x000D2EB4
	public void Awake()
	{
		this.inputPanels = new UIInputPanel[6];
		float num = -222f;
		float num2 = 53f;
		for (int i = 0; i < this.inputPanels.Length; i++)
		{
			UIInputPanel component = UnityEngine.Object.Instantiate<GameObject>(this.input_panel_pfb).GetComponent<UIInputPanel>();
			component.transform.SetParent(base.transform, false);
			component.GetComponent<RectTransform>().anchoredPosition = new Vector3(138f, num + num2 * (float)i, 0f);
			component.gameObject.SetActive(false);
			this.inputPanels[i] = component;
		}
		this.inputHelps[0] = new InputHelp(new InputDetails[]
		{
			new InputDetails(InputActions.Accept, "Hit Dice", false, Pole.Positive, ControllerType.Keyboard, InputDetailsPriority.Normal)
		});
		this.inputHelps[1] = new InputHelp(new InputDetails[]
		{
			new InputDetails(InputActions.Action1, "Use Item", false, Pole.Positive, ControllerType.Keyboard, InputDetailsPriority.Large),
			new InputDetails(InputActions.Accept, "Hit Dice", false, Pole.Positive, ControllerType.Keyboard, InputDetailsPriority.Normal),
			new InputDetails(InputActions.Action2, "View Map", false, Pole.Positive, ControllerType.Keyboard, InputDetailsPriority.Normal)
		});
		this.inputHelps[2] = new InputHelp(new InputDetails[]
		{
			new InputDetails(InputActions.Accept, "Hit Dice", false, Pole.Positive, ControllerType.Keyboard, InputDetailsPriority.Normal),
			new InputDetails(InputActions.Action2, "View Map", false, Pole.Positive, ControllerType.Keyboard, InputDetailsPriority.Normal)
		});
		this.inputHelps[3] = new InputHelp(new InputDetails[]
		{
			new InputDetails(InputActions.Action1, "Use Item", false, Pole.Positive, ControllerType.Keyboard, InputDetailsPriority.Disabled),
			new InputDetails(InputActions.Accept, "Hit Dice", false, Pole.Positive, ControllerType.Keyboard, InputDetailsPriority.Normal),
			new InputDetails(InputActions.Action2, "View Map", false, Pole.Positive, ControllerType.Keyboard, InputDetailsPriority.Normal)
		});
		this.inputHelps[4] = new InputHelp(new InputDetails[]
		{
			new InputDetails(InputActions.Vertical, "Up", true, Pole.Positive, ControllerType.Keyboard, InputDetailsPriority.Normal),
			new InputDetails(InputActions.Horizontal, "Left", true, Pole.Negative, ControllerType.Keyboard, InputDetailsPriority.Normal),
			new InputDetails(InputActions.Vertical, "Down", true, Pole.Negative, ControllerType.Keyboard, InputDetailsPriority.Normal),
			new InputDetails(InputActions.Horizontal, "Right", true, Pole.Positive, ControllerType.Keyboard, InputDetailsPriority.Normal),
			new InputDetails(InputActions.Action2, "Exit Map", false, Pole.Positive, ControllerType.Keyboard, InputDetailsPriority.Normal)
		}, new InputDetails[]
		{
			new InputDetails(InputActions.Vertical, "UpDown", false, Pole.Positive, ControllerType.Keyboard, InputDetailsPriority.Normal),
			new InputDetails(InputActions.Horizontal, "LeftRight", false, Pole.Positive, ControllerType.Keyboard, InputDetailsPriority.Normal),
			new InputDetails(InputActions.Action2, "Exit Map", false, Pole.Positive, ControllerType.Keyboard, InputDetailsPriority.Normal)
		});
		this.inputHelps[5] = new InputHelp(new InputDetails[]
		{
			new InputDetails(InputActions.Cancel, "Close Inventory", false, Pole.Positive, ControllerType.Keyboard, InputDetailsPriority.Normal)
		}, new InputDetails[]
		{
			new InputDetails(InputActions.Accept, "Equip Selected Item", false, Pole.Positive, ControllerType.Keyboard, InputDetailsPriority.Normal),
			new InputDetails(InputActions.Cancel, "Close Inventory", false, Pole.Positive, ControllerType.Keyboard, InputDetailsPriority.Normal)
		});
		this.SetInputHelp(BoardInputType.RollTurnOrder);
	}

	// Token: 0x060022F4 RID: 8948 RVA: 0x000D4FD8 File Offset: 0x000D31D8
	public void SetPlayer(BoardPlayer _player)
	{
		if (this.lastHelper != null)
		{
			this.lastHelper.RemoveLastActiveControllerChangedDelegate(new PlayerActiveControllerChangedDelegate(this.OnControllerChanged));
		}
		this.player = _player;
		if (!this.player.GamePlayer.IsLocalPlayer || this.player.GamePlayer.IsAI)
		{
			this.HideWindow();
			return;
		}
		this.ShowWindow();
		this.lastHelper = this.player.GamePlayer.RewiredPlayer.controllers;
		this.lastHelper.AddLastActiveControllerChangedDelegate(new PlayerActiveControllerChangedDelegate(this.OnControllerChanged));
		for (int i = 0; i < this.inputPanels.Length; i++)
		{
			this.inputPanels[i].glyph.SetPlayer(this.player.GamePlayer.RewiredPlayer);
		}
	}

	// Token: 0x060022F5 RID: 8949 RVA: 0x00019398 File Offset: 0x00017598
	public void OnControllerChanged(Player player, Controller controller)
	{
		this.UpdateInputHelp();
	}

	// Token: 0x060022F6 RID: 8950 RVA: 0x000193A0 File Offset: 0x000175A0
	public void SetInputHelp(InputHelp inputHelp)
	{
		if (!this.player.GamePlayer.IsLocalPlayer || this.player.GamePlayer.IsAI)
		{
			return;
		}
		this.curInputHelp = inputHelp;
		this.UpdateInputHelp();
	}

	// Token: 0x060022F7 RID: 8951 RVA: 0x000D50A4 File Offset: 0x000D32A4
	public void SetInputHelp(BoardInputType inputType)
	{
		if ((this.player == null || !this.player.GamePlayer.IsLocalPlayer || this.player.GamePlayer.IsAI) && inputType != BoardInputType.RollTurnOrder)
		{
			return;
		}
		if (inputType == BoardInputType.RollTurnOrder)
		{
			UIInputPanel[] array = this.inputPanels;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(false);
			}
			List<GamePlayer> playerList = GameManager.PlayerList;
			int num = 0;
			for (int j = 0; j < playerList.Count; j++)
			{
				if (playerList[j].IsLocalPlayer && !playerList[j].IsAI)
				{
					this.inputPanels[num].glyph.SetPlayer(playerList[j].RewiredPlayer);
					this.inputPanels[num].SetInput(new InputDetails(InputActions.Accept, "Hit Dice", false, Pole.Positive, ControllerType.Keyboard, InputDetailsPriority.Normal), playerList[j].Name + " ");
					num++;
				}
			}
			return;
		}
		if (this.player == null || !this.player.GamePlayer.IsLocalPlayer)
		{
			return;
		}
		this.curInputHelp = this.inputHelps[(int)inputType];
		this.UpdateInputHelp();
	}

	// Token: 0x060022F8 RID: 8952 RVA: 0x000D51E0 File Offset: 0x000D33E0
	public void UpdateInputHelp()
	{
		ControllerType controllerType = ControllerType.Joystick;
		if (this.player != null)
		{
			Controller lastActiveController = this.player.GamePlayer.RewiredPlayer.controllers.GetLastActiveController();
			controllerType = ((lastActiveController != null) ? lastActiveController.type : ((this.player.GamePlayer.RewiredPlayer.id == 0) ? ControllerType.Mouse : ControllerType.Joystick));
		}
		if (this.curInputHelp.seperateControllerActions && controllerType == ControllerType.Joystick)
		{
			this.SetInputTypes(this.curInputHelp.controller);
			return;
		}
		this.SetInputTypes(this.curInputHelp.keyboard);
	}

	// Token: 0x060022F9 RID: 8953 RVA: 0x000D5274 File Offset: 0x000D3474
	public void SetInputTypes(InputDetails[] inputDetails)
	{
		UIInputPanel[] array = this.inputPanels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < inputDetails.Length; j++)
		{
			int num = inputDetails.Length - j - 1;
			this.inputPanels[num].SetInput(inputDetails[j], "");
		}
	}

	// Token: 0x060022FA RID: 8954 RVA: 0x000193D4 File Offset: 0x000175D4
	public void ShowWindow()
	{
		if (this.hidden)
		{
			LeanTween.alphaCanvas(this.canvasGroup, 1f, 0.25f).setEase(LeanTweenType.easeInOutSine);
			this.hidden = false;
		}
	}

	// Token: 0x060022FB RID: 8955 RVA: 0x00019402 File Offset: 0x00017602
	public void HideWindow()
	{
		if (!this.hidden)
		{
			LeanTween.alphaCanvas(this.canvasGroup, 0f, 0.25f).setEase(LeanTweenType.easeInOutSine);
			this.hidden = true;
		}
	}

	// Token: 0x060022FC RID: 8956 RVA: 0x00019430 File Offset: 0x00017630
	public void OnDestroy()
	{
		if (this.lastHelper != null)
		{
			this.lastHelper.RemoveLastActiveControllerChangedDelegate(new PlayerActiveControllerChangedDelegate(this.OnControllerChanged));
		}
	}

	// Token: 0x040025DF RID: 9695
	public GameObject input_panel_pfb;

	// Token: 0x040025E0 RID: 9696
	public CanvasGroup canvasGroup;

	// Token: 0x040025E1 RID: 9697
	private const int panelPoolSize = 6;

	// Token: 0x040025E2 RID: 9698
	private UIInputPanel[] inputPanels;

	// Token: 0x040025E3 RID: 9699
	private bool hidden;

	// Token: 0x040025E4 RID: 9700
	private BoardPlayer player;

	// Token: 0x040025E5 RID: 9701
	private InputHelp[] inputHelps = new InputHelp[6];

	// Token: 0x040025E6 RID: 9702
	private InputHelp curInputHelp;

	// Token: 0x040025E7 RID: 9703
	private Player.ControllerHelper lastHelper;
}
