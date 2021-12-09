using System;
using UnityEngine;
using ZP.Net;

// Token: 0x02000203 RID: 515
public class PongGoal : MonoBehaviour
{
	// Token: 0x06000F2C RID: 3884 RVA: 0x0000D1CF File Offset: 0x0000B3CF
	public void Start()
	{
		this.m_cameraShake = base.transform.root.GetComponentInChildren<CameraShake>();
		if (this.m_playerIndex >= GameManager.GetPlayerCount())
		{
			this.m_emptyBlocker.SetActive(true);
		}
	}

	// Token: 0x06000F2D RID: 3885 RVA: 0x00078C3C File Offset: 0x00076E3C
	public void OnTriggerEnter(Collider other)
	{
		if (!NetSystem.IsServer)
		{
			return;
		}
		PongBall componentInParent = other.GetComponentInParent<PongBall>();
		if (componentInParent != null)
		{
			componentInParent.OnHitGoal(this.m_playerIndex);
			this.m_cameraShake.AddShake(0.1f);
		}
	}

	// Token: 0x04000EED RID: 3821
	[SerializeField]
	protected int m_playerIndex;

	// Token: 0x04000EEE RID: 3822
	[SerializeField]
	protected GameObject m_emptyBlocker;

	// Token: 0x04000EEF RID: 3823
	private CameraShake m_cameraShake;
}
