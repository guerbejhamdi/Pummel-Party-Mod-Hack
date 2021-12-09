using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x0200051C RID: 1308
public class MinigameLoadingScreen : MonoBehaviour
{
	// Token: 0x0600221C RID: 8732 RVA: 0x00018B84 File Offset: 0x00016D84
	public void Awake()
	{
		this.m_gridLayout = this.playersPanel.GetComponent<GridLayoutGroup>();
	}

	// Token: 0x0600221D RID: 8733 RVA: 0x000D1370 File Offset: 0x000CF570
	public void Start()
	{
		if (!NetSystem.IsServer)
		{
			this.forceStartButton.SetState(BasicButtonBase.BasicButtonState.Disabled);
		}
		this.controlGroups = new MinigameLoadingScreen.ControlGroup[this.controlGroupParents.Length];
		for (int i = 0; i < this.controlGroups.Length; i++)
		{
			this.controlGroups[i] = new MinigameLoadingScreen.ControlGroup(this.controlGroupParents[i]);
		}
		int players = GameManager.GetLocalNonAIPlayers().Count - 1;
		this.ResizeContainers(players);
	}

	// Token: 0x0600221E RID: 8734 RVA: 0x000D13E4 File Offset: 0x000CF5E4
	private void ResizeContainers(int players)
	{
		if (players < 0 || players > this.m_controlsPosSize.Length - 1)
		{
			return;
		}
		Vector2 vector = this.m_controlsPosSize[players];
		Vector2 vector2 = this.m_instructionsPosSize[players];
		this.m_controlsContainer.anchoredPosition = new Vector2(vector.x, this.m_instructionsContainer.anchoredPosition.y);
		this.m_controlsContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, vector.y);
		this.m_instructionsContainer.anchoredPosition = new Vector2(vector2.x, this.m_instructionsContainer.anchoredPosition.y);
		this.m_instructionsContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, vector2.y);
	}

	// Token: 0x0600221F RID: 8735 RVA: 0x000D1490 File Offset: 0x000CF690
	public void SetMinigameInfo(MinigameDefinition minigameDef, MinigameAlternate alternate)
	{
		this.minigameName.text = LocalizationManager.GetTranslation(minigameDef.minigameNameToken, true, 0, true, false, null, null, true);
		this.minigameDesc.text = LocalizationManager.GetTranslation(minigameDef.descriptionToken, true, 0, true, false, null, null, true);
		if (alternate != null)
		{
			this.videoPlayer.Play(alternate.videoClipPath, alternate.screenshot);
		}
		else
		{
			this.videoPlayer.Play(minigameDef.videoClipPath, minigameDef.screenshot);
		}
		List<GamePlayer> localNonAIPlayers = GameManager.GetLocalNonAIPlayers();
		InputDetails[] keyboard = minigameDef.inputHelp.keyboard;
		for (int i = 0; i < keyboard.Length; i++)
		{
			keyboard[i].GetMapping();
		}
		this.controlsChanged.SetActive(minigameDef.controlsChanged);
		for (int j = 0; j < 8; j++)
		{
			if (j < localNonAIPlayers.Count)
			{
				this.controlTitleColors[j].gameObject.SetActive(true);
				this.controlTitleColors[j].color = localNonAIPlayers[j].Color.uiColor;
			}
			else
			{
				this.controlTitleColors[j].gameObject.SetActive(false);
			}
		}
		for (int k = 0; k < this.controlGroups.Length; k++)
		{
			this.controlGroups[k].root.SetActive(k < keyboard.Length);
		}
		for (int l = 0; l < keyboard.Length; l++)
		{
			if (this.controlGroups.Length - 1 < l)
			{
				Debug.LogError("Too many minigame inputs for UI Elements");
			}
			else
			{
				this.controlGroups[l].actionText.text = LocalizationManager.GetTranslation(keyboard[l].description, true, 0, true, false, null, null, true);
				for (int m = 0; m < 8; m++)
				{
					if (m < localNonAIPlayers.Count)
					{
						this.controlGroups[l].glyphs[m].gameObject.SetActive(true);
						this.controlGroups[l].glyphs[m].SetPlayer(localNonAIPlayers[m].RewiredPlayer);
						this.controlGroups[l].glyphs[m].SetValues(keyboard[l]);
					}
					else
					{
						this.controlGroups[l].glyphs[m].gameObject.SetActive(false);
					}
				}
			}
		}
		if (this.turnOrderNoteTxt != null)
		{
			this.turnOrderNoteTxt.SetActive(GameManager.partyGameMode == PartyGameMode.BoardGame);
		}
		this.m_gridLayout = this.playersPanel.GetComponent<GridLayoutGroup>();
		if (GameManager.GetPlayerCount() > 7)
		{
			this.m_gridLayout.cellSize = new Vector2(this.m_gridLayout.cellSize.x, 34f);
			this.m_gridLayout.spacing = new Vector2(0f, 3f);
			this.m_gridLayout.padding = new RectOffset(0, 0, 5, 0);
			this.m_gridLayout.enabled = true;
			return;
		}
		if (GameManager.GetPlayerCount() > 6)
		{
			this.m_gridLayout.cellSize = new Vector2(this.m_gridLayout.cellSize.x, 39f);
			this.m_gridLayout.spacing = new Vector2(0f, 3f);
			this.m_gridLayout.padding = new RectOffset(0, 0, 5, 0);
			this.m_gridLayout.enabled = true;
			return;
		}
		this.m_gridLayout.cellSize = new Vector2(this.m_gridLayout.cellSize.x, 40f);
		this.m_gridLayout.spacing = new Vector2(0f, 5f);
		this.m_gridLayout.padding = new RectOffset(0, 0, 18, 0);
		this.m_gridLayout.enabled = false;
	}

	// Token: 0x06002220 RID: 8736 RVA: 0x000D184C File Offset: 0x000CFA4C
	private void Update()
	{
		if (GameManager.Minigame != null)
		{
			List<GamePlayer> playerList = GameManager.PlayerList;
			for (int i = 0; i < playerList.Count; i++)
			{
				if (playerList[i].IsLocalPlayer && (playerList[i].IsAI || playerList[i].RewiredPlayer.GetButtonDown(InputActions.Accept)))
				{
					GameManager.Minigame.PlayerReady(playerList[i].GlobalID);
				}
			}
		}
	}

	// Token: 0x06002221 RID: 8737 RVA: 0x000D18C8 File Offset: 0x000CFAC8
	public void ResetPlayerStatus()
	{
		for (int i = 0; i < this.playerUIList.Count; i++)
		{
			this.playerUIList[i].SetPlayerStatus(PlayerMinigameLoadStatus.Loading);
		}
	}

	// Token: 0x06002222 RID: 8738 RVA: 0x000D1900 File Offset: 0x000CFB00
	public void SetupPlayerStatus()
	{
		for (int i = 0; i < GameManager.PlayerList.Count; i++)
		{
			if (this.playerUIList.Count == i)
			{
				this.AddPlayerStatus(GameManager.PlayerList[i], i);
			}
			else
			{
				this.playerUIList[i].Initialize(GameManager.PlayerList[i]);
			}
		}
	}

	// Token: 0x06002223 RID: 8739 RVA: 0x000D1964 File Offset: 0x000CFB64
	private UIMinigamePlayerStatus AddPlayerStatus(GamePlayer player, int index)
	{
		float num = -9f;
		float num2 = 40f;
		float num3 = 5f;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.UIStatusPfb);
		gameObject.transform.SetParent(this.playersPanel.transform, false);
		gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, num - (num2 + num3) * (float)index, 0f);
		UIMinigamePlayerStatus component = gameObject.GetComponent<UIMinigamePlayerStatus>();
		component.Initialize(player);
		this.playerUIList.Add(component);
		return component;
	}

	// Token: 0x06002224 RID: 8740 RVA: 0x00018B97 File Offset: 0x00016D97
	public void DoButtonEvent(MinigameLoadingScreenButtonEvent type)
	{
		if (type == MinigameLoadingScreenButtonEvent.ForceStart && GameManager.Minigame != null && GameManager.Minigame.AllClientsLoaded())
		{
			GameManager.Minigame.SetAllClientsReady();
		}
	}

	// Token: 0x06002225 RID: 8741 RVA: 0x00018BBF File Offset: 0x00016DBF
	public void SetFadeImage(bool val)
	{
		this.fadeImage.enabled = val;
	}

	// Token: 0x06002226 RID: 8742 RVA: 0x00018BCD File Offset: 0x00016DCD
	public void FadeOut(float time)
	{
		this.fadeImage.canvasRenderer.SetAlpha(1f);
		this.fadeImage.CrossFadeAlpha(0f, time, false);
	}

	// Token: 0x06002227 RID: 8743 RVA: 0x00018BF6 File Offset: 0x00016DF6
	public void FadeIn(float time)
	{
		this.fadeImage.canvasRenderer.SetAlpha(0f);
		this.fadeImage.CrossFadeAlpha(1f, time, false);
	}

	// Token: 0x06002228 RID: 8744 RVA: 0x000D19E8 File Offset: 0x000CFBE8
	public void ShowScreen(bool value)
	{
		if (value)
		{
			this.window.SetState(MainMenuWindowState.Visible);
		}
		else
		{
			this.window.SetState(MainMenuWindowState.Hidden);
		}
		this.menuBorderAnimator.SetBool("Hidden", !value);
		if (!value)
		{
			this.videoPlayer.Stop();
		}
	}

	// Token: 0x04002535 RID: 9525
	[Header("References")]
	public RawImage videoImg;

	// Token: 0x04002536 RID: 9526
	public AutoPlayVideo videoPlayer;

	// Token: 0x04002537 RID: 9527
	public Text minigameName;

	// Token: 0x04002538 RID: 9528
	public Text minigameDesc;

	// Token: 0x04002539 RID: 9529
	public MainMenuWindow window;

	// Token: 0x0400253A RID: 9530
	public Animator menuBorderAnimator;

	// Token: 0x0400253B RID: 9531
	public GameObject playersPanel;

	// Token: 0x0400253C RID: 9532
	public Image fadeImage;

	// Token: 0x0400253D RID: 9533
	public Transform[] controlGroupParents;

	// Token: 0x0400253E RID: 9534
	public Image[] controlTitleColors;

	// Token: 0x0400253F RID: 9535
	public MinigameLoadingScreenButton forceStartButton;

	// Token: 0x04002540 RID: 9536
	public GameObject turnOrderNoteTxt;

	// Token: 0x04002541 RID: 9537
	public GameObject controlsChanged;

	// Token: 0x04002542 RID: 9538
	public RectTransform m_controlsContainer;

	// Token: 0x04002543 RID: 9539
	public RectTransform m_instructionsContainer;

	// Token: 0x04002544 RID: 9540
	public Vector2[] m_controlsPosSize;

	// Token: 0x04002545 RID: 9541
	public Vector2[] m_instructionsPosSize;

	// Token: 0x04002546 RID: 9542
	[Header("Prefabs")]
	public GameObject UIStatusPfb;

	// Token: 0x04002547 RID: 9543
	private List<UIMinigamePlayerStatus> playerUIList = new List<UIMinigamePlayerStatus>();

	// Token: 0x04002548 RID: 9544
	private MinigameLoadingScreen.ControlGroup[] controlGroups;

	// Token: 0x04002549 RID: 9545
	private GridLayoutGroup m_gridLayout;

	// Token: 0x0200051D RID: 1309
	private struct ControlGroup
	{
		// Token: 0x0600222A RID: 8746 RVA: 0x000D1A34 File Offset: 0x000CFC34
		public ControlGroup(Transform parent)
		{
			this.root = parent.gameObject;
			this.actionText = parent.GetChild(0).GetComponentInChildren<Text>();
			this.glyphs = new UIGlyph[8];
			for (int i = 0; i < 8; i++)
			{
				this.glyphs[i] = parent.GetChild(i + 1).GetComponentInChildren<UIGlyph>();
			}
		}

		// Token: 0x0400254A RID: 9546
		public GameObject root;

		// Token: 0x0400254B RID: 9547
		public Text actionText;

		// Token: 0x0400254C RID: 9548
		public UIGlyph[] glyphs;
	}
}
