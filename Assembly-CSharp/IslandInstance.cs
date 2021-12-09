using System;
using UnityEngine;

// Token: 0x020000B6 RID: 182
public class IslandInstance : MonoBehaviour
{
	// Token: 0x060003CF RID: 975 RVA: 0x0003B56C File Offset: 0x0003976C
	public void DoRandom()
	{
		int index = GameManager.rand.Next();
		IslandRandomBase[] componentsInChildren = base.gameObject.GetComponentsInChildren<IslandRandomBase>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].DoRandom(index);
		}
	}

	// Token: 0x0400040E RID: 1038
	public bool[] enabledSkins;
}
