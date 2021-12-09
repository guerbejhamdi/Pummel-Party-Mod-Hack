using System;
using UnityEngine;

// Token: 0x020001FA RID: 506
public class PlanesBullet : MonoBehaviour
{
	// Token: 0x06000ED5 RID: 3797 RVA: 0x00076D7C File Offset: 0x00074F7C
	private void Awake()
	{
		this.playerLayer = LayerMask.NameToLayer("Players");
		this.hitLayer = LayerMask.GetMask(new string[]
		{
			"Players",
			"WorldWall",
			"WorldGround"
		});
		UnityEngine.Object.Destroy(base.gameObject, this.m_lifeTime);
		this.m_color.r = UnityEngine.Random.value;
		this.m_color.g = UnityEngine.Random.value;
		this.m_color.b = UnityEngine.Random.value;
		this.m_color.a = 1f;
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x0000CEBD File Offset: 0x0000B0BD
	public void Init(PlanesPlayer owner, bool proxy)
	{
		this.m_owner = owner;
		this.m_isProxy = proxy;
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x00076E14 File Offset: 0x00075014
	private void FixedUpdate()
	{
		if (!this.m_isProxy)
		{
			this.TestCollision();
		}
		base.transform.position += base.transform.forward * this.m_speed * Time.deltaTime;
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x00076E68 File Offset: 0x00075068
	private void TestCollision()
	{
		Vector3 position = base.transform.position;
		float maxDistance = Vector3.Distance(base.transform.position, this.m_end.position);
		Vector3 normalized = (this.m_end.position - base.transform.position).normalized;
		RaycastHit hitInfo;
		if (Physics.Raycast(position, normalized, out hitInfo, maxDistance, this.hitLayer, QueryTriggerInteraction.Collide))
		{
			if (hitInfo.collider.gameObject.layer == this.playerLayer)
			{
				PlanesPlayer componentInParent = hitInfo.collider.gameObject.GetComponentInParent<PlanesPlayer>();
				if (componentInParent != null && this.m_owner != null)
				{
					this.m_owner.OnBulletHitPlayer(componentInParent, hitInfo);
				}
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04000E96 RID: 3734
	[SerializeField]
	private float m_speed = 50f;

	// Token: 0x04000E97 RID: 3735
	[SerializeField]
	private float m_raycastDist = 6f;

	// Token: 0x04000E98 RID: 3736
	[SerializeField]
	private float m_lifeTime = 5f;

	// Token: 0x04000E99 RID: 3737
	[SerializeField]
	private Transform m_end;

	// Token: 0x04000E9A RID: 3738
	private bool m_isProxy;

	// Token: 0x04000E9B RID: 3739
	private PlanesPlayer m_owner;

	// Token: 0x04000E9C RID: 3740
	private Color m_color;

	// Token: 0x04000E9D RID: 3741
	private int playerLayer;

	// Token: 0x04000E9E RID: 3742
	private int hitLayer;
}
