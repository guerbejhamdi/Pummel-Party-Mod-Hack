using System;
using UnityEngine;
using ZP.Utility;

// Token: 0x02000013 RID: 19
public class BattyBattersBall : MonoBehaviour
{
	// Token: 0x06000049 RID: 73 RVA: 0x0002B524 File Offset: 0x00029724
	public void Update()
	{
		if (!this.hit && base.transform.position.y > -10f)
		{
			this.velocity += Vector3.down * this.gravity * Time.deltaTime;
			base.transform.position += this.velocity * Time.deltaTime;
		}
	}

	// Token: 0x0600004A RID: 74 RVA: 0x0002B5A4 File Offset: 0x000297A4
	public void Hit()
	{
		this.rb.isKinematic = false;
		Vector3 normalized = ZPMath.RandomVec3(GameManager.rand, new Vector3(0f, 0f, -1f), new Vector3(0f, 1f, 1f)).normalized;
		normalized = (-Vector3.right + normalized * 0.25f).normalized;
		Vector3 force = normalized * ZPMath.RandomFloat(GameManager.rand, this.minThrowStrength, this.maxThrowStrength);
		this.rb.AddForce(force);
		this.spherCol.enabled = true;
		this.hit = true;
		this.hitTime = Time.time;
	}

	// Token: 0x0400003B RID: 59
	public Rigidbody rb;

	// Token: 0x0400003C RID: 60
	public SphereCollider spherCol;

	// Token: 0x0400003D RID: 61
	public float gravity;

	// Token: 0x0400003E RID: 62
	public float minThrowStrength = 1200f;

	// Token: 0x0400003F RID: 63
	public float maxThrowStrength = 1400f;

	// Token: 0x04000040 RID: 64
	private bool hit;

	// Token: 0x04000041 RID: 65
	private Vector3 velocity;

	// Token: 0x04000042 RID: 66
	private float hitTime;
}
