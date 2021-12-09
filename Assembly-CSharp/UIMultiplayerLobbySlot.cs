using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x0200055C RID: 1372
public class UIMultiplayerLobbySlot : MonoBehaviour
{
	// Token: 0x06002409 RID: 9225 RVA: 0x000D944C File Offset: 0x000D764C
	public void Set8PlayerSlots(bool enabled)
	{
		if (this.maxPlayersEnabled == enabled)
		{
			return;
		}
		this.maxPlayersEnabled = enabled;
		Vector2 b = new Vector2(0f, 230f);
		if (enabled)
		{
			this.playerWindow.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 150f);
			this.nameContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30f);
			((RectTransform)this.playerRawImg.transform).anchoredPosition += new Vector2(0f, 40f);
			this.keys.anchoredPosition = new Vector2(this.keys.anchoredPosition.x, 160f);
			this.keysGrid.cellSize = new Vector2(32f, 32f);
			this.keysGrid.padding = new RectOffset(10, 10, 10, 10);
		}
		else
		{
			this.playerWindow.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 380f);
			this.nameContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 52f);
			((RectTransform)this.playerRawImg.transform).anchoredPosition += new Vector2(0f, -40f);
			this.keys.anchoredPosition = new Vector2(this.keys.anchoredPosition.x, 345f);
			this.keysGrid.cellSize = new Vector2(46f, 46f);
			this.keysGrid.padding = new RectOffset(5, 5, 5, 5);
		}
		for (int i = 0; i < base.transform.childCount; i++)
		{
			RectTransform rectTransform = (RectTransform)base.transform.GetChild(i);
			if (!(rectTransform == this.playerWindow))
			{
				if (enabled)
				{
					rectTransform.anchoredPosition += b;
				}
				else
				{
					rectTransform.anchoredPosition -= b;
				}
			}
		}
	}

	// Token: 0x1700043F RID: 1087
	// (get) Token: 0x0600240A RID: 9226 RVA: 0x00019F76 File Offset: 0x00018176
	public ScoreScreenPlayer PlayerObject
	{
		get
		{
			return this.player;
		}
	}

	// Token: 0x17000440 RID: 1088
	// (get) Token: 0x0600240B RID: 9227 RVA: 0x00019F7E File Offset: 0x0001817E
	public bool IsFilled
	{
		get
		{
			return this.is_filled;
		}
	}

	// Token: 0x17000441 RID: 1089
	// (get) Token: 0x0600240C RID: 9228 RVA: 0x00019F86 File Offset: 0x00018186
	public ushort OwnerID
	{
		get
		{
			return this.owner_id;
		}
	}

	// Token: 0x17000442 RID: 1090
	// (get) Token: 0x0600240D RID: 9229 RVA: 0x00019F8E File Offset: 0x0001818E
	public ushort CurColorIndex
	{
		get
		{
			return this.color_index;
		}
	}

	// Token: 0x17000443 RID: 1091
	// (get) Token: 0x0600240E RID: 9230 RVA: 0x00019F96 File Offset: 0x00018196
	public PlayerColor CurColor
	{
		get
		{
			return GameManager.GetColorAtIndex((int)this.color_index);
		}
	}

	// Token: 0x17000444 RID: 1092
	// (get) Token: 0x0600240F RID: 9231 RVA: 0x00019FA3 File Offset: 0x000181A3
	public ushort TempColorIndex
	{
		get
		{
			return this.tempColorIndex;
		}
	}

	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x06002410 RID: 9232 RVA: 0x00019FAB File Offset: 0x000181AB
	public PlayerColor TempColor
	{
		get
		{
			return GameManager.GetColorAtIndex((int)this.tempColorIndex);
		}
	}

	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x06002411 RID: 9233 RVA: 0x00019FB8 File Offset: 0x000181B8
	public ushort CurSkinIndex
	{
		get
		{
			return this.skinIndex;
		}
	}

	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x06002412 RID: 9234 RVA: 0x00019FC0 File Offset: 0x000181C0
	public PlayerSkin CurSkin
	{
		get
		{
			return GameManager.GetSkinAtIndex((int)this.skinIndex);
		}
	}

	// Token: 0x17000448 RID: 1096
	// (get) Token: 0x06002413 RID: 9235 RVA: 0x00019FCD File Offset: 0x000181CD
	public ushort TempSkinIndex
	{
		get
		{
			return this.tempSkinIndex;
		}
	}

	// Token: 0x17000449 RID: 1097
	// (get) Token: 0x06002414 RID: 9236 RVA: 0x00019FD5 File Offset: 0x000181D5
	public PlayerSkin TempSkin
	{
		get
		{
			return GameManager.GetSkinAtIndex((int)this.tempSkinIndex);
		}
	}

	// Token: 0x1700044A RID: 1098
	// (get) Token: 0x06002415 RID: 9237 RVA: 0x00019FE2 File Offset: 0x000181E2
	public byte CurHat
	{
		get
		{
			return this.hat;
		}
	}

	// Token: 0x1700044B RID: 1099
	// (get) Token: 0x06002416 RID: 9238 RVA: 0x00019FEA File Offset: 0x000181EA
	public byte TempHat
	{
		get
		{
			return this.tempHat;
		}
	}

	// Token: 0x1700044C RID: 1100
	// (get) Token: 0x06002417 RID: 9239 RVA: 0x00019FF2 File Offset: 0x000181F2
	// (set) Token: 0x06002418 RID: 9240 RVA: 0x00019FFA File Offset: 0x000181FA
	public Name TempName
	{
		get
		{
			return this.tempName;
		}
		set
		{
			this.tempName = value;
		}
	}

	// Token: 0x1700044D RID: 1101
	// (get) Token: 0x06002419 RID: 9241 RVA: 0x0001A003 File Offset: 0x00018203
	// (set) Token: 0x0600241A RID: 9242 RVA: 0x0001A00B File Offset: 0x0001820B
	public Name CurName
	{
		get
		{
			return this.curName;
		}
		set
		{
			this.curName = value;
		}
	}

	// Token: 0x1700044E RID: 1102
	// (get) Token: 0x0600241B RID: 9243 RVA: 0x0001A014 File Offset: 0x00018214
	public byte CurCape
	{
		get
		{
			return this.cape;
		}
	}

	// Token: 0x1700044F RID: 1103
	// (get) Token: 0x0600241C RID: 9244 RVA: 0x0001A01C File Offset: 0x0001821C
	public byte TempCape
	{
		get
		{
			return this.tempCape;
		}
	}

	// Token: 0x0600241D RID: 9245 RVA: 0x000D9634 File Offset: 0x000D7834
	public void Awake()
	{
		this.player_name_txt = base.transform.Find("PlayerWindow/NameBackground/NameText").GetComponent<Text>();
		MultiplayerLobbyScene component = GameObject.Find("MultiplayerLobbyScene").GetComponent<MultiplayerLobbyScene>();
		this.player = component.GetPlayer(this.slot_index);
		this.buttons = base.GetComponentsInChildren<LobbySlotButton>(true);
		this.tabBars = base.GetComponentsInChildren<TabBar>(true);
		List<KeyboardKeyCode> list = new List<KeyboardKeyCode>();
		for (int i = 97; i <= 122; i++)
		{
			list.Add((KeyboardKeyCode)i);
		}
		list.Add(KeyboardKeyCode.Backspace);
		for (int j = 0; j < list.Count; j++)
		{
			KeyboardButton component2 = UnityEngine.Object.Instantiate<GameObject>(this.keyboardPrefab, this.keyboardParent).GetComponent<KeyboardButton>();
			component2.Setup(list[j], this);
			component2.gameObject.SetActive(true);
		}
		this.name_edit_wnd.UpdateSelectables();
	}

	// Token: 0x0600241E RID: 9246 RVA: 0x0001A024 File Offset: 0x00018224
	public void Start()
	{
		int num = this.slot_index;
	}

	// Token: 0x0600241F RID: 9247 RVA: 0x000D9708 File Offset: 0x000D7908
	public void Reset()
	{
		this.initialized = false;
		this.default_wnd.SetState(MainMenuWindowState.Hidden);
		this.customise_wnd.SetState(MainMenuWindowState.Hidden);
		this.ready_wnd.SetState(MainMenuWindowState.Hidden);
		this.not_ready_wnd.SetState(MainMenuWindowState.Hidden);
		this.appearance_wnd.SetState(MainMenuWindowState.Hidden);
		this.name_wnd.SetState(MainMenuWindowState.Hidden);
		this.name_edit_wnd.SetState(MainMenuWindowState.Hidden);
		this.error_wnd.SetState(MainMenuWindowState.Hidden);
		this.empty_wnd.SetState(MainMenuWindowState.Visible);
		this.tempName = null;
		if (this.curName != null)
		{
			this.curName.status = NameStatus.Free;
		}
		this.curName = null;
		this.player.SetRotation(180f);
	}

	// Token: 0x06002420 RID: 9248 RVA: 0x000D97BC File Offset: 0x000D79BC
	public void SetSlotStatus(bool filled, string name, ushort id, ushort color, ushort skin, byte hat, byte cape, short localID, bool is_ai, bool ready, BotDifficulty slot_bot_difficulty)
	{
		if (!this.initialized)
		{
			this.initialized = true;
			this.is_filled = !filled;
			this.is_ready = !ready;
		}
		this.UpdateSlot(filled, name, id, color, skin, hat, cape, localID, is_ai, ready, slot_bot_difficulty);
	}

	// Token: 0x06002421 RID: 9249 RVA: 0x000D9808 File Offset: 0x000D7A08
	public void UpdateSlot(bool is_filled, string player_name, ushort owner_id, ushort color_index, ushort skin_index, byte hat, byte cape, short localID, bool is_ai, bool is_ready, BotDifficulty slot_bot_difficulty)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (is_filled)
		{
			if (this.is_filled != is_filled)
			{
				this.empty_wnd.SetState(MainMenuWindowState.Hidden);
				if (NetSystem.MyPlayer.UserID == owner_id)
				{
					int num = (int)(is_ai ? 0 : localID);
					this.eventSystemGroup.EventSystemID = num;
					this.default_wnd.SetState(MainMenuWindowState.Visible);
					if (!is_ai)
					{
						this.default_wnd.DisabledObjects.Remove(this.difficultySelectObject);
					}
					else
					{
						this.default_wnd.DisabledObjects.Remove(this.readyButton);
					}
					if (this.setButtonPlayerCoroutine != null)
					{
						base.StopCoroutine(this.setButtonPlayerCoroutine);
					}
					this.setButtonPlayerCoroutine = base.StartCoroutine(this.SetButtonPlayer(num));
					this.customiseNameButton.SetState(is_ai ? BasicButtonBase.BasicButtonState.Disabled : BasicButtonBase.BasicButtonState.Enabled);
					this.customiseAppearanceButton.SetState((GameManager.SaveToLoad != null) ? BasicButtonBase.BasicButtonState.Disabled : BasicButtonBase.BasicButtonState.Enabled);
					this.difficultySelectObject.SetActive(is_ai);
					this.readyButton.SetActive(!is_ai);
					this.leaveSlotText.text = LocalizationManager.GetTranslation(is_ai ? "Remove Bot" : "Leave Slot", true, 0, true, false, null, null, true);
				}
				else
				{
					this.eventSystemGroup.EventSystemID = -1;
				}
				this.player.gameObject.SetActive(true);
			}
			if (this.is_ready != is_ready || (this.is_filled != is_filled && is_filled))
			{
				if (is_ready)
				{
					this.not_ready_wnd.SetState(MainMenuWindowState.Hidden);
					this.ready_wnd.SetState(MainMenuWindowState.Visible);
					this.ready_wnd.transform.Find("UnreadyBtn").gameObject.SetActive(NetSystem.MyPlayer.UserID == owner_id);
				}
				else
				{
					this.ready_wnd.SetState(MainMenuWindowState.Hidden);
					this.not_ready_wnd.SetState(MainMenuWindowState.Hidden);
					if (NetSystem.MyPlayer.UserID != owner_id)
					{
						this.not_ready_wnd.SetState(MainMenuWindowState.Visible);
					}
					else
					{
						this.default_wnd.SetState(MainMenuWindowState.Visible);
					}
				}
			}
			if (this.appearance_wnd.Hidden)
			{
				this.tempColorIndex = color_index;
				this.tempSkinIndex = skin_index;
				this.tempHat = hat;
				this.tempCape = cape;
			}
			if (NetSystem.MyPlayer.UserID != owner_id || is_ai)
			{
				this.SetName(player_name);
			}
			else
			{
				if (this.is_filled != is_filled)
				{
					if (this.curName != null)
					{
						this.curName.status = NameStatus.Free;
					}
					this.curName = GameManager.LobbyController.Names.Find((Name m) => m.name == player_name);
					this.curName.status = NameStatus.InUse;
				}
				if (this.name_wnd.Hidden && this.name_edit_wnd.Hidden)
				{
					this.tempName = this.curName;
					this.SetName(this.curName.name);
				}
			}
			this.UpdateColor(true);
			this.UpdateSkin();
			this.UpdateHat(true);
			this.UpdateCape(true);
			this.UpdateDifficulty(slot_bot_difficulty);
		}
		else
		{
			if (this.curName != null)
			{
				this.curName.status = NameStatus.Free;
			}
			this.curName = null;
			this.tempName = null;
			this.SetName(LocalizationManager.GetTranslation("Empty", true, 0, true, false, null, null, true));
			for (int i = 0; i < this.corner_images.Length; i++)
			{
				this.corner_images[i].color = this.empty_color;
			}
			if (this.is_filled != is_filled)
			{
				if (NetSystem.MyPlayer.UserID == this.owner_id)
				{
					this.default_wnd.SetState(MainMenuWindowState.Hidden);
				}
				else
				{
					this.ready_wnd.SetState(MainMenuWindowState.Hidden);
					this.not_ready_wnd.SetState(MainMenuWindowState.Hidden);
				}
				this.empty_wnd.SetState(MainMenuWindowState.Visible);
				this.tempColorIndex = color_index;
				this.tempSkinIndex = skin_index;
				this.tempHat = hat;
				this.tempCape = cape;
				this.player.gameObject.SetActive(false);
			}
			if (GameManager.LobbyController != null)
			{
				List<int> list = new List<int>
				{
					0,
					1,
					2,
					3,
					4,
					5,
					6,
					7
				};
				int j = 0;
				while (j < list.Count)
				{
					for (int k = 0; k < 8; k++)
					{
						if (GameManager.LobbyController.SlotStatus(k) && GameManager.LobbyController.SlotOwner(k) == NetSystem.MyPlayer.UserID && (int)GameManager.LobbyController.SlotLocalID(k) == list[j])
						{
							list.RemoveAt(j);
							break;
						}
						if (k == 7)
						{
							j++;
						}
					}
				}
				for (int l = 0; l < 8; l++)
				{
					if (!GameManager.LobbyController.SlotStatus(l))
					{
						if (l == this.slot_index)
						{
							int num2 = is_ai ? 0 : list[0];
							this.eventSystemGroup.EventSystemID = num2;
							if (this.setButtonPlayerCoroutine != null)
							{
								base.StopCoroutine(this.setButtonPlayerCoroutine);
							}
							this.setButtonPlayerCoroutine = base.StartCoroutine(this.SetButtonPlayer(num2));
							break;
						}
						list.RemoveAt(0);
						if (list.Count == 0)
						{
							break;
						}
					}
				}
			}
		}
		this.is_filled = is_filled;
		this.owner_id = owner_id;
		this.color_index = color_index;
		this.skinIndex = skin_index;
		this.hat = hat;
		this.cape = cape;
		this.localID = localID;
		this.is_ready = is_ready;
		this.is_ai = is_ai;
		this.slot_bot_difficulty = slot_bot_difficulty;
	}

	// Token: 0x06002422 RID: 9250 RVA: 0x0001A02F File Offset: 0x0001822F
	private IEnumerator SetButtonPlayer(int id)
	{
		Player p = ReInput.players.GetPlayer(id);
		bool isNull = p == null;
		LobbySlotButton[] array = this.buttons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetPlayer(p, isNull);
			yield return null;
		}
		array = null;
		TabBar[] array2 = this.tabBars;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].SetPlayer(p, isNull);
			yield return null;
		}
		array2 = null;
		yield break;
	}

	// Token: 0x06002423 RID: 9251 RVA: 0x000D9D70 File Offset: 0x000D7F70
	public void IncrementName(bool right)
	{
		int num = GameManager.LobbyController.Names.FindIndex((Name m) => m == this.tempName);
		if (!right && num == 0)
		{
			num = GameManager.LobbyController.Names.Count - 1;
		}
		else if (right && num == GameManager.LobbyController.Names.Count - 1)
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
		this.tempName = GameManager.LobbyController.Names[num];
		this.UpdateNameButtonStatus();
	}

	// Token: 0x06002424 RID: 9252 RVA: 0x0001A045 File Offset: 0x00018245
	public void SetName(string name)
	{
		this.player_name_txt.text = name;
		this.namePickerTxt.text = name;
		this.nameInputField.text = name;
	}

	// Token: 0x06002425 RID: 9253 RVA: 0x000D9DF8 File Offset: 0x000D7FF8
	public void UpdateNameButtonStatus()
	{
		if ((NetSystem.MyPlayer.UserID == this.owner_id && this.tempName == null) || this.curName == null)
		{
			return;
		}
		bool flag = this.tempName.save && (this.tempName.status != NameStatus.InUse || this.tempName == this.curName) && this.tempName.status != NameStatus.BeingEdited;
		bool flag2 = (this.tempName.status != NameStatus.InUse || this.tempName == this.curName) && this.tempName.status != NameStatus.BeingEdited;
		this.nameEditButton.SetState(flag ? BasicButtonBase.BasicButtonState.Enabled : BasicButtonBase.BasicButtonState.Disabled);
		this.nameDeleteButton.SetState(flag ? BasicButtonBase.BasicButtonState.Enabled : BasicButtonBase.BasicButtonState.Disabled);
		this.nameApplyButton.SetState(flag2 ? BasicButtonBase.BasicButtonState.Enabled : BasicButtonBase.BasicButtonState.Disabled);
		this.SetName(this.tempName.name);
	}

	// Token: 0x06002426 RID: 9254 RVA: 0x000D9EE0 File Offset: 0x000D80E0
	public void IncrementColor(bool right)
	{
		ushort num = this.tempColorIndex;
		if (!right && num == 0)
		{
			num = (ushort)(GameManager.GetPlayerColorCount() - 1);
		}
		else if (right && (int)num == GameManager.GetPlayerColorCount() - 1)
		{
			num = 0;
		}
		else if (right)
		{
			num += 1;
		}
		else
		{
			num -= 1;
		}
		this.tempColorIndex = num;
		this.player.SetRotation(180f);
		this.UpdateColor(true);
	}

	// Token: 0x06002427 RID: 9255 RVA: 0x0001A06B File Offset: 0x0001826B
	public void SetRotation(float rotation)
	{
		this.player.SetRotation(rotation);
	}

	// Token: 0x06002428 RID: 9256 RVA: 0x000D9F44 File Offset: 0x000D8144
	public void UpdateColor(bool updateButton = true)
	{
		PlayerColor colorAtIndex = GameManager.GetColorAtIndex((int)this.tempColorIndex);
		for (int i = 0; i < this.corner_images.Length; i++)
		{
			this.corner_images[i].color = colorAtIndex.uiColor;
		}
		this.player.SetPlayerColor(colorAtIndex);
		this.colorSelectImage.color = colorAtIndex.uiColor;
		this.colorDenied.SetActive(!this.CanUseColor(this.tempColorIndex));
		if (updateButton)
		{
			this.UpdateApperanceButtons();
		}
	}

	// Token: 0x06002429 RID: 9257 RVA: 0x000D9FC4 File Offset: 0x000D81C4
	public void IncrementSkin(bool right)
	{
		ushort num = this.tempSkinIndex;
		if (!right && num == 0)
		{
			num = (ushort)(GameManager.GetPlayerSkinCount() - 1);
		}
		else if (right && (int)num == GameManager.GetPlayerSkinCount() - 1)
		{
			num = 0;
		}
		else if (right)
		{
			num += 1;
		}
		else
		{
			num -= 1;
		}
		this.tempSkinIndex = num;
		this.player.SetRotation(180f);
		this.UpdateSkin();
	}

	// Token: 0x0600242A RID: 9258 RVA: 0x000DA028 File Offset: 0x000D8228
	public void IncrementHat(bool right)
	{
		byte b = this.tempHat;
		if (!right && b == 0)
		{
			b = (byte)(GameManager.GetPlayerHatCount() - 1);
		}
		else if (right && (int)b == GameManager.GetPlayerHatCount() - 1)
		{
			b = 0;
		}
		else if (right)
		{
			b += 1;
		}
		else
		{
			b -= 1;
		}
		this.tempHat = b;
		this.player.SetRotation(180f);
		this.UpdateHat(true);
	}

	// Token: 0x0600242B RID: 9259 RVA: 0x000DA08C File Offset: 0x000D828C
	public void IncrementCape(bool right)
	{
		byte b = this.tempCape;
		if (!right && b == 0)
		{
			b = 9;
		}
		else if (right && b == 9)
		{
			b = 0;
		}
		else if (right)
		{
			b += 1;
		}
		else
		{
			b -= 1;
		}
		this.tempCape = b;
		this.player.SetRotation(0f);
		this.UpdateCape(true);
	}

	// Token: 0x0600242C RID: 9260 RVA: 0x000DA0E4 File Offset: 0x000D82E4
	public void IncrementDifficulty(bool right)
	{
		byte b = (byte)this.slot_bot_difficulty;
		if (!right && b == 0)
		{
			b = 2;
		}
		else if (right && b == 2)
		{
			b = 0;
		}
		else if (right)
		{
			b += 1;
		}
		else
		{
			b -= 1;
		}
		this.slot_bot_difficulty = (BotDifficulty)b;
		this.UpdateDifficulty(this.slot_bot_difficulty);
		GameManager.LobbyController.SetSlotBotDifficulty((short)this.slot_index, (byte)this.slot_bot_difficulty);
	}

	// Token: 0x0600242D RID: 9261 RVA: 0x000DA148 File Offset: 0x000D8348
	private void UpdateDifficulty(BotDifficulty difficulty)
	{
		this.difficultySelectText.text = LocalizationManager.GetTranslation(this.difficultyStrings[(int)((byte)difficulty)], true, 0, true, false, null, null, true);
	}

	// Token: 0x0600242E RID: 9262 RVA: 0x000DA178 File Offset: 0x000D8378
	private void UpdateSkin()
	{
		PlayerSkin skinAtIndex = GameManager.GetSkinAtIndex((int)this.tempSkinIndex);
		this.player.SetPlayerSkin(skinAtIndex);
		this.skinSelectText.text = LocalizationManager.GetTranslation(skinAtIndex.skinNameToken, true, 0, true, false, null, null, true);
	}

	// Token: 0x0600242F RID: 9263 RVA: 0x000DA1BC File Offset: 0x000D83BC
	public void UpdateHat(bool updateButton = true)
	{
		CharacterHat hatAtIndex = GameManager.GetHatAtIndex((int)this.tempHat);
		this.player.SetPlayerHat(hatAtIndex);
		this.hatSelectText.text = LocalizationManager.GetTranslation(hatAtIndex.hatNameToken, true, 0, true, false, null, null, true);
		this.hatSelectText.color = (this.CanUseHat(this.tempHat) ? Color.white : Color.red);
		if (updateButton)
		{
			this.UpdateApperanceButtons();
		}
	}

	// Token: 0x06002430 RID: 9264 RVA: 0x000DA22C File Offset: 0x000D842C
	public void UpdateCape(bool updateButton = true)
	{
		this.player.SetPlayerCape(this.tempCape);
		Text text = this.capeSelectText;
		CapeType capeType = (CapeType)this.tempCape;
		text.text = LocalizationManager.GetTranslation(capeType.ToString(), true, 0, true, false, null, null, true);
		this.capeSelectText.color = (this.CanUseCape(this.tempCape) ? Color.white : Color.red);
		if (updateButton)
		{
			this.UpdateApperanceButtons();
		}
	}

	// Token: 0x06002431 RID: 9265 RVA: 0x000DA2A4 File Offset: 0x000D84A4
	public void UpdateApperanceButtons()
	{
		if (!this.CanUseColor(this.tempColorIndex) || !this.CanUseHat(this.tempHat) || !this.CanUseCape(this.tempCape))
		{
			this.appearanceApplyButton.SetState(BasicButtonBase.BasicButtonState.Disabled);
			return;
		}
		this.appearanceApplyButton.SetState(BasicButtonBase.BasicButtonState.Enabled);
	}

	// Token: 0x06002432 RID: 9266 RVA: 0x0001A079 File Offset: 0x00018279
	private bool CanUseCape(byte tempCape)
	{
		return tempCape == 0 || tempCape == 9 || GameManager.unlocked[(int)(tempCape - 1)];
	}

	// Token: 0x06002433 RID: 9267 RVA: 0x0001A08E File Offset: 0x0001828E
	private bool CanUseColor(ushort tempColorIndex)
	{
		return tempColorIndex == this.color_index || !GameManager.LobbyController.ColorStatus((int)tempColorIndex);
	}

	// Token: 0x06002434 RID: 9268 RVA: 0x0001A0A9 File Offset: 0x000182A9
	private bool CanUseHat(byte tempHat)
	{
		return tempHat == this.hat || !GameManager.LobbyController.HatStatus((int)tempHat);
	}

	// Token: 0x06002435 RID: 9269 RVA: 0x000DA2F4 File Offset: 0x000D84F4
	public void KeyboardPress(KeyboardKeyCode keyCode)
	{
		if (keyCode != KeyboardKeyCode.Backspace)
		{
			if (keyCode != KeyboardKeyCode.KeypadEnter && this.nameInputField.text.Length < 12)
			{
				string value = (this.nameInputField.text.Length == 0) ? keyCode.ToString() : keyCode.ToString().ToLower();
				this.nameInputField.text = this.nameInputField.text.Insert(this.nameInputField.text.Length, value);
			}
		}
		else if (this.nameInputField.text.Length != 0)
		{
			this.nameInputField.text = this.nameInputField.text.Remove(this.nameInputField.text.Length - 1);
		}
		this.OnTextChanged();
	}

	// Token: 0x06002436 RID: 9270 RVA: 0x000DA3D0 File Offset: 0x000D85D0
	public void OnTextChanged()
	{
		if (this.tempName == null)
		{
			return;
		}
		this.tempName.name = this.nameInputField.text;
		if (this.nameInputField.text.Length == 0 || GameManager.LobbyController.Names.Find((Name m) => m.name == this.nameInputField.text && m != this.tempName) != null)
		{
			this.nameEditApplyButton.SetState(BasicButtonBase.BasicButtonState.Disabled);
			return;
		}
		this.nameEditApplyButton.SetState(BasicButtonBase.BasicButtonState.Enabled);
	}

	// Token: 0x04002728 RID: 10024
	public Image[] corner_images;

	// Token: 0x04002729 RID: 10025
	public int slot_index;

	// Token: 0x0400272A RID: 10026
	public Color empty_color;

	// Token: 0x0400272B RID: 10027
	public MainMenuWindow default_wnd;

	// Token: 0x0400272C RID: 10028
	public MainMenuWindow customise_wnd;

	// Token: 0x0400272D RID: 10029
	public MainMenuWindow empty_wnd;

	// Token: 0x0400272E RID: 10030
	public MainMenuWindow ready_wnd;

	// Token: 0x0400272F RID: 10031
	public MainMenuWindow not_ready_wnd;

	// Token: 0x04002730 RID: 10032
	public MainMenuWindow appearance_wnd;

	// Token: 0x04002731 RID: 10033
	public MainMenuWindow name_wnd;

	// Token: 0x04002732 RID: 10034
	public MainMenuWindow name_edit_wnd;

	// Token: 0x04002733 RID: 10035
	public MainMenuWindow error_wnd;

	// Token: 0x04002734 RID: 10036
	public GameObject colorDenied;

	// Token: 0x04002735 RID: 10037
	public Image colorSelectImage;

	// Token: 0x04002736 RID: 10038
	public Text errorWndText;

	// Token: 0x04002737 RID: 10039
	public Text skinSelectText;

	// Token: 0x04002738 RID: 10040
	public Text difficultySelectText;

	// Token: 0x04002739 RID: 10041
	public Text hatSelectText;

	// Token: 0x0400273A RID: 10042
	public Text capeSelectText;

	// Token: 0x0400273B RID: 10043
	public Text leaveSlotText;

	// Token: 0x0400273C RID: 10044
	public GameObject difficultySelectObject;

	// Token: 0x0400273D RID: 10045
	public GameObject readyButton;

	// Token: 0x0400273E RID: 10046
	public LobbySlotButton appearanceApplyButton;

	// Token: 0x0400273F RID: 10047
	public EventSystemGroup eventSystemGroup;

	// Token: 0x04002740 RID: 10048
	public Text namePickerTxt;

	// Token: 0x04002741 RID: 10049
	public GameObject keyboardPrefab;

	// Token: 0x04002742 RID: 10050
	public InputField nameInputField;

	// Token: 0x04002743 RID: 10051
	public Transform keyboardParent;

	// Token: 0x04002744 RID: 10052
	public LobbySlotButton nameEditApplyButton;

	// Token: 0x04002745 RID: 10053
	public LobbySlotButton nameEditButton;

	// Token: 0x04002746 RID: 10054
	public LobbySlotButton nameDeleteButton;

	// Token: 0x04002747 RID: 10055
	public LobbySlotButton nameApplyButton;

	// Token: 0x04002748 RID: 10056
	public LobbySlotButton customiseNameButton;

	// Token: 0x04002749 RID: 10057
	public LobbySlotButton customiseBackButton;

	// Token: 0x0400274A RID: 10058
	public LobbySlotButton customiseAppearanceButton;

	// Token: 0x0400274B RID: 10059
	public RectTransform playerWindow;

	// Token: 0x0400274C RID: 10060
	public GameObject playerRawImg;

	// Token: 0x0400274D RID: 10061
	public RectTransform nameContainer;

	// Token: 0x0400274E RID: 10062
	public RectTransform keys;

	// Token: 0x0400274F RID: 10063
	public GridLayoutGroup keysGrid;

	// Token: 0x04002750 RID: 10064
	private bool maxPlayersEnabled;

	// Token: 0x04002751 RID: 10065
	private bool initialized;

	// Token: 0x04002752 RID: 10066
	private ScoreScreenPlayer player;

	// Token: 0x04002753 RID: 10067
	private ushort tempColorIndex;

	// Token: 0x04002754 RID: 10068
	private ushort tempSkinIndex;

	// Token: 0x04002755 RID: 10069
	private byte tempHat;

	// Token: 0x04002756 RID: 10070
	private byte tempCape;

	// Token: 0x04002757 RID: 10071
	private Name tempName;

	// Token: 0x04002758 RID: 10072
	private Name curName;

	// Token: 0x04002759 RID: 10073
	private Text player_name_txt;

	// Token: 0x0400275A RID: 10074
	private LobbySlotButton[] buttons;

	// Token: 0x0400275B RID: 10075
	private TabBar[] tabBars;

	// Token: 0x0400275C RID: 10076
	private bool is_filled;

	// Token: 0x0400275D RID: 10077
	private ushort owner_id;

	// Token: 0x0400275E RID: 10078
	private ushort color_index;

	// Token: 0x0400275F RID: 10079
	private ushort skinIndex;

	// Token: 0x04002760 RID: 10080
	private byte hat;

	// Token: 0x04002761 RID: 10081
	private byte cape;

	// Token: 0x04002762 RID: 10082
	private short localID = -1;

	// Token: 0x04002763 RID: 10083
	private bool is_ready;

	// Token: 0x04002764 RID: 10084
	private bool is_ai;

	// Token: 0x04002765 RID: 10085
	private BotDifficulty slot_bot_difficulty;

	// Token: 0x04002766 RID: 10086
	private Coroutine setButtonPlayerCoroutine;

	// Token: 0x04002767 RID: 10087
	private string[] difficultyStrings = new string[]
	{
		"Easy",
		"Normal",
		"Hard"
	};
}
