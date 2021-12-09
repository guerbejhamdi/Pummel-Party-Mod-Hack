using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004E3 RID: 1251
public class TwitchCrowdTest : MonoBehaviour
{
	// Token: 0x06002106 RID: 8454 RVA: 0x000CD7D4 File Offset: 0x000CB9D4
	private void Start()
	{
		MeshFilter[] componentsInChildren = base.GetComponentsInChildren<MeshFilter>();
		CombineInstance[] array = new CombineInstance[componentsInChildren.Length];
		int vertexCount = componentsInChildren[0].mesh.vertexCount;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			List<Color> list = new List<Color>();
			List<Vector2> list2 = new List<Vector2>();
			for (int j = 0; j < vertexCount; j++)
			{
				list2.Add(Vector2.right * (float)(i + 1));
			}
			Color item = this.colorList[UnityEngine.Random.Range(0, this.colorList.Length)];
			for (int k = 0; k < vertexCount; k++)
			{
				list.Add(item);
			}
			componentsInChildren[i].mesh.SetUVs(0, list2);
			componentsInChildren[i].mesh.SetColors(list);
			componentsInChildren[i].mesh.UploadMeshData(false);
		}
		for (int l = 0; l < componentsInChildren.Length; l++)
		{
			array[l].mesh = componentsInChildren[l].sharedMesh;
			array[l].transform = componentsInChildren[l].transform.localToWorldMatrix;
			componentsInChildren[l].gameObject.SetActive(false);
		}
		this.meshFilter = base.gameObject.AddComponent<MeshFilter>();
		this.meshFilter.mesh = new Mesh();
		this.meshFilter.mesh.CombineMeshes(array);
	}

	// Token: 0x06002107 RID: 8455 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x040023C2 RID: 9154
	public Color[] colorList;

	// Token: 0x040023C3 RID: 9155
	private MeshFilter meshFilter;
}
