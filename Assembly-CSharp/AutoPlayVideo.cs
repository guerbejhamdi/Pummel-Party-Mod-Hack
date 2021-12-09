using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x0200044E RID: 1102
public class AutoPlayVideo : MonoBehaviour
{
	// Token: 0x06001E48 RID: 7752 RVA: 0x000164E5 File Offset: 0x000146E5
	private void Start()
	{
		this.videoPlayer.errorReceived -= this.OnVideoErrorRecieved;
		this.videoPlayer.errorReceived += this.OnVideoErrorRecieved;
	}

	// Token: 0x06001E49 RID: 7753 RVA: 0x00016515 File Offset: 0x00014715
	private void RequestCompleted(AsyncOperation obj)
	{
		this.videoPlayer.clip = (VideoClip)this.curRequest.asset;
		base.StartCoroutine(this.Play());
	}

	// Token: 0x06001E4A RID: 7754 RVA: 0x000C3D50 File Offset: 0x000C1F50
	public void Play(string videoClipPath, Sprite fallbackImage)
	{
		this.fallbackImageUI.gameObject.SetActive(false);
		this.m_fallbackImage = fallbackImage;
		if (videoClipPath != "")
		{
			this.curRequest = Resources.LoadAsync(videoClipPath);
			this.curRequest.completed += this.RequestCompleted;
		}
	}

	// Token: 0x06001E4B RID: 7755 RVA: 0x0001653F File Offset: 0x0001473F
	public void Stop()
	{
		this.videoPlayer.Stop();
	}

	// Token: 0x06001E4C RID: 7756 RVA: 0x0001654C File Offset: 0x0001474C
	private IEnumerator Play()
	{
		this.m_videoErrorRecieved = false;
		this.videoPlayer.Prepare();
		while (!this.videoPlayer.isPrepared && !this.m_videoErrorRecieved)
		{
			yield return null;
		}
		if (!this.m_videoErrorRecieved)
		{
			this.image.texture = this.videoPlayer.texture;
			this.image.color = Color.white;
			this.videoPlayer.Play();
		}
		yield break;
	}

	// Token: 0x06001E4D RID: 7757 RVA: 0x000C3DA8 File Offset: 0x000C1FA8
	private void OnVideoErrorRecieved(VideoPlayer source, string message)
	{
		Debug.LogError("Error preparing video : " + message + " - " + ((source.clip != null) ? source.clip.name : "null"));
		this.m_videoErrorRecieved = true;
		this.fallbackImageUI.sprite = this.m_fallbackImage;
		this.fallbackImageUI.gameObject.SetActive(true);
	}

	// Token: 0x04002132 RID: 8498
	public Image fallbackImageUI;

	// Token: 0x04002133 RID: 8499
	public RawImage image;

	// Token: 0x04002134 RID: 8500
	public VideoPlayer videoPlayer;

	// Token: 0x04002135 RID: 8501
	public bool play;

	// Token: 0x04002136 RID: 8502
	private Sprite m_fallbackImage;

	// Token: 0x04002137 RID: 8503
	private ResourceRequest curRequest;

	// Token: 0x04002138 RID: 8504
	private Sprite m_minigameVideoFallbackImage;

	// Token: 0x04002139 RID: 8505
	private bool m_videoErrorRecieved;
}
