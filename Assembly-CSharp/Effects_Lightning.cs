using System;
using UnityEngine;

// Token: 0x02000245 RID: 581
public class Effects_Lightning : MonoBehaviour
{
	// Token: 0x060010C3 RID: 4291 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Awake()
	{
	}

	// Token: 0x060010C4 RID: 4292 RVA: 0x000831C8 File Offset: 0x000813C8
	private void Setup()
	{
		this.m_light = base.GetComponent<Light>();
		this.m_baseIntensity = this.m_light.intensity;
		this.m_nextFlashTime = Time.time + UnityEngine.Random.Range(this.m_minFlashFrequency, this.m_maxFlashFrequency);
		this.m_flashStartTime = -100f;
		this.UpdateLightning();
	}

	// Token: 0x060010C5 RID: 4293 RVA: 0x00083220 File Offset: 0x00081420
	public void Update()
	{
		if ((GameManager.Minigame != null && GameManager.Minigame.AllClientsReady()) || this.m_dontRequireMinigame)
		{
			if (this.firstTime)
			{
				this.Setup();
				this.firstTime = false;
				return;
			}
			if (Time.time >= this.m_nextFlashTime)
			{
				this.m_flashStartTime = Time.time;
				this.m_nextFlashTime = Time.time + UnityEngine.Random.Range(this.m_minFlashFrequency, this.m_maxFlashFrequency);
				Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
				eulerAngles.y = UnityEngine.Random.Range(0f, 360f);
				base.transform.localRotation = Quaternion.Euler(eulerAngles);
				AudioSystem.PlayOneShot(this.m_lightningSfx[UnityEngine.Random.Range(0, this.m_lightningSfx.Length - 1)], UnityEngine.Random.Range(0.7f, 1f), 0f, 1f);
			}
			this.UpdateLightning();
		}
	}

	// Token: 0x060010C6 RID: 4294 RVA: 0x00083318 File Offset: 0x00081518
	public void ShootLightning()
	{
		this.m_flashStartTime = Time.time;
		this.m_nextFlashTime = Time.time + UnityEngine.Random.Range(this.m_minFlashFrequency, this.m_maxFlashFrequency);
		Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
		eulerAngles.y = UnityEngine.Random.Range(0f, 360f);
		base.transform.localRotation = Quaternion.Euler(eulerAngles);
		AudioSystem.PlayOneShot(this.m_lightningSfx[UnityEngine.Random.Range(0, this.m_lightningSfx.Length - 1)], UnityEngine.Random.Range(0.7f, 1f), 0f, 1f);
	}

	// Token: 0x060010C7 RID: 4295 RVA: 0x000833C0 File Offset: 0x000815C0
	private void UpdateLightning()
	{
		float time = (Time.time - this.m_flashStartTime) / this.m_flashTime;
		this.m_light.intensity = this.m_flashCurve.Evaluate(time) * this.m_baseIntensity;
		this.m_light.enabled = (this.m_light.intensity > 0f);
	}

	// Token: 0x04001146 RID: 4422
	[SerializeField]
	protected float m_minFlashFrequency = 3f;

	// Token: 0x04001147 RID: 4423
	[SerializeField]
	protected float m_maxFlashFrequency = 10f;

	// Token: 0x04001148 RID: 4424
	[SerializeField]
	protected bool m_dontRequireMinigame;

	// Token: 0x04001149 RID: 4425
	[Header("Lighting")]
	[SerializeField]
	protected AnimationCurve m_flashCurve;

	// Token: 0x0400114A RID: 4426
	[SerializeField]
	protected float m_flashTime = 0.1f;

	// Token: 0x0400114B RID: 4427
	[Header("Audio")]
	[SerializeField]
	protected AudioClip[] m_lightningSfx;

	// Token: 0x0400114C RID: 4428
	private float m_flashStartTime;

	// Token: 0x0400114D RID: 4429
	private float m_nextFlashTime;

	// Token: 0x0400114E RID: 4430
	private float m_baseIntensity = 1f;

	// Token: 0x0400114F RID: 4431
	private Light m_light;

	// Token: 0x04001150 RID: 4432
	private bool firstTime = true;
}
