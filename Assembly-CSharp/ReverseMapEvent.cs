using System;
using System.Collections;
using LlockhamIndustries.Decals;
using UnityEngine;

// Token: 0x02000322 RID: 802
public class ReverseMapEvent : MainBoardEvent
{
	// Token: 0x060015E5 RID: 5605 RVA: 0x00010967 File Offset: 0x0000EB67
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode boardNode, int seed)
	{
		base.DoGenericBoardEventActions();
		this.rand = new System.Random(seed);
		this.ReverseSpaces();
		UnityEngine.Object.Instantiate<GameObject>(this.uiIcon, GameObject.Find("BoardCanvas/BoardUI").transform);
		yield return new WaitForSeconds(0.5f);
		float startTime = Time.time;
		Transform root = GameObject.Find("AdditiveRoot/Arrows").transform;
		Quaternion[] targets = new Quaternion[root.childCount];
		Quaternion[] start = new Quaternion[root.childCount];
		for (int i = 0; i < root.childCount; i++)
		{
			Transform child = root.GetChild(i);
			targets[i] = Quaternion.Euler(child.rotation.eulerAngles.x, child.rotation.eulerAngles.y, child.rotation.eulerAngles.z + 180f);
			start[i] = Quaternion.Euler(child.rotation.eulerAngles.x, child.rotation.eulerAngles.y, child.rotation.eulerAngles.z);
		}
		while (Time.time - startTime < 1f)
		{
			for (int j = 0; j < root.childCount; j++)
			{
				Transform child2 = root.GetChild(j);
				child2.rotation = Quaternion.Lerp(start[j], targets[j], Time.time - startTime);
				ProjectionRenderer component = child2.gameObject.GetComponent<ProjectionRenderer>();
				component.SetColor(0, Color.Lerp((!this.reversed) ? this.baseColor : this.reverseColor, this.reversed ? this.baseColor : this.reverseColor, (Time.time - startTime) / 1f));
				component.UpdateProperties();
			}
			yield return null;
		}
		for (int k = 0; k < root.childCount; k++)
		{
			root.GetChild(k).rotation = targets[k];
		}
		yield return new WaitForSeconds(0.5f);
		base.Finished = true;
		this.reversed = !this.reversed;
		yield return null;
		yield break;
	}

	// Token: 0x060015E6 RID: 5606 RVA: 0x0009CCA8 File Offset: 0x0009AEA8
	private void ReverseSpaces()
	{
		BoardNode[] boardNodes = GameManager.Board.BoardNodes;
		for (int i = 0; i < boardNodes.Length; i++)
		{
			if (boardNodes[i].baseNodeType != BoardNodeType.Graveyard)
			{
				for (int j = 0; j < boardNodes[i].nodeConnections.Length; j++)
				{
					if (boardNodes[i].nodeConnections[j].node.baseNodeType != BoardNodeType.Graveyard)
					{
						if (boardNodes[i].nodeConnections[j].connection_type == BoardNodeConnectionDirection.Back)
						{
							boardNodes[i].nodeConnections[j].connection_type = BoardNodeConnectionDirection.Forward;
						}
						else
						{
							boardNodes[i].nodeConnections[j].connection_type = BoardNodeConnectionDirection.Back;
						}
					}
				}
			}
		}
	}

	// Token: 0x060015E7 RID: 5607 RVA: 0x0001097D File Offset: 0x0000EB7D
	public override int GetEventValue1()
	{
		if (!this.reversed)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x060015E8 RID: 5608 RVA: 0x0001098A File Offset: 0x0000EB8A
	public override void SetupFromLoad(int val1, int val2)
	{
		if (val1 == 1)
		{
			this.ReverseSpaces();
		}
		this.reversed = (val1 == 1);
	}

	// Token: 0x04001707 RID: 5895
	public GameObject uiIcon;

	// Token: 0x04001708 RID: 5896
	public Color baseColor;

	// Token: 0x04001709 RID: 5897
	public Color reverseColor;

	// Token: 0x0400170A RID: 5898
	private bool reversed;
}
