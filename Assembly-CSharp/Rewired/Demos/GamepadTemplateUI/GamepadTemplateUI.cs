using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos.GamepadTemplateUI
{
	// Token: 0x020006E5 RID: 1765
	public class GamepadTemplateUI : MonoBehaviour
	{
		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06003370 RID: 13168 RVA: 0x000230CE File Offset: 0x000212CE
		private Player player
		{
			get
			{
				return ReInput.players.GetPlayer(this.playerId);
			}
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x0010AC50 File Offset: 0x00108E50
		private void Awake()
		{
			this._uiElementsArray = new GamepadTemplateUI.UIElement[]
			{
				new GamepadTemplateUI.UIElement(0, this.leftStickX),
				new GamepadTemplateUI.UIElement(1, this.leftStickY),
				new GamepadTemplateUI.UIElement(17, this.leftStickButton),
				new GamepadTemplateUI.UIElement(2, this.rightStickX),
				new GamepadTemplateUI.UIElement(3, this.rightStickY),
				new GamepadTemplateUI.UIElement(18, this.rightStickButton),
				new GamepadTemplateUI.UIElement(4, this.actionBottomRow1),
				new GamepadTemplateUI.UIElement(5, this.actionBottomRow2),
				new GamepadTemplateUI.UIElement(6, this.actionBottomRow3),
				new GamepadTemplateUI.UIElement(7, this.actionTopRow1),
				new GamepadTemplateUI.UIElement(8, this.actionTopRow2),
				new GamepadTemplateUI.UIElement(9, this.actionTopRow3),
				new GamepadTemplateUI.UIElement(14, this.center1),
				new GamepadTemplateUI.UIElement(15, this.center2),
				new GamepadTemplateUI.UIElement(16, this.center3),
				new GamepadTemplateUI.UIElement(19, this.dPadUp),
				new GamepadTemplateUI.UIElement(20, this.dPadRight),
				new GamepadTemplateUI.UIElement(21, this.dPadDown),
				new GamepadTemplateUI.UIElement(22, this.dPadLeft),
				new GamepadTemplateUI.UIElement(10, this.leftShoulder),
				new GamepadTemplateUI.UIElement(11, this.leftTrigger),
				new GamepadTemplateUI.UIElement(12, this.rightShoulder),
				new GamepadTemplateUI.UIElement(13, this.rightTrigger)
			};
			for (int i = 0; i < this._uiElementsArray.Length; i++)
			{
				this._uiElements.Add(this._uiElementsArray[i].id, this._uiElementsArray[i].element);
			}
			this._sticks = new GamepadTemplateUI.Stick[]
			{
				new GamepadTemplateUI.Stick(this.leftStick, 0, 1),
				new GamepadTemplateUI.Stick(this.rightStick, 2, 3)
			};
			ReInput.ControllerConnectedEvent += this.OnControllerConnected;
			ReInput.ControllerDisconnectedEvent += this.OnControllerDisconnected;
		}

		// Token: 0x06003372 RID: 13170 RVA: 0x000230E0 File Offset: 0x000212E0
		private void Start()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.DrawLabels();
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x000230F0 File Offset: 0x000212F0
		private void OnDestroy()
		{
			ReInput.ControllerConnectedEvent -= this.OnControllerConnected;
			ReInput.ControllerDisconnectedEvent -= this.OnControllerDisconnected;
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x00023114 File Offset: 0x00021314
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.DrawActiveElements();
		}

		// Token: 0x06003375 RID: 13173 RVA: 0x0010AE68 File Offset: 0x00109068
		private void DrawActiveElements()
		{
			for (int i = 0; i < this._uiElementsArray.Length; i++)
			{
				this._uiElementsArray[i].element.Deactivate();
			}
			for (int j = 0; j < this._sticks.Length; j++)
			{
				this._sticks[j].Reset();
			}
			IList<InputAction> actions = ReInput.mapping.Actions;
			for (int k = 0; k < actions.Count; k++)
			{
				this.ActivateElements(this.player, actions[k].id);
			}
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x0010AEF0 File Offset: 0x001090F0
		private void ActivateElements(Player player, int actionId)
		{
			float axis = player.GetAxis(actionId);
			if (axis == 0f)
			{
				return;
			}
			IList<InputActionSourceData> currentInputSources = player.GetCurrentInputSources(actionId);
			for (int i = 0; i < currentInputSources.Count; i++)
			{
				InputActionSourceData inputActionSourceData = currentInputSources[i];
				IGamepadTemplate template = inputActionSourceData.controller.GetTemplate<IGamepadTemplate>();
				if (template != null)
				{
					template.GetElementTargets(inputActionSourceData.actionElementMap, this._tempTargetList);
					for (int j = 0; j < this._tempTargetList.Count; j++)
					{
						ControllerTemplateElementTarget controllerTemplateElementTarget = this._tempTargetList[j];
						int id = controllerTemplateElementTarget.element.id;
						ControllerUIElement controllerUIElement = this._uiElements[id];
						if (controllerTemplateElementTarget.elementType == ControllerTemplateElementType.Axis)
						{
							controllerUIElement.Activate(axis);
						}
						else if (controllerTemplateElementTarget.elementType == ControllerTemplateElementType.Button && (player.GetButton(actionId) || player.GetNegativeButton(actionId)))
						{
							controllerUIElement.Activate(1f);
						}
						GamepadTemplateUI.Stick stick = this.GetStick(id);
						if (stick != null)
						{
							stick.SetAxisPosition(id, axis * 20f);
						}
					}
				}
			}
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x0010B00C File Offset: 0x0010920C
		private void DrawLabels()
		{
			for (int i = 0; i < this._uiElementsArray.Length; i++)
			{
				this._uiElementsArray[i].element.ClearLabels();
			}
			IList<InputAction> actions = ReInput.mapping.Actions;
			for (int j = 0; j < actions.Count; j++)
			{
				this.DrawLabels(this.player, actions[j]);
			}
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x0010B070 File Offset: 0x00109270
		private void DrawLabels(Player player, InputAction action)
		{
			Controller firstControllerWithTemplate = player.controllers.GetFirstControllerWithTemplate<IGamepadTemplate>();
			if (firstControllerWithTemplate == null)
			{
				return;
			}
			IGamepadTemplate template = firstControllerWithTemplate.GetTemplate<IGamepadTemplate>();
			ControllerMap map = player.controllers.maps.GetMap(firstControllerWithTemplate, "Default", "Default");
			if (map == null)
			{
				return;
			}
			for (int i = 0; i < this._uiElementsArray.Length; i++)
			{
				ControllerUIElement element = this._uiElementsArray[i].element;
				int id = this._uiElementsArray[i].id;
				IControllerTemplateElement element2 = template.GetElement(id);
				this.DrawLabel(element, action, map, template, element2);
			}
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x0010B0FC File Offset: 0x001092FC
		private void DrawLabel(ControllerUIElement uiElement, InputAction action, ControllerMap controllerMap, IControllerTemplate template, IControllerTemplateElement element)
		{
			if (element.source == null)
			{
				return;
			}
			if (element.source.type == ControllerTemplateElementSourceType.Axis)
			{
				IControllerTemplateAxisSource controllerTemplateAxisSource = element.source as IControllerTemplateAxisSource;
				if (controllerTemplateAxisSource.splitAxis)
				{
					ActionElementMap firstElementMapWithElementTarget = controllerMap.GetFirstElementMapWithElementTarget(controllerTemplateAxisSource.positiveTarget, action.id, true);
					if (firstElementMapWithElementTarget != null)
					{
						uiElement.SetLabel(firstElementMapWithElementTarget.actionDescriptiveName, AxisRange.Positive);
					}
					firstElementMapWithElementTarget = controllerMap.GetFirstElementMapWithElementTarget(controllerTemplateAxisSource.negativeTarget, action.id, true);
					if (firstElementMapWithElementTarget != null)
					{
						uiElement.SetLabel(firstElementMapWithElementTarget.actionDescriptiveName, AxisRange.Negative);
						return;
					}
				}
				else
				{
					ActionElementMap firstElementMapWithElementTarget = controllerMap.GetFirstElementMapWithElementTarget(controllerTemplateAxisSource.fullTarget, action.id, true);
					if (firstElementMapWithElementTarget != null)
					{
						uiElement.SetLabel(firstElementMapWithElementTarget.actionDescriptiveName, AxisRange.Full);
						return;
					}
					ControllerElementTarget elementTarget = new ControllerElementTarget(controllerTemplateAxisSource.fullTarget)
					{
						axisRange = AxisRange.Positive
					};
					firstElementMapWithElementTarget = controllerMap.GetFirstElementMapWithElementTarget(elementTarget, action.id, true);
					if (firstElementMapWithElementTarget != null)
					{
						uiElement.SetLabel(firstElementMapWithElementTarget.actionDescriptiveName, AxisRange.Positive);
					}
					elementTarget = new ControllerElementTarget(controllerTemplateAxisSource.fullTarget)
					{
						axisRange = AxisRange.Negative
					};
					firstElementMapWithElementTarget = controllerMap.GetFirstElementMapWithElementTarget(elementTarget, action.id, true);
					if (firstElementMapWithElementTarget != null)
					{
						uiElement.SetLabel(firstElementMapWithElementTarget.actionDescriptiveName, AxisRange.Negative);
						return;
					}
				}
			}
			else if (element.source.type == ControllerTemplateElementSourceType.Button)
			{
				IControllerTemplateButtonSource controllerTemplateButtonSource = element.source as IControllerTemplateButtonSource;
				ActionElementMap firstElementMapWithElementTarget = controllerMap.GetFirstElementMapWithElementTarget(controllerTemplateButtonSource.target, action.id, true);
				if (firstElementMapWithElementTarget != null)
				{
					uiElement.SetLabel(firstElementMapWithElementTarget.actionDescriptiveName, AxisRange.Full);
				}
			}
		}

		// Token: 0x0600337A RID: 13178 RVA: 0x0010B254 File Offset: 0x00109454
		private GamepadTemplateUI.Stick GetStick(int elementId)
		{
			for (int i = 0; i < this._sticks.Length; i++)
			{
				if (this._sticks[i].ContainsElement(elementId))
				{
					return this._sticks[i];
				}
			}
			return null;
		}

		// Token: 0x0600337B RID: 13179 RVA: 0x00023124 File Offset: 0x00021324
		private void OnControllerConnected(ControllerStatusChangedEventArgs args)
		{
			this.DrawLabels();
		}

		// Token: 0x0600337C RID: 13180 RVA: 0x00023124 File Offset: 0x00021324
		private void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
		{
			this.DrawLabels();
		}

		// Token: 0x040031B2 RID: 12722
		private const float stickRadius = 20f;

		// Token: 0x040031B3 RID: 12723
		public int playerId;

		// Token: 0x040031B4 RID: 12724
		[SerializeField]
		private RectTransform leftStick;

		// Token: 0x040031B5 RID: 12725
		[SerializeField]
		private RectTransform rightStick;

		// Token: 0x040031B6 RID: 12726
		[SerializeField]
		private ControllerUIElement leftStickX;

		// Token: 0x040031B7 RID: 12727
		[SerializeField]
		private ControllerUIElement leftStickY;

		// Token: 0x040031B8 RID: 12728
		[SerializeField]
		private ControllerUIElement leftStickButton;

		// Token: 0x040031B9 RID: 12729
		[SerializeField]
		private ControllerUIElement rightStickX;

		// Token: 0x040031BA RID: 12730
		[SerializeField]
		private ControllerUIElement rightStickY;

		// Token: 0x040031BB RID: 12731
		[SerializeField]
		private ControllerUIElement rightStickButton;

		// Token: 0x040031BC RID: 12732
		[SerializeField]
		private ControllerUIElement actionBottomRow1;

		// Token: 0x040031BD RID: 12733
		[SerializeField]
		private ControllerUIElement actionBottomRow2;

		// Token: 0x040031BE RID: 12734
		[SerializeField]
		private ControllerUIElement actionBottomRow3;

		// Token: 0x040031BF RID: 12735
		[SerializeField]
		private ControllerUIElement actionTopRow1;

		// Token: 0x040031C0 RID: 12736
		[SerializeField]
		private ControllerUIElement actionTopRow2;

		// Token: 0x040031C1 RID: 12737
		[SerializeField]
		private ControllerUIElement actionTopRow3;

		// Token: 0x040031C2 RID: 12738
		[SerializeField]
		private ControllerUIElement leftShoulder;

		// Token: 0x040031C3 RID: 12739
		[SerializeField]
		private ControllerUIElement leftTrigger;

		// Token: 0x040031C4 RID: 12740
		[SerializeField]
		private ControllerUIElement rightShoulder;

		// Token: 0x040031C5 RID: 12741
		[SerializeField]
		private ControllerUIElement rightTrigger;

		// Token: 0x040031C6 RID: 12742
		[SerializeField]
		private ControllerUIElement center1;

		// Token: 0x040031C7 RID: 12743
		[SerializeField]
		private ControllerUIElement center2;

		// Token: 0x040031C8 RID: 12744
		[SerializeField]
		private ControllerUIElement center3;

		// Token: 0x040031C9 RID: 12745
		[SerializeField]
		private ControllerUIElement dPadUp;

		// Token: 0x040031CA RID: 12746
		[SerializeField]
		private ControllerUIElement dPadRight;

		// Token: 0x040031CB RID: 12747
		[SerializeField]
		private ControllerUIElement dPadDown;

		// Token: 0x040031CC RID: 12748
		[SerializeField]
		private ControllerUIElement dPadLeft;

		// Token: 0x040031CD RID: 12749
		private GamepadTemplateUI.UIElement[] _uiElementsArray;

		// Token: 0x040031CE RID: 12750
		private Dictionary<int, ControllerUIElement> _uiElements = new Dictionary<int, ControllerUIElement>();

		// Token: 0x040031CF RID: 12751
		private IList<ControllerTemplateElementTarget> _tempTargetList = new List<ControllerTemplateElementTarget>(2);

		// Token: 0x040031D0 RID: 12752
		private GamepadTemplateUI.Stick[] _sticks;

		// Token: 0x020006E6 RID: 1766
		private class Stick
		{
			// Token: 0x17000914 RID: 2324
			// (get) Token: 0x0600337E RID: 13182 RVA: 0x0002314B File Offset: 0x0002134B
			// (set) Token: 0x0600337F RID: 13183 RVA: 0x00023177 File Offset: 0x00021377
			public Vector2 position
			{
				get
				{
					if (!(this._transform != null))
					{
						return Vector2.zero;
					}
					return this._transform.anchoredPosition - this._origPosition;
				}
				set
				{
					if (this._transform == null)
					{
						return;
					}
					this._transform.anchoredPosition = this._origPosition + value;
				}
			}

			// Token: 0x06003380 RID: 13184 RVA: 0x0010B290 File Offset: 0x00109490
			public Stick(RectTransform transform, int xAxisElementId, int yAxisElementId)
			{
				if (transform == null)
				{
					return;
				}
				this._transform = transform;
				this._origPosition = this._transform.anchoredPosition;
				this._xAxisElementId = xAxisElementId;
				this._yAxisElementId = yAxisElementId;
			}

			// Token: 0x06003381 RID: 13185 RVA: 0x0002319F File Offset: 0x0002139F
			public void Reset()
			{
				if (this._transform == null)
				{
					return;
				}
				this._transform.anchoredPosition = this._origPosition;
			}

			// Token: 0x06003382 RID: 13186 RVA: 0x000231C1 File Offset: 0x000213C1
			public bool ContainsElement(int elementId)
			{
				return !(this._transform == null) && (elementId == this._xAxisElementId || elementId == this._yAxisElementId);
			}

			// Token: 0x06003383 RID: 13187 RVA: 0x0010B2E4 File Offset: 0x001094E4
			public void SetAxisPosition(int elementId, float value)
			{
				if (this._transform == null)
				{
					return;
				}
				Vector2 position = this.position;
				if (elementId == this._xAxisElementId)
				{
					position.x = value;
				}
				else if (elementId == this._yAxisElementId)
				{
					position.y = value;
				}
				this.position = position;
			}

			// Token: 0x040031D1 RID: 12753
			private RectTransform _transform;

			// Token: 0x040031D2 RID: 12754
			private Vector2 _origPosition;

			// Token: 0x040031D3 RID: 12755
			private int _xAxisElementId = -1;

			// Token: 0x040031D4 RID: 12756
			private int _yAxisElementId = -1;
		}

		// Token: 0x020006E7 RID: 1767
		private class UIElement
		{
			// Token: 0x06003384 RID: 13188 RVA: 0x000231E7 File Offset: 0x000213E7
			public UIElement(int id, ControllerUIElement element)
			{
				this.id = id;
				this.element = element;
			}

			// Token: 0x040031D5 RID: 12757
			public int id;

			// Token: 0x040031D6 RID: 12758
			public ControllerUIElement element;
		}
	}
}
