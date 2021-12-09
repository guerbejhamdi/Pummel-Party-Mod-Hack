using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B7 RID: 439
public class IcebergCheckpoint : MonoBehaviour
{
	// Token: 0x06000CA8 RID: 3240 RVA: 0x0000BD0F File Offset: 0x00009F0F
	public void Awake()
	{
		IcebergCheckpoint.m_checkPoints.Clear();
	}

	// Token: 0x06000CA9 RID: 3241 RVA: 0x0000BD1B File Offset: 0x00009F1B
	public void Start()
	{
		IcebergCheckpoint.m_checkPoints.Add(this);
	}

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x06000CAA RID: 3242 RVA: 0x0000BD28 File Offset: 0x00009F28
	public int Index
	{
		get
		{
			return this.m_index;
		}
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06000CAB RID: 3243 RVA: 0x0000BD30 File Offset: 0x00009F30
	public bool IsAICheckpoint
	{
		get
		{
			return this.m_isAICheckpoint;
		}
	}

	// Token: 0x06000CAC RID: 3244 RVA: 0x0006A144 File Offset: 0x00068344
	public static IcebergCheckpoint GetNextCheckPoint(int index)
	{
		foreach (IcebergCheckpoint icebergCheckpoint in IcebergCheckpoint.m_checkPoints)
		{
			if (icebergCheckpoint.Index == index + 1 && icebergCheckpoint.IsAICheckpoint)
			{
				return icebergCheckpoint;
			}
		}
		return null;
	}

	// Token: 0x04000C09 RID: 3081
	[SerializeField]
	protected int m_index;

	// Token: 0x04000C0A RID: 3082
	[SerializeField]
	protected bool m_isAICheckpoint = true;

	// Token: 0x04000C0B RID: 3083
	private static List<IcebergCheckpoint> m_checkPoints = new List<IcebergCheckpoint>();
}
