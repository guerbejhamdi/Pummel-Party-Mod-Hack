using System;
using UnityEngine;

// Token: 0x020000A6 RID: 166
public class HeapStackAllocator : MonoBehaviour
{
	// Token: 0x0600037D RID: 893 RVA: 0x000392D8 File Offset: 0x000374D8
	private void Start()
	{
		object[] array = new object[10240];
		for (int i = 0; i < 10240; i++)
		{
			array[i] = new byte[10240];
		}
	}
}
