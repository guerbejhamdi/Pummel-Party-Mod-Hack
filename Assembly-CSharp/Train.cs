using System;
using UnityEngine;

// Token: 0x02000249 RID: 585
public class Train : MonoBehaviour
{
	// Token: 0x060010E7 RID: 4327 RVA: 0x00084CA0 File Offset: 0x00082EA0
	public void Awake()
	{
		this.m_source = base.GetComponent<AudioSource>();
		this.m_source.volume = AudioSystem.GetVolume(SoundType.Effect, this.m_source.volume);
		this.m_trainSteam.volume = AudioSystem.GetVolume(SoundType.Effect, this.m_trainSteam.volume);
		this.m_whistleTime = Time.time + this.m_trainWhistleDelay;
		float num = 0f;
		float num2 = 0f;
		GameObject[] array = this.m_bigWheels;
		for (int i = 0; i < array.Length; i++)
		{
			Renderer component = array[i].GetComponent<Renderer>();
			if (!(component == null))
			{
				num = Mathf.Max(new float[]
				{
					num,
					component.bounds.extents.x,
					component.bounds.extents.y,
					component.bounds.extents.z
				});
			}
		}
		array = this.m_smallWheels;
		for (int i = 0; i < array.Length; i++)
		{
			Renderer component2 = array[i].GetComponent<Renderer>();
			if (!(component2 == null))
			{
				num2 = Mathf.Max(new float[]
				{
					num2,
					component2.bounds.extents.x,
					component2.bounds.extents.y,
					component2.bounds.extents.z
				});
			}
		}
		this.m_bigWheelCircumference = 6.2831855f * num;
		this.m_smallWheelCircumference = 6.2831855f * num2;
	}

	// Token: 0x060010E8 RID: 4328 RVA: 0x00084E2C File Offset: 0x0008302C
	public void Update()
	{
		Vector3 b = this.m_direction.normalized * this.m_trainSpeed * Time.deltaTime;
		float magnitude = b.magnitude;
		base.transform.position += b;
		GameObject[] array = this.m_bigWheels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].transform.Rotate(Vector3.right, magnitude / this.m_bigWheelCircumference * 360f);
		}
		array = this.m_smallWheels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].transform.Rotate(Vector3.right, magnitude / this.m_smallWheelCircumference * 360f);
		}
		if (this.m_whistleTime > 0f && Time.time > this.m_whistleTime)
		{
			this.m_source.PlayOneShot(this.m_trainWhistle);
			this.m_whistleTime = -1f;
		}
	}

	// Token: 0x0400118B RID: 4491
	[Header("Movement")]
	[SerializeField]
	protected float m_trainSpeed = 5f;

	// Token: 0x0400118C RID: 4492
	[SerializeField]
	protected Vector3 m_direction = Vector3.forward;

	// Token: 0x0400118D RID: 4493
	[Header("Wheels")]
	[SerializeField]
	protected GameObject[] m_bigWheels;

	// Token: 0x0400118E RID: 4494
	[SerializeField]
	protected GameObject[] m_smallWheels;

	// Token: 0x0400118F RID: 4495
	[Header("SFX")]
	[SerializeField]
	protected float m_trainWhistleDelay = 7f;

	// Token: 0x04001190 RID: 4496
	[SerializeField]
	protected AudioClip m_trainWhistle;

	// Token: 0x04001191 RID: 4497
	[SerializeField]
	protected AudioSource m_trainSteam;

	// Token: 0x04001192 RID: 4498
	private float m_bigWheelCircumference;

	// Token: 0x04001193 RID: 4499
	private float m_smallWheelCircumference;

	// Token: 0x04001194 RID: 4500
	private float m_whistleTime;

	// Token: 0x04001195 RID: 4501
	private AudioSource m_source;
}
