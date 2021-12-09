using System;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x02000792 RID: 1938
	public class BlendShape : MonoBehaviour
	{
		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06003728 RID: 14120 RVA: 0x000258E0 File Offset: 0x00023AE0
		public bool HasBlendShapes
		{
			get
			{
				return this.mesh && this.mesh.sharedMesh.blendShapeCount > 0;
			}
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x00118408 File Offset: 0x00116608
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

		// Token: 0x0600372A RID: 14122 RVA: 0x00025904 File Offset: 0x00023B04
		private void Awake()
		{
			if (this.random)
			{
				this.RandomizeShapes();
			}
		}

		// Token: 0x0600372B RID: 14123 RVA: 0x00118454 File Offset: 0x00116654
		private void Reset()
		{
			this.mesh = base.GetComponentInChildren<SkinnedMeshRenderer>();
			if (this.mesh)
			{
				this.blendShapes = new float[this.mesh.sharedMesh.blendShapeCount];
				for (int i = 0; i < this.blendShapes.Length; i++)
				{
					this.blendShapes[i] = this.mesh.GetBlendShapeWeight(i);
				}
			}
		}

		// Token: 0x0600372C RID: 14124 RVA: 0x001184BC File Offset: 0x001166BC
		public virtual void SetShapesCount()
		{
			if (this.mesh)
			{
				this.blendShapes = new float[this.mesh.sharedMesh.blendShapeCount];
				for (int i = 0; i < this.blendShapes.Length; i++)
				{
					this.blendShapes[i] = this.mesh.GetBlendShapeWeight(i);
				}
			}
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x00118518 File Offset: 0x00116718
		public virtual void RandomizeShapes()
		{
			if (this.HasBlendShapes)
			{
				for (int i = 0; i < this.blendShapes.Length; i++)
				{
					this.blendShapes[i] = (float)UnityEngine.Random.Range(0, 100);
					this.mesh.SetBlendShapeWeight(i, this.blendShapes[i]);
				}
				this.UpdateLODs();
			}
		}

		// Token: 0x0600372E RID: 14126 RVA: 0x00025914 File Offset: 0x00023B14
		public virtual void SetBlendShape(string name, float value)
		{
			if (this.HasBlendShapes)
			{
				this.PinnedShape = this.mesh.sharedMesh.GetBlendShapeIndex(name);
				if (this.PinnedShape != -1)
				{
					this.mesh.SetBlendShapeWeight(this.PinnedShape, value);
				}
			}
		}

		// Token: 0x0600372F RID: 14127 RVA: 0x0011856C File Offset: 0x0011676C
		public virtual void SetBlendShape(int index, float value)
		{
			if (this.HasBlendShapes)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = this.mesh;
				this.PinnedShape = index;
				skinnedMeshRenderer.SetBlendShapeWeight(index, value);
			}
		}

		// Token: 0x06003730 RID: 14128 RVA: 0x00025950 File Offset: 0x00023B50
		public virtual void _PinShape(string name)
		{
			this.PinnedShape = this.mesh.sharedMesh.GetBlendShapeIndex(name);
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x00025969 File Offset: 0x00023B69
		public virtual void _PinShape(int index)
		{
			this.PinnedShape = index;
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x00118598 File Offset: 0x00116798
		public virtual void _PinnedShapeSetValue(float value)
		{
			if (this.PinnedShape != -1)
			{
				value = Mathf.Clamp(value, 0f, 100f);
				this.blendShapes[this.PinnedShape] = value;
				this.mesh.SetBlendShapeWeight(this.PinnedShape, value);
				this.UpdateLODs(this.PinnedShape);
			}
		}

		// Token: 0x06003733 RID: 14131 RVA: 0x001185EC File Offset: 0x001167EC
		public virtual void UpdateBlendShapes()
		{
			if (this.mesh && this.blendShapes != null)
			{
				if (this.blendShapes.Length != this.mesh.sharedMesh.blendShapeCount)
				{
					this.blendShapes = new float[this.mesh.sharedMesh.blendShapeCount];
				}
				for (int i = 0; i < this.blendShapes.Length; i++)
				{
					this.mesh.SetBlendShapeWeight(i, this.blendShapes[i]);
				}
				this.UpdateLODs();
			}
		}

		// Token: 0x06003734 RID: 14132 RVA: 0x00118670 File Offset: 0x00116870
		protected virtual void UpdateLODs()
		{
			for (int i = 0; i < this.blendShapes.Length; i++)
			{
				this.UpdateLODs(i);
			}
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x00118698 File Offset: 0x00116898
		protected virtual void UpdateLODs(int index)
		{
			if (this.LODs != null)
			{
				SkinnedMeshRenderer[] lods = this.LODs;
				for (int i = 0; i < lods.Length; i++)
				{
					lods[i].SetBlendShapeWeight(index, this.blendShapes[index]);
				}
			}
		}

		// Token: 0x04003649 RID: 13897
		public SkinnedMeshRenderer mesh;

		// Token: 0x0400364A RID: 13898
		public SkinnedMeshRenderer[] LODs;

		// Token: 0x0400364B RID: 13899
		[Range(0f, 100f)]
		public float[] blendShapes;

		// Token: 0x0400364C RID: 13900
		public bool random;

		// Token: 0x0400364D RID: 13901
		public int PinnedShape;
	}
}
