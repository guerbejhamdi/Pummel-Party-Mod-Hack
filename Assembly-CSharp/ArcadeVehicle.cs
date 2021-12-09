using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class ArcadeVehicle : MonoBehaviour
{
	// Token: 0x06000012 RID: 18 RVA: 0x00003A00 File Offset: 0x00001C00
	private void Awake()
	{
		this.rt = (RectTransform)base.transform;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00003A13 File Offset: 0x00001C13
	private void Update()
	{
		this.rt.anchoredPosition += Vector2.right * Time.deltaTime * this.Speed;
	}

	// Token: 0x04000019 RID: 25
	public float Speed;

	// Token: 0x0400001A RID: 26
	public RectTransform rt;
}
