using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;

// Token: 0x020001C9 RID: 457
public class LaserLeapPlayer : Movement1
{
	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06000D14 RID: 3348 RVA: 0x0000C034 File Offset: 0x0000A234
	public int Lives
	{
		get
		{
			return this.m_lives;
		}
	}

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06000D15 RID: 3349 RVA: 0x0000C03C File Offset: 0x0000A23C
	// (set) Token: 0x06000D16 RID: 3350 RVA: 0x0000C044 File Offset: 0x0000A244
	public bool IsInvulnerable { get; set; }

	// Token: 0x06000D17 RID: 3351 RVA: 0x0006C064 File Offset: 0x0006A264
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.minigameController = (LaserLeapController)GameManager.Minigame;
		this.mover = base.GetComponent<CharacterMover>();
		CharacterMover characterMover = this.mover;
		characterMover.OnJump = (CharacterMover.OnJumpDelegate)Delegate.Combine(characterMover.OnJump, new CharacterMover.OnJumpDelegate(this.OnJump));
		if (!NetSystem.IsServer)
		{
			this.score.Recieve = new RecieveProxy(base.ScoreRecieve);
		}
		if (!base.IsOwner)
		{
			base.GetComponent<CharacterController>().enabled = false;
		}
		if (!this.player.IsAI || !base.IsOwner)
		{
			base.GetComponent<NavMeshAgent>().enabled = false;
		}
	}

	// Token: 0x06000D18 RID: 3352 RVA: 0x0006C110 File Offset: 0x0006A310
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

	// Token: 0x06000D19 RID: 3353 RVA: 0x0000398C File Offset: 0x00001B8C
	public void OnDestroy()
	{
	}

	// Token: 0x06000D1A RID: 3354 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x06000D1B RID: 3355 RVA: 0x0006C160 File Offset: 0x0006A360
	protected override void Start()
	{
		base.Start();
		this.minigameController.AddPlayer(this);
		this.healthBar = GameManager.UIController.CreateHealthBar(base.transform, 1.5f, this.minigameController.MinigameCamera);
		this.healthBar.gameObject.SetActive(false);
	}

	// Token: 0x06000D1C RID: 3356 RVA: 0x0006C1B8 File Offset: 0x0006A3B8
	private void Update()
	{
		if (this.healthBar != null && !this.isDead && !this.healthBar.gameObject.activeInHierarchy && this.minigameController.State == MinigameControllerState.Playing)
		{
			this.healthBar.gameObject.SetActive(true);
		}
		if (this.IsInvulnerable && Time.time > this.m_invulnerabilityEndTime)
		{
			this.IsInvulnerable = false;
			this.SetFlashEnabled(false);
		}
		base.UpdateController();
		if (this.agent != null)
		{
			this.agent.updatePosition = false;
			this.agent.updateRotation = false;
		}
	}

	// Token: 0x06000D1D RID: 3357 RVA: 0x0006C25C File Offset: 0x0006A45C
	private void SetFlashEnabled(bool flash)
	{
		if (this.m_flashMaterial == null)
		{
			this.m_flashMaterial = new Material(this.m_LaserHitFlashMat);
		}
		if (this.renderers == null)
		{
			this.renderers = base.GetComponentsInChildren<MeshRenderer>();
		}
		if (this.skinnedRenderers == null)
		{
			this.skinnedRenderers = base.GetComponentsInChildren<SkinnedMeshRenderer>();
		}
		if (this.materials == null)
		{
			this.materials = new List<Material>();
			foreach (MeshRenderer meshRenderer in this.renderers)
			{
				meshRenderer.materials = new List<Material>(meshRenderer.materials)
				{
					this.m_flashMaterial
				}.ToArray();
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.skinnedRenderers)
			{
				skinnedMeshRenderer.materials = new List<Material>(skinnedMeshRenderer.materials)
				{
					this.m_flashMaterial
				}.ToArray();
			}
		}
		this.m_flashMaterial.SetColor("_Color", flash ? new Color(1f, 1f, 1f, 1f) : new Color(0f, 0f, 0f, 0f));
	}

	// Token: 0x06000D1E RID: 3358 RVA: 0x0000C04D File Offset: 0x0000A24D
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.SERVER)]
	public void RPCOwnerKillPlayer(NetPlayer sender)
	{
		this.KillPlayer(true);
	}

	// Token: 0x06000D1F RID: 3359 RVA: 0x0000C04D File Offset: 0x0000A24D
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCKillPlayer(NetPlayer sender)
	{
		this.KillPlayer(true);
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06000D20 RID: 3360 RVA: 0x0000C056 File Offset: 0x0000A256
	public int Placement
	{
		get
		{
			return this.m_placement;
		}
	}

	// Token: 0x06000D21 RID: 3361 RVA: 0x0006C388 File Offset: 0x0006A588
	public void KillPlayer(bool send_rpc = true)
	{
		if (!this.isDead)
		{
			Vector3 onUnitSphere = UnityEngine.Random.onUnitSphere;
			onUnitSphere.y = 100f;
			onUnitSphere.x *= 100f;
			onUnitSphere.z *= 40f;
			this.playerAnim.SpawnRagdoll(onUnitSphere * 0.05f);
			this.isDead = true;
			this.Deactivate();
			this.healthBar.gameObject.SetActive(false);
			AudioSystem.PlayOneShot("DeathSplash01", 0.5f, 0.1f);
			if (Settings.BloodEffects)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.m_playerDeathEffect, base.transform.position, Quaternion.LookRotation(Vector3.up));
			}
			if (NetSystem.IsServer && send_rpc)
			{
				base.SendRPC("RPCKillPlayer", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
			}
			this.m_placement = this.minigameController.DeadPlayerCount;
			LaserLeapController laserLeapController = this.minigameController;
			int deadPlayerCount = laserLeapController.DeadPlayerCount;
			laserLeapController.DeadPlayerCount = deadPlayerCount + 1;
		}
	}

	// Token: 0x06000D22 RID: 3362 RVA: 0x0006C484 File Offset: 0x0006A684
	public void OnLaserHit()
	{
		if (this.IsInvulnerable || !base.IsOwner || this.m_lives <= 0 || this.minigameController.State != MinigameControllerState.Playing)
		{
			return;
		}
		if (base.IsOwner)
		{
			base.SendRPC("RPCLaserHit", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		this.LaserHit();
	}

	// Token: 0x06000D23 RID: 3363 RVA: 0x0006C4D8 File Offset: 0x0006A6D8
	private void LaserHit()
	{
		AudioSystem.PlayOneShot("Burn01", 1f, 0f);
		this.IsInvulnerable = true;
		this.SetFlashEnabled(true);
		this.m_invulnerabilityEndTime = Time.time + 2f;
		this.m_lives--;
		this.healthBar.SetHealth((float)this.m_lives / 3f);
		if (this.m_lives <= 0 && base.IsOwner)
		{
			this.KillPlayer(true);
			base.SendRPC("RPCOwnerKillPlayer", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x06000D24 RID: 3364 RVA: 0x0000C05E File Offset: 0x0000A25E
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCLaserHit(NetPlayer sender)
	{
		this.LaserHit();
	}

	// Token: 0x06000D25 RID: 3365 RVA: 0x0006C568 File Offset: 0x0006A768
	protected override void DoMovement()
	{
		CharacterMoverInput input = default(CharacterMoverInput);
		bool val = !this.minigameController.Playable || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput || this.isDead;
		if (!base.GamePlayer.IsAI)
		{
			Vector2 axis = new Vector2(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
			input = new CharacterMoverInput(axis, this.player.RewiredPlayer.GetButtonDown(InputActions.Accept), false);
		}
		else
		{
			input = this.GetAIInput();
		}
		input.NullInput(val);
		this.mover.CalculateVelocity(input, Time.deltaTime);
		this.mover.DoMovement(Time.deltaTime);
		this.mover.SmoothSlope();
		if (this.mover.MovementAxis != Vector2.zero)
		{
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(new Vector3(this.mover.MovementAxis.x, 0f, this.mover.MovementAxis.y), Vector3.up), 1500f * Time.deltaTime);
		}
		if (this.agent != null)
		{
			this.agent.nextPosition = base.transform.position;
			this.agent.velocity = this.mover.Velocity;
		}
		this.velocity.Value = this.mover.Velocity;
		this.netIsGrounded.Value = (this.curOffMeshLinkTranslationType != OffMeshLinkTranslateType.Parabola && this.mover.Grounded);
		base.DoMovement();
	}

	// Token: 0x06000D26 RID: 3366 RVA: 0x0006C730 File Offset: 0x0006A930
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

	// Token: 0x06000D27 RID: 3367 RVA: 0x0006C7D8 File Offset: 0x0006A9D8
	private CharacterMoverInput GetAIInput()
	{
		CharacterMoverInput result = default(CharacterMoverInput);
		Vector2 vector = new Vector2(base.transform.position.x, base.transform.position.z);
		if (this.agent.isOnOffMeshLink)
		{
			if (this.curOffMeshLinkTranslationType == OffMeshLinkTranslateType.None)
			{
				this.curOffMeshLinkTranslationType = OffMeshLinkTranslateType.Parabola;
				this.OnJump();
				float initialHorizontalVelocity = 5f;
				base.StartCoroutine(base.GetParabolicPath(this.mover, this.mover.gravity, 1500f, initialHorizontalVelocity, true));
			}
		}
		else
		{
			float num = 0.36f;
			if (this.offsetUpdateTimer.Elapsed(true))
			{
				this.m_offset = UnityEngine.Random.onUnitSphere * 0.25f;
				this.m_offset.y = 0f;
			}
			if (this.targetUpdateTimer.Elapsed(true))
			{
				this.targetPosition = this.minigameController.GetRandomNavMeshPoint();
			}
			if (this.pathUpdateTimer.Elapsed(false) && !this.agent.pathPending && this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
			{
				this.agent.SetDestination(this.targetPosition + this.m_offset);
				this.pathUpdateTimer.Start();
			}
			Vector2 a = new Vector2(this.targetPosition.x, this.targetPosition.z);
			Vector2 b = new Vector2(this.agent.pathEndPosition.x, this.agent.pathEndPosition.z);
			if (((a - vector).sqrMagnitude > num && Vector2.Distance(vector, b) > 0.5f) || this.m_aiShouldJump)
			{
				Vector3 vector2 = this.agent.steeringTarget - base.transform.position;
				Vector2 normalized = new Vector2(vector2.x, vector2.z).normalized;
				result = new CharacterMoverInput(normalized, this.m_aiShouldJump, false);
				this.m_aiShouldJump = false;
			}
			else
			{
				result.NullInput();
				this.targetPosition = this.minigameController.GetRandomNavMeshPoint();
				this.targetUpdateTimer.Start();
				this.agent.SetDestination(this.targetPosition + this.m_offset);
				this.pathUpdateTimer.Start();
			}
		}
		return result;
	}

	// Token: 0x06000D28 RID: 3368 RVA: 0x0000C066 File Offset: 0x0000A266
	private void SetCurAIState(LaserLeapAIState newState)
	{
		if (!base.GamePlayer.IsLocalPlayer)
		{
			return;
		}
		this.curAIState = newState;
	}

	// Token: 0x06000D29 RID: 3369 RVA: 0x0006CA2C File Offset: 0x0006AC2C
	public override void ResetPlayer()
	{
		this.playerAnim.SetPlayerRotationImmediate(this.startRotation.eulerAngles.y);
		if (base.IsOwner)
		{
			this.mover.Velocity = Vector3.zero;
			this.velocity.Value = Vector3.zero;
		}
		base.ResetPlayer();
	}

	// Token: 0x06000D2A RID: 3370 RVA: 0x0000C07D File Offset: 0x0000A27D
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x06000D2B RID: 3371 RVA: 0x0000480A File Offset: 0x00002A0A
	protected void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x06000D2C RID: 3372 RVA: 0x0006CA84 File Offset: 0x0006AC84
	public void OnTriggerEnter(Collider other)
	{
		if (!this.m_colliders.Contains(other) && other.gameObject.CompareTag("MinigameCustom01"))
		{
			this.m_colliders.Add(other);
			if (UnityEngine.Random.value < 0.9f)
			{
				this.m_aiShouldJump = true;
			}
		}
	}

	// Token: 0x06000D2D RID: 3373 RVA: 0x0000C085 File Offset: 0x0000A285
	public void OnTriggerExit(Collider other)
	{
		if (this.m_colliders.Contains(other))
		{
			this.m_colliders.Remove(other);
		}
	}

	// Token: 0x04000C8B RID: 3211
	public float base_speed = 6f;

	// Token: 0x04000C8C RID: 3212
	[SerializeField]
	private SkinnedMeshRenderer m_renderer;

	// Token: 0x04000C8D RID: 3213
	[SerializeField]
	private Material m_LaserHitFlashMat;

	// Token: 0x04000C8E RID: 3214
	[SerializeField]
	protected GameObject m_playerDeathEffect;

	// Token: 0x04000C8F RID: 3215
	private LaserLeapController minigameController;

	// Token: 0x04000C90 RID: 3216
	private CharacterMover mover;

	// Token: 0x04000C91 RID: 3217
	private CameraShake cameraShake;

	// Token: 0x04000C92 RID: 3218
	private LaserLeapAIState curAIState;

	// Token: 0x04000C93 RID: 3219
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x04000C94 RID: 3220
	private ActionTimer offsetUpdateTimer = new ActionTimer(1f, 1f);

	// Token: 0x04000C95 RID: 3221
	private ActionTimer targetUpdateTimer = new ActionTimer(2f, 4f);

	// Token: 0x04000C96 RID: 3222
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x04000C97 RID: 3223
	private float m_invulnerabilityEndTime;

	// Token: 0x04000C98 RID: 3224
	private int m_lives = 3;

	// Token: 0x04000C9A RID: 3226
	private MeshRenderer[] renderers;

	// Token: 0x04000C9B RID: 3227
	private SkinnedMeshRenderer[] skinnedRenderers;

	// Token: 0x04000C9C RID: 3228
	private List<Material> materials;

	// Token: 0x04000C9D RID: 3229
	private Material m_flashMaterial;

	// Token: 0x04000C9E RID: 3230
	private UIMinigameHealthBar healthBar;

	// Token: 0x04000C9F RID: 3231
	private float m_damageFlashTime;

	// Token: 0x04000CA0 RID: 3232
	private bool m_damageFlashHidden;

	// Token: 0x04000CA1 RID: 3233
	private int m_placement = 10;

	// Token: 0x04000CA2 RID: 3234
	private Vector3 m_offset = Vector3.zero;

	// Token: 0x04000CA3 RID: 3235
	private HashSet<Collider> m_colliders = new HashSet<Collider>();

	// Token: 0x04000CA4 RID: 3236
	private bool m_aiShouldJump;
}
