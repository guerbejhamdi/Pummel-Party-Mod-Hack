using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000049 RID: 73
public class Destructable : MonoBehaviour
{
	// Token: 0x06000137 RID: 311 RVA: 0x00030BDC File Offset: 0x0002EDDC
	private void Start()
	{
		foreach (object obj in this.m_debriParent.transform)
		{
			Transform transform = (Transform)obj;
			this.m_debriList.Add(new Destructable.DebriInfo(transform.gameObject, transform, transform.gameObject.GetComponent<Rigidbody>(), transform.localPosition, transform.localRotation));
		}
	}

	// Token: 0x06000138 RID: 312 RVA: 0x000045DB File Offset: 0x000027DB
	private void LateUpdate()
	{
		if (this.m_wasDestroyed)
		{
			this.m_wasDestroyed = false;
			Destructable.m_curDestructionIndex++;
		}
	}

	// Token: 0x06000139 RID: 313 RVA: 0x00030C64 File Offset: 0x0002EE64
	public void Destroy(Transform newParent)
	{
		if (this.m_isDestroyed)
		{
			return;
		}
		this.m_wasDestroyed = true;
		this.m_staticParent.SetActive(false);
		foreach (Destructable.DebriInfo debriInfo in this.m_debriList)
		{
			debriInfo.transform.SetParent(newParent);
			float d = 1f - Mathf.Clamp01(Vector3.Distance(this.m_forceOrigin.position, debriInfo.transform.position) / 5f);
			Vector3 vector = (debriInfo.transform.position - this.m_forceOrigin.position).normalized * d * 30000f * 0.5f;
			vector.y = Mathf.Abs(vector.y) * 0.5f;
			debriInfo.body.AddForceAtPosition(vector, base.transform.position);
		}
		this.m_isDestroyed = true;
	}

	// Token: 0x0600013A RID: 314 RVA: 0x00030D80 File Offset: 0x0002EF80
	public void Reset()
	{
		Destructable.m_curDestructionIndex = 0;
		this.m_staticParent.SetActive(true);
		foreach (Destructable.DebriInfo debriInfo in this.m_debriList)
		{
			debriInfo.transform.SetParent(this.m_debriParent.transform);
			debriInfo.transform.localPosition = debriInfo.localPos;
			debriInfo.transform.localRotation = debriInfo.localRot;
			debriInfo.body.ResetInertiaTensor();
			debriInfo.body.velocity = Vector3.zero;
			debriInfo.body.angularVelocity = Vector3.zero;
		}
		this.m_isDestroyed = false;
	}

	// Token: 0x0400018E RID: 398
	[SerializeField]
	private GameObject m_staticParent;

	// Token: 0x0400018F RID: 399
	[SerializeField]
	private GameObject m_debriParent;

	// Token: 0x04000190 RID: 400
	[SerializeField]
	private Transform m_forceOrigin;

	// Token: 0x04000191 RID: 401
	private static int m_curDestructionIndex;

	// Token: 0x04000192 RID: 402
	private bool m_isDestroyed;

	// Token: 0x04000193 RID: 403
	private List<Destructable.DebriInfo> m_debriList = new List<Destructable.DebriInfo>();

	// Token: 0x04000194 RID: 404
	private bool m_wasDestroyed;

	// Token: 0x0200004A RID: 74
	private struct DebriInfo
	{
		// Token: 0x0600013D RID: 317 RVA: 0x0000460B File Offset: 0x0000280B
		public DebriInfo(GameObject o, Transform t, Rigidbody b, Vector3 lp, Quaternion lr)
		{
			this.gameObj = o;
			this.transform = t;
			this.body = b;
			this.localPos = lp;
			this.localRot = lr;
		}

		// Token: 0x04000195 RID: 405
		public GameObject gameObj;

		// Token: 0x04000196 RID: 406
		public Transform transform;

		// Token: 0x04000197 RID: 407
		public Rigidbody body;

		// Token: 0x04000198 RID: 408
		public Vector3 localPos;

		// Token: 0x04000199 RID: 409
		public Quaternion localRot;
	}
}
