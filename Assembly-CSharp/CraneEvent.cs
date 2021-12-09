using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000346 RID: 838
public class CraneEvent : BoardNodeEvent
{
	// Token: 0x0600169C RID: 5788 RVA: 0x0009FE6C File Offset: 0x0009E06C
	private void Start()
	{
		this.rigidBodies = base.GetComponentsInChildren<Rigidbody>();
		for (int i = 0; i < this.rigidBodies.Length; i++)
		{
			this.transforms.Add(this.rigidBodies[i].transform);
			this.startPositions.Add(this.rigidBodies[i].transform.position);
			this.rotations.Add(this.rigidBodies[i].transform.rotation);
			this.rigidBodies[i].isKinematic = true;
			CraneCollisionHitSound craneCollisionHitSound = this.rigidBodies[i].gameObject.AddComponent<CraneCollisionHitSound>();
			craneCollisionHitSound.clips = this.hitClips;
			craneCollisionHitSound.minVol = this.minVol;
			craneCollisionHitSound.maxVol = this.maxVol;
			craneCollisionHitSound.minPitch = this.minPitch;
			craneCollisionHitSound.maxPitch = this.maxPitch;
			craneCollisionHitSound.minMagnitude = 0.3f;
		}
	}

	// Token: 0x0600169D RID: 5789 RVA: 0x00011093 File Offset: 0x0000F293
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode boardNode, int seed)
	{
		this.rand = new System.Random(seed);
		this.Reset(false);
		AudioSystem.PlayOneShot(this.whipCrack, this.whipVolume, 0f, 1f);
		AudioSystem.PlayOneShot(this.wood, this.woodVol, 0f, 1f);
		for (int i = 0; i < this.particleSystems.Length; i++)
		{
			this.particleSystems[i].Play();
		}
		yield return new WaitForSeconds(this.waitTime);
		DamageInstance d = new DamageInstance
		{
			damage = this.rand.Next(7, 9),
			origin = base.transform.position,
			blood = true,
			ragdoll = true,
			ragdollVel = 10f,
			bloodVel = 0f,
			bloodAmount = 1f,
			sound = true,
			volume = 0.25f,
			details = "Crane Event",
			removeKeys = true
		};
		player.ApplyDamage(d);
		GameManager.Board.boardCamera.AddShake(0.35f);
		yield return new WaitForSeconds(2f);
		base.StartCoroutine(this.Reset());
		yield break;
	}

	// Token: 0x0600169E RID: 5790 RVA: 0x000110B0 File Offset: 0x0000F2B0
	private IEnumerator Reset()
	{
		yield return new WaitForSeconds(2f);
		this.Reset(true);
		yield break;
	}

	// Token: 0x0600169F RID: 5791 RVA: 0x0009FF58 File Offset: 0x0009E158
	private void Reset(bool kinematic)
	{
		for (int i = 0; i < this.transforms.Count; i++)
		{
			this.transforms[i].position = this.startPositions[i];
			this.transforms[i].rotation = this.rotations[i];
			this.rigidBodies[i].velocity = Vector3.zero;
			this.rigidBodies[i].angularVelocity = Vector3.zero;
			this.rigidBodies[i].isKinematic = kinematic;
		}
	}

	// Token: 0x060016A0 RID: 5792 RVA: 0x000110BF File Offset: 0x0000F2BF
	private void OnDisable()
	{
		base.StopAllCoroutines();
		this.Reset(true);
	}

	// Token: 0x040017D4 RID: 6100
	public MeshRenderer meshRenderer;

	// Token: 0x040017D5 RID: 6101
	public SkinnedMeshRenderer skinnedMeshRenderer;

	// Token: 0x040017D6 RID: 6102
	public ParticleSystem[] particleSystems;

	// Token: 0x040017D7 RID: 6103
	public float waitTime = 2.5f;

	// Token: 0x040017D8 RID: 6104
	[Header("Sounds")]
	public AudioClip whipCrack;

	// Token: 0x040017D9 RID: 6105
	public float whipVolume = 0.5f;

	// Token: 0x040017DA RID: 6106
	public AudioClip wood;

	// Token: 0x040017DB RID: 6107
	public float woodVol = 0.5f;

	// Token: 0x040017DC RID: 6108
	public AudioClip[] hitClips;

	// Token: 0x040017DD RID: 6109
	public float maxVol = 0.025f;

	// Token: 0x040017DE RID: 6110
	public float minVol = 0.025f;

	// Token: 0x040017DF RID: 6111
	public float maxPitch = 0.65f;

	// Token: 0x040017E0 RID: 6112
	public float minPitch = 0.55f;

	// Token: 0x040017E1 RID: 6113
	private Rigidbody[] rigidBodies;

	// Token: 0x040017E2 RID: 6114
	private List<Transform> transforms = new List<Transform>();

	// Token: 0x040017E3 RID: 6115
	private List<Vector3> startPositions = new List<Vector3>();

	// Token: 0x040017E4 RID: 6116
	private List<Quaternion> rotations = new List<Quaternion>();
}
