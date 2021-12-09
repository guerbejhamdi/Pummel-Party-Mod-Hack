using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020006A7 RID: 1703
	[AddComponentMenu("")]
	public class UISliderControl : UIControl
	{
		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x0600312C RID: 12588 RVA: 0x00021552 File Offset: 0x0001F752
		// (set) Token: 0x0600312D RID: 12589 RVA: 0x0002155A File Offset: 0x0001F75A
		public bool showIcon
		{
			get
			{
				return this._showIcon;
			}
			set
			{
				if (this.iconImage == null)
				{
					return;
				}
				this.iconImage.gameObject.SetActive(value);
				this._showIcon = value;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x0600312E RID: 12590 RVA: 0x00021583 File Offset: 0x0001F783
		// (set) Token: 0x0600312F RID: 12591 RVA: 0x0002158B File Offset: 0x0001F78B
		public bool showSlider
		{
			get
			{
				return this._showSlider;
			}
			set
			{
				if (this.slider == null)
				{
					return;
				}
				this.slider.gameObject.SetActive(value);
				this._showSlider = value;
			}
		}

		// Token: 0x06003130 RID: 12592 RVA: 0x00102AC4 File Offset: 0x00100CC4
		public override void SetCancelCallback(Action cancelCallback)
		{
			base.SetCancelCallback(cancelCallback);
			if (cancelCallback == null || this.slider == null)
			{
				return;
			}
			if (this.slider is ICustomSelectable)
			{
				(this.slider as ICustomSelectable).CancelEvent += delegate()
				{
					cancelCallback();
				};
				return;
			}
			EventTrigger eventTrigger = this.slider.GetComponent<EventTrigger>();
			if (eventTrigger == null)
			{
				eventTrigger = this.slider.gameObject.AddComponent<EventTrigger>();
			}
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.callback = new EventTrigger.TriggerEvent();
			entry.eventID = EventTriggerType.Cancel;
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				cancelCallback();
			});
			if (eventTrigger.triggers == null)
			{
				eventTrigger.triggers = new List<EventTrigger.Entry>();
			}
			eventTrigger.triggers.Add(entry);
		}

		// Token: 0x0400304A RID: 12362
		public Image iconImage;

		// Token: 0x0400304B RID: 12363
		public Slider slider;

		// Token: 0x0400304C RID: 12364
		private bool _showIcon;

		// Token: 0x0400304D RID: 12365
		private bool _showSlider;
	}
}
