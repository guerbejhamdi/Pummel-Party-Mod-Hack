using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x02000264 RID: 612
public class SpookySpikesController : MinigameController
{
	// Token: 0x1700018C RID: 396
	// (get) Token: 0x060011FC RID: 4604 RVA: 0x0000E984 File Offset: 0x0000CB84
	// (set) Token: 0x060011FD RID: 4605 RVA: 0x0000E98C File Offset: 0x0000CB8C
	public float MinigameSpeed { get; set; }

	// Token: 0x060011FE RID: 4606 RVA: 0x0000AD54 File Offset: 0x00008F54
	public void KillPlayer()
	{
		this.players_alive--;
	}

	// Token: 0x060011FF RID: 4607 RVA: 0x0000E995 File Offset: 0x0000CB95
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("SpookySpikesPlayer", null);
		}
		this.MinigameSpeed = 1f;
	}

	// Token: 0x06001200 RID: 4608 RVA: 0x00009BEC File Offset: 0x00007DEC
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.StartMinigame();
	}

	// Token: 0x06001201 RID: 4609 RVA: 0x0008B930 File Offset: 0x00089B30
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing)
		{
			this.MinigameSpeed += Time.deltaTime * 0.04f;
			this.MinigameSpeed = Mathf.Clamp(this.MinigameSpeed, 0f, 4.1f);
			if (NetSystem.IsServer && (this.ui_timer.time_test <= 0f || this.players_alive <= 0))
			{
				base.EndRound(1f, 1f, false);
			}
			float num = this.speed * Time.deltaTime * this.MinigameSpeed;
			int i = 0;
			while (i < this.spikes.Count)
			{
				this.spikes[i].transform.position += Vector3.right * num;
				if (this.spikes[i].transform.position.x > 25f)
				{
					UnityEngine.Object.Destroy(this.spikes[i]);
					this.spikes.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
			if (NetSystem.IsServer)
			{
				this.amountMoved += num;
				if (this.amountMoved >= this.spacing)
				{
					this.amountMoved -= this.spacing;
					if (UnityEngine.Random.value > this.spawnChance.Evaluate(base.TimeSinceStart() / this.round_length))
					{
						this.SpawnSpikes((double)UnityEngine.Random.value > 0.5);
					}
				}
			}
		}
	}

	// Token: 0x06001202 RID: 4610 RVA: 0x0000E9BB File Offset: 0x0000CBBB
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSpawn(NetPlayer sender, bool bottom)
	{
		this.SpawnSpikes(bottom);
	}

	// Token: 0x06001203 RID: 4611 RVA: 0x0008BAB4 File Offset: 0x00089CB4
	private void SpawnSpikes(bool bottom)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSpawn", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				bottom
			});
		}
		Vector3 position = new Vector3(-17.5f, bottom ? 0.38f : 1.75f, 0f);
		GameObject gameObject = base.Spawn(this.spikePrefab, position, Quaternion.identity);
		this.spikes.Add(gameObject);
		MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].sharedMaterial = (bottom ? this.silverMat : this.goldMat);
		}
	}

	// Token: 0x040012B8 RID: 4792
	[Header("Minigame Specific Attributes")]
	public GameObject spikePrefab;

	// Token: 0x040012B9 RID: 4793
	public float speed = 5f;

	// Token: 0x040012BA RID: 4794
	public Material goldMat;

	// Token: 0x040012BB RID: 4795
	public Material silverMat;

	// Token: 0x040012BC RID: 4796
	public AnimationCurve spawnChance;

	// Token: 0x040012BD RID: 4797
	private List<GameObject> spikes = new List<GameObject>();

	// Token: 0x040012BE RID: 4798
	private float spacing = 5f;

	// Token: 0x040012BF RID: 4799
	private int count = 16;

	// Token: 0x040012C0 RID: 4800
	private float amountMoved;
}
