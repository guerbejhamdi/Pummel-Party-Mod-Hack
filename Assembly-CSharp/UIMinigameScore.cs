using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ZP.Utility;

// Token: 0x02000556 RID: 1366
public class UIMinigameScore : MonoBehaviour
{
	// Token: 0x060023F2 RID: 9202 RVA: 0x00019E5F File Offset: 0x0001805F
	public IEnumerator Start()
	{
		HorizontalLayoutGroup componentInParent = base.GetComponentInParent<HorizontalLayoutGroup>();
		if (componentInParent != null)
		{
			if (GameManager.GetPlayerCount() > 4)
			{
				GameObject gameObject = new GameObject("ScoreParentScaler");
				gameObject.AddComponent<RectTransform>();
				RectTransform rectTransform = (RectTransform)base.transform;
				RectTransform rectTransform2 = (RectTransform)gameObject.transform;
				rectTransform2.SetParent(rectTransform.parent, false);
				rectTransform2.anchoredPosition = rectTransform.anchoredPosition;
				rectTransform2.anchorMax = rectTransform.anchorMax;
				rectTransform2.anchorMin = rectTransform.anchorMin;
				rectTransform.SetParent(rectTransform2, false);
				rectTransform.anchoredPosition = new Vector2(0f, 0f);
				rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
				rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
				rectTransform2.localScale = new Vector3(0.75f, 0.75f, 0.75f);
				componentInParent.spacing = -15f;
				componentInParent.padding = new RectOffset(0, 0, 0, 10);
				componentInParent.childControlWidth = false;
				componentInParent.childControlHeight = false;
				rectTransform2.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 195f);
				rectTransform2.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50f);
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 195f);
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50f);
			}
			else
			{
				componentInParent.spacing = 150f;
				componentInParent.padding = new RectOffset(0, 0, 0, 0);
			}
		}
		for (;;)
		{
			if (this.currentScore != this.lastScore)
			{
				int num = Mathf.Min(Mathf.Abs(this.currentScore - this.lastScore), this.scoreUpdateSpeed);
				this.currentScore += ((this.currentScore > this.lastScore) ? (-num) : num);
				this.scoreText.text = this.currentScore.ToString();
			}
			yield return new WaitForSeconds(this.scoreUpdateInterval);
		}
		yield break;
	}

	// Token: 0x060023F3 RID: 9203 RVA: 0x000D8D0C File Offset: 0x000D6F0C
	public void Initialize(GamePlayer player)
	{
		Color uiColor = player.Color.uiColor;
		this.scoreText.text = "0";
		this.scoreText.color = uiColor;
		this.background.color = uiColor;
		this.portrait.uvRect = this.portraitUVs[(int)player.GlobalID];
		if (player.GlobalID >= 4)
		{
			this.portrait.texture = this.portraitAtlas2;
		}
	}

	// Token: 0x060023F4 RID: 9204 RVA: 0x000D8D84 File Offset: 0x000D6F84
	public void SetScore(int value)
	{
		if (value != this.lastScore && this.showChanges)
		{
			int num = value - this.lastScore;
			this.changeTotal += num;
			if (Mathf.Abs(this.changeTotal) >= this.minChangeText)
			{
				this.SpawnText(this.changeTotal);
				this.changeTotal = 0;
			}
		}
		this.lastScore = value;
	}

	// Token: 0x060023F5 RID: 9205 RVA: 0x000D8DE8 File Offset: 0x000D6FE8
	public void SpawnText(int value)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.scoreGainPrefab);
		gameObject.transform.SetParent(this.scoreText.transform.parent, false);
		float x = ZPMath.RandomFloat(GameManager.rand, -20f, 20f);
		gameObject.transform.localPosition = new Vector3(x, 45f, 0f);
		Text componentInChildren = gameObject.GetComponentInChildren<Text>();
		componentInChildren.text = ((value > 0) ? "+" : "") + value.ToString();
		componentInChildren.color = this.scoreText.color;
	}

	// Token: 0x040026FF RID: 9983
	public Image background;

	// Token: 0x04002700 RID: 9984
	public Text scoreText;

	// Token: 0x04002701 RID: 9985
	public RawImage portrait;

	// Token: 0x04002702 RID: 9986
	public GameObject scoreGainPrefab;

	// Token: 0x04002703 RID: 9987
	public int scoreUpdateSpeed = 1;

	// Token: 0x04002704 RID: 9988
	public bool showChanges = true;

	// Token: 0x04002705 RID: 9989
	public int minChangeText = 1;

	// Token: 0x04002706 RID: 9990
	public RenderTexture portraitAtlas2;

	// Token: 0x04002707 RID: 9991
	private int lastScore;

	// Token: 0x04002708 RID: 9992
	private int currentScore;

	// Token: 0x04002709 RID: 9993
	private float scoreUpdateInterval = 0.005f;

	// Token: 0x0400270A RID: 9994
	private Rect[] portraitUVs = new Rect[]
	{
		new Rect(0f, 0f, 0.25f, 1f),
		new Rect(0.25f, 0f, 0.25f, 1f),
		new Rect(0.5f, 0f, 0.25f, 1f),
		new Rect(0.75f, 0f, 0.25f, 1f),
		new Rect(0f, 0f, 0.25f, 1f),
		new Rect(0.25f, 0f, 0.25f, 1f),
		new Rect(0.5f, 0f, 0.25f, 1f),
		new Rect(0.75f, 0f, 0.25f, 1f)
	};

	// Token: 0x0400270B RID: 9995
	private int changeTotal;
}
