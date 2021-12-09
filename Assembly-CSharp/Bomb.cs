using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x02000157 RID: 343
public class Bomb : MonoBehaviour
{
	// Token: 0x060009E6 RID: 2534 RVA: 0x000585B0 File Offset: 0x000567B0
	private void Start()
	{
		this.minigame_controller = (BomberController)GameManager.Minigame;
		this.nodes = GameObject.Find("Nodes").GetComponent<NodeCreator>();
		if (this.player_slot >= 0 && this.player_slot < BomberController.PLAYER_LAYERS.Length)
		{
			base.gameObject.layer = BomberController.PLAYER_LAYERS[this.player_slot];
		}
		((BomberPlayer)this.minigame_controller.GetPlayerInSlot((short)this.player_slot)).BombsRemaining--;
		this.minigame_controller.GetHitNodes(this.node, this.range, this.hitNodes);
		for (int i = 0; i < this.hitNodes.Count; i++)
		{
			this.hitNodes[i].AddHittingBomb(this);
		}
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x0000A85F File Offset: 0x00008A5F
	public void Setup(float place_time, int closest_node, int range, int player_slot)
	{
		this.place_time = place_time;
		this.node = closest_node;
		this.range = range;
		this.player_slot = player_slot;
	}

	// Token: 0x060009E8 RID: 2536 RVA: 0x0005867C File Offset: 0x0005687C
	private void Update()
	{
		if (NetSystem.NetTime.GameTime - this.place_time > this.explode_time)
		{
			AudioSystem.PlayOneShot(this.explode, 0.5f, 0.1f, 1f);
			for (int i = 0; i < this.hitNodes.Count; i++)
			{
				this.hitNodes[i].RemoveHittingBomb(this);
				this.hitNodes[i].bombNode.Explode(this.player_slot);
				if (NetSystem.IsServer && this.hitNodes[i].bombNode.Blocked)
				{
					this.hitNodes[i].bombNode.Blocked = false;
				}
			}
			this.nodes.nodes[this.node].occupier = -1;
			BomberPlayer bomberPlayer = (BomberPlayer)this.minigame_controller.GetPlayerInSlot((short)this.player_slot);
			int bombsRemaining = bomberPlayer.BombsRemaining;
			bomberPlayer.BombsRemaining = bombsRemaining + 1;
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x040008B7 RID: 2231
	public float explode_time = 3f;

	// Token: 0x040008B8 RID: 2232
	public GameObject explode_effect;

	// Token: 0x040008B9 RID: 2233
	public AudioClip explode;

	// Token: 0x040008BA RID: 2234
	private NodeCreator nodes;

	// Token: 0x040008BB RID: 2235
	private BomberController minigame_controller;

	// Token: 0x040008BC RID: 2236
	private float place_time;

	// Token: 0x040008BD RID: 2237
	private int node;

	// Token: 0x040008BE RID: 2238
	private int range;

	// Token: 0x040008BF RID: 2239
	private int player_slot;

	// Token: 0x040008C0 RID: 2240
	private List<Node> hitNodes = new List<Node>();
}
