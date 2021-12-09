using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x020003BB RID: 955
public class BoardNode : MonoBehaviour
{
	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06001942 RID: 6466 RVA: 0x00012CB8 File Offset: 0x00010EB8
	// (set) Token: 0x06001943 RID: 6467 RVA: 0x00012CC0 File Offset: 0x00010EC0
	public bool CurHasInteraction { get; set; }

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06001944 RID: 6468 RVA: 0x00012CC9 File Offset: 0x00010EC9
	// (set) Token: 0x06001945 RID: 6469 RVA: 0x00012CD1 File Offset: 0x00010ED1
	public Interaction CurInteractionScript { get; set; }

	// Token: 0x06001946 RID: 6470 RVA: 0x00012CDA File Offset: 0x00010EDA
	public static Vector3 GetSharedNodeOffset(int index)
	{
		if (index < BoardNode.sharedNodeOffsets.Length)
		{
			return BoardNode.sharedNodeOffsets[index];
		}
		return Vector3.zero;
	}

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06001947 RID: 6471 RVA: 0x00012CF7 File Offset: 0x00010EF7
	public BoardPlayer[] NodeSlots
	{
		get
		{
			return this.nodeSlots;
		}
	}

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06001948 RID: 6472 RVA: 0x00012CFF File Offset: 0x00010EFF
	public BoardPlayer LastPlayer
	{
		get
		{
			return this.lastPlayer;
		}
	}

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x06001949 RID: 6473 RVA: 0x00012D07 File Offset: 0x00010F07
	public Vector3 NodePosition
	{
		get
		{
			return base.transform.position;
		}
	}

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x0600194A RID: 6474 RVA: 0x00012D14 File Offset: 0x00010F14
	// (set) Token: 0x0600194B RID: 6475 RVA: 0x00012D1C File Offset: 0x00010F1C
	public int NodeID
	{
		get
		{
			return this.nodeId;
		}
		set
		{
			this.nodeId = value;
		}
	}

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x0600194C RID: 6476 RVA: 0x00012D25 File Offset: 0x00010F25
	// (set) Token: 0x0600194D RID: 6477 RVA: 0x00012D2D File Offset: 0x00010F2D
	public bool Visited
	{
		get
		{
			return this.visited;
		}
		set
		{
			this.visited = value;
		}
	}

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x0600194E RID: 6478 RVA: 0x00012D36 File Offset: 0x00010F36
	private GameMap Map
	{
		get
		{
			if (this.map == null)
			{
				this.map = base.GetComponentInParent<GameMap>();
			}
			return this.map;
		}
	}

	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x0600194F RID: 6479 RVA: 0x00012D58 File Offset: 0x00010F58
	// (set) Token: 0x06001950 RID: 6480 RVA: 0x000AA188 File Offset: 0x000A8388
	public BoardNodeType CurrentNodeType
	{
		get
		{
			return this.currentNodeType;
		}
		set
		{
			if (value != this.currentNodeType || this.nodeObject == null)
			{
				this.currentNodeType = value;
				while (base.transform.childCount > 0)
				{
					Transform child = base.transform.GetChild(0);
					if (!(child != null))
					{
						break;
					}
					UnityEngine.Object.DestroyImmediate(child.gameObject);
				}
				UnityEngine.Object.DestroyImmediate(this.nodeObject);
				if (this.currentNodeType != BoardNodeType.Pathing && this.currentNodeType != BoardNodeType.Graveyard)
				{
					this.nodeObject = UnityEngine.Object.Instantiate<GameObject>(base.GetComponentInParent<GameMap>().spacePrefabs[(int)this.currentNodeType]);
					if (this.currentNodeType == BoardNodeType.Recruit && this.interactionScript != null)
					{
						this.SetRecruitColor();
					}
					this.nodeObject.transform.SetParent(base.transform, false);
					this.nodeObject.name = this.currentNodeType.ToString();
				}
			}
		}
	}

	// Token: 0x06001951 RID: 6481 RVA: 0x00012D60 File Offset: 0x00010F60
	public void ResetNode()
	{
		this.CurrentNodeType = this.baseNodeType;
		this.CurHasInteraction = this.hasInteraction;
		this.CurInteractionScript = this.interactionScript;
	}

	// Token: 0x06001952 RID: 6482 RVA: 0x00012D86 File Offset: 0x00010F86
	public void SetRecruitColor()
	{
		this.nodeObject.GetComponentInChildren<MeshFilter>().sharedMesh = this.Map.recruitMeshes2[(int)RecruitEventManager.eventManager.owners[this].owner.GamePlayer.ColorIndex];
	}

	// Token: 0x06001953 RID: 6483 RVA: 0x000AA274 File Offset: 0x000A8474
	public void Awake()
	{
		this.currentNodeType = this.baseNodeType;
		this.CurInteractionScript = this.interactionScript;
		this.CurHasInteraction = this.hasInteraction;
		List<BoardNodeConnection> list = new List<BoardNodeConnection>();
		for (int i = 0; i < this.nodeConnections.Length; i++)
		{
			if (this.nodeConnections[i].connection_type != BoardNodeConnectionDirection.Both)
			{
				list.Add(this.nodeConnections[i]);
			}
			else
			{
				BoardNodeConnection copy = this.nodeConnections[i].GetCopy();
				copy.connection_type = BoardNodeConnectionDirection.Forward;
				BoardNodeConnection copy2 = this.nodeConnections[i].GetCopy();
				copy2.connection_type = BoardNodeConnectionDirection.Back;
				list.Insert(0, copy2);
				list.Insert(0, copy);
			}
		}
		this.nodeConnections = list.ToArray();
	}

	// Token: 0x06001954 RID: 6484 RVA: 0x000AA324 File Offset: 0x000A8524
	public BoardNode()
	{
		this.nodeConnections = new BoardNodeConnection[2];
		this.currentNodeType = this.baseNodeType;
	}

	// Token: 0x06001955 RID: 6485 RVA: 0x000AA44C File Offset: 0x000A864C
	public List<BoardNode> GetForwardNodes(BoardNode last_node = null, bool includeCannon = false)
	{
		List<BoardNode> list = new List<BoardNode>();
		foreach (BoardNodeConnection boardNodeConnection in this.nodeConnections)
		{
			if (boardNodeConnection.connection_type == BoardNodeConnectionDirection.Forward && boardNodeConnection.node != last_node && (boardNodeConnection.transition != BoardNodeTransition.Cannon || includeCannon))
			{
				list.Add(boardNodeConnection.node);
			}
		}
		return list;
	}

	// Token: 0x06001956 RID: 6486 RVA: 0x00012DC3 File Offset: 0x00010FC3
	public void StartEvent(BoardPlayer _player, int seed)
	{
		this.eventFinished = false;
		base.StartCoroutine(this.Events(_player, seed));
	}

	// Token: 0x06001957 RID: 6487 RVA: 0x00012DDB File Offset: 0x00010FDB
	public IEnumerator Events(BoardPlayer _player, int seed)
	{
		this.player = _player;
		if (this.nodeEventType == BoardNodeEventType.Simple)
		{
			switch (this.eventAction)
			{
			case NodeEventAction.GiveHealth:
				yield return base.StartCoroutine(this.GiveHealth());
				break;
			case NodeEventAction.GiveGold:
				yield return base.StartCoroutine(this.GiveGold((this.currentNodeType == BoardNodeType.SmallGold) ? 3 : 10, 10));
				break;
			case NodeEventAction.GiveItem:
				yield return base.StartCoroutine(this.GiveItem(seed));
				break;
			case NodeEventAction.Damage:
			{
				Debug.Log("Doing Board Node Damage");
				yield return new WaitForSeconds(0.5f);
				DamageInstance d = new DamageInstance
				{
					damage = 5,
					origin = base.transform.position,
					blood = true,
					ragdoll = true,
					bloodAmount = 1f,
					details = "Node Damage Type",
					removeKeys = true
				};
				this.player.ApplyDamage(d);
				yield return new WaitForSeconds(2f);
				break;
			}
			default:
				Debug.LogError("Unhandled node event action");
				break;
			}
		}
		else if (this.nodeEventType == BoardNodeEventType.Custom)
		{
			yield return base.StartCoroutine(this.DoNodeEvent(_player, seed));
		}
		this.eventFinished = true;
		yield break;
	}

	// Token: 0x06001958 RID: 6488 RVA: 0x00012DF8 File Offset: 0x00010FF8
	private IEnumerator GiveGold(int total, int increment)
	{
		while (total > 0)
		{
			int num = Mathf.Min(total, increment);
			total -= num;
			if (NetSystem.IsServer)
			{
				GameManager.KeyController.SpawnKeys(5, this, this.player);
			}
			yield return new WaitForSeconds(1.5f);
		}
		yield break;
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x00012E15 File Offset: 0x00011015
	private IEnumerator GiveHealth()
	{
		Debug.Log("Board Node Give Health");
		int amount = (this.currentNodeType == BoardNodeType.SmallHeal) ? 3 : 10;
		if (this.player != null)
		{
			AudioSystem.PlayOneShot("HealingMagicSpellEffect_Pond5", 0.5f, 0.01f);
			GameObject gameObject = (GameObject)Resources.Load("Prefabs/Effects/FX_BigHeal");
			if (gameObject != null)
			{
				UnityEngine.Object.Instantiate<GameObject>(gameObject, this.player.transform.position + new Vector3(0f, 0.4f, 0f), Quaternion.identity);
			}
		}
		this.player.ApplyHeal(amount);
		yield return null;
		yield break;
	}

	// Token: 0x0600195A RID: 6490 RVA: 0x00012E24 File Offset: 0x00011024
	private IEnumerator GiveItem(int seed)
	{
		this.player.GiveItem((byte)seed, true);
		yield return new WaitForSeconds(2f);
		yield break;
	}

	// Token: 0x0600195B RID: 6491 RVA: 0x00012E3A File Offset: 0x0001103A
	private IEnumerator DoNodeEvent(BoardPlayer _player, int seed)
	{
		if (this.nodeEvent != null)
		{
			if (this.nodeEvent != null)
			{
				yield return base.StartCoroutine(this.nodeEvent.DoEvent(_player, this, seed));
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x0600195C RID: 6492 RVA: 0x00012E57 File Offset: 0x00011057
	public bool IsEventFinished()
	{
		return this.eventFinished;
	}

	// Token: 0x0600195D RID: 6493 RVA: 0x000AA4AC File Offset: 0x000A86AC
	public void EnterNode(BoardPlayer player)
	{
		bool flag = player == this.lastPlayer;
		for (int i = 0; i < this.nodeSlots.Length; i++)
		{
			if (this.nodeSlots[i] == player)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			return;
		}
		if (this.lastPlayer != null && !this.lastPlayer.TwitchMapEvent)
		{
			this.lastPlayer.MoveOffset(base.transform.position + this.GetNodeSlot(this.lastPlayer));
		}
		this.lastPlayer = player;
	}

	// Token: 0x0600195E RID: 6494 RVA: 0x00012E5F File Offset: 0x0001105F
	public void LeaveNode(BoardPlayer player)
	{
		if (this.lastPlayer == player)
		{
			this.lastPlayer = null;
		}
		this.FreeNodeSlot(player);
	}

	// Token: 0x0600195F RID: 6495 RVA: 0x000AA53C File Offset: 0x000A873C
	public Vector3 GetNodeSlot(BoardPlayer player)
	{
		for (int i = 0; i < this.nodeSlots.Length; i++)
		{
			if (this.nodeSlots[i] == null || this.nodeSlots[i] == player)
			{
				this.nodeSlots[i] = player;
				if (this.nodeOffsetType == NodeOffsetType.Spread)
				{
					return BoardNode.GetSharedNodeOffset(i);
				}
				if (this.nodeOffsetType == NodeOffsetType.Custom)
				{
					return this.customNodeOffsets[i];
				}
			}
		}
		return Vector3.zero;
	}

	// Token: 0x06001960 RID: 6496 RVA: 0x000AA5B0 File Offset: 0x000A87B0
	public void FreeNodeSlot(BoardPlayer player)
	{
		for (int i = 0; i < this.nodeSlots.Length; i++)
		{
			if (this.nodeSlots[i] == player)
			{
				this.nodeSlots[i] = null;
				return;
			}
		}
	}

	// Token: 0x06001961 RID: 6497 RVA: 0x000AA5EC File Offset: 0x000A87EC
	public Vector3 GetPlayersSlotPosition(BoardPlayer player)
	{
		for (int i = 0; i < this.nodeSlots.Length; i++)
		{
			if (this.nodeSlots[i] == player)
			{
				return this.GetSlotPosition(i);
			}
		}
		return base.transform.position;
	}

	// Token: 0x06001962 RID: 6498 RVA: 0x00012E7D File Offset: 0x0001107D
	public Vector3 GetSlotPosition(int i)
	{
		return base.transform.position + BoardNode.GetSharedNodeOffset(i);
	}

	// Token: 0x04001AF3 RID: 6899
	public BoardNodeType baseNodeType;

	// Token: 0x04001AF4 RID: 6900
	public BoardNodeEventType nodeEventType;

	// Token: 0x04001AF5 RID: 6901
	public NodeEventAction eventAction;

	// Token: 0x04001AF6 RID: 6902
	public BoardNodeConnection[] nodeConnections;

	// Token: 0x04001AF7 RID: 6903
	public BoardNodeEvent nodeEvent;

	// Token: 0x04001AF8 RID: 6904
	public float dist = 1f;

	// Token: 0x04001AF9 RID: 6905
	public float yRotation;

	// Token: 0x04001AFA RID: 6906
	public bool blockGoalSpawn;

	// Token: 0x04001AFB RID: 6907
	public bool hasInteraction;

	// Token: 0x04001AFC RID: 6908
	public Interaction interactionScript;

	// Token: 0x04001AFD RID: 6909
	public GameObject nodeObject;

	// Token: 0x04001AFE RID: 6910
	public NodeOffsetType nodeOffsetType;

	// Token: 0x04001AFF RID: 6911
	public Vector3[] customNodeOffsets = new Vector3[]
	{
		new Vector3(1.6f, 0f, 0f),
		new Vector3(-1.6f, 0f, 0f),
		new Vector3(0f, 0f, 1.6f),
		new Vector3(0f, 0f, -1.6f),
		new Vector3(-1.1f, 0f, -1.1f),
		new Vector3(1.1f, 0f, -1.1f),
		new Vector3(1.1f, 0f, 1.1f),
		new Vector3(-1.1f, 0f, 1.1f)
	};

	// Token: 0x04001B02 RID: 6914
	private BoardPlayer[] nodeSlots = new BoardPlayer[8];

	// Token: 0x04001B03 RID: 6915
	private BoardNodeType currentNodeType;

	// Token: 0x04001B04 RID: 6916
	private BoardPlayer player;

	// Token: 0x04001B05 RID: 6917
	private BoardPlayer lastPlayer;

	// Token: 0x04001B06 RID: 6918
	private bool visited;

	// Token: 0x04001B07 RID: 6919
	private int nodeId;

	// Token: 0x04001B08 RID: 6920
	private bool eventFinished;

	// Token: 0x04001B09 RID: 6921
	private static Vector3[] sharedNodeOffsets = new Vector3[]
	{
		new Vector3(1.6f, 0f, 0f),
		new Vector3(-1.6f, 0f, 0f),
		new Vector3(0f, 0f, 1.6f),
		new Vector3(0f, 0f, -1.6f),
		new Vector3(-1.1f, 0f, -1.1f),
		new Vector3(1.1f, 0f, -1.1f),
		new Vector3(1.1f, 0f, 1.1f),
		new Vector3(-1.1f, 0f, 1.1f)
	};

	// Token: 0x04001B0A RID: 6922
	private GameMap map;
}
