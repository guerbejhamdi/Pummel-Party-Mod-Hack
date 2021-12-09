using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Utility;

// Token: 0x020002B8 RID: 696
public class ObjectiveTrail : MonoBehaviour
{
	// Token: 0x06001418 RID: 5144 RVA: 0x0000FCD8 File Offset: 0x0000DED8
	public void Setup(BoardNode startNode)
	{
		base.transform.parent = GameManager.BoardRoot.transform.root;
		base.transform.position = startNode.NodePosition + this.offset;
		this.StartMove(startNode);
	}

	// Token: 0x06001419 RID: 5145 RVA: 0x000978F8 File Offset: 0x00095AF8
	public void Update()
	{
		if (!this.finished)
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
					base.transform.position = zero + this.offset;
					return;
				}
				this.finished = true;
				UnityEngine.Object.Destroy(base.gameObject, 5f);
				for (int i = 0; i < this.particles.Length; i++)
				{
					this.particles[i].Stop();
				}
			}
		}
	}

	// Token: 0x0600141A RID: 5146 RVA: 0x000979F4 File Offset: 0x00095BF4
	private void StartMove(BoardNode startNode)
	{
		this.curMoveCounter = 0f;
		this.moveVelocity = 12f;
		this.moveSpline.ResetStep();
		this.moveSpline.Clear();
		this.moveSpline.AddPoint(startNode.NodePosition);
		BoardNode boardNode = startNode;
		while (this.curMoveSteps > 0)
		{
			List<BoardNode> forwardNodes = boardNode.GetForwardNodes(null, true);
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
			this.moveSpline.AddPoint(boardNode.NodePosition);
			this.targetNode = boardNode;
			if (boardNode.baseNodeType != BoardNodeType.Pathing)
			{
				this.curMoveSteps--;
			}
			if (boardNode.CurrentNodeType == BoardNodeType.Trophy)
			{
				break;
			}
		}
		this.moveSpline.CalculateSpline(0.3f);
	}

	// Token: 0x04001550 RID: 5456
	public ParticleSystem[] particles;

	// Token: 0x04001551 RID: 5457
	public float moveSpeed = 20f;

	// Token: 0x04001552 RID: 5458
	public float acceleration = 20f;

	// Token: 0x04001553 RID: 5459
	public Vector3 offset = new Vector3(0f, 0.75f, 0f);

	// Token: 0x04001554 RID: 5460
	private bool finished;

	// Token: 0x04001555 RID: 5461
	private int curMoveSteps = 100;

	// Token: 0x04001556 RID: 5462
	private float curMoveCounter;

	// Token: 0x04001557 RID: 5463
	private float moveVelocity = 12f;

	// Token: 0x04001558 RID: 5464
	private Spline moveSpline = new Spline();

	// Token: 0x04001559 RID: 5465
	private BoardNode targetNode;

	// Token: 0x0400155A RID: 5466
	private float splineLength;
}
