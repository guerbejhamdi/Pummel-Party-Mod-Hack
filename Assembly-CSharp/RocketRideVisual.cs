using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000324 RID: 804
public class RocketRideVisual : MonoBehaviour
{
	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x060015F0 RID: 5616 RVA: 0x000109B7 File Offset: 0x0000EBB7
	// (set) Token: 0x060015F1 RID: 5617 RVA: 0x000109BF File Offset: 0x0000EBBF
	public bool LiftOff { get; set; }

	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x060015F2 RID: 5618 RVA: 0x000109C8 File Offset: 0x0000EBC8
	// (set) Token: 0x060015F3 RID: 5619 RVA: 0x000109D0 File Offset: 0x0000EBD0
	public bool Lit { get; set; }

	// Token: 0x060015F4 RID: 5620 RVA: 0x0009D058 File Offset: 0x0009B258
	public void Update()
	{
		if (this.LiftOff)
		{
			base.transform.position += base.transform.forward * (this.rocketSkewerItem.moveSpeed + this.bonusSpeed) * Time.deltaTime;
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, this.upRot, 75f * Time.deltaTime);
			this.bonusSpeed += 7f * Time.deltaTime;
		}
		for (int i = 0; i < this.attachedRagdolls.Count; i++)
		{
			this.attachedRagdolls[i].transform.position = this.noseTransform.position;
			this.attachedRagdolls[i].transform.rotation = this.noseTransform.rotation;
		}
		if (this.Lit && !this.LiftOff)
		{
			float num = 0.07f;
			if (GameManager.Board.boardCamera.CameraShake.Trauma < num)
			{
				GameManager.Board.boardCamera.CameraShake.Trauma = num;
			}
		}
	}

	// Token: 0x060015F5 RID: 5621 RVA: 0x000109D9 File Offset: 0x0000EBD9
	public void OnDropAnimEnd()
	{
		this.rocketSkewerItem.OnDropAnimEnd();
	}

	// Token: 0x060015F6 RID: 5622 RVA: 0x000109E6 File Offset: 0x0000EBE6
	public void OnLightAnimEnd()
	{
		this.rocketSkewerItem.OnLightAnimEnd();
	}

	// Token: 0x060015F7 RID: 5623 RVA: 0x000109F3 File Offset: 0x0000EBF3
	public void OnDropHitGround()
	{
		GameManager.Board.boardCamera.AddShake(0.2f);
	}

	// Token: 0x060015F8 RID: 5624 RVA: 0x00010A09 File Offset: 0x0000EC09
	public void OnDropHitSOund()
	{
		AudioSystem.PlayOneShot(this.hitGroundSound, 0.5f, 0f, 1f);
	}

	// Token: 0x04001713 RID: 5907
	public Animation anim;

	// Token: 0x04001714 RID: 5908
	public Transform stand;

	// Token: 0x04001715 RID: 5909
	public Transform nozeltransform;

	// Token: 0x04001716 RID: 5910
	public Transform playerTransform;

	// Token: 0x04001717 RID: 5911
	public Transform noseTransform;

	// Token: 0x04001718 RID: 5912
	public AudioClip hitGroundSound;

	// Token: 0x04001719 RID: 5913
	public RocketSkewerItem rocketSkewerItem;

	// Token: 0x0400171A RID: 5914
	public List<Transform> attachedRagdolls = new List<Transform>();

	// Token: 0x0400171B RID: 5915
	public GameObject scaffoldObject;

	// Token: 0x0400171C RID: 5916
	public GameObject[] scaffoldDebriObjects;

	// Token: 0x0400171D RID: 5917
	private Quaternion upRot = Quaternion.LookRotation(Vector3.up);

	// Token: 0x0400171E RID: 5918
	private float bonusSpeed;
}
