using System;
using System.Collections;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000003 RID: 3
public class ArcadePaddleBall : NetBehaviour
{
	// Token: 0x06000003 RID: 3 RVA: 0x0002A650 File Offset: 0x00028850
	public override void OnNetInitialize()
	{
		if (!base.IsOwner)
		{
			this.n_position.Recieve = new RecieveProxy(this.RecievePosition);
		}
		else
		{
			Vector2 vector = Vector3.Lerp(Vector2.right, (GameManager.rand.NextDouble() > 0.5) ? Vector2.down : Vector2.up, ZPMath.RandomFloat(GameManager.rand, 0.25f, 0.75f)).normalized;
			Vector2 vector2 = Vector3.Lerp(Vector2.right, (GameManager.rand.NextDouble() > 0.5) ? Vector2.down : Vector2.up, ZPMath.RandomFloat(GameManager.rand, 0.25f, 0.75f)).normalized;
			this.dir = ((GameManager.rand.NextDouble() > 0.5) ? vector : vector2);
		}
		this.dir.Normalize();
		base.StartCoroutine(this.Setup());
		base.OnNetDestroy();
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00003996 File Offset: 0x00001B96
	public IEnumerator Setup()
	{
		while (!this.setup)
		{
			if (GameManager.Board.CurPlayer.EquippedItem != null)
			{
				this.m_item = (ChallengeItem)GameManager.Board.CurPlayer.EquippedItem;
				this.setup = true;
				break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x0002A76C File Offset: 0x0002896C
	private void Update()
	{
		if (this.setup && this.m_item.ChallengeItemGameState == ChallengeItemState.PlayingGame)
		{
			if (!this.m_item.m_arcade.m_paddleBall.gameObject.activeSelf)
			{
				this.m_item.m_arcade.m_paddleBall.gameObject.SetActive(true);
			}
			RectTransform paddleBall = this.m_item.m_arcade.m_paddleBall;
			new Rect(paddleBall.anchoredPosition - this.ballSize, this.ballSize * 2f);
			Rect other = new Rect(this.m_item.m_arcade.m_PlayerObjects[0].anchoredPosition - this.paddleSize, this.paddleSize * 2f);
			Rect other2 = new Rect(this.m_item.m_arcade.m_PlayerObjects[1].anchoredPosition - this.paddleSize, this.paddleSize * 2f);
			if (base.IsOwner)
			{
				this.speed += this.acceleration * Time.deltaTime;
				float d = Mathf.Clamp(this.speed, this.minSpeed, this.maxSpeed);
				Vector2 a = this.dir * d;
				Vector2 anchoredPosition = paddleBall.anchoredPosition;
				Vector2 vector = paddleBall.anchoredPosition + a * Time.deltaTime;
				vector.x = Mathf.Clamp(vector.x, this.min.x + this.ballSize.x, this.max.x - this.ballSize.x);
				vector.y = Mathf.Clamp(vector.y, this.min.y + this.ballSize.y, this.max.y - this.ballSize.y);
				int num = 8;
				for (int i = 1; i <= num; i++)
				{
					float t = (float)i / (float)num;
					Vector2 vector2 = Vector2.Lerp(anchoredPosition, vector, t);
					Rect rect = new Rect(vector2 - this.ballSize, this.ballSize * 2f);
					if (rect.Overlaps(other))
					{
						Vector2 vector3 = vector2 - this.m_item.m_arcade.m_PlayerObjects[0].anchoredPosition;
						if (vector3.y >= 15f)
						{
							this.dir = new Vector2(0.5f, 0.5f).normalized;
						}
						else if (vector3.y <= -15f)
						{
							this.dir = new Vector2(0.5f, -0.5f).normalized;
						}
						else
						{
							this.dir = new Vector2(1f, 0f);
						}
						vector2.x = other.right + this.ballSize.x;
						vector = vector2;
						this.PlayHitSound();
						break;
					}
					if (rect.Overlaps(other2))
					{
						Vector2 vector4 = vector2 - this.m_item.m_arcade.m_PlayerObjects[1].anchoredPosition;
						if (vector4.y >= 15f)
						{
							this.dir = new Vector2(-0.5f, 0.5f).normalized;
						}
						else if (vector4.y <= -15f)
						{
							this.dir = new Vector2(-0.5f, -0.5f).normalized;
						}
						else
						{
							this.dir = new Vector2(-1f, 0f);
						}
						vector2.x = other2.left - this.ballSize.x;
						vector = vector2;
						this.PlayHitSound();
						break;
					}
				}
				paddleBall.anchoredPosition = vector;
				if (paddleBall.anchoredPosition.x <= this.min.x + this.ballSize.x)
				{
					this.m_item.p2.GiveScore();
					paddleBall.anchoredPosition = Vector2.zero;
					this.dir = Vector3.Lerp(Vector2.right, (GameManager.rand.NextDouble() > 0.5) ? Vector2.down : Vector2.up, ZPMath.RandomFloat(GameManager.rand, 0.25f, 0.75f)).normalized;
					this.speed = -256f;
				}
				if (paddleBall.anchoredPosition.x >= this.max.x - this.ballSize.x)
				{
					this.m_item.p1.GiveScore();
					paddleBall.anchoredPosition = Vector2.zero;
					this.dir = Vector3.Lerp(Vector2.left, (GameManager.rand.NextDouble() > 0.5) ? Vector2.down : Vector2.up, ZPMath.RandomFloat(GameManager.rand, 0.25f, 0.75f)).normalized;
					this.speed = -256f;
				}
				if (paddleBall.anchoredPosition.y >= this.max.y - this.ballSize.y || paddleBall.anchoredPosition.y <= this.min.y + this.ballSize.y)
				{
					this.dir.y = -this.dir.y;
					this.PlayHitSound();
				}
				this.n_position.Value = paddleBall.anchoredPosition;
				return;
			}
			bool gotPosition = this.m_gotPosition;
			this.m_gotPosition = false;
		}
	}

	// Token: 0x06000006 RID: 6 RVA: 0x0002AD24 File Offset: 0x00028F24
	public void RecievePosition(object _pos)
	{
		if (this == null || base.transform == null)
		{
			return;
		}
		this.m_gotPosition = true;
		Vector2 anchoredPosition = (Vector2)_pos;
		this.m_item.m_arcade.m_paddleBall.anchoredPosition = anchoredPosition;
	}

	// Token: 0x06000007 RID: 7 RVA: 0x0002AD70 File Offset: 0x00028F70
	public void RecieveVelocity(object _vel)
	{
		Vector2 velocity = (Vector2)_vel;
		this.m_velocity = velocity;
		this.recievedVelocity = true;
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000039A5 File Offset: 0x00001BA5
	private void PlayHitSound()
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCPlayHitSound", NetRPCDelivery.UNRELIABLE, Array.Empty<object>());
		}
		AudioSystem.PlayOneShot(this.hitClip, 2f, 0f, 1f);
	}

	// Token: 0x06000009 RID: 9 RVA: 0x000039DA File Offset: 0x00001BDA
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	private void RPCPlayHitSound(NetPlayer sender)
	{
		this.PlayHitSound();
	}

	// Token: 0x04000002 RID: 2
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.ALWAYS_SEND)]
	public NetVec2 n_position = new NetVec2(Vector2.zero);

	// Token: 0x04000003 RID: 3
	private bool setup;

	// Token: 0x04000004 RID: 4
	private ChallengeItem m_item;

	// Token: 0x04000005 RID: 5
	private float minSpeed;

	// Token: 0x04000006 RID: 6
	private float maxSpeed = 1000f;

	// Token: 0x04000007 RID: 7
	private float acceleration = 300f;

	// Token: 0x04000008 RID: 8
	private float speed = 256f;

	// Token: 0x04000009 RID: 9
	private Vector2 dir = new Vector2(0.5f, 0.5f);

	// Token: 0x0400000A RID: 10
	private Vector2 m_velocity;

	// Token: 0x0400000B RID: 11
	private Vector2 min = new Vector2(-665f, -512f);

	// Token: 0x0400000C RID: 12
	private Vector2 max = new Vector2(665f, 388f);

	// Token: 0x0400000D RID: 13
	private Vector2[] sizes = new Vector2[]
	{
		new Vector2(30f, 30f),
		new Vector2(15f, 100f)
	};

	// Token: 0x0400000E RID: 14
	private Vector2 ballSize = new Vector2(7.5f, 7.5f);

	// Token: 0x0400000F RID: 15
	private Vector2 paddleSize = new Vector2(7.5f, 60f);

	// Token: 0x04000010 RID: 16
	private bool m_gotPosition;

	// Token: 0x04000011 RID: 17
	private bool recievedVelocity;

	// Token: 0x04000012 RID: 18
	public AudioClip hitClip;
}
