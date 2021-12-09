using System;
using UnityEngine;

// Token: 0x02000131 RID: 305
public class ForceShadows : MonoBehaviour
{
	// Token: 0x060008CB RID: 2251 RVA: 0x00009EB4 File Offset: 0x000080B4
	public void Awake()
	{
		QualitySettings.shadows = ShadowQuality.HardOnly;
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x00009EBC File Offset: 0x000080BC
	public void OnDestroy()
	{
		QualitySettings.shadows = Settings.Shadows;
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x00009EC8 File Offset: 0x000080C8
	private void Update()
	{
		if (!this.boardWorld.activeSelf)
		{
			if (QualitySettings.shadows != Settings.Shadows)
			{
				QualitySettings.shadows = Settings.Shadows;
				return;
			}
		}
		else if (QualitySettings.shadows != ShadowQuality.HardOnly)
		{
			QualitySettings.shadows = ShadowQuality.HardOnly;
		}
	}

	// Token: 0x0400072C RID: 1836
	public GameObject boardWorld;
}
