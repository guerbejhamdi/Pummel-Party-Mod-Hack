using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000544 RID: 1348
[RequireComponent(typeof(Image))]
public class UIGlyph : MonoBehaviour
{
	// Token: 0x1700043A RID: 1082
	// (get) Token: 0x060023B2 RID: 9138 RVA: 0x00019C12 File Offset: 0x00017E12
	// (set) Token: 0x060023B3 RID: 9139 RVA: 0x000D77B0 File Offset: 0x000D59B0
	public bool Enabled
	{
		get
		{
			return this.glyphEnabled;
		}
		set
		{
			this.glyphEnabled = value;
			if (this.glyph != null)
			{
				if (this.glyphEnabled && this.curGlyphDetails != null)
				{
					this.glyph.color = this.curGlyphDetails.color;
					for (int i = 0; i < this.setToGlyphColor.Length; i++)
					{
						this.setToGlyphColor[i].color = this.curGlyphDetails.color;
					}
					return;
				}
				this.glyph.color = this.disabledColor;
				for (int j = 0; j < this.setToGlyphColor.Length; j++)
				{
					this.setToGlyphColor[j].color = this.disabledColor;
				}
			}
		}
	}

	// Token: 0x060023B4 RID: 9140 RVA: 0x00019C1A File Offset: 0x00017E1A
	public void OnXInputChanged()
	{
		this.lastHelper = null;
	}

	// Token: 0x060023B5 RID: 9141 RVA: 0x000D7860 File Offset: 0x000D5A60
	private void Start()
	{
		this.DoMappings();
		if (this.useDefaultPlayer)
		{
			Player player = ReInput.players.GetPlayer(this.defaultPlayerIndex);
			if (player != null)
			{
				this.SetPlayer(player);
			}
		}
	}

	// Token: 0x060023B6 RID: 9142 RVA: 0x00019C23 File Offset: 0x00017E23
	private void DoMappings()
	{
		if (!this.mappingsSet)
		{
			this.inputDetails.GetMapping();
			this.mappingsSet = true;
		}
	}

	// Token: 0x060023B7 RID: 9143 RVA: 0x00019C3F File Offset: 0x00017E3F
	public void SetValues(InputDetails inputDetails)
	{
		this.inputDetails = inputDetails;
		if (this.rewiredPlayer != null)
		{
			this.GetGlyph(this.rewiredPlayer, this.rewiredPlayer.controllers.GetLastActiveController());
		}
	}

	// Token: 0x060023B8 RID: 9144 RVA: 0x00019C6C File Offset: 0x00017E6C
	public void ForceUpdateGlyph()
	{
		if (this.rewiredPlayer != null)
		{
			this.GetGlyph(this.rewiredPlayer, this.rewiredPlayer.controllers.GetLastActiveController());
		}
	}

	// Token: 0x060023B9 RID: 9145 RVA: 0x000D7898 File Offset: 0x000D5A98
	public void SetPlayer(Player player)
	{
		if (!ReInput.isReady)
		{
			Debug.LogError("Reinput not ready");
			return;
		}
		this.DoMappings();
		if (this.lastHelper != null)
		{
			this.lastHelper.RemoveLastActiveControllerChangedDelegate(new PlayerActiveControllerChangedDelegate(this.ControllerChanged));
		}
		this.rewiredPlayer = player;
		if (player == null)
		{
			return;
		}
		this.lastHelper = this.rewiredPlayer.controllers;
		this.lastHelper.AddLastActiveControllerChangedDelegate(new PlayerActiveControllerChangedDelegate(this.ControllerChanged));
		this.ControllerChanged(this.rewiredPlayer, this.rewiredPlayer.controllers.GetLastActiveController());
	}

	// Token: 0x060023BA RID: 9146 RVA: 0x000D792C File Offset: 0x000D5B2C
	private void ControllerChanged(Player player, Controller controller)
	{
		if (this == null)
		{
			if (this.lastHelper != null)
			{
				this.lastHelper.RemoveLastActiveControllerChangedDelegate(new PlayerActiveControllerChangedDelegate(this.ControllerChanged));
			}
			return;
		}
		bool flag = ((controller != null) ? controller.type : ((player.id == 0) ? ControllerType.Mouse : ControllerType.Joystick)) == ControllerType.Joystick;
		if (this.animator != null && this.animator.isInitialized && this.glyph != null)
		{
			this.animator.SetBool("Joystick", flag);
		}
		if ((this.glyphType == UIGlyphType.Joystick && flag) || (this.glyphType == UIGlyphType.Keyboard && !flag) || this.glyphType == UIGlyphType.Both)
		{
			this.GetGlyph(this.rewiredPlayer, controller);
		}
	}

	// Token: 0x060023BB RID: 9147 RVA: 0x000D79E8 File Offset: 0x000D5BE8
	private void GetGlyph(Player p, Controller controller)
	{
		if (controller == null)
		{
			if (p.id == 0)
			{
				controller = p.controllers.Keyboard;
			}
			else
			{
				if (p.controllers.Joysticks.Count <= 0)
				{
					return;
				}
				controller = p.controllers.Joysticks[0];
			}
		}
		else if (controller.type == ControllerType.Mouse)
		{
			controller = p.controllers.Keyboard;
		}
		int num = (controller.type == ControllerType.Keyboard) ? this.inputDetails.keyboardActionID : this.inputDetails.joystickActionID;
		if (num != -1)
		{
			InputAction action = ReInput.mapping.GetAction(num);
			if (action != null)
			{
				this.ShowGlyphHelp(p, controller, action);
			}
		}
	}

	// Token: 0x060023BC RID: 9148 RVA: 0x000D7A8C File Offset: 0x000D5C8C
	private void ShowGlyphHelp(Player p, Controller controller, InputAction action)
	{
		if (p == null || controller == null || action == null)
		{
			return;
		}
		GlyphDatabase.GlyphDetails glyphDetails = null;
		if (controller != null)
		{
			if (controller.type != ControllerType.Joystick)
			{
				using (IEnumerator<ActionElementMap> enumerator = p.controllers.maps.ElementMapsWithAction(action.id, true).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActionElementMap actionElementMap = enumerator.Current;
						if (actionElementMap.controllerMap.controllerType == this.inputDetails.controllerType)
						{
							if (actionElementMap.controllerMap.controllerType == ControllerType.Mouse)
							{
								glyphDetails = GlyphDatabase.Instance.GetMouseGlyph((MouseInputElement)actionElementMap.elementIdentifierId);
								break;
							}
							if (actionElementMap.controllerMap.controllerType == ControllerType.Keyboard && actionElementMap.axisContribution == this.inputDetails.axisContribution)
							{
								glyphDetails = GlyphDatabase.Instance.GetKeyboardGlyph(actionElementMap.keyboardKeyCode);
								break;
							}
						}
					}
					goto IL_1DB;
				}
			}
			ActionElementMap actionElementMap2;
			if (action.name == "Horizontal" || action.name == "Vertical")
			{
				actionElementMap2 = p.controllers.maps.GetFirstAxisMapWithAction(controller, action.id, true);
			}
			else
			{
				actionElementMap2 = p.controllers.maps.GetFirstElementMapWithAction(controller, action.id, true);
			}
			if (actionElementMap2 == null)
			{
				return;
			}
			glyphDetails = GlyphDatabase.Instance.GetGlyph((controller as Joystick).hardwareTypeGuid, actionElementMap2.elementIdentifierId, actionElementMap2.axisRange);
		}
		else
		{
			ActionElementMap firstElementMapWithAction = p.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, action.id, true);
			if (firstElementMapWithAction == null)
			{
				if (!this.xb1ActionElements.ContainsKey(action.id))
				{
					return;
				}
				UIGlyph.ActionToElement actionToElement = this.xb1ActionElements[action.id];
				glyphDetails = GlyphDatabase.Instance.GetGlyph(GlyphDatabase.XB1ControllerGUID, actionToElement.elementIdentifierID, actionToElement.axisRange);
			}
			else
			{
				glyphDetails = GlyphDatabase.Instance.GetGlyph(GlyphDatabase.PS4ControllerGUID, firstElementMapWithAction.elementIdentifierId, firstElementMapWithAction.axisRange);
			}
		}
		IL_1DB:
		if (glyphDetails == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Couldn't find glyph: ",
				this.inputDetails.keyboardActionID.ToString(),
				" Player: ",
				p.id.ToString(),
				" Controller Type: ",
				controller.type.ToString()
			}));
			return;
		}
		this.curGlyphDetails = glyphDetails;
		if (this.glyph != null)
		{
			this.glyph.sprite = glyphDetails.glyph;
		}
		this.Enabled = this.Enabled;
	}

	// Token: 0x060023BD RID: 9149 RVA: 0x00019C92 File Offset: 0x00017E92
	private void OnDestroy()
	{
		if (this.lastHelper != null)
		{
			this.lastHelper.RemoveLastActiveControllerChangedDelegate(new PlayerActiveControllerChangedDelegate(this.ControllerChanged));
		}
	}

	// Token: 0x0400268D RID: 9869
	public UIGlyphType glyphType;

	// Token: 0x0400268E RID: 9870
	public InputDetails inputDetails;

	// Token: 0x0400268F RID: 9871
	public Color disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

	// Token: 0x04002690 RID: 9872
	public Image glyph;

	// Token: 0x04002691 RID: 9873
	public Animator animator;

	// Token: 0x04002692 RID: 9874
	public bool useDefaultPlayer;

	// Token: 0x04002693 RID: 9875
	public int defaultPlayerIndex;

	// Token: 0x04002694 RID: 9876
	public Graphic[] setToGlyphColor;

	// Token: 0x04002695 RID: 9877
	private Player rewiredPlayer;

	// Token: 0x04002696 RID: 9878
	private GlyphDatabase.GlyphDetails curGlyphDetails;

	// Token: 0x04002697 RID: 9879
	private bool glyphEnabled = true;

	// Token: 0x04002698 RID: 9880
	private bool mappingsSet;

	// Token: 0x04002699 RID: 9881
	private Player.ControllerHelper lastHelper;

	// Token: 0x0400269A RID: 9882
	private Dictionary<int, UIGlyph.ActionToElement> xb1ActionElements = new Dictionary<int, UIGlyph.ActionToElement>
	{
		{
			23,
			new UIGlyph.ActionToElement(6, AxisRange.Positive)
		},
		{
			27,
			new UIGlyph.ActionToElement(7, AxisRange.Positive)
		},
		{
			34,
			new UIGlyph.ActionToElement(8, AxisRange.Positive)
		},
		{
			33,
			new UIGlyph.ActionToElement(9, AxisRange.Positive)
		},
		{
			35,
			new UIGlyph.ActionToElement(5, AxisRange.Positive)
		},
		{
			24,
			new UIGlyph.ActionToElement(1, AxisRange.Full)
		},
		{
			25,
			new UIGlyph.ActionToElement(0, AxisRange.Full)
		},
		{
			42,
			new UIGlyph.ActionToElement(2, AxisRange.Full)
		},
		{
			41,
			new UIGlyph.ActionToElement(3, AxisRange.Full)
		},
		{
			46,
			new UIGlyph.ActionToElement(4, AxisRange.Positive)
		},
		{
			47,
			new UIGlyph.ActionToElement(2, AxisRange.Full)
		},
		{
			48,
			new UIGlyph.ActionToElement(3, AxisRange.Full)
		},
		{
			50,
			new UIGlyph.ActionToElement(3, AxisRange.Full)
		},
		{
			51,
			new UIGlyph.ActionToElement(2, AxisRange.Full)
		},
		{
			49,
			new UIGlyph.ActionToElement(0, AxisRange.Full)
		},
		{
			56,
			new UIGlyph.ActionToElement(9, AxisRange.Positive)
		},
		{
			57,
			new UIGlyph.ActionToElement(6, AxisRange.Positive)
		},
		{
			58,
			new UIGlyph.ActionToElement(8, AxisRange.Positive)
		},
		{
			59,
			new UIGlyph.ActionToElement(7, AxisRange.Positive)
		},
		{
			60,
			new UIGlyph.ActionToElement(16, AxisRange.Positive)
		},
		{
			61,
			new UIGlyph.ActionToElement(17, AxisRange.Positive)
		},
		{
			62,
			new UIGlyph.ActionToElement(18, AxisRange.Positive)
		},
		{
			63,
			new UIGlyph.ActionToElement(19, AxisRange.Positive)
		},
		{
			5,
			new UIGlyph.ActionToElement(12, AxisRange.Positive)
		},
		{
			6,
			new UIGlyph.ActionToElement(13, AxisRange.Positive)
		},
		{
			8,
			new UIGlyph.ActionToElement(10, AxisRange.Positive)
		},
		{
			9,
			new UIGlyph.ActionToElement(11, AxisRange.Positive)
		},
		{
			4,
			new UIGlyph.ActionToElement(0, AxisRange.Positive)
		},
		{
			64,
			new UIGlyph.ActionToElement(0, AxisRange.Positive)
		},
		{
			65,
			new UIGlyph.ActionToElement(11, AxisRange.Positive)
		},
		{
			66,
			new UIGlyph.ActionToElement(10, AxisRange.Positive)
		}
	};

	// Token: 0x02000545 RID: 1349
	private struct ActionToElement
	{
		// Token: 0x060023BF RID: 9151 RVA: 0x00019CB3 File Offset: 0x00017EB3
		public ActionToElement(int elementIdentifierID, AxisRange axisRange)
		{
			this.elementIdentifierID = elementIdentifierID;
			this.axisRange = axisRange;
		}

		// Token: 0x0400269B RID: 9883
		public int elementIdentifierID;

		// Token: 0x0400269C RID: 9884
		public AxisRange axisRange;
	}
}
