using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace UnityEngine.AI
{
	// Token: 0x020007B5 RID: 1973
	[ExecuteInEditMode]
	[DefaultExecutionOrder(-102)]
	[AddComponentMenu("Navigation/NavMeshSurface", 30)]
	[HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
	public class NavMeshSurface : MonoBehaviour
	{
		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06003824 RID: 14372 RVA: 0x000264E9 File Offset: 0x000246E9
		// (set) Token: 0x06003825 RID: 14373 RVA: 0x000264F1 File Offset: 0x000246F1
		public int agentTypeID
		{
			get
			{
				return this.m_AgentTypeID;
			}
			set
			{
				this.m_AgentTypeID = value;
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06003826 RID: 14374 RVA: 0x000264FA File Offset: 0x000246FA
		// (set) Token: 0x06003827 RID: 14375 RVA: 0x00026502 File Offset: 0x00024702
		public CollectObjects collectObjects
		{
			get
			{
				return this.m_CollectObjects;
			}
			set
			{
				this.m_CollectObjects = value;
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06003828 RID: 14376 RVA: 0x0002650B File Offset: 0x0002470B
		// (set) Token: 0x06003829 RID: 14377 RVA: 0x00026513 File Offset: 0x00024713
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

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x0600382A RID: 14378 RVA: 0x0002651C File Offset: 0x0002471C
		// (set) Token: 0x0600382B RID: 14379 RVA: 0x00026524 File Offset: 0x00024724
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

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x0600382C RID: 14380 RVA: 0x0002652D File Offset: 0x0002472D
		// (set) Token: 0x0600382D RID: 14381 RVA: 0x00026535 File Offset: 0x00024735
		public LayerMask layerMask
		{
			get
			{
				return this.m_LayerMask;
			}
			set
			{
				this.m_LayerMask = value;
			}
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x0600382E RID: 14382 RVA: 0x0002653E File Offset: 0x0002473E
		// (set) Token: 0x0600382F RID: 14383 RVA: 0x00026546 File Offset: 0x00024746
		public NavMeshCollectGeometry useGeometry
		{
			get
			{
				return this.m_UseGeometry;
			}
			set
			{
				this.m_UseGeometry = value;
			}
		}

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06003830 RID: 14384 RVA: 0x0002654F File Offset: 0x0002474F
		// (set) Token: 0x06003831 RID: 14385 RVA: 0x00026557 File Offset: 0x00024757
		public int defaultArea
		{
			get
			{
				return this.m_DefaultArea;
			}
			set
			{
				this.m_DefaultArea = value;
			}
		}

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06003832 RID: 14386 RVA: 0x00026560 File Offset: 0x00024760
		// (set) Token: 0x06003833 RID: 14387 RVA: 0x00026568 File Offset: 0x00024768
		public bool ignoreNavMeshAgent
		{
			get
			{
				return this.m_IgnoreNavMeshAgent;
			}
			set
			{
				this.m_IgnoreNavMeshAgent = value;
			}
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06003834 RID: 14388 RVA: 0x00026571 File Offset: 0x00024771
		// (set) Token: 0x06003835 RID: 14389 RVA: 0x00026579 File Offset: 0x00024779
		public bool ignoreNavMeshObstacle
		{
			get
			{
				return this.m_IgnoreNavMeshObstacle;
			}
			set
			{
				this.m_IgnoreNavMeshObstacle = value;
			}
		}

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06003836 RID: 14390 RVA: 0x00026582 File Offset: 0x00024782
		// (set) Token: 0x06003837 RID: 14391 RVA: 0x0002658A File Offset: 0x0002478A
		public bool overrideTileSize
		{
			get
			{
				return this.m_OverrideTileSize;
			}
			set
			{
				this.m_OverrideTileSize = value;
			}
		}

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x06003838 RID: 14392 RVA: 0x00026593 File Offset: 0x00024793
		// (set) Token: 0x06003839 RID: 14393 RVA: 0x0002659B File Offset: 0x0002479B
		public int tileSize
		{
			get
			{
				return this.m_TileSize;
			}
			set
			{
				this.m_TileSize = value;
			}
		}

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x0600383A RID: 14394 RVA: 0x000265A4 File Offset: 0x000247A4
		// (set) Token: 0x0600383B RID: 14395 RVA: 0x000265AC File Offset: 0x000247AC
		public bool overrideVoxelSize
		{
			get
			{
				return this.m_OverrideVoxelSize;
			}
			set
			{
				this.m_OverrideVoxelSize = value;
			}
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x0600383C RID: 14396 RVA: 0x000265B5 File Offset: 0x000247B5
		// (set) Token: 0x0600383D RID: 14397 RVA: 0x000265BD File Offset: 0x000247BD
		public float voxelSize
		{
			get
			{
				return this.m_VoxelSize;
			}
			set
			{
				this.m_VoxelSize = value;
			}
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x0600383E RID: 14398 RVA: 0x000265C6 File Offset: 0x000247C6
		// (set) Token: 0x0600383F RID: 14399 RVA: 0x000265CE File Offset: 0x000247CE
		public bool buildHeightMesh
		{
			get
			{
				return this.m_BuildHeightMesh;
			}
			set
			{
				this.m_BuildHeightMesh = value;
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x06003840 RID: 14400 RVA: 0x000265D7 File Offset: 0x000247D7
		// (set) Token: 0x06003841 RID: 14401 RVA: 0x000265DF File Offset: 0x000247DF
		public NavMeshData navMeshData
		{
			get
			{
				return this.m_NavMeshData;
			}
			set
			{
				this.m_NavMeshData = value;
			}
		}

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x06003842 RID: 14402 RVA: 0x000265E8 File Offset: 0x000247E8
		public static List<NavMeshSurface> activeSurfaces
		{
			get
			{
				return NavMeshSurface.s_NavMeshSurfaces;
			}
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x000265EF File Offset: 0x000247EF
		private void OnEnable()
		{
			NavMeshSurface.Register(this);
			this.AddData();
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x000265FD File Offset: 0x000247FD
		private void OnDisable()
		{
			this.RemoveData();
			NavMeshSurface.Unregister(this);
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x0011A9CC File Offset: 0x00118BCC
		public void AddData()
		{
			if (this.m_NavMeshDataInstance.valid)
			{
				return;
			}
			if (this.m_NavMeshData != null)
			{
				this.m_NavMeshDataInstance = NavMesh.AddNavMeshData(this.m_NavMeshData, base.transform.position, base.transform.rotation);
				this.m_NavMeshDataInstance.owner = this;
			}
			this.m_LastPosition = base.transform.position;
			this.m_LastRotation = base.transform.rotation;
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x0002660B File Offset: 0x0002480B
		public void RemoveData()
		{
			this.m_NavMeshDataInstance.Remove();
			this.m_NavMeshDataInstance = default(NavMeshDataInstance);
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x0011AA4C File Offset: 0x00118C4C
		public NavMeshBuildSettings GetBuildSettings()
		{
			NavMeshBuildSettings settingsByID = NavMesh.GetSettingsByID(this.m_AgentTypeID);
			if (settingsByID.agentTypeID == -1)
			{
				Debug.LogWarning("No build settings for agent type ID " + this.agentTypeID.ToString(), this);
				settingsByID.agentTypeID = this.m_AgentTypeID;
			}
			if (this.overrideTileSize)
			{
				settingsByID.overrideTileSize = true;
				settingsByID.tileSize = this.tileSize;
			}
			if (this.overrideVoxelSize)
			{
				settingsByID.overrideVoxelSize = true;
				settingsByID.voxelSize = this.voxelSize;
			}
			return settingsByID;
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x0011AAD8 File Offset: 0x00118CD8
		public void BuildNavMesh()
		{
			List<NavMeshBuildSource> sources = this.CollectSources();
			Bounds localBounds = new Bounds(this.m_Center, NavMeshSurface.Abs(this.m_Size));
			if (this.m_CollectObjects == CollectObjects.All || this.m_CollectObjects == CollectObjects.Children)
			{
				localBounds = this.CalculateWorldBounds(sources);
			}
			NavMeshData navMeshData = NavMeshBuilder.BuildNavMeshData(this.GetBuildSettings(), sources, localBounds, base.transform.position, base.transform.rotation);
			if (navMeshData != null)
			{
				navMeshData.name = base.gameObject.name;
				this.RemoveData();
				this.m_NavMeshData = navMeshData;
				if (base.isActiveAndEnabled)
				{
					this.AddData();
				}
			}
		}

		// Token: 0x06003849 RID: 14409 RVA: 0x0011AB78 File Offset: 0x00118D78
		public AsyncOperation UpdateNavMesh(NavMeshData data)
		{
			List<NavMeshBuildSource> sources = this.CollectSources();
			Bounds localBounds = new Bounds(this.m_Center, NavMeshSurface.Abs(this.m_Size));
			if (this.m_CollectObjects == CollectObjects.All || this.m_CollectObjects == CollectObjects.Children)
			{
				localBounds = this.CalculateWorldBounds(sources);
			}
			return NavMeshBuilder.UpdateNavMeshDataAsync(data, this.GetBuildSettings(), sources, localBounds);
		}

		// Token: 0x0600384A RID: 14410 RVA: 0x0011ABCC File Offset: 0x00118DCC
		private static void Register(NavMeshSurface surface)
		{
			if (NavMeshSurface.s_NavMeshSurfaces.Count == 0)
			{
				NavMesh.onPreUpdate = (NavMesh.OnNavMeshPreUpdate)Delegate.Combine(NavMesh.onPreUpdate, new NavMesh.OnNavMeshPreUpdate(NavMeshSurface.UpdateActive));
			}
			if (!NavMeshSurface.s_NavMeshSurfaces.Contains(surface))
			{
				NavMeshSurface.s_NavMeshSurfaces.Add(surface);
			}
		}

		// Token: 0x0600384B RID: 14411 RVA: 0x00026624 File Offset: 0x00024824
		private static void Unregister(NavMeshSurface surface)
		{
			NavMeshSurface.s_NavMeshSurfaces.Remove(surface);
			if (NavMeshSurface.s_NavMeshSurfaces.Count == 0)
			{
				NavMesh.onPreUpdate = (NavMesh.OnNavMeshPreUpdate)Delegate.Remove(NavMesh.onPreUpdate, new NavMesh.OnNavMeshPreUpdate(NavMeshSurface.UpdateActive));
			}
		}

		// Token: 0x0600384C RID: 14412 RVA: 0x0011AC20 File Offset: 0x00118E20
		private static void UpdateActive()
		{
			for (int i = 0; i < NavMeshSurface.s_NavMeshSurfaces.Count; i++)
			{
				NavMeshSurface.s_NavMeshSurfaces[i].UpdateDataIfTransformChanged();
			}
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x0011AC54 File Offset: 0x00118E54
		private void AppendModifierVolumes(ref List<NavMeshBuildSource> sources)
		{
			List<NavMeshModifierVolume> list;
			if (this.m_CollectObjects == CollectObjects.Children)
			{
				list = new List<NavMeshModifierVolume>(base.GetComponentsInChildren<NavMeshModifierVolume>());
				list.RemoveAll((NavMeshModifierVolume x) => !x.isActiveAndEnabled);
			}
			else
			{
				list = NavMeshModifierVolume.activeModifiers;
			}
			foreach (NavMeshModifierVolume navMeshModifierVolume in list)
			{
				if ((this.m_LayerMask & 1 << navMeshModifierVolume.gameObject.layer) != 0 && navMeshModifierVolume.AffectsAgentType(this.m_AgentTypeID))
				{
					Vector3 pos = navMeshModifierVolume.transform.TransformPoint(navMeshModifierVolume.center);
					Vector3 lossyScale = navMeshModifierVolume.transform.lossyScale;
					Vector3 size = new Vector3(navMeshModifierVolume.size.x * Mathf.Abs(lossyScale.x), navMeshModifierVolume.size.y * Mathf.Abs(lossyScale.y), navMeshModifierVolume.size.z * Mathf.Abs(lossyScale.z));
					NavMeshBuildSource item = default(NavMeshBuildSource);
					item.shape = NavMeshBuildSourceShape.ModifierBox;
					item.transform = Matrix4x4.TRS(pos, navMeshModifierVolume.transform.rotation, Vector3.one);
					item.size = size;
					item.area = navMeshModifierVolume.area;
					sources.Add(item);
				}
			}
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x0011ADDC File Offset: 0x00118FDC
		private List<NavMeshBuildSource> CollectSources()
		{
			List<NavMeshBuildSource> list = new List<NavMeshBuildSource>();
			List<NavMeshBuildMarkup> list2 = new List<NavMeshBuildMarkup>();
			List<NavMeshModifier> list3;
			if (this.m_CollectObjects == CollectObjects.Children)
			{
				list3 = new List<NavMeshModifier>(base.GetComponentsInChildren<NavMeshModifier>());
				list3.RemoveAll((NavMeshModifier x) => !x.isActiveAndEnabled);
			}
			else
			{
				list3 = NavMeshModifier.activeModifiers;
			}
			foreach (NavMeshModifier navMeshModifier in list3)
			{
				if ((this.m_LayerMask & 1 << navMeshModifier.gameObject.layer) != 0 && navMeshModifier.AffectsAgentType(this.m_AgentTypeID))
				{
					list2.Add(new NavMeshBuildMarkup
					{
						root = navMeshModifier.transform,
						overrideArea = navMeshModifier.overrideArea,
						area = navMeshModifier.area,
						ignoreFromBuild = navMeshModifier.ignoreFromBuild
					});
				}
			}
			if (this.m_CollectObjects == CollectObjects.All)
			{
				NavMeshBuilder.CollectSources(null, this.m_LayerMask, this.m_UseGeometry, this.m_DefaultArea, list2, list);
			}
			else if (this.m_CollectObjects == CollectObjects.Children)
			{
				NavMeshBuilder.CollectSources(base.transform, this.m_LayerMask, this.m_UseGeometry, this.m_DefaultArea, list2, list);
			}
			else if (this.m_CollectObjects == CollectObjects.Volume)
			{
				NavMeshBuilder.CollectSources(NavMeshSurface.GetWorldBounds(Matrix4x4.TRS(base.transform.position, base.transform.rotation, Vector3.one), new Bounds(this.m_Center, this.m_Size)), this.m_LayerMask, this.m_UseGeometry, this.m_DefaultArea, list2, list);
			}
			if (this.m_IgnoreNavMeshAgent)
			{
				list.RemoveAll((NavMeshBuildSource x) => x.component != null && x.component.gameObject.GetComponent<NavMeshAgent>() != null);
			}
			if (this.m_IgnoreNavMeshObstacle)
			{
				list.RemoveAll((NavMeshBuildSource x) => x.component != null && x.component.gameObject.GetComponent<NavMeshObstacle>() != null);
			}
			this.AppendModifierVolumes(ref list);
			return list;
		}

		// Token: 0x0600384F RID: 14415 RVA: 0x0002665E File Offset: 0x0002485E
		private static Vector3 Abs(Vector3 v)
		{
			return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x0011B00C File Offset: 0x0011920C
		private static Bounds GetWorldBounds(Matrix4x4 mat, Bounds bounds)
		{
			Vector3 a = NavMeshSurface.Abs(mat.MultiplyVector(Vector3.right));
			Vector3 a2 = NavMeshSurface.Abs(mat.MultiplyVector(Vector3.up));
			Vector3 a3 = NavMeshSurface.Abs(mat.MultiplyVector(Vector3.forward));
			Vector3 center = mat.MultiplyPoint(bounds.center);
			Vector3 size = a * bounds.size.x + a2 * bounds.size.y + a3 * bounds.size.z;
			return new Bounds(center, size);
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x0011B0A4 File Offset: 0x001192A4
		private Bounds CalculateWorldBounds(List<NavMeshBuildSource> sources)
		{
			Matrix4x4 inverse = Matrix4x4.TRS(base.transform.position, base.transform.rotation, Vector3.one).inverse;
			Bounds result = default(Bounds);
			foreach (NavMeshBuildSource navMeshBuildSource in sources)
			{
				switch (navMeshBuildSource.shape)
				{
				case NavMeshBuildSourceShape.Mesh:
				{
					Mesh mesh = navMeshBuildSource.sourceObject as Mesh;
					result.Encapsulate(NavMeshSurface.GetWorldBounds(inverse * navMeshBuildSource.transform, mesh.bounds));
					break;
				}
				case NavMeshBuildSourceShape.Terrain:
				{
					TerrainData terrainData = navMeshBuildSource.sourceObject as TerrainData;
					result.Encapsulate(NavMeshSurface.GetWorldBounds(inverse * navMeshBuildSource.transform, new Bounds(0.5f * terrainData.size, terrainData.size)));
					break;
				}
				case NavMeshBuildSourceShape.Box:
				case NavMeshBuildSourceShape.Sphere:
				case NavMeshBuildSourceShape.Capsule:
				case NavMeshBuildSourceShape.ModifierBox:
					result.Encapsulate(NavMeshSurface.GetWorldBounds(inverse * navMeshBuildSource.transform, new Bounds(Vector3.zero, navMeshBuildSource.size)));
					break;
				}
			}
			result.Expand(0.1f);
			return result;
		}

		// Token: 0x06003852 RID: 14418 RVA: 0x00026686 File Offset: 0x00024886
		private bool HasTransformChanged()
		{
			return this.m_LastPosition != base.transform.position || this.m_LastRotation != base.transform.rotation;
		}

		// Token: 0x06003853 RID: 14419 RVA: 0x000266BD File Offset: 0x000248BD
		private void UpdateDataIfTransformChanged()
		{
			if (this.HasTransformChanged())
			{
				this.RemoveData();
				this.AddData();
			}
		}

		// Token: 0x040036EB RID: 14059
		[SerializeField]
		private int m_AgentTypeID;

		// Token: 0x040036EC RID: 14060
		[SerializeField]
		private CollectObjects m_CollectObjects;

		// Token: 0x040036ED RID: 14061
		[SerializeField]
		private Vector3 m_Size = new Vector3(10f, 10f, 10f);

		// Token: 0x040036EE RID: 14062
		[SerializeField]
		private Vector3 m_Center = new Vector3(0f, 2f, 0f);

		// Token: 0x040036EF RID: 14063
		[SerializeField]
		private LayerMask m_LayerMask = -1;

		// Token: 0x040036F0 RID: 14064
		[SerializeField]
		private NavMeshCollectGeometry m_UseGeometry;

		// Token: 0x040036F1 RID: 14065
		[SerializeField]
		private int m_DefaultArea;

		// Token: 0x040036F2 RID: 14066
		[SerializeField]
		private bool m_IgnoreNavMeshAgent = true;

		// Token: 0x040036F3 RID: 14067
		[SerializeField]
		private bool m_IgnoreNavMeshObstacle = true;

		// Token: 0x040036F4 RID: 14068
		[SerializeField]
		private bool m_OverrideTileSize;

		// Token: 0x040036F5 RID: 14069
		[SerializeField]
		private int m_TileSize = 256;

		// Token: 0x040036F6 RID: 14070
		[SerializeField]
		private bool m_OverrideVoxelSize;

		// Token: 0x040036F7 RID: 14071
		[SerializeField]
		private float m_VoxelSize;

		// Token: 0x040036F8 RID: 14072
		[SerializeField]
		private bool m_BuildHeightMesh;

		// Token: 0x040036F9 RID: 14073
		[FormerlySerializedAs("m_BakedNavMeshData")]
		[SerializeField]
		private NavMeshData m_NavMeshData;

		// Token: 0x040036FA RID: 14074
		private NavMeshDataInstance m_NavMeshDataInstance;

		// Token: 0x040036FB RID: 14075
		private Vector3 m_LastPosition = Vector3.zero;

		// Token: 0x040036FC RID: 14076
		private Quaternion m_LastRotation = Quaternion.identity;

		// Token: 0x040036FD RID: 14077
		private static readonly List<NavMeshSurface> s_NavMeshSurfaces = new List<NavMeshSurface>();
	}
}
