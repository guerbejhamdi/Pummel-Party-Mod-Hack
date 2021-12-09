using System;
using Rewired;
using UnityEngine;

// Token: 0x02000532 RID: 1330
[Serializable]
public struct InputDetails
{
	// Token: 0x06002301 RID: 8961 RVA: 0x000D52D4 File Offset: 0x000D34D4
	public InputDetails(int keyboardActionID = -1, string description = "", bool detailed = false, Pole axisContribution = Pole.Positive, ControllerType controllerType = ControllerType.Keyboard, InputDetailsPriority priority = InputDetailsPriority.Normal)
	{
		this.keyboardActionID = keyboardActionID;
		this.joystickActionID = keyboardActionID;
		this.description = description;
		this.axisContribution = axisContribution;
		this.controllerType = controllerType;
		this.detailed = detailed;
		this.priority = priority;
		this.isFolded = true;
		this.keyboardAction = "";
		this.joystickAction = "";
	}

	// Token: 0x06002302 RID: 8962 RVA: 0x000D5334 File Offset: 0x000D3534
	public InputDetails(int keyboardActionID = -1, int joystickActionID = -1, string description = "", bool detailed = false, Pole axisContribution = Pole.Positive, ControllerType controllerType = ControllerType.Keyboard, InputDetailsPriority priority = InputDetailsPriority.Normal)
	{
		this.keyboardActionID = keyboardActionID;
		this.joystickActionID = joystickActionID;
		this.description = description;
		this.axisContribution = axisContribution;
		this.controllerType = controllerType;
		this.detailed = detailed;
		this.priority = priority;
		this.isFolded = true;
		this.keyboardAction = "";
		this.joystickAction = "";
	}

	// Token: 0x06002303 RID: 8963 RVA: 0x000D5394 File Offset: 0x000D3594
	public void GetMapping()
	{
		if (!string.IsNullOrEmpty(this.keyboardAction))
		{
			this.keyboardActionID = ReInput.mapping.GetActionId(this.keyboardAction);
		}
		if (!string.IsNullOrEmpty(this.joystickAction))
		{
			this.joystickActionID = ReInput.mapping.GetActionId(this.joystickAction);
		}
	}

	// Token: 0x040025EB RID: 9707
	[HideInInspector]
	public bool isFolded;

	// Token: 0x040025EC RID: 9708
	public int keyboardActionID;

	// Token: 0x040025ED RID: 9709
	public int joystickActionID;

	// Token: 0x040025EE RID: 9710
	public string keyboardAction;

	// Token: 0x040025EF RID: 9711
	public string joystickAction;

	// Token: 0x040025F0 RID: 9712
	public string description;

	// Token: 0x040025F1 RID: 9713
	public bool detailed;

	// Token: 0x040025F2 RID: 9714
	public Pole axisContribution;

	// Token: 0x040025F3 RID: 9715
	public ControllerType controllerType;

	// Token: 0x040025F4 RID: 9716
	public InputDetailsPriority priority;
}
