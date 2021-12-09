using System;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x02000799 RID: 1945
	[ExecuteInEditMode]
	public class LinkedBlendShapes : MonoBehaviour
	{
		// Token: 0x0600374D RID: 14157 RVA: 0x00025A41 File Offset: 0x00023C41
		private void Start()
		{
			base.enabled = false;
		}

		// Token: 0x0600374E RID: 14158 RVA: 0x00025A4A File Offset: 0x00023C4A
		private void Update()
		{
			this.UpdateSlaveBlendShapes();
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x00118A14 File Offset: 0x00116C14
		public virtual void UpdateSlaveBlendShapes()
		{
			if (this.master && this.slave && this.slave.sharedMesh)
			{
				for (int i = 0; i < this.slave.sharedMesh.blendShapeCount; i++)
				{
					this.slave.SetBlendShapeWeight(i, this.master.GetBlendShapeWeight(i));
				}
			}
		}

		// Token: 0x0400365F RID: 13919
		public SkinnedMeshRenderer master;

		// Token: 0x04003660 RID: 13920
		public SkinnedMeshRenderer slave;
	}
}
