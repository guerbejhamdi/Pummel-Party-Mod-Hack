using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020004F3 RID: 1267
public class ControllerFocus : MonoBehaviour
{
	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x06002160 RID: 8544 RVA: 0x000182F7 File Offset: 0x000164F7
	public static ControllerType[] LastControllerType
	{
		get
		{
			return ControllerFocus.lastControllerType;
		}
	}

	// Token: 0x06002161 RID: 8545 RVA: 0x000CEB70 File Offset: 0x000CCD70
	public static void AddWindow(int i, MainMenuWindow newWindow)
	{
		if (i == -1 || newWindow.ignoreAsCurrent)
		{
			return;
		}
		if (ControllerFocus.curWindow[i].Contains(newWindow))
		{
			ControllerFocus.curWindow[i].Remove(newWindow);
		}
		ControllerFocus.curWindow[i].Add(newWindow);
		if (ControllerFocus.LastControllerType[i] == ControllerType.Joystick)
		{
			EventSystem.GetSystem(i).SetSelectedGameObject(ControllerFocus.CurWindow(i).GetSelectable());
		}
	}

	// Token: 0x06002162 RID: 8546 RVA: 0x000CEBD4 File Offset: 0x000CCDD4
	public static void RemoveWindow(int i, MainMenuWindow window)
	{
		if (i == -1 || window.ignoreAsCurrent)
		{
			return;
		}
		if (ControllerFocus.LastControllerType[i] == ControllerType.Joystick && window == ControllerFocus.CurWindow(i))
		{
			MainMenuWindow mainMenuWindow = null;
			if (ControllerFocus.curWindow[i].Count > 1)
			{
				for (int j = ControllerFocus.curWindow[i].Count - 2; j >= 0; j--)
				{
					if (ControllerFocus.curWindow[i][j] != null)
					{
						mainMenuWindow = ControllerFocus.curWindow[i][j];
						break;
					}
				}
			}
			if (mainMenuWindow != null)
			{
				EventSystem.GetSystem(i).SetSelectedGameObject(mainMenuWindow.GetSelectable());
			}
			else
			{
				EventSystem.GetSystem(i).SetSelectedGameObject(null);
			}
		}
		ControllerFocus.curWindow[i].Remove(window);
	}

	// Token: 0x06002163 RID: 8547 RVA: 0x000182FE File Offset: 0x000164FE
	public static MainMenuWindow CurWindow(int i)
	{
		if (ControllerFocus.curWindow[i].Count <= 0)
		{
			return null;
		}
		return ControllerFocus.curWindow[i][ControllerFocus.curWindow[i].Count - 1];
	}

	// Token: 0x06002164 RID: 8548 RVA: 0x0001832B File Offset: 0x0001652B
	private void Start()
	{
		this.Setup();
		GameManager.OnXInput = (GameManager.OnXInputEvent)Delegate.Combine(GameManager.OnXInput, new GameManager.OnXInputEvent(this.OnXInputChange));
	}

	// Token: 0x06002165 RID: 8549 RVA: 0x00018353 File Offset: 0x00016553
	private void OnXInputChange()
	{
		this.Setup();
	}

	// Token: 0x06002166 RID: 8550 RVA: 0x000CEC90 File Offset: 0x000CCE90
	private void Setup()
	{
		for (int i = 0; i < 8; i++)
		{
			ReInput.players.GetPlayer(i).controllers.AddLastActiveControllerChangedDelegate(new PlayerActiveControllerChangedDelegate(this.OnControllerChange));
			EventSystem.GetSystem(i).IsJoystick = (i != 0);
		}
	}

	// Token: 0x06002167 RID: 8551 RVA: 0x000CECDC File Offset: 0x000CCEDC
	private void Update()
	{
		for (int i = 0; i < 8; i++)
		{
			if (ControllerFocus.lastControllerType[i] == ControllerType.Joystick)
			{
				int num = ControllerFocus.curWindow[i].Count - 1;
				GameObject currentSelectedGameObject = EventSystem.GetSystem(i).currentSelectedGameObject;
				Selectable selectable = null;
				if (currentSelectedGameObject != null)
				{
					selectable = currentSelectedGameObject.GetComponent<Selectable>();
				}
				while ((currentSelectedGameObject == null || !currentSelectedGameObject.activeSelf || !currentSelectedGameObject.activeInHierarchy || (selectable != null && (!selectable.interactable || !selectable.enabled))) && num >= 0 && num < ControllerFocus.curWindow[i].Count)
				{
					MainMenuWindow mainMenuWindow = ControllerFocus.curWindow[i][num];
					if (mainMenuWindow != null && !mainMenuWindow.Hidden && mainMenuWindow.Interactable)
					{
						GameObject selectable2 = mainMenuWindow.GetSelectable();
						if (selectable2 != null)
						{
							EventSystem.GetSystem(i).SetSelectedGameObject(selectable2);
							Selectable component = selectable2.GetComponent<Selectable>();
							if (component != null && component.interactable && component.enabled)
							{
								component.Select();
								component.OnSelect(new BaseEventData(EventSystem.GetSystem(i)));
							}
						}
					}
					num--;
				}
			}
		}
		if (GameManager.DEBUGGING)
		{
			EventSystem system = EventSystem.GetSystem(0);
			if (system != null)
			{
				DebugTextUI.AddLine("Selected: " + ((system.currentSelectedGameObject == null) ? "Null" : this.GetHeirarchyString(system.currentSelectedGameObject.transform)), false);
			}
			string text = "Windows: ";
			for (int j = 0; j < ControllerFocus.curWindow[0].Count; j++)
			{
				if (ControllerFocus.curWindow[0][j] != null)
				{
					text = text + ControllerFocus.curWindow[0][j].gameObject.name + ", ";
				}
			}
			DebugTextUI.AddLine(text, false);
		}
	}

	// Token: 0x06002168 RID: 8552 RVA: 0x000CEECC File Offset: 0x000CD0CC
	private string GetHeirarchyString(Transform t)
	{
		string text = t.gameObject.name;
		Transform parent = t.parent;
		while (parent != null)
		{
			text = parent.gameObject.name + "/" + text;
			parent = parent.parent;
		}
		return text;
	}

	// Token: 0x06002169 RID: 8553 RVA: 0x000CEF18 File Offset: 0x000CD118
	private void OnControllerChange(Player player, Controller controller)
	{
		if (controller == null)
		{
			return;
		}
		if ((controller.type == ControllerType.Keyboard || controller.type == ControllerType.Mouse) && ControllerFocus.lastControllerType[player.id] == ControllerType.Joystick)
		{
			EventSystem.GetSystem(player.id).IsJoystick = false;
			EventSystem.GetSystem(player.id).SetSelectedGameObject(null);
			if (player.id == 0)
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
		}
		else if (controller.type == ControllerType.Joystick && (ControllerFocus.lastControllerType[player.id] == ControllerType.Keyboard || ControllerFocus.lastControllerType[player.id] == ControllerType.Mouse))
		{
			EventSystem.GetSystem(player.id).IsJoystick = true;
			MainMenuWindow mainMenuWindow = ControllerFocus.CurWindow(player.id);
			if (mainMenuWindow != null)
			{
				EventSystem.GetSystem(player.id).SetSelectedGameObject(mainMenuWindow.GetSelectable());
			}
			if (player.id == 0)
			{
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
		ControllerFocus.lastControllerType[player.id] = controller.type;
	}

	// Token: 0x0600216B RID: 8555 RVA: 0x000CF00C File Offset: 0x000CD20C
	// Note: this type is marked as 'beforefieldinit'.
	static ControllerFocus()
	{
		ControllerType[] array = new ControllerType[8];
		RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.1850D435B74DEE508B61F4912852DD3C89361AC89C307F197F26DA6F606F7D48).FieldHandle);
		ControllerFocus.lastControllerType = array;
	}

	// Token: 0x04002417 RID: 9239
	private const int PLAYER_COUNT = 8;

	// Token: 0x04002418 RID: 9240
	public static List<MainMenuWindow>[] curWindow = new List<MainMenuWindow>[]
	{
		new List<MainMenuWindow>(),
		new List<MainMenuWindow>(),
		new List<MainMenuWindow>(),
		new List<MainMenuWindow>(),
		new List<MainMenuWindow>(),
		new List<MainMenuWindow>(),
		new List<MainMenuWindow>(),
		new List<MainMenuWindow>()
	};

	// Token: 0x04002419 RID: 9241
	private static ControllerType[] lastControllerType;
}
