using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using I2.Loc;
using Prime31.TransitionKit;
using Rewired;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using ZP.Net;
using ZP.Utility;

// Token: 0x020003D9 RID: 985
public class GameBoardController : NetBehaviour
{
	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x06001A82 RID: 6786 RVA: 0x00013982 File Offset: 0x00011B82
	// (set) Token: 0x06001A83 RID: 6787 RVA: 0x0001398A File Offset: 0x00011B8A
	public BoardGoalBase[] GoalScript { get; private set; }

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x06001A84 RID: 6788 RVA: 0x00013993 File Offset: 0x00011B93
	// (set) Token: 0x06001A85 RID: 6789 RVA: 0x0001399B File Offset: 0x00011B9B
	public FakeChestController[] FakeChestControllers { get; private set; }

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x06001A86 RID: 6790 RVA: 0x000139A4 File Offset: 0x00011BA4
	// (set) Token: 0x06001A87 RID: 6791 RVA: 0x000139AC File Offset: 0x00011BAC
	public WeaponGoal[] WeaponGoalScript { get; private set; }

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x06001A88 RID: 6792 RVA: 0x000139B5 File Offset: 0x00011BB5
	// (set) Token: 0x06001A89 RID: 6793 RVA: 0x000139BD File Offset: 0x00011BBD
	public bool[] goalIsReal { get; set; }

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x06001A8A RID: 6794 RVA: 0x000139C6 File Offset: 0x00011BC6
	// (set) Token: 0x06001A8B RID: 6795 RVA: 0x000139CE File Offset: 0x00011BCE
	public List<PersistentItem> PersistentItems
	{
		get
		{
			return this.persistentItems;
		}
		set
		{
			this.persistentItems = value;
		}
	}

	// Token: 0x06001A8C RID: 6796 RVA: 0x000139D7 File Offset: 0x00011BD7
	public void AddPersistentItem(PersistentItem item)
	{
		this.persistentItems.Add(item);
	}

	// Token: 0x06001A8D RID: 6797 RVA: 0x000139E5 File Offset: 0x00011BE5
	public void RemovePersistentItem(PersistentItem item)
	{
		this.persistentItems.Remove(item);
	}

	// Token: 0x170002ED RID: 749
	// (get) Token: 0x06001A8E RID: 6798 RVA: 0x000139F4 File Offset: 0x00011BF4
	public GameBoardState BoardState
	{
		get
		{
			return this.curBoardState;
		}
	}

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x06001A8F RID: 6799 RVA: 0x000139FC File Offset: 0x00011BFC
	public GameBoardCamera Camera
	{
		get
		{
			return this.boardCamera;
		}
	}

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x06001A90 RID: 6800 RVA: 0x00013A04 File Offset: 0x00011C04
	public BoardPlayer CurPlayer
	{
		get
		{
			return this.curPlayer;
		}
	}

	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x06001A91 RID: 6801 RVA: 0x00013A0C File Offset: 0x00011C0C
	public BoardNode[] BoardNodes
	{
		get
		{
			return this.boardNodes;
		}
	}

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x06001A92 RID: 6802 RVA: 0x00013A14 File Offset: 0x00011C14
	public BoardNode BoardStartNode
	{
		get
		{
			return this.startNode;
		}
	}

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x06001A93 RID: 6803 RVA: 0x00013A1C File Offset: 0x00011C1C
	public Vector3 PlayerCamOffset
	{
		get
		{
			return this.playerCamOffset;
		}
	}

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x06001A94 RID: 6804 RVA: 0x00013A24 File Offset: 0x00011C24
	public GameMap CurrentMap
	{
		get
		{
			return this.currentMap;
		}
	}

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x06001A95 RID: 6805 RVA: 0x00013A2C File Offset: 0x00011C2C
	public BoardNode[] GoalNode
	{
		get
		{
			return this.goalNodes;
		}
	}

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x06001A96 RID: 6806 RVA: 0x00013A34 File Offset: 0x00011C34
	public int CurnTurnNum
	{
		get
		{
			return this.curTurnNum;
		}
	}

	// Token: 0x06001A97 RID: 6807 RVA: 0x00013A3C File Offset: 0x00011C3C
	public int GetActorCount()
	{
		return this.boardActors.Count;
	}

	// Token: 0x06001A98 RID: 6808 RVA: 0x00013A49 File Offset: 0x00011C49
	public void AddToMyPlayerList(BoardPlayer b)
	{
		this.myPlayers.Add(b);
	}

	// Token: 0x06001A99 RID: 6809 RVA: 0x00013A57 File Offset: 0x00011C57
	public BoardActor GetActor(int id)
	{
		return this.GetActor((byte)id);
	}

	// Token: 0x06001A9A RID: 6810 RVA: 0x000AFE9C File Offset: 0x000AE09C
	public BoardActor GetActor(byte id)
	{
		foreach (BoardActor boardActor in this.boardActors)
		{
			if (boardActor.ActorID == id)
			{
				return boardActor;
			}
		}
		return null;
	}

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x06001A9B RID: 6811 RVA: 0x00013A61 File Offset: 0x00011C61
	public List<BoardPlayer> BoardPlayers
	{
		get
		{
			return this.boardPlayerList;
		}
	}

	// Token: 0x06001A9C RID: 6812 RVA: 0x00013A69 File Offset: 0x00011C69
	public void PlayBoardMusic()
	{
		AudioSystem.PlayMusic(GameManager.mapSettings.music, 0.5f, GameManager.mapSettings.musicVol);
	}

	// Token: 0x06001A9D RID: 6813 RVA: 0x000AFEF8 File Offset: 0x000AE0F8
	public override void OnNetInitialize()
	{
		if (GameManager.partyGameMode == PartyGameMode.BoardGame)
		{
			base.StartCoroutine(this.CreateBoardModifiers());
		}
		GameManager.SetGameBoard(this);
		GameManager.Initialize(GameState.GameBoard);
		GameManager.FinishedLoading();
		this.minigameIntroScene = UnityEngine.Object.Instantiate<GameObject>(this.minigameIntroScenePrefab);
		if (GameManager.partyGameMode != PartyGameMode.MinigamesOnly)
		{
			this.minigameIntroScene.SetActive(false);
		}
		this.resultScreenScene = GameObject.Find("ResultScreenScene");
		this.uiController = GameManager.UIController;
		GameManager.scoreUIScene.Setup();
		if (GameManager.partyGameMode == PartyGameMode.BoardGame)
		{
			AudioSystem.PlayMusic(GameManager.mapSettings.music, 0.5f, GameManager.mapSettings.musicVol);
			AudioSystem.PlayAmbient(GameManager.mapSettings.ambience, 0.5f, GameManager.mapSettings.ambienceVol);
			for (int i = 0; i < GameManager.ItemList.items.Length; i++)
			{
				GameManager.ItemList.items[i].prefab = Resources.Load<GameObject>(GameManager.ItemList.items[i].prefabPath);
				GameManager.ItemList.items[i].recievePrefab = Resources.Load<GameObject>(GameManager.ItemList.items[i].recievePrefabPath);
				if (!GameManager.ItemList.items[i].heldPrefabPath.Equals(""))
				{
					GameManager.ItemList.items[i].heldPrefab = Resources.Load<GameObject>(GameManager.ItemList.items[i].heldPrefabPath);
				}
			}
		}
	}

	// Token: 0x06001A9E RID: 6814 RVA: 0x00013A89 File Offset: 0x00011C89
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSetSeedAndMap(NetPlayer sender, int seed, int mapIndex)
	{
		this.randomMapSeed = seed;
		this.randomMapRand = new System.Random(seed);
		this.randomMapIndex = mapIndex;
		this.gotRandMapSeed = true;
	}

	// Token: 0x06001A9F RID: 6815 RVA: 0x000B006C File Offset: 0x000AE26C
	private void SetNodeToRecruit(BoardNode node)
	{
		node.baseNodeType = BoardNodeType.Recruit;
		node.CurrentNodeType = BoardNodeType.Recruit;
		node.nodeEventType = BoardNodeEventType.Custom;
		node.nodeEvent = GameObject.Find("BoardSquares").GetComponent<RecruitEventManager>();
		node.hasInteraction = false;
		node.interactionScript = null;
		node.CurHasInteraction = false;
		node.CurInteractionScript = null;
	}

	// Token: 0x06001AA0 RID: 6816 RVA: 0x00013AAC File Offset: 0x00011CAC
	private bool ValidGraveyardNodeType(BoardNodeType type)
	{
		return type != BoardNodeType.Graveyard && type != BoardNodeType.Pathing && type != BoardNodeType.Recruit && type != BoardNodeType.BoardStart;
	}

	// Token: 0x06001AA1 RID: 6817 RVA: 0x000B00C4 File Offset: 0x000AE2C4
	private bool ValidNode(BoardNode startNode)
	{
		if (startNode.nodeConnections.Length > 2 || !this.ValidGraveyardNodeType(startNode.baseNodeType))
		{
			return false;
		}
		BoardNode boardNode = startNode.GetForwardNodes(null, false)[0];
		if (boardNode.nodeConnections.Length > 2 || !this.ValidGraveyardNodeType(boardNode.baseNodeType))
		{
			return false;
		}
		BoardNode boardNode2 = boardNode.GetForwardNodes(null, false)[0];
		if (!this.ValidGraveyardNodeType(boardNode2.baseNodeType))
		{
			return false;
		}
		BoardNode boardNode3 = null;
		for (int i = 0; i < startNode.nodeConnections.Length; i++)
		{
			if (startNode.nodeConnections[i].connection_type == BoardNodeConnectionDirection.Back)
			{
				boardNode3 = startNode.nodeConnections[i].node;
			}
		}
		return this.ValidGraveyardNodeType(boardNode3.baseNodeType);
	}

	// Token: 0x06001AA2 RID: 6818 RVA: 0x00013AC4 File Offset: 0x00011CC4
	private IEnumerator CreateBoardModifiers()
	{
		Debug.LogError("CreateBoardModifiers - Start");
		if (NetSystem.IsServer)
		{
			Debug.Log("Create Board Modifiers");
			List<BoardModifier> activeModifiers = BoardModifier.ActiveModifiers;
			int modifierSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
			if (activeModifiers.Count > 0)
			{
				this.SetupBoardModifiers(activeModifiers, modifierSeed);
			}
			else
			{
				this.SkipBoardModifiers();
			}
		}
		Debug.LogError("CreateBoardModifiers - Waiting");
		yield return new WaitUntil(() => this.modifiersSetup);
		Debug.LogError("CreateBoardModifiers - Done");
		if (GameManager.CurMap.sceneName == "IslandsMap_Scene")
		{
			base.StartCoroutine(this.SetupRandomMap());
		}
		else
		{
			this.SetupBoardMode();
			this.DoStarts();
		}
		yield break;
	}

	// Token: 0x06001AA3 RID: 6819 RVA: 0x000B017C File Offset: 0x000AE37C
	private void SetupBoardModifiers(List<BoardModifier> modifiers, int modifierSeed)
	{
		if (NetSystem.IsServer)
		{
			ushort[] array = new ushort[modifiers.Count];
			for (int i = 0; i < modifiers.Count; i++)
			{
				array[i] = (ushort)modifiers[i].GetGameModifierID();
			}
			base.SendRPC("RPCSetupBoardModifiers", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				array,
				modifierSeed
			});
		}
		else
		{
			BoardModifier.ActiveModifiers = modifiers;
		}
		this.m_modifiers = modifiers;
		this.m_modifierDefinitions = new List<GameModifierDefinition>();
		for (int j = 0; j < modifiers.Count; j++)
		{
			int gameModifierID = modifiers[j].GetGameModifierID();
			Debug.Log("Initializing Board Modifier : " + modifiers[j].GetGameModifierID().ToString());
			modifiers[j].BoardPreInitialize(this);
			GameModifierDefinition modifierByID = GameManager.GetModifierByID(gameModifierID);
			if (modifierByID != null)
			{
				this.m_modifierDefinitions.Add(modifierByID);
			}
		}
		if (GameManager.modifierUI == null)
		{
			GameManager.modifierUI = (UnityEngine.Object.Instantiate(Resources.Load("ModifierUI")) as GameObject).GetComponent<ModifierUI>();
			GameManager.modifierUI.Show();
		}
		if (GameManager.modifierUI != null)
		{
			GameManager.modifierUI.SetModifiers(this.m_modifierDefinitions);
		}
		else
		{
			Debug.LogError("Modifier UI is not create yet");
		}
		this.modifiersSetup = true;
	}

	// Token: 0x06001AA4 RID: 6820 RVA: 0x00013AD3 File Offset: 0x00011CD3
	private void SkipBoardModifiers()
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSkipBoardModifiers", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		BoardModifier.ActiveModifiers = new List<BoardModifier>();
		this.modifiersSetup = true;
	}

	// Token: 0x06001AA5 RID: 6821 RVA: 0x000B02C4 File Offset: 0x000AE4C4
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSetupBoardModifiers(NetPlayer sender, ushort[] modifiers, int seed)
	{
		GameRuleset activeRuleset = GameManager.RulesetManager.ActiveRuleset;
		if (activeRuleset == null || activeRuleset.Modifiers == null)
		{
			Debug.LogError("Unable to setup board modifiers on client, ruleset or modifiers null");
			Debug.LogError((activeRuleset == null).ToString() + ", " + (activeRuleset.Modifiers == null).ToString());
			return;
		}
		List<BoardModifier> list = new List<BoardModifier>();
		for (int i = 0; i < modifiers.Length; i++)
		{
			BoardModifier boardModifier = (BoardModifier)activeRuleset.Modifiers.CreateModifierInstance(modifiers[i]);
			if (boardModifier != null)
			{
				list.Add(boardModifier);
			}
			else
			{
				Debug.LogError("Failed to create modifier on client " + modifiers[i].ToString());
			}
		}
		this.SetupBoardModifiers(list, seed);
	}

	// Token: 0x06001AA6 RID: 6822 RVA: 0x00013AFE File Offset: 0x00011CFE
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSkipBoardModifiers(NetPlayer sender)
	{
		this.SkipBoardModifiers();
	}

	// Token: 0x06001AA7 RID: 6823 RVA: 0x00013B06 File Offset: 0x00011D06
	private IEnumerator SetupRandomMap()
	{
		GameObject mapObj = GameObject.Find("BoardSquares");
		if (GameManager.CurMap.sceneName == "IslandsMap_Scene")
		{
			GameBoardController.<>c__DisplayClass123_0 CS$<>8__locals1 = new GameBoardController.<>c__DisplayClass123_0();
			if (NetSystem.IsServer)
			{
				List<int> list = new List<int>();
				int num = int.MaxValue;
				for (int i = 1; i < 9; i++)
				{
					num = Mathf.Min(num, RBPrefs.GetInt("RandomMapPlayCount_" + i.ToString(), 0));
				}
				for (int j = 1; j < 9; j++)
				{
					if (RBPrefs.GetInt("RandomMapPlayCount_" + j.ToString(), 0) <= num)
					{
						list.Add(j);
					}
				}
				this.randomMapIndex = ((GameManager.SaveToLoad != null) ? ((int)GameManager.SaveToLoad.randomMapIndex) : list[GameManager.rand.Next(0, list.Count)]);
				this.randomMapSeed = ((GameManager.SaveToLoad != null) ? GameManager.SaveToLoad.randomMapSeed : GameManager.rand.Next());
				this.randomMapRand = new System.Random(this.randomMapSeed);
				base.SendRPC("RPCSetSeedAndMap", NetRPCDelivery.RELIABLE_ORDERED, new object[]
				{
					this.randomMapSeed,
					this.randomMapIndex
				});
			}
			else
			{
				yield return new WaitUntil(() => this.gotRandMapSeed);
			}
			RBPrefs.SetInt("RandomMapPlayCount_" + this.randomMapIndex.ToString(), RBPrefs.GetInt("RandomMapPlayCount_" + this.randomMapIndex.ToString(), 0) + 1);
			CS$<>8__locals1.op = SceneManager.LoadSceneAsync("IslandAdditive_" + this.randomMapIndex.ToString(), LoadSceneMode.Additive);
			yield return new WaitUntil(() => CS$<>8__locals1.op.isDone);
			GameObject gameObject = GameObject.Find("AdditiveRoot");
			UnityEngine.Object.Destroy(gameObject.GetComponent<GameMap>());
			gameObject.transform.parent = GameObject.Find("BoardWorld").transform;
			mapObj.GetComponent<GameMap>().mapExtents = gameObject.GetComponent<GameMap>().mapExtents;
			UnityEngine.Object.Destroy(gameObject.GetComponent<GameMap>());
			BoardNode[] componentsInChildren = gameObject.GetComponentsInChildren<BoardNode>();
			Transform transform = gameObject.transform.Find("Graveyards");
			while (transform.childCount > 0)
			{
				RandomMapGraveyard component = transform.GetChild(0).GetComponent<RandomMapGraveyard>();
				BoardNode component2 = transform.GetChild(0).GetComponent<BoardNode>();
				this.DoThing(component.targetNode, component2, BoardNodeTransition.Walking);
				transform.GetChild(0).parent = mapObj.transform;
			}
			IslandNode[] componentsInChildren2 = gameObject.GetComponentsInChildren<IslandNode>();
			for (int k = 0; k < componentsInChildren2.Length; k++)
			{
				for (int l = 0; l < componentsInChildren2[k].islandConnections.Length; l++)
				{
					this.DoThing(componentsInChildren2[k].islandConnections[l].end, componentsInChildren2[k].islandConnections[l].start, componentsInChildren2[k].islandConnections[l].transition);
				}
				if (componentsInChildren2[k].startPoint != null)
				{
					componentsInChildren2[k].startPoint.baseNodeType = BoardNodeType.BoardStart;
					componentsInChildren2[k].startPoint.CurrentNodeType = BoardNodeType.BoardStart;
					componentsInChildren2[k].startPoint.nodeEventType = BoardNodeEventType.None;
					componentsInChildren2[k].startPoint.nodeEvent = null;
				}
			}
			IslandNode[] componentsInChildren3 = gameObject.GetComponentsInChildren<IslandNode>();
			for (int m = 0; m < componentsInChildren3.Length; m++)
			{
				componentsInChildren3[m].DoRandom(this.randomMapRand);
			}
			int num2 = 0;
			for (int n = 0; n < componentsInChildren.Length; n++)
			{
				if (componentsInChildren[n].baseNodeType != BoardNodeType.Graveyard && componentsInChildren[n].baseNodeType != BoardNodeType.Pathing)
				{
					num2++;
				}
			}
			int num3 = (int)((float)num2 * 0.1641791f);
			for (int num4 = 0; num4 < num3; num4++)
			{
				int num5 = 300;
				for (int num6 = 0; num6 < num5; num6++)
				{
					int num7 = this.randomMapRand.Next(0, componentsInChildren.Length);
					if (this.ValidNode(componentsInChildren[num7]))
					{
						this.SetNodeToRecruit(componentsInChildren[num7].GetForwardNodes(null, false)[0]);
						this.SetNodeToRecruit(componentsInChildren[num7]);
						break;
					}
				}
			}
			BoardNodeType[] array = new BoardNodeType[]
			{
				BoardNodeType.BigHeal,
				BoardNodeType.BigGold,
				BoardNodeType.Hazard,
				BoardNodeType.Item,
				BoardNodeType.MapEvent
			};
			BinaryTree binaryTree = new BinaryTree(new float[]
			{
				0f,
				0.26666668f,
				0.51111114f,
				0.7111111f,
				0.95555556f
			});
			int num8 = 0;
			List<BoardNode> list2 = new List<BoardNode>();
			List<BoardNode> list3 = new List<BoardNode>();
			for (int num9 = 0; num9 < componentsInChildren.Length; num9++)
			{
				list3.Add(componentsInChildren[num9]);
			}
			for (int num10 = 0; num10 < componentsInChildren.Length; num10++)
			{
				int index = this.randomMapRand.Next(0, list3.Count);
				list2.Add(list3[index]);
				list3.RemoveAt(index);
			}
			for (int num11 = 0; num11 < list2.Count; num11++)
			{
				if (list2[num11].baseNodeType != BoardNodeType.Graveyard)
				{
					list2[num11].yRotation += list2[num11].transform.rotation.eulerAngles.y;
					Vector3 forward = list2[num11].transform.forward;
					forward.y = 0f;
					forward.Normalize();
					float num12 = Vector3.SignedAngle(-Vector3.forward, forward, Vector3.up);
					list2[num11].transform.Rotate(Vector3.up, -num12);
					if (list2[num11].baseNodeType != BoardNodeType.Graveyard && list2[num11].baseNodeType != BoardNodeType.Pathing && list2[num11].baseNodeType != BoardNodeType.Recruit && list2[num11].baseNodeType != BoardNodeType.BoardStart)
					{
						BoardNodeType boardNodeType = array[binaryTree.FindPoint((float)this.randomMapRand.NextDouble())];
						if (num8 < 2)
						{
							boardNodeType = BoardNodeType.MapEvent;
							num8++;
						}
						list2[num11].baseNodeType = boardNodeType;
						list2[num11].CurrentNodeType = boardNodeType;
						list2[num11].nodeEventType = BoardNodeEventType.None;
						list2[num11].nodeEvent = null;
						list2[num11].hasInteraction = false;
						list2[num11].interactionScript = null;
						list2[num11].CurHasInteraction = false;
						list2[num11].CurInteractionScript = null;
						switch (boardNodeType)
						{
						case BoardNodeType.BigHeal:
							list2[num11].nodeEventType = BoardNodeEventType.Simple;
							list2[num11].eventAction = NodeEventAction.GiveHealth;
							break;
						case BoardNodeType.BigGold:
							list2[num11].nodeEventType = BoardNodeEventType.Simple;
							list2[num11].eventAction = NodeEventAction.GiveGold;
							break;
						case BoardNodeType.Hazard:
							list2[num11].nodeEventType = BoardNodeEventType.Custom;
							list2[num11].nodeEvent = GameObject.Find("BoardWorld").transform.Find("Events/RandomFlyingObjectEvent").gameObject.GetComponent<RandomFlyingObjectEvent>();
							break;
						case BoardNodeType.Item:
							list2[num11].nodeEventType = BoardNodeEventType.Simple;
							list2[num11].eventAction = NodeEventAction.GiveItem;
							break;
						case BoardNodeType.MapEvent:
							list2[num11].nodeEventType = BoardNodeEventType.Custom;
							list2[num11].nodeEvent = GameObject.Find("BoardWorld").transform.Find("Events/ReverseEvent").gameObject.GetComponent<ReverseMapEvent>();
							break;
						}
					}
				}
				list2[num11].transform.parent = mapObj.transform;
			}
			GameObject original = (GameObject)Resources.Load("RandomMapArrow");
			Transform parent = gameObject.transform.Find("Arrows");
			for (int num13 = 0; num13 < componentsInChildren.Length; num13++)
			{
				List<BoardNodeConnection> list4 = new List<BoardNodeConnection>();
				for (int num14 = 0; num14 < componentsInChildren[num13].nodeConnections.Length; num14++)
				{
					if (componentsInChildren[num13].nodeConnections[num14].node.baseNodeType != BoardNodeType.Graveyard)
					{
						list4.Add(componentsInChildren[num13].nodeConnections[num14]);
					}
				}
				if (list4.Count > 2)
				{
					for (int num15 = 0; num15 < list4.Count; num15++)
					{
						object obj = (list4[num15].connection_type == BoardNodeConnectionDirection.Forward) ? componentsInChildren[num13].NodePosition : list4[num15].node.NodePosition;
						Vector3 b = (list4[num15].connection_type == BoardNodeConnectionDirection.Forward) ? list4[num15].node.NodePosition : componentsInChildren[num13].NodePosition;
						object a = obj;
						Vector3 position = (a + b) / 2f;
						Quaternion rotation = Quaternion.LookRotation(a - b) * Quaternion.Euler(90f, 0f, 0f);
						UnityEngine.Object.Instantiate<GameObject>(original, position, rotation, parent);
					}
				}
			}
			CS$<>8__locals1 = null;
		}
		this.SetupBoardMode();
		this.DoStarts();
		yield break;
	}

	// Token: 0x06001AA8 RID: 6824 RVA: 0x000B0378 File Offset: 0x000AE578
	private void DoThing(BoardNode forward, BoardNode back, BoardNodeTransition transitionType)
	{
		BoardNodeConnection[] array = new BoardNodeConnection[forward.nodeConnections.Length + 1];
		for (int i = 0; i < forward.nodeConnections.Length; i++)
		{
			array[i] = forward.nodeConnections[i];
		}
		array[array.Length - 1] = new BoardNodeConnection
		{
			connection_type = BoardNodeConnectionDirection.Back,
			node = back,
			transition = transitionType
		};
		forward.nodeConnections = array;
		BoardNodeConnection[] array2 = new BoardNodeConnection[back.nodeConnections.Length + 1];
		for (int j = 0; j < back.nodeConnections.Length; j++)
		{
			array2[j] = back.nodeConnections[j];
		}
		array2[array2.Length - 1] = new BoardNodeConnection
		{
			connection_type = BoardNodeConnectionDirection.Forward,
			node = forward,
			transition = transitionType
		};
		back.nodeConnections = array2;
	}

	// Token: 0x06001AA9 RID: 6825 RVA: 0x000B0434 File Offset: 0x000AE634
	private void SetupBoardMode()
	{
		GameObject gameObject = GameObject.Find("BoardSquares");
		GameObject gameObject2 = GameObject.Find("BoardCamera");
		this.boardCamera = gameObject2.GetComponent<GameBoardCamera>();
		if (gameObject == null)
		{
			Debug.LogError("Unable to find GameMap object in scene, this object is required!!");
		}
		else
		{
			this.currentMap = gameObject.GetComponent<GameMap>();
		}
		this.currentMap.SetupMap();
		this.startNode = this.currentMap.GetStartNode();
		this.boardNodes = this.currentMap.GetBoardNodes();
		this.mainBoardEvent = this.currentMap.mainBoardEvent;
		this.potentialGoalBoardNodes = new List<BoardNode>();
		foreach (BoardNode boardNode in this.boardNodes)
		{
			if (boardNode.GetForwardNodes(null, false).Count <= 1 && boardNode.CurrentNodeType != BoardNodeType.Pathing && boardNode.CurrentNodeType != BoardNodeType.Graveyard && !boardNode.blockGoalSpawn)
			{
				this.potentialGoalBoardNodes.Add(boardNode);
			}
		}
		this.goalBoardNodesTemp = new GameBoardController.PotentialNode[this.potentialGoalBoardNodes.Count];
		this.boardCamera.MoveToInstant(this.startNode.transform, new Vector3(0f, -0.25f, 0f));
		this.directionChoicePrefab = Resources.Load<GameObject>("Prefabs/DirectionChoiceArrow");
		this.isHalloweenMap = (GameManager.CurMap.name == "HalloweenMapName");
		this.isWinterMap = (GameManager.CurMap.name == "WinterMapName");
		this.areFakeChestsActive = BoardModifier.IsBoardModifierActive(BoardModifierID.FakeChests);
		if (this.isHalloweenMap && !this.areFakeChestsActive)
		{
			this.goalObjPrefab = Resources.Load<GameObject>("GoalAssets/PrisonerCage");
		}
		else if (this.areFakeChestsActive)
		{
			this.goalObjPrefab = Resources.Load<GameObject>("GoalAssets/FakeChest_ShellGame");
		}
		else
		{
			this.goalObjPrefab = Resources.Load<GameObject>("GoalAssets/TreasureChest/TreasureChest");
		}
		this.weaponObjPrefab = Resources.Load<GameObject>("GoalAssets/WeaponSpace/WeaponGoal");
	}

	// Token: 0x06001AAA RID: 6826 RVA: 0x00013B15 File Offset: 0x00011D15
	public void Start()
	{
		StatTracker.ResetStats();
		if (GameManager.partyGameMode == PartyGameMode.MinigamesOnly)
		{
			this.DoStarts();
		}
	}

	// Token: 0x06001AAB RID: 6827 RVA: 0x00013B2A File Offset: 0x00011D2A
	public void OnDestroy()
	{
		BoardModifier.Destroy();
		BoardModifier.ActiveModifiers = new List<BoardModifier>();
	}

	// Token: 0x06001AAC RID: 6828 RVA: 0x000B0610 File Offset: 0x000AE810
	private void DoStarts()
	{
		if (NetSystem.IsServer)
		{
			if (NetSystem.PlayerCount == 1)
			{
				this.StartGame();
			}
		}
		else
		{
			base.SendRPC("RPCReady", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		for (int i = 0; i < this.AIRollChances.Length; i++)
		{
			List<float> list = new List<float>();
			for (int j = 0; j < this.AIRollChances[i].Length; j++)
			{
				list.Add(this.BinaryTreeTotals[i]);
				this.BinaryTreeTotals[i] += this.AIRollChances[i][j];
			}
			this.RollBinaryTrees[i] = new BinaryTree(list.ToArray());
		}
		base.StartCoroutine(this.PreLoadObjects());
	}

	// Token: 0x06001AAD RID: 6829 RVA: 0x000B06BC File Offset: 0x000AE8BC
	private GameObject PreSpawn(GameObject o)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(o, this.spawnPos, Quaternion.identity);
		this.objects.Add(gameObject);
		return gameObject;
	}

	// Token: 0x06001AAE RID: 6830 RVA: 0x00013B3B File Offset: 0x00011D3B
	private IEnumerator PreLoadObjects()
	{
		if (GameManager.partyGameMode == PartyGameMode.BoardGame)
		{
			this.spawnPos = this.boardCamera.transform.position + this.boardCamera.transform.forward * 100f;
		}
		else
		{
			this.spawnPos = new Vector3(-100f, 0f, 0f);
		}
		for (int i = 0; i < this.preLoadObjects.Length; i++)
		{
			this.PreSpawn(this.preLoadObjects[i]);
		}
		if (GameManager.partyGameMode == PartyGameMode.BoardGame)
		{
			Animator component = this.PreSpawn(this.goalObjPrefab).GetComponent<Animator>();
			if (component != null)
			{
				component.enabled = false;
			}
			yield return null;
			ItemDetails itemFromEnum = GameManager.GetItemFromEnum(Items.Eggplant);
			this.PreSpawn(itemFromEnum.heldPrefab);
			this.PreSpawn(itemFromEnum.recievePrefab);
			GameObject gameObject = this.PreSpawn(NetSystem.GetPrefab("EggplantController").game_object);
			Eggplant component2 = gameObject.GetComponent<Eggplant>();
			component2.enabled = false;
			gameObject.GetComponent<AudioSource>().volume = 0f;
			this.PreSpawn(component2.explosionPrefab);
			AudioSystem.PlayOneShot(component2.explodeSound, 0f, 0f, 1f);
			yield return null;
			yield return new WaitUntil(() => this.boardPlayerList != null && this.boardPlayerList.Count > 0 && this.boardPlayerList[0] != null);
			this.PreSpawn(this.boardPlayerList[0].bloodyDamageEffect);
			AudioSystem.PlayOneShot(this.boardPlayerList[0].sfxBloodyDamage, 0f, 0f, 1f);
			yield return null;
			this.PreSpawn(GameManager.KeyController.keyPrefab);
		}
		GameObject clipTarget = null;
		PortalEffect portal = PortalEffect.Spawn(this.spawnPos, Vector3.down, PortalOrientation.Vertical, clipTarget, false);
		yield return null;
		portal.Release(false);
		yield return null;
		PlayerRagdoll.CreatePool(10);
		yield return null;
		GameManager.UIController.SetupWorldTextPool(15);
		for (int j = 0; j < this.objects.Count; j++)
		{
			UnityEngine.Object.Destroy(this.objects[j]);
		}
		yield return null;
		if (NetSystem.IsServer)
		{
			this.FinishedPreloading();
		}
		else
		{
			base.SendRPC("RPCPreLoaded", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		yield break;
	}

	// Token: 0x06001AAF RID: 6831 RVA: 0x000B06E8 File Offset: 0x000AE8E8
	private void SetMinigameOnlyNextMinigame()
	{
		byte minigameID = 0;
		MinigameDefinition randomMinigame = GameManager.GetRandomMinigame();
		for (int i = 0; i < GameManager.GetMinigameList().Count; i++)
		{
			if (randomMinigame.minigameName == GameManager.GetMinigameList()[i].minigameName)
			{
				minigameID = (byte)i;
			}
		}
		ActionSetupMinigameOnlyLobby newAction = new ActionSetupMinigameOnlyLobby(minigameID);
		this.QueueAction(newAction, true, true);
	}

	// Token: 0x06001AB0 RID: 6832 RVA: 0x000B0744 File Offset: 0x000AE944
	public void LoadGameBoard()
	{
		if (NetSystem.IsServer && GameManager.partyGameMode == PartyGameMode.MinigamesOnly)
		{
			this.SetMinigameOnlyNextMinigame();
		}
		ActionSimple newAction = new ActionSimple(SimpleBoardAction.LoadGameBoard);
		this.QueueAction(newAction, true, true);
		GameBoardState new_state = (GameManager.partyGameMode == PartyGameMode.BoardGame) ? GameBoardState.PlayTurns : GameBoardState.MinigamesOnlyPlay;
		this.SwitchState(new_state);
	}

	// Token: 0x06001AB1 RID: 6833 RVA: 0x000B078C File Offset: 0x000AE98C
	public void ShowMinigameResults()
	{
		ActionShowMinigameResults newAction = new ActionShowMinigameResults();
		this.QueueAction(newAction, true, true);
		this.SwitchState(GameBoardState.ShowMinigameResults);
	}

	// Token: 0x06001AB2 RID: 6834 RVA: 0x000B07B0 File Offset: 0x000AE9B0
	public void EnableBoard()
	{
		PlayerRagdoll.DespawnAll();
		if (GameManager.partyGameMode == PartyGameMode.BoardGame)
		{
			foreach (BoardPlayer boardPlayer in this.boardPlayerList)
			{
				boardPlayer.gameObject.SetActive(true);
			}
			foreach (GameObject gameObject in this.objectList)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(true);
				}
			}
			if (this.GoalScript != null)
			{
				for (int i = 0; i < this.GoalScript.Length; i++)
				{
					if (this.GoalScript[i] != null)
					{
						this.GoalScript[i].gameObject.SetActive(true);
					}
				}
			}
			if (this.WeaponGoalScript != null)
			{
				for (int j = 0; j < this.WeaponGoalScript.Length; j++)
				{
					if (this.WeaponGoalScript[j] != null)
					{
						this.WeaponGoalScript[j].gameObject.SetActive(true);
					}
				}
			}
			for (int k = 0; k < this.persistentItems.Count; k++)
			{
				this.persistentItems[k].Enable();
			}
			this.boardCamera.gameObject.SetActive(true);
			this.boardCamera.gameObject.GetComponent<AudioListener>().enabled = true;
			AudioSystem.PlayMusic(GameManager.mapSettings.music, 0.5f, GameManager.mapSettings.musicVol);
			AudioSystem.PlayAmbient(GameManager.mapSettings.ambience, 0.5f, GameManager.mapSettings.ambienceVol);
			BoardModifier.BoardReturnFromMinigame();
			return;
		}
		AudioSystem.PlayMusic(this.minigameLoadMusic, 0.5f, 1f);
	}

	// Token: 0x06001AB3 RID: 6835 RVA: 0x000B098C File Offset: 0x000AEB8C
	public void DisableBoard(bool enablePlayers)
	{
		PlayerRagdoll.DespawnAll();
		if (GameManager.partyGameMode == PartyGameMode.BoardGame)
		{
			if (!enablePlayers)
			{
				foreach (BoardPlayer boardPlayer in this.boardPlayerList)
				{
					boardPlayer.gameObject.SetActive(false);
				}
			}
			foreach (GameObject gameObject in this.objectList)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
			}
			if (this.GoalScript != null)
			{
				for (int i = 0; i < this.GoalScript.Length; i++)
				{
					if (this.GoalScript[i])
					{
						this.GoalScript[i].gameObject.SetActive(false);
					}
				}
			}
			if (this.WeaponGoalScript != null)
			{
				for (int j = 0; j < this.WeaponGoalScript.Length; j++)
				{
					if (this.WeaponGoalScript[j])
					{
						this.WeaponGoalScript[j].gameObject.SetActive(false);
					}
				}
			}
			for (int k = 0; k < this.persistentItems.Count; k++)
			{
				this.persistentItems[k].Disable();
			}
			this.boardCamera.gameObject.SetActive(false);
			this.boardCamera.gameObject.GetComponent<AudioListener>().enabled = false;
			BoardModifier.BoardEnterMinigame();
		}
		AudioSystem.StopPooledSounds(0.5f);
	}

	// Token: 0x06001AB4 RID: 6836 RVA: 0x000B0B24 File Offset: 0x000AED24
	public void StartGame()
	{
		GameManager.UIController.SetInputStatus(false);
		if (NetSystem.IsServer && GameManager.partyGameMode == PartyGameMode.BoardGame)
		{
			int num = 0;
			foreach (GamePlayer gamePlayer in GameManager.PlayerList)
			{
				NetSystem.Spawn("BoardPlayer", this.startNode.transform.position + BoardNode.GetSharedNodeOffset(num), (ushort)gamePlayer.GlobalID, gamePlayer.NetOwner).GetComponent<BoardPlayer>().Initialize(gamePlayer);
				num++;
			}
			if (this.isHalloweenMap)
			{
				Transform transform = GameManager.BoardRoot.transform.Find("Events/KillerSpawnPoint");
				Vector3 pos = (GameManager.SaveToLoad == null) ? transform.position : GameManager.SaveToLoad.killerPosition;
				NetSystem.Spawn("SlasherEnemy", pos, 0, NetSystem.MyPlayer).GetComponent<SlasherEnemy>().Initialize();
			}
		}
	}

	// Token: 0x06001AB5 RID: 6837 RVA: 0x000B0C28 File Offset: 0x000AEE28
	private void ForceMinigame(string minigame_name, int alt_index)
	{
		Debug.LogError("Foricing minigame " + minigame_name + " alternate = " + alt_index.ToString());
		MinigameDefinition minigameByName = GameManager.GetMinigameByName(minigame_name);
		GameManager.AssignTeams(minigameByName);
		ActionChangeGameState newAction = new ActionChangeGameState(GameBoardState.Minigame);
		ActionLoadMinigame newAction2 = new ActionLoadMinigame(minigameByName.minigameName, alt_index);
		this.turnOrderList = new List<BoardPlayer>();
		foreach (BoardPlayer item in this.boardPlayerList)
		{
			this.turnOrderList.Add(item);
		}
		this.QueueAction(newAction, true, true);
		this.QueueAction(newAction2, true, true);
	}

	// Token: 0x06001AB6 RID: 6838 RVA: 0x00013B4A File Offset: 0x00011D4A
	public byte RegisterActor(BoardActor actor)
	{
		this.boardActors.Add(actor);
		return (byte)(this.boardActors.Count - 1);
	}

	// Token: 0x06001AB7 RID: 6839 RVA: 0x00013B66 File Offset: 0x00011D66
	public void RegisterBoardPlayer(BoardPlayer board_player)
	{
		this.boardPlayerList.Add(board_player);
		if (board_player.GamePlayer.IsLocalPlayer)
		{
			this.myPlayers.Add(board_player);
		}
	}

	// Token: 0x06001AB8 RID: 6840 RVA: 0x000B0CD8 File Offset: 0x000AEED8
	public void SwitchState(GameBoardState new_state)
	{
		Debug.Log("Switching Board State: " + new_state.ToString());
		if (new_state == GameBoardState.DetermineTurnOrder || new_state == GameBoardState.LoadingSave)
		{
			PlatformAchievementManager.Instance.TriggerAchievement("ACH_FIRST_GAME");
		}
		ActionChangeGameState newAction = new ActionChangeGameState(new_state);
		this.QueueAction(newAction, true, true);
	}

	// Token: 0x06001AB9 RID: 6841 RVA: 0x000B0D2C File Offset: 0x000AEF2C
	public void SetState(GameBoardState new_state)
	{
		switch (new_state)
		{
		case GameBoardState.DetermineTurnOrder:
			if (GameManager.LoadScreen != null)
			{
				GameManager.LoadScreen.Destroy();
			}
			if (NetSystem.IsServer)
			{
				this.turnOrderList = new List<BoardPlayer>();
				ActionSimple newAction = new ActionSimple(SimpleBoardAction.RollDiceTurnOrder);
				this.QueueAction(newAction, true, true);
			}
			for (int i = 0; i < this.myPlayers.Count; i++)
			{
				if (this.myPlayers[i].GamePlayer.IsAI)
				{
					float num = 1.25f;
					this.myPlayers[i].AIDelayTimer.SetInterval(num + 0.2f * (float)i, num + 0.5f * (float)i, true);
				}
			}
			break;
		case GameBoardState.PlayTurns:
			if (this.minigame_choice_wnd != null)
			{
				UnityEngine.Object.Destroy(this.minigame_choice_wnd.gameObject);
			}
			if (NetSystem.IsServer && this.curTurnNum != 0 && !this.firstTurn)
			{
				this.Save();
			}
			this.curTurnNum++;
			GameManager.totalTurns++;
			this.firstTurn = false;
			break;
		case GameBoardState.SpawnGoal:
		{
			int num2 = (this.isHalloweenMap || this.areFakeChestsActive) ? 3 : 1;
			int num3 = GameManager.WeaponsCacheEnabled ? 1 : 0;
			this.goalNodes = new BoardNode[num2];
			this.goalIsReal = new bool[num2];
			this.FakeChestControllers = new FakeChestController[num2];
			this.GoalScript = new BoardGoalBase[num2];
			this.weaponNodes = new BoardNode[num3];
			this.WeaponGoalScript = new WeaponGoal[num3];
			if (NetSystem.IsServer)
			{
				this.QueueAction(new ActionWait(1f), true, true);
				if (this.areFakeChestsActive)
				{
					this.QueueAction(new ActionShellGame(GameManager.rand.Next()), true, true);
				}
				else
				{
					this.QueueAction(new ActionSpawnGoal(0, this.GetGoalSpawnNodeIndex(false)), true, true);
				}
			}
			break;
		}
		case GameBoardState.Minigame:
			if (GameManager.LoadScreen != null)
			{
				GameManager.LoadScreen.Destroy();
			}
			break;
		case GameBoardState.LoadingSave:
			if (GameManager.LoadScreen != null)
			{
				GameManager.LoadScreen.Destroy();
			}
			if (GameManager.SaveToLoad != null)
			{
				this.Load();
				this.goalNodes = new BoardNode[GameManager.SaveToLoad.goalNodeIDs.Length];
				this.goalIsReal = new bool[GameManager.SaveToLoad.goalNodeIDs.Length];
				this.GoalScript = new BoardGoalBase[GameManager.SaveToLoad.goalNodeIDs.Length];
				this.FakeChestControllers = new FakeChestController[GameManager.SaveToLoad.goalNodeIDs.Length];
				this.weaponNodes = new BoardNode[GameManager.SaveToLoad.weaponNodeIDs.Length];
				this.WeaponGoalScript = new WeaponGoal[GameManager.SaveToLoad.weaponNodeIDs.Length];
				if (NetSystem.IsServer)
				{
					this.QueueAction(new ActionWait(1f), true, true);
					if (this.areFakeChestsActive)
					{
						this.QueueAction(new ActionShellGame(GameManager.SaveToLoad.shellGameSeed), true, true);
					}
					else
					{
						for (int j = 0; j < GameManager.SaveToLoad.goalNodeIDs.Length; j++)
						{
							if (GameManager.SaveToLoad.goalNodeIDs[j] != -1)
							{
								this.QueueAction(new ActionSpawnGoal((byte)j, GameManager.SaveToLoad.goalNodeIDs[j]), true, true);
								break;
							}
						}
					}
				}
			}
			break;
		case GameBoardState.MinigamesOnlyPlay:
			if (!this.firstTurn)
			{
				this.curTurnNum++;
			}
			this.uiController.minigameOnlySceneController.SetTime(15f);
			GameManager.totalTurns++;
			this.firstTurn = false;
			if (GameManager.LoadScreen != null)
			{
				GameManager.LoadScreen.Destroy();
			}
			break;
		case GameBoardState.ShowingKiller:
			this.boardCamera.SetTrackedObject(this.slasherEnemy.transform, Vector3.zero);
			if (NetSystem.IsServer)
			{
				this.QueueAction(new ActionSimple(SimpleBoardAction.ShowKiller), true, true);
			}
			break;
		}
		this.curBoardState = new_state;
	}

	// Token: 0x06001ABA RID: 6842 RVA: 0x000B1120 File Offset: 0x000AF320
	private short GetGoalSpawnNodeIndex(bool isWeapon = false)
	{
		for (int i = 0; i < this.potentialGoalBoardNodes.Count; i++)
		{
			this.goalBoardNodesTemp[i].node = this.potentialGoalBoardNodes[i];
			this.goalBoardNodesTemp[i].dist = -1000;
			bool flag = false;
			for (int j = 0; j < this.goalNodes.Length; j++)
			{
				if (this.goalNodes[j] != null && this.potentialGoalBoardNodes[i].NodeID == this.goalNodes[j].NodeID)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.goalBoardNodesTemp[i].dist = -1000;
			}
			else
			{
				this.dists.Clear();
				for (int k = 0; k < GameManager.GetPlayerCount(); k++)
				{
					this.dists.Add(this.CurrentMap.DistToNode(GameManager.GetPlayerAt(k).BoardObject.CurrentNode, this.potentialGoalBoardNodes[i], BoardNodeConnectionDirection.Forward));
				}
				if (isWeapon)
				{
					for (int l = 0; l < this.goalNodes.Length; l++)
					{
						if (this.goalNodes[l] != null)
						{
							this.dists.Add(this.CurrentMap.DistToNode(this.goalNodes[l], this.potentialGoalBoardNodes[i], BoardNodeConnectionDirection.Forward));
						}
					}
				}
				else if (!this.firstWeapon)
				{
					for (int m = 0; m < this.weaponNodes.Length; m++)
					{
						this.dists.Add(this.CurrentMap.DistToNode(this.weaponNodes[m], this.potentialGoalBoardNodes[i], BoardNodeConnectionDirection.Forward));
					}
				}
				for (int n = 0; n < this.goalNodes.Length; n++)
				{
					if (this.goalNodes[n] != null)
					{
						this.dists.Add(this.CurrentMap.DistToNode(this.potentialGoalBoardNodes[i], this.goalNodes[n], BoardNodeConnectionDirection.Forward));
						this.dists.Add(this.CurrentMap.DistToNode(this.goalNodes[n], this.potentialGoalBoardNodes[i], BoardNodeConnectionDirection.Forward));
					}
				}
				this.goalBoardNodesTemp[i].dist = Mathf.Min(this.dists.ToArray());
			}
		}
		Array.Sort<GameBoardController.PotentialNode>(this.goalBoardNodesTemp, (GameBoardController.PotentialNode i1, GameBoardController.PotentialNode i2) => i2.dist.CompareTo(i1.dist));
		return (short)this.goalBoardNodesTemp[GameManager.rand.Next(0, 15)].node.NodeID;
	}

	// Token: 0x06001ABB RID: 6843 RVA: 0x000B13C4 File Offset: 0x000AF5C4
	private byte GetAIDiceRoll()
	{
		if (GameManager.RulesetManager == null || GameManager.RulesetManager.ActiveRuleset == null || (GameManager.RulesetManager.ActiveRuleset.General.MinDiceRoll == 0 && GameManager.RulesetManager.ActiveRuleset.General.MaxDiceRoll == 9))
		{
			return this.GetDefaultAIRollChance((byte)this.CurPlayer.GamePlayer.Difficulty);
		}
		if (!this.m_triedDynamicRollChances)
		{
			this.CreateDynamicAIRollChances();
		}
		if (this.m_createdDynamicRollChances)
		{
			return this.GetDynamicAIRollChance((byte)this.CurPlayer.GamePlayer.Difficulty);
		}
		return this.GetDefaultAIRollChance((byte)this.CurPlayer.GamePlayer.Difficulty);
	}

	// Token: 0x06001ABC RID: 6844 RVA: 0x00013B8D File Offset: 0x00011D8D
	private byte GetDefaultAIRollChance(byte d)
	{
		return (byte)(this.RollBinaryTrees[(int)d].FindPoint(ZPMath.RandomFloat(GameManager.rand, 0f, this.BinaryTreeTotals[(int)d])) + 2);
	}

	// Token: 0x06001ABD RID: 6845 RVA: 0x000B1470 File Offset: 0x000AF670
	private byte GetDynamicAIRollChance(byte d)
	{
		if (!this.m_triedDynamicRollChances)
		{
			this.CreateDynamicAIRollChances();
		}
		return (byte)(this.DynamicRollBinaryTrees[(int)d].FindPoint(ZPMath.RandomFloat(GameManager.rand, 0f, this.DynamicBinaryTreeTotals[(int)d])) + GameManager.RulesetManager.ActiveRuleset.General.MinDiceRoll);
	}

	// Token: 0x06001ABE RID: 6846 RVA: 0x000B14C8 File Offset: 0x000AF6C8
	private void CreateDynamicAIRollChances()
	{
		this.m_triedDynamicRollChances = true;
		try
		{
			int num = GameManager.RulesetManager.ActiveRuleset.General.MaxDiceRoll - GameManager.RulesetManager.ActiveRuleset.General.MinDiceRoll + 1;
			for (int i = 0; i < 3; i++)
			{
				float a = this.AIRollRangeMin[i];
				float b = this.AIRollRangeMax[i];
				List<float> list = new List<float>();
				for (int j = 0; j < num; j++)
				{
					float t = (float)j / (float)num;
					float num2 = Mathf.Lerp(a, b, t);
					list.Add(this.DynamicBinaryTreeTotals[i]);
					Debug.Log(i.ToString() + " : " + list.ToString());
					this.DynamicBinaryTreeTotals[i] += num2;
				}
				this.DynamicRollBinaryTrees[i] = new BinaryTree(list.ToArray());
			}
			this.m_createdDynamicRollChances = true;
		}
		catch (Exception)
		{
			Debug.LogError("Failed creating chance table");
			this.m_createdDynamicRollChances = false;
		}
	}

	// Token: 0x06001ABF RID: 6847 RVA: 0x000B15D8 File Offset: 0x000AF7D8
	public void OnTurnsEnd()
	{
		if (this.IterateEvents(PersistentItemEventType.LastTurn))
		{
			return;
		}
		if (this.ShouldGoToAwardsScene(ref this.ev, ref this.challenges))
		{
			this.SwitchState(GameBoardState.PummelAwards);
			this.waitForKeysMaxAwards.Start();
			this.keysFinishedAwards = false;
			return;
		}
		this.StartMinigame();
	}

	// Token: 0x06001AC0 RID: 6848 RVA: 0x000B1624 File Offset: 0x000AF824
	private bool ShouldGoToAwardsScene(ref StatChallengeBoardEvent ev, ref List<GobletChallenge> challenges)
	{
		if (GameManager.partyGameMode == PartyGameMode.MinigamesOnly)
		{
			return false;
		}
		if (GameManager.TurnCount == 9999 && GameManager.WinningRelics == 9999)
		{
			return false;
		}
		bool flag = false;
		int playerCount = GameManager.GetPlayerCount();
		int num = int.MinValue;
		for (int i = 0; i < playerCount; i++)
		{
			GamePlayer playerAt = GameManager.GetPlayerAt(i);
			num = Mathf.Max(num, playerAt.BoardObject.GoalScore);
		}
		if (!this.m_midGameAwardsComplete)
		{
			if (GameManager.TurnCount == 9999)
			{
				if (num != 0 && num == GameManager.WinningRelics / 2)
				{
					ev = StatChallengeBoardEvent.MidGame;
					flag = true;
				}
			}
			else if (GameManager.Board.CurnTurnNum == GameManager.TurnCount / 2)
			{
				ev = StatChallengeBoardEvent.MidGame;
				flag = true;
			}
		}
		if (num >= GameManager.WinningRelics || GameManager.Board.CurnTurnNum >= GameManager.TurnCount)
		{
			ev = StatChallengeBoardEvent.EndGame;
			flag = true;
		}
		if (flag)
		{
			challenges = GameManager.RulesetManager.ActiveRuleset.GobletChallenges.GetChallenges(ev);
			if (ev == StatChallengeBoardEvent.EndGame || challenges.Count > 0)
			{
				if (ev == StatChallengeBoardEvent.MidGame)
				{
					this.m_midGameAwardsComplete = true;
				}
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001AC1 RID: 6849 RVA: 0x000B1720 File Offset: 0x000AF920
	private void StartMinigame()
	{
		this.minigameStarted = true;
		if (this.curTurnNum >= GameManager.TurnCount)
		{
			this.FinishGame();
			return;
		}
		this.SwitchState(GameBoardState.SelectingMinigame);
		this.QueueAction(new ActionWait(0.75f), true, true);
		ActionSimple newAction = new ActionSimple(SimpleBoardAction.ChooseMinigame);
		this.QueueAction(newAction, true, true);
		for (int i = 0; i < this.persistentItems.Count; i++)
		{
			this.persistentItems[i].ResetFinished();
		}
	}

	// Token: 0x06001AC2 RID: 6850 RVA: 0x000B1798 File Offset: 0x000AF998
	public void Update()
	{
		if (GameManager.DEBUGGING && Input.GetKeyDown(KeyCode.Minus))
		{
			if (Input.GetKeyDown(KeyCode.Minus))
			{
				this.CurPlayer.GiveGold(5, true);
				this.CurPlayer.GiveItem(GameManager.ItemList.GetRandomItem(this.CurPlayer, false).itemID, true);
			}
			if (Input.GetKeyDown(KeyCode.KeypadMinus))
			{
				Time.timeScale = Mathf.Clamp(Time.timeScale - 1f, 0f, float.MaxValue);
			}
			if (Input.GetKeyDown(KeyCode.KeypadPlus))
			{
				Time.timeScale = Mathf.Clamp(Time.timeScale + 1f, 0f, float.MaxValue);
			}
		}
		bool debugging = GameManager.DEBUGGING;
		if (this.actionWaitTime > 0f)
		{
			this.actionWaitTime -= Time.deltaTime;
			return;
		}
		if (this.actionWaitFrame > 0)
		{
			this.actionWaitFrame--;
			return;
		}
		if (this.currentAction == null && this.actionQueue.Count > 0)
		{
			this.currentAction = this.actionQueue.Dequeue();
			Debug.Log("Starting Action-" + this.tempActionNum.ToString() + " : " + this.currentAction.ActionType.ToString());
			this.tempActionNum++;
		}
		if (this.currentAction != null)
		{
			this.DoAction();
			return;
		}
		switch (this.curBoardState)
		{
		case GameBoardState.Loading:
		case GameBoardState.Initializing:
		case GameBoardState.SpawnGoal:
		case GameBoardState.ShowMinigameResults:
		case GameBoardState.SelectingMinigame:
		case GameBoardState.LoadingSave:
		case GameBoardState.ShowingKiller:
		case GameBoardState.KillersTurn:
			break;
		case GameBoardState.DetermineTurnOrder:
			foreach (BoardPlayer boardPlayer in this.myPlayers)
			{
				if (boardPlayer.IsRollingDice && this.currentAction == null && ((!boardPlayer.GamePlayer.IsAI && !GameManager.IsGamePaused && boardPlayer.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.Accept)) || (boardPlayer.GamePlayer.IsAI && boardPlayer.AIDelayTimer.Elapsed(true))))
				{
					int num = GameManager.rand.Next(0, 10);
					this.QueueAction(new ActionHitDice(boardPlayer.GamePlayer.GlobalID, (byte)num), true, true);
					boardPlayer.IsRollingDice = false;
				}
				if (GameManager.DEBUGGING && boardPlayer.IsRollingDice && this.currentAction == null)
				{
					for (int i = 0; i < this.num_keys.Length; i++)
					{
						if (Input.GetKeyDown(this.num_keys[i]))
						{
							this.QueueAction(new ActionHitDice(boardPlayer.GamePlayer.GlobalID, (byte)i), true, true);
							boardPlayer.IsRollingDice = false;
							break;
						}
					}
				}
			}
			if (NetSystem.IsServer)
			{
				int num2 = 0;
				using (List<BoardPlayer>.Enumerator enumerator = this.boardPlayerList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.TurnOrderRoll != -1)
						{
							num2++;
						}
					}
				}
				if (num2 == this.boardPlayerList.Count)
				{
					this.SwitchState(GameBoardState.SpawnGoal);
					return;
				}
			}
			break;
		case GameBoardState.PlayTurns:
			if (NetSystem.IsServer && this.curPlayer == null && this.curPlayerIndex >= this.turnOrderList.Count)
			{
				if (!this.minigameStarted)
				{
					this.minigameStarted = true;
					if (!this.isHalloweenMap)
					{
						this.OnTurnsEnd();
						return;
					}
					this.SwitchState(GameBoardState.KillersTurn);
					BoardActor attackTarget = this.slasherEnemy.GetAttackTarget();
					if (this.slasherEnemy.DeathTurnsRemaining != 0)
					{
						SlasherEnemy slasherEnemy = this.slasherEnemy;
						int deathTurnsRemaining = slasherEnemy.DeathTurnsRemaining;
						slasherEnemy.DeathTurnsRemaining = deathTurnsRemaining - 1;
						if (this.slasherEnemy.DeathTurnsRemaining == 0)
						{
							this.QueueAction(new ActionSimple(SimpleBoardAction.RespawnKiller), true, true);
							return;
						}
						this.OnTurnsEnd();
						return;
					}
					else
					{
						if (attackTarget != null)
						{
							this.QueueAction(new ActionKillerAttack(attackTarget.ActorID), true, true);
							return;
						}
						BoardPlayer boardPlayer2 = null;
						for (int j = 0; j < this.boardPlayerList.Count; j++)
						{
							if ((boardPlayer2 == null || this.boardPlayerList[j].CurScorePosition < boardPlayer2.CurScorePosition) && this.slasherEnemy.lastKilledActor != this.boardPlayerList[j])
							{
								boardPlayer2 = this.boardPlayerList[j];
							}
						}
						Vector3 target = (boardPlayer2 == null) ? Vector3.zero : boardPlayer2.transform.position;
						this.QueueAction(new ActionKillerMove(this.slasherEnemy.GetMovePoint(this.slasherEnemy.moveDist, target)), true, true);
						return;
					}
				}
			}
			else
			{
				this.GetNextPlayer();
				if (this.curPlayer != null && this.curPlayer.GamePlayer.IsLocalPlayer)
				{
					if ((this.curPlayer.ForceTurnEnd || this.curPlayer.GamePlayer.IsAI) && this.curPlayer.AIDelayTimer.Elapsed(true))
					{
						if (this.curPlayer.PlayerState == BoardPlayerState.GetTurnInput && this.curPlayer.IsRollingDice)
						{
							if (GameManager.DEBUGGING && GameManager.rand.NextDouble() < 0.05000000074505806)
							{
								this.AIMapTimer.Start();
								this.QueueAction(new ActionSimple(SimpleBoardAction.StartViewMap), true, true);
								return;
							}
							if (this.CurPlayer.HasUsedItem || this.curPlayer.ForceTurnEnd || GameManager.rand.NextDouble() >= (double)this.useItemChances[(int)this.CurPlayer.GamePlayer.Difficulty])
							{
								byte aidiceRoll = this.GetAIDiceRoll();
								this.QueueAction(new ActionHitDice(this.curPlayer.GamePlayer.GlobalID, aidiceRoll), true, true);
								return;
							}
							int num3 = GameManager.ItemList.items.Length;
							int[] array = new int[num3];
							List<int> list = new List<int>();
							for (int k = 0; k < num3; k++)
							{
								list.Add(k);
							}
							for (int l = 0; l < num3; l++)
							{
								int index = GameManager.rand.Next(0, list.Count);
								array[l] = list[index];
								list.RemoveAt(index);
							}
							ItemAIUse itemAIUse = null;
							if (!this.curPlayer.ItemsDisabled && !this.curPlayer.TwitchMapEvent)
							{
								for (int m = 0; m < num3; m++)
								{
									if (this.curPlayer.GetItemCount((byte)m) > 0)
									{
										ItemAIUse target2 = GameManager.ItemList.items[m].prefab.GetComponent<Item>().GetTarget(this.curPlayer);
										if (itemAIUse == null || (target2 != null && target2.priority > itemAIUse.priority))
										{
											this.AIItemToUse = m;
											itemAIUse = target2;
										}
									}
								}
							}
							if (itemAIUse != null)
							{
								this.QueueAction(new ActionSimple(SimpleBoardAction.OpenInventory), true, true);
								return;
							}
							byte aidiceRoll2 = this.GetAIDiceRoll();
							this.QueueAction(new ActionHitDice(this.curPlayer.GamePlayer.GlobalID, aidiceRoll2), true, true);
							return;
						}
						else if (this.curPlayer.PlayerState == BoardPlayerState.WaitingIntersection)
						{
							bool flag = false;
							for (int n = 0; n < this.activeArrows.Count; n++)
							{
								if (this.activeArrows[n].Selected)
								{
									flag = true;
									this.QueueAction(new ActionChooseDirection(this.curPlayer.GamePlayer.GlobalID, (byte)this.activeArrows[n].Direction), true, true);
									break;
								}
							}
							if (!flag)
							{
								this.SelectClosestArrow();
								this.CurPlayer.AIDelayTimer.SetInterval(0.4f, 0.7f, true);
								return;
							}
						}
						else
						{
							if (this.curPlayer.PlayerState == BoardPlayerState.MakingInteractionChoice)
							{
								GameManager.Board.OnInteractionChoice(this.curPlayer.CurrentNode.CurInteractionScript.GetAIChoice());
								return;
							}
							if (this.curPlayer.PlayerState == BoardPlayerState.InventoryOpen)
							{
								if (!this.curPlayer.ForceTurnEnd && (!GameManager.DEBUGGING || GameManager.rand.NextDouble() > 0.05))
								{
									this.uiController.InventoryUI.OnClick(this.AIItemToUse);
									return;
								}
								this.QueueAction(new ActionSimple(SimpleBoardAction.CloseInventory), true, true);
								return;
							}
							else if (this.curPlayer.PlayerState == BoardPlayerState.ItemEquipped && this.curPlayer.EquippedItem != null && this.curPlayer.EquippedItem.CurState == Item.ItemState.Aiming)
							{
								if (this.curPlayer.ForceTurnEnd || (GameManager.DEBUGGING && GameManager.rand.NextDouble() <= 0.05))
								{
									this.CurPlayer.EquippedItem.CancelItem();
									return;
								}
								if (this.CurPlayer.EquippedItem.aiAutoUse)
								{
									this.CurPlayer.EquippedItem.AIUseItem();
									return;
								}
							}
							else if (this.CurPlayer.PlayerState == BoardPlayerState.ViewingMap && (this.curPlayer.ForceTurnEnd || this.AIMapTimer.Elapsed(true)))
							{
								this.QueueAction(new ActionSimple(SimpleBoardAction.EndViewMap), true, true);
								return;
							}
						}
					}
					else if (!this.curPlayer.GamePlayer.IsAI && !GameManager.IsGamePaused)
					{
						if (this.curPlayer.PlayerState == BoardPlayerState.GetTurnInput || this.curPlayer.PlayerState == BoardPlayerState.WaitingIntersection)
						{
							if (this.curPlayer.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.Action2))
							{
								this.QueueAction(new ActionSimple(SimpleBoardAction.StartViewMap), true, true);
							}
							else if (!this.curPlayer.HasUsedItem && this.curPlayer.PlayerState == BoardPlayerState.GetTurnInput && this.curPlayer.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.Action1))
							{
								if (this.curPlayer.ItemsDisabled || this.curPlayer.TwitchMapEvent)
								{
									AudioSystem.PlayOneShot(this.itemsDisabled, 1f, 0f, 1f);
								}
								else
								{
									this.QueueAction(new ActionSimple(SimpleBoardAction.OpenInventory), true, true);
								}
							}
							else if (this.curPlayer.IsRollingDice && (this.curPlayer.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.Accept) || this.curPlayer.ForceTurnEnd))
							{
								this.curPlayer.IsRollingDice = false;
								byte roll_number;
								if (GameManager.RulesetManager.ActiveRuleset != null)
								{
									int minDiceRoll = GameManager.RulesetManager.ActiveRuleset.General.MinDiceRoll;
									int maxValue = GameManager.RulesetManager.ActiveRuleset.General.MaxDiceRoll + 1;
									roll_number = (byte)GameManager.rand.Next(minDiceRoll, maxValue);
								}
								else
								{
									roll_number = (byte)GameManager.rand.Next(2, 10);
								}
								this.QueueAction(new ActionHitDice(this.curPlayer.GamePlayer.GlobalID, roll_number), true, true);
							}
							else
							{
								if (this.curPlayer.PlayerState == BoardPlayerState.WaitingIntersection)
								{
									if (!this.curPlayer.ForceTurnEnd)
									{
										if (this.curPlayer.GamePlayer.RewiredPlayer.controllers.GetLastActiveController().type == ControllerType.Joystick)
										{
											Vector2 zero = new Vector2(this.curPlayer.GamePlayer.RewiredPlayer.GetAxis(InputActions.Horizontal), this.curPlayer.GamePlayer.RewiredPlayer.GetAxis(InputActions.Vertical));
											if (GameManager.IsGamePaused)
											{
												zero = Vector2.zero;
											}
											float num4 = 0.5f;
											float num5 = num4 * num4;
											if (zero.sqrMagnitude > num5)
											{
												DirectionChoiceArrow directionChoiceArrow = null;
												float num6 = float.MinValue;
												for (int num7 = 0; num7 < this.activeArrows.Count; num7++)
												{
													Vector3 vector = this.activeArrows[num7].transform.position - this.curPlayer.transform.position;
													vector.y = 0f;
													vector.Normalize();
													float num8 = Vector2.Dot(new Vector2(vector.x, vector.z), zero);
													if (num8 > num6)
													{
														num6 = num8;
														directionChoiceArrow = this.activeArrows[num7];
													}
												}
												directionChoiceArrow.Selected = true;
												for (int num9 = 0; num9 < this.activeArrows.Count; num9++)
												{
													if (this.activeArrows[num9] != directionChoiceArrow)
													{
														this.activeArrows[num9].Selected = false;
													}
												}
											}
										}
										else
										{
											foreach (DirectionChoiceArrow directionChoiceArrow2 in this.activeArrows)
											{
												directionChoiceArrow2.Selected = false;
											}
										}
									}
									if ((!this.curPlayer.GamePlayer.RewiredPlayer.controllers.hasMouse || !this.curPlayer.GamePlayer.RewiredPlayer.controllers.Mouse.GetButtonDown(0)) && !this.curPlayer.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.Accept))
									{
										goto IL_E80;
									}
									using (List<DirectionChoiceArrow>.Enumerator enumerator2 = this.activeArrows.GetEnumerator())
									{
										while (enumerator2.MoveNext())
										{
											DirectionChoiceArrow directionChoiceArrow3 = enumerator2.Current;
											if (directionChoiceArrow3.Selected || directionChoiceArrow3.MouseOver)
											{
												this.QueueAction(new ActionChooseDirection(this.curPlayer.GamePlayer.GlobalID, (byte)directionChoiceArrow3.Direction), true, true);
												break;
											}
										}
										goto IL_E80;
									}
								}
								if (GameManager.DEBUGGING)
								{
									for (int num10 = 0; num10 <= 9; num10++)
									{
										if (this.curPlayer.IsRollingDice && Input.GetKeyDown(this.num_keys[num10]))
										{
											this.QueueAction(new ActionHitDice(this.curPlayer.GamePlayer.GlobalID, (Input.GetKey(KeyCode.LeftShift) && num10 == 0) ? 100 : ((byte)num10)), true, true);
										}
									}
								}
							}
						}
						IL_E80:
						if (this.curPlayer.PlayerState == BoardPlayerState.ViewingMap && this.curPlayer.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.Action2))
						{
							this.QueueAction(new ActionSimple(SimpleBoardAction.EndViewMap), true, true);
						}
						if (this.curPlayer.PlayerState == BoardPlayerState.InventoryOpen && this.curPlayer.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.Cancel))
						{
							this.QueueAction(new ActionSimple(SimpleBoardAction.CloseInventory), true, true);
							return;
						}
					}
				}
			}
			break;
		case GameBoardState.Minigame:
			if (GameManager.Minigame != null)
			{
				GameManager.Minigame.UpdateBase();
				return;
			}
			break;
		case GameBoardState.EndingGame:
			if (NetSystem.IsServer && GameManager.partyGameMode != PartyGameMode.MinigamesOnly && !this.keysFinished && (GameManager.KeyController.KeysFinished() || this.waitForKeysMax.Elapsed(true)))
			{
				this.keysFinished = true;
				ActionShowWinner newAction = new ActionShowWinner(true);
				this.QueueAction(newAction, true, true);
				return;
			}
			break;
		case GameBoardState.MinigamesOnlyPlay:
			if (this.uiController.minigameOnlySceneController.nextMinigame != this.nextMinigame)
			{
				this.uiController.minigameOnlySceneController.SetNextMinigame(this.nextMinigame);
			}
			if (NetSystem.IsServer && !this.minigameStarted && this.uiController.minigameOnlySceneController.timer.time_test <= 1.5f)
			{
				this.minigameStarted = true;
				if (this.curTurnNum >= GameManager.MinigameModeCount)
				{
					this.FinishGame();
					return;
				}
				this.SwitchState(GameBoardState.SelectingMinigame);
				this.QueueAction(new ActionWait(0.75f), true, true);
				ActionSimple newAction2 = new ActionSimple(SimpleBoardAction.ChooseMinigame);
				this.QueueAction(newAction2, true, true);
				return;
			}
			break;
		case GameBoardState.PummelAwards:
			if (NetSystem.IsServer && !this.keysFinishedAwards && (GameManager.KeyController.KeysFinished() || this.waitForKeysMaxAwards.Elapsed(true)))
			{
				this.keysFinishedAwards = true;
				this.QueueAction(new ActionWait(0.75f), true, true);
				ActionStartPummelAwards newAction3 = new ActionStartPummelAwards(this.challenges != null && this.challenges.Count > 0, this.ev, this.IsGameFinished());
				this.QueueAction(newAction3, true, true);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06001AC3 RID: 6851 RVA: 0x000B27EC File Offset: 0x000B09EC
	public void SortTurnOrderList(int seed = 0)
	{
		this.turnOrderList.Clear();
		this.turnOrderList.AddRange(this.boardPlayerList);
		this.turnOrderList.Shuffle(seed);
		this.turnOrderList = (from o in this.turnOrderList
		orderby o.TurnOrderRoll descending
		select o).ToList<BoardPlayer>();
		this.uiController.turnOrderUI.UpdateOrder(this.turnOrderList);
	}

	// Token: 0x06001AC4 RID: 6852 RVA: 0x000B286C File Offset: 0x000B0A6C
	public void ClickedItem(int itemID)
	{
		if (!this.CurPlayer.HasUsedItem && this.curPlayer.GetItemCount((byte)itemID) > 0 && this.curPlayer.PlayerState == BoardPlayerState.InventoryOpen)
		{
			this.QueueAction(new ActionEquipItem((byte)itemID, this.curPlayer.GamePlayer.GlobalID), true, true);
		}
	}

	// Token: 0x06001AC5 RID: 6853 RVA: 0x000B28C4 File Offset: 0x000B0AC4
	public void OnInteractionChoice(int choice)
	{
		if (this.curPlayer.IsOwner && this.curPlayer.CurrentNode.CurInteractionScript != null && this.curPlayer.CurrentNode.CurInteractionScript.GetType() == typeof(TrophyInteraction))
		{
			if (choice == 1)
			{
				choice = 0;
			}
			else
			{
				List<int> list = new List<int>();
				for (int i = 0; i < 6; i++)
				{
					if (!this.curPlayer.GetGoalStatus(i))
					{
						list.Add(i);
					}
				}
				if (list.Count == 0)
				{
					choice = 1;
				}
				else
				{
					choice = (int)((byte)(list[GameManager.rand.Next(0, list.Count)] + 1));
				}
			}
		}
		this.QueueAction(new ActionInteractionChoice((byte)choice, GameManager.rand.Next()), true, true);
	}

	// Token: 0x06001AC6 RID: 6854 RVA: 0x000B2994 File Offset: 0x000B0B94
	public void FinishGame()
	{
		RBPrefs.Save();
		this.SwitchState(GameBoardState.EndingGame);
		this.keysFinished = false;
		this.waitForKeysMax.Start();
		if (GameManager.partyGameMode == PartyGameMode.MinigamesOnly)
		{
			ActionShowWinner newAction = new ActionShowWinner(true);
			this.QueueAction(newAction, true, true);
		}
	}

	// Token: 0x06001AC7 RID: 6855 RVA: 0x000B29D8 File Offset: 0x000B0BD8
	public void QueueAction(BoardAction newAction, bool relay = true, bool do_local = true)
	{
		if (newAction.ActionType == BoardActionType.Simple)
		{
			ActionSimple actionSimple = (ActionSimple)newAction;
			Debug.Log("Queueing Simple Action = " + actionSimple.SimpleAction.ToString() + " Current Action: " + ((this.currentAction == null) ? "NULL" : ((this.currentAction.ActionType == BoardActionType.Simple) ? ((ActionSimple)this.currentAction).SimpleAction.ToString() : this.currentAction.ActionType.ToString())));
		}
		else
		{
			Debug.Log("Queueing Action = " + newAction.ActionType.ToString() + " Current Action: " + ((this.currentAction == null) ? "NULL" : ((this.currentAction.ActionType == BoardActionType.Simple) ? ((ActionSimple)this.currentAction).SimpleAction.ToString() : this.currentAction.ActionType.ToString())));
		}
		if (do_local)
		{
			this.actionQueue.Enqueue(newAction);
		}
		if (relay)
		{
			this.actionStream.Reset();
			newAction.SerializeAction(this.actionStream, true);
			if (NetSystem.IsServer)
			{
				base.SendRPC("RPCBoardActionServer", NetRPCDelivery.RELIABLE_ORDERED, new object[]
				{
					(byte)NetSystem.MyPlayer.Slot,
					(byte)newAction.ActionType,
					this.actionStream.GetDataCopy()
				});
				return;
			}
			base.SendRPC("RPCBoardAction", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				(byte)newAction.ActionType,
				this.actionStream.GetDataCopy()
			});
		}
	}

	// Token: 0x06001AC8 RID: 6856 RVA: 0x000B2B9C File Offset: 0x000B0D9C
	public void KillPlayer(BoardPlayer player, BoardPlayer killer, Vector3 origin, float force)
	{
		short killer_id = (killer == null) ? -1 : killer.GamePlayer.GlobalID;
		ActionKillPlayer newAction = new ActionKillPlayer(player.GamePlayer.GlobalID, killer_id, origin, force);
		this.QueueAction(newAction, false, true);
	}

	// Token: 0x06001AC9 RID: 6857 RVA: 0x000B2BE0 File Offset: 0x000B0DE0
	public void DespawnWeaponSpace(int index)
	{
		GameManager.Board.WeaponGoalScript[index].Despawn();
		GameManager.Board.weaponNodes[index].ResetNode();
		this.boardCamera.SetTrackedObject(null, new Vector3(0f, 0f, 0f));
	}

	// Token: 0x06001ACA RID: 6858 RVA: 0x00013BB6 File Offset: 0x00011DB6
	public void OpenWeaponSpace(int index)
	{
		GameManager.Board.WeaponGoalScript[index].Open();
	}

	// Token: 0x06001ACB RID: 6859 RVA: 0x000B2C30 File Offset: 0x000B0E30
	private void GetNextPlayer()
	{
		if (NetSystem.IsServer)
		{
			if (this.curPlayer == null && this.turnOrderList != null)
			{
				if (this.curPlayerIndex >= this.turnOrderList.Count)
				{
					this.curPlayerIndex = 0;
				}
				this.curPlayer = this.turnOrderList[this.curPlayerIndex];
				Debug.Log("Setting curPlayer: Is Null: " + (this.curPlayer == null).ToString());
			}
			this.DoPersistentEventsStartTurn();
		}
	}

	// Token: 0x06001ACC RID: 6860 RVA: 0x000B2CB4 File Offset: 0x000B0EB4
	private void DoPersistentEventsStartTurn()
	{
		if (this.IterateEvents(PersistentItemEventType.FirstTurn))
		{
			return;
		}
		if (this.IterateEvents(PersistentItemEventType.StartTurn))
		{
			return;
		}
		if (!this.startedPlayersTurn)
		{
			this.QueueAction(new ActionWait(1f), true, true);
			this.QueueAction(new ActionStartTurn(this.curPlayer.GamePlayer.GlobalID, (short)this.curPlayerIndex), true, true);
			this.startedPlayersTurn = true;
		}
	}

	// Token: 0x06001ACD RID: 6861 RVA: 0x000B2D1C File Offset: 0x000B0F1C
	private bool IterateEvents(PersistentItemEventType eventType)
	{
		for (int i = 0; i < this.persistentItems.Count; i++)
		{
			if (this.persistentItems[i].InProgress)
			{
				return true;
			}
			if (this.persistentItems[i].HasEvent(eventType) && !this.persistentItems[i].HasFinished[(int)eventType])
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
					{
						this.persistentItems[i].GetByteArray(eventType, binaryWriter);
						this.persistentItems[i].InProgress = true;
						this.QueueAction(new ActionWait(1f), true, true);
						this.QueueAction(new ActionPersistentItemEvent((byte)i, eventType, ((MemoryStream)binaryWriter.BaseStream).ToArray()), true, true);
					}
				}
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001ACE RID: 6862 RVA: 0x000B2E20 File Offset: 0x000B1020
	private void SpawnWeapon()
	{
		if (NetSystem.IsServer)
		{
			int i = 0;
			while (i < this.weaponNodes.Length)
			{
				if (this.weaponNodes[i] == null)
				{
					short nodeIndex = (GameManager.SaveToLoad != null) ? GameManager.SaveToLoad.weaponNodeIDs[i] : this.GetGoalSpawnNodeIndex(true);
					byte weaponID = (GameManager.SaveToLoad != null) ? GameManager.SaveToLoad.weaponIDs[i] : GameManager.ItemList.GetRandomItem(this.CurPlayer, true).itemID;
					this.QueueAction(new ActionSpawnWeapon((byte)i, weaponID, nodeIndex), true, true);
					if (i == this.weaponNodes.Length - 1)
					{
						GameManager.SaveToLoad = null;
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}
	}

	// Token: 0x06001ACF RID: 6863 RVA: 0x00013BC9 File Offset: 0x00011DC9
	private void FinishSpawningGoals()
	{
		if (GameManager.WeaponsCacheEnabled)
		{
			this.SpawnWeapon();
		}
		else if (NetSystem.IsServer)
		{
			if (this.isHalloweenMap)
			{
				this.SwitchState(GameBoardState.ShowingKiller);
			}
			else
			{
				this.GetNextPlayer();
				this.SwitchState(GameBoardState.PlayTurns);
			}
		}
		this.firstGoal = false;
	}

	// Token: 0x06001AD0 RID: 6864 RVA: 0x000B2ED0 File Offset: 0x000B10D0
	private void DoAction()
	{
		if (this.currentAction == null)
		{
			return;
		}
		switch (this.currentAction.ActionType)
		{
		case BoardActionType.Simple:
		{
			ActionSimple actionSimple = (ActionSimple)this.currentAction;
			if (actionSimple.Step == 0)
			{
				Debug.Log("Starting simple action: " + actionSimple.SimpleAction.ToString());
			}
			this.DoActionSimple(actionSimple.SimpleAction);
			return;
		}
		case BoardActionType.Wait:
		{
			ActionWait actionWait = (ActionWait)this.currentAction;
			this.actionWaitTime = actionWait.WaitTime;
			this.currentAction = null;
			return;
		}
		case BoardActionType.ChangeGameState:
		{
			ActionChangeGameState actionChangeGameState = (ActionChangeGameState)this.currentAction;
			this.SetState(actionChangeGameState.BoardState);
			this.currentAction = null;
			return;
		}
		case BoardActionType.SpawnGoal:
		{
			ActionSpawnGoal actionSpawnGoal = (ActionSpawnGoal)this.currentAction;
			switch (actionSpawnGoal.Step)
			{
			case 0:
			{
				int step;
				if (this.goalNodes[(int)actionSpawnGoal.GoalIndex] == null)
				{
					if (this.firstGoal && actionSpawnGoal.GoalIndex == 0)
					{
						this.SortTurnOrderList((int)actionSpawnGoal.HackyFixSeed);
					}
					ActionSpawnGoal actionSpawnGoal2 = actionSpawnGoal;
					step = actionSpawnGoal2.Step;
					actionSpawnGoal2.Step = step + 1;
					return;
				}
				if (this.GoalScript[(int)actionSpawnGoal.GoalIndex] != null)
				{
					this.GoalScript[(int)actionSpawnGoal.GoalIndex].Despawn();
					this.goalNodes[(int)actionSpawnGoal.GoalIndex].ResetNode();
				}
				ActionSpawnGoal actionSpawnGoal3 = actionSpawnGoal;
				step = actionSpawnGoal3.Step;
				actionSpawnGoal3.Step = step + 1;
				this.boardCamera.SetTrackedObject(null, new Vector3(0f, 0f, 0f));
				return;
			}
			case 1:
			{
				this.actionWaitTime = 0.5f;
				ActionSpawnGoal actionSpawnGoal4 = actionSpawnGoal;
				int step = actionSpawnGoal4.Step;
				actionSpawnGoal4.Step = step + 1;
				GameManager.UIController.HideBoardControls();
				return;
			}
			case 2:
			{
				this.goalNodes[(int)actionSpawnGoal.GoalIndex] = this.boardNodes[(int)actionSpawnGoal.NodeIndex];
				this.goalNodes[(int)actionSpawnGoal.GoalIndex].CurrentNodeType = BoardNodeType.Trophy;
				this.goalNodes[(int)actionSpawnGoal.GoalIndex].CurHasInteraction = true;
				this.boardCamera.MoveTo(this.goalNodes[(int)actionSpawnGoal.GoalIndex].gameObject.transform, new Vector3(0f, 0.35f, 0f), GameManager.Board.boardCamera.targetDistScale);
				ActionSpawnGoal actionSpawnGoal5 = actionSpawnGoal;
				int step = actionSpawnGoal5.Step;
				actionSpawnGoal5.Step = step + 1;
				return;
			}
			case 3:
				if (this.boardCamera.WithinDistance(0.5f))
				{
					this.goalNodes[(int)actionSpawnGoal.GoalIndex] = this.boardNodes[(int)actionSpawnGoal.NodeIndex];
					Quaternion quaternion = Quaternion.Euler(new Vector3(0f, this.goalNodes[(int)actionSpawnGoal.GoalIndex].yRotation + this.goalNodes[(int)actionSpawnGoal.GoalIndex].transform.eulerAngles.y, 0f));
					Vector3 a = quaternion * Vector3.forward;
					Vector3 position = this.goalNodes[(int)actionSpawnGoal.GoalIndex].NodePosition + a * 1.5f;
					position.y -= 0.15f;
					if (this.GoalScript[(int)actionSpawnGoal.GoalIndex] == null)
					{
						this.GoalScript[(int)actionSpawnGoal.GoalIndex] = UnityEngine.Object.Instantiate<GameObject>(this.goalObjPrefab, position, quaternion * Quaternion.Euler(0f, 180f, 0f)).GetComponent<BoardGoalBase>();
						this.GoalScript[(int)actionSpawnGoal.GoalIndex].Spawn();
						if (this.areFakeChestsActive)
						{
							FakeChestController component = this.GoalScript[(int)actionSpawnGoal.GoalIndex].GetComponent<FakeChestController>();
							this.FakeChestControllers[(int)actionSpawnGoal.GoalIndex] = component;
							component.SetChestIndex(this.m_shellGameController.chestColors[(int)actionSpawnGoal.GoalIndex]);
							component.SetRealChest((int)actionSpawnGoal.GoalIndex == this.m_realChestIndex, true);
						}
					}
					else
					{
						this.GoalScript[(int)actionSpawnGoal.GoalIndex].transform.position = position;
						this.GoalScript[(int)actionSpawnGoal.GoalIndex].transform.rotation = quaternion * Quaternion.Euler(0f, 180f, 0f);
						this.GoalScript[(int)actionSpawnGoal.GoalIndex].Spawn();
						if (this.areFakeChestsActive)
						{
							this.FakeChestControllers[(int)actionSpawnGoal.GoalIndex].SetChestIndex(this.m_shellGameController.chestColors[(int)actionSpawnGoal.GoalIndex]);
							this.FakeChestControllers[(int)actionSpawnGoal.GoalIndex].SetRealChest((int)actionSpawnGoal.GoalIndex == this.m_realChestIndex, true);
						}
					}
					if (this.GoalScript[(int)actionSpawnGoal.GoalIndex].GetType() == typeof(TreasureChest))
					{
						TreasureChest treasureChest = (TreasureChest)this.GoalScript[(int)actionSpawnGoal.GoalIndex];
						StayRelative component2 = this.boardNodes[(int)actionSpawnGoal.NodeIndex].gameObject.GetComponent<StayRelative>();
						if (component2 != null)
						{
							if (treasureChest.StayRelative == null)
							{
								treasureChest.StayRelative = treasureChest.gameObject.AddComponent<StayRelative>();
							}
							treasureChest.StayRelative.target = component2.target;
							treasureChest.StayRelative.TargetReset();
						}
						else if (treasureChest.StayRelative != null)
						{
							UnityEngine.Object.Destroy(treasureChest.StayRelative);
						}
					}
					this.goalNodes[(int)actionSpawnGoal.GoalIndex].CurInteractionScript = this.GoalScript[(int)actionSpawnGoal.GoalIndex].GetComponent<Interaction>();
					if (this.firstGoal && actionSpawnGoal.GoalIndex == 0)
					{
						if (this.isHalloweenMap && !this.areFakeChestsActive)
						{
							this.actionWaitTime = 2f;
							string translation = LocalizationManager.GetTranslation("BoardExplanationHalloween", true, 0, true, false, null, null, true);
							GameManager.UIController.GetComponentInChildren<HalloweenTextDissolve>().Show(translation, 1.5f);
						}
						else
						{
							this.uiController.GetComponentInChildren<SimpleInteractionDialog>().Activate("Treasure Chest", "BoardExplanation", null, GameManager.PlayerList[0], this.areFakeChestsActive ? this.shellCrownIcon : this.crownIcon);
							this.actionWaitTime = 5f;
						}
					}
					else
					{
						this.actionWaitTime = 2f;
					}
					ActionSpawnGoal actionSpawnGoal6 = actionSpawnGoal;
					int step = actionSpawnGoal6.Step;
					actionSpawnGoal6.Step = step + 1;
					return;
				}
				break;
			case 4:
			{
				int step;
				if (this.firstGoal)
				{
					if ((int)actionSpawnGoal.GoalIndex == this.goalNodes.Length - 1)
					{
						if (this.isHalloweenMap)
						{
							GameManager.UIController.GetComponentInChildren<HalloweenTextDissolve>().Hide(1f);
							this.actionWaitTime = 1f;
						}
						else
						{
							this.uiController.GetComponentInChildren<SimpleInteractionDialog>().window.SetState(MainMenuWindowState.Hidden);
							GameManager.UIController.SetInputStatus(false);
						}
					}
					ActionSpawnGoal actionSpawnGoal7 = actionSpawnGoal;
					step = actionSpawnGoal7.Step;
					actionSpawnGoal7.Step = step + 1;
					return;
				}
				if (!this.areFakeChestsActive)
				{
					this.CameraTrackCurrentPlayer();
				}
				ActionSpawnGoal actionSpawnGoal8 = actionSpawnGoal;
				step = actionSpawnGoal8.Step;
				actionSpawnGoal8.Step = step + 1;
				return;
			}
			case 5:
				if (this.firstGoal)
				{
					if ((int)actionSpawnGoal.GoalIndex == this.goalNodes.Length - 1)
					{
						this.FinishSpawningGoals();
					}
					else if (NetSystem.IsServer)
					{
						bool flag = false;
						for (int i = 0; i < this.goalNodes.Length; i++)
						{
							if (this.goalNodes[i] == null && (GameManager.SaveToLoad == null || GameManager.SaveToLoad.goalNodeIDs[i] != -1))
							{
								short node_index = (GameManager.SaveToLoad != null) ? GameManager.SaveToLoad.goalNodeIDs[i] : this.GetGoalSpawnNodeIndex(false);
								this.QueueAction(new ActionSpawnGoal((byte)i, node_index), true, true);
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.FinishSpawningGoals();
						}
					}
					this.currentAction = null;
					return;
				}
				if (!this.firstGoal && this.boardCamera.WithinDistance(0.5f))
				{
					if (this.areFakeChestsActive)
					{
						bool flag2 = false;
						for (int j = 0; j < this.goalNodes.Length; j++)
						{
							if (this.goalNodes[j] == null)
							{
								if (NetSystem.IsServer)
								{
									short node_index2 = (GameManager.SaveToLoad != null) ? GameManager.SaveToLoad.goalNodeIDs[j] : this.GetGoalSpawnNodeIndex(false);
									this.QueueAction(new ActionSpawnGoal((byte)j, node_index2), true, true);
								}
								this.m_goalSpawnQueued[j] = true;
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							this.EndInteraction();
						}
						this.currentAction = null;
						return;
					}
					this.EndInteraction();
					this.currentAction = null;
					return;
				}
				break;
			default:
				return;
			}
			break;
		}
		case BoardActionType.MovePlayer:
		{
			ActionMovePlayer actionMovePlayer = (ActionMovePlayer)this.currentAction;
			if (!actionMovePlayer.Initialized)
			{
				if (this.curPlayer.PlayerState == BoardPlayerState.GetTurnInput || this.curPlayer.PlayerState == BoardPlayerState.CompletedInteraction || this.curPlayer.PlayerState == BoardPlayerState.CompletedDirectionChoice || this.curPlayer.PlayerState == BoardPlayerState.WaitingIntersection)
				{
					this.CameraTrackCurrentPlayer();
					if (actionMovePlayer.IntersectionStart)
					{
						this.curPlayer.ChooseDirection((int)actionMovePlayer.Direction);
					}
					else
					{
						this.curPlayer.Move((int)actionMovePlayer.Steps);
					}
					actionMovePlayer.Initialized = true;
					return;
				}
			}
			else if (this.curPlayer.PlayerState == BoardPlayerState.Idle)
			{
				if (this.curPlayer.CurrentNode.nodeEventType == BoardNodeEventType.None || this.curPlayer.CurrentNode.CurrentNodeType == BoardNodeType.Trophy || this.curPlayer.CurrentNode.CurrentNodeType == BoardNodeType.Weapon)
				{
					this.EndTurn();
					this.currentAction = null;
					return;
				}
				if (NetSystem.IsServer)
				{
					int seed;
					if (this.CurPlayer.CurrentNode.nodeEventType == BoardNodeEventType.Simple && this.CurPlayer.CurrentNode.eventAction == NodeEventAction.GiveItem)
					{
						seed = (int)GameManager.ItemList.GetRandomItem(this.curPlayer, false).itemID;
					}
					else
					{
						seed = GameManager.rand.Next(int.MinValue, int.MaxValue);
					}
					ActionDoNodeEvent newAction = new ActionDoNodeEvent((ushort)this.curPlayer.CurrentNode.NodeID, (ushort)this.curPlayer.GamePlayer.GlobalID, seed);
					this.QueueAction(newAction, true, true);
				}
				this.currentAction = null;
				return;
			}
			else
			{
				if (this.curPlayer.PlayerState == BoardPlayerState.WaitingIntersection)
				{
					this.CreateDirectionChoices(this.curPlayer.CurrentNode, this.curPlayer.NodeChoices);
					this.currentAction = null;
					return;
				}
				if (this.curPlayer.PlayerState == BoardPlayerState.MakingInteractionChoice)
				{
					this.currentAction = null;
					return;
				}
			}
			break;
		}
		case BoardActionType.ChooseDirection:
		{
			AudioSystem.PlayOneShot("MultimediaButtonClick007_STD_ZapSplat", 0.3f, 0f);
			foreach (DirectionChoiceArrow directionChoiceArrow in this.activeArrows)
			{
				directionChoiceArrow.DestroyArrow();
			}
			this.activeArrows.Clear();
			ActionChooseDirection actionChooseDirection = (ActionChooseDirection)this.currentAction;
			if (!actionChooseDirection.Initialized)
			{
				actionChooseDirection.Initialized = true;
			}
			this.curPlayer.PlayerState = BoardPlayerState.CompletedDirectionChoice;
			if (this.curPlayer.GamePlayer.IsLocalPlayer)
			{
				ActionMovePlayer newAction2 = new ActionMovePlayer(this.curPlayer.GamePlayer.GlobalID, (byte)this.curPlayer.MoveStepsRemaining, actionChooseDirection.Direction, true);
				this.QueueAction(newAction2, true, true);
			}
			this.currentAction = null;
			return;
		}
		case BoardActionType.HitDice:
		{
			ActionHitDice actionHitDice = (ActionHitDice)this.currentAction;
			BoardPlayer player = this.GetPlayer(actionHitDice.PlayerID);
			switch (actionHitDice.Step)
			{
			case 0:
				player.HitDice((int)actionHitDice.RollNumber);
				if (this.curBoardState != GameBoardState.PlayTurns)
				{
					this.currentAction = null;
					return;
				}
				GameManager.UIController.HideBoardControls();
				GameManager.UIController.SetInputStatus(false);
				if (player.TwitchMapEvent)
				{
					this.actionWaitTime = 2f;
					ActionHitDice actionHitDice2 = actionHitDice;
					int step = actionHitDice2.Step;
					actionHitDice2.Step = step + 1;
					return;
				}
				if (player.GamePlayer.IsLocalPlayer)
				{
					ActionWait newAction3 = new ActionWait(1f);
					ActionMovePlayer newAction4 = new ActionMovePlayer(this.curPlayer.GamePlayer.GlobalID, actionHitDice.RollNumber, 0, false);
					this.QueueAction(newAction3, true, true);
					this.QueueAction(newAction4, true, true);
					this.currentAction = null;
					return;
				}
				this.currentAction = null;
				return;
			case 1:
				if (actionHitDice.RollNumber > 4 || player.TwitchMapEventFailRolls == 2)
				{
					TwitchMapEvent twitchMapEvent = (TwitchMapEvent)GameManager.Board.mainBoardEvent;
					AudioSystem.PlayOneShot(twitchMapEvent.staticSound, 0.8f, 0f, 1f);
					twitchMapEvent.glitchAnimation.SetTrigger("Glitch");
					this.actionWaitTime = 1.5f;
					ActionHitDice actionHitDice3 = actionHitDice;
					int step = actionHitDice3.Step;
					actionHitDice3.Step = step + 1;
					return;
				}
				player.TwitchMapEventFailRolls++;
				this.currentAction = null;
				player.PlayerState = BoardPlayerState.Idle;
				this.EndTurn();
				return;
			case 2:
			{
				this.curPlayer.transform.position = this.CurPlayer.CurrentNode.GetPlayersSlotPosition(this.curPlayer);
				this.boardCamera.MoveToInstant(this.CurPlayer.transform.position, this.PlayerCamOffset);
				this.CameraTrackCurrentPlayer();
				this.actionWaitTime = 1.5f;
				ActionHitDice actionHitDice4 = actionHitDice;
				int step = actionHitDice4.Step;
				actionHitDice4.Step = step + 1;
				return;
			}
			case 3:
				player.TwitchMapEvent = false;
				player.TwitchMapEventFailRolls = 0;
				if (player.GamePlayer.IsLocalPlayer)
				{
					ActionWait newAction5 = new ActionWait(1f);
					ActionMovePlayer newAction6 = new ActionMovePlayer(this.curPlayer.GamePlayer.GlobalID, actionHitDice.RollNumber, 0, false);
					this.QueueAction(newAction5, true, true);
					this.QueueAction(newAction6, true, true);
				}
				this.currentAction = null;
				return;
			default:
				return;
			}
			break;
		}
		case BoardActionType.StartTurn:
		{
			ActionStartTurn actionStartTurn = (ActionStartTurn)this.currentAction;
			switch (actionStartTurn.Step)
			{
			case 0:
				Debug.Log("Starting turn " + actionStartTurn.PlayerID.ToString());
				this.curPlayer = this.GetPlayer(actionStartTurn.PlayerID);
				this.curPlayerIndex = (int)actionStartTurn.TurnOrderIndex;
				Debug.Log("CurPlayer Set: Is Null: " + (this.curPlayer == null).ToString());
				if (actionStartTurn.TurnOrderIndex == 0 && this.curMainBoardEvent != null)
				{
					base.StartCoroutine(this.curMainBoardEvent.DoFirstTurnEvent(this.CurPlayer));
					BoardAction boardAction = this.currentAction;
					int step = boardAction.Step;
					boardAction.Step = step + 1;
					return;
				}
				this.currentAction.Step = 2;
				return;
			case 1:
				if (this.curMainBoardEvent == null || this.curMainBoardEvent.Finished)
				{
					this.actionWaitTime = 0.5f;
					BoardAction boardAction2 = this.currentAction;
					int step = boardAction2.Step;
					boardAction2.Step = step + 1;
					if (this.curMainBoardEvent != null)
					{
						this.curMainBoardEvent.Finished = false;
						return;
					}
				}
				break;
			case 2:
			{
				this.CameraTrackCurrentPlayer();
				BoardAction boardAction3 = this.currentAction;
				int step = boardAction3.Step;
				boardAction3.Step = step + 1;
				return;
			}
			case 3:
				if (this.boardCamera.WithinDistance(0.05f))
				{
					string text = this.curPlayer.GamePlayer.Name + " " + LocalizationManager.GetTranslation("Start Turn", true, 0, true, false, null, null, true) + "!";
					if (this.curPlayer.TurnSkip == 2)
					{
						BoardPlayer boardPlayer = this.curPlayer;
						byte turnSkip = boardPlayer.TurnSkip;
						boardPlayer.TurnSkip = turnSkip - 1;
						text = this.CurPlayer.GamePlayer.Name + " - " + LocalizationManager.GetTranslation("Turn Skipped", true, 0, true, false, null, null, true);
					}
					GameManager.UIController.ShowLargeText(text, LargeTextType.PlayerStart, 3f, false, false);
					this.actionWaitTime = 0.5f;
					BoardAction boardAction4 = this.currentAction;
					int step = boardAction4.Step;
					boardAction4.Step = step + 1;
					return;
				}
				break;
			case 4:
				if (this.curMainBoardEvent != null)
				{
					base.StartCoroutine(this.curMainBoardEvent.DoTurnStartEvent(this.curPlayer));
					BoardAction boardAction5 = this.currentAction;
					int step = boardAction5.Step;
					boardAction5.Step = step + 1;
					return;
				}
				this.currentAction.Step = 6;
				return;
			case 5:
				if (this.curMainBoardEvent.Finished)
				{
					BoardAction boardAction6 = this.currentAction;
					int step = boardAction6.Step;
					boardAction6.Step = step + 1;
					this.curMainBoardEvent.Finished = false;
					return;
				}
				break;
			case 6:
				if (this.curPlayer.TurnSkip != 1 && this.curPlayer.IsOwner)
				{
					this.QueueAction(new ActionSimple(SimpleBoardAction.RollDiceMovement), true, true);
				}
				this.currentAction = null;
				return;
			default:
				return;
			}
			break;
		}
		case BoardActionType.LoadMinigame:
		{
			ActionLoadMinigame actionLoadMinigame = (ActionLoadMinigame)this.currentAction;
			switch (actionLoadMinigame.Step)
			{
			case 0:
			{
				if (this.OnLoadMinigameFadeout != null)
				{
					this.OnLoadMinigameFadeout.Invoke();
				}
				if (this.minigame_choice_wnd != null)
				{
					UnityEngine.Object.Destroy(this.minigame_choice_wnd.gameObject);
				}
				if (GameManager.partyGameMode == PartyGameMode.MinigamesOnly)
				{
					this.uiController.minigameOnlySceneController.Hide();
				}
				else
				{
					GameManager.UIController.HideScoreBoard();
					GameManager.UIController.boardDetails.SetActive(false);
					GameManager.UIController.SetInputStatus(true);
					if (GameManager.modifierUI)
					{
						GameManager.modifierUI.Hide();
					}
				}
				this.actionWaitTime = 0.5f;
				BoardAction boardAction7 = this.currentAction;
				int step = boardAction7.Step;
				boardAction7.Step = step + 1;
				return;
			}
			case 1:
			{
				if (GameManager.partyGameMode == PartyGameMode.BoardGame)
				{
					AudioSystem.PlayMusic(this.minigameLoadMusic, 0.5f, 1f);
				}
				MinigameDefinition minigameByName = GameManager.GetMinigameByName(actionLoadMinigame.GameName);
				GameManager.LoadMinigame(minigameByName, actionLoadMinigame.AlternateIndex);
				GameManager.localPlayedMinigameList.Add(minigameByName);
				BoardAction boardAction8 = this.currentAction;
				int step = boardAction8.Step;
				boardAction8.Step = step + 1;
				return;
			}
			case 2:
				if (GameManager.SwitchState == SceneSwitchState.ScreenObscured || GameManager.SwitchState == SceneSwitchState.FadeOut)
				{
					if (GameManager.partyGameMode != PartyGameMode.MinigamesOnly)
					{
						this.minigameIntroScene.SetActive(true);
					}
					BoardAction boardAction9 = this.currentAction;
					int step = boardAction9.Step;
					boardAction9.Step = step + 1;
					return;
				}
				break;
			case 3:
				if (GameManager.SwitchState == SceneSwitchState.FadeOut || GameManager.SwitchState == SceneSwitchState.None)
				{
					MinigameDefinition minigameByName2 = GameManager.GetMinigameByName(actionLoadMinigame.GameName);
					MinigameAlternate alternate = null;
					if (actionLoadMinigame.AlternateIndex > 0 && minigameByName2.alternates != null && actionLoadMinigame.AlternateIndex <= minigameByName2.alternates.Length)
					{
						alternate = minigameByName2.alternates[actionLoadMinigame.AlternateIndex - 1];
					}
					this.uiController.MinigameLoadScreen.SetMinigameInfo(minigameByName2, alternate);
					this.uiController.MinigameLoadScreen.ResetPlayerStatus();
					GameManager.CreateMinigameController();
					GameManager.DisableGameBoard(true, false);
					AudioListener componentInChildren = GameObject.Find("MinigameRoot").GetComponentInChildren<AudioListener>(true);
					if (componentInChildren != null)
					{
						componentInChildren.enabled = true;
					}
					BoardAction boardAction10 = this.currentAction;
					int step = boardAction10.Step;
					boardAction10.Step = step + 1;
					return;
				}
				break;
			case 4:
				if (GameManager.SwitchState == SceneSwitchState.None)
				{
					this.uiController.MinigameLoadScreen.ShowScreen(true);
					this.actionWaitTime = 0.1f;
					BoardAction boardAction11 = this.currentAction;
					int step = boardAction11.Step;
					boardAction11.Step = step + 1;
					return;
				}
				break;
			case 5:
			{
				this.uiController.MinigameLoadScreen.SetupPlayerStatus();
				this.actionWaitTime = 0.1f;
				BoardAction boardAction12 = this.currentAction;
				int step = boardAction12.Step;
				boardAction12.Step = step + 1;
				return;
			}
			case 6:
				if (GameManager.MinigameWaitForLoad())
				{
					GameManager.InitializeMinigame();
					BoardAction boardAction13 = this.currentAction;
					int step = boardAction13.Step;
					boardAction13.Step = step + 1;
					return;
				}
				break;
			case 7:
				if (GameManager.Minigame != null)
				{
					if (!GameManager.Minigame.LoadedLocally)
					{
						GameManager.Minigame.CheckMinigameLoaded();
						return;
					}
					if (!NetSystem.IsServer)
					{
						this.currentAction = null;
						return;
					}
					if (GameManager.Minigame.AllClientsReady())
					{
						ActionSimple newAction7 = new ActionSimple(SimpleBoardAction.StartMinigame);
						this.QueueAction(newAction7, true, true);
						this.currentAction = null;
						return;
					}
				}
				break;
			default:
				return;
			}
			break;
		}
		case BoardActionType.DoNodeEvent:
		{
			ActionDoNodeEvent actionDoNodeEvent = (ActionDoNodeEvent)this.currentAction;
			BoardNode boardNode = this.boardNodes[(int)actionDoNodeEvent.NodeID];
			int step2 = actionDoNodeEvent.Step;
			if (step2 == 0)
			{
				if (NetSystem.IsServer && (boardNode.nodeEventType != BoardNodeEventType.Custom || boardNode.nodeEvent == null || boardNode.nodeEvent.EndTurnAfterEvent(boardNode)))
				{
					this.QueueAction(new ActionSimple(SimpleBoardAction.EndTurn), true, true);
				}
				boardNode.StartEvent(this.curPlayer, actionDoNodeEvent.Seed);
				ActionDoNodeEvent actionDoNodeEvent2 = actionDoNodeEvent;
				int step = actionDoNodeEvent2.Step;
				actionDoNodeEvent2.Step = step + 1;
				return;
			}
			if (step2 != 1)
			{
				return;
			}
			if (boardNode.IsEventFinished())
			{
				this.currentAction = null;
				return;
			}
			break;
		}
		case BoardActionType.KillPlayer:
		{
			ActionKillPlayer actionKillPlayer = (ActionKillPlayer)this.currentAction;
			GamePlayer playerWithID = GameManager.GetPlayerWithID(actionKillPlayer.PlayerID);
			BoardPlayer boardPlayer2 = (actionKillPlayer.KillerID != -1) ? GameManager.GetPlayerAt((int)actionKillPlayer.KillerID).BoardObject : null;
			switch (actionKillPlayer.Step)
			{
			case 0:
				if (boardPlayer2 == null || boardPlayer2.PlayerState != BoardPlayerState.ItemUsing || (boardPlayer2.PlayerState == BoardPlayerState.ItemUsing && boardPlayer2.EquippedItem.CurState == Item.ItemState.Finished))
				{
					if (playerWithID != null && (boardPlayer2 == null || boardPlayer2.GamePlayer != null) && Time.time - this.lastKillMsgTime > 1f)
					{
						string text2 = (boardPlayer2 == null) ? "Hazard" : string.Concat(new string[]
						{
							"<color=#",
							ColorUtility.ToHtmlStringRGBA(boardPlayer2.GamePlayer.Color.uiColor),
							">",
							boardPlayer2.GamePlayer.Name,
							"</color>"
						});
						string text3 = string.Concat(new string[]
						{
							text2,
							",<color=#",
							ColorUtility.ToHtmlStringRGBA(playerWithID.Color.uiColor),
							">",
							playerWithID.Name,
							"</color>"
						});
						GameManager.UIController.ShowLargeText(text3, LargeTextType.PlayerKilled, 3f, false, false);
						AudioSystem.PlayOneShot("BellsDoomSingleToll_Pond5", 0.5f, 0.1f);
						this.lastKillMsgTime = Time.time;
					}
					this.actionWaitTime = 2.25f;
					BoardAction boardAction14 = this.currentAction;
					int step = boardAction14.Step;
					boardAction14.Step = step + 1;
					return;
				}
				break;
			case 1:
			{
				BoardNode graveyard = this.currentMap.GetGraveyard(this.goalNodes, false);
				playerWithID.BoardObject.transform.position = graveyard.NodePosition + Vector3.up * 0.2f;
				playerWithID.BoardObject.CurrentNode = graveyard;
				playerWithID.BoardObject.RevivePlayer();
				this.boardCamera.MoveTo(graveyard.transform, this.PlayerCamOffset, this.boardCamera.targetDistScale);
				BoardAction boardAction15 = this.currentAction;
				int step = boardAction15.Step;
				boardAction15.Step = step + 1;
				return;
			}
			case 2:
				if (this.boardCamera.WithinDistance(0.1f))
				{
					this.actionWaitTime = 1f;
					BoardAction boardAction16 = this.currentAction;
					int step = boardAction16.Step;
					boardAction16.Step = step + 1;
					return;
				}
				break;
			case 3:
				this.currentAction = null;
				return;
			default:
				return;
			}
			break;
		}
		case BoardActionType.ShowMinigameResults:
			switch (((ActionShowMinigameResults)this.currentAction).Step)
			{
			case 0:
			{
				AudioSystem.PlayMusic(null, 0.4f, 1f);
				AudioSystem.StopPooledSounds(0.5f);
				GameManager.UIController.ClearMinigameUI();
				this.actionWaitTime = 0.5f;
				BoardAction boardAction17 = this.currentAction;
				int step = boardAction17.Step;
				boardAction17.Step = step + 1;
				return;
			}
			case 1:
			{
				this.resultScreenScene.GetComponent<ResultSceenScene>().Show();
				AudioSystem.PlayMusic(this.victoryMusic, 0.1f, 1f);
				this.actionWaitFrame = 2;
				BoardAction boardAction18 = this.currentAction;
				int step = boardAction18.Step;
				boardAction18.Step = step + 1;
				return;
			}
			case 2:
			{
				GameManager.ReleaseMinigame();
				BoardAction boardAction19 = this.currentAction;
				int step = boardAction19.Step;
				boardAction19.Step = step + 1;
				this.actionWaitTime = 5f;
				return;
			}
			case 3:
				if (NetSystem.IsServer)
				{
					this.LoadGameBoard();
					this.currentAction = null;
					return;
				}
				this.currentAction = null;
				return;
			default:
				return;
			}
			break;
		case BoardActionType.EquipItem:
		{
			ActionEquipItem actionEquipItem = (ActionEquipItem)this.currentAction;
			GamePlayer playerWithID2 = GameManager.GetPlayerWithID(actionEquipItem.playerID);
			if (NetSystem.IsServer)
			{
				byte itemID = actionEquipItem.ItemID;
				if (itemID >= 0 && (int)itemID < GameManager.ItemList.items.Length)
				{
					NetSystem.Spawn(GameManager.ItemList.items[(int)itemID].netPrefabName, (ushort)actionEquipItem.playerID, playerWithID2.NetOwner);
				}
				else
				{
					Debug.LogError("Item id not found");
				}
				this.currentAction = null;
				return;
			}
			if (playerWithID2.BoardObject.PlayerState != BoardPlayerState.ItemEquipped)
			{
				playerWithID2.BoardObject.SetState(BoardPlayerState.ItemEquipped);
			}
			if (playerWithID2.BoardObject.EquippedItem != null)
			{
				this.currentAction = null;
				return;
			}
			break;
		}
		case BoardActionType.UseItem:
		case BoardActionType.UnEquipItem:
			break;
		case BoardActionType.InteractionChoice:
		{
			ActionInteractionChoice actionInteractionChoice = (ActionInteractionChoice)this.currentAction;
			switch (actionInteractionChoice.Step)
			{
			case 0:
				if (this.curPlayer.PlayerState == BoardPlayerState.MakingInteractionChoice)
				{
					ActionInteractionChoice actionInteractionChoice2 = actionInteractionChoice;
					int step = actionInteractionChoice2.Step;
					actionInteractionChoice2.Step = step + 1;
					return;
				}
				this.actionWaitTime = 0.1f;
				return;
			case 1:
			{
				actionInteractionChoice.interactionScript = this.curPlayer.CurrentNode.CurInteractionScript;
				this.CurPlayer.PlayerState = BoardPlayerState.Interacting;
				actionInteractionChoice.interactionScript.OnInteractionChoice(actionInteractionChoice.InteractionChoice, actionInteractionChoice.Seed);
				ActionInteractionChoice actionInteractionChoice3 = actionInteractionChoice;
				int step = actionInteractionChoice3.Step;
				actionInteractionChoice3.Step = step + 1;
				return;
			}
			case 2:
				if (actionInteractionChoice.interactionScript.Finished)
				{
					this.currentAction = null;
					return;
				}
				break;
			default:
				return;
			}
			break;
		}
		case BoardActionType.ShowWinner:
		{
			ActionShowWinner actionShowWinner = (ActionShowWinner)this.currentAction;
			switch (actionShowWinner.Step)
			{
			case 0:
			{
				if (GameManager.partyGameMode == PartyGameMode.MinigamesOnly)
				{
					this.uiController.minigameOnlySceneController.Hide();
					if (actionShowWinner.placements[0].IsLocalPlayer && !actionShowWinner.placements[0].IsAI)
					{
						PlatformAchievementManager.Instance.TriggerAchievement("ACH_WIN_FIRST_GAME");
					}
				}
				GameManager.SwapScene(new FadeTransition
				{
					duration = 1f,
					fadedDelay = 1.25f,
					fadeToColor = Color.black,
					nextScene = "EndScreen",
					sceneSwitcher = GameManager.switcher
				});
				if (GameManager.partyGameMode == PartyGameMode.BoardGame)
				{
					this.uiController.HideScoreBoard();
					this.uiController.boardDetails.SetActive(false);
				}
				ActionShowWinner actionShowWinner2 = actionShowWinner;
				int step = actionShowWinner2.Step;
				actionShowWinner2.Step = step + 1;
				return;
			}
			case 1:
				if (GameManager.SwitchState == SceneSwitchState.ScreenObscured || GameManager.SwitchState == SceneSwitchState.FadeOut)
				{
					this.minigameIntroScene.SetActive(false);
					BoardAction boardAction20 = this.currentAction;
					int step = boardAction20.Step;
					boardAction20.Step = step + 1;
					return;
				}
				break;
			case 2:
				if (GameManager.SwitchState == SceneSwitchState.FadeOut || GameManager.SwitchState == SceneSwitchState.None)
				{
					GameManager.DisableGameBoard(false, false);
					if (NetSystem.IsServer)
					{
						NetSystem.Spawn("EndScreenManager", 0, NetSystem.MyPlayer);
					}
					AudioListener componentInChildren2 = GameObject.Find("EndScreenParent").GetComponentInChildren<AudioListener>(true);
					if (componentInChildren2 != null)
					{
						componentInChildren2.enabled = true;
					}
					BoardAction boardAction21 = this.currentAction;
					int step = boardAction21.Step;
					boardAction21.Step = step + 1;
					AudioSystem.PlayMusic(this.gameFinishMusic, 0.35f, 1f);
					return;
				}
				break;
			case 3:
				if (this.endScreenManager != null)
				{
					BoardAction boardAction22 = this.currentAction;
					int step = boardAction22.Step;
					boardAction22.Step = step + 1;
					this.endScreenManager.placements = actionShowWinner.placements;
					this.endScreenManager.SetClientLoaded(NetSystem.MyPlayer.UserID, true);
					return;
				}
				break;
			case 4:
				if (GameManager.SwitchState == SceneSwitchState.None)
				{
					this.actionWaitTime = 0.2f;
					BoardAction boardAction23 = this.currentAction;
					int step = boardAction23.Step;
					boardAction23.Step = step + 1;
					return;
				}
				break;
			case 5:
				if (this.endScreenManager.AllClientsLoaded())
				{
					BoardAction boardAction24 = this.currentAction;
					int step = boardAction24.Step;
					boardAction24.Step = step + 1;
					this.endScreenManager.StartIntro();
					return;
				}
				break;
			case 6:
				this.currentAction = null;
				if (NetSystem.IsServer)
				{
					ActionSendStatistics newAction8 = new ActionSendStatistics(true);
					this.QueueAction(newAction8, true, true);
					return;
				}
				break;
			default:
				return;
			}
			break;
		}
		case BoardActionType.SetupMinigameOnlyLobby:
		{
			ActionSetupMinigameOnlyLobby actionSetupMinigameOnlyLobby = (ActionSetupMinigameOnlyLobby)this.currentAction;
			if (actionSetupMinigameOnlyLobby.Step == 0)
			{
				this.nextMinigame = GameManager.GetMinigameList()[(int)actionSetupMinigameOnlyLobby.MinigameID];
				this.currentAction = null;
				return;
			}
			break;
		}
		case BoardActionType.KillerMove:
		{
			ActionKillerMove actionKillerMove = (ActionKillerMove)this.currentAction;
			switch (actionKillerMove.Step)
			{
			case 0:
			{
				this.slasherEnemy.StartMusic();
				this.boardCamera.SetTrackedObject(this.slasherEnemy.transform, Vector3.zero);
				ActionKillerMove actionKillerMove2 = actionKillerMove;
				int step = actionKillerMove2.Step;
				actionKillerMove2.Step = step + 1;
				return;
			}
			case 1:
				if (this.boardCamera.WithinDistance(0.5f))
				{
					ActionKillerMove actionKillerMove3 = actionKillerMove;
					int step = actionKillerMove3.Step;
					actionKillerMove3.Step = step + 1;
					return;
				}
				break;
			case 2:
			{
				this.slasherEnemy.MoveTo(actionKillerMove.Point);
				ActionKillerMove actionKillerMove4 = actionKillerMove;
				int step = actionKillerMove4.Step;
				actionKillerMove4.Step = step + 1;
				return;
			}
			case 3:
				if (this.slasherEnemy.FinishedMoving())
				{
					this.actionWaitTime = 1f;
					ActionKillerMove actionKillerMove5 = actionKillerMove;
					int step = actionKillerMove5.Step;
					actionKillerMove5.Step = step + 1;
					return;
				}
				break;
			case 4:
				if (NetSystem.IsServer)
				{
					this.OnTurnsEnd();
				}
				this.currentAction = null;
				this.slasherEnemy.StopMusic();
				return;
			default:
				return;
			}
			break;
		}
		case BoardActionType.KillerAttack:
		{
			ActionKillerAttack actionKillerAttack = (ActionKillerAttack)this.currentAction;
			switch (actionKillerAttack.Step)
			{
			case 0:
			{
				this.boardCamera.SetTrackedObject(this.slasherEnemy.transform, Vector3.zero);
				ActionKillerAttack actionKillerAttack2 = actionKillerAttack;
				int step = actionKillerAttack2.Step;
				actionKillerAttack2.Step = step + 1;
				this.slasherEnemy.StartMusic();
				return;
			}
			case 1:
				if (this.boardCamera.WithinDistance(0.5f))
				{
					ActionKillerAttack actionKillerAttack3 = actionKillerAttack;
					int step = actionKillerAttack3.Step;
					actionKillerAttack3.Step = step + 1;
					return;
				}
				break;
			case 2:
			{
				this.slasherEnemy.MoveTo(this.GetActor(actionKillerAttack.ActorID).transform.position);
				ActionKillerAttack actionKillerAttack4 = actionKillerAttack;
				int step = actionKillerAttack4.Step;
				actionKillerAttack4.Step = step + 1;
				return;
			}
			case 3:
				if (this.slasherEnemy.FinishedMoving())
				{
					this.slasherEnemy.Attack(actionKillerAttack.ActorID);
					ActionKillerAttack actionKillerAttack5 = actionKillerAttack;
					int step = actionKillerAttack5.Step;
					actionKillerAttack5.Step = step + 1;
					return;
				}
				break;
			case 4:
				if (this.slasherEnemy.attackFinished)
				{
					if (NetSystem.IsServer)
					{
						this.OnTurnsEnd();
					}
					this.slasherEnemy.StopMusic();
					this.currentAction = null;
					return;
				}
				break;
			default:
				return;
			}
			break;
		}
		case BoardActionType.StartPummelAwards:
		{
			ActionStartPummelAwards actionStartPummelAwards = (ActionStartPummelAwards)this.currentAction;
			switch (actionStartPummelAwards.Step)
			{
			case 0:
			{
				AudioSystem.PlayMusic(null, 1.5f, 1f);
				if (GameManager.partyGameMode == PartyGameMode.MinigamesOnly)
				{
					this.uiController.minigameOnlySceneController.Hide();
				}
				GameManager.SwapScene(new FadeTransition
				{
					duration = 2f,
					fadedDelay = 0.5f,
					fadeToColor = Color.black,
					nextScene = "GobletChallengeAwardScene",
					sceneSwitcher = GameManager.switcher
				});
				if (GameManager.partyGameMode == PartyGameMode.BoardGame)
				{
					this.uiController.HideScoreBoard();
					this.uiController.boardDetails.SetActive(false);
				}
				ActionStartPummelAwards actionStartPummelAwards2 = actionStartPummelAwards;
				int step = actionStartPummelAwards2.Step;
				actionStartPummelAwards2.Step = step + 1;
				return;
			}
			case 1:
				if (GameManager.SwitchState == SceneSwitchState.ScreenObscured || GameManager.SwitchState == SceneSwitchState.FadeOut)
				{
					this.minigameIntroScene.SetActive(false);
					BoardAction boardAction25 = this.currentAction;
					int step = boardAction25.Step;
					boardAction25.Step = step + 1;
					return;
				}
				break;
			case 2:
				if (GameManager.SwitchState == SceneSwitchState.FadeOut || GameManager.SwitchState == SceneSwitchState.None)
				{
					GameManager.DisableGameBoard(false, true);
					foreach (BoardPlayer boardPlayer3 in this.boardPlayerList)
					{
						boardPlayer3.WasVisible = boardPlayer3.Visible;
						boardPlayer3.Visible = false;
					}
					if (NetSystem.IsServer)
					{
						NetSystem.Spawn("AwardSceneManager", 0, NetSystem.MyPlayer);
					}
					AudioListener componentInChildren3 = GameObject.Find("GobletChallengeAwardsParent").GetComponentInChildren<AudioListener>(true);
					if (componentInChildren3 != null)
					{
						componentInChildren3.enabled = true;
					}
					BoardAction boardAction26 = this.currentAction;
					int step = boardAction26.Step;
					boardAction26.Step = step + 1;
					return;
				}
				break;
			case 3:
				if (this.awardSceneManager != null)
				{
					BoardAction boardAction27 = this.currentAction;
					int step = boardAction27.Step;
					boardAction27.Step = step + 1;
					this.awardSceneManager.SetClientLoaded(NetSystem.MyPlayer.UserID, true);
					return;
				}
				break;
			case 4:
				if (GameManager.SwitchState == SceneSwitchState.None)
				{
					this.actionWaitTime = 0.2f;
					BoardAction boardAction28 = this.currentAction;
					int step = boardAction28.Step;
					boardAction28.Step = step + 1;
					return;
				}
				break;
			case 5:
				if (this.awardSceneManager.AllClientsLoaded())
				{
					BoardAction boardAction29 = this.currentAction;
					int step = boardAction29.Step;
					boardAction29.Step = step + 1;
					this.awardSceneManager.StartIntro(actionStartPummelAwards.doIntroduction, actionStartPummelAwards.ev, actionStartPummelAwards.isGameFinished);
					return;
				}
				break;
			case 6:
				this.currentAction = null;
				return;
			default:
				return;
			}
			break;
		}
		case BoardActionType.EndPummelAwards:
		{
			ActionEndPummelAwards actionEndPummelAwards = (ActionEndPummelAwards)this.currentAction;
			switch (actionEndPummelAwards.Step)
			{
			case 0:
			{
				this.actionWaitTime = 3f;
				BoardAction boardAction30 = this.currentAction;
				int step = boardAction30.Step;
				boardAction30.Step = step + 1;
				return;
			}
			case 1:
			{
				GobletChallengeAwards gobletChallengeAwards = UnityEngine.Object.FindObjectOfType<GobletChallengeAwards>();
				if (gobletChallengeAwards != null)
				{
					AudioSystem.PlayMusic(null, 0.4f, 1f);
					AudioSystem.StopPooledSounds(0.5f);
					gobletChallengeAwards.ShowOutro();
					this.actionWaitTime = 3f;
					BoardAction boardAction31 = this.currentAction;
					int step = boardAction31.Step;
					boardAction31.Step = step + 1;
					return;
				}
				break;
			}
			case 2:
			{
				GobletChallengeAwards gobletChallengeAwards2 = UnityEngine.Object.FindObjectOfType<GobletChallengeAwards>();
				if (gobletChallengeAwards2 != null)
				{
					UnityEngine.Object.Destroy(gobletChallengeAwards2.gameObject, 1f);
					BoardAction boardAction32 = this.currentAction;
					int step = boardAction32.Step;
					boardAction32.Step = step + 1;
					return;
				}
				break;
			}
			case 3:
				if (!NetSystem.IsServer)
				{
					BoardAction boardAction33 = this.currentAction;
					int step = boardAction33.Step;
					boardAction33.Step = step + 1;
					return;
				}
				if (this.awardSceneManager == null)
				{
					this.awardSceneManager = UnityEngine.Object.FindObjectOfType<AwardSceneManager>();
				}
				if (this.awardSceneManager != null)
				{
					this.awardSceneManager.Cleanup();
					BoardAction boardAction34 = this.currentAction;
					int step = boardAction34.Step;
					boardAction34.Step = step + 1;
					return;
				}
				break;
			case 4:
				foreach (BoardPlayer boardPlayer4 in this.boardPlayerList)
				{
					boardPlayer4.Visible = boardPlayer4.WasVisible;
				}
				if (!NetSystem.IsServer)
				{
					this.currentAction = null;
					return;
				}
				if (actionEndPummelAwards.isGameFinished)
				{
					if (NetSystem.IsServer)
					{
						this.FinishGame();
					}
					this.currentAction = null;
					return;
				}
				if (NetSystem.IsServer)
				{
					this.StartMinigame();
				}
				this.currentAction = null;
				return;
			default:
				return;
			}
			break;
		}
		case BoardActionType.SendStatistics:
		{
			ActionSendStatistics actionSendStatistics = (ActionSendStatistics)this.currentAction;
			int step2 = actionSendStatistics.Step;
			if (step2 == 0)
			{
				if (!NetSystem.IsServer)
				{
					Debug.Log("Applying statistics!");
					actionSendStatistics.ApplyStats();
				}
				BoardAction boardAction35 = this.currentAction;
				int step = boardAction35.Step;
				boardAction35.Step = step + 1;
				this.actionWaitTime = 1f;
				return;
			}
			if (step2 != 1)
			{
				return;
			}
			this.currentAction = null;
			return;
		}
		case BoardActionType.SpawnWeapon:
		{
			ActionSpawnWeapon actionSpawnWeapon = (ActionSpawnWeapon)this.currentAction;
			switch (actionSpawnWeapon.Step)
			{
			case 0:
			{
				if (!(this.weaponNodes[(int)actionSpawnWeapon.WeaponIndex] == null))
				{
					this.DespawnWeaponSpace((int)actionSpawnWeapon.WeaponIndex);
				}
				ActionSpawnWeapon actionSpawnWeapon2 = actionSpawnWeapon;
				int step = actionSpawnWeapon2.Step;
				actionSpawnWeapon2.Step = step + 1;
				return;
			}
			case 1:
			{
				this.actionWaitTime = 0.5f;
				ActionSpawnWeapon actionSpawnWeapon3 = actionSpawnWeapon;
				int step = actionSpawnWeapon3.Step;
				actionSpawnWeapon3.Step = step + 1;
				GameManager.UIController.HideBoardControls();
				return;
			}
			case 2:
			{
				this.weaponNodes[(int)actionSpawnWeapon.WeaponIndex] = this.boardNodes[(int)actionSpawnWeapon.NodeIndex];
				this.weaponNodes[(int)actionSpawnWeapon.WeaponIndex].CurrentNodeType = BoardNodeType.Weapon;
				this.weaponNodes[(int)actionSpawnWeapon.WeaponIndex].CurHasInteraction = true;
				this.boardCamera.MoveTo(this.weaponNodes[(int)actionSpawnWeapon.WeaponIndex].gameObject.transform, new Vector3(0f, 0.35f, 0f), GameManager.Board.boardCamera.targetDistScale);
				ActionSpawnWeapon actionSpawnWeapon4 = actionSpawnWeapon;
				int step = actionSpawnWeapon4.Step;
				actionSpawnWeapon4.Step = step + 1;
				return;
			}
			case 3:
				if (this.boardCamera.WithinDistance(0.5f))
				{
					Quaternion quaternion2 = Quaternion.Euler(new Vector3(0f, this.weaponNodes[(int)actionSpawnWeapon.WeaponIndex].yRotation + this.weaponNodes[(int)actionSpawnWeapon.WeaponIndex].transform.eulerAngles.y, 0f));
					Vector3 a2 = quaternion2 * Vector3.forward;
					Vector3 position2 = this.weaponNodes[(int)actionSpawnWeapon.WeaponIndex].NodePosition + a2 * 1.5f;
					position2.y -= 0.15f;
					if (this.WeaponGoalScript[(int)actionSpawnWeapon.WeaponIndex] == null)
					{
						this.WeaponGoalScript[(int)actionSpawnWeapon.WeaponIndex] = UnityEngine.Object.Instantiate<GameObject>(this.weaponObjPrefab, position2, quaternion2 * Quaternion.Euler(0f, 180f, 0f)).GetComponent<WeaponGoal>();
						this.WeaponGoalScript[(int)actionSpawnWeapon.WeaponIndex].Spawn(actionSpawnWeapon.WeaponID);
					}
					else
					{
						this.WeaponGoalScript[(int)actionSpawnWeapon.WeaponIndex].transform.position = position2;
						this.WeaponGoalScript[(int)actionSpawnWeapon.WeaponIndex].transform.rotation = quaternion2 * Quaternion.Euler(0f, 180f, 0f);
						this.WeaponGoalScript[(int)actionSpawnWeapon.WeaponIndex].Spawn(actionSpawnWeapon.WeaponID);
					}
					this.weaponNodes[(int)actionSpawnWeapon.WeaponIndex].CurInteractionScript = this.WeaponGoalScript[(int)actionSpawnWeapon.WeaponIndex].GetComponent<Interaction>();
					if (this.firstWeapon && actionSpawnWeapon.WeaponIndex == 0)
					{
						this.uiController.GetComponentInChildren<SimpleInteractionDialog>().Activate("WeaponCacheTitle", "WeaponCacheDescription", null, GameManager.PlayerList[0], this.weaponIcon);
						this.actionWaitTime = 5f;
					}
					else
					{
						this.actionWaitTime = 2f;
					}
					ActionSpawnWeapon actionSpawnWeapon5 = actionSpawnWeapon;
					int step = actionSpawnWeapon5.Step;
					actionSpawnWeapon5.Step = step + 1;
					return;
				}
				break;
			case 4:
			{
				int step;
				if (this.firstWeapon)
				{
					if ((int)actionSpawnWeapon.WeaponIndex == this.weaponNodes.Length - 1)
					{
						this.uiController.GetComponentInChildren<SimpleInteractionDialog>().window.SetState(MainMenuWindowState.Hidden);
						GameManager.UIController.SetInputStatus(false);
					}
					ActionSpawnWeapon actionSpawnWeapon6 = actionSpawnWeapon;
					step = actionSpawnWeapon6.Step;
					actionSpawnWeapon6.Step = step + 1;
					return;
				}
				ActionSpawnWeapon actionSpawnWeapon7 = actionSpawnWeapon;
				step = actionSpawnWeapon7.Step;
				actionSpawnWeapon7.Step = step + 1;
				return;
			}
			case 5:
				if (this.firstWeapon)
				{
					if ((int)actionSpawnWeapon.WeaponIndex == this.weaponNodes.Length - 1)
					{
						if (NetSystem.IsServer)
						{
							if (this.isHalloweenMap)
							{
								this.SwitchState(GameBoardState.ShowingKiller);
							}
							else
							{
								this.GetNextPlayer();
								this.SwitchState(GameBoardState.PlayTurns);
							}
						}
						this.firstWeapon = false;
					}
					else
					{
						this.SpawnWeapon();
					}
					this.currentAction = null;
					return;
				}
				if (!this.firstWeapon)
				{
					this.currentAction = null;
					return;
				}
				break;
			default:
				return;
			}
			break;
		}
		case BoardActionType.PersistentItemEvent:
		{
			ActionPersistentItemEvent actionPersistentItemEvent = (ActionPersistentItemEvent)this.currentAction;
			PersistentItem persistentItem = this.persistentItems[(int)actionPersistentItemEvent.ItemIndex];
			int step2 = actionPersistentItemEvent.Step;
			if (step2 == 0)
			{
				using (MemoryStream memoryStream = new MemoryStream(actionPersistentItemEvent.Array))
				{
					using (BinaryReader binaryReader = new BinaryReader(memoryStream))
					{
						base.StartCoroutine(persistentItem.DoEvent(actionPersistentItemEvent.EventType, binaryReader));
					}
				}
				ActionPersistentItemEvent actionPersistentItemEvent2 = actionPersistentItemEvent;
				int step = actionPersistentItemEvent2.Step;
				actionPersistentItemEvent2.Step = step + 1;
				return;
			}
			if (step2 != 1)
			{
				return;
			}
			if (persistentItem.HasFinished[(int)actionPersistentItemEvent.EventType])
			{
				this.currentAction = null;
				if (persistentItem.DestroyAfter)
				{
					this.RemovePersistentItem(persistentItem);
					persistentItem.DoDestroy();
				}
				if (NetSystem.IsServer && actionPersistentItemEvent.EventType == PersistentItemEventType.LastTurn)
				{
					this.OnTurnsEnd();
				}
			}
			break;
		}
		case BoardActionType.ShellGame:
		{
			ActionShellGame actionShellGame = (ActionShellGame)this.currentAction;
			switch (actionShellGame.Step)
			{
			case 0:
			{
				this.shellGameSeed = actionShellGame.Seed;
				this.m_realChestIndex = actionShellGame.RealChestIndex;
				for (int k = 0; k < this.GoalScript.Length; k++)
				{
					BoardGoalBase boardGoalBase = this.GoalScript[k];
					BoardNode x = this.goalNodes[k];
					if (boardGoalBase != null && x != null)
					{
						boardGoalBase.Despawn();
						this.goalNodes[k].ResetNode();
					}
					this.goalNodes[k] = null;
					this.m_goalSpawnQueued[k] = false;
				}
				ActionShellGame actionShellGame2 = actionShellGame;
				int step = actionShellGame2.Step;
				actionShellGame2.Step = step + 1;
				this.actionWaitTime = 1f;
				return;
			}
			case 1:
			{
				Transform transform = GameManager.BoardRoot.transform.Find("ShellGameLookAtPoint");
				if (transform == null)
				{
					transform = GameManager.BoardRoot.transform.Find("AdditiveRoot/ShellGameLookAtPoint");
				}
				if (transform == null)
				{
					transform = new GameObject("ShellGameLookAtPoint").transform;
				}
				this.boardCamera.MoveTo(transform, new Vector3(0f, 0.35f, 0f), GameManager.Board.boardCamera.targetDistScale);
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>((GameObject)Resources.Load("ShellGame"), null);
				this.m_shellGameController = gameObject.GetComponent<ShellGameTest>();
				gameObject.transform.position = transform.position;
				ActionShellGame actionShellGame3 = actionShellGame;
				int step = actionShellGame3.Step;
				actionShellGame3.Step = step + 1;
				return;
			}
			case 2:
				if (this.boardCamera.WithinDistance(0.5f))
				{
					this.actionWaitTime = 1f;
					ActionShellGame actionShellGame4 = actionShellGame;
					int step = actionShellGame4.Step;
					actionShellGame4.Step = step + 1;
					return;
				}
				break;
			case 3:
			{
				this.m_shellGameController.DoShellGame(actionShellGame);
				this.actionWaitTime = 16f;
				ActionShellGame actionShellGame5 = actionShellGame;
				int step = actionShellGame5.Step;
				actionShellGame5.Step = step + 1;
				return;
			}
			case 4:
				if (NetSystem.IsServer)
				{
					if (GameManager.SaveToLoad != null)
					{
						for (int l = 0; l < GameManager.SaveToLoad.goalNodeIDs.Length; l++)
						{
							if (GameManager.SaveToLoad.goalNodeIDs[l] != -1)
							{
								this.QueueAction(new ActionSpawnGoal((byte)l, GameManager.SaveToLoad.goalNodeIDs[l]), true, true);
								break;
							}
						}
					}
					else
					{
						this.QueueAction(new ActionSpawnGoal(0, this.GetGoalSpawnNodeIndex(false)), true, true);
					}
				}
				this.m_goalSpawnQueued[0] = true;
				this.currentAction = null;
				return;
			default:
				return;
			}
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x06001AD1 RID: 6865 RVA: 0x000B59C0 File Offset: 0x000B3BC0
	public int GetGoalIndex(BoardNode node)
	{
		int result = 0;
		for (int i = 0; i < this.goalNodes.Length; i++)
		{
			if (node == this.goalNodes[i])
			{
				result = i;
			}
		}
		return result;
	}

	// Token: 0x06001AD2 RID: 6866 RVA: 0x000B59F8 File Offset: 0x000B3BF8
	public int GetWeaponGoalIndex(BoardNode node)
	{
		int result = 0;
		for (int i = 0; i < this.weaponNodes.Length; i++)
		{
			if (node == this.weaponNodes[i])
			{
				result = i;
			}
		}
		return result;
	}

	// Token: 0x06001AD3 RID: 6867 RVA: 0x00013C07 File Offset: 0x00011E07
	public void SpawnNewGoal(int index)
	{
		if (this.areFakeChestsActive)
		{
			this.QueueAction(new ActionShellGame(GameManager.rand.Next()), true, true);
			return;
		}
		this.QueueAction(new ActionSpawnGoal((byte)index, this.GetGoalSpawnNodeIndex(false)), true, true);
	}

	// Token: 0x06001AD4 RID: 6868 RVA: 0x0000398C File Offset: 0x00001B8C
	public void DoShellGame()
	{
	}

	// Token: 0x06001AD5 RID: 6869 RVA: 0x000B5A30 File Offset: 0x000B3C30
	public void SpawnNewWeaponGoal(int index)
	{
		byte itemID = GameManager.ItemList.GetRandomItem(this.CurPlayer, true).itemID;
		this.QueueAction(new ActionSpawnWeapon((byte)index, itemID, this.GetGoalSpawnNodeIndex(true)), true, true);
	}

	// Token: 0x06001AD6 RID: 6870 RVA: 0x000B5A6C File Offset: 0x000B3C6C
	public void EndTurn()
	{
		Debug.Log("Ending Turn");
		if (this.curPlayer != null)
		{
			this.curPlayer.EndTurn();
		}
		else
		{
			Debug.LogError("Curplayer is null on EndTurn() this shouldn't happen");
		}
		this.curPlayer = null;
		this.curPlayerIndex++;
		this.boardCamera.SetTrackedObject(null, Vector3.zero);
		for (int i = 0; i < this.persistentItems.Count; i++)
		{
			this.persistentItems[i].HasFinished[1] = false;
		}
		this.startedPlayersTurn = false;
	}

	// Token: 0x06001AD7 RID: 6871 RVA: 0x000B5B00 File Offset: 0x000B3D00
	public void EndInteraction()
	{
		Debug.Log("Ending Interaction");
		this.curPlayer.PlayerState = BoardPlayerState.CompletedInteraction;
		if (this.curPlayer.GamePlayer.IsLocalPlayer)
		{
			ActionWait newAction = new ActionWait(1f);
			ActionMovePlayer newAction2 = new ActionMovePlayer(this.curPlayer.GamePlayer.GlobalID, (byte)this.curPlayer.MoveStepsRemaining, 0, false);
			this.QueueAction(newAction, true, true);
			this.QueueAction(newAction2, true, true);
		}
	}

	// Token: 0x06001AD8 RID: 6872 RVA: 0x000B5B78 File Offset: 0x000B3D78
	private void DoActionSimple(SimpleBoardAction action)
	{
		try
		{
			switch (action)
			{
			case SimpleBoardAction.RollDiceTurnOrder:
				switch (this.currentAction.Step)
				{
				case 0:
				{
					this.actionWaitTime = 0.5f;
					BoardAction boardAction = this.currentAction;
					int step = boardAction.Step;
					boardAction.Step = step + 1;
					if (GameManager.modifierUI != null && !this.m_shownModifiers)
					{
						GameManager.modifierUI.ShowAllModifiers((float)this.m_modifierDefinitions.Count * 4f);
						this.m_shownModifiers = true;
						goto IL_9F2;
					}
					goto IL_9F2;
				}
				case 1:
				{
					this.actionWaitTime = 1f;
					BoardAction boardAction2 = this.currentAction;
					int step = boardAction2.Step;
					boardAction2.Step = step + 1;
					string translation = LocalizationManager.GetTranslation("Determine Turn Order", true, 0, true, false, null, null, true);
					GameManager.UIController.ShowLargeText(translation + "!", LargeTextType.RollTurnOrder, 3f, false, false);
					GameManager.UIController.SetBoardInputHelpType(BoardInputType.RollTurnOrder);
					GameManager.UIController.ShowBoardControls();
					goto IL_9F2;
				}
				case 2:
					foreach (BoardPlayer boardPlayer in this.boardPlayerList)
					{
						boardPlayer.RollDice(BoardDiceType.TurnOrder);
					}
					this.currentAction = null;
					goto IL_9F2;
				default:
					goto IL_9F2;
				}
				break;
			case SimpleBoardAction.RollDiceMovement:
			{
				if (this.CurPlayer.postMinigameItem != 255)
				{
					this.CurPlayer.GiveItem(this.CurPlayer.postMinigameItem, true);
					this.CurPlayer.postMinigameItem = byte.MaxValue;
				}
				bool isServer = NetSystem.IsServer;
				this.curPlayer.StartTurn();
				this.curPlayer.RollDice(BoardDiceType.Movement);
				this.currentAction = null;
				goto IL_9F2;
			}
			case SimpleBoardAction.StartMinigame:
				switch (this.currentAction.Step)
				{
				case 0:
				{
					GameManager.UIController.SetInputStatus(false);
					AudioSystem.PlayMusic(null, 0.5f, 1f);
					this.uiController.MinigameLoadScreen.SetFadeImage(true);
					this.uiController.MinigameLoadScreen.FadeIn(0.25f);
					this.uiController.MinigameLoadScreen.ShowScreen(false);
					this.actionWaitTime = 0.3f;
					BoardAction boardAction3 = this.currentAction;
					int step = boardAction3.Step;
					boardAction3.Step = step + 1;
					GameManager.Minigame.OnPlayersReady();
					goto IL_9F2;
				}
				case 1:
				{
					this.minigameIntroScene.SetActive(false);
					GameManager.Minigame.SetCamerasEnabled(true);
					this.actionWaitTime = 0.05f;
					BoardAction boardAction4 = this.currentAction;
					int step = boardAction4.Step;
					boardAction4.Step = step + 1;
					goto IL_9F2;
				}
				case 2:
				{
					this.uiController.MinigameLoadScreen.FadeOut(0.25f);
					this.actionWaitTime = 0.3f;
					BoardAction boardAction5 = this.currentAction;
					int step = boardAction5.Step;
					boardAction5.Step = step + 1;
					goto IL_9F2;
				}
				case 3:
					this.uiController.MinigameLoadScreen.SetFadeImage(false);
					GameManager.Minigame.StartMinigameBase(1f);
					this.currentAction = null;
					goto IL_9F2;
				default:
					goto IL_9F2;
				}
				break;
			case SimpleBoardAction.LoadGameBoard:
				switch (this.currentAction.Step)
				{
				case 0:
				{
					this.minigameStarted = false;
					this.uiController.MinigameLoadScreen.SetFadeImage(true);
					this.uiController.MinigameLoadScreen.FadeIn(0.4f);
					AudioSystem.PlayMusic(null, 0.5f, 1f);
					NetSystem.SetSendRate(30, 30);
					this.actionWaitTime = 0.5f;
					BoardAction boardAction6 = this.currentAction;
					int step = boardAction6.Step;
					boardAction6.Step = step + 1;
					goto IL_9F2;
				}
				case 1:
				{
					if (GameManager.partyGameMode == PartyGameMode.BoardGame)
					{
						this.GetNextPlayer();
					}
					else
					{
						this.minigameIntroScene.SetActive(true);
					}
					GameManager.EnableGameBoard();
					GameManager.HideResultScreen();
					if (GameManager.partyGameMode == PartyGameMode.BoardGame)
					{
						GameManager.UIController.ShowScoreBoard();
						GameManager.UIController.boardDetails.SetActive(true);
						if (GameManager.modifierUI)
						{
							GameManager.modifierUI.Show();
						}
						if (this.uiController != null && this.uiController.boardDetails != null)
						{
							this.uiController.boardDetails.SetActive(true);
						}
					}
					else
					{
						this.uiController.minigameOnlySceneController.Show();
					}
					this.actionWaitTime = 0.1f;
					BoardAction boardAction7 = this.currentAction;
					int step = boardAction7.Step;
					boardAction7.Step = step + 1;
					goto IL_9F2;
				}
				case 2:
				{
					this.uiController.MinigameLoadScreen.FadeOut(0.4f);
					this.actionWaitTime = 0.5f;
					BoardAction boardAction8 = this.currentAction;
					int step = boardAction8.Step;
					boardAction8.Step = step + 1;
					goto IL_9F2;
				}
				case 3:
					this.uiController.MinigameLoadScreen.SetFadeImage(false);
					this.currentAction = null;
					goto IL_9F2;
				default:
					goto IL_9F2;
				}
				break;
			case SimpleBoardAction.StartViewMap:
				AudioSystem.PlayOneShot("MultimediaButtonClick007_STD_ZapSplat", 0.3f, 0f);
				this.curPlayer.StartViewMap();
				this.currentAction = null;
				goto IL_9F2;
			case SimpleBoardAction.EndViewMap:
				AudioSystem.PlayOneShot("MultimediaButtonClick007_STD_ZapSplat", 0.3f, 0f);
				this.curPlayer.EndViewMap();
				this.currentAction = null;
				goto IL_9F2;
			case SimpleBoardAction.Other:
				this.currentAction = null;
				goto IL_9F2;
			case SimpleBoardAction.ChooseMinigame:
			{
				int step2 = this.currentAction.Step;
				if (step2 != 0)
				{
					if (step2 != 1)
					{
						goto IL_9F2;
					}
					if (this.minigame_choice_wnd.minigame_choice != "")
					{
						if (this.minigame_choice_wnd.minigame_choice == "Skip")
						{
							this.GetNextPlayer();
							this.SwitchState(GameBoardState.PlayTurns);
						}
						else
						{
							this.SwitchState(GameBoardState.Minigame);
							ActionLoadMinigame newAction = new ActionLoadMinigame(this.minigame_choice_wnd.minigame_choice, 0);
							this.QueueAction(newAction, true, true);
							GameManager.UIController.SetInputStatus(false);
							this.minigameStarted = true;
						}
						this.currentAction = null;
						goto IL_9F2;
					}
					goto IL_9F2;
				}
				else
				{
					bool flag = false;
					if (flag)
					{
						this.minigame_choice_wnd = GameManager.UIController.ShowMinigameChoiceWindow();
						GameManager.UIController.SetInputStatus(true);
					}
					if (!NetSystem.IsServer)
					{
						this.currentAction = null;
						goto IL_9F2;
					}
					if (flag)
					{
						BoardAction boardAction9 = this.currentAction;
						int step = boardAction9.Step;
						boardAction9.Step = step + 1;
						goto IL_9F2;
					}
					if (false)
					{
						this.GetNextPlayer();
						this.SwitchState(GameBoardState.PlayTurns);
						this.minigameStarted = false;
					}
					else
					{
						this.SwitchState(GameBoardState.Minigame);
						MinigameDefinition randomMinigame;
						if (GameManager.partyGameMode == PartyGameMode.MinigamesOnly)
						{
							randomMinigame = this.uiController.minigameOnlySceneController.nextMinigame;
						}
						else
						{
							randomMinigame = GameManager.GetRandomMinigame();
						}
						int randomAlternateIndex = randomMinigame.GetRandomAlternateIndex();
						ActionLoadMinigame newAction2 = new ActionLoadMinigame(randomMinigame.minigameName, randomAlternateIndex);
						this.QueueAction(newAction2, true, true);
						this.minigameStarted = true;
					}
					this.currentAction = null;
					goto IL_9F2;
				}
				break;
			}
			case SimpleBoardAction.FinishUsingItem:
			{
				int step2 = this.currentAction.Step;
				if (step2 == 0)
				{
					GameManager.Board.CameraTrackCurrentPlayer();
					this.actionWaitTime = 1f;
					BoardAction boardAction10 = this.currentAction;
					int step = boardAction10.Step;
					boardAction10.Step = step + 1;
					goto IL_9F2;
				}
				if (step2 != 1)
				{
					goto IL_9F2;
				}
				bool skipTurnAfterUse = this.curPlayer.EquippedItem.skipTurnAfterUse;
				this.curPlayer.EquippedItem.Unequip(skipTurnAfterUse);
				this.currentAction = null;
				if (skipTurnAfterUse)
				{
					this.EndTurn();
					goto IL_9F2;
				}
				goto IL_9F2;
			}
			case SimpleBoardAction.OpenInventory:
				if (this.CurPlayer.GamePlayer.IsLocalPlayer)
				{
					GameManager.UIController.SetBoardInputHelpType(BoardInputType.Inventory);
				}
				this.curPlayer.OpenInventory();
				this.currentAction = null;
				goto IL_9F2;
			case SimpleBoardAction.CloseInventory:
				if (this.CurPlayer.GamePlayer.IsLocalPlayer)
				{
					GameManager.UIController.SetBoardInputHelpType(this.CurPlayer.HasUsedItem ? BoardInputType.PlayerTurnUsedItem : BoardInputType.PlayerTurn);
				}
				this.curPlayer.CloseInventory();
				this.currentAction = null;
				goto IL_9F2;
			case SimpleBoardAction.EndTurn:
				this.EndTurn();
				this.currentAction = null;
				goto IL_9F2;
			case SimpleBoardAction.SetupItem:
				if (this.curPlayer.EquippedItem != null)
				{
					this.curPlayer.EquippedItem.Setup();
					this.currentAction = null;
					goto IL_9F2;
				}
				goto IL_9F2;
			case SimpleBoardAction.CancelItem:
				if (this.curPlayer.EquippedItem != null)
				{
					this.curPlayer.EquippedItem.Unequip(false);
					this.currentAction = null;
					goto IL_9F2;
				}
				goto IL_9F2;
			case SimpleBoardAction.ShowKiller:
				switch (this.currentAction.Step)
				{
				case 0:
				{
					BoardAction boardAction11 = this.currentAction;
					int step = boardAction11.Step;
					boardAction11.Step = step + 1;
					goto IL_9F2;
				}
				case 1:
					if (this.boardCamera.WithinDistance(0.5f))
					{
						this.slasherEnemy.Reveal();
						string translation2 = LocalizationManager.GetTranslation("BoardExplanationHalloweenKiller", true, 0, true, false, null, null, true);
						GameManager.UIController.GetComponentInChildren<HalloweenTextDissolve>().Show(translation2, 1f);
						this.actionWaitTime = 5.5f;
						BoardAction boardAction12 = this.currentAction;
						int step = boardAction12.Step;
						boardAction12.Step = step + 1;
						goto IL_9F2;
					}
					goto IL_9F2;
				case 2:
					if (NetSystem.IsServer)
					{
						this.GetNextPlayer();
						this.SwitchState(GameBoardState.PlayTurns);
					}
					GameManager.UIController.GetComponentInChildren<HalloweenTextDissolve>().Hide(1f);
					this.currentAction = null;
					goto IL_9F2;
				default:
					goto IL_9F2;
				}
				break;
			case SimpleBoardAction.RespawnKiller:
				switch (this.currentAction.Step)
				{
				case 0:
				{
					Transform transform = GameManager.BoardRoot.transform.Find("Events/KillerSpawnPoint");
					this.slasherEnemy.transform.position = transform.position;
					this.boardCamera.SetTrackedObject(this.slasherEnemy.transform, Vector3.zero);
					BoardAction boardAction13 = this.currentAction;
					int step = boardAction13.Step;
					boardAction13.Step = step + 1;
					goto IL_9F2;
				}
				case 1:
					if (this.boardCamera.WithinDistance(0.5f))
					{
						this.slasherEnemy.Reveal();
						this.actionWaitTime = 5.5f;
						BoardAction boardAction14 = this.currentAction;
						int step = boardAction14.Step;
						boardAction14.Step = step + 1;
						goto IL_9F2;
					}
					goto IL_9F2;
				case 2:
					if (NetSystem.IsServer)
					{
						this.OnTurnsEnd();
					}
					this.currentAction = null;
					goto IL_9F2;
				default:
					goto IL_9F2;
				}
				break;
			}
			Debug.LogError("Board simple action not implemented : " + action.ToString());
			this.currentAction = null;
			IL_9F2:;
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.ToString() + "Simple Action Type: " + action.ToString());
			Debug.LogError(ex.StackTrace);
		}
	}

	// Token: 0x06001AD9 RID: 6873 RVA: 0x000B65DC File Offset: 0x000B47DC
	private void CreateDirectionChoices(BoardNode cur_node, List<BoardNode> node_choices)
	{
		this.activeArrows.Clear();
		int num = 0;
		foreach (BoardNode boardNode in node_choices)
		{
			Vector3 position = cur_node.NodePosition + (boardNode.NodePosition - cur_node.NodePosition) * 0.5f + new Vector3(0f, 0.5f, 0f);
			Quaternion rotation = default(Quaternion);
			Vector3 normalized = (boardNode.NodePosition - cur_node.NodePosition).normalized;
			rotation.SetLookRotation(normalized, Vector3.up);
			DirectionChoiceArrow component = UnityEngine.Object.Instantiate<GameObject>(this.directionChoicePrefab, position, rotation).GetComponent<DirectionChoiceArrow>();
			component.Initialize(num, this.curPlayer);
			this.activeArrows.Add(component);
			num++;
		}
		if (this.curPlayer != null && GameManager.UIController != null)
		{
			Vector3 vector = Vector3.zero;
			if (this.activeArrows != null && this.activeArrows.Count > 0)
			{
				foreach (DirectionChoiceArrow directionChoiceArrow in this.activeArrows)
				{
					vector += directionChoiceArrow.transform.position;
				}
				vector /= (float)this.activeArrows.Count;
			}
			string translation = LocalizationManager.GetTranslation("Step", true, 0, true, false, null, null, true);
			string translation2 = LocalizationManager.GetTranslation("Steps", true, 0, true, false, null, null, true);
			string str = " " + ((this.curPlayer.MoveStepsRemaining > 1) ? translation2 : translation);
			GameManager.UIController.SpawnWorldText(this.curPlayer.MoveStepsRemaining.ToString() + str, vector, 7f, WorldTextType.StepsRemaining, 0f, null);
		}
		if (this.curPlayer != null && this.curPlayer.GamePlayer.IsLocalPlayer && !this.curPlayer.GamePlayer.IsAI && this.curPlayer.GamePlayer.RewiredPlayer.controllers.GetLastActiveController().type == ControllerType.Joystick)
		{
			this.SelectClosestArrow();
		}
	}

	// Token: 0x06001ADA RID: 6874 RVA: 0x000B6854 File Offset: 0x000B4A54
	public int ClosestGoalIndex(BoardPlayer p = null)
	{
		if (p == null)
		{
			p = this.curPlayer;
		}
		int result = 0;
		int num = int.MaxValue;
		for (int i = 0; i < this.goalNodes.Length; i++)
		{
			if (!(this.goalNodes[i] == null))
			{
				int num2 = this.CurrentMap.DistToNode(p.CurrentNode, this.goalNodes[i], BoardNodeConnectionDirection.Forward);
				if (num2 < num)
				{
					result = i;
					num = num2;
				}
			}
		}
		return result;
	}

	// Token: 0x06001ADB RID: 6875 RVA: 0x000B68C4 File Offset: 0x000B4AC4
	private void SelectClosestArrow()
	{
		int num = -1;
		int num2 = int.MaxValue;
		for (int i = 0; i < this.activeArrows.Count; i++)
		{
			int num3 = this.ClosestGoalIndex(null);
			int num4 = this.CurrentMap.DistToNode(this.curPlayer.NodeChoices[this.activeArrows[i].Direction], this.goalNodes[num3], BoardNodeConnectionDirection.Forward);
			if (num4 < num2)
			{
				num = i;
				num2 = num4;
			}
		}
		if (this.CurPlayer.GamePlayer.IsAI && GameManager.rand.NextDouble() < (double)this.chooseWrongDirectionChance[(int)this.CurPlayer.GamePlayer.Difficulty])
		{
			if (num == 1)
			{
				num = 0;
			}
			else
			{
				num = 0;
			}
		}
		this.activeArrows[num].Selected = true;
	}

	// Token: 0x06001ADC RID: 6876 RVA: 0x000B6990 File Offset: 0x000B4B90
	private BoardPlayer GetPlayer(short global_id)
	{
		foreach (BoardPlayer boardPlayer in this.boardPlayerList)
		{
			if (boardPlayer.GamePlayer.GlobalID == global_id)
			{
				return boardPlayer;
			}
		}
		return null;
	}

	// Token: 0x06001ADD RID: 6877 RVA: 0x000B69F4 File Offset: 0x000B4BF4
	public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion transform)
	{
		if (prefab == null)
		{
			Debug.LogError("GameBoardController cannot spawn prefab because it is null!");
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, position, transform);
		if (gameObject)
		{
			try
			{
				this.objectList.Add(gameObject);
			}
			catch (Exception ex)
			{
				Debug.LogError("GameBoardController cannot spawn prefab! " + ex.ToString());
				return null;
			}
			return gameObject;
		}
		return gameObject;
	}

	// Token: 0x06001ADE RID: 6878 RVA: 0x00013C3F File Offset: 0x00011E3F
	public void BoardDestroy(GameObject prefab)
	{
		if (prefab != null)
		{
			UnityEngine.Object.Destroy(prefab);
			if (this.objectList.Contains(prefab))
			{
				this.objectList.Remove(prefab);
			}
		}
	}

	// Token: 0x06001ADF RID: 6879 RVA: 0x00013C6B File Offset: 0x00011E6B
	public void CameraTrackCurrentPlayer()
	{
		this.boardCamera.SetTrackedObject(this.curPlayer.gameObject.transform, this.playerCamOffset);
	}

	// Token: 0x06001AE0 RID: 6880 RVA: 0x000B6A64 File Offset: 0x000B4C64
	private void Load()
	{
		this.curTurnNum = GameManager.SaveToLoad.curTurnNum;
		if (GameManager.SaveToLoad.eventActive && !this.mainBoardEvent.loadLate)
		{
			this.mainBoardEvent.SetupFromLoad(GameManager.SaveToLoad.eventValue1, GameManager.SaveToLoad.eventValue2);
		}
		short num = 0;
		while ((int)num < this.boardPlayerList.Count)
		{
			this.GetPlayer(num).Load(GameManager.SaveToLoad.players[(int)num]);
			num += 1;
		}
		this.uiController.UpdateScores();
		this.SortTurnOrderList(0);
		GameManager.KeyController.CurKeyID = GameManager.SaveToLoad.curKeyID;
		GameManager.KeyController.Load(GameManager.SaveToLoad.keys);
		RecruitEventManager.eventManager.Load(GameManager.SaveToLoad.reaperNodes);
		using (MemoryStream memoryStream = new MemoryStream(GameManager.SaveToLoad.persistentItems))
		{
			using (BinaryReader binaryReader = new BinaryReader(memoryStream))
			{
				int num2 = (int)binaryReader.ReadByte();
				for (int i = 0; i < num2; i++)
				{
					int num3 = (int)binaryReader.ReadInt16();
					PersistentItem component = UnityEngine.Object.Instantiate<GameObject>(this.persistentItemPrefabs[num3]).GetComponent<PersistentItem>();
					component.Load(binaryReader);
					this.persistentItems.Add(component);
				}
			}
		}
		StatTracker.ApplyStats(GameManager.SaveToLoad.stats);
		if (GameManager.SaveToLoad.eventActive && this.mainBoardEvent.loadLate)
		{
			this.mainBoardEvent.SetupFromLoad(GameManager.SaveToLoad.eventValue1, GameManager.SaveToLoad.eventValue2);
		}
		this.boardCamera.MoveToInstant(this.turnOrderList[0].transform, this.playerCamOffset);
	}

	// Token: 0x06001AE1 RID: 6881 RVA: 0x000B6C34 File Offset: 0x000B4E34
	private void Save()
	{
		if (GameManager.CurGameSave == null)
		{
			GameSave gameSave = new GameSave();
			GameManager.CurGameSave = gameSave;
			gameSave.ownersName = GameManager.GetPlayerAt(0).Name;
			gameSave.time = DateTime.Now;
			for (int i = 0; i < GameManager.lobbyOptions.Length; i++)
			{
				gameSave.lobbyOptions[i] = GameManager.lobbyOptions[i];
			}
			if (GameManager.Saves.Count >= GameManager.MaxGameSaves)
			{
				GameManager.Saves.RemoveAt(0);
			}
			GameManager.Saves.Add(gameSave);
		}
		TurnSave turnSave = new TurnSave();
		turnSave.goalNodeIDs = new short[this.goalNodes.Length];
		for (int j = 0; j < this.goalNodes.Length; j++)
		{
			if (this.goalNodes[j] == null)
			{
				turnSave.goalNodeIDs[j] = -1;
			}
			else
			{
				turnSave.goalNodeIDs[j] = (short)this.goalNodes[j].NodeID;
			}
		}
		turnSave.weaponNodeIDs = new short[this.weaponNodes.Length];
		turnSave.weaponIDs = new byte[this.weaponNodes.Length];
		for (int k = 0; k < this.weaponNodes.Length; k++)
		{
			turnSave.weaponNodeIDs[k] = (short)this.weaponNodes[k].NodeID;
			turnSave.weaponIDs[k] = this.WeaponGoalScript[k].itemID;
		}
		if (this.slasherEnemy != null)
		{
			turnSave.killerPosition = this.slasherEnemy.transform.position;
		}
		turnSave.randomMapSeed = this.randomMapSeed;
		turnSave.randomMapIndex = (byte)this.randomMapIndex;
		turnSave.shellGameSeed = this.shellGameSeed;
		turnSave.curTurnNum = this.curTurnNum;
		turnSave.playerCount = (byte)GameManager.GetPlayerCount();
		turnSave.eventActive = (this.curMainBoardEvent != null);
		if (turnSave.eventActive)
		{
			turnSave.eventValue1 = this.curMainBoardEvent.GetEventValue1();
			turnSave.eventValue2 = this.curMainBoardEvent.GetEventValue2();
		}
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				binaryWriter.Write((byte)this.persistentItems.Count);
				for (int l = 0; l < this.persistentItems.Count; l++)
				{
					binaryWriter.Write(this.persistentItems[l].PersistentItemID());
					this.persistentItems[l].Save(binaryWriter);
				}
				turnSave.persistentItems = memoryStream.ToArray();
			}
		}
		for (int m = 0; m < 8; m++)
		{
			BoardPlayer player = this.GetPlayer((short)m);
			turnSave.players[m] = ((player != null) ? player.Save() : new PlayerSave());
		}
		turnSave.curKeyID = GameManager.KeyController.CurKeyID;
		turnSave.keys = GameManager.KeyController.Save();
		turnSave.reaperNodes = RecruitEventManager.eventManager.Save();
		turnSave.stats = StatTracker.CollectStats();
		if (GameManager.CurGameSave.turnSaves.Count >= GameManager.MaxTurnSaves)
		{
			GameManager.CurGameSave.turnSaves.RemoveAt(0);
		}
		GameManager.CurGameSave.turnSaves.Add(turnSave);
		string savePath = GameManager.GetSavePath();
		try
		{
			if (File.Exists(savePath))
			{
				File.Delete(savePath);
			}
			using (BinaryWriter binaryWriter2 = new BinaryWriter(File.Open(savePath, FileMode.Create)))
			{
				binaryWriter2.Write(GameManager.SaveVersion);
				binaryWriter2.Write((byte)GameManager.Saves.Count);
				for (int n = 0; n < GameManager.Saves.Count; n++)
				{
					GameManager.Saves[n].Serialize(binaryWriter2);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Saving turn failed: " + ex.ToString());
		}
	}

	// Token: 0x06001AE2 RID: 6882 RVA: 0x000B7030 File Offset: 0x000B5230
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void RPCReady(NetPlayer sender)
	{
		string translation = LocalizationManager.GetTranslation("Loading Players", true, 0, true, false, null, null, true);
		GameManager.LoadScreen.SetStatus(string.Concat(new string[]
		{
			translation,
			" ",
			this.playersReady.ToString(),
			"/",
			NetSystem.PlayerCount.ToString()
		}));
		this.playersReady++;
		if (this.playersReady >= NetSystem.PlayerCount - 1)
		{
			this.StartGame();
		}
	}

	// Token: 0x06001AE3 RID: 6883 RVA: 0x00013C8E File Offset: 0x00011E8E
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void RPCPreLoaded(NetPlayer sender)
	{
		this.FinishedPreloading();
	}

	// Token: 0x06001AE4 RID: 6884 RVA: 0x00013C96 File Offset: 0x00011E96
	private void FinishedPreloading()
	{
		this.playersPreLoaded++;
		if (this.playersPreLoaded >= NetSystem.PlayerCount)
		{
			this.DoStart();
		}
	}

	// Token: 0x06001AE5 RID: 6885 RVA: 0x00013CB9 File Offset: 0x00011EB9
	private void DoStart()
	{
		if (GameManager.partyGameMode == PartyGameMode.BoardGame)
		{
			this.SwitchState((GameManager.SaveToLoad != null) ? GameBoardState.LoadingSave : GameBoardState.DetermineTurnOrder);
			return;
		}
		this.SwitchState(GameBoardState.MinigamesOnlyPlay);
		this.SetMinigameOnlyNextMinigame();
	}

	// Token: 0x06001AE6 RID: 6886 RVA: 0x000B70BC File Offset: 0x000B52BC
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.PROXY)]
	public void RPCActionChangeGameState(NetPlayer sender, byte new_state)
	{
		string str = "Change Game State RPC: ";
		GameBoardState gameBoardState = (GameBoardState)new_state;
		Debug.Log(str + gameBoardState.ToString());
		ActionChangeGameState item = new ActionChangeGameState((GameBoardState)new_state);
		this.actionQueue.Enqueue(item);
	}

	// Token: 0x06001AE7 RID: 6887 RVA: 0x00013CE3 File Offset: 0x00011EE3
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void RPCBoardAction(NetPlayer sender, byte _ActionType, byte[] _ActionData)
	{
		this.BoardAction(_ActionType, _ActionData);
		base.SendRPC("RPCBoardActionServer", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			(byte)sender.Slot,
			_ActionType,
			_ActionData
		});
	}

	// Token: 0x06001AE8 RID: 6888 RVA: 0x00013D1B File Offset: 0x00011F1B
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCBoardActionServer(NetPlayer sender, byte slot, byte _ActionType, byte[] _ActionData)
	{
		if ((int)slot != NetSystem.MyPlayer.Slot)
		{
			this.BoardAction(_ActionType, _ActionData);
		}
	}

	// Token: 0x06001AE9 RID: 6889 RVA: 0x000B70FC File Offset: 0x000B52FC
	private void BoardAction(byte _ActionType, byte[] _ActionData)
	{
		BoardAction boardAction = null;
		BoardActionType boardActionType = (BoardActionType)_ActionType;
		if (boardActionType != BoardActionType.Simple)
		{
			Debug.Log("Queuing action from RPC: " + boardActionType.ToString());
		}
		switch (boardActionType)
		{
		case BoardActionType.Simple:
			boardAction = new ActionSimple(SimpleBoardAction.EndViewMap);
			break;
		case BoardActionType.Wait:
			boardAction = new ActionWait(0f);
			break;
		case BoardActionType.ChangeGameState:
			boardAction = new ActionChangeGameState(GameBoardState.DetermineTurnOrder);
			break;
		case BoardActionType.SpawnGoal:
			boardAction = new ActionSpawnGoal(0, 0);
			break;
		case BoardActionType.MovePlayer:
			boardAction = new ActionMovePlayer(0, 0, 0, false);
			break;
		case BoardActionType.ChooseDirection:
			boardAction = new ActionChooseDirection(0, 0);
			break;
		case BoardActionType.HitDice:
			boardAction = new ActionHitDice(0, 0);
			break;
		case BoardActionType.StartTurn:
			boardAction = new ActionStartTurn(0, 0);
			break;
		case BoardActionType.LoadMinigame:
			boardAction = new ActionLoadMinigame("", 0);
			break;
		case BoardActionType.DoNodeEvent:
			boardAction = new ActionDoNodeEvent(0, 0, 0);
			break;
		case BoardActionType.KillPlayer:
			boardAction = new ActionKillPlayer(0, 0, Vector3.zero, 0f);
			break;
		case BoardActionType.ShowMinigameResults:
			boardAction = new ActionShowMinigameResults();
			break;
		case BoardActionType.EquipItem:
			boardAction = new ActionEquipItem(0, 0);
			break;
		case BoardActionType.UseItem:
			boardAction = new ActionUseItem(0, 0, null, 0);
			break;
		case BoardActionType.UnEquipItem:
			boardAction = new ActionUnEquipItem(0);
			break;
		case BoardActionType.InteractionChoice:
			boardAction = new ActionInteractionChoice(0, 0);
			break;
		case BoardActionType.ShowWinner:
			boardAction = new ActionShowWinner(false);
			break;
		case BoardActionType.SetupMinigameOnlyLobby:
			boardAction = new ActionSetupMinigameOnlyLobby(0);
			break;
		case BoardActionType.KillerMove:
			boardAction = new ActionKillerMove(Vector3.zero);
			break;
		case BoardActionType.KillerAttack:
			boardAction = new ActionKillerAttack(0);
			break;
		case BoardActionType.StartPummelAwards:
			boardAction = new ActionStartPummelAwards();
			break;
		case BoardActionType.EndPummelAwards:
			boardAction = new ActionEndPummelAwards();
			break;
		case BoardActionType.SendStatistics:
			boardAction = new ActionSendStatistics(false);
			break;
		case BoardActionType.SpawnWeapon:
			boardAction = new ActionSpawnWeapon(0, 0, 0);
			break;
		case BoardActionType.PersistentItemEvent:
			boardAction = new ActionPersistentItemEvent(0, PersistentItemEventType.FirstTurn, null);
			break;
		case BoardActionType.ShellGame:
			boardAction = new ActionShellGame(0);
			break;
		default:
			Debug.LogError("Action not implemented in RPCBoardAction() : " + boardActionType.ToString());
			break;
		}
		this.actionStream.Reset();
		this.actionStream.SetData(_ActionData, _ActionData.Length * 8);
		boardAction.SerializeAction(this.actionStream, false);
		if (boardActionType == BoardActionType.Simple)
		{
			Debug.Log("Queuing Simple Action from RPC: " + ((ActionSimple)boardAction).SimpleAction.ToString() + " Current Action: " + ((this.currentAction == null) ? "NULL" : ((this.currentAction.ActionType == BoardActionType.Simple) ? ((ActionSimple)this.currentAction).SimpleAction.ToString() : this.currentAction.ActionType.ToString())));
		}
		this.actionQueue.Enqueue(boardAction);
	}

	// Token: 0x06001AEA RID: 6890 RVA: 0x000B73BC File Offset: 0x000B55BC
	public bool IsGameFinished()
	{
		if (GameManager.WinningRelics != 9999)
		{
			using (List<GamePlayer>.Enumerator enumerator = GameManager.PlayerList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.BoardObject.GoalScore >= GameManager.WinningRelics)
					{
						return true;
					}
				}
			}
		}
		return GameManager.TurnCount != 9999 && this.CurnTurnNum >= GameManager.TurnCount;
	}

	// Token: 0x06001AEB RID: 6891 RVA: 0x00013D33 File Offset: 0x00011F33
	public void EndGame(bool clientHost, bool hostDisconnect)
	{
		base.SendRPC("RPCEndGame", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			clientHost,
			hostDisconnect
		});
	}

	// Token: 0x06001AEC RID: 6892 RVA: 0x00013D59 File Offset: 0x00011F59
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCEndGame(NetPlayer sender, bool clientHost, bool hostDisconnect)
	{
		DebuggingController.cur.StartEndGame(clientHost, hostDisconnect);
	}

	// Token: 0x06001AED RID: 6893 RVA: 0x00013D67 File Offset: 0x00011F67
	[NetRPC(true, NetRPCSecurity.ALL, NetRPCSecurity.ALL)]
	public void RPCMovePlayer(NetPlayer sender, short playerGlobalID, int nodeID)
	{
		if (GameManager.DEBUGGING)
		{
			this.MovePlayer(playerGlobalID, nodeID, false);
		}
	}

	// Token: 0x06001AEE RID: 6894 RVA: 0x000B7448 File Offset: 0x000B5648
	public void MovePlayer(short playerGlobalID, int nodeID, bool relay)
	{
		if (!GameManager.DEBUGGING)
		{
			return;
		}
		if (relay)
		{
			base.SendRPC("RPCMovePlayer", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				playerGlobalID,
				nodeID
			});
		}
		GameManager.GetPlayerWithID(playerGlobalID).BoardObject.SetNode(nodeID);
	}

	// Token: 0x04001C6F RID: 7279
	public GameBoardCamera boardCamera;

	// Token: 0x04001C70 RID: 7280
	public AudioClip minigameLoadMusic;

	// Token: 0x04001C71 RID: 7281
	public AudioClip victoryMusic;

	// Token: 0x04001C72 RID: 7282
	public AudioClip gameFinishMusic;

	// Token: 0x04001C73 RID: 7283
	public AudioClip gameFinishFanFare;

	// Token: 0x04001C74 RID: 7284
	public AudioClip itemsDisabled;

	// Token: 0x04001C75 RID: 7285
	public List<GameObject> trophies;

	// Token: 0x04001C76 RID: 7286
	public short goalCost = 40;

	// Token: 0x04001C77 RID: 7287
	public GameObject minigameIntroScenePrefab;

	// Token: 0x04001C78 RID: 7288
	public MainBoardEvent curMainBoardEvent;

	// Token: 0x04001C79 RID: 7289
	public MainBoardEvent mainBoardEvent;

	// Token: 0x04001C7A RID: 7290
	public Sprite crownIcon;

	// Token: 0x04001C7B RID: 7291
	public Sprite shellCrownIcon;

	// Token: 0x04001C7C RID: 7292
	public Sprite weaponIcon;

	// Token: 0x04001C7D RID: 7293
	public GameObject[] persistentItemPrefabs;

	// Token: 0x04001C7E RID: 7294
	public UnityEvent OnLoadMinigameFadeout = new UnityEvent();

	// Token: 0x04001C7F RID: 7295
	private GameObject goalObjPrefab;

	// Token: 0x04001C80 RID: 7296
	private GameObject weaponObjPrefab;

	// Token: 0x04001C81 RID: 7297
	private GameObject directionChoicePrefab;

	// Token: 0x04001C82 RID: 7298
	private List<BoardActor> boardActors = new List<BoardActor>();

	// Token: 0x04001C83 RID: 7299
	private List<BoardPlayer> boardPlayerList = new List<BoardPlayer>();

	// Token: 0x04001C84 RID: 7300
	private List<BoardPlayer> turnOrderList = new List<BoardPlayer>();

	// Token: 0x04001C85 RID: 7301
	private List<BoardPlayer> myPlayers = new List<BoardPlayer>();

	// Token: 0x04001C86 RID: 7302
	private int playersReady;

	// Token: 0x04001C87 RID: 7303
	private int playersPreLoaded;

	// Token: 0x04001C88 RID: 7304
	private int curPlayerIndex;

	// Token: 0x04001C89 RID: 7305
	private BoardPlayer curPlayer;

	// Token: 0x04001C8A RID: 7306
	private bool startedPlayersTurn;

	// Token: 0x04001C8B RID: 7307
	private List<DirectionChoiceArrow> activeArrows = new List<DirectionChoiceArrow>();

	// Token: 0x04001C8C RID: 7308
	private GameBoardState curBoardState;

	// Token: 0x04001C8D RID: 7309
	private Queue<BoardAction> actionQueue = new Queue<BoardAction>();

	// Token: 0x04001C8E RID: 7310
	private BoardAction currentAction;

	// Token: 0x04001C8F RID: 7311
	private float actionWaitTime;

	// Token: 0x04001C90 RID: 7312
	private int actionWaitFrame;

	// Token: 0x04001C91 RID: 7313
	private GameMap currentMap;

	// Token: 0x04001C92 RID: 7314
	private BoardNode[] boardNodes;

	// Token: 0x04001C93 RID: 7315
	private BoardNode startNode;

	// Token: 0x04001C97 RID: 7319
	private GameUIController uiController;

	// Token: 0x04001C98 RID: 7320
	private GameObject minigameIntroScene;

	// Token: 0x04001C99 RID: 7321
	private GameObject resultScreenScene;

	// Token: 0x04001C9A RID: 7322
	private bool minigameStarted;

	// Token: 0x04001C9B RID: 7323
	private MinigameChoiceWindow minigame_choice_wnd;

	// Token: 0x04001C9C RID: 7324
	private bool firstGoal = true;

	// Token: 0x04001C9D RID: 7325
	private bool firstWeapon = true;

	// Token: 0x04001C9E RID: 7326
	private BoardNode[] goalNodes;

	// Token: 0x04001C9F RID: 7327
	private BoardNode[] weaponNodes;

	// Token: 0x04001CA1 RID: 7329
	private System.Random rand;

	// Token: 0x04001CA2 RID: 7330
	private Vector3 playerCamOffset = new Vector3(0f, 0.4325f, 0f);

	// Token: 0x04001CA3 RID: 7331
	private ZPBitStream actionStream = new ZPBitStream();

	// Token: 0x04001CA4 RID: 7332
	private bool firstTurn = true;

	// Token: 0x04001CA5 RID: 7333
	private int curTurnNum;

	// Token: 0x04001CA6 RID: 7334
	public EndScreenManager endScreenManager;

	// Token: 0x04001CA7 RID: 7335
	public AwardSceneManager awardSceneManager;

	// Token: 0x04001CA8 RID: 7336
	private ActionTimer AIMapTimer = new ActionTimer(0.25f, 4f);

	// Token: 0x04001CA9 RID: 7337
	private Vector2 AIMapDir = Vector2.zero;

	// Token: 0x04001CAA RID: 7338
	private MinigameDefinition nextMinigame;

	// Token: 0x04001CAB RID: 7339
	private int AIItemToUse = -1;

	// Token: 0x04001CAC RID: 7340
	private float lastKillMsgTime;

	// Token: 0x04001CAD RID: 7341
	private List<PersistentItem> persistentItems = new List<PersistentItem>();

	// Token: 0x04001CAE RID: 7342
	private bool m_shownModifiers;

	// Token: 0x04001CAF RID: 7343
	private List<BoardModifier> m_modifiers = new List<BoardModifier>();

	// Token: 0x04001CB0 RID: 7344
	private List<GameModifierDefinition> m_modifierDefinitions = new List<GameModifierDefinition>();

	// Token: 0x04001CB1 RID: 7345
	private System.Random randomMapRand;

	// Token: 0x04001CB2 RID: 7346
	private bool gotRandMapSeed;

	// Token: 0x04001CB3 RID: 7347
	private int randomMapIndex;

	// Token: 0x04001CB4 RID: 7348
	private int randomMapSeed;

	// Token: 0x04001CB5 RID: 7349
	private bool modifiersSetup;

	// Token: 0x04001CB6 RID: 7350
	public GameObject[] preLoadObjects;

	// Token: 0x04001CB7 RID: 7351
	private List<GameObject> objects = new List<GameObject>();

	// Token: 0x04001CB8 RID: 7352
	private Vector3 spawnPos = Vector3.zero;

	// Token: 0x04001CB9 RID: 7353
	public bool isHalloweenMap = true;

	// Token: 0x04001CBA RID: 7354
	public bool isWinterMap = true;

	// Token: 0x04001CBB RID: 7355
	public bool areFakeChestsActive = true;

	// Token: 0x04001CBC RID: 7356
	public SlasherEnemy slasherEnemy;

	// Token: 0x04001CBD RID: 7357
	private int tempindex;

	// Token: 0x04001CBE RID: 7358
	private List<int> dists = new List<int>();

	// Token: 0x04001CBF RID: 7359
	private List<BoardNode> potentialGoalBoardNodes;

	// Token: 0x04001CC0 RID: 7360
	private GameBoardController.PotentialNode[] goalBoardNodesTemp;

	// Token: 0x04001CC1 RID: 7361
	private string[] num_keys = new string[]
	{
		"0",
		"1",
		"2",
		"3",
		"4",
		"5",
		"6",
		"7",
		"8",
		"9"
	};

	// Token: 0x04001CC2 RID: 7362
	private int tempActionNum;

	// Token: 0x04001CC3 RID: 7363
	private float[] useItemChances = new float[]
	{
		0.5f,
		0.75f,
		1.1f
	};

	// Token: 0x04001CC4 RID: 7364
	private BinaryTree[] RollBinaryTrees = new BinaryTree[3];

	// Token: 0x04001CC5 RID: 7365
	private float[] BinaryTreeTotals = new float[3];

	// Token: 0x04001CC6 RID: 7366
	private float[][] AIRollChances = new float[][]
	{
		new float[]
		{
			2f,
			1.5f,
			1.25f,
			1f,
			1f,
			0.85f,
			0.75f,
			0.5f
		},
		new float[]
		{
			1.5f,
			1.25f,
			1.1f,
			1f,
			1f,
			0.9f,
			0.8f,
			0.7f
		},
		new float[]
		{
			1f,
			1f,
			1f,
			1f,
			1f,
			1f,
			1f,
			1f
		}
	};

	// Token: 0x04001CC7 RID: 7367
	private BinaryTree[] DynamicRollBinaryTrees = new BinaryTree[3];

	// Token: 0x04001CC8 RID: 7368
	private float[] DynamicBinaryTreeTotals = new float[3];

	// Token: 0x04001CC9 RID: 7369
	private float[] AIRollRangeMin = new float[]
	{
		2f,
		1.5f,
		1f
	};

	// Token: 0x04001CCA RID: 7370
	private float[] AIRollRangeMax = new float[]
	{
		0.5f,
		0.7f,
		1f
	};

	// Token: 0x04001CCB RID: 7371
	private bool m_triedDynamicRollChances;

	// Token: 0x04001CCC RID: 7372
	private bool m_createdDynamicRollChances;

	// Token: 0x04001CCD RID: 7373
	private StatChallengeBoardEvent ev;

	// Token: 0x04001CCE RID: 7374
	private List<GobletChallenge> challenges = new List<GobletChallenge>();

	// Token: 0x04001CCF RID: 7375
	private bool m_midGameAwardsComplete;

	// Token: 0x04001CD0 RID: 7376
	private Dictionary<byte, int> m_rolls = new Dictionary<byte, int>();

	// Token: 0x04001CD1 RID: 7377
	private ActionTimer waitForKeysMax = new ActionTimer(10f);

	// Token: 0x04001CD2 RID: 7378
	private bool keysFinished;

	// Token: 0x04001CD3 RID: 7379
	private ActionTimer waitForKeysMaxAwards = new ActionTimer(10f);

	// Token: 0x04001CD4 RID: 7380
	private bool keysFinishedAwards;

	// Token: 0x04001CD5 RID: 7381
	private ShellGameTest m_shellGameController;

	// Token: 0x04001CD6 RID: 7382
	private int shellGameSeed;

	// Token: 0x04001CD7 RID: 7383
	private int m_realChestIndex;

	// Token: 0x04001CD8 RID: 7384
	private bool[] m_goalSpawnQueued = new bool[3];

	// Token: 0x04001CD9 RID: 7385
	private float[] chooseWrongDirectionChance = new float[]
	{
		0.4f,
		0.2f,
		-1f
	};

	// Token: 0x04001CDA RID: 7386
	private List<GameObject> objectList = new List<GameObject>();

	// Token: 0x020003DA RID: 986
	private struct PotentialNode
	{
		// Token: 0x04001CDB RID: 7387
		public BoardNode node;

		// Token: 0x04001CDC RID: 7388
		public int dist;
	}
}
