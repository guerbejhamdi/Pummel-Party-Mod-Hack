using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000565 RID: 1381
public class UISpellingWordBar : MonoBehaviour
{
	// Token: 0x0600245A RID: 9306 RVA: 0x000DA7DC File Offset: 0x000D89DC
	private void Update()
	{
		if (this.m_target != null)
		{
			Vector3 b = new Vector3(0f, this.m_height, 0f);
			base.transform.position = this.m_cam.WorldToScreenPoint(this.m_target.position + b);
		}
	}

	// Token: 0x0600245B RID: 9307 RVA: 0x0001A221 File Offset: 0x00018421
	public void Initialize(Transform target, float height, Camera cam)
	{
		this.m_target = target;
		this.m_height = height;
		this.m_cam = cam;
	}

	// Token: 0x0600245C RID: 9308 RVA: 0x000DA838 File Offset: 0x000D8A38
	public void SetWord(string word)
	{
		word = word.ToUpper();
		Text[] letterTxts = this.m_letterTxts;
		for (int i = 0; i < letterTxts.Length; i++)
		{
			letterTxts[i].transform.parent.gameObject.SetActive(false);
		}
		for (int j = 0; j < word.Length - 1; j++)
		{
			this.m_letterTxts[j].transform.parent.gameObject.SetActive(true);
			this.m_letterTxts[j].text = word[j].ToString();
		}
	}

	// Token: 0x0600245D RID: 9309 RVA: 0x000DA8C8 File Offset: 0x000D8AC8
	public void SetProgress(int progress)
	{
		for (int i = 0; i < this.m_letterTxts.Length; i++)
		{
			this.m_letterTxts[i].color = ((i < progress) ? this.m_completedColor : this.m_defaultColor);
		}
	}

	// Token: 0x0400278E RID: 10126
	[SerializeField]
	private CanvasGroup m_group;

	// Token: 0x0400278F RID: 10127
	[SerializeField]
	private Text[] m_letterTxts;

	// Token: 0x04002790 RID: 10128
	[SerializeField]
	private Color m_completedColor;

	// Token: 0x04002791 RID: 10129
	[SerializeField]
	private Color m_defaultColor;

	// Token: 0x04002792 RID: 10130
	private Transform m_target;

	// Token: 0x04002793 RID: 10131
	private float m_height;

	// Token: 0x04002794 RID: 10132
	private Camera m_cam;
}
