using System;
using UnityEngine;

// Token: 0x02000230 RID: 560
[ExecuteInEditMode]
public class SourcePos : MonoBehaviour
{
	// Token: 0x06001042 RID: 4162 RVA: 0x0000DB39 File Offset: 0x0000BD39
	private void Update()
	{
		Shader.SetGlobalVector("_SourcePos", base.transform.position);
	}
}
