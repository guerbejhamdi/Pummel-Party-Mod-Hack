using System;
using UnityEngine;

// Token: 0x02000163 RID: 355
[ExecuteInEditMode]
public class NodeCreator : MonoBehaviour
{
	// Token: 0x06000A39 RID: 2617 RVA: 0x0005A80C File Offset: 0x00058A0C
	public NodeCreator()
	{
		this.nodes = new Node[0];
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x04000913 RID: 2323
	public bool draw_boxes = true;

	// Token: 0x04000914 RID: 2324
	public Color box_color = new Color(1f, 0f, 0f);

	// Token: 0x04000915 RID: 2325
	public Color line_color = new Color(0f, 1f, 0f);

	// Token: 0x04000916 RID: 2326
	public Node[] nodes;
}
