using System;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200018D RID: 397
public class CannonCirclePlayer : Movement1
{
	// Token: 0x06000B53 RID: 2899 RVA: 0x00060EE8 File Offset: 0x0005F0E8
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.mover = base.GetComponent<CharacterMover>();
		if (base.IsOwner)
		{
			this.net_z_rotation.Value = ZPMath.CompressFloat(0f, -45f, 45f);
		}
		if (!base.IsOwner)
		{
			base.GetComponent<CharacterController>().enabled = false;
		}
	}

	// Token: 0x06000B54 RID: 2900 RVA: 0x0000AEDF File Offset: 0x000090DF
	public override void OnOwnerChanged()
	{
		if (NetSystem.IsServer)
		{
			base.GetComponent<CharacterController>().enabled = true;
		}
		base.OnOwnerChanged();
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x00060F44 File Offset: 0x0005F144
	protected override void Start()
	{
		base.Start();
		this.minigameController = (CannonCircleController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.cameraShake = this.minigameController.Root.GetComponentInChildren<CameraShake>();
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x0000ABAF File Offset: 0x00008DAF
	private void Update()
	{
		if (!this.isDead)
		{
			base.UpdateController();
		}
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x00060FA0 File Offset: 0x0005F1A0
	protected override void DoMovement()
	{
		CharacterMoverInput input = default(CharacterMoverInput);
		bool val = !this.minigameController.Playable || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput || this.isDead;
		if (!base.GamePlayer.IsAI)
		{
			Vector2 axis = new Vector2(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
			input = new CharacterMoverInput(axis, false, false);
		}
		else
		{
			input = this.GetAIInput();
		}
		if (this.agent == null || !this.agent.isOnOffMeshLink)
		{
			input.NullInput(val);
			this.mover.CalculateVelocity(input, Time.deltaTime);
			this.mover.DoMovement(Time.deltaTime);
			this.mover.SmoothSlope();
			if (this.mover.MovementAxis != Vector2.zero)
			{
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(new Vector3(this.mover.MovementAxis.x, 0f, this.mover.MovementAxis.y), Vector3.up), 1500f * Time.deltaTime);
			}
		}
		this.velocity.Value = this.mover.Velocity;
		this.netIsGrounded.Value = (this.curOffMeshLinkTranslationType != OffMeshLinkTranslateType.Parabola && this.mover.Grounded);
		base.DoMovement();
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x00061138 File Offset: 0x0005F338
	protected override void UpdateAnimationState(PlayerAnimation playerAnim)
	{
		Vector2 vector = new Vector2(this.velocity.Value.x, this.velocity.Value.z);
		float num = Mathf.Clamp01(vector.magnitude / this.mover.maxSpeed);
		playerAnim.Velocity = num;
		playerAnim.VelocityY = this.velocity.Value.y;
		playerAnim.MovementAxis = ((num > 0.01f) ? Vector2.up : Vector2.zero);
		playerAnim.Grounded = this.netIsGrounded.Value;
		playerAnim.SetPlayerRotation(this.rotation.Value);
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x000611E0 File Offset: 0x0005F3E0
	private CharacterMoverInput GetAIInput()
	{
		if (this.avoidanceTimer.Elapsed(true))
		{
			float d = 60f;
			float num = 9f;
			Transform x = null;
			Vector3 a = Vector3.zero;
			float num2 = float.MaxValue;
			int i = 0;
			while (i < this.minigameController.cannonCircleProjectiles.Count)
			{
				if (this.minigameController.cannonCircleProjectiles[i] == null)
				{
					this.minigameController.cannonCircleProjectiles.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
			foreach (Transform transform in this.minigameController.cannonCircleProjectiles)
			{
				Vector3 vector = ZPMath.ProjectPointOnLineSegment(transform.position, transform.position + transform.forward * d, base.transform.position);
				if (!vector.Equals(transform.position) && (vector - base.transform.position).sqrMagnitude < num)
				{
					float sqrMagnitude = (transform.position - base.transform.position).sqrMagnitude;
					if (sqrMagnitude < num2)
					{
						x = transform;
						a = vector;
						num2 = sqrMagnitude;
					}
				}
			}
			if (x != null)
			{
				Vector3 vector2 = a - base.transform.position;
				this.curAvoidanceVec = new Vector2(vector2.x, vector2.z);
				this.curAvoidanceVec.Normalize();
				Vector3 vector3 = base.transform.position - Vector3.zero;
				Vector2 normalized = new Vector2(vector3.x, vector3.z).normalized;
				float num3 = Vector3.Dot(normalized, this.curAvoidanceVec);
				if (Vector3.Dot(normalized, -this.curAvoidanceVec) > num3)
				{
					this.curAvoidanceVec = -this.curAvoidanceVec;
				}
			}
			else
			{
				this.curAvoidanceVec = Vector2.zero;
			}
		}
		float num4 = 0.36f;
		Vector2 b = new Vector2(base.transform.position.x, base.transform.position.z);
		if ((new Vector2(this.targetPosition.x, this.targetPosition.z) - b).sqrMagnitude <= num4)
		{
			this.targetPosition = ZPMath.RandomPointInUnitSphere(GameManager.rand) * 10f;
		}
		Vector3 vector4 = this.targetPosition - base.transform.position;
		Vector2 vector5 = new Vector2(vector4.x, vector4.z).normalized;
		if (this.curAvoidanceVec != Vector2.zero)
		{
			vector5 *= 0.5f;
			vector5 += this.curAvoidanceVec;
			vector5.Normalize();
		}
		return new CharacterMoverInput(vector5, false, false);
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x0000B333 File Offset: 0x00009533
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCKillPlayer(NetPlayer sender, Vector3 origin)
	{
		this.KillPlayer(origin, 13f);
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x000614F8 File Offset: 0x0005F6F8
	public void KillPlayer(Vector3 origin, float force)
	{
		if (base.IsDead)
		{
			return;
		}
		if (base.IsOwner)
		{
			base.SendRPC("RPCKillPlayer", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				origin
			});
		}
		this.minigameController.PlayerDied(this);
		float d = 20f;
		float num = 1f;
		if (Settings.BloodEffects && Time.time - this.lastBlood > this.minBloodInterval)
		{
			ParticleSystem component = UnityEngine.Object.Instantiate<GameObject>(this.bloodyDamageEffect, base.MidPoint, Quaternion.identity).GetComponent<ParticleSystem>();
			ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = component.velocityOverLifetime;
			velocityOverLifetime.enabled = true;
			velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
			Vector3 vector = (base.MidPoint - origin).normalized * d;
			velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(Mathf.Min(0f, vector.x), Mathf.Max(0f, vector.x));
			velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(Mathf.Min(0f, vector.y), Mathf.Max(0f, vector.y));
			velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(Mathf.Min(0f, vector.z), Mathf.Max(0f, vector.z));
			ParticleSystem.EmissionModule emission = component.emission;
			ParticleSystem.Burst[] array = new ParticleSystem.Burst[emission.burstCount];
			emission.GetBursts(array);
			array[0].maxCount = (short)((float)array[0].maxCount * num);
			array[0].minCount = (short)((float)array[0].minCount * num);
			emission.SetBursts(array);
			this.lastBlood = Time.time;
		}
		base.IsDead = true;
		this.SpawnRagdoll(origin, force);
		AudioSystem.PlayOneShot("DeathSplash01", base.transform.position, 0.25f, AudioRolloffMode.Logarithmic, 15f, 100f, 0f);
		if (base.IsOwner)
		{
			this.mover.Velocity = Vector3.zero;
		}
		if (this.cameraShake != null)
		{
			this.cameraShake.enabled = false;
		}
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x00053F14 File Offset: 0x00052114
	private void SpawnRagdoll(Vector3 origin, float force)
	{
		Vector3 normalized = (base.transform.position + Vector3.up * 0.875f - origin).normalized;
		this.playerAnim.SpawnRagdoll(normalized * force);
		this.Deactivate();
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x0000B341 File Offset: 0x00009541
	public void OnTriggerEnter(Collider other)
	{
		this.Trigger(other);
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x0000B341 File Offset: 0x00009541
	public void OnTriggerStay(Collider other)
	{
		this.Trigger(other);
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x0000B34A File Offset: 0x0000954A
	private void Trigger(Collider other)
	{
		if (base.IsOwner)
		{
			this.KillPlayer(other.transform.position, 7f);
		}
	}

	// Token: 0x04000A68 RID: 2664
	public GameObject bloodyDamageEffect;

	// Token: 0x04000A69 RID: 2665
	private float lastBlood;

	// Token: 0x04000A6A RID: 2666
	private float minBloodInterval = 0.5f;

	// Token: 0x04000A6B RID: 2667
	private CannonCircleController minigameController;

	// Token: 0x04000A6C RID: 2668
	private CharacterMover mover;

	// Token: 0x04000A6D RID: 2669
	private CameraShake cameraShake;

	// Token: 0x04000A6E RID: 2670
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x04000A6F RID: 2671
	private ActionTimer avoidanceTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x04000A70 RID: 2672
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x04000A71 RID: 2673
	private Vector2 curAvoidanceVec = Vector3.zero;
}
