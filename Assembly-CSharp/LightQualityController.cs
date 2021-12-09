using System;
using UnityEngine;

// Token: 0x02000464 RID: 1124
public class LightQualityController : MonoBehaviour
{
	// Token: 0x06001E8E RID: 7822 RVA: 0x000C5BE4 File Offset: 0x000C3DE4
	public void Awake()
	{
		foreach (LightQualityGroup lightQualityGroup in this.m_qualityGroups)
		{
			bool enabled = (GameManager.GetLocalPlayerCount() <= lightQualityGroup.lightMaxPlayers || lightQualityGroup.lightMaxPlayers == 0) && (lightQualityGroup.lightEnabledPlatforms & QualityConstants.Platform) > DetailPlatform.None;
			bool flag = (GameManager.GetLocalPlayerCount() <= lightQualityGroup.shadowMaxPlayers || lightQualityGroup.shadowMaxPlayers == 0) && (lightQualityGroup.shadowEnabledPlatforms & QualityConstants.Platform) > DetailPlatform.None;
			foreach (Light light in lightQualityGroup.targetLights)
			{
				light.enabled = enabled;
				if (light.shadows != LightShadows.None)
				{
					light.shadows = (flag ? LightShadows.Hard : LightShadows.None);
				}
			}
		}
	}

	// Token: 0x04002193 RID: 8595
	[SerializeField]
	private LightQualityGroup[] m_qualityGroups;
}
