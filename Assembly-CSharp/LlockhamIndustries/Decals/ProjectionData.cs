using System;
using System.Collections.Generic;

namespace LlockhamIndustries.Decals
{
	// Token: 0x0200086C RID: 2156
	public class ProjectionData
	{
		// Token: 0x06003D38 RID: 15672 RVA: 0x00028ECA File Offset: 0x000270CA
		public void Update()
		{
			this.projection.Update();
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x00028ED7 File Offset: 0x000270D7
		public void Add(ProjectionRenderer Instance)
		{
			if (!this.instances.Contains(Instance))
			{
				this.instances.Add(Instance);
				Instance.Data = this;
				DynamicDecals.System.MarkRenderers();
			}
		}

		// Token: 0x06003D3A RID: 15674 RVA: 0x00028F04 File Offset: 0x00027104
		public void Remove(ProjectionRenderer Instance)
		{
			this.instances.Remove(Instance);
			if (Instance.Data == this)
			{
				Instance.Data = null;
			}
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x00028F23 File Offset: 0x00027123
		public void MoveToTop(ProjectionRenderer Instance)
		{
			this.instances.Remove(Instance);
			this.instances.Add(Instance);
			DynamicDecals.System.MarkRenderers();
		}

		// Token: 0x06003D3C RID: 15676 RVA: 0x00028F48 File Offset: 0x00027148
		public void MoveToBottom(ProjectionRenderer Instance)
		{
			this.instances.Remove(Instance);
			this.instances.Insert(0, Instance);
			DynamicDecals.System.MarkRenderers();
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x00028F6E File Offset: 0x0002716E
		public ProjectionData(Projection Projection)
		{
			this.projection = Projection;
			this.instances = new List<ProjectionRenderer>();
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x001312C8 File Offset: 0x0012F4C8
		public void AssertOrder(ref int Order)
		{
			if (this.projection.Instanced)
			{
				foreach (ProjectionRenderer projectionRenderer in this.instances)
				{
					projectionRenderer.Renderer.sortingOrder = Order;
				}
				Order++;
				return;
			}
			foreach (ProjectionRenderer projectionRenderer2 in this.instances)
			{
				projectionRenderer2.Renderer.sortingOrder = Order;
				Order++;
			}
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x00131380 File Offset: 0x0012F580
		public void EnableRenderers()
		{
			for (int i = 0; i < this.instances.Count; i++)
			{
				this.instances[i].InitializeRenderer(true);
			}
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x001313B8 File Offset: 0x0012F5B8
		public void DisableRenderers()
		{
			for (int i = 0; i < this.instances.Count; i++)
			{
				this.instances[i].TerminateRenderer();
			}
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x001313EC File Offset: 0x0012F5EC
		public void UpdateRenderers()
		{
			for (int i = 0; i < this.instances.Count; i++)
			{
				this.instances[i].UpdateProjection();
			}
		}

		// Token: 0x04003A00 RID: 14848
		public Projection projection;

		// Token: 0x04003A01 RID: 14849
		public List<ProjectionRenderer> instances;
	}
}
