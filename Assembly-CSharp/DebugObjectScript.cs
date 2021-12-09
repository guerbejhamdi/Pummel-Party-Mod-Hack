using System;
using UnityEngine;

// Token: 0x02000476 RID: 1142
public class DebugObjectScript : MonoBehaviour
{
	// Token: 0x06001EDB RID: 7899 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x06001EDC RID: 7900 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x06001EDD RID: 7901 RVA: 0x00016C4F File Offset: 0x00014E4F
	private void OnWillRenderObject()
	{
		if (this.wireframe)
		{
			GL.wireframe = true;
		}
	}

	// Token: 0x040021D5 RID: 8661
	public bool wireframe;
}
