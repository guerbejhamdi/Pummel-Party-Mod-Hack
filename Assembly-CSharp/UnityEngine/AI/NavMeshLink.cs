using System;
using System.Collections.Generic;

namespace UnityEngine.AI
{
	// Token: 0x020007B1 RID: 1969
	[ExecuteInEditMode]
	[DefaultExecutionOrder(-101)]
	[AddComponentMenu("Navigation/NavMeshLink", 33)]
	[HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
	public class NavMeshLink : MonoBehaviour
	{
		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x060037F0 RID: 14320 RVA: 0x000261AC File Offset: 0x000243AC
		// (set) Token: 0x060037F1 RID: 14321 RVA: 0x000261B4 File Offset: 0x000243B4
		public int agentTypeID
		{
			get
			{
				return this.m_AgentTypeID;
			}
			set
			{
				this.m_AgentTypeID = value;
				this.UpdateLink();
			}
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x060037F2 RID: 14322 RVA: 0x000261C3 File Offset: 0x000243C3
		// (set) Token: 0x060037F3 RID: 14323 RVA: 0x000261CB File Offset: 0x000243CB
		public Vector3 startPoint
		{
			get
			{
				return this.m_StartPoint;
			}
			set
			{
				this.m_StartPoint = value;
				this.UpdateLink();
			}
		}

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x060037F4 RID: 14324 RVA: 0x000261DA File Offset: 0x000243DA
		// (set) Token: 0x060037F5 RID: 14325 RVA: 0x000261E2 File Offset: 0x000243E2
		public Vector3 endPoint
		{
			get
			{
				return this.m_EndPoint;
			}
			set
			{
				this.m_EndPoint = value;
				this.UpdateLink();
			}
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x060037F6 RID: 14326 RVA: 0x000261F1 File Offset: 0x000243F1
		// (set) Token: 0x060037F7 RID: 14327 RVA: 0x000261F9 File Offset: 0x000243F9
		public float width
		{
			get
			{
				return this.m_Width;
			}
			set
			{
				this.m_Width = value;
				this.UpdateLink();
			}
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x060037F8 RID: 14328 RVA: 0x00026208 File Offset: 0x00024408
		// (set) Token: 0x060037F9 RID: 14329 RVA: 0x00026210 File Offset: 0x00024410
		public int costModifier
		{
			get
			{
				return this.m_CostModifier;
			}
			set
			{
				this.m_CostModifier = value;
				this.UpdateLink();
			}
		}

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x060037FA RID: 14330 RVA: 0x0002621F File Offset: 0x0002441F
		// (set) Token: 0x060037FB RID: 14331 RVA: 0x00026227 File Offset: 0x00024427
		public bool bidirectional
		{
			get
			{
				return this.m_Bidirectional;
			}
			set
			{
				this.m_Bidirectional = value;
				this.UpdateLink();
			}
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x060037FC RID: 14332 RVA: 0x00026236 File Offset: 0x00024436
		// (set) Token: 0x060037FD RID: 14333 RVA: 0x0002623E File Offset: 0x0002443E
		public bool autoUpdate
		{
			get
			{
				return this.m_AutoUpdatePosition;
			}
			set
			{
				this.SetAutoUpdate(value);
			}
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x060037FE RID: 14334 RVA: 0x00026247 File Offset: 0x00024447
		// (set) Token: 0x060037FF RID: 14335 RVA: 0x0002624F File Offset: 0x0002444F
		public int area
		{
			get
			{
				return this.m_Area;
			}
			set
			{
				this.m_Area = value;
				this.UpdateLink();
			}
		}

		// Token: 0x06003800 RID: 14336 RVA: 0x0002625E File Offset: 0x0002445E
		private void OnEnable()
		{
			this.AddLink();
			if (this.m_AutoUpdatePosition && this.m_LinkInstance.valid)
			{
				NavMeshLink.AddTracking(this);
			}
		}

		// Token: 0x06003801 RID: 14337 RVA: 0x00026281 File Offset: 0x00024481
		private void OnDisable()
		{
			NavMeshLink.RemoveTracking(this);
			this.m_LinkInstance.Remove();
		}

		// Token: 0x06003802 RID: 14338 RVA: 0x00026294 File Offset: 0x00024494
		public void UpdateLink()
		{
			this.m_LinkInstance.Remove();
			this.AddLink();
		}

		// Token: 0x06003803 RID: 14339 RVA: 0x000262A7 File Offset: 0x000244A7
		private static void AddTracking(NavMeshLink link)
		{
			if (NavMeshLink.s_Tracked.Count == 0)
			{
				NavMesh.onPreUpdate = (NavMesh.OnNavMeshPreUpdate)Delegate.Combine(NavMesh.onPreUpdate, new NavMesh.OnNavMeshPreUpdate(NavMeshLink.UpdateTrackedInstances));
			}
			NavMeshLink.s_Tracked.Add(link);
		}

		// Token: 0x06003804 RID: 14340 RVA: 0x000262E0 File Offset: 0x000244E0
		private static void RemoveTracking(NavMeshLink link)
		{
			NavMeshLink.s_Tracked.Remove(link);
			if (NavMeshLink.s_Tracked.Count == 0)
			{
				NavMesh.onPreUpdate = (NavMesh.OnNavMeshPreUpdate)Delegate.Remove(NavMesh.onPreUpdate, new NavMesh.OnNavMeshPreUpdate(NavMeshLink.UpdateTrackedInstances));
			}
		}

		// Token: 0x06003805 RID: 14341 RVA: 0x0002631A File Offset: 0x0002451A
		private void SetAutoUpdate(bool value)
		{
			if (this.m_AutoUpdatePosition == value)
			{
				return;
			}
			this.m_AutoUpdatePosition = value;
			if (value)
			{
				NavMeshLink.AddTracking(this);
				return;
			}
			NavMeshLink.RemoveTracking(this);
		}

		// Token: 0x06003806 RID: 14342 RVA: 0x0011A7D8 File Offset: 0x001189D8
		private void AddLink()
		{
			this.m_LinkInstance = NavMesh.AddLink(new NavMeshLinkData
			{
				startPosition = this.m_StartPoint,
				endPosition = this.m_EndPoint,
				width = this.m_Width,
				costModifier = (float)this.m_CostModifier,
				bidirectional = this.m_Bidirectional,
				area = this.m_Area,
				agentTypeID = this.m_AgentTypeID
			}, base.transform.position, base.transform.rotation);
			if (this.m_LinkInstance.valid)
			{
				this.m_LinkInstance.owner = this;
			}
			this.m_LastPosition = base.transform.position;
			this.m_LastRotation = base.transform.rotation;
		}

		// Token: 0x06003807 RID: 14343 RVA: 0x0002633D File Offset: 0x0002453D
		private bool HasTransformChanged()
		{
			return this.m_LastPosition != base.transform.position || this.m_LastRotation != base.transform.rotation;
		}

		// Token: 0x06003808 RID: 14344 RVA: 0x00026374 File Offset: 0x00024574
		private void OnDidApplyAnimationProperties()
		{
			this.UpdateLink();
		}

		// Token: 0x06003809 RID: 14345 RVA: 0x0011A8A8 File Offset: 0x00118AA8
		private static void UpdateTrackedInstances()
		{
			foreach (NavMeshLink navMeshLink in NavMeshLink.s_Tracked)
			{
				if (navMeshLink.HasTransformChanged())
				{
					navMeshLink.UpdateLink();
				}
			}
		}

		// Token: 0x040036D1 RID: 14033
		[SerializeField]
		private int m_AgentTypeID;

		// Token: 0x040036D2 RID: 14034
		[SerializeField]
		private Vector3 m_StartPoint = new Vector3(0f, 0f, -2.5f);

		// Token: 0x040036D3 RID: 14035
		[SerializeField]
		private Vector3 m_EndPoint = new Vector3(0f, 0f, 2.5f);

		// Token: 0x040036D4 RID: 14036
		[SerializeField]
		private float m_Width;

		// Token: 0x040036D5 RID: 14037
		[SerializeField]
		private int m_CostModifier = -1;

		// Token: 0x040036D6 RID: 14038
		[SerializeField]
		private bool m_Bidirectional = true;

		// Token: 0x040036D7 RID: 14039
		[SerializeField]
		private bool m_AutoUpdatePosition;

		// Token: 0x040036D8 RID: 14040
		[SerializeField]
		private int m_Area;

		// Token: 0x040036D9 RID: 14041
		private NavMeshLinkInstance m_LinkInstance;

		// Token: 0x040036DA RID: 14042
		private Vector3 m_LastPosition = Vector3.zero;

		// Token: 0x040036DB RID: 14043
		private Quaternion m_LastRotation = Quaternion.identity;

		// Token: 0x040036DC RID: 14044
		private static readonly List<NavMeshLink> s_Tracked = new List<NavMeshLink>();
	}
}
