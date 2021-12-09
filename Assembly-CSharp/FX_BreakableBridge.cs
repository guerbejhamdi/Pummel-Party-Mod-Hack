using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000231 RID: 561
public class FX_BreakableBridge : MonoBehaviour
{
	// Token: 0x06001044 RID: 4164 RVA: 0x00080140 File Offset: 0x0007E340
	public void Start()
	{
		this.m_bridgePiecePos = new Vector3[this.m_bridgePieces.Length];
		this.m_bridgePieceRot = new Quaternion[this.m_bridgePieces.Length];
		this.m_bridgeColliders = new Collider[this.m_bridgePieces.Length];
		for (int i = 0; i < this.m_bridgePieces.Length; i++)
		{
			this.m_bridgePiecePos[i] = this.m_bridgePieces[i].transform.position;
			this.m_bridgePieceRot[i] = this.m_bridgePieces[i].transform.rotation;
			this.m_bridgeColliders[i] = this.m_bridgePieces[i].GetComponent<Collider>();
		}
	}

	// Token: 0x06001045 RID: 4165 RVA: 0x0000DB55 File Offset: 0x0000BD55
	public void BreakBridge()
	{
		AudioSystem.PlayOneShot(this.m_bridgeBreakClip, 1f, 0.5f, 1f);
		UnityEngine.Object.Instantiate<GameObject>(this.m_breakParticlePfb, this.m_breakParticleSpawn.position, Quaternion.identity);
		this.ToggleBridge(true);
	}

	// Token: 0x06001046 RID: 4166 RVA: 0x0000DB94 File Offset: 0x0000BD94
	public void FixBridge()
	{
		this.ToggleBridge(false);
	}

	// Token: 0x06001047 RID: 4167 RVA: 0x000801EC File Offset: 0x0007E3EC
	private void ToggleBridge(bool broken)
	{
		if (this.m_fixRoutine != null)
		{
			base.StopCoroutine(this.m_fixRoutine);
			this.m_fixRoutine = null;
		}
		if (broken)
		{
			this.m_bridgeIntact.SetActive(!broken);
			this.m_bridgeBroken.SetActive(broken);
		}
		for (int i = 0; i < this.m_bridgePieces.Length; i++)
		{
			this.m_bridgePieces[i].useGravity = broken;
			this.m_bridgePieces[i].isKinematic = !broken;
			this.m_bridgeColliders[i].enabled = broken;
		}
		if (!broken)
		{
			this.m_fixRoutine = base.StartCoroutine(this.RepairBridgeCoroutine());
		}
	}

	// Token: 0x06001048 RID: 4168 RVA: 0x0000DB9D File Offset: 0x0000BD9D
	private IEnumerator RepairBridgeCoroutine()
	{
		float t = 0f;
		Vector3[] startPos = new Vector3[this.m_bridgePieces.Length];
		Quaternion[] startRot = new Quaternion[this.m_bridgePieces.Length];
		for (int i = 0; i < this.m_bridgePieces.Length; i++)
		{
			startPos[i] = this.m_bridgePieces[i].transform.position;
			startRot[i] = this.m_bridgePieces[i].transform.rotation;
		}
		while (t <= this.m_bridgeFixTime)
		{
			for (int j = 0; j < this.m_bridgePieces.Length; j++)
			{
				this.m_bridgePieces[j].transform.position = Vector3.Lerp(startPos[j], this.m_bridgePiecePos[j], t);
				this.m_bridgePieces[j].transform.rotation = Quaternion.Lerp(startRot[j], this.m_bridgePieceRot[j], t);
			}
			t += Time.deltaTime;
			yield return null;
		}
		this.m_bridgeIntact.SetActive(true);
		this.m_bridgeBroken.SetActive(false);
		this.m_fixRoutine = null;
		yield break;
	}

	// Token: 0x04001098 RID: 4248
	[SerializeField]
	private GameObject m_bridgeIntact;

	// Token: 0x04001099 RID: 4249
	[SerializeField]
	private GameObject m_bridgeBroken;

	// Token: 0x0400109A RID: 4250
	[SerializeField]
	private Rigidbody[] m_bridgePieces;

	// Token: 0x0400109B RID: 4251
	[SerializeField]
	private AudioClip m_bridgeBreakClip;

	// Token: 0x0400109C RID: 4252
	[SerializeField]
	private GameObject m_breakParticlePfb;

	// Token: 0x0400109D RID: 4253
	[SerializeField]
	private Transform m_breakParticleSpawn;

	// Token: 0x0400109E RID: 4254
	private Collider[] m_bridgeColliders;

	// Token: 0x0400109F RID: 4255
	private Vector3[] m_bridgePiecePos;

	// Token: 0x040010A0 RID: 4256
	private Quaternion[] m_bridgePieceRot;

	// Token: 0x040010A1 RID: 4257
	private float m_bridgeFixTime = 1f;

	// Token: 0x040010A2 RID: 4258
	private Coroutine m_fixRoutine;
}
