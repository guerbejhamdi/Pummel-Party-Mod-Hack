using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000162 RID: 354
[Serializable]
public class Node
{
	// Token: 0x06000A34 RID: 2612 RVA: 0x0005A7A8 File Offset: 0x000589A8
	public Node()
	{
		this.id = 0;
		this.position = Vector3.zero;
		this.y_rotation = 0f;
		this.groups = new int[0];
		this.connections = new int[0];
		this.blocker = true;
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06000A35 RID: 2613 RVA: 0x0000AA2B File Offset: 0x00008C2B
	public List<Bomb> HittingBombs
	{
		get
		{
			return this.hittingBombs;
		}
	}

	// Token: 0x06000A36 RID: 2614 RVA: 0x0000AA33 File Offset: 0x00008C33
	public void AddHittingBomb(Bomb b)
	{
		this.hittingBombs.Add(b);
		this.bombNode.SetIndicatorState(true);
	}

	// Token: 0x06000A37 RID: 2615 RVA: 0x0000AA4D File Offset: 0x00008C4D
	public void RemoveHittingBomb(Bomb b)
	{
		this.hittingBombs.Remove(b);
		if (this.hittingBombs.Count == 0)
		{
			this.bombNode.SetIndicatorState(false);
		}
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x0000AA75 File Offset: 0x00008C75
	public void ClearHittingBombs()
	{
		this.hittingBombs.Clear();
		this.bombNode.SetIndicatorState(false);
	}

	// Token: 0x04000909 RID: 2313
	public int id;

	// Token: 0x0400090A RID: 2314
	public Vector3 position;

	// Token: 0x0400090B RID: 2315
	public float y_rotation;

	// Token: 0x0400090C RID: 2316
	public int[] groups;

	// Token: 0x0400090D RID: 2317
	public int[] connections;

	// Token: 0x0400090E RID: 2318
	public bool blocker;

	// Token: 0x0400090F RID: 2319
	public int unblockedPlayerCount;

	// Token: 0x04000910 RID: 2320
	public BombNode bombNode;

	// Token: 0x04000911 RID: 2321
	public short occupier = -1;

	// Token: 0x04000912 RID: 2322
	private List<Bomb> hittingBombs = new List<Bomb>();
}
