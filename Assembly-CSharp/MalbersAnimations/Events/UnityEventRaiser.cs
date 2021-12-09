using System;
using UnityEngine;
using UnityEngine.Events;

namespace MalbersAnimations.Events
{
	// Token: 0x02000779 RID: 1913
	public class UnityEventRaiser : MonoBehaviour
	{
		// Token: 0x060036C9 RID: 14025 RVA: 0x0002556F File Offset: 0x0002376F
		public void OnEnable()
		{
			this.OnEnableEvent.Invoke();
		}

		// Token: 0x04003600 RID: 13824
		public UnityEvent OnEnableEvent;
	}
}
