using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000682 RID: 1666
	[AddComponentMenu("")]
	public class CustomToggle : Toggle, ICustomSelectable, ICancelHandler, IEventSystemHandler
	{
		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06002FBA RID: 12218 RVA: 0x000209A8 File Offset: 0x0001EBA8
		// (set) Token: 0x06002FBB RID: 12219 RVA: 0x000209B0 File Offset: 0x0001EBB0
		public Sprite disabledHighlightedSprite
		{
			get
			{
				return this._disabledHighlightedSprite;
			}
			set
			{
				this._disabledHighlightedSprite = value;
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06002FBC RID: 12220 RVA: 0x000209B9 File Offset: 0x0001EBB9
		// (set) Token: 0x06002FBD RID: 12221 RVA: 0x000209C1 File Offset: 0x0001EBC1
		public Color disabledHighlightedColor
		{
			get
			{
				return this._disabledHighlightedColor;
			}
			set
			{
				this._disabledHighlightedColor = value;
			}
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06002FBE RID: 12222 RVA: 0x000209CA File Offset: 0x0001EBCA
		// (set) Token: 0x06002FBF RID: 12223 RVA: 0x000209D2 File Offset: 0x0001EBD2
		public string disabledHighlightedTrigger
		{
			get
			{
				return this._disabledHighlightedTrigger;
			}
			set
			{
				this._disabledHighlightedTrigger = value;
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06002FC0 RID: 12224 RVA: 0x000209DB File Offset: 0x0001EBDB
		// (set) Token: 0x06002FC1 RID: 12225 RVA: 0x000209E3 File Offset: 0x0001EBE3
		public bool autoNavUp
		{
			get
			{
				return this._autoNavUp;
			}
			set
			{
				this._autoNavUp = value;
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06002FC2 RID: 12226 RVA: 0x000209EC File Offset: 0x0001EBEC
		// (set) Token: 0x06002FC3 RID: 12227 RVA: 0x000209F4 File Offset: 0x0001EBF4
		public bool autoNavDown
		{
			get
			{
				return this._autoNavDown;
			}
			set
			{
				this._autoNavDown = value;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06002FC4 RID: 12228 RVA: 0x000209FD File Offset: 0x0001EBFD
		// (set) Token: 0x06002FC5 RID: 12229 RVA: 0x00020A05 File Offset: 0x0001EC05
		public bool autoNavLeft
		{
			get
			{
				return this._autoNavLeft;
			}
			set
			{
				this._autoNavLeft = value;
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06002FC6 RID: 12230 RVA: 0x00020A0E File Offset: 0x0001EC0E
		// (set) Token: 0x06002FC7 RID: 12231 RVA: 0x00020A16 File Offset: 0x0001EC16
		public bool autoNavRight
		{
			get
			{
				return this._autoNavRight;
			}
			set
			{
				this._autoNavRight = value;
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06002FC8 RID: 12232 RVA: 0x00020746 File Offset: 0x0001E946
		private bool isDisabled
		{
			get
			{
				return !this.IsInteractable();
			}
		}

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06002FC9 RID: 12233 RVA: 0x00100CC8 File Offset: 0x000FEEC8
		// (remove) Token: 0x06002FCA RID: 12234 RVA: 0x00100D00 File Offset: 0x000FEF00
		private event UnityAction _CancelEvent;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06002FCB RID: 12235 RVA: 0x00020A1F File Offset: 0x0001EC1F
		// (remove) Token: 0x06002FCC RID: 12236 RVA: 0x00020A28 File Offset: 0x0001EC28
		public event UnityAction CancelEvent
		{
			add
			{
				this._CancelEvent += value;
			}
			remove
			{
				this._CancelEvent -= value;
			}
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x00100D38 File Offset: 0x000FEF38
		public override Selectable FindSelectableOnLeft()
		{
			if ((base.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavLeft)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.left);
			}
			return base.FindSelectableOnLeft();
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x00100D78 File Offset: 0x000FEF78
		public override Selectable FindSelectableOnRight()
		{
			if ((base.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavRight)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.right);
			}
			return base.FindSelectableOnRight();
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x00100DB8 File Offset: 0x000FEFB8
		public override Selectable FindSelectableOnUp()
		{
			if ((base.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavUp)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.up);
			}
			return base.FindSelectableOnUp();
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x00100DF8 File Offset: 0x000FEFF8
		public override Selectable FindSelectableOnDown()
		{
			if ((base.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavDown)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.down);
			}
			return base.FindSelectableOnDown();
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x00020A31 File Offset: 0x0001EC31
		protected override void OnCanvasGroupChanged()
		{
			base.OnCanvasGroupChanged();
			if (EventSystem.current == null)
			{
				return;
			}
			this.EvaluateHightlightDisabled(EventSystem.current.currentSelectedGameObject == base.gameObject);
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x00100E38 File Offset: 0x000FF038
		protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
		{
			if (this.isHighlightDisabled)
			{
				Color disabledHighlightedColor = this._disabledHighlightedColor;
				Sprite disabledHighlightedSprite = this._disabledHighlightedSprite;
				string disabledHighlightedTrigger = this._disabledHighlightedTrigger;
				if (base.gameObject.activeInHierarchy)
				{
					switch (base.transition)
					{
					case Selectable.Transition.ColorTint:
						this.StartColorTween(disabledHighlightedColor * base.colors.colorMultiplier, instant);
						return;
					case Selectable.Transition.SpriteSwap:
						this.DoSpriteSwap(disabledHighlightedSprite);
						return;
					case Selectable.Transition.Animation:
						this.TriggerAnimation(disabledHighlightedTrigger);
						return;
					default:
						return;
					}
				}
			}
			else
			{
				base.DoStateTransition(state, instant);
			}
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x00100824 File Offset: 0x000FEA24
		private void StartColorTween(Color targetColor, bool instant)
		{
			if (base.targetGraphic == null)
			{
				return;
			}
			base.targetGraphic.CrossFadeColor(targetColor, instant ? 0f : base.colors.fadeDuration, true, true);
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x0002079E File Offset: 0x0001E99E
		private void DoSpriteSwap(Sprite newSprite)
		{
			if (base.image == null)
			{
				return;
			}
			base.image.overrideSprite = newSprite;
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x00100EC0 File Offset: 0x000FF0C0
		private void TriggerAnimation(string triggername)
		{
			if (base.animator == null || !base.animator.enabled || !base.animator.isActiveAndEnabled || base.animator.runtimeAnimatorController == null || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			base.animator.ResetTrigger(this._disabledHighlightedTrigger);
			base.animator.SetTrigger(triggername);
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x00020A62 File Offset: 0x0001EC62
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			this.EvaluateHightlightDisabled(true);
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x00020A72 File Offset: 0x0001EC72
		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			this.EvaluateHightlightDisabled(false);
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x00100F30 File Offset: 0x000FF130
		private void EvaluateHightlightDisabled(bool isSelected)
		{
			if (!isSelected)
			{
				if (this.isHighlightDisabled)
				{
					this.isHighlightDisabled = false;
					Selectable.SelectionState state = this.isDisabled ? Selectable.SelectionState.Disabled : base.currentSelectionState;
					this.DoStateTransition(state, false);
					return;
				}
			}
			else
			{
				if (!this.isDisabled)
				{
					return;
				}
				this.isHighlightDisabled = true;
				this.DoStateTransition(Selectable.SelectionState.Disabled, false);
			}
		}

		// Token: 0x06002FD9 RID: 12249 RVA: 0x00020A82 File Offset: 0x0001EC82
		public void OnCancel(BaseEventData eventData)
		{
			if (this._CancelEvent != null)
			{
				this._CancelEvent();
			}
		}

		// Token: 0x04002F79 RID: 12153
		[SerializeField]
		private Sprite _disabledHighlightedSprite;

		// Token: 0x04002F7A RID: 12154
		[SerializeField]
		private Color _disabledHighlightedColor;

		// Token: 0x04002F7B RID: 12155
		[SerializeField]
		private string _disabledHighlightedTrigger;

		// Token: 0x04002F7C RID: 12156
		[SerializeField]
		private bool _autoNavUp = true;

		// Token: 0x04002F7D RID: 12157
		[SerializeField]
		private bool _autoNavDown = true;

		// Token: 0x04002F7E RID: 12158
		[SerializeField]
		private bool _autoNavLeft = true;

		// Token: 0x04002F7F RID: 12159
		[SerializeField]
		private bool _autoNavRight = true;

		// Token: 0x04002F80 RID: 12160
		private bool isHighlightDisabled;
	}
}
