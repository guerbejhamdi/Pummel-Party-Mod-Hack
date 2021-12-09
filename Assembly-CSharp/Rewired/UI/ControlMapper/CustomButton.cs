using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x0200067F RID: 1663
	[AddComponentMenu("")]
	public class CustomButton : Button, ICustomSelectable, ICancelHandler, IEventSystemHandler
	{
		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06002F6E RID: 12142 RVA: 0x000206CF File Offset: 0x0001E8CF
		// (set) Token: 0x06002F6F RID: 12143 RVA: 0x000206D7 File Offset: 0x0001E8D7
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

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06002F70 RID: 12144 RVA: 0x000206E0 File Offset: 0x0001E8E0
		// (set) Token: 0x06002F71 RID: 12145 RVA: 0x000206E8 File Offset: 0x0001E8E8
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

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06002F72 RID: 12146 RVA: 0x000206F1 File Offset: 0x0001E8F1
		// (set) Token: 0x06002F73 RID: 12147 RVA: 0x000206F9 File Offset: 0x0001E8F9
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

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06002F74 RID: 12148 RVA: 0x00020702 File Offset: 0x0001E902
		// (set) Token: 0x06002F75 RID: 12149 RVA: 0x0002070A File Offset: 0x0001E90A
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

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06002F76 RID: 12150 RVA: 0x00020713 File Offset: 0x0001E913
		// (set) Token: 0x06002F77 RID: 12151 RVA: 0x0002071B File Offset: 0x0001E91B
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

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06002F78 RID: 12152 RVA: 0x00020724 File Offset: 0x0001E924
		// (set) Token: 0x06002F79 RID: 12153 RVA: 0x0002072C File Offset: 0x0001E92C
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

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06002F7A RID: 12154 RVA: 0x00020735 File Offset: 0x0001E935
		// (set) Token: 0x06002F7B RID: 12155 RVA: 0x0002073D File Offset: 0x0001E93D
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

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06002F7C RID: 12156 RVA: 0x00020746 File Offset: 0x0001E946
		private bool isDisabled
		{
			get
			{
				return !this.IsInteractable();
			}
		}

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06002F7D RID: 12157 RVA: 0x001006B4 File Offset: 0x000FE8B4
		// (remove) Token: 0x06002F7E RID: 12158 RVA: 0x001006EC File Offset: 0x000FE8EC
		private event UnityAction _CancelEvent;

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06002F7F RID: 12159 RVA: 0x00020751 File Offset: 0x0001E951
		// (remove) Token: 0x06002F80 RID: 12160 RVA: 0x0002075A File Offset: 0x0001E95A
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

		// Token: 0x06002F81 RID: 12161 RVA: 0x00100724 File Offset: 0x000FE924
		public override Selectable FindSelectableOnLeft()
		{
			if ((base.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavLeft)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.left);
			}
			return base.FindSelectableOnLeft();
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x00100764 File Offset: 0x000FE964
		public override Selectable FindSelectableOnRight()
		{
			if ((base.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavRight)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.right);
			}
			return base.FindSelectableOnRight();
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x001007A4 File Offset: 0x000FE9A4
		public override Selectable FindSelectableOnUp()
		{
			if ((base.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavUp)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.up);
			}
			return base.FindSelectableOnUp();
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x001007E4 File Offset: 0x000FE9E4
		public override Selectable FindSelectableOnDown()
		{
			if ((base.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavDown)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.down);
			}
			return base.FindSelectableOnDown();
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x00020763 File Offset: 0x0001E963
		protected override void OnCanvasGroupChanged()
		{
			base.OnCanvasGroupChanged();
			if (EventSystem.current == null)
			{
				return;
			}
			this.EvaluateHightlightDisabled(EventSystem.current.currentSelectedGameObject == base.gameObject);
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x00020794 File Offset: 0x0001E994
		protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
		{
			base.DoStateTransition(state, instant);
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x00100824 File Offset: 0x000FEA24
		private void StartColorTween(Color targetColor, bool instant)
		{
			if (base.targetGraphic == null)
			{
				return;
			}
			base.targetGraphic.CrossFadeColor(targetColor, instant ? 0f : base.colors.fadeDuration, true, true);
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x0002079E File Offset: 0x0001E99E
		private void DoSpriteSwap(Sprite newSprite)
		{
			if (base.image == null)
			{
				return;
			}
			base.image.overrideSprite = newSprite;
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x00100868 File Offset: 0x000FEA68
		private void TriggerAnimation(string triggername)
		{
			if (base.animator == null || !base.animator.enabled || !base.animator.isActiveAndEnabled || base.animator.runtimeAnimatorController == null || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			base.animator.ResetTrigger(this._disabledHighlightedTrigger);
			base.animator.SetTrigger(triggername);
		}

		// Token: 0x06002F8A RID: 12170 RVA: 0x000207BB File Offset: 0x0001E9BB
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			this.EvaluateHightlightDisabled(true);
		}

		// Token: 0x06002F8B RID: 12171 RVA: 0x000207CB File Offset: 0x0001E9CB
		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			this.EvaluateHightlightDisabled(false);
		}

		// Token: 0x06002F8C RID: 12172 RVA: 0x000207DB File Offset: 0x0001E9DB
		private void Press()
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			base.onClick.Invoke();
		}

		// Token: 0x06002F8D RID: 12173 RVA: 0x001008D8 File Offset: 0x000FEAD8
		public override void OnPointerClick(PointerEventData eventData)
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.Press();
			if (!this.IsActive() || !this.IsInteractable())
			{
				this.isHighlightDisabled = true;
				this.DoStateTransition(Selectable.SelectionState.Disabled, false);
			}
		}

		// Token: 0x06002F8E RID: 12174 RVA: 0x000207F9 File Offset: 0x0001E9F9
		public override void OnSubmit(BaseEventData eventData)
		{
			this.Press();
			if (!this.IsActive() || !this.IsInteractable())
			{
				this.isHighlightDisabled = true;
				this.DoStateTransition(Selectable.SelectionState.Disabled, false);
				return;
			}
			this.DoStateTransition(Selectable.SelectionState.Pressed, false);
			base.StartCoroutine(this.OnFinishSubmit());
		}

		// Token: 0x06002F8F RID: 12175 RVA: 0x00020836 File Offset: 0x0001EA36
		private IEnumerator OnFinishSubmit()
		{
			float fadeTime = base.colors.fadeDuration;
			float elapsedTime = 0f;
			while (elapsedTime < fadeTime)
			{
				elapsedTime += Time.unscaledDeltaTime;
				yield return null;
			}
			this.DoStateTransition(base.currentSelectionState, false);
			yield break;
		}

		// Token: 0x06002F90 RID: 12176 RVA: 0x00100924 File Offset: 0x000FEB24
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

		// Token: 0x06002F91 RID: 12177 RVA: 0x00020845 File Offset: 0x0001EA45
		public void OnCancel(BaseEventData eventData)
		{
			if (this._CancelEvent != null)
			{
				this._CancelEvent();
			}
		}

		// Token: 0x04002F62 RID: 12130
		[SerializeField]
		private Sprite _disabledHighlightedSprite;

		// Token: 0x04002F63 RID: 12131
		[SerializeField]
		private Color _disabledHighlightedColor;

		// Token: 0x04002F64 RID: 12132
		[SerializeField]
		private string _disabledHighlightedTrigger;

		// Token: 0x04002F65 RID: 12133
		[SerializeField]
		private bool _autoNavUp = true;

		// Token: 0x04002F66 RID: 12134
		[SerializeField]
		private bool _autoNavDown = true;

		// Token: 0x04002F67 RID: 12135
		[SerializeField]
		private bool _autoNavLeft = true;

		// Token: 0x04002F68 RID: 12136
		[SerializeField]
		private bool _autoNavRight = true;

		// Token: 0x04002F69 RID: 12137
		private bool isHighlightDisabled;
	}
}
