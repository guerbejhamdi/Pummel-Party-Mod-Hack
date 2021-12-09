using System;
using System.Collections.Generic;
using LlockhamIndustries.ExtensionMethods;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000863 RID: 2147
	public class DynamicDecalSettings : ScriptableObject
	{
		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x06003CE5 RID: 15589 RVA: 0x00028A29 File Offset: 0x00026C29
		public bool UseMaskLayers
		{
			get
			{
				return this.maskMethod == DecalMaskMethod.Layer || this.maskMethod == DecalMaskMethod.Both;
			}
		}

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x06003CE6 RID: 15590 RVA: 0x00028A3E File Offset: 0x00026C3E
		// (set) Token: 0x06003CE7 RID: 15591 RVA: 0x00028A46 File Offset: 0x00026C46
		public ProjectionLayer[] Layers
		{
			get
			{
				return this.layers;
			}
			set
			{
				if (this.layers != value)
				{
					this.layers = value;
					this.CalculatePasses();
				}
			}
		}

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x06003CE8 RID: 15592 RVA: 0x00028A5E File Offset: 0x00026C5E
		public List<ReplacementPass> Passes
		{
			get
			{
				return this.passes;
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x06003CE9 RID: 15593 RVA: 0x00028A66 File Offset: 0x00026C66
		public List<Material> Materials
		{
			get
			{
				return this.materials;
			}
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06003CEA RID: 15594 RVA: 0x00028A6E File Offset: 0x00026C6E
		// (set) Token: 0x06003CEB RID: 15595 RVA: 0x00028A76 File Offset: 0x00026C76
		public ShaderReplacementType Replacement
		{
			get
			{
				return this.replacement;
			}
			set
			{
				if (this.replacement != value)
				{
					this.replacement = value;
					DynamicDecals.System.UpdateRenderers();
				}
			}
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06003CEC RID: 15596 RVA: 0x00028A92 File Offset: 0x00026C92
		public bool SinglePassVR
		{
			get
			{
				return this.singlepassVR;
			}
		}

		// Token: 0x06003CED RID: 15597 RVA: 0x00028A9A File Offset: 0x00026C9A
		public DynamicDecalSettings()
		{
			this.ResetPools();
			this.ResetMasking();
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x00028AAE File Offset: 0x00026CAE
		public void ResetSettings()
		{
			DynamicDecals.System.UpdateRenderers();
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x00028ABA File Offset: 0x00026CBA
		public void ResetPools()
		{
			this.pools = new PoolInstance[]
			{
				new PoolInstance("Default", null)
			};
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x00130384 File Offset: 0x0012E584
		public void ResetMasking()
		{
			this.maskMethod = DecalMaskMethod.Both;
			this.layers = new ProjectionLayer[]
			{
				new ProjectionLayer("Layer 1"),
				new ProjectionLayer("Layer 2"),
				new ProjectionLayer("Layer 3"),
				new ProjectionLayer("Layer 4")
			};
			this.CalculatePasses();
			if (this.materials == null)
			{
				this.materials = new List<Material>();
				this.materialQueues = new List<int>();
				return;
			}
			this.ClearMaterials();
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x00028AD6 File Offset: 0x00026CD6
		public void ResetGeneral()
		{
			this.replacement = ShaderReplacementType.Standard;
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x00130414 File Offset: 0x0012E614
		public void CalculatePasses()
		{
			if (this.passes == null)
			{
				this.passes = new List<ReplacementPass>();
			}
			else
			{
				this.passes.Clear();
			}
			for (int i = 0; i < 32; i++)
			{
				Vector4 layerVector = this.LayerVector(i);
				this.AddToPasses(i, layerVector);
			}
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x00130460 File Offset: 0x0012E660
		private Vector4 LayerVector(int LayerIndex)
		{
			Vector4 result = new Vector4(0f, 0f, 0f, 0f);
			if (this.layers[0].layers.Contains(LayerIndex))
			{
				result.x = 1f;
			}
			if (this.layers[1].layers.Contains(LayerIndex))
			{
				result.y = 1f;
			}
			if (this.layers[2].layers.Contains(LayerIndex))
			{
				result.z = 1f;
			}
			if (this.layers[3].layers.Contains(LayerIndex))
			{
				result.w = 1f;
			}
			return result;
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x00130520 File Offset: 0x0012E720
		private void AddToPasses(int LayerIndex, Vector4 LayerVector)
		{
			for (int i = 0; i < this.passes.Count; i++)
			{
				if (this.passes[i].vector == LayerVector)
				{
					this.passes[i].layers = this.passes[i].layers.Add(LayerIndex);
					return;
				}
			}
			this.passes.Add(new ReplacementPass(LayerIndex, LayerVector));
		}

		// Token: 0x06003CF5 RID: 15605 RVA: 0x00130598 File Offset: 0x0012E798
		public void AddMaterial(Material p_Material)
		{
			if (p_Material.renderQueue < 2999 && !this.materials.Contains(p_Material))
			{
				this.materials.Add(p_Material);
				this.materialQueues.Add(p_Material.renderQueue);
				p_Material.renderQueue = 2999;
			}
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x001305E8 File Offset: 0x0012E7E8
		public void RemoveMaterial(Material p_Material)
		{
			int num = -1;
			for (int i = 0; i < this.materials.Count; i++)
			{
				if (this.materials[i] == p_Material)
				{
					num = i;
					break;
				}
			}
			if (num != -1)
			{
				this.RemoveMaterial(num);
			}
		}

		// Token: 0x06003CF7 RID: 15607 RVA: 0x00028ADF File Offset: 0x00026CDF
		public void RemoveMaterial(int p_Index)
		{
			this.materials[p_Index].renderQueue = this.materialQueues[p_Index];
			this.materials.RemoveAt(p_Index);
			this.materialQueues.RemoveAt(p_Index);
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x00130630 File Offset: 0x0012E830
		public void ClearMaterials()
		{
			for (int i = this.materials.Count - 1; i >= 0; i--)
			{
				this.materials[i].renderQueue = this.materialQueues[i];
				this.materials.RemoveAt(i);
				this.materialQueues.RemoveAt(i);
			}
		}

		// Token: 0x06003CF9 RID: 15609 RVA: 0x0000398C File Offset: 0x00001B8C
		public void CalculateVR()
		{
		}

		// Token: 0x040039C2 RID: 14786
		public PoolInstance[] pools;

		// Token: 0x040039C3 RID: 14787
		public DecalMaskMethod maskMethod;

		// Token: 0x040039C4 RID: 14788
		[SerializeField]
		private ProjectionLayer[] layers;

		// Token: 0x040039C5 RID: 14789
		[SerializeField]
		private List<ReplacementPass> passes;

		// Token: 0x040039C6 RID: 14790
		[SerializeField]
		private ShaderReplacementType replacement;

		// Token: 0x040039C7 RID: 14791
		[SerializeField]
		private List<Material> materials;

		// Token: 0x040039C8 RID: 14792
		[SerializeField]
		private List<int> materialQueues;

		// Token: 0x040039C9 RID: 14793
		[SerializeField]
		private bool singlepassVR;
	}
}
