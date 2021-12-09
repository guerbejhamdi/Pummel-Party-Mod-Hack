using System;
using MalbersAnimations.Events;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x020007B0 RID: 1968
	public class TriggerProxy : MonoBehaviour
	{
		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x060037E9 RID: 14313 RVA: 0x000260D0 File Offset: 0x000242D0
		// (set) Token: 0x060037EA RID: 14314 RVA: 0x000260D8 File Offset: 0x000242D8
		public bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				this.active = value;
			}
		}

		// Token: 0x060037EB RID: 14315 RVA: 0x000260E1 File Offset: 0x000242E1
		private void OnTriggerStay(Collider other)
		{
			if (!this.active)
			{
				return;
			}
			if (MalbersTools.Layer_in_LayerMask(other.gameObject.layer, this.Ignore))
			{
				return;
			}
			this.OnTrigger_Stay.Invoke(other);
		}

		// Token: 0x060037EC RID: 14316 RVA: 0x00026111 File Offset: 0x00024311
		private void OnTriggerEnter(Collider other)
		{
			if (!this.active)
			{
				return;
			}
			if (MalbersTools.Layer_in_LayerMask(other.gameObject.layer, this.Ignore))
			{
				return;
			}
			this.OnTrigger_Enter.Invoke(other);
		}

		// Token: 0x060037ED RID: 14317 RVA: 0x00026141 File Offset: 0x00024341
		private void OnTriggerExit(Collider other)
		{
			if (!this.active)
			{
				return;
			}
			if (MalbersTools.Layer_in_LayerMask(other.gameObject.layer, this.Ignore))
			{
				return;
			}
			this.OnTrigger_Exit.Invoke(other);
		}

		// Token: 0x060037EE RID: 14318 RVA: 0x0011A7A0 File Offset: 0x001189A0
		private void Reset()
		{
			Collider component = base.GetComponent<Collider>();
			this.Active = true;
			if (component)
			{
				component.isTrigger = true;
				return;
			}
			Debug.LogError("This Script requires a Collider, please add any type of collider");
		}

		// Token: 0x040036CB RID: 14027
		[Tooltip("Ignore this Objects with this layers")]
		public LayerMask Ignore;

		// Token: 0x040036CC RID: 14028
		[SerializeField]
		private bool active = true;

		// Token: 0x040036CD RID: 14029
		public ColliderEvent OnTrigger_Enter = new ColliderEvent();

		// Token: 0x040036CE RID: 14030
		public ColliderEvent OnTrigger_Stay = new ColliderEvent();

		// Token: 0x040036CF RID: 14031
		public ColliderEvent OnTrigger_Exit = new ColliderEvent();

		// Token: 0x040036D0 RID: 14032
		public CollisionEvent OnCollision_Enter = new CollisionEvent();
	}
}
