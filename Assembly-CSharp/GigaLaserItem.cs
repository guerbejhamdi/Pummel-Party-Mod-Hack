using System;
using System.Collections;
using LlockhamIndustries.Decals;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200036F RID: 879
public class GigaLaserItem : Item
{
	// Token: 0x1700023C RID: 572
	// (get) Token: 0x060017A4 RID: 6052 RVA: 0x000119C7 File Offset: 0x0000FBC7
	// (set) Token: 0x060017A5 RID: 6053 RVA: 0x000A3F74 File Offset: 0x000A2174
	public byte TargetID
	{
		get
		{
			return this.targetID.Value;
		}
		set
		{
			if (base.IsOwner)
			{
				this.targetID.Value = value;
			}
			Transform transform = GameManager.Board.GetActor(this.targetID.Value).transform;
			GameManager.Board.boardCamera.SetTrackedObject(transform, GameManager.Board.PlayerCamOffset);
			this.m_targeterObj.transform.position = transform.position - new Vector3(0f, 0.75f, 0f);
			if (this.m_laser != null)
			{
				this.m_laser.SetTargetPosition(transform.position + this.m_targetOffset);
			}
		}
	}

	// Token: 0x060017A6 RID: 6054 RVA: 0x00003C5A File Offset: 0x00001E5A
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
	}

	// Token: 0x060017A7 RID: 6055 RVA: 0x000A4024 File Offset: 0x000A2224
	public override void Setup()
	{
		base.Setup();
		base.GetAITarget();
		this.m_targeterObj = UnityEngine.Object.Instantiate<GameObject>(this.m_targeterPrefab, Vector3.zero, Quaternion.Euler(90f, 0f, 0f));
		Vector3 position = this.player.BoardObject.transform.position - this.player.BoardObject.transform.forward * 1.25f;
		this.m_laserObj = UnityEngine.Object.Instantiate<GameObject>(this.m_gigaLaserPrefab, position, Quaternion.Euler(0f, 0f, 0f));
		this.m_laser = this.m_laserObj.GetComponentInChildren<GigaLaserHelper>();
		this.m_setupTime = Time.time;
		this.TargetID = ((this.player.GlobalID == 0) ? 1 : 0);
		this.targetID.Recieve = new RecieveProxy(this.TargetReceive);
		base.SetNetworkState(Item.ItemState.Setup);
	}

	// Token: 0x060017A8 RID: 6056 RVA: 0x000A411C File Offset: 0x000A231C
	public override void Update()
	{
		if (this.curState == Item.ItemState.Aiming)
		{
			if (base.IsOwner)
			{
				if (this.player.IsAI)
				{
					if (!this.foundTarget)
					{
						if (this.aiTimer.Elapsed(true))
						{
							if (this.AITarget.ActorID == this.TargetID)
							{
								this.foundTarget = true;
								this.aiTimer = new ActionTimer(1.5f);
								this.aiTimer.Start();
							}
							else
							{
								this.TargetID = this.Increment(true, this.TargetID);
							}
						}
					}
					else if (this.aiTimer.Elapsed(true))
					{
						base.AIUseItem();
					}
				}
				else if (!GameManager.IsGamePaused)
				{
					if (this.player.RewiredPlayer.GetNegativeButtonDown(InputActions.Horizontal))
					{
						this.TargetID = this.Increment(true, this.TargetID);
					}
					else if (this.player.RewiredPlayer.GetButtonDown(InputActions.Horizontal))
					{
						this.TargetID = this.Increment(false, this.TargetID);
					}
				}
			}
			if (Time.time - this.m_setupTime > 1f && !this.m_laserDone && this.curState != Item.ItemState.Cancelled && this.m_laser != null)
			{
				byte value = this.targetID.Value;
				BoardActor actor = GameManager.Board.GetActor(value);
				if (actor != null)
				{
					this.m_laser.SetTargetPosition(actor.transform.position + this.m_targetOffset);
				}
			}
		}
		base.Update();
	}

	// Token: 0x060017A9 RID: 6057 RVA: 0x000A42A4 File Offset: 0x000A24A4
	private byte Increment(bool left, byte curID)
	{
		if (left)
		{
			curID = ((curID == 0) ? ((byte)(GameManager.Board.GetActorCount() - 1)) : (curID - 1));
		}
		else
		{
			curID = (((int)curID == GameManager.Board.GetActorCount() - 1) ? 0 : (curID + 1));
		}
		if ((short)curID == this.player.GlobalID || GameManager.Board.GetActor(curID).LocalHealth <= 0)
		{
			return this.Increment(left, curID);
		}
		return curID;
	}

	// Token: 0x060017AA RID: 6058 RVA: 0x000A4314 File Offset: 0x000A2514
	protected override void Use(int seed)
	{
		base.Use(seed);
		byte b = (byte)this.rand.Next(9, 12);
		float num = ZPMath.RandomFloat(this.rand, 15f, 165f);
		base.SendRPC("RPCUseGigaLaser", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			this.TargetID,
			b,
			num
		});
		base.StartCoroutine(this.UseGigaLaser(this.TargetID, b, num));
	}

	// Token: 0x060017AB RID: 6059 RVA: 0x000119D4 File Offset: 0x0000FBD4
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCUseGigaLaser(NetPlayer sender, byte playerID, byte damage, float yRot)
	{
		base.Use(0);
		base.StartCoroutine(this.UseGigaLaser(playerID, damage, yRot));
	}

	// Token: 0x060017AC RID: 6060 RVA: 0x000119EE File Offset: 0x0000FBEE
	private IEnumerator UseGigaLaser(byte playerID, byte damage, float yRot)
	{
		this.useTarget = playerID;
		this.useDamage = damage;
		this.DespawnTargeter();
		this.cam = GameManager.Board.Camera;
		this.m_sun = RenderSettings.sun;
		this.m_lightStartColor = this.m_sun.color;
		this.m_lightStartIntensity = this.m_sun.intensity;
		this.m_ambStartSkyColor = RenderSettings.ambientSkyColor;
		this.m_ambStartEquatorColor = RenderSettings.ambientEquatorColor;
		this.m_ambStartGroundColor = RenderSettings.ambientGroundColor;
		this.m_startAmbientIntensity = RenderSettings.ambientIntensity;
		this.m_startReflectionIntensity = RenderSettings.reflectionIntensity;
		Vector3 vector = GameManager.Board.GetActor(playerID).transform.position - this.m_laserObj.transform.position;
		float magnitude = vector.magnitude;
		GameManager.Board.boardCamera.SetTrackedObject(null, Vector3.zero);
		Vector3 b = vector.normalized * Mathf.Clamp(magnitude, magnitude / 2f, 2.5f);
		GameManager.Board.boardCamera.MoveTo(this.m_laserObj.transform.position + GameManager.Board.PlayerCamOffset + b);
		yield return new WaitUntil(() => GameManager.Board.boardCamera.WithinDistance(0.5f));
		AudioSystem.PlayOneShot(this.m_fireAudio, 1f, 0f, 1f);
		this.m_laser.Fire();
		float startTime = Time.time;
		bool hasFired = false;
		while (Time.time - startTime < this.m_fireTime)
		{
			float num = Mathf.Clamp01((Time.time - startTime) / this.m_fireTime);
			this.SetLighting(this.m_fireLightBrightnessCurve.Evaluate(num));
			if (num > 0.35f && !hasFired)
			{
				this.GigaLaserHit();
				hasFired = true;
			}
			yield return null;
		}
		this.m_laserDone = true;
		base.Finish(false);
		yield break;
	}

	// Token: 0x060017AD RID: 6061 RVA: 0x000A4398 File Offset: 0x000A2598
	public void GigaLaserHit()
	{
		Vector3 position = this.m_laserObj.transform.position;
		DamageInstance d = new DamageInstance
		{
			damage = 30,
			origin = position,
			blood = true,
			ragdoll = true,
			ragdollVel = 15f,
			bloodVel = 13f,
			bloodAmount = 1f,
			sound = true,
			volume = 0.75f,
			details = "Giga Laser",
			killer = this.player.BoardObject,
			removeKeys = true
		};
		GameManager.Board.GetActor(this.useTarget).ApplyDamage(d);
		this.cam.AddShake(0.5f);
	}

	// Token: 0x060017AE RID: 6062 RVA: 0x000A4468 File Offset: 0x000A2668
	private void SetLighting(float t)
	{
		this.m_sun.color = this.m_lightStartColor * t;
		this.m_sun.intensity = this.m_lightStartIntensity * t;
		RenderSettings.ambientSkyColor = this.m_ambStartSkyColor * t;
		RenderSettings.ambientEquatorColor = this.m_ambStartEquatorColor * t;
		RenderSettings.ambientGroundColor = this.m_ambStartGroundColor * t;
		RenderSettings.ambientIntensity = this.m_startAmbientIntensity * t;
		RenderSettings.reflectionIntensity = this.m_startAmbientIntensity * t;
	}

	// Token: 0x060017AF RID: 6063 RVA: 0x00011A0B File Offset: 0x0000FC0B
	public override void Unequip(bool endingTurn)
	{
		this.DespawnTargeter();
		GameManager.Board.CameraTrackCurrentPlayer();
		this.m_laser.Despawn();
		UnityEngine.Object.Destroy(this.m_laserObj, 3f);
		base.Unequip(endingTurn);
	}

	// Token: 0x060017B0 RID: 6064 RVA: 0x000A44EC File Offset: 0x000A26EC
	private void DespawnTargeter()
	{
		if (this.m_targeterObj == null)
		{
			return;
		}
		foreach (Fade fade in this.m_targeterObj.GetComponentsInChildren<Fade>())
		{
			GameObject gameObject = fade.gameObject;
			UnityEngine.Object.Destroy(fade);
			Fade fade2 = gameObject.AddComponent<Fade>();
			fade2.type = LlockhamIndustries.Decals.FadeType.Scale;
			fade2.wrapMode = FadeWrapMode.Once;
			fade2.fadeLength = 0.5f;
			fade2.fade = this.targetDespawnCurve;
		}
	}

	// Token: 0x060017B1 RID: 6065 RVA: 0x00011A3F File Offset: 0x0000FC3F
	public void TargetReceive(object val)
	{
		this.TargetID = (byte)val;
	}

	// Token: 0x060017B2 RID: 6066 RVA: 0x000A455C File Offset: 0x000A275C
	public override ItemAIUse GetTarget(BoardPlayer user)
	{
		ItemAIUse result = null;
		int num = int.MaxValue;
		for (int i = 0; i < GameManager.Board.GetActorCount(); i++)
		{
			BoardActor actor = GameManager.Board.GetActor((byte)i);
			if (!(actor == user) && actor.LocalHealth > 0)
			{
				short num2 = actor.LocalHealth;
				if (actor.GetType() == typeof(BoardPlayer) && user.GamePlayer.IsAI && !((BoardPlayer)actor).GamePlayer.IsAI)
				{
					num2 += this.difficultyDistanceChange[(int)user.GamePlayer.Difficulty];
				}
				if ((int)num2 < num)
				{
					num = (int)actor.LocalHealth;
					result = new ItemAIUse(actor, 1f);
				}
			}
		}
		return result;
	}

	// Token: 0x0400191F RID: 6431
	[SerializeField]
	private GameObject m_gigaLaserPrefab;

	// Token: 0x04001920 RID: 6432
	[SerializeField]
	private GameObject m_targeterPrefab;

	// Token: 0x04001921 RID: 6433
	[SerializeField]
	private AnimationCurve targetDespawnCurve;

	// Token: 0x04001922 RID: 6434
	[SerializeField]
	private float m_fireTime;

	// Token: 0x04001923 RID: 6435
	[SerializeField]
	private AnimationCurve m_fireLightBrightnessCurve;

	// Token: 0x04001924 RID: 6436
	[SerializeField]
	private AudioClip m_fireAudio;

	// Token: 0x04001925 RID: 6437
	private GameObject m_targeterObj;

	// Token: 0x04001926 RID: 6438
	private WreckingBallVisual wreckingBallVisualScript;

	// Token: 0x04001927 RID: 6439
	private byte useTarget;

	// Token: 0x04001928 RID: 6440
	private byte useDamage;

	// Token: 0x04001929 RID: 6441
	private ActionTimer aiTimer = new ActionTimer(0.5f);

	// Token: 0x0400192A RID: 6442
	private bool foundTarget;

	// Token: 0x0400192B RID: 6443
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<byte> targetID = new NetVar<byte>(0);

	// Token: 0x0400192C RID: 6444
	private GameObject m_laserObj;

	// Token: 0x0400192D RID: 6445
	private GigaLaserHelper m_laser;

	// Token: 0x0400192E RID: 6446
	private Vector3 m_targetOffset = new Vector3(0f, 0.75f, 0f);

	// Token: 0x0400192F RID: 6447
	private float m_setupTime;

	// Token: 0x04001930 RID: 6448
	private Color m_lightStartColor;

	// Token: 0x04001931 RID: 6449
	private float m_lightStartIntensity;

	// Token: 0x04001932 RID: 6450
	private Color m_ambStartSkyColor;

	// Token: 0x04001933 RID: 6451
	private Color m_ambStartEquatorColor;

	// Token: 0x04001934 RID: 6452
	private Color m_ambStartGroundColor;

	// Token: 0x04001935 RID: 6453
	private float m_startAmbientIntensity;

	// Token: 0x04001936 RID: 6454
	private float m_startReflectionIntensity;

	// Token: 0x04001937 RID: 6455
	private Light m_sun;

	// Token: 0x04001938 RID: 6456
	private GameBoardCamera cam;

	// Token: 0x04001939 RID: 6457
	private bool m_laserDone;

	// Token: 0x0400193A RID: 6458
	private short[] difficultyDistanceChange = new short[]
	{
		12,
		6,
		3
	};
}
