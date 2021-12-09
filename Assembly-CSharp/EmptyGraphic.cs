using System;
using UnityEngine.UI;

// Token: 0x020004F7 RID: 1271
public class EmptyGraphic : Graphic
{
	// Token: 0x0600217A RID: 8570 RVA: 0x0001842E File Offset: 0x0001662E
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();
	}
}
