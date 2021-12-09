using System;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000167 RID: 359
public class BoundingBlocksPlayer : Movement1
{
	// Token: 0x06000A50 RID: 2640 RVA: 0x0005B2C4 File Offset: 0x000594C4
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.mover = base.GetComponent<CharacterMover>();
		if (base.IsOwner)
		{
			this.net_z_rotation.Value = ZPMath.CompressFloat(0f, -45f, 45f);
			this.GetRandomAIPosition();
		}
		if (!base.IsOwner && !NetSystem.IsServer)
		{
			base.GetComponent<CharacterController>().enabled = false;
		}
		this.pointlight.color = base.GamePlayer.Color.skinColor1;
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x0000AB84 File Offset: 0x00008D84
	public override void OnOwnerChanged()
	{
		if (NetSystem.IsServer)
		{
			base.GetComponent<CharacterController>().enabled = true;
			this.mover.SetForwardVector(Vector3.forward);
		}
		base.OnOwnerChanged();
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x0005B348 File Offset: 0x00059548
	protected override void Start()
	{
		base.Start();
		this.minigameController = (BoundingBlocksController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.cameraShake = this.minigameController.Root.GetComponentInChildren<CameraShake>();
		if (!this.player.IsAI)
		{
			this.mover.SetForwardVector(Vector3.forward);
		}
	}

	// Token: 0x06000A54 RID: 2644 RVA: 0x0000ABAF File Offset: 0x00008DAF
	private void Update()
	{
		if (!this.isDead)
		{
			base.UpdateController();
		}
	}

	// Token: 0x06000A55 RID: 2645 RVA: 0x0005B3C0 File Offset: 0x000595C0
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

	// Token: 0x06000A56 RID: 2646 RVA: 0x0005B558 File Offset: 0x00059758
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

	// Token: 0x06000A57 RID: 2647 RVA: 0x0005B600 File Offset: 0x00059800
	private CharacterMoverInput GetAIInput()
	{
		CharacterMoverInput result = default(CharacterMoverInput);
		float num = 0.36f;
		Vector2 b = new Vector2(base.transform.position.x, base.transform.position.z);
		Vector2 a = new Vector2(this.targetPosition.x, this.targetPosition.z);
		bool flag = true;
		if (this.movementCheckTimer.Elapsed(true))
		{
			if (Vector3.Distance(this.lastPosition, base.transform.position) < 0.5f)
			{
				flag = false;
			}
			this.lastPosition = base.transform.position;
		}
		if ((a - b).sqrMagnitude > num && flag)
		{
			Vector3 vector = this.targetPosition - base.transform.position;
			Vector2 normalized = new Vector2(vector.x, vector.z).normalized;
			result = new CharacterMoverInput(normalized, false, false);
		}
		else
		{
			this.GetRandomAIPosition();
		}
		return result;
	}

	// Token: 0x06000A58 RID: 2648 RVA: 0x0005B700 File Offset: 0x00059900
	private void GetRandomAIPosition()
	{
		if (this.minigameController == null)
		{
			return;
		}
		this.targetPosition = new Vector3(ZPMath.RandomFloat(GameManager.rand, -(this.minigameController.gridSize / 2f) * this.minigameController.gridDimension, this.minigameController.gridSize / 2f * this.minigameController.gridDimension), 0f, ZPMath.RandomFloat(GameManager.rand, -(this.minigameController.gridSize / 2f) * this.minigameController.gridDimension, this.minigameController.gridSize / 2f * this.minigameController.gridDimension));
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x0005B7B8 File Offset: 0x000599B8
	public void OnTriggerEnter(Collider other)
	{
		if (NetSystem.IsServer && this.minigameController != null && this.minigameController.Playable)
		{
			BoundingBlocksTile componentInParent = other.gameObject.GetComponentInParent<BoundingBlocksTile>();
			if (componentInParent != null)
			{
				this.minigameController.ClaimTile(componentInParent.id, (short)base.OwnerSlot);
			}
		}
	}

	// Token: 0x04000929 RID: 2345
	public CharacterController characterController;

	// Token: 0x0400092A RID: 2346
	public Light pointlight;

	// Token: 0x0400092B RID: 2347
	private BoundingBlocksController minigameController;

	// Token: 0x0400092C RID: 2348
	private CharacterMover mover;

	// Token: 0x0400092D RID: 2349
	private CameraShake cameraShake;

	// Token: 0x0400092E RID: 2350
	private ActionTimer movementCheckTimer = new ActionTimer(0.4f, 0.5f);

	// Token: 0x0400092F RID: 2351
	private Vector3 lastPosition = Vector3.zero;

	// Token: 0x04000930 RID: 2352
	private Vector3 targetPosition = Vector3.zero;
}
