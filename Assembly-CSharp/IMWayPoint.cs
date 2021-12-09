using System;
using UnityEngine;

// Token: 0x0200012A RID: 298
internal interface IMWayPoint
{
	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x060008B7 RID: 2231
	Transform NextTarget { get; }

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x060008B8 RID: 2232
	float StoppinDistance { get; }
}
