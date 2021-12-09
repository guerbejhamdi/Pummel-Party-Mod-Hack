using System;
using UnityEngine;
using UnityEngine.Events;

namespace MalbersAnimations.Utilities
{
	// Token: 0x02000788 RID: 1928
	[Serializable]
	public class Effect
	{
		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x060036FD RID: 14077 RVA: 0x000256C9 File Offset: 0x000238C9
		// (set) Token: 0x060036FE RID: 14078 RVA: 0x000256D1 File Offset: 0x000238D1
		public Transform Owner
		{
			get
			{
				return this.owner;
			}
			set
			{
				this.owner = value;
			}
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x060036FF RID: 14079 RVA: 0x000256DA File Offset: 0x000238DA
		// (set) Token: 0x06003700 RID: 14080 RVA: 0x000256E2 File Offset: 0x000238E2
		public GameObject Instance
		{
			get
			{
				return this.instance;
			}
			set
			{
				this.instance = value;
			}
		}

		// Token: 0x04003621 RID: 13857
		public string Name = "EffectName";

		// Token: 0x04003622 RID: 13858
		public int ID;

		// Token: 0x04003623 RID: 13859
		public bool active = true;

		// Token: 0x04003624 RID: 13860
		public Transform root;

		// Token: 0x04003625 RID: 13861
		public bool isChild;

		// Token: 0x04003626 RID: 13862
		public bool useRootRotation = true;

		// Token: 0x04003627 RID: 13863
		public GameObject effect;

		// Token: 0x04003628 RID: 13864
		public Vector3 RotationOffset;

		// Token: 0x04003629 RID: 13865
		public Vector3 PositionOffset;

		// Token: 0x0400362A RID: 13866
		public Vector3 ScaleMultiplier = Vector3.one;

		// Token: 0x0400362B RID: 13867
		public float life = 10f;

		// Token: 0x0400362C RID: 13868
		public float delay;

		// Token: 0x0400362D RID: 13869
		public bool instantiate = true;

		// Token: 0x0400362E RID: 13870
		public bool toggleable;

		// Token: 0x0400362F RID: 13871
		public bool On;

		// Token: 0x04003630 RID: 13872
		public EffectModifier Modifier;

		// Token: 0x04003631 RID: 13873
		public UnityEvent OnPlay;

		// Token: 0x04003632 RID: 13874
		public UnityEvent OnStop;

		// Token: 0x04003633 RID: 13875
		protected Transform owner;

		// Token: 0x04003634 RID: 13876
		protected GameObject instance;
	}
}
