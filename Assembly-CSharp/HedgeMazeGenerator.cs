using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020000AA RID: 170
public class HedgeMazeGenerator : MonoBehaviour
{
	// Token: 0x0600038A RID: 906 RVA: 0x00039438 File Offset: 0x00037638
	private void Start()
	{
		this.spaces = new HedgeMazeGenerator.GridSpace[this.height, this.width];
		this.tweens = new LTDescr[this.height, this.width];
		float num = -((float)this.width / 2f) * this.gridSize;
		float num2 = (float)this.height / 2f * this.gridSize;
		for (int i = 0; i < this.width - (this.removeBottomRight ? 1 : 0); i++)
		{
			Vector3 a = new Vector3(num + (float)i * this.gridSize, 0f, num2 - 0f * this.gridSize);
			this.SpawnHedge(a + new Vector3(this.gridSize / 2f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
		}
		for (int j = 0; j < this.height - (this.removeBottomLeft ? 1 : 0); j++)
		{
			Vector3 a2 = new Vector3(num + 0f * this.gridSize, 0f, num2 - (float)j * this.gridSize);
			this.SpawnHedge(a2 + new Vector3(0f, 0f, -this.gridSize / 2f), Quaternion.Euler(0f, 90f, 0f));
		}
		for (int k = 0; k < this.height; k++)
		{
			for (int l = 0; l < this.width; l++)
			{
				Vector3 a3 = new Vector3(num + (float)l * this.gridSize, 0f, num2 - (float)k * this.gridSize);
				this.spaces[k, l] = new HedgeMazeGenerator.GridSpace(this.SpawnHedge(a3 + new Vector3(this.gridSize, 0f, -this.gridSize / 2f), Quaternion.Euler(0f, 90f, 0f)), this.SpawnHedge(a3 + new Vector3(this.gridSize / 2f, 0f, -this.gridSize), Quaternion.Euler(0f, 0f, 0f)));
			}
		}
		base.StartCoroutine(this.UpdateMaze(10, true));
	}

	// Token: 0x0600038B RID: 907 RVA: 0x00005ED8 File Offset: 0x000040D8
	public IEnumerator UpdateMaze(int seed, bool first = false)
	{
		this.rand = new System.Random(seed);
		this.grid = new byte[this.height, this.width];
		if (!first)
		{
			AudioSystem.PlayOneShot(this.bushRustleClip, 4f, 0f, 1f);
		}
		int num;
		for (int y = 0; y < this.grid.GetLength(0); y = num)
		{
			for (int x = 0; x < this.grid.GetLength(1); x = num)
			{
				if (x != this.grid.GetLength(1) - 1 && this.spaces[y, x].e != null)
				{
					this.Tween(this.spaces[y, x].e, true);
				}
				if (y != this.grid.GetLength(0) - 1 && this.spaces[y, x].s != null)
				{
					this.Tween(this.spaces[y, x].s, true);
				}
				if (!first && this.delaySpawn)
				{
					yield return new WaitForSeconds(this.spawnDelayTime);
				}
				num = x + 1;
			}
			num = y + 1;
		}
		if (!first)
		{
			yield return new WaitForSeconds(0.3f);
		}
		this.Maze();
		if (this.removeMiddle)
		{
			this.grid[5, 5] = 6;
			this.grid[5, 6] = 6;
			this.grid[6, 5] = 6;
			this.grid[6, 6] = 6;
		}
		if (!first)
		{
			AudioSystem.PlayOneShot(this.bushRustleClip, 4f, 0f, 1f);
		}
		for (int y = 0; y < this.grid.GetLength(0); y = num)
		{
			for (int x = 0; x < this.grid.GetLength(1); x = num)
			{
				if ((this.grid[y, x] & this.S) == 0 && this.spaces[y, x].s != null)
				{
					this.Tween(this.spaces[y, x].s, false);
				}
				if ((this.grid[y, x] & this.E) == 0 && this.spaces[y, x].e != null)
				{
					this.Tween(this.spaces[y, x].e, false);
				}
				if (!first && this.delaySpawn)
				{
					yield return new WaitForSeconds(this.spawnDelayTime);
				}
				num = x + 1;
			}
			num = y + 1;
		}
		if (this.isBoard)
		{
			for (int i = 0; i < this.nodes.Length; i++)
			{
				this.nodes[i].nodeConnections = new BoardNodeConnection[0];
			}
			this.BuildPath(5, 0);
			this.AddConnection(this.nodes[5], this.entranceBoardNode, BoardNodeConnectionDirection.Back, BoardNodeTransition.Walking);
			this.AddConnection(this.nodes[this.width * (this.height - 1)], this.endBoardNode, BoardNodeConnectionDirection.Forward, BoardNodeTransition.Walking);
		}
		yield return new WaitForSeconds(0.25f);
		yield break;
	}

	// Token: 0x0600038C RID: 908 RVA: 0x000396A0 File Offset: 0x000378A0
	private void OnEnable()
	{
		if (this.grid != null)
		{
			for (int i = 0; i < this.grid.GetLength(0); i++)
			{
				for (int j = 0; j < this.grid.GetLength(1); j++)
				{
					if ((this.grid[i, j] & this.S) != 0 && this.spaces[i, j].e != null)
					{
						this.Tween(this.spaces[i, j].s, true);
					}
					if ((this.grid[i, j] & this.E) != 0 && this.spaces[i, j].e != null)
					{
						this.Tween(this.spaces[i, j].e, true);
					}
				}
			}
		}
	}

	// Token: 0x0600038D RID: 909 RVA: 0x00039784 File Offset: 0x00037984
	private void Tween(GameObject g, bool hidden)
	{
		if (!hidden)
		{
			g.SetActive(true);
		}
		LeanTween.cancel(g);
		LeanTween.scale(g, hidden ? this.size : Vector3.one, hidden ? 0.2333f : 0.2333f).setEase(hidden ? this.curve : this.inCurve).setOnComplete(delegate()
		{
			if (hidden)
			{
				g.SetActive(false);
			}
		});
	}

	// Token: 0x0600038E RID: 910 RVA: 0x00039828 File Offset: 0x00037A28
	private GameObject SpawnHedge(Vector3 pos, Quaternion rot)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.hedgePrefab, Vector3.zero, rot, base.transform);
		gameObject.transform.localPosition = pos;
		gameObject.transform.localScale = new Vector3(this.gridSize / 5f, this.scale, this.scale);
		if (!this.isBoard)
		{
			BoxCollider componentInChildren = gameObject.GetComponentInChildren<BoxCollider>();
			if (componentInChildren != null)
			{
				NavMeshModifier navMeshModifier = componentInChildren.gameObject.AddComponent<NavMeshModifier>();
				navMeshModifier.overrideArea = true;
				navMeshModifier.area = 1;
			}
		}
		return gameObject;
	}

	// Token: 0x0600038F RID: 911 RVA: 0x000398B4 File Offset: 0x00037AB4
	private void BuildPath(int x, int y)
	{
		int num = y * this.width + x;
		bool flag = false;
		if (x + 1 < this.width && (this.grid[y, x] & this.E) != 0 && !this.AlreadyConnected(this.nodes[num], this.nodes[num + 1]))
		{
			this.AddConnection(this.nodes[num], this.nodes[num + 1], BoardNodeConnectionDirection.Forward, BoardNodeTransition.Walking);
			this.AddConnection(this.nodes[num + 1], this.nodes[num], BoardNodeConnectionDirection.Back, BoardNodeTransition.Walking);
			this.BuildPath(x + 1, y);
			flag = true;
		}
		if (x - 1 >= 0 && (this.grid[y, x - 1] & this.E) != 0 && !this.AlreadyConnected(this.nodes[num], this.nodes[num - 1]))
		{
			this.AddConnection(this.nodes[y * this.width + x], this.nodes[y * this.width + x - 1], BoardNodeConnectionDirection.Forward, BoardNodeTransition.Walking);
			this.AddConnection(this.nodes[y * this.width + x - 1], this.nodes[y * this.width + x], BoardNodeConnectionDirection.Back, BoardNodeTransition.Walking);
			this.BuildPath(x - 1, y);
			flag = true;
		}
		if (y + 1 < this.height && (this.grid[y, x] & this.S) != 0 && !this.AlreadyConnected(this.nodes[num], this.nodes[num + this.width]))
		{
			this.AddConnection(this.nodes[y * this.width + x], this.nodes[y * this.width + x + this.width], BoardNodeConnectionDirection.Forward, BoardNodeTransition.Walking);
			this.AddConnection(this.nodes[y * this.width + x + this.width], this.nodes[y * this.width + x], BoardNodeConnectionDirection.Back, BoardNodeTransition.Walking);
			this.BuildPath(x, y + 1);
			flag = true;
		}
		if (y - 1 >= 0 && (this.grid[y - 1, x] & this.S) != 0 && !this.AlreadyConnected(this.nodes[num], this.nodes[num - this.width]))
		{
			this.AddConnection(this.nodes[y * this.width + x], this.nodes[y * this.width + x - this.width], BoardNodeConnectionDirection.Forward, BoardNodeTransition.Walking);
			this.AddConnection(this.nodes[y * this.width + x - this.width], this.nodes[y * this.width + x], BoardNodeConnectionDirection.Back, BoardNodeTransition.Walking);
			this.BuildPath(x, y - 1);
			flag = true;
		}
		if (!flag && (x != 0 || y != this.height - 1))
		{
			this.AddConnection(this.nodes[y * this.width + x], this.entranceBoardNode, BoardNodeConnectionDirection.Forward, BoardNodeTransition.Teleport);
			this.AddConnection(this.entranceBoardNode, this.nodes[y * this.width + x], BoardNodeConnectionDirection.Back, BoardNodeTransition.Teleport);
		}
	}

	// Token: 0x06000390 RID: 912 RVA: 0x00039B9C File Offset: 0x00037D9C
	private bool AlreadyConnected(BoardNode node, BoardNode node2)
	{
		for (int i = 0; i < node.nodeConnections.Length; i++)
		{
			if (node.nodeConnections[i].node == node2)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000391 RID: 913 RVA: 0x00039BD4 File Offset: 0x00037DD4
	private void AddConnection(BoardNode node, BoardNode node2, BoardNodeConnectionDirection type, BoardNodeTransition transitionType = BoardNodeTransition.Walking)
	{
		int num = node.nodeConnections.Length;
		BoardNodeConnection[] array = new BoardNodeConnection[num + 1];
		for (int i = 0; i < num; i++)
		{
			array[i] = node.nodeConnections[i];
		}
		array[num] = new BoardNodeConnection
		{
			connection_type = type,
			node = node2,
			transition = transitionType
		};
		node.nodeConnections = array;
	}

	// Token: 0x06000392 RID: 914 RVA: 0x00039C30 File Offset: 0x00037E30
	private void AddFrontier(int x, int y)
	{
		if (x >= 0 && y >= 0 && y < this.grid.GetLength(0) && x < this.grid.GetLength(1) && this.grid[y, x] == 0)
		{
			ref byte ptr = ref this.grid[y, x];
			ptr |= this.FRONTIER;
			this.frontier.Add(new Vector2((float)x, (float)y));
		}
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00039CA0 File Offset: 0x00037EA0
	public void Mark(int x, int y)
	{
		ref byte ptr = ref this.grid[y, x];
		ptr |= this.IN;
		this.AddFrontier(x - 1, y);
		this.AddFrontier(x + 1, y);
		this.AddFrontier(x, y - 1);
		this.AddFrontier(x, y + 1);
	}

	// Token: 0x06000394 RID: 916 RVA: 0x00039CF0 File Offset: 0x00037EF0
	public List<Vector2> Neighbours(int x, int y)
	{
		List<Vector2> list = new List<Vector2>();
		if (x > 0 && (this.grid[y, x - 1] & this.IN) != 0)
		{
			list.Add(new Vector2((float)(x - 1), (float)y));
		}
		if (x + 1 < this.grid.GetLength(1) && (this.grid[y, x + 1] & this.IN) != 0)
		{
			list.Add(new Vector2((float)(x + 1), (float)y));
		}
		if (y > 0 && (this.grid[y - 1, x] & this.IN) != 0)
		{
			list.Add(new Vector2((float)x, (float)(y - 1)));
		}
		if (y + 1 < this.grid.GetLength(0) && (this.grid[y + 1, x] & this.IN) != 0)
		{
			list.Add(new Vector2((float)x, (float)(y + 1)));
		}
		return list;
	}

	// Token: 0x06000395 RID: 917 RVA: 0x00005EF5 File Offset: 0x000040F5
	private byte Direction(int fx, int fy, int tx, int ty)
	{
		if (fx < tx)
		{
			return this.E;
		}
		if (fx > tx)
		{
			return this.W;
		}
		if (fy < ty)
		{
			return this.S;
		}
		return this.N;
	}

	// Token: 0x06000396 RID: 918 RVA: 0x00005F1F File Offset: 0x0000411F
	private bool Empty(byte val)
	{
		return val == 0 || val == this.FRONTIER;
	}

	// Token: 0x06000397 RID: 919 RVA: 0x00039DD4 File Offset: 0x00037FD4
	private void Maze()
	{
		this.Mark(this.rand.Next(0, this.width), this.rand.Next(0, this.height));
		while (this.frontier.Count > 0)
		{
			int index = this.rand.Next(0, this.frontier.Count);
			int num = (int)this.frontier[index].x;
			int num2 = (int)this.frontier[index].y;
			this.frontier.RemoveAt(index);
			List<Vector2> list = this.Neighbours(num, num2);
			Vector2 vector = list[this.rand.Next(0, list.Count)];
			int num3 = (int)vector.x;
			int num4 = (int)vector.y;
			byte b = this.Direction(num, num2, num3, num4);
			ref byte ptr = ref this.grid[num2, num];
			ptr |= b;
			byte b2 = 0;
			if (b == this.W)
			{
				b2 = this.E;
			}
			if (b == this.E)
			{
				b2 = this.W;
			}
			if (b == this.N)
			{
				b2 = this.S;
			}
			if (b == this.S)
			{
				b2 = this.N;
			}
			ref byte ptr2 = ref this.grid[num4, num3];
			ptr2 |= b2;
			this.Mark(num, num2);
		}
	}

	// Token: 0x04000394 RID: 916
	public AudioClip bushRustleClip;

	// Token: 0x04000395 RID: 917
	public GameObject hedgePrefab;

	// Token: 0x04000396 RID: 918
	public BoardNode entranceBoardNode;

	// Token: 0x04000397 RID: 919
	public BoardNode endBoardNode;

	// Token: 0x04000398 RID: 920
	public BoardNode[] nodes;

	// Token: 0x04000399 RID: 921
	public Transform nodeRoot;

	// Token: 0x0400039A RID: 922
	public bool delaySpawn = true;

	// Token: 0x0400039B RID: 923
	public float spawnDelayTime = 0.01f;

	// Token: 0x0400039C RID: 924
	public bool removeBottomLeft = true;

	// Token: 0x0400039D RID: 925
	public bool removeBottomRight = true;

	// Token: 0x0400039E RID: 926
	public bool removeMiddle;

	// Token: 0x0400039F RID: 927
	public float scale = 1f;

	// Token: 0x040003A0 RID: 928
	public int width = 10;

	// Token: 0x040003A1 RID: 929
	public int height = 10;

	// Token: 0x040003A2 RID: 930
	public float gridSize = 5f;

	// Token: 0x040003A3 RID: 931
	public bool isBoard = true;

	// Token: 0x040003A4 RID: 932
	private byte[,] grid;

	// Token: 0x040003A5 RID: 933
	private byte N = 1;

	// Token: 0x040003A6 RID: 934
	private byte S = 2;

	// Token: 0x040003A7 RID: 935
	private byte E = 4;

	// Token: 0x040003A8 RID: 936
	private byte W = 8;

	// Token: 0x040003A9 RID: 937
	private byte IN = 16;

	// Token: 0x040003AA RID: 938
	private byte FRONTIER = 32;

	// Token: 0x040003AB RID: 939
	private float sX;

	// Token: 0x040003AC RID: 940
	private float sY;

	// Token: 0x040003AD RID: 941
	private List<Vector2> frontier = new List<Vector2>();

	// Token: 0x040003AE RID: 942
	private HedgeMazeGenerator.GridSpace[,] spaces;

	// Token: 0x040003AF RID: 943
	private LTDescr[,] tweens;

	// Token: 0x040003B0 RID: 944
	private System.Random rand;

	// Token: 0x040003B1 RID: 945
	private Vector3 size = new Vector3(1f, 0f, 1f);

	// Token: 0x040003B2 RID: 946
	public AnimationCurve curve;

	// Token: 0x040003B3 RID: 947
	public AnimationCurve inCurve;

	// Token: 0x020000AB RID: 171
	private struct GridSpace
	{
		// Token: 0x06000399 RID: 921 RVA: 0x00005F2F File Offset: 0x0000412F
		public GridSpace(GameObject east, GameObject south)
		{
			this.s = south.transform.GetChild(0).gameObject;
			this.e = east.transform.GetChild(0).gameObject;
		}

		// Token: 0x040003B4 RID: 948
		public GameObject s;

		// Token: 0x040003B5 RID: 949
		public GameObject e;
	}
}
