using System;
using UnityEngine;

// Token: 0x02000189 RID: 393
public class BurglarPlayerGoal : MonoBehaviour
{
	// Token: 0x17000100 RID: 256
	// (get) Token: 0x06000B3C RID: 2876 RVA: 0x0000B2AF File Offset: 0x000094AF
	public Transform AITarget
	{
		get
		{
			return this.m_aiTarget;
		}
	}

	// Token: 0x06000B3D RID: 2877 RVA: 0x00060B54 File Offset: 0x0005ED54
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

	// Token: 0x04000A57 RID: 2647
	[SerializeField]
	private Material m_baseMaterial;

	// Token: 0x04000A58 RID: 2648
	[SerializeField]
	public MeshRenderer[] m_swapMatRenders;

	// Token: 0x04000A59 RID: 2649
	[SerializeField]
	private Transform m_aiTarget;

	// Token: 0x04000A5A RID: 2650
	[SerializeField]
	private Light m_light;

	// Token: 0x04000A5B RID: 2651
	private Material m_coloredMat;
}
