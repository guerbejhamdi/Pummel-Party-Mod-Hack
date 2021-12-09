using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020001C1 RID: 449
public class NavMeshLinkUpdater : MonoBehaviour
{
	// Token: 0x06000CF8 RID: 3320 RVA: 0x0000BF49 File Offset: 0x0000A149
	private void Start()
	{
		this.m_link = base.GetComponent<NavMeshLink>();
	}

	// Token: 0x06000CF9 RID: 3321 RVA: 0x0006B7C4 File Offset: 0x000699C4
	private void LateUpdate()
	{
		this.i++;
		if (this.i >= 4)
		{
			if (Vector3.Distance(this.m_target.position, base.transform.position) < this.m_maxDistance)
			{
				this.m_link.endPoint = this.m_target.position - base.transform.position;
				this.m_link.UpdateLink();
				if (!this.m_link.enabled)
				{
					this.m_link.enabled = true;
				}
			}
			else
			{
				this.m_link.enabled = false;
			}
			this.i = 0;
		}
	}

	// Token: 0x06000CFA RID: 3322 RVA: 0x0000BF57 File Offset: 0x0000A157
	public Vector3 GetEndPos()
	{
		return this.m_target.position;
	}

	// Token: 0x04000C4F RID: 3151
	[SerializeField]
	protected Transform m_target;

	// Token: 0x04000C50 RID: 3152
	[SerializeField]
	protected float m_maxDistance = 2f;

	// Token: 0x04000C51 RID: 3153
	private NavMeshLink m_link;

	// Token: 0x04000C52 RID: 3154
	private int i;
}
