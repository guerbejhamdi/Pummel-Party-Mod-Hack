using System;
using UnityEngine;

// Token: 0x02000243 RID: 579
public class Effect_Tumbleweed : MonoBehaviour
{
	// Token: 0x060010BD RID: 4285 RVA: 0x0000DEC8 File Offset: 0x0000C0C8
	public void Awake()
	{
		this.m_body = base.GetComponent<Rigidbody>();
	}

	// Token: 0x060010BE RID: 4286 RVA: 0x000830E0 File Offset: 0x000812E0
	public void FixedUpdate()
	{
		if (this.m_body)
		{
			this.m_body.AddTorque(this.m_windDirection.normalized * this.m_windForce);
			this.m_body.AddTorque(Vector3.up);
			Vector3 normalized = this.m_windDirection.normalized;
			this.m_body.AddForce(normalized * this.m_windForce, ForceMode.Force);
		}
	}

	// Token: 0x0400113E RID: 4414
	[SerializeField]
	protected Vector3 m_windDirection;

	// Token: 0x0400113F RID: 4415
	[SerializeField]
	protected float m_windForce = 1f;

	// Token: 0x04001140 RID: 4416
	private Rigidbody m_body;
}
