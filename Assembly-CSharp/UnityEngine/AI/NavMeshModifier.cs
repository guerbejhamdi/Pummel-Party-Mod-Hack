using System;
using System.Collections.Generic;

namespace UnityEngine.AI
{
	// Token: 0x020007B2 RID: 1970
	[ExecuteInEditMode]
	[AddComponentMenu("Navigation/NavMeshModifier", 32)]
	[HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
	public class NavMeshModifier : MonoBehaviour
	{
		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x0600380C RID: 14348 RVA: 0x00026388 File Offset: 0x00024588
		// (set) Token: 0x0600380D RID: 14349 RVA: 0x00026390 File Offset: 0x00024590
		public bool overrideArea
		{
			get
			{
				return this.m_OverrideArea;
			}
			set
			{
				this.m_OverrideArea = value;
			}
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x0600380E RID: 14350 RVA: 0x00026399 File Offset: 0x00024599
		// (set) Token: 0x0600380F RID: 14351 RVA: 0x000263A1 File Offset: 0x000245A1
		public int area
		{
			get
			{
				return this.m_Area;
			}
			set
			{
				this.m_Area = value;
			}
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x06003810 RID: 14352 RVA: 0x000263AA File Offset: 0x000245AA
		// (set) Token: 0x06003811 RID: 14353 RVA: 0x000263B2 File Offset: 0x000245B2
		public bool ignoreFromBuild
		{
			get
			{
				return this.m_IgnoreFromBuild;
			}
			set
			{
				this.m_IgnoreFromBuild = value;
			}
		}

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x06003812 RID: 14354 RVA: 0x000263BB File Offset: 0x000245BB
		public static List<NavMeshModifier> activeModifiers
		{
			get
			{
				return NavMeshModifier.s_NavMeshModifiers;
			}
		}

		// Token: 0x06003813 RID: 14355 RVA: 0x000263C2 File Offset: 0x000245C2
		private void OnEnable()
		{
			if (!NavMeshModifier.s_NavMeshModifiers.Contains(this))
			{
				NavMeshModifier.s_NavMeshModifiers.Add(this);
			}
		}

		// Token: 0x06003814 RID: 14356 RVA: 0x000263DC File Offset: 0x000245DC
		private void OnDisable()
		{
			NavMeshModifier.s_NavMeshModifiers.Remove(this);
		}

		// Token: 0x06003815 RID: 14357 RVA: 0x000263EA File Offset: 0x000245EA
		public bool AffectsAgentType(int agentTypeID)
		{
			return this.m_AffectedAgents.Count != 0 && (this.m_AffectedAgents[0] == -1 || this.m_AffectedAgents.IndexOf(agentTypeID) != -1);
		}

		// Token: 0x040036DD RID: 14045
		[SerializeField]
		private bool m_OverrideArea;

		// Token: 0x040036DE RID: 14046
		[SerializeField]
		private int m_Area;

		// Token: 0x040036DF RID: 14047
		[SerializeField]
		private bool m_IgnoreFromBuild;

		// Token: 0x040036E0 RID: 14048
		[SerializeField]
		private List<int> m_AffectedAgents = new List<int>(new int[]
		{
			-1
		});

		// Token: 0x040036E1 RID: 14049
		private static readonly List<NavMeshModifier> s_NavMeshModifiers = new List<NavMeshModifier>();
	}
}
