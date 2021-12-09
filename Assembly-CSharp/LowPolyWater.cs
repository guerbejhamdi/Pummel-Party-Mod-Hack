using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000419 RID: 1049
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class LowPolyWater : MonoBehaviour
{
	// Token: 0x06001D22 RID: 7458 RVA: 0x00015792 File Offset: 0x00013992
	private void Init()
	{
		this.waterMesh = new Mesh();
		this.meshFilter = base.GetComponent<MeshFilter>();
		this.BuildMesh();
		this.meshFilter.sharedMesh = this.waterMesh;
	}

	// Token: 0x06001D23 RID: 7459 RVA: 0x000BEF04 File Offset: 0x000BD104
	public void BuildMesh()
	{
		if (this.waterMesh != null)
		{
			this.waterMesh.Clear();
		}
		else
		{
			this.waterMesh = new Mesh();
			this.meshFilter = base.GetComponent<MeshFilter>();
			this.meshFilter.sharedMesh = this.waterMesh;
		}
		List<Vector3> list = new List<Vector3>();
		List<Vector4> list2 = new List<Vector4>();
		List<int> list3 = new List<int>();
		int num = 0;
		Vector3 b = new Vector3(this.cellSize * (float)this.width * 0.5f, 0f, this.cellSize * (float)this.height * 0.5f);
		for (int i = 0; i < this.width; i++)
		{
			for (int j = 0; j < this.height; j++)
			{
				Vector3 vector = new Vector3((float)i * this.cellSize, 0f, (float)j * this.cellSize) - b;
				if (this.shape == LowPolyWater.WaterShape.Square || Vector3.Distance(vector + new Vector3(this.cellSize / 2f, 0f, this.cellSize / 2f), new Vector3(0f, 0f, 0f)) < this.radius)
				{
					list.Add(vector);
					list2.Add(new Vector4(0f, this.cellSize, this.cellSize, 0f));
					list.Add(vector + Vector3.forward * this.cellSize);
					list2.Add(new Vector4(this.cellSize, -this.cellSize, -this.cellSize, 0f));
					list.Add(vector - Vector3.left * this.cellSize);
					list2.Add(new Vector4(-this.cellSize, 0f, -this.cellSize, this.cellSize));
					list.Add(vector + new Vector3(0f, 0f, this.cellSize));
					list2.Add(new Vector4(this.cellSize, 0f, this.cellSize, -this.cellSize));
					list.Add(vector + new Vector3(this.cellSize, 0f, this.cellSize));
					list2.Add(new Vector4(0f, -this.cellSize, -this.cellSize, 0f));
					list.Add(vector + new Vector3(this.cellSize, 0f, 0f));
					list2.Add(new Vector4(-this.cellSize, this.cellSize, 0f, this.cellSize));
					for (int k = 0; k < 6; k++)
					{
						list3.Add(num++);
					}
				}
			}
		}
		this.waterMesh.vertices = list.ToArray();
		this.waterMesh.tangents = list2.ToArray();
		this.waterMesh.SetTriangles(list3, 0);
	}

	// Token: 0x06001D24 RID: 7460 RVA: 0x000157C2 File Offset: 0x000139C2
	private void OnDestroy()
	{
		if (Application.isEditor)
		{
			UnityEngine.Object.DestroyImmediate(this.waterMesh);
			return;
		}
		UnityEngine.Object.Destroy(this.waterMesh);
	}

	// Token: 0x04001F9C RID: 8092
	public int width = 24;

	// Token: 0x04001F9D RID: 8093
	public int height = 24;

	// Token: 0x04001F9E RID: 8094
	public float cellSize = 0.1f;

	// Token: 0x04001F9F RID: 8095
	public LowPolyWater.WaterShape shape;

	// Token: 0x04001FA0 RID: 8096
	public float radius = 8f;

	// Token: 0x04001FA1 RID: 8097
	private MeshFilter meshFilter;

	// Token: 0x04001FA2 RID: 8098
	private Mesh waterMesh;

	// Token: 0x0200041A RID: 1050
	public enum WaterShape
	{
		// Token: 0x04001FA4 RID: 8100
		Square,
		// Token: 0x04001FA5 RID: 8101
		Circle
	}
}
