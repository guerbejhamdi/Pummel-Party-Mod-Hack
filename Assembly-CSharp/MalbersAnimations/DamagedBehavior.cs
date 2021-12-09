using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x0200070B RID: 1803
	public class DamagedBehavior : StateMachineBehaviour
	{
		// Token: 0x060034F6 RID: 13558 RVA: 0x00110820 File Offset: 0x0010EA20
		public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
		{
			Animal component = animator.GetComponent<Animal>();
			component.Damaged = false;
			animator.SetBool(Hash.Damaged, false);
			component.StartCoroutine(component.CDamageInterrupt());
			if (!this.DirectionalDamage)
			{
				return;
			}
			Vector3 hitDirection = component.HitDirection;
			Vector3 forward = animator.transform.forward;
			hitDirection.y = 0f;
			forward.y = 0f;
			float num = Vector3.Angle(forward, hitDirection);
			if (Vector3.Dot(component.T_Right, component.HitDirection) < 0f)
			{
				if (num > 0f && num <= 60f)
				{
					this.Side = 3;
				}
				else if (num > 60f && num <= 120f)
				{
					this.Side = 2;
				}
				else if (num > 120f && num <= 180f)
				{
					this.Side = 1;
				}
			}
			else if (num > 0f && num <= 60f)
			{
				this.Side = -3;
			}
			else if (num > 60f && num <= 120f)
			{
				this.Side = -2;
			}
			else if (num > 120f && num <= 180f)
			{
				this.Side = -1;
			}
			animator.SetInteger(Hash.IDInt, this.Side);
		}

		// Token: 0x040033BB RID: 13243
		private int Side;

		// Token: 0x040033BC RID: 13244
		public bool DirectionalDamage = true;
	}
}
