using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x0200004C RID: 76
public class DisconnectTransition : MonoBehaviour
{
	// Token: 0x06000140 RID: 320 RVA: 0x00030E48 File Offset: 0x0002F048
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		SceneManager.sceneLoaded += this.SceneManager_sceneLoaded;
		base.StartCoroutine(this.Transition());
		this.startColor = this.fadeImage.color;
		this.rectTransform.gameObject.SetActive(!this.fade);
		this.fadeImage.gameObject.SetActive(this.fade);
	}

	// Token: 0x06000141 RID: 321 RVA: 0x00004632 File Offset: 0x00002832
	private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		this.sceneLoaded = true;
	}

	// Token: 0x06000142 RID: 322 RVA: 0x0000463B File Offset: 0x0000283B
	private IEnumerator Transition()
	{
		float startTime = Time.time;
		float num;
		while ((num = Time.time - startTime) < this.transitionTime)
		{
			this.Set(num, true);
			yield return null;
		}
		this.Set(Mathf.Clamp01(num), true);
		yield return null;
		if (GameManager.LoadScreen != null)
		{
			GameManager.LoadScreen.Destroy();
		}
		SceneManager.LoadScene("MainMenu");
		GameManager.CurState = GameState.MainMenu;
		yield return new WaitUntil(() => this.sceneLoaded);
		startTime = Time.time;
		while ((num = Time.time - startTime) < this.transitionTime)
		{
			this.Set(num, false);
			yield return null;
		}
		this.Set(Mathf.Clamp01(num), false);
		yield return null;
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06000143 RID: 323 RVA: 0x00030EC0 File Offset: 0x0002F0C0
	private void Set(float elapsed, bool fadeIn)
	{
		if (!this.fade)
		{
			this.rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(fadeIn ? this.startX : this.endX, fadeIn ? this.endX : this.startX, elapsed / this.transitionTime), 200f);
		}
		if (this.fade)
		{
			this.fadeImage.color = new Color(this.startColor.r, this.startColor.g, this.startColor.b, fadeIn ? (elapsed / this.transitionTime) : (1f - elapsed / this.transitionTime));
		}
	}

	// Token: 0x0400019B RID: 411
	public RectTransform rectTransform;

	// Token: 0x0400019C RID: 412
	public Image fadeImage;

	// Token: 0x0400019D RID: 413
	public float transitionTime = 1f;

	// Token: 0x0400019E RID: 414
	public float startX = -3500f;

	// Token: 0x0400019F RID: 415
	public float endX;

	// Token: 0x040001A0 RID: 416
	public bool fade = true;

	// Token: 0x040001A1 RID: 417
	public float outStartX = 1000f;

	// Token: 0x040001A2 RID: 418
	public float outEndX = 4000f;

	// Token: 0x040001A3 RID: 419
	private bool sceneLoaded;

	// Token: 0x040001A4 RID: 420
	private Color startColor;
}
