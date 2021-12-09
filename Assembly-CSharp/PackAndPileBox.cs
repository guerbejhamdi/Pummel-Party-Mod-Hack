using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001EE RID: 494
public class PackAndPileBox : MonoBehaviour
{
	// Token: 0x06000E66 RID: 3686 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Awake()
	{
	}

	// Token: 0x06000E67 RID: 3687 RVA: 0x00073408 File Offset: 0x00071608
	public void Spawn(Vector3 startPos, Vector3 endPos, Material mat)
	{
		this.m_startPos = startPos;
		this.m_endPos = endPos;
		this.m_boxRenderer.sharedMaterial = mat;
		base.StartCoroutine(this.SpawnRoutine());
		AudioSystem.PlayOneShot(this.m_spawnClip, 0.5f, 0.1f, 1f);
	}

	// Token: 0x06000E68 RID: 3688 RVA: 0x0000CBB1 File Offset: 0x0000ADB1
	private IEnumerator SpawnRoutine()
	{
		this.m_landed = false;
		this.m_anim.enabled = false;
		yield return null;
		this.m_anim.enabled = true;
		float startTime = Time.time;
		float scaleTime = 0.1f;
		while (Time.time - startTime < scaleTime)
		{
			float d = Mathf.Clamp01((Time.time - startTime) / scaleTime);
			this.m_box.transform.localScale = Vector3.one * d;
			yield return null;
		}
		this.m_box.transform.localScale = Vector3.one;
		float num = Vector3.Distance(this.m_startPos, this.m_endPos);
		float time = num / this.m_speed;
		startTime = Time.time;
		while (Time.time - startTime < time)
		{
			float num2 = Mathf.Clamp01((Time.time - startTime) / time);
			num2 = this.m_landLerpCurve.Evaluate(num2);
			if (num2 > 0.9f && !this.m_landed)
			{
				this.Land();
			}
			base.transform.position = Vector3.Lerp(this.m_startPos, this.m_endPos, num2);
			yield return null;
		}
		base.transform.position = this.m_endPos;
		if (!this.m_landed)
		{
			this.Land();
		}
		yield return new WaitForSeconds(0.5f);
		yield break;
	}

	// Token: 0x06000E69 RID: 3689 RVA: 0x0000CBC0 File Offset: 0x0000ADC0
	private void Land()
	{
		this.m_landed = true;
		this.m_anim.SetTrigger("Land");
		AudioSystem.PlayOneShot(this.m_landClip, 2.5f, 0.1f, 1f);
	}

	// Token: 0x04000DE5 RID: 3557
	public float m_speed = 9.8f;

	// Token: 0x04000DE6 RID: 3558
	public AudioClip m_spawnClip;

	// Token: 0x04000DE7 RID: 3559
	public AudioClip m_landClip;

	// Token: 0x04000DE8 RID: 3560
	public GameObject m_box;

	// Token: 0x04000DE9 RID: 3561
	public Animator m_anim;

	// Token: 0x04000DEA RID: 3562
	public AnimationCurve m_landLerpCurve;

	// Token: 0x04000DEB RID: 3563
	public MeshRenderer m_boxRenderer;

	// Token: 0x04000DEC RID: 3564
	private Vector3 m_startPos;

	// Token: 0x04000DED RID: 3565
	private Vector3 m_endPos;

	// Token: 0x04000DEE RID: 3566
	private bool m_landed;
}
