using System;
using Rewired;
using UnityEngine;

// Token: 0x02000418 RID: 1048
public class JoystickAssignment : MonoBehaviour
{
	// Token: 0x06001D20 RID: 7456 RVA: 0x0001577B File Offset: 0x0001397B
	private void Start()
	{
		ReInput.configuration.autoAssignJoysticks = true;
		ReInput.controllers.AutoAssignJoysticks();
	}
}
