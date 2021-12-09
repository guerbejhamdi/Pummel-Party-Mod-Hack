using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using ZP.Utility;

// Token: 0x0200005F RID: 95
public class FinderPlayerFake : MonoBehaviour
{
	// Token: 0x1700003E RID: 62
	// (get) Token: 0x060001BD RID: 445 RVA: 0x00004A9C File Offset: 0x00002C9C
	// (set) Token: 0x060001BE RID: 446 RVA: 0x00004AA4 File Offset: 0x00002CA4
	public bool SharkAttack
	{
		get
		{
			return this.sharkAttack;
		}
		set
		{
			this.sharkAttack = value;
			this.mover.Velocity = Vector3.zero;
		}
	}

	// Token: 0x060001BF RID: 447 RVA: 0x0003313C File Offset: 0x0003133C
	private void Awake()
	{
		this.controller = (FinderController)GameManager.Minigame;
		this.mover.IsAI = true;
		this.agent.updatePosition = true;
		this.agent.updateRotation = false;
		this.agent.updateUpAxis = false;
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x00004ABD File Offset: 0x00002CBD
	private void Start()
	{
		this.playerAnim.Animator.SetTrigger("TreadingWater");
		this.playerAnim.Animator.SetFloat("RandomOffset", (float)GameManager.rand.NextDouble());
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x00004AF4 File Offset: 0x00002CF4
	private void Update()
	{
		if (this.gotPositions)
		{
			this.DoMovement();
		}
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x0003318C File Offset: 0x0003138C
	private void DoMovement()
	{
		CharacterMoverInput aiinput = this.GetAIInput();
		aiinput.NullInput(this.SharkAttack);
		this.mover.CalculateVelocity(aiinput, Time.deltaTime);
		this.mover.DoMovement(Time.deltaTime);
		this.mover.SmoothSlope();
		if (this.mover.MovementAxis != Vector2.zero)
		{
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(new Vector3(this.mover.MovementAxis.x, 0f, this.mover.MovementAxis.y), Vector3.up), this.turnSpeed * Time.deltaTime);
		}
		this.UpdateAnimationState();
		this.playerAnim.UpdateAnimationState();
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x0003325C File Offset: 0x0003145C
	private void UpdateAnimationState()
	{
		Vector2 vector = new Vector2(this.mover.Velocity.x, this.mover.Velocity.z);
		float num = Mathf.Clamp01(vector.magnitude / this.mover.maxSpeed);
		this.playerAnim.Velocity = num;
		this.playerAnim.VelocityY = this.mover.Velocity.y;
		this.playerAnim.MovementAxis = ((num > 0.01f) ? Vector2.up : Vector2.zero);
		this.playerAnim.Grounded = true;
		this.playerAnim.SetPlayerRotation(base.transform.rotation.eulerAngles.y);
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x00004B04 File Offset: 0x00002D04
	public void GetPositions(int seed)
	{
		this.gotPositions = true;
		base.StartCoroutine(this.GetPositionsWait(seed));
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x00004B1B File Offset: 0x00002D1B
	private IEnumerator GetPositionsWait(int seed)
	{
		this.rand = new System.Random(seed);
		ZPMath.RandomFloat(this.rand, 0f, 3f);
		yield return null;
		this.currentTarget = this.controller.GetRandomNavMeshPoint(this.rand);
		this.nextTarget = this.controller.GetRandomNavMeshPoint(this.rand);
		this.gotToTarget = false;
		if (this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
		{
			this.agent.SetDestination(this.currentTarget);
		}
		yield break;
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x00033320 File Offset: 0x00031520
	private CharacterMoverInput GetAIInput()
	{
		float num = 1f;
		Vector2 b = new Vector2(base.transform.position.x, base.transform.position.z);
		Vector3 vector = this.gotToTarget ? this.nextTarget : this.currentTarget;
		if (this.pathUpdateTimer.Elapsed(true) && this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
		{
			this.agent.SetDestination(vector);
		}
		if ((new Vector2(vector.x, vector.z) - b).sqrMagnitude <= num || vector == Vector3.zero)
		{
			this.gotToTarget = true;
		}
		Vector3 vector2 = this.agent.steeringTarget - base.transform.position;
		Vector2 normalized = new Vector2(vector2.x, vector2.z).normalized;
		CharacterMoverInput result = new CharacterMoverInput(normalized, false, false);
		Debug.DrawLine(base.transform.position, vector, Color.red);
		return result;
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x00033434 File Offset: 0x00031634
	public void Setup(int playerID)
	{
		this.playerAnim.SetPlayer(playerID);
		Material material = new Material(this.floatyRenderer.material);
		material.color = GameManager.GetPlayerAt(playerID).Color.skinColor1;
		this.floatyRenderer.material = material;
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x00033480 File Offset: 0x00031680
	public void Kill()
	{
		this.isDead = true;
		this.character.SetActive(false);
		AudioSystem.PlayOneShot(this.popSound, 0.2f, 0f, 1f);
		this.controller.Spawn(this.smokeParticle, base.transform.position, Quaternion.identity);
	}

	// Token: 0x04000213 RID: 531
	public GameObject character;

	// Token: 0x04000214 RID: 532
	public MeshRenderer floatyRenderer;

	// Token: 0x04000215 RID: 533
	public float turnSpeed = 1500f;

	// Token: 0x04000216 RID: 534
	public PlayerAnimation playerAnim;

	// Token: 0x04000217 RID: 535
	public CharacterMover mover;

	// Token: 0x04000218 RID: 536
	public NavMeshAgent agent;

	// Token: 0x04000219 RID: 537
	public SkinnedMeshRenderer smr;

	// Token: 0x0400021A RID: 538
	public AudioClip popSound;

	// Token: 0x0400021B RID: 539
	private FinderController controller;

	// Token: 0x0400021C RID: 540
	private System.Random rand;

	// Token: 0x0400021D RID: 541
	private bool sharkAttack;

	// Token: 0x0400021E RID: 542
	private bool gotPositions;

	// Token: 0x0400021F RID: 543
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x04000220 RID: 544
	private Vector3 currentTarget = Vector3.zero;

	// Token: 0x04000221 RID: 545
	private Vector3 nextTarget = Vector3.zero;

	// Token: 0x04000222 RID: 546
	private bool gotToTarget;

	// Token: 0x04000223 RID: 547
	public bool isDead;

	// Token: 0x04000224 RID: 548
	public GameObject smokeParticle;
}
