using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200056B RID: 1387
public class UITurnTimer : MonoBehaviour
{
	// Token: 0x06002473 RID: 9331 RVA: 0x0001A2F8 File Offset: 0x000184F8
	public void Awake()
	{
		this.anim = base.GetComponent<Animator>();
	}

	// Token: 0x06002474 RID: 9332 RVA: 0x000DAF3C File Offset: 0x000D913C
	private void Update()
	{
		bool flag = this.active && this.curTime <= this.minTime;
		float num = this.fadeSpeed * Time.deltaTime;
		this.group.alpha = Mathf.Clamp01(this.group.alpha + (flag ? num : (-num)));
		if (!this.active && this.fuseSource != null)
		{
			this.fuseSource.FadeAudio(0.5f, FadeType.Out);
			this.fuseSource = null;
		}
		if (!this.active && this.fillParent.activeSelf)
		{
			this.fillParent.SetActive(false);
		}
	}

	// Token: 0x06002475 RID: 9333 RVA: 0x000DAFE8 File Offset: 0x000D91E8
	public void SetState(bool active)
	{
		if (!GameManager.TurnLengthLimited)
		{
			return;
		}
		if (!active && this.active)
		{
			if (this.fuseSource != null)
			{
				this.fuseSource.FadeAudio(0.5f, FadeType.Out);
				this.fuseSource = null;
			}
			LeanTween.cancel(this.timerText.gameObject);
			LeanTween.cancel(base.gameObject);
		}
		this.active = active;
	}

	// Token: 0x06002476 RID: 9334 RVA: 0x000DB050 File Offset: 0x000D9250
	public void SetTime(float time, float totalTime)
	{
		int num = (int)Mathf.Ceil(totalTime - time);
		this.timerText.text = num.ToString();
		float num2 = (totalTime - time) / (totalTime * 0.5f);
		this.fill.color = this.fillColor.Evaluate(1f - num2);
		this.fill.fillAmount = num2;
		if (num2 < 1f && num2 > 0f)
		{
			if (this.fuseSource == null && this.active)
			{
				this.fuseSource = AudioSystem.PlayLooping(this.fuseClip, 0.5f, 0.5f);
			}
			if (!this.fillParent.activeSelf)
			{
				this.fillParent.SetActive(true);
			}
		}
		else if (num2 <= 0f)
		{
			if (this.fuseSource != null)
			{
				this.fuseSource.FadeAudio(0.5f, FadeType.Out);
				this.fuseSource = null;
			}
			if (this.fillParent.activeSelf)
			{
				this.fillParent.SetActive(false);
			}
		}
		this.curTime = Mathf.Clamp01(time / totalTime);
		if (this.curTime > 0f && !this.blipActive)
		{
			this.blipActive = true;
			this.Blip();
			return;
		}
		if (this.curTime <= 0f && this.blipActive)
		{
			LeanTween.cancel(this.timerText.gameObject);
			this.blipActive = false;
		}
	}

	// Token: 0x06002477 RID: 9335 RVA: 0x000DB1B0 File Offset: 0x000D93B0
	private void Blip()
	{
		if (!this.blipActive)
		{
			return;
		}
		LeanTween.scale(this.timerText.gameObject.GetComponent<RectTransform>(), this.blip, 0.125f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong(1);
		LeanTween.delayedCall(base.gameObject, 1f, new Action(this.Blip));
	}

	// Token: 0x06002478 RID: 9336 RVA: 0x0001A306 File Offset: 0x00018506
	public void OnDestroy()
	{
		if (this.fuseSource != null)
		{
			this.fuseSource.FadeAudio(0.5f, FadeType.Out);
		}
	}

	// Token: 0x040027B2 RID: 10162
	public CanvasGroup group;

	// Token: 0x040027B3 RID: 10163
	public Image fill;

	// Token: 0x040027B4 RID: 10164
	public Gradient fillColor;

	// Token: 0x040027B5 RID: 10165
	public float fadeSpeed = 2f;

	// Token: 0x040027B6 RID: 10166
	public AudioClip fuseClip;

	// Token: 0x040027B7 RID: 10167
	public float minTime = 1f;

	// Token: 0x040027B8 RID: 10168
	public GameObject fillParent;

	// Token: 0x040027B9 RID: 10169
	public Text timerText;

	// Token: 0x040027BA RID: 10170
	[HideInInspector]
	public bool active;

	// Token: 0x040027BB RID: 10171
	private TempAudioSource fuseSource;

	// Token: 0x040027BC RID: 10172
	private float curTime;

	// Token: 0x040027BD RID: 10173
	private Animator anim;

	// Token: 0x040027BE RID: 10174
	private bool blipActive = true;

	// Token: 0x040027BF RID: 10175
	private Vector3 blip = new Vector3(1.2f, 1.2f, 1.2f);
}
