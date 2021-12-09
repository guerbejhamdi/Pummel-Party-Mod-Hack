using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200016A RID: 362
public class BoxDropBox : MonoBehaviour
{
	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x06000A68 RID: 2664 RVA: 0x0000AC7B File Offset: 0x00008E7B
	// (set) Token: 0x06000A69 RID: 2665 RVA: 0x0000AC83 File Offset: 0x00008E83
	public int Index { get; set; }

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x06000A6A RID: 2666 RVA: 0x0000AC8C File Offset: 0x00008E8C
	public bool IsDropping
	{
		get
		{
			return this.m_isDropping;
		}
	}

	// Token: 0x06000A6B RID: 2667 RVA: 0x0005BD44 File Offset: 0x00059F44
	public void Init(int x, int y, int index)
	{
		this.Index = this.Index;
		int num;
		if ((x == 0 && y == 0) || (x == 9 && y == 9) || (x == 0 && y == 9) || (y == 0 && x == 9))
		{
			num = 0;
		}
		else
		{
			num = UnityEngine.Random.Range(1, this.m_graphics.Length);
		}
		this.m_graphics[num].SetActive(true);
		base.transform.rotation = Quaternion.Euler(0f, (float)UnityEngine.Random.Range(0, 4) * 90f, 0f);
		this.m_body = this.m_boxDropRoot.AddComponent<Rigidbody>();
		this.m_body.isKinematic = true;
		this.m_boxCollider = base.GetComponentInChildren<BoxCollider>();
		this.m_dropGlow.SetActive(false);
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x0000398C File Offset: 0x00001B8C
	public static void GetClosestBox(Vector3 position, bool isDropping)
	{
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x0000AC94 File Offset: 0x00008E94
	public void Drop(float delay)
	{
		if (this.m_isDropping)
		{
			return;
		}
		this.m_isDropping = true;
		base.StartCoroutine(this.DropRoutine(delay));
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x0000ACB4 File Offset: 0x00008EB4
	public IEnumerator DropRoutine(float delay)
	{
		float time = 0f;
		float num = 0.1f;
		float shakeTime = delay - num + UnityEngine.Random.value * 0.5f;
		float riseTime = delay / 4f;
		float riseHeight = 0.045f + UnityEngine.Random.value * 0.15f;
		float shakeTargetTime = 0f;
		float fallTime = 1.35f;
		float returnTime = 0.3f + UnityEngine.Random.value * 0.3f;
		this.m_sourceTag.enabled = false;
		Vector3 shakeTarget = UnityEngine.Random.onUnitSphere * 0.15f;
		shakeTarget.y = 0f;
		Vector3 startPos = this.m_boxDropRoot.transform.localPosition;
		Quaternion startRot = this.m_boxDropRoot.transform.localRotation;
		while (time < shakeTime)
		{
			float num2 = Mathf.Clamp01(time / riseTime);
			float y = num2 * riseHeight;
			if (shakeTargetTime > 0.05f)
			{
				shakeTarget = UnityEngine.Random.onUnitSphere * 0.15f;
				shakeTarget.y = 0f;
			}
			Vector3 localPosition = Vector3.MoveTowards(this.m_boxDropRoot.transform.localPosition, shakeTarget, 1f * Time.deltaTime * num2);
			localPosition.y = y;
			this.m_boxDropRoot.transform.localPosition = localPosition;
			time += Time.deltaTime;
			shakeTargetTime += Time.deltaTime;
			yield return null;
		}
		this.m_body.isKinematic = false;
		this.m_body.detectCollisions = false;
		this.m_boxCollider.enabled = false;
		this.m_body.angularVelocity = UnityEngine.Random.onUnitSphere * 360f;
		Vector3 onUnitSphere = UnityEngine.Random.onUnitSphere;
		onUnitSphere.y -= 2f;
		this.m_body.velocity = onUnitSphere;
		time = 0f;
		while (time < fallTime)
		{
			time += Time.deltaTime;
			yield return null;
		}
		this.m_body.isKinematic = true;
		this.m_body.velocity = Vector3.zero;
		this.m_body.angularVelocity = Vector3.zero;
		this.m_body.detectCollisions = true;
		yield return null;
		time = 0f;
		Vector3 lerpPos = this.m_boxDropRoot.transform.localPosition;
		Quaternion lerpRot = this.m_boxDropRoot.transform.localRotation;
		while (time < returnTime)
		{
			float t = this.m_returnCurve.Evaluate(Mathf.Clamp01(time / returnTime));
			this.m_boxDropRoot.transform.localPosition = Vector3.LerpUnclamped(lerpPos, startPos, t);
			this.m_boxDropRoot.transform.localRotation = Quaternion.Lerp(lerpRot, startRot, t);
			time += Time.deltaTime;
			yield return null;
		}
		this.m_boxDropRoot.transform.localPosition = startPos;
		this.m_boxDropRoot.transform.localRotation = startRot;
		this.m_boxCollider.enabled = true;
		this.m_isDropping = false;
		this.m_sourceTag.enabled = true;
		yield break;
	}

	// Token: 0x04000941 RID: 2369
	[SerializeField]
	protected NavMeshSourceTag m_sourceTag;

	// Token: 0x04000942 RID: 2370
	[SerializeField]
	protected GameObject[] m_graphics;

	// Token: 0x04000943 RID: 2371
	[SerializeField]
	protected GameObject m_boxDropRoot;

	// Token: 0x04000944 RID: 2372
	[SerializeField]
	protected AnimationCurve m_returnCurve;

	// Token: 0x04000945 RID: 2373
	[SerializeField]
	protected GameObject m_dropGlow;

	// Token: 0x04000946 RID: 2374
	private BoxCollider m_boxCollider;

	// Token: 0x04000947 RID: 2375
	private bool m_isDropping;

	// Token: 0x04000949 RID: 2377
	private Rigidbody m_body;
}
