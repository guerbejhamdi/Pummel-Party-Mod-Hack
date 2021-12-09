using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x02000158 RID: 344
public class BombNode : NetBehaviour
{
	// Token: 0x060009EA RID: 2538 RVA: 0x00058784 File Offset: 0x00056984
	public override void OnNetInitialize()
	{
		this.node = ((BomberController)GameManager.Minigame).nodes.nodes[this.pos.Value];
		if (!NetSystem.IsServer)
		{
			this.buffType.Recieve = new RecieveProxy(this.BuffTypeRecieve);
			this.blocked.Recieve = new RecieveProxy(this.BlockedRecieve);
			this.Blocked = this.blocked.Value;
			this.node.bombNode = this;
			this.UpdateBlockerVisual();
		}
		base.OnNetInitialize();
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x00058818 File Offset: 0x00056A18
	public void Explode(int exploder_slot)
	{
		UnityEngine.Object.Instantiate<GameObject>(this.explode_effect, base.transform.position, Quaternion.LookRotation(Vector3.up));
		for (int i = 0; i < this.collidingPlayers.Count; i++)
		{
			if (this.collidingPlayers[i].IsOwner)
			{
				this.collidingPlayers[i].KillPlayer(base.transform.position, exploder_slot, true);
			}
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x060009EC RID: 2540 RVA: 0x0000A89C File Offset: 0x00008A9C
	// (set) Token: 0x060009ED RID: 2541 RVA: 0x0000A8A9 File Offset: 0x00008AA9
	public BuffType Buff
	{
		get
		{
			return (BuffType)this.buffType.Value;
		}
		set
		{
			if (value != (BuffType)this.buffType.Value || this.blockerVisual == null)
			{
				this.buffType.Value = (int)value;
				this.UpdateBlockerVisual();
			}
		}
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x0000A8D9 File Offset: 0x00008AD9
	public void BuffTypeRecieve(object val)
	{
		this.UpdateBlockerVisual();
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x00058890 File Offset: 0x00056A90
	private void UpdateBlockerVisual()
	{
		if (this.blockerVisual != null)
		{
			UnityEngine.Object.Destroy(this.blockerVisual);
		}
		this.blockerVisual = UnityEngine.Object.Instantiate<GameObject>(this.visual_objects[0], Vector3.zero, Quaternion.identity);
		this.blockerVisual.transform.parent = base.transform.Find("Blocker");
		this.blockerVisual.transform.localPosition = Vector3.zero;
		this.blockerVisual.transform.localRotation = Quaternion.identity;
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x0000A8E1 File Offset: 0x00008AE1
	public void BlockedRecieve(object val)
	{
		this.Blocked = (bool)val;
	}

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x060009F1 RID: 2545 RVA: 0x0000A8EF File Offset: 0x00008AEF
	// (set) Token: 0x060009F2 RID: 2546 RVA: 0x00058924 File Offset: 0x00056B24
	public bool Blocked
	{
		get
		{
			return this.blocked.Value;
		}
		set
		{
			if (value != this.blockedOld)
			{
				this.blockedOld = value;
				if (NetSystem.IsServer)
				{
					this.blocked.Value = value;
				}
				base.transform.Find("Blocker").gameObject.SetActive(value);
				if (!value && ((BomberController)GameManager.Minigame).Playable)
				{
					if (NetSystem.IsServer && this.buffType.Value != 2)
					{
						((BomberController)GameManager.Minigame).NetSpawn(this.buffNetNames[this.buffType.Value], base.transform.position, Quaternion.identity, 0, null);
					}
					for (int i = 0; i < this.debri_objects.Count; i++)
					{
						GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.debri_objects[i], Vector3.zero, Quaternion.identity);
						gameObject.transform.parent = base.transform;
						gameObject.transform.localPosition = Vector3.zero;
						gameObject.transform.localRotation = Quaternion.identity;
					}
				}
			}
		}
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x0000A8FC File Offset: 0x00008AFC
	private void OnTriggerEnter(Collider c)
	{
		this.EnteredTrigger(c);
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x0000A8FC File Offset: 0x00008AFC
	private void OnTriggerStay(Collider c)
	{
		this.EnteredTrigger(c);
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x00058A3C File Offset: 0x00056C3C
	private void EnteredTrigger(Collider c)
	{
		BomberPlayer component = c.GetComponent<BomberPlayer>();
		if (component != null && !this.collidingPlayers.Contains(component))
		{
			this.collidingPlayers.Add(component);
		}
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x00058A74 File Offset: 0x00056C74
	private void OnTriggerExit(Collider c)
	{
		BomberPlayer component = c.GetComponent<BomberPlayer>();
		if (component != null && this.collidingPlayers.Contains(component))
		{
			this.collidingPlayers.Remove(component);
		}
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x0000A905 File Offset: 0x00008B05
	public void SetIndicatorState(bool active)
	{
		this.indicatorAnimation.Play(active ? "SkullIndicatorEnable" : "SkullIndicatorDisable");
	}

	// Token: 0x040008C1 RID: 2241
	public GameObject explode_effect;

	// Token: 0x040008C2 RID: 2242
	public List<BomberPlayer> collidingPlayers = new List<BomberPlayer>();

	// Token: 0x040008C3 RID: 2243
	public List<GameObject> visual_objects = new List<GameObject>();

	// Token: 0x040008C4 RID: 2244
	public List<GameObject> debri_objects = new List<GameObject>();

	// Token: 0x040008C5 RID: 2245
	public GameObject explodeIndicator;

	// Token: 0x040008C6 RID: 2246
	public Animation indicatorAnimation;

	// Token: 0x040008C7 RID: 2247
	private List<string> buffNetNames = new List<string>
	{
		"BuffBombRange",
		"BuffBombCount"
	};

	// Token: 0x040008C8 RID: 2248
	private GameObject blockerVisual;

	// Token: 0x040008C9 RID: 2249
	private Node node;

	// Token: 0x040008CA RID: 2250
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<int> pos = new NetVar<int>(0);

	// Token: 0x040008CB RID: 2251
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<int> buffType = new NetVar<int>(0);

	// Token: 0x040008CC RID: 2252
	private bool blockedOld = true;

	// Token: 0x040008CD RID: 2253
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<bool> blocked = new NetVar<bool>(true);
}
