using System;
using System.Collections.Generic;

namespace UnityEngine.AI
{
	// Token: 0x020007B3 RID: 1971
	[ExecuteInEditMode]
	[AddComponentMenu("Navigation/NavMeshModifierVolume", 31)]
	[HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
	public class NavMeshModifierVolume : MonoBehaviour
	{
		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x06003818 RID: 14360 RVA: 0x00026447 File Offset: 0x00024647
		// (set) Token: 0x06003819 RID: 14361 RVA: 0x0002644F File Offset: 0x0002464F
		public Vector3 size
		{
			get
			{
				return this.m_Size;
			}
			set
			{
				this.m_Size = value;
			}
		}

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x0600381A RID: 14362 RVA: 0x00026458 File Offset: 0x00024658
		// (set) Token: 0x0600381B RID: 14363 RVA: 0x00026460 File Offset: 0x00024660
		public Vector3 center
		{
			get
			{
				return this.m_Center;
			}
			set
			{
				this.m_Center = value;
			}
		}

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x0600381C RID: 14364 RVA: 0x00026469 File Offset: 0x00024669
		// (set) Token: 0x0600381D RID: 14365 RVA: 0x00026471 File Offset: 0x00024671
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

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x0600381E RID: 14366 RVA: 0x0002647A File Offset: 0x0002467A
		public static List<NavMeshModifierVolume> activeModifiers
		{
			get
			{
				return NavMeshModifierVolume.s_NavMeshModifiers;
			}
		}

		// Token: 0x0600381F RID: 14367 RVA: 0x00026481 File Offset: 0x00024681
		private void OnEnable()
		{
			if (!NavMeshModifierVolume.s_NavMeshModifiers.Contains(this))
			{
				NavMeshModifierVolume.s_NavMeshModifiers.Add(this);
			}
		}

		// Token: 0x06003820 RID: 14368 RVA: 0x0002649B File Offset: 0x0002469B
		private void OnDisable()
		{
			NavMeshModifierVolume.s_NavMeshModifiers.Remove(this);
		}

		// Token: 0x06003821 RID: 14369 RVA: 0x000264A9 File Offset: 0x000246A9
		public bool AffectsAgentType(int agentTypeID)
		{
			return this.m_AffectedAgents.Count != 0 && (this.m_AffectedAgents[0] == -1 || this.m_AffectedAgents.IndexOf(agentTypeID) != -1);
		}

		// Token: 0x040036E2 RID: 14050
		[SerializeField]
		private Vector3 m_Size = new Vector3(4f, 3f, 4f);

		// Token: 0x040036E3 RID: 14051
		[SerializeField]
		private Vector3 m_Center = new Vector3(0f, 1f, 0f);

		// Token: 0x040036E4 RID: 14052
		[SerializeField]
		private int m_Area;

		// Token: 0x040036E5 RID: 14053
		[SerializeField]
		private List<int> m_AffectedAgents = new List<int>(new int[]
		{
			-1
		});

		// Token: 0x040036E6 RID: 14054
		private static readonly List<NavMeshModifierVolume> s_NavMeshModifiers = new List<NavMeshModifierVolume>();
	}
}
