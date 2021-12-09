using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000567 RID: 1383
public class UIRounds : MonoBehaviour
{
	// Token: 0x06002469 RID: 9321 RVA: 0x000DAC34 File Offset: 0x000D8E34
	public void Setup(int roundCount)
	{
		float num = (float)roundCount;
		this.images = new Image[roundCount];
		this.animators = new Animator[roundCount];
		float num2 = -((num * this.width + (num - 1f) * this.space) / 2f);
		int num3 = 0;
		while ((float)num3 < num)
		{
			num2 += this.width / 2f;
			GameObject gameObject = new GameObject("Name", new Type[]
			{
				typeof(Image),
				typeof(Shadow),
				typeof(Animator)
			});
			Image component = gameObject.GetComponent<Image>();
			this.images[num3] = component;
			component.color = ((num3 == 0) ? this.curRoundColor : this.backgroundColor);
			gameObject.GetComponent<Shadow>().effectColor = this.shadowColor;
			gameObject.transform.SetParent(base.transform, false);
			RectTransform component2 = gameObject.GetComponent<RectTransform>();
			component2.anchorMin = new Vector2(0.5f, 1f);
			component2.anchorMax = new Vector2(0.5f, 1f);
			component2.pivot = new Vector2(0.5f, 0.5f);
			component2.sizeDelta = new Vector2(this.width, this.height);
			component2.anchoredPosition = new Vector3(num2, -(this.topOffset + this.height / 2f), 0f);
			num2 += this.space;
			num2 += this.width / 2f;
			num3++;
		}
	}

	// Token: 0x0600246A RID: 9322 RVA: 0x000DADBC File Offset: 0x000D8FBC
	public void SetRound(int round)
	{
		if (round > this.images.Length)
		{
			return;
		}
		for (int i = 0; i < round; i++)
		{
			if (i == round - 1)
			{
				this.images[i].color = this.curRoundColor;
			}
			else
			{
				this.images[i].color = this.highlightedColor;
			}
		}
	}

	// Token: 0x040027A2 RID: 10146
	public RuntimeAnimatorController controller;

	// Token: 0x040027A3 RID: 10147
	public float space = 5f;

	// Token: 0x040027A4 RID: 10148
	public float width = 60f;

	// Token: 0x040027A5 RID: 10149
	public float height = 16f;

	// Token: 0x040027A6 RID: 10150
	public float topOffset = 10f;

	// Token: 0x040027A7 RID: 10151
	public Color backgroundColor;

	// Token: 0x040027A8 RID: 10152
	public Color highlightedColor;

	// Token: 0x040027A9 RID: 10153
	public Color curRoundColor;

	// Token: 0x040027AA RID: 10154
	public Color shadowColor = Color.white;

	// Token: 0x040027AB RID: 10155
	private Image[] images;

	// Token: 0x040027AC RID: 10156
	private Animator[] animators;
}
