using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using ZP.Net;

// Token: 0x0200022E RID: 558
public class RunnerPlayer : CharacterBase
{
	// Token: 0x0600102F RID: 4143 RVA: 0x0007F550 File Offset: 0x0007D750
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		if (!base.IsOwner)
		{
			this.position.Recieve = new RecieveProxy(this.RecievePosition);
			this.velocity.Recieve = new RecieveProxy(this.RecieveVelocity);
			this.score.Recieve = new RecieveProxy(base.ScoreRecieve);
			return;
		}
		this.position.Value = base.transform.position;
		this.velocity.Value = Vector3.zero;
	}

	// Token: 0x06001030 RID: 4144 RVA: 0x0007F5E4 File Offset: 0x0007D7E4
	protected override void Start()
	{
		base.Start();
		this.minigameController = (RunnerController)GameManager.Minigame;
		if (this.minigameController != null)
		{
			this.minigameController.AddPlayer(this);
			this.movement = this.minigameController.Root.GetComponentInChildren<RunnerMovement>();
			if (this.movement != null)
			{
				base.transform.rotation = Quaternion.identity;
			}
			else
			{
				Debug.LogError("Movement is null..?");
			}
		}
		this.m_broomStartPos = this.m_broom.transform.localPosition;
		this.m_broomStartRot = this.m_broom.transform.localRotation;
		this.m_aiUniqueOffset = UnityEngine.Random.onUnitSphere * 0.1f;
		this.m_aiTargetPosition = this.m_centerTransform.position - new Vector3(100f, 0f, 0f);
	}

	// Token: 0x06001031 RID: 4145 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Awake()
	{
	}

	// Token: 0x06001032 RID: 4146 RVA: 0x0007F6CC File Offset: 0x0007D8CC
	private void OnTriggerEnter(Collider other)
	{
		if (base.IsOwner)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("MinigameUtil1"))
			{
				this.KillPlayer();
				return;
			}
			if (other.gameObject.layer == LayerMask.NameToLayer("MinigameUtil2"))
			{
				int instanceID = other.gameObject.GetInstanceID();
				if (!this.m_hitCollectables.Contains(instanceID))
				{
					this.m_hitCollectables.Add(instanceID);
					this.GainCollectable();
				}
			}
		}
	}

	// Token: 0x06001033 RID: 4147 RVA: 0x0007F744 File Offset: 0x0007D944
	private void Update()
	{
		if (!this.m_isAlive)
		{
			return;
		}
		Player player = null;
		float num = 0f;
		float num2 = 0f;
		if (base.IsOwner)
		{
			if (!this.player.IsAI)
			{
				if (this.player != null)
				{
					player = this.player.RewiredPlayer;
				}
				if (player != null)
				{
					num = this.player.RewiredPlayer.GetAxis(InputActions.Horizontal);
					num2 = this.player.RewiredPlayer.GetAxis(InputActions.Vertical);
				}
			}
			else
			{
				try
				{
					if (this.m_centerTransform.position.x < this.m_aiTargetPosition.x || !this.m_hasGottenTarget)
					{
						RunnerAITargetNode bestNode = RunnerAITargetNode.GetBestNode(this.m_centerTransform.position, this.player.Difficulty != BotDifficulty.Hard);
						if (bestNode != null)
						{
							this.m_aiTargetPosition = bestNode.transform.position + this.m_aiUniqueOffset;
						}
						else
						{
							this.m_aiTargetPosition = this.m_centerTransform.position + this.m_aiUniqueOffset;
						}
						this.m_hasGottenTarget = true;
					}
					Debug.DrawLine(this.m_centerTransform.position, this.m_aiTargetPosition, Color.yellow);
					float num3 = 0f;
					Vector3 a = this.m_centerTransform.position;
					Vector3 aiTargetPosition = this.m_aiTargetPosition;
					a.x = 0f;
					aiTargetPosition.x = 0f;
					float num4 = Vector3.Distance(a, aiTargetPosition);
					if (Mathf.Abs(this.m_aiTargetPosition.x - this.m_centerTransform.position.x) < 30f)
					{
						if (num4 > 0.5f)
						{
							num3 = 0.75f;
						}
						else if (num4 > 0.25f)
						{
							num3 = 0.5f;
						}
						else if (num4 > 0.15f)
						{
							num3 = 0.25f;
						}
					}
					if (this.m_centerTransform.position.z < this.m_aiTargetPosition.z)
					{
						num = num3;
					}
					if (this.m_centerTransform.position.z > this.m_aiTargetPosition.z)
					{
						num = -num3;
					}
					if (this.m_centerTransform.position.y > this.m_aiTargetPosition.y)
					{
						num2 = -num3;
					}
					if (this.m_centerTransform.position.y < this.m_aiTargetPosition.y)
					{
						num2 = num3;
					}
				}
				catch (Exception ex)
				{
					Debug.LogError("sorcers sprint error : " + ex.Message);
				}
			}
		}
		if (base.IsOwner)
		{
			Vector3 vector = base.transform.position;
			if (vector.z <= -4.5f && num < 0f)
			{
				num = 0f;
				this.m_velocity.z = 0f;
			}
			if (vector.z >= 4.5f && num > 0f)
			{
				num = 0f;
				this.m_velocity.z = 0f;
			}
			if (vector.y <= 0f && num2 < 0f)
			{
				num2 = 0f;
				this.m_velocity.y = 0f;
			}
			if (vector.y >= 3.5f && num2 > 0f)
			{
				num2 = 0f;
				this.m_velocity.y = 0f;
			}
			if (Mathf.Abs(num) < 0.01f && Mathf.Abs(num2) < 0.01f)
			{
				if (this.player.IsAI)
				{
					this.m_velocity = Vector3.SmoothDamp(this.m_velocity, Vector3.zero, ref this.m_smoothDampVelocity, 0.05f);
				}
				else
				{
					this.m_velocity = Vector3.SmoothDamp(this.m_velocity, Vector3.zero, ref this.m_smoothDampVelocity, 0.1f);
				}
			}
			Vector3 b = new Vector3(0f, num2, num) * Time.deltaTime;
			b.Scale(this.m_movementSpeed);
			this.m_velocity += b;
			this.m_velocity = Vector3.ClampMagnitude(this.m_velocity, this.m_maxSpeed);
		}
		else if (!this.recievedVelocity)
		{
			this.m_velocity = Vector3.SmoothDamp(this.m_velocity, Vector3.zero, ref this.m_smoothDampVelocity, 0.15f);
		}
		base.transform.position += this.m_velocity * Time.deltaTime;
		this.m_broom.transform.localPosition = this.m_broomStartPos + Vector3.up * this.m_broomBounceCurve.Evaluate(Time.time * this.m_broomBoundSpeed);
		this.m_broom.transform.localRotation = this.m_broomStartRot * Quaternion.Euler(this.m_velocity.y / this.m_maxSpeed * -15f, this.m_velocity.z / this.m_maxSpeed * -15f, 0f);
		Vector3 vector2 = base.transform.position;
		vector2.z = Mathf.Clamp(vector2.z, -4.5f, 4.5f);
		vector2.y = Mathf.Clamp(vector2.y, 0f, 3.5f);
		base.transform.position = vector2;
		if (this.movement != null)
		{
			base.transform.position = new Vector3(this.movement.transform.position.x, base.transform.position.y, base.transform.position.z);
		}
		if (base.IsOwner)
		{
			this.position.Value = new Vector2(base.transform.position.z, base.transform.position.y);
			this.velocity.Value = new Vector2(this.m_velocity.z, this.m_velocity.y);
		}
	}

	// Token: 0x06001034 RID: 4148 RVA: 0x0007FD30 File Offset: 0x0007DF30
	private void KillPlayer()
	{
		if (base.IsOwner)
		{
			base.SendRPC("KillPlayerRPC", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		if (NetSystem.IsServer)
		{
			this.minigameController.PlayerDied(this);
		}
		this.m_isAlive = false;
		Vector3 vector = new Vector3(-10f, 0f, 0f) + Vector3.up * 1f * 4f;
		PlayerCosmetics componentInChildren = base.GetComponentInChildren<PlayerCosmetics>();
		base.StartCoroutine(this.SpawnRagdoll(componentInChildren));
		this.m_broom.transform.parent = null;
		this.m_broom.AddComponent<BoxCollider>();
		Rigidbody rigidbody = this.m_broom.AddComponent<Rigidbody>();
		rigidbody.velocity = vector;
		rigidbody.angularVelocity = UnityEngine.Random.insideUnitSphere * 45f;
		UnityEngine.Object.Destroy(this.m_broom, 4f);
		this.m_playerRoot.SetActive(false);
		AudioSystem.PlayOneShot(this.m_sfxKillPlayer, 1f, 0.1f, 1f);
		AudioSystem.PlayOneShot(this.m_sfxKillerPlayer2, 0.7f, 0.1f, 1f);
		if (Settings.BloodEffects)
		{
			ParticleSystem component = UnityEngine.Object.Instantiate<GameObject>(this.playerDeathEffect, base.MidPoint, Quaternion.identity).GetComponent<ParticleSystem>();
			ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = component.velocityOverLifetime;
			velocityOverLifetime.enabled = true;
			velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
			Vector3 midPoint = base.MidPoint;
			velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(-50f, -20f);
			velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(5f, 10f);
			velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f, 0f);
			ParticleSystem.EmissionModule emission = component.emission;
			ParticleSystem.Burst[] array = new ParticleSystem.Burst[emission.burstCount];
			emission.GetBursts(array);
			array[0].maxCount = (short)((float)array[0].maxCount * 1.5f);
			array[0].minCount = (short)((float)array[0].minCount * 1.5f);
			emission.SetBursts(array);
		}
	}

	// Token: 0x06001035 RID: 4149 RVA: 0x0000DB03 File Offset: 0x0000BD03
	private IEnumerator SpawnRagdoll(PlayerCosmetics cosmetics)
	{
		yield return null;
		Vector3 force = new Vector3(-15f, 0f, 0f) + Vector3.up * 2f * 4f;
		cosmetics.SpawnRagdoll(force);
		yield break;
	}

	// Token: 0x06001036 RID: 4150 RVA: 0x0000DB12 File Offset: 0x0000BD12
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void KillPlayerRPC(NetPlayer sender)
	{
		this.KillPlayer();
	}

	// Token: 0x06001037 RID: 4151 RVA: 0x0007FF3C File Offset: 0x0007E13C
	private void GainCollectable()
	{
		if (NetSystem.IsServer)
		{
			this.Score += 1;
		}
		if (base.IsOwner)
		{
			base.SendRPC("GainCollectableRPC", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		AudioSystem.PlayOneShot(this.m_sfxGainCollectable, 1f, 0.1f, 1f);
		UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate<GameObject>(this.m_gainCollectable, this.m_centerTransform.position, Quaternion.identity), 5f);
	}

	// Token: 0x06001038 RID: 4152 RVA: 0x0000DB1A File Offset: 0x0000BD1A
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void GainCollectableRPC(NetPlayer sender)
	{
		this.GainCollectable();
	}

	// Token: 0x06001039 RID: 4153 RVA: 0x0007FFB8 File Offset: 0x0007E1B8
	public void RecievePosition(object _pos)
	{
		if (this == null || base.transform == null)
		{
			return;
		}
		this.gotPosition = true;
		Vector2 vector = (Vector2)_pos;
		base.transform.position = new Vector3(base.transform.position.x, vector.y, vector.x);
	}

	// Token: 0x0600103A RID: 4154 RVA: 0x00080018 File Offset: 0x0007E218
	public void RecieveVelocity(object _vel)
	{
		Vector2 vector = (Vector2)_vel;
		this.m_velocity = new Vector3(0f, vector.y, vector.x);
		this.recievedVelocity = true;
	}

	// Token: 0x04001078 RID: 4216
	[SerializeField]
	protected Vector3 m_movementSpeed;

	// Token: 0x04001079 RID: 4217
	[SerializeField]
	protected float m_deccelerationSpeed = 0.95f;

	// Token: 0x0400107A RID: 4218
	[SerializeField]
	protected float m_maxSpeed = 5f;

	// Token: 0x0400107B RID: 4219
	[SerializeField]
	protected GameObject m_gainCollectable;

	// Token: 0x0400107C RID: 4220
	[SerializeField]
	protected Transform m_centerTransform;

	// Token: 0x0400107D RID: 4221
	[SerializeField]
	protected GameObject m_playerRoot;

	// Token: 0x0400107E RID: 4222
	[Header("Broom Bounce")]
	[SerializeField]
	protected float m_broomBoundSpeed = 3f;

	// Token: 0x0400107F RID: 4223
	[SerializeField]
	protected AnimationCurve m_broomBounceCurve;

	// Token: 0x04001080 RID: 4224
	[SerializeField]
	protected GameObject m_broom;

	// Token: 0x04001081 RID: 4225
	[Header("SFX")]
	[SerializeField]
	protected AudioClip m_sfxGainCollectable;

	// Token: 0x04001082 RID: 4226
	[SerializeField]
	protected AudioClip m_sfxKillPlayer;

	// Token: 0x04001083 RID: 4227
	[SerializeField]
	protected AudioClip m_sfxKillerPlayer2;

	// Token: 0x04001084 RID: 4228
	[SerializeField]
	protected GameObject playerDeathEffect;

	// Token: 0x04001085 RID: 4229
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec2 position = new NetVec2(Vector2.zero);

	// Token: 0x04001086 RID: 4230
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec2 velocity = new NetVec2(Vector2.zero);

	// Token: 0x04001087 RID: 4231
	private bool gotPosition;

	// Token: 0x04001088 RID: 4232
	private RunnerController minigameController;

	// Token: 0x04001089 RID: 4233
	private Vector3 m_broomStartPos;

	// Token: 0x0400108A RID: 4234
	private Quaternion m_broomStartRot;

	// Token: 0x0400108B RID: 4235
	private Vector3 m_velocity = Vector3.zero;

	// Token: 0x0400108C RID: 4236
	private float m_nextAIUpdate;

	// Token: 0x0400108D RID: 4237
	private bool m_isAlive = true;

	// Token: 0x0400108E RID: 4238
	private RunnerMovement movement;

	// Token: 0x0400108F RID: 4239
	private HashSet<int> m_hitCollectables = new HashSet<int>();

	// Token: 0x04001090 RID: 4240
	private Vector3 m_smoothDampVelocity;

	// Token: 0x04001091 RID: 4241
	private Vector3 m_aiTargetPosition;

	// Token: 0x04001092 RID: 4242
	private Vector3 m_aiUniqueOffset;

	// Token: 0x04001093 RID: 4243
	private bool m_hasGottenTarget;

	// Token: 0x04001094 RID: 4244
	private bool recievedVelocity;
}
