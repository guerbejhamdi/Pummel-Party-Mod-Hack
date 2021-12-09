using System;
using UnityEngine;

// Token: 0x02000579 RID: 1401
public class Chunk
{
	// Token: 0x060024A5 RID: 9381 RVA: 0x000DBAC0 File Offset: 0x000D9CC0
	public Chunk(Vector3 p, VoxelGrid grid)
	{
		this.grid = grid;
		this.gridPosition = p;
		this.voxelPosition = p * (float)grid.detailPerChunk;
		float num = -((float)grid.detailPerChunk * grid.voxelSize / 2f);
		this.localPosition = new Vector3(num, num, num);
		this.id = (int)(p.x + p.y * grid.size.x + p.z * (grid.size.x * grid.size.y));
	}

	// Token: 0x060024A6 RID: 9382 RVA: 0x000DBB58 File Offset: 0x000D9D58
	public void SetupGameObject()
	{
		float num = (float)this.grid.detailPerChunk * this.grid.voxelSize;
		this.gameObject = new GameObject(string.Concat(new string[]
		{
			"Chunk",
			this.gridPosition.x.ToString(),
			"-",
			this.gridPosition.y.ToString(),
			"-",
			this.gridPosition.z.ToString()
		}), new Type[]
		{
			typeof(MeshFilter),
			typeof(MeshRenderer),
			typeof(MeshCollider)
		});
		if (this.grid.UseNavMesh)
		{
			this.gameObject.AddComponent<NavMeshSourceTag>();
		}
		this.gameObject.tag = "VoxelTerrain";
		this.gameObject.layer = this.grid.layer;
		this.gameObject.transform.parent = this.grid.transform;
		this.gameObject.transform.localPosition = -new Vector3((float)(this.grid.chunkSize.x - 1) / 2f * num, (float)(this.grid.chunkSize.y - 1) / 2f * num, (float)(this.grid.chunkSize.z - 1) / 2f * num) + new Vector3(this.gridPosition.x * num, this.gridPosition.y * num, this.gridPosition.z * num);
		this.gameObject.transform.localRotation = Quaternion.identity;
		this.gameObject.transform.localScale = Vector3.one;
		this.mesh = new Mesh();
		if (!this.grid.isStatic)
		{
			this.mesh.MarkDynamic();
		}
		this.mesh.name = string.Concat(new string[]
		{
			"ChunkMesh",
			this.gridPosition.x.ToString(),
			"-",
			this.gridPosition.y.ToString(),
			"-",
			this.gridPosition.z.ToString()
		});
		this.gameObject.GetComponent<MeshFilter>().mesh = this.mesh;
		this.meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
		this.meshRenderer.sharedMaterial = this.grid.chunkMat;
		this.meshRenderer.shadowCastingMode = this.grid.shadowCastingMode;
		this.meshCollider = this.gameObject.GetComponent<MeshCollider>();
		this.meshCollider.sharedMesh = new Mesh();
		this.meshCollider.material = this.grid.physicMaterial;
	}

	// Token: 0x04002808 RID: 10248
	public bool building;

	// Token: 0x04002809 RID: 10249
	public bool forceColliderUpdate;

	// Token: 0x0400280A RID: 10250
	public Mesh mesh;

	// Token: 0x0400280B RID: 10251
	public MeshCollider meshCollider;

	// Token: 0x0400280C RID: 10252
	public MeshRenderer meshRenderer;

	// Token: 0x0400280D RID: 10253
	public Vector3 localPosition;

	// Token: 0x0400280E RID: 10254
	public Vector3 voxelPosition;

	// Token: 0x0400280F RID: 10255
	public Vector3 gridPosition;

	// Token: 0x04002810 RID: 10256
	public ChunkUpdate lastUpdate;

	// Token: 0x04002811 RID: 10257
	public GameObject gameObject;

	// Token: 0x04002812 RID: 10258
	public int id;

	// Token: 0x04002813 RID: 10259
	private VoxelGrid grid;
}
