using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003F4 RID: 1012
[ExecuteInEditMode]
public class GameMap : MonoBehaviour
{
	// Token: 0x06001BCB RID: 7115 RVA: 0x00014596 File Offset: 0x00012796
	public void Start()
	{
		if (this.temp)
		{
			return;
		}
		this.ps = base.GetComponentsInChildren<ParticleSystem>();
	}

	// Token: 0x06001BCC RID: 7116 RVA: 0x000BA5F0 File Offset: 0x000B87F0
	private void CheckParticles()
	{
		if (this.num >= this.ps.Length)
		{
			this.num = 0;
		}
		for (int i = this.num; i < this.ps.Length; i++)
		{
			if (!(this.ps[i] == null))
			{
				bool flag = (this.ps[i].transform.position - this.boardCamera.position).sqrMagnitude <= this.maxParticleDist * this.maxParticleDist;
				if (flag && this.ps[i].isStopped)
				{
					this.ps[i].Play();
				}
				else if (!flag && !this.ps[i].isStopped)
				{
					this.ps[i].Stop(false, ParticleSystemStopBehavior.StopEmitting);
				}
				if (this.time < this.interval)
				{
					this.num = i;
					return;
				}
				this.time -= this.interval;
			}
		}
		this.num = 0;
	}

	// Token: 0x06001BCD RID: 7117 RVA: 0x000145AD File Offset: 0x000127AD
	private void Update()
	{
		if (this.temp)
		{
			return;
		}
		if (!Application.isPlaying)
		{
			this.UpdateNodes();
			return;
		}
		this.CheckParticles();
	}

	// Token: 0x06001BCE RID: 7118 RVA: 0x000145CC File Offset: 0x000127CC
	public void UpdateNodes()
	{
		this.nodes = base.transform.GetComponentsInChildren<BoardNode>();
	}

	// Token: 0x06001BCF RID: 7119 RVA: 0x000BA6F4 File Offset: 0x000B88F4
	public void SetupMap()
	{
		this.nodes = base.transform.GetComponentsInChildren<BoardNode>();
		for (int i = 0; i < this.nodes.Length; i++)
		{
			if (this.nodes[i].baseNodeType == BoardNodeType.BoardStart)
			{
				this.startNode = this.nodes[i];
			}
			else if (this.nodes[i].baseNodeType == BoardNodeType.Graveyard)
			{
				this.graveyards.Add(this.nodes[i]);
			}
			this.nodes[i].NodeID = i;
		}
	}

	// Token: 0x06001BD0 RID: 7120 RVA: 0x000BA778 File Offset: 0x000B8978
	public BoardNode GetGraveyard(BoardNode[] startNodes, bool findClosest)
	{
		BoardNode result = null;
		int num = findClosest ? int.MaxValue : int.MinValue;
		for (int i = 0; i < this.graveyards.Count; i++)
		{
			int num2 = int.MaxValue;
			for (int j = 0; j < startNodes.Length; j++)
			{
				if (!(startNodes[j] == null))
				{
					int num3 = this.DistToNode(this.graveyards[i], startNodes[j], BoardNodeConnectionDirection.Forward);
					if (num3 < num2)
					{
						num2 = num3;
					}
				}
			}
			if (findClosest ? (num2 < num) : (num2 > num))
			{
				num = num2;
				result = this.graveyards[i];
			}
		}
		return result;
	}

	// Token: 0x06001BD1 RID: 7121 RVA: 0x000BA814 File Offset: 0x000B8A14
	public int DistToNode(BoardNode firstNode, BoardNode target, BoardNodeConnectionDirection directions = BoardNodeConnectionDirection.Forward)
	{
		SearchNode path = this.GetPath(firstNode, target, directions, false);
		if (path == null)
		{
			string str = (firstNode != null) ? firstNode.name : "null";
			string str2 = (target != null) ? target.name : "null";
			Debug.LogError("First Node: " + str + " Target: " + str2);
		}
		if (path == null)
		{
			return 0;
		}
		return path.pathCost;
	}

	// Token: 0x06001BD2 RID: 7122 RVA: 0x000BA880 File Offset: 0x000B8A80
	public SearchNode GetPath(BoardNode firstNode, BoardNode target, BoardNodeConnectionDirection directions = BoardNodeConnectionDirection.Forward, bool ignoreSpecialTransitionNodes = false)
	{
		SearchNode item = new SearchNode(firstNode, 0, 0, null);
		MinHeap minHeap = new MinHeap();
		minHeap.Add(item);
		bool[] array = new bool[this.nodes.Length];
		while (minHeap.HasNext())
		{
			SearchNode searchNode = minHeap.ExtractFirst();
			if (searchNode.node == target)
			{
				return new SearchNode(target, searchNode.pathCost + 1, searchNode.cost + 1, searchNode);
			}
			for (int i = 0; i < searchNode.node.nodeConnections.Length; i++)
			{
				if ((searchNode.node.nodeConnections[i].connection_type == BoardNodeConnectionDirection.Forward || directions != BoardNodeConnectionDirection.Forward) && (searchNode.node.nodeConnections[i].connection_type == BoardNodeConnectionDirection.Back || directions != BoardNodeConnectionDirection.Back) && (!ignoreSpecialTransitionNodes || searchNode.node.nodeConnections[i].transition == BoardNodeTransition.Walking))
				{
					BoardNode node = searchNode.node.nodeConnections[i].node;
					if (node != null && !array[node.NodeID])
					{
						array[node.NodeID] = true;
						int num = searchNode.pathCost + 1;
						SearchNode item2 = new SearchNode(node, num, num, searchNode);
						minHeap.Add(item2);
					}
				}
			}
		}
		return null;
	}

	// Token: 0x06001BD3 RID: 7123 RVA: 0x000145DF File Offset: 0x000127DF
	public BoardNode GetStartNode()
	{
		return this.startNode;
	}

	// Token: 0x06001BD4 RID: 7124 RVA: 0x000145E7 File Offset: 0x000127E7
	public BoardNode[] GetBoardNodes()
	{
		if (this.nodes == null)
		{
			this.nodes = base.GetComponentsInChildren<BoardNode>();
		}
		return this.nodes;
	}

	// Token: 0x06001BD5 RID: 7125 RVA: 0x000BA9B8 File Offset: 0x000B8BB8
	private void UpdateNodeInfo()
	{
		BoardNode[] componentsInChildren = base.GetComponentsInChildren<BoardNode>();
		BoardNode boardNode = null;
		int num = 1;
		int num2 = 1;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Visited = false;
			if (componentsInChildren[i].baseNodeType == BoardNodeType.BoardStart)
			{
				boardNode = componentsInChildren[i];
				boardNode.name = "Board_Start";
			}
			if (componentsInChildren[i].baseNodeType == BoardNodeType.Graveyard)
			{
				componentsInChildren[i].Visited = true;
				componentsInChildren[i].name = "Graveyard_" + num.ToString();
				num++;
			}
			if (componentsInChildren[i].baseNodeType == BoardNodeType.Pathing)
			{
				componentsInChildren[i].Visited = true;
				componentsInChildren[i].name = "Node_P_" + num2.ToString();
				num2++;
			}
		}
		if (boardNode != null)
		{
			boardNode.name = "Board_Start";
			int num3 = 1;
			int num4 = 1;
			for (int j = 0; j < boardNode.nodeConnections.Length; j++)
			{
				if (j == 0)
				{
					this.SetNodeNames(boardNode.nodeConnections[j].node, ref num3, ref num4);
				}
				else
				{
					this.SetNodeNames(boardNode.nodeConnections[j].node, ref num3, ref num4);
				}
			}
			return;
		}
	}

	// Token: 0x06001BD6 RID: 7126 RVA: 0x000BAAE0 File Offset: 0x000B8CE0
	private void SetNodeNames(BoardNode node, ref int index_a, ref int index_b)
	{
		if (node == null || node.Visited)
		{
			return;
		}
		if (node.baseNodeType != BoardNodeType.Pathing)
		{
			node.name = "Node_" + index_a.ToString() + "_" + index_b.ToString();
			index_b++;
		}
		node.Visited = true;
		for (int i = 0; i < node.nodeConnections.Length; i++)
		{
			if (i == 0)
			{
				this.SetNodeNames(node.nodeConnections[i].node, ref index_a, ref index_b);
			}
			else
			{
				index_a++;
				this.SetNodeNames(node.nodeConnections[i].node, ref index_a, ref index_b);
			}
		}
	}

	// Token: 0x06001BD7 RID: 7127 RVA: 0x000BAB80 File Offset: 0x000B8D80
	private bool IntersectRaySphere(Ray ray, Vector3 sphereOrigin, float sphereRadius, ref float t, ref Vector3 q)
	{
		Vector3 vector = ray.origin - sphereOrigin;
		float num = Vector3.Dot(vector, ray.direction);
		float num2 = Vector3.Dot(vector, vector) - sphereRadius * sphereRadius;
		if (num2 > 0f && num > 0f)
		{
			return false;
		}
		float num3 = num * num - num2;
		if (num3 < 0f)
		{
			return false;
		}
		t = -num - Mathf.Sqrt(num3);
		if (t < 0f)
		{
			t = 0f;
		}
		q = ray.origin + t * ray.direction;
		return true;
	}

	// Token: 0x04001DF8 RID: 7672
	public MainBoardEvent mainBoardEvent;

	// Token: 0x04001DF9 RID: 7673
	public bool showConnections;

	// Token: 0x04001DFA RID: 7674
	public bool showRadiusCircles;

	// Token: 0x04001DFB RID: 7675
	public bool showNodeOffsets;

	// Token: 0x04001DFC RID: 7676
	public GameObject[] spacePrefabs = new GameObject[0];

	// Token: 0x04001DFD RID: 7677
	public Mesh[] recruitMeshes2 = new Mesh[4];

	// Token: 0x04001DFE RID: 7678
	public GameObject connectionPrefab;

	// Token: 0x04001DFF RID: 7679
	public Rect mapExtents = new Rect(100f, 100f, 100f, 100f);

	// Token: 0x04001E00 RID: 7680
	public Transform boardCamera;

	// Token: 0x04001E01 RID: 7681
	public float maxParticleDist = 20f;

	// Token: 0x04001E02 RID: 7682
	public bool temp;

	// Token: 0x04001E03 RID: 7683
	private BoardNode startNode;

	// Token: 0x04001E04 RID: 7684
	private BoardNode[] nodes;

	// Token: 0x04001E05 RID: 7685
	private List<BoardNode> graveyards = new List<BoardNode>();

	// Token: 0x04001E06 RID: 7686
	private double next_name_update;

	// Token: 0x04001E07 RID: 7687
	private ParticleSystem[] ps;

	// Token: 0x04001E08 RID: 7688
	private int num;

	// Token: 0x04001E09 RID: 7689
	private float time;

	// Token: 0x04001E0A RID: 7690
	private float interval = 0.01f;

	// Token: 0x020003F5 RID: 1013
	private struct NodeValues
	{
		// Token: 0x06001BD9 RID: 7129 RVA: 0x00014603 File Offset: 0x00012803
		public NodeValues(BoardNode node)
		{
			this.position = node.NodePosition;
			this.nodeType = node.baseNodeType;
			this.yRotation = 180f;
			this.nodeSquareObject = null;
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x0001462F File Offset: 0x0001282F
		public void UpdateValues(BoardNode node)
		{
			this.position = node.NodePosition;
			this.nodeType = node.baseNodeType;
			this.yRotation = node.yRotation;
		}

		// Token: 0x04001E0B RID: 7691
		public Vector3 position;

		// Token: 0x04001E0C RID: 7692
		public BoardNodeType nodeType;

		// Token: 0x04001E0D RID: 7693
		public float yRotation;

		// Token: 0x04001E0E RID: 7694
		public GameObject nodeSquareObject;
	}
}
