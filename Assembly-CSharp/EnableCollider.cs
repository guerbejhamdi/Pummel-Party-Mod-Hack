using System;
using UnityEngine;

// Token: 0x02000570 RID: 1392
public class EnableCollider : MonoBehaviour
{
	// Token: 0x06002490 RID: 9360 RVA: 0x0001A461 File Offset: 0x00018661
	public void Awake()
	{
		base.GetComponent<BoxCollider>().enabled = true;
	}
}
