using System;
using UnityEngine;

// Token: 0x02000141 RID: 321
public class FX_LavaBubble : MonoBehaviour
{
	// Token: 0x06000938 RID: 2360 RVA: 0x0000A312 File Offset: 0x00008512
	private void Start()
	{
		this.m_anim = base.GetComponent<Animator>();
		this.m_nextBubbleTime = Time.time + UnityEngine.Random.Range(this.m_minBubbleTime, this.m_maxBubbleTime);
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x0000A33D File Offset: 0x0000853D
	private void Update()
	{
		if (Time.time > this.m_nextBubbleTime)
		{
			this.m_anim.SetTrigger("Bubble");
			this.m_nextBubbleTime = Time.time + UnityEngine.Random.Range(this.m_minBubbleTime, this.m_maxBubbleTime);
		}
	}

	// Token: 0x040007BE RID: 1982
	[SerializeField]
	protected float m_minBubbleTime;

	// Token: 0x040007BF RID: 1983
	[SerializeField]
	protected float m_maxBubbleTime;

	// Token: 0x040007C0 RID: 1984
	private float m_nextBubbleTime;

	// Token: 0x040007C1 RID: 1985
	private Animator m_anim;
}
