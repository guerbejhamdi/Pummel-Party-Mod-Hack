using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000325 RID: 805
public class RocketSkewerItem : Item
{
	// Token: 0x060015FA RID: 5626 RVA: 0x00003C5A File Offset: 0x00001E5A
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
	}

	// Token: 0x060015FB RID: 5627 RVA: 0x0009D190 File Offset: 0x0009B390
	public override void Setup()
	{
		base.Setup();
		this.rocketRideObject = UnityEngine.Object.Instantiate<GameObject>(this.rocketRidePrefab, this.player.BoardObject.transform.position + new Vector3(0f, 0.75f, 0f), Quaternion.identity);
		this.rocketRideVisual = this.rocketRideObject.GetComponent<RocketRideVisual>();
		this.rocketRideVisual.rocketSkewerItem = this;
		this.StartMove();
		float t = 2f / this.moveSpline.SplineLength;
		Vector3 a = Vector3.zero;
		Vector3 zero = Vector3.zero;
		this.moveSpline.EvaluateSpline(t, ref zero, ref a);
		a.y = 0f;
		a = -a.normalized;
		this.rocketRideObject.transform.rotation = Quaternion.LookRotation(-a);
		this.rocketRideObject.transform.position += -(this.rocketRideObject.transform.right * 1f);
		this.moveSpline.points[0] = this.rocketRideObject.transform.position - new Vector3(0f, 0.75f, 0f);
		this.moveSpline.CalculateSpline(0.3f);
		this.moveSpline.EvaluateSpline(t, ref zero, ref a);
		a.y = 0f;
		a = -a.normalized;
		this.rocketRideObject.transform.rotation = Quaternion.LookRotation(-a);
		this.player.BoardObject.PlayerAnimation.SetPlayerRotation(this.rocketRideObject.transform.rotation.eulerAngles.y);
		this.splineLength = this.moveSpline.SplineLength;
		Debug.Log("SplineLength: " + this.splineLength.ToString());
		base.StartCoroutine(this.StartWait());
	}

	// Token: 0x060015FC RID: 5628 RVA: 0x00010A48 File Offset: 0x0000EC48
	private IEnumerator StartWait()
	{
		yield return new WaitForSeconds(0.7f);
		this.rocketState = RocketSkewerItem.RocketSkewerItemState.WaitingForUse;
		base.SetNetworkState(Item.ItemState.Setup);
		yield break;
	}

	// Token: 0x060015FD RID: 5629 RVA: 0x0009D3A0 File Offset: 0x0009B5A0
	public override void Update()
	{
		if (this.rocketState == RocketSkewerItem.RocketSkewerItemState.Dropping)
		{
			return;
		}
		if (this.rocketState == RocketSkewerItem.RocketSkewerItemState.WaitingForUse)
		{
			if (base.IsOwner && this.player.IsAI && this.curState == Item.ItemState.Aiming && base.AIUseItem())
			{
				this.rocketState = RocketSkewerItem.RocketSkewerItemState.Lighting;
			}
			base.Update();
			return;
		}
		if (this.rocketState != RocketSkewerItem.RocketSkewerItemState.Lighting)
		{
			base.Update();
			Item.ItemState curState = this.curState;
			if (curState != Item.ItemState.Aiming && curState == Item.ItemState.Using)
			{
				this.curMoveCounter += Time.deltaTime;
				if (this.targetNode != null)
				{
					if (!this.moveSpline.StepSpline(this.moveVelocity * Time.deltaTime))
					{
						this.moveVelocity = Mathf.MoveTowards(this.moveVelocity, this.moveSpeed, this.acceleration * Time.deltaTime);
						Vector3 vector = Vector3.zero;
						Vector3 zero = Vector3.zero;
						this.moveSpline.EvaluateSpline(this.moveSpline.CurrentStepT, ref zero, ref vector);
						vector = -vector.normalized;
						this.rocketRideObject.transform.rotation = Quaternion.RotateTowards(this.rocketRideObject.transform.rotation, Quaternion.LookRotation(-vector), Time.deltaTime * 220f);
						float num = 70f * this.moveSpline.CurrentStepT;
						Vector3 b = new Vector3(Mathf.PerlinNoise(num, num), Mathf.PerlinNoise(0f, num), Mathf.PerlinNoise(num, 0f)) * 0.05f;
						this.rocketRideObject.transform.position = zero + new Vector3(0f, 0.75f, 0f) + b;
						this.player.BoardObject.PlayerAnimation.SetPlayerRotation(this.rocketRideObject.transform.rotation.eulerAngles.y);
						this.lean = Mathf.MoveTowards(this.lean, 0f, Time.deltaTime * 100f);
						float num2 = Mathf.DeltaAngle(this.rocketRideObject.transform.rotation.eulerAngles.y, this.lastY);
						this.lean = Mathf.Clamp(this.lean + num2 * 1f, -45f, 45f);
						this.lastY = this.rocketRideObject.transform.rotation.eulerAngles.y;
						Mathf.DeltaAngle(this.lastY, this.rocketRideObject.transform.rotation.eulerAngles.y);
						this.rocketRideObject.transform.Find("Child").localRotation = Quaternion.Euler(0f, 0f, this.lean);
						this.player.BoardObject.MovementTangent = vector;
						this.player.BoardObject.moveVelocity = this.moveVelocity;
						if (this.hits.Count > 0 && this.moveSpline.CurrentStepT >= this.hits[0].hitTime)
						{
							DamageInstance d = new DamageInstance
							{
								damage = this.rand.Next(14, 16),
								origin = this.rocketRideObject.transform.position,
								blood = true,
								ragdoll = true,
								ragdollVel = 0f,
								bloodVel = 13f,
								bloodAmount = 1f,
								sound = true,
								volume = 0.75f,
								details = "Rocket Skewer",
								killer = this.player.BoardObject,
								removeKeys = true
							};
							bool flag = this.hits[0].player.CactusScript != null || this.hits[0].player.PresentScript != null || this.hits[0].player.TwitchMapEvent;
							this.hits[0].player.ApplyDamage(d);
							GameManager.Board.boardCamera.AddShake(0.15f);
							if (!flag)
							{
								Transform item = this.hits[0].player.NewestRagdoll.transform.Find("Character/Armature");
								this.rocketRideVisual.attachedRagdolls.Add(item);
							}
							this.hits.RemoveAt(0);
							this.achievementHitsCount++;
							if (this.player.IsLocalPlayer && !this.player.IsAI && this.achievementHitsCount >= 3)
							{
								PlatformAchievementManager.Instance.TriggerAchievement("ACH_EXTRA_MEAT");
								return;
							}
						}
					}
					else
					{
						this.rocketState = RocketSkewerItem.RocketSkewerItemState.Finished;
						this.targetNode = null;
						this.Reset();
						this.player.BoardObject.moveVelocity = 0f;
						this.player.BoardObject.diceEffect.startPos = this.player.BoardObject.DicePosition();
						this.rocketRideVisual.LiftOff = true;
						this.player.BoardObject.GroundSnap = true;
						this.rocketSoundSource.FadeAudio(2.5f, FadeType.Out);
						base.Finish(false);
					}
				}
			}
		}
	}

	// Token: 0x060015FE RID: 5630 RVA: 0x0009D910 File Offset: 0x0009BB10
	private void StartMove()
	{
		BoardPlayer boardObject = this.player.BoardObject;
		this.curMoveCounter = 0f;
		this.moveVelocity = 12f;
		this.moveSpline.ResetStep();
		this.moveSpline.Clear();
		this.moveSpline.AddPoint(this.rocketRideObject.transform.position - new Vector3(0f, 0.75f, 0f));
		BoardNode boardNode = boardObject.CurrentNode;
		while (this.curMoveSteps > 0)
		{
			List<BoardNode> forwardNodes = boardNode.GetForwardNodes(null, false);
			if (forwardNodes.Count == 1)
			{
				boardNode = forwardNodes[0];
			}
			else
			{
				int index = -1;
				int num = int.MaxValue;
				for (int i = 0; i < forwardNodes.Count; i++)
				{
					int num2 = GameManager.Board.ClosestGoalIndex(null);
					int num3 = GameManager.Board.CurrentMap.DistToNode(forwardNodes[i], GameManager.Board.GoalNode[num2], BoardNodeConnectionDirection.Forward);
					if (num3 < num)
					{
						index = i;
						num = num3;
					}
				}
				boardNode = forwardNodes[index];
			}
			bool flag = false;
			for (int j = 0; j < forwardNodes.Count; j++)
			{
				if (forwardNodes[j].CurrentNodeType == BoardNodeType.Trophy)
				{
					this.curMoveSteps = 0;
					if (this.moveSpline.points.Count == 1)
					{
						this.moveSpline.AddPoint(this.moveSpline.points[this.moveSpline.points.Count - 1]);
						this.targetNode = boardObject.CurrentNode;
					}
					flag = true;
				}
			}
			if (flag)
			{
				break;
			}
			this.moveSpline.AddPoint(boardNode.NodePosition);
			this.targetNode = boardNode;
			if (base.IsOwner && boardNode.LastPlayer != null && boardNode.LastPlayer != this.player.BoardObject)
			{
				this.hits.Add(new RocketSkewerItem.RocketHit
				{
					player = boardNode.LastPlayer
				});
			}
			if (boardNode.baseNodeType != BoardNodeType.Pathing)
			{
				this.curMoveSteps--;
			}
		}
		int k = 0;
		while (k < this.hits.Count)
		{
			if (this.hits[k].player.CurrentNode == this.targetNode)
			{
				this.hits.RemoveAt(k);
			}
			else
			{
				k++;
			}
		}
		this.moveSpline.CalculateSpline(0.3f);
	}

	// Token: 0x060015FF RID: 5631 RVA: 0x0009DB88 File Offset: 0x0009BD88
	protected override void Use(int seed)
	{
		base.Use(seed);
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		byte[] array = new byte[this.hits.Count];
		float[] array2 = new float[this.hits.Count];
		for (int i = 0; i < this.hits.Count; i++)
		{
			float num = float.MaxValue;
			RocketSkewerItem.RocketHit rocketHit = this.hits[i];
			for (float num2 = 0f; num2 < 1f; num2 += 0.01f)
			{
				this.moveSpline.EvaluateSpline(num2, ref zero, ref zero2);
				float sqrMagnitude = (rocketHit.player.CurrentNode.NodePosition - zero).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					rocketHit.hitTime = num2 - 1.2f / this.splineLength;
					num = sqrMagnitude;
				}
			}
			this.hits[i] = rocketHit;
			array[i] = (byte)this.hits[i].player.GamePlayer.GlobalID;
			array2[i] = this.hits[i].hitTime;
		}
		base.SendRPC("RPCUseRocket", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			array,
			array2,
			seed
		});
		base.StartCoroutine(this.UseRocket());
	}

	// Token: 0x06001600 RID: 5632 RVA: 0x0009DCE8 File Offset: 0x0009BEE8
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCUseRocket(NetPlayer sender, byte[] hitPlayerIDs, float[] hitTimes, int seed)
	{
		for (int i = 0; i < hitPlayerIDs.Length; i++)
		{
			this.hits.Add(new RocketSkewerItem.RocketHit(GameManager.GetPlayerAt((int)hitPlayerIDs[i]).BoardObject, hitTimes[i]));
		}
		base.Use(seed);
		this.curState = Item.ItemState.Using;
		base.StartCoroutine(this.UseRocket());
	}

	// Token: 0x06001601 RID: 5633 RVA: 0x00010A57 File Offset: 0x0000EC57
	private IEnumerator UseRocket()
	{
		this.fuseAudioSource = AudioSystem.PlayLooping(this.fuseSound, 0.5f, 0.5f);
		this.rocketRideVisual.anim.Play("RocketLightAnimation");
		this.player.BoardObject.CurrentNode = this.targetNode;
		this.used = true;
		this.player.BoardObject.PlayerAnimation.SetPlayerRotationImmediate(0f);
		this.player.BoardObject.PlayerAnimation.usePlayerRotation = false;
		this.player.BoardObject.transform.SetParent(this.rocketRideObject.transform.Find("Child"), false);
		this.player.BoardObject.transform.localPosition = this.rocketRideVisual.playerTransform.localPosition;
		this.player.BoardObject.GroundSnap = false;
		this.player.BoardObject.PlayerAnimation.Animator.SetBool("HoldingRocket", true);
		yield return new WaitForSeconds(2f);
		this.OnLightAnimEnd2();
		yield break;
	}

	// Token: 0x06001602 RID: 5634 RVA: 0x0009DD40 File Offset: 0x0009BF40
	public override void Unequip(bool endingTurn)
	{
		bool flag = this.used;
		this.player.BoardObject.PlayerAnimation.SetPlayerRotationImmediate(180f);
		UnityEngine.Object.Destroy(this.rocketRideObject, this.used ? 2f : 0f);
		base.Unequip(endingTurn);
	}

	// Token: 0x06001603 RID: 5635 RVA: 0x0009DD94 File Offset: 0x0009BF94
	private void Reset()
	{
		this.player.BoardObject.transform.parent = null;
		this.player.BoardObject.transform.position = this.player.BoardObject.CurrentNode.GetPlayersSlotPosition(this.player.BoardObject);
		this.player.BoardObject.transform.rotation = Quaternion.identity;
		this.player.BoardObject.PlayerAnimation.Animator.SetBool("HoldingRocket", false);
		this.player.BoardObject.PlayerAnimation.Animator.SetBool("RocketActive", false);
		this.player.BoardObject.PlayerAnimation.usePlayerRotation = true;
		this.player.BoardObject.PlayerAnimation.SetPlayerRotationImmediate(180f);
	}

	// Token: 0x06001604 RID: 5636 RVA: 0x0000398C File Offset: 0x00001B8C
	public void OnDropAnimEnd()
	{
	}

	// Token: 0x06001605 RID: 5637 RVA: 0x0000398C File Offset: 0x00001B8C
	public void OnLightAnimEnd()
	{
	}

	// Token: 0x06001606 RID: 5638 RVA: 0x0009DE78 File Offset: 0x0009C078
	private void OnLightAnimEnd2()
	{
		this.rocketState = RocketSkewerItem.RocketSkewerItemState.Riding;
		this.fuseAudioSource.FadeAudio(0.5f, FadeType.Out);
		this.rocketRideVisual.stand.transform.parent = null;
		AudioSystem.PlayOneShot(this.explosion, 0.5f, 0f, 1f);
		UnityEngine.Object.Instantiate<GameObject>(this.explosionPrefab, this.rocketRideVisual.nozeltransform.position, this.rocketRideVisual.nozeltransform.rotation);
		UnityEngine.Object.Destroy(this.rocketRideVisual.stand.gameObject, 2f);
		this.rocketSoundSource = AudioSystem.PlayLooping(this.rocketSound, 0.25f, 2.5f);
		this.lastY = this.rocketRideObject.transform.rotation.eulerAngles.y;
		GameManager.Board.boardCamera.AddShake(0.4f);
		this.rocketRideVisual.Lit = true;
		this.player.BoardObject.PlayerAnimation.Animator.SetBool("RocketActive", true);
		foreach (GameObject gameObject in this.rocketRideVisual.scaffoldDebriObjects)
		{
			gameObject.transform.parent = null;
			gameObject.AddComponent<Rigidbody>().AddExplosionForce(250f, this.rocketRideVisual.scaffoldObject.transform.position, 10f, 1f);
			UnityEngine.Object.Destroy(gameObject, 10f);
		}
		UnityEngine.Object.Destroy(this.rocketRideVisual.scaffoldObject);
	}

	// Token: 0x06001607 RID: 5639 RVA: 0x0009E004 File Offset: 0x0009C204
	public override ItemAIUse GetTarget(BoardPlayer user)
	{
		int num = GameManager.Board.ClosestGoalIndex(null);
		SearchNode path = GameManager.Board.CurrentMap.GetPath(user.CurrentNode, GameManager.Board.GoalNode[num], BoardNodeConnectionDirection.Forward, false);
		Debug.Log("Path Cost: " + path.pathCost.ToString());
		if (path.pathCost <= 2)
		{
			return null;
		}
		return new ItemAIUse(user, 5f);
	}

	// Token: 0x06001608 RID: 5640 RVA: 0x0009E074 File Offset: 0x0009C274
	public void LateUpdate()
	{
		if (this.rocketState == RocketSkewerItem.RocketSkewerItemState.Riding)
		{
			this.player.BoardObject.PlayerAnimation.GetBone(PlayerBone.Spine).rotation *= Quaternion.Euler(0f, 0f, -this.lean / 2f);
		}
	}

	// Token: 0x04001721 RID: 5921
	public GameObject rocketRidePrefab;

	// Token: 0x04001722 RID: 5922
	public float moveSpeed;

	// Token: 0x04001723 RID: 5923
	public float acceleration = 11f;

	// Token: 0x04001724 RID: 5924
	public float rocketLightTime = 2f;

	// Token: 0x04001725 RID: 5925
	public AudioClip fuseSound;

	// Token: 0x04001726 RID: 5926
	public AudioClip explosion;

	// Token: 0x04001727 RID: 5927
	public AudioClip rocketSound;

	// Token: 0x04001728 RID: 5928
	public GameObject explosionPrefab;

	// Token: 0x04001729 RID: 5929
	private GameObject rocketRideObject;

	// Token: 0x0400172A RID: 5930
	private RocketSkewerItem.RocketSkewerItemState rocketState;

	// Token: 0x0400172B RID: 5931
	private RocketRideVisual rocketRideVisual;

	// Token: 0x0400172C RID: 5932
	private TempAudioSource fuseAudioSource;

	// Token: 0x0400172D RID: 5933
	private TempAudioSource rocketSoundSource;

	// Token: 0x0400172E RID: 5934
	private bool used;

	// Token: 0x0400172F RID: 5935
	private float lean;

	// Token: 0x04001730 RID: 5936
	private float lastY;

	// Token: 0x04001731 RID: 5937
	private int curMoveSteps = 10;

	// Token: 0x04001732 RID: 5938
	private float curMoveCounter;

	// Token: 0x04001733 RID: 5939
	private float moveVelocity = 12f;

	// Token: 0x04001734 RID: 5940
	private Spline moveSpline = new Spline();

	// Token: 0x04001735 RID: 5941
	private BoardNode targetNode;

	// Token: 0x04001736 RID: 5942
	private float splineLength;

	// Token: 0x04001737 RID: 5943
	private List<RocketSkewerItem.RocketHit> hits = new List<RocketSkewerItem.RocketHit>();

	// Token: 0x04001738 RID: 5944
	private int achievementHitsCount;

	// Token: 0x02000326 RID: 806
	private struct RocketHit
	{
		// Token: 0x0600160A RID: 5642 RVA: 0x00010A66 File Offset: 0x0000EC66
		public RocketHit(BoardPlayer player, float hitTime)
		{
			this.player = player;
			this.hitTime = hitTime;
		}

		// Token: 0x04001739 RID: 5945
		public BoardPlayer player;

		// Token: 0x0400173A RID: 5946
		public float hitTime;
	}

	// Token: 0x02000327 RID: 807
	private enum RocketSkewerItemState
	{
		// Token: 0x0400173C RID: 5948
		Dropping,
		// Token: 0x0400173D RID: 5949
		WaitingForUse,
		// Token: 0x0400173E RID: 5950
		Lighting,
		// Token: 0x0400173F RID: 5951
		Riding,
		// Token: 0x04001740 RID: 5952
		Finished
	}
}
