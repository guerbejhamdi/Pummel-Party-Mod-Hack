using System;
using UnityEngine;
using ZP.Net;

// Token: 0x020001EB RID: 491
public class NetworkTestPlayer : CharacterBase
{
	// Token: 0x06000E4E RID: 3662 RVA: 0x00072714 File Offset: 0x00070914
	public override void OnNetInitialize()
	{
		this.minigame_controller = GameManager.Minigame;
		this.minigame_controller.AddPlayer(this);
		if (base.IsOwner)
		{
			this.position.Value = base.transform.position;
		}
		else
		{
			this.position.Recieve = new RecieveProxy(this.RecievePosition);
		}
		base.OnNetInitialize();
	}

	// Token: 0x06000E4F RID: 3663 RVA: 0x0000CAF7 File Offset: 0x0000ACF7
	private new void Start()
	{
		if (base.OwnerSlot == 1 || base.OwnerSlot == 3)
		{
			this.set_instant = true;
		}
	}

	// Token: 0x06000E50 RID: 3664 RVA: 0x00072778 File Offset: 0x00070978
	private void Update()
	{
		if (base.IsOwner)
		{
			this.DoMovement();
			this.position.Value = base.transform.position;
			return;
		}
		base.GetComponent<MeshRenderer>().material.color = (this.set_instant ? Color.green : Color.red);
		if (!this.set_instant && this.got_position)
		{
			base.transform.position = this.position.Value;
			this.got_position = false;
			return;
		}
		if (!this.set_instant)
		{
			base.transform.position += this.velocity.Value * Time.deltaTime;
		}
	}

	// Token: 0x06000E51 RID: 3665 RVA: 0x0000CB12 File Offset: 0x0000AD12
	public void RecievePosition(object _pos)
	{
		if (this.set_instant)
		{
			base.transform.position = this.position.Value;
			return;
		}
		this.got_position = true;
	}

	// Token: 0x06000E52 RID: 3666 RVA: 0x00072830 File Offset: 0x00070A30
	private void DoMovement()
	{
		Vector3 a = this.startPosition + this.dir * (this.forward ? 1f : -1f) * this.range;
		Vector3 vector = a - base.transform.position;
		Vector3 b = vector.normalized * this.speed * Time.deltaTime;
		if (b.magnitude > vector.magnitude)
		{
			base.transform.position = a;
			this.forward = !this.forward;
		}
		else
		{
			base.transform.position += b;
		}
		this.velocity.Value = this.dir * (this.forward ? 1f : -1f) * this.speed;
	}

	// Token: 0x04000DC0 RID: 3520
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 position = new NetVec3(Vector3.zero);

	// Token: 0x04000DC1 RID: 3521
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 velocity = new NetVec3(Vector3.zero);

	// Token: 0x04000DC2 RID: 3522
	private bool got_position;

	// Token: 0x04000DC3 RID: 3523
	private MinigameController minigame_controller;

	// Token: 0x04000DC4 RID: 3524
	private bool set_instant;

	// Token: 0x04000DC5 RID: 3525
	private float range = 5f;

	// Token: 0x04000DC6 RID: 3526
	private Vector3 dir = new Vector3(1f, 0f, 0f);

	// Token: 0x04000DC7 RID: 3527
	private bool forward;

	// Token: 0x04000DC8 RID: 3528
	private float speed = 5f;
}
