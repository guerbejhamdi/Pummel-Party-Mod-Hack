using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x020006D1 RID: 1745
	[AddComponentMenu("")]
	[RequireComponent(typeof(RectTransform))]
	public sealed class UIPointer : UIBehaviour
	{
		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06003303 RID: 13059 RVA: 0x00022AC2 File Offset: 0x00020CC2
		// (set) Token: 0x06003304 RID: 13060 RVA: 0x00022ACA File Offset: 0x00020CCA
		public bool autoSort
		{
			get
			{
				return this._autoSort;
			}
			set
			{
				if (value == this._autoSort)
				{
					return;
				}
				this._autoSort = value;
				if (value)
				{
					base.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06003305 RID: 13061 RVA: 0x0010962C File Offset: 0x0010782C
		protected override void Awake()
		{
			base.Awake();
			Graphic[] componentsInChildren = base.GetComponentsInChildren<Graphic>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].raycastTarget = false;
			}
			if (this._hideHardwarePointer)
			{
				Cursor.visible = false;
			}
			if (this._autoSort)
			{
				base.transform.SetAsLastSibling();
			}
			this.GetDependencies();
		}

		// Token: 0x06003306 RID: 13062 RVA: 0x00022AEB File Offset: 0x00020CEB
		private void Update()
		{
			if (this._autoSort && base.transform.GetSiblingIndex() < base.transform.parent.childCount - 1)
			{
				base.transform.SetAsLastSibling();
			}
		}

		// Token: 0x06003307 RID: 13063 RVA: 0x00022B1F File Offset: 0x00020D1F
		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			this.GetDependencies();
		}

		// Token: 0x06003308 RID: 13064 RVA: 0x00022B2D File Offset: 0x00020D2D
		protected override void OnCanvasGroupChanged()
		{
			base.OnCanvasGroupChanged();
			this.GetDependencies();
		}

		// Token: 0x06003309 RID: 13065 RVA: 0x00109684 File Offset: 0x00107884
		public void OnScreenPositionChanged(Vector2 screenPosition)
		{
			if (this._canvas == null)
			{
				return;
			}
			Camera cam = null;
			RenderMode renderMode = this._canvas.renderMode;
			if (renderMode != RenderMode.ScreenSpaceOverlay && renderMode - RenderMode.ScreenSpaceCamera <= 1)
			{
				cam = this._canvas.worldCamera;
			}
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform.parent as RectTransform, screenPosition, cam, out vector);
			base.transform.localPosition = new Vector3(vector.x, vector.y, base.transform.localPosition.z);
		}

		// Token: 0x0600330A RID: 13066 RVA: 0x00022B3B File Offset: 0x00020D3B
		private void GetDependencies()
		{
			this._canvas = base.transform.root.GetComponentInChildren<Canvas>();
		}

		// Token: 0x0400314B RID: 12619
		[Tooltip("Should the hardware pointer be hidden?")]
		[SerializeField]
		private bool _hideHardwarePointer = true;

		// Token: 0x0400314C RID: 12620
		[Tooltip("Sets the pointer to the last sibling in the parent hierarchy. Do not enable this on multiple UIPointers under the same parent transform or they will constantly fight each other for dominance.")]
		[SerializeField]
		private bool _autoSort = true;

		// Token: 0x0400314D RID: 12621
		private Canvas _canvas;
	}
}
