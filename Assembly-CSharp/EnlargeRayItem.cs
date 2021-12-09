using System;
using System.Collections;
using LlockhamIndustries.Decals;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000058 RID: 88
public class EnlargeRayItem : Item
{
	// Token: 0x17000034 RID: 52
	// (get) Token: 0x0600018D RID: 397 RVA: 0x00004902 File Offset: 0x00002B02
	// (set) Token: 0x0600018E RID: 398 RVA: 0x00032758 File Offset: 0x00030958
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
			Transform transform = GameManager.PlayerList[(int)this.targetID.Value].BoardObject.transform;
			GameManager.Board.boardCamera.SetTrackedObject(transform, GameManager.Board.PlayerCamOffset);
			this.wreckingBallTargeterObject.transform.position = transform.position - new Vector3(0f, 0.75f, 0f);
		}
	}

	// Token: 0x0600018F RID: 399 RVA: 0x000327E4 File Offset: 0x000309E4
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		base.GetAITarget();
		this.wreckingBallTargeterObject = UnityEngine.Object.Instantiate<GameObject>(this.wreckingBallTargeter, Vector3.zero, Quaternion.Euler(90f, 0f, 0f));
		this.TargetID = ((this.player.GlobalID == 0) ? 1 : 0);
		this.targetID.Recieve = new RecieveProxy(this.TargetReceive);
	}

	// Token: 0x06000190 RID: 400 RVA: 0x00032858 File Offset: 0x00030A58
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
			else if (this.player.RewiredPlayer.GetNegativeButtonDown(InputActions.Horizontal))
			{
				this.TargetID = this.Increment(true, this.TargetID);
			}
			else if (this.player.RewiredPlayer.GetButtonDown(InputActions.Horizontal))
			{
				this.TargetID = this.Increment(false, this.TargetID);
			}
		}
		base.Update();
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000490F File Offset: 0x00002B0F
	private byte Increment(bool left, byte curID)
	{
		if (left)
		{
			curID = ((curID == 0) ? 3 : (curID - 1));
		}
		else
		{
			curID = ((curID == 3) ? 0 : (curID + 1));
		}
		if ((short)curID == this.player.GlobalID)
		{
			return this.Increment(left, curID);
		}
		return curID;
	}

	// Token: 0x06000192 RID: 402 RVA: 0x00032964 File Offset: 0x00030B64
	protected override void Use(int seed)
	{
		base.Use(seed);
		byte b = (byte)this.rand.Next(6, 8);
		ZPMath.RandomFloat(this.rand, 15f, 165f);
		base.SendRPC("RPCUseEnlargeRay", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			this.TargetID,
			b
		});
		base.StartCoroutine(this.UseEnlargeRay(this.TargetID, b));
	}

	// Token: 0x06000193 RID: 403 RVA: 0x00004947 File Offset: 0x00002B47
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCUseEnlargeRay(NetPlayer sender, byte playerID, byte damage)
	{
		base.Use(0);
		base.StartCoroutine(this.UseEnlargeRay(playerID, damage));
	}

	// Token: 0x06000194 RID: 404 RVA: 0x0000495F File Offset: 0x00002B5F
	private IEnumerator UseEnlargeRay(byte playerID, byte damage)
	{
		BoardPlayer target = GameManager.PlayerList[(int)playerID].BoardObject;
		EnlargeRayEnlarger enlargeRayEnlarger = target.PlayerAnimation.GetBone(PlayerBone.Head).gameObject.AddComponent<EnlargeRayEnlarger>();
		enlargeRayEnlarger.scaleCurve = this.scaleCurve;
		enlargeRayEnlarger.scaleTime = this.scaleTime;
		enlargeRayEnlarger.maxScale = this.scaleSize;
		this.DespawnTargeter();
		yield return new WaitForSeconds(this.scaleTime / 2f);
		target.PlayerAnimation.Animator.SetBool("Floating", true);
		yield return new WaitForSeconds(this.scaleTime / 2f);
		target.TurnSkip = 2;
		base.Finish(false);
		yield break;
	}

	// Token: 0x06000195 RID: 405 RVA: 0x00004975 File Offset: 0x00002B75
	public override void Unequip(bool endingTurn)
	{
		this.player.BoardObject.PlayerAnimation.Carrying = false;
		this.DespawnTargeter();
		GameManager.Board.CameraTrackCurrentPlayer();
		base.Unequip(endingTurn);
	}

	// Token: 0x06000196 RID: 406 RVA: 0x000329DC File Offset: 0x00030BDC
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

	// Token: 0x06000197 RID: 407 RVA: 0x000049A4 File Offset: 0x00002BA4
	public void TargetReceive(object val)
	{
		this.TargetID = (byte)val;
	}

	// Token: 0x06000198 RID: 408 RVA: 0x00032A38 File Offset: 0x00030C38
	public override ItemAIUse GetTarget(BoardPlayer user)
	{
		ItemAIUse result = null;
		int num = int.MaxValue;
		for (int i = 0; i < GameManager.PlayerList.Count; i++)
		{
			BoardPlayer boardObject = GameManager.PlayerList[i].BoardObject;
			if (!(boardObject == user) && (int)boardObject.LocalHealth < num)
			{
				num = (int)boardObject.LocalHealth;
				result = new ItemAIUse(boardObject, 1f);
			}
		}
		return result;
	}

	// Token: 0x040001E7 RID: 487
	public GameObject wreckingBallTargeter;

	// Token: 0x040001E8 RID: 488
	public AnimationCurve targetDespawnCurve;

	// Token: 0x040001E9 RID: 489
	[Header("Scaling")]
	public AnimationCurve scaleCurve;

	// Token: 0x040001EA RID: 490
	public Vector3 scaleSize;

	// Token: 0x040001EB RID: 491
	public float scaleTime;

	// Token: 0x040001EC RID: 492
	private GameObject wreckingBallTargeterObject;

	// Token: 0x040001ED RID: 493
	private ActionTimer aiTimer = new ActionTimer(0.5f);

	// Token: 0x040001EE RID: 494
	private bool foundTarget;

	// Token: 0x040001EF RID: 495
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<byte> targetID = new NetVar<byte>(0);
}
