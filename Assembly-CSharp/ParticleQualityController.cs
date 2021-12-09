using System;
using UnityEngine;

// Token: 0x02000465 RID: 1125
public class ParticleQualityController : MonoBehaviour
{
	// Token: 0x06001E90 RID: 7824 RVA: 0x000C5CA4 File Offset: 0x000C3EA4
	public void Awake()
	{
		int particleQuality = (int)this.GetParticleQuality();
		ParticleSystem.EmissionModule emission = base.GetComponent<ParticleSystem>().emission;
		emission.rateOverTimeMultiplier *= QualityConstants.ParticleSpawnRatio[particleQuality];
		emission.rateOverDistanceMultiplier *= QualityConstants.ParticleSpawnRatio[particleQuality];
	}

	// Token: 0x06001E91 RID: 7825 RVA: 0x00016870 File Offset: 0x00014A70
	private ParticleQuality GetParticleQuality()
	{
		if (GameManager.GetLocalPlayerCount() > this.m_maxPlayers && this.m_maxPlayers != 0)
		{
			return ParticleQuality.Medium;
		}
		return Settings.ParticleQuality;
	}

	// Token: 0x04002194 RID: 8596
	[SerializeField]
	public int m_maxPlayers;
}
