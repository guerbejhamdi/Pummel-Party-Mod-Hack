using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020006B7 RID: 1719
	[AddComponentMenu("")]
	public class ControlRemappingDemo1 : MonoBehaviour
	{
		// Token: 0x06003221 RID: 12833 RVA: 0x000220D0 File Offset: 0x000202D0
		private void Awake()
		{
			this.inputMapper.options.timeout = 5f;
			this.inputMapper.options.ignoreMouseXAxis = true;
			this.inputMapper.options.ignoreMouseYAxis = true;
			this.Initialize();
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x0002210F File Offset: 0x0002030F
		private void OnEnable()
		{
			this.Subscribe();
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x00022117 File Offset: 0x00020317
		private void OnDisable()
		{
			this.Unsubscribe();
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x001056F0 File Offset: 0x001038F0
		private void Initialize()
		{
			this.dialog = new ControlRemappingDemo1.DialogHelper();
			this.actionQueue = new Queue<ControlRemappingDemo1.QueueEntry>();
			this.selectedController = new ControlRemappingDemo1.ControllerSelection();
			ReInput.ControllerConnectedEvent += this.JoystickConnected;
			ReInput.ControllerPreDisconnectEvent += this.JoystickPreDisconnect;
			ReInput.ControllerDisconnectedEvent += this.JoystickDisconnected;
			this.ResetAll();
			this.initialized = true;
			ReInput.userDataStore.Load();
			if (ReInput.unityJoystickIdentificationRequired)
			{
				this.IdentifyAllJoysticks();
			}
		}

		// Token: 0x06003225 RID: 12837 RVA: 0x00105778 File Offset: 0x00103978
		private void Setup()
		{
			if (this.setupFinished)
			{
				return;
			}
			this.style_wordWrap = new GUIStyle(GUI.skin.label);
			this.style_wordWrap.wordWrap = true;
			this.style_centeredBox = new GUIStyle(GUI.skin.box);
			this.style_centeredBox.alignment = TextAnchor.MiddleCenter;
			this.setupFinished = true;
		}

		// Token: 0x06003226 RID: 12838 RVA: 0x0002211F File Offset: 0x0002031F
		private void Subscribe()
		{
			this.Unsubscribe();
			this.inputMapper.ConflictFoundEvent += this.OnConflictFound;
			this.inputMapper.StoppedEvent += this.OnStopped;
		}

		// Token: 0x06003227 RID: 12839 RVA: 0x00022155 File Offset: 0x00020355
		private void Unsubscribe()
		{
			this.inputMapper.RemoveAllEventListeners();
		}

		// Token: 0x06003228 RID: 12840 RVA: 0x001057D8 File Offset: 0x001039D8
		public void OnGUI()
		{
			if (!this.initialized)
			{
				return;
			}
			this.Setup();
			this.HandleMenuControl();
			if (!this.showMenu)
			{
				this.DrawInitialScreen();
				return;
			}
			this.SetGUIStateStart();
			this.ProcessQueue();
			this.DrawPage();
			this.ShowDialog();
			this.SetGUIStateEnd();
			this.busy = false;
		}

		// Token: 0x06003229 RID: 12841 RVA: 0x00105830 File Offset: 0x00103A30
		private void HandleMenuControl()
		{
			if (this.dialog.enabled)
			{
				return;
			}
			if (Event.current.type == EventType.Layout && ReInput.players.GetSystemPlayer().GetButtonDown("Menu"))
			{
				if (this.showMenu)
				{
					ReInput.userDataStore.Save();
					this.Close();
					return;
				}
				this.Open();
			}
		}

		// Token: 0x0600322A RID: 12842 RVA: 0x00022162 File Offset: 0x00020362
		private void Close()
		{
			this.ClearWorkingVars();
			this.showMenu = false;
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x00022171 File Offset: 0x00020371
		private void Open()
		{
			this.showMenu = true;
		}

		// Token: 0x0600322C RID: 12844 RVA: 0x00105890 File Offset: 0x00103A90
		private void DrawInitialScreen()
		{
			ActionElementMap firstElementMapWithAction = ReInput.players.GetSystemPlayer().controllers.maps.GetFirstElementMapWithAction("Menu", true);
			GUIContent content;
			if (firstElementMapWithAction != null)
			{
				content = new GUIContent("Press " + firstElementMapWithAction.elementIdentifierName + " to open the menu.");
			}
			else
			{
				content = new GUIContent("There is no element assigned to open the menu!");
			}
			GUILayout.BeginArea(this.GetScreenCenteredRect(300f, 50f));
			GUILayout.Box(content, this.style_centeredBox, new GUILayoutOption[]
			{
				GUILayout.ExpandHeight(true),
				GUILayout.ExpandWidth(true)
			});
			GUILayout.EndArea();
		}

		// Token: 0x0600322D RID: 12845 RVA: 0x00105928 File Offset: 0x00103B28
		private void DrawPage()
		{
			if (GUI.enabled != this.pageGUIState)
			{
				GUI.enabled = this.pageGUIState;
			}
			GUILayout.BeginArea(new Rect(((float)Screen.width - (float)Screen.width * 0.9f) * 0.5f, ((float)Screen.height - (float)Screen.height * 0.9f) * 0.5f, (float)Screen.width * 0.9f, (float)Screen.height * 0.9f));
			this.DrawPlayerSelector();
			this.DrawJoystickSelector();
			this.DrawMouseAssignment();
			this.DrawControllerSelector();
			this.DrawCalibrateButton();
			this.DrawMapCategories();
			this.actionScrollPos = GUILayout.BeginScrollView(this.actionScrollPos, Array.Empty<GUILayoutOption>());
			this.DrawCategoryActions();
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}

		// Token: 0x0600322E RID: 12846 RVA: 0x001059EC File Offset: 0x00103BEC
		private void DrawPlayerSelector()
		{
			if (ReInput.players.allPlayerCount == 0)
			{
				GUILayout.Label("There are no players.", Array.Empty<GUILayoutOption>());
				return;
			}
			GUILayout.Space(15f);
			GUILayout.Label("Players:", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			foreach (Player player in ReInput.players.GetPlayers(true))
			{
				if (this.selectedPlayer == null)
				{
					this.selectedPlayer = player;
				}
				bool flag = player == this.selectedPlayer;
				bool flag2 = GUILayout.Toggle(flag, (player.descriptiveName != string.Empty) ? player.descriptiveName : player.name, "Button", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				});
				if (flag2 != flag && flag2)
				{
					this.selectedPlayer = player;
					this.selectedController.Clear();
					this.selectedMapCategoryId = -1;
				}
			}
			GUILayout.EndHorizontal();
		}

		// Token: 0x0600322F RID: 12847 RVA: 0x00105B00 File Offset: 0x00103D00
		private void DrawMouseAssignment()
		{
			bool enabled = GUI.enabled;
			if (this.selectedPlayer == null)
			{
				GUI.enabled = false;
			}
			GUILayout.Space(15f);
			GUILayout.Label("Assign Mouse:", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			bool flag = this.selectedPlayer != null && this.selectedPlayer.controllers.hasMouse;
			bool flag2 = GUILayout.Toggle(flag, "Assign Mouse", "Button", new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			});
			if (flag2 != flag)
			{
				if (flag2)
				{
					this.selectedPlayer.controllers.hasMouse = true;
					using (IEnumerator<Player> enumerator = ReInput.players.Players.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Player player = enumerator.Current;
							if (player != this.selectedPlayer)
							{
								player.controllers.hasMouse = false;
							}
						}
						goto IL_E9;
					}
				}
				this.selectedPlayer.controllers.hasMouse = false;
			}
			IL_E9:
			GUILayout.EndHorizontal();
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06003230 RID: 12848 RVA: 0x00105C1C File Offset: 0x00103E1C
		private void DrawJoystickSelector()
		{
			bool enabled = GUI.enabled;
			if (this.selectedPlayer == null)
			{
				GUI.enabled = false;
			}
			GUILayout.Space(15f);
			GUILayout.Label("Assign Joysticks:", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			bool flag = this.selectedPlayer == null || this.selectedPlayer.controllers.joystickCount == 0;
			bool flag2 = GUILayout.Toggle(flag, "None", "Button", new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			});
			if (flag2 != flag)
			{
				this.selectedPlayer.controllers.ClearControllersOfType(ControllerType.Joystick);
				this.ControllerSelectionChanged();
			}
			if (this.selectedPlayer != null)
			{
				foreach (Joystick joystick in ReInput.controllers.Joysticks)
				{
					flag = this.selectedPlayer.controllers.ContainsController(joystick);
					flag2 = GUILayout.Toggle(flag, joystick.name, "Button", new GUILayoutOption[]
					{
						GUILayout.ExpandWidth(false)
					});
					if (flag2 != flag)
					{
						this.EnqueueAction(new ControlRemappingDemo1.JoystickAssignmentChange(this.selectedPlayer.id, joystick.id, flag2));
					}
				}
			}
			GUILayout.EndHorizontal();
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06003231 RID: 12849 RVA: 0x00105D78 File Offset: 0x00103F78
		private void DrawControllerSelector()
		{
			if (this.selectedPlayer == null)
			{
				return;
			}
			bool enabled = GUI.enabled;
			GUILayout.Space(15f);
			GUILayout.Label("Controller to Map:", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (!this.selectedController.hasSelection)
			{
				this.selectedController.Set(0, ControllerType.Keyboard);
				this.ControllerSelectionChanged();
			}
			bool flag = this.selectedController.type == ControllerType.Keyboard;
			if (GUILayout.Toggle(flag, "Keyboard", "Button", new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			}) != flag)
			{
				this.selectedController.Set(0, ControllerType.Keyboard);
				this.ControllerSelectionChanged();
			}
			if (!this.selectedPlayer.controllers.hasMouse)
			{
				GUI.enabled = false;
			}
			flag = (this.selectedController.type == ControllerType.Mouse);
			if (GUILayout.Toggle(flag, "Mouse", "Button", new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			}) != flag)
			{
				this.selectedController.Set(0, ControllerType.Mouse);
				this.ControllerSelectionChanged();
			}
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
			foreach (Joystick joystick in this.selectedPlayer.controllers.Joysticks)
			{
				flag = (this.selectedController.type == ControllerType.Joystick && this.selectedController.id == joystick.id);
				if (GUILayout.Toggle(flag, joystick.name, "Button", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				}) != flag)
				{
					this.selectedController.Set(joystick.id, ControllerType.Joystick);
					this.ControllerSelectionChanged();
				}
			}
			GUILayout.EndHorizontal();
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06003232 RID: 12850 RVA: 0x00105F4C File Offset: 0x0010414C
		private void DrawCalibrateButton()
		{
			if (this.selectedPlayer == null)
			{
				return;
			}
			bool enabled = GUI.enabled;
			GUILayout.Space(10f);
			Controller controller = this.selectedController.hasSelection ? this.selectedPlayer.controllers.GetController(this.selectedController.type, this.selectedController.id) : null;
			if (controller == null || this.selectedController.type != ControllerType.Joystick)
			{
				GUI.enabled = false;
				GUILayout.Button("Select a controller to calibrate", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				});
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}
			else if (GUILayout.Button("Calibrate " + controller.name, new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			}))
			{
				Joystick joystick = controller as Joystick;
				if (joystick != null)
				{
					CalibrationMap calibrationMap = joystick.calibrationMap;
					if (calibrationMap != null)
					{
						this.EnqueueAction(new ControlRemappingDemo1.Calibration(this.selectedPlayer, joystick, calibrationMap));
					}
				}
			}
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06003233 RID: 12851 RVA: 0x00106048 File Offset: 0x00104248
		private void DrawMapCategories()
		{
			if (this.selectedPlayer == null)
			{
				return;
			}
			if (!this.selectedController.hasSelection)
			{
				return;
			}
			bool enabled = GUI.enabled;
			GUILayout.Space(15f);
			GUILayout.Label("Categories:", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			foreach (InputMapCategory inputMapCategory in ReInput.mapping.UserAssignableMapCategories)
			{
				if (!this.selectedPlayer.controllers.maps.ContainsMapInCategory(this.selectedController.type, inputMapCategory.id))
				{
					GUI.enabled = false;
				}
				else if (this.selectedMapCategoryId < 0)
				{
					this.selectedMapCategoryId = inputMapCategory.id;
					this.selectedMap = this.selectedPlayer.controllers.maps.GetFirstMapInCategory(this.selectedController.type, this.selectedController.id, inputMapCategory.id);
				}
				bool flag = inputMapCategory.id == this.selectedMapCategoryId;
				if (GUILayout.Toggle(flag, (inputMapCategory.descriptiveName != string.Empty) ? inputMapCategory.descriptiveName : inputMapCategory.name, "Button", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				}) != flag)
				{
					this.selectedMapCategoryId = inputMapCategory.id;
					this.selectedMap = this.selectedPlayer.controllers.maps.GetFirstMapInCategory(this.selectedController.type, this.selectedController.id, inputMapCategory.id);
				}
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}
			GUILayout.EndHorizontal();
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06003234 RID: 12852 RVA: 0x0010621C File Offset: 0x0010441C
		private void DrawCategoryActions()
		{
			if (this.selectedPlayer == null)
			{
				return;
			}
			if (this.selectedMapCategoryId < 0)
			{
				return;
			}
			bool enabled = GUI.enabled;
			if (this.selectedMap == null)
			{
				return;
			}
			GUILayout.Space(15f);
			GUILayout.Label("Actions:", Array.Empty<GUILayoutOption>());
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(this.selectedMapCategoryId);
			if (mapCategory == null)
			{
				return;
			}
			InputCategory actionCategory = ReInput.mapping.GetActionCategory(mapCategory.name);
			if (actionCategory == null)
			{
				return;
			}
			float width = 150f;
			foreach (InputAction inputAction in ReInput.mapping.ActionsInCategory(actionCategory.id))
			{
				string text = (inputAction.descriptiveName != string.Empty) ? inputAction.descriptiveName : inputAction.name;
				if (inputAction.type == InputActionType.Button)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label(text, new GUILayoutOption[]
					{
						GUILayout.Width(width)
					});
					this.DrawAddActionMapButton(this.selectedPlayer.id, inputAction, AxisRange.Positive, this.selectedController, this.selectedMap);
					foreach (ActionElementMap actionElementMap in this.selectedMap.AllMaps)
					{
						if (actionElementMap.actionId == inputAction.id)
						{
							this.DrawActionAssignmentButton(this.selectedPlayer.id, inputAction, AxisRange.Positive, this.selectedController, this.selectedMap, actionElementMap);
						}
					}
					GUILayout.EndHorizontal();
				}
				else if (inputAction.type == InputActionType.Axis)
				{
					if (this.selectedController.type != ControllerType.Keyboard)
					{
						GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
						GUILayout.Label(text, new GUILayoutOption[]
						{
							GUILayout.Width(width)
						});
						this.DrawAddActionMapButton(this.selectedPlayer.id, inputAction, AxisRange.Full, this.selectedController, this.selectedMap);
						foreach (ActionElementMap actionElementMap2 in this.selectedMap.AllMaps)
						{
							if (actionElementMap2.actionId == inputAction.id && actionElementMap2.elementType != ControllerElementType.Button && actionElementMap2.axisType != AxisType.Split)
							{
								this.DrawActionAssignmentButton(this.selectedPlayer.id, inputAction, AxisRange.Full, this.selectedController, this.selectedMap, actionElementMap2);
								this.DrawInvertButton(this.selectedPlayer.id, inputAction, Pole.Positive, this.selectedController, this.selectedMap, actionElementMap2);
							}
						}
						GUILayout.EndHorizontal();
					}
					string text2 = (inputAction.positiveDescriptiveName != string.Empty) ? inputAction.positiveDescriptiveName : (inputAction.descriptiveName + " +");
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label(text2, new GUILayoutOption[]
					{
						GUILayout.Width(width)
					});
					this.DrawAddActionMapButton(this.selectedPlayer.id, inputAction, AxisRange.Positive, this.selectedController, this.selectedMap);
					foreach (ActionElementMap actionElementMap3 in this.selectedMap.AllMaps)
					{
						if (actionElementMap3.actionId == inputAction.id && actionElementMap3.axisContribution == Pole.Positive && actionElementMap3.axisType != AxisType.Normal)
						{
							this.DrawActionAssignmentButton(this.selectedPlayer.id, inputAction, AxisRange.Positive, this.selectedController, this.selectedMap, actionElementMap3);
						}
					}
					GUILayout.EndHorizontal();
					string text3 = (inputAction.negativeDescriptiveName != string.Empty) ? inputAction.negativeDescriptiveName : (inputAction.descriptiveName + " -");
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label(text3, new GUILayoutOption[]
					{
						GUILayout.Width(width)
					});
					this.DrawAddActionMapButton(this.selectedPlayer.id, inputAction, AxisRange.Negative, this.selectedController, this.selectedMap);
					foreach (ActionElementMap actionElementMap4 in this.selectedMap.AllMaps)
					{
						if (actionElementMap4.actionId == inputAction.id && actionElementMap4.axisContribution == Pole.Negative && actionElementMap4.axisType != AxisType.Normal)
						{
							this.DrawActionAssignmentButton(this.selectedPlayer.id, inputAction, AxisRange.Negative, this.selectedController, this.selectedMap, actionElementMap4);
						}
					}
					GUILayout.EndHorizontal();
				}
			}
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06003235 RID: 12853 RVA: 0x00106710 File Offset: 0x00104910
		private void DrawActionAssignmentButton(int playerId, InputAction action, AxisRange actionRange, ControlRemappingDemo1.ControllerSelection controller, ControllerMap controllerMap, ActionElementMap elementMap)
		{
			if (GUILayout.Button(elementMap.elementIdentifierName, new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false),
				GUILayout.MinWidth(30f)
			}))
			{
				InputMapper.Context context = new InputMapper.Context
				{
					actionId = action.id,
					actionRange = actionRange,
					controllerMap = controllerMap,
					actionElementMapToReplace = elementMap
				};
				this.EnqueueAction(new ControlRemappingDemo1.ElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChangeType.ReassignOrRemove, context));
				this.startListening = true;
			}
			GUILayout.Space(4f);
		}

		// Token: 0x06003236 RID: 12854 RVA: 0x00106790 File Offset: 0x00104990
		private void DrawInvertButton(int playerId, InputAction action, Pole actionAxisContribution, ControlRemappingDemo1.ControllerSelection controller, ControllerMap controllerMap, ActionElementMap elementMap)
		{
			bool invert = elementMap.invert;
			bool flag = GUILayout.Toggle(invert, "Invert", new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			});
			if (flag != invert)
			{
				elementMap.invert = flag;
			}
			GUILayout.Space(10f);
		}

		// Token: 0x06003237 RID: 12855 RVA: 0x001067D8 File Offset: 0x001049D8
		private void DrawAddActionMapButton(int playerId, InputAction action, AxisRange actionRange, ControlRemappingDemo1.ControllerSelection controller, ControllerMap controllerMap)
		{
			if (GUILayout.Button("Add...", new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			}))
			{
				InputMapper.Context context = new InputMapper.Context
				{
					actionId = action.id,
					actionRange = actionRange,
					controllerMap = controllerMap
				};
				this.EnqueueAction(new ControlRemappingDemo1.ElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChangeType.Add, context));
				this.startListening = true;
			}
			GUILayout.Space(10f);
		}

		// Token: 0x06003238 RID: 12856 RVA: 0x0002217A File Offset: 0x0002037A
		private void ShowDialog()
		{
			this.dialog.Update();
		}

		// Token: 0x06003239 RID: 12857 RVA: 0x00106840 File Offset: 0x00104A40
		private void DrawModalWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			this.dialog.DrawConfirmButton("Okay");
			GUILayout.FlexibleSpace();
			this.dialog.DrawCancelButton();
			GUILayout.EndHorizontal();
		}

		// Token: 0x0600323A RID: 12858 RVA: 0x001068AC File Offset: 0x00104AAC
		private void DrawModalWindow_OkayOnly(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			this.dialog.DrawConfirmButton("Okay");
			GUILayout.EndHorizontal();
		}

		// Token: 0x0600323B RID: 12859 RVA: 0x00106908 File Offset: 0x00104B08
		private void DrawElementAssignmentWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			ControlRemappingDemo1.ElementAssignmentChange elementAssignmentChange = this.actionQueue.Peek() as ControlRemappingDemo1.ElementAssignmentChange;
			if (elementAssignmentChange == null)
			{
				this.dialog.Cancel();
				return;
			}
			float num;
			if (!this.dialog.busy)
			{
				if (this.startListening && this.inputMapper.status == InputMapper.Status.Idle)
				{
					this.inputMapper.Start(elementAssignmentChange.context);
					this.startListening = false;
				}
				if (this.conflictFoundEventData != null)
				{
					this.dialog.Confirm();
					return;
				}
				num = this.inputMapper.timeRemaining;
				if (num == 0f)
				{
					this.dialog.Cancel();
					return;
				}
			}
			else
			{
				num = this.inputMapper.options.timeout;
			}
			GUILayout.Label("Assignment will be canceled in " + ((int)Mathf.Ceil(num)).ToString() + "...", this.style_wordWrap, Array.Empty<GUILayoutOption>());
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x00106A14 File Offset: 0x00104C14
		private void DrawElementAssignmentProtectedConflictWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			if (!(this.actionQueue.Peek() is ControlRemappingDemo1.ElementAssignmentChange))
			{
				this.dialog.Cancel();
				return;
			}
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			this.dialog.DrawConfirmButton(ControlRemappingDemo1.UserResponse.Custom1, "Add");
			GUILayout.FlexibleSpace();
			this.dialog.DrawCancelButton();
			GUILayout.EndHorizontal();
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x00106AA0 File Offset: 0x00104CA0
		private void DrawElementAssignmentNormalConflictWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			if (!(this.actionQueue.Peek() is ControlRemappingDemo1.ElementAssignmentChange))
			{
				this.dialog.Cancel();
				return;
			}
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			this.dialog.DrawConfirmButton(ControlRemappingDemo1.UserResponse.Confirm, "Replace");
			GUILayout.FlexibleSpace();
			this.dialog.DrawConfirmButton(ControlRemappingDemo1.UserResponse.Custom1, "Add");
			GUILayout.FlexibleSpace();
			this.dialog.DrawCancelButton();
			GUILayout.EndHorizontal();
		}

		// Token: 0x0600323E RID: 12862 RVA: 0x00106B40 File Offset: 0x00104D40
		private void DrawReassignOrRemoveElementAssignmentWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			this.dialog.DrawConfirmButton("Reassign");
			GUILayout.FlexibleSpace();
			this.dialog.DrawCancelButton("Remove");
			GUILayout.EndHorizontal();
		}

		// Token: 0x0600323F RID: 12863 RVA: 0x00106BB0 File Offset: 0x00104DB0
		private void DrawFallbackJoystickIdentificationWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			ControlRemappingDemo1.FallbackJoystickIdentification fallbackJoystickIdentification = this.actionQueue.Peek() as ControlRemappingDemo1.FallbackJoystickIdentification;
			if (fallbackJoystickIdentification == null)
			{
				this.dialog.Cancel();
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.Label("Press any button or axis on \"" + fallbackJoystickIdentification.joystickName + "\" now.", this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Skip", Array.Empty<GUILayoutOption>()))
			{
				this.dialog.Cancel();
				return;
			}
			if (this.dialog.busy)
			{
				return;
			}
			if (!ReInput.controllers.SetUnityJoystickIdFromAnyButtonOrAxisPress(fallbackJoystickIdentification.joystickId, 0.8f, false))
			{
				return;
			}
			this.dialog.Confirm();
		}

		// Token: 0x06003240 RID: 12864 RVA: 0x00106C80 File Offset: 0x00104E80
		private void DrawCalibrationWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			ControlRemappingDemo1.Calibration calibration = this.actionQueue.Peek() as ControlRemappingDemo1.Calibration;
			if (calibration == null)
			{
				this.dialog.Cancel();
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			bool enabled = GUI.enabled;
			GUILayout.BeginVertical(new GUILayoutOption[]
			{
				GUILayout.Width(200f)
			});
			this.calibrateScrollPos = GUILayout.BeginScrollView(this.calibrateScrollPos, Array.Empty<GUILayoutOption>());
			if (calibration.recording)
			{
				GUI.enabled = false;
			}
			IList<ControllerElementIdentifier> axisElementIdentifiers = calibration.joystick.AxisElementIdentifiers;
			for (int i = 0; i < axisElementIdentifiers.Count; i++)
			{
				ControllerElementIdentifier controllerElementIdentifier = axisElementIdentifiers[i];
				bool flag = calibration.selectedElementIdentifierId == controllerElementIdentifier.id;
				bool flag2 = GUILayout.Toggle(flag, controllerElementIdentifier.name, "Button", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				});
				if (flag != flag2)
				{
					calibration.selectedElementIdentifierId = controllerElementIdentifier.id;
				}
			}
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.BeginVertical(new GUILayoutOption[]
			{
				GUILayout.Width(200f)
			});
			if (calibration.selectedElementIdentifierId >= 0)
			{
				float axisRawById = calibration.joystick.GetAxisRawById(calibration.selectedElementIdentifierId);
				GUILayout.Label("Raw Value: " + axisRawById.ToString(), Array.Empty<GUILayoutOption>());
				int axisIndexById = calibration.joystick.GetAxisIndexById(calibration.selectedElementIdentifierId);
				AxisCalibration axis = calibration.calibrationMap.GetAxis(axisIndexById);
				GUILayout.Label("Calibrated Value: " + calibration.joystick.GetAxisById(calibration.selectedElementIdentifierId).ToString(), Array.Empty<GUILayoutOption>());
				GUILayout.Label("Zero: " + axis.calibratedZero.ToString(), Array.Empty<GUILayoutOption>());
				GUILayout.Label("Min: " + axis.calibratedMin.ToString(), Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max: " + axis.calibratedMax.ToString(), Array.Empty<GUILayoutOption>());
				GUILayout.Label("Dead Zone: " + axis.deadZone.ToString(), Array.Empty<GUILayoutOption>());
				GUILayout.Space(15f);
				bool flag3 = GUILayout.Toggle(axis.enabled, "Enabled", "Button", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				});
				if (axis.enabled != flag3)
				{
					axis.enabled = flag3;
				}
				GUILayout.Space(10f);
				bool flag4 = GUILayout.Toggle(calibration.recording, "Record Min/Max", "Button", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				});
				if (flag4 != calibration.recording)
				{
					if (flag4)
					{
						axis.calibratedMax = 0f;
						axis.calibratedMin = 0f;
					}
					calibration.recording = flag4;
				}
				if (calibration.recording)
				{
					axis.calibratedMin = Mathf.Min(new float[]
					{
						axis.calibratedMin,
						axisRawById,
						axis.calibratedMin
					});
					axis.calibratedMax = Mathf.Max(new float[]
					{
						axis.calibratedMax,
						axisRawById,
						axis.calibratedMax
					});
					GUI.enabled = false;
				}
				if (GUILayout.Button("Set Zero", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				}))
				{
					axis.calibratedZero = axisRawById;
				}
				if (GUILayout.Button("Set Dead Zone", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				}))
				{
					axis.deadZone = axisRawById;
				}
				bool flag5 = GUILayout.Toggle(axis.invert, "Invert", "Button", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				});
				if (axis.invert != flag5)
				{
					axis.invert = flag5;
				}
				GUILayout.Space(10f);
				if (GUILayout.Button("Reset", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				}))
				{
					axis.Reset();
				}
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}
			else
			{
				GUILayout.Label("Select an axis to begin.", Array.Empty<GUILayoutOption>());
			}
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			if (calibration.recording)
			{
				GUI.enabled = false;
			}
			if (GUILayout.Button("Close", Array.Empty<GUILayoutOption>()))
			{
				this.calibrateScrollPos = default(Vector2);
				this.dialog.Confirm();
			}
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06003241 RID: 12865 RVA: 0x00107120 File Offset: 0x00105320
		private void DialogResultCallback(int queueActionId, ControlRemappingDemo1.UserResponse response)
		{
			foreach (ControlRemappingDemo1.QueueEntry queueEntry in this.actionQueue)
			{
				if (queueEntry.id == queueActionId)
				{
					if (response != ControlRemappingDemo1.UserResponse.Cancel)
					{
						queueEntry.Confirm(response);
						break;
					}
					queueEntry.Cancel();
					break;
				}
			}
		}

		// Token: 0x06003242 RID: 12866 RVA: 0x00022187 File Offset: 0x00020387
		private Rect GetScreenCenteredRect(float width, float height)
		{
			return new Rect((float)Screen.width * 0.5f - width * 0.5f, (float)((double)Screen.height * 0.5 - (double)(height * 0.5f)), width, height);
		}

		// Token: 0x06003243 RID: 12867 RVA: 0x000221BF File Offset: 0x000203BF
		private void EnqueueAction(ControlRemappingDemo1.QueueEntry entry)
		{
			if (entry == null)
			{
				return;
			}
			this.busy = true;
			GUI.enabled = false;
			this.actionQueue.Enqueue(entry);
		}

		// Token: 0x06003244 RID: 12868 RVA: 0x0010718C File Offset: 0x0010538C
		private void ProcessQueue()
		{
			if (this.dialog.enabled)
			{
				return;
			}
			if (this.busy || this.actionQueue.Count == 0)
			{
				return;
			}
			while (this.actionQueue.Count > 0)
			{
				ControlRemappingDemo1.QueueEntry queueEntry = this.actionQueue.Peek();
				bool flag = false;
				switch (queueEntry.queueActionType)
				{
				case ControlRemappingDemo1.QueueActionType.JoystickAssignment:
					flag = this.ProcessJoystickAssignmentChange((ControlRemappingDemo1.JoystickAssignmentChange)queueEntry);
					break;
				case ControlRemappingDemo1.QueueActionType.ElementAssignment:
					flag = this.ProcessElementAssignmentChange((ControlRemappingDemo1.ElementAssignmentChange)queueEntry);
					break;
				case ControlRemappingDemo1.QueueActionType.FallbackJoystickIdentification:
					flag = this.ProcessFallbackJoystickIdentification((ControlRemappingDemo1.FallbackJoystickIdentification)queueEntry);
					break;
				case ControlRemappingDemo1.QueueActionType.Calibrate:
					flag = this.ProcessCalibration((ControlRemappingDemo1.Calibration)queueEntry);
					break;
				}
				if (!flag)
				{
					break;
				}
				this.actionQueue.Dequeue();
			}
		}

		// Token: 0x06003245 RID: 12869 RVA: 0x00107248 File Offset: 0x00105448
		private bool ProcessJoystickAssignmentChange(ControlRemappingDemo1.JoystickAssignmentChange entry)
		{
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				return true;
			}
			Player player = ReInput.players.GetPlayer(entry.playerId);
			if (player == null)
			{
				return true;
			}
			if (!entry.assign)
			{
				player.controllers.RemoveController(ControllerType.Joystick, entry.joystickId);
				this.ControllerSelectionChanged();
				return true;
			}
			if (player.controllers.ContainsController(ControllerType.Joystick, entry.joystickId))
			{
				return true;
			}
			if (!ReInput.controllers.IsJoystickAssigned(entry.joystickId) || entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				player.controllers.AddController(ControllerType.Joystick, entry.joystickId, true);
				this.ControllerSelectionChanged();
				return true;
			}
			this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.JoystickConflict, new ControlRemappingDemo1.WindowProperties
			{
				title = "Joystick Reassignment",
				message = "This joystick is already assigned to another player. Do you want to reassign this joystick to " + player.descriptiveName + "?",
				rect = this.GetScreenCenteredRect(250f, 200f),
				windowDrawDelegate = new Action<string, string>(this.DrawModalWindow)
			}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
			return false;
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x00107360 File Offset: 0x00105560
		private bool ProcessElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChange entry)
		{
			switch (entry.changeType)
			{
			case ControlRemappingDemo1.ElementAssignmentChangeType.Add:
			case ControlRemappingDemo1.ElementAssignmentChangeType.Replace:
				return this.ProcessAddOrReplaceElementAssignment(entry);
			case ControlRemappingDemo1.ElementAssignmentChangeType.Remove:
				return this.ProcessRemoveElementAssignment(entry);
			case ControlRemappingDemo1.ElementAssignmentChangeType.ReassignOrRemove:
				return this.ProcessRemoveOrReassignElementAssignment(entry);
			case ControlRemappingDemo1.ElementAssignmentChangeType.ConflictCheck:
				return this.ProcessElementAssignmentConflictCheck(entry);
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x001073B8 File Offset: 0x001055B8
		private bool ProcessRemoveOrReassignElementAssignment(ControlRemappingDemo1.ElementAssignmentChange entry)
		{
			if (entry.context.controllerMap == null)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				ControlRemappingDemo1.ElementAssignmentChange elementAssignmentChange = new ControlRemappingDemo1.ElementAssignmentChange(entry);
				elementAssignmentChange.changeType = ControlRemappingDemo1.ElementAssignmentChangeType.Remove;
				this.actionQueue.Enqueue(elementAssignmentChange);
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				ControlRemappingDemo1.ElementAssignmentChange elementAssignmentChange2 = new ControlRemappingDemo1.ElementAssignmentChange(entry);
				elementAssignmentChange2.changeType = ControlRemappingDemo1.ElementAssignmentChangeType.Replace;
				this.actionQueue.Enqueue(elementAssignmentChange2);
				return true;
			}
			this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties
			{
				title = "Reassign or Remove",
				message = "Do you want to reassign or remove this assignment?",
				rect = this.GetScreenCenteredRect(250f, 200f),
				windowDrawDelegate = new Action<string, string>(this.DrawReassignOrRemoveElementAssignmentWindow)
			}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
			return false;
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x0010748C File Offset: 0x0010568C
		private bool ProcessRemoveElementAssignment(ControlRemappingDemo1.ElementAssignmentChange entry)
		{
			if (entry.context.controllerMap == null)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				entry.context.controllerMap.DeleteElementMap(entry.context.actionElementMapToReplace.id);
				return true;
			}
			this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.DeleteAssignmentConfirmation, new ControlRemappingDemo1.WindowProperties
			{
				title = "Remove Assignment",
				message = "Are you sure you want to remove this assignment?",
				rect = this.GetScreenCenteredRect(250f, 200f),
				windowDrawDelegate = new Action<string, string>(this.DrawModalWindow)
			}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
			return false;
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x0010754C File Offset: 0x0010574C
		private bool ProcessAddOrReplaceElementAssignment(ControlRemappingDemo1.ElementAssignmentChange entry)
		{
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				this.inputMapper.Stop();
				return true;
			}
			if (entry.state != ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				string text;
				if (entry.context.controllerMap.controllerType == ControllerType.Keyboard)
				{
					if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
					{
						text = "Press any key to assign it to this action. You may also use the modifier keys Command, Control, Alt, and Shift. If you wish to assign a modifier key itself to this action, press and hold the key for 1 second.";
					}
					else
					{
						text = "Press any key to assign it to this action. You may also use the modifier keys Control, Alt, and Shift. If you wish to assign a modifier key itself to this action, press and hold the key for 1 second.";
					}
					if (Application.isEditor)
					{
						text += "\n\nNOTE: Some modifier key combinations will not work in the Unity Editor, but they will work in a game build.";
					}
				}
				else if (entry.context.controllerMap.controllerType == ControllerType.Mouse)
				{
					text = "Press any mouse button or axis to assign it to this action.\n\nTo assign mouse movement axes, move the mouse quickly in the direction you want mapped to the action. Slow movements will be ignored.";
				}
				else
				{
					text = "Press any button or axis to assign it to this action.";
				}
				this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties
				{
					title = "Assign",
					message = text,
					rect = this.GetScreenCenteredRect(250f, 200f),
					windowDrawDelegate = new Action<string, string>(this.DrawElementAssignmentWindow)
				}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
				return false;
			}
			if (Event.current.type != EventType.Layout)
			{
				return false;
			}
			if (this.conflictFoundEventData != null)
			{
				ControlRemappingDemo1.ElementAssignmentChange elementAssignmentChange = new ControlRemappingDemo1.ElementAssignmentChange(entry);
				elementAssignmentChange.changeType = ControlRemappingDemo1.ElementAssignmentChangeType.ConflictCheck;
				this.actionQueue.Enqueue(elementAssignmentChange);
			}
			return true;
		}

		// Token: 0x0600324A RID: 12874 RVA: 0x0010767C File Offset: 0x0010587C
		private bool ProcessElementAssignmentConflictCheck(ControlRemappingDemo1.ElementAssignmentChange entry)
		{
			if (entry.context.controllerMap == null)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				this.inputMapper.Stop();
				return true;
			}
			if (this.conflictFoundEventData == null)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				if (entry.response == ControlRemappingDemo1.UserResponse.Confirm)
				{
					this.conflictFoundEventData.responseCallback(InputMapper.ConflictResponse.Replace);
				}
				else
				{
					if (entry.response != ControlRemappingDemo1.UserResponse.Custom1)
					{
						throw new NotImplementedException();
					}
					this.conflictFoundEventData.responseCallback(InputMapper.ConflictResponse.Add);
				}
				return true;
			}
			if (this.conflictFoundEventData.isProtected)
			{
				string message = this.conflictFoundEventData.assignment.elementDisplayName + " is already in use and is protected from reassignment. You cannot remove the protected assignment, but you can still assign the action to this element. If you do so, the element will trigger multiple actions when activated.";
				this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties
				{
					title = "Assignment Conflict",
					message = message,
					rect = this.GetScreenCenteredRect(250f, 200f),
					windowDrawDelegate = new Action<string, string>(this.DrawElementAssignmentProtectedConflictWindow)
				}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
			}
			else
			{
				string message2 = this.conflictFoundEventData.assignment.elementDisplayName + " is already in use. You may replace the other conflicting assignments, add this assignment anyway which will leave multiple actions assigned to this element, or cancel this assignment.";
				this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties
				{
					title = "Assignment Conflict",
					message = message2,
					rect = this.GetScreenCenteredRect(250f, 200f),
					windowDrawDelegate = new Action<string, string>(this.DrawElementAssignmentNormalConflictWindow)
				}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
			}
			return false;
		}

		// Token: 0x0600324B RID: 12875 RVA: 0x00107818 File Offset: 0x00105A18
		private bool ProcessFallbackJoystickIdentification(ControlRemappingDemo1.FallbackJoystickIdentification entry)
		{
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				return true;
			}
			this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.JoystickConflict, new ControlRemappingDemo1.WindowProperties
			{
				title = "Joystick Identification Required",
				message = "A joystick has been attached or removed. You will need to identify each joystick by pressing a button on the controller listed below:",
				rect = this.GetScreenCenteredRect(250f, 200f),
				windowDrawDelegate = new Action<string, string>(this.DrawFallbackJoystickIdentificationWindow)
			}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback), 1f);
			return false;
		}

		// Token: 0x0600324C RID: 12876 RVA: 0x001078AC File Offset: 0x00105AAC
		private bool ProcessCalibration(ControlRemappingDemo1.Calibration entry)
		{
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				return true;
			}
			this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.JoystickConflict, new ControlRemappingDemo1.WindowProperties
			{
				title = "Calibrate Controller",
				message = "Select an axis to calibrate on the " + entry.joystick.name + ".",
				rect = this.GetScreenCenteredRect(450f, 480f),
				windowDrawDelegate = new Action<string, string>(this.DrawCalibrationWindow)
			}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
			return false;
		}

		// Token: 0x0600324D RID: 12877 RVA: 0x000221DE File Offset: 0x000203DE
		private void PlayerSelectionChanged()
		{
			this.ClearControllerSelection();
		}

		// Token: 0x0600324E RID: 12878 RVA: 0x000221E6 File Offset: 0x000203E6
		private void ControllerSelectionChanged()
		{
			this.ClearMapSelection();
		}

		// Token: 0x0600324F RID: 12879 RVA: 0x000221EE File Offset: 0x000203EE
		private void ClearControllerSelection()
		{
			this.selectedController.Clear();
			this.ClearMapSelection();
		}

		// Token: 0x06003250 RID: 12880 RVA: 0x00022201 File Offset: 0x00020401
		private void ClearMapSelection()
		{
			this.selectedMapCategoryId = -1;
			this.selectedMap = null;
		}

		// Token: 0x06003251 RID: 12881 RVA: 0x00022211 File Offset: 0x00020411
		private void ResetAll()
		{
			this.ClearWorkingVars();
			this.initialized = false;
			this.showMenu = false;
		}

		// Token: 0x06003252 RID: 12882 RVA: 0x00107950 File Offset: 0x00105B50
		private void ClearWorkingVars()
		{
			this.selectedPlayer = null;
			this.ClearMapSelection();
			this.selectedController.Clear();
			this.actionScrollPos = default(Vector2);
			this.dialog.FullReset();
			this.actionQueue.Clear();
			this.busy = false;
			this.startListening = false;
			this.conflictFoundEventData = null;
			this.inputMapper.Stop();
		}

		// Token: 0x06003253 RID: 12883 RVA: 0x001079B8 File Offset: 0x00105BB8
		private void SetGUIStateStart()
		{
			this.guiState = true;
			if (this.busy)
			{
				this.guiState = false;
			}
			this.pageGUIState = (this.guiState && !this.busy && !this.dialog.enabled && !this.dialog.busy);
			if (GUI.enabled != this.guiState)
			{
				GUI.enabled = this.guiState;
			}
		}

		// Token: 0x06003254 RID: 12884 RVA: 0x00022227 File Offset: 0x00020427
		private void SetGUIStateEnd()
		{
			this.guiState = true;
			if (!GUI.enabled)
			{
				GUI.enabled = this.guiState;
			}
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x00107A28 File Offset: 0x00105C28
		private void JoystickConnected(ControllerStatusChangedEventArgs args)
		{
			if (ReInput.controllers.IsControllerAssigned(args.controllerType, args.controllerId))
			{
				using (IEnumerator<Player> enumerator = ReInput.players.AllPlayers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Player player = enumerator.Current;
						if (player.controllers.ContainsController(args.controllerType, args.controllerId))
						{
							ReInput.userDataStore.LoadControllerData(player.id, args.controllerType, args.controllerId);
						}
					}
					goto IL_90;
				}
			}
			ReInput.userDataStore.LoadControllerData(args.controllerType, args.controllerId);
			IL_90:
			if (ReInput.unityJoystickIdentificationRequired)
			{
				this.IdentifyAllJoysticks();
			}
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x00107AE4 File Offset: 0x00105CE4
		private void JoystickPreDisconnect(ControllerStatusChangedEventArgs args)
		{
			if (this.selectedController.hasSelection && args.controllerType == this.selectedController.type && args.controllerId == this.selectedController.id)
			{
				this.ClearControllerSelection();
			}
			if (this.showMenu)
			{
				if (ReInput.controllers.IsControllerAssigned(args.controllerType, args.controllerId))
				{
					using (IEnumerator<Player> enumerator = ReInput.players.AllPlayers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Player player = enumerator.Current;
							if (player.controllers.ContainsController(args.controllerType, args.controllerId))
							{
								ReInput.userDataStore.SaveControllerData(player.id, args.controllerType, args.controllerId);
							}
						}
						return;
					}
				}
				ReInput.userDataStore.SaveControllerData(args.controllerType, args.controllerId);
			}
		}

		// Token: 0x06003257 RID: 12887 RVA: 0x00022242 File Offset: 0x00020442
		private void JoystickDisconnected(ControllerStatusChangedEventArgs args)
		{
			if (this.showMenu)
			{
				this.ClearWorkingVars();
			}
			if (ReInput.unityJoystickIdentificationRequired)
			{
				this.IdentifyAllJoysticks();
			}
		}

		// Token: 0x06003258 RID: 12888 RVA: 0x0002225F File Offset: 0x0002045F
		private void OnConflictFound(InputMapper.ConflictFoundEventData data)
		{
			this.conflictFoundEventData = data;
		}

		// Token: 0x06003259 RID: 12889 RVA: 0x00022268 File Offset: 0x00020468
		private void OnStopped(InputMapper.StoppedEventData data)
		{
			this.conflictFoundEventData = null;
		}

		// Token: 0x0600325A RID: 12890 RVA: 0x00107BD8 File Offset: 0x00105DD8
		public void IdentifyAllJoysticks()
		{
			if (ReInput.controllers.joystickCount == 0)
			{
				return;
			}
			this.ClearWorkingVars();
			this.Open();
			foreach (Joystick joystick in ReInput.controllers.Joysticks)
			{
				this.actionQueue.Enqueue(new ControlRemappingDemo1.FallbackJoystickIdentification(joystick.id, joystick.name));
			}
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x0000398C File Offset: 0x00001B8C
		protected void CheckRecompile()
		{
		}

		// Token: 0x0600325C RID: 12892 RVA: 0x0000398C File Offset: 0x00001B8C
		private void RecompileWindow(int windowId)
		{
		}

		// Token: 0x040030A5 RID: 12453
		private const float defaultModalWidth = 250f;

		// Token: 0x040030A6 RID: 12454
		private const float defaultModalHeight = 200f;

		// Token: 0x040030A7 RID: 12455
		private const float assignmentTimeout = 5f;

		// Token: 0x040030A8 RID: 12456
		private ControlRemappingDemo1.DialogHelper dialog;

		// Token: 0x040030A9 RID: 12457
		private InputMapper inputMapper = new InputMapper();

		// Token: 0x040030AA RID: 12458
		private InputMapper.ConflictFoundEventData conflictFoundEventData;

		// Token: 0x040030AB RID: 12459
		private bool guiState;

		// Token: 0x040030AC RID: 12460
		private bool busy;

		// Token: 0x040030AD RID: 12461
		private bool pageGUIState;

		// Token: 0x040030AE RID: 12462
		private Player selectedPlayer;

		// Token: 0x040030AF RID: 12463
		private int selectedMapCategoryId;

		// Token: 0x040030B0 RID: 12464
		private ControlRemappingDemo1.ControllerSelection selectedController;

		// Token: 0x040030B1 RID: 12465
		private ControllerMap selectedMap;

		// Token: 0x040030B2 RID: 12466
		private bool showMenu;

		// Token: 0x040030B3 RID: 12467
		private bool startListening;

		// Token: 0x040030B4 RID: 12468
		private Vector2 actionScrollPos;

		// Token: 0x040030B5 RID: 12469
		private Vector2 calibrateScrollPos;

		// Token: 0x040030B6 RID: 12470
		private Queue<ControlRemappingDemo1.QueueEntry> actionQueue;

		// Token: 0x040030B7 RID: 12471
		private bool setupFinished;

		// Token: 0x040030B8 RID: 12472
		[NonSerialized]
		private bool initialized;

		// Token: 0x040030B9 RID: 12473
		private bool isCompiling;

		// Token: 0x040030BA RID: 12474
		private GUIStyle style_wordWrap;

		// Token: 0x040030BB RID: 12475
		private GUIStyle style_centeredBox;

		// Token: 0x020006B8 RID: 1720
		private class ControllerSelection
		{
			// Token: 0x0600325E RID: 12894 RVA: 0x00022284 File Offset: 0x00020484
			public ControllerSelection()
			{
				this.Clear();
			}

			// Token: 0x170008E8 RID: 2280
			// (get) Token: 0x0600325F RID: 12895 RVA: 0x00022292 File Offset: 0x00020492
			// (set) Token: 0x06003260 RID: 12896 RVA: 0x0002229A File Offset: 0x0002049A
			public int id
			{
				get
				{
					return this._id;
				}
				set
				{
					this._idPrev = this._id;
					this._id = value;
				}
			}

			// Token: 0x170008E9 RID: 2281
			// (get) Token: 0x06003261 RID: 12897 RVA: 0x000222AF File Offset: 0x000204AF
			// (set) Token: 0x06003262 RID: 12898 RVA: 0x000222B7 File Offset: 0x000204B7
			public ControllerType type
			{
				get
				{
					return this._type;
				}
				set
				{
					this._typePrev = this._type;
					this._type = value;
				}
			}

			// Token: 0x170008EA RID: 2282
			// (get) Token: 0x06003263 RID: 12899 RVA: 0x000222CC File Offset: 0x000204CC
			public int idPrev
			{
				get
				{
					return this._idPrev;
				}
			}

			// Token: 0x170008EB RID: 2283
			// (get) Token: 0x06003264 RID: 12900 RVA: 0x000222D4 File Offset: 0x000204D4
			public ControllerType typePrev
			{
				get
				{
					return this._typePrev;
				}
			}

			// Token: 0x170008EC RID: 2284
			// (get) Token: 0x06003265 RID: 12901 RVA: 0x000222DC File Offset: 0x000204DC
			public bool hasSelection
			{
				get
				{
					return this._id >= 0;
				}
			}

			// Token: 0x06003266 RID: 12902 RVA: 0x000222EA File Offset: 0x000204EA
			public void Set(int id, ControllerType type)
			{
				this.id = id;
				this.type = type;
			}

			// Token: 0x06003267 RID: 12903 RVA: 0x000222FA File Offset: 0x000204FA
			public void Clear()
			{
				this._id = -1;
				this._idPrev = -1;
				this._type = ControllerType.Joystick;
				this._typePrev = ControllerType.Joystick;
			}

			// Token: 0x040030BC RID: 12476
			private int _id;

			// Token: 0x040030BD RID: 12477
			private int _idPrev;

			// Token: 0x040030BE RID: 12478
			private ControllerType _type;

			// Token: 0x040030BF RID: 12479
			private ControllerType _typePrev;
		}

		// Token: 0x020006B9 RID: 1721
		private class DialogHelper
		{
			// Token: 0x170008ED RID: 2285
			// (get) Token: 0x06003268 RID: 12904 RVA: 0x00022318 File Offset: 0x00020518
			private float busyTimer
			{
				get
				{
					if (!this._busyTimerRunning)
					{
						return 0f;
					}
					return this._busyTime - Time.realtimeSinceStartup;
				}
			}

			// Token: 0x170008EE RID: 2286
			// (get) Token: 0x06003269 RID: 12905 RVA: 0x00022334 File Offset: 0x00020534
			// (set) Token: 0x0600326A RID: 12906 RVA: 0x0002233C File Offset: 0x0002053C
			public bool enabled
			{
				get
				{
					return this._enabled;
				}
				set
				{
					if (!value)
					{
						this._enabled = value;
						this._type = ControlRemappingDemo1.DialogHelper.DialogType.None;
						this.StateChanged(0.1f);
						return;
					}
					if (this._type == ControlRemappingDemo1.DialogHelper.DialogType.None)
					{
						return;
					}
					this.StateChanged(0.25f);
				}
			}

			// Token: 0x170008EF RID: 2287
			// (get) Token: 0x0600326B RID: 12907 RVA: 0x0002236F File Offset: 0x0002056F
			// (set) Token: 0x0600326C RID: 12908 RVA: 0x00022381 File Offset: 0x00020581
			public ControlRemappingDemo1.DialogHelper.DialogType type
			{
				get
				{
					if (!this._enabled)
					{
						return ControlRemappingDemo1.DialogHelper.DialogType.None;
					}
					return this._type;
				}
				set
				{
					if (value == ControlRemappingDemo1.DialogHelper.DialogType.None)
					{
						this._enabled = false;
						this.StateChanged(0.1f);
					}
					else
					{
						this._enabled = true;
						this.StateChanged(0.25f);
					}
					this._type = value;
				}
			}

			// Token: 0x170008F0 RID: 2288
			// (get) Token: 0x0600326D RID: 12909 RVA: 0x000223B3 File Offset: 0x000205B3
			public bool busy
			{
				get
				{
					return this._busyTimerRunning;
				}
			}

			// Token: 0x0600326E RID: 12910 RVA: 0x000223BB File Offset: 0x000205BB
			public DialogHelper()
			{
				this.drawWindowDelegate = new Action<int>(this.DrawWindow);
				this.drawWindowFunction = new GUI.WindowFunction(this.drawWindowDelegate.Invoke);
			}

			// Token: 0x0600326F RID: 12911 RVA: 0x000223EC File Offset: 0x000205EC
			public void StartModal(int queueActionId, ControlRemappingDemo1.DialogHelper.DialogType type, ControlRemappingDemo1.WindowProperties windowProperties, Action<int, ControlRemappingDemo1.UserResponse> resultCallback)
			{
				this.StartModal(queueActionId, type, windowProperties, resultCallback, -1f);
			}

			// Token: 0x06003270 RID: 12912 RVA: 0x000223FE File Offset: 0x000205FE
			public void StartModal(int queueActionId, ControlRemappingDemo1.DialogHelper.DialogType type, ControlRemappingDemo1.WindowProperties windowProperties, Action<int, ControlRemappingDemo1.UserResponse> resultCallback, float openBusyDelay)
			{
				this.currentActionId = queueActionId;
				this.windowProperties = windowProperties;
				this.type = type;
				this.resultCallback = resultCallback;
				if (openBusyDelay >= 0f)
				{
					this.StateChanged(openBusyDelay);
				}
			}

			// Token: 0x06003271 RID: 12913 RVA: 0x0002242E File Offset: 0x0002062E
			public void Update()
			{
				this.Draw();
				this.UpdateTimers();
			}

			// Token: 0x06003272 RID: 12914 RVA: 0x00107C58 File Offset: 0x00105E58
			public void Draw()
			{
				if (!this._enabled)
				{
					return;
				}
				bool enabled = GUI.enabled;
				GUI.enabled = true;
				GUILayout.Window(this.windowProperties.windowId, this.windowProperties.rect, this.drawWindowFunction, this.windowProperties.title, Array.Empty<GUILayoutOption>());
				GUI.FocusWindow(this.windowProperties.windowId);
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}

			// Token: 0x06003273 RID: 12915 RVA: 0x0002243C File Offset: 0x0002063C
			public void DrawConfirmButton()
			{
				this.DrawConfirmButton("Confirm");
			}

			// Token: 0x06003274 RID: 12916 RVA: 0x00107CCC File Offset: 0x00105ECC
			public void DrawConfirmButton(string title)
			{
				bool enabled = GUI.enabled;
				if (this.busy)
				{
					GUI.enabled = false;
				}
				if (GUILayout.Button(title, Array.Empty<GUILayoutOption>()))
				{
					this.Confirm(ControlRemappingDemo1.UserResponse.Confirm);
				}
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}

			// Token: 0x06003275 RID: 12917 RVA: 0x00022449 File Offset: 0x00020649
			public void DrawConfirmButton(ControlRemappingDemo1.UserResponse response)
			{
				this.DrawConfirmButton(response, "Confirm");
			}

			// Token: 0x06003276 RID: 12918 RVA: 0x00107D10 File Offset: 0x00105F10
			public void DrawConfirmButton(ControlRemappingDemo1.UserResponse response, string title)
			{
				bool enabled = GUI.enabled;
				if (this.busy)
				{
					GUI.enabled = false;
				}
				if (GUILayout.Button(title, Array.Empty<GUILayoutOption>()))
				{
					this.Confirm(response);
				}
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}

			// Token: 0x06003277 RID: 12919 RVA: 0x00022457 File Offset: 0x00020657
			public void DrawCancelButton()
			{
				this.DrawCancelButton("Cancel");
			}

			// Token: 0x06003278 RID: 12920 RVA: 0x00107D54 File Offset: 0x00105F54
			public void DrawCancelButton(string title)
			{
				bool enabled = GUI.enabled;
				if (this.busy)
				{
					GUI.enabled = false;
				}
				if (GUILayout.Button(title, Array.Empty<GUILayoutOption>()))
				{
					this.Cancel();
				}
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}

			// Token: 0x06003279 RID: 12921 RVA: 0x00022464 File Offset: 0x00020664
			public void Confirm()
			{
				this.Confirm(ControlRemappingDemo1.UserResponse.Confirm);
			}

			// Token: 0x0600327A RID: 12922 RVA: 0x0002246D File Offset: 0x0002066D
			public void Confirm(ControlRemappingDemo1.UserResponse response)
			{
				this.resultCallback(this.currentActionId, response);
				this.Close();
			}

			// Token: 0x0600327B RID: 12923 RVA: 0x00022487 File Offset: 0x00020687
			public void Cancel()
			{
				this.resultCallback(this.currentActionId, ControlRemappingDemo1.UserResponse.Cancel);
				this.Close();
			}

			// Token: 0x0600327C RID: 12924 RVA: 0x000224A1 File Offset: 0x000206A1
			private void DrawWindow(int windowId)
			{
				this.windowProperties.windowDrawDelegate(this.windowProperties.title, this.windowProperties.message);
			}

			// Token: 0x0600327D RID: 12925 RVA: 0x000224C9 File Offset: 0x000206C9
			private void UpdateTimers()
			{
				if (this._busyTimerRunning && this.busyTimer <= 0f)
				{
					this._busyTimerRunning = false;
				}
			}

			// Token: 0x0600327E RID: 12926 RVA: 0x000224E7 File Offset: 0x000206E7
			private void StartBusyTimer(float time)
			{
				this._busyTime = time + Time.realtimeSinceStartup;
				this._busyTimerRunning = true;
			}

			// Token: 0x0600327F RID: 12927 RVA: 0x000224FD File Offset: 0x000206FD
			private void Close()
			{
				this.Reset();
				this.StateChanged(0.1f);
			}

			// Token: 0x06003280 RID: 12928 RVA: 0x00022510 File Offset: 0x00020710
			private void StateChanged(float delay)
			{
				this.StartBusyTimer(delay);
			}

			// Token: 0x06003281 RID: 12929 RVA: 0x00022519 File Offset: 0x00020719
			private void Reset()
			{
				this._enabled = false;
				this._type = ControlRemappingDemo1.DialogHelper.DialogType.None;
				this.currentActionId = -1;
				this.resultCallback = null;
			}

			// Token: 0x06003282 RID: 12930 RVA: 0x00022537 File Offset: 0x00020737
			private void ResetTimers()
			{
				this._busyTimerRunning = false;
			}

			// Token: 0x06003283 RID: 12931 RVA: 0x00022540 File Offset: 0x00020740
			public void FullReset()
			{
				this.Reset();
				this.ResetTimers();
			}

			// Token: 0x040030C0 RID: 12480
			private const float openBusyDelay = 0.25f;

			// Token: 0x040030C1 RID: 12481
			private const float closeBusyDelay = 0.1f;

			// Token: 0x040030C2 RID: 12482
			private ControlRemappingDemo1.DialogHelper.DialogType _type;

			// Token: 0x040030C3 RID: 12483
			private bool _enabled;

			// Token: 0x040030C4 RID: 12484
			private float _busyTime;

			// Token: 0x040030C5 RID: 12485
			private bool _busyTimerRunning;

			// Token: 0x040030C6 RID: 12486
			private Action<int> drawWindowDelegate;

			// Token: 0x040030C7 RID: 12487
			private GUI.WindowFunction drawWindowFunction;

			// Token: 0x040030C8 RID: 12488
			private ControlRemappingDemo1.WindowProperties windowProperties;

			// Token: 0x040030C9 RID: 12489
			private int currentActionId;

			// Token: 0x040030CA RID: 12490
			private Action<int, ControlRemappingDemo1.UserResponse> resultCallback;

			// Token: 0x020006BA RID: 1722
			public enum DialogType
			{
				// Token: 0x040030CC RID: 12492
				None,
				// Token: 0x040030CD RID: 12493
				JoystickConflict,
				// Token: 0x040030CE RID: 12494
				ElementConflict,
				// Token: 0x040030CF RID: 12495
				KeyConflict,
				// Token: 0x040030D0 RID: 12496
				DeleteAssignmentConfirmation = 10,
				// Token: 0x040030D1 RID: 12497
				AssignElement
			}
		}

		// Token: 0x020006BB RID: 1723
		private abstract class QueueEntry
		{
			// Token: 0x170008F1 RID: 2289
			// (get) Token: 0x06003284 RID: 12932 RVA: 0x0002254E File Offset: 0x0002074E
			// (set) Token: 0x06003285 RID: 12933 RVA: 0x00022556 File Offset: 0x00020756
			public int id { get; protected set; }

			// Token: 0x170008F2 RID: 2290
			// (get) Token: 0x06003286 RID: 12934 RVA: 0x0002255F File Offset: 0x0002075F
			// (set) Token: 0x06003287 RID: 12935 RVA: 0x00022567 File Offset: 0x00020767
			public ControlRemappingDemo1.QueueActionType queueActionType { get; protected set; }

			// Token: 0x170008F3 RID: 2291
			// (get) Token: 0x06003288 RID: 12936 RVA: 0x00022570 File Offset: 0x00020770
			// (set) Token: 0x06003289 RID: 12937 RVA: 0x00022578 File Offset: 0x00020778
			public ControlRemappingDemo1.QueueEntry.State state { get; protected set; }

			// Token: 0x170008F4 RID: 2292
			// (get) Token: 0x0600328A RID: 12938 RVA: 0x00022581 File Offset: 0x00020781
			// (set) Token: 0x0600328B RID: 12939 RVA: 0x00022589 File Offset: 0x00020789
			public ControlRemappingDemo1.UserResponse response { get; protected set; }

			// Token: 0x170008F5 RID: 2293
			// (get) Token: 0x0600328C RID: 12940 RVA: 0x00022592 File Offset: 0x00020792
			protected static int nextId
			{
				get
				{
					int result = ControlRemappingDemo1.QueueEntry.uidCounter;
					ControlRemappingDemo1.QueueEntry.uidCounter++;
					return result;
				}
			}

			// Token: 0x0600328D RID: 12941 RVA: 0x000225A5 File Offset: 0x000207A5
			public QueueEntry(ControlRemappingDemo1.QueueActionType queueActionType)
			{
				this.id = ControlRemappingDemo1.QueueEntry.nextId;
				this.queueActionType = queueActionType;
			}

			// Token: 0x0600328E RID: 12942 RVA: 0x000225BF File Offset: 0x000207BF
			public void Confirm(ControlRemappingDemo1.UserResponse response)
			{
				this.state = ControlRemappingDemo1.QueueEntry.State.Confirmed;
				this.response = response;
			}

			// Token: 0x0600328F RID: 12943 RVA: 0x000225CF File Offset: 0x000207CF
			public void Cancel()
			{
				this.state = ControlRemappingDemo1.QueueEntry.State.Canceled;
			}

			// Token: 0x040030D6 RID: 12502
			private static int uidCounter;

			// Token: 0x020006BC RID: 1724
			public enum State
			{
				// Token: 0x040030D8 RID: 12504
				Waiting,
				// Token: 0x040030D9 RID: 12505
				Confirmed,
				// Token: 0x040030DA RID: 12506
				Canceled
			}
		}

		// Token: 0x020006BD RID: 1725
		private class JoystickAssignmentChange : ControlRemappingDemo1.QueueEntry
		{
			// Token: 0x170008F6 RID: 2294
			// (get) Token: 0x06003290 RID: 12944 RVA: 0x000225D8 File Offset: 0x000207D8
			// (set) Token: 0x06003291 RID: 12945 RVA: 0x000225E0 File Offset: 0x000207E0
			public int playerId { get; private set; }

			// Token: 0x170008F7 RID: 2295
			// (get) Token: 0x06003292 RID: 12946 RVA: 0x000225E9 File Offset: 0x000207E9
			// (set) Token: 0x06003293 RID: 12947 RVA: 0x000225F1 File Offset: 0x000207F1
			public int joystickId { get; private set; }

			// Token: 0x170008F8 RID: 2296
			// (get) Token: 0x06003294 RID: 12948 RVA: 0x000225FA File Offset: 0x000207FA
			// (set) Token: 0x06003295 RID: 12949 RVA: 0x00022602 File Offset: 0x00020802
			public bool assign { get; private set; }

			// Token: 0x06003296 RID: 12950 RVA: 0x0002260B File Offset: 0x0002080B
			public JoystickAssignmentChange(int newPlayerId, int joystickId, bool assign) : base(ControlRemappingDemo1.QueueActionType.JoystickAssignment)
			{
				this.playerId = newPlayerId;
				this.joystickId = joystickId;
				this.assign = assign;
			}
		}

		// Token: 0x020006BE RID: 1726
		private class ElementAssignmentChange : ControlRemappingDemo1.QueueEntry
		{
			// Token: 0x170008F9 RID: 2297
			// (get) Token: 0x06003297 RID: 12951 RVA: 0x00022629 File Offset: 0x00020829
			// (set) Token: 0x06003298 RID: 12952 RVA: 0x00022631 File Offset: 0x00020831
			public ControlRemappingDemo1.ElementAssignmentChangeType changeType { get; set; }

			// Token: 0x170008FA RID: 2298
			// (get) Token: 0x06003299 RID: 12953 RVA: 0x0002263A File Offset: 0x0002083A
			// (set) Token: 0x0600329A RID: 12954 RVA: 0x00022642 File Offset: 0x00020842
			public InputMapper.Context context { get; private set; }

			// Token: 0x0600329B RID: 12955 RVA: 0x0002264B File Offset: 0x0002084B
			public ElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChangeType changeType, InputMapper.Context context) : base(ControlRemappingDemo1.QueueActionType.ElementAssignment)
			{
				this.changeType = changeType;
				this.context = context;
			}

			// Token: 0x0600329C RID: 12956 RVA: 0x00022662 File Offset: 0x00020862
			public ElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChange other) : this(other.changeType, other.context.Clone())
			{
			}
		}

		// Token: 0x020006BF RID: 1727
		private class FallbackJoystickIdentification : ControlRemappingDemo1.QueueEntry
		{
			// Token: 0x170008FB RID: 2299
			// (get) Token: 0x0600329D RID: 12957 RVA: 0x0002267B File Offset: 0x0002087B
			// (set) Token: 0x0600329E RID: 12958 RVA: 0x00022683 File Offset: 0x00020883
			public int joystickId { get; private set; }

			// Token: 0x170008FC RID: 2300
			// (get) Token: 0x0600329F RID: 12959 RVA: 0x0002268C File Offset: 0x0002088C
			// (set) Token: 0x060032A0 RID: 12960 RVA: 0x00022694 File Offset: 0x00020894
			public string joystickName { get; private set; }

			// Token: 0x060032A1 RID: 12961 RVA: 0x0002269D File Offset: 0x0002089D
			public FallbackJoystickIdentification(int joystickId, string joystickName) : base(ControlRemappingDemo1.QueueActionType.FallbackJoystickIdentification)
			{
				this.joystickId = joystickId;
				this.joystickName = joystickName;
			}
		}

		// Token: 0x020006C0 RID: 1728
		private class Calibration : ControlRemappingDemo1.QueueEntry
		{
			// Token: 0x170008FD RID: 2301
			// (get) Token: 0x060032A2 RID: 12962 RVA: 0x000226B4 File Offset: 0x000208B4
			// (set) Token: 0x060032A3 RID: 12963 RVA: 0x000226BC File Offset: 0x000208BC
			public Player player { get; private set; }

			// Token: 0x170008FE RID: 2302
			// (get) Token: 0x060032A4 RID: 12964 RVA: 0x000226C5 File Offset: 0x000208C5
			// (set) Token: 0x060032A5 RID: 12965 RVA: 0x000226CD File Offset: 0x000208CD
			public ControllerType controllerType { get; private set; }

			// Token: 0x170008FF RID: 2303
			// (get) Token: 0x060032A6 RID: 12966 RVA: 0x000226D6 File Offset: 0x000208D6
			// (set) Token: 0x060032A7 RID: 12967 RVA: 0x000226DE File Offset: 0x000208DE
			public Joystick joystick { get; private set; }

			// Token: 0x17000900 RID: 2304
			// (get) Token: 0x060032A8 RID: 12968 RVA: 0x000226E7 File Offset: 0x000208E7
			// (set) Token: 0x060032A9 RID: 12969 RVA: 0x000226EF File Offset: 0x000208EF
			public CalibrationMap calibrationMap { get; private set; }

			// Token: 0x060032AA RID: 12970 RVA: 0x000226F8 File Offset: 0x000208F8
			public Calibration(Player player, Joystick joystick, CalibrationMap calibrationMap) : base(ControlRemappingDemo1.QueueActionType.Calibrate)
			{
				this.player = player;
				this.joystick = joystick;
				this.calibrationMap = calibrationMap;
				this.selectedElementIdentifierId = -1;
			}

			// Token: 0x040030E6 RID: 12518
			public int selectedElementIdentifierId;

			// Token: 0x040030E7 RID: 12519
			public bool recording;
		}

		// Token: 0x020006C1 RID: 1729
		private struct WindowProperties
		{
			// Token: 0x040030E8 RID: 12520
			public int windowId;

			// Token: 0x040030E9 RID: 12521
			public Rect rect;

			// Token: 0x040030EA RID: 12522
			public Action<string, string> windowDrawDelegate;

			// Token: 0x040030EB RID: 12523
			public string title;

			// Token: 0x040030EC RID: 12524
			public string message;
		}

		// Token: 0x020006C2 RID: 1730
		private enum QueueActionType
		{
			// Token: 0x040030EE RID: 12526
			None,
			// Token: 0x040030EF RID: 12527
			JoystickAssignment,
			// Token: 0x040030F0 RID: 12528
			ElementAssignment,
			// Token: 0x040030F1 RID: 12529
			FallbackJoystickIdentification,
			// Token: 0x040030F2 RID: 12530
			Calibrate
		}

		// Token: 0x020006C3 RID: 1731
		private enum ElementAssignmentChangeType
		{
			// Token: 0x040030F4 RID: 12532
			Add,
			// Token: 0x040030F5 RID: 12533
			Replace,
			// Token: 0x040030F6 RID: 12534
			Remove,
			// Token: 0x040030F7 RID: 12535
			ReassignOrRemove,
			// Token: 0x040030F8 RID: 12536
			ConflictCheck
		}

		// Token: 0x020006C4 RID: 1732
		public enum UserResponse
		{
			// Token: 0x040030FA RID: 12538
			Confirm,
			// Token: 0x040030FB RID: 12539
			Cancel,
			// Token: 0x040030FC RID: 12540
			Custom1,
			// Token: 0x040030FD RID: 12541
			Custom2
		}
	}
}
