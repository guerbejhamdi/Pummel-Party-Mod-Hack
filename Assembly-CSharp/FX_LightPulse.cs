using System;
using UnityEngine;

// Token: 0x0200024D RID: 589
[ExecuteInEditMode]
public class FX_LightPulse : MonoBehaviour
{
	// Token: 0x06001109 RID: 4361 RVA: 0x0008601C File Offset: 0x0008421C
	public void Update()
	{
		float num = this.m_intensityCurve.Evaluate(Time.time);
		Shader.SetGlobalFloat("_LightPulse", num);
		if (Application.isPlaying)
		{
			for (int i = 0; i < this.m_lights.Length; i++)
			{
				this.m_lights[i].intensity = num;
			}
		}
	}

	// Token: 0x040011B9 RID: 4537
	[SerializeField]
	protected Light[] m_lights;

	// Token: 0x040011BA RID: 4538
	[SerializeField]
	protected AnimationCurve m_intensityCurve;
}
