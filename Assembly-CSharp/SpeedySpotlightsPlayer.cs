using System;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200025E RID: 606
public class SpeedySpotlightsPlayer : Movement1
{
	// Token: 0x17000186 RID: 390
	// (get) Token: 0x060011A8 RID: 4520 RVA: 0x0000E71A File Offset: 0x0000C91A
	// (set) Token: 0x060011A9 RID: 4521 RVA: 0x00089614 File Offset: 0x00087814
	public bool BeingHit
	{
		get
		{
			return this.beingHit.Value;
		}
		set
		{
			this.flameParticles.enableEmission = (value || base.IsDead);
			if (value)
			{
				if (this.source == null)
				{
					this.source = AudioSystem.PlayLooping(this.sizzleClip, 0.25f, 0.6f);
				}
			}
			else if (this.source != null)
			{
				this.source.FadeAudio(0.5f, FadeType.Out);
				this.source = null;
			}
			if (base.IsOwner)
			{
				this.beingHit.Value = value;
			}
		}
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x060011AA RID: 4522 RVA: 0x0000E727 File Offset: 0x0000C927
	// (set) Token: 0x060011AB RID: 4523 RVA: 0x000896A0 File Offset: 0x000878A0
	public byte Health
	{
		get
		{
			return this.health.Value;
		}
		set
		{
			this.health.Value = value;
			if (this.minigameController != null && this.minigameController.Playable)
			{
				this.Score = (short)value;
				if (base.IsOwner && value == 0)
				{
					this.KillPlayer(GameManager.rand.Next(int.MaxValue), true);
				}
			}
		}
	}

	// Token: 0x060011AC RID: 4524 RVA: 0x000896FC File Offset: 0x000878FC
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.mover = base.GetComponent<CharacterMover>();
		if (base.IsOwner)
		{
			this.net_z_rotation.Value = ZPMath.CompressFloat(0f, -45f, 45f);
			this.GetRandomAIPos();
		}
		else
		{
			NetVar<bool> netVar = this.beingHit;
			netVar.Recieve = (RecieveProxy)Delegate.Combine(netVar.Recieve, new RecieveProxy(this.BeingHitRecieve));
			NetVar<byte> netVar2 = this.health;
			netVar2.Recieve = (RecieveProxy)Delegate.Combine(netVar2.Recieve, new RecieveProxy(this.DamageRecieve));
		}
		if (!base.IsOwner)
		{
			base.GetComponent<CharacterController>().enabled = false;
		}
		if (!this.player.IsAI || !base.IsOwner)
		{
			base.GetComponent<NavMeshAgent>().enabled = false;
		}
		if (this.player.IsAI && base.IsOwner)
		{
			this.mover.IsAI = true;
		}
		this.pointLight.color = this.player.Color.skinColor1;
	}

	// Token: 0x060011AD RID: 4525 RVA: 0x0008980C File Offset: 0x00087A0C
	public override void OnOwnerChanged()
	{
		if (NetSystem.IsServer)
		{
			base.GetComponent<CharacterController>().enabled = true;
			base.GetComponent<NavMeshAgent>().enabled = true;
			this.mover.IsAI = true;
			this.mover.SetForwardVector(Vector3.forward);
		}
		base.OnOwnerChanged();
	}

	// Token: 0x060011AE RID: 4526 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x060011AF RID: 4527 RVA: 0x0008985C File Offset: 0x00087A5C
	protected override void Start()
	{
		base.Start();
		this.minigameController = (SpeedySpotlightsController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.AddLight(this.pointLight, this.lightIntensity);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.cameraShake = this.minigameController.Root.GetComponentInChildren<CameraShake>();
		if (!this.player.IsAI)
		{
			this.mover.SetForwardVector(Vector3.forward);
		}
	}

	// Token: 0x060011B0 RID: 4528 RVA: 0x0000ABAF File Offset: 0x00008DAF
	private void Update()
	{
		if (!this.isDead)
		{
			base.UpdateController();
		}
	}

	// Token: 0x060011B1 RID: 4529 RVA: 0x000898EC File Offset: 0x00087AEC
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

	// Token: 0x060011B2 RID: 4530 RVA: 0x00089A84 File Offset: 0x00087C84
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

	// Token: 0x060011B3 RID: 4531 RVA: 0x00089B2C File Offset: 0x00087D2C
	private Vector3 GetLightHitPoint(MinigameSpotlight light)
	{
		if (light == null || light.gameObject == null || light.gameObject.transform == null)
		{
			return Vector3.zero;
		}
		Vector3 forward = light.gameObject.transform.forward;
		float d = (Vector3.zero.y - this.minigameController.lightSpawnPoint.y) / forward.y;
		return this.minigameController.lightSpawnPoint + forward * d;
	}

	// Token: 0x060011B4 RID: 4532 RVA: 0x00089BB0 File Offset: 0x00087DB0
	private CharacterMoverInput GetAIInput()
	{
		CharacterMoverInput result = default(CharacterMoverInput);
		float num = 0.36f;
		Vector2 b = new Vector2(base.transform.position.x, base.transform.position.z);
		if (this.pathUpdateTimer.Elapsed(true) && this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
		{
			this.agent.SetDestination(this.targetPosition);
		}
		if (this.runAwayTimer.Elapsed(false))
		{
			this.closestLight = null;
			float num2 = 3.5f;
			float num3 = num2 * num2;
			float num4 = float.MaxValue;
			for (int i = 0; i < this.minigameController.minigameSpotlights.Count; i++)
			{
				float sqrMagnitude = (this.GetLightHitPoint(this.minigameController.minigameSpotlights[i]) - base.transform.position).sqrMagnitude;
				if (sqrMagnitude < num3 && sqrMagnitude < num4)
				{
					num4 = sqrMagnitude;
					this.closestLight = this.minigameController.minigameSpotlights[i];
					this.runAwayTimer.SetInterval(0.4f, 0.9f, true);
					switch (base.GamePlayer.Difficulty)
					{
					case BotDifficulty.Easy:
						this.runAwayTimer.SetInterval(1.2f, 2f, true);
						break;
					case BotDifficulty.Normal:
						this.runAwayTimer.SetInterval(0.8f, 1.6f, true);
						break;
					case BotDifficulty.Hard:
						this.runAwayTimer.SetInterval(0.3f, 0.8f, true);
						break;
					}
				}
			}
			if ((double)this.difficultyChances[(int)base.GamePlayer.Difficulty] > GameManager.rand.NextDouble())
			{
				this.closestLight = null;
			}
		}
		if ((new Vector2(this.targetPosition.x, this.targetPosition.z) - b).sqrMagnitude > num || this.closestLight != null)
		{
			Vector3 vector = Vector3.zero;
			if (this.closestLight != null)
			{
				vector = base.transform.position - this.GetLightHitPoint(this.closestLight);
			}
			else
			{
				vector = this.agent.steeringTarget - base.transform.position;
			}
			Vector2 normalized = new Vector2(vector.x, vector.z).normalized;
			result = new CharacterMoverInput(normalized, false, false);
		}
		else
		{
			this.GetRandomAIPos();
		}
		return result;
	}

	// Token: 0x060011B5 RID: 4533 RVA: 0x0000E734 File Offset: 0x0000C934
	private void GetRandomAIPos()
	{
		this.targetPosition = new Vector3(ZPMath.RandomFloat(GameManager.rand, -8f, 8f), 0f, ZPMath.RandomFloat(GameManager.rand, -8f, 8f));
	}

	// Token: 0x060011B6 RID: 4534 RVA: 0x0000E76E File Offset: 0x0000C96E
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCKillPlayer(NetPlayer sender, int seed)
	{
		this.KillPlayer(seed, false);
	}

	// Token: 0x060011B7 RID: 4535 RVA: 0x00089E30 File Offset: 0x00088030
	public void KillPlayer(int seed, bool send_rpc = true)
	{
		if (!this.isDead)
		{
			Vector3 vector = ZPMath.RandomPointInUnitSphere(new System.Random(seed));
			vector.z = Mathf.Abs(vector.z);
			vector.y = 0f;
			vector.Normalize();
			vector.y = 30f;
			vector.x *= 60f;
			vector.z *= 60f;
			GameObject gameObject = this.playerAnim.SpawnRagdoll(vector * 0.15f);
			ParticleSystem.ShapeModule shape = this.flameParticles.shape;
			Transform transform = gameObject.transform.Find("Character/CharacterBase");
			shape.skinnedMeshRenderer = transform.GetComponent<SkinnedMeshRenderer>();
			gameObject.transform.Find("Character/Armature/mixamorig:Hips");
			this.flameParticles.enableEmission = true;
			this.BeingHit = false;
			this.isDead = true;
			this.Deactivate();
			this.cameraShake.AddShake(0.3f);
			this.minigameController.PlayerDied(this);
			if (base.IsOwner && send_rpc)
			{
				base.SendRPC("RPCKillPlayer", NetRPCDelivery.RELIABLE_UNORDERED, new object[]
				{
					seed
				});
			}
		}
	}

	// Token: 0x060011B8 RID: 4536 RVA: 0x0000E778 File Offset: 0x0000C978
	public override void Deactivate()
	{
		this.pointLight.enabled = false;
		base.Deactivate();
	}

	// Token: 0x060011B9 RID: 4537 RVA: 0x0000E78C File Offset: 0x0000C98C
	public override void Activate()
	{
		this.pointLight.enabled = true;
		this.pointLight.intensity = this.lightIntensity;
		base.Activate();
	}

	// Token: 0x060011BA RID: 4538 RVA: 0x0000E7B1 File Offset: 0x0000C9B1
	private void BeingHitRecieve(object val)
	{
		this.BeingHit = (bool)val;
	}

	// Token: 0x060011BB RID: 4539 RVA: 0x0000E7BF File Offset: 0x0000C9BF
	private void DamageRecieve(object val)
	{
		this.Health = (byte)val;
	}

	// Token: 0x060011BC RID: 4540 RVA: 0x0000E7CD File Offset: 0x0000C9CD
	public void OnDestroy()
	{
		if (this.source != null)
		{
			this.source.FadeAudio(0.5f, FadeType.Out);
		}
	}

	// Token: 0x0400125F RID: 4703
	public Light pointLight;

	// Token: 0x04001260 RID: 4704
	public float lightIntensity = 2f;

	// Token: 0x04001261 RID: 4705
	public ParticleSystem flameParticles;

	// Token: 0x04001262 RID: 4706
	public AudioClip sizzleClip;

	// Token: 0x04001263 RID: 4707
	public Color fireLightColor;

	// Token: 0x04001264 RID: 4708
	public float fireLightIntensity = 1f;

	// Token: 0x04001265 RID: 4709
	public float fireLightRange = 5f;

	// Token: 0x04001266 RID: 4710
	private SpeedySpotlightsController minigameController;

	// Token: 0x04001267 RID: 4711
	private CharacterMover mover;

	// Token: 0x04001268 RID: 4712
	private CameraShake cameraShake;

	// Token: 0x04001269 RID: 4713
	private TempAudioSource source;

	// Token: 0x0400126A RID: 4714
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVar<bool> beingHit = new NetVar<bool>(false);

	// Token: 0x0400126B RID: 4715
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVar<byte> health = new NetVar<byte>(0);

	// Token: 0x0400126C RID: 4716
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x0400126D RID: 4717
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x0400126E RID: 4718
	private ActionTimer runAwayTimer = new ActionTimer(1f);

	// Token: 0x0400126F RID: 4719
	private MinigameSpotlight closestLight;

	// Token: 0x04001270 RID: 4720
	private float[] difficultyChances = new float[]
	{
		0.5f,
		0.25f,
		-0.1f
	};
}
