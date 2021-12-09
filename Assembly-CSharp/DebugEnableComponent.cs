using System;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class DebugEnableComponent : MonoBehaviour
{
	// Token: 0x06000103 RID: 259 RVA: 0x0002FE14 File Offset: 0x0002E014
	private void Awake()
	{
		if (GameManager.DEBUGGING)
		{
			for (int i = 0; i < this.components.Length; i++)
			{
				this.components[i].enabled = true;
			}
		}
	}

	// Token: 0x0400015E RID: 350
	public MonoBehaviour[] components;
}
