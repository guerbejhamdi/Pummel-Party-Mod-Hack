using System;
using System.Collections;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x02000138 RID: 312
public class MinigameOnlySceneController : MonoBehaviour
{
	// Token: 0x060008EE RID: 2286 RVA: 0x00050D28 File Offset: 0x0004EF28
	private void Start()
	{
		if (GameManager.partyGameMode != PartyGameMode.MinigamesOnly)
		{
			return;
		}
		this.refs = GameManager.BoardRoot.transform.GetComponent<MinigameOnlyReferences>();
		this.skipButton.SetState(NetSystem.IsServer ? BasicButtonBase.BasicButtonState.Enabled : BasicButtonBase.BasicButtonState.Disabled);
		int num = GameManager.GetPlayerCount() - 1;
		this.playerCam.transform.localPosition = this.uiSettings[num].cameraPos;
		this.playerCam.orthographicSize = this.uiSettings[num].cameraOrthoScale;
		for (int i = 0; i < 8; i++)
		{
			if (i >= GameManager.GetPlayerCount())
			{
				this.refs.parents[i].gameObject.SetActive(false);
				this.refs.multiplayerLobbyScene.GetPlayer(i).gameObject.SetActive(false);
				this.refs.pedestalParents[i].gameObject.SetActive(false);
				this.connectionSlots[i].SetActive(false);
				this.scoreSlots[i].SetActive(false);
				this.connectionGroup.cellSize = new Vector2(this.uiSettings[num].cellSize, this.connectionGroup.cellSize.y);
				this.scoreGroup.cellSize = new Vector2(this.uiSettings[num].cellSize, this.scoreGroup.cellSize.y);
			}
			else
			{
				this.refs.parents[i].gameObject.SetActive(true);
				float scale = this.uiSettings[num].scale;
				this.refs.parents[i].localScale = new Vector3(scale, scale, scale);
				this.connectionSlots[i].SetActive(true);
				this.scoreSlots[i].SetActive(true);
				GamePlayer playerAt = GameManager.GetPlayerAt(i);
				this.refs.multiplayerLobbyScene.GetPlayer(i).gameObject.SetActive(true);
				this.refs.multiplayerLobbyScene.GetPlayer(i).SetPlayerSlot(i);
				this.refs.renderers[i].sharedMaterial = new Material(this.refs.renderers[i].sharedMaterial);
				this.refs.renderers[i].sharedMaterial.color = playerAt.Color.skinColor1;
				this.playerNames[i].text = playerAt.Name;
				Image[] array = null;
				switch (i)
				{
				case 0:
					array = this.playerImages1;
					break;
				case 1:
					array = this.playerImages2;
					break;
				case 2:
					array = this.playerImages3;
					break;
				case 3:
					array = this.playerImages4;
					break;
				case 4:
					array = this.playerImages5;
					break;
				case 5:
					array = this.playerImages6;
					break;
				case 6:
					array = this.playerImages7;
					break;
				case 7:
					array = this.playerImages8;
					break;
				}
				for (int j = 0; j < array.Length; j++)
				{
					array[j].color = playerAt.Color.skinColor1;
				}
			}
		}
		this.refs.multiplayerLobbyScene.Show();
		this.borderAnimator.SetBool("Hidden", false);
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x0000A0A5 File Offset: 0x000082A5
	public void AddScore(int index, int score)
	{
		this.scores[index] += score;
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x00051040 File Offset: 0x0004F240
	public void SetNextMinigame(MinigameDefinition md)
	{
		if (GameManager.Board.CurnTurnNum == GameManager.MinigameModeCount)
		{
			this.nextMinigameText.text = LocalizationManager.GetTranslation("GamePlay_Finished", true, 0, true, false, null, null, true) + "!";
			this.nextMinigameDescriptionText.text = "";
			this.video.Play("", null);
			return;
		}
		if (md != null)
		{
			this.nextMinigame = md;
			this.nextMinigameText.text = LocalizationManager.GetTranslation("GamePlay_NextMinigame", true, 0, true, false, null, null, true) + ": " + LocalizationManager.GetTranslation(this.nextMinigame.minigameNameToken, true, 0, true, false, null, null, true);
			this.nextMinigameDescriptionText.text = LocalizationManager.GetTranslation(this.nextMinigame.descriptionToken, true, 0, true, false, null, null, true);
			this.video.Play(this.nextMinigame.videoClipPath, this.nextMinigame.screenshot);
		}
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x0005113C File Offset: 0x0004F33C
	public void SetTime(float time)
	{
		this.minigameCount.text = GameManager.Board.CurnTurnNum.ToString() + "/" + GameManager.MinigameModeCount.ToString();
		this.timer.time_test = time;
		base.StartCoroutine(this.UpdateScores());
		if (this.ticks != null)
		{
			base.StopCoroutine(this.ticks);
		}
		this.ticks = base.StartCoroutine(this.TimerTicks(time));
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x0000A0B8 File Offset: 0x000082B8
	private IEnumerator TimerTicks(float time)
	{
		int i = 0;
		while ((float)i < time / 1f)
		{
			if (time - (float)i * 1f <= 5f)
			{
				AudioSystem.PlayOneShot("ButtonPress01_SFXR", 0.5f, 0f);
			}
			yield return new WaitForSeconds(1f);
			int num = i + 1;
			i = num;
		}
		yield break;
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x0000A0C7 File Offset: 0x000082C7
	public void Hide()
	{
		base.StartCoroutine(this.HideRoutine());
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x0000A0D6 File Offset: 0x000082D6
	public IEnumerator HideRoutine()
	{
		float alpha = 1f;
		while (alpha != 0f)
		{
			alpha = Mathf.Clamp01(alpha - 1f * Time.deltaTime);
			this.canvasGroup.alpha = alpha;
			yield return null;
		}
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		yield break;
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x0000A0E5 File Offset: 0x000082E5
	public void Show()
	{
		base.StartCoroutine(this.ShowRoutine());
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x0000A0F4 File Offset: 0x000082F4
	public IEnumerator ShowRoutine()
	{
		float alpha = 0f;
		while (alpha != 1f)
		{
			alpha = Mathf.Clamp01(alpha + 2f * Time.deltaTime);
			yield return null;
		}
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
		yield break;
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x0000A103 File Offset: 0x00008303
	private IEnumerator UpdateScores()
	{
		yield return new WaitForSeconds(2f);
		for (;;)
		{
			for (int i = 0; i < 8; i++)
			{
				this.currentScore[i] = Mathf.MoveTowards(this.currentScore[i], (float)this.scores[i], 10f * Time.deltaTime);
				this.scoreTexts[i].text = ((int)this.currentScore[i]).ToString();
				float t = Mathf.Clamp01(this.currentScore[i] / this.maxScore);
				Vector3 localScale = this.refs.pedestalParents[i].localScale;
				localScale.y = Mathf.Lerp(this.baseScale, this.maxScale, t);
				this.refs.pedestalParents[i].localScale = localScale;
				Vector3 position = this.refs.playerTransforms[i].position;
				position.y = this.refs.pedestalParents[i].transform.position.y + this.refs.pedestalParents[i].lossyScale.y;
				this.refs.playerTransforms[i].position = position;
			}
			bool flag = true;
			for (int j = 0; j < 8; j++)
			{
				if (this.currentScore[j] != (float)this.scores[j])
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x0000A112 File Offset: 0x00008312
	public void Skip()
	{
		this.timer.time_test = 1.4f;
		if (this.ticks != null)
		{
			base.StopCoroutine(this.ticks);
		}
	}

	// Token: 0x04000755 RID: 1877
	public CanvasGroup canvasGroup;

	// Token: 0x04000756 RID: 1878
	public Animator borderAnimator;

	// Token: 0x04000757 RID: 1879
	public RawImage playerImage;

	// Token: 0x04000758 RID: 1880
	public float baseScale = 0.1f;

	// Token: 0x04000759 RID: 1881
	public float maxScale = 1.3f;

	// Token: 0x0400075A RID: 1882
	public float maxScore = 300f;

	// Token: 0x0400075B RID: 1883
	public Text[] scoreTexts;

	// Token: 0x0400075C RID: 1884
	public RawImage renderImage;

	// Token: 0x0400075D RID: 1885
	public MinigameDefinition nextMinigame;

	// Token: 0x0400075E RID: 1886
	public Text nextMinigameText;

	// Token: 0x0400075F RID: 1887
	public Text nextMinigameDescriptionText;

	// Token: 0x04000760 RID: 1888
	public AutoPlayVideo video;

	// Token: 0x04000761 RID: 1889
	public UIMinigameTimer timer;

	// Token: 0x04000762 RID: 1890
	public Text minigameCount;

	// Token: 0x04000763 RID: 1891
	public BasicButton skipButton;

	// Token: 0x04000764 RID: 1892
	public Text[] playerNames;

	// Token: 0x04000765 RID: 1893
	public Image[] playerImages1;

	// Token: 0x04000766 RID: 1894
	public Image[] playerImages2;

	// Token: 0x04000767 RID: 1895
	public Image[] playerImages3;

	// Token: 0x04000768 RID: 1896
	public Image[] playerImages4;

	// Token: 0x04000769 RID: 1897
	public Image[] playerImages5;

	// Token: 0x0400076A RID: 1898
	public Image[] playerImages6;

	// Token: 0x0400076B RID: 1899
	public Image[] playerImages7;

	// Token: 0x0400076C RID: 1900
	public Image[] playerImages8;

	// Token: 0x0400076D RID: 1901
	public int[] scores = new int[8];

	// Token: 0x0400076E RID: 1902
	private float[] currentScore = new float[8];

	// Token: 0x0400076F RID: 1903
	private MinigameOnlyReferences refs;

	// Token: 0x04000770 RID: 1904
	[Header("References")]
	public GameObject[] connectionSlots;

	// Token: 0x04000771 RID: 1905
	public GameObject[] scoreSlots;

	// Token: 0x04000772 RID: 1906
	public GridLayoutGroup connectionGroup;

	// Token: 0x04000773 RID: 1907
	public GridLayoutGroup scoreGroup;

	// Token: 0x04000774 RID: 1908
	public Camera playerCam;

	// Token: 0x04000775 RID: 1909
	public MinigamesOnlyPlayersUISettings[] uiSettings;

	// Token: 0x04000776 RID: 1910
	private Coroutine ticks;
}
