using System;
using UnityEngine;

// Token: 0x02000477 RID: 1143
public struct DebugLine
{
	// Token: 0x06001EDF RID: 7903 RVA: 0x00016C5F File Offset: 0x00014E5F
	public DebugLine(Vector3 _start, Vector3 _end, Color _color, bool _depth_test)
	{
		this.start = _start;
		this.end = _end;
		this.color = _color;
		this.depth_test = _depth_test;
	}

	// Token: 0x040021D6 RID: 8662
	public Vector3 start;

	// Token: 0x040021D7 RID: 8663
	public Vector3 end;

	// Token: 0x040021D8 RID: 8664
	public Color color;

	// Token: 0x040021D9 RID: 8665
	public bool depth_test;
}
