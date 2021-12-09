using System;
using UnityEngine;

// Token: 0x020004A2 RID: 1186
public class SlasherAnimEventHelper : MonoBehaviour
{
	// Token: 0x06001FAD RID: 8109 RVA: 0x0001736B File Offset: 0x0001556B
	public void Step(int foot)
	{
		this.slasher.Step(foot);
	}

	// Token: 0x0400228F RID: 8847
	public SlasherEnemy slasher;
}
