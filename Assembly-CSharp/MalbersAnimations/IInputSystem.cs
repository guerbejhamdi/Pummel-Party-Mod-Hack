using System;

namespace MalbersAnimations
{
	// Token: 0x02000731 RID: 1841
	public interface IInputSystem
	{
		// Token: 0x060035A6 RID: 13734
		float GetAxis(string Axis);

		// Token: 0x060035A7 RID: 13735
		float GetAxisRaw(string Axis);

		// Token: 0x060035A8 RID: 13736
		bool GetButtonDown(string button);

		// Token: 0x060035A9 RID: 13737
		bool GetButtonUp(string button);

		// Token: 0x060035AA RID: 13738
		bool GetButton(string button);
	}
}
