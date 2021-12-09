using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x020006C9 RID: 1737
	[AddComponentMenu("")]
	[RequireComponent(typeof(Image))]
	public class TouchJoystickExample : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler
	{
		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x060032C6 RID: 12998 RVA: 0x00022896 File Offset: 0x00020A96
		// (set) Token: 0x060032C7 RID: 12999 RVA: 0x0002289E File Offset: 0x00020A9E
		public Vector2 position { get; private set; }

		// Token: 0x060032C8 RID: 13000 RVA: 0x000228A7 File Offset: 0x00020AA7
		private void Start()
		{
			if (SystemInfo.deviceType == DeviceType.Handheld)
			{
				this.allowMouseControl = false;
			}
			this.StoreOrigValues();
		}

		// Token: 0x060032C9 RID: 13001 RVA: 0x00108250 File Offset: 0x00106450
		private void Update()
		{
			if ((float)Screen.width != this.origScreenResolution.x || (float)Screen.height != this.origScreenResolution.y || Screen.orientation != this.origScreenOrientation)
			{
				this.Restart();
				this.StoreOrigValues();
			}
		}

		// Token: 0x060032CA RID: 13002 RVA: 0x000228BE File Offset: 0x00020ABE
		private void Restart()
		{
			this.hasFinger = false;
			(base.transform as RectTransform).anchoredPosition = this.origAnchoredPosition;
			this.position = Vector2.zero;
		}

		// Token: 0x060032CB RID: 13003 RVA: 0x0010829C File Offset: 0x0010649C
		private void StoreOrigValues()
		{
			this.origAnchoredPosition = (base.transform as RectTransform).anchoredPosition;
			this.origWorldPosition = base.transform.position;
			this.origScreenResolution = new Vector2((float)Screen.width, (float)Screen.height);
			this.origScreenOrientation = Screen.orientation;
		}

		// Token: 0x060032CC RID: 13004 RVA: 0x001082F4 File Offset: 0x001064F4
		private void UpdateValue(Vector3 value)
		{
			Vector3 vector = this.origWorldPosition - value;
			vector.y = -vector.y;
			vector /= (float)this.radius;
			this.position = new Vector2(-vector.x, vector.y);
		}

		// Token: 0x060032CD RID: 13005 RVA: 0x000228E8 File Offset: 0x00020AE8
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (this.hasFinger)
			{
				return;
			}
			if (!this.allowMouseControl && TouchJoystickExample.IsMousePointerId(eventData.pointerId))
			{
				return;
			}
			this.hasFinger = true;
			this.lastFingerId = eventData.pointerId;
		}

		// Token: 0x060032CE RID: 13006 RVA: 0x0002291C File Offset: 0x00020B1C
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (eventData.pointerId != this.lastFingerId)
			{
				return;
			}
			if (!this.allowMouseControl && TouchJoystickExample.IsMousePointerId(eventData.pointerId))
			{
				return;
			}
			this.Restart();
		}

		// Token: 0x060032CF RID: 13007 RVA: 0x00108344 File Offset: 0x00106544
		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			if (!this.hasFinger || eventData.pointerId != this.lastFingerId)
			{
				return;
			}
			Vector3 vector = new Vector3(eventData.position.x - this.origWorldPosition.x, eventData.position.y - this.origWorldPosition.y);
			vector = Vector3.ClampMagnitude(vector, (float)this.radius);
			Vector3 vector2 = this.origWorldPosition + vector;
			base.transform.position = vector2;
			this.UpdateValue(vector2);
		}

		// Token: 0x060032D0 RID: 13008 RVA: 0x00022875 File Offset: 0x00020A75
		private static bool IsMousePointerId(int id)
		{
			return id == -1 || id == -2 || id == -3;
		}

		// Token: 0x04003115 RID: 12565
		public bool allowMouseControl = true;

		// Token: 0x04003116 RID: 12566
		public int radius = 50;

		// Token: 0x04003117 RID: 12567
		private Vector2 origAnchoredPosition;

		// Token: 0x04003118 RID: 12568
		private Vector3 origWorldPosition;

		// Token: 0x04003119 RID: 12569
		private Vector2 origScreenResolution;

		// Token: 0x0400311A RID: 12570
		private ScreenOrientation origScreenOrientation;

		// Token: 0x0400311B RID: 12571
		[NonSerialized]
		private bool hasFinger;

		// Token: 0x0400311C RID: 12572
		[NonSerialized]
		private int lastFingerId;
	}
}
