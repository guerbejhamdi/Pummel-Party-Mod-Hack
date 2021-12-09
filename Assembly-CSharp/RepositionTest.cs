using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200046B RID: 1131
[ExecuteInEditMode]
public class RepositionTest : MonoBehaviour
{
	// Token: 0x06001E98 RID: 7832 RVA: 0x000C5D7C File Offset: 0x000C3F7C
	public void Reposition()
	{
		MeshFilter[] componentsInChildren = base.GetComponentsInChildren<MeshFilter>();
		List<Mesh> list = new List<Mesh>();
		List<Transform> list2 = new List<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			list.Add(componentsInChildren[i].sharedMesh);
			list2.Add(componentsInChildren[i].transform);
		}
		Vector3 position = base.transform.root.position;
		base.transform.root.position = Vector3.zero;
		base.transform.localPosition = Vector3.zero;
		Bounds bounds = this.GetBounds(list, list2);
		base.transform.localPosition = -bounds.center;
		base.transform.root.position = position;
	}

	// Token: 0x06001E99 RID: 7833 RVA: 0x0003AEA8 File Offset: 0x000390A8
	private Bounds GetBounds(List<Mesh> m, List<Transform> t)
	{
		Bounds result = default(Bounds);
		bool flag = true;
		for (int i = 0; i < m.Count; i++)
		{
			for (int j = 0; j < m[i].vertices.Length; j++)
			{
				Vector3 vector = t[i].TransformPoint(m[i].vertices[j]);
				if (flag)
				{
					result = new Bounds(vector, Vector3.zero);
					flag = false;
				}
				else
				{
					result.Encapsulate(vector);
				}
			}
		}
		return result;
	}
}
