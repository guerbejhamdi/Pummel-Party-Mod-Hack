using System;
using UnityEngine;

// Token: 0x0200048E RID: 1166
public class ZPRoot : MonoBehaviour
{
	// Token: 0x06001F62 RID: 8034 RVA: 0x00017058 File Offset: 0x00015258
	public void Awake()
	{
		if (this.dont_destroy_on_load)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	// Token: 0x0400223F RID: 8767
	public bool dont_destroy_on_load = true;
}
