using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003C4 RID: 964
public class BoardNodeEvent : MonoBehaviour
{
	// Token: 0x06001984 RID: 6532 RVA: 0x00012F25 File Offset: 0x00011125
	public virtual IEnumerator DoEvent(BoardPlayer player, BoardNode node, int seed)
	{
		yield return null;
		yield break;
	}

	// Token: 0x06001985 RID: 6533 RVA: 0x00005651 File Offset: 0x00003851
	public virtual bool EndTurnAfterEvent(BoardNode node)
	{
		return true;
	}

	// Token: 0x04001B30 RID: 6960
	public System.Random rand;
}
