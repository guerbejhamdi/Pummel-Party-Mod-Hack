using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Utility;

// Token: 0x0200059F RID: 1439
public class ZombieHazard : BoardNodeEvent
{
	// Token: 0x0600256C RID: 9580 RVA: 0x000E21E0 File Offset: 0x000E03E0
	private void OnEnable()
	{
		this.pathinRand = new System.Random(this.seed);
		if (this.mainMenu)
		{
			this.startPosition = base.transform.position;
			return;
		}
		this.agent.transform.position = this.node.transform.position;
	}

	// Token: 0x0600256D RID: 9581 RVA: 0x000E2238 File Offset: 0x000E0438
	public Vector3 GetRandomNavMeshPoint()
	{
		if (this.binaryTree == null)
		{
			this.triangulation = NavMesh.CalculateTriangulation();
			if (this.triangulation.vertices.Length != 0)
			{
				List<float> list = new List<float>();
				for (int i = 0; i < this.triangulation.indices.Length / 3; i++)
				{
					int num = i * 3;
					Vector3 vector = this.triangulation.vertices[this.triangulation.indices[num]];
					Vector3 vector2 = this.triangulation.vertices[this.triangulation.indices[num + 1]];
					Vector3 vector3 = this.triangulation.vertices[this.triangulation.indices[num + 2]];
					float num2 = Vector3.Distance(vector, vector2);
					float num3 = Vector3.Distance(vector2, vector3);
					float num4 = Vector3.Distance(vector3, vector);
					float num5 = (num2 + num3 + num4) / 2f;
					float num6 = Mathf.Sqrt(num5 * (num5 - num2) * (num5 - num3) * (num5 - num4));
					list.Add(this.totalArea);
					this.totalArea += num6;
				}
				this.binaryTree = new BinaryTree(list.ToArray());
			}
		}
		if (this.binaryTree != null)
		{
			float p = ZPMath.RandomFloat(this.pathinRand, 0f, this.totalArea);
			int num7 = this.binaryTree.FindPoint(p) * 3;
			Vector3[] vertices = this.triangulation.vertices;
			int[] indices = this.triangulation.indices;
			return ZPMath.RandomTrianglePoint(vertices[indices[num7]], vertices[indices[num7 + 1]], vertices[indices[num7 + 2]], this.pathinRand);
		}
		return Vector3.zero;
	}

	// Token: 0x0600256E RID: 9582 RVA: 0x000E23F0 File Offset: 0x000E05F0
	private void Update()
	{
		if (!this.eventActive && this.agent.remainingDistance < 0.05f)
		{
			Vector3 vector = Vector3.zero;
			if (!this.mainMenu)
			{
				vector = this.node.transform.position;
				Vector3 a = ZPMath.RandomPointInUnitSphere(this.pathinRand);
				a.y = 0f;
				vector = this.node.transform.position + a * 3.5f;
				NavMeshHit navMeshHit;
				if (NavMesh.SamplePosition(vector, out navMeshHit, 3f, -1))
				{
					vector = navMeshHit.position;
					Debug.DrawLine(this.node.transform.position, vector, Color.green, float.MaxValue);
				}
				else
				{
					Debug.DrawLine(this.node.transform.position, vector, Color.red, float.MaxValue);
				}
			}
			else
			{
				vector = this.GetRandomNavMeshPoint();
			}
			if (this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
			{
				this.agent.SetDestination(vector);
			}
		}
		this.anim.SetFloat("VelocityNorm", Mathf.Clamp((Vector3.Dot(this.agent.velocity.normalized, (this.agent.steeringTarget - base.transform.position).normalized) + 1f) / 2f, this.minSpeed, this.maxSpeed));
	}

	// Token: 0x0600256F RID: 9583 RVA: 0x0001ACAD File Offset: 0x00018EAD
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode node, int seed)
	{
		float startTime = Time.time;
		this.eventActive = true;
		this.rand = new System.Random(seed);
		float stopDistance = this.agent.stoppingDistance;
		this.agent.stoppingDistance = 1.25f;
		if (this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
		{
			this.agent.SetDestination(player.transform.position);
		}
		AudioSystem.PlayOneShot(this.activateClip, 0.75f, 0f, 1f);
		yield return new WaitUntil(delegate()
		{
			Vector3 normalized = (player.transform.position - this.transform.position).normalized;
			this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(normalized), 90f * Time.deltaTime);
			float num2 = Vector3.Dot(this.transform.forward, normalized);
			return (this.agent.remainingDistance < 1.35f && num2 > 0.975f) || Time.time - startTime > 3f;
		});
		this.anim.SetTrigger("NeckBite");
		yield return new WaitForSeconds(1f);
		float biteLength = 1.7f;
		int hitCount = this.rand.Next(7, 9);
		AudioSystem.PlayOneShot(this.biteClip, 0.5f, 0f, 1f);
		int num;
		for (int i = 0; i < hitCount; i = num)
		{
			DamageInstance d = new DamageInstance
			{
				damage = 1,
				origin = player.transform.position - ZPMath.RandomVec3(this.rand, -1f, 1f),
				blood = true,
				ragdollVel = 0.5f,
				bloodVel = 8f,
				bloodAmount = 0.65f,
				details = "Zombie Bite",
				hitAnim = true,
				removeKeys = (i == hitCount - 1)
			};
			player.ApplyDamage(d);
			GameManager.Board.boardCamera.AddShake(0.1f);
			AudioSystem.PlayOneShot(this.hitSound, 0.3f, 0.05f, 1f);
			yield return new WaitForSeconds(biteLength / (float)hitCount);
			num = i + 1;
		}
		yield return new WaitForSeconds(1f);
		this.agent.stoppingDistance = stopDistance;
		this.eventActive = false;
		yield break;
	}

	// Token: 0x040028DD RID: 10461
	public NavMeshAgent agent;

	// Token: 0x040028DE RID: 10462
	public BoardNode node;

	// Token: 0x040028DF RID: 10463
	public Animator anim;

	// Token: 0x040028E0 RID: 10464
	public AudioClip hitSound;

	// Token: 0x040028E1 RID: 10465
	public AudioClip biteClip;

	// Token: 0x040028E2 RID: 10466
	public AudioClip activateClip;

	// Token: 0x040028E3 RID: 10467
	public int seed;

	// Token: 0x040028E4 RID: 10468
	public ActionTimer newPointTimer = new ActionTimer(6f, 11f);

	// Token: 0x040028E5 RID: 10469
	private bool eventActive;

	// Token: 0x040028E6 RID: 10470
	private System.Random pathinRand;

	// Token: 0x040028E7 RID: 10471
	public float minSpeed = 0.3f;

	// Token: 0x040028E8 RID: 10472
	public float maxSpeed = 1f;

	// Token: 0x040028E9 RID: 10473
	public bool mainMenu;

	// Token: 0x040028EA RID: 10474
	private Vector3 startPosition;

	// Token: 0x040028EB RID: 10475
	private NavMeshTriangulation triangulation;

	// Token: 0x040028EC RID: 10476
	private BinaryTree binaryTree;

	// Token: 0x040028ED RID: 10477
	private float totalArea;
}
