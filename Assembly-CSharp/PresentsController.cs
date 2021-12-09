using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x02000207 RID: 519
public class PresentsController : MinigameController
{
	// Token: 0x06000F3D RID: 3901 RVA: 0x0000D305 File Offset: 0x0000B505
	public void AddSpawnedObject(GameObject obj)
	{
		this.m_spawnedObjects.Add(obj);
	}

	// Token: 0x06000F3E RID: 3902 RVA: 0x00079354 File Offset: 0x00077554
	public void Awake()
	{
		Material[] conveyorColorMats = this.m_conveyorColorMats;
		for (int i = 0; i < conveyorColorMats.Length; i++)
		{
			conveyorColorMats[i].color = Color.gray;
		}
	}

	// Token: 0x06000F3F RID: 3903 RVA: 0x00079384 File Offset: 0x00077584
	public void ClearSpawnedObjects()
	{
		foreach (GameObject gameObject in this.m_spawnedObjects)
		{
			if (gameObject != null)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
	}

	// Token: 0x06000F40 RID: 3904 RVA: 0x0000D313 File Offset: 0x0000B513
	public void OnDestroy()
	{
		this.ClearSpawnedObjects();
	}

	// Token: 0x06000F41 RID: 3905 RVA: 0x0000D31B File Offset: 0x0000B51B
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("PresentsPlayer", null);
		}
		this.m_spawnPoints = UnityEngine.Object.FindObjectsOfType<ObjectSpawnPoint>();
		this.m_rand = new System.Random(6124489);
	}

	// Token: 0x06000F42 RID: 3906 RVA: 0x0000D351 File Offset: 0x0000B551
	public void SetConveyorColor(int index, Color col)
	{
		this.m_conveyorColorMats[index].color = col;
	}

	// Token: 0x06000F43 RID: 3907 RVA: 0x0000BCDB File Offset: 0x00009EDB
	public override void OnPlayersReady()
	{
		base.OnPlayersReady();
	}

	// Token: 0x06000F44 RID: 3908 RVA: 0x000793E0 File Offset: 0x000775E0
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.BottomLeft, new Vector2(145f, 45f), 68f, false);
		if (NetSystem.IsServer)
		{
			this.StartSpawning();
		}
		base.StartMinigame();
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x00079438 File Offset: 0x00077638
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing)
		{
			if (this.ui_timer.time_test <= 0f && !this.m_presentsDestroyed)
			{
				for (int i = this.m_allPresentGroups.Count - 1; i >= 0; i--)
				{
					if (!(this.m_allPresentGroups[i] == null))
					{
						this.m_allPresentGroups[i].Clear();
					}
				}
				this.m_presentsDestroyed = true;
			}
			if (NetSystem.IsServer)
			{
				if (this.ui_timer.time_test <= 0f)
				{
					base.EndRound(1f, 1f, false);
				}
				else if (this.m_spawnerTime <= this.m_nextSpawnTime)
				{
					float t = 1f - this.ui_timer.time_test / this.round_length;
					float minInclusive = Mathf.Lerp(this.m_startMinSpawnTime, this.m_endMinSpawnTime, t);
					float maxInclusive = Mathf.Lerp(this.m_startMaxSpawnTime, this.m_endMaxSpawnTime, t);
					this.m_nextSpawnTime = this.m_spawnerTime - UnityEngine.Random.Range(minInclusive, maxInclusive);
					if (this.m_spawnerTime < 15f)
					{
						PresentsController.m_speedMultiplier = Mathf.MoveTowards(PresentsController.m_speedMultiplier, 1.3f, Time.deltaTime);
					}
					this.SpawnObjectRow(0);
				}
			}
			if (this.m_isSpawning)
			{
				this.m_spawnerTime -= Time.deltaTime;
				if (this.m_spawnerTime < 15f)
				{
					PresentsController.m_speedMultiplier = Mathf.MoveTowards(PresentsController.m_speedMultiplier, 1.3f, Time.deltaTime);
				}
			}
		}
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x0000D361 File Offset: 0x0000B561
	public float GetTime()
	{
		return this.ui_timer.time_test;
	}

	// Token: 0x06000F47 RID: 3911 RVA: 0x000795B4 File Offset: 0x000777B4
	private void SpawnObjectRow(int seed = 0)
	{
		if (NetSystem.IsServer)
		{
			seed = this.m_rand.Next();
			base.SendRPC("RPCSpawnObjectRow", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				seed
			});
		}
		bool flag = true;
		foreach (ObjectSpawnPoint objectSpawnPoint in this.m_spawnPoints)
		{
			if (objectSpawnPoint.SpawnIndex < GameManager.GetPlayerCount())
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_presentGroupPfb, objectSpawnPoint.transform);
				gameObject.transform.localPosition = Vector3.zero;
				PresentsGroup component = gameObject.GetComponent<PresentsGroup>();
				component.Setup(this, seed);
				if (flag)
				{
					this.m_presentGroups.Add(component);
					flag = false;
				}
				this.m_allPresentGroups.Add(component);
			}
		}
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x0000D36E File Offset: 0x0000B56E
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSpawnObjectRow(NetPlayer sender, int seed)
	{
		this.SpawnObjectRow(seed);
	}

	// Token: 0x06000F49 RID: 3913 RVA: 0x0000D377 File Offset: 0x0000B577
	public void RemoveGroup(PresentsGroup group)
	{
		if (this.m_presentGroups.Contains(group))
		{
			this.m_presentGroups.Remove(group);
		}
	}

	// Token: 0x06000F4A RID: 3914 RVA: 0x0000D394 File Offset: 0x0000B594
	public PresentsGroup GetNextGroup()
	{
		if (this.m_presentGroups.Count > 1)
		{
			return this.m_presentGroups[0];
		}
		return null;
	}

	// Token: 0x06000F4B RID: 3915 RVA: 0x00079668 File Offset: 0x00077868
	private void StartSpawning()
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("StartSpawningRPC", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		this.m_spawnerTime = 60f;
		this.m_isSpawning = true;
		this.SpawnObjectRow(0);
		this.m_nextSpawnTime = this.m_spawnerTime - 1f;
	}

	// Token: 0x06000F4C RID: 3916 RVA: 0x0000D3B2 File Offset: 0x0000B5B2
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void StartSpawningRPC(NetPlayer sender)
	{
		this.StartSpawning();
	}

	// Token: 0x06000F4D RID: 3917 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x06000F4E RID: 3918 RVA: 0x000796B8 File Offset: 0x000778B8
	public override void BuildResults()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			if (this.players[i].IsOwner && !this.players[i].GamePlayer.IsAI && !((PresentsPlayer)this.players[i]).gotCoal)
			{
				PlatformAchievementManager.Instance.TriggerAchievement("ACH_GIFT_GRAB");
			}
		}
		base.BuildResults();
	}

	// Token: 0x06000F4F RID: 3919 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000F50 RID: 3920 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x04000F08 RID: 3848
	[SerializeField]
	protected GameObject m_presentGroupPfb;

	// Token: 0x04000F09 RID: 3849
	[SerializeField]
	protected Material[] m_conveyorColorMats;

	// Token: 0x04000F0A RID: 3850
	[SerializeField]
	protected float m_startMinSpawnTime = 2f;

	// Token: 0x04000F0B RID: 3851
	[SerializeField]
	protected float m_startMaxSpawnTime = 3f;

	// Token: 0x04000F0C RID: 3852
	[SerializeField]
	protected float m_endMinSpawnTime = 0.5f;

	// Token: 0x04000F0D RID: 3853
	[SerializeField]
	protected float m_endMaxSpawnTime = 1.5f;

	// Token: 0x04000F0E RID: 3854
	private ObjectSpawnPoint[] m_spawnPoints;

	// Token: 0x04000F0F RID: 3855
	private int m_seed;

	// Token: 0x04000F10 RID: 3856
	private System.Random m_rand;

	// Token: 0x04000F11 RID: 3857
	private bool m_isSpawning;

	// Token: 0x04000F12 RID: 3858
	private float m_spawnerTime = 60f;

	// Token: 0x04000F13 RID: 3859
	private float m_nextSpawnTime;

	// Token: 0x04000F14 RID: 3860
	public static float m_speedMultiplier = 1f;

	// Token: 0x04000F15 RID: 3861
	private List<PresentsGroup> m_presentGroups = new List<PresentsGroup>();

	// Token: 0x04000F16 RID: 3862
	private List<PresentsGroup> m_allPresentGroups = new List<PresentsGroup>();

	// Token: 0x04000F17 RID: 3863
	private List<GameObject> m_spawnedObjects = new List<GameObject>();

	// Token: 0x04000F18 RID: 3864
	private bool m_presentsDestroyed;
}
