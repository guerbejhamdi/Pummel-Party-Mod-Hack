using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000658 RID: 1624
	[AddComponentMenu("")]
	public class CanvasScalerExt : CanvasScaler
	{
		// Token: 0x06002CFA RID: 11514 RVA: 0x0001E5F3 File Offset: 0x0001C7F3
		public void ForceRefresh()
		{
			this.Handle();
		}
	}
}
