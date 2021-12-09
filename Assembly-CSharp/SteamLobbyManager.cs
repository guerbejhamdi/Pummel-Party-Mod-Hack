using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004C4 RID: 1220
public class SteamLobbyManager : MonoBehaviour
{
	// Token: 0x06002066 RID: 8294 RVA: 0x00017A4F File Offset: 0x00015C4F
	private IEnumerator Start()
	{
		while (!SteamManager.Initialized)
		{
			yield return null;
		}
		this.Callback_lobbyList = Callback<LobbyMatchList_t>.Create(new Callback<LobbyMatchList_t>.DispatchDelegate(this.OnLobbiesUpdated));
		yield break;
	}

	// Token: 0x06002067 RID: 8295 RVA: 0x000CACFC File Offset: 0x000C8EFC
	private void OnLobbiesUpdated(LobbyMatchList_t result)
	{
		for (int i = 0; i < this.lobbyEntries.Count; i++)
		{
			UnityEngine.Object.Destroy(this.lobbyEntries[i].gameObject);
		}
		this.lobbyEntries.Clear();
		Debug.Log("Lobbies Found: " + result.m_nLobbiesMatching.ToString());
		int num = 0;
		int num2 = 0;
		while ((long)num2 < (long)((ulong)result.m_nLobbiesMatching))
		{
			CSteamID lobbyByIndex = SteamMatchmaking.GetLobbyByIndex(num2);
			string lobbyData = SteamMatchmaking.GetLobbyData(lobbyByIndex, "time");
			uint num3 = 0U;
			uint.TryParse(lobbyData, out num3);
			uint serverRealTime = SteamUtils.GetServerRealTime();
			bool flag = false;
			uint num4 = serverRealTime - num3;
			if (serverRealTime < num3 || num4 <= 30U || num4 >= 86400U)
			{
				LobbyEntry lobbyEntry;
				if (num2 >= this.lobbyEntries.Count)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.lobbyUIPrefab);
					gameObject.transform.SetParent(this.content, false);
					lobbyEntry = gameObject.GetComponent<LobbyEntry>();
					this.lobbyEntries.Add(lobbyEntry);
				}
				else
				{
					lobbyEntry = this.lobbyEntries[num2];
				}
				string mapName = "Pirate Island";
				string mode = "Board Game";
				string text = SteamMatchmaking.GetLobbyData(lobbyByIndex, "name");
				text = text.Replace("'s game", "");
				string text2 = SteamMatchmaking.GetLobbyData(lobbyByIndex, "version");
				if (text2.Equals(""))
				{
					text2 = "Unknown";
				}
				string currentLanguageCode = LocalizationManager.CurrentLanguageCode;
				string translation = LocalizationManager.GetTranslation("GameOwner", true, 0, true, false, null, null, true);
				if (currentLanguageCode == "fr" || currentLanguageCode == "it" || currentLanguageCode == "es" || currentLanguageCode == "ru" || currentLanguageCode == "zh-CN" || currentLanguageCode == "zh-TW")
				{
					text = translation.Replace("%PlayerName%", text);
				}
				else
				{
					text += translation;
				}
				if (!this.uniqueLobbies.Contains(text))
				{
					this.uniqueLobbies.Add(text);
					Debug.Log("Unique Lobbies: " + this.uniqueLobbies.Count.ToString() + " : " + text);
				}
				if (flag)
				{
					Debug.LogError(string.Concat(new string[]
					{
						"Removing Old Game \nName: ",
						text,
						"\nVersion: ",
						text2,
						"\nTime: ",
						num3.ToString(),
						"\nActual Time: ",
						serverRealTime.ToString()
					}));
				}
				int i2 = 0;
				if (int.TryParse(SteamMatchmaking.GetLobbyData(lobbyByIndex, "map"), out i2))
				{
					mapName = LocalizationManager.GetTranslation(GameManager.GetMap(i2).name, true, 0, true, false, null, null, true);
				}
				int num5 = 0;
				if (int.TryParse(SteamMatchmaking.GetLobbyData(lobbyByIndex, "GameMode"), out num5))
				{
					mode = LocalizationManager.GetTranslation(GameManager.GameModeStrings[num5], true, 0, true, false, null, null, true);
				}
				lobbyEntry.Setup(num2, text, mapName, mode, SteamMatchmaking.GetNumLobbyMembers(lobbyByIndex).ToString() + "/" + SteamMatchmaking.GetLobbyMemberLimit(lobbyByIndex).ToString(), text2);
				num++;
			}
			num2++;
		}
		this.noGames.enabled = (num == 0);
	}

	// Token: 0x06002068 RID: 8296 RVA: 0x0000398C File Offset: 0x00001B8C
	private void OnDestroy()
	{
	}

	// Token: 0x0400232A RID: 9002
	public GameObject lobbyUIPrefab;

	// Token: 0x0400232B RID: 9003
	public Transform content;

	// Token: 0x0400232C RID: 9004
	public Toggle friendsOnlyToggle;

	// Token: 0x0400232D RID: 9005
	public Text noGames;

	// Token: 0x0400232E RID: 9006
	private List<LobbyEntry> lobbyEntries = new List<LobbyEntry>();

	// Token: 0x0400232F RID: 9007
	private List<string> uniqueLobbies = new List<string>();

	// Token: 0x04002330 RID: 9008
	protected Callback<LobbyMatchList_t> Callback_lobbyList;
}
