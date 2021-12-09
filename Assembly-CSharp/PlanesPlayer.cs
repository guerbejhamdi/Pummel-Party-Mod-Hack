using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x020001FC RID: 508
public class PlanesPlayer : PlaneBase
{
	// Token: 0x06000EE4 RID: 3812 RVA: 0x0000CF11 File Offset: 0x0000B111
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.m_planesController = (PlanesController)this.minigameController;
	}

	// Token: 0x06000EE5 RID: 3813 RVA: 0x0000CF2A File Offset: 0x0000B12A
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x06000EE6 RID: 3814 RVA: 0x0000CF32 File Offset: 0x0000B132
	public override void FinishedSpawning()
	{
		base.FinishedSpawning();
	}

	// Token: 0x06000EE7 RID: 3815 RVA: 0x000771A0 File Offset: 0x000753A0
	private void Update()
	{
		if (base.IsOwner)
		{
			if (this.minigameController.Playable && !this.isDead)
			{
				if (this.player.IsAI)
				{
					if (this.m_aiTarget == null || this.m_aiTarget.isDead)
					{
						this.UpdateAITarget();
					}
					if (this.m_aiTarget != null)
					{
						base.SetAITarget(this.m_aiTarget.transform.position);
					}
				}
				this.m_lockTarget = null;
				this.m_smallestLockAngle = float.MaxValue;
				this.m_lockDistance = 0f;
				this.m_lockDir = Vector3.zero;
				for (int i = 0; i < this.minigameController.GetPlayerCount(); i++)
				{
					PlanesPlayer planesPlayer = (PlanesPlayer)this.minigameController.GetPlayer(i);
					if (!planesPlayer.IsDead && !(planesPlayer == this))
					{
						Vector3 normalized = (planesPlayer.transform.position - base.transform.position).normalized;
						float num = Vector3.Distance(planesPlayer.transform.position, base.transform.position);
						float num2 = Vector3.Angle(base.transform.forward, normalized);
						if (num < this.m_maxLockDist && num2 < this.m_maxLockAngle && num2 < this.m_smallestLockAngle)
						{
							this.m_lockTarget = planesPlayer;
							this.m_smallestLockAngle = num2;
							this.m_lockDistance = num;
							this.m_lockDir = normalized;
						}
					}
				}
				this.m_lockAccuracy = (this.m_smallestLockAngle / this.m_maxLockAngle + this.m_lockDistance / this.m_maxLockDist) * 0.5f;
			}
			base.UpdatePlaneController();
			if (GameManager.Minigame != null && GameManager.Minigame.AllClientsReady() && this.minigameController.Playable && !this.player.IsAI)
			{
				if (this.m_aimer == null)
				{
					GameObject gameObject = this.minigameController.Spawn(this.m_aimerPfb, Vector3.zero, Quaternion.identity);
					this.m_aimer = gameObject.GetComponent<PlanesUIAimer>();
					this.m_aimer.SetCamera(this.m_cam);
				}
				else if (this.m_lockTarget == null)
				{
					this.m_aimer.SetLockScale(1f);
					this.m_aimer.SetPosition(base.transform.position + base.transform.forward * 10f);
					this.m_aimer.SetHasTarget(false);
				}
				else
				{
					this.m_aimer.SetLockScale(this.m_lockAccuracy);
					this.m_aimer.SetPosition(this.m_lockTarget.transform.position);
					this.m_aimer.SetHasTarget(true);
					this.m_aimer.SetEnemyHealth((int)this.m_lockTarget.Health, 5);
				}
			}
			if (this.minigameController.Playable && !this.isDead)
			{
				if (!base.GamePlayer.IsAI)
				{
					if ((this.player.RewiredPlayer.GetButton(InputActions.Accept) || this.player.RewiredPlayer.GetButton(InputActions.UseItemShoot)) && Time.time > this.m_nextFireTime)
					{
						this.m_nextFireTime = Time.time + this.m_fireCooldown;
						this.Shoot(this.GetShootDirection(), false);
					}
				}
				else if (Time.time > this.m_nextFireTime && this.m_lockTarget != null)
				{
					this.m_nextFireTime = Time.time + this.m_fireCooldown;
					this.Shoot(this.GetShootDirection(), false);
				}
			}
			if (this.m_aimer != null)
			{
				this.m_aimer.SetPlayerHealth((int)base.Health, 5);
				return;
			}
		}
		else
		{
			base.UpdatePlaneController();
		}
	}

	// Token: 0x06000EE8 RID: 3816 RVA: 0x0007756C File Offset: 0x0007576C
	private void UpdateAITarget()
	{
		List<PlanesPlayer> list = new List<PlanesPlayer>();
		for (int i = 0; i < this.minigameController.GetPlayerCount(); i++)
		{
			PlanesPlayer planesPlayer = (PlanesPlayer)this.minigameController.GetPlayerInSlot((short)i);
			if (planesPlayer != this)
			{
				list.Add(planesPlayer);
			}
		}
		this.m_aiTarget = list[UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x06000EE9 RID: 3817 RVA: 0x000775D0 File Offset: 0x000757D0
	private Vector3 GetShootDirection()
	{
		if (this.m_lockTarget == null)
		{
			return base.transform.forward;
		}
		float num = 500f;
		float d = this.m_lockDistance / num;
		Vector3 a = this.m_lockTarget.transform.position + this.m_lockTarget.transform.forward * this.m_planeVelocity * d;
		float num2 = 3f * Mathf.Max(0.25f, this.m_lockAccuracy);
		Quaternion rotation = Quaternion.Euler(UnityEngine.Random.Range(-num2, num2), UnityEngine.Random.Range(-num2, num2), UnityEngine.Random.Range(-num2, num2));
		Vector3 normalized = (a - this.m_bulletSpawn.position).normalized;
		return rotation * normalized;
	}

	// Token: 0x06000EEA RID: 3818 RVA: 0x0000CF3A File Offset: 0x0000B13A
	public void OnBulletHitPlayer(PlanesPlayer hitPlayer, RaycastHit hitInfo)
	{
		if (hitPlayer == this)
		{
			return;
		}
		if (this.minigameController != null)
		{
			this.m_planesController.TryDamagePlayer(this, hitPlayer, hitInfo.point);
		}
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x0000CF68 File Offset: 0x0000B168
	public void OnPlayerDamaged(Vector3 worldPos)
	{
		AudioSystem.PlayOneShot(this.m_explodeSound, worldPos, 0.2f, AudioRolloffMode.Logarithmic, 100f, 100f, 0f);
		UnityEngine.Object.Instantiate<GameObject>(this.m_hitSparkPfb, base.transform.position, Quaternion.identity);
	}

	// Token: 0x06000EEC RID: 3820 RVA: 0x00077694 File Offset: 0x00075894
	private void Shoot(Vector3 direction, bool proxy = false)
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCShoot", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				direction
			});
		}
		this.m_muzzleFlash.Emit(1);
		AudioSystem.PlayOneShot(this.m_shootClips[UnityEngine.Random.Range(0, this.m_shootClips.Length - 1)], base.transform.position, 1f, AudioRolloffMode.Logarithmic, 100f, 100f, 0f);
		this.minigameController.Spawn(this.m_bulletPfb, this.m_bulletSpawn.position, Quaternion.LookRotation(direction, Vector3.up)).GetComponent<PlanesBullet>().Init(this, proxy);
	}

	// Token: 0x06000EED RID: 3821 RVA: 0x00077740 File Offset: 0x00075940
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCShoot(NetPlayer sender, Vector3 direction)
	{
		try
		{
			this.Shoot(direction, true);
		}
		catch (Exception ex)
		{
			Debug.LogError("PlanesPlayer Error : " + ex.Message);
		}
	}

	// Token: 0x04000E9F RID: 3743
	[Header("Shooting ")]
	[SerializeField]
	protected ParticleSystem m_muzzleFlash;

	// Token: 0x04000EA0 RID: 3744
	[SerializeField]
	protected float m_fireCooldown = 0.25f;

	// Token: 0x04000EA1 RID: 3745
	[SerializeField]
	protected AudioClip[] m_shootClips;

	// Token: 0x04000EA2 RID: 3746
	[SerializeField]
	protected GameObject m_bulletPfb;

	// Token: 0x04000EA3 RID: 3747
	[SerializeField]
	protected Transform m_bulletSpawn;

	// Token: 0x04000EA4 RID: 3748
	[Header("UI")]
	[SerializeField]
	private GameObject m_aimerPfb;

	// Token: 0x04000EA5 RID: 3749
	private float m_nextFireTime;

	// Token: 0x04000EA6 RID: 3750
	private PlanesUIAimer m_aimer;

	// Token: 0x04000EA7 RID: 3751
	private PlanesController m_planesController;

	// Token: 0x04000EA8 RID: 3752
	private PlanesPlayer m_lockTarget;

	// Token: 0x04000EA9 RID: 3753
	private float m_maxLockAngle = 10f;

	// Token: 0x04000EAA RID: 3754
	private float m_maxLockDist = 300f;

	// Token: 0x04000EAB RID: 3755
	private float m_lockDistance;

	// Token: 0x04000EAC RID: 3756
	private float m_smallestLockAngle;

	// Token: 0x04000EAD RID: 3757
	private Vector3 m_lockDir = Vector3.zero;

	// Token: 0x04000EAE RID: 3758
	private float m_lockAccuracy;

	// Token: 0x04000EAF RID: 3759
	private PlanesPlayer m_aiTarget;
}
