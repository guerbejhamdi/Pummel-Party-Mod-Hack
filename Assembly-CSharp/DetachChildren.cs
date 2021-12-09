using System;
using UnityEngine;

// Token: 0x0200056F RID: 1391
public class DetachChildren : MonoBehaviour
{
	// Token: 0x0600248E RID: 9358 RVA: 0x000DB730 File Offset: 0x000D9930
	private void OnDestroy()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(base.transform.GetChild(i).gameObject, 5f);
			base.transform.GetChild(i).parent = null;
		}
	}
}
