using System;
using UnityEngine;

// Token: 0x02000192 RID: 402
public class RollyCarFollowCam : MonoBehaviour
{
	// Token: 0x06000B89 RID: 2953 RVA: 0x0000B54A File Offset: 0x0000974A
	private void Start()
	{
		this.lastVel = this.target.forward;
		this.curDirection = this.target.forward;
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x0000B56E File Offset: 0x0000976E
	private void FixedUpdate()
	{
		this.DoCamera();
	}

	// Token: 0x06000B8B RID: 2955 RVA: 0x0006260C File Offset: 0x0006080C
	private void DoCamera()
	{
		Vector3 normalized = this.lastVel;
		if (this.rb.velocity != Vector3.zero && Vector3.Dot(this.rb.velocity.normalized, this.target.forward) > 0.1f)
		{
			normalized = this.rb.velocity.normalized;
			this.lastVel = normalized;
		}
		this.curDirection = Vector3.Slerp(this.curDirection, normalized, Time.deltaTime * 5f);
		Vector3 a = this.curDirection;
		a.y = 0f;
		a.Normalize();
		base.transform.position = Vector3.Lerp(base.transform.position, this.target.position - a * this.backOffset + Vector3.up * this.heightOffset, 0.5f);
		this.offsetVel = Vector3.MoveTowards(this.offsetVel, this.rb.velocity, 90f * Time.deltaTime);
		if (Vector3.Dot(this.rb.velocity, this.target.forward) < -0.2f)
		{
			this.offsetVel = Vector3.zero;
		}
		Quaternion quaternion = Quaternion.LookRotation((this.target.position + this.offsetVel * this.forwardLook - base.transform.position).normalized);
		Vector3 eulerAngles = base.transform.rotation.eulerAngles;
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.Euler(this.camAngle, quaternion.eulerAngles.y, 0f), 0.5f);
	}

	// Token: 0x04000AB6 RID: 2742
	public Transform target;

	// Token: 0x04000AB7 RID: 2743
	public Rigidbody rb;

	// Token: 0x04000AB8 RID: 2744
	public float heightOffset = 5f;

	// Token: 0x04000AB9 RID: 2745
	public float backOffset = 5f;

	// Token: 0x04000ABA RID: 2746
	public float camAngle = 23f;

	// Token: 0x04000ABB RID: 2747
	public float forwardLook = 0.2f;

	// Token: 0x04000ABC RID: 2748
	private Vector3 lastVel;

	// Token: 0x04000ABD RID: 2749
	private Vector3 curDirection;

	// Token: 0x04000ABE RID: 2750
	private Vector3 offsetVel;
}
