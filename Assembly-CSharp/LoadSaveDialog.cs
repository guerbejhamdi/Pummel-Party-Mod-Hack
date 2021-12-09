using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x0200011C RID: 284
public class LoadSaveDialog : MonoBehaviour
{
	// Token: 0x0600087B RID: 2171 RVA: 0x00009CBA File Offset: 0x00007EBA
	public void Awake()
	{
		this.LoadSaves();
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x00009CBA File Offset: 0x00007EBA
	private void OnLoad()
	{
		this.LoadSaves();
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x0004F02C File Offset: 0x0004D22C
	private void LoadSaves()
	{
		GameManager.Saves.Clear();
		if (File.Exists(GameManager.GetSavePath()))
		{
			try
			{
				using (BinaryReader binaryReader = new BinaryReader(File.Open(GameManager.GetSavePath(), FileMode.Open)))
				{
					short num = binaryReader.ReadInt16();
					Debug.Log("Version: " + num.ToString());
					if (num != GameManager.SaveVersion)
					{
						this.loadButton.SetState(BasicButtonBase.BasicButtonState.Disabled);
						return;
					}
					int num2 = (int)binaryReader.ReadByte();
					Debug.Log("Save Count: " + num2.ToString());
					for (int i = 0; i < num2; i++)
					{
						GameSave gameSave = new GameSave();
						gameSave.Serialize(binaryReader);
						GameManager.Saves.Add(gameSave);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Loading turn data failed: " + ex.ToString());
				this.loadButton.SetState(BasicButtonBase.BasicButtonState.Disabled);
				return;
			}
			this.loadButton.SetState(BasicButtonBase.BasicButtonState.Enabled);
			this.UpdateUI();
			return;
		}
		this.loadButton.SetState(BasicButtonBase.BasicButtonState.Disabled);
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x0004F150 File Offset: 0x0004D350
	private void UpdateUI()
	{
		for (int i = 0; i < this.elements.Count; i++)
		{
			UnityEngine.Object.Destroy(this.elements[i].gameObject);
		}
		this.elements.Clear();
		for (int j = 0; j < GameManager.Saves.Count; j++)
		{
			GameSave gameSave = GameManager.Saves[j];
			for (int k = 0; k < gameSave.turnSaves.Count; k++)
			{
				TurnSave turnSave = gameSave.turnSaves[k];
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.savePrefab);
				gameObject.transform.SetParent(this.listParent, false);
				LoadSaveDialogElement component = gameObject.GetComponent<LoadSaveDialogElement>();
				bool first = j == 0 && k == 0;
				if (j == 0 && k == 0)
				{
					this.selected = component;
				}
				component.Setup(gameSave, k, first);
				this.elements.Add(component);
			}
		}
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x0004F234 File Offset: 0x0004D434
	public void Load()
	{
		if (this.selected != null)
		{
			GameManager.LobbyController.SetLobbySettings(this.selected.save.lobbyOptions);
			GameManager.SaveToLoad = this.selected.save.turnSaves[this.selected.index];
			for (int i = 7; i >= 0; i--)
			{
				GameManager.LobbyController.ForceRemovePlayer((short)i);
			}
			GameManager.LobbyController.UpdatePlayers();
			GameManager.LobbyController.LoadSave();
		}
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x0004F2BC File Offset: 0x0004D4BC
	public void Selected(LoadSaveDialogElement selected)
	{
		this.selected = selected;
		for (int i = 0; i < this.elements.Count; i++)
		{
			this.elements[i].toggle.isOn = (selected == this.elements[i]);
		}
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x0000398C File Offset: 0x00001B8C
	private void OnDestroy()
	{
	}

	// Token: 0x040006D2 RID: 1746
	public GameObject savePrefab;

	// Token: 0x040006D3 RID: 1747
	public Transform listParent;

	// Token: 0x040006D4 RID: 1748
	public BasicButtonBase loadButton;

	// Token: 0x040006D5 RID: 1749
	private List<LoadSaveDialogElement> elements = new List<LoadSaveDialogElement>();

	// Token: 0x040006D6 RID: 1750
	private LoadSaveDialogElement selected;
}
