using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000707 RID: 1799
	public class MakeDamage : MonoBehaviour
	{
		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x060034E7 RID: 13543 RVA: 0x00023F03 File Offset: 0x00022103
		public Collider Collider
		{
			get
			{
				if (!this._collider)
				{
					this._collider = base.GetComponent<Collider>();
				}
				return this._collider;
			}
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x00023F24 File Offset: 0x00022124
		private void Start()
		{
			if (this.Collider)
			{
				this.Collider.isTrigger = true;
				return;
			}
			Debug.LogWarning(base.name + " needs a Collider so 'AttackTrigger' can function correctly");
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x00110364 File Offset: 0x0010E564
		private void OnTriggerEnter(Collider other)
		{
			if (other.transform.root == base.transform.root)
			{
				return;
			}
			DamageValues dv = new DamageValues(-other.bounds.center + this.Collider.bounds.center, this.damageMultiplier);
			if (other.isTrigger)
			{
				return;
			}
			IMDamagable componentInParent = other.GetComponentInParent<IMDamagable>();
			if (componentInParent != null)
			{
				componentInParent.getDamaged(dv);
			}
		}

		// Token: 0x040033A0 RID: 13216
		public float damageMultiplier = 1f;

		// Token: 0x040033A1 RID: 13217
		private Collider _collider;
	}
}
