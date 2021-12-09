using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;

// Token: 0x02000422 RID: 1058
public class MinigameController : NetBehaviour
{
	// Token: 0x1700037C RID: 892
	// (get) Token: 0x06001D33 RID: 7475 RVA: 0x000158B2 File Offset: 0x00013AB2
	[HideInInspector]
	public Camera MinigameCamera
	{
		get
		{
			if (this.minigameCameras.Count <= 0)
			{
				return null;
			}
			return this.minigameCameras[0];
		}
	}

	// Token: 0x1700037D RID: 893
	// (get) Token: 0x06001D34 RID: 7476 RVA: 0x000158D0 File Offset: 0x00013AD0
	public bool LoadedLocally
	{
		get
		{
			return this.loaded_locally;
		}
	}

	// Token: 0x1700037E RID: 894
	// (get) Token: 0x06001D35 RID: 7477 RVA: 0x000158D8 File Offset: 0x00013AD8
	public MinigameControllerState State
	{
		get
		{
			return this.controllerState;
		}
	}

	// Token: 0x1700037F RID: 895
	// (get) Token: 0x06001D36 RID: 7478 RVA: 0x000158E0 File Offset: 0x00013AE0
	// (set) Token: 0x06001D37 RID: 7479 RVA: 0x000158E8 File Offset: 0x00013AE8
	public GameObject Root
	{
		get
		{
			return this.minigame_root;
		}
		set
		{
			this.minigame_root = value;
		}
	}

	// Token: 0x06001D38 RID: 7480 RVA: 0x000158F1 File Offset: 0x00013AF1
	public PlayerMinigameLoadStatus GetPlayerStatus(short globalID)
	{
		if (this.playerStatus.ContainsKey(globalID))
		{
			return this.playerStatus[globalID];
		}
		return PlayerMinigameLoadStatus.Loading;
	}

	// Token: 0x17000380 RID: 896
	// (get) Token: 0x06001D39 RID: 7481 RVA: 0x0001590F File Offset: 0x00013B0F
	public int PlayersAlive
	{
		get
		{
			return this.players_alive;
		}
	}

	// Token: 0x17000381 RID: 897
	// (get) Token: 0x06001D3A RID: 7482 RVA: 0x00015917 File Offset: 0x00013B17
	// (set) Token: 0x06001D3B RID: 7483 RVA: 0x0001591F File Offset: 0x00013B1F
	public Transform[] SpawnPoints { get; set; }

	// Token: 0x06001D3C RID: 7484 RVA: 0x00015928 File Offset: 0x00013B28
	public override void OnNetInitialize()
	{
		GameManager.RegisterMinigameController(this);
		this.object_list = new Dictionary<int, GameObject>();
		this.net_object_list = new Dictionary<int, NetBehaviour>();
		NetSystem.SetSendRate(this.net_server_rate, this.net_client_rate);
	}

	// Token: 0x06001D3D RID: 7485 RVA: 0x00015957 File Offset: 0x00013B57
	public override void OnNetDestroy()
	{
		if (GameManager.Minigame != null)
		{
			this.ReleaseMinigame();
			this.DestroyObjects();
			GameManager.Minigame = null;
		}
	}

	// Token: 0x06001D3E RID: 7486 RVA: 0x00015978 File Offset: 0x00013B78
	public virtual void InitializeMinigame()
	{
		this.FindSpawnPoints();
		this.FindAllCameras();
	}

	// Token: 0x06001D3F RID: 7487 RVA: 0x000BF5C0 File Offset: 0x000BD7C0
	protected void FindSpawnPoints()
	{
		if (this.SpawnPoints == null)
		{
			Transform transform = this.minigame_root.transform.Find("SpawnPoints" + GameManager.GetPlayerCount().ToString());
			if (transform == null)
			{
				transform = this.minigame_root.transform.Find("SpawnPoints");
			}
			if (transform != null)
			{
				this.SpawnPoints = new Transform[transform.childCount];
				for (int i = 0; i < this.SpawnPoints.Length; i++)
				{
					this.SpawnPoints[i] = transform.GetChild(i);
				}
			}
		}
	}

	// Token: 0x06001D40 RID: 7488 RVA: 0x00015986 File Offset: 0x00013B86
	protected void FindAllCameras()
	{
		this.minigameCameras.Add(this.minigame_root.GetComponentInChildren<Camera>());
	}

	// Token: 0x06001D41 RID: 7489 RVA: 0x000BF65C File Offset: 0x000BD85C
	public virtual void OnPlayersReady()
	{
		MinigameAudioOverride component = this.Root.GetComponent<MinigameAudioOverride>();
		if (component == null)
		{
			AudioSystem.PlayAmbient(this.ambience, this.ambienceVolume, 1f);
			return;
		}
		AudioSystem.PlayAmbient(component.ambience, component.ambienceVolume, 1f);
	}

	// Token: 0x06001D42 RID: 7490 RVA: 0x000BF6AC File Offset: 0x000BD8AC
	public virtual void StartMinigame()
	{
		GameManager.scoreUIScene.State(true);
		if (this.music != null)
		{
			AudioSystem.PlayMusic(this.music, 1f, this.music_volume);
		}
		this.StartNewRound();
		if (this.rounds > 1)
		{
			this.CreateRoundUI();
		}
	}

	// Token: 0x06001D43 RID: 7491 RVA: 0x000BF700 File Offset: 0x000BD900
	public virtual void RoundEnded()
	{
		if (this.round >= this.rounds - 1)
		{
			AudioSystem.PlayMusic(null, 0.5f, 1f);
			AudioSystem.PlayOneShot("BoxingBellRing_STD_ZapSplat", 0.5f, 0f);
			if (NetSystem.IsServer)
			{
				this.BuildResults();
			}
		}
	}

	// Token: 0x06001D44 RID: 7492 RVA: 0x000BF750 File Offset: 0x000BD950
	public virtual void ResetRound()
	{
		this.round++;
		if (this.round >= this.rounds)
		{
			if (NetSystem.IsServer)
			{
				GameManager.ShowResultScreen();
			}
			this.controllerState = MinigameControllerState.ShowScoreScreen;
			return;
		}
		for (int i = 0; i < this.players.Count; i++)
		{
			this.players[i].ResetPlayer();
		}
		this.controllerState = MinigameControllerState.RoundStartWait;
	}

	// Token: 0x06001D45 RID: 7493 RVA: 0x0001599E File Offset: 0x00013B9E
	public virtual void RoundStarting()
	{
		if (this.uiRounds != null)
		{
			this.uiRounds.SetRound(this.round + 1);
		}
	}

	// Token: 0x06001D46 RID: 7494 RVA: 0x000159C1 File Offset: 0x00013BC1
	public virtual void StartNewRound()
	{
		this.players_alive = GameManager.GetPlayerCount();
		this.round_begin_time = Time.time;
	}

	// Token: 0x06001D47 RID: 7495 RVA: 0x000BF7C0 File Offset: 0x000BD9C0
	public virtual void BuildResults()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			this.players[i].GamePlayer.MinigameScore = (int)this.players[i].Score;
		}
	}

	// Token: 0x06001D48 RID: 7496 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void ReleaseMinigame()
	{
	}

	// Token: 0x06001D49 RID: 7497 RVA: 0x000159D9 File Offset: 0x00013BD9
	public virtual bool HasLoadedLocally()
	{
		return this.players.Count >= GameManager.GetPlayerCount();
	}

	// Token: 0x06001D4A RID: 7498 RVA: 0x000053AE File Offset: 0x000035AE
	public virtual Type GetPlayerType()
	{
		return null;
	}

	// Token: 0x06001D4B RID: 7499 RVA: 0x000159F0 File Offset: 0x00013BF0
	public bool CheckControllerLoaded()
	{
		if (!NetSystem.IsServer)
		{
			base.SendRPC("ControllerLoaded", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
			this.controllerState = MinigameControllerState.Initializing;
			return true;
		}
		if (this.controllers_loaded >= NetSystem.PlayerCount - 1)
		{
			this.controllerState = MinigameControllerState.Initializing;
			return true;
		}
		return false;
	}

	// Token: 0x06001D4C RID: 7500 RVA: 0x00015A2C File Offset: 0x00013C2C
	public void StartWaitForLoad()
	{
		this.controllerState = MinigameControllerState.WaitingForLoad;
	}

	// Token: 0x06001D4D RID: 7501 RVA: 0x000BF80C File Offset: 0x000BDA0C
	public void InitializeBase()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			if (this.players[i].player_root != null)
			{
				if (this.spawnType != PlayerSpawnType.None)
				{
					this.players[i].Deactivate();
				}
				else
				{
					this.players[i].Activate();
				}
			}
		}
		this.world_fader = this.minigame_root.GetComponent<WorldFader>();
		this.player_spawn_fx = Resources.Load<GameObject>("Prefabs/Effects/MinigameSpawn_FX");
	}

	// Token: 0x06001D4E RID: 7502 RVA: 0x000BF898 File Offset: 0x000BDA98
	public void StartMinigameBase(float time_before_start)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("StartMinigame", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				NetSystem.NetTime.GameTime,
				time_before_start
			});
			this.start_waiting = true;
			this.start_time = NetSystem.NetTime.LocalTime + time_before_start;
		}
	}

	// Token: 0x06001D4F RID: 7503 RVA: 0x000BF8F4 File Offset: 0x000BDAF4
	public void EndRound(float wait_before_reset = 1f, float time_before_start = 1f, bool forceEnd = false)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("StartNewRound", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				NetSystem.NetTime.GameTime,
				wait_before_reset,
				time_before_start,
				forceEnd
			});
			this.NewRound(Time.time + wait_before_reset, Time.time + wait_before_reset + time_before_start, forceEnd);
		}
	}

	// Token: 0x06001D50 RID: 7504 RVA: 0x00015A35 File Offset: 0x00013C35
	private void NewRound(float reset_time, float start_time, bool forceEnd)
	{
		if (forceEnd)
		{
			this.round = this.rounds - 1;
		}
		this.RoundEnded();
		this.new_round = true;
		this.round_start_time = start_time;
		this.round_reset_time = reset_time;
		this.controllerState = MinigameControllerState.RoundResetWait;
	}

	// Token: 0x06001D51 RID: 7505 RVA: 0x000BF960 File Offset: 0x000BDB60
	public virtual void UpdateBase()
	{
		switch (this.controllerState)
		{
		case MinigameControllerState.Initializing:
			if (this.start_waiting && Time.time >= this.start_time)
			{
				this.start_waiting = false;
				if (this.spawnType == PlayerSpawnType.None)
				{
					this.controllerState = MinigameControllerState.Countdown;
					return;
				}
				this.controllerState = MinigameControllerState.EnablePlayers;
				return;
			}
			break;
		case MinigameControllerState.FadeIn:
		case MinigameControllerState.Playing:
		case MinigameControllerState.PreScoreScreen:
		case MinigameControllerState.ShowScoreScreen:
			break;
		case MinigameControllerState.EnablePlayers:
			if (!this.spawning)
			{
				this.spawning = true;
				base.StartCoroutine(this.DoSpawnPlayersAnimation());
				this.OnSpawnPlayers();
				return;
			}
			if (this.finishedSpawning)
			{
				this.next_state_time = Time.time + 0.5f;
				this.controllerState = MinigameControllerState.Countdown;
				return;
			}
			break;
		case MinigameControllerState.Countdown:
			if (Time.time >= this.next_state_time)
			{
				if (this.countdown < 0)
				{
					if (this.new_round)
					{
						this.StartNewRound();
						this.controllerState = MinigameControllerState.Playing;
						this.new_round = false;
						return;
					}
					this.controllerState = MinigameControllerState.StartPlay;
					return;
				}
				else
				{
					if (this.countdown > 0)
					{
						GameManager.UIController.ShowCountdownText(this.countdown.ToString(), this.countdown_time_step);
						this.countdown--;
						this.next_state_time = Time.time + this.countdown_time_step;
						return;
					}
					this.countdown--;
					this.next_state_time = Time.time + 0f;
					return;
				}
			}
			break;
		case MinigameControllerState.StartPlay:
			this.StartMinigame();
			this.controllerState = MinigameControllerState.Playing;
			break;
		case MinigameControllerState.RoundResetWait:
			if (Time.time >= this.round_reset_time)
			{
				this.ResetRound();
				return;
			}
			break;
		case MinigameControllerState.RoundStartWait:
			if (Time.time >= this.round_start_time)
			{
				if (this.spawnType == PlayerSpawnType.None)
				{
					for (int i = 0; i < this.players.Count; i++)
					{
						this.players[i].Activate();
					}
					this.controllerState = MinigameControllerState.Countdown;
				}
				else
				{
					this.controllerState = MinigameControllerState.EnablePlayers;
				}
				this.players_enabled = 0;
				this.countdown = 3;
				this.RoundStarting();
				this.spawning = false;
				this.finishedSpawning = false;
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06001D52 RID: 7506 RVA: 0x0000398C File Offset: 0x00001B8C
	protected virtual void OnSpawnPlayers()
	{
	}

	// Token: 0x06001D53 RID: 7507 RVA: 0x000BFB64 File Offset: 0x000BDD64
	public void DestroyObjects()
	{
		foreach (KeyValuePair<int, GameObject> keyValuePair in this.object_list)
		{
			if (keyValuePair.Value != null)
			{
				UnityEngine.Object.Destroy(keyValuePair.Value);
			}
		}
		if (NetSystem.IsServer)
		{
			foreach (KeyValuePair<int, NetBehaviour> keyValuePair2 in this.net_object_list)
			{
				if (keyValuePair2.Value != null)
				{
					NetSystem.Kill(keyValuePair2.Value);
				}
			}
		}
	}

	// Token: 0x06001D54 RID: 7508 RVA: 0x00015A6B File Offset: 0x00013C6B
	protected virtual IEnumerator DoSpawnPlayersAnimation()
	{
		float gravity = 9.8f;
		float heightOffset = 10f;
		Material material = (Material)Resources.Load("Materials/PlayerClipMaterial");
		PlayerAnimation[] anims = new PlayerAnimation[this.players.Count];
		RuntimeAnimatorController[] controllers = new RuntimeAnimatorController[this.players.Count];
		float[] yVelocity = new float[this.players.Count];
		Vector3[] targetPositions = new Vector3[this.players.Count];
		float[] targetY = new float[this.players.Count];
		PortalEffect[] portalEffects = new PortalEffect[this.players.Count];
		NavMeshAgent[] navMeshAgent = new NavMeshAgent[this.players.Count];
		int num;
		for (int i = 0; i < this.players.Count; i = num)
		{
			if (this.players[i].doSpawning)
			{
				targetY[i] = this.players[i].transform.position.y;
				this.players[i].Activate();
				OutlineSource componentInChildren = this.players[i].gameObject.GetComponentInChildren<OutlineSource>();
				if (componentInChildren != null)
				{
					componentInChildren.enabled = false;
				}
				this.players[i].enabled = false;
				anims[i] = this.players[i].gameObject.GetComponentInChildren<PlayerAnimation>(true);
				if (this.spawnType != PlayerSpawnType.PortalAboveNoAnimation)
				{
					controllers[i] = anims[i].Animator.runtimeAnimatorController;
					anims[i].Animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("BoardAC");
					anims[i].Grounded = false;
					anims[i].FireFallingTrigger();
				}
				Vector3 position = this.players[i].transform.position + Vector3.up * this.portalOffset;
				portalEffects[i] = PortalEffect.Spawn(position, Vector3.down, PortalOrientation.Horizontal, this.players[i].gameObject, true);
				targetPositions[i] = this.players[i].transform.position;
				this.players[i].gameObject.transform.position = this.players[i].transform.position + Vector3.up * heightOffset;
				navMeshAgent[i] = this.players[i].GetComponent<NavMeshAgent>();
				if (navMeshAgent[i] != null && this.players[i].GamePlayer.IsAI)
				{
					navMeshAgent[i].enabled = false;
				}
				yield return new WaitForSeconds(0.1f);
			}
			num = i + 1;
		}
		float startTime = Time.time;
		float maxTime = 6f;
		while (Time.time - startTime < maxTime)
		{
			bool flag = true;
			for (int j = 0; j < this.players.Count; j++)
			{
				if (this.players[j].doSpawning)
				{
					yVelocity[j] += gravity * Time.deltaTime;
					this.players[j].transform.position -= new Vector3(0f, yVelocity[j] * Time.deltaTime, 0f);
					if (this.players[j].transform.position.y < targetY[j] + 1.5f && this.spawnType != PlayerSpawnType.PortalAboveNoAnimation)
					{
						anims[j].Grounded = true;
						anims[j].MovementAxis = Vector3.zero;
					}
					if (this.players[j].transform.position.y < targetY[j])
					{
						this.players[j].transform.position = targetPositions[j];
						if (this.spawnType != PlayerSpawnType.PortalAboveNoAnimation)
						{
							anims[j].VelocityY = yVelocity[j];
						}
					}
					else
					{
						flag = false;
					}
				}
			}
			if (flag)
			{
				break;
			}
			yield return null;
		}
		yield return new WaitForSeconds(1.4f);
		for (int k = 0; k < this.players.Count; k++)
		{
			if (this.players[k].doSpawning)
			{
				OutlineSource componentInChildren2 = this.players[k].gameObject.GetComponentInChildren<OutlineSource>(true);
				if (componentInChildren2 != null && this.players[k].enableOutlineSource)
				{
					componentInChildren2.enabled = true;
				}
				if (portalEffects[k] != null)
				{
					portalEffects[k].Release(true);
				}
				if (this.spawnType != PlayerSpawnType.PortalAboveNoAnimation)
				{
					anims[k].Animator.runtimeAnimatorController = controllers[k];
				}
				if (navMeshAgent[k] != null && this.players[k].GamePlayer.IsAI && this.players[k].IsOwner)
				{
					navMeshAgent[k].enabled = true;
				}
				this.players[k].enabled = true;
				this.players[k].FinishedSpawning();
			}
		}
		OutlineSource.m_sourcesChanged = true;
		this.finishedSpawning = true;
		yield break;
	}

	// Token: 0x06001D55 RID: 7509 RVA: 0x000BFC28 File Offset: 0x000BDE28
	public void PlayerReady(short globalID)
	{
		if (!this.playerStatus.ContainsKey(globalID) || this.playerStatus[globalID] != PlayerMinigameLoadStatus.Ready)
		{
			this.SetPlayerReady(globalID);
			base.SendRPC("ClientReady", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				globalID
			});
		}
	}

	// Token: 0x06001D56 RID: 7510 RVA: 0x000BFC74 File Offset: 0x000BDE74
	public void CheckMinigameLoaded()
	{
		if (!this.loaded_locally && this.HasLoadedLocally())
		{
			this.SetClientLoaded(NetSystem.MyPlayer.UserID);
			base.SendRPC("ClientLoaded", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				NetSystem.MyPlayer.UserID
			});
			this.InitializeBase();
			this.loaded_locally = true;
		}
	}

	// Token: 0x06001D57 RID: 7511 RVA: 0x000BFCD4 File Offset: 0x000BDED4
	public void SetAllClientsReady()
	{
		List<GamePlayer> playerList = GameManager.PlayerList;
		for (int i = 0; i < playerList.Count; i++)
		{
			if (!this.playerStatus.ContainsKey(playerList[i].GlobalID))
			{
				return;
			}
			this.playerStatus[playerList[i].GlobalID] = PlayerMinigameLoadStatus.Ready;
		}
	}

	// Token: 0x06001D58 RID: 7512 RVA: 0x000BFD2C File Offset: 0x000BDF2C
	public bool AllClientsLoaded()
	{
		List<GamePlayer> playerList = GameManager.PlayerList;
		for (int i = 0; i < playerList.Count; i++)
		{
			if (!this.playerStatus.ContainsKey(playerList[i].GlobalID))
			{
				return false;
			}
			if (this.playerStatus[playerList[i].GlobalID] == PlayerMinigameLoadStatus.Loading)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001D59 RID: 7513 RVA: 0x000BFD88 File Offset: 0x000BDF88
	public bool AllClientsReady()
	{
		List<GamePlayer> playerList = GameManager.PlayerList;
		for (int i = 0; i < playerList.Count; i++)
		{
			if (!this.playerStatus.ContainsKey(playerList[i].GlobalID))
			{
				return false;
			}
			if (this.playerStatus[playerList[i].GlobalID] != PlayerMinigameLoadStatus.Ready)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001D5A RID: 7514 RVA: 0x000BFDE4 File Offset: 0x000BDFE4
	private void SetClientLoaded(ushort userID)
	{
		List<GamePlayer> playerList = GameManager.PlayerList;
		for (int i = 0; i < playerList.Count; i++)
		{
			if (playerList[i].NetOwner.UserID == userID)
			{
				if (this.playerStatus.ContainsKey(playerList[i].GlobalID))
				{
					this.playerStatus[playerList[i].GlobalID] = PlayerMinigameLoadStatus.Waiting;
				}
				else
				{
					this.playerStatus.Add(playerList[i].GlobalID, PlayerMinigameLoadStatus.Waiting);
				}
				if (this.OnPlayerStatusChange != null)
				{
					this.OnPlayerStatusChange(playerList[i].GlobalID, PlayerMinigameLoadStatus.Waiting);
				}
			}
		}
	}

	// Token: 0x06001D5B RID: 7515 RVA: 0x000BFE90 File Offset: 0x000BE090
	private void SetPlayerReady(short globalID)
	{
		if (this.playerStatus.ContainsKey(globalID))
		{
			this.playerStatus[globalID] = PlayerMinigameLoadStatus.Ready;
		}
		else
		{
			this.playerStatus.Add(globalID, PlayerMinigameLoadStatus.Ready);
		}
		if (!GameManager.GetPlayerAt((int)globalID).IsAI)
		{
			AudioSystem.PlayOneShot("MultimediaButtonClick007_STD_ZapSplat", 0.3f, 0f);
		}
		if (this.OnPlayerStatusChange != null)
		{
			this.OnPlayerStatusChange(globalID, PlayerMinigameLoadStatus.Ready);
		}
	}

	// Token: 0x06001D5C RID: 7516 RVA: 0x0000398C File Offset: 0x00001B8C
	public void AddPlayerObject(int index, GameObject player)
	{
	}

	// Token: 0x06001D5D RID: 7517 RVA: 0x00015A7A File Offset: 0x00013C7A
	public void ShowResultScreen()
	{
		this.controllerState = MinigameControllerState.PreScoreScreen;
	}

	// Token: 0x06001D5E RID: 7518 RVA: 0x000BFF00 File Offset: 0x000BE100
	public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion transform)
	{
		if (prefab == null)
		{
			Debug.LogError("Minigame Controller cannot spawn prefab because it is null!");
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, position, transform);
		if (gameObject)
		{
			try
			{
				this.object_list.Add(gameObject.GetInstanceID(), gameObject);
			}
			catch (Exception ex)
			{
				Debug.LogError("Minigame Controller cannot spawn prefab! " + ex.ToString());
				return null;
			}
			return gameObject;
		}
		return gameObject;
	}

	// Token: 0x06001D5F RID: 7519 RVA: 0x00015A83 File Offset: 0x00013C83
	public GameObject NetSpawn(string netprefab_name, Vector3 position, ushort ownerSlot = 0, NetPlayer owner = null)
	{
		return this.NetSpawn(netprefab_name, position, Quaternion.identity, ownerSlot, owner);
	}

	// Token: 0x06001D60 RID: 7520 RVA: 0x00015A95 File Offset: 0x00013C95
	public GameObject NetSpawn(string netprefab_name, Vector3 position, Vector3 rotation, ushort ownerSlot = 0, NetPlayer owner = null)
	{
		return this.NetSpawn(netprefab_name, position, Quaternion.Euler(rotation), ownerSlot, owner);
	}

	// Token: 0x06001D61 RID: 7521 RVA: 0x000BFF74 File Offset: 0x000BE174
	public GameObject NetSpawn(string netprefab_name, Vector3 position, Quaternion rotation, ushort ownerSlot = 0, NetPlayer owner = null)
	{
		GameObject gameObject = NetSystem.Spawn(netprefab_name, position, rotation, ownerSlot, owner);
		if (gameObject)
		{
			try
			{
				NetBehaviour component = gameObject.GetComponent<NetBehaviour>();
				this.net_object_list.Add(component.GetInstanceID(), component);
			}
			catch (Exception ex)
			{
				Debug.LogError("Minigame Controller cannot spawn prefab!: " + ex.ToString());
				return null;
			}
			return gameObject;
		}
		return gameObject;
	}

	// Token: 0x06001D62 RID: 7522 RVA: 0x00015AA9 File Offset: 0x00013CA9
	public void Kill(GameObject obj)
	{
		this.Kill(obj, 0f);
	}

	// Token: 0x06001D63 RID: 7523 RVA: 0x00015AB7 File Offset: 0x00013CB7
	public void Kill(GameObject obj, float delay)
	{
		if (obj == null)
		{
			Debug.LogError("Minigame Controller cannot kill object because it is null!");
			return;
		}
		this.object_list.Remove(obj.GetInstanceID());
		UnityEngine.Object.Destroy(obj, delay);
	}

	// Token: 0x06001D64 RID: 7524 RVA: 0x00015AE6 File Offset: 0x00013CE6
	public void NetKill(NetBehaviour obj)
	{
		if (obj == null)
		{
			Debug.LogError("Minigame Controller cannot kill object because it is null!");
			return;
		}
		this.object_list.Remove(obj.GetInstanceID());
		this.net_object_list.Remove(obj.GetInstanceID());
		NetSystem.Kill(obj);
	}

	// Token: 0x06001D65 RID: 7525 RVA: 0x00015B26 File Offset: 0x00013D26
	public void UpdateScoreUI(int player_slot, short score)
	{
		if (this.ui_score != null && this.ui_score[player_slot] != null)
		{
			this.ui_score[player_slot].SetScore((int)score);
		}
	}

	// Token: 0x06001D66 RID: 7526 RVA: 0x000BFFE0 File Offset: 0x000BE1E0
	public void SetScoreUIParent(int playerSlot, Transform tr)
	{
		if (this.ui_score != null && this.ui_score[playerSlot] != null)
		{
			this.ui_score[playerSlot].transform.SetParent(tr);
			this.ui_score[playerSlot].transform.position = Vector3.zero;
			((RectTransform)this.ui_score[playerSlot].transform).anchoredPosition = Vector2.zero;
		}
	}

	// Token: 0x06001D67 RID: 7527 RVA: 0x00015B4E File Offset: 0x00013D4E
	protected void CreateTimer(UIAnchorType type, Vector2 pos, float start_time)
	{
		this.ui_timer = GameManager.UIController.CreateMinigameTimer(type, pos);
		this.ui_timer.time_test = start_time;
	}

	// Token: 0x06001D68 RID: 7528 RVA: 0x000C004C File Offset: 0x000BE24C
	protected void CreateScoreUI(UIAnchorType type, Vector2 pos, float vertical_spacing, bool forceAnchor = false)
	{
		for (int i = 0; i < GameManager.GetPlayerCount(); i++)
		{
			GamePlayer playerAt = GameManager.GetPlayerAt(i);
			this.ui_score[i] = GameManager.UIController.CreateMinigameScore(type, pos + new Vector2(0f, vertical_spacing * (float)i), forceAnchor);
			this.ui_score[i].Initialize(playerAt);
		}
	}

	// Token: 0x06001D69 RID: 7529 RVA: 0x00015B6E File Offset: 0x00013D6E
	protected void CreateRoundUI()
	{
		this.uiRounds = GameManager.UIController.CreateMinigameRoundsUI();
		this.uiRounds.Setup(this.rounds);
	}

	// Token: 0x06001D6A RID: 7530 RVA: 0x000C00A8 File Offset: 0x000BE2A8
	protected void SpawnPlayers(string net_name, Transform[] customSpawnPoints = null)
	{
		if (customSpawnPoints == null)
		{
			this.FindSpawnPoints();
		}
		else
		{
			this.SpawnPoints = customSpawnPoints;
		}
		int num = 0;
		MinigameSpawnPositionType minigameSpawnPositionType = this.spawnPositionType;
		if (minigameSpawnPositionType == MinigameSpawnPositionType.Default)
		{
			int num2 = 0;
			for (int i = 0; i < GameManager.GetPlayerCount(); i++)
			{
				num = num2 % this.SpawnPoints.Length;
				Vector3 position = this.SpawnPoints[num].position;
				Quaternion rotation = this.SpawnPoints[num].rotation;
				this.NetSpawn(net_name, position, rotation, (ushort)i, GameManager.GetPlayerAt(i).NetOwner).GetComponent<CharacterBase>();
				if (GameManager.GetPlayerCount() == 2 && !this.overrideTwoPlayerSpawn)
				{
					num2 += 2;
				}
				else
				{
					num2++;
				}
			}
			return;
		}
		if (minigameSpawnPositionType != MinigameSpawnPositionType.Spread)
		{
			return;
		}
		Vector3 position2 = this.SpawnPoints[0].position;
		Vector3 position3 = this.SpawnPoints[this.SpawnPoints.Length - 1].position;
		float num3 = 0f;
		float num4 = Vector3.Distance(position2, position3);
		if (GameManager.GetPlayerCount() == 2)
		{
			num3 = 0f;
			num4 = 1f;
		}
		else if (GameManager.GetPlayerCount() >= 3)
		{
			num4 = num4 / (float)(GameManager.GetPlayerCount() - 1) / num4;
		}
		for (int j = 0; j < GameManager.GetPlayerCount(); j++)
		{
			Vector3 position4 = Vector3.Lerp(position2, position3, num3);
			Quaternion rotation2 = this.SpawnPoints[0].rotation;
			this.NetSpawn(net_name, position4, rotation2, (ushort)j, GameManager.GetPlayerAt(j).NetOwner).GetComponent<CharacterBase>();
			num++;
			num3 += num4;
		}
	}

	// Token: 0x06001D6B RID: 7531 RVA: 0x000C0214 File Offset: 0x000BE414
	protected T[] SpawnPlayers<T>(string net_name)
	{
		T[] array = new T[GameManager.GetPlayerCount()];
		for (int i = 0; i < GameManager.GetPlayerCount(); i++)
		{
			CharacterBase component = this.NetSpawn(net_name, this.SpawnPoints[i].position, this.SpawnPoints[i].rotation, (ushort)i, GameManager.GetPlayerAt(i).NetOwner).GetComponent<CharacterBase>();
			array[i] = component.gameObject.GetComponent<T>();
		}
		return array;
	}

	// Token: 0x06001D6C RID: 7532 RVA: 0x00015B91 File Offset: 0x00013D91
	public void AddPlayer(CharacterBase p)
	{
		this.players.Add(p);
	}

	// Token: 0x06001D6D RID: 7533 RVA: 0x000C0284 File Offset: 0x000BE484
	public CharacterBase GetPlayerInSlot(short slot)
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			if (this.players[i].OwnerSlot == (ushort)slot)
			{
				return this.players[i];
			}
		}
		return null;
	}

	// Token: 0x06001D6E RID: 7534 RVA: 0x00015B9F File Offset: 0x00013D9F
	public CharacterBase GetPlayer(int i)
	{
		return this.players[i];
	}

	// Token: 0x06001D6F RID: 7535 RVA: 0x00015BAD File Offset: 0x00013DAD
	public int GetPlayerCount()
	{
		return this.players.Count;
	}

	// Token: 0x06001D70 RID: 7536 RVA: 0x00015BBA File Offset: 0x00013DBA
	public float TimeSinceStart()
	{
		return Time.time - this.start_time;
	}

	// Token: 0x17000382 RID: 898
	// (get) Token: 0x06001D71 RID: 7537 RVA: 0x00015BC8 File Offset: 0x00013DC8
	public bool Playable
	{
		get
		{
			return this.State == MinigameControllerState.Playing;
		}
	}

	// Token: 0x06001D72 RID: 7538 RVA: 0x000C02CC File Offset: 0x000BE4CC
	public void SetCamerasEnabled(bool enabled)
	{
		for (int i = 0; i < this.minigameCameras.Count; i++)
		{
			if (this.minigameCameras[i] != null)
			{
				this.minigameCameras[i].enabled = enabled;
			}
		}
	}

	// Token: 0x06001D73 RID: 7539 RVA: 0x00015BD3 File Offset: 0x00013DD3
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void ControllerLoaded(NetPlayer sender)
	{
		this.controllers_loaded++;
	}

	// Token: 0x06001D74 RID: 7540 RVA: 0x00015BE3 File Offset: 0x00013DE3
	[NetRPC(true, NetRPCSecurity.ALL, NetRPCSecurity.ALL)]
	public void ClientLoaded(NetPlayer sender, ushort userID)
	{
		this.SetClientLoaded(userID);
	}

	// Token: 0x06001D75 RID: 7541 RVA: 0x00015BEC File Offset: 0x00013DEC
	[NetRPC(true, NetRPCSecurity.ALL, NetRPCSecurity.ALL)]
	public void ClientReady(NetPlayer sender, short globalID)
	{
		this.SetPlayerReady(globalID);
	}

	// Token: 0x06001D76 RID: 7542 RVA: 0x00015BF5 File Offset: 0x00013DF5
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.PROXY)]
	public void StartMinigame(NetPlayer sender, float game_time, float start_offset)
	{
		this.start_waiting = true;
		this.start_time = NetSystem.NetTime.LocalTime + (start_offset - (NetSystem.NetTime.GameTime - game_time));
	}

	// Token: 0x06001D77 RID: 7543 RVA: 0x000C0318 File Offset: 0x000BE518
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.PROXY)]
	public void StartNewRound(NetPlayer sender, float game_time, float reset_offset, float start_offset, bool forceEnd)
	{
		float num = NetSystem.NetTime.GameTime - game_time;
		this.NewRound(Time.time + (reset_offset - num), Time.time + (reset_offset + start_offset - num), forceEnd);
	}

	// Token: 0x04001FD3 RID: 8147
	public MinigameController.PlayerStatusChange OnPlayerStatusChange;

	// Token: 0x04001FD4 RID: 8148
	protected Dictionary<short, PlayerMinigameLoadStatus> playerStatus = new Dictionary<short, PlayerMinigameLoadStatus>();

	// Token: 0x04001FD5 RID: 8149
	protected int controllers_loaded;

	// Token: 0x04001FD6 RID: 8150
	protected bool loaded_locally;

	// Token: 0x04001FD7 RID: 8151
	[HideInInspector]
	public List<Camera> minigameCameras = new List<Camera>();

	// Token: 0x04001FD8 RID: 8152
	protected Dictionary<int, GameObject> object_list;

	// Token: 0x04001FD9 RID: 8153
	protected Dictionary<int, NetBehaviour> net_object_list;

	// Token: 0x04001FDA RID: 8154
	protected MinigameControllerState controllerState;

	// Token: 0x04001FDB RID: 8155
	protected WorldFader world_fader;

	// Token: 0x04001FDC RID: 8156
	protected GameObject minigame_root;

	// Token: 0x04001FDD RID: 8157
	private bool start_waiting;

	// Token: 0x04001FDE RID: 8158
	private float start_time;

	// Token: 0x04001FDF RID: 8159
	private float fade_in_time = 1f;

	// Token: 0x04001FE0 RID: 8160
	private float next_state_time;

	// Token: 0x04001FE1 RID: 8161
	private float player_enable_time = 0.35f;

	// Token: 0x04001FE2 RID: 8162
	private float next_player_enable;

	// Token: 0x04001FE3 RID: 8163
	private int players_enabled;

	// Token: 0x04001FE4 RID: 8164
	private float countdown_time_step = 0.5f;

	// Token: 0x04001FE5 RID: 8165
	private int countdown = 3;

	// Token: 0x04001FE6 RID: 8166
	private GameObject player_spawn_fx;

	// Token: 0x04001FE7 RID: 8167
	private float round_start_time;

	// Token: 0x04001FE8 RID: 8168
	private float round_reset_time;

	// Token: 0x04001FE9 RID: 8169
	private bool new_round;

	// Token: 0x04001FEA RID: 8170
	private ResultSceenScene result_screen;

	// Token: 0x04001FEB RID: 8171
	[Header("Base minigame attributes")]
	public PlayerSpawnType spawnType;

	// Token: 0x04001FEC RID: 8172
	public float portalOffset = 4.5f;

	// Token: 0x04001FED RID: 8173
	public float music_volume = 1f;

	// Token: 0x04001FEE RID: 8174
	public AudioClip music;

	// Token: 0x04001FEF RID: 8175
	public float ambienceVolume = 1f;

	// Token: 0x04001FF0 RID: 8176
	public AudioClip ambience;

	// Token: 0x04001FF1 RID: 8177
	public int net_client_rate = 60;

	// Token: 0x04001FF2 RID: 8178
	public int net_server_rate = 60;

	// Token: 0x04001FF3 RID: 8179
	public int rounds = 3;

	// Token: 0x04001FF4 RID: 8180
	public float round_length = 45f;

	// Token: 0x04001FF5 RID: 8181
	[HideInInspector]
	public List<CharacterBase> players = new List<CharacterBase>();

	// Token: 0x04001FF6 RID: 8182
	protected int players_alive;

	// Token: 0x04001FF7 RID: 8183
	protected int round;

	// Token: 0x04001FF8 RID: 8184
	protected float round_begin_time;

	// Token: 0x04001FF9 RID: 8185
	protected UIMinigameTimer ui_timer;

	// Token: 0x04001FFA RID: 8186
	protected UIMinigameScore[] ui_score = new UIMinigameScore[8];

	// Token: 0x04001FFB RID: 8187
	protected UIRounds uiRounds;

	// Token: 0x04001FFD RID: 8189
	private bool spawning;

	// Token: 0x04001FFE RID: 8190
	protected bool finishedSpawning;

	// Token: 0x04001FFF RID: 8191
	public MinigameSpawnPositionType spawnPositionType;

	// Token: 0x04002000 RID: 8192
	public bool overrideTwoPlayerSpawn;

	// Token: 0x02000423 RID: 1059
	// (Invoke) Token: 0x06001D7A RID: 7546
	public delegate void PlayerStatusChange(short globalID, PlayerMinigameLoadStatus status);
}
