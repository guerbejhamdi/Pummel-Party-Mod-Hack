using System;
using UnityEngine;

// Token: 0x02000206 RID: 518
public class PresentTrigger : MonoBehaviour
{
	// Token: 0x06000F3A RID: 3898 RVA: 0x0000D2CC File Offset: 0x0000B4CC
	private void Start()
	{
		this.m_group = base.GetComponentInParent<PresentsGroup>();
	}

	// Token: 0x06000F3B RID: 3899 RVA: 0x0000D2DA File Offset: 0x0000B4DA
	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "PresentsPlayer")
		{
			this.m_group.HitPlayer(other.GetComponentInParent<PresentsPlayer>(), this.m_index);
		}
	}

	// Token: 0x04000F06 RID: 3846
	[SerializeField]
	protected int m_index;

	// Token: 0x04000F07 RID: 3847
	private PresentsGroup m_group;
}
