using System;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x02000793 RID: 1939
	[Serializable]
	public class MeshBlendShapes
	{
		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x06003737 RID: 14135 RVA: 0x00025972 File Offset: 0x00023B72
		public bool HasBlendShapes
		{
			get
			{
				return this.mesh && this.mesh.sharedMesh.blendShapeCount > 0;
			}
		}

		// Token: 0x06003738 RID: 14136 RVA: 0x001186D4 File Offset: 0x001168D4
		public virtual void UpdateBlendShapes()
		{
			if (this.mesh != null && this.blendShapes != null)
			{
				if (this.NameID == string.Empty)
				{
					this.NameID = this.mesh.name;
				}
				if (this.blendShapes.Length != this.mesh.sharedMesh.blendShapeCount)
				{
					this.blendShapes = new float[this.mesh.sharedMesh.blendShapeCount];
				}
				for (int i = 0; i < this.blendShapes.Length; i++)
				{
					this.mesh.SetBlendShapeWeight(i, this.blendShapes[i]);
				}
			}
		}

		// Token: 0x06003739 RID: 14137 RVA: 0x0011877C File Offset: 0x0011697C
		public virtual float[] GetBlendShapeValues()
		{
			if (this.HasBlendShapes)
			{
				float[] array = new float[this.mesh.sharedMesh.blendShapeCount];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.mesh.GetBlendShapeWeight(i);
				}
				return array;
			}
			return null;
		}

		// Token: 0x0600373A RID: 14138 RVA: 0x001187C8 File Offset: 0x001169C8
		public void SetRandom()
		{
			if (this.HasBlendShapes)
			{
				for (int i = 0; i < this.blendShapes.Length; i++)
				{
					this.blendShapes[i] = (float)UnityEngine.Random.Range(0, 100);
					this.mesh.SetBlendShapeWeight(i, this.blendShapes[i]);
				}
			}
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x00118818 File Offset: 0x00116A18
		public void SetBlendShape(string name, float value)
		{
			if (this.HasBlendShapes)
			{
				int blendShapeIndex = this.mesh.sharedMesh.GetBlendShapeIndex(name);
				if (blendShapeIndex != -1)
				{
					this.mesh.SetBlendShapeWeight(blendShapeIndex, value);
				}
			}
		}

		// Token: 0x0600373C RID: 14140 RVA: 0x00025996 File Offset: 0x00023B96
		public void SetBlendShape(int index, float value)
		{
			if (this.HasBlendShapes)
			{
				this.mesh.SetBlendShapeWeight(index, value);
			}
		}

		// Token: 0x0400364E RID: 13902
		public string NameID;

		// Token: 0x0400364F RID: 13903
		public SkinnedMeshRenderer mesh;

		// Token: 0x04003650 RID: 13904
		[Range(0f, 100f)]
		public float[] blendShapes;
	}
}
