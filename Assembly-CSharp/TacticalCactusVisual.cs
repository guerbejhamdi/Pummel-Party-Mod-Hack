using System;
using UnityEngine;

// Token: 0x020004CB RID: 1227
public class TacticalCactusVisual : MonoBehaviour
{
	// Token: 0x06002099 RID: 8345 RVA: 0x000CC304 File Offset: 0x000CA504
	private void Update()
	{
		if (!this.hit)
		{
			base.transform.rotation = Quaternion.Euler(0f, this.player.PlayerAnimation.PlayerRotation, 0f);
			this.anim.SetFloat("Velocity", this.player.moveVelocity / this.player.moveSpeed);
		}
	}

	// Token: 0x0600209A RID: 8346 RVA: 0x00017B71 File Offset: 0x00015D71
	public void HitDice()
	{
		this.anim.SetTrigger("HitDice");
	}

	// Token: 0x0600209B RID: 8347 RVA: 0x000CC36C File Offset: 0x000CA56C
	public void Ragdoll(Vector3 origin, float force)
	{
		force = Mathf.Clamp(force, 5f, float.MaxValue);
		base.transform.parent = this.player.transform.parent;
		this.rigidBody.isKinematic = false;
		for (int i = 0; i < this.colliders.Length; i++)
		{
			this.colliders[i].enabled = true;
		}
		Vector3 normalized = (base.transform.position + Vector3.up * 0.875f - origin).normalized;
		this.rigidBody.velocity = normalized * force * 1f;
		this.hit = true;
		UnityEngine.Object.Destroy(base.gameObject, 3f);
	}

	// Token: 0x0400235C RID: 9052
	public Animator anim;

	// Token: 0x0400235D RID: 9053
	public Rigidbody rigidBody;

	// Token: 0x0400235E RID: 9054
	public Collider[] colliders;

	// Token: 0x0400235F RID: 9055
	public GameObject smokeParticleSystem;

	// Token: 0x04002360 RID: 9056
	public AudioClip smokePoof;

	// Token: 0x04002361 RID: 9057
	[HideInInspector]
	public BoardPlayer player;

	// Token: 0x04002362 RID: 9058
	private bool hit;
}
