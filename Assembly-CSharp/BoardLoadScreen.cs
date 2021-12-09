using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x020004F2 RID: 1266
public class BoardLoadScreen : MonoBehaviour
{
	// Token: 0x06002157 RID: 8535 RVA: 0x00018230 File Offset: 0x00016430
	private void Awake()
	{
		this.canvas = base.GetComponent<Canvas>();
		GameManager.LoadScreen = this;
		UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
		this.canvas.enabled = true;
		this.Hide();
	}

	// Token: 0x06002158 RID: 8536 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Update()
	{
	}

	// Token: 0x06002159 RID: 8537 RVA: 0x00018266 File Offset: 0x00016466
	public void FadeOut(float time)
	{
		this.backgorund.canvasRenderer.SetAlpha(1f);
		this.backgorund.CrossFadeAlpha(0f, time, false);
	}

	// Token: 0x0600215A RID: 8538 RVA: 0x0001828F File Offset: 0x0001648F
	public void FadeIn(float time)
	{
		this.backgorund.canvasRenderer.SetAlpha(0f);
		this.backgorund.CrossFadeAlpha(1f, time, false);
	}

	// Token: 0x0600215B RID: 8539 RVA: 0x000182B8 File Offset: 0x000164B8
	public void SetStatus(string status)
	{
		if (this.load_status_txt != null)
		{
			this.load_status_txt.text = status;
		}
	}

	// Token: 0x0600215C RID: 8540 RVA: 0x000CEABC File Offset: 0x000CCCBC
	public void Show()
	{
		base.gameObject.SetActive(true);
		string translation = LocalizationManager.GetTranslation("Loading Players", true, 0, true, false, null, null, true);
		GameManager.LoadScreen.SetStatus(translation + " 0/" + NetSystem.PlayerCount.ToString());
		this.FadeIn(0.5f);
	}

	// Token: 0x0600215D RID: 8541 RVA: 0x000182D4 File Offset: 0x000164D4
	public void Hide()
	{
		base.gameObject.SetActive(false);
		this.backgorund.canvasRenderer.SetAlpha(0f);
	}

	// Token: 0x0600215E RID: 8542 RVA: 0x000CEB14 File Offset: 0x000CCD14
	public void Destroy()
	{
		if (this.destroying)
		{
			return;
		}
		this.destroying = true;
		UnityEngine.Object.Destroy(base.gameObject, 0.5f);
		this.FadeOut(0.5f);
		this.spinner.gameObject.SetActive(false);
		this.load_status_txt.gameObject.SetActive(false);
	}

	// Token: 0x04002412 RID: 9234
	public Image backgorund;

	// Token: 0x04002413 RID: 9235
	public Text load_status_txt;

	// Token: 0x04002414 RID: 9236
	public Image spinner;

	// Token: 0x04002415 RID: 9237
	private Canvas canvas;

	// Token: 0x04002416 RID: 9238
	private bool destroying;
}
