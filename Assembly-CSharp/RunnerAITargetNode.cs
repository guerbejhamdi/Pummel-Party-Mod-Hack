using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000229 RID: 553
public class RunnerAITargetNode : MonoBehaviour
{
	// Token: 0x06001015 RID: 4117 RVA: 0x0000DA9A File Offset: 0x0000BC9A
	public void Awake()
	{
		RunnerAITargetNode.m_activeNodes.Add(this);
	}

	// Token: 0x06001016 RID: 4118 RVA: 0x0000DAA7 File Offset: 0x0000BCA7
	private void OnDestroy()
	{
		RunnerAITargetNode.m_activeNodes.Remove(this);
	}

	// Token: 0x06001017 RID: 4119 RVA: 0x0007EF4C File Offset: 0x0007D14C
	public static RunnerAITargetNode GetBestNode(Vector3 pos, bool GetRandomNodeOnX)
	{
		float num = float.PositiveInfinity;
		List<RunnerAITargetNode> list = new List<RunnerAITargetNode>();
		foreach (RunnerAITargetNode runnerAITargetNode in RunnerAITargetNode.m_activeNodes)
		{
			if (runnerAITargetNode.transform.position.x < pos.x)
			{
				float num2 = Mathf.Abs(pos.x - runnerAITargetNode.transform.position.x);
				if (Mathf.Approximately(num2, num))
				{
					list.Add(runnerAITargetNode);
				}
				if (num2 < num)
				{
					list.Clear();
					list.Add(runnerAITargetNode);
					num = num2;
				}
			}
		}
		if (GetRandomNodeOnX)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			return list[index];
		}
		num = float.PositiveInfinity;
		RunnerAITargetNode result = null;
		foreach (RunnerAITargetNode runnerAITargetNode2 in list)
		{
			float num3 = Vector3.Distance(pos, runnerAITargetNode2.transform.position);
			if (num3 < num)
			{
				result = runnerAITargetNode2;
				num = num3;
			}
		}
		return result;
	}

	// Token: 0x04001064 RID: 4196
	[SerializeField]
	private RunnerAITargetNodeType m_type;

	// Token: 0x04001065 RID: 4197
	private static List<RunnerAITargetNode> m_activeNodes = new List<RunnerAITargetNode>();
}
