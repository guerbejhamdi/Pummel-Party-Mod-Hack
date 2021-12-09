using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using LlockhamIndustries.Decals;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000366 RID: 870
public class ChallengeItem : Item
{
	// Token: 0x1700022E RID: 558
	// (get) Token: 0x06001751 RID: 5969 RVA: 0x0001168F File Offset: 0x0000F88F
	public ChallengeItemGameDetails CurGame
	{
		get
		{
			return this.m_curGame;
		}
	}

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x06001752 RID: 5970 RVA: 0x00011697 File Offset: 0x0000F897
	// (set) Token: 0x06001753 RID: 5971 RVA: 0x0001169F File Offset: 0x0000F89F
	public ChallengeItemState ChallengeItemGameState { get; set; }

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x06001754 RID: 5972 RVA: 0x000116A8 File Offset: 0x0000F8A8
	// (set) Token: 0x06001755 RID: 5973 RVA: 0x000A1E78 File Offset: 0x000A0078
	public byte TargetID
	{
		get
		{
			return this.targetID.Value;
		}
		set
		{
			if (base.IsOwner)
			{
				this.targetID.Value = value;
			}
			BoardActor actor = GameManager.Board.GetActor(this.targetID.Value);
			if (actor.GetType() == typeof(BoardPlayer))
			{
				Transform transform = actor.transform;
				GameManager.Board.boardCamera.SetTrackedObject(transform, GameManager.Board.PlayerCamOffset);
				this.m_targeterObj.transform.position = transform.position - new Vector3(0f, 0.75f, 0f);
				BoardPlayer boardPlayer = (BoardPlayer)actor;
				if (this.m_ui != null && boardPlayer != null)
				{
					this.m_ui.SetPlayer(1, boardPlayer.GamePlayer);
				}
			}
		}
	}

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x06001756 RID: 5974 RVA: 0x000116B5 File Offset: 0x0000F8B5
	// (set) Token: 0x06001757 RID: 5975 RVA: 0x000116C2 File Offset: 0x0000F8C2
	public byte GameID
	{
		get
		{
			return this.gameID.Value;
		}
		set
		{
			if (base.IsOwner)
			{
				this.gameID.Value = value;
			}
			this.m_arcade.SetSelectedGame((int)value);
		}
	}

	// Token: 0x06001758 RID: 5976 RVA: 0x00003C5A File Offset: 0x00001E5A
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
	}

	// Token: 0x06001759 RID: 5977 RVA: 0x000A1F4C File Offset: 0x000A014C
	public override void Setup()
	{
		base.Setup();
		base.GetAITarget();
		this.m_targeterObj = UnityEngine.Object.Instantiate<GameObject>(this.m_targeterPrefab, Vector3.zero, Quaternion.Euler(90f, 0f, 0f));
		this.m_challengeUIObj = UnityEngine.Object.Instantiate<GameObject>(this.m_challengeItemUIPrefab, Vector3.zero, Quaternion.identity);
		this.m_ui = this.m_challengeUIObj.GetComponent<ChallengeItemUI>();
		this.m_ui.SetPlayer(0, this.player);
		GameManager.scoreUIScene.State(true);
		Vector3 position = this.player.BoardObject.transform.position - this.player.BoardObject.transform.forward * 1.25f;
		this.m_arcadeObj = UnityEngine.Object.Instantiate<GameObject>(this.m_arcadePrefab, position, Quaternion.Euler(0f, 0f, 0f));
		this.m_arcade = this.m_arcadeObj.GetComponent<ArcadeMachineHelper>();
		this.TargetID = ((this.player.GlobalID == 0) ? 1 : 0);
		this.targetID.Recieve = new RecieveProxy(this.TargetReceive);
		this.gameID.Recieve = new RecieveProxy(this.GameReceive);
		base.SetNetworkState(Item.ItemState.Setup);
		this.m_numGames = Enum.GetValues(typeof(ChallengeItemGameType)).Length;
		this.ChallengeItemGameState = ChallengeItemState.ChooseOpponent;
		AudioSystem.PlayMusic(this.m_music, 0.5f, 1f);
	}

	// Token: 0x0600175A RID: 5978 RVA: 0x000A20CC File Offset: 0x000A02CC
	public override void Update()
	{
		if (this.curState == Item.ItemState.Aiming && base.IsOwner)
		{
			if (this.player.IsAI)
			{
				if (!this.foundTarget)
				{
					if (this.aiTimer.Elapsed(true))
					{
						if (this.AITarget.ActorID == this.TargetID)
						{
							this.foundTarget = true;
							this.aiTimer = new ActionTimer(1.5f);
							this.aiTimer.Start();
						}
						else
						{
							this.TargetID = this.Increment(true, this.TargetID);
						}
					}
				}
				else if (this.aiTimer.Elapsed(true))
				{
					base.AIUseItem();
				}
			}
			else if (!GameManager.IsGamePaused)
			{
				if (this.player.RewiredPlayer.GetNegativeButtonDown(InputActions.Horizontal))
				{
					this.TargetID = this.Increment(true, this.TargetID);
				}
				else if (this.player.RewiredPlayer.GetButtonDown(InputActions.Horizontal))
				{
					this.TargetID = this.Increment(false, this.TargetID);
				}
			}
		}
		switch (this.ChallengeItemGameState)
		{
		case ChallengeItemState.ChooseGame:
			if (base.IsOwner)
			{
				if (!this.player.IsAI)
				{
					int num = (int)this.GameID;
					if (this.player.RewiredPlayer.GetNegativeButtonDown(InputActions.Horizontal))
					{
						num = ((num - 1 >= 0) ? (num - 1) : (this.m_numGames - 1));
						this.GameID = (byte)num;
					}
					else if (this.player.RewiredPlayer.GetButtonDown(InputActions.Horizontal))
					{
						num = ((num + 1 <= this.m_numGames - 1) ? (num + 1) : 0);
						this.GameID = (byte)num;
					}
					if (this.player.RewiredPlayer.GetButtonDown(InputActions.Accept))
					{
						this.SelectGame(this.GameID);
					}
				}
				else if (Time.time - this.UseTimeStart > 2f)
				{
					this.SelectGame(this.GameID);
				}
			}
			break;
		case ChallengeItemState.WaitingForReady:
		{
			float num2 = Time.time - this.m_readyWaitStartTime;
			int num3 = (int)Mathf.Clamp(this.m_readyWaitTime - num2, 0f, this.m_readyWaitTime);
			this.m_arcade.m_readyWindowHeaderTxt.text = LocalizationManager.GetTranslation("GameStartingIn", true, 0, true, false, null, null, true) + " " + num3.ToString() + "..";
			this.m_arcade.m_readyWindowDescriptionTxt.text = LocalizationManager.GetTranslation(this.m_curGame.description, true, 0, true, false, null, null, true);
			if (base.IsOwner && (num2 > this.m_readyWaitTime || (this.p1 != null && this.p2 != null && this.p1.IsReady && this.p2.IsReady)))
			{
				this.StartGameWithCountdown(this.GameID, NetSystem.NetTime.GameTime);
			}
			break;
		}
		case ChallengeItemState.GameCountdown:
			this.m_arcade.SetCountDown(this.m_gameStartTime - Time.time);
			if (Time.time >= this.m_gameStartTime)
			{
				this.ChallengeItemGameState = ChallengeItemState.PlayingGame;
				this.m_arcade.SetCountDown(-1f);
				this.m_gameEndTime = Time.time + (float)this.m_curGame.gameLength;
			}
			break;
		case ChallengeItemState.PlayingGame:
		{
			float num4 = this.m_gameEndTime - Time.time;
			if (num4 <= 0f)
			{
				if (base.IsOwner)
				{
					this.EndGame();
				}
			}
			else
			{
				this.m_arcade.SetGameTime(Mathf.Clamp(num4, 0f, (float)this.m_curGame.gameLength));
				ChallengeItemGameType gameType = this.m_curGame.gameType;
				if (gameType != ChallengeItemGameType.RoadDodge)
				{
					if (gameType != ChallengeItemGameType.Paddles)
					{
					}
				}
				else if (NetSystem.IsServer)
				{
					foreach (ArcadeSpawnPoint arcadeSpawnPoint in this.m_arcade.spawnPoints)
					{
						if (Time.time > arcadeSpawnPoint.NextSpawnTime)
						{
							this.SpawnVehicle(arcadeSpawnPoint.SpawnIndex, UnityEngine.Random.Range(0, this.m_vehiclePfbs.Length), UnityEngine.Random.Range(400f, 650f));
							arcadeSpawnPoint.NextSpawnTime = Time.time + UnityEngine.Random.Range(1f, 2.5f);
						}
					}
				}
			}
			break;
		}
		}
		base.Update();
	}

	// Token: 0x0600175B RID: 5979 RVA: 0x000A2528 File Offset: 0x000A0728
	public void SpawnVehicle(int spawnIndex, int vehicleIndex, float speed)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSpawnVehicle", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				(short)spawnIndex,
				(short)vehicleIndex,
				speed
			});
		}
		ArcadeSpawnPoint arcadeSpawnPoint = null;
		foreach (ArcadeSpawnPoint arcadeSpawnPoint2 in this.m_arcade.spawnPoints)
		{
			if (arcadeSpawnPoint2.SpawnIndex == spawnIndex)
			{
				arcadeSpawnPoint = arcadeSpawnPoint2;
				break;
			}
		}
		if (arcadeSpawnPoint != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_vehiclePfbs[vehicleIndex], arcadeSpawnPoint.transform.position, arcadeSpawnPoint.transform.rotation);
			gameObject.transform.SetParent(arcadeSpawnPoint.transform, false);
			((RectTransform)gameObject.transform).anchoredPosition3D = Vector3.zero;
			gameObject.transform.SetParent(arcadeSpawnPoint.transform.parent.parent.parent);
			ArcadeVehicle component = gameObject.GetComponent<ArcadeVehicle>();
			component.Speed = speed * (float)(arcadeSpawnPoint.left ? -1 : 1);
			this.m_spawnedVehicles.Add(component);
			UnityEngine.Object.Destroy(gameObject, 10f);
			return;
		}
		Debug.LogError("Vehicle spawn was null with id = " + spawnIndex.ToString());
	}

	// Token: 0x0600175C RID: 5980 RVA: 0x000116E4 File Offset: 0x0000F8E4
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSpawnVehicle(NetPlayer sender, short spawnIndex, short vehicleIndex, float speed)
	{
		this.SpawnVehicle((int)spawnIndex, (int)vehicleIndex, speed);
	}

	// Token: 0x0600175D RID: 5981 RVA: 0x000116F0 File Offset: 0x0000F8F0
	private void EndGame()
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCEndGame", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		this.ChallengeItemGameState = ChallengeItemState.Finalizing;
		base.StartCoroutine(this.EndGameCoroutine());
	}

	// Token: 0x0600175E RID: 5982 RVA: 0x0001171F File Offset: 0x0000F91F
	private IEnumerator EndGameCoroutine()
	{
		AudioSystem.PlayOneShot(this.m_endBell, 0.5f, 0f, 1f);
		yield return new WaitForSeconds(1f);
		if (base.IsOwner)
		{
			this.FinishGame(this.p1.netScore.Value >= this.p2.netScore.Value, this.p1.netScore.Value == this.p2.netScore.Value);
		}
		yield break;
	}

	// Token: 0x0600175F RID: 5983 RVA: 0x0001172E File Offset: 0x0000F92E
	private void FinishGame(bool won, bool tie)
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCFinishGame", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				won,
				tie
			});
		}
		base.StartCoroutine(this.FinishGameCoroutine(won, tie));
	}

	// Token: 0x06001760 RID: 5984 RVA: 0x0001176B File Offset: 0x0000F96B
	private IEnumerator FinishGameCoroutine(bool won, bool tie)
	{
		BoardPlayer winner = won ? this.player.BoardObject : this.challengeTarget;
		LeanTween.move(this.m_arcade.gameObject, this.m_arcade.transform.position + new Vector3(0f, 10f, 0f), 0.5f).setEase(LeanTweenType.easeInOutSine);
		GameManager.Board.boardCamera.SetTrackedObject(winner.transform, GameManager.Board.PlayerCamOffset);
		GameManager.Board.boardCamera.enabled = true;
		this.m_arcade.m_postProcessVolume.gameObject.SetActive(false);
		if (NetSystem.IsServer)
		{
			NetSystem.Kill(this.p1);
			NetSystem.Kill(this.p2);
			if (this.ball != null)
			{
				NetSystem.Kill(this.ball);
			}
		}
		yield return new WaitUntil(() => GameManager.Board.boardCamera.WithinDistance(0.5f));
		if (!tie)
		{
			AudioSystem.PlayOneShot(this.gobletFanfare, 1f, 0f, 1f);
			winner.GiveTrophy(1, 0, true);
		}
		yield return new WaitForSeconds(2.5f);
		base.Finish(false);
		yield break;
	}

	// Token: 0x06001761 RID: 5985 RVA: 0x00011788 File Offset: 0x0000F988
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	private void RPCEndGame(NetPlayer sender)
	{
		this.EndGame();
	}

	// Token: 0x06001762 RID: 5986 RVA: 0x00011790 File Offset: 0x0000F990
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	private void RPCFinishGame(NetPlayer sender, bool won, bool tie)
	{
		this.FinishGame(won, tie);
	}

	// Token: 0x06001763 RID: 5987 RVA: 0x000A265C File Offset: 0x000A085C
	private void StartGameWithCountdown(byte id, float time)
	{
		float num = this.m_gameStartCountdownLength;
		if (base.IsOwner)
		{
			base.SendRPC("RPCStartGameWithCountdown", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				this.GameID,
				time
			});
		}
		else
		{
			num -= NetSystem.NetTime.GameTime - time;
		}
		this.ChallengeItemGameState = ChallengeItemState.GameCountdown;
		this.m_arcade.m_scoreboardWindow.SetActive(true);
		this.m_arcade.m_gameReadyWindow.SetActive(false);
		this.m_arcade.m_gameScenes[(int)id].SetActive(true);
		RectTransform[] playerObjects = this.m_arcade.m_PlayerObjects;
		for (int i = 0; i < playerObjects.Length; i++)
		{
			playerObjects[i].gameObject.SetActive(true);
		}
		this.m_gameStartTime = Time.time + num;
	}

	// Token: 0x06001764 RID: 5988 RVA: 0x0001179A File Offset: 0x0000F99A
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	private void RPCStartGameWithCountdown(NetPlayer sender, byte id, float time)
	{
		this.StartGameWithCountdown(id, time);
	}

	// Token: 0x06001765 RID: 5989 RVA: 0x000A2724 File Offset: 0x000A0924
	private void SelectGame(byte id)
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCSelectGame", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				this.GameID
			});
		}
		this.ChallengeItemGameState = ChallengeItemState.WaitingForReady;
		this.m_readyWaitStartTime = Time.time;
		this.m_arcade.m_gameSelectWindow.SetActive(false);
		this.m_arcade.m_gameReadyWindow.SetActive(true);
		this.m_curGame = this.m_games[(int)id];
		this.m_arcade.m_PlayerObjects[0].anchoredPosition = this.player1StartPositions[(int)id];
		this.m_arcade.m_PlayerObjects[1].anchoredPosition = this.player2StartPositions[(int)id];
		if (NetSystem.IsServer && id == 1)
		{
			this.ball = NetSystem.Spawn("ArcadePaddleBall", 0, NetSystem.MyPlayer).GetComponent<ArcadePaddleBall>();
		}
	}

	// Token: 0x06001766 RID: 5990 RVA: 0x000117A4 File Offset: 0x0000F9A4
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	private void RPCSelectGame(NetPlayer sender, byte id)
	{
		this.SelectGame(id);
	}

	// Token: 0x06001767 RID: 5991 RVA: 0x000A2800 File Offset: 0x000A0A00
	private byte Increment(bool left, byte curID)
	{
		if (left)
		{
			curID = ((curID == 0) ? ((byte)(GameManager.Board.GetActorCount() - 1)) : (curID - 1));
		}
		else
		{
			curID = (((int)curID == GameManager.Board.GetActorCount() - 1) ? 0 : (curID + 1));
		}
		if ((short)curID == this.player.GlobalID || GameManager.Board.GetActor(curID).LocalHealth <= 0 || GameManager.Board.GetActor(curID).GetType() != typeof(BoardPlayer))
		{
			return this.Increment(left, curID);
		}
		return curID;
	}

	// Token: 0x06001768 RID: 5992 RVA: 0x000A2890 File Offset: 0x000A0A90
	protected override void Use(int seed)
	{
		base.Use(seed);
		byte b = (byte)this.rand.Next(9, 12);
		float num = ZPMath.RandomFloat(this.rand, 15f, 165f);
		byte b2 = (byte)GameManager.rand.Next(0, 2);
		base.SendRPC("RPCUseChallengeItem", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			this.TargetID,
			b,
			num,
			b2
		});
		base.StartCoroutine(this.UseChallengeItem(this.TargetID, b, num, b2));
	}

	// Token: 0x06001769 RID: 5993 RVA: 0x000117AD File Offset: 0x0000F9AD
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCUseChallengeItem(NetPlayer sender, byte playerID, byte damage, float yRot, byte gameID)
	{
		base.Use(0);
		base.StartCoroutine(this.UseChallengeItem(playerID, damage, yRot, gameID));
	}

	// Token: 0x0600176A RID: 5994 RVA: 0x000117C9 File Offset: 0x0000F9C9
	private IEnumerator UseChallengeItem(byte playerID, byte damage, float yRot, byte gameID)
	{
		ChallengeItem.Instance = this;
		NetSystem.SetSendRate(60, 60);
		this.challengeTarget = (BoardPlayer)GameManager.Board.GetActor(playerID);
		if (NetSystem.IsServer)
		{
			this.p1 = NetSystem.Spawn("ChallengeItemPlayer", this.player.BoardObject.OwnerSlot, this.player.BoardObject.Owner).GetComponent<ChallengeItemPlayer>();
			this.p1.arcadeSlot.Value = 0;
			this.p2 = NetSystem.Spawn("ChallengeItemPlayer", this.challengeTarget.OwnerSlot, this.challengeTarget.Owner).GetComponent<ChallengeItemPlayer>();
			this.p2.arcadeSlot.Value = 1;
		}
		this.m_arcade.m_PlayerNames[0].text = this.player.Name;
		this.m_arcade.m_PlayerNames[1].text = this.challengeTarget.GamePlayer.Name;
		this.m_arcade.m_readyWindowPlayerNameTxt[0].text = this.player.Name;
		this.m_arcade.m_readyWindowPlayerNameTxt[1].text = this.challengeTarget.GamePlayer.Name;
		this.renderTexture = new RenderTexture(1330, 1024, 0, RenderTextureFormat.ARGB32);
		this.renderTexture.filterMode = FilterMode.Bilinear;
		this.m_arcade.m_screenCamera.enabled = true;
		this.m_arcade.m_screenCamera.targetTexture = this.renderTexture;
		this.m_ScreenMaterial.mainTexture = this.renderTexture;
		this.m_ScreenMaterial.SetTexture("_EmissionMap", this.renderTexture);
		Material[] sharedMaterials = this.m_arcade.m_screenMeshRenderer.sharedMaterials;
		sharedMaterials[1] = this.m_ScreenMaterial;
		this.m_arcade.m_screenMeshRenderer.sharedMaterials = sharedMaterials;
		this.m_ui.Despawn();
		UnityEngine.Object.Destroy(this.m_challengeUIObj, 3f);
		LeanTween.move(this.m_arcade.gameObject, this.m_arcade.transform.position + new Vector3(0f, 3f, 0f), 0.5f).setEase(LeanTweenType.easeInOutSine);
		LeanTween.rotate(this.m_arcade.gameObject, new Vector3(0f, 180f, 0f), 0.5f).setEase(LeanTweenType.easeInOutSine);
		yield return new WaitForSeconds(0.5f);
		GameManager.Board.boardCamera.enabled = false;
		this.m_nearClip = GameManager.Board.boardCamera.Cam.nearClipPlane;
		GameManager.Board.boardCamera.Cam.nearClipPlane = 0.1f;
		LeanTween.move(GameManager.Board.boardCamera.gameObject, this.m_arcade.m_cameraTransform.position, 1f).setEase(LeanTweenType.easeInOutSine);
		LeanTween.rotate(GameManager.Board.boardCamera.gameObject, this.m_arcade.m_cameraTransform.rotation.eulerAngles, 1f).setEase(LeanTweenType.easeInOutSine);
		yield return new WaitForSeconds(1f);
		this.m_arcade.m_postProcessVolume.enabled = true;
		DepthOfField dof;
		this.m_arcade.m_postProcessVolume.profile.TryGetSettings<DepthOfField>(out dof);
		LeanTween.value(20f, 10f, 0.5f).setEase(LeanTweenType.easeInOutSine).setOnUpdate(delegate(float val)
		{
			if (dof != null)
			{
				dof.aperture.value = val;
			}
		});
		yield return new WaitForSeconds(0.5f);
		this.DespawnTargeter();
		this.ChallengeItemGameState = ChallengeItemState.ChooseGame;
		this.m_arcade.m_gameSelectWindow.SetActive(true);
		this.m_arcade.SetSelectedGame(0);
		if (base.IsOwner && this.player.IsAI)
		{
			this.GameID = gameID;
		}
		this.UseTimeStart = Time.time;
		yield break;
	}

	// Token: 0x0600176B RID: 5995 RVA: 0x000A292C File Offset: 0x000A0B2C
	public override void Unequip(bool endingTurn)
	{
		this.DespawnTargeter();
		if (this.renderTexture != null)
		{
			this.m_arcade.m_screenCamera.enabled = false;
			this.m_arcade.m_screenCamera.targetTexture = null;
			this.renderTexture.Release();
			UnityEngine.Object.Destroy(this.renderTexture);
			GameManager.Board.boardCamera.Cam.nearClipPlane = this.m_nearClip;
		}
		GameManager.Board.boardCamera.enabled = true;
		GameManager.Board.CameraTrackCurrentPlayer();
		if (this.m_ui != null)
		{
			this.m_ui.Despawn();
		}
		this.m_arcade.Despawn();
		if (this.m_challengeUIObj != null)
		{
			UnityEngine.Object.Destroy(this.m_challengeUIObj, 3f);
		}
		UnityEngine.Object.Destroy(this.m_arcadeObj);
		GameManager.Board.PlayBoardMusic();
		NetSystem.SetSendRate(30, 30);
		base.Unequip(endingTurn);
	}

	// Token: 0x0600176C RID: 5996 RVA: 0x000A2A20 File Offset: 0x000A0C20
	private void DespawnTargeter()
	{
		if (this.m_targeterObj == null)
		{
			return;
		}
		UnityEngine.Object.Destroy(this.m_targeterObj.GetComponent<Fade>());
		Fade fade = this.m_targeterObj.AddComponent<Fade>();
		fade.type = LlockhamIndustries.Decals.FadeType.Scale;
		fade.wrapMode = FadeWrapMode.Once;
		fade.fadeLength = 0.5f;
		fade.fade = this.targetDespawnCurve;
	}

	// Token: 0x0600176D RID: 5997 RVA: 0x000117E7 File Offset: 0x0000F9E7
	public void TargetReceive(object val)
	{
		this.TargetID = (byte)val;
	}

	// Token: 0x0600176E RID: 5998 RVA: 0x000117F5 File Offset: 0x0000F9F5
	public void GameReceive(object val)
	{
		this.GameID = (byte)val;
	}

	// Token: 0x0600176F RID: 5999 RVA: 0x000A2A7C File Offset: 0x000A0C7C
	public override ItemAIUse GetTarget(BoardPlayer user)
	{
		ItemAIUse result = null;
		int num = int.MaxValue;
		for (int i = 0; i < GameManager.Board.GetActorCount(); i++)
		{
			BoardActor actor = GameManager.Board.GetActor((byte)i);
			if (!(actor == user) && actor.LocalHealth > 0 && !(actor.GetType() != typeof(BoardPlayer)))
			{
				short num2 = actor.LocalHealth;
				if (actor.GetType() == typeof(BoardPlayer) && user.GamePlayer.IsAI && !((BoardPlayer)actor).GamePlayer.IsAI)
				{
					num2 += this.difficultyDistanceChange[(int)user.GamePlayer.Difficulty];
				}
				if ((int)num2 < num)
				{
					num = (int)actor.LocalHealth;
					result = new ItemAIUse(actor, 1f);
				}
			}
		}
		return result;
	}

	// Token: 0x040018B4 RID: 6324
	[SerializeField]
	private GameObject m_challengeItemUIPrefab;

	// Token: 0x040018B5 RID: 6325
	[SerializeField]
	private GameObject m_arcadePrefab;

	// Token: 0x040018B6 RID: 6326
	[SerializeField]
	private GameObject m_targeterPrefab;

	// Token: 0x040018B7 RID: 6327
	[SerializeField]
	private AnimationCurve targetDespawnCurve;

	// Token: 0x040018B8 RID: 6328
	[SerializeField]
	private AnimationCurve targetSpawnCurve;

	// Token: 0x040018B9 RID: 6329
	[SerializeField]
	private Material m_ScreenMaterial;

	// Token: 0x040018BA RID: 6330
	[SerializeField]
	private ChallengeItemGameDetails[] m_games;

	// Token: 0x040018BB RID: 6331
	[SerializeField]
	private AudioClip m_music;

	// Token: 0x040018BC RID: 6332
	[SerializeField]
	private AudioClip m_endBell;

	// Token: 0x040018BD RID: 6333
	[SerializeField]
	private GameObject[] m_vehiclePfbs;

	// Token: 0x040018BE RID: 6334
	[SerializeField]
	private AudioClip gobletFanfare;

	// Token: 0x040018BF RID: 6335
	private GameObject m_targeterObj;

	// Token: 0x040018C0 RID: 6336
	private byte useTarget;

	// Token: 0x040018C1 RID: 6337
	private byte useDamage;

	// Token: 0x040018C2 RID: 6338
	private ActionTimer aiTimer = new ActionTimer(0.5f);

	// Token: 0x040018C3 RID: 6339
	private bool foundTarget;

	// Token: 0x040018C4 RID: 6340
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<byte> targetID = new NetVar<byte>(0);

	// Token: 0x040018C5 RID: 6341
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<byte> gameID = new NetVar<byte>(0);

	// Token: 0x040018C6 RID: 6342
	private GameObject m_arcadeObj;

	// Token: 0x040018C7 RID: 6343
	[HideInInspector]
	public ArcadeMachineHelper m_arcade;

	// Token: 0x040018C8 RID: 6344
	private GameObject m_challengeUIObj;

	// Token: 0x040018C9 RID: 6345
	private ChallengeItemUI m_ui;

	// Token: 0x040018CA RID: 6346
	private Vector3 m_targetOffset = new Vector3(0f, 0.75f, 0f);

	// Token: 0x040018CB RID: 6347
	private int m_numGames = 2;

	// Token: 0x040018CC RID: 6348
	private int m_numReadyPlayers;

	// Token: 0x040018CD RID: 6349
	private float m_readyWaitStartTime;

	// Token: 0x040018CE RID: 6350
	private float m_readyWaitTime = 5f;

	// Token: 0x040018CF RID: 6351
	private float m_gameStartTime;

	// Token: 0x040018D0 RID: 6352
	private float m_gameStartCountdownLength = 3f;

	// Token: 0x040018D1 RID: 6353
	private float m_gameEndTime;

	// Token: 0x040018D2 RID: 6354
	private ChallengeItemGameDetails m_curGame;

	// Token: 0x040018D3 RID: 6355
	public List<ArcadeVehicle> m_spawnedVehicles = new List<ArcadeVehicle>();

	// Token: 0x040018D5 RID: 6357
	public Vector2[] player1StartPositions = new Vector2[]
	{
		new Vector2(-50f, -470f),
		new Vector2(-615f, -124f)
	};

	// Token: 0x040018D6 RID: 6358
	public Vector2[] player2StartPositions = new Vector2[]
	{
		new Vector2(50f, -470f),
		new Vector2(615f, -124f)
	};

	// Token: 0x040018D7 RID: 6359
	private Color m_lightStartColor;

	// Token: 0x040018D8 RID: 6360
	private float m_lightStartIntensity;

	// Token: 0x040018D9 RID: 6361
	private Color m_ambStartSkyColor;

	// Token: 0x040018DA RID: 6362
	private Color m_ambStartEquatorColor;

	// Token: 0x040018DB RID: 6363
	private Color m_ambStartGroundColor;

	// Token: 0x040018DC RID: 6364
	private float m_startAmbientIntensity;

	// Token: 0x040018DD RID: 6365
	private float m_startReflectionIntensity;

	// Token: 0x040018DE RID: 6366
	private Light m_sun;

	// Token: 0x040018DF RID: 6367
	private GameBoardCamera cam;

	// Token: 0x040018E0 RID: 6368
	private bool m_laserDone;

	// Token: 0x040018E1 RID: 6369
	private RenderTexture renderTexture;

	// Token: 0x040018E2 RID: 6370
	public static ChallengeItem Instance;

	// Token: 0x040018E3 RID: 6371
	public ChallengeItemPlayer p1;

	// Token: 0x040018E4 RID: 6372
	public ChallengeItemPlayer p2;

	// Token: 0x040018E5 RID: 6373
	private ArcadePaddleBall ball;

	// Token: 0x040018E6 RID: 6374
	private float m_nearClip;

	// Token: 0x040018E7 RID: 6375
	private BoardPlayer challengeTarget;

	// Token: 0x040018E8 RID: 6376
	private float UseTimeStart;

	// Token: 0x040018E9 RID: 6377
	private short[] difficultyDistanceChange = new short[]
	{
		12,
		6,
		3
	};
}
