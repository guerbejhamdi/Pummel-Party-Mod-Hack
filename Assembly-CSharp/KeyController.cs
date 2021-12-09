using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x020000C1 RID: 193
public class KeyController : NetBehaviour
{
	// Token: 0x17000088 RID: 136
	// (get) Token: 0x060003E7 RID: 999 RVA: 0x0000629D File Offset: 0x0000449D
	// (set) Token: 0x060003E8 RID: 1000 RVA: 0x000062A5 File Offset: 0x000044A5
	public int CurKeyID
	{
		get
		{
			return this.curKeyID;
		}
		set
		{
			this.curKeyID = value;
		}
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x000062AE File Offset: 0x000044AE
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		GameManager.KeyController = this;
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x000062BC File Offset: 0x000044BC
	private void Start()
	{
		this.pickupDistSqr = this.pickupDist * this.pickupDist;
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x0003B7B0 File Offset: 0x000399B0
	public bool KeysFinished()
	{
		bool result = true;
		for (int i = 0; i < this.activeKeys.Count; i++)
		{
			if (this.activeKeys[i].CurState != BoardKey.BoardKeyState.Idle)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x0003B7F0 File Offset: 0x000399F0
	private void Update()
	{
		if (NetSystem.IsServer && GameManager.Board.BoardState != GameBoardState.Minigame && GameManager.Board.BoardState != GameBoardState.Initializing && GameManager.Board.BoardState != GameBoardState.Loading)
		{
			for (int i = 0; i < GameManager.PlayerList.Count; i++)
			{
				BoardPlayer boardObject = GameManager.PlayerList[i].BoardObject;
				if (boardObject.PlayerState != BoardPlayerState.Dying && boardObject.PlayerState != BoardPlayerState.Dead && boardObject.PlayerState != BoardPlayerState.Ragdolling)
				{
					int j = 0;
					while (j < this.activeKeys.Count)
					{
						if (boardObject != this.activeKeys[j].Owner && this.activeKeys[j].CurState == BoardKey.BoardKeyState.Idle)
						{
							if ((boardObject.transform.position - this.activeKeys[j].transform.position).sqrMagnitude <= this.pickupDistSqr)
							{
								this.PickupKey(boardObject, this.activeKeys[j].ID);
							}
							else
							{
								j++;
							}
						}
						else
						{
							j++;
						}
					}
				}
			}
		}
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x0003B928 File Offset: 0x00039B28
	public void PickupKey(BoardPlayer player, int keyID)
	{
		for (int i = 0; i < this.activeKeys.Count; i++)
		{
			if (this.activeKeys[i].ID == keyID)
			{
				this.activeKeys[i].Pickup(player);
				this.activeKeys.RemoveAt(i);
				break;
			}
		}
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCPickupKey", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				player.GamePlayer.GlobalID,
				keyID
			});
		}
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x000062D1 File Offset: 0x000044D1
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCPickupKey(NetPlayer sender, short playerID, int keyID)
	{
		this.PickupKey(GameManager.GetPlayerAt((int)playerID).BoardObject, keyID);
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0003B9B4 File Offset: 0x00039BB4
	public void SpawnKeys(int count, BoardPlayer owner, BoardPlayer target = null)
	{
		for (int i = 0; i < count; i++)
		{
			this.SpawnKey(owner, target, (short)owner.CurrentNode.NodeID, this.curKeyID, GameManager.rand.Next(), false);
			this.curKeyID++;
		}
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x0003BA00 File Offset: 0x00039C00
	public void SpawnKeys(int count, BoardNode node, BoardPlayer target = null)
	{
		for (int i = 0; i < count; i++)
		{
			this.SpawnKey(null, target, (short)node.NodeID, this.curKeyID, GameManager.rand.Next(), false);
			this.curKeyID++;
		}
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x0003BA48 File Offset: 0x00039C48
	public void SpawnKey(BoardPlayer owner, BoardPlayer target, short nodeID, int keyID, int seed, bool loadFromSave = false)
	{
		BoardKey component = UnityEngine.Object.Instantiate<GameObject>(this.keyPrefab).GetComponent<BoardKey>();
		component.transform.parent = GameManager.BoardRoot.transform;
		component.Setup(owner, keyID, GameManager.Board.BoardNodes[(int)nodeID], seed);
		this.activeKeys.Add(component);
		if (NetSystem.IsServer && !loadFromSave)
		{
			this.curKeyID++;
			short num = (owner == null) ? -1 : owner.GamePlayer.GlobalID;
			base.SendRPC("RPCSpawnKey", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				num,
				nodeID,
				keyID,
				seed
			});
			if (target != null)
			{
				this.PickupKey(target, keyID);
			}
		}
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x0003BB1C File Offset: 0x00039D1C
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSpawnKey(NetPlayer sender, short playerID, short nodeID, int keyID, int seed)
	{
		BoardPlayer owner = null;
		if (playerID != -1)
		{
			owner = GameManager.GetPlayerAt((int)playerID).BoardObject;
		}
		this.SpawnKey(owner, null, nodeID, keyID, seed, false);
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x0003BB4C File Offset: 0x00039D4C
	public KeySave[] Save()
	{
		KeySave[] array = new KeySave[this.activeKeys.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new KeySave();
			array[i].playerID = (byte)this.activeKeys[i].Owner.OwnerSlot;
			array[i].keyID = this.activeKeys[i].ID;
			array[i].nodeID = (short)this.activeKeys[i].Node.NodeID;
			array[i].seed = this.activeKeys[i].Seed;
		}
		return array;
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x0003BBF4 File Offset: 0x00039DF4
	public void Load(KeySave[] keys)
	{
		for (int i = 0; i < keys.Length; i++)
		{
			BoardPlayer boardObject = GameManager.GetPlayerAt((int)keys[i].playerID).BoardObject;
			this.SpawnKey(boardObject, null, keys[i].nodeID, keys[i].keyID, keys[i].seed, true);
		}
	}

	// Token: 0x04000448 RID: 1096
	public GameObject keyPrefab;

	// Token: 0x04000449 RID: 1097
	public float pickupDist = 1f;

	// Token: 0x0400044A RID: 1098
	private int curKeyID;

	// Token: 0x0400044B RID: 1099
	private List<BoardKey> activeKeys = new List<BoardKey>();

	// Token: 0x0400044C RID: 1100
	private float pickupDistSqr;
}
