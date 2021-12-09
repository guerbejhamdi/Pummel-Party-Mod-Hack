using System;
using UnityEngine;

// Token: 0x0200049A RID: 1178
public class ShaderPingPong : MonoBehaviour
{
	// Token: 0x06001F86 RID: 8070 RVA: 0x000171CB File Offset: 0x000153CB
	private void Start()
	{
		this.m = base.GetComponent<SkinnedMeshRenderer>().material;
	}

	// Token: 0x06001F87 RID: 8071 RVA: 0x000171DE File Offset: 0x000153DE
	private void Update()
	{
		this.m.SetVector("_StartPoint", new Vector4(0f, -2f + Mathf.PingPong(Time.time, 2.2f), 0f, 0f));
	}

	// Token: 0x0400225E RID: 8798
	private Material m;
}
