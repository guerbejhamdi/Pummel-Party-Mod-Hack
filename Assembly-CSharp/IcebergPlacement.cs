using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Utility;

// Token: 0x020001BD RID: 445
[ExecuteInEditMode]
public class IcebergPlacement : MonoBehaviour
{
	// Token: 0x06000CD9 RID: 3289 RVA: 0x0006AA80 File Offset: 0x00068C80
	private void Start()
	{
		float num = 0f;
		for (int i = 0; i < base.transform.childCount - 1; i++)
		{
			Transform child = base.transform.GetChild(i);
			Transform child2 = base.transform.GetChild(i + 1);
			this.m_segmentStart.Add(child.position);
			this.m_segmentLengths.Add(num);
			float num2 = Vector3.Distance(child.position, child2.position);
			num += num2;
		}
	}

	// Token: 0x06000CDA RID: 3290 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x06000CDB RID: 3291 RVA: 0x0006AAFC File Offset: 0x00068CFC
	public float GetDistanceAlongPath(Vector3 position)
	{
		int index = 0;
		float num = float.PositiveInfinity;
		Vector3 vector = Vector3.zero;
		for (int i = 0; i < base.transform.childCount - 1; i++)
		{
			Transform child = base.transform.GetChild(i);
			Transform child2 = base.transform.GetChild(i + 1);
			Vector3 vector2 = ZPMath.ClosestPointOnLine(child.position, child2.position, position);
			float num2 = Vector3.Distance(vector2, position);
			if (num2 < num)
			{
				num = num2;
				index = i;
				vector = vector2;
			}
		}
		float num3 = this.m_segmentLengths[index] + Vector3.Distance(this.m_segmentStart[index], vector);
		Debug.DrawLine(position, this.m_segmentStart[index], Color.red);
		Debug.DrawLine(position, vector, Color.yellow);
		Debug.Log(num3);
		return num3;
	}

	// Token: 0x04000C2D RID: 3117
	[SerializeField]
	protected Transform m_test;

	// Token: 0x04000C2E RID: 3118
	private List<float> m_segmentLengths = new List<float>();

	// Token: 0x04000C2F RID: 3119
	private List<Vector3> m_segmentStart = new List<Vector3>();
}
