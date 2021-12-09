using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020006A2 RID: 1698
	[AddComponentMenu("")]
	public abstract class UIElementInfo : MonoBehaviour, ISelectHandler, IEventSystemHandler
	{
		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06003118 RID: 12568 RVA: 0x00102794 File Offset: 0x00100994
		// (remove) Token: 0x06003119 RID: 12569 RVA: 0x001027CC File Offset: 0x001009CC
		public event Action<GameObject> OnSelectedEvent;

		// Token: 0x0600311A RID: 12570 RVA: 0x00021483 File Offset: 0x0001F683
		public void OnSelect(BaseEventData eventData)
		{
			if (this.OnSelectedEvent != null)
			{
				this.OnSelectedEvent(base.gameObject);
			}
		}

		// Token: 0x0400303F RID: 12351
		public string identifier;

		// Token: 0x04003040 RID: 12352
		public int intData;

		// Token: 0x04003041 RID: 12353
		public Text text;
	}
}
