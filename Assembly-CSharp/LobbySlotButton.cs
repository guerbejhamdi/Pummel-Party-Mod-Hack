using System;
using I2.Loc;
using UnityEngine;

// Token: 0x0200050D RID: 1293
public class LobbySlotButton : BasicButtonBase
{
	// Token: 0x060021C8 RID: 8648 RVA: 0x0001881B File Offset: 0x00016A1B
	protected override void Awake()
	{
		this.slot = base.GetComponentInParent<UIMultiplayerLobbySlot>();
		base.Awake();
	}

	// Token: 0x060021C9 RID: 8649 RVA: 0x0001882F File Offset: 0x00016A2F
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x060021CA RID: 8650 RVA: 0x000CFED8 File Offset: 0x000CE0D8
	public override void OnSubmit()
	{
		if (this.button == null || !this.button.IsInteractable())
		{
			return;
		}
		switch (this.action)
		{
		case LobbySlotAction.GoCustomiseWindow:
			this.slot.default_wnd.SetState(MainMenuWindowState.Hidden);
			this.slot.customise_wnd.SetState(MainMenuWindowState.Visible);
			goto IL_83D;
		case LobbySlotAction.ReadyUp:
			this.slot.default_wnd.SetState(MainMenuWindowState.Hidden);
			GameManager.LobbyController.SetSlotReadyState((short)this.slot.slot_index, true);
			goto IL_83D;
		case LobbySlotAction.LeaveSlot:
			if (this.slot.CurName != null)
			{
				this.slot.CurName.status = NameStatus.Free;
			}
			GameManager.LeaveLobbySlot(this.slot.slot_index);
			goto IL_83D;
		case LobbySlotAction.LeaveCustomiseWindow:
			this.slot.customise_wnd.SetState(MainMenuWindowState.Hidden);
			this.slot.default_wnd.SetState(MainMenuWindowState.Visible);
			goto IL_83D;
		case LobbySlotAction.JoinSlot:
			GameManager.RequestLobbySlot(0, this.slot.slot_index);
			goto IL_83D;
		case LobbySlotAction.Unready:
			this.slot.ready_wnd.SetState(MainMenuWindowState.Hidden);
			GameManager.LobbyController.SetSlotReadyState((short)this.slot.slot_index, false);
			goto IL_83D;
		case LobbySlotAction.GoAppearanceWindow:
			this.slot.appearance_wnd.SetState(MainMenuWindowState.Visible);
			this.slot.customise_wnd.SetState(MainMenuWindowState.Hidden);
			this.slot.UpdateColor(false);
			this.slot.UpdateHat(false);
			this.slot.UpdateCape(false);
			this.slot.UpdateApperanceButtons();
			goto IL_83D;
		case LobbySlotAction.LeaveAppearanceWindow:
			this.slot.customise_wnd.SetState(MainMenuWindowState.Visible);
			if (this.slot.CurColorIndex != this.slot.TempColorIndex)
			{
				GameManager.LobbyController.SetSlotColor((short)this.slot.slot_index, this.slot.TempColorIndex);
			}
			if (this.slot.CurSkinIndex != this.slot.TempSkinIndex)
			{
				GameManager.LobbyController.SetSlotSkin((short)this.slot.slot_index, this.slot.TempSkinIndex);
			}
			if (this.slot.CurHat != this.slot.TempHat)
			{
				GameManager.LobbyController.SetSlotHat((short)this.slot.slot_index, this.slot.TempHat);
			}
			if (this.slot.CurCape != this.slot.TempCape)
			{
				GameManager.LobbyController.SetSlotCape((short)this.slot.slot_index, this.slot.TempCape);
			}
			this.slot.appearance_wnd.SetState(MainMenuWindowState.Hidden);
			this.slot.SetRotation(180f);
			goto IL_83D;
		case LobbySlotAction.GoNameWindow:
			this.slot.name_wnd.SetState(MainMenuWindowState.Visible);
			this.slot.customise_wnd.SetState(MainMenuWindowState.Hidden);
			this.slot.UpdateNameButtonStatus();
			goto IL_83D;
		case LobbySlotAction.LeaveNameWindow:
			this.slot.customise_wnd.SetState(MainMenuWindowState.Visible);
			this.slot.name_wnd.SetState(MainMenuWindowState.Hidden);
			this.slot.CurName.status = NameStatus.Free;
			this.slot.TempName.status = NameStatus.InUse;
			this.slot.CurName = this.slot.TempName;
			if (this.slot.CurName.name != GameManager.LobbyController.SlotName(this.slot.slot_index))
			{
				GameManager.LobbyController.SetSlotName((short)this.slot.slot_index, this.slot.CurName.name);
			}
			GameManager.LobbyController.UpdateSlotNameButtons();
			goto IL_83D;
		case LobbySlotAction.ColorLeft:
			this.slot.IncrementColor(false);
			goto IL_83D;
		case LobbySlotAction.ColorRight:
			this.slot.IncrementColor(true);
			goto IL_83D;
		case LobbySlotAction.NameLeft:
			this.slot.IncrementName(false);
			goto IL_83D;
		case LobbySlotAction.NameRight:
			this.slot.IncrementName(true);
			goto IL_83D;
		case LobbySlotAction.GoEditNameWindow:
			this.slot.name_wnd.SetState(MainMenuWindowState.Hidden);
			this.slot.name_edit_wnd.SetState(MainMenuWindowState.Visible);
			this.slot.PlayerObject.gameObject.SetActive(false);
			this.slot.nameEditApplyButton.SetState(BasicButtonBase.BasicButtonState.Enabled);
			this.slot.nameEditApplyButton.action = LobbySlotAction.LeaveEditNameWindow;
			this.slot.TempName.status = NameStatus.BeingEdited;
			GameManager.LobbyController.UpdateSlotNameButtons();
			goto IL_83D;
		case LobbySlotAction.GoNewNameWindow:
		{
			Name name = new Name("Player", NameStatus.BeingEdited, true);
			GameManager.LobbyController.Names.Add(name);
			this.slot.TempName = name;
			this.slot.UpdateNameButtonStatus();
			this.slot.name_wnd.SetState(MainMenuWindowState.Hidden);
			this.slot.name_edit_wnd.SetState(MainMenuWindowState.Visible);
			this.slot.PlayerObject.gameObject.SetActive(false);
			this.slot.nameEditApplyButton.SetState(BasicButtonBase.BasicButtonState.Disabled);
			this.slot.nameEditApplyButton.action = LobbySlotAction.LeaveNewNameWindow;
			goto IL_83D;
		}
		case LobbySlotAction.LeaveEditNameWindow:
		case LobbySlotAction.LeaveNewNameWindow:
			this.slot.PlayerObject.gameObject.SetActive(true);
			this.slot.name_edit_wnd.SetState(MainMenuWindowState.Hidden);
			this.slot.name_wnd.SetState(MainMenuWindowState.Visible);
			if (this.slot.CurName == this.slot.TempName)
			{
				this.slot.TempName.status = NameStatus.InUse;
			}
			else
			{
				this.slot.TempName.status = NameStatus.Free;
			}
			GameManager.LobbyController.UpdateSlotNameButtons();
			goto IL_83D;
		case LobbySlotAction.DeleteName:
			if (this.slot.TempName.save && (this.slot.TempName.status == NameStatus.Free || this.slot.TempName == this.slot.CurName))
			{
				Name originalName = this.slot.TempName;
				int num = GameManager.LobbyController.Names.FindIndex((Name m) => m == originalName);
				Name tempName = GameManager.LobbyController.Names[(num == 0) ? (GameManager.LobbyController.Names.Count - 1) : (num - 1)];
				Name name2 = null;
				int count = GameManager.LobbyController.Names.Count;
				for (int i = 0; i < count; i++)
				{
					num = ((num == 0) ? (GameManager.LobbyController.Names.Count - 1) : (num - 1));
					name2 = GameManager.LobbyController.Names[num];
					if (name2.status == NameStatus.Free && name2 != originalName)
					{
						break;
					}
				}
				foreach (UIMultiplayerLobbySlot uimultiplayerLobbySlot in GameManager.LobbyController.UIController.lobby_slots)
				{
					if (uimultiplayerLobbySlot.TempName == originalName)
					{
						if (uimultiplayerLobbySlot.TempName == uimultiplayerLobbySlot.CurName)
						{
							uimultiplayerLobbySlot.TempName = name2;
							uimultiplayerLobbySlot.CurName = name2;
							uimultiplayerLobbySlot.CurName.status = NameStatus.InUse;
						}
						else
						{
							uimultiplayerLobbySlot.TempName = tempName;
						}
					}
				}
				GameManager.LobbyController.Names.Remove(originalName);
				GameManager.LobbyController.UpdateSlotNameButtons();
				goto IL_83D;
			}
			goto IL_83D;
		case LobbySlotAction.SkinLeft:
			this.slot.IncrementSkin(false);
			goto IL_83D;
		case LobbySlotAction.SkinRight:
			this.slot.IncrementSkin(true);
			goto IL_83D;
		case LobbySlotAction.HatLeft:
			this.slot.IncrementHat(false);
			goto IL_83D;
		case LobbySlotAction.HatRight:
			this.slot.IncrementHat(true);
			goto IL_83D;
		case LobbySlotAction.DifficultyLeft:
			this.slot.IncrementDifficulty(false);
			goto IL_83D;
		case LobbySlotAction.DifficultyRight:
			this.slot.IncrementDifficulty(true);
			goto IL_83D;
		case LobbySlotAction.CapeLeft:
			this.slot.IncrementCape(false);
			goto IL_83D;
		case LobbySlotAction.CapeRight:
			this.slot.IncrementCape(true);
			goto IL_83D;
		case LobbySlotAction.LeaveErrorWindow:
			this.slot.error_wnd.SetState(MainMenuWindowState.Hidden);
			this.slot.name_wnd.SetState(MainMenuWindowState.Visible);
			goto IL_83D;
		}
		Debug.LogError("Lobby Slot Action: " + this.action.ToString() + " - Not implemented");
		IL_83D:
		base.OnSubmit();
	}

	// Token: 0x060021CB RID: 8651 RVA: 0x000D0728 File Offset: 0x000CE928
	private void OnGotSanitizedName(SanitizeTextResultCode result, string text)
	{
		GameManager.MainMenuUIController.SetLobbyState(MainMenuWindowState.Visible);
		if (result == SanitizeTextResultCode.Success)
		{
			if (this.slot.TempName.status == NameStatus.BeingEdited)
			{
				this.slot.TempName.name = text;
			}
			else
			{
				Name name = new Name(text, NameStatus.BeingEdited, true);
				GameManager.LobbyController.Names.Add(name);
				this.slot.TempName = name;
				this.slot.UpdateNameButtonStatus();
			}
			if (this.slot.CurName == this.slot.TempName)
			{
				this.slot.TempName.status = NameStatus.InUse;
			}
			else
			{
				this.slot.TempName.status = NameStatus.Free;
			}
			this.slot.name_wnd.SetState(MainMenuWindowState.Visible);
		}
		else
		{
			if (this.slot.CurName == this.slot.TempName)
			{
				this.slot.TempName.status = NameStatus.InUse;
			}
			else
			{
				this.slot.TempName.status = NameStatus.Free;
			}
			this.slot.name_wnd.SetState(MainMenuWindowState.Hidden);
			this.slot.error_wnd.SetState(MainMenuWindowState.Visible);
			if (result == SanitizeTextResultCode.Offensive)
			{
				string text2 = LocalizationManager.GetTranslation("UIOffensiveTextPrompt", true, 0, true, false, null, null, true);
				text2 = text2.Replace("%OffensiveString%", text);
				this.slot.errorWndText.text = text2;
			}
			else
			{
				this.slot.errorWndText.text = LocalizationManager.GetTranslation("UINameFailed", true, 0, true, false, null, null, true);
			}
		}
		GameManager.LobbyController.UpdateSlotNameButtons();
	}

	// Token: 0x040024BD RID: 9405
	[Header("LobbySlotButton Vars")]
	public LobbySlotAction action;

	// Token: 0x040024BE RID: 9406
	private UIMultiplayerLobbySlot slot;
}
