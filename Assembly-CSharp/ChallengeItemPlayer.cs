using System;
using System.Collections;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000375 RID: 885
public class ChallengeItemPlayer : NetBehaviour
{
	// Token: 0x17000241 RID: 577
	// (get) Token: 0x060017CD RID: 6093 RVA: 0x00011B33 File Offset: 0x0000FD33
	// (set) Token: 0x060017CE RID: 6094 RVA: 0x000A4B9C File Offset: 0x000A2D9C
	public bool IsReady
	{
		get
		{
			return this.isReady;
		}
		set
		{
			this.isReady = value;
			if (this.m_item != null && this.arcadeSlot.Value != 254)
			{
				this.m_item.m_arcade.SetPlayerReady((int)this.arcadeSlot.Value);
			}
			if (base.IsOwner)
			{
				base.SendRPC("RPCReady", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
			}
		}
	}

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x060017CF RID: 6095 RVA: 0x00011B3B File Offset: 0x0000FD3B
	// (set) Token: 0x060017D0 RID: 6096 RVA: 0x000A4C04 File Offset: 0x000A2E04
	private short Score
	{
		get
		{
			return this.score;
		}
		set
		{
			if (this.score != value)
			{
				this.score = (short)Mathf.Clamp((int)value, 0, 255);
				if (this.m_item != null && this.arcadeSlot.Value != 254)
				{
					this.m_item.m_arcade.m_PlayerScores[(int)this.arcadeSlot.Value].text = this.score.ToString();
				}
				if (base.IsOwner)
				{
					this.netScore.Value = (byte)this.score;
				}
				AudioSystem.PlayOneShot(this.scoreClip, 1f, 0f, 1f);
			}
		}
	}

	// Token: 0x060017D1 RID: 6097 RVA: 0x000A4CB0 File Offset: 0x000A2EB0
	public override void OnNetInitialize()
	{
		this.player = GameManager.GetPlayerAt((int)base.OwnerSlot);
		base.StartCoroutine(this.Setup());
		if (!base.IsOwner)
		{
			NetVar<byte> netVar = this.netScore;
			netVar.Recieve = (RecieveProxy)Delegate.Combine(netVar.Recieve, new RecieveProxy(this.RecieveScore));
		}
		base.OnNetInitialize();
	}

	// Token: 0x060017D2 RID: 6098 RVA: 0x00011B43 File Offset: 0x0000FD43
	private void RecieveScore(object val)
	{
		this.Score = (short)((byte)val);
	}

	// Token: 0x060017D3 RID: 6099 RVA: 0x000A4D10 File Offset: 0x000A2F10
	public void Update()
	{
		if (this.m_item == null)
		{
			return;
		}
		if (base.IsOwner && this.m_item.ChallengeItemGameState == ChallengeItemState.WaitingForReady)
		{
			if (this.readyStateTime == -1f)
			{
				this.readyStateTime = Time.time;
			}
			if (!this.IsReady && Time.time - this.readyStateTime > 0.1f)
			{
				if (this.player.IsAI)
				{
					this.IsReady = true;
				}
				else if (this.player.RewiredPlayer.GetButtonDown(InputActions.Accept))
				{
					this.IsReady = true;
				}
			}
		}
		if (!this.m_setGame && this.m_graphic != null && (this.m_item.ChallengeItemGameState == ChallengeItemState.PlayingGame || this.m_item.ChallengeItemGameState == ChallengeItemState.GameCountdown))
		{
			this.m_graphic.SetGame(this.m_item.CurGame.gameType);
			this.m_setGame = true;
		}
		Vector2 vector = Vector2.zero;
		if (base.IsOwner)
		{
			if (this.player.IsAI)
			{
				vector = this.GetAIInput();
			}
			else
			{
				vector = new Vector2(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
			}
			Vector2 normalized = vector.normalized;
			vector = new Vector2(vector.x * Mathf.Abs(normalized.x), vector.y * Mathf.Abs(normalized.y));
			this.UpdateJoystick(vector * 34f);
			if ((this.m_item.ChallengeItemGameState == ChallengeItemState.PlayingGame || this.m_item.ChallengeItemGameState == ChallengeItemState.GameCountdown) && this.m_item.CurGame.gameType == ChallengeItemGameType.Paddles)
			{
				vector.x = 0f;
			}
		}
		else
		{
			if (this.m_item.CurGame != null && this.m_item.CurGame.gameType == ChallengeItemGameType.RoadDodge)
			{
				this.interpolator.Update();
				this.playerObject.anchoredPosition = this.interpolator.CurrentPosition;
			}
			else
			{
				this.playerObject.anchoredPosition = this.position.Value;
			}
			this.UpdateJoystick(Vector2.zero);
		}
		if (base.IsOwner && (this.m_item.ChallengeItemGameState == ChallengeItemState.PlayingGame || this.m_item.ChallengeItemGameState == ChallengeItemState.GameCountdown))
		{
			if (this.playerObject != null)
			{
				if (this.m_item.ChallengeItemGameState == ChallengeItemState.PlayingGame)
				{
					float d = (this.m_item.CurGame.gameType == ChallengeItemGameType.RoadDodge) ? 350f : 1000f;
					Vector2 vector2 = this.playerObject.anchoredPosition + new Vector2(vector.x, vector.y) * Time.deltaTime * d;
					vector2.x = Mathf.Clamp(vector2.x, this.min.x + this.sizes[(int)this.m_item.CurGame.gameType].x * 0.5f, this.max.x - this.sizes[(int)this.m_item.CurGame.gameType].x * 0.5f);
					vector2.y = Mathf.Clamp(vector2.y, this.min.y + this.sizes[(int)this.m_item.CurGame.gameType].y * 0.5f, this.max.y - this.sizes[(int)this.m_item.CurGame.gameType].y * 0.5f);
					this.playerObject.anchoredPosition = vector2;
				}
				this.position.Value = this.playerObject.anchoredPosition;
			}
			if (this.m_item.CurGame.gameType == ChallengeItemGameType.RoadDodge)
			{
				if (this.playerObject.anchoredPosition.y >= 350f)
				{
					this.playerObject.anchoredPosition = ((this.arcadeSlot.Value == 0) ? this.m_item.player1StartPositions[0] : this.m_item.player2StartPositions[0]);
					short num = this.Score;
					this.Score = num + 1;
				}
				Rect other = new Rect(this.playerObject.anchoredPosition - new Vector2(15f, 15f), new Vector2(30f, 30f));
				Vector2 vector3 = new Vector2(140f, 80f);
				int i = 0;
				while (i < this.m_item.m_spawnedVehicles.Count)
				{
					if (this.m_item.m_spawnedVehicles[i] != null)
					{
						Rect rect = new Rect(this.m_item.m_spawnedVehicles[i].rt.anchoredPosition - vector3 / 2f, vector3);
						if (rect.Overlaps(other))
						{
							this.PlayHitSound();
							this.playerObject.anchoredPosition = ((this.arcadeSlot.Value == 0) ? this.m_item.player1StartPositions[0] : this.m_item.player2StartPositions[0]);
							return;
						}
						i++;
					}
					else
					{
						this.m_item.m_spawnedVehicles.RemoveAt(i);
					}
				}
			}
		}
	}

	// Token: 0x060017D4 RID: 6100 RVA: 0x000A5294 File Offset: 0x000A3494
	private Vector2 GetAIInput()
	{
		if (this.m_item.ChallengeItemGameState != ChallengeItemState.PlayingGame)
		{
			return Vector2.zero;
		}
		if (this.m_item.CurGame.gameType == ChallengeItemGameType.RoadDodge)
		{
			bool flag = false;
			float num = 0f;
			Rect other = new Rect(this.playerObject.anchoredPosition - new Vector2(15f, 15f) + new Vector2(50f, 50f), new Vector2(30f, 30f));
			Vector2 vector = new Vector2(140f, 80f);
			for (int i = 0; i < this.m_item.m_spawnedVehicles.Count; i++)
			{
				if (this.m_item.m_spawnedVehicles[i] != null)
				{
					Rect rect = new Rect(this.m_item.m_spawnedVehicles[i].rt.anchoredPosition - vector / 2f, vector);
					if (rect.Overlaps(other))
					{
						num = Mathf.Sign(this.m_item.m_spawnedVehicles[i].Speed);
						flag = true;
						break;
					}
				}
			}
			return new Vector2(flag ? (-num) : 0f, flag ? 0f : 1f);
		}
		Vector2 vector2 = this.playerObject.anchoredPosition - this.m_item.m_arcade.m_paddleBall.anchoredPosition;
		Vector2 zero = Vector2.zero;
		if (Mathf.Abs(vector2.x) < 300f && Mathf.Abs(vector2.y) > 25f)
		{
			zero = new Vector2(0f, (vector2.y > 0f) ? -1f : 1f);
		}
		else
		{
			zero = Vector2.zero;
		}
		this.AIInput = Vector2.MoveTowards(this.AIInput, zero, Time.deltaTime * 7f);
		return this.AIInput;
	}

	// Token: 0x060017D5 RID: 6101 RVA: 0x00011B51 File Offset: 0x0000FD51
	public void RecievePosition(object _pos)
	{
		this.interpolator.NewPosition(_pos);
	}

	// Token: 0x060017D6 RID: 6102 RVA: 0x000A5490 File Offset: 0x000A3690
	public void GiveScore()
	{
		if (base.IsOwner)
		{
			short num = this.Score;
			this.Score = num + 1;
			return;
		}
		base.SendRPC("GiveScore", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x00011B5F File Offset: 0x0000FD5F
	private void PlayHitSound()
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCPlayHitSound", NetRPCDelivery.UNRELIABLE, Array.Empty<object>());
		}
		AudioSystem.PlayOneShot(this.hitClip, 1f, 0f, 1f);
	}

	// Token: 0x060017D8 RID: 6104 RVA: 0x00011B94 File Offset: 0x0000FD94
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	private void RPCPlayHitSound(NetPlayer sender)
	{
		this.PlayHitSound();
	}

	// Token: 0x060017D9 RID: 6105 RVA: 0x000A54C8 File Offset: 0x000A36C8
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.OWNER)]
	private void GiveScore(NetPlayer sender)
	{
		short num = this.Score;
		this.Score = num + 1;
	}

	// Token: 0x060017DA RID: 6106 RVA: 0x00011B9C File Offset: 0x0000FD9C
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	private void RPCReady(NetPlayer sender)
	{
		this.IsReady = true;
	}

	// Token: 0x060017DB RID: 6107 RVA: 0x000A54E8 File Offset: 0x000A36E8
	private void UpdateJoystick(Vector2 rot)
	{
		if (base.IsOwner)
		{
			this.xRot.Value = ZPMath.CompressFloat(rot.x, -35f, 35f);
			this.yRot.Value = ZPMath.CompressFloat(rot.y, -35f, 35f);
		}
		else
		{
			rot.x = ZPMath.DecompressFloat(this.xRot.Value, -35f, 35f);
			rot.y = ZPMath.DecompressFloat(this.yRot.Value, -35f, 35f);
		}
		if (this.targetJoystick != null)
		{
			this.targetJoystick.localRotation = Quaternion.Euler(-rot.y, 0f, rot.x);
		}
	}

	// Token: 0x060017DC RID: 6108 RVA: 0x00011BA5 File Offset: 0x0000FDA5
	private IEnumerator Setup()
	{
		while (this.targetJoystick == null)
		{
			if (this.arcadeSlot.Value != 254 && GameManager.Board.CurPlayer.EquippedItem != null)
			{
				ChallengeItem challengeItem = (ChallengeItem)GameManager.Board.CurPlayer.EquippedItem;
				if (challengeItem.m_arcade != null)
				{
					this.targetJoystick = challengeItem.m_arcade.m_JoystickTransforms[(int)this.arcadeSlot.Value];
					this.playerObject = challengeItem.m_arcade.m_PlayerObjects[(int)this.arcadeSlot.Value];
					if (base.IsOwner)
					{
						this.position.Value = this.playerObject.anchoredPosition;
					}
					this.m_graphic = this.playerObject.GetComponent<ChallengePlayerGraphic>();
					this.m_graphic.SetPlayer(this.player);
					this.interpolator = new Interpolator(Interpolator.InterpolationType.Position);
					if (!base.IsOwner)
					{
						NetVec3 netVec = this.position;
						netVec.Recieve = (RecieveProxy)Delegate.Combine(netVec.Recieve, new RecieveProxy(this.RecievePosition));
					}
					this.m_item = challengeItem;
					if (this.arcadeSlot.Value == 0)
					{
						challengeItem.p1 = this;
					}
					else if (this.arcadeSlot.Value == 1)
					{
						challengeItem.p2 = this;
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400194F RID: 6479
	public AudioClip scoreClip;

	// Token: 0x04001950 RID: 6480
	private GamePlayer player;

	// Token: 0x04001951 RID: 6481
	private Transform targetJoystick;

	// Token: 0x04001952 RID: 6482
	private RectTransform playerObject;

	// Token: 0x04001953 RID: 6483
	private Interpolator interpolator;

	// Token: 0x04001954 RID: 6484
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<byte> arcadeSlot = new NetVar<byte>(254);

	// Token: 0x04001955 RID: 6485
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 position = new NetVec3(Vector3.zero);

	// Token: 0x04001956 RID: 6486
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<byte> netScore = new NetVar<byte>(0);

	// Token: 0x04001957 RID: 6487
	private ChallengeItem m_item;

	// Token: 0x04001958 RID: 6488
	private ChallengePlayerGraphic m_graphic;

	// Token: 0x04001959 RID: 6489
	private bool m_setGame;

	// Token: 0x0400195A RID: 6490
	private float readyStateTime = -1f;

	// Token: 0x0400195B RID: 6491
	private bool isReady;

	// Token: 0x0400195C RID: 6492
	private short score;

	// Token: 0x0400195D RID: 6493
	private Vector2 min = new Vector2(-665f, -512f);

	// Token: 0x0400195E RID: 6494
	private Vector2 max = new Vector2(665f, 388f);

	// Token: 0x0400195F RID: 6495
	private Vector2[] sizes = new Vector2[]
	{
		new Vector2(30f, 30f),
		new Vector2(15f, 100f)
	};

	// Token: 0x04001960 RID: 6496
	private Vector2 AIInput = Vector2.zero;

	// Token: 0x04001961 RID: 6497
	public AudioClip hitClip;

	// Token: 0x04001962 RID: 6498
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<byte> xRot = new NetVar<byte>(0);

	// Token: 0x04001963 RID: 6499
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<byte> yRot = new NetVar<byte>(0);
}
