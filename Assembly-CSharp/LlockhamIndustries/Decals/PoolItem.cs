using System;
using System.Collections.Generic;
using LlockhamIndustries.ExtensionMethods;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x0200086F RID: 2159
	public class PoolItem
	{
		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x06003D62 RID: 15714 RVA: 0x0002909B File Offset: 0x0002729B
		public ProjectionPool Pool
		{
			get
			{
				return this.pool;
			}
		}

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06003D63 RID: 15715 RVA: 0x000290A3 File Offset: 0x000272A3
		public ProjectionRenderer Renderer
		{
			get
			{
				return this.renderer;
			}
		}

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06003D64 RID: 15716 RVA: 0x00132478 File Offset: 0x00130678
		private bool Valid
		{
			get
			{
				if (this.renderer == null)
				{
					if (this.pool.activePool != null)
					{
						this.pool.activePool.Remove(this);
					}
					if (this.pool.inactivePool != null)
					{
						this.pool.inactivePool.Remove(this);
					}
					return false;
				}
				return true;
			}
		}

		// Token: 0x06003D65 RID: 15717 RVA: 0x001324D4 File Offset: 0x001306D4
		public PoolItem(ProjectionPool Pool)
		{
			this.pool = Pool;
			GameObject gameObject = new GameObject("Projection");
			gameObject.transform.SetParent(this.pool.Parent);
			gameObject.SetActive(false);
			this.renderer = gameObject.AddComponent<ProjectionRenderer>();
			this.renderer.PoolItem = this;
		}

		// Token: 0x06003D66 RID: 15718 RVA: 0x00132530 File Offset: 0x00130730
		internal void Initialize(ProjectionRenderer Renderer = null, bool IncludeBehaviours = false)
		{
			if (this.Valid)
			{
				this.renderer.transform.SetParent(this.pool.Parent);
				if (Renderer != null)
				{
					this.renderer.Projection = Renderer.Projection;
					this.renderer.Tiling = Renderer.Tiling;
					this.renderer.Offset = Renderer.Offset;
					this.renderer.MaskMethod = Renderer.MaskMethod;
					this.renderer.MaskLayer1 = Renderer.MaskLayer1;
					this.renderer.MaskLayer2 = Renderer.MaskLayer2;
					this.renderer.MaskLayer3 = Renderer.MaskLayer3;
					this.renderer.MaskLayer4 = Renderer.MaskLayer4;
					this.renderer.Properties = Renderer.Properties;
					if (IncludeBehaviours)
					{
						foreach (MonoBehaviour monoBehaviour in Renderer.GetComponents<MonoBehaviour>())
						{
							if (!(monoBehaviour.GetType() == typeof(Transform)) && !(monoBehaviour.GetType() == typeof(ProjectionRenderer)))
							{
								this.renderer.gameObject.AddComponent(monoBehaviour).enabled = monoBehaviour.enabled;
							}
						}
					}
					this.renderer.transform.localScale = Renderer.transform.localScale;
					this.renderer.gameObject.layer = Renderer.gameObject.layer;
					this.renderer.gameObject.tag = Renderer.gameObject.tag;
				}
				else
				{
					this.renderer.transform.localScale = Vector3.one;
				}
				this.renderer.gameObject.SetActive(true);
			}
		}

		// Token: 0x06003D67 RID: 15719 RVA: 0x001326E8 File Offset: 0x001308E8
		internal void Terminate()
		{
			if (this.Valid)
			{
				this.renderer.gameObject.SetActive(false);
				foreach (Component component in this.renderer.gameObject.GetComponents<Component>())
				{
					if (!(component.GetType() == typeof(Transform)) && !(component.GetType() == typeof(ProjectionRenderer)))
					{
						UnityEngine.Object.Destroy(component);
					}
				}
				this.renderer.transform.SetParent(this.pool.Parent);
			}
		}

		// Token: 0x06003D68 RID: 15720 RVA: 0x00132784 File Offset: 0x00130984
		public void Return()
		{
			this.pool.activePool.Remove(this);
			this.Terminate();
			if (this.pool.inactivePool == null)
			{
				this.pool.inactivePool = new List<PoolItem>();
			}
			this.pool.inactivePool.Add(this);
		}

		// Token: 0x04003A12 RID: 14866
		private ProjectionPool pool;

		// Token: 0x04003A13 RID: 14867
		private ProjectionRenderer renderer;
	}
}
