using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020002B4 RID: 692
[DefaultExecutionOrder(-200)]
public class NavMeshSourceTag : MonoBehaviour
{
	// Token: 0x0600140C RID: 5132 RVA: 0x000975AC File Offset: 0x000957AC
	private void OnEnable()
	{
		MeshFilter component = base.GetComponent<MeshFilter>();
		if (component != null)
		{
			NavMeshSourceTag.m_Meshes.Add(component);
		}
		Terrain component2 = base.GetComponent<Terrain>();
		if (component2 != null)
		{
			NavMeshSourceTag.m_Terrains.Add(component2);
		}
	}

	// Token: 0x0600140D RID: 5133 RVA: 0x000975F0 File Offset: 0x000957F0
	private void OnDisable()
	{
		MeshFilter component = base.GetComponent<MeshFilter>();
		if (component != null)
		{
			NavMeshSourceTag.m_Meshes.Remove(component);
		}
		Terrain component2 = base.GetComponent<Terrain>();
		if (component2 != null)
		{
			NavMeshSourceTag.m_Terrains.Remove(component2);
		}
	}

	// Token: 0x0600140E RID: 5134 RVA: 0x00097638 File Offset: 0x00095838
	public static void Collect(ref List<NavMeshBuildSource> sources)
	{
		sources.Clear();
		for (int i = 0; i < NavMeshSourceTag.m_Meshes.Count; i++)
		{
			MeshFilter meshFilter = NavMeshSourceTag.m_Meshes[i];
			if (!(meshFilter == null))
			{
				Mesh sharedMesh = meshFilter.sharedMesh;
				if (!(sharedMesh == null))
				{
					NavMeshBuildSource item = default(NavMeshBuildSource);
					item.shape = NavMeshBuildSourceShape.Mesh;
					item.sourceObject = sharedMesh;
					item.transform = meshFilter.transform.localToWorldMatrix;
					item.area = 0;
					sources.Add(item);
				}
			}
		}
		for (int j = 0; j < NavMeshSourceTag.m_Terrains.Count; j++)
		{
			Terrain terrain = NavMeshSourceTag.m_Terrains[j];
			if (!(terrain == null))
			{
				NavMeshBuildSource item2 = default(NavMeshBuildSource);
				item2.shape = NavMeshBuildSourceShape.Terrain;
				item2.sourceObject = terrain.terrainData;
				item2.transform = Matrix4x4.TRS(terrain.transform.position, Quaternion.identity, Vector3.one);
				item2.area = 0;
				sources.Add(item2);
			}
		}
	}

	// Token: 0x04001543 RID: 5443
	public static List<MeshFilter> m_Meshes = new List<MeshFilter>();

	// Token: 0x04001544 RID: 5444
	public static List<Terrain> m_Terrains = new List<Terrain>();
}
