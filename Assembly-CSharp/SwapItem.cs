using System;
using System.Collections;
using System.Collections.Generic;
using LlockhamIndustries.Decals;
using UnityEngine;
using ZP.Net;

// Token: 0x020004C6 RID: 1222
public class SwapItem : Item
{
	// Token: 0x06002070 RID: 8304 RVA: 0x00003C5A File Offset: 0x00001E5A
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
	}

	// Token: 0x06002071 RID: 8305 RVA: 0x000CB09C File Offset: 0x000C929C
	public override void Setup()
	{
		base.Setup();
		if (!base.IsOwner)
		{
			this.cameraPosition.Recieve = new RecieveProxy(this.RecieveCamPosition);
			this.interpolator = new Interpolator(Interpolator.InterpolationType.Position);
		}
		else
		{
			this.cameraPosition.Value = this.player.BoardObject.MidPoint;
		}
		base.GetAITarget();
		this.boardCam = GameManager.GetCamera();
		this.projector = UnityEngine.Object.Instantiate<GameObject>(this.projectorPrefab, this.GetTargetPoint().point, Quaternion.Euler(90f, 0f, 0f));
		this.originalCameraAngle = this.boardCam.targetCameraAngle;
		this.originalCamDistScale = this.boardCam.targetDistScale;
		this.boardCam.MoveToInstant(this.player.BoardObject.MidPoint, Vector3.zero);
		this.boardCam.targetDistScale = 0.95f;
		this.boardCam.targetCameraAngle = 45f;
		base.SetNetworkState(Item.ItemState.Setup);
		this.AITimeoutTimer.Start();
	}

	// Token: 0x06002072 RID: 8306 RVA: 0x000CB1B0 File Offset: 0x000C93B0
	public override void Update()
	{
		base.Update();
		if (this.curState == Item.ItemState.Aiming)
		{
			if (base.IsOwner)
			{
				float d = 13f;
				Vector3 a = Vector3.zero;
				if (this.player.IsAI)
				{
					if (!this.waitingToUse)
					{
						Vector3 vector = this.AITarget.transform.position - this.projector.transform.position;
						vector.y = 0f;
						if (vector.magnitude < 0.5f)
						{
							this.waitingToUse = true;
							this.AIUseWaitTimer.Start();
						}
						else if (this.AITimeoutTimer.Elapsed(true))
						{
							Vector3 position = this.AITarget.transform.position;
							this.cameraPosition.Value = position;
							base.AIUseItem();
						}
						else
						{
							a = vector.normalized;
						}
					}
				}
				else if (!GameManager.IsGamePaused)
				{
					a = new Vector3(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), 0f, this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
				}
				float d2 = Mathf.Clamp(a.magnitude, -1f, 1f);
				a = a.normalized * d2;
				Vector3 vector2 = this.cameraPosition.Value;
				vector2 += a * d * Time.deltaTime;
				Rect mapExtents = GameManager.Board.CurrentMap.mapExtents;
				vector2.x = Mathf.Clamp(vector2.x, mapExtents.xMin, mapExtents.xMax);
				vector2.z = Mathf.Clamp(vector2.z, mapExtents.yMin, mapExtents.yMax);
				this.cameraPosition.Value = vector2;
				this.boardCam.MoveToInstant(this.cameraPosition.Value, Vector3.zero);
				if (this.waitingToUse && this.AIUseWaitTimer.Elapsed(true))
				{
					base.AIUseItem();
				}
			}
			else
			{
				this.interpolator.Update();
				this.boardCam.MoveToInstant(this.interpolator.CurrentPosition, Vector3.zero);
			}
			Vector3 point = this.GetTargetPoint().point;
			this.projector.transform.position = point;
			Vector3 normalized = (point - this.player.BoardObject.transform.position).normalized;
			this.player.BoardObject.PlayerAnimation.SetPlayerRotation(Quaternion.LookRotation(normalized).eulerAngles.y);
		}
	}

	// Token: 0x06002073 RID: 8307 RVA: 0x000CB450 File Offset: 0x000C9650
	private RaycastHit GetTargetPoint()
	{
		Camera cam = this.boardCam.Cam;
		Ray ray = cam.ScreenPointToRay(new Vector3((float)(cam.pixelWidth / 2), (float)(cam.pixelHeight / 2), 0f));
		RaycastHit result;
		if (!Physics.Raycast(ray, out result, 100f, 3072))
		{
			float d = cam.transform.position.y / ray.direction.y;
			result.point = ray.origin + ray.direction * d;
		}
		return result;
	}

	// Token: 0x06002074 RID: 8308 RVA: 0x000CB4E0 File Offset: 0x000C96E0
	protected override void Use(int seed)
	{
		List<byte> list = new List<byte>();
		RaycastHit targetPoint = this.GetTargetPoint();
		float num = 1.5f;
		float num2 = num * num;
		for (int i = 0; i < GameManager.GetPlayerCount(); i++)
		{
			BoardPlayer boardObject = GameManager.GetPlayerAt(i).BoardObject;
			if (boardObject != this.player.BoardObject && !boardObject.TwitchMapEvent)
			{
				Vector3 vector = boardObject.transform.position - targetPoint.point;
				vector.y = 0f;
				if (vector.sqrMagnitude < num2)
				{
					list.Add((byte)i);
				}
			}
		}
		if (list.Count > 0 || this.player.IsAI)
		{
			base.Use(seed);
			base.StartCoroutine(this.StartEvent(list.ToArray()));
			base.SendRPC("RPCStartEvent", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				list.ToArray()
			});
			return;
		}
		AudioSystem.PlayOneShot(this.errorSoundClip, 1f, 0f, 1f);
	}

	// Token: 0x06002075 RID: 8309 RVA: 0x00017A93 File Offset: 0x00015C93
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCStartEvent(NetPlayer sender, byte[] hitPlayerIDs)
	{
		base.StartCoroutine(this.StartEvent(hitPlayerIDs));
	}

	// Token: 0x06002076 RID: 8310 RVA: 0x00017AA3 File Offset: 0x00015CA3
	public IEnumerator StartEvent(byte[] hitPlayerIDs)
	{
		if (!base.IsOwner)
		{
			base.Use(0);
		}
		float portalSpawnWaitTime = 0.1f;
		List<BoardPlayer> hitPlayers = new List<BoardPlayer>();
		for (int j = 0; j < hitPlayerIDs.Length; j++)
		{
			hitPlayers.Add(GameManager.GetPlayerAt((int)hitPlayerIDs[j]).BoardObject);
		}
		this.DespawnTargeter();
		RaycastHit targetPoint = this.GetTargetPoint();
		Vector3 point = targetPoint.point;
		Vector3 normal = targetPoint.normal;
		PortalEffect portal = null;
		if (hitPlayers.Count != 0)
		{
			portal = PortalEffect.Spawn(point, normal, PortalOrientation.Horizontal, hitPlayers[0].gameObject, true);
		}
		yield return new WaitForSeconds(portalSpawnWaitTime);
		int k = 0;
		while (k < hitPlayers.Count)
		{
			if (hitPlayers[k].CactusScript != null)
			{
				hitPlayers[k].RemoveCactus(hitPlayers[k].transform.position, 5f);
				hitPlayers.RemoveAt(k);
			}
			else
			{
				k++;
			}
		}
		if (hitPlayers.Count != 0)
		{
			yield return base.StartCoroutine(this.SinkPlayers(hitPlayers));
			yield return new WaitForSeconds(0.375f);
		}
		if (portal != null)
		{
			portal.Release(true);
		}
		yield return new WaitForSeconds(portalSpawnWaitTime);
		if (hitPlayers.Count != 0)
		{
			this.boardCam.MoveTo(this.player.BoardObject.transform.position + GameManager.Board.PlayerCamOffset);
			yield return new WaitUntil(() => this.boardCam.WithinDistance(0.5f));
			Physics.Raycast(this.player.BoardObject.MidPoint, Vector3.down, out targetPoint, 5f, 3072);
			portal = PortalEffect.Spawn(targetPoint.point, targetPoint.normal, PortalOrientation.Horizontal, this.player.BoardObject.gameObject, true);
			yield return new WaitForSeconds(portalSpawnWaitTime);
			BoardPlayerState preState = this.player.BoardObject.PlayerState;
			yield return base.StartCoroutine(this.SinkPlayers(new List<BoardPlayer>
			{
				this.player.BoardObject
			}));
			yield return new WaitForSeconds(0.375f);
			if (portal != null)
			{
				portal.Release(true);
			}
			yield return new WaitForSeconds(portalSpawnWaitTime);
			yield return new WaitForSeconds(0.1f);
			BoardNode targetsNode = hitPlayers[0].CurrentNode;
			BoardNode currentNode = this.player.BoardObject.CurrentNode;
			this.player.BoardObject.CurrentNode.LeaveNode(this.player.BoardObject);
			for (int l = 0; l < hitPlayers.Count; l++)
			{
				hitPlayers[l].CurrentNode = currentNode;
			}
			this.player.BoardObject.CurrentNode = targetsNode;
			float portalHeight = 5f;
			PortalEffect[] portals = new PortalEffect[hitPlayers.Count];
			int num;
			for (int i = 0; i < hitPlayers.Count; i = num)
			{
				Vector3 position = hitPlayers[i].CurrentNode.GetPlayersSlotPosition(hitPlayers[i]) + Vector3.up * portalHeight;
				portals[i] = PortalEffect.Spawn(position, Vector3.down, PortalOrientation.Horizontal, hitPlayers[i].gameObject, true);
				yield return new WaitForSeconds(portalSpawnWaitTime);
				num = i + 1;
			}
			Coroutine[] fallCoroutines = new Coroutine[hitPlayers.Count];
			for (int m = 0; m < hitPlayers.Count; m++)
			{
				fallCoroutines[m] = base.StartCoroutine(hitPlayers[m].StartRagdoll(0f, 24f, false, 0.2f));
			}
			for (int i = 0; i < hitPlayers.Count; i = num)
			{
				yield return fallCoroutines[i];
				num = i + 1;
			}
			for (int i = 0; i < portals.Length; i = num)
			{
				if (portals[i] != null)
				{
					portals[i].Release(true);
				}
				yield return new WaitForSeconds(portalSpawnWaitTime);
				num = i + 1;
			}
			Vector3 pos = targetsNode.GetPlayersSlotPosition(this.player.BoardObject) + GameManager.Board.PlayerCamOffset;
			this.boardCam.MoveTo(pos);
			yield return new WaitUntil(() => this.boardCam.WithinDistance(0.5f));
			Vector3 position2 = this.player.BoardObject.CurrentNode.GetPlayersSlotPosition(this.player.BoardObject) + Vector3.up * portalHeight;
			portal = PortalEffect.Spawn(position2, Vector3.down, PortalOrientation.Horizontal, this.player.BoardObject.gameObject, true);
			yield return new WaitForSeconds(portalSpawnWaitTime);
			yield return base.StartCoroutine(this.player.BoardObject.StartRagdoll(0f, 24f, false, 0.2f));
			this.player.BoardObject.SetState(preState);
			this.player.BoardObject.diceEffect.startPos = this.player.BoardObject.DicePosition();
			if (portal != null)
			{
				portal.Release(true);
			}
			yield return new WaitForSeconds(portalSpawnWaitTime);
			targetsNode = null;
			portals = null;
			fallCoroutines = null;
		}
		base.Finish(false);
		yield break;
	}

	// Token: 0x06002077 RID: 8311 RVA: 0x00017AB9 File Offset: 0x00015CB9
	private IEnumerator SinkPlayers(List<BoardPlayer> players)
	{
		for (int i = 0; i < players.Count; i++)
		{
			players[i].SetState(BoardPlayerState.Ragdolling);
			players[i].PlayerAnimation.Grounded = false;
			players[i].PlayerAnimation.FireFallingTrigger();
		}
		float fallSpeed = 14f;
		float dropDistance = 3f;
		float droppedDistance = 0f;
		for (;;)
		{
			float num = fallSpeed * Time.deltaTime;
			droppedDistance += num;
			for (int j = 0; j < players.Count; j++)
			{
				players[j].transform.position -= new Vector3(0f, num, 0f);
			}
			if (droppedDistance >= dropDistance)
			{
				break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002078 RID: 8312 RVA: 0x000CB5DC File Offset: 0x000C97DC
	private void DespawnTargeter()
	{
		if (this.projector == null)
		{
			return;
		}
		Fade componentInChildren = this.projector.GetComponentInChildren<Fade>();
		Fade fade = componentInChildren.gameObject.AddComponent<Fade>();
		UnityEngine.Object.Destroy(componentInChildren);
		fade.type = LlockhamIndustries.Decals.FadeType.Scale;
		fade.wrapMode = FadeWrapMode.Once;
		fade.fadeLength = 0.5f;
		fade.fade = this.targetDespawnCurve;
	}

	// Token: 0x06002079 RID: 8313 RVA: 0x00017AC8 File Offset: 0x00015CC8
	public void RecieveCamPosition(object _pos)
	{
		this.interpolator.NewPosition(_pos);
	}

	// Token: 0x0600207A RID: 8314 RVA: 0x000CB63C File Offset: 0x000C983C
	public override void Unequip(bool endingTurn)
	{
		this.player.BoardObject.PlayerAnimation.SetPlayerRotation(180f);
		this.DespawnTargeter();
		this.boardCam.targetDistScale = this.originalCamDistScale;
		this.boardCam.targetCameraAngle = this.originalCameraAngle;
		this.boardCam.SetTrackedObject(this.player.BoardObject.transform, GameManager.Board.PlayerCamOffset);
		base.Unequip(endingTurn);
	}

	// Token: 0x0600207B RID: 8315 RVA: 0x000CB6B8 File Offset: 0x000C98B8
	public override ItemAIUse GetTarget(BoardPlayer user)
	{
		int num = GameManager.Board.ClosestGoalIndex(user);
		float num2 = 10f;
		float num3 = 2f;
		float num4 = (float)GameManager.Board.CurrentMap.DistToNode(user.CurrentNode, GameManager.Board.GoalNode[num], BoardNodeConnectionDirection.Forward);
		ItemAIUse itemAIUse = new ItemAIUse();
		float num5 = 2.1474836E+09f;
		for (int i = 0; i < GameManager.GetPlayerCount(); i++)
		{
			BoardPlayer boardObject = GameManager.GetPlayerAt(i).BoardObject;
			if (!(boardObject == user))
			{
				int num6 = GameManager.Board.ClosestGoalIndex(boardObject);
				float num7 = (float)GameManager.Board.CurrentMap.DistToNode(boardObject.CurrentNode, GameManager.Board.GoalNode[num6], BoardNodeConnectionDirection.Forward);
				if (user.GamePlayer.IsAI && !boardObject.GamePlayer.IsAI)
				{
					num7 += (float)this.difficultyDistanceChange[(int)user.GamePlayer.Difficulty];
				}
				if (num5 != 0f && num7 < num2 && num7 < num4 - num3 && num7 < num5)
				{
					itemAIUse.player = boardObject;
					num5 = num7;
				}
			}
		}
		if (itemAIUse.player != null)
		{
			itemAIUse.priority = 1.5f * ((num2 - num5) / num2);
			return itemAIUse;
		}
		return null;
	}

	// Token: 0x0600207C RID: 8316 RVA: 0x000CB7FC File Offset: 0x000C99FC
	public SwapItem()
	{
		int[] array = new int[3];
		array[0] = 4;
		array[1] = 2;
		this.difficultyDistanceChange = array;
		base..ctor();
	}

	// Token: 0x04002334 RID: 9012
	[Header("Swap Item Settings")]
	public GameObject projectorPrefab;

	// Token: 0x04002335 RID: 9013
	public AnimationCurve targetDespawnCurve;

	// Token: 0x04002336 RID: 9014
	public Material portalClipMat;

	// Token: 0x04002337 RID: 9015
	public AudioClip errorSoundClip;

	// Token: 0x04002338 RID: 9016
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	private NetVec3 cameraPosition = new NetVec3();

	// Token: 0x04002339 RID: 9017
	private Interpolator interpolator;

	// Token: 0x0400233A RID: 9018
	private GameObject projector;

	// Token: 0x0400233B RID: 9019
	private GameBoardCamera boardCam;

	// Token: 0x0400233C RID: 9020
	private float originalCameraAngle;

	// Token: 0x0400233D RID: 9021
	private float originalCamDistScale;

	// Token: 0x0400233E RID: 9022
	private bool waitingToUse;

	// Token: 0x0400233F RID: 9023
	private ActionTimer AIUseWaitTimer = new ActionTimer(0.35f, 0.6f);

	// Token: 0x04002340 RID: 9024
	private ActionTimer AITimeoutTimer = new ActionTimer(10f);

	// Token: 0x04002341 RID: 9025
	private int[] difficultyDistanceChange;
}
