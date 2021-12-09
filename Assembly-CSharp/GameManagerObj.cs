using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020003F1 RID: 1009
public class GameManagerObj : MonoBehaviour
{
	// Token: 0x06001BB8 RID: 7096 RVA: 0x0000398C File Offset: 0x00001B8C
	private void OnGUI()
	{
	}

	// Token: 0x06001BB9 RID: 7097 RVA: 0x000BA468 File Offset: 0x000B8668
	private void Awake()
	{
		RBPrefs.Load();
		Debug.Log("Build Time: " + BuildTime.buildTime.ToString("G"));
		if (this.dont_destroy_on_load)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		GameManager.Awake();
		GameManager.gameManagerObj = this;
		for (int i = 0; i < this.world_text_curves.Length; i++)
		{
			GameManager.SetTextData(this.world_text_curves[i]);
		}
		List<PlayerColor> list = new List<PlayerColor>();
		for (int j = 0; j < this.playerColors.Length; j++)
		{
			if (this.playerColors[j].enabled)
			{
				list.Add(this.playerColors[j]);
			}
		}
		GameManager.ItemList = this.itemList;
		this.OnLoad();
		GameManager.SetupRulesets();
		GameManager.SetMapData(this.maps);
		GameManager.SetPlayerColors(list.ToArray());
		GameManager.SetPlayerSkins(this.playerSkins);
		GameManager.SetPlayerHats(this.playerHats);
		GameManager.SetSceneSwitcher(GameObject.Find("SceneSwitcher").GetComponent<SceneSwitcher>());
		GameManager.FallbackMinigame = this.m_fallbackMinigame;
		SceneManager.sceneLoaded += this.OnSceneLoaded;
		GameManager.SetGlobalPlayerEmission(1f);
		GameManager.PlatformHelper = UnityEngine.Object.FindObjectOfType<PlatformManagerHelper>();
	}

	// Token: 0x06001BBA RID: 7098 RVA: 0x000144B0 File Offset: 0x000126B0
	private void OnLoad()
	{
		GameManager.LoadGameData(this.minigames, this.modifiers);
	}

	// Token: 0x06001BBB RID: 7099 RVA: 0x000144C3 File Offset: 0x000126C3
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "MainMenu")
		{
			GameManager.MainMenuLoaded = true;
			if (!this.firstTime)
			{
				GameManager.Reset();
				return;
			}
			this.firstTime = false;
		}
	}

	// Token: 0x06001BBC RID: 7100 RVA: 0x000144F3 File Offset: 0x000126F3
	private void Start()
	{
		Debug.Log("GameManager Start");
		Settings.ApplySettings();
		SceneManager.LoadScene("MainMenu");
		SteamUserStats.RequestCurrentStats();
	}

	// Token: 0x06001BBD RID: 7101 RVA: 0x00014514 File Offset: 0x00012714
	private void Update()
	{
		GameManager.Update();
		ThreadManager.Update();
	}

	// Token: 0x06001BBE RID: 7102 RVA: 0x0000398C File Offset: 0x00001B8C
	private void OnApplicationFocus(bool focus)
	{
	}

	// Token: 0x06001BBF RID: 7103 RVA: 0x00014520 File Offset: 0x00012720
	private void OnApplicationQuit()
	{
		if (GameManager.OnQuit != null)
		{
			GameManager.OnQuit();
		}
	}

	// Token: 0x06001BC0 RID: 7104 RVA: 0x00014520 File Offset: 0x00012720
	public void OnDestroy()
	{
		if (GameManager.OnQuit != null)
		{
			GameManager.OnQuit();
		}
	}

	// Token: 0x06001BC1 RID: 7105 RVA: 0x00014533 File Offset: 0x00012733
	public void DelayedCall(DelayedCall callback, float delay)
	{
		base.StartCoroutine(this.Delayed(callback, delay));
	}

	// Token: 0x06001BC2 RID: 7106 RVA: 0x00014544 File Offset: 0x00012744
	private IEnumerator Delayed(DelayedCall callback, float delay)
	{
		yield return new WaitForSeconds(delay);
		callback();
		yield break;
	}

	// Token: 0x06001BC3 RID: 7107 RVA: 0x0001455A File Offset: 0x0001275A
	public void DoCoroutine(IEnumerator routine)
	{
		base.StartCoroutine(routine);
	}

	// Token: 0x04001DDF RID: 7647
	public bool dont_destroy_on_load;

	// Token: 0x04001DE0 RID: 7648
	public WorldTextData[] world_text_curves;

	// Token: 0x04001DE1 RID: 7649
	public PlayerColor[] playerColors;

	// Token: 0x04001DE2 RID: 7650
	public CharacterSkin[] playerSkins;

	// Token: 0x04001DE3 RID: 7651
	public CharacterHat[] playerHats;

	// Token: 0x04001DE4 RID: 7652
	public Color[] player_colors = new Color[8];

	// Token: 0x04001DE5 RID: 7653
	public MapDetails[] maps;

	// Token: 0x04001DE6 RID: 7654
	public ItemList itemList;

	// Token: 0x04001DE7 RID: 7655
	public MinigameDefinition[] minigames;

	// Token: 0x04001DE8 RID: 7656
	public GameModifierDefinition[] modifiers;

	// Token: 0x04001DE9 RID: 7657
	[SerializeField]
	private MinigameDefinition m_fallbackMinigame;

	// Token: 0x04001DEA RID: 7658
	private bool firstTime = true;
}
