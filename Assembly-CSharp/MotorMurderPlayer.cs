using System;
using Rewired;
using UnityEngine;
using ZP.Net;

// Token: 0x020001E1 RID: 481
public class MotorMurderPlayer : CharacterBase
{
	// Token: 0x1700013E RID: 318
	// (get) Token: 0x06000DFA RID: 3578 RVA: 0x0000C832 File Offset: 0x0000AA32
	// (set) Token: 0x06000DFB RID: 3579 RVA: 0x000704DC File Offset: 0x0006E6DC
	public float Heat
	{
		get
		{
			if (!base.IsOwner)
			{
				return (float)this.heat.Value;
			}
			return this.heatLocal;
		}
		set
		{
			this.heatLocal = Mathf.Clamp(value, 0f, 255f);
			this.heat.Value = (byte)this.heatLocal;
			if (base.IsOwner)
			{
				if (this.heat.Value >= 255 && !this.overheated)
				{
					AudioSystem.PlayOneShot(this.overheatSFX, 4f, 0f, 1f);
					this.overheated = true;
					return;
				}
				if (this.heat.Value < 128 && this.overheated)
				{
					this.overheated = false;
				}
			}
		}
	}

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x06000DFC RID: 3580 RVA: 0x0000C84F File Offset: 0x0000AA4F
	// (set) Token: 0x06000DFD RID: 3581 RVA: 0x00070578 File Offset: 0x0006E778
	public float Health
	{
		get
		{
			if (!base.IsOwner)
			{
				return (float)this.health.Value;
			}
			return this.healthLocal;
		}
		set
		{
			this.healthLocal = Mathf.Clamp(value, 0f, 50f);
			this.health.Value = (byte)this.healthLocal;
			if (this.healthLocal == 0f && !base.IsDead)
			{
				this.Kill(this.lastHitter);
			}
		}
	}

	// Token: 0x06000DFE RID: 3582 RVA: 0x000705D0 File Offset: 0x0006E7D0
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.Health = this.maxhealth;
		this.minigameController = (MotorMurderController)GameManager.Minigame;
		if (base.IsOwner)
		{
			this.position.Value = this.rollyCar.transform.position;
			this.rotation.Value = this.rollyCar.root.transform.rotation.eulerAngles;
		}
		else
		{
			this.position.Recieve = new RecieveProxy(this.RecievePosition);
			this.interpolator = new Interpolator(this.rollyCar.transform, Interpolator.InterpolationType.PositionTransform);
		}
		Material material = new Material(this.bodyRenderer.sharedMaterial);
		material.SetColor("_Color", this.player.Color.skinColor1);
		Color skinColor = this.player.Color.skinColor1;
		skinColor.r *= 0.2f;
		skinColor.g *= 0.2f;
		skinColor.b *= 0.2f;
		material.SetColor("_EmissionColor", skinColor);
		this.bodyRenderer.sharedMaterial = material;
		if (!base.IsOwner)
		{
			this.rollyCar.rb.isKinematic = true;
		}
	}

	// Token: 0x06000DFF RID: 3583 RVA: 0x0000C86C File Offset: 0x0000AA6C
	public void RecievePosition(object _pos)
	{
		this.interpolator.NewPosition(_pos);
	}

	// Token: 0x06000E00 RID: 3584 RVA: 0x0000B3FE File Offset: 0x000095FE
	public override void OnOwnerChanged()
	{
		bool isServer = NetSystem.IsServer;
		base.OnOwnerChanged();
	}

	// Token: 0x06000E01 RID: 3585 RVA: 0x0000C87A File Offset: 0x0000AA7A
	protected override void Start()
	{
		base.Start();
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this.bodyRenderer.transform);
	}

	// Token: 0x06000E02 RID: 3586 RVA: 0x00070718 File Offset: 0x0006E918
	private void Update()
	{
		if (base.IsOwner)
		{
			this.playable = (this.minigameController != null && this.minigameController.State >= MinigameControllerState.FadeIn);
			if (this.minigameController.Playable && !base.IsDead)
			{
				if (!this.player.IsAI)
				{
					this.vertical = this.player.RewiredPlayer.GetAxis(InputActions.Vertical);
					this.horizontal = this.player.RewiredPlayer.GetAxis(InputActions.Horizontal);
					this.isJoystick = false;
					if (this.player.RewiredPlayer.controllers.GetLastActiveController() != null)
					{
						this.isJoystick = (this.player.RewiredPlayer.controllers.GetLastActiveController().type == ControllerType.Joystick);
						if (this.isJoystick)
						{
							this.vertical = (this.player.RewiredPlayer.GetButton(InputActions.UseItemShoot) ? 1f : (this.player.RewiredPlayer.GetButton(InputActions.Cancel) ? -1f : 0f));
							if (this.player.RewiredPlayer.GetButton(InputActions.Accept))
							{
								this.LocalShoot(this.currentBulletType);
							}
						}
						else if (this.player.RewiredPlayer.GetButton(InputActions.Accept))
						{
							this.LocalShoot(this.currentBulletType);
						}
					}
					else if (this.player.RewiredPlayer.GetButton(InputActions.Accept))
					{
						this.LocalShoot(this.currentBulletType);
					}
				}
				else
				{
					this.vertical = 1f;
					float num = 16f;
					float num2 = 15f;
					float num3 = -90f;
					Vector3 vector = Vector3.zero;
					int num4 = 0;
					while ((float)num4 < num)
					{
						Vector3 vector2 = Quaternion.Euler(0f, this.rollyCar.root.rotation.eulerAngles.y + num3, 0f) * Vector3.forward;
						RaycastHit raycastHit;
						if (Physics.Raycast(this.rollyCar.transform.position, vector2, out raycastHit, num2, 2048))
						{
							vector += raycastHit.point - this.rollyCar.transform.position;
							Debug.DrawLine(this.rollyCar.transform.position, raycastHit.point, Color.red);
						}
						else
						{
							vector += this.rollyCar.transform.position + vector2 * num2 - this.rollyCar.transform.position;
							Debug.DrawLine(this.rollyCar.transform.position, this.rollyCar.transform.position + vector2 * num2, Color.green);
						}
						num3 += 180f / num;
						num4++;
					}
					vector.Normalize();
					Debug.DrawLine(this.rollyCar.transform.position, this.rollyCar.transform.position + vector * num2, Color.blue);
					if (Vector3.Dot(vector, this.rollyCar.root.forward) < 0.995f)
					{
						if (Vector3.Dot(this.rollyCar.root.transform.right, vector) > 0f)
						{
							this.horizontal = Mathf.MoveTowards(this.horizontal, 1f, 4f);
						}
						else
						{
							this.horizontal = Mathf.MoveTowards(this.horizontal, -1f, 4f);
						}
					}
					else
					{
						this.horizontal = Mathf.MoveTowards(this.horizontal, 0f, 2f);
					}
					for (int i = 0; i < GameManager.GetPlayerCount(); i++)
					{
						MotorMurderPlayer motorMurderPlayer = (MotorMurderPlayer)this.minigameController.GetPlayer(i);
						if (!(motorMurderPlayer == this) && !motorMurderPlayer.IsDead)
						{
							Vector3 vector3 = motorMurderPlayer.rollyCar.transform.position - this.rollyCar.transform.position;
							float magnitude = vector3.magnitude;
							Vector3 normalized = vector3.normalized;
							RaycastHit raycastHit2;
							if (magnitude < this.bulletMaxDist && Vector3.Dot(normalized, this.rollyCar.root.forward) > this.aiBulletAngles[(int)this.currentBulletType] && !Physics.Raycast(this.rollyCar.transform.position, normalized, out raycastHit2, magnitude, 2048))
							{
								this.LocalShoot(this.currentBulletType);
								break;
							}
						}
					}
				}
				if (this.heatTimer.Elapsed(false))
				{
					this.Heat -= Time.deltaTime * 128f;
				}
				if (this.buffTimer.Elapsed(false))
				{
					this.currentBulletType = 0;
				}
			}
			else
			{
				this.vertical = 0f;
				this.horizontal = 0f;
			}
			this.UpdateNetVals();
			this.drifting.Value = this.rollyCar.Drifting;
		}
		else
		{
			this.UpdateNetVals();
			this.rollyCar.SetDrift(this.drifting.Value);
		}
		if (base.IsDead && this.respawnTimer.Elapsed(false))
		{
			this.ResetPlayer();
		}
	}

	// Token: 0x06000E03 RID: 3587 RVA: 0x0000C8AE File Offset: 0x0000AAAE
	private void LocalShoot(byte bulletTypeID)
	{
		if (!this.overheated && this.Heat < 255f && this.shootTimer.Elapsed(false))
		{
			this.Shoot(bulletTypeID);
		}
	}

	// Token: 0x06000E04 RID: 3588 RVA: 0x0000C8DA File Offset: 0x0000AADA
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCShoot(NetPlayer sender, byte typeID)
	{
		this.Shoot(typeID);
	}

	// Token: 0x06000E05 RID: 3589 RVA: 0x00070C80 File Offset: 0x0006EE80
	private void Shoot(byte typeID)
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCShoot", NetRPCDelivery.UNRELIABLE, new object[]
			{
				typeID
			});
			this.Heat += 10f;
			this.heatTimer.Start();
			this.shootTimer.Start();
		}
		AudioSystem.PlayOneShot(this.shootSFX[UnityEngine.Random.Range(0, this.shootSFX.Length)], 0.6f, 0f, 1f);
		switch (typeID)
		{
		case 0:
			this.SpawnBullet(typeID, this.bulletSpawnPoint.position, Quaternion.LookRotation(this.bulletSpawnPoint.forward));
			return;
		case 1:
			this.SpawnBullet(typeID, this.bulletSpawnPoint.position - this.bulletSpawnPoint.right * 0.5f, Quaternion.LookRotation(this.bulletSpawnPoint.forward));
			this.SpawnBullet(typeID, this.bulletSpawnPoint.position + this.bulletSpawnPoint.right * 0.5f, Quaternion.LookRotation(this.bulletSpawnPoint.forward));
			return;
		case 2:
			this.SpawnBullet(typeID, this.bulletSpawnPoint.position, Quaternion.LookRotation((this.bulletSpawnPoint.forward - this.bulletSpawnPoint.right * 0.25f).normalized));
			this.SpawnBullet(typeID, this.bulletSpawnPoint.position, Quaternion.LookRotation(this.bulletSpawnPoint.forward));
			this.SpawnBullet(typeID, this.bulletSpawnPoint.position, Quaternion.LookRotation((this.bulletSpawnPoint.forward + this.bulletSpawnPoint.right * 0.25f).normalized));
			return;
		case 3:
			this.SpawnBullet(typeID, this.bulletSpawnPoint.position, Quaternion.LookRotation((this.bulletSpawnPoint.forward - this.bulletSpawnPoint.right * 0.1875f).normalized));
			this.SpawnBullet(typeID, this.bulletSpawnPoint.position, Quaternion.LookRotation((this.bulletSpawnPoint.forward - this.bulletSpawnPoint.right * 0.0625f).normalized));
			this.SpawnBullet(typeID, this.bulletSpawnPoint.position, Quaternion.LookRotation((this.bulletSpawnPoint.forward + this.bulletSpawnPoint.right * 0.0625f).normalized));
			this.SpawnBullet(typeID, this.bulletSpawnPoint.position, Quaternion.LookRotation((this.bulletSpawnPoint.forward + this.bulletSpawnPoint.right * 0.1875f).normalized));
			return;
		default:
			return;
		}
	}

	// Token: 0x06000E06 RID: 3590 RVA: 0x00070F70 File Offset: 0x0006F170
	private void SpawnBullet(byte typeID, Vector3 spawnPoint, Quaternion rot)
	{
		MotorMurderBullet component = this.minigameController.Spawn(this.bulletPrefab, spawnPoint, rot).GetComponent<MotorMurderBullet>();
		component.SetType(typeID);
		float distance = this.bulletMaxDist;
		RaycastHit raycastHit;
		if (Physics.Raycast(component.transform.position, component.transform.forward, out raycastHit, this.bulletMaxDist, this.bulletHitMask))
		{
			Debug.DrawLine(component.transform.position, raycastHit.point);
			if (raycastHit.distance < this.bulletMaxDist)
			{
				distance = raycastHit.distance;
			}
			component.HitSomething = true;
			if (base.IsOwner && raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Debri"))
			{
				MotorMurderPlayer componentInParent = raycastHit.collider.gameObject.GetComponentInParent<MotorMurderPlayer>();
				if (componentInParent != null)
				{
					component.HitSomething = false;
					componentInParent.Hit(raycastHit.point, base.OwnerSlot, true);
				}
			}
		}
		component.maxDistance = distance;
	}

	// Token: 0x06000E07 RID: 3591 RVA: 0x00071070 File Offset: 0x0006F270
	public void Hit(Vector3 hitPoint, ushort hitterID, bool sendRPC = false)
	{
		this.lastHitter = hitterID;
		if (base.IsOwner)
		{
			this.Health -= 1f;
		}
		else if (sendRPC)
		{
			base.SendRPC("RPCHit", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				hitPoint,
				hitterID
			});
		}
		AudioSystem.PlayOneShot(this.hitSFX, 0.25f, 0f, 1f);
		UnityEngine.Object.Instantiate<GameObject>(this.hitFX, hitPoint, Quaternion.identity);
	}

	// Token: 0x06000E08 RID: 3592 RVA: 0x0000C8E3 File Offset: 0x0000AAE3
	[NetRPC(true, NetRPCSecurity.ALL, NetRPCSecurity.ALL)]
	public void RPCHit(NetPlayer sender, Vector3 hitPoint, ushort hitterID)
	{
		this.Hit(hitPoint, hitterID, false);
	}

	// Token: 0x06000E09 RID: 3593 RVA: 0x0000C8EE File Offset: 0x0000AAEE
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCKill(NetPlayer sender, ushort killerID)
	{
		this.Kill(killerID);
	}

	// Token: 0x06000E0A RID: 3594 RVA: 0x000710F4 File Offset: 0x0006F2F4
	public void Kill(ushort killerID)
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCKill", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				killerID
			});
		}
		base.IsDead = true;
		UnityEngine.Object.Instantiate<GameObject>(this.killFX, this.rollyCar.transform.position, Quaternion.identity);
		AudioSystem.PlayOneShot(this.explodeClip, 0.5f, 0f, 1f);
		this.Deactivate();
		this.minigameController.statusBars[(int)base.GamePlayer.GlobalID].gameObject.SetActive(false);
		this.minigameController.PlayerDied(this);
		if (NetSystem.IsServer)
		{
			CharacterBase player = this.minigameController.GetPlayer((int)killerID);
			player.Score += 10;
		}
		this.respawnTimer.Start();
	}

	// Token: 0x06000E0B RID: 3595 RVA: 0x000711C8 File Offset: 0x0006F3C8
	public override void ResetPlayer()
	{
		base.IsDead = false;
		this.Activate();
		this.minigameController.statusBars[(int)base.GamePlayer.GlobalID].gameObject.SetActive(true);
		this.Heat = 0f;
		this.Health = this.maxhealth;
		this.currentBulletType = 0;
		if (base.IsOwner)
		{
			Transform transform = this.minigameController.buffSpawnPoints[UnityEngine.Random.Range(0, this.minigameController.buffSpawnPoints.Length)];
			this.rollyCar.transform.position = new Vector3(transform.position.x, this.rollyCar.transform.position.y, transform.position.z);
			this.rollyCar.transform.rotation = transform.rotation;
			this.rollyCar.root.rotation = transform.rotation;
			this.rollyCar.rb.velocity = Vector3.zero;
			this.rollyCar.rb.angularVelocity = Vector3.zero;
		}
	}

	// Token: 0x06000E0C RID: 3596 RVA: 0x000712E4 File Offset: 0x0006F4E4
	private void UpdateNetVals()
	{
		if (base.IsOwner)
		{
			this.rotation.Value = this.rollyCar.root.transform.localRotation.eulerAngles;
			this.position.Value = this.rollyCar.transform.localPosition;
			return;
		}
		this.interpolator.Update();
		this.rollyCar.root.transform.localPosition = this.rollyCar.transform.localPosition;
		this.rollyCar.root.transform.localRotation = Quaternion.Euler(this.rotation.Value);
	}

	// Token: 0x06000E0D RID: 3597 RVA: 0x0000C8F7 File Offset: 0x0000AAF7
	private void FixedUpdate()
	{
		if (base.IsOwner)
		{
			this.rollyCar.DoUpdate(this.vertical, this.horizontal, this.isJoystick, this.playable);
		}
		this.rollyCar.SetEngine(this.playable);
	}

	// Token: 0x06000E0E RID: 3598 RVA: 0x00071394 File Offset: 0x0006F594
	public void PassCollision(Collider other)
	{
		MotorMurderBuff component = other.gameObject.GetComponent<MotorMurderBuff>();
		if (component != null && !component.Despawning)
		{
			AudioSystem.PlayOneShot(this.pickupBuff, 0.5f, 0f, 1f);
			this.currentBulletType = component.Type;
			this.buffTimer.Start();
			component.Despawn();
			this.minigameController.DespawnBuff(component.ID, true);
		}
	}

	// Token: 0x04000D5A RID: 3418
	public RollyCar rollyCar;

	// Token: 0x04000D5B RID: 3419
	public MeshRenderer bodyRenderer;

	// Token: 0x04000D5C RID: 3420
	public GameObject bulletPrefab;

	// Token: 0x04000D5D RID: 3421
	public Transform bulletSpawnPoint;

	// Token: 0x04000D5E RID: 3422
	public AudioClip overheatSFX;

	// Token: 0x04000D5F RID: 3423
	public AudioClip pickupBuff;

	// Token: 0x04000D60 RID: 3424
	public float maxhealth = 25f;

	// Token: 0x04000D61 RID: 3425
	public GameObject hitFX;

	// Token: 0x04000D62 RID: 3426
	public AudioClip hitSFX;

	// Token: 0x04000D63 RID: 3427
	private MotorMurderController minigameController;

	// Token: 0x04000D64 RID: 3428
	private CameraShake cameraShake;

	// Token: 0x04000D65 RID: 3429
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 position = new NetVec3(Vector3.zero);

	// Token: 0x04000D66 RID: 3430
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 rotation = new NetVec3(Vector3.zero);

	// Token: 0x04000D67 RID: 3431
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVar<bool> drifting = new NetVar<bool>(false);

	// Token: 0x04000D68 RID: 3432
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVar<byte> health = new NetVar<byte>(25);

	// Token: 0x04000D69 RID: 3433
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVar<byte> heat = new NetVar<byte>(0);

	// Token: 0x04000D6A RID: 3434
	private float healthLocal = 25f;

	// Token: 0x04000D6B RID: 3435
	private float heatLocal;

	// Token: 0x04000D6C RID: 3436
	private bool overheated = true;

	// Token: 0x04000D6D RID: 3437
	private ActionTimer buffTimer = new ActionTimer(8f);

	// Token: 0x04000D6E RID: 3438
	private ushort lastHitter;

	// Token: 0x04000D6F RID: 3439
	private ActionTimer respawnTimer = new ActionTimer(4f);

	// Token: 0x04000D70 RID: 3440
	private Interpolator interpolator;

	// Token: 0x04000D71 RID: 3441
	private float vertical;

	// Token: 0x04000D72 RID: 3442
	private float horizontal;

	// Token: 0x04000D73 RID: 3443
	private bool isJoystick;

	// Token: 0x04000D74 RID: 3444
	private bool playable;

	// Token: 0x04000D75 RID: 3445
	private ActionTimer shootTimer = new ActionTimer(0.1f);

	// Token: 0x04000D76 RID: 3446
	private ActionTimer heatTimer = new ActionTimer(1f);

	// Token: 0x04000D77 RID: 3447
	private byte currentBulletType;

	// Token: 0x04000D78 RID: 3448
	private float[] aiBulletAngles = new float[]
	{
		0.8f,
		0.7f,
		0.5f,
		0.6f
	};

	// Token: 0x04000D79 RID: 3449
	public AudioClip[] shootSFX;

	// Token: 0x04000D7A RID: 3450
	public float bulletMaxDist = 35f;

	// Token: 0x04000D7B RID: 3451
	public LayerMask bulletHitMask;

	// Token: 0x04000D7C RID: 3452
	public GameObject killFX;

	// Token: 0x04000D7D RID: 3453
	public AudioClip explodeClip;
}
