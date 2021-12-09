using System;
using System.Collections;
using I2.Loc;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004C2 RID: 1218
public class SteamFriendsDialogElement : MonoBehaviour
{
	// Token: 0x0600205B RID: 8283 RVA: 0x000CA980 File Offset: 0x000C8B80
	public void Setup(CSteamID steamID)
	{
		this.steamID = steamID;
		EPersonaState friendPersonaState = SteamFriends.GetFriendPersonaState(steamID);
		this.username.text = SteamFriends.GetFriendPersonaName(steamID);
		this.avatar.texture = this.GetSmallAvatar(steamID);
		FriendGameInfo_t friendGameInfo_t;
		SteamFriends.GetFriendGamePlayed(steamID, out friendGameInfo_t);
		switch (friendPersonaState)
		{
		case EPersonaState.k_EPersonaStateOffline:
			this.status.text = LocalizationManager.GetTranslation("Offline", true, 0, true, false, null, null, true);
			break;
		case EPersonaState.k_EPersonaStateOnline:
			this.status.text = LocalizationManager.GetTranslation("Online", true, 0, true, false, null, null, true);
			break;
		case EPersonaState.k_EPersonaStateBusy:
			this.status.text = LocalizationManager.GetTranslation("Busy", true, 0, true, false, null, null, true);
			break;
		case EPersonaState.k_EPersonaStateAway:
			this.status.text = LocalizationManager.GetTranslation("Away", true, 0, true, false, null, null, true);
			break;
		case EPersonaState.k_EPersonaStateSnooze:
			this.status.text = LocalizationManager.GetTranslation("Snooze", true, 0, true, false, null, null, true);
			break;
		case EPersonaState.k_EPersonaStateLookingToTrade:
			this.status.text = LocalizationManager.GetTranslation("Looking To Trade", true, 0, true, false, null, null, true);
			break;
		case EPersonaState.k_EPersonaStateLookingToPlay:
			this.status.text = LocalizationManager.GetTranslation("Looking To Play", true, 0, true, false, null, null, true);
			break;
		case EPersonaState.k_EPersonaStateMax:
			this.status.text = LocalizationManager.GetTranslation("Online", true, 0, true, false, null, null, true);
			break;
		}
		if (friendGameInfo_t.m_gameID.AppID().m_AppId == 880940U)
		{
			this.username.color = this.ingameCol;
			this.status.color = this.ingameCol;
			this.status.text = LocalizationManager.GetTranslation("Ingame", true, 0, true, false, null, null, true);
			return;
		}
		this.username.color = this.onlineCol;
		this.status.color = this.onlineCol;
	}

	// Token: 0x0600205C RID: 8284 RVA: 0x000CAB78 File Offset: 0x000C8D78
	public Texture2D GetSmallAvatar(CSteamID user)
	{
		int smallFriendAvatar = SteamFriends.GetSmallFriendAvatar(user);
		uint num;
		uint num2;
		if (SteamUtils.GetImageSize(smallFriendAvatar, out num, out num2) && num > 0U && num2 > 0U)
		{
			byte[] array = new byte[num * num2 * 4U];
			Texture2D texture2D = new Texture2D((int)num, (int)num2, TextureFormat.RGBA32, false, true);
			if (SteamUtils.GetImageRGBA(smallFriendAvatar, array, (int)(num * num2 * 4U)))
			{
				texture2D.LoadRawTextureData(array);
				texture2D.Apply();
			}
			return texture2D;
		}
		Debug.LogError("Couldn't get avatar.");
		return new Texture2D(0, 0);
	}

	// Token: 0x0600205D RID: 8285 RVA: 0x000CABE8 File Offset: 0x000C8DE8
	public void OnClick()
	{
		this.button.interactable = false;
		AudioSystem.PlayOneShot("MultimediaButtonClick007_STD_ZapSplat", 0.4f, 0f);
		SteamMatchmaking.InviteUserToLobby((CSteamID)GameManager.CurrentLobby, this.steamID);
		base.StartCoroutine(this.DisableButton());
	}

	// Token: 0x0600205E RID: 8286 RVA: 0x00017A29 File Offset: 0x00015C29
	private IEnumerator DisableButton()
	{
		yield return new WaitForSeconds(1f);
		if (this.button != null)
		{
			this.button.interactable = true;
		}
		yield break;
	}

	// Token: 0x0400231F RID: 8991
	public RawImage avatar;

	// Token: 0x04002320 RID: 8992
	public Text username;

	// Token: 0x04002321 RID: 8993
	public Text status;

	// Token: 0x04002322 RID: 8994
	public Button button;

	// Token: 0x04002323 RID: 8995
	private Color32 onlineCol = new Color32(61, 101, 119, byte.MaxValue);

	// Token: 0x04002324 RID: 8996
	private Color32 offlineCol = new Color32(113, 113, 113, byte.MaxValue);

	// Token: 0x04002325 RID: 8997
	private Color32 ingameCol = new Color32(143, 192, 86, byte.MaxValue);

	// Token: 0x04002326 RID: 8998
	private CSteamID steamID;
}
