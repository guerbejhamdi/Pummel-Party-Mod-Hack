using System;
using UnityEngine;

// Token: 0x0200007F RID: 127
public class ParticleScalingMode : MonoBehaviour
{
	// Token: 0x06000288 RID: 648 RVA: 0x00035CE0 File Offset: 0x00033EE0
	private void Awake()
	{
		ParticleSystem component = base.GetComponent<ParticleSystem>();
		if (component != null)
		{
			component.main.scalingMode = this.m_scalingMode;
		}
	}

	// Token: 0x040002D0 RID: 720
	[SerializeField]
	private ParticleSystemScalingMode m_scalingMode;
}
