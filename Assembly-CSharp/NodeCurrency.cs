using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Utility;

// Token: 0x020002B5 RID: 693
public class NodeCurrency : MonoBehaviour
{
	// Token: 0x06001411 RID: 5137 RVA: 0x0000FCA7 File Offset: 0x0000DEA7
	private void Start()
	{
		this.gameMap = base.GetComponentInParent<GameMap>();
	}

	// Token: 0x06001412 RID: 5138 RVA: 0x00097744 File Offset: 0x00095944
	public void UpdatePositions()
	{
		Vector3 up = Vector3.up;
		float radius = 1.3f;
		float num = 35f;
		List<NodeCurrency.CurrencyPoint> list = new List<NodeCurrency.CurrencyPoint>();
		float num2 = 0.011111111f;
		for (float num3 = 0f; num3 < 360f; num3 += num2)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(ZPMath.CirclePosition(base.transform.position + up, radius, num3), Vector3.down, out raycastHit, 1.5f, 1024))
			{
				list.Add(new NodeCurrency.CurrencyPoint(raycastHit.point, raycastHit.normal, num3));
			}
		}
		list.Sort(delegate(NodeCurrency.CurrencyPoint x, NodeCurrency.CurrencyPoint y)
		{
			float num4 = Mathf.Abs(x.normal.y - y.normal.y);
			if (x.normal.y == y.normal.y)
			{
				return 0;
			}
			if (num4 < 0.05f)
			{
				return 0;
			}
			if (x.normal.y > y.normal.y)
			{
				return -1;
			}
			return 1;
		});
		if (list.Count == 0)
		{
			return;
		}
		List<NodeCurrency.CurrencyPoint> list2 = new List<NodeCurrency.CurrencyPoint>();
		for (int i = 0; i < 5; i++)
		{
			list2.Add(list[0]);
			list.RemoveAt(0);
			int j = 0;
			while (j < list.Count)
			{
				if (Mathf.Abs(Mathf.DeltaAngle(list[j].angle, list2[i].angle)) < num)
				{
					list.RemoveAt(j);
				}
				else
				{
					j++;
				}
			}
			if (list.Count == 0)
			{
				return;
			}
		}
		this.positions = list2.ToArray();
	}

	// Token: 0x04001545 RID: 5445
	public int currency;

	// Token: 0x04001546 RID: 5446
	public const int points = 5;

	// Token: 0x04001547 RID: 5447
	public NodeCurrency.CurrencyPoint[] positions;

	// Token: 0x04001548 RID: 5448
	public GameMap gameMap;

	// Token: 0x04001549 RID: 5449
	private MeshFilter[] filters;

	// Token: 0x0400154A RID: 5450
	private GameObject[] gameObjects;

	// Token: 0x020002B6 RID: 694
	[Serializable]
	public struct CurrencyPoint
	{
		// Token: 0x06001414 RID: 5140 RVA: 0x0000FCB5 File Offset: 0x0000DEB5
		public CurrencyPoint(Vector3 position, Vector3 normal, float angle)
		{
			this.position = position;
			this.normal = normal;
			this.angle = angle;
		}

		// Token: 0x0400154B RID: 5451
		public Vector3 position;

		// Token: 0x0400154C RID: 5452
		public Vector3 normal;

		// Token: 0x0400154D RID: 5453
		public float angle;
	}
}
