using System;
using UnityEngine;

// Token: 0x020001E9 RID: 489
[ExecuteInEditMode]
public class MysteryMazeVisibility : MonoBehaviour
{
	// Token: 0x06000E44 RID: 3652 RVA: 0x0000CAB0 File Offset: 0x0000ACB0
	private void Update()
	{
		Shader.SetGlobalVector("_MysteryMazePos" + this.m_index.ToString(), base.transform.position);
	}

	// Token: 0x04000DBF RID: 3519
	[SerializeField]
	private int m_index;
}
