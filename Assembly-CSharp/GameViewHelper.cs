using System;
using UnityEngine;

// Token: 0x02000457 RID: 1111
public static class GameViewHelper
{
	// Token: 0x06001E71 RID: 7793 RVA: 0x00016760 File Offset: 0x00014960
	public static Rect GetRect()
	{
		return new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
	}
}
