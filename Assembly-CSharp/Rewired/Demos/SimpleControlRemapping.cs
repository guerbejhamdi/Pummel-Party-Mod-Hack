using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x020006DD RID: 1757
	[AddComponentMenu("")]
	public class SimpleControlRemapping : MonoBehaviour
	{
		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x0600333C RID: 13116 RVA: 0x00022CD7 File Offset: 0x00020ED7
		private Player player
		{
			get
			{
				return ReInput.players.GetPlayer(0);
			}
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x0600333D RID: 13117 RVA: 0x0010A2B0 File Offset: 0x001084B0
		private ControllerMap controllerMap
		{
			get
			{
				if (this.controller == null)
				{
					return null;
				}
				return this.player.controllers.maps.GetMap(this.controller.type, this.controller.id, "Default", "Default");
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x0600333E RID: 13118 RVA: 0x00022DC5 File Offset: 0x00020FC5
		private Controller controller
		{
			get
			{
				return this.player.controllers.GetController(this.selectedControllerType, this.selectedControllerId);
			}
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x0010A2FC File Offset: 0x001084FC
		private void OnEnable()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.inputMapper.options.timeout = 5f;
			this.inputMapper.options.ignoreMouseXAxis = true;
			this.inputMapper.options.ignoreMouseYAxis = true;
			ReInput.ControllerConnectedEvent += this.OnControllerChanged;
			ReInput.ControllerDisconnectedEvent += this.OnControllerChanged;
			this.inputMapper.InputMappedEvent += this.OnInputMapped;
			this.inputMapper.StoppedEvent += this.OnStopped;
			this.InitializeUI();
		}

		// Token: 0x06003340 RID: 13120 RVA: 0x00022DE3 File Offset: 0x00020FE3
		private void OnDisable()
		{
			this.inputMapper.Stop();
			this.inputMapper.RemoveAllEventListeners();
			ReInput.ControllerConnectedEvent -= this.OnControllerChanged;
			ReInput.ControllerDisconnectedEvent -= this.OnControllerChanged;
		}

		// Token: 0x06003341 RID: 13121 RVA: 0x0010A3A0 File Offset: 0x001085A0
		private void RedrawUI()
		{
			if (this.controller == null)
			{
				this.ClearUI();
				return;
			}
			this.controllerNameUIText.text = this.controller.name;
			for (int i = 0; i < this.rows.Count; i++)
			{
				SimpleControlRemapping.Row row = this.rows[i];
				InputAction action = this.rows[i].action;
				string text = string.Empty;
				int actionElementMapId = -1;
				foreach (ActionElementMap actionElementMap in this.controllerMap.ElementMapsWithAction(action.id))
				{
					if (actionElementMap.ShowInField(row.actionRange))
					{
						text = actionElementMap.elementIdentifierName;
						actionElementMapId = actionElementMap.id;
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

		// Token: 0x06003342 RID: 13122 RVA: 0x0010A4DC File Offset: 0x001086DC
		private void ClearUI()
		{
			if (this.selectedControllerType == ControllerType.Joystick)
			{
				this.controllerNameUIText.text = "No joysticks attached";
			}
			else
			{
				this.controllerNameUIText.text = string.Empty;
			}
			for (int i = 0; i < this.rows.Count; i++)
			{
				this.rows[i].text.text = string.Empty;
			}
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x0010A548 File Offset: 0x00108748
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

		// Token: 0x06003344 RID: 13124 RVA: 0x0010A6C0 File Offset: 0x001088C0
		private void CreateUIRow(InputAction action, AxisRange actionRange, string label)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.textPrefab);
			gameObject.transform.SetParent(this.actionGroupTransform);
			gameObject.transform.SetAsLastSibling();
			gameObject.GetComponent<Text>().text = label;
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab);
			gameObject2.transform.SetParent(this.fieldGroupTransform);
			gameObject2.transform.SetAsLastSibling();
			this.rows.Add(new SimpleControlRemapping.Row
			{
				action = action,
				actionRange = actionRange,
				button = gameObject2.GetComponent<Button>(),
				text = gameObject2.GetComponentInChildren<Text>()
			});
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x0010A760 File Offset: 0x00108960
		private void SetSelectedController(ControllerType controllerType)
		{
			bool flag = false;
			if (controllerType != this.selectedControllerType)
			{
				this.selectedControllerType = controllerType;
				flag = true;
			}
			int num = this.selectedControllerId;
			if (this.selectedControllerType == ControllerType.Joystick)
			{
				if (this.player.controllers.joystickCount > 0)
				{
					this.selectedControllerId = this.player.controllers.Joysticks[0].id;
				}
				else
				{
					this.selectedControllerId = -1;
				}
			}
			else
			{
				this.selectedControllerId = 0;
			}
			if (this.selectedControllerId != num)
			{
				flag = true;
			}
			if (flag)
			{
				this.inputMapper.Stop();
				this.RedrawUI();
			}
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x00022E1D File Offset: 0x0002101D
		public void OnControllerSelected(int controllerType)
		{
			this.SetSelectedController((ControllerType)controllerType);
		}

		// Token: 0x06003347 RID: 13127 RVA: 0x00022E26 File Offset: 0x00021026
		private void OnInputFieldClicked(int index, int actionElementMapToReplaceId)
		{
			if (index < 0 || index >= this.rows.Count)
			{
				return;
			}
			if (this.controller == null)
			{
				return;
			}
			base.StartCoroutine(this.StartListeningDelayed(index, actionElementMapToReplaceId));
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x00022E53 File Offset: 0x00021053
		private IEnumerator StartListeningDelayed(int index, int actionElementMapToReplaceId)
		{
			yield return new WaitForSeconds(0.1f);
			this.inputMapper.Start(new InputMapper.Context
			{
				actionId = this.rows[index].action.id,
				controllerMap = this.controllerMap,
				actionRange = this.rows[index].actionRange,
				actionElementMapToReplace = this.controllerMap.GetElementMap(actionElementMapToReplaceId)
			});
			this.player.controllers.maps.SetMapsEnabled(false, "UI");
			this.statusUIText.text = "Listening...";
			yield break;
		}

		// Token: 0x06003349 RID: 13129 RVA: 0x00022E70 File Offset: 0x00021070
		private void OnControllerChanged(ControllerStatusChangedEventArgs args)
		{
			this.SetSelectedController(this.selectedControllerType);
		}

		// Token: 0x0600334A RID: 13130 RVA: 0x00022E7E File Offset: 0x0002107E
		private void OnInputMapped(InputMapper.InputMappedEventData data)
		{
			this.RedrawUI();
		}

		// Token: 0x0600334B RID: 13131 RVA: 0x00022E86 File Offset: 0x00021086
		private void OnStopped(InputMapper.StoppedEventData data)
		{
			this.statusUIText.text = string.Empty;
			this.player.controllers.maps.SetMapsEnabled(true, "UI");
		}

		// Token: 0x04003182 RID: 12674
		private const string category = "Default";

		// Token: 0x04003183 RID: 12675
		private const string layout = "Default";

		// Token: 0x04003184 RID: 12676
		private const string uiCategory = "UI";

		// Token: 0x04003185 RID: 12677
		private InputMapper inputMapper = new InputMapper();

		// Token: 0x04003186 RID: 12678
		public GameObject buttonPrefab;

		// Token: 0x04003187 RID: 12679
		public GameObject textPrefab;

		// Token: 0x04003188 RID: 12680
		public RectTransform fieldGroupTransform;

		// Token: 0x04003189 RID: 12681
		public RectTransform actionGroupTransform;

		// Token: 0x0400318A RID: 12682
		public Text controllerNameUIText;

		// Token: 0x0400318B RID: 12683
		public Text statusUIText;

		// Token: 0x0400318C RID: 12684
		private ControllerType selectedControllerType;

		// Token: 0x0400318D RID: 12685
		private int selectedControllerId;

		// Token: 0x0400318E RID: 12686
		private List<SimpleControlRemapping.Row> rows = new List<SimpleControlRemapping.Row>();

		// Token: 0x020006DE RID: 1758
		private class Row
		{
			// Token: 0x0400318F RID: 12687
			public InputAction action;

			// Token: 0x04003190 RID: 12688
			public AxisRange actionRange;

			// Token: 0x04003191 RID: 12689
			public Button button;

			// Token: 0x04003192 RID: 12690
			public Text text;
		}
	}
}
