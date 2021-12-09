using System;
using System.Collections.Generic;
using UnityEngine;

namespace LlockhamIndustries.ExtensionMethods
{
	// Token: 0x02000862 RID: 2146
	public static class LayerMaskExtensionMethods
	{
		// Token: 0x06003CDC RID: 15580 RVA: 0x000289BA File Offset: 0x00026BBA
		public static bool Contains(this LayerMask Mask, int Layer)
		{
			return Mask == (Mask | 1 << Layer);
		}

		// Token: 0x06003CDD RID: 15581 RVA: 0x000289D1 File Offset: 0x00026BD1
		public static LayerMask Remove(this LayerMask Mask, int Layer)
		{
			return Mask & ~(1 << Layer);
		}

		// Token: 0x06003CDE RID: 15582 RVA: 0x000289E6 File Offset: 0x00026BE6
		public static LayerMask Remove(this LayerMask Mask, LayerMask Layers)
		{
			return Mask & ~Layers;
		}

		// Token: 0x06003CDF RID: 15583 RVA: 0x000289FB File Offset: 0x00026BFB
		public static LayerMask Add(this LayerMask Mask, int Layer)
		{
			Mask |= 1 << Layer;
			return Mask;
		}

		// Token: 0x06003CE0 RID: 15584 RVA: 0x00028A12 File Offset: 0x00026C12
		public static LayerMask Add(this LayerMask Mask, LayerMask Layers)
		{
			Mask |= Layers;
			return Mask;
		}

		// Token: 0x06003CE1 RID: 15585 RVA: 0x001302B4 File Offset: 0x0012E4B4
		public static int[] ContainedLayers(this LayerMask Mask)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < 32; i++)
			{
				if (Mask.Contains(i))
				{
					list.Add(i);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x001302EC File Offset: 0x0012E4EC
		public static string[] ContainedLayerNames(this LayerMask Mask)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < 32; i++)
			{
				if (Mask.Contains(i))
				{
					list.Add(LayerMask.LayerToName(i));
				}
			}
			return list.ToArray();
		}

		// Token: 0x06003CE3 RID: 15587 RVA: 0x00130328 File Offset: 0x0012E528
		public static void LogLayers(this LayerMask Mask)
		{
			int[] array = Mask.ContainedLayers();
			for (int i = 0; i < array.Length; i++)
			{
				Debug.Log(array[i]);
			}
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x00130358 File Offset: 0x0012E558
		public static void LogLayerNames(this LayerMask Mask)
		{
			string[] array = Mask.ContainedLayerNames();
			for (int i = 0; i < array.Length; i++)
			{
				Debug.Log(array[i]);
			}
		}
	}
}
