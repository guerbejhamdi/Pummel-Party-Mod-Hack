using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000734 RID: 1844
	public class DefaultInput : IInputSystem
	{
		// Token: 0x060035B0 RID: 13744 RVA: 0x00024565 File Offset: 0x00022765
		public float GetAxis(string Axis)
		{
			return Input.GetAxis(Axis);
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x0002456D File Offset: 0x0002276D
		public float GetAxisRaw(string Axis)
		{
			return Input.GetAxisRaw(Axis);
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x00024575 File Offset: 0x00022775
		public bool GetButton(string button)
		{
			return Input.GetButton(button);
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x0002457D File Offset: 0x0002277D
		public bool GetButtonDown(string button)
		{
			return Input.GetButtonDown(button);
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x00024585 File Offset: 0x00022785
		public bool GetButtonUp(string button)
		{
			return Input.GetButtonUp(button);
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x0002458D File Offset: 0x0002278D
		public static IInputSystem GetInputSystem(string PlayerID = "")
		{
			return new DefaultInput();
		}
	}
}
