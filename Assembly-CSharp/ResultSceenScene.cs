using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000434 RID: 1076
public class ResultSceenScene : MonoBehaviour
{
	// Token: 0x06001DBA RID: 7610 RVA: 0x00015DEF File Offset: 0x00013FEF
	public void Start()
	{
		if (ResultSceenScene.NUM_TEST_PLAYERS != 0)
		{
			this.Show();
		}
	}

	// Token: 0x06001DBB RID: 7611 RVA: 0x000C1BF8 File Offset: 0x000BFDF8
	public void Update()
	{
		if (this.started)
		{
			float num = Mathf.Clamp01((Time.time - this.fade_start_time) / 0.5f);
			this.composite_post.brightness = Mathf.Lerp(1f, this.fade_brightness, num);
			this.composite_post.fade = num;
		}
	}

	// Token: 0x06001DBC RID: 7612 RVA: 0x00015DFE File Offset: 0x00013FFE
	public void Show()
	{
		this.results = GameManager.GetMinigameResults();
		base.StartCoroutine(this.Initialize());
		this.started = true;
	}

	// Token: 0x06001DBD RID: 7613 RVA: 0x00015E1F File Offset: 0x0001401F
	public void Hide()
	{
		this.started = false;
		this.root.SetActive(false);
		this.ReleaseRenderTexture();
		UnityEngine.Object.Destroy(this.result_screen.gameObject);
	}

	// Token: 0x06001DBE RID: 7614 RVA: 0x00015E4A File Offset: 0x0001404A
	public IEnumerator Initialize()
	{
		this.SetupRT(ref this.render_texture, this.cameraRef);
		this.composite_post = this.composite_camera.GetComponent<CompositeTexture>();
		this.composite_post.target_tex = this.render_texture;
		this.composite_post.zoom_time = 0f;
		this.composite_post.brightness = 1f;
		this.fade_start_time = Time.time + 0.5f;
		yield return new WaitForEndOfFrame();
		this.root.SetActive(true);
		Rect rect = GameViewHelper.GetRect();
		UnityEngine.Object.Destroy(this.screenshot);
		this.screenshot = null;
		this.screenshot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false, false);
		this.screenshot.filterMode = FilterMode.Point;
		this.ReadScreenshotTexture();
		this.composite_camera.GetComponent<Skybox>().material.SetTexture("_MainTex", this.screenshot);
		if (ResultSceenScene.NUM_TEST_PLAYERS == 0)
		{
			this.result_screen = GameManager.UIController.ShowMinigameResultScreen();
		}
		this.SetResults_Player();
		this.SetResults_UI();
		GameObject gameObject = GameObject.Find("MinigameRoot");
		if (gameObject != null)
		{
			AudioListener componentInChildren = gameObject.GetComponentInChildren<AudioListener>(true);
			if (componentInChildren != null)
			{
				componentInChildren.enabled = false;
			}
		}
		yield break;
	}

	// Token: 0x06001DBF RID: 7615 RVA: 0x00015E59 File Offset: 0x00014059
	private void SetupRT(ref RenderTexture texture, GameObject cam_obj)
	{
		Camera component = cam_obj.GetComponent<Camera>();
		texture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
		texture.name = "ResultScreenScene";
		texture.filterMode = FilterMode.Point;
		component.targetTexture = texture;
	}

	// Token: 0x06001DC0 RID: 7616 RVA: 0x000C1C50 File Offset: 0x000BFE50
	private void SetResults_Player()
	{
		int num;
		if (ResultSceenScene.NUM_TEST_PLAYERS != 0)
		{
			num = ResultSceenScene.NUM_TEST_PLAYERS - 1;
		}
		else
		{
			num = GameManager.GetPlayerCount() - 1;
		}
		if (num >= 4)
		{
			this.cameraRef.GetComponent<Camera>().fieldOfView = 25f;
		}
		PlayerResultPositions playerResultPositions = this.result_positions[num];
		for (int i = 0; i < 8; i++)
		{
			this.result_player_objs[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < num + 1; j++)
		{
			Vector3 localPosition = playerResultPositions.positions[j];
			this.result_player_objs[j].gameObject.SetActive(true);
			this.result_player_objs[j].transform.localPosition = localPosition;
		}
		for (int k = 0; k < this.results.players.Count; k++)
		{
			this.result_player_objs[k].SetPlayerSlot((int)this.results.players[k].GlobalID);
			this.result_player_objs[k].SetPosition(k);
			this.result_screen.SetupPlayer(k, this.results.players[k], this.results.placements[k], (short)this.results.gold[k], this.results.itemIDs[k]);
			if (this.results.placements[k] == 0)
			{
				StatTracker.IncrementStat(StatType.MinigamesWon, this.results.players[k].GlobalID, 1.0);
			}
			else
			{
				StatTracker.IncrementStat(StatType.MinigamesLost, this.results.players[k].GlobalID, 1.0);
			}
		}
		if (GameManager.partyGameMode == PartyGameMode.BoardGame)
		{
			for (int l = 0; l < this.results.players.Count; l++)
			{
				this.results.players[l].BoardObject.TurnOrderRoll = 8 - l;
			}
			if (ResultSceenScene.NUM_TEST_PLAYERS == 0)
			{
				GameManager.Board.SortTurnOrderList(0);
			}
		}
	}

	// Token: 0x06001DC1 RID: 7617 RVA: 0x000C1E58 File Offset: 0x000C0058
	private void SetResults_UI()
	{
		if (this.results.players[0].IsLocalPlayer && !this.results.players[0].IsAI)
		{
			PlatformAchievementManager.Instance.TriggerAchievement("ACH_WIN_FIRST_MINIGAME");
			GameManager.SetMinigameWon();
		}
		if (GameManager.GetPlayerCount() > 1)
		{
			int index = GameManager.GetPlayerCount() - 1;
			if (this.results.players[index].IsLocalPlayer && !this.results.players[index].IsAI)
			{
				GameManager.SetMinigameLost();
			}
		}
		GamePlayer gamePlayer = this.results.players[0];
		string text = ColorUtility.ToHtmlStringRGBA(gamePlayer.Color.uiColor);
		string translation = LocalizationManager.GetTranslation("Wins", true, 0, true, false, null, null, true);
		this.result_screen.SetWinnerText(string.Concat(new string[]
		{
			"<color=#",
			text,
			">",
			gamePlayer.Name,
			"</color> ",
			translation,
			"!"
		}));
		RBPrefs.Save();
	}

	// Token: 0x06001DC2 RID: 7618 RVA: 0x00015E90 File Offset: 0x00014090
	private void ReadScreenshotTexture()
	{
		this.screenshot.ReadPixels(GameViewHelper.GetRect(), 0, 0, false);
		this.screenshot.Apply();
	}

	// Token: 0x06001DC3 RID: 7619 RVA: 0x000C1F70 File Offset: 0x000C0170
	private T getOrAddComponent<T>(GameObject obj = null) where T : Component
	{
		if (obj == null)
		{
			obj = base.gameObject;
		}
		T t = obj.GetComponent<T>();
		if (t == null)
		{
			t = obj.AddComponent<T>();
		}
		return t;
	}

	// Token: 0x06001DC4 RID: 7620 RVA: 0x00015EB0 File Offset: 0x000140B0
	private void OnDestroy()
	{
		this.ReleaseRenderTexture();
	}

	// Token: 0x06001DC5 RID: 7621 RVA: 0x000C1FAC File Offset: 0x000C01AC
	private void ReleaseRenderTexture()
	{
		if (this.render_texture != null)
		{
			if (this.cameraRef != null)
			{
				Camera component = this.cameraRef.GetComponent<Camera>();
				if (component != null)
				{
					component.targetTexture = null;
				}
			}
			this.render_texture.Release();
			UnityEngine.Object.Destroy(this.render_texture);
		}
	}

	// Token: 0x04002084 RID: 8324
	public GameObject root;

	// Token: 0x04002085 RID: 8325
	public GameObject cameraRef;

	// Token: 0x04002086 RID: 8326
	public GameObject composite_camera;

	// Token: 0x04002087 RID: 8327
	public ScoreScreenPlayer[] result_player_objs;

	// Token: 0x04002088 RID: 8328
	public Vector3 loser_pos_offset = Vector3.zero;

	// Token: 0x04002089 RID: 8329
	public PlayerResultPositions[] result_positions;

	// Token: 0x0400208A RID: 8330
	public Texture2D screenshot;

	// Token: 0x0400208B RID: 8331
	private Image gui_tex;

	// Token: 0x0400208C RID: 8332
	private Camera cam;

	// Token: 0x0400208D RID: 8333
	private RenderTexture render_texture;

	// Token: 0x0400208E RID: 8334
	private CompositeTexture composite_post;

	// Token: 0x0400208F RID: 8335
	private bool started;

	// Token: 0x04002090 RID: 8336
	private float fade_start_time;

	// Token: 0x04002091 RID: 8337
	private float fade_brightness = 0.35f;

	// Token: 0x04002092 RID: 8338
	private MinigameResults results;

	// Token: 0x04002093 RID: 8339
	private UIMinigameResultScreen result_screen;

	// Token: 0x04002094 RID: 8340
	private List<GamePlayer> player_list;

	// Token: 0x04002095 RID: 8341
	private int t_width;

	// Token: 0x04002096 RID: 8342
	private int t_height;

	// Token: 0x04002097 RID: 8343
	public static int NUM_TEST_PLAYERS;
}
