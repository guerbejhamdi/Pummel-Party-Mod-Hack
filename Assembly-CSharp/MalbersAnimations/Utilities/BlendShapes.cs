using System;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x02000794 RID: 1940
	public class BlendShapes : MonoBehaviour
	{
		// Token: 0x0600373E RID: 14142 RVA: 0x000259AD File Offset: 0x00023BAD
		private void Awake()
		{
			if (this.random)
			{
				this.RandomShapes();
			}
		}

		// Token: 0x0600373F RID: 14143 RVA: 0x00118850 File Offset: 0x00116A50
		public virtual void RandomShapes()
		{
			foreach (MeshBlendShapes meshBlendShapes in this.Shapes)
			{
				meshBlendShapes.SetRandom();
			}
		}

		// Token: 0x06003740 RID: 14144 RVA: 0x001188A0 File Offset: 0x00116AA0
		public virtual void UpdateBlendShapes()
		{
			foreach (MeshBlendShapes meshBlendShapes in this.Shapes)
			{
				meshBlendShapes.UpdateBlendShapes();
			}
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x001188F0 File Offset: 0x00116AF0
		public virtual void SetBlendShape(string name, float value)
		{
			foreach (MeshBlendShapes meshBlendShapes in this.Shapes)
			{
				meshBlendShapes.SetBlendShape(name, value);
			}
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x00118944 File Offset: 0x00116B44
		public virtual void SetBlendShape(int index, float value)
		{
			foreach (MeshBlendShapes meshBlendShapes in this.Shapes)
			{
				meshBlendShapes.SetBlendShape(base.name, value);
			}
		}

		// Token: 0x04003651 RID: 13905
		[SerializeField]
		public List<MeshBlendShapes> Shapes;

		// Token: 0x04003652 RID: 13906
		public bool random;
	}
}
