using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Utility;

// Token: 0x020000B7 RID: 183
public class IslandNode : MonoBehaviour
{
	// Token: 0x060003D1 RID: 977 RVA: 0x0003B5A8 File Offset: 0x000397A8
	public void DoRandom(System.Random rand)
	{
		IslandInstance componentInChildren = base.GetComponentInChildren<IslandInstance>();
		List<int> list = new List<int>();
		List<float> list2 = new List<float>();
		float num = 0f;
		for (int i = 0; i < componentInChildren.enabledSkins.Length; i++)
		{
			if (componentInChildren.enabledSkins[i])
			{
				list.Add(i);
				list2.Add(num);
				num += IslandNode.chances[i];
			}
		}
		BinaryTree binaryTree = new BinaryTree(list2.ToArray());
		int index = list[binaryTree.FindPoint(ZPMath.RandomFloat(rand, 0f, num))];
		index = 0;
		IslandRandomBase[] componentsInChildren = base.gameObject.GetComponentsInChildren<IslandRandomBase>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			componentsInChildren[j].DoRandom(index);
		}
	}

	// Token: 0x0400040F RID: 1039
	public BoardNode startPoint;

	// Token: 0x04000410 RID: 1040
	public IslandConnection[] islandConnections;

	// Token: 0x04000411 RID: 1041
	private static float[] chances = new float[]
	{
		0.75f,
		0.125f,
		0.125f
	};
}
