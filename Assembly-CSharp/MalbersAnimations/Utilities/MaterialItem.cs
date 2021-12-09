using System;
using MalbersAnimations.Events;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x020007AA RID: 1962
	[Serializable]
	public class MaterialItem
	{
		// Token: 0x060037CB RID: 14283 RVA: 0x00025F63 File Offset: 0x00024163
		public MaterialItem()
		{
			this.Name = "NameHere";
			this.mesh = null;
			this.materials = new Material[0];
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x00025F94 File Offset: 0x00024194
		public MaterialItem(MeshRenderer MR)
		{
			this.Name = "NameHere";
			this.mesh = MR;
			this.materials = new Material[0];
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x00025FC5 File Offset: 0x000241C5
		public MaterialItem(string name, MeshRenderer MR, Material[] mats)
		{
			this.Name = name;
			this.mesh = MR;
			this.materials = mats;
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x00025FED File Offset: 0x000241ED
		public MaterialItem(string name, MeshRenderer MR)
		{
			this.Name = name;
			this.mesh = MR;
			this.materials = new Material[0];
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x0011A228 File Offset: 0x00118428
		public virtual void ChangeMaterial()
		{
			this.current++;
			if (this.current < 0)
			{
				this.current = 0;
			}
			this.current %= this.materials.Length;
			Material[] sharedMaterials = this.mesh.sharedMaterials;
			if (this.materials[this.current] != null)
			{
				sharedMaterials[this.indexM] = this.materials[this.current];
				this.mesh.sharedMaterials = sharedMaterials;
				this.ChangeLOD(this.current);
				this.OnMaterialChanged.Invoke(this.current);
				return;
			}
			Debug.LogWarning("The Material on the Slot: " + this.current.ToString() + " is empty");
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x0011A2E8 File Offset: 0x001184E8
		public virtual void Set_by_BinaryIndex(int binaryCurrent)
		{
			int index = 0;
			for (int i = 0; i < this.materials.Length; i++)
			{
				if (MalbersTools.IsBitActive(binaryCurrent, i))
				{
					index = i;
					break;
				}
			}
			this.ChangeMaterial(index);
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x0011A320 File Offset: 0x00118520
		internal void ChangeLOD(int index)
		{
			if (!this.HasLODs)
			{
				return;
			}
			foreach (Renderer renderer in this.LODs)
			{
				if (renderer == null)
				{
					break;
				}
				Material[] sharedMaterials = renderer.sharedMaterials;
				sharedMaterials[this.indexM] = this.materials[this.current];
				if (this.materials[this.current] != null)
				{
					renderer.sharedMaterials = sharedMaterials;
				}
			}
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x0011A390 File Offset: 0x00118590
		internal void ChangeLOD(Material mat)
		{
			if (!this.HasLODs)
			{
				return;
			}
			Material[] sharedMaterials = this.mesh.sharedMaterials;
			sharedMaterials[this.indexM] = mat;
			Renderer[] lods = this.LODs;
			for (int i = 0; i < lods.Length; i++)
			{
				lods[i].sharedMaterials = sharedMaterials;
			}
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x0002601A File Offset: 0x0002421A
		public virtual void NextMaterial()
		{
			this.ChangeMaterial();
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x0011A3DC File Offset: 0x001185DC
		public virtual void ChangeMaterial(int index)
		{
			if (index < 0)
			{
				index = 0;
			}
			index %= this.materials.Length;
			if (!(this.materials[index] != null))
			{
				Debug.LogWarning("The material on the Slot: " + index.ToString() + "  is empty");
				return;
			}
			Material[] sharedMaterials = this.mesh.sharedMaterials;
			if (sharedMaterials.Length - 1 < this.indexM)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"The Meshes on the ",
					this.Name,
					" Material Item, does not have ",
					(this.indexM + 1).ToString(),
					" Materials, please change the ID parameter to value lower than ",
					sharedMaterials.Length.ToString()
				}));
				return;
			}
			sharedMaterials[this.indexM] = this.materials[index];
			this.mesh.sharedMaterials = sharedMaterials;
			this.current = index;
			this.ChangeLOD(index);
			this.OnMaterialChanged.Invoke(this.current);
		}

		// Token: 0x060037D5 RID: 14293 RVA: 0x0011A4D4 File Offset: 0x001186D4
		public virtual void PreviousMaterial()
		{
			this.current--;
			if (this.current < 0)
			{
				this.current = this.materials.Length - 1;
			}
			if (this.materials[this.current] != null)
			{
				Material[] sharedMaterials = this.mesh.sharedMaterials;
				sharedMaterials[this.indexM] = this.materials[this.current];
				this.mesh.sharedMaterials = sharedMaterials;
				this.ChangeLOD(this.current);
				this.OnMaterialChanged.Invoke(this.current);
				return;
			}
			Debug.LogWarning("The Material on the Slot: " + this.current.ToString() + " is empty");
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x0011A588 File Offset: 0x00118788
		public virtual void ChangeMaterial(Material mat)
		{
			Material[] sharedMaterials = this.mesh.sharedMaterials;
			sharedMaterials[this.indexM] = mat;
			this.mesh.sharedMaterials = sharedMaterials;
			this.ChangeLOD(mat);
		}

		// Token: 0x060037D7 RID: 14295 RVA: 0x00026022 File Offset: 0x00024222
		public virtual void ChangeMaterial(bool Next = true)
		{
			if (Next)
			{
				this.NextMaterial();
				return;
			}
			this.PreviousMaterial();
		}

		// Token: 0x040036B4 RID: 14004
		[SerializeField]
		[HideInInspector]
		public string Name;

		// Token: 0x040036B5 RID: 14005
		public Renderer mesh;

		// Token: 0x040036B6 RID: 14006
		public Material[] materials;

		// Token: 0x040036B7 RID: 14007
		public bool Linked;

		// Token: 0x040036B8 RID: 14008
		[Range(0f, 100f)]
		public int Master;

		// Token: 0x040036B9 RID: 14009
		[HideInInspector]
		[SerializeField]
		public int current;

		// Token: 0x040036BA RID: 14010
		public bool HasLODs;

		// Token: 0x040036BB RID: 14011
		public Renderer[] LODs;

		// Token: 0x040036BC RID: 14012
		[Tooltip("Material ID")]
		public int indexM;

		// Token: 0x040036BD RID: 14013
		public IntEvent OnMaterialChanged = new IntEvent();
	}
}
