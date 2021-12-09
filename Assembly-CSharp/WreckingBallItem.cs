using System;
using System.Collections;
using LlockhamIndustries.Decals;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200059C RID: 1436
public class WreckingBallItem : Item
{
	// Token: 0x17000468 RID: 1128
	// (get) Token: 0x06002553 RID: 9555 RVA: 0x0001ABB0 File Offset: 0x00018DB0
	// (set) Token: 0x06002554 RID: 9556 RVA: 0x000E1BAC File Offset: 0x000DFDAC
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
			this.wreckingBallTargeterObject.transform.position = transform.position - new Vector3(0f, 0.75f, 0f);
		}
	}

	// Token: 0x06002555 RID: 9557 RVA: 0x00003C5A File Offset: 0x00001E5A
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
	}

	// Token: 0x06002556 RID: 9558 RVA: 0x000E1C34 File Offset: 0x000DFE34
	public override void Setup()
	{
		base.Setup();
		base.GetAITarget();
		this.player.BoardObject.PlayerAnimation.Carrying = true;
		this.wreckingBallTargeterObject = UnityEngine.Object.Instantiate<GameObject>(this.wreckingBallTargeter, Vector3.zero, Quaternion.Euler(90f, 0f, 0f));
		this.TargetID = ((this.player.GlobalID == 0) ? 1 : 0);
		this.targetID.Recieve = new RecieveProxy(this.TargetReceive);
		base.SetNetworkState(Item.ItemState.Setup);
	}

	// Token: 0x06002557 RID: 9559 RVA: 0x000E1CC4 File Offset: 0x000DFEC4
	public override void Update()
	{
		if (this.curState == Item.ItemState.Aiming && base.IsOwner)
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
		base.Update();
	}

	// Token: 0x06002558 RID: 9560 RVA: 0x000E1DD8 File Offset: 0x000DFFD8
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

	// Token: 0x06002559 RID: 9561 RVA: 0x000E1E48 File Offset: 0x000E0048
	protected override void Use(int seed)
	{
		base.Use(seed);
		byte b = (byte)this.rand.Next(9, 12);
		float num = ZPMath.RandomFloat(this.rand, 15f, 165f);
		base.SendRPC("RPCUseWreckingBall", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			this.TargetID,
			b,
			num
		});
		base.StartCoroutine(this.UseWreckingBall(this.TargetID, b, num));
	}

	// Token: 0x0600255A RID: 9562 RVA: 0x0001ABBD File Offset: 0x00018DBD
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCUseWreckingBall(NetPlayer sender, byte playerID, byte damage, float yRot)
	{
		base.Use(0);
		base.StartCoroutine(this.UseWreckingBall(playerID, damage, yRot));
	}

	// Token: 0x0600255B RID: 9563 RVA: 0x0001ABD7 File Offset: 0x00018DD7
	private IEnumerator UseWreckingBall(byte playerID, byte damage, float yRot)
	{
		this.useTarget = playerID;
		this.useDamage = damage;
		this.DespawnTargeter();
		AudioSystem.PlayOneShot(this.swingClip, 1f, 0f, 1f);
		Vector3 midPoint = GameManager.Board.GetActor(playerID).MidPoint;
		this.wreckingBallVisualScript = UnityEngine.Object.Instantiate<GameObject>(this.wreckingBallVisual, midPoint + new Vector3(0f, 7f, 0f), Quaternion.Euler(0f, yRot, 0f)).GetComponentInChildren<WreckingBallVisual>();
		this.wreckingBallVisualScript.wreckingBallItem = this;
		yield return new WaitForSeconds(0.7f);
		this.WreckingBallHit();
		yield return new WaitForSeconds(0.7666f);
		this.WreckingBallEnd();
		yield break;
	}

	// Token: 0x0600255C RID: 9564 RVA: 0x000E1ECC File Offset: 0x000E00CC
	public void WreckingBallHit()
	{
		DamageInstance d = new DamageInstance
		{
			damage = (int)this.useDamage,
			origin = this.wreckingBallVisualScript.hitPoint.position,
			blood = true,
			ragdoll = true,
			ragdollVel = 15f,
			bloodVel = 13f,
			bloodAmount = 1f,
			sound = true,
			volume = 0.75f,
			details = "Wrecking Ball",
			killer = this.player.BoardObject,
			removeKeys = true
		};
		GameManager.Board.GetActor(this.useTarget).ApplyDamage(d);
		GameManager.Board.boardCamera.AddShake(0.4f);
	}

	// Token: 0x0600255D RID: 9565 RVA: 0x0001ABFB File Offset: 0x00018DFB
	public void WreckingBallEnd()
	{
		UnityEngine.Object.Destroy(this.wreckingBallVisualScript.transform.parent.gameObject);
		base.Finish(false);
	}

	// Token: 0x0600255E RID: 9566 RVA: 0x0001AC1E File Offset: 0x00018E1E
	public override void Unequip(bool endingTurn)
	{
		this.player.BoardObject.PlayerAnimation.Carrying = false;
		this.DespawnTargeter();
		GameManager.Board.CameraTrackCurrentPlayer();
		base.Unequip(endingTurn);
	}

	// Token: 0x0600255F RID: 9567 RVA: 0x000E1FA0 File Offset: 0x000E01A0
	private void DespawnTargeter()
	{
		if (this.wreckingBallTargeterObject == null)
		{
			return;
		}
		UnityEngine.Object.Destroy(this.wreckingBallTargeterObject.GetComponent<Fade>());
		Fade fade = this.wreckingBallTargeterObject.AddComponent<Fade>();
		fade.type = LlockhamIndustries.Decals.FadeType.Scale;
		fade.wrapMode = FadeWrapMode.Once;
		fade.fadeLength = 0.5f;
		fade.fade = this.targetDespawnCurve;
	}

	// Token: 0x06002560 RID: 9568 RVA: 0x0001AC4D File Offset: 0x00018E4D
	public void TargetReceive(object val)
	{
		this.TargetID = (byte)val;
	}

	// Token: 0x06002561 RID: 9569 RVA: 0x000E1FFC File Offset: 0x000E01FC
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

	// Token: 0x040028C9 RID: 10441
	public GameObject wreckingBallVisual;

	// Token: 0x040028CA RID: 10442
	public GameObject wreckingBallTargeter;

	// Token: 0x040028CB RID: 10443
	public AnimationCurve targetDespawnCurve;

	// Token: 0x040028CC RID: 10444
	public AudioClip swingClip;

	// Token: 0x040028CD RID: 10445
	private GameObject wreckingBallTargeterObject;

	// Token: 0x040028CE RID: 10446
	private WreckingBallVisual wreckingBallVisualScript;

	// Token: 0x040028CF RID: 10447
	private byte useTarget;

	// Token: 0x040028D0 RID: 10448
	private byte useDamage;

	// Token: 0x040028D1 RID: 10449
	private ActionTimer aiTimer = new ActionTimer(0.5f);

	// Token: 0x040028D2 RID: 10450
	private bool foundTarget;

	// Token: 0x040028D3 RID: 10451
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<byte> targetID = new NetVar<byte>(0);

	// Token: 0x040028D4 RID: 10452
	private short[] difficultyDistanceChange = new short[]
	{
		12,
		6,
		3
	};
}
