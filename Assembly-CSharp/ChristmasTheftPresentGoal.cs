using System;
using UnityEngine;

// Token: 0x02000198 RID: 408
public class ChristmasTheftPresentGoal : MonoBehaviour
{
	// Token: 0x1700010B RID: 267
	// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x0000B65E File Offset: 0x0000985E
	public int PlayerIndex
	{
		get
		{
			return this.m_playerIndex;
		}
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x00063B40 File Offset: 0x00061D40
	public Vector3 GetPosition(int index)
	{
		return base.transform.position + base.transform.right * this.m_slotOffsets[index].x + base.transform.forward * this.m_slotOffsets[index].y;
	}

	// Token: 0x04000AE9 RID: 2793
	[SerializeField]
	protected int m_playerIndex;

	// Token: 0x04000AEA RID: 2794
	private Vector2[] m_slotOffsets = new Vector2[]
	{
		new Vector2(-1.5f, 0f),
		new Vector2(-0.5f, 0f),
		new Vector2(0.5f, 0f),
		new Vector2(1.5f, 0f),
		new Vector2(-1.5f, 1f),
		new Vector2(-0.5f, 1f),
		new Vector2(0.5f, 1f),
		new Vector2(1.5f, 1f)
	};
}
