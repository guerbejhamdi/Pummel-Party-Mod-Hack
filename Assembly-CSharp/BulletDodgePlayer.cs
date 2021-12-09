using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000172 RID: 370
public class BulletDodgePlayer : Movement1
{
	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x0000AED7 File Offset: 0x000090D7
	public BulletDodgePlayer.BulletDodgePlayerState CurState
	{
		get
		{
			return this.cur_state;
		}
	}

	// Token: 0x06000AB7 RID: 2743 RVA: 0x0005D54C File Offset: 0x0005B74C
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.mover = base.GetComponent<CharacterMover>();
		this.mover.SetForwardVector(new Vector3(1f, 0f, 0f));
		CharacterMover characterMover = this.mover;
		characterMover.OnJump = (CharacterMover.OnJumpDelegate)Delegate.Combine(characterMover.OnJump, new CharacterMover.OnJumpDelegate(this.OnJump));
		if (!base.IsOwner)
		{
			base.GetComponent<CharacterController>().enabled = false;
		}
	}

	// Token: 0x06000AB8 RID: 2744 RVA: 0x0000AEDF File Offset: 0x000090DF
	public override void OnOwnerChanged()
	{
		if (NetSystem.IsServer)
		{
			base.GetComponent<CharacterController>().enabled = true;
		}
		base.OnOwnerChanged();
	}

	// Token: 0x06000AB9 RID: 2745 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x06000ABA RID: 2746 RVA: 0x0005D5C8 File Offset: 0x0005B7C8
	protected override void Start()
	{
		base.Start();
		this.minigameController = (BulletDodgeController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.playerColorLight.color = base.GamePlayer.Color.skinColor1;
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x0005D628 File Offset: 0x0005B828
	private void Update()
	{
		if (this.cur_state == BulletDodgePlayer.BulletDodgePlayerState.Dead)
		{
			if (Time.time - this.death_time > this.death_length)
			{
				this.cur_state = BulletDodgePlayer.BulletDodgePlayerState.Immune;
				this.ResetPlayer();
				return;
			}
		}
		else if (this.cur_state != BulletDodgePlayer.BulletDodgePlayerState.Permadeath)
		{
			base.UpdateController();
			if (this.cur_state == BulletDodgePlayer.BulletDodgePlayerState.Immune)
			{
				if (Time.time - this.last_flicker > this.immune_flicker_rate)
				{
					this.last_flicker = Time.time;
				}
				if (Time.time - this.death_time > this.immunity_length + this.death_length)
				{
					this.cur_state = BulletDodgePlayer.BulletDodgePlayerState.Active;
				}
			}
		}
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x0005D6BC File Offset: 0x0005B8BC
	protected override void DoMovement()
	{
		CharacterMoverInput characterMoverInput = default(CharacterMoverInput);
		bool val = !this.minigameController.Playable || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput || this.isDead;
		if (!base.GamePlayer.IsAI)
		{
			Vector2 vector = new Vector2(0f, this.player.RewiredPlayer.GetAxis(InputActions.Horizontal));
			characterMoverInput = new CharacterMoverInput(vector, this.player.RewiredPlayer.GetButtonDown(InputActions.Accept), false);
		}
		else
		{
			characterMoverInput = this.GetAIInput();
		}
		characterMoverInput.NullInput(val);
		this.mover.CalculateVelocity(characterMoverInput, Time.deltaTime);
		this.mover.DoMovement(Time.deltaTime);
		float y = base.transform.rotation.eulerAngles.y;
		if (characterMoverInput.axis.y > 0.1f)
		{
			y = -100f;
		}
		if (characterMoverInput.axis.y < -0.1f)
		{
			y = 100f;
		}
		base.transform.rotation = Quaternion.Euler(0f, y, 0f);
		this.netIsGrounded.Value = this.controller.isGrounded;
		this.velocity.Value = this.mover.Velocity;
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x0005D814 File Offset: 0x0005BA14
	protected override void UpdateAnimationState(PlayerAnimation player_anim)
	{
		Vector2 vector = new Vector2(this.velocity.Value.x, this.velocity.Value.z);
		float num = Mathf.Clamp01(vector.magnitude / this.mover.maxSpeed);
		this.playerAnim.Velocity = num;
		player_anim.VelocityY = this.velocity.y;
		this.playerAnim.MovementAxis = ((num > 0.01f) ? Vector2.up : Vector2.zero);
		player_anim.Grounded = this.netIsGrounded.Value;
		player_anim.SetPlayerRotation(-this.rotation.Value);
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x0005D8C0 File Offset: 0x0005BAC0
	private CharacterMoverInput GetAIInput()
	{
		if (this.curAIState == BulletDodgePlayer.BulletDodgeAIState.Moving)
		{
			if (this.speedTimer.Elapsed(true))
			{
				this.tarSpeed = ZPMath.RandomFloat(GameManager.rand, 0.6f, 1f);
				if ((float)GameManager.rand.NextDouble() < 0.1f)
				{
					this.tarSpeed = 0f;
				}
			}
			this.curSpeed = Mathf.Lerp(this.curSpeed, this.tarSpeed, Time.deltaTime);
			if (this.curAIState == BulletDodgePlayer.BulletDodgeAIState.Moving)
			{
				if (this.movementTimer.Elapsed(true))
				{
					this.moveLeft = (GameManager.rand.NextDouble() > 0.5);
				}
				if (this.moveLeft && base.transform.position.x < -14f)
				{
					this.moveLeft = false;
					this.movementTimer.Start();
				}
				else if (!this.moveLeft && base.transform.position.x > 14f)
				{
					this.moveLeft = true;
					this.movementTimer.Start();
				}
				this.axis = new Vector2(0f, this.moveLeft ? (-this.curSpeed) : this.curSpeed);
			}
			using (List<BulletDodgeProjectile>.Enumerator enumerator = BulletDodgeProjectile.projectiles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BulletDodgeProjectile bulletDodgeProjectile = enumerator.Current;
					if ((base.transform.position - bulletDodgeProjectile.transform.position).sqrMagnitude < 75f && ((bulletDodgeProjectile.velocity.x < -0.1f && bulletDodgeProjectile.transform.position.x > base.transform.position.x) || (bulletDodgeProjectile.velocity.x > 0.1f && bulletDodgeProjectile.transform.position.x < base.transform.position.x)))
					{
						switch (bulletDodgeProjectile.type)
						{
						case BulletDodgeBulletType.Unassigned:
							Debug.Log("Bullet Type not Set");
							goto IL_27D;
						case BulletDodgeBulletType.Left3:
							base.StartCoroutine(this.DodgeNormal(true, false));
							goto IL_27D;
						case BulletDodgeBulletType.Right3:
							base.StartCoroutine(this.DodgeNormal(false, false));
							goto IL_27D;
						case BulletDodgeBulletType.Left6:
							base.StartCoroutine(this.DodgeNormal(true, true));
							goto IL_27D;
						case BulletDodgeBulletType.Right6:
							base.StartCoroutine(this.DodgeNormal(false, true));
							goto IL_27D;
						case BulletDodgeBulletType.Spinner:
							goto IL_26D;
						default:
							goto IL_27D;
						}
					}
				}
				IL_26D:;
			}
			IL_27D:
			Vector3 vector = Vector3.one * 1000f;
			float num = float.MaxValue;
			bool flag = false;
			foreach (BulletDodgeProjectile bulletDodgeProjectile2 in BulletDodgeProjectile.projectiles)
			{
				if (bulletDodgeProjectile2.type == BulletDodgeBulletType.Spinner)
				{
					flag = true;
					float sqrMagnitude = (bulletDodgeProjectile2.spinnerHitPoint - base.transform.position).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						vector = bulletDodgeProjectile2.spinnerHitPoint;
					}
				}
			}
			if (flag)
			{
				float num2 = Mathf.Sqrt(num);
				if (num2 < 1.5f)
				{
					float num3 = Mathf.Clamp(num2 / 1.5f, 0.1f, 0.5f);
					this.axis.y = ((vector.x < base.transform.position.x) ? num3 : (-num3));
				}
				else
				{
					this.axis.y = 0f;
				}
			}
		}
		CharacterMoverInput result = new CharacterMoverInput(this.axis, this.jump, false);
		this.axis = Vector2.zero;
		this.jump = false;
		return result;
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x0000AEFA File Offset: 0x000090FA
	private IEnumerator DodgeNormal(bool left, bool six = false)
	{
		this.curAIState = BulletDodgePlayer.BulletDodgeAIState.Dodging;
		this.jump = true;
		float seconds = UnityEngine.Random.Range(six ? 0.07f : 0.13f, six ? this.difficultJumpDelayTimeSixMax[(int)this.player.Difficulty] : this.difficultJumpDelayTimeMax[(int)this.player.Difficulty]);
		yield return new WaitForSeconds(seconds);
		this.jump = true;
		yield return new WaitForSeconds(0.03f);
		float startTime = Time.time;
		float interval = 0.2f;
		while (Time.time - startTime < interval)
		{
			float num = this.jumpMoveSpeed.Evaluate((Time.time - startTime) / interval);
			this.axis.y = (left ? (-num) : num);
			yield return null;
		}
		while (!this.mover.Grounded)
		{
			foreach (BulletDodgeProjectile bulletDodgeProjectile in BulletDodgeProjectile.projectiles)
			{
				int num2 = 0;
				int num3 = 0;
				if (bulletDodgeProjectile.type == BulletDodgeBulletType.Left3 || bulletDodgeProjectile.type == BulletDodgeBulletType.Left6 || bulletDodgeProjectile.type == BulletDodgeBulletType.Right3 || bulletDodgeProjectile.type == BulletDodgeBulletType.Right6)
				{
					if (bulletDodgeProjectile.velocity.x > 0.5f && bulletDodgeProjectile.transform.position.x < base.transform.position.x)
					{
						num3++;
					}
					else if (bulletDodgeProjectile.velocity.x < -0.5f && bulletDodgeProjectile.transform.position.x > base.transform.position.x)
					{
						num2++;
					}
				}
				if (num2 != 0 || num3 != 0)
				{
					if (num2 >= num3 && base.transform.position.x > -14f)
					{
						this.axis.y = -1f;
					}
					else if (num3 > num2 && base.transform.position.x < 14f)
					{
						this.axis.y = 1f;
					}
				}
			}
			yield return null;
		}
		this.curAIState = BulletDodgePlayer.BulletDodgeAIState.Moving;
		yield break;
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x0005DC9C File Offset: 0x0005BE9C
	private void OnTriggerEnter(Collider c)
	{
		if (base.IsOwner && this.minigameController.State == MinigameControllerState.Playing && ((c.gameObject.name == "Bullet" && this.cur_state != BulletDodgePlayer.BulletDodgePlayerState.Immune) || c.gameObject.name == "DeathZone"))
		{
			this.KillPlayer();
		}
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x0000AF17 File Offset: 0x00009117
	public override void ResetPlayer()
	{
		this.Activate();
		this.curAIState = BulletDodgePlayer.BulletDodgeAIState.Moving;
		base.StopAllCoroutines();
		base.ResetPlayer();
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x0000AF32 File Offset: 0x00009132
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x06000AC3 RID: 2755 RVA: 0x0000480A File Offset: 0x00002A0A
	protected void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x0000AF3A File Offset: 0x0000913A
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCKillPlayer(NetPlayer sender)
	{
		this.KillPlayer();
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x0005DCFC File Offset: 0x0005BEFC
	public void KillPlayer()
	{
		if (!this.isDead)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.player_death_effect, base.transform.position, Quaternion.LookRotation(Vector3.up));
			AudioSystem.PlayOneShot("DeathSplash01", 0.5f, 0.1f);
			if (this.lives <= 0)
			{
				this.cur_state = BulletDodgePlayer.BulletDodgePlayerState.Permadeath;
				this.isDead = true;
				this.Deactivate();
				if (NetSystem.IsServer)
				{
					if (this.minigameController == null)
					{
						this.minigameController = (BulletDodgeController)GameManager.Minigame;
					}
					if (this.minigameController != null)
					{
						this.minigameController.PlayerDied(this);
					}
				}
			}
			else
			{
				this.cur_state = BulletDodgePlayer.BulletDodgePlayerState.Dead;
				this.isDead = true;
				this.Deactivate();
				this.death_time = Time.time;
				this.lives--;
				this.UpdateLives();
			}
			if (base.IsOwner)
			{
				base.SendRPC("RPCKillPlayer", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
			}
		}
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x0000AF42 File Offset: 0x00009142
	public void UpdateLives()
	{
		if (this.minigameController != null)
		{
			this.minigameController.UpdateUILives((int)base.OwnerSlot, this.lives);
		}
	}

	// Token: 0x04000999 RID: 2457
	public float immunity_length = 3f;

	// Token: 0x0400099A RID: 2458
	public float death_length = 1.5f;

	// Token: 0x0400099B RID: 2459
	public float immune_flicker_rate = 0.25f;

	// Token: 0x0400099C RID: 2460
	public int lives = 2;

	// Token: 0x0400099D RID: 2461
	public GameObject player_death_effect;

	// Token: 0x0400099E RID: 2462
	public BulletDodgeController minigameController;

	// Token: 0x0400099F RID: 2463
	public AnimationCurve jumpMoveSpeed;

	// Token: 0x040009A0 RID: 2464
	public Light playerColorLight;

	// Token: 0x040009A1 RID: 2465
	private BulletDodgePlayer.BulletDodgePlayerState cur_state;

	// Token: 0x040009A2 RID: 2466
	private float death_time;

	// Token: 0x040009A3 RID: 2467
	private float last_flicker;

	// Token: 0x040009A4 RID: 2468
	private CharacterMover mover;

	// Token: 0x040009A5 RID: 2469
	private BulletDodgePlayer.BulletDodgeAIState curAIState;

	// Token: 0x040009A6 RID: 2470
	private ActionTimer movementTimer = new ActionTimer(0.5f, 2.25f);

	// Token: 0x040009A7 RID: 2471
	private ActionTimer speedTimer = new ActionTimer(0.25f, 1f);

	// Token: 0x040009A8 RID: 2472
	private float curSpeed = 1f;

	// Token: 0x040009A9 RID: 2473
	private float tarSpeed = 1f;

	// Token: 0x040009AA RID: 2474
	private bool moveLeft;

	// Token: 0x040009AB RID: 2475
	private Vector3 axis = Vector2.zero;

	// Token: 0x040009AC RID: 2476
	private bool jump;

	// Token: 0x040009AD RID: 2477
	private float[] difficultJumpDelayTimeSixMax = new float[]
	{
		0.3f,
		0.18f,
		0.07f
	};

	// Token: 0x040009AE RID: 2478
	private float[] difficultJumpDelayTimeMax = new float[]
	{
		0.4f,
		0.26f,
		0.13f
	};

	// Token: 0x02000173 RID: 371
	public enum BulletDodgePlayerState
	{
		// Token: 0x040009B0 RID: 2480
		Active,
		// Token: 0x040009B1 RID: 2481
		Dead,
		// Token: 0x040009B2 RID: 2482
		Immune,
		// Token: 0x040009B3 RID: 2483
		Permadeath
	}

	// Token: 0x02000174 RID: 372
	private enum BulletDodgeAIState
	{
		// Token: 0x040009B5 RID: 2485
		Moving,
		// Token: 0x040009B6 RID: 2486
		Dodging,
		// Token: 0x040009B7 RID: 2487
		AvoidingSpinner
	}
}
