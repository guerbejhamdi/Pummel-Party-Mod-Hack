using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZP.Utility;

// Token: 0x0200038A RID: 906
public class SummonItemPersistent : PersistentItem
{
	// Token: 0x1700025C RID: 604
	// (get) Token: 0x0600186C RID: 6252 RVA: 0x000121B1 File Offset: 0x000103B1
	// (set) Token: 0x0600186D RID: 6253 RVA: 0x000121B9 File Offset: 0x000103B9
	private bool IsMoving
	{
		get
		{
			return this;
		}
		set
		{
			this.isMoving = value;
			this.bunnyAnimator.SetBool("Stand", !value);
			this.vertTarget = (value ? 2f : 0f);
		}
	}

	// Token: 0x0600186E RID: 6254 RVA: 0x000121EB File Offset: 0x000103EB
	public override IEnumerator InitializeRoutine(GamePlayer owner, BoardActor target)
	{
		yield return base.InitializeRoutine(owner, target);
		this.curNode = owner.BoardObject.CurrentNode;
		this.targetNode = this.curNode;
		yield break;
	}

	// Token: 0x0600186F RID: 6255 RVA: 0x00012208 File Offset: 0x00010408
	private void Start()
	{
		this.SetPosition();
		this.IsMoving = false;
	}

	// Token: 0x06001870 RID: 6256 RVA: 0x000A7278 File Offset: 0x000A5478
	private void Update()
	{
		this.curVert = Mathf.MoveTowards(this.curVert, this.vertTarget, 6f * Time.deltaTime);
		this.bunnyAnimator.SetFloat("Vertical", this.curVert);
		this.bunnyAnimator.SetBool("Stand", this.curVert == 0f);
	}

	// Token: 0x06001871 RID: 6257 RVA: 0x000A72DC File Offset: 0x000A54DC
	private void SetPosition()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position + Vector3.up * 3f, Vector3.down, out raycastHit, 5f, 1024))
		{
			Vector3 position = base.transform.position;
			position.y = raycastHit.point.y + this.offset.y;
			base.transform.position = position;
		}
	}

	// Token: 0x06001872 RID: 6258 RVA: 0x00012217 File Offset: 0x00010417
	public IEnumerator Move(int steps, int seed)
	{
		this.IsMoving = true;
		this.curMoveSteps = steps;
		this.curMoveCounter = 0f;
		this.StartMove(seed);
		GameManager.Board.boardCamera.lookAhead = true;
		for (;;)
		{
			this.curMoveCounter += Time.deltaTime;
			if (this.targetNode != null)
			{
				if (this.moveSpline.StepSpline(this.moveVelocity * Time.deltaTime))
				{
					break;
				}
				this.moveVelocity = Mathf.MoveTowards(this.moveVelocity, this.moveSpeed, this.acceleration * Time.deltaTime);
				Vector3 vector = Vector3.zero;
				Vector3 zero = Vector3.zero;
				this.moveSpline.EvaluateSpline(this.moveSpline.CurrentStepT, ref zero, ref vector);
				vector = -vector.normalized;
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(-vector), Time.deltaTime * 500f);
				base.transform.position = zero + new Vector3(0f, 0.75f, 0f);
				GameManager.Board.boardCamera.tangent = vector;
				GameManager.Board.boardCamera.moveVelocity = this.moveVelocity;
				this.SetPosition();
			}
			yield return null;
		}
		this.curNode = this.targetNode;
		GameManager.Board.boardCamera.lookAhead = false;
		this.IsMoving = false;
		yield break;
	}

	// Token: 0x06001873 RID: 6259 RVA: 0x000A7358 File Offset: 0x000A5558
	private void StartMove(int seed)
	{
		System.Random random = new System.Random(seed);
		this.moveSpline.ResetStep();
		this.moveSpline.Clear();
		this.moveSpline.AddPoint(base.transform.position - new Vector3(0f, 0.75f, 0f));
		if (this.curMoveSteps == 0)
		{
			this.moveSpline.AddPoint(this.curNode.transform.position - new Vector3(0f, 0.75f, 0f));
		}
		BoardNode boardNode = this.curNode;
		while (this.curMoveSteps > 0)
		{
			List<BoardNode> forwardNodes = boardNode.GetForwardNodes(null, false);
			boardNode = forwardNodes[random.Next(0, forwardNodes.Count)];
			this.moveSpline.AddPoint(boardNode.NodePosition);
			this.targetNode = boardNode;
			if (boardNode.baseNodeType != BoardNodeType.Pathing)
			{
				this.curMoveSteps--;
			}
		}
		this.moveSpline.CalculateSpline(0.3f);
	}

	// Token: 0x06001874 RID: 6260 RVA: 0x00012234 File Offset: 0x00010434
	public override IEnumerator DoEvent(PersistentItemEventType type, BinaryReader reader)
	{
		short steps = reader.ReadInt16();
		int seed = reader.ReadInt32();
		bool hitPlayer = reader.ReadBoolean();
		byte id = hitPlayer ? reader.ReadByte() : 0;
		BoardPlayer hitActor = (BoardPlayer)GameManager.Board.GetActor(id);
		Transform headTransform = hitActor.PlayerAnimation.GetBone(PlayerBone.Head);
		if (type == PersistentItemEventType.LastTurn)
		{
			GameManager.Board.boardCamera.SetTrackedObject(this.bunnyTransform, GameManager.Board.PlayerCamOffset);
			yield return new WaitUntil(() => GameManager.Board.boardCamera.WithinDistance(0.5f));
			yield return new WaitForSeconds(0.25f);
			yield return this.Move((int)steps, seed);
			if (hitPlayer)
			{
				this.bunnyAnimator.SetBool("Stand", false);
				this.isMoving = false;
				this.bunnyAnimator.SetBool("_Jump", true);
				float startTime = Time.time;
				float maxTime = 5f;
				float speed = 10f;
				Vector3 offset = Vector3.up;
				while (Time.time - startTime < maxTime)
				{
					Vector3 vector = headTransform.position - offset - base.transform.position;
					float magnitude = vector.magnitude;
					vector.Normalize();
					float num = speed * Time.deltaTime;
					if (magnitude <= num)
					{
						base.transform.position = headTransform.position - offset;
						break;
					}
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(vector), Time.deltaTime * 1000f);
					base.transform.position += vector * num;
					yield return null;
				}
				this.curNode = hitActor.CurrentNode;
				this.bunnyAnimator.SetFloat("Vertical", 0f);
				this.bunnyAnimator.SetBool("_Jump", false);
				this.bunnyAnimator.SetBool("Stand", true);
				DamageInstance d = new DamageInstance
				{
					damage = 30,
					origin = this.bunnyTransform.position,
					blood = true,
					ragdoll = true,
					ragdollVel = 0f,
					bloodVel = 13f,
					bloodAmount = 1f,
					sound = true,
					volume = 0.75f,
					details = "Summoned Rabbit",
					killer = this.owner.BoardObject,
					removeKeys = true
				};
				hitActor.ApplyDamage(d);
				yield return new WaitForSeconds(0.15f);
				this.SetPosition();
				offset = default(Vector3);
			}
			yield return new WaitForSeconds(1f);
		}
		this.Finish(type, false);
		yield break;
	}

	// Token: 0x06001875 RID: 6261 RVA: 0x000A745C File Offset: 0x000A565C
	public override void GetByteArray(PersistentItemEventType type, BinaryWriter writer)
	{
		if (type == PersistentItemEventType.LastTurn)
		{
			int num = 4;
			int i = num;
			BoardNode boardNode = this.curNode;
			BoardActor boardActor = null;
			int num2 = GameManager.rand.Next();
			System.Random random = new System.Random(num2);
			while (i > 0)
			{
				List<BoardNode> forwardNodes = boardNode.GetForwardNodes(null, false);
				boardNode = forwardNodes[random.Next(0, forwardNodes.Count)];
				if (boardNode.LastPlayer != null && boardNode.LastPlayer != this.owner.BoardObject && boardNode.LastPlayer.PlayerState == BoardPlayerState.Idle)
				{
					boardActor = boardNode.LastPlayer;
					i--;
					break;
				}
				if (boardNode.baseNodeType != BoardNodeType.Pathing)
				{
					i--;
				}
			}
			writer.Write((short)(num - i - (boardActor ? 1 : 0)));
			writer.Write(num2);
			bool flag = boardActor != null;
			writer.Write(flag);
			if (flag)
			{
				writer.Write(boardActor.ActorID);
			}
		}
	}

	// Token: 0x06001876 RID: 6262 RVA: 0x000A7548 File Offset: 0x000A5748
	public override void Save(BinaryWriter writer)
	{
		writer.Write(base.transform.position.x);
		writer.Write(base.transform.position.y);
		writer.Write(base.transform.position.z);
		writer.Write(base.transform.rotation.eulerAngles.x);
		writer.Write(base.transform.rotation.eulerAngles.y);
		writer.Write(base.transform.rotation.eulerAngles.z);
		writer.Write((short)this.curNode.NodeID);
		writer.Write(this.owner.GlobalID);
	}

	// Token: 0x06001877 RID: 6263 RVA: 0x000A7614 File Offset: 0x000A5814
	public override void Load(BinaryReader reader)
	{
		base.transform.position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		base.transform.rotation = Quaternion.Euler(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		this.curNode = GameManager.Board.BoardNodes[(int)reader.ReadInt16()];
		this.owner = GameManager.GetPlayerWithID(reader.ReadInt16());
	}

	// Token: 0x06001878 RID: 6264 RVA: 0x0000539F File Offset: 0x0000359F
	public override short PersistentItemID()
	{
		return 0;
	}

	// Token: 0x040019E2 RID: 6626
	public Transform bunnyTransform;

	// Token: 0x040019E3 RID: 6627
	public Animator bunnyAnimator;

	// Token: 0x040019E4 RID: 6628
	public Vector3 offset;

	// Token: 0x040019E5 RID: 6629
	private BoardNode curNode;

	// Token: 0x040019E6 RID: 6630
	public float moveSpeed;

	// Token: 0x040019E7 RID: 6631
	public float acceleration = 11f;

	// Token: 0x040019E8 RID: 6632
	private int curMoveSteps = 10;

	// Token: 0x040019E9 RID: 6633
	private float curMoveCounter;

	// Token: 0x040019EA RID: 6634
	private float moveVelocity;

	// Token: 0x040019EB RID: 6635
	private Spline moveSpline = new Spline();

	// Token: 0x040019EC RID: 6636
	private BoardNode targetNode;

	// Token: 0x040019ED RID: 6637
	private float splineLength;

	// Token: 0x040019EE RID: 6638
	private float curVert;

	// Token: 0x040019EF RID: 6639
	private float vertTarget;

	// Token: 0x040019F0 RID: 6640
	private bool isMoving = true;
}
