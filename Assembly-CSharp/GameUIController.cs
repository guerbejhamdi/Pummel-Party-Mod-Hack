using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020003FB RID: 1019
public class GameUIController : MonoBehaviour
{
	// Token: 0x17000338 RID: 824
	// (get) Token: 0x06001C12 RID: 7186 RVA: 0x0001489B File Offset: 0x00012A9B
	public InteractionDialog InteractionDialog
	{
		get
		{
			return this.interactionDialog;
		}
	}

	// Token: 0x17000339 RID: 825
	// (get) Token: 0x06001C13 RID: 7187 RVA: 0x000148A3 File Offset: 0x00012AA3
	public InventoryUI InventoryUI
	{
		get
		{
			return this.inventoryUI;
		}
	}

	// Token: 0x1700033A RID: 826
	// (get) Token: 0x06001C14 RID: 7188 RVA: 0x000148AB File Offset: 0x00012AAB
	public Transform MinigameUIRoot
	{
		get
		{
			return this.minigame_ui_root;
		}
	}

	// Token: 0x1700033B RID: 827
	// (get) Token: 0x06001C15 RID: 7189 RVA: 0x000148B3 File Offset: 0x00012AB3
	public MinigameLoadingScreen MinigameLoadScreen
	{
		get
		{
			return this.minigame_load_screen;
		}
	}

	// Token: 0x1700033C RID: 828
	// (get) Token: 0x06001C16 RID: 7190 RVA: 0x000148BB File Offset: 0x00012ABB
	public Canvas Canvas
	{
		get
		{
			return this.ui_canvas;
		}
	}

	// Token: 0x06001C17 RID: 7191 RVA: 0x000148C3 File Offset: 0x00012AC3
	private void OnApplicationFocus(bool focus)
	{
		Canvas.ForceUpdateCanvases();
		this.ui_camera;
	}

	// Token: 0x06001C18 RID: 7192 RVA: 0x000BB01C File Offset: 0x000B921C
	public void SetInputStatus(bool val)
	{
		val = true;
		EventSystem[] array = this.event_sys;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(val);
		}
	}

	// Token: 0x06001C19 RID: 7193 RVA: 0x000148D6 File Offset: 0x00012AD6
	public GameUIController()
	{
		this.world_text_list = new List<UIWorldText>();
	}

	// Token: 0x06001C1A RID: 7194 RVA: 0x000BB050 File Offset: 0x000B9250
	public void SetupWorldTextPool(int count)
	{
		for (int i = 0; i < count; i++)
		{
			UIWorldText component = UnityEngine.Object.Instantiate<GameObject>(this.world_text_pfb, Vector3.zero, Quaternion.identity, this.worldTextRoot).GetComponent<UIWorldText>();
			component.Active = false;
			this.world_text_list.Add(component);
		}
	}

	// Token: 0x06001C1B RID: 7195 RVA: 0x000BB0A0 File Offset: 0x000B92A0
	public void Initialize()
	{
		this.ui_canvas = base.GetComponent<Canvas>();
		this.ui_camera = base.GetComponentInChildren<Camera>();
		this.player_score_list = new List<UIPlayerScoreNew>();
		this.player_score_pfb = Resources.Load<GameObject>("Prefabs/UI/PlayerScoreUI");
		this.world_text_pfb = Resources.Load<GameObject>("Prefabs/UI/UIWorldText");
		this.countdown_text_pfb = Resources.Load<GameObject>("Prefabs/UI/UICountDownText");
		this.minigame_timer_pfb = Resources.Load<GameObject>("Prefabs/UI/UIMinigameTimer");
		this.minigame_score_pfb = Resources.Load<GameObject>("Prefabs/UI/UIMinigameScore");
		this.minigame_round_pfb = Resources.Load<GameObject>("Prefabs/UI/UIMinigameRound");
		this.minigame_health_pfb = Resources.Load<GameObject>("Prefabs/UI/MinigameHealthBar");
		this.minigame_score_screen_pfb = Resources.Load<GameObject>("Prefabs/UI/UIMinigameScoreScreen");
		this.minigame_choice_wnd_pfb = Resources.Load<GameObject>("Prefabs/UI/MinigameChoiceWindow");
		this.debug_text_pfb = Resources.Load<GameObject>("Prefabs/UI/DebugWorldText");
		this.event_sys = GameObject.Find("Input").GetComponentsInChildren<EventSystem>();
		if (GameManager.partyGameMode == PartyGameMode.BoardGame)
		{
			this.board_ui_root.gameObject.SetActive(true);
			this.minigameOnlyUIRoot.gameObject.SetActive(false);
		}
		else
		{
			this.board_ui_root.gameObject.SetActive(false);
			this.minigameOnlyUIRoot.gameObject.SetActive(true);
			this.minigameOnlySceneController = this.minigameOnlyUIRoot.GetComponent<MinigameOnlySceneController>();
		}
		this.minigame_ui_root = base.transform.Find("MinigameUI");
		this.minigameScoreUIRoot = this.minigame_ui_root.Find("ScoreParent");
		this.minigame_load_screen = base.transform.Find("MinigameLoadingScreen").GetComponent<MinigameLoadingScreen>();
		this.game_pause_screen = base.transform.Find("GamePauseScreen").GetComponent<UIPauseScreen>();
		if (GameManager.partyGameMode == PartyGameMode.BoardGame)
		{
			this.interactionDialog = base.gameObject.GetComponentInChildren<SimpleInteractionDialog>();
			this.inventoryUI = base.gameObject.GetComponentInChildren<InventoryUI>();
			this.score_board = this.board_ui_root.Find("ScoreBoard").gameObject;
			this.map_view_ui = this.board_ui_root.Find("MapViewUI").GetComponent<UIMapView>();
			this.map_view_ui.HideWindow();
			this.board_controls = this.board_ui_root.Find("BoardControls").GetComponent<UIBoardControls>();
			this.HideBoardControls();
		}
		this.game_pause_screen.HideScreen();
		this.turnOrderUI = base.GetComponentInChildren<UITurnOrder>();
	}

	// Token: 0x06001C1C RID: 7196 RVA: 0x000148E9 File Offset: 0x00012AE9
	public void OnDestroy()
	{
		AudioSystem.StopPooledSounds(0.5f);
	}

	// Token: 0x06001C1D RID: 7197 RVA: 0x000BB2EC File Offset: 0x000B94EC
	public void UpdateUI()
	{
		foreach (UIWorldText uiworldText in this.world_text_list)
		{
			if (uiworldText != null && uiworldText.Active)
			{
				uiworldText.UpdateText(Time.deltaTime * 2f);
			}
		}
		for (int i = this.world_text_list.Count - 1; i >= 0; i--)
		{
			if (this.world_text_list[i] != null && this.world_text_list[i].Active && !this.world_text_list[i].IsAlive && this.world_text_list[i] != null)
			{
				this.world_text_list[i].Active = false;
			}
		}
	}

	// Token: 0x06001C1E RID: 7198 RVA: 0x000148F5 File Offset: 0x00012AF5
	public void ShowPauseScreen()
	{
		this.game_pause_screen.ShowScreen();
	}

	// Token: 0x06001C1F RID: 7199 RVA: 0x00014902 File Offset: 0x00012B02
	public void HidePauseScreen()
	{
		this.game_pause_screen.HideScreen();
	}

	// Token: 0x06001C20 RID: 7200 RVA: 0x0001490F File Offset: 0x00012B0F
	public void EnableBoardUI()
	{
		this.SetCanvasState((GameManager.partyGameMode == PartyGameMode.BoardGame) ? this.board_ui_root : this.minigameOnlyUIRoot, true);
	}

	// Token: 0x06001C21 RID: 7201 RVA: 0x0001492D File Offset: 0x00012B2D
	public void DisableBoardUI()
	{
		this.SetCanvasState((GameManager.partyGameMode == PartyGameMode.BoardGame) ? this.board_ui_root : this.minigameOnlyUIRoot, false);
	}

	// Token: 0x06001C22 RID: 7202 RVA: 0x0001494B File Offset: 0x00012B4B
	private void SetCanvasState(Transform root, bool active)
	{
		this.BoardUIActive = active;
		root.gameObject.GetComponent<CanvasGroup>().interactable = active;
		root.gameObject.GetComponent<CanvasGroup>().alpha = (active ? 1f : 0f);
	}

	// Token: 0x06001C23 RID: 7203 RVA: 0x00014984 File Offset: 0x00012B84
	public void ShowBoardControls()
	{
		this.board_controls.ShowWindow();
	}

	// Token: 0x06001C24 RID: 7204 RVA: 0x00014991 File Offset: 0x00012B91
	public void HideBoardControls()
	{
		this.board_controls.HideWindow();
	}

	// Token: 0x06001C25 RID: 7205 RVA: 0x0001499E File Offset: 0x00012B9E
	public void SetBoardInputHelpType(BoardInputType input_type)
	{
		this.board_controls.SetInputHelp(input_type);
	}

	// Token: 0x06001C26 RID: 7206 RVA: 0x000149AC File Offset: 0x00012BAC
	public void SetBoardInputHelp(InputHelp inputHelp)
	{
		this.UpdateActionID(inputHelp.controller);
		this.UpdateActionID(inputHelp.keyboard);
		this.board_controls.SetInputHelp(inputHelp);
	}

	// Token: 0x06001C27 RID: 7207 RVA: 0x000BB3D4 File Offset: 0x000B95D4
	private void UpdateActionID(InputDetails[] details)
	{
		for (int i = 0; i < details.Length; i++)
		{
			details[i].GetMapping();
		}
	}

	// Token: 0x06001C28 RID: 7208 RVA: 0x000149D2 File Offset: 0x00012BD2
	public void SetInputPlayer(BoardPlayer player)
	{
		this.board_controls.SetPlayer(player);
	}

	// Token: 0x06001C29 RID: 7209 RVA: 0x000149E0 File Offset: 0x00012BE0
	public void ShowMapViewUI()
	{
		this.map_view_ui.ShowWindow();
	}

	// Token: 0x06001C2A RID: 7210 RVA: 0x000149ED File Offset: 0x00012BED
	public void HideMapViewUI()
	{
		this.map_view_ui.HideWindow();
	}

	// Token: 0x06001C2B RID: 7211 RVA: 0x000149FA File Offset: 0x00012BFA
	public void SetMapViewUITitle(string text)
	{
		this.map_view_ui.SetTitle(text);
	}

	// Token: 0x06001C2C RID: 7212 RVA: 0x00014A08 File Offset: 0x00012C08
	public void ShowScoreBoard()
	{
		this.score_board.gameObject.SetActive(true);
	}

	// Token: 0x06001C2D RID: 7213 RVA: 0x00014A1B File Offset: 0x00012C1B
	public void HideScoreBoard()
	{
		this.score_board.gameObject.SetActive(false);
	}

	// Token: 0x06001C2E RID: 7214 RVA: 0x000BB3FC File Offset: 0x000B95FC
	public void ClearMinigameUI()
	{
		int childCount = this.minigame_ui_root.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = this.minigame_ui_root.GetChild(i);
			if (child.gameObject.name != "ScoreParent")
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
			else
			{
				for (int j = 0; j < child.childCount; j++)
				{
					UnityEngine.Object.Destroy(child.GetChild(j).gameObject);
				}
			}
		}
	}

	// Token: 0x06001C2F RID: 7215 RVA: 0x000BB474 File Offset: 0x000B9674
	public UIPlayerScoreNew AddPlayerScoreUI(BoardPlayer player)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.player_score_pfb);
		gameObject.transform.SetParent(this.score_board.transform, false);
		UIPlayerScoreNew component = gameObject.GetComponent<UIPlayerScoreNew>();
		component.Setup(player, this.player_score_list.Count);
		this.player_score_list.Add(component);
		return component;
	}

	// Token: 0x06001C30 RID: 7216 RVA: 0x000BB4C8 File Offset: 0x000B96C8
	public void SpawnWorldText(string text, Vector3 position, float life, WorldTextType type, float maxOffset = 0f, Camera customCamera = null)
	{
		WorldTextData worldTextData = GameManager.GetWorldTextData(type);
		if (worldTextData != null)
		{
			Vector3 b = UnityEngine.Random.onUnitSphere * maxOffset;
			UIWorldText free = this.GetFree();
			free.Initialize(text, position + b, life, worldTextData, this.worldTextRoot);
			free.UpdateText(0f);
			if (customCamera != null)
			{
				free.SetCamera(customCamera);
			}
		}
	}

	// Token: 0x06001C31 RID: 7217 RVA: 0x000BB528 File Offset: 0x000B9728
	private UIWorldText GetFree()
	{
		for (int i = 0; i < this.world_text_list.Count; i++)
		{
			if (!this.world_text_list[i].Active)
			{
				return this.world_text_list[i];
			}
		}
		float num = float.MinValue;
		int index = -1;
		for (int j = 0; j < this.world_text_list.Count; j++)
		{
			if (this.world_text_list[j].TimeAlive > num)
			{
				num = this.world_text_list[j].TimeAlive;
				index = j;
			}
		}
		return this.world_text_list[index];
	}

	// Token: 0x06001C32 RID: 7218 RVA: 0x00014A2E File Offset: 0x00012C2E
	public DebugWorldText CreateDebugWorldText()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.debug_text_pfb);
		gameObject.transform.SetParent(this.minigame_ui_root, false);
		return gameObject.GetComponent<DebugWorldText>();
	}

	// Token: 0x06001C33 RID: 7219 RVA: 0x000BB5C0 File Offset: 0x000B97C0
	public void ShowLargeText(string text, LargeTextType text_type, float life = 3f, bool forMinigame = false, bool putMid = false)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.large_text_pfb);
		Transform parent = forMinigame ? this.MinigameUIRoot : this.board_ui_root;
		gameObject.transform.SetParent(parent, false);
		gameObject.GetComponent<UILargeText>().Init(life, text, text_type);
		if (putMid)
		{
			((RectTransform)gameObject.transform).anchoredPosition = new Vector2(0f, 100f);
			((RectTransform)gameObject.transform).anchorMin = new Vector2(0.5f, 0.5f);
			((RectTransform)gameObject.transform).anchorMax = new Vector2(0.5f, 0.5f);
			return;
		}
		((RectTransform)gameObject.transform).anchoredPosition = new Vector2(0f, -150f);
	}

	// Token: 0x06001C34 RID: 7220 RVA: 0x000BB688 File Offset: 0x000B9888
	public void ShowCountdownText(string text, float life)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.countdown_text_pfb);
		UnityEngine.Object.Destroy(gameObject, life);
		gameObject.transform.SetParent(this.minigame_ui_root, false);
		gameObject.GetComponent<UICountdownText>().Init(text);
		((RectTransform)gameObject.transform).anchoredPosition = new Vector2(0f, 0f);
	}

	// Token: 0x06001C35 RID: 7221 RVA: 0x000BB6E4 File Offset: 0x000B98E4
	public UIMinigameTimer CreateMinigameTimer(UIAnchorType anchor_type, Vector2 offset)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.minigame_timer_pfb);
		gameObject.transform.SetParent(this.minigame_ui_root, false);
		UIMinigameTimer component = gameObject.GetComponent<UIMinigameTimer>();
		RectTransform rect = (RectTransform)gameObject.transform;
		this.SetAnchor(anchor_type, rect, offset);
		return component;
	}

	// Token: 0x06001C36 RID: 7222 RVA: 0x000BB72C File Offset: 0x000B992C
	public UIMinigameScore CreateMinigameScore(UIAnchorType anchor_type, Vector2 offset, bool forceAnchor = false)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.minigame_score_pfb);
		gameObject.transform.SetParent(this.minigameScoreUIRoot, false);
		UIMinigameScore component = gameObject.GetComponent<UIMinigameScore>();
		if (forceAnchor)
		{
			RectTransform rect = (RectTransform)gameObject.transform;
			this.SetAnchor(anchor_type, rect, offset);
		}
		return component;
	}

	// Token: 0x06001C37 RID: 7223 RVA: 0x00014A52 File Offset: 0x00012C52
	public void SetScoreAnchorType(TextAnchor anchor, RectOffset padding)
	{
		HorizontalLayoutGroup component = this.minigameScoreUIRoot.GetComponent<HorizontalLayoutGroup>();
		component.childAlignment = anchor;
		component.padding = padding;
	}

	// Token: 0x06001C38 RID: 7224 RVA: 0x00014A6C File Offset: 0x00012C6C
	public UIMinigameHealthBar CreateHealthBar(Transform target, float height, Camera cam)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.minigame_health_pfb);
		gameObject.transform.SetParent(this.MinigameUIRoot, false);
		UIMinigameHealthBar component = gameObject.GetComponent<UIMinigameHealthBar>();
		component.Initialize(target, height, cam);
		return component;
	}

	// Token: 0x06001C39 RID: 7225 RVA: 0x00014A99 File Offset: 0x00012C99
	public void AddCustomUIObject(GameObject uiObj)
	{
		uiObj.transform.SetParent(this.MinigameUIRoot, false);
	}

	// Token: 0x06001C3A RID: 7226 RVA: 0x00014AAD File Offset: 0x00012CAD
	public UIRounds CreateMinigameRoundsUI()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.minigame_round_pfb);
		gameObject.transform.SetParent(this.minigame_ui_root, false);
		return gameObject.GetComponent<UIRounds>();
	}

	// Token: 0x06001C3B RID: 7227 RVA: 0x00014AD1 File Offset: 0x00012CD1
	public UIMinigameResultScreen ShowMinigameResultScreen()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.minigame_score_screen_pfb);
		gameObject.transform.SetParent(this.minigame_ui_root, false);
		return gameObject.GetComponent<UIMinigameResultScreen>();
	}

	// Token: 0x06001C3C RID: 7228 RVA: 0x00014AF5 File Offset: 0x00012CF5
	public MinigameChoiceWindow ShowMinigameChoiceWindow()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.minigame_choice_wnd_pfb);
		gameObject.transform.SetParent(this.board_ui_root, false);
		return gameObject.GetComponent<MinigameChoiceWindow>();
	}

	// Token: 0x06001C3D RID: 7229 RVA: 0x000BB778 File Offset: 0x000B9978
	private void SetAnchor(UIAnchorType anchor_type, RectTransform rect, Vector2 offset)
	{
		rect.pivot = new Vector2(0.5f, 0.5f);
		switch (anchor_type)
		{
		case UIAnchorType.TopLeft:
			rect.anchorMin = new Vector2(0f, 1f);
			rect.anchorMax = new Vector2(0f, 1f);
			rect.anchoredPosition = new Vector2(offset.x, -offset.y);
			return;
		case UIAnchorType.TopRight:
			rect.anchorMin = new Vector2(1f, 1f);
			rect.anchorMax = new Vector2(1f, 1f);
			rect.anchoredPosition = new Vector2(-offset.x, -offset.y);
			return;
		case UIAnchorType.BottomLeft:
			rect.anchorMin = new Vector2(0f, 0f);
			rect.anchorMax = new Vector2(0f, 0f);
			rect.anchoredPosition = new Vector2(offset.x, offset.y);
			return;
		case UIAnchorType.BottomRight:
			rect.anchorMin = new Vector2(1f, 0f);
			rect.anchorMax = new Vector2(1f, 0f);
			rect.anchoredPosition = new Vector2(-offset.x, offset.y);
			return;
		case UIAnchorType.Top:
			rect.anchorMin = new Vector2(0.5f, 0f);
			rect.anchorMax = new Vector2(0.5f, 0f);
			rect.anchoredPosition = new Vector2(offset.x, -offset.y);
			return;
		default:
			Debug.LogWarning("Anchor Type [" + anchor_type.ToString() + "] not implemented");
			return;
		}
	}

	// Token: 0x06001C3E RID: 7230 RVA: 0x0000398C File Offset: 0x00001B8C
	public void UpdateItems()
	{
	}

	// Token: 0x06001C3F RID: 7231 RVA: 0x000BB92C File Offset: 0x000B9B2C
	public void UpdateScores()
	{
		List<UIPlayerScoreNew> list = new List<UIPlayerScoreNew>();
		for (int i = 0; i < this.player_score_list.Count; i++)
		{
			if (list.Count == 0)
			{
				list.Add(this.player_score_list[i]);
			}
			else
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (this.player_score_list[i].Player.GoalScore > list[j].Player.GoalScore || (this.player_score_list[i].Player.GoalScore == list[j].Player.GoalScore && this.player_score_list[i].Player.Gold > list[j].Player.Gold))
					{
						list.Insert(j, this.player_score_list[i]);
						break;
					}
					if (j == list.Count - 1)
					{
						list.Add(this.player_score_list[i]);
						break;
					}
				}
			}
		}
		for (int k = 0; k < list.Count; k++)
		{
			list[k].UpdateStats(k);
		}
	}

	// Token: 0x06001C40 RID: 7232 RVA: 0x00014B19 File Offset: 0x00012D19
	public void DoButtonEvent(MainMenuButtonEventType btn_event)
	{
		GameManager.OptionsWindow.DoButtonEvent(btn_event);
	}

	// Token: 0x04001E3B RID: 7739
	public GameObject large_text_pfb;

	// Token: 0x04001E3C RID: 7740
	public UITurnTimer turnTimer;

	// Token: 0x04001E3D RID: 7741
	public GameObject boardDetails;

	// Token: 0x04001E3E RID: 7742
	public PlayerDisconnectUIMessage playerDisconnectUI;

	// Token: 0x04001E3F RID: 7743
	public ItemMinigameSelection minigameSelection;

	// Token: 0x04001E40 RID: 7744
	[Header("World Text")]
	public RectTransform worldTextRoot;

	// Token: 0x04001E41 RID: 7745
	private GameObject score_board;

	// Token: 0x04001E42 RID: 7746
	private List<UIPlayerScoreNew> player_score_list;

	// Token: 0x04001E43 RID: 7747
	private GameObject player_score_pfb;

	// Token: 0x04001E44 RID: 7748
	private GameObject world_text_pfb;

	// Token: 0x04001E45 RID: 7749
	private GameObject countdown_text_pfb;

	// Token: 0x04001E46 RID: 7750
	private GameObject minigame_timer_pfb;

	// Token: 0x04001E47 RID: 7751
	private GameObject minigame_score_pfb;

	// Token: 0x04001E48 RID: 7752
	private GameObject minigame_round_pfb;

	// Token: 0x04001E49 RID: 7753
	private GameObject minigame_health_pfb;

	// Token: 0x04001E4A RID: 7754
	private GameObject minigame_score_screen_pfb;

	// Token: 0x04001E4B RID: 7755
	private GameObject debug_text_pfb;

	// Token: 0x04001E4C RID: 7756
	private GameObject minigame_choice_wnd_pfb;

	// Token: 0x04001E4D RID: 7757
	public Transform minigameOnlyUIRoot;

	// Token: 0x04001E4E RID: 7758
	public Transform board_ui_root;

	// Token: 0x04001E4F RID: 7759
	private Transform minigame_ui_root;

	// Token: 0x04001E50 RID: 7760
	private Transform minigameScoreUIRoot;

	// Token: 0x04001E51 RID: 7761
	private UILoadingScreen loading_screen;

	// Token: 0x04001E52 RID: 7762
	private MinigameLoadingScreen minigame_load_screen;

	// Token: 0x04001E53 RID: 7763
	private UIPauseScreen game_pause_screen;

	// Token: 0x04001E54 RID: 7764
	private UIBoardControls board_controls;

	// Token: 0x04001E55 RID: 7765
	private UIMapView map_view_ui;

	// Token: 0x04001E56 RID: 7766
	private InteractionDialog interactionDialog;

	// Token: 0x04001E57 RID: 7767
	private InventoryUI inventoryUI;

	// Token: 0x04001E58 RID: 7768
	private List<UIWorldText> world_text_list;

	// Token: 0x04001E59 RID: 7769
	private Canvas ui_canvas;

	// Token: 0x04001E5A RID: 7770
	private Camera ui_camera;

	// Token: 0x04001E5B RID: 7771
	private EventSystem[] event_sys;

	// Token: 0x04001E5C RID: 7772
	public MinigameOnlySceneController minigameOnlySceneController;

	// Token: 0x04001E5D RID: 7773
	public UITurnOrder turnOrderUI;

	// Token: 0x04001E5E RID: 7774
	private bool BoardUIActive;
}
