using System;
using UnityEngine;

// Token: 0x02000233 RID: 563
public class SelfishStrideBridge : MonoBehaviour
{
	// Token: 0x1700016F RID: 367
	// (get) Token: 0x06001050 RID: 4176 RVA: 0x0000DBD6 File Offset: 0x0000BDD6
	// (set) Token: 0x06001051 RID: 4177 RVA: 0x0000DBDE File Offset: 0x0000BDDE
	public int KeyCount { get; set; }

	// Token: 0x06001052 RID: 4178 RVA: 0x0000DBE7 File Offset: 0x0000BDE7
	public void BreakBridge()
	{
		this.bridgeCollider.enabled = false;
		this.breakableBridge.BreakBridge();
	}

	// Token: 0x06001053 RID: 4179 RVA: 0x0000DC00 File Offset: 0x0000BE00
	public void FixBridge()
	{
		this.bridgeCollider.enabled = true;
		this.breakableBridge.FixBridge();
	}

	// Token: 0x040010A9 RID: 4265
	[Header("Stand Positions")]
	public Transform onePlayerStandPosition;

	// Token: 0x040010AA RID: 4266
	public Transform[] multiPlayerStandPositions;

	// Token: 0x040010AB RID: 4267
	[Header("End Position")]
	public Transform endPosition;

	// Token: 0x040010AC RID: 4268
	[Header("Key Spawns")]
	public Transform oneKeySpawnPosition;

	// Token: 0x040010AD RID: 4269
	public Transform[] threeKeySpawnPositions;

	// Token: 0x040010AE RID: 4270
	public Transform[] fiveKeySpawnPositions;

	// Token: 0x040010AF RID: 4271
	[Header("Bridge")]
	public GameObject bridge;

	// Token: 0x040010B0 RID: 4272
	public FX_BreakableBridge breakableBridge;

	// Token: 0x040010B1 RID: 4273
	public Collider bridgeCollider;
}
