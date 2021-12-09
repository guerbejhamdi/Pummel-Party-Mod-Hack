using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000681 RID: 1665
	[AddComponentMenu("")]
	public class CustomSlider : Slider, ICustomSelectable, ICancelHandler, IEventSystemHandler
	{
		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06002F99 RID: 12185 RVA: 0x00020895 File Offset: 0x0001EA95
		// (set) Token: 0x06002F9A RID: 12186 RVA: 0x0002089D File Offset: 0x0001EA9D
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

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06002F9B RID: 12187 RVA: 0x000208A6 File Offset: 0x0001EAA6
		// (set) Token: 0x06002F9C RID: 12188 RVA: 0x000208AE File Offset: 0x0001EAAE
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

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06002F9D RID: 12189 RVA: 0x000208B7 File Offset: 0x0001EAB7
		// (set) Token: 0x06002F9E RID: 12190 RVA: 0x000208BF File Offset: 0x0001EABF
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

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06002F9F RID: 12191 RVA: 0x000208C8 File Offset: 0x0001EAC8
		// (set) Token: 0x06002FA0 RID: 12192 RVA: 0x000208D0 File Offset: 0x0001EAD0
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

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06002FA1 RID: 12193 RVA: 0x000208D9 File Offset: 0x0001EAD9
		// (set) Token: 0x06002FA2 RID: 12194 RVA: 0x000208E1 File Offset: 0x0001EAE1
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

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06002FA3 RID: 12195 RVA: 0x000208EA File Offset: 0x0001EAEA
		// (set) Token: 0x06002FA4 RID: 12196 RVA: 0x000208F2 File Offset: 0x0001EAF2
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

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06002FA5 RID: 12197 RVA: 0x000208FB File Offset: 0x0001EAFB
		// (set) Token: 0x06002FA6 RID: 12198 RVA: 0x00020903 File Offset: 0x0001EB03
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

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06002FA7 RID: 12199 RVA: 0x00020746 File Offset: 0x0001E946
		private bool isDisabled
		{
			get
			{
				return !this.IsInteractable();
			}
		}

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06002FA8 RID: 12200 RVA: 0x00100A0C File Offset: 0x000FEC0C
		// (remove) Token: 0x06002FA9 RID: 12201 RVA: 0x00100A44 File Offset: 0x000FEC44
		private event UnityAction _CancelEvent;

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06002FAA RID: 12202 RVA: 0x0002090C File Offset: 0x0001EB0C
		// (remove) Token: 0x06002FAB RID: 12203 RVA: 0x00020915 File Offset: 0x0001EB15
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

		// Token: 0x06002FAC RID: 12204 RVA: 0x00100A7C File Offset: 0x000FEC7C
		public override Selectable FindSelectableOnLeft()
		{
			if ((base.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavLeft)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.left);
			}
			return base.FindSelectableOnLeft();
		}

		// Token: 0x06002FAD RID: 12205 RVA: 0x00100ABC File Offset: 0x000FECBC
		public override Selectable FindSelectableOnRight()
		{
			if ((base.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavRight)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.right);
			}
			return base.FindSelectableOnRight();
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x00100AFC File Offset: 0x000FECFC
		public override Selectable FindSelectableOnUp()
		{
			if ((base.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavUp)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.up);
			}
			return base.FindSelectableOnUp();
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x00100B3C File Offset: 0x000FED3C
		public override Selectable FindSelectableOnDown()
		{
			if ((base.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavDown)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.down);
			}
			return base.FindSelectableOnDown();
		}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x0002091E File Offset: 0x0001EB1E
		protected override void OnCanvasGroupChanged()
		{
			base.OnCanvasGroupChanged();
			if (EventSystem.current == null)
			{
				return;
			}
			this.EvaluateHightlightDisabled(EventSystem.current.currentSelectedGameObject == base.gameObject);
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x00100B7C File Offset: 0x000FED7C
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

		// Token: 0x06002FB2 RID: 12210 RVA: 0x00100824 File Offset: 0x000FEA24
		private void StartColorTween(Color targetColor, bool instant)
		{
			if (base.targetGraphic == null)
			{
				return;
			}
			base.targetGraphic.CrossFadeColor(targetColor, instant ? 0f : base.colors.fadeDuration, true, true);
		}

		// Token: 0x06002FB3 RID: 12211 RVA: 0x0002079E File Offset: 0x0001E99E
		private void DoSpriteSwap(Sprite newSprite)
		{
			if (base.image == null)
			{
				return;
			}
			base.image.overrideSprite = newSprite;
		}

		// Token: 0x06002FB4 RID: 12212 RVA: 0x00100C04 File Offset: 0x000FEE04
		private void TriggerAnimation(string triggername)
		{
			if (base.animator == null || !base.animator.enabled || !base.animator.isActiveAndEnabled || base.animator.runtimeAnimatorController == null || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			base.animator.ResetTrigger(this._disabledHighlightedTrigger);
			base.animator.SetTrigger(triggername);
		}

		// Token: 0x06002FB5 RID: 12213 RVA: 0x0002094F File Offset: 0x0001EB4F
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			this.EvaluateHightlightDisabled(true);
		}

		// Token: 0x06002FB6 RID: 12214 RVA: 0x0002095F File Offset: 0x0001EB5F
		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			this.EvaluateHightlightDisabled(false);
		}

		// Token: 0x06002FB7 RID: 12215 RVA: 0x00100C74 File Offset: 0x000FEE74
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

		// Token: 0x06002FB8 RID: 12216 RVA: 0x0002096F File Offset: 0x0001EB6F
		public void OnCancel(BaseEventData eventData)
		{
			if (this._CancelEvent != null)
			{
				this._CancelEvent();
			}
		}

		// Token: 0x04002F70 RID: 12144
		[SerializeField]
		private Sprite _disabledHighlightedSprite;

		// Token: 0x04002F71 RID: 12145
		[SerializeField]
		private Color _disabledHighlightedColor;

		// Token: 0x04002F72 RID: 12146
		[SerializeField]
		private string _disabledHighlightedTrigger;

		// Token: 0x04002F73 RID: 12147
		[SerializeField]
		private bool _autoNavUp = true;

		// Token: 0x04002F74 RID: 12148
		[SerializeField]
		private bool _autoNavDown = true;

		// Token: 0x04002F75 RID: 12149
		[SerializeField]
		private bool _autoNavLeft = true;

		// Token: 0x04002F76 RID: 12150
		[SerializeField]
		private bool _autoNavRight = true;

		// Token: 0x04002F77 RID: 12151
		private bool isHighlightDisabled;
	}
}
