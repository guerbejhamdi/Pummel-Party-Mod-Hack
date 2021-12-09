using System;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x0200079C RID: 1948
	public class LookAtTrigger : MonoBehaviour
	{
		// Token: 0x06003761 RID: 14177 RVA: 0x00118E30 File Offset: 0x00117030
		private void OnTriggerEnter(Collider other)
		{
			if (other.isTrigger)
			{
				return;
			}
			LookAt componentInParent = other.GetComponentInParent<LookAt>();
			if (!componentInParent)
			{
				return;
			}
			componentInParent.Active = true;
			componentInParent.Target = base.transform;
		}

		// Token: 0x06003762 RID: 14178 RVA: 0x00118E6C File Offset: 0x0011706C
		private void OnTriggerExit(Collider other)
		{
			if (other.isTrigger)
			{
				return;
			}
			LookAt componentInParent = other.GetComponentInParent<LookAt>();
			if (!componentInParent)
			{
				return;
			}
			componentInParent.Target = null;
		}
	}
}
