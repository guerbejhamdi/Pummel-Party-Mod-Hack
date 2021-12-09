using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000701 RID: 1793
	public class FollowTarget : MonoBehaviour
	{
		// Token: 0x060034DE RID: 13534 RVA: 0x00023EAF File Offset: 0x000220AF
		private void Start()
		{
			this.animal = base.GetComponentInParent<Animal>();
		}

		// Token: 0x060034DF RID: 13535 RVA: 0x0010FEF0 File Offset: 0x0010E0F0
		private void Update()
		{
			Vector3 vector = this.target.position - base.transform.position;
			float num = Vector3.Distance(base.transform.position, this.target.position);
			this.animal.Move((num > this.stopDistance) ? vector : Vector3.zero, true);
		}

		// Token: 0x060034E0 RID: 13536 RVA: 0x00023EBD File Offset: 0x000220BD
		private void OnDisable()
		{
			this.animal.Move(Vector3.zero, true);
		}

		// Token: 0x04003347 RID: 13127
		public Transform target;

		// Token: 0x04003348 RID: 13128
		public float stopDistance = 3f;

		// Token: 0x04003349 RID: 13129
		private Animal animal;
	}
}
