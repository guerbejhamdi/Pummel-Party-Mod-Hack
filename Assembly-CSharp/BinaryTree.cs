using System;
using UnityEngine;

// Token: 0x02000450 RID: 1104
public class BinaryTree
{
	// Token: 0x06001E55 RID: 7765 RVA: 0x00016572 File Offset: 0x00014772
	public BinaryTree(float[] points)
	{
		this.Setup(points);
	}

	// Token: 0x06001E56 RID: 7766 RVA: 0x00016581 File Offset: 0x00014781
	public BinaryTree(float[] points, int offset)
	{
		this.offset = offset;
		this.Setup(points);
	}

	// Token: 0x06001E57 RID: 7767 RVA: 0x000C3EBC File Offset: 0x000C20BC
	private void Setup(float[] points)
	{
		if (points.Length > 1)
		{
			int num = points.Length / 2;
			int num2 = (int)Mathf.Ceil((float)points.Length / 2f);
			float[] array = new float[num];
			float[] array2 = new float[num2];
			for (int i = 0; i < num; i++)
			{
				array[i] = points[i];
			}
			for (int j = 0; j < num2; j++)
			{
				array2[j] = points[num + j];
			}
			this.val = array2[0];
			this.leftNode = new BinaryTree(array, this.offset);
			this.rightNode = new BinaryTree(array2, this.offset + num);
		}
	}

	// Token: 0x06001E58 RID: 7768 RVA: 0x00016597 File Offset: 0x00014797
	public int FindPoint(float p)
	{
		if (this.leftNode == null)
		{
			return this.offset;
		}
		if (p < this.val)
		{
			return this.leftNode.FindPoint(p);
		}
		return this.rightNode.FindPoint(p);
	}

	// Token: 0x0400213D RID: 8509
	public BinaryTree leftNode;

	// Token: 0x0400213E RID: 8510
	public BinaryTree rightNode;

	// Token: 0x0400213F RID: 8511
	private float val;

	// Token: 0x04002140 RID: 8512
	private int offset;
}
