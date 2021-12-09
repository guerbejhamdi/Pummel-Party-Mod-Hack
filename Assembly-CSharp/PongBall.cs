using System;
using System.Collections;
using UnityEngine;
using ZP.Net;

// Token: 0x02000200 RID: 512
public class PongBall : NetBehaviour
{
	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06000F0D RID: 3853 RVA: 0x0000D099 File Offset: 0x0000B299
	public Vector3 Velocity
	{
		get
		{
			return this.m_velocity;
		}
	}

	// Token: 0x06000F0E RID: 3854 RVA: 0x000783D8 File Offset: 0x000765D8
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		if (!base.IsOwner)
		{
			this.n_position.Recieve = new RecieveProxy(this.RecievePosition);
			this.n_velocity.Recieve = new RecieveProxy(this.RecieveVelocity);
		}
		else
		{
			this.n_position.Value = base.transform.position;
			this.n_velocity.Value = Vector3.zero;
		}
		if (!NetSystem.IsServer && GameManager.Minigame != null)
		{
			((PongController)GameManager.Minigame).AddBall(this);
		}
	}

	// Token: 0x06000F0F RID: 3855 RVA: 0x00078478 File Offset: 0x00076678
	private void Update()
	{
		if (NetSystem.IsServer)
		{
			if (!this.m_isRespawning)
			{
				if (Time.time >= this.m_spawnEndTime)
				{
					Vector3 vector = base.transform.position;
					vector += this.m_velocity * Mathf.Clamp01(Time.time - this.m_spawnEndTime) * Time.deltaTime;
					float num = Vector3.Distance(base.transform.position, vector);
					Debug.DrawRay(base.transform.position, this.m_velocity.normalized, Color.green);
					RaycastHit[] array = this.DoRayCast(base.transform.position, num);
					if (array.Length == 0)
					{
						base.transform.position = vector;
					}
					else
					{
						int num2 = 0;
						if (array[0].point == Vector3.zero)
						{
							array = this.DoRayCast(base.transform.position - this.m_velocity.normalized * (this.m_radius + 1.1f), num + (this.m_radius + 1.1f));
							Debug.DrawLine(base.transform.position, base.transform.position - this.m_velocity.normalized * (this.m_radius + 0.1f), Color.red, 10f);
							if (array.Length > 1 && array[0].point == Vector3.zero)
							{
								num2 = 1;
							}
						}
						if (array.Length != 0 && array[num2].collider != this.lastHitCollider && array[num2].point != Vector3.zero)
						{
							this.lastHitCollider = array[num2].collider;
							base.transform.position = array[num2].point + array[num2].normal * (this.m_radius * 1.5f);
							PongPlayer componentInParent = array[num2].collider.gameObject.GetComponentInParent<PongPlayer>();
							if (componentInParent != null)
							{
								this.m_velocity = componentInParent.GetBounceDirection(array[num2].point) * this.m_velocity.magnitude;
								componentInParent.OnBallHitWall();
								this.m_lastHitPlayer = componentInParent;
								this.UpdateColor(-1);
								AudioSystem.PlayOneShot(this.m_bounce, 1f, 0.01f, 1f);
								this.m_explodeParticles.Emit(10);
							}
							else
							{
								this.m_velocity = Vector3.Reflect(this.m_velocity.normalized, array[0].normal) * this.m_velocity.magnitude;
							}
						}
						else
						{
							base.transform.position = vector;
						}
					}
				}
				this.n_position.Value = new Vector2(base.transform.position.x, base.transform.position.z);
				this.n_velocity.Value = new Vector2(this.m_velocity.x, this.m_velocity.z);
				return;
			}
		}
		else if (!this.m_isRespawning)
		{
			if (!this.m_gotPosition)
			{
				base.transform.position += this.m_velocity * Time.deltaTime;
			}
			this.m_gotPosition = false;
		}
	}

	// Token: 0x06000F10 RID: 3856 RVA: 0x0000D0A1 File Offset: 0x0000B2A1
	private RaycastHit[] DoRayCast(Vector3 pos, float dist)
	{
		return Physics.SphereCastAll(pos, this.m_radius, this.m_velocity.normalized, dist, LayerMask.GetMask(new string[]
		{
			"MinigameUtil1"
		}), QueryTriggerInteraction.Ignore);
	}

	// Token: 0x06000F11 RID: 3857 RVA: 0x000787E8 File Offset: 0x000769E8
	public void OnHitGoal(int playerIndex)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCHitGoal", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				playerIndex
			});
			if (this.m_lastHitPlayer != null)
			{
				PongPlayer lastHitPlayer = this.m_lastHitPlayer;
				short score = lastHitPlayer.Score;
				lastHitPlayer.Score = score + 1;
				this.m_lastHitPlayer = null;
			}
		}
		base.StartCoroutine(this.Respawn());
		this.m_explodeParticles.Emit(100);
		AudioSystem.PlayOneShot(this.m_scoreClip, 1f, 0.1f, 1f);
		this.lastHitCollider = null;
	}

	// Token: 0x06000F12 RID: 3858 RVA: 0x0000D0CF File Offset: 0x0000B2CF
	private IEnumerator Respawn()
	{
		this.m_isRespawning = true;
		this.m_graphic.SetActive(false);
		yield return null;
		base.transform.position = new Vector3(100f, 100f, 100f);
		yield return new WaitForSeconds(0.5f);
		this.UpdateColor(-1);
		this.m_isRespawning = false;
		this.m_graphic.SetActive(true);
		base.transform.position = new Vector3(0f, 0.5f, 0f);
		if (NetSystem.IsServer)
		{
			Vector3 onUnitSphere = UnityEngine.Random.onUnitSphere;
			onUnitSphere.y = 0f;
			this.Init(onUnitSphere.normalized * 7f);
		}
		yield break;
	}

	// Token: 0x06000F13 RID: 3859 RVA: 0x00078880 File Offset: 0x00076A80
	private void UpdateColor(int colorIndex = -1)
	{
		if (colorIndex == -1)
		{
			if (this.m_lastHitPlayer != null)
			{
				colorIndex = (int)(this.m_lastHitPlayer.GamePlayer.ColorIndex + 1);
			}
			else
			{
				colorIndex = 0;
			}
		}
		Color color = this.m_colors[colorIndex];
		this.m_light.color = color;
		this.m_ballRenderer.material.SetColor("_Color", color * 16f);
		foreach (ParticleSystemRenderer particleSystemRenderer in this.m_systems)
		{
			particleSystemRenderer.sharedMaterial = this.m_particleMaterials[colorIndex];
			particleSystemRenderer.trailMaterial = this.m_particleMaterials[colorIndex];
		}
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSetColor", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				(byte)colorIndex
			});
		}
	}

	// Token: 0x06000F14 RID: 3860 RVA: 0x0000D0DE File Offset: 0x0000B2DE
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSetColor(NetPlayer sender, byte colorIndex)
	{
		this.UpdateColor((int)colorIndex);
		AudioSystem.PlayOneShot(this.m_bounce, 1f, 0.01f, 1f);
		this.m_explodeParticles.Emit(10);
	}

	// Token: 0x06000F15 RID: 3861 RVA: 0x0000D10E File Offset: 0x0000B30E
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCHitGoal(NetPlayer sender, int playerIndex)
	{
		this.OnHitGoal(playerIndex);
	}

	// Token: 0x06000F16 RID: 3862 RVA: 0x0000D117 File Offset: 0x0000B317
	public void Init(Vector3 startVelocity)
	{
		this.m_velocity = startVelocity;
		this.m_spawnEndTime = Time.time + 1f;
	}

	// Token: 0x06000F17 RID: 3863 RVA: 0x00078948 File Offset: 0x00076B48
	public void RecievePosition(object _pos)
	{
		if (this == null || base.transform == null)
		{
			return;
		}
		this.m_gotPosition = true;
		Vector2 vector = (Vector2)_pos;
		base.transform.position = new Vector3(vector.x, base.transform.position.y, vector.y);
	}

	// Token: 0x06000F18 RID: 3864 RVA: 0x000789A8 File Offset: 0x00076BA8
	public void RecieveVelocity(object _vel)
	{
		Vector2 vector = (Vector2)_vel;
		this.m_velocity = new Vector3(vector.x, 0f, vector.y);
		this.recievedVelocity = true;
	}

	// Token: 0x04000ED4 RID: 3796
	[SerializeField]
	protected float m_radius = 0.3f;

	// Token: 0x04000ED5 RID: 3797
	[SerializeField]
	protected GameObject m_graphic;

	// Token: 0x04000ED6 RID: 3798
	[SerializeField]
	protected ParticleSystemRenderer[] m_systems;

	// Token: 0x04000ED7 RID: 3799
	[SerializeField]
	protected Material[] m_particleMaterials;

	// Token: 0x04000ED8 RID: 3800
	[SerializeField]
	protected Light m_light;

	// Token: 0x04000ED9 RID: 3801
	[SerializeField]
	protected MeshRenderer m_ballRenderer;

	// Token: 0x04000EDA RID: 3802
	[SerializeField]
	protected ParticleSystem m_explodeParticles;

	// Token: 0x04000EDB RID: 3803
	[SerializeField]
	protected Color[] m_colors;

	// Token: 0x04000EDC RID: 3804
	[SerializeField]
	protected AudioClip m_bounce;

	// Token: 0x04000EDD RID: 3805
	[SerializeField]
	protected AudioClip m_scoreClip;

	// Token: 0x04000EDE RID: 3806
	private Vector3 m_velocity;

	// Token: 0x04000EDF RID: 3807
	private float m_spawnEndTime;

	// Token: 0x04000EE0 RID: 3808
	private PongPlayer m_lastHitPlayer;

	// Token: 0x04000EE1 RID: 3809
	private bool m_isRespawning;

	// Token: 0x04000EE2 RID: 3810
	private bool m_gotPosition;

	// Token: 0x04000EE3 RID: 3811
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.ALWAYS_SEND)]
	public NetVec2 n_position = new NetVec2(Vector2.zero);

	// Token: 0x04000EE4 RID: 3812
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.ALWAYS_SEND)]
	public NetVec2 n_velocity = new NetVec2(Vector2.zero);

	// Token: 0x04000EE5 RID: 3813
	private Collider lastHitCollider;

	// Token: 0x04000EE6 RID: 3814
	private bool recievedVelocity;
}
