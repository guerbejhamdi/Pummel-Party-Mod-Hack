using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000737 RID: 1847
	internal interface IWayPoint
	{
		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x060035BC RID: 13756
		float StoppingDistance { get; }

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x060035BD RID: 13757
		Transform NextTarget { get; }

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x060035BE RID: 13758
		float WaitTime { get; }

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x060035BF RID: 13759
		WayPointType PointType { get; }
	}
}
