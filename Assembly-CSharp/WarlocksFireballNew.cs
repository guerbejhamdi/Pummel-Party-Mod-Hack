using System;
using UnityEngine;

// Token: 0x02000297 RID: 663
public class WarlocksFireballNew : MonoBehaviour
{
	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06001383 RID: 4995 RVA: 0x0000F855 File Offset: 0x0000DA55
	public Vector3 Dir
	{
		get
		{
			return this.dir;
		}
	}

	// Token: 0x06001384 RID: 4996 RVA: 0x0000F85D File Offset: 0x0000DA5D
	public void Setup(Vector3 _dir, short _owner_slot, short _id)
	{
		this.dir = _dir;
		this.owner_slot = _owner_slot;
		this.id = _id;
		this.minigame_controller = (WarlockControllerNew)GameManager.Minigame;
		this.minigame_controller.fireballs.Add(this);
	}

	// Token: 0x06001385 RID: 4997 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x06001386 RID: 4998 RVA: 0x00096310 File Offset: 0x00094510
	private void FixedUpdate()
	{
		Vector3 position = base.transform.position;
		float num = Time.fixedDeltaTime * this.speed;
		base.transform.position += this.dir * num;
		if (!this.is_dead)
		{
			RaycastHit[] array = Physics.SphereCastAll(position, 0.4f, this.dir, num, 2304);
			if (array.Length != 0)
			{
				if (array[0].collider.transform.root.tag == "Player")
				{
					WarlockPlayerNew1 component = array[0].collider.gameObject.GetComponent<WarlockPlayerNew1>();
					if (component.OwnerSlot != (ushort)this.owner_slot && !component.Stunned && !component.IsDead)
					{
						base.transform.position = array[0].point;
						this.minigame_controller.DestroyFireball(this.id, (byte)component.OwnerSlot);
						return;
					}
				}
				else
				{
					this.minigame_controller.DestroyFireball(this.id, 254);
					base.transform.position = array[0].point;
				}
			}
		}
	}

	// Token: 0x06001387 RID: 4999 RVA: 0x0009643C File Offset: 0x0009463C
	public void DestroyFireball(bool proxy, byte hitPlayer)
	{
		if (this.is_dead)
		{
			this.minigame_controller.fireballs.Remove(this);
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.is_dead = true;
		UnityEngine.Object.Instantiate<GameObject>(this.fireball_hit_pfb, base.transform.position, Quaternion.LookRotation(Vector3.up));
		UnityEngine.Object.Instantiate<GameObject>(this.fireball_remnants, base.transform.position, Quaternion.LookRotation(Vector3.up));
		if (hitPlayer != 254)
		{
			WarlockPlayerNew1 warlockPlayerNew = (WarlockPlayerNew1)this.minigame_controller.GetPlayerInSlot((short)hitPlayer);
			if (warlockPlayerNew.IsOwner)
			{
				warlockPlayerNew.Push(this.dir * 20f);
			}
		}
		if (!proxy)
		{
			this.minigame_controller.fireballs.Remove(this);
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		for (int i = 0; i < base.transform.childCount; i++)
		{
			base.transform.GetChild(i).gameObject.SetActive(false);
		}
	}

	// Token: 0x06001388 RID: 5000 RVA: 0x00096544 File Offset: 0x00094744
	public void ProxyUndo()
	{
		this.is_dead = false;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			base.transform.GetChild(i).gameObject.SetActive(true);
		}
	}

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x06001389 RID: 5001 RVA: 0x0000F895 File Offset: 0x0000DA95
	public short ID
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x040014D4 RID: 5332
	public GameObject fireball_hit_pfb;

	// Token: 0x040014D5 RID: 5333
	public GameObject fireball_remnants;

	// Token: 0x040014D6 RID: 5334
	private WarlockControllerNew minigame_controller;

	// Token: 0x040014D7 RID: 5335
	private Vector3 network_position;

	// Token: 0x040014D8 RID: 5336
	private Vector3 dir;

	// Token: 0x040014D9 RID: 5337
	private float speed = 10f;

	// Token: 0x040014DA RID: 5338
	private short owner_slot;

	// Token: 0x040014DB RID: 5339
	private short id;

	// Token: 0x040014DC RID: 5340
	private bool is_dead;
}
