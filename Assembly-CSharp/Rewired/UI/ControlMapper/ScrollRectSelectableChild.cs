using System;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x0200068F RID: 1679
	[AddComponentMenu("")]
	[RequireComponent(typeof(Selectable))]
	public class ScrollRectSelectableChild : MonoBehaviour, ISelectHandler, IEventSystemHandler
	{
		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x060030A4 RID: 12452 RVA: 0x0002106E File Offset: 0x0001F26E
		private RectTransform parentScrollRectContentTransform
		{
			get
			{
				return this.parentScrollRect.content;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x060030A5 RID: 12453 RVA: 0x00101B9C File Offset: 0x000FFD9C
		private Selectable selectable
		{
			get
			{
				Selectable result;
				if ((result = this._selectable) == null)
				{
					result = (this._selectable = base.GetComponent<Selectable>());
				}
				return result;
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x060030A6 RID: 12454 RVA: 0x0002107B File Offset: 0x0001F27B
		private RectTransform rectTransform
		{
			get
			{
				return base.transform as RectTransform;
			}
		}

		// Token: 0x060030A7 RID: 12455 RVA: 0x00021088 File Offset: 0x0001F288
		private void Start()
		{
			this.parentScrollRect = base.transform.GetComponentInParent<ScrollRect>();
			if (this.parentScrollRect == null)
			{
				Debug.LogError("Rewired Control Mapper: No ScrollRect found! This component must be a child of a ScrollRect!");
				return;
			}
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x00101BC4 File Offset: 0x000FFDC4
		public void OnSelect(BaseEventData eventData)
		{
			if (this.parentScrollRect == null)
			{
				return;
			}
			if (!(eventData is AxisEventData))
			{
				return;
			}
			RectTransform rectTransform = this.parentScrollRect.transform as RectTransform;
			Rect child = MathTools.TransformRect(this.rectTransform.rect, this.rectTransform, rectTransform);
			Rect rect = rectTransform.rect;
			Rect rect2 = rectTransform.rect;
			float height;
			if (this.useCustomEdgePadding)
			{
				height = this.customEdgePadding;
			}
			else
			{
				height = child.height;
			}
			rect2.yMax -= height;
			rect2.yMin += height;
			if (MathTools.RectContains(rect2, child))
			{
				return;
			}
			Vector2 vector;
			if (!MathTools.GetOffsetToContainRect(rect2, child, out vector))
			{
				return;
			}
			Vector2 anchoredPosition = this.parentScrollRectContentTransform.anchoredPosition;
			anchoredPosition.x = Mathf.Clamp(anchoredPosition.x + vector.x, 0f, Mathf.Abs(rect.width - this.parentScrollRectContentTransform.sizeDelta.x));
			anchoredPosition.y = Mathf.Clamp(anchoredPosition.y + vector.y, 0f, Mathf.Abs(rect.height - this.parentScrollRectContentTransform.sizeDelta.y));
			this.parentScrollRectContentTransform.anchoredPosition = anchoredPosition;
		}

		// Token: 0x04002FE9 RID: 12265
		public bool useCustomEdgePadding;

		// Token: 0x04002FEA RID: 12266
		public float customEdgePadding = 50f;

		// Token: 0x04002FEB RID: 12267
		private ScrollRect parentScrollRect;

		// Token: 0x04002FEC RID: 12268
		private Selectable _selectable;
	}
}
