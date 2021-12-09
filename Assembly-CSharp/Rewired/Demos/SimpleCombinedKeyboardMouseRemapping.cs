using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x020006D8 RID: 1752
	[AddComponentMenu("")]
	public class SimpleCombinedKeyboardMouseRemapping : MonoBehaviour
	{
		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06003327 RID: 13095 RVA: 0x00022CD7 File Offset: 0x00020ED7
		private Player player
		{
			get
			{
				return ReInput.players.GetPlayer(0);
			}
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x00109B7C File Offset: 0x00107D7C
		private void OnEnable()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.inputMapper_keyboard.options.timeout = 5f;
			this.inputMapper_mouse.options.timeout = 5f;
			this.inputMapper_mouse.options.ignoreMouseXAxis = true;
			this.inputMapper_mouse.options.ignoreMouseYAxis = true;
			this.inputMapper_keyboard.options.allowButtonsOnFullAxisAssignment = false;
			this.inputMapper_mouse.options.allowButtonsOnFullAxisAssignment = false;
			this.inputMapper_keyboard.InputMappedEvent += this.OnInputMapped;
			this.inputMapper_keyboard.StoppedEvent += this.OnStopped;
			this.inputMapper_mouse.InputMappedEvent += this.OnInputMapped;
			this.inputMapper_mouse.StoppedEvent += this.OnStopped;
			this.InitializeUI();
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x00022CE4 File Offset: 0x00020EE4
		private void OnDisable()
		{
			this.inputMapper_keyboard.Stop();
			this.inputMapper_mouse.Stop();
			this.inputMapper_keyboard.RemoveAllEventListeners();
			this.inputMapper_mouse.RemoveAllEventListeners();
		}

		// Token: 0x0600332A RID: 13098 RVA: 0x00109C64 File Offset: 0x00107E64
		private void RedrawUI()
		{
			this.controllerNameUIText.text = "Keyboard/Mouse";
			for (int i = 0; i < this.rows.Count; i++)
			{
				SimpleCombinedKeyboardMouseRemapping.Row row = this.rows[i];
				InputAction action = this.rows[i].action;
				string text = string.Empty;
				int actionElementMapId = -1;
				for (int j = 0; j < 2; j++)
				{
					ControllerType controllerType = (j == 0) ? ControllerType.Keyboard : ControllerType.Mouse;
					foreach (ActionElementMap actionElementMap in this.player.controllers.maps.GetMap(controllerType, 0, "Default", "Default").ElementMapsWithAction(action.id))
					{
						if (actionElementMap.ShowInField(row.actionRange))
						{
							text = actionElementMap.elementIdentifierName;
							actionElementMapId = actionElementMap.id;
							break;
						}
					}
					if (actionElementMapId >= 0)
					{
						break;
					}
				}
				row.text.text = text;
				row.button.onClick.RemoveAllListeners();
				int index = i;
				row.button.onClick.AddListener(delegate()
				{
					this.OnInputFieldClicked(index, actionElementMapId);
				});
			}
		}

		// Token: 0x0600332B RID: 13099 RVA: 0x00109DD0 File Offset: 0x00107FD0
		private void ClearUI()
		{
			this.controllerNameUIText.text = string.Empty;
			for (int i = 0; i < this.rows.Count; i++)
			{
				this.rows[i].text.text = string.Empty;
			}
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x00109E20 File Offset: 0x00108020
		private void InitializeUI()
		{
			foreach (object obj in this.actionGroupTransform)
			{
				UnityEngine.Object.Destroy(((Transform)obj).gameObject);
			}
			foreach (object obj2 in this.fieldGroupTransform)
			{
				UnityEngine.Object.Destroy(((Transform)obj2).gameObject);
			}
			foreach (InputAction inputAction in ReInput.mapping.ActionsInCategory("Default"))
			{
				if (inputAction.type == InputActionType.Axis)
				{
					this.CreateUIRow(inputAction, AxisRange.Full, inputAction.descriptiveName);
					this.CreateUIRow(inputAction, AxisRange.Positive, (!string.IsNullOrEmpty(inputAction.positiveDescriptiveName)) ? inputAction.positiveDescriptiveName : (inputAction.descriptiveName + " +"));
					this.CreateUIRow(inputAction, AxisRange.Negative, (!string.IsNullOrEmpty(inputAction.negativeDescriptiveName)) ? inputAction.negativeDescriptiveName : (inputAction.descriptiveName + " -"));
				}
				else if (inputAction.type == InputActionType.Button)
				{
					this.CreateUIRow(inputAction, AxisRange.Positive, inputAction.descriptiveName);
				}
			}
			this.RedrawUI();
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x00109F98 File Offset: 0x00108198
		private void CreateUIRow(InputAction action, AxisRange actionRange, string label)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.textPrefab);
			gameObject.transform.SetParent(this.actionGroupTransform);
			gameObject.transform.SetAsLastSibling();
			gameObject.GetComponent<Text>().text = label;
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab);
			gameObject2.transform.SetParent(this.fieldGroupTransform);
			gameObject2.transform.SetAsLastSibling();
			this.rows.Add(new SimpleCombinedKeyboardMouseRemapping.Row
			{
				action = action,
				actionRange = actionRange,
				button = gameObject2.GetComponent<Button>(),
				text = gameObject2.GetComponentInChildren<Text>()
			});
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x0010A038 File Offset: 0x00108238
		private void OnInputFieldClicked(int index, int actionElementMapToReplaceId)
		{
			if (index < 0 || index >= this.rows.Count)
			{
				return;
			}
			ControllerMap map = this.player.controllers.maps.GetMap(ControllerType.Keyboard, 0, "Default", "Default");
			ControllerMap map2 = this.player.controllers.maps.GetMap(ControllerType.Mouse, 0, "Default", "Default");
			ControllerMap controllerMap;
			if (map.ContainsElementMap(actionElementMapToReplaceId))
			{
				controllerMap = map;
			}
			else if (map2.ContainsElementMap(actionElementMapToReplaceId))
			{
				controllerMap = map2;
			}
			else
			{
				controllerMap = null;
			}
			this._replaceTargetMapping = new SimpleCombinedKeyboardMouseRemapping.TargetMapping
			{
				actionElementMapId = actionElementMapToReplaceId,
				controllerMap = controllerMap
			};
			base.StartCoroutine(this.StartListeningDelayed(index, map, map2, actionElementMapToReplaceId));
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x00022D12 File Offset: 0x00020F12
		private IEnumerator StartListeningDelayed(int index, ControllerMap keyboardMap, ControllerMap mouseMap, int actionElementMapToReplaceId)
		{
			yield return new WaitForSeconds(0.1f);
			this.inputMapper_keyboard.Start(new InputMapper.Context
			{
				actionId = this.rows[index].action.id,
				controllerMap = keyboardMap,
				actionRange = this.rows[index].actionRange,
				actionElementMapToReplace = keyboardMap.GetElementMap(actionElementMapToReplaceId)
			});
			this.inputMapper_mouse.Start(new InputMapper.Context
			{
				actionId = this.rows[index].action.id,
				controllerMap = mouseMap,
				actionRange = this.rows[index].actionRange,
				actionElementMapToReplace = mouseMap.GetElementMap(actionElementMapToReplaceId)
			});
			this.player.controllers.maps.SetMapsEnabled(false, "UI");
			this.statusUIText.text = "Listening...";
			yield break;
		}

		// Token: 0x06003330 RID: 13104 RVA: 0x0010A0E8 File Offset: 0x001082E8
		private void OnInputMapped(InputMapper.InputMappedEventData data)
		{
			this.inputMapper_keyboard.Stop();
			this.inputMapper_mouse.Stop();
			if (this._replaceTargetMapping.controllerMap != null && data.actionElementMap.controllerMap != this._replaceTargetMapping.controllerMap)
			{
				this._replaceTargetMapping.controllerMap.DeleteElementMap(this._replaceTargetMapping.actionElementMapId);
			}
			this.RedrawUI();
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x00022D3E File Offset: 0x00020F3E
		private void OnStopped(InputMapper.StoppedEventData data)
		{
			this.statusUIText.text = string.Empty;
			this.player.controllers.maps.SetMapsEnabled(true, "UI");
		}

		// Token: 0x04003165 RID: 12645
		private const string category = "Default";

		// Token: 0x04003166 RID: 12646
		private const string layout = "Default";

		// Token: 0x04003167 RID: 12647
		private const string uiCategory = "UI";

		// Token: 0x04003168 RID: 12648
		private InputMapper inputMapper_keyboard = new InputMapper();

		// Token: 0x04003169 RID: 12649
		private InputMapper inputMapper_mouse = new InputMapper();

		// Token: 0x0400316A RID: 12650
		public GameObject buttonPrefab;

		// Token: 0x0400316B RID: 12651
		public GameObject textPrefab;

		// Token: 0x0400316C RID: 12652
		public RectTransform fieldGroupTransform;

		// Token: 0x0400316D RID: 12653
		public RectTransform actionGroupTransform;

		// Token: 0x0400316E RID: 12654
		public Text controllerNameUIText;

		// Token: 0x0400316F RID: 12655
		public Text statusUIText;

		// Token: 0x04003170 RID: 12656
		private List<SimpleCombinedKeyboardMouseRemapping.Row> rows = new List<SimpleCombinedKeyboardMouseRemapping.Row>();

		// Token: 0x04003171 RID: 12657
		private SimpleCombinedKeyboardMouseRemapping.TargetMapping _replaceTargetMapping;

		// Token: 0x020006D9 RID: 1753
		private class Row
		{
			// Token: 0x04003172 RID: 12658
			public InputAction action;

			// Token: 0x04003173 RID: 12659
			public AxisRange actionRange;

			// Token: 0x04003174 RID: 12660
			public Button button;

			// Token: 0x04003175 RID: 12661
			public Text text;
		}

		// Token: 0x020006DA RID: 1754
		private struct TargetMapping
		{
			// Token: 0x04003176 RID: 12662
			public ControllerMap controllerMap;

			// Token: 0x04003177 RID: 12663
			public int actionElementMapId;
		}
	}
}
