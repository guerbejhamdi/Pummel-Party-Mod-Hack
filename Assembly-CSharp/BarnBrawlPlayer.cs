using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000148 RID: 328
public class BarnBrawlPlayer : Movement1
{
	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x06000956 RID: 2390 RVA: 0x0000A3DB File Offset: 0x000085DB
	// (set) Token: 0x06000957 RID: 2391 RVA: 0x0000A3F6 File Offset: 0x000085F6
	private short ProxyHealth
	{
		get
		{
			if (!NetSystem.IsServer)
			{
				return this.proxyHealth;
			}
			return this.health.Value;
		}
		set
		{
			this.proxyHealth = value;
			if (NetSystem.IsServer)
			{
				this.health.Value = value;
			}
		}
	}

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x06000958 RID: 2392 RVA: 0x0000A412 File Offset: 0x00008612
	// (set) Token: 0x06000959 RID: 2393 RVA: 0x000533E0 File Offset: 0x000515E0
	public bool HoldingShotgun
	{
		get
		{
			return this.holdingShotgun;
		}
		set
		{
			if (this.holdingShotgun != value)
			{
				this.shootChanceTimer.Start();
				if (NetSystem.IsServer)
				{
					base.SendRPC("SetHoldingShotgunRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
					{
						value
					});
				}
				if (value)
				{
					this.shotgunObject.SetActive(value);
					AudioSystem.PlayOneShot(this.shotgunPickup, 0.25f, 0f, 1f);
				}
				if (base.IsOwner && !this.player.IsAI)
				{
					this.crossHair.enabled = value;
				}
				this.holdingShotgun = value;
			}
		}
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x00053474 File Offset: 0x00051674
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.mover = base.GetComponent<CharacterMover>();
		this.minigameController = (BarnBrawlController)GameManager.Minigame;
		this.playerAnim.RegisterListener(new AnimationEventListener(this.OnShotgunShot), AnimationEventType.BarnBrawlShotgunShot);
		if (base.IsOwner)
		{
			CharacterMover characterMover = this.mover;
			characterMover.OnJump = (CharacterMover.OnJumpDelegate)Delegate.Combine(characterMover.OnJump, new CharacterMover.OnJumpDelegate(this.OnJump));
			List<GamePlayer> localNonAIPlayers = GameManager.GetLocalNonAIPlayers();
			if (!this.player.IsAI || localNonAIPlayers.Count == 0)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.cameraPrefab, Vector3.zero, Quaternion.identity);
				gameObject.transform.parent = base.transform;
				this.cam = base.gameObject.GetComponentInChildren<Camera>();
				this.cameraShake = base.gameObject.GetComponentInChildren<CameraShake>();
				this.playerCam = base.gameObject.GetComponent<ThirdPersonCamera>();
				this.playerCam.SetTargetCamera(this.cam);
				this.playerCam.YRotation = base.transform.rotation.eulerAngles.y;
				this.playerCam.ZRotation = 5f;
				this.playerCam.RotateCamera();
				this.playerCam.UpdateCamera();
				this.saturationVolume = gameObject.GetComponentInChildren<PostProcessVolume>();
				this.minigameController.minigameCameras.Add(this.cam);
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
				}
				Vector3 vector = new Vector3((float)Screen.width * this.cam.rect.x + (float)Screen.width * (this.cam.rect.width * 0.5f), (float)Screen.height * this.cam.rect.y + (float)Screen.height * (this.cam.rect.height * 0.5f));
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.aimer);
				gameObject2.transform.SetParent(GameManager.UIController.MinigameUIRoot);
				gameObject2.transform.position = vector;
				this.crossHair = gameObject2.GetComponent<Image>();
				this.crossHair.enabled = false;
				GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.deathTextPrefab);
				gameObject3.transform.SetParent(GameManager.UIController.MinigameUIRoot);
				gameObject3.transform.position = vector;
				this.deathText = gameObject3.GetComponent<Text>();
				this.deathText.enabled = false;
				GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(this.killedTextPrefab);
				gameObject4.transform.SetParent(GameManager.UIController.MinigameUIRoot);
				gameObject4.transform.position = vector + Vector3.up * 250f;
				this.killedText = gameObject4.GetComponent<BarnBrawlKilledText>();
			}
		}
		if (!base.IsOwner || NetSystem.IsServer)
		{
			base.gameObject.GetComponent<CapsuleCollider>().isTrigger = false;
			Rigidbody rigidbody = base.gameObject.AddComponent<Rigidbody>();
			rigidbody.isKinematic = true;
			rigidbody.useGravity = false;
		}
		if (!base.IsOwner || this.player.IsAI)
		{
			base.GetComponent<ThirdPersonCamera>().enabled = false;
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

	// Token: 0x0600095B RID: 2395 RVA: 0x0000A41A File Offset: 0x0000861A
	public override void OnOwnerChanged()
	{
		if (NetSystem.IsServer)
		{
			base.GetComponent<NavMeshAgent>().enabled = true;
			this.mover.IsAI = true;
		}
		base.OnOwnerChanged();
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x0000A441 File Offset: 0x00008641
	protected override void Start()
	{
		base.Start();
		this.minigameController.AddPlayer(this);
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x0000A455 File Offset: 0x00008655
	public override void Activate()
	{
		base.Activate();
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x0000A45D File Offset: 0x0000865D
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCDoShotgunShot(NetPlayer sender, int seed, byte[] details)
	{
		this.DoShotgunShot(seed, details);
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x00053850 File Offset: 0x00051A50
	private void DoShotgunShot(int seed, byte[] details)
	{
		if (this.cameraShake != null)
		{
			this.cameraShake.AddShake(0.5f);
		}
		this.playerAnim.Animator.SetTrigger("ShootShotgun");
		this.HoldingShotgun = false;
		this.muzzleFlashEffect.Play();
		this.cartridgeEjectEffect.Play();
		AudioSystem.PlayOneShot(this.shotgunFire, 0.2f, 0f, 1f);
		Vector3 position = this.muzzle.position;
		System.Random random = new System.Random(seed);
		ZPBitStream zpbitStream;
		if (base.IsOwner)
		{
			zpbitStream = new ZPBitStream();
			zpbitStream.Write(position.x);
			zpbitStream.Write(position.y);
			zpbitStream.Write(position.z);
		}
		else
		{
			zpbitStream = new ZPBitStream(details, details.Length * 8);
			position = new Vector3(zpbitStream.ReadFloat(), zpbitStream.ReadFloat(), zpbitStream.ReadFloat());
		}
		int num = random.Next(this.minProjectiles, this.maxProjectiles + 1);
		Quaternion lhs = Quaternion.LookRotation(this.player.IsAI ? base.transform.forward : (this.targetPoint - this.muzzle.transform.position).normalized);
		int num2 = 0;
		List<int> list = new List<int>();
		for (int i = 0; i < num; i++)
		{
			float d = 0.1f;
			float x = ZPMath.RandomFloat(random, this.minSpread.y, this.maxSpread.y);
			float y = ZPMath.RandomFloat(random, this.minSpread.x, this.maxSpread.x);
			if (i == 0)
			{
				x = (y = 0f);
			}
			Quaternion rhs = Quaternion.Euler(x, y, 0f);
			Vector3 b = ZPMath.RandomPointInUnitSphere(random) * d;
			Ray ray = new Ray(position + b, lhs * rhs * Vector3.forward);
			float num3 = 30f;
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, num3, this.hitMask, QueryTriggerInteraction.Collide))
			{
				this.minigameController.HandleImpact(hit);
				if (base.IsOwner)
				{
					BarnBrawlPlayer component = hit.collider.gameObject.GetComponent<BarnBrawlPlayer>();
					zpbitStream.Write(component != null);
					if (component != null)
					{
						if (!list.Contains((int)component.GamePlayer.GlobalID))
						{
							num2++;
							list.Add((int)component.GamePlayer.GlobalID);
						}
						if (component.ApplyDamage(this, 1, position, 13f, 20f, 1f) && this.killedText != null)
						{
							this.killedText.Set(component);
						}
						zpbitStream.Write((byte)component.GamePlayer.GlobalID);
					}
				}
				this.CreateTrail(ray.origin, hit.point);
			}
			else
			{
				if (base.IsOwner)
				{
					zpbitStream.Write(false);
				}
				this.CreateTrail(ray.origin, ray.origin + ray.direction * num3);
			}
			if (!base.IsOwner && zpbitStream.ReadBool())
			{
				((BarnBrawlPlayer)this.minigameController.GetPlayer((int)zpbitStream.ReadByte())).ApplyDamage(this, 1, position, 13f, 20f, 1f);
			}
		}
		if (!this.player.IsAI && num2 >= 2)
		{
			PlatformAchievementManager.Instance.TriggerAchievement("ACH_BARN_BRAWL");
		}
		if (base.IsOwner)
		{
			base.SendRPC("RPCDoShotgunShot", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				seed,
				zpbitStream.GetDataCopy()
			});
		}
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x00053BF4 File Offset: 0x00051DF4
	public bool ApplyDamage(BarnBrawlPlayer player, int amount, Vector3 origin, float ragdollForce = 0f, float bloodForce = 0f, float bloodAmount = 1f)
	{
		if (base.IsDead)
		{
			return false;
		}
		if (Settings.BloodEffects && Time.time - this.lastBlood > this.minBloodInterval)
		{
			ParticleSystem component = UnityEngine.Object.Instantiate<GameObject>(this.bloodyDamageEffect, base.MidPoint, Quaternion.identity).GetComponent<ParticleSystem>();
			ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = component.velocityOverLifetime;
			velocityOverLifetime.enabled = true;
			velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
			Vector3 vector = (base.MidPoint - origin).normalized * bloodForce;
			velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(Mathf.Min(0f, vector.x), Mathf.Max(0f, vector.x));
			velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(Mathf.Min(0f, vector.y), Mathf.Max(0f, vector.y));
			velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(Mathf.Min(0f, vector.z), Mathf.Max(0f, vector.z));
			ParticleSystem.EmissionModule emission = component.emission;
			ParticleSystem.Burst[] array = new ParticleSystem.Burst[emission.burstCount];
			emission.GetBursts(array);
			array[0].maxCount = (short)((float)array[0].maxCount * bloodAmount);
			array[0].minCount = (short)((float)array[0].minCount * bloodAmount);
			emission.SetBursts(array);
			this.lastBlood = Time.time;
		}
		if ((int)this.ProxyHealth <= amount)
		{
			if (NetSystem.IsServer)
			{
				player.Score += 50;
			}
			this.killingPlayer = player;
			this.KillPlayer(origin, ragdollForce);
			return true;
		}
		this.ProxyHealth -= (short)amount;
		return false;
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x00053DAC File Offset: 0x00051FAC
	public void KillPlayer(Vector3 origin, float force)
	{
		base.IsDead = true;
		this.deathTimer.Start();
		this.SpawnRagdoll(origin, force);
		AudioSystem.PlayOneShot("DeathSplash01", 0.5f, 0.1f);
		this.ProxyHealth = 0;
		this.HoldingShotgun = false;
		this.SetText((int)this.deathTimer.Remaining);
		this.minigameController.AddKillFeed(this.killingPlayer, this);
		if (base.IsOwner)
		{
			this.mover.Velocity = Vector3.zero;
		}
		if (this.cameraShake != null)
		{
			this.cameraShake.enabled = false;
		}
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x00053E4C File Offset: 0x0005204C
	private void SetText(int seconds)
	{
		if (this.deathText == null)
		{
			return;
		}
		if (seconds != this.lastDeathTextSeconds)
		{
			AudioSystem.PlayOneShot(this.timerTick, 1f, 0f, 1f);
			this.deathText.enabled = true;
			this.lastDeathTextSeconds = seconds;
			string text = ColorUtility.ToHtmlStringRGBA(this.killingPlayer.GamePlayer.Color.uiColor);
			this.deathText.text = string.Concat(new string[]
			{
				"Killed by <color=#",
				text,
				">",
				this.killingPlayer.GamePlayer.Name,
				"</color>\n <size=30> Respawning in ",
				seconds.ToString(),
				"...</size>"
			});
		}
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x00053F14 File Offset: 0x00052114
	private void SpawnRagdoll(Vector3 origin, float force)
	{
		Vector3 normalized = (base.transform.position + Vector3.up * 0.875f - origin).normalized;
		this.playerAnim.SpawnRagdoll(normalized * force);
		this.Deactivate();
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x0000A467 File Offset: 0x00008667
	public void CreateTrail(Vector3 startPosition, Vector3 endPosition)
	{
		UnityEngine.Object.Instantiate<GameObject>(this.smokeTrailPrefab).GetComponent<ShotgunSmokeTrail>().Setup(startPosition, endPosition);
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x00053F68 File Offset: 0x00052168
	private void Update()
	{
		this.shotgunStrength = Mathf.Clamp01(this.shotgunStrength + (this.holdingShotgun ? 4f : -4f) * Time.deltaTime);
		float num = 2f;
		if (base.IsOwner && !this.player.IsAI)
		{
			this.saturationVolume.weight = Mathf.Clamp01(this.saturationVolume.weight + (base.IsDead ? num : (-num)) * Time.deltaTime);
		}
		if (!this.isDead)
		{
			base.UpdateController();
			if (base.IsOwner && !this.player.IsAI && this.holdingShotgun && this.minigameController.Playable)
			{
				Vector3 position = this.rayCastPoint.position;
				Plane plane = new Plane(base.transform.forward, position);
				Ray screenPointRay = this.playerCam.GetScreenPointRay();
				float distance = 0f;
				float num2 = 5f;
				float num3 = 20f;
				Color color = new Color(1f, 0f, 0f, 0.4f);
				Color color2 = new Color(1f, 1f, 1f, 0.4f);
				float num4 = 1f;
				float num5 = 2.25f;
				float num6 = 5f;
				float num7 = 20f;
				float num8 = num7 - num6;
				if (plane.Raycast(screenPointRay, out distance))
				{
					Vector3 point = screenPointRay.GetPoint(distance);
					Ray ray = new Ray(point, screenPointRay.direction);
					int layerMask = 3329;
					RaycastHit raycastHit;
					if (Physics.Raycast(ray, out raycastHit, num3, layerMask))
					{
						if (raycastHit.distance < num2)
						{
							this.targetPoint = ray.GetPoint(num2);
						}
						else if (raycastHit.distance > num3)
						{
							this.targetPoint = ray.GetPoint(num3);
						}
						else
						{
							this.targetPoint = raycastHit.point;
						}
						if (raycastHit.collider.gameObject.layer == 8)
						{
							this.crossHair.color = color;
						}
						else
						{
							this.crossHair.color = color2;
						}
						if (raycastHit.distance <= num6)
						{
							this.crossHair.rectTransform.localScale = Vector3.one * num5;
						}
						else if (raycastHit.distance >= num7)
						{
							this.crossHair.rectTransform.localScale = Vector3.one * num4;
						}
						else
						{
							float num9 = (raycastHit.distance - num6) / num8;
							this.crossHair.rectTransform.localScale = Vector3.one * (num5 - (num5 - num4) * num9);
						}
					}
					else
					{
						this.targetPoint = this.playerCam.GetPosition() + screenPointRay.direction * num3;
						this.crossHair.color = color2;
						this.crossHair.rectTransform.localScale = Vector3.one;
					}
				}
				this.targetSphere.transform.position = this.targetPoint;
				if (!GameManager.IsGamePaused && this.player.RewiredPlayer.GetButtonDown(InputActions.UseItemShoot))
				{
					this.DoShotgunShot(GameManager.rand.Next(0, int.MaxValue), null);
					return;
				}
			}
		}
		else
		{
			if (this.deathTimer.Elapsed(false))
			{
				this.ResetPlayer();
				return;
			}
			if (base.IsOwner && !this.player.IsAI)
			{
				Vector3 normalized = (this.killingPlayer.transform.position - this.cam.transform.position).normalized;
				Quaternion to = Quaternion.LookRotation(normalized);
				float maxDegreesDelta = Mathf.Clamp(Vector3.Dot(this.cam.transform.forward, normalized) * -1f + 1f, 0.06f, 2f) * 250f * Time.deltaTime;
				this.cam.transform.rotation = Quaternion.RotateTowards(this.cam.transform.rotation, to, maxDegreesDelta);
				this.SetText((int)this.deathTimer.Remaining);
			}
		}
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x00054398 File Offset: 0x00052598
	protected override void DoMovement()
	{
		CharacterMoverInput input = default(CharacterMoverInput);
		if (!this.player.IsAI)
		{
			Vector3 lookPos = this.playerCam.GetLookPos();
			this.mover.SetForwardVector(new Vector3(lookPos.x, 0f, lookPos.z).normalized);
			Vector2 axis = new Vector2(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
			if (this.moving)
			{
				axis.y = 1f;
			}
			input = new CharacterMoverInput(axis, this.player.RewiredPlayer.GetButtonDown(InputActions.Accept), false);
		}
		else
		{
			input = this.GetAIInput();
		}
		input.NullInput(!this.minigameController.Playable || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput);
		if (this.agent == null || !this.agent.isOnOffMeshLink)
		{
			this.mover.CalculateVelocity(input, Time.deltaTime);
			this.mover.DoMovement(Time.deltaTime);
		}
		if (!this.player.IsAI)
		{
			this.mover.SmoothSlope();
		}
		if (!this.player.IsAI && this.minigameController.Playable && !GameManager.IsGamePaused && GameManager.PollInput && this.minigameController.State != MinigameControllerState.EnablePlayers)
		{
			this.playerCam.RotateCamera();
			this.playerCam.UpdateCamera();
		}
		else if (this.player.IsAI)
		{
			float num = 0f;
			if (this.targetPlayer != null)
			{
				Vector3 vector = this.targetPlayer.transform.position + this.lookAtOffset - base.transform.position;
				vector.y = 0f;
				Vector2 vector2 = new Vector2(0f, base.transform.position.y);
				Vector2 vector3 = new Vector2(vector.magnitude, this.targetPlayer.transform.position.y);
				num = Mathf.Atan2(vector3.y - vector2.y, vector3.x - vector2.x) * 57.29578f;
				num = Mathf.Clamp(-num, -45f, 45f);
			}
			this.aiZRot = Mathf.MoveTowards(this.aiZRot, num, Time.deltaTime * 360f);
		}
		this.velocity.Value = this.mover.Velocity;
		this.netIsGrounded.Value = this.mover.Grounded;
		this.net_movement_axis.Value = this.playerAnim.GetMovementAxis();
		this.net_z_rotation.Value = ZPMath.CompressFloat(this.player.IsAI ? this.aiZRot : this.playerCam.ZRotation, -45f, 45f);
		base.DoMovement();
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x000546B8 File Offset: 0x000528B8
	private CharacterMoverInput GetAIInput()
	{
		CharacterMoverInput result = default(CharacterMoverInput);
		new Vector2(base.transform.position.x, base.transform.position.z);
		float num = 121f;
		if (this.agent.isOnOffMeshLink)
		{
			if (this.curOffMeshLinkTranslationType == OffMeshLinkTranslateType.None)
			{
				this.curOffMeshLinkTranslationType = OffMeshLinkTranslateType.Parabola;
				this.OnJump();
				float initialHorizontalVelocity = 5f;
				base.StartCoroutine(base.GetParabolicPath(this.mover, this.mover.gravity, 1500f, initialHorizontalVelocity, false));
			}
		}
		else
		{
			if (!this.holdingShotgun && (this.targetPickup == null || this.targetCheckTimer.Elapsed(true)))
			{
				this.targetPlayer = null;
				float num2 = float.MaxValue;
				for (int i = 0; i < this.minigameController.ShotgunPickups.Length; i++)
				{
					BarnBrawlShotgunPickup barnBrawlShotgunPickup = this.minigameController.ShotgunPickups[i];
					if (barnBrawlShotgunPickup != null)
					{
						float sqrMagnitude = (barnBrawlShotgunPickup.transform.position - base.transform.position).sqrMagnitude;
						if (sqrMagnitude < num2)
						{
							num2 = sqrMagnitude;
							this.targetPickup = barnBrawlShotgunPickup;
						}
					}
				}
			}
			if (this.holdingShotgun && (this.targetPlayer == null || this.targetPlayer.isDead || this.targetCheckTimer.Elapsed(true)))
			{
				this.targetPickup = null;
				float num3 = float.MaxValue;
				if (this.targetPlayer != null)
				{
					num3 = (this.targetPlayer.transform.position - base.transform.position).sqrMagnitude * 0.8f;
				}
				short num4 = 0;
				while ((int)num4 < this.minigameController.GetPlayerCount())
				{
					CharacterBase playerInSlot = this.minigameController.GetPlayerInSlot(num4);
					if (!(playerInSlot == this) && !playerInSlot.IsDead && !(playerInSlot == this.targetPlayer))
					{
						float sqrMagnitude2 = (playerInSlot.transform.position - base.transform.position).sqrMagnitude;
						if (sqrMagnitude2 < num3)
						{
							num3 = sqrMagnitude2;
							this.targetPlayer = (BarnBrawlPlayer)playerInSlot;
							float d = ZPMath.RandomFloat(GameManager.rand, this.difficultyOffsetMin[(int)base.GamePlayer.Difficulty], this.difficultyOffsetMax[(int)base.GamePlayer.Difficulty]);
							this.lookAtOffset = ZPMath.RandomPointInUnitSphere(GameManager.rand).normalized * d;
						}
					}
					num4 += 1;
				}
			}
			this.targetPosition = (this.targetPickup ? this.targetPickup.transform.position : (this.targetPlayer ? this.targetPlayer.transform.position : base.transform.position));
			RaycastHit raycastHit;
			if (Physics.Raycast(this.targetPosition, Vector3.down, out raycastHit, 6f, 9216))
			{
				this.targetPosition = raycastHit.point;
			}
			bool flag = false;
			if (this.targetPlayer != null)
			{
				Vector3 lhs = this.targetPlayer.transform.position + this.lookAtOffset - base.transform.position;
				lhs.y = 0f;
				lhs.Normalize();
				flag = (Vector3.Dot(lhs, base.transform.forward) > 0.85f);
			}
			if (this.targetPlayer != null && this.HoldingShotgun && this.checkCanSee.Elapsed(true))
			{
				this.canSee = false;
				RaycastHit raycastHit2;
				if (Physics.Raycast(base.transform.position, (this.targetPlayer.transform.position - base.transform.position).normalized, out raycastHit2, 11f, 9472))
				{
					BarnBrawlPlayer component = raycastHit2.collider.gameObject.GetComponent<BarnBrawlPlayer>();
					this.canSee = (component != null && component == this.targetPlayer);
					Debug.DrawLine(base.transform.position, raycastHit2.point, Color.green, 0.1f);
				}
				else
				{
					Debug.DrawLine(base.transform.position, this.targetPlayer.transform.position, Color.red, 0.1f);
				}
			}
			float sqrMagnitude3 = (this.targetPosition - base.transform.position).sqrMagnitude;
			if (!this.canSee || !this.holdingShotgun || sqrMagnitude3 > num || sqrMagnitude3 <= 5f)
			{
				if (sqrMagnitude3 <= 5f && this.targetPlayer != null)
				{
					if (this.moveAwayGetPointTimer.Elapsed(true))
					{
						float d2 = 9f;
						Vector3[] array = new Vector3[]
						{
							(Vector3.down - base.transform.right * d2).normalized,
							(Vector3.down - Vector3.forward * d2).normalized,
							(Vector3.down + Vector3.right * d2).normalized
						};
						float num5 = float.MinValue;
						Vector3 vector = Vector3.zero;
						for (int j = 0; j < array.Length; j++)
						{
							RaycastHit raycastHit3;
							if (Physics.Raycast(base.transform.position, array[j], out raycastHit3, 10f) && raycastHit3.distance > num5)
							{
								num5 = raycastHit3.distance;
								vector = raycastHit3.point;
							}
						}
						this.moveAwayPoint = vector;
					}
					this.targetPosition = this.moveAwayPoint;
				}
				Vector3 vector2 = this.agent.steeringTarget - base.transform.position;
				vector2.y = 0f;
				vector2.Normalize();
				result = new CharacterMoverInput(new Vector2(vector2.x, vector2.z), false, false);
			}
			else if (this.canSee && flag)
			{
				if (this.shootChanceTimer.Elapsed(true) && GameManager.rand.NextDouble() > (double)this.difficultyChanceToNotShoot[(int)base.GamePlayer.Difficulty])
				{
					this.DoShotgunShot(GameManager.rand.Next(0, int.MaxValue), null);
				}
				this.RandomMovement();
				result = new CharacterMoverInput(this.randomMovementVector, false, false);
			}
			else if (this.canSee)
			{
				this.RandomMovement();
				result = new CharacterMoverInput(this.randomMovementVector, false, false);
			}
			if (this.pathUpdateTimer.Elapsed(true))
			{
				NavMeshHit navMeshHit;
				NavMesh.SamplePosition(this.targetPosition, out navMeshHit, 1f, -1);
				if (this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
				{
					this.agent.SetDestination(navMeshHit.position);
				}
			}
		}
		Vector3 vector3 = ((this.targetPickup != null) ? this.agent.steeringTarget : this.targetPosition) + ((this.targetPlayer != null) ? this.lookAtOffset : Vector3.zero) - base.transform.position;
		vector3.y = 0f;
		vector3.Normalize();
		if (vector3 != Vector3.zero)
		{
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(vector3, Vector3.up), 550f * Time.deltaTime);
		}
		return result;
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x00054E74 File Offset: 0x00053074
	private void RandomMovement()
	{
		if (this.randomMovementTimer.Elapsed(true))
		{
			this.randomMovementVector.Set((float)GameManager.rand.NextDouble() * 2f - 1f, (float)(GameManager.rand.NextDouble() * 2.0) - 1f);
			this.randomMovementVector.Normalize();
		}
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x00054ED8 File Offset: 0x000530D8
	protected override void UpdateAnimationState(PlayerAnimation playerAnim)
	{
		Vector2 vector = new Vector2(this.velocity.Value.x, this.velocity.Value.z);
		playerAnim.Velocity = vector.magnitude / this.mover.maxSpeed;
		playerAnim.VelocityY = this.velocity.Value.y;
		playerAnim.Grounded = this.netIsGrounded.Value;
		if (playerAnim.Animator.isActiveAndEnabled)
		{
			playerAnim.Animator.SetFloat("ShotgunStrength", this.shotgunStrength);
		}
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
		playerAnim.SetPlayerRotation(this.rotation.Value + 10f);
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x0005503C File Offset: 0x0005323C
	private void OnShotgunShot(PlayerAnimationEvent anim_event)
	{
		if (this.HoldingShotgun)
		{
			return;
		}
		this.shotgunObject.SetActive(false);
		this.shotgunRigidbody.SetActive(true);
		this.shotgunRigidbody.transform.position = this.shotgunObject.transform.position;
		this.shotgunRigidbody.transform.rotation = this.shotgunObject.transform.rotation;
		this.shotgunRigidbody.transform.parent = null;
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x000550BC File Offset: 0x000532BC
	public override void ResetPlayer()
	{
		this.Activate();
		this.shotgunObject.SetActive(false);
		this.ProxyHealth = 0;
		this.mover.Velocity = Vector3.zero;
		if (this.cam != null)
		{
			this.cam.transform.localRotation = Quaternion.identity;
		}
		if (this.deathText != null)
		{
			this.deathText.enabled = false;
		}
		if (this.cameraShake != null)
		{
			this.cameraShake.enabled = true;
		}
		base.ResetPlayer();
		if (this.spawnPoints == null)
		{
			this.spawnPoints = UnityEngine.Object.FindObjectsOfType<BarnBrawlSpawnPoint>();
		}
		if (this.spawnPoints != null && this.spawnPoints.Length != 0)
		{
			int num = UnityEngine.Random.Range(0, this.spawnPoints.Length);
			base.transform.position = this.spawnPoints[num].transform.position;
			base.transform.rotation = this.spawnPoints[num].transform.rotation;
		}
		if (this.playerCam != null)
		{
			this.playerCam.ZRotation = 5f;
			this.playerCam.YRotation = base.transform.rotation.eulerAngles.y;
		}
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x0000A480 File Offset: 0x00008680
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x0000480A File Offset: 0x00002A0A
	protected void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x0000A488 File Offset: 0x00008688
	private void OnTrigger(Collider collider)
	{
		if (NetSystem.IsServer && !this.HoldingShotgun && collider.gameObject.CompareTag("BarnBrawlPickup"))
		{
			NetSystem.Kill(collider.GetComponent<BarnBrawlShotgunPickup>());
			this.HoldingShotgun = true;
		}
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x0000A4BD File Offset: 0x000086BD
	public void OnTriggerEnter(Collider collider)
	{
		this.OnTrigger(collider);
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x0000A4BD File Offset: 0x000086BD
	public void OnTriggerStay(Collider collider)
	{
		this.OnTrigger(collider);
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x0000A4C6 File Offset: 0x000086C6
	public Transform GetHandTransform()
	{
		return this.playerAnim.GetBone(PlayerBone.RightHand);
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x0000A4D5 File Offset: 0x000086D5
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void SetHoldingShotgunRPC(NetPlayer sender, bool holdingShotgun)
	{
		this.HoldingShotgun = holdingShotgun;
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x0000A4DE File Offset: 0x000086DE
	public void OnDestroy()
	{
		UnityEngine.Object.Destroy(this.shotgunRigidbody);
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x00055200 File Offset: 0x00053400
	public BarnBrawlPlayer()
	{
		float[] array = new float[3];
		array[0] = 1.5f;
		array[1] = 0.75f;
		this.difficultyOffsetMin = array;
		this.difficultyOffsetMax = new float[]
		{
			2.5f,
			1.25f,
			0.75f
		};
		this.difficultyChanceToNotShoot = new float[]
		{
			0.8f,
			0.65f,
			0.4f
		};
		this.randomMovementTimer = new ActionTimer(0.25f, 1f);
		this.randomMovementVector = Vector2.zero;
		this.shootChanceTimer = new ActionTimer(0.8f, 1.2f);
		base..ctor();
	}

	// Token: 0x040007E1 RID: 2017
	public GameObject cameraPrefab;

	// Token: 0x040007E2 RID: 2018
	public GameObject bloodyDamageEffect;

	// Token: 0x040007E3 RID: 2019
	public GameObject aimer;

	// Token: 0x040007E4 RID: 2020
	public Transform rayCastPoint;

	// Token: 0x040007E5 RID: 2021
	public Transform targetSphere;

	// Token: 0x040007E6 RID: 2022
	public GameObject deathTextPrefab;

	// Token: 0x040007E7 RID: 2023
	public GameObject killedTextPrefab;

	// Token: 0x040007E8 RID: 2024
	public AudioClip timerTick;

	// Token: 0x040007E9 RID: 2025
	[Header("Shotgun")]
	public int minProjectiles = 7;

	// Token: 0x040007EA RID: 2026
	public int maxProjectiles = 10;

	// Token: 0x040007EB RID: 2027
	public Vector2 minSpread = new Vector2(-10f, -10f);

	// Token: 0x040007EC RID: 2028
	public Vector2 maxSpread = new Vector2(10f, 10f);

	// Token: 0x040007ED RID: 2029
	public Transform muzzle;

	// Token: 0x040007EE RID: 2030
	public LayerMask hitMask;

	// Token: 0x040007EF RID: 2031
	public AudioClip shotgunFire;

	// Token: 0x040007F0 RID: 2032
	public AudioClip shotgunPickup;

	// Token: 0x040007F1 RID: 2033
	public GameObject shotgunObject;

	// Token: 0x040007F2 RID: 2034
	public GameObject shotgunRigidbody;

	// Token: 0x040007F3 RID: 2035
	public GameObject smokeTrailPrefab;

	// Token: 0x040007F4 RID: 2036
	public ParticleSystem muzzleFlashEffect;

	// Token: 0x040007F5 RID: 2037
	public ParticleSystem cartridgeEjectEffect;

	// Token: 0x040007F6 RID: 2038
	private bool holdingShotgun;

	// Token: 0x040007F7 RID: 2039
	private float shotgunStrength;

	// Token: 0x040007F8 RID: 2040
	private float lastBlood;

	// Token: 0x040007F9 RID: 2041
	private float minBloodInterval = 0.5f;

	// Token: 0x040007FA RID: 2042
	private short proxyHealth = 1;

	// Token: 0x040007FB RID: 2043
	private Camera cam;

	// Token: 0x040007FC RID: 2044
	private CharacterMover mover;

	// Token: 0x040007FD RID: 2045
	private ActionTimer deathTimer = new ActionTimer(4f);

	// Token: 0x040007FE RID: 2046
	private BarnBrawlController minigameController;

	// Token: 0x040007FF RID: 2047
	private Image crossHair;

	// Token: 0x04000800 RID: 2048
	private Vector3 targetPoint = Vector3.zero;

	// Token: 0x04000801 RID: 2049
	private float aiZRot;

	// Token: 0x04000802 RID: 2050
	private PostProcessVolume saturationVolume;

	// Token: 0x04000803 RID: 2051
	private BarnBrawlPlayer killingPlayer;

	// Token: 0x04000804 RID: 2052
	private Text deathText;

	// Token: 0x04000805 RID: 2053
	private BarnBrawlKilledText killedText;

	// Token: 0x04000806 RID: 2054
	private CameraShake cameraShake;

	// Token: 0x04000807 RID: 2055
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	private NetVar<short> health = new NetVar<short>(1);

	// Token: 0x04000808 RID: 2056
	private int lastDeathTextSeconds;

	// Token: 0x04000809 RID: 2057
	private bool moving;

	// Token: 0x0400080A RID: 2058
	private BarnBrawlShotgunPickup targetPickup;

	// Token: 0x0400080B RID: 2059
	private BarnBrawlPlayer targetPlayer;

	// Token: 0x0400080C RID: 2060
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x0400080D RID: 2061
	private Vector3 moveAwayPoint = Vector3.zero;

	// Token: 0x0400080E RID: 2062
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x0400080F RID: 2063
	private bool throwing;

	// Token: 0x04000810 RID: 2064
	private Vector3 lookAtOffset = Vector3.zero;

	// Token: 0x04000811 RID: 2065
	private ActionTimer checkCanSee = new ActionTimer(0.05f, 0.075f);

	// Token: 0x04000812 RID: 2066
	private ActionTimer targetCheckTimer = new ActionTimer(0.75f, 1f);

	// Token: 0x04000813 RID: 2067
	private ActionTimer moveAwayGetPointTimer = new ActionTimer(0.4f, 0.6f);

	// Token: 0x04000814 RID: 2068
	private bool canSee;

	// Token: 0x04000815 RID: 2069
	private float[] difficultyOffsetMin;

	// Token: 0x04000816 RID: 2070
	private float[] difficultyOffsetMax;

	// Token: 0x04000817 RID: 2071
	private float[] difficultyChanceToNotShoot;

	// Token: 0x04000818 RID: 2072
	private ActionTimer randomMovementTimer;

	// Token: 0x04000819 RID: 2073
	private Vector2 randomMovementVector;

	// Token: 0x0400081A RID: 2074
	private ActionTimer shootChanceTimer;

	// Token: 0x0400081B RID: 2075
	private BarnBrawlSpawnPoint[] spawnPoints;
}
