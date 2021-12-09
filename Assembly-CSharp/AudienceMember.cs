using System;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class AudienceMember : MonoBehaviour
{
	// Token: 0x060001F3 RID: 499 RVA: 0x00004CD7 File Offset: 0x00002ED7
	private void LateUpdate()
	{
		this.m_head.LookAt(this.m_lookTarget);
	}

	// Token: 0x04000254 RID: 596
	[SerializeField]
	private Transform m_lookTarget;

	// Token: 0x04000255 RID: 597
	[SerializeField]
	private Transform m_head;
}
