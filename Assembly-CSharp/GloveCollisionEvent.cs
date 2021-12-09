using System;
using UnityEngine;

// Token: 0x02000372 RID: 882
public class GloveCollisionEvent : MonoBehaviour
{
	// Token: 0x060017BD RID: 6077 RVA: 0x00011A70 File Offset: 0x0000FC70
	public void Awake()
	{
		this.m_glove = base.GetComponentInParent<PunchingGlove>();
	}

	// Token: 0x060017BE RID: 6078 RVA: 0x000A48E0 File Offset: 0x000A2AE0
	private void OnCollisionEnter(Collision collision)
	{
		if (!this.m_glove || (collision.rigidbody != null && collision.rigidbody.gameObject == this.m_ignore))
		{
			return;
		}
		this.m_glove.OnGloveImpact(collision);
	}

	// Token: 0x04001944 RID: 6468
	[SerializeField]
	protected GameObject m_ignore;

	// Token: 0x04001945 RID: 6469
	private PunchingGlove m_glove;
}
