using System;
using System.Collections.Generic;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x0200086E RID: 2158
	public class ProjectionPool
	{
		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06003D58 RID: 15704 RVA: 0x00029045 File Offset: 0x00027245
		public string Title
		{
			get
			{
				return this.instance.title;
			}
		}

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06003D59 RID: 15705 RVA: 0x00029052 File Offset: 0x00027252
		private int Limit
		{
			get
			{
				return this.instance.limits[QualitySettings.GetQualityLevel()];
			}
		}

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06003D5A RID: 15706 RVA: 0x00029065 File Offset: 0x00027265
		public int ID
		{
			get
			{
				return this.instance.id;
			}
		}

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06003D5B RID: 15707 RVA: 0x001322AC File Offset: 0x001304AC
		internal Transform Parent
		{
			get
			{
				if (this.parent == null)
				{
					GameObject gameObject = new GameObject(this.instance.title + " Pool");
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
					this.parent = gameObject.transform;
				}
				return this.parent;
			}
		}

		// Token: 0x06003D5C RID: 15708 RVA: 0x00029072 File Offset: 0x00027272
		public ProjectionPool(PoolInstance Instance)
		{
			this.instance = Instance;
		}

		// Token: 0x06003D5D RID: 15709 RVA: 0x00029081 File Offset: 0x00027281
		public static ProjectionPool GetPool(string Title)
		{
			return DynamicDecals.System.GetPool(Title);
		}

		// Token: 0x06003D5E RID: 15710 RVA: 0x0002908E File Offset: 0x0002728E
		public static ProjectionPool GetPool(int ID)
		{
			return DynamicDecals.System.GetPool(ID);
		}

		// Token: 0x06003D5F RID: 15711 RVA: 0x001322FC File Offset: 0x001304FC
		public bool CheckIntersecting(Vector3 Point, float intersectionStrength)
		{
			if (this.activePool != null && this.activePool.Count > 0)
			{
				for (int i = this.activePool.Count - 1; i >= 0; i--)
				{
					if (this.activePool[i].Renderer != null)
					{
						if (this.activePool[i].Renderer.CheckIntersecting(Point) > intersectionStrength)
						{
							return true;
						}
					}
					else
					{
						this.activePool.RemoveAt(i);
					}
				}
			}
			return false;
		}

		// Token: 0x06003D60 RID: 15712 RVA: 0x0013237C File Offset: 0x0013057C
		public ProjectionRenderer Request(ProjectionRenderer Renderer = null, bool IncludeBehaviours = false)
		{
			ProjectionRenderer projectionRenderer = null;
			while (projectionRenderer == null)
			{
				projectionRenderer = this.RequestRenderer(Renderer, IncludeBehaviours);
			}
			return projectionRenderer;
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x001323A0 File Offset: 0x001305A0
		private ProjectionRenderer RequestRenderer(ProjectionRenderer Renderer = null, bool IncludeBehaviours = false)
		{
			if (this.activePool == null)
			{
				this.activePool = new List<PoolItem>();
			}
			if (this.inactivePool != null && this.inactivePool.Count > 0)
			{
				PoolItem poolItem = this.inactivePool[0];
				this.inactivePool.RemoveAt(0);
				this.activePool.Add(poolItem);
				poolItem.Initialize(Renderer, IncludeBehaviours);
				return poolItem.Renderer;
			}
			if (this.activePool.Count < this.Limit)
			{
				PoolItem poolItem2 = new PoolItem(this);
				poolItem2.Initialize(Renderer, IncludeBehaviours);
				this.activePool.Add(poolItem2);
				return poolItem2.Renderer;
			}
			PoolItem poolItem3 = this.activePool[0];
			poolItem3.Terminate();
			this.activePool.RemoveAt(0);
			this.activePool.Add(poolItem3);
			poolItem3.Initialize(Renderer, IncludeBehaviours);
			return poolItem3.Renderer;
		}

		// Token: 0x04003A0E RID: 14862
		private PoolInstance instance;

		// Token: 0x04003A0F RID: 14863
		private Transform parent;

		// Token: 0x04003A10 RID: 14864
		internal List<PoolItem> activePool;

		// Token: 0x04003A11 RID: 14865
		internal List<PoolItem> inactivePool;
	}
}
