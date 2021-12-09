using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using ZP.Net;
using ZP.Utility;

// Token: 0x020001B4 RID: 436
public class HorseFighterPlayer : Movement1
{
	// Token: 0x17000119 RID: 281
	// (get) Token: 0x06000C86 RID: 3206 RVA: 0x0000BC61 File Offset: 0x00009E61
	// (set) Token: 0x06000C87 RID: 3207 RVA: 0x0000BC7C File Offset: 0x00009E7C
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

	// Token: 0x06000C88 RID: 3208 RVA: 0x00068AD8 File Offset: 0x00066CD8
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.mover = base.GetComponent<CharacterMover>();
		this.minigameController = (HorseFighterController)GameManager.Minigame;
		if (base.IsOwner)
		{
			CharacterMover characterMover = this.mover;
			characterMover.OnJump = (CharacterMover.OnJumpDelegate)Delegate.Combine(characterMover.OnJump, new CharacterMover.OnJumpDelegate(this.OnJump));
			if (!this.player.IsAI)
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
				List<GamePlayer> localNonAIPlayers = GameManager.GetLocalNonAIPlayers();
				if (localNonAIPlayers.Count > 1)
				{
					this.cam.rect = base.GetPlayerSplitScreenRect(this.player);
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

	// Token: 0x06000C89 RID: 3209 RVA: 0x0000BC98 File Offset: 0x00009E98
	public override void OnOwnerChanged()
	{
		if (NetSystem.IsServer)
		{
			base.GetComponent<NavMeshAgent>().enabled = true;
			this.mover.IsAI = true;
		}
		base.OnOwnerChanged();
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x0000BCBF File Offset: 0x00009EBF
	protected override void Start()
	{
		base.Start();
		this.minigameController.AddPlayer(this);
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x0000A455 File Offset: 0x00008655
	public override void Activate()
	{
		base.Activate();
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x00068E74 File Offset: 0x00067074
	public bool ApplyDamage(HorseFighterPlayer player, int amount, Vector3 origin, float ragdollForce = 0f, float bloodForce = 0f, float bloodAmount = 1f)
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

	// Token: 0x06000C8E RID: 3214 RVA: 0x0006902C File Offset: 0x0006722C
	public void KillPlayer(Vector3 origin, float force)
	{
		base.IsDead = true;
		this.deathTimer.Start();
		this.SpawnRagdoll(origin, force);
		AudioSystem.PlayOneShot("DeathSplash01", 0.5f, 0.1f);
		this.ProxyHealth = 0;
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

	// Token: 0x06000C8F RID: 3215 RVA: 0x000690C4 File Offset: 0x000672C4
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

	// Token: 0x06000C90 RID: 3216 RVA: 0x00053F14 File Offset: 0x00052114
	private void SpawnRagdoll(Vector3 origin, float force)
	{
		Vector3 normalized = (base.transform.position + Vector3.up * 0.875f - origin).normalized;
		this.playerAnim.SpawnRagdoll(normalized * force);
		this.Deactivate();
	}

	// Token: 0x06000C91 RID: 3217 RVA: 0x0006918C File Offset: 0x0006738C
	private void Update()
	{
		float num = 2f;
		if (base.IsOwner && !this.player.IsAI)
		{
			this.saturationVolume.weight = Mathf.Clamp01(this.saturationVolume.weight + (base.IsDead ? num : (-num)) * Time.deltaTime);
		}
		if (!this.isDead)
		{
			base.UpdateController();
			if (base.IsOwner)
			{
				bool isAI = this.player.IsAI;
				return;
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

	// Token: 0x06000C92 RID: 3218 RVA: 0x000692E8 File Offset: 0x000674E8
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

	// Token: 0x06000C93 RID: 3219 RVA: 0x00069608 File Offset: 0x00067808
	private CharacterMoverInput GetAIInput()
	{
		CharacterMoverInput result = default(CharacterMoverInput);
		new Vector2(base.transform.position.x, base.transform.position.z);
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
			if (this.targetPlayer != null)
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
			float sqrMagnitude = (this.targetPosition - base.transform.position).sqrMagnitude;
			if (!this.canSee)
			{
				if (sqrMagnitude <= 5f && this.targetPlayer != null)
				{
					if (this.moveAwayGetPointTimer.Elapsed(true))
					{
						float d = 9f;
						Vector3[] array = new Vector3[]
						{
							(Vector3.down - base.transform.right * d).normalized,
							(Vector3.down - Vector3.forward * d).normalized,
							(Vector3.down + Vector3.right * d).normalized
						};
						float num = float.MinValue;
						Vector3 vector = Vector3.zero;
						for (int i = 0; i < array.Length; i++)
						{
							RaycastHit raycastHit3;
							if (Physics.Raycast(base.transform.position, array[i], out raycastHit3, 10f) && raycastHit3.distance > num)
							{
								num = raycastHit3.distance;
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
				this.RandomMovement();
				result = new CharacterMoverInput(this.randomMovementVector, false, false);
			}
			else if (this.canSee)
			{
				this.RandomMovement();
				result = new CharacterMoverInput(this.randomMovementVector, false, false);
			}
			if (this.pathUpdateTimer.Elapsed(true) && this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
			{
				NavMeshHit navMeshHit;
				NavMesh.SamplePosition(this.targetPosition, out navMeshHit, 1f, -1);
				this.agent.SetDestination(navMeshHit.position);
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

	// Token: 0x06000C94 RID: 3220 RVA: 0x00069B34 File Offset: 0x00067D34
	private void RandomMovement()
	{
		if (this.randomMovementTimer.Elapsed(true))
		{
			this.randomMovementVector.Set((float)GameManager.rand.NextDouble() * 2f - 1f, (float)(GameManager.rand.NextDouble() * 2.0) - 1f);
			this.randomMovementVector.Normalize();
		}
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x00069B98 File Offset: 0x00067D98
	protected override void UpdateAnimationState(PlayerAnimation playerAnim)
	{
		this.vert = Mathf.MoveTowards(this.vert, Vector3.Dot(this.velocity.Value.normalized, base.transform.forward) * this.velocity.Value.magnitude * 0.5f, Time.deltaTime * 10f);
		this.horz = Mathf.MoveTowards(this.horz, Vector3.Dot(this.velocity.Value.normalized, base.transform.right) * this.velocity.Value.magnitude * 0.5f, Time.deltaTime * 9f);
		this.horseAnimator.SetFloat("Vertical", this.vert);
		this.horseAnimator.SetFloat("Horizontal", this.horz);
		if (Input.GetMouseButtonDown(0))
		{
			this.horseAnimator.SetBool("Attack1", true);
		}
		else
		{
			this.horseAnimator.SetBool("Attack1", false);
		}
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
		playerAnim.SetPlayerRotation(this.rotation.Value + 10f);
	}

	// Token: 0x06000C96 RID: 3222 RVA: 0x00069DE0 File Offset: 0x00067FE0
	public override void ResetPlayer()
	{
		this.Activate();
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

	// Token: 0x06000C97 RID: 3223 RVA: 0x0000BCD3 File Offset: 0x00009ED3
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x06000C98 RID: 3224 RVA: 0x0000480A File Offset: 0x00002A0A
	protected void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x06000C99 RID: 3225 RVA: 0x00069F18 File Offset: 0x00068118
	public HorseFighterPlayer()
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

	// Token: 0x04000BDA RID: 3034
	public GameObject cameraPrefab;

	// Token: 0x04000BDB RID: 3035
	public GameObject bloodyDamageEffect;

	// Token: 0x04000BDC RID: 3036
	public GameObject aimer;

	// Token: 0x04000BDD RID: 3037
	public Transform targetSphere;

	// Token: 0x04000BDE RID: 3038
	public GameObject deathTextPrefab;

	// Token: 0x04000BDF RID: 3039
	public GameObject killedTextPrefab;

	// Token: 0x04000BE0 RID: 3040
	public AudioClip timerTick;

	// Token: 0x04000BE1 RID: 3041
	public Animator horseAnimator;

	// Token: 0x04000BE2 RID: 3042
	private float lastBlood;

	// Token: 0x04000BE3 RID: 3043
	private float minBloodInterval = 0.5f;

	// Token: 0x04000BE4 RID: 3044
	private short proxyHealth = 1;

	// Token: 0x04000BE5 RID: 3045
	private Camera cam;

	// Token: 0x04000BE6 RID: 3046
	private CharacterMover mover;

	// Token: 0x04000BE7 RID: 3047
	private ActionTimer deathTimer = new ActionTimer(4f);

	// Token: 0x04000BE8 RID: 3048
	private HorseFighterController minigameController;

	// Token: 0x04000BE9 RID: 3049
	private Image crossHair;

	// Token: 0x04000BEA RID: 3050
	private Vector3 targetPoint = Vector3.zero;

	// Token: 0x04000BEB RID: 3051
	private float aiZRot;

	// Token: 0x04000BEC RID: 3052
	private PostProcessVolume saturationVolume;

	// Token: 0x04000BED RID: 3053
	private HorseFighterPlayer killingPlayer;

	// Token: 0x04000BEE RID: 3054
	private Text deathText;

	// Token: 0x04000BEF RID: 3055
	private BarnBrawlKilledText killedText;

	// Token: 0x04000BF0 RID: 3056
	private CameraShake cameraShake;

	// Token: 0x04000BF1 RID: 3057
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	private NetVar<short> health = new NetVar<short>(1);

	// Token: 0x04000BF2 RID: 3058
	private int lastDeathTextSeconds;

	// Token: 0x04000BF3 RID: 3059
	private bool moving;

	// Token: 0x04000BF4 RID: 3060
	private BarnBrawlShotgunPickup targetPickup;

	// Token: 0x04000BF5 RID: 3061
	private BarnBrawlPlayer targetPlayer;

	// Token: 0x04000BF6 RID: 3062
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x04000BF7 RID: 3063
	private Vector3 moveAwayPoint = Vector3.zero;

	// Token: 0x04000BF8 RID: 3064
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x04000BF9 RID: 3065
	private bool throwing;

	// Token: 0x04000BFA RID: 3066
	private Vector3 lookAtOffset = Vector3.zero;

	// Token: 0x04000BFB RID: 3067
	private ActionTimer checkCanSee = new ActionTimer(0.05f, 0.075f);

	// Token: 0x04000BFC RID: 3068
	private ActionTimer targetCheckTimer = new ActionTimer(0.75f, 1f);

	// Token: 0x04000BFD RID: 3069
	private ActionTimer moveAwayGetPointTimer = new ActionTimer(0.4f, 0.6f);

	// Token: 0x04000BFE RID: 3070
	private bool canSee;

	// Token: 0x04000BFF RID: 3071
	private float[] difficultyOffsetMin;

	// Token: 0x04000C00 RID: 3072
	private float[] difficultyOffsetMax;

	// Token: 0x04000C01 RID: 3073
	private float[] difficultyChanceToNotShoot;

	// Token: 0x04000C02 RID: 3074
	private ActionTimer randomMovementTimer;

	// Token: 0x04000C03 RID: 3075
	private Vector2 randomMovementVector;

	// Token: 0x04000C04 RID: 3076
	private ActionTimer shootChanceTimer;

	// Token: 0x04000C05 RID: 3077
	private float vert;

	// Token: 0x04000C06 RID: 3078
	private float horz;

	// Token: 0x04000C07 RID: 3079
	private BarnBrawlSpawnPoint[] spawnPoints;
}
