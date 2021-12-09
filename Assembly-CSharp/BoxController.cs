using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000169 RID: 361
public class BoxController : MonoBehaviour
{
	// Token: 0x06000A5C RID: 2652 RVA: 0x0005B814 File Offset: 0x00059A14
	public void Awake()
	{
		for (int i = 0; i < this.m_dimensions; i++)
		{
			for (int j = 0; j < this.m_dimensions; j++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_boxPrefab, this.GetBoxPosition(i, j), Quaternion.identity);
				BoxDropBox component = gameObject.GetComponent<BoxDropBox>();
				gameObject.transform.localScale = Vector3.one * this.m_scale;
				gameObject.transform.parent = base.transform;
				component.Init(i, j, i * this.m_dimensions + j);
				this.m_boxes.Add(component);
			}
		}
		this.m_surface = base.GetComponent<NavMeshSurface>();
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x0000ABF2 File Offset: 0x00008DF2
	public void Update()
	{
		float num = Time.time - this.m_lastUpdate;
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x0005B8BC File Offset: 0x00059ABC
	public void DropBoxes(int permutation, float delay)
	{
		for (int i = 0; i < this.m_dimensions; i++)
		{
			for (int j = 0; j < this.m_dimensions; j++)
			{
				int index = i * this.m_dimensions + j;
				if (this.ShouldBoxDrop(i, j, permutation))
				{
					this.m_boxes[index].Drop(delay);
				}
			}
		}
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x0005B914 File Offset: 0x00059B14
	public void OnDrawGizmos()
	{
		for (int i = 0; i < this.m_dimensions; i++)
		{
			for (int j = 0; j < this.m_dimensions; j++)
			{
				int dimensions = this.m_dimensions;
				if (this.ShouldBoxDrop(i, j, this.m_testDropIndex))
				{
					Gizmos.color = Color.green;
				}
				else
				{
					Gizmos.color = Color.red;
				}
				Gizmos.DrawCube(this.GetBoxPosition(i, j), new Vector3(this.m_scale, this.m_scale, this.m_scale) * 0.2f);
				Color color = Gizmos.color;
				color.a = 0.25f;
				Gizmos.color = color;
				Gizmos.DrawWireCube(this.GetBoxPosition(i, j), new Vector3(this.m_scale, this.m_scale, this.m_scale));
			}
		}
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x0000AC07 File Offset: 0x00008E07
	private float ManhattanDistance(Vector3 a, Vector3 b)
	{
		return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
	}

	// Token: 0x06000A61 RID: 2657 RVA: 0x0000AC2E File Offset: 0x00008E2E
	private float ChebyshevDistance(Vector3 a, Vector3 b)
	{
		return Mathf.Max(Mathf.Abs(b.x - a.x), Mathf.Abs(b.z - a.z));
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x0005B9E8 File Offset: 0x00059BE8
	public bool ShouldBoxDrop(int x, int y, int permutation)
	{
		permutation %= BoxController.GetBoxDropPermuationCount();
		int num = x * this.m_dimensions + y;
		this.GetStartPos();
		Vector3 boxPosition = this.GetBoxPosition(x, y);
		Vector3 vector = boxPosition / this.m_scale;
		switch (permutation)
		{
		case 0:
			return num < this.m_dimensions * this.m_dimensions / 2;
		case 1:
			return num >= this.m_dimensions * this.m_dimensions / 2;
		case 2:
			return x % 2 == y % 2;
		case 3:
			return x % 2 != y % 2;
		case 4:
			return x % 2 == 0;
		case 5:
			return y % 2 == 0;
		case 6:
			return Vector3.Distance(Vector3.zero, boxPosition) < this.m_dropDistance * this.m_scale;
		case 7:
			return Vector3.Distance(Vector3.zero, boxPosition) > this.m_dropDistance * this.m_scale;
		case 8:
			return (int)this.ChebyshevDistance(Vector3.zero, vector) % 2 == 0;
		case 9:
			return (int)this.ChebyshevDistance(Vector3.zero, vector) % 2 != 0;
		case 10:
			return vector.x < 0f && vector.z < 0f;
		case 11:
			return vector.x > 0f && vector.z < 0f;
		case 12:
			return vector.x > 0f && vector.z > 0f;
		case 13:
			return vector.x < 0f && vector.z > 0f;
		case 14:
			return x % 3 != 0 && y % 3 != 0;
		case 15:
			return x % 3 == 0 || y % 3 == 0;
		case 16:
			return x == y || x == y + 1 || x == y - 1 || x + y == this.m_dimensions || x + 1 + y == this.m_dimensions || x + 2 + y == this.m_dimensions;
		case 17:
			return x != y && x != y + 1 && x != y - 1 && x + y != this.m_dimensions && x + 1 + y != this.m_dimensions && x + 2 + y != this.m_dimensions;
		default:
			return false;
		}
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x0000AC59 File Offset: 0x00008E59
	public static int GetBoxDropPermuationCount()
	{
		return 18;
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x0005BC28 File Offset: 0x00059E28
	private Vector3 GetBoxPosition(int x, int y)
	{
		return this.GetStartPos() + new Vector3((float)x * this.m_scale, 0f, (float)y * this.m_scale) + new Vector3(this.m_scale, 0f, this.m_scale) * 0.5f + this.m_offset;
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x0005BC8C File Offset: 0x00059E8C
	private Vector3 GetStartPos()
	{
		float num = (float)this.m_dimensions * this.m_scale * 0.5f;
		return new Vector3(-num, 0f, -num);
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x0005BCBC File Offset: 0x00059EBC
	public BoxDropBox GetClosestBox(Vector3 pos, bool isDropping, float minDistance, float maxDistance = 1000f)
	{
		BoxDropBox result = null;
		float num = float.PositiveInfinity;
		foreach (BoxDropBox boxDropBox in this.m_boxes)
		{
			if (boxDropBox.IsDropping == isDropping)
			{
				float num2 = Vector3.Distance(pos, boxDropBox.transform.position);
				if (num2 < num && num2 >= minDistance && num2 <= maxDistance)
				{
					result = boxDropBox;
					num = num2;
				}
			}
		}
		return result;
	}

	// Token: 0x04000937 RID: 2359
	[SerializeField]
	protected int m_dimensions;

	// Token: 0x04000938 RID: 2360
	[SerializeField]
	protected float m_scale;

	// Token: 0x04000939 RID: 2361
	[SerializeField]
	protected GameObject m_boxPrefab;

	// Token: 0x0400093A RID: 2362
	[SerializeField]
	protected Vector3 m_offset;

	// Token: 0x0400093B RID: 2363
	[SerializeField]
	[Range(0f, 10f)]
	protected float m_dropDistance = 3f;

	// Token: 0x0400093C RID: 2364
	[SerializeField]
	[Range(0f, 20f)]
	protected int m_testDropIndex;

	// Token: 0x0400093D RID: 2365
	private NavMeshSurface m_surface;

	// Token: 0x0400093E RID: 2366
	private List<BoxDropBox> m_boxes = new List<BoxDropBox>();

	// Token: 0x0400093F RID: 2367
	private int dropPerm;

	// Token: 0x04000940 RID: 2368
	private float m_lastUpdate;
}
