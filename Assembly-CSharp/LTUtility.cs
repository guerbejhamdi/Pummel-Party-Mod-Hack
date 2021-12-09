using System;
using UnityEngine;

// Token: 0x02000102 RID: 258
public class LTUtility
{
	// Token: 0x0600075E RID: 1886 RVA: 0x0004A840 File Offset: 0x00048A40
	public static Vector3[] reverse(Vector3[] arr)
	{
		int num = arr.Length;
		int i = 0;
		int num2 = num - 1;
		while (i < num2)
		{
			Vector3 vector = arr[i];
			arr[i] = arr[num2];
			arr[num2] = vector;
			i++;
			num2--;
		}
		return arr;
	}
}
