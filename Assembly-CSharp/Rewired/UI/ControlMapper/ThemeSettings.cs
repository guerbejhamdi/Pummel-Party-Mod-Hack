using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000691 RID: 1681
	[Serializable]
	public class ThemeSettings : ScriptableObject
	{
		// Token: 0x060030AB RID: 12459 RVA: 0x00101D08 File Offset: 0x000FFF08
		public void Apply(ThemedElement.ElementInfo[] elementInfo)
		{
			if (elementInfo == null)
			{
				return;
			}
			for (int i = 0; i < elementInfo.Length; i++)
			{
				if (elementInfo[i] != null)
				{
					this.Apply(elementInfo[i].themeClass, elementInfo[i].component);
				}
			}
		}

		// Token: 0x060030AC RID: 12460 RVA: 0x00101D44 File Offset: 0x000FFF44
		private void Apply(string themeClass, Component component)
		{
			if (component as Selectable != null)
			{
				this.Apply(themeClass, (Selectable)component);
				return;
			}
			if (component as Image != null)
			{
				this.Apply(themeClass, (Image)component);
				return;
			}
			if (component as Text != null)
			{
				this.Apply(themeClass, (Text)component);
				return;
			}
			if (component as UIImageHelper != null)
			{
				this.Apply(themeClass, (UIImageHelper)component);
				return;
			}
		}

		// Token: 0x060030AD RID: 12461 RVA: 0x00101DC4 File Offset: 0x000FFFC4
		private void Apply(string themeClass, Selectable item)
		{
			if (item == null)
			{
				return;
			}
			ThemeSettings.SelectableSettings_Base selectableSettings_Base;
			if (item as Button != null)
			{
				if (themeClass != null && themeClass == "inputGridField")
				{
					selectableSettings_Base = this._inputGridFieldSettings;
				}
				else
				{
					selectableSettings_Base = this._buttonSettings;
				}
			}
			else if (item as Scrollbar != null)
			{
				selectableSettings_Base = this._scrollbarSettings;
			}
			else if (item as Slider != null)
			{
				selectableSettings_Base = this._sliderSettings;
			}
			else if (item as Toggle != null)
			{
				if (themeClass != null && themeClass == "button")
				{
					selectableSettings_Base = this._buttonSettings;
				}
				else
				{
					selectableSettings_Base = this._selectableSettings;
				}
			}
			else
			{
				selectableSettings_Base = this._selectableSettings;
			}
			selectableSettings_Base.Apply(item);
		}

		// Token: 0x060030AE RID: 12462 RVA: 0x00101E78 File Offset: 0x00100078
		private void Apply(string themeClass, Image item)
		{
			if (item == null)
			{
				return;
			}
			if (themeClass != null)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(themeClass);
				if (num <= 2822822017U)
				{
					if (num <= 665291243U)
					{
						if (num != 106194061U)
						{
							if (num != 283896133U)
							{
								if (num != 665291243U)
								{
									return;
								}
								if (!(themeClass == "calibrationBackground"))
								{
									return;
								}
								if (this._calibrationBackground != null)
								{
									this._calibrationBackground.CopyTo(item);
									return;
								}
							}
							else
							{
								if (!(themeClass == "popupWindow"))
								{
									return;
								}
								if (this._popupWindowBackground != null)
								{
									this._popupWindowBackground.CopyTo(item);
									return;
								}
							}
						}
						else
						{
							if (!(themeClass == "invertToggleButtonBackground"))
							{
								return;
							}
							if (this._buttonSettings != null)
							{
								this._buttonSettings.imageSettings.CopyTo(item);
							}
						}
					}
					else if (num != 2579191547U)
					{
						if (num != 2601460036U)
						{
							if (num != 2822822017U)
							{
								return;
							}
							if (!(themeClass == "invertToggle"))
							{
								return;
							}
							if (this._invertToggle != null)
							{
								this._invertToggle.CopyTo(item);
								return;
							}
						}
						else
						{
							if (!(themeClass == "area"))
							{
								return;
							}
							if (this._areaBackground != null)
							{
								this._areaBackground.CopyTo(item);
								return;
							}
						}
					}
					else
					{
						if (!(themeClass == "calibrationDeadzone"))
						{
							return;
						}
						if (this._calibrationDeadzone != null)
						{
							this._calibrationDeadzone.CopyTo(item);
							return;
						}
					}
				}
				else if (num <= 3490313510U)
				{
					if (num != 2998767316U)
					{
						if (num != 3338297968U)
						{
							if (num != 3490313510U)
							{
								return;
							}
							if (!(themeClass == "calibrationRawValueMarker"))
							{
								return;
							}
							if (this._calibrationRawValueMarker != null)
							{
								this._calibrationRawValueMarker.CopyTo(item);
								return;
							}
						}
						else
						{
							if (!(themeClass == "calibrationCalibratedZeroMarker"))
							{
								return;
							}
							if (this._calibrationCalibratedZeroMarker != null)
							{
								this._calibrationCalibratedZeroMarker.CopyTo(item);
								return;
							}
						}
					}
					else
					{
						if (!(themeClass == "mainWindow"))
						{
							return;
						}
						if (this._mainWindowBackground != null)
						{
							this._mainWindowBackground.CopyTo(item);
							return;
						}
					}
				}
				else if (num != 3776179782U)
				{
					if (num != 3836396811U)
					{
						if (num != 3911450241U)
						{
							return;
						}
						if (!(themeClass == "invertToggleBackground"))
						{
							return;
						}
						if (this._inputGridFieldSettings != null)
						{
							this._inputGridFieldSettings.imageSettings.CopyTo(item);
							return;
						}
					}
					else
					{
						if (!(themeClass == "calibrationZeroMarker"))
						{
							return;
						}
						if (this._calibrationZeroMarker != null)
						{
							this._calibrationZeroMarker.CopyTo(item);
							return;
						}
					}
				}
				else
				{
					if (!(themeClass == "calibrationValueMarker"))
					{
						return;
					}
					if (this._calibrationValueMarker != null)
					{
						this._calibrationValueMarker.CopyTo(item);
						return;
					}
				}
			}
		}

		// Token: 0x060030AF RID: 12463 RVA: 0x00102110 File Offset: 0x00100310
		private void Apply(string themeClass, Text item)
		{
			if (item == null)
			{
				return;
			}
			ThemeSettings.TextSettings textSettings;
			if (themeClass != null)
			{
				if (themeClass == "button")
				{
					textSettings = this._buttonTextSettings;
					goto IL_42;
				}
				if (themeClass == "inputGridField")
				{
					textSettings = this._inputGridFieldTextSettings;
					goto IL_42;
				}
			}
			textSettings = this._textSettings;
			IL_42:
			if (textSettings.font != null)
			{
				item.font = textSettings.font;
			}
			item.color = textSettings.color;
			item.lineSpacing = textSettings.lineSpacing;
			if (textSettings.sizeMultiplier != 1f)
			{
				item.fontSize = (int)((float)item.fontSize * textSettings.sizeMultiplier);
				item.resizeTextMaxSize = (int)((float)item.resizeTextMaxSize * textSettings.sizeMultiplier);
				item.resizeTextMinSize = (int)((float)item.resizeTextMinSize * textSettings.sizeMultiplier);
			}
			if (textSettings.style != ThemeSettings.FontStyleOverride.Default)
			{
				item.fontStyle = ThemeSettings.GetFontStyle(textSettings.style);
			}
		}

		// Token: 0x060030B0 RID: 12464 RVA: 0x000210C7 File Offset: 0x0001F2C7
		private void Apply(string themeClass, UIImageHelper item)
		{
			if (item == null)
			{
				return;
			}
			item.SetEnabledStateColor(this._invertToggle.color);
			item.SetDisabledStateColor(this._invertToggleDisabledColor);
			item.Refresh();
		}

		// Token: 0x060030B1 RID: 12465 RVA: 0x000210F6 File Offset: 0x0001F2F6
		private static FontStyle GetFontStyle(ThemeSettings.FontStyleOverride style)
		{
			return (FontStyle)(style - 1);
		}

		// Token: 0x04002FEE RID: 12270
		[SerializeField]
		private ThemeSettings.ImageSettings _mainWindowBackground;

		// Token: 0x04002FEF RID: 12271
		[SerializeField]
		private ThemeSettings.ImageSettings _popupWindowBackground;

		// Token: 0x04002FF0 RID: 12272
		[SerializeField]
		private ThemeSettings.ImageSettings _areaBackground;

		// Token: 0x04002FF1 RID: 12273
		[SerializeField]
		private ThemeSettings.SelectableSettings _selectableSettings;

		// Token: 0x04002FF2 RID: 12274
		[SerializeField]
		private ThemeSettings.SelectableSettings _buttonSettings;

		// Token: 0x04002FF3 RID: 12275
		[SerializeField]
		private ThemeSettings.SelectableSettings _inputGridFieldSettings;

		// Token: 0x04002FF4 RID: 12276
		[SerializeField]
		private ThemeSettings.ScrollbarSettings _scrollbarSettings;

		// Token: 0x04002FF5 RID: 12277
		[SerializeField]
		private ThemeSettings.SliderSettings _sliderSettings;

		// Token: 0x04002FF6 RID: 12278
		[SerializeField]
		private ThemeSettings.ImageSettings _invertToggle;

		// Token: 0x04002FF7 RID: 12279
		[SerializeField]
		private Color _invertToggleDisabledColor;

		// Token: 0x04002FF8 RID: 12280
		[SerializeField]
		private ThemeSettings.ImageSettings _calibrationBackground;

		// Token: 0x04002FF9 RID: 12281
		[SerializeField]
		private ThemeSettings.ImageSettings _calibrationValueMarker;

		// Token: 0x04002FFA RID: 12282
		[SerializeField]
		private ThemeSettings.ImageSettings _calibrationRawValueMarker;

		// Token: 0x04002FFB RID: 12283
		[SerializeField]
		private ThemeSettings.ImageSettings _calibrationZeroMarker;

		// Token: 0x04002FFC RID: 12284
		[SerializeField]
		private ThemeSettings.ImageSettings _calibrationCalibratedZeroMarker;

		// Token: 0x04002FFD RID: 12285
		[SerializeField]
		private ThemeSettings.ImageSettings _calibrationDeadzone;

		// Token: 0x04002FFE RID: 12286
		[SerializeField]
		private ThemeSettings.TextSettings _textSettings;

		// Token: 0x04002FFF RID: 12287
		[SerializeField]
		private ThemeSettings.TextSettings _buttonTextSettings;

		// Token: 0x04003000 RID: 12288
		[SerializeField]
		private ThemeSettings.TextSettings _inputGridFieldTextSettings;

		// Token: 0x02000692 RID: 1682
		[Serializable]
		private abstract class SelectableSettings_Base
		{
			// Token: 0x17000871 RID: 2161
			// (get) Token: 0x060030B3 RID: 12467 RVA: 0x000210FB File Offset: 0x0001F2FB
			public Selectable.Transition transition
			{
				get
				{
					return this._transition;
				}
			}

			// Token: 0x17000872 RID: 2162
			// (get) Token: 0x060030B4 RID: 12468 RVA: 0x00021103 File Offset: 0x0001F303
			public ThemeSettings.CustomColorBlock selectableColors
			{
				get
				{
					return this._colors;
				}
			}

			// Token: 0x17000873 RID: 2163
			// (get) Token: 0x060030B5 RID: 12469 RVA: 0x0002110B File Offset: 0x0001F30B
			public ThemeSettings.CustomSpriteState spriteState
			{
				get
				{
					return this._spriteState;
				}
			}

			// Token: 0x17000874 RID: 2164
			// (get) Token: 0x060030B6 RID: 12470 RVA: 0x00021113 File Offset: 0x0001F313
			public ThemeSettings.CustomAnimationTriggers animationTriggers
			{
				get
				{
					return this._animationTriggers;
				}
			}

			// Token: 0x060030B7 RID: 12471 RVA: 0x001021F8 File Offset: 0x001003F8
			public virtual void Apply(Selectable item)
			{
				Selectable.Transition transition = this._transition;
				bool flag = item.transition != transition;
				item.transition = transition;
				ICustomSelectable customSelectable = item as ICustomSelectable;
				if (transition == Selectable.Transition.ColorTint)
				{
					ThemeSettings.CustomColorBlock colors = this._colors;
					colors.fadeDuration = 0f;
					item.colors = colors;
					colors.fadeDuration = this._colors.fadeDuration;
					item.colors = colors;
					if (customSelectable != null)
					{
						customSelectable.disabledHighlightedColor = colors.disabledHighlightedColor;
					}
				}
				else if (transition == Selectable.Transition.SpriteSwap)
				{
					item.spriteState = this._spriteState;
					if (customSelectable != null)
					{
						customSelectable.disabledHighlightedSprite = this._spriteState.disabledHighlightedSprite;
					}
				}
				else if (transition == Selectable.Transition.Animation)
				{
					item.animationTriggers.disabledTrigger = this._animationTriggers.disabledTrigger;
					item.animationTriggers.highlightedTrigger = this._animationTriggers.highlightedTrigger;
					item.animationTriggers.normalTrigger = this._animationTriggers.normalTrigger;
					item.animationTriggers.pressedTrigger = this._animationTriggers.pressedTrigger;
					if (customSelectable != null)
					{
						customSelectable.disabledHighlightedTrigger = this._animationTriggers.disabledHighlightedTrigger;
					}
				}
				if (flag)
				{
					item.targetGraphic.CrossFadeColor(item.targetGraphic.color, 0f, true, true);
				}
			}

			// Token: 0x04003001 RID: 12289
			[SerializeField]
			protected Selectable.Transition _transition;

			// Token: 0x04003002 RID: 12290
			[SerializeField]
			protected ThemeSettings.CustomColorBlock _colors;

			// Token: 0x04003003 RID: 12291
			[SerializeField]
			protected ThemeSettings.CustomSpriteState _spriteState;

			// Token: 0x04003004 RID: 12292
			[SerializeField]
			protected ThemeSettings.CustomAnimationTriggers _animationTriggers;
		}

		// Token: 0x02000693 RID: 1683
		[Serializable]
		private class SelectableSettings : ThemeSettings.SelectableSettings_Base
		{
			// Token: 0x17000875 RID: 2165
			// (get) Token: 0x060030B9 RID: 12473 RVA: 0x0002111B File Offset: 0x0001F31B
			public ThemeSettings.ImageSettings imageSettings
			{
				get
				{
					return this._imageSettings;
				}
			}

			// Token: 0x060030BA RID: 12474 RVA: 0x00021123 File Offset: 0x0001F323
			public override void Apply(Selectable item)
			{
				if (item == null)
				{
					return;
				}
				base.Apply(item);
				if (this._imageSettings != null)
				{
					this._imageSettings.CopyTo(item.targetGraphic as Image);
				}
			}

			// Token: 0x04003005 RID: 12293
			[SerializeField]
			private ThemeSettings.ImageSettings _imageSettings;
		}

		// Token: 0x02000694 RID: 1684
		[Serializable]
		private class SliderSettings : ThemeSettings.SelectableSettings_Base
		{
			// Token: 0x17000876 RID: 2166
			// (get) Token: 0x060030BC RID: 12476 RVA: 0x0002115C File Offset: 0x0001F35C
			public ThemeSettings.ImageSettings handleImageSettings
			{
				get
				{
					return this._handleImageSettings;
				}
			}

			// Token: 0x17000877 RID: 2167
			// (get) Token: 0x060030BD RID: 12477 RVA: 0x00021164 File Offset: 0x0001F364
			public ThemeSettings.ImageSettings fillImageSettings
			{
				get
				{
					return this._fillImageSettings;
				}
			}

			// Token: 0x17000878 RID: 2168
			// (get) Token: 0x060030BE RID: 12478 RVA: 0x0002116C File Offset: 0x0001F36C
			public ThemeSettings.ImageSettings backgroundImageSettings
			{
				get
				{
					return this._backgroundImageSettings;
				}
			}

			// Token: 0x060030BF RID: 12479 RVA: 0x0010233C File Offset: 0x0010053C
			private void Apply(Slider item)
			{
				if (item == null)
				{
					return;
				}
				if (this._handleImageSettings != null)
				{
					this._handleImageSettings.CopyTo(item.targetGraphic as Image);
				}
				if (this._fillImageSettings != null)
				{
					RectTransform fillRect = item.fillRect;
					if (fillRect != null)
					{
						this._fillImageSettings.CopyTo(fillRect.GetComponent<Image>());
					}
				}
				if (this._backgroundImageSettings != null)
				{
					Transform transform = item.transform.Find("Background");
					if (transform != null)
					{
						this._backgroundImageSettings.CopyTo(transform.GetComponent<Image>());
					}
				}
			}

			// Token: 0x060030C0 RID: 12480 RVA: 0x00021174 File Offset: 0x0001F374
			public override void Apply(Selectable item)
			{
				base.Apply(item);
				this.Apply(item as Slider);
			}

			// Token: 0x04003006 RID: 12294
			[SerializeField]
			private ThemeSettings.ImageSettings _handleImageSettings;

			// Token: 0x04003007 RID: 12295
			[SerializeField]
			private ThemeSettings.ImageSettings _fillImageSettings;

			// Token: 0x04003008 RID: 12296
			[SerializeField]
			private ThemeSettings.ImageSettings _backgroundImageSettings;
		}

		// Token: 0x02000695 RID: 1685
		[Serializable]
		private class ScrollbarSettings : ThemeSettings.SelectableSettings_Base
		{
			// Token: 0x17000879 RID: 2169
			// (get) Token: 0x060030C2 RID: 12482 RVA: 0x00021189 File Offset: 0x0001F389
			public ThemeSettings.ImageSettings handle
			{
				get
				{
					return this._handleImageSettings;
				}
			}

			// Token: 0x1700087A RID: 2170
			// (get) Token: 0x060030C3 RID: 12483 RVA: 0x00021191 File Offset: 0x0001F391
			public ThemeSettings.ImageSettings background
			{
				get
				{
					return this._backgroundImageSettings;
				}
			}

			// Token: 0x060030C4 RID: 12484 RVA: 0x001023D0 File Offset: 0x001005D0
			private void Apply(Scrollbar item)
			{
				if (item == null)
				{
					return;
				}
				if (this._handleImageSettings != null)
				{
					this._handleImageSettings.CopyTo(item.targetGraphic as Image);
				}
				if (this._backgroundImageSettings != null)
				{
					this._backgroundImageSettings.CopyTo(item.GetComponent<Image>());
				}
			}

			// Token: 0x060030C5 RID: 12485 RVA: 0x00021199 File Offset: 0x0001F399
			public override void Apply(Selectable item)
			{
				base.Apply(item);
				this.Apply(item as Scrollbar);
			}

			// Token: 0x04003009 RID: 12297
			[SerializeField]
			private ThemeSettings.ImageSettings _handleImageSettings;

			// Token: 0x0400300A RID: 12298
			[SerializeField]
			private ThemeSettings.ImageSettings _backgroundImageSettings;
		}

		// Token: 0x02000696 RID: 1686
		[Serializable]
		private class ImageSettings
		{
			// Token: 0x1700087B RID: 2171
			// (get) Token: 0x060030C7 RID: 12487 RVA: 0x000211AE File Offset: 0x0001F3AE
			public Color color
			{
				get
				{
					return this._color;
				}
			}

			// Token: 0x1700087C RID: 2172
			// (get) Token: 0x060030C8 RID: 12488 RVA: 0x000211B6 File Offset: 0x0001F3B6
			public Sprite sprite
			{
				get
				{
					return this._sprite;
				}
			}

			// Token: 0x1700087D RID: 2173
			// (get) Token: 0x060030C9 RID: 12489 RVA: 0x000211BE File Offset: 0x0001F3BE
			public Material materal
			{
				get
				{
					return this._materal;
				}
			}

			// Token: 0x1700087E RID: 2174
			// (get) Token: 0x060030CA RID: 12490 RVA: 0x000211C6 File Offset: 0x0001F3C6
			public Image.Type type
			{
				get
				{
					return this._type;
				}
			}

			// Token: 0x1700087F RID: 2175
			// (get) Token: 0x060030CB RID: 12491 RVA: 0x000211CE File Offset: 0x0001F3CE
			public bool preserveAspect
			{
				get
				{
					return this._preserveAspect;
				}
			}

			// Token: 0x17000880 RID: 2176
			// (get) Token: 0x060030CC RID: 12492 RVA: 0x000211D6 File Offset: 0x0001F3D6
			public bool fillCenter
			{
				get
				{
					return this._fillCenter;
				}
			}

			// Token: 0x17000881 RID: 2177
			// (get) Token: 0x060030CD RID: 12493 RVA: 0x000211DE File Offset: 0x0001F3DE
			public Image.FillMethod fillMethod
			{
				get
				{
					return this._fillMethod;
				}
			}

			// Token: 0x17000882 RID: 2178
			// (get) Token: 0x060030CE RID: 12494 RVA: 0x000211E6 File Offset: 0x0001F3E6
			public float fillAmout
			{
				get
				{
					return this._fillAmout;
				}
			}

			// Token: 0x17000883 RID: 2179
			// (get) Token: 0x060030CF RID: 12495 RVA: 0x000211EE File Offset: 0x0001F3EE
			public bool fillClockwise
			{
				get
				{
					return this._fillClockwise;
				}
			}

			// Token: 0x17000884 RID: 2180
			// (get) Token: 0x060030D0 RID: 12496 RVA: 0x000211F6 File Offset: 0x0001F3F6
			public int fillOrigin
			{
				get
				{
					return this._fillOrigin;
				}
			}

			// Token: 0x060030D1 RID: 12497 RVA: 0x00102420 File Offset: 0x00100620
			public virtual void CopyTo(Image image)
			{
				if (image == null)
				{
					return;
				}
				image.color = this._color;
				image.sprite = this._sprite;
				image.material = this._materal;
				image.type = this._type;
				image.preserveAspect = this._preserveAspect;
				image.fillCenter = this._fillCenter;
				image.fillMethod = this._fillMethod;
				image.fillAmount = this._fillAmout;
				image.fillClockwise = this._fillClockwise;
				image.fillOrigin = this._fillOrigin;
			}

			// Token: 0x0400300B RID: 12299
			[SerializeField]
			private Color _color = Color.white;

			// Token: 0x0400300C RID: 12300
			[SerializeField]
			private Sprite _sprite;

			// Token: 0x0400300D RID: 12301
			[SerializeField]
			private Material _materal;

			// Token: 0x0400300E RID: 12302
			[SerializeField]
			private Image.Type _type;

			// Token: 0x0400300F RID: 12303
			[SerializeField]
			private bool _preserveAspect;

			// Token: 0x04003010 RID: 12304
			[SerializeField]
			private bool _fillCenter;

			// Token: 0x04003011 RID: 12305
			[SerializeField]
			private Image.FillMethod _fillMethod;

			// Token: 0x04003012 RID: 12306
			[SerializeField]
			private float _fillAmout;

			// Token: 0x04003013 RID: 12307
			[SerializeField]
			private bool _fillClockwise;

			// Token: 0x04003014 RID: 12308
			[SerializeField]
			private int _fillOrigin;
		}

		// Token: 0x02000697 RID: 1687
		[Serializable]
		private struct CustomColorBlock
		{
			// Token: 0x17000885 RID: 2181
			// (get) Token: 0x060030D3 RID: 12499 RVA: 0x00021211 File Offset: 0x0001F411
			// (set) Token: 0x060030D4 RID: 12500 RVA: 0x00021219 File Offset: 0x0001F419
			public float colorMultiplier
			{
				get
				{
					return this.m_ColorMultiplier;
				}
				set
				{
					this.m_ColorMultiplier = value;
				}
			}

			// Token: 0x17000886 RID: 2182
			// (get) Token: 0x060030D5 RID: 12501 RVA: 0x00021222 File Offset: 0x0001F422
			// (set) Token: 0x060030D6 RID: 12502 RVA: 0x0002122A File Offset: 0x0001F42A
			public Color disabledColor
			{
				get
				{
					return this.m_DisabledColor;
				}
				set
				{
					this.m_DisabledColor = value;
				}
			}

			// Token: 0x17000887 RID: 2183
			// (get) Token: 0x060030D7 RID: 12503 RVA: 0x00021233 File Offset: 0x0001F433
			// (set) Token: 0x060030D8 RID: 12504 RVA: 0x0002123B File Offset: 0x0001F43B
			public float fadeDuration
			{
				get
				{
					return this.m_FadeDuration;
				}
				set
				{
					this.m_FadeDuration = value;
				}
			}

			// Token: 0x17000888 RID: 2184
			// (get) Token: 0x060030D9 RID: 12505 RVA: 0x00021244 File Offset: 0x0001F444
			// (set) Token: 0x060030DA RID: 12506 RVA: 0x0002124C File Offset: 0x0001F44C
			public Color highlightedColor
			{
				get
				{
					return this.m_HighlightedColor;
				}
				set
				{
					this.m_HighlightedColor = value;
				}
			}

			// Token: 0x17000889 RID: 2185
			// (get) Token: 0x060030DB RID: 12507 RVA: 0x00021255 File Offset: 0x0001F455
			// (set) Token: 0x060030DC RID: 12508 RVA: 0x0002125D File Offset: 0x0001F45D
			public Color normalColor
			{
				get
				{
					return this.m_NormalColor;
				}
				set
				{
					this.m_NormalColor = value;
				}
			}

			// Token: 0x1700088A RID: 2186
			// (get) Token: 0x060030DD RID: 12509 RVA: 0x00021266 File Offset: 0x0001F466
			// (set) Token: 0x060030DE RID: 12510 RVA: 0x0002126E File Offset: 0x0001F46E
			public Color pressedColor
			{
				get
				{
					return this.m_PressedColor;
				}
				set
				{
					this.m_PressedColor = value;
				}
			}

			// Token: 0x1700088B RID: 2187
			// (get) Token: 0x060030DF RID: 12511 RVA: 0x00021277 File Offset: 0x0001F477
			// (set) Token: 0x060030E0 RID: 12512 RVA: 0x0002127F File Offset: 0x0001F47F
			public Color selectedColor
			{
				get
				{
					return this.m_SelectedColor;
				}
				set
				{
					this.m_SelectedColor = value;
				}
			}

			// Token: 0x1700088C RID: 2188
			// (get) Token: 0x060030E1 RID: 12513 RVA: 0x00021288 File Offset: 0x0001F488
			// (set) Token: 0x060030E2 RID: 12514 RVA: 0x00021290 File Offset: 0x0001F490
			public Color disabledHighlightedColor
			{
				get
				{
					return this.m_DisabledHighlightedColor;
				}
				set
				{
					this.m_DisabledHighlightedColor = value;
				}
			}

			// Token: 0x060030E3 RID: 12515 RVA: 0x001024B0 File Offset: 0x001006B0
			public static implicit operator ColorBlock(ThemeSettings.CustomColorBlock item)
			{
				return new ColorBlock
				{
					selectedColor = item.m_SelectedColor,
					colorMultiplier = item.m_ColorMultiplier,
					disabledColor = item.m_DisabledColor,
					fadeDuration = item.m_FadeDuration,
					highlightedColor = item.m_HighlightedColor,
					normalColor = item.m_NormalColor,
					pressedColor = item.m_PressedColor
				};
			}

			// Token: 0x04003015 RID: 12309
			[SerializeField]
			private float m_ColorMultiplier;

			// Token: 0x04003016 RID: 12310
			[SerializeField]
			private Color m_DisabledColor;

			// Token: 0x04003017 RID: 12311
			[SerializeField]
			private float m_FadeDuration;

			// Token: 0x04003018 RID: 12312
			[SerializeField]
			private Color m_HighlightedColor;

			// Token: 0x04003019 RID: 12313
			[SerializeField]
			private Color m_NormalColor;

			// Token: 0x0400301A RID: 12314
			[SerializeField]
			private Color m_PressedColor;

			// Token: 0x0400301B RID: 12315
			[SerializeField]
			private Color m_SelectedColor;

			// Token: 0x0400301C RID: 12316
			[SerializeField]
			private Color m_DisabledHighlightedColor;
		}

		// Token: 0x02000698 RID: 1688
		[Serializable]
		private struct CustomSpriteState
		{
			// Token: 0x1700088D RID: 2189
			// (get) Token: 0x060030E4 RID: 12516 RVA: 0x00021299 File Offset: 0x0001F499
			// (set) Token: 0x060030E5 RID: 12517 RVA: 0x000212A1 File Offset: 0x0001F4A1
			public Sprite disabledSprite
			{
				get
				{
					return this.m_DisabledSprite;
				}
				set
				{
					this.m_DisabledSprite = value;
				}
			}

			// Token: 0x1700088E RID: 2190
			// (get) Token: 0x060030E6 RID: 12518 RVA: 0x000212AA File Offset: 0x0001F4AA
			// (set) Token: 0x060030E7 RID: 12519 RVA: 0x000212B2 File Offset: 0x0001F4B2
			public Sprite highlightedSprite
			{
				get
				{
					return this.m_HighlightedSprite;
				}
				set
				{
					this.m_HighlightedSprite = value;
				}
			}

			// Token: 0x1700088F RID: 2191
			// (get) Token: 0x060030E8 RID: 12520 RVA: 0x000212BB File Offset: 0x0001F4BB
			// (set) Token: 0x060030E9 RID: 12521 RVA: 0x000212C3 File Offset: 0x0001F4C3
			public Sprite pressedSprite
			{
				get
				{
					return this.m_PressedSprite;
				}
				set
				{
					this.m_PressedSprite = value;
				}
			}

			// Token: 0x17000890 RID: 2192
			// (get) Token: 0x060030EA RID: 12522 RVA: 0x000212CC File Offset: 0x0001F4CC
			// (set) Token: 0x060030EB RID: 12523 RVA: 0x000212D4 File Offset: 0x0001F4D4
			public Sprite selectedSprite
			{
				get
				{
					return this.m_SelectedSprite;
				}
				set
				{
					this.m_SelectedSprite = value;
				}
			}

			// Token: 0x17000891 RID: 2193
			// (get) Token: 0x060030EC RID: 12524 RVA: 0x000212DD File Offset: 0x0001F4DD
			// (set) Token: 0x060030ED RID: 12525 RVA: 0x000212E5 File Offset: 0x0001F4E5
			public Sprite disabledHighlightedSprite
			{
				get
				{
					return this.m_DisabledHighlightedSprite;
				}
				set
				{
					this.m_DisabledHighlightedSprite = value;
				}
			}

			// Token: 0x060030EE RID: 12526 RVA: 0x00102524 File Offset: 0x00100724
			public static implicit operator SpriteState(ThemeSettings.CustomSpriteState item)
			{
				return new SpriteState
				{
					selectedSprite = item.m_SelectedSprite,
					disabledSprite = item.m_DisabledSprite,
					highlightedSprite = item.m_HighlightedSprite,
					pressedSprite = item.m_PressedSprite
				};
			}

			// Token: 0x0400301D RID: 12317
			[SerializeField]
			private Sprite m_DisabledSprite;

			// Token: 0x0400301E RID: 12318
			[SerializeField]
			private Sprite m_HighlightedSprite;

			// Token: 0x0400301F RID: 12319
			[SerializeField]
			private Sprite m_PressedSprite;

			// Token: 0x04003020 RID: 12320
			[SerializeField]
			private Sprite m_SelectedSprite;

			// Token: 0x04003021 RID: 12321
			[SerializeField]
			private Sprite m_DisabledHighlightedSprite;
		}

		// Token: 0x02000699 RID: 1689
		[Serializable]
		private class CustomAnimationTriggers
		{
			// Token: 0x060030EF RID: 12527 RVA: 0x00102570 File Offset: 0x00100770
			public CustomAnimationTriggers()
			{
				this.m_DisabledTrigger = string.Empty;
				this.m_HighlightedTrigger = string.Empty;
				this.m_NormalTrigger = string.Empty;
				this.m_PressedTrigger = string.Empty;
				this.m_SelectedTrigger = string.Empty;
				this.m_DisabledHighlightedTrigger = string.Empty;
			}

			// Token: 0x17000892 RID: 2194
			// (get) Token: 0x060030F0 RID: 12528 RVA: 0x000212EE File Offset: 0x0001F4EE
			// (set) Token: 0x060030F1 RID: 12529 RVA: 0x000212F6 File Offset: 0x0001F4F6
			public string disabledTrigger
			{
				get
				{
					return this.m_DisabledTrigger;
				}
				set
				{
					this.m_DisabledTrigger = value;
				}
			}

			// Token: 0x17000893 RID: 2195
			// (get) Token: 0x060030F2 RID: 12530 RVA: 0x000212FF File Offset: 0x0001F4FF
			// (set) Token: 0x060030F3 RID: 12531 RVA: 0x00021307 File Offset: 0x0001F507
			public string highlightedTrigger
			{
				get
				{
					return this.m_HighlightedTrigger;
				}
				set
				{
					this.m_HighlightedTrigger = value;
				}
			}

			// Token: 0x17000894 RID: 2196
			// (get) Token: 0x060030F4 RID: 12532 RVA: 0x00021310 File Offset: 0x0001F510
			// (set) Token: 0x060030F5 RID: 12533 RVA: 0x00021318 File Offset: 0x0001F518
			public string normalTrigger
			{
				get
				{
					return this.m_NormalTrigger;
				}
				set
				{
					this.m_NormalTrigger = value;
				}
			}

			// Token: 0x17000895 RID: 2197
			// (get) Token: 0x060030F6 RID: 12534 RVA: 0x00021321 File Offset: 0x0001F521
			// (set) Token: 0x060030F7 RID: 12535 RVA: 0x00021329 File Offset: 0x0001F529
			public string pressedTrigger
			{
				get
				{
					return this.m_PressedTrigger;
				}
				set
				{
					this.m_PressedTrigger = value;
				}
			}

			// Token: 0x17000896 RID: 2198
			// (get) Token: 0x060030F8 RID: 12536 RVA: 0x00021332 File Offset: 0x0001F532
			// (set) Token: 0x060030F9 RID: 12537 RVA: 0x0002133A File Offset: 0x0001F53A
			public string selectedTrigger
			{
				get
				{
					return this.m_SelectedTrigger;
				}
				set
				{
					this.m_SelectedTrigger = value;
				}
			}

			// Token: 0x17000897 RID: 2199
			// (get) Token: 0x060030FA RID: 12538 RVA: 0x00021343 File Offset: 0x0001F543
			// (set) Token: 0x060030FB RID: 12539 RVA: 0x0002134B File Offset: 0x0001F54B
			public string disabledHighlightedTrigger
			{
				get
				{
					return this.m_DisabledHighlightedTrigger;
				}
				set
				{
					this.m_DisabledHighlightedTrigger = value;
				}
			}

			// Token: 0x060030FC RID: 12540 RVA: 0x001025C8 File Offset: 0x001007C8
			public static implicit operator AnimationTriggers(ThemeSettings.CustomAnimationTriggers item)
			{
				return new AnimationTriggers
				{
					selectedTrigger = item.m_SelectedTrigger,
					disabledTrigger = item.m_DisabledTrigger,
					highlightedTrigger = item.m_HighlightedTrigger,
					normalTrigger = item.m_NormalTrigger,
					pressedTrigger = item.m_PressedTrigger
				};
			}

			// Token: 0x04003022 RID: 12322
			[SerializeField]
			private string m_DisabledTrigger;

			// Token: 0x04003023 RID: 12323
			[SerializeField]
			private string m_HighlightedTrigger;

			// Token: 0x04003024 RID: 12324
			[SerializeField]
			private string m_NormalTrigger;

			// Token: 0x04003025 RID: 12325
			[SerializeField]
			private string m_PressedTrigger;

			// Token: 0x04003026 RID: 12326
			[SerializeField]
			private string m_SelectedTrigger;

			// Token: 0x04003027 RID: 12327
			[SerializeField]
			private string m_DisabledHighlightedTrigger;
		}

		// Token: 0x0200069A RID: 1690
		[Serializable]
		private class TextSettings
		{
			// Token: 0x17000898 RID: 2200
			// (get) Token: 0x060030FD RID: 12541 RVA: 0x00021354 File Offset: 0x0001F554
			public Color color
			{
				get
				{
					return this._color;
				}
			}

			// Token: 0x17000899 RID: 2201
			// (get) Token: 0x060030FE RID: 12542 RVA: 0x0002135C File Offset: 0x0001F55C
			public Font font
			{
				get
				{
					return this._font;
				}
			}

			// Token: 0x1700089A RID: 2202
			// (get) Token: 0x060030FF RID: 12543 RVA: 0x00021364 File Offset: 0x0001F564
			public ThemeSettings.FontStyleOverride style
			{
				get
				{
					return this._style;
				}
			}

			// Token: 0x1700089B RID: 2203
			// (get) Token: 0x06003100 RID: 12544 RVA: 0x0002136C File Offset: 0x0001F56C
			public float sizeMultiplier
			{
				get
				{
					return this._sizeMultiplier;
				}
			}

			// Token: 0x1700089C RID: 2204
			// (get) Token: 0x06003101 RID: 12545 RVA: 0x00021374 File Offset: 0x0001F574
			public float lineSpacing
			{
				get
				{
					return this._lineSpacing;
				}
			}

			// Token: 0x04003028 RID: 12328
			[SerializeField]
			private Color _color = Color.white;

			// Token: 0x04003029 RID: 12329
			[SerializeField]
			private Font _font;

			// Token: 0x0400302A RID: 12330
			[SerializeField]
			private ThemeSettings.FontStyleOverride _style;

			// Token: 0x0400302B RID: 12331
			[SerializeField]
			private float _sizeMultiplier = 1f;

			// Token: 0x0400302C RID: 12332
			[SerializeField]
			private float _lineSpacing = 1f;
		}

		// Token: 0x0200069B RID: 1691
		private enum FontStyleOverride
		{
			// Token: 0x0400302E RID: 12334
			Default,
			// Token: 0x0400302F RID: 12335
			Normal,
			// Token: 0x04003030 RID: 12336
			Bold,
			// Token: 0x04003031 RID: 12337
			Italic,
			// Token: 0x04003032 RID: 12338
			BoldAndItalic
		}
	}
}
