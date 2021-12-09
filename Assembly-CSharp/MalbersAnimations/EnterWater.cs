using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000744 RID: 1860
	public class EnterWater : MonoBehaviour
	{
		// Token: 0x0600361B RID: 13851 RVA: 0x00024B36 File Offset: 0x00022D36
		private void OnTriggerEnter(Collider other)
		{
			other.transform.root.SendMessage("EnterWater", true, SendMessageOptions.DontRequireReceiver);
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x00024B54 File Offset: 0x00022D54
		private void OnTriggerExit(Collider other)
		{
			other.transform.root.SendMessage("EnterWater", false, SendMessageOptions.DontRequireReceiver);
		}
	}
}
