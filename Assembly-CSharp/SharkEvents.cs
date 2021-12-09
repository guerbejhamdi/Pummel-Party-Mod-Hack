using System;
using UnityEngine;

// Token: 0x0200049D RID: 1181
public class SharkEvents : MonoBehaviour
{
	// Token: 0x06001F94 RID: 8084 RVA: 0x000172B4 File Offset: 0x000154B4
	public void AttackHit()
	{
		this.sharkPlayer.AttackHit();
	}

	// Token: 0x0400226A RID: 8810
	public FinderPlayerShark sharkPlayer;
}
