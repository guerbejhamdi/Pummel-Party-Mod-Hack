using System;
using UnityEngine;
using ZP.Net;

// Token: 0x02000338 RID: 824
public class BoardActor : NetBehaviour
{
	// Token: 0x170001FE RID: 510
	// (get) Token: 0x06001661 RID: 5729 RVA: 0x00010E21 File Offset: 0x0000F021
	// (set) Token: 0x06001662 RID: 5730 RVA: 0x00010E3C File Offset: 0x0000F03C
	protected virtual short ProxyHealth
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

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x06001663 RID: 5731 RVA: 0x00010E58 File Offset: 0x0000F058
	public short ServerHealth
	{
		get
		{
			return this.health.Value;
		}
	}

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x06001664 RID: 5732 RVA: 0x00010E65 File Offset: 0x0000F065
	public short LocalHealth
	{
		get
		{
			return this.proxyHealth;
		}
	}

	// Token: 0x17000201 RID: 513
	// (get) Token: 0x06001665 RID: 5733 RVA: 0x00010E6D File Offset: 0x0000F06D
	// (set) Token: 0x06001666 RID: 5734 RVA: 0x00010E75 File Offset: 0x0000F075
	public byte ActorID { get; set; }

	// Token: 0x06001667 RID: 5735 RVA: 0x0009F248 File Offset: 0x0009D448
	public override void OnNetInitialize()
	{
		this.proxyHealth = this.maxHealth;
		byte b = GameManager.Board.RegisterActor(this);
		if (!NetSystem.IsServer)
		{
			this.health.Recieve = new RecieveProxy(this.RecieveHealth);
		}
		else
		{
			this.ActorID = b;
			base.SendRPC("RPCSetActorID", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				b
			});
		}
		base.OnNetInitialize();
	}

	// Token: 0x06001668 RID: 5736 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Initialize()
	{
	}

	// Token: 0x06001669 RID: 5737 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x17000202 RID: 514
	// (get) Token: 0x0600166A RID: 5738 RVA: 0x00010E7E File Offset: 0x0000F07E
	public virtual Vector3 MidPoint
	{
		get
		{
			return base.transform.position + Vector3.up;
		}
	}

	// Token: 0x0600166B RID: 5739 RVA: 0x00010E95 File Offset: 0x0000F095
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSetActorID(NetPlayer sender, byte actorID)
	{
		this.ActorID = actorID;
	}

	// Token: 0x0600166C RID: 5740 RVA: 0x0009F2B8 File Offset: 0x0009D4B8
	public virtual void ApplyDamage(DamageInstance d)
	{
		if (Time.time != this.lastOutputTime)
		{
			string text = "";
			if (d.killer != null)
			{
				text = text + "Source: " + d.killer.OwnerSlot.ToString();
			}
			text = text + " Target: " + base.OwnerSlot.ToString();
			text = text + " Damage: " + d.damage.ToString();
			text = text + " Details: " + d.details;
			Debug.Log(text);
			this.lastOutputTime = Time.time;
		}
	}

	// Token: 0x0600166D RID: 5741 RVA: 0x0009F360 File Offset: 0x0009D560
	protected void SpawnBlood(DamageInstance d)
	{
		if (Settings.BloodEffects && d.blood && Time.time - this.lastBlood > this.minBloodInterval)
		{
			float num = 1f;
			float d2 = 1f;
			if (BoardModifier.IsBoardModifierActive(BoardModifierID.BloodBath))
			{
				num = 5f;
				d2 = 2f;
			}
			ParticleSystem component = UnityEngine.Object.Instantiate<GameObject>(this.bloodyDamageEffect, this.MidPoint, Quaternion.identity).GetComponent<ParticleSystem>();
			ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = component.velocityOverLifetime;
			velocityOverLifetime.enabled = true;
			velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
			Vector3 vector = (this.MidPoint - d.origin).normalized * d.bloodVel * d2;
			velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(Mathf.Min(0f, vector.x), Mathf.Max(0f, vector.x));
			velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(Mathf.Min(0f, vector.y), Mathf.Max(0f, vector.y));
			velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(Mathf.Min(0f, vector.z), Mathf.Max(0f, vector.z));
			ParticleSystem.EmissionModule emission = component.emission;
			ParticleSystem.Burst[] array = new ParticleSystem.Burst[emission.burstCount];
			emission.GetBursts(array);
			array[0].maxCount = (short)((float)array[0].maxCount * d.bloodAmount * num);
			array[0].minCount = (short)((float)array[0].minCount * d.bloodAmount * num);
			emission.SetBursts(array);
			this.lastBlood = Time.time;
		}
	}

	// Token: 0x0600166E RID: 5742 RVA: 0x0009F518 File Offset: 0x0009D718
	private void Update()
	{
		if (this.ProxyHealth != this.health.Value)
		{
			this.proxyHealthDifTime += Time.deltaTime;
			if (this.proxyHealthDifTime > 3f && this.healthDifTimer.Elapsed(true))
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Proxy health is different: ",
					this.proxyHealth.ToString(),
					" : ",
					this.health.Value.ToString(),
					" : ",
					this.proxyHealthDifTime.ToString()
				}));
				return;
			}
		}
		else
		{
			this.proxyHealthDifTime = 0f;
		}
	}

	// Token: 0x0600166F RID: 5743 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void RecieveHealth(object health)
	{
	}

	// Token: 0x04001796 RID: 6038
	public short maxHealth = 30;

	// Token: 0x04001797 RID: 6039
	public GameObject bloodyDamageEffect;

	// Token: 0x04001798 RID: 6040
	private short proxyHealth = 30;

	// Token: 0x04001799 RID: 6041
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	protected NetVar<short> health = new NetVar<short>(30);

	// Token: 0x0400179B RID: 6043
	private float proxyHealthDifTime;

	// Token: 0x0400179C RID: 6044
	private ActionTimer healthDifTimer = new ActionTimer(1f);

	// Token: 0x0400179D RID: 6045
	private float lastOutputTime;

	// Token: 0x0400179E RID: 6046
	private float lastBlood;

	// Token: 0x0400179F RID: 6047
	private float minBloodInterval = 0.5f;
}
