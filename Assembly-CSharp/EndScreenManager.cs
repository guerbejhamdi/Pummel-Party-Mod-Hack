using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000050 RID: 80
public class EndScreenManager : NetBehaviour
{
	// Token: 0x1700002A RID: 42
	// (get) Token: 0x06000152 RID: 338 RVA: 0x000046DF File Offset: 0x000028DF
	public Transform Root
	{
		get
		{
			return this.root;
		}
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x06000153 RID: 339 RVA: 0x000046E7 File Offset: 0x000028E7
	// (set) Token: 0x06000154 RID: 340 RVA: 0x000046EF File Offset: 0x000028EF
	public EndScreenManager.EndScreenState CurEndScreenState { get; set; }

	// Token: 0x06000155 RID: 341 RVA: 0x000046F8 File Offset: 0x000028F8
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.rand = new System.Random(UnityEngine.Random.Range(0, int.MaxValue));
		NetSystem.SetSendRate(60, 60);
		GameManager.Board.endScreenManager = this;
		base.StartCoroutine(this.Spawnplayers());
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00004737 File Offset: 0x00002937
	private IEnumerator Spawnplayers()
	{
		GameObject g = null;
		while (g == null)
		{
			g = GameObject.Find("EndScreenParent");
			yield return null;
		}
		this.root = GameObject.Find("EndScreenParent").transform;
		if (NetSystem.IsServer)
		{
			EndScreenPlacementController endScreenPlacementController = UnityEngine.Object.FindObjectOfType<EndScreenPlacementController>();
			Transform[] spawnPoints = endScreenPlacementController.SpawnPoints;
			Transform transform = this.root.Find("LastPlaceSpawn");
			for (int i = 0; i < GameManager.GetPlayerCount(); i++)
			{
				GamePlayer playerAt = GameManager.GetPlayerAt(i);
				Transform transform2;
				if ((int)playerAt.Placement == GameManager.GetPlayerCount() - 1)
				{
					transform2 = transform;
				}
				else
				{
					transform2 = spawnPoints[(int)playerAt.Placement];
					if (endScreenPlacementController != null)
					{
						endScreenPlacementController.SetPlacementActive((int)playerAt.Placement, true);
					}
				}
				NetSystem.Spawn("EndScreenPlayer", transform2.position, transform2.rotation, (ushort)i, playerAt.NetOwner);
			}
		}
		yield break;
	}

	// Token: 0x06000157 RID: 343 RVA: 0x000311AC File Offset: 0x0002F3AC
	public void StartIntro()
	{
		this.stateTimer.SetInterval(5f, true);
		this.CurEndScreenState = EndScreenManager.EndScreenState.Intro;
		string text = ColorUtility.ToHtmlStringRGBA(this.placements[0].Color.uiColor);
		string translation = LocalizationManager.GetTranslation("Wins", true, 0, true, false, null, null, true);
		string text2 = string.Concat(new string[]
		{
			"<color=#",
			text,
			">",
			this.placements[0].Name,
			"</color> ",
			translation,
			"!"
		});
		GameManager.UIController.ShowLargeText(text2, LargeTextType.PlayerWins, 99999f, false, false);
	}

	// Token: 0x06000158 RID: 344 RVA: 0x0003125C File Offset: 0x0002F45C
	public void SetClientLoaded(ushort netUserID, bool sendRPC = false)
	{
		if (sendRPC)
		{
			base.SendRPC("ClientReady", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				netUserID
			});
		}
		List<GamePlayer> playerList = GameManager.PlayerList;
		for (int i = 0; i < playerList.Count; i++)
		{
			if (playerList[i].NetOwner.UserID == netUserID && !this.loadedPlayers.Contains(playerList[i].GlobalID))
			{
				this.loadedPlayers.Add(playerList[i].GlobalID);
			}
		}
	}

	// Token: 0x06000159 RID: 345 RVA: 0x000312E4 File Offset: 0x0002F4E4
	public bool AllClientsLoaded()
	{
		List<GamePlayer> playerList = GameManager.PlayerList;
		for (int i = 0; i < playerList.Count; i++)
		{
			if (!this.loadedPlayers.Contains(playerList[i].GlobalID))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600015A RID: 346 RVA: 0x00031324 File Offset: 0x0002F524
	public void Update()
	{
		switch (this.CurEndScreenState)
		{
		case EndScreenManager.EndScreenState.Loading:
		case EndScreenManager.EndScreenState.Playing:
			break;
		case EndScreenManager.EndScreenState.Intro:
			if (this.stateTimer.Elapsed(false))
			{
				this.CurEndScreenState = EndScreenManager.EndScreenState.Playing;
				if (GameManager.partyGameMode == PartyGameMode.BoardGame)
				{
					GameManager.UIController.ShowScoreBoard();
				}
				for (int i = 0; i < this.players.Count; i++)
				{
					this.players[i].StopPlacementAnim();
				}
				UnityEngine.Object.Instantiate(Resources.Load("UI/StatUI"));
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x0600015B RID: 347 RVA: 0x00004746 File Offset: 0x00002946
	public bool Playable
	{
		get
		{
			return this.CurEndScreenState == EndScreenManager.EndScreenState.Playing;
		}
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x0600015C RID: 348 RVA: 0x000313A8 File Offset: 0x0002F5A8
	public int PlayersAlive
	{
		get
		{
			int num = 0;
			for (int i = 0; i < this.players.Count; i++)
			{
				if (!this.players[i].IsDead)
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x0600015D RID: 349 RVA: 0x00004751 File Offset: 0x00002951
	[NetRPC(true, NetRPCSecurity.ALL, NetRPCSecurity.ALL)]
	public void ClientReady(NetPlayer sender, ushort playerID)
	{
		this.SetClientLoaded(playerID, false);
	}

	// Token: 0x0600015E RID: 350 RVA: 0x000313E8 File Offset: 0x0002F5E8
	public Vector3 GetRandomNavMeshPoint()
	{
		if (this.binaryTree == null)
		{
			this.triangulation = NavMesh.CalculateTriangulation();
			if (this.triangulation.vertices.Length != 0)
			{
				List<float> list = new List<float>();
				for (int i = 0; i < this.triangulation.indices.Length / 3; i++)
				{
					int num = i * 3;
					Vector3 vector = this.triangulation.vertices[this.triangulation.indices[num]];
					Vector3 vector2 = this.triangulation.vertices[this.triangulation.indices[num + 1]];
					Vector3 vector3 = this.triangulation.vertices[this.triangulation.indices[num + 2]];
					float num2 = Vector3.Distance(vector, vector2);
					float num3 = Vector3.Distance(vector2, vector3);
					float num4 = Vector3.Distance(vector3, vector);
					float num5 = (num2 + num3 + num4) / 2f;
					float num6 = Mathf.Sqrt(num5 * (num5 - num2) * (num5 - num3) * (num5 - num4));
					list.Add(this.totalArea);
					this.totalArea += num6;
				}
				this.binaryTree = new BinaryTree(list.ToArray());
			}
		}
		if (this.binaryTree != null)
		{
			float p = ZPMath.RandomFloat(this.rand, 0f, this.totalArea);
			int num7 = this.binaryTree.FindPoint(p) * 3;
			Vector3[] vertices = this.triangulation.vertices;
			int[] indices = this.triangulation.indices;
			return ZPMath.RandomTrianglePoint(vertices[indices[num7]], vertices[indices[num7 + 1]], vertices[indices[num7 + 2]], this.rand);
		}
		return Vector3.zero;
	}

	// Token: 0x040001AF RID: 431
	public List<EndScreenPlayer> players = new List<EndScreenPlayer>();

	// Token: 0x040001B0 RID: 432
	public List<GamePlayer> placements;

	// Token: 0x040001B1 RID: 433
	private List<short> loadedPlayers = new List<short>();

	// Token: 0x040001B2 RID: 434
	private Transform root;

	// Token: 0x040001B4 RID: 436
	private ActionTimer stateTimer = new ActionTimer(0.1f);

	// Token: 0x040001B5 RID: 437
	private NavMeshTriangulation triangulation;

	// Token: 0x040001B6 RID: 438
	private BinaryTree binaryTree;

	// Token: 0x040001B7 RID: 439
	private float totalArea;

	// Token: 0x040001B8 RID: 440
	private System.Random rand;

	// Token: 0x02000051 RID: 81
	public enum EndScreenState
	{
		// Token: 0x040001BA RID: 442
		Loading,
		// Token: 0x040001BB RID: 443
		Intro,
		// Token: 0x040001BC RID: 444
		Playing
	}
}
