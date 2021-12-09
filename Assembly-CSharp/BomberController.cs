using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using ZP.Net;

// Token: 0x0200015B RID: 347
public class BomberController : MinigameController
{
	// Token: 0x060009FE RID: 2558 RVA: 0x00058C40 File Offset: 0x00056E40
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.nodes = GameObject.Find("Nodes").GetComponent<NodeCreator>();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("BomberPlayer", null);
			base.StartCoroutine(this.SpawnNodesSlowly());
		}
		this.explodedNodes = new BomberController.NodeExplode[this.nodes.nodes.Length][];
		for (int i = 0; i < this.nodes.nodes.Length; i++)
		{
			this.explodedNodes[i] = new BomberController.NodeExplode[this.nodes.nodes[i].groups.Length];
			for (int j = 0; j < this.explodedNodes[i].Length; j++)
			{
				this.explodedNodes[i][j].group = this.nodes.nodes[i].groups[j];
				this.explodedNodes[i][j].exploded = false;
			}
		}
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x0000A956 File Offset: 0x00008B56
	private IEnumerator SpawnNodesSlowly()
	{
		int nodesPerFrame = 5;
		int curCount = 0;
		int num;
		for (int i = 0; i < this.nodes.nodes.Length; i = num)
		{
			this.nodes.nodes[i].bombNode = base.NetSpawn("BombNode", new Vector3(this.nodes.nodes[i].position.x, this.nodes.nodes[i].position.y, this.nodes.nodes[i].position.z), Quaternion.Euler(0f, this.nodes.nodes[i].y_rotation, 0f), 0, null).GetComponent<BombNode>();
			this.nodes.nodes[i].bombNode.pos.Value = i;
			if (!this.nodes.nodes[i].blocker || (this.nodes.nodes[i].unblockedPlayerCount != 0 && this.nodes.nodes[i].unblockedPlayerCount <= GameManager.GetPlayerCount()))
			{
				this.nodes.nodes[i].bombNode.Buff = BuffType.NONE;
				this.nodes.nodes[i].bombNode.Blocked = false;
			}
			else
			{
				this.nodes.nodes[i].bombNode.Buff = this.GetRandomBuff();
			}
			num = curCount;
			curCount = num + 1;
			if (curCount >= nodesPerFrame)
			{
				curCount = 0;
				yield return null;
			}
			num = i + 1;
		}
		yield break;
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x00058D2C File Offset: 0x00056F2C
	public void GetHitNodes(int nodeID, int range, List<Node> hitNodes)
	{
		for (int i = 0; i < this.explodedNodes.Length; i++)
		{
			for (int j = 0; j < this.explodedNodes[i].Length; j++)
			{
				this.explodedNodes[i][j].exploded = false;
			}
		}
		Node node = this.nodes.nodes[nodeID];
		hitNodes.Add(node);
		for (int k = 0; k < this.explodedNodes[nodeID].Length; k++)
		{
			this.explodedNodes[nodeID][k].exploded = true;
		}
		for (int l = 0; l < node.connections.Length; l++)
		{
			int nodeIndex = this.GetNodeIndex(node.connections[l]);
			int sharedGroup = this.GetSharedGroup(nodeID, nodeIndex);
			this.GetHitNodes2(nodeIndex, sharedGroup, range, hitNodes);
		}
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x00058DFC File Offset: 0x00056FFC
	private void GetHitNodes2(int nodeIndex, int group, int curRange, List<Node> hitNodes)
	{
		hitNodes.Add(this.nodes.nodes[nodeIndex]);
		int groupIndex = this.GetGroupIndex(nodeIndex, group);
		this.explodedNodes[nodeIndex][groupIndex].exploded = true;
		if (!this.nodes.nodes[nodeIndex].bombNode.Blocked && curRange > 1)
		{
			for (int i = 0; i < this.nodes.nodes[nodeIndex].connections.Length; i++)
			{
				int nodeIndex2 = this.GetNodeIndex(this.nodes.nodes[nodeIndex].connections[i]);
				int sharedGroup = this.GetSharedGroup(nodeIndex, nodeIndex2);
				if (sharedGroup != -1 && sharedGroup == group)
				{
					groupIndex = this.GetGroupIndex(nodeIndex2, sharedGroup);
					if (!this.explodedNodes[nodeIndex2][groupIndex].exploded)
					{
						this.GetHitNodes2(nodeIndex2, sharedGroup, curRange - 1, hitNodes);
					}
				}
			}
		}
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x00058ED0 File Offset: 0x000570D0
	public int GetNodeIndex(int id)
	{
		for (int i = 0; i < this.nodes.nodes.Length; i++)
		{
			if (this.nodes.nodes[i].id == id)
			{
				return i;
			}
		}
		Debug.LogError("ID not found in nodes");
		return -1;
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x00058F18 File Offset: 0x00057118
	private int GetSharedGroup(int id1, int id2)
	{
		for (int i = 0; i < this.nodes.nodes[id1].groups.Length; i++)
		{
			for (int j = 0; j < this.nodes.nodes[id2].groups.Length; j++)
			{
				if (this.nodes.nodes[id1].groups[i] == this.nodes.nodes[id2].groups[j])
				{
					return this.nodes.nodes[id1].groups[i];
				}
			}
		}
		return -1;
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x00058FA4 File Offset: 0x000571A4
	private int GetGroupIndex(int connectionIndex, int group)
	{
		for (int i = 0; i < this.explodedNodes[connectionIndex].Length; i++)
		{
			if (this.explodedNodes[connectionIndex][i].group == group)
			{
				return i;
			}
		}
		return 0;
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x00009BEC File Offset: 0x00007DEC
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.StartMinigame();
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x00058FE0 File Offset: 0x000571E0
	public override void RoundEnded()
	{
		if (NetSystem.IsServer)
		{
			for (int i = 0; i < this.players.Count; i++)
			{
				if (!((BomberPlayer)this.players[i]).IsDead)
				{
					CharacterBase characterBase = this.players[i];
					characterBase.Score += (short)((this.players.Count - 1) * 25);
				}
				else
				{
					CharacterBase characterBase2 = this.players[i];
					characterBase2.Score += this.players[i].RoundScore * 25;
					this.players[i].RoundScore = 0;
				}
			}
		}
		base.RoundEnded();
	}

	// Token: 0x06000A07 RID: 2567 RVA: 0x000590A0 File Offset: 0x000572A0
	public override void ResetRound()
	{
		this.ui_timer.time_test = this.round_length;
		this.suddenDeath = false;
		if (NetSystem.IsServer)
		{
			for (int i = 0; i < this.nodes.nodes.Length; i++)
			{
				this.nodes.nodes[i].bombNode.Buff = this.GetRandomBuff();
			}
		}
		for (int j = 0; j < this.nodes.nodes.Length; j++)
		{
			bool flag = !this.nodes.nodes[j].blocker || (this.nodes.nodes[j].unblockedPlayerCount != 0 && this.nodes.nodes[j].unblockedPlayerCount <= GameManager.GetPlayerCount());
			this.nodes.nodes[j].bombNode.Blocked = !flag;
			this.nodes.nodes[j].occupier = -1;
			this.nodes.nodes[j].bombNode.collidingPlayers.Clear();
			this.nodes.nodes[j].ClearHittingBombs();
		}
		Bomb[] array = UnityEngine.Object.FindObjectsOfType<Bomb>();
		for (int k = 0; k < array.Length; k++)
		{
			UnityEngine.Object.Destroy(array[k].gameObject);
		}
		BomberBuff[] array2 = UnityEngine.Object.FindObjectsOfType<BomberBuff>();
		for (int l = 0; l < array2.Length; l++)
		{
			base.NetKill(array2[l]);
		}
		base.ResetRound();
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000A09 RID: 2569 RVA: 0x00059218 File Offset: 0x00057418
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing)
		{
			if (this.ui_timer.time_test <= 0f && !this.suddenDeath)
			{
				GameManager.UIController.ShowLargeText(LocalizationManager.GetTranslation("Sudden Death", true, 0, true, false, null, null, true), LargeTextType.PlayerWins, 4f, true, false);
				this.suddenDeath = true;
				for (int i = 0; i < this.players.Count; i++)
				{
					((BomberPlayer)this.players[i]).bombRange = 30;
					((BomberPlayer)this.players[i]).MaxBombs = 10;
					((BomberPlayer)this.players[i]).BombsRemaining = 10;
				}
			}
			if (NetSystem.IsServer && (this.players_alive <= 1 || this.ui_timer.time_test <= -20f))
			{
				base.EndRound(3f, 3f, false);
			}
		}
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x0000A236 File Offset: 0x00008436
	public override void BuildResults()
	{
		base.BuildResults();
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x0000A965 File Offset: 0x00008B65
	[NetRPC(true, NetRPCSecurity.ALL, NetRPCSecurity.ALL)]
	public void RPCPlaceBomb(NetPlayer sender, float game_time, int node, int range, int player_slot)
	{
		this.PlaceBomb2(game_time, node, range, player_slot);
	}

	// Token: 0x06000A0D RID: 2573 RVA: 0x0005930C File Offset: 0x0005750C
	public void PlaceBomb(float game_time, int node, int range, int player_slot)
	{
		base.SendRPC("RPCPlaceBomb", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			game_time,
			node,
			range,
			player_slot
		});
		this.PlaceBomb2(game_time, node, range, player_slot);
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x0005935C File Offset: 0x0005755C
	private void PlaceBomb2(float game_time, int node, int range, int player_slot)
	{
		base.Spawn(this.bomb, this.nodes.nodes[node].position, Quaternion.Euler(0f, this.nodes.nodes[node].y_rotation, 0f)).GetComponent<Bomb>().Setup(game_time, node, range, player_slot);
		this.nodes.nodes[node].occupier = (short)player_slot;
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x000593CC File Offset: 0x000575CC
	public void PlayerDied(int player_slot, int killer_slot)
	{
		((BomberPlayer)base.GetPlayerInSlot((short)player_slot)).RoundScore = (short)(this.players.Count - this.players_alive);
		if (killer_slot != -1 && killer_slot != player_slot)
		{
			BomberPlayer bomberPlayer = (BomberPlayer)base.GetPlayerInSlot((short)killer_slot);
			bomberPlayer.Score += 50;
		}
		this.players_alive--;
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x00059430 File Offset: 0x00057630
	private BuffType GetRandomBuff()
	{
		float value = UnityEngine.Random.value;
		if (value < 0.5f)
		{
			return BuffType.BOMB_RANGE;
		}
		if (value < 0.8f)
		{
			return BuffType.BOMB_COUNT;
		}
		return BuffType.NONE;
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x00059458 File Offset: 0x00057658
	public BomberSearchNode GetPath(Node firstNode, Node target, short ownerSlot)
	{
		BomberSearchNode item = new BomberSearchNode(firstNode, 0, 0, null);
		BomberMinHeap bomberMinHeap = new BomberMinHeap();
		bomberMinHeap.Add(item);
		bool[] array = new bool[this.nodes.nodes.Length];
		while (bomberMinHeap.HasNext())
		{
			BomberSearchNode bomberSearchNode = bomberMinHeap.ExtractFirst();
			if (bomberSearchNode.node == target)
			{
				return bomberSearchNode;
			}
			for (int i = 0; i < bomberSearchNode.node.connections.Length; i++)
			{
				int nodeIndex = this.GetNodeIndex(bomberSearchNode.node.connections[i]);
				Node node = this.nodes.nodes[nodeIndex];
				if (!node.bombNode.Blocked && (node.occupier == -1 || node.occupier == ownerSlot) && node != null && !array[nodeIndex])
				{
					array[nodeIndex] = true;
					int num = bomberSearchNode.pathCost + 1;
					BomberSearchNode item2 = new BomberSearchNode(node, num, num, bomberSearchNode);
					bomberMinHeap.Add(item2);
				}
			}
		}
		return null;
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x00059548 File Offset: 0x00057748
	public Node GetPathClosestSafe(Node firstNode, short ownerSlot)
	{
		BomberSearchNode item = new BomberSearchNode(firstNode, 0, 0, null);
		BomberMinHeap bomberMinHeap = new BomberMinHeap();
		bomberMinHeap.Add(item);
		bool[] array = new bool[this.nodes.nodes.Length];
		while (bomberMinHeap.HasNext())
		{
			BomberSearchNode bomberSearchNode = bomberMinHeap.ExtractFirst();
			if (bomberSearchNode.node.HittingBombs.Count == 0)
			{
				return bomberSearchNode.node;
			}
			for (int i = 0; i < bomberSearchNode.node.connections.Length; i++)
			{
				int nodeIndex = this.GetNodeIndex(bomberSearchNode.node.connections[i]);
				Node node = this.nodes.nodes[nodeIndex];
				if (!node.bombNode.Blocked && (node.occupier == -1 || node.occupier == ownerSlot) && node != null && !array[nodeIndex])
				{
					array[nodeIndex] = true;
					int num = bomberSearchNode.pathCost + 1;
					BomberSearchNode item2 = new BomberSearchNode(node, num, num, bomberSearchNode);
					bomberMinHeap.Add(item2);
				}
			}
		}
		return null;
	}

	// Token: 0x040008D6 RID: 2262
	[Header("Minigame specific attributes")]
	public GameObject bomb;

	// Token: 0x040008D7 RID: 2263
	public GameObject blocker;

	// Token: 0x040008D8 RID: 2264
	public NodeCreator nodes;

	// Token: 0x040008D9 RID: 2265
	public List<BomberBuff> bomberBuffs = new List<BomberBuff>();

	// Token: 0x040008DA RID: 2266
	private bool suddenDeath;

	// Token: 0x040008DB RID: 2267
	private BomberController.NodeExplode[][] explodedNodes;

	// Token: 0x040008DC RID: 2268
	public static int[] PLAYER_LAYERS = new int[]
	{
		18,
		19,
		20,
		21,
		17,
		24,
		30,
		31
	};

	// Token: 0x0200015C RID: 348
	public struct NodeExplode
	{
		// Token: 0x040008DD RID: 2269
		public int group;

		// Token: 0x040008DE RID: 2270
		public bool exploded;
	}
}
