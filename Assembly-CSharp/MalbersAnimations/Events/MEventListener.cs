using System;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations.Events
{
	// Token: 0x02000778 RID: 1912
	public class MEventListener : MonoBehaviour
	{
		// Token: 0x060036C6 RID: 14022 RVA: 0x00117270 File Offset: 0x00115470
		private void OnEnable()
		{
			foreach (MEventItemListener meventItemListener in this.Events)
			{
				if (meventItemListener.Event)
				{
					meventItemListener.Event.RegisterListener(meventItemListener);
				}
			}
		}

		// Token: 0x060036C7 RID: 14023 RVA: 0x001172D8 File Offset: 0x001154D8
		private void OnDisable()
		{
			foreach (MEventItemListener meventItemListener in this.Events)
			{
				if (meventItemListener.Event)
				{
					meventItemListener.Event.UnregisterListener(meventItemListener);
				}
			}
		}

		// Token: 0x040035FF RID: 13823
		public List<MEventItemListener> Events = new List<MEventItemListener>();
	}
}
