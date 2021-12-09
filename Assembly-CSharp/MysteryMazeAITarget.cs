using System;
using UnityEngine;

// Token: 0x020001E3 RID: 483
public class MysteryMazeAITarget : MonoBehaviour
{
	// Token: 0x17000140 RID: 320
	// (get) Token: 0x06000E12 RID: 3602 RVA: 0x0000C971 File Offset: 0x0000AB71
	public bool FinalTarget
	{
		get
		{
			return this.m_finalTarget;
		}
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x06000E13 RID: 3603 RVA: 0x0000C979 File Offset: 0x0000AB79
	public int GroupIndex
	{
		get
		{
			return this.m_groupIndex;
		}
	}

	// Token: 0x04000D84 RID: 3460
	[SerializeField]
	protected bool m_finalTarget;

	// Token: 0x04000D85 RID: 3461
	[SerializeField]
	protected int m_groupIndex;

	// Token: 0x04000D86 RID: 3462
	[SerializeField]
	protected int m_index;
}
