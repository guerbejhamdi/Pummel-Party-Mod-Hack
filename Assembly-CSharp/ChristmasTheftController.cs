using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000193 RID: 403
public class ChristmasTheftController : MinigameController
{
	// Token: 0x06000B8D RID: 2957 RVA: 0x000627E8 File Offset: 0x000609E8
	public ChristmasTheftPresent GetClosestPresent(ChristmasTheftPlayer player, Vector3 pos)
	{
		ChristmasTheftPresent result = null;
		float num = float.PositiveInfinity;
		foreach (KeyValuePair<byte, ChristmasTheftPresent> keyValuePair in this.m_presents)
		{
			ChristmasTheftPresent value = keyValuePair.Value;
			if (value.State != ChristmasTheftPresentState.Held && value.Player != player)
			{
				float num2 = Vector3.Distance(pos, value.transform.position);
				if (num2 < num)
				{
					num = num2;
					result = value;
				}
			}
		}
		return result;
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x00062880 File Offset: 0x00060A80
	private void OnDestroy()
	{
		foreach (KeyValuePair<byte, ChristmasTheftPresent> keyValuePair in this.m_presents)
		{
			ChristmasTheftPresent value = keyValuePair.Value;
			if (value != null)
			{
				UnityEngine.Object.Destroy(value.gameObject);
			}
		}
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x000628E8 File Offset: 0x00060AE8
	public List<ChristmasTheftPresent> GetPresentsByDistance(ChristmasTheftPlayer player, Vector3 pos)
	{
		List<ChristmasTheftPresent> list = new List<ChristmasTheftPresent>();
		foreach (KeyValuePair<byte, ChristmasTheftPresent> keyValuePair in this.m_presents)
		{
			ChristmasTheftPresent value = keyValuePair.Value;
			if (value.State != ChristmasTheftPresentState.Held && value.Player != player)
			{
				float distance = Vector3.Distance(pos, value.transform.position);
				value.Distance = distance;
				int num = 0;
				if (num <= list.Count)
				{
					if (num == list.Count)
					{
						list.Add(value);
					}
					else if (value.Distance < list[num].Distance)
					{
						list.Insert(num, value);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06000B90 RID: 2960 RVA: 0x000629C0 File Offset: 0x00060BC0
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

	// Token: 0x06000B91 RID: 2961 RVA: 0x00062B78 File Offset: 0x00060D78
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.rand = new System.Random(UnityEngine.Random.Range(0, int.MaxValue));
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("ChristmasTheftPlayer", null);
		}
		if (NetSystem.IsServer)
		{
			byte b = 1;
			foreach (ChristmasTheftPresentSpawn christmasTheftPresentSpawn in UnityEngine.Object.FindObjectsOfType<ChristmasTheftPresentSpawn>())
			{
				this.SpawnPresent(christmasTheftPresentSpawn.transform.position, b);
				b += 1;
			}
		}
		this.triangulation = NavMesh.CalculateTriangulation();
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x0000ACF2 File Offset: 0x00008EF2
	public override void StartMinigame()
	{
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.StartMinigame();
	}

	// Token: 0x06000B93 RID: 2963 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x06000B94 RID: 2964 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000B95 RID: 2965 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000B96 RID: 2966 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000B97 RID: 2967 RVA: 0x0000A1F9 File Offset: 0x000083F9
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer && this.ui_timer.time_test <= 0f)
		{
			base.EndRound(1f, 1f, false);
		}
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x06000B9A RID: 2970 RVA: 0x00062BF8 File Offset: 0x00060DF8
	public void RequestDropPresents(ChristmasTheftPlayer player)
	{
		if (NetSystem.IsServer)
		{
			if (player.PresentsHeld <= 0)
			{
				return;
			}
			using (Dictionary<byte, ChristmasTheftPresent>.Enumerator enumerator = this.m_presents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<byte, ChristmasTheftPresent> keyValuePair = enumerator.Current;
					if (keyValuePair.Value.State == ChristmasTheftPresentState.Held && keyValuePair.Value.Player == player)
					{
						int num = 0;
						Vector3 zero = Vector3.zero;
						if (player.GetFreeSlotInfo(out num, out zero))
						{
							this.SetPresentState(keyValuePair.Value.ID, ChristmasTheftPresentState.Owned, zero, player);
							player.SetSlotStatus(num, true);
							keyValuePair.Value.Slot = num;
							byte presentsHeld = player.PresentsHeld;
							player.PresentsHeld = presentsHeld - 1;
							short score = player.Score;
							player.Score = score + 1;
						}
					}
				}
				return;
			}
		}
		base.SendRPC("RPCRequestDropPresents", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			player.GamePlayer.GlobalID
		});
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x00062D0C File Offset: 0x00060F0C
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void RPCRequestDropPresents(NetPlayer sender, short playerGlobalID)
	{
		ChristmasTheftPlayer christmasTheftPlayer = null;
		foreach (CharacterBase characterBase in this.players)
		{
			if (characterBase.GamePlayer.GlobalID == playerGlobalID)
			{
				christmasTheftPlayer = (ChristmasTheftPlayer)characterBase;
				break;
			}
		}
		if (christmasTheftPlayer != null)
		{
			this.RequestDropPresents(christmasTheftPlayer);
			return;
		}
		Debug.LogError("error getting theft player with global id = " + playerGlobalID.ToString());
	}

	// Token: 0x06000B9C RID: 2972 RVA: 0x00062D98 File Offset: 0x00060F98
	public void RequestGrabPresent(byte presentID, ChristmasTheftPlayer player)
	{
		if (NetSystem.IsServer)
		{
			ChristmasTheftPresent christmasTheftPresent = null;
			if (this.m_presents.TryGetValue(presentID, out christmasTheftPresent) && (christmasTheftPresent.State == ChristmasTheftPresentState.Free || (christmasTheftPresent.State == ChristmasTheftPresentState.Owned && christmasTheftPresent.Player != player)) && player.PresentsHeld == 0)
			{
				if (christmasTheftPresent.State == ChristmasTheftPresentState.Owned && christmasTheftPresent.Slot != -1 && christmasTheftPresent.Player != null)
				{
					christmasTheftPresent.Player.SetSlotStatus(christmasTheftPresent.Slot, false);
					christmasTheftPresent.Slot = -1;
					ChristmasTheftPlayer player2 = christmasTheftPresent.Player;
					short score = player2.Score;
					player2.Score = score - 1;
				}
				this.SetPresentState(presentID, ChristmasTheftPresentState.Held, Vector3.zero, player);
				byte presentsHeld = player.PresentsHeld;
				player.PresentsHeld = presentsHeld + 1;
				return;
			}
		}
		else
		{
			base.SendRPC("RPCRequestGrabPresent", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				presentID,
				player.GamePlayer.GlobalID
			});
		}
	}

	// Token: 0x06000B9D RID: 2973 RVA: 0x00062E94 File Offset: 0x00061094
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void RPCRequestGrabPresent(NetPlayer sender, byte presentID, short playerGlobalID)
	{
		ChristmasTheftPlayer christmasTheftPlayer = null;
		foreach (CharacterBase characterBase in this.players)
		{
			if (characterBase.GamePlayer.GlobalID == playerGlobalID)
			{
				christmasTheftPlayer = (ChristmasTheftPlayer)characterBase;
				break;
			}
		}
		if (christmasTheftPlayer != null)
		{
			this.RequestGrabPresent(presentID, christmasTheftPlayer);
			return;
		}
		Debug.LogError("error getting theft player with global id = " + playerGlobalID.ToString());
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x00062F24 File Offset: 0x00061124
	public void SetPresentState(byte presentID, ChristmasTheftPresentState newState, Vector3 position, ChristmasTheftPlayer player)
	{
		if (NetSystem.IsServer)
		{
			if (player != null)
			{
				base.SendRPC("RPCSetPresentState", NetRPCDelivery.RELIABLE_ORDERED, new object[]
				{
					presentID,
					(byte)newState,
					position,
					player.GamePlayer.GlobalID
				});
			}
			else
			{
				base.SendRPC("RPCSetPresentState", NetRPCDelivery.RELIABLE_ORDERED, new object[]
				{
					presentID,
					(byte)newState,
					position,
					-1
				});
			}
		}
		ChristmasTheftPresent christmasTheftPresent = null;
		if (this.m_presents.TryGetValue(presentID, out christmasTheftPresent))
		{
			if (newState == ChristmasTheftPresentState.Held)
			{
				if (christmasTheftPresent.State == ChristmasTheftPresentState.Free)
				{
					AudioSystem.PlayOneShot(this.m_grabPresentClip, christmasTheftPresent.transform.position, 1f, AudioRolloffMode.Linear, 20f, 60f, 0f);
				}
				else if (christmasTheftPresent.State == ChristmasTheftPresentState.Owned)
				{
					AudioSystem.PlayOneShot(this.m_swipePresentClip, christmasTheftPresent.transform.position, 1f, AudioRolloffMode.Linear, 20f, 60f, 0f);
				}
			}
			else if (christmasTheftPresent.State == ChristmasTheftPresentState.Held && newState == ChristmasTheftPresentState.Owned)
			{
				AudioSystem.PlayOneShot(this.m_gainPresentClip, position, 1f, AudioRolloffMode.Linear, 20f, 60f, 0f);
			}
			christmasTheftPresent.SetState(newState, position, player);
		}
	}

	// Token: 0x06000B9F RID: 2975 RVA: 0x0006307C File Offset: 0x0006127C
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSetPresentState(NetPlayer sender, byte presentID, byte state, Vector3 position, short playerID)
	{
		ChristmasTheftPlayer christmasTheftPlayer = null;
		if (playerID != -1)
		{
			foreach (CharacterBase characterBase in this.players)
			{
				if (characterBase.GamePlayer.GlobalID == playerID)
				{
					christmasTheftPlayer = (ChristmasTheftPlayer)characterBase;
					break;
				}
			}
		}
		if (christmasTheftPlayer != null)
		{
			this.SetPresentState(presentID, (ChristmasTheftPresentState)state, position, christmasTheftPlayer);
		}
	}

	// Token: 0x06000BA0 RID: 2976 RVA: 0x000630FC File Offset: 0x000612FC
	public void SpawnPresent(Vector3 position, byte id)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSpawnPresent", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				position,
				id
			});
		}
		ChristmasTheftPresent component = base.Spawn(this.m_presentPfb, position, Quaternion.identity).GetComponent<ChristmasTheftPresent>();
		if (component != null)
		{
			component.ID = id;
			component.SetState(ChristmasTheftPresentState.Free, position, null);
			this.m_presents.Add(id, component);
		}
	}

	// Token: 0x06000BA1 RID: 2977 RVA: 0x0000B5AA File Offset: 0x000097AA
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSpawnPresent(NetPlayer sender, Vector3 position, byte presentID)
	{
		this.SpawnPresent(position, presentID);
	}

	// Token: 0x04000ABF RID: 2751
	[SerializeField]
	private GameObject m_presentPfb;

	// Token: 0x04000AC0 RID: 2752
	[SerializeField]
	protected AudioClip m_grabPresentClip;

	// Token: 0x04000AC1 RID: 2753
	[SerializeField]
	protected AudioClip m_swipePresentClip;

	// Token: 0x04000AC2 RID: 2754
	[SerializeField]
	protected AudioClip m_gainPresentClip;

	// Token: 0x04000AC3 RID: 2755
	private System.Random rand;

	// Token: 0x04000AC4 RID: 2756
	private Dictionary<byte, ChristmasTheftPresent> m_presents = new Dictionary<byte, ChristmasTheftPresent>();

	// Token: 0x04000AC5 RID: 2757
	private NavMeshTriangulation triangulation;

	// Token: 0x04000AC6 RID: 2758
	private BinaryTree binaryTree;

	// Token: 0x04000AC7 RID: 2759
	private float totalArea;
}
