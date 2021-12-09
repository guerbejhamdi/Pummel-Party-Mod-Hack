using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020007F2 RID: 2034
	[Serializable]
	public class EventCallback
	{
		// Token: 0x060039C0 RID: 14784 RVA: 0x00027315 File Offset: 0x00025515
		public void Execute(UnityEngine.Object Sender = null)
		{
			if (this.HasCallback() && Application.isPlaying)
			{
				this.Target.gameObject.SendMessage(this.MethodName, Sender, SendMessageOptions.DontRequireReceiver);
			}
		}

		// Token: 0x060039C1 RID: 14785 RVA: 0x0002733E File Offset: 0x0002553E
		public bool HasCallback()
		{
			return this.Target != null && !string.IsNullOrEmpty(this.MethodName);
		}

		// Token: 0x04003816 RID: 14358
		public MonoBehaviour Target;

		// Token: 0x04003817 RID: 14359
		public string MethodName = string.Empty;
	}
}
