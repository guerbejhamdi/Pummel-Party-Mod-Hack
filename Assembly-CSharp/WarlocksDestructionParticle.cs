using System;
using UnityEngine;

// Token: 0x02000296 RID: 662
public class WarlocksDestructionParticle : MonoBehaviour, IWarlocksDestructionListener
{
	// Token: 0x0600137F RID: 4991 RVA: 0x0000F80E File Offset: 0x0000DA0E
	public void Awake()
	{
		this.m_system = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x06001380 RID: 4992 RVA: 0x0000F81C File Offset: 0x0000DA1C
	public void OnDestructionLevelChanged(int level)
	{
		if (level == this.m_destructionLevel && this.m_system != null)
		{
			this.m_system.Emit(this.m_emitParticleCount);
		}
	}

	// Token: 0x06001381 RID: 4993 RVA: 0x0000398C File Offset: 0x00001B8C
	public void OnResetDestruction()
	{
	}

	// Token: 0x040014D1 RID: 5329
	[SerializeField]
	protected int m_destructionLevel;

	// Token: 0x040014D2 RID: 5330
	[SerializeField]
	protected int m_emitParticleCount = 1;

	// Token: 0x040014D3 RID: 5331
	private ParticleSystem m_system;
}
