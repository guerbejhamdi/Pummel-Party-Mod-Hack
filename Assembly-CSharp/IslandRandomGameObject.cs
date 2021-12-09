using System;
using UnityEngine;

// Token: 0x020000BA RID: 186
public class IslandRandomGameObject : IslandRandomBase
{
	// Token: 0x060003D6 RID: 982 RVA: 0x0003B664 File Offset: 0x00039864
	public override void DoRandom(int index)
	{
		for (int i = 0; i < this.objects.Length; i++)
		{
			if (this.objects[i] != null)
			{
				this.objects[i].SetActive(i == index);
			}
		}
	}

	// Token: 0x04000419 RID: 1049
	public GameObject[] objects;
}
