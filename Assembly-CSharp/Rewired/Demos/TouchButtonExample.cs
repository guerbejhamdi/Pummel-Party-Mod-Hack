using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x020006C8 RID: 1736
	[AddComponentMenu("")]
	[RequireComponent(typeof(Image))]
	public class TouchButtonExample : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x060032BE RID: 12990 RVA: 0x0002280C File Offset: 0x00020A0C
		// (set) Token: 0x060032BF RID: 12991 RVA: 0x00022814 File Offset: 0x00020A14
		public bool isPressed { get; private set; }

		// Token: 0x060032C0 RID: 12992 RVA: 0x0002281D File Offset: 0x00020A1D
		private void Awake()
		{
			if (SystemInfo.deviceType == DeviceType.Handheld)
			{
				this.allowMouseControl = false;
			}
		}

		// Token: 0x060032C1 RID: 12993 RVA: 0x0002282E File Offset: 0x00020A2E
		private void Restart()
		{
			this.isPressed = false;
		}

		// Token: 0x060032C2 RID: 12994 RVA: 0x00022837 File Offset: 0x00020A37
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (!this.allowMouseControl && TouchButtonExample.IsMousePointerId(eventData.pointerId))
			{
				return;
			}
			this.isPressed = true;
		}

		// Token: 0x060032C3 RID: 12995 RVA: 0x00022856 File Offset: 0x00020A56
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (!this.allowMouseControl && TouchButtonExample.IsMousePointerId(eventData.pointerId))
			{
				return;
			}
			this.isPressed = false;
		}

		// Token: 0x060032C4 RID: 12996 RVA: 0x00022875 File Offset: 0x00020A75
		private static bool IsMousePointerId(int id)
		{
			return id == -1 || id == -2 || id == -3;
		}

		// Token: 0x04003113 RID: 12563
		public bool allowMouseControl = true;
	}
}
