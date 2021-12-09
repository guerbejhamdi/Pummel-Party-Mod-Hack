using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200027F RID: 639
public class TreasureHuntPlayer : Movement1
{
	// Token: 0x060012B5 RID: 4789 RVA: 0x00090010 File Offset: 0x0008E210
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.mover = base.GetComponent<CharacterMover>();
		this.minigameController = (TreasureHuntController)GameManager.Minigame;
		if (base.IsOwner)
		{
			CharacterMover characterMover = this.mover;
			characterMover.OnJump = (CharacterMover.OnJumpDelegate)Delegate.Combine(characterMover.OnJump, new CharacterMover.OnJumpDelegate(this.OnJump));
			List<GamePlayer> localNonAIPlayers = GameManager.GetLocalNonAIPlayers();
			if (!this.player.IsAI || localNonAIPlayers.Count == 0)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.camera_prefab, Vector3.zero, Quaternion.identity);
				gameObject.transform.parent = base.transform;
				this.cam = base.gameObject.GetComponentInChildren<Camera>();
				this.playerCam = base.gameObject.GetComponent<ThirdPersonCamera>();
				this.playerCam.SetTargetCamera(this.cam);
				this.playerCam.YRotation = base.transform.rotation.eulerAngles.y;
				this.playerCam.ZRotation = 5f;
				this.playerCam.RotateCamera();
				this.playerCam.UpdateCamera();
				if (localNonAIPlayers.Count > 1)
				{
					this.cam.rect = base.GetPlayerSplitScreenRect(this.player);
				}
				else if (localNonAIPlayers.Count == 0)
				{
					this.cam.rect = base.GetPlayerSplitScreenRectWithAI(this.player);
				}
				if (localNonAIPlayers.Count > 0 && localNonAIPlayers[0] == this.player)
				{
					gameObject.GetComponent<AudioListener>().enabled = true;
					base.gameObject.GetComponent<ParticleSystem>().emission.enabled = true;
				}
			}
		}
		this.vacuumSound = base.GetComponent<VacuumSound>();
		this.targeter = base.transform.Find("Targeter");
		this.vacuumParticle = base.transform.Find("Targeter/SandParticle").GetComponent<ParticleSystem>();
		this.staticVacuumParticle = base.transform.Find("Targeter/StaticSandParticles").GetComponent<ParticleSystem>();
		this.playerAnim.Setup();
		this.suctionPoint = this.playerAnim.GetBone(PlayerBone.RightHand).Find("VacuumTemp/SuctionPoint");
		if (!base.IsOwner)
		{
			base.GetComponent<ThirdPersonCamera>().enabled = false;
			base.GetComponent<CharacterController>().enabled = false;
		}
		if (this.player.IsAI)
		{
			base.GetComponent<ThirdPersonCamera>().enabled = false;
		}
	}

	// Token: 0x060012B6 RID: 4790 RVA: 0x0000AEDF File Offset: 0x000090DF
	public override void OnOwnerChanged()
	{
		if (NetSystem.IsServer)
		{
			base.GetComponent<CharacterController>().enabled = true;
		}
		base.OnOwnerChanged();
	}

	// Token: 0x060012B7 RID: 4791 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x060012B8 RID: 4792 RVA: 0x0009026C File Offset: 0x0008E46C
	protected override void Start()
	{
		base.Start();
		this.temp_particles = new ParticleSystem.Particle[this.vacuumParticle.main.maxParticles];
		this.minigameController.AddPlayer(this);
		BotDifficulty difficulty = base.GamePlayer.Difficulty;
		if (difficulty == BotDifficulty.Easy)
		{
			this.checkCollectiblesTimer.SetInterval(3.5f, 6f, true);
			return;
		}
		if (difficulty != BotDifficulty.Normal)
		{
			return;
		}
		this.checkCollectiblesTimer.SetInterval(2.5f, 4f, true);
	}

	// Token: 0x060012B9 RID: 4793 RVA: 0x0000F06B File Offset: 0x0000D26B
	public override void FinishedSpawning()
	{
		this.playerAnim.PistolGripRight = true;
		this.playerAnim.ShotgunStrength = 0f;
	}

	// Token: 0x060012BA RID: 4794 RVA: 0x000902EC File Offset: 0x0008E4EC
	public override void Activate()
	{
		base.Activate();
		if (this.vacuumPipe == null)
		{
			this.playerAnim.PistolGripRight = true;
			this.playerAnim.ShotgunStrength = 0f;
			this.vacuumPipe = this.minigameController.Spawn(this.vacuum_pipe_pfb, this.pipe_start.position, Quaternion.identity).GetComponent<VacuumPipe>();
			this.vacuumPipe.SetTorsoTransform(this.playerAnim.GetBone(PlayerBone.Spine1));
			this.vacuumPipe.SetStartTransform(this.pipe_start);
			this.vacuumPipe.SetEndTransform(this.pipe_end);
			if (this.vacuumRenderers != null)
			{
				Material material = new Material(this.baseVacuumMaterial);
				material.SetColor("_EmissionColor", base.GamePlayer.Color.skinColor1 * 2f);
				foreach (MeshRenderer meshRenderer in this.vacuumRenderers)
				{
					Material[] sharedMaterials = meshRenderer.sharedMaterials;
					for (int j = 0; j < meshRenderer.sharedMaterials.Length; j++)
					{
						if (meshRenderer.sharedMaterials[j] == this.baseVacuumMaterial)
						{
							sharedMaterials[j] = material;
						}
					}
					meshRenderer.sharedMaterials = sharedMaterials;
				}
			}
		}
	}

	// Token: 0x060012BB RID: 4795 RVA: 0x0009042C File Offset: 0x0008E62C
	private void Update()
	{
		if (!this.isDead)
		{
			base.UpdateController();
		}
		float num = base.IsOwner ? (this.player.IsAI ? this.aiZRot : this.playerCam.ZRotation) : ZPMath.DecompressFloat(this.net_z_rotation.Value, -45f, 45f);
		float y = base.IsOwner ? (this.player.IsAI ? this.playerAnim.PlayerRotation : this.playerCam.YRotation) : this.rotation.Value;
		Vector3 a = Quaternion.Euler(num + 20f, y, 0f) * Vector3.forward * 2f + base.transform.up;
		this.wallDistance = this.maxDigDistance;
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, a.normalized, out raycastHit, this.wallDistance, 1024))
		{
			this.wallDistance = raycastHit.distance;
			this.targeter.position = raycastHit.point + raycastHit.normal * 0.5f;
			this.targeter.LookAt(raycastHit.point + raycastHit.normal);
		}
		else
		{
			Vector3 vector = base.transform.position + this.wallDistance * a.normalized;
			RaycastHit raycastHit2;
			if (Physics.Raycast(vector, -Vector3.up, out raycastHit2, 2.5f, 1024))
			{
				this.targeter.position = raycastHit2.point + raycastHit2.normal * 0.5f;
				this.targeter.LookAt(raycastHit2.point + raycastHit2.normal);
			}
			RaycastHit raycastHit3;
			if (Physics.Raycast(vector, Vector3.up, out raycastHit3, 2.5f, 1024) && raycastHit2.collider != null && raycastHit3.distance < raycastHit2.distance)
			{
				this.targeter.position = raycastHit3.point + raycastHit3.normal * 0.5f;
				this.targeter.LookAt(raycastHit3.point + raycastHit3.normal);
			}
			if (raycastHit2.collider == null && raycastHit3.collider == null)
			{
				this.targeter.position = vector - Vector3.up * 2.5f;
				this.targeter.LookAt(vector);
			}
		}
		if (base.IsOwner)
		{
			this.vacuuming.Value = (((this.player.IsAI && (this.aiState == TreasureHuntPlayer.TreasureHuntAIState.Digging || this.aiState == TreasureHuntPlayer.TreasureHuntAIState.UncoveringCollectable || this.aiState == TreasureHuntPlayer.TreasureHuntAIState.UncoveringTreasure)) || (!this.player.IsAI && this.player.RewiredPlayer.GetButton(InputActions.UseItemShoot))) && !GameManager.IsGamePaused);
		}
		this.vacuumSound.vacuum_active = this.vacuuming.Value;
		this.vacuumParticle.GetParticles(this.temp_particles);
		this.particles.Clear();
		this.particles.AddRange(this.temp_particles);
		this.vacuumParticle.emission.enabled = (this.staticVacuumParticle.emission.enabled = this.vacuuming.Value);
		if (this.vacuuming.Value)
		{
			int i = 0;
			while (i < this.particles.Count)
			{
				if ((this.particles[i].position - this.suctionPoint.position).magnitude < 0.5f)
				{
					this.particles.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
			for (int j = 0; j < this.particles.Count; j++)
			{
				ParticleSystem.Particle value = this.particles[j];
				Vector3 a2 = this.suctionPoint.position - value.position;
				Vector3 a3 = ZPMath.ClosestPointOnLine(base.transform.position + a * 100f, this.suctionPoint.position, value.position) - value.position;
				Vector3 b = (a2 + a3 * 0.5f).normalized * this.vacuumParticleMaxSpeed * Time.deltaTime;
				if (b.sqrMagnitude > a2.sqrMagnitude)
				{
					value.position = this.suctionPoint.position;
					value.velocity = Vector3.zero;
				}
				else
				{
					value.position += b;
				}
				this.particles[j] = value;
			}
		}
		if (!this.vacuuming.Value && this.vacuumLast)
		{
			for (int k = 0; k < this.particles.Count; k++)
			{
				ParticleSystem.Particle value2 = this.particles[k];
				value2.velocity *= 0.2f;
				this.particles[k] = value2;
			}
		}
		this.vacuumLast = this.vacuuming.Value;
		this.vacuumParticle.SetParticles(this.particles.ToArray(), this.particles.Count);
		if (base.IsOwner && !GameManager.IsGamePaused && this.vacuuming.Value && Time.time - this.lastDigTime > this.curDigInterval)
		{
			this.curDigInterval = ZPMath.RandomFloat(GameManager.rand, this.minDigInterval, this.maxDigInterval);
			this.minigameController.grid.Vacuum(this.targeter.position, false);
			this.lastDigTime = Time.time;
		}
	}

	// Token: 0x060012BC RID: 4796 RVA: 0x00090A48 File Offset: 0x0008EC48
	protected override void DoMovement()
	{
		CharacterMoverInput input = default(CharacterMoverInput);
		if (!this.player.IsAI)
		{
			Vector3 lookPos = this.playerCam.GetLookPos();
			this.mover.SetForwardVector(new Vector3(lookPos.x, 0f, lookPos.z).normalized);
			input = new CharacterMoverInput(new Vector2(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), this.player.RewiredPlayer.GetAxis(InputActions.Vertical)), this.player.RewiredPlayer.GetButtonDown(InputActions.Accept), false);
		}
		else
		{
			input = this.GetAIInput();
		}
		input.NullInput(!this.minigameController.Playable || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput || (this.player.IsAI && base.AIActive));
		this.mover.CalculateVelocity(input, Time.deltaTime);
		this.mover.DoMovement(Time.deltaTime);
		this.mover.SmoothSlope();
		if (!this.player.IsAI && this.minigameController.Playable && !GameManager.IsGamePaused && GameManager.PollInput)
		{
			this.playerCam.RotateCamera();
			this.playerCam.UpdateCamera();
		}
		else if (this.player.IsAI)
		{
			Vector3 vector = this.targetPoint - base.transform.position;
			vector.y = 0f;
			Vector2 vector2 = new Vector2(0f, base.transform.position.y);
			Vector2 vector3 = new Vector2(vector.magnitude, this.targetPoint.y);
			float num = Mathf.Atan2(vector3.y - vector2.y, vector3.x - vector2.x) * 57.29578f;
			num = Mathf.Clamp(-num, -45f, 45f);
			this.aiZRot = Mathf.MoveTowards(this.aiZRot, num, Time.deltaTime * 360f);
		}
		this.velocity.Value = this.mover.Velocity;
		this.netIsGrounded.Value = this.controller.isGrounded;
		this.net_movement_axis.Value = this.playerAnim.GetMovementAxis();
		this.net_z_rotation.Value = ZPMath.CompressFloat(this.player.IsAI ? this.aiZRot : this.playerCam.ZRotation, -45f, 45f);
		base.DoMovement();
	}

	// Token: 0x060012BD RID: 4797 RVA: 0x00090CF8 File Offset: 0x0008EEF8
	private CharacterMoverInput GetAIInput()
	{
		CharacterMoverInput result = default(CharacterMoverInput);
		new Vector2(base.transform.position.x, base.transform.position.z);
		this.mover.SetForwardVector(base.transform.forward);
		Vector3 vector = this.targetPoint - base.transform.position;
		vector.y = 0f;
		float sqrMagnitude = vector.sqrMagnitude;
		vector.Normalize();
		if (this.minigameController.treasure == null && (this.aiState == TreasureHuntPlayer.TreasureHuntAIState.GettingTreasure || this.aiState == TreasureHuntPlayer.TreasureHuntAIState.UncoveringTreasure))
		{
			this.targetCollectable = null;
			this.aiState = TreasureHuntPlayer.TreasureHuntAIState.Digging;
		}
		float num = (this.targetCollectable == null) ? float.MaxValue : (this.targetCollectable.transform.position - base.transform.position).sqrMagnitude;
		if (this.aiState != TreasureHuntPlayer.TreasureHuntAIState.MovingToCenter && this.aiState != TreasureHuntPlayer.TreasureHuntAIState.GettingTreasure && this.aiState != TreasureHuntPlayer.TreasureHuntAIState.UncoveringTreasure)
		{
			if (num >= 16f)
			{
				this.targetCollectable = null;
			}
			if (this.targetCollectable == null)
			{
				this.aiState = TreasureHuntPlayer.TreasureHuntAIState.Digging;
			}
		}
		if (this.minigameController.treasure != null && this.minigameController.treasure.interactable && (this.minigameController.treasure.Outline || !this.minigameController.treasure.Stuck))
		{
			if (!this.treasureFirstTime)
			{
				this.treasureFirstTime = true;
				switch (base.GamePlayer.Difficulty)
				{
				case BotDifficulty.Easy:
					this.TreasureGetWaitTimer.SetInterval(7f, 10f, true);
					break;
				case BotDifficulty.Normal:
					this.TreasureGetWaitTimer.SetInterval(3f, 5f, true);
					break;
				case BotDifficulty.Hard:
					this.TreasureGetWaitTimer.SetInterval(1f, 2f, true);
					break;
				}
			}
			else if (this.TreasureGetWaitTimer.Elapsed(false))
			{
				this.targetCollectable = null;
				this.aiState = TreasureHuntPlayer.TreasureHuntAIState.GettingTreasure;
				Vector3 vector2 = this.minigameController.treasure.transform.position - base.transform.position;
				float magnitude = vector2.magnitude;
				if (this.minigameController.treasure.Stuck || Physics.Raycast(base.transform.position, vector2.normalized, magnitude, 1024))
				{
					this.aiState = TreasureHuntPlayer.TreasureHuntAIState.UncoveringTreasure;
				}
				else
				{
					this.aiState = TreasureHuntPlayer.TreasureHuntAIState.GettingTreasure;
				}
			}
		}
		else if (this.aiState == TreasureHuntPlayer.TreasureHuntAIState.UncoveringTreasure || this.aiState == TreasureHuntPlayer.TreasureHuntAIState.GettingTreasure)
		{
			this.targetCollectable = null;
			this.aiState = TreasureHuntPlayer.TreasureHuntAIState.Digging;
		}
		if (this.checkCollectiblesTimer.Elapsed(true) && this.aiState != TreasureHuntPlayer.TreasureHuntAIState.MovingToCenter && this.aiState != TreasureHuntPlayer.TreasureHuntAIState.GettingTreasure && this.aiState != TreasureHuntPlayer.TreasureHuntAIState.UncoveringTreasure)
		{
			foreach (TreasureHuntObject treasureHuntObject in this.minigameController.objects)
			{
				if (treasureHuntObject != null && !treasureHuntObject.Buried && (this.targetCollectable == null || (this.targetCollectable.Stuck && !treasureHuntObject.Stuck)))
				{
					Vector3 vector3 = treasureHuntObject.transform.position - base.transform.position;
					if (vector3.y <= ((!treasureHuntObject.Stuck) ? 1f : 2f))
					{
						float sqrMagnitude2 = vector3.sqrMagnitude;
						if (sqrMagnitude2 < 16f && (this.targetCollectable == null || sqrMagnitude2 < num || (this.targetCollectable.Stuck && !treasureHuntObject.Stuck)))
						{
							this.targetCollectable = treasureHuntObject;
							num = sqrMagnitude2;
						}
					}
				}
			}
		}
		if (this.targetCollectable != null)
		{
			this.aiState = (this.targetCollectable.Stuck ? TreasureHuntPlayer.TreasureHuntAIState.UncoveringCollectable : TreasureHuntPlayer.TreasureHuntAIState.GettingCollectable);
		}
		if (this.aiState == TreasureHuntPlayer.TreasureHuntAIState.MovingToCenter)
		{
			if (base.transform.position.y < 10.7f)
			{
				this.aiState = TreasureHuntPlayer.TreasureHuntAIState.Digging;
			}
			result = new CharacterMoverInput(new Vector2(0f, 1f), false, false);
		}
		else if (this.aiState == TreasureHuntPlayer.TreasureHuntAIState.Digging || this.aiState == TreasureHuntPlayer.TreasureHuntAIState.UncoveringCollectable || this.aiState == TreasureHuntPlayer.TreasureHuntAIState.UncoveringTreasure)
		{
			if (this.aiState == TreasureHuntPlayer.TreasureHuntAIState.Digging)
			{
				if (this.targetChangeTimer.Elapsed(true) || sqrMagnitude < 4f || this.targetPoint == Vector3.zero)
				{
					int num2 = 100;
					for (int i = 0; i < num2; i++)
					{
						float num3 = this.minigameController.grid.grid_size.y - 6f;
						int minValue = (int)(num3 - num3 * Mathf.Clamp01(this.minigameController.TimeSinceStart() / 60f));
						this.gridPoint = new Vector3i(GameManager.rand.Next(0, (int)this.minigameController.grid.grid_size.x), GameManager.rand.Next(minValue, (int)this.minigameController.grid.grid_size.y), GameManager.rand.Next(0, (int)this.minigameController.grid.grid_size.z));
						if (this.minigameController.grid.voxelGrid.Filled(this.gridPoint))
						{
							this.targetPoint = this.minigameController.grid.voxelGrid.GridPointToWorldSpace(this.gridPoint);
							break;
						}
						this.minigameController.TimeSinceStart();
					}
				}
			}
			else if (this.aiState == TreasureHuntPlayer.TreasureHuntAIState.UncoveringCollectable)
			{
				this.targetPoint = this.targetCollectable.transform.position;
			}
			else if (this.minigameController.treasure != null)
			{
				this.targetPoint = this.minigameController.treasure.transform.position;
			}
			float num4 = 2f;
			float num5 = 5f;
			float y = Mathf.Clamp01((this.wallDistance - num4) / num5);
			result = new CharacterMoverInput(new Vector2(0f, y), false, false);
		}
		else
		{
			if (this.aiState == TreasureHuntPlayer.TreasureHuntAIState.GettingCollectable)
			{
				this.targetPoint = this.targetCollectable.transform.position;
			}
			else if (this.aiState == TreasureHuntPlayer.TreasureHuntAIState.GettingTreasure)
			{
				this.targetPoint = this.minigameController.treasure.transform.position;
			}
			result = new CharacterMoverInput(new Vector2(0f, 1f), false, false);
		}
		if (vector != Vector3.zero)
		{
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(vector), 550f * Time.deltaTime);
		}
		Debug.DrawLine(base.transform.position, this.targetPoint, Color.red, 0.1f);
		return result;
	}

	// Token: 0x060012BE RID: 4798 RVA: 0x0009140C File Offset: 0x0008F60C
	protected override void UpdateAnimationState(PlayerAnimation playerAnim)
	{
		Vector2 vector = new Vector2(this.velocity.Value.x, this.velocity.Value.z);
		playerAnim.Velocity = vector.magnitude / this.mover.maxSpeed;
		playerAnim.VelocityY = this.velocity.Value.y;
		playerAnim.Grounded = this.netIsGrounded.Value;
		if (base.IsOwner)
		{
			playerAnim.SpineRotation = (this.player.IsAI ? this.aiZRot : this.playerCam.ZRotation);
			Vector3 vector2 = base.transform.rotation * new Vector3(this.mover.MovementAxis.y, 0f, this.mover.MovementAxis.x);
			playerAnim.MovementAxis = new Vector2(vector2.z, vector2.x);
		}
		else
		{
			playerAnim.MovementAxisByte = this.net_movement_axis.Value;
			playerAnim.SpineRotation = ZPMath.DecompressFloat(this.net_z_rotation.Value, -45f, 45f);
		}
		playerAnim.SetPlayerRotation(this.rotation.Value);
	}

	// Token: 0x060012BF RID: 4799 RVA: 0x0000F089 File Offset: 0x0000D289
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x060012C0 RID: 4800 RVA: 0x0000480A File Offset: 0x00002A0A
	private void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x060012C1 RID: 4801 RVA: 0x0000F091 File Offset: 0x0000D291
	private void OnTriggerEnter(Collider c)
	{
		this.DoTrigger(c);
	}

	// Token: 0x060012C2 RID: 4802 RVA: 0x0000F091 File Offset: 0x0000D291
	private void OnTriggerStay(Collider c)
	{
		this.DoTrigger(c);
	}

	// Token: 0x060012C3 RID: 4803 RVA: 0x00091544 File Offset: 0x0008F744
	private void DoTrigger(Collider c)
	{
		if (base.IsOwner && this.minigameController.Playable && !this.isDead)
		{
			TreasureHuntObject component = c.transform.root.gameObject.GetComponent<TreasureHuntObject>();
			if (component != null)
			{
				component.Interact((short)base.OwnerSlot, false);
			}
		}
	}

	// Token: 0x040013CB RID: 5067
	public GameObject camera_prefab;

	// Token: 0x040013CC RID: 5068
	public GameObject vacuum_pipe_pfb;

	// Token: 0x040013CD RID: 5069
	public Transform pipe_start;

	// Token: 0x040013CE RID: 5070
	public Transform pipe_end;

	// Token: 0x040013CF RID: 5071
	public TreasureHuntController minigameController;

	// Token: 0x040013D0 RID: 5072
	public Camera cam;

	// Token: 0x040013D1 RID: 5073
	public Material baseVacuumMaterial;

	// Token: 0x040013D2 RID: 5074
	public MeshRenderer[] vacuumRenderers;

	// Token: 0x040013D3 RID: 5075
	private CharacterMover mover;

	// Token: 0x040013D4 RID: 5076
	private Transform targeter;

	// Token: 0x040013D5 RID: 5077
	private VacuumPipe vacuumPipe;

	// Token: 0x040013D6 RID: 5078
	private Transform suctionPoint;

	// Token: 0x040013D7 RID: 5079
	private const float sqr = 3600f;

	// Token: 0x040013D8 RID: 5080
	private float maxDigDistance = 6f;

	// Token: 0x040013D9 RID: 5081
	private float lastDigTime;

	// Token: 0x040013DA RID: 5082
	private float curDigInterval = 0.1f;

	// Token: 0x040013DB RID: 5083
	private float maxDigInterval = 0.1f;

	// Token: 0x040013DC RID: 5084
	private float minDigInterval = 0.08f;

	// Token: 0x040013DD RID: 5085
	private bool vacuumLast;

	// Token: 0x040013DE RID: 5086
	private VacuumSound vacuumSound;

	// Token: 0x040013DF RID: 5087
	private List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();

	// Token: 0x040013E0 RID: 5088
	private ParticleSystem vacuumParticle;

	// Token: 0x040013E1 RID: 5089
	private ParticleSystem staticVacuumParticle;

	// Token: 0x040013E2 RID: 5090
	private float vacuumParticleMaxSpeed = 25f;

	// Token: 0x040013E3 RID: 5091
	private float aiZRot;

	// Token: 0x040013E4 RID: 5092
	private TreasureHuntPlayer.TreasureHuntAIState aiState;

	// Token: 0x040013E5 RID: 5093
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<bool> vacuuming = new NetVar<bool>(false);

	// Token: 0x040013E6 RID: 5094
	private float wallDistance;

	// Token: 0x040013E7 RID: 5095
	private ParticleSystem.Particle[] temp_particles;

	// Token: 0x040013E8 RID: 5096
	private Vector3 targetPoint = Vector3.zero;

	// Token: 0x040013E9 RID: 5097
	private TreasureHuntObject targetCollectable;

	// Token: 0x040013EA RID: 5098
	private Vector3i gridPoint = Vector3i.zero;

	// Token: 0x040013EB RID: 5099
	private ActionTimer targetChangeTimer = new ActionTimer(2f, 6f);

	// Token: 0x040013EC RID: 5100
	private ActionTimer checkCollectiblesTimer = new ActionTimer(0.05f, 0.07f);

	// Token: 0x040013ED RID: 5101
	private bool treasureFirstTime;

	// Token: 0x040013EE RID: 5102
	private ActionTimer TreasureGetWaitTimer = new ActionTimer(0f);

	// Token: 0x02000280 RID: 640
	private enum TreasureHuntAIState
	{
		// Token: 0x040013F0 RID: 5104
		MovingToCenter,
		// Token: 0x040013F1 RID: 5105
		Digging,
		// Token: 0x040013F2 RID: 5106
		UncoveringCollectable,
		// Token: 0x040013F3 RID: 5107
		GettingCollectable,
		// Token: 0x040013F4 RID: 5108
		UncoveringTreasure,
		// Token: 0x040013F5 RID: 5109
		GettingTreasure
	}
}
