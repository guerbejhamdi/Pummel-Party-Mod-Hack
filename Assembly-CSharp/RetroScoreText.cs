using System;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class RetroScoreText : MonoBehaviour
{
	// Token: 0x0600093F RID: 2367 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x00052CBC File Offset: 0x00050EBC
	public void SetNumber(int num)
	{
		num %= 99;
		string text = num.ToString();
		if (text.Length > 1)
		{
			this.m_digits[0].sprite = this.m_digitSprites[int.Parse(text.Substring(0, 1))];
			this.m_digits[1].sprite = this.m_digitSprites[int.Parse(text.Substring(1, 1))];
			return;
		}
		this.m_digits[0].sprite = this.m_digitSprites[0];
		this.m_digits[1].sprite = this.m_digitSprites[int.Parse(text.Substring(0, 1))];
	}

	// Token: 0x040007C5 RID: 1989
	[SerializeField]
	protected SpriteRenderer[] m_digits;

	// Token: 0x040007C6 RID: 1990
	[SerializeField]
	protected Sprite[] m_digitSprites;

	// Token: 0x040007C7 RID: 1991
	private float m_currentNum;

	// Token: 0x040007C8 RID: 1992
	private float m_lastTime;
}
