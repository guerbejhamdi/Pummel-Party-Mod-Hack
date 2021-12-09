using System;
using UnityEngine;

// Token: 0x02000475 RID: 1141
public class DebugBehaviour : MonoBehaviour
{
	// Token: 0x06001ED6 RID: 7894 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x06001ED7 RID: 7895 RVA: 0x00016C3C File Offset: 0x00014E3C
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			DebugUtility.ToggleDebugText();
		}
	}

	// Token: 0x06001ED8 RID: 7896 RVA: 0x0000398C File Offset: 0x00001B8C
	private void OnWillRenderObject()
	{
	}

	// Token: 0x06001ED9 RID: 7897 RVA: 0x0000398C File Offset: 0x00001B8C
	private void OnRenderObject()
	{
	}
}
