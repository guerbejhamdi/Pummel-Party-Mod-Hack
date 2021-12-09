using System;
using UnityEngine;

// Token: 0x02000460 RID: 1120
public class PrintShader : MonoBehaviour
{
	// Token: 0x06001E87 RID: 7815 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x06001E88 RID: 7816 RVA: 0x00016841 File Offset: 0x00014A41
	private void Update()
	{
		Debug.LogError(base.GetComponent<MeshRenderer>().sharedMaterial.shader.name);
	}
}
