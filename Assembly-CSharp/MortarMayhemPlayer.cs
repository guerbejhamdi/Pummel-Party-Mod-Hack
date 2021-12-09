using System;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x020001DC RID: 476
public class MortarMayhemPlayer : Movement1
{
	// Token: 0x06000DC6 RID: 3526 RVA: 0x0006F80C File Offset: 0x0006DA0C
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.minigameController = (MortarMayhemController)GameManager.Minigame;
		this.m_boxController = this.minigameController.Root.GetComponentInChildren<BoxController>();
		this.mover = base.GetComponent<CharacterMover>();
		if (base.IsOwner)
		{
			this.net_z_rotation.Value = ZPMath.CompressFloat(0f, -45f, 45f);
		}
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
		this.targetPosition = UnityEngine.Random.onUnitSphere * 10f;
		this.targetPosition.y = 0f;
	}

	// Token: 0x06000DC7 RID: 3527 RVA: 0x0006F8F0 File Offset: 0x0006DAF0
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

	// Token: 0x06000DC8 RID: 3528 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x06000DC9 RID: 3529 RVA: 0x0000C644 File Offset: 0x0000A844
	protected override void Start()
	{
		base.Start();
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
	}

	// Token: 0x06000DCA RID: 3530 RVA: 0x0006F940 File Offset: 0x0006DB40
	private void Update()
	{
		if (base.IsOwner && !base.IsDead && base.transform.position.y < -6f)
		{
			this.KillPlayer(true);
		}
		base.UpdateController();
		if (this.agent != null)
		{
			this.agent.updatePosition = false;
			this.agent.updateRotation = false;
		}
	}

	// Token: 0x06000DCB RID: 3531 RVA: 0x0000C66E File Offset: 0x0000A86E
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCKillPlayer(NetPlayer sender)
	{
		this.KillPlayer(true);
	}

	// Token: 0x06000DCC RID: 3532 RVA: 0x0006F9A8 File Offset: 0x0006DBA8
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
			AudioSystem.PlayOneShot("DeathSplash01", 0.5f, 0.1f);
			if (Settings.BloodEffects)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.m_playerDeathEffect, base.transform.position, Quaternion.LookRotation(Vector3.up));
			}
			this.minigameController.PlayerDied(this);
			if (base.IsOwner)
			{
				base.SendRPC("RPCKillPlayer", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
			}
		}
	}

	// Token: 0x06000DCD RID: 3533 RVA: 0x0006FA78 File Offset: 0x0006DC78
	protected override void DoMovement()
	{
		CharacterMoverInput input = default(CharacterMoverInput);
		bool val = !this.minigameController.Playable || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput || this.isDead || this.minigameController.mortarMayhemState != MortarMayhemController.MortarMayhemState.DoingPattern;
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

	// Token: 0x06000DCE RID: 3534 RVA: 0x0006FC54 File Offset: 0x0006DE54
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

	// Token: 0x06000DCF RID: 3535 RVA: 0x0006FCFC File Offset: 0x0006DEFC
	private CharacterMoverInput GetAIInput()
	{
		CharacterMoverInput result = default(CharacterMoverInput);
		Vector2 vector = new Vector2(base.transform.position.x, base.transform.position.z);
		if (this.agent.isOnOffMeshLink)
		{
			if (this.curOffMeshLinkTranslationType == OffMeshLinkTranslateType.None)
			{
				this.curOffMeshLinkTranslationType = OffMeshLinkTranslateType.Parabola;
				float initialHorizontalVelocity = 5f;
				base.StartCoroutine(base.GetParabolicPath(this.mover, this.mover.gravity, 1500f, initialHorizontalVelocity, true));
			}
		}
		else
		{
			if (this.curX != this.minigameController.curX[(int)base.OwnerSlot] || this.curY != this.minigameController.curY[(int)base.OwnerSlot])
			{
				this.curX = this.minigameController.curX[(int)base.OwnerSlot];
				this.curY = this.minigameController.curY[(int)base.OwnerSlot];
				if (UnityEngine.Random.value > this.failChances[(int)base.GamePlayer.Difficulty])
				{
					this.targetPosition = this.minigameController.GetGridPos((int)base.OwnerSlot, (int)this.curX, (int)this.curY);
				}
				else
				{
					int num = UnityEngine.Random.Range(-1, 2);
					int num2 = UnityEngine.Random.Range(-1, 2);
					this.targetPosition = this.minigameController.GetGridPos((int)base.OwnerSlot, (int)this.curX + num, (int)this.curY + num2);
				}
			}
			float num3 = 0.36f;
			if (this.pathUpdateTimer.Elapsed(true) && this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
			{
				this.agent.SetDestination(this.targetPosition);
			}
			Debug.DrawLine(base.transform.position, this.targetPosition, Color.green);
			Debug.DrawLine(base.transform.position, this.agent.pathEndPosition, Color.red);
			Vector2 a = new Vector2(this.targetPosition.x, this.targetPosition.z);
			Vector2 b = new Vector2(this.agent.pathEndPosition.x, this.agent.pathEndPosition.z);
			if ((a - vector).sqrMagnitude > num3 && Vector2.Distance(vector, b) > 0.5f)
			{
				Vector3 vector2 = this.agent.steeringTarget - base.transform.position;
				Vector2 normalized = new Vector2(vector2.x, vector2.z).normalized;
				result = new CharacterMoverInput(normalized, false, false);
			}
			else
			{
				result.NullInput();
			}
		}
		return result;
	}

	// Token: 0x06000DD0 RID: 3536 RVA: 0x0000C677 File Offset: 0x0000A877
	private void SetCurAIState(IcebergAIState newState)
	{
		if (!base.GamePlayer.IsLocalPlayer)
		{
			return;
		}
		this.curAIState = newState;
	}

	// Token: 0x04000D2F RID: 3375
	[SerializeField]
	protected GameObject m_playerDeathEffect;

	// Token: 0x04000D30 RID: 3376
	public float base_speed = 6f;

	// Token: 0x04000D31 RID: 3377
	private MortarMayhemController minigameController;

	// Token: 0x04000D32 RID: 3378
	private CharacterMover mover;

	// Token: 0x04000D33 RID: 3379
	private CameraShake cameraShake;

	// Token: 0x04000D34 RID: 3380
	private IcebergAIState curAIState;

	// Token: 0x04000D35 RID: 3381
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x04000D36 RID: 3382
	private ActionTimer hidePositionTimer = new ActionTimer(0.25f, 0.55f);

	// Token: 0x04000D37 RID: 3383
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x04000D38 RID: 3384
	private BoxController m_boxController;

	// Token: 0x04000D39 RID: 3385
	public byte curX;

	// Token: 0x04000D3A RID: 3386
	public byte curY;

	// Token: 0x04000D3B RID: 3387
	private float[] failChances = new float[]
	{
		0.15f,
		0.1f,
		0.05f
	};
}
