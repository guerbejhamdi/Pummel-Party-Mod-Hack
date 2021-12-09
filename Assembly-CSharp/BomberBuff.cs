using System;
using UnityEngine;
using ZP.Net;

// Token: 0x0200015A RID: 346
public class BomberBuff : NetBehaviour
{
	// Token: 0x060009F9 RID: 2553 RVA: 0x0000A922 File Offset: 0x00008B22
	public override void OnNetInitialize()
	{
		((BomberController)GameManager.Minigame).bomberBuffs.Add(this);
		base.OnNetInitialize();
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x0000A93F File Offset: 0x00008B3F
	[NetRPC(true, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void RPCPickedUp(NetPlayer sender)
	{
		NetSystem.Kill(this);
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x00058B2C File Offset: 0x00056D2C
	private void OnTriggerEnter(Collider c)
	{
		BomberPlayer component = c.gameObject.GetComponent<BomberPlayer>();
		if (component != null && component.IsOwner && !this.used)
		{
			this.used = true;
			BuffType buffType = this.buff_type;
			if (buffType != BuffType.BOMB_RANGE)
			{
				if (buffType == BuffType.BOMB_COUNT)
				{
					BomberPlayer bomberPlayer = component;
					int num = bomberPlayer.MaxBombs;
					bomberPlayer.MaxBombs = num + 1;
					BomberPlayer bomberPlayer2 = component;
					num = bomberPlayer2.BombsRemaining;
					bomberPlayer2.BombsRemaining = num + 1;
				}
			}
			else
			{
				component.bombRange++;
			}
			if (!NetSystem.IsServer)
			{
				base.SendRPC("RPCPickedUp", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
				return;
			}
			NetSystem.Kill(this);
		}
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x00058BC4 File Offset: 0x00056DC4
	public override void OnNetDestroy()
	{
		if (this.collection_sound != null)
		{
			AudioSystem.PlayOneShot(this.collection_sound, 1f, 0.1f, 1f);
		}
		if (this.collection_fx_pfb != null)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.collection_fx_pfb, base.transform.position, Quaternion.LookRotation(Vector3.up));
		}
		((BomberController)GameManager.Minigame).bomberBuffs.Remove(this);
	}

	// Token: 0x040008D2 RID: 2258
	public BuffType buff_type = BuffType.NONE;

	// Token: 0x040008D3 RID: 2259
	public GameObject collection_fx_pfb;

	// Token: 0x040008D4 RID: 2260
	public AudioClip collection_sound;

	// Token: 0x040008D5 RID: 2261
	private bool used;
}
