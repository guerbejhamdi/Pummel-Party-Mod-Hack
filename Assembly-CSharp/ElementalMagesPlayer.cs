using System;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x020001A5 RID: 421
public class ElementalMagesPlayer : Movement1
{
	// Token: 0x06000C0E RID: 3086 RVA: 0x000652D8 File Offset: 0x000634D8
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.mover = base.GetComponent<CharacterMover>();
		if (base.IsOwner)
		{
			this.net_z_rotation.Value = ZPMath.CompressFloat(0f, -45f, 45f);
			this.GetRandomAIPosition();
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
	}

	// Token: 0x06000C0F RID: 3087 RVA: 0x0006537C File Offset: 0x0006357C
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

	// Token: 0x06000C10 RID: 3088 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x06000C11 RID: 3089 RVA: 0x000653CC File Offset: 0x000635CC
	protected override void Start()
	{
		base.Start();
		this.minigameController = (ElementalMagesController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.cameraShake = this.minigameController.Root.GetComponentInChildren<CameraShake>();
		if (!this.player.IsAI)
		{
			this.mover.SetForwardVector(Vector3.forward);
			if (base.GamePlayer.Difficulty == BotDifficulty.Normal)
			{
				this.getCrystalDelayTimer = new ActionTimer(1.6f, 2.2f);
				return;
			}
			this.getCrystalDelayTimer = new ActionTimer(0.4f, 0.9f);
		}
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x0000ABAF File Offset: 0x00008DAF
	private void Update()
	{
		if (!this.isDead)
		{
			base.UpdateController();
		}
	}

	// Token: 0x06000C13 RID: 3091 RVA: 0x00065480 File Offset: 0x00063680
	public void FixedUpdate()
	{
		if (!this.isDead && NetSystem.IsServer && this.minigameController.Playable)
		{
			this.flip = !this.flip;
			if (this.flip)
			{
				this.minigameController.DoSplat(base.transform.position, 0f, 0, (byte)base.OwnerSlot);
			}
		}
	}

	// Token: 0x06000C14 RID: 3092 RVA: 0x000654E4 File Offset: 0x000636E4
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

	// Token: 0x06000C15 RID: 3093 RVA: 0x0006567C File Offset: 0x0006387C
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

	// Token: 0x06000C16 RID: 3094 RVA: 0x00065724 File Offset: 0x00063924
	private CharacterMoverInput GetAIInput()
	{
		CharacterMoverInput result = default(CharacterMoverInput);
		float num = 0.36f;
		Vector2 b = new Vector2(base.transform.position.x, base.transform.position.z);
		if (this.pathUpdateTimer.Elapsed(true) && this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
		{
			this.agent.SetDestination(this.targetPosition);
		}
		Vector2 a = new Vector2(this.targetPosition.x, this.targetPosition.z);
		if (base.GamePlayer.Difficulty == BotDifficulty.Hard || base.GamePlayer.Difficulty == BotDifficulty.Normal)
		{
			if (this.minigameController.crystals.Count > 0)
			{
				if (this.targetCrystal == null)
				{
					this.targetCrystal = this.minigameController.crystals[0];
					this.getCrystalDelayTimer.Start();
				}
				if (this.getCrystalDelayTimer.Elapsed(true))
				{
					this.targetPosition = this.targetCrystal.gameObject.transform.position;
				}
			}
			else
			{
				this.targetCrystal = null;
			}
		}
		if ((a - b).sqrMagnitude > num)
		{
			Vector3 vector = this.agent.steeringTarget - base.transform.position;
			Vector2 normalized = new Vector2(vector.x, vector.z).normalized;
			result = new CharacterMoverInput(normalized, false, false);
		}
		else
		{
			this.GetRandomAIPosition();
		}
		return result;
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x0000B921 File Offset: 0x00009B21
	private void GetRandomAIPosition()
	{
		this.targetPosition = new Vector3(ZPMath.RandomFloat(GameManager.rand, -9.5f, 9.5f), 0f, ZPMath.RandomFloat(GameManager.rand, -9.5f, 9.5f));
	}

	// Token: 0x04000B44 RID: 2884
	private ElementalMagesController minigameController;

	// Token: 0x04000B45 RID: 2885
	private CharacterMover mover;

	// Token: 0x04000B46 RID: 2886
	private CameraShake cameraShake;

	// Token: 0x04000B47 RID: 2887
	private bool flip;

	// Token: 0x04000B48 RID: 2888
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x04000B49 RID: 2889
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x04000B4A RID: 2890
	private ActionTimer getCrystalDelayTimer = new ActionTimer(0.25f, 0.4f);

	// Token: 0x04000B4B RID: 2891
	private ElementalMagesController.Crystal targetCrystal;
}
