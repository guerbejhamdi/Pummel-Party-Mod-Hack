using System;
using UnityEngine;

// Token: 0x020001D5 RID: 469
public class MemoryMenuPlayerGoal : MonoBehaviour
{
	// Token: 0x17000132 RID: 306
	// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x0000C507 File Offset: 0x0000A707
	public Transform AITarget
	{
		get
		{
			return this.m_aiTarget;
		}
	}

	// Token: 0x06000DA1 RID: 3489 RVA: 0x0006EBD0 File Offset: 0x0006CDD0
	public void SetColor(Color c)
	{
		if (this.m_coloredMat == null)
		{
			this.m_coloredMat = new Material(this.m_baseMaterial);
			this.m_coloredMat.SetColor("_Color", c);
		}
		MeshRenderer[] swapMatRenders = this.m_swapMatRenders;
		for (int i = 0; i < swapMatRenders.Length; i++)
		{
			swapMatRenders[i].sharedMaterial = this.m_coloredMat;
		}
		this.m_light.color = c;
	}

	// Token: 0x04000D04 RID: 3332
	[SerializeField]
	private Material m_baseMaterial;

	// Token: 0x04000D05 RID: 3333
	[SerializeField]
	public MeshRenderer[] m_swapMatRenders;

	// Token: 0x04000D06 RID: 3334
	[SerializeField]
	private Transform m_aiTarget;

	// Token: 0x04000D07 RID: 3335
	[SerializeField]
	private Light m_light;

	// Token: 0x04000D08 RID: 3336
	private Material m_coloredMat;
}
