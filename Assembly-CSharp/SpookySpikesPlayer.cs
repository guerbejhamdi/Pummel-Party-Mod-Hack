using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x02000265 RID: 613
public class SpookySpikesPlayer : CharacterBase
{
	// Token: 0x06001205 RID: 4613 RVA: 0x00004FEE File Offset: 0x000031EE
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
	}

	// Token: 0x06001206 RID: 4614 RVA: 0x0008BB50 File Offset: 0x00089D50
	protected override void Start()
	{
		base.Start();
		this.minigameController = (SpookySpikesController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.cameraShake = this.minigameController.Root.GetComponentInChildren<CameraShake>();
		this.beginPosition = base.transform.position;
	}

	// Token: 0x06001207 RID: 4615 RVA: 0x0008BBBC File Offset: 0x00089DBC
	private void Update()
	{
		if (this.minigameController.Playable)
		{
			this.curDur = this.duration / this.minigameController.MinigameSpeed;
			if (Time.time - this.lastInputTime > 0.3f)
			{
				this.lastInput = SpookySpikesPlayer.MovementState.Running;
			}
			if (base.IsOwner && !this.player.IsAI && this.hits < 5)
			{
				if (base.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.Accept))
				{
					if (this.curState == SpookySpikesPlayer.MovementState.Running)
					{
						base.StartCoroutine(this.Jump());
						this.lastInput = SpookySpikesPlayer.MovementState.Running;
					}
					else
					{
						this.lastInput = SpookySpikesPlayer.MovementState.Jumping;
						this.lastInputTime = Time.time;
					}
				}
				else if (this.lastInput == SpookySpikesPlayer.MovementState.Jumping && this.curState == SpookySpikesPlayer.MovementState.Running)
				{
					base.StartCoroutine(this.Jump());
					this.lastInput = SpookySpikesPlayer.MovementState.Running;
				}
				if (base.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.Action1) || this.lastInput == SpookySpikesPlayer.MovementState.Crouching)
				{
					if (this.curState == SpookySpikesPlayer.MovementState.Running)
					{
						base.StartCoroutine(this.Crouch());
						this.lastInput = SpookySpikesPlayer.MovementState.Running;
					}
					else if (this.lastInput == SpookySpikesPlayer.MovementState.Running)
					{
						this.lastInput = SpookySpikesPlayer.MovementState.Crouching;
						this.lastInputTime = Time.time;
					}
				}
			}
			if (this.root_active)
			{
				this.playerAnim.Animator.SetFloat("Speed", this.minigameController.MinigameSpeed);
			}
			if (base.transform.position.x < this.beginPosition.x + (float)this.hits * this.hitDistance)
			{
				base.transform.position = new Vector3(base.transform.position.x + 5f * Time.deltaTime * this.minigameController.MinigameSpeed, base.transform.position.y, base.transform.position.z);
				return;
			}
			if (this.hits >= 5 && this.root_active)
			{
				base.transform.position -= Vector3.up * Time.deltaTime * 15f;
				if (base.transform.position.y < -5f)
				{
					this.minigameController.KillPlayer();
					this.Deactivate();
				}
			}
		}
	}

	// Token: 0x06001208 RID: 4616 RVA: 0x0008BE10 File Offset: 0x0008A010
	private void OnTriggerEnter(Collider other)
	{
		if (!base.IsOwner)
		{
			return;
		}
		if (this.hitColliders.Contains(other.transform.gameObject))
		{
			return;
		}
		this.hitColliders.Add(other.transform.gameObject);
		if (other.gameObject.name == "ScoreCollider")
		{
			if (NetSystem.IsServer)
			{
				short score = this.Score;
				this.Score = score + 1;
				return;
			}
			base.SendRPC("RPCGotScore", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
			return;
		}
		else
		{
			if (other.gameObject.name == "HitCollider")
			{
				this.OnHit();
				return;
			}
			if (this.player.IsAI)
			{
				float time = this.minigameController.TimeSinceStart() / this.minigameController.round_length;
				float num = this.difficultyCurves[(int)this.player.Difficulty].Evaluate(time);
				if (UnityEngine.Random.value < num)
				{
					if (other.transform.parent.position.y > 0.75f)
					{
						base.StartCoroutine(this.Crouch());
						return;
					}
					base.StartCoroutine(this.Jump());
				}
			}
			return;
		}
	}

	// Token: 0x06001209 RID: 4617 RVA: 0x0008BF34 File Offset: 0x0008A134
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.SERVER)]
	public void RPCGotScore(NetPlayer sender)
	{
		short score = this.Score;
		this.Score = score + 1;
	}

	// Token: 0x0600120A RID: 4618 RVA: 0x0000E9F5 File Offset: 0x0000CBF5
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCHit(NetPlayer sender)
	{
		this.OnHit();
	}

	// Token: 0x0600120B RID: 4619 RVA: 0x0008BF54 File Offset: 0x0008A154
	private void OnHit()
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCHit", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		if (this.hits < 5)
		{
			AudioSystem.PlayOneShot("DeathSplash01", 0.5f, 0.1f);
			if (Settings.BloodEffects)
			{
				ParticleSystem component = UnityEngine.Object.Instantiate<GameObject>(this.playerDeathEffect, base.MidPoint, Quaternion.identity).GetComponent<ParticleSystem>();
				ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = component.velocityOverLifetime;
				velocityOverLifetime.enabled = true;
				velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
				Vector3 midPoint = base.MidPoint;
				Vector3 vector = new Vector3(5f, 1f, 0f);
				velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(Mathf.Min(0f, vector.x), Mathf.Max(0f, vector.x));
				velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(Mathf.Min(0f, vector.y), Mathf.Max(0f, vector.y));
				velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(Mathf.Min(0f, vector.z), Mathf.Max(0f, vector.z));
				ParticleSystem.EmissionModule emission = component.emission;
				ParticleSystem.Burst[] array = new ParticleSystem.Burst[emission.burstCount];
				emission.GetBursts(array);
				array[0].maxCount = (short)((float)array[0].maxCount * 0.5f);
				array[0].minCount = (short)((float)array[0].minCount * 0.5f);
				emission.SetBursts(array);
			}
			this.hits++;
		}
	}

	// Token: 0x0600120C RID: 4620 RVA: 0x0000E9FD File Offset: 0x0000CBFD
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCJump(NetPlayer sender)
	{
		if (this.curState == SpookySpikesPlayer.MovementState.Running)
		{
			base.StartCoroutine(this.Jump());
		}
	}

	// Token: 0x0600120D RID: 4621 RVA: 0x0000EA14 File Offset: 0x0000CC14
	private IEnumerator Jump()
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCJump", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		this.curState = SpookySpikesPlayer.MovementState.Jumping;
		this.playerAnim.Animator.ResetTrigger("FinishJump");
		this.playerAnim.Animator.SetTrigger("Jump");
		float startY = base.transform.position.y;
		float startTime = Time.time;
		bool stoppedAnim = false;
		yield return new WaitUntil(delegate()
		{
			float num = Time.time - startTime;
			float num2 = this.curDur * 1f;
			if (num >= num2)
			{
				this.transform.position = new Vector3(this.transform.position.x, startY, this.transform.position.z);
				return true;
			}
			if (!stoppedAnim && num >= num2 - this.transitionDuration / this.minigameController.MinigameSpeed)
			{
				this.playerAnim.Animator.SetTrigger("FinishJump");
				stoppedAnim = true;
			}
			float num3 = num / num2;
			float num4 = this.height * 4f * (num3 - num3 * num3);
			Vector3 position = this.transform.position;
			position.y = startY;
			position.y += num4;
			this.transform.position = position;
			return false;
		});
		this.curState = SpookySpikesPlayer.MovementState.Running;
		yield break;
	}

	// Token: 0x0600120E RID: 4622 RVA: 0x0000EA23 File Offset: 0x0000CC23
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCCrouch(NetPlayer sender)
	{
		if (this.curState == SpookySpikesPlayer.MovementState.Running)
		{
			base.StartCoroutine(this.Crouch());
		}
	}

	// Token: 0x0600120F RID: 4623 RVA: 0x0000EA3A File Offset: 0x0000CC3A
	private IEnumerator Crouch()
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCCrouch", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		this.curState = SpookySpikesPlayer.MovementState.Crouching;
		this.playerAnim.Animator.ResetTrigger("StopCrouch");
		this.playerAnim.Animator.SetTrigger("Crouch");
		this.capsuleCollider.center = new Vector3(0f, -0.5f, 0f);
		float startTime = Time.time;
		bool stoppedAnim = false;
		yield return new WaitUntil(delegate()
		{
			float num = Time.time - startTime;
			float num2 = this.curDur * 1f;
			if (num >= num2)
			{
				return true;
			}
			if (!stoppedAnim && num >= num2 - 0.65f / this.minigameController.MinigameSpeed)
			{
				this.playerAnim.Animator.SetTrigger("StopCrouch");
				stoppedAnim = true;
			}
			return false;
		});
		this.capsuleCollider.center = Vector3.zero;
		this.curState = SpookySpikesPlayer.MovementState.Running;
		yield break;
	}

	// Token: 0x040012C2 RID: 4802
	public float jumpVel = 10f;

	// Token: 0x040012C3 RID: 4803
	public float grav = 10f;

	// Token: 0x040012C4 RID: 4804
	public float transitionDuration = 0.15f;

	// Token: 0x040012C5 RID: 4805
	public AnimationCurve[] difficultyCurves = new AnimationCurve[3];

	// Token: 0x040012C6 RID: 4806
	public float height = 2f;

	// Token: 0x040012C7 RID: 4807
	public float duration = 1f;

	// Token: 0x040012C8 RID: 4808
	public GameObject playerDeathEffect;

	// Token: 0x040012C9 RID: 4809
	public CapsuleCollider capsuleCollider;

	// Token: 0x040012CA RID: 4810
	private SpookySpikesPlayer.MovementState curState;

	// Token: 0x040012CB RID: 4811
	private SpookySpikesController minigameController;

	// Token: 0x040012CC RID: 4812
	private CameraShake cameraShake;

	// Token: 0x040012CD RID: 4813
	private float curDur;

	// Token: 0x040012CE RID: 4814
	private int hits;

	// Token: 0x040012CF RID: 4815
	private Vector3 beginPosition;

	// Token: 0x040012D0 RID: 4816
	private Vector3 curPosition;

	// Token: 0x040012D1 RID: 4817
	private float hitDistance = 1.25f;

	// Token: 0x040012D2 RID: 4818
	private float lastInputTime;

	// Token: 0x040012D3 RID: 4819
	private SpookySpikesPlayer.MovementState lastInput;

	// Token: 0x040012D4 RID: 4820
	private List<GameObject> hitColliders = new List<GameObject>();

	// Token: 0x02000266 RID: 614
	private enum MovementState
	{
		// Token: 0x040012D6 RID: 4822
		Running,
		// Token: 0x040012D7 RID: 4823
		Jumping,
		// Token: 0x040012D8 RID: 4824
		Crouching
	}
}
