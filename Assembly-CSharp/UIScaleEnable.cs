using System;
using UnityEngine;

// Token: 0x02000145 RID: 325
public class UIScaleEnable : MonoBehaviour
{
	// Token: 0x06000942 RID: 2370 RVA: 0x00052D5C File Offset: 0x00050F5C
	private void Start()
	{
		foreach (object obj in base.transform)
		{
			((Transform)obj).gameObject.SetActive(true);
		}
	}
}
