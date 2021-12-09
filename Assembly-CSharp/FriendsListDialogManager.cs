using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class FriendsListDialogManager : MonoBehaviour
{
	// Token: 0x060001E8 RID: 488 RVA: 0x00004C53 File Offset: 0x00002E53
	public void Show()
	{
		this.GetFriends();
		this.mainMenuWindow.SetState(MainMenuWindowState.Visible);
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x00004C67 File Offset: 0x00002E67
	public void Hide()
	{
		this.mainMenuWindow.SetState(MainMenuWindowState.Hidden);
	}

	// Token: 0x060001EA RID: 490 RVA: 0x00034158 File Offset: 0x00032358
	private void GetFriends()
	{
		for (int i = 0; i < this.elements.Count; i++)
		{
			UnityEngine.Object.Destroy(this.elements[i].gameObject);
		}
		this.elements.Clear();
		if (SteamManager.Initialized)
		{
			int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
			List<CSteamID> list = new List<CSteamID>();
			List<CSteamID> list2 = new List<CSteamID>();
			for (int j = 0; j < friendCount; j++)
			{
				CSteamID friendByIndex = SteamFriends.GetFriendByIndex(j, EFriendFlags.k_EFriendFlagImmediate);
				if (SteamFriends.GetFriendPersonaState(friendByIndex) != EPersonaState.k_EPersonaStateOffline)
				{
					FriendGameInfo_t friendGameInfo_t;
					if (SteamFriends.GetFriendGamePlayed(friendByIndex, out friendGameInfo_t) & friendGameInfo_t.m_gameID.AppID().m_AppId == 880940U)
					{
						list.Add(friendByIndex);
					}
					else
					{
						list2.Add(friendByIndex);
					}
				}
			}
			list.Sort((CSteamID x, CSteamID y) => SteamFriends.GetFriendPersonaName(x).CompareTo(SteamFriends.GetFriendPersonaName(y)));
			list2.Sort((CSteamID x, CSteamID y) => SteamFriends.GetFriendPersonaName(x).CompareTo(SteamFriends.GetFriendPersonaName(y)));
			foreach (CSteamID friendSteamId in list)
			{
				this.CreateElement(friendSteamId);
			}
			foreach (CSteamID friendSteamId2 in list2)
			{
				this.CreateElement(friendSteamId2);
			}
		}
	}

	// Token: 0x060001EB RID: 491 RVA: 0x000342E0 File Offset: 0x000324E0
	public void CreateElement(CSteamID friendSteamId)
	{
		SteamFriendsDialogElement component = UnityEngine.Object.Instantiate<GameObject>(this.friendsListDialogElementPrefab, this.parent).GetComponent<SteamFriendsDialogElement>();
		component.Setup(friendSteamId);
		this.elements.Add(component);
	}

	// Token: 0x0400024C RID: 588
	public MainMenuWindow mainMenuWindow;

	// Token: 0x0400024D RID: 589
	public GameObject friendsListDialogElementPrefab;

	// Token: 0x0400024E RID: 590
	public Transform parent;

	// Token: 0x0400024F RID: 591
	private List<SteamFriendsDialogElement> elements = new List<SteamFriendsDialogElement>();
}
