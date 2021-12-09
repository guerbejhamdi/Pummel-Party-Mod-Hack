using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using ZP.Utility;

// Token: 0x02000583 RID: 1411
public class VoxelGrid : MonoBehaviour
{
	// Token: 0x14000007 RID: 7
	// (add) Token: 0x060024BE RID: 9406 RVA: 0x000DEAD4 File Offset: 0x000DCCD4
	// (remove) Token: 0x060024BF RID: 9407 RVA: 0x000DEB0C File Offset: 0x000DCD0C
	public event ChunkUpdated ChunkUpdatedEvent;

	// Token: 0x060024C0 RID: 9408 RVA: 0x0001A5CD File Offset: 0x000187CD
	public void Start()
	{
		if (this.buildOnStart)
		{
			this.Setup();
			this.BuildChunks();
		}
	}

	// Token: 0x060024C1 RID: 9409 RVA: 0x0001A5E3 File Offset: 0x000187E3
	public void Setup(Vector3 size, int detailPerChunk, float voxelSize, Material chunkMat, int layer)
	{
		this.size = size;
		this.detailPerChunk = detailPerChunk;
		this.voxelSize = voxelSize;
		this.chunkMat = chunkMat;
		this.layer = layer;
		this.Setup();
	}

	// Token: 0x060024C2 RID: 9410 RVA: 0x000DEB44 File Offset: 0x000DCD44
	public void Setup()
	{
		this.gridSize = this.size + new Vector3(3f, 3f, 3f);
		if (this.gridBinaryFile != null)
		{
			this.Load(this.gridBinaryFile);
		}
		else if (this.data == null)
		{
			this.data = VoxelFiller.FillGrid((int)this.gridSize.x, (int)this.gridSize.y, (int)this.gridSize.z);
		}
		this.builders = new ChunkBuilder[Environment.ProcessorCount * 4];
		for (int i = 0; i < Environment.ProcessorCount * 4; i++)
		{
			this.builders[i] = new ChunkBuilder(this);
		}
		this.chunkSize = new Vector3i((int)this.size.x / this.detailPerChunk, (int)this.size.y / this.detailPerChunk, (int)this.size.z / this.detailPerChunk);
		this.chunks = new Chunk[this.chunkSize.x, this.chunkSize.y, this.chunkSize.z];
		this.SetupChunks();
	}

	// Token: 0x060024C3 RID: 9411 RVA: 0x000DEC74 File Offset: 0x000DCE74
	private void SetupChunks()
	{
		for (int i = 0; i < this.chunkSize.x; i++)
		{
			for (int j = 0; j < this.chunkSize.y; j++)
			{
				for (int k = 0; k < this.chunkSize.z; k++)
				{
					this.chunks[i, j, k] = new Chunk(new Vector3((float)i, (float)j, (float)k), this);
					this.chunks[i, j, k].forceColliderUpdate = true;
				}
			}
		}
	}

	// Token: 0x060024C4 RID: 9412 RVA: 0x000DECF8 File Offset: 0x000DCEF8
	public void BuildChunks()
	{
		this.buildStarted = true;
		ThreadManager.StartJobBatchAssign(new EventHandler(this.CompleteHandler), true);
		for (int i = 0; i < this.chunkSize.x; i++)
		{
			for (int j = 0; j < this.chunkSize.y; j++)
			{
				for (int k = 0; k < this.chunkSize.z; k++)
				{
					ThreadManager.EnqueueBatch(new ChunkJob(this.chunks[i, j, k], this));
				}
			}
		}
		ThreadManager.FinishJobBatchAssign();
	}

	// Token: 0x060024C5 RID: 9413 RVA: 0x0001A610 File Offset: 0x00018810
	private void CompleteHandler(object sender, EventArgs e)
	{
		this.buildCompleted = true;
	}

	// Token: 0x060024C6 RID: 9414 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Update()
	{
	}

	// Token: 0x060024C7 RID: 9415 RVA: 0x0001A619 File Offset: 0x00018819
	public void LateUpdate()
	{
		if (!this.buildCompleted)
		{
			return;
		}
		this.UpdateGrid(false, false);
	}

	// Token: 0x060024C8 RID: 9416 RVA: 0x000DED80 File Offset: 0x000DCF80
	public void UpdateGrid(bool editor, bool followTerrain)
	{
		this.position = base.transform.position;
		this.test.Clear();
		for (int i = 0; i < this.chunkUpdates.Count; i++)
		{
			ChunkUpdate chunkUpdate = this.chunkUpdates[i];
			this.test.Add(chunkUpdate);
			if (chunkUpdate.vertices != null && chunkUpdate.vertices.Length != 0)
			{
				if (chunkUpdate.chunk.gameObject == null)
				{
					chunkUpdate.chunk.SetupGameObject();
				}
				chunkUpdate.chunk.mesh.Clear();
				chunkUpdate.chunk.meshRenderer.enabled = true;
				chunkUpdate.chunk.meshCollider.enabled = true;
				Vector3[] array = new Vector3[chunkUpdate.indices.Length];
				int[] array2 = new int[chunkUpdate.indices.Length];
				Color32[] array3 = new Color32[chunkUpdate.indices.Length];
				for (int j = 0; j < chunkUpdate.indices.Length; j++)
				{
					array[j] = chunkUpdate.vertices[chunkUpdate.indices[j]];
					array3[j] = chunkUpdate.colors[chunkUpdate.indices[j]];
					array2[j] = j;
				}
				chunkUpdate.chunk.mesh.vertices = array;
				chunkUpdate.chunk.mesh.colors32 = array3;
				chunkUpdate.chunk.mesh.triangles = array2;
				chunkUpdate.chunk.mesh.RecalculateNormals();
				if (!editor)
				{
					Mesh sharedMesh = chunkUpdate.chunk.meshCollider.sharedMesh;
					sharedMesh.Clear();
					sharedMesh.vertices = chunkUpdate.vertices;
					sharedMesh.normals = chunkUpdate.normals;
					sharedMesh.triangles = chunkUpdate.indices;
					chunkUpdate.chunk.meshCollider.sharedMesh = null;
					chunkUpdate.chunk.meshCollider.sharedMesh = sharedMesh;
					if (this.ChunkUpdatedEvent != null)
					{
						this.ChunkUpdatedEvent(chunkUpdate.chunk);
					}
				}
				else if (followTerrain || chunkUpdate.chunk.forceColliderUpdate)
				{
					Mesh sharedMesh2 = chunkUpdate.chunk.meshCollider.sharedMesh;
					sharedMesh2.Clear();
					sharedMesh2.vertices = chunkUpdate.vertices;
					sharedMesh2.normals = chunkUpdate.normals;
					sharedMesh2.triangles = chunkUpdate.indices;
					chunkUpdate.chunk.meshCollider.sharedMesh = null;
					chunkUpdate.chunk.meshCollider.sharedMesh = sharedMesh2;
					chunkUpdate.chunk.forceColliderUpdate = false;
					chunkUpdate.chunk.lastUpdate.colliderSet = true;
					if (this.ChunkUpdatedEvent != null)
					{
						this.ChunkUpdatedEvent(chunkUpdate.chunk);
					}
				}
			}
			else if (chunkUpdate.chunk.gameObject != null)
			{
				chunkUpdate.chunk.mesh.Clear();
				chunkUpdate.chunk.meshRenderer.enabled = false;
				chunkUpdate.chunk.meshCollider.enabled = false;
			}
		}
		List<ChunkUpdate> obj = this.chunkUpdates;
		lock (obj)
		{
			for (int k = 0; k < this.test.Count; k++)
			{
				this.chunkUpdates.Remove(this.test[k]);
			}
		}
	}

	// Token: 0x060024C9 RID: 9417 RVA: 0x000DF0EC File Offset: 0x000DD2EC
	public ChunkUpdate[] GetChunkUpdates()
	{
		List<ChunkUpdate> obj = this.chunkUpdates;
		ChunkUpdate[] result;
		lock (obj)
		{
			result = this.chunkUpdates.ToArray();
			this.chunkUpdates.Clear();
		}
		return result;
	}

	// Token: 0x060024CA RID: 9418 RVA: 0x000DF140 File Offset: 0x000DD340
	public void AddChunkUpdates(ChunkUpdate update)
	{
		List<ChunkUpdate> obj = this.chunkUpdates;
		lock (obj)
		{
			this.chunkUpdates.Remove(update.chunk.lastUpdate);
			this.chunkUpdates.Add(update);
		}
	}

	// Token: 0x060024CB RID: 9419 RVA: 0x0001A62C File Offset: 0x0001882C
	public void Load(string path)
	{
		this.Load(File.Open(path, FileMode.Open));
	}

	// Token: 0x060024CC RID: 9420 RVA: 0x0001A63B File Offset: 0x0001883B
	public void Load(TextAsset t)
	{
		this.Load(new MemoryStream(t.bytes));
	}

	// Token: 0x060024CD RID: 9421 RVA: 0x000DF1A0 File Offset: 0x000DD3A0
	public void Load(Stream s)
	{
		using (BinaryReader binaryReader = new BinaryReader(s))
		{
			this.size = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
			this.detailPerChunk = binaryReader.ReadInt32();
			this.voxelSize = binaryReader.ReadSingle();
			this.gridSize = this.size + new Vector3(3f, 3f, 3f);
			int count = (int)(this.gridSize.x * this.gridSize.y * this.gridSize.z * 2f);
			byte[] arr = binaryReader.ReadBytes(count);
			this.data = ArrayUtility.unflatten_byte_array_3d<short>(arr, (int)this.gridSize.x, (int)this.gridSize.y, (int)this.gridSize.z);
		}
	}

	// Token: 0x060024CE RID: 9422 RVA: 0x000DF290 File Offset: 0x000DD490
	public bool LoadFromFile(string path)
	{
		try
		{
			if (Directory.Exists(path) || File.Exists(path))
			{
				using (BinaryReader binaryReader = new BinaryReader(File.Open(path, FileMode.Open)))
				{
					this.size = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
					this.detailPerChunk = binaryReader.ReadInt32();
					this.voxelSize = binaryReader.ReadSingle();
					this.gridSize = this.size + new Vector3(3f, 3f, 3f);
					int count = (int)(this.gridSize.x * this.gridSize.y * this.gridSize.z * 2f);
					byte[] arr = binaryReader.ReadBytes(count);
					this.data = ArrayUtility.unflatten_byte_array_3d<short>(arr, (int)this.gridSize.x, (int)this.gridSize.y, (int)this.gridSize.z);
					goto IL_ED;
				}
				goto IL_E9;
				IL_ED:
				return true;
			}
			IL_E9:
			return false;
		}
		catch (Exception message)
		{
			VoxelFiller.FillGrid(this.data);
			Debug.Log(message);
			return false;
		}
		return true;
	}

	// Token: 0x060024CF RID: 9423 RVA: 0x000DF3C0 File Offset: 0x000DD5C0
	public bool Edit(Edit e, bool skipBuild = false)
	{
		e.pos -= base.transform.position;
		e.pos += (this.gridSize - Vector3.one) * this.voxelSize / 2f;
		if (e.shape == BrushShape.Sphere && (e.action == BrushAction.Add || e.action == BrushAction.Subtract))
		{
			e.size *= 2f;
		}
		float num = 2f;
		int num2 = (int)(Mathf.Clamp(e.pos.x - e.size / num, 0f, this.gridSize.x * this.voxelSize) / this.voxelSize);
		int num3 = (int)(Mathf.Clamp(e.pos.x + e.size / num, -1f, this.gridSize.x * this.voxelSize - this.voxelSize) / this.voxelSize);
		int num4 = (int)(Mathf.Clamp(e.pos.y - e.size / num, 0f, this.gridSize.y * this.voxelSize) / this.voxelSize);
		int num5 = (int)(Mathf.Clamp(e.pos.y + e.size / num, -1f, this.gridSize.y * this.voxelSize - this.voxelSize) / this.voxelSize);
		int num6 = (int)(Mathf.Clamp(e.pos.z - e.size / num, 0f, this.gridSize.z * this.voxelSize) / this.voxelSize);
		int num7 = (int)(Mathf.Clamp(e.pos.z + e.size / num, -1f, this.gridSize.z * this.voxelSize - this.voxelSize) / this.voxelSize);
		this.edits.Clear();
		for (int i = num2; i <= num3; i++)
		{
			for (int j = num4; j <= num5; j++)
			{
				for (int k = num6; k <= num7; k++)
				{
					switch (e.shape)
					{
					case BrushShape.Sphere:
					{
						float num8 = Vector3.Distance(e.pos, new Vector3((float)i, (float)j, (float)k) * this.voxelSize) / (e.size / 2f);
						if (num8 <= 1f)
						{
							switch (e.action)
							{
							case BrushAction.Subtract:
							{
								short num9 = (short)(32767f - 65534f * num8);
								if (num9 > this.data[i][j][k])
								{
									this.edits.Add(new VoxelGrid.GridEdit(i, j, k, num9));
								}
								break;
							}
							case BrushAction.Add:
							{
								short num10 = (short)(-32768f + 65534f * num8);
								if (num10 < this.data[i][j][k])
								{
									this.edits.Add(new VoxelGrid.GridEdit(i, j, k, num10));
								}
								break;
							}
							case BrushAction.Decrease:
							{
								ushort num11 = (ushort)(65535f * e.opacity * (1f - num8));
								this.edits.Add(new VoxelGrid.GridEdit(i, j, k, (short)Mathf.Clamp((int)(this.data[i][j][k] - (short)num11), -32768, 32767)));
								break;
							}
							case BrushAction.Increase:
							{
								ushort num12 = (ushort)(65535f * e.opacity * (1f - num8));
								this.edits.Add(new VoxelGrid.GridEdit(i, j, k, (short)Mathf.Clamp((int)(this.data[i][j][k] + (short)num12), -32768, 32767)));
								break;
							}
							case BrushAction.DecreaseRelative:
							{
								short num13 = (short)Mathf.Clamp(Mathf.Max(65534f * e.opacity, Mathf.Abs((float)this.data[i][j][k] * 15f) * e.opacity) * (1f - num8), -32768f, 32767f);
								this.edits.Add(new VoxelGrid.GridEdit(i, j, k, (short)Mathf.Clamp((int)(this.data[i][j][k] - num13), -32768, 32767)));
								break;
							}
							case BrushAction.IncreaseRelative:
							{
								short num14 = (short)Mathf.Clamp(Mathf.Max(65534f * e.opacity, Mathf.Abs((float)this.data[i][j][k] * 15f) * e.opacity) * (1f - num8), -32768f, 32767f);
								this.edits.Add(new VoxelGrid.GridEdit(i, j, k, (short)Mathf.Clamp((int)(this.data[i][j][k] + num14), -32768, 32767)));
								break;
							}
							case BrushAction.Smooth:
							{
								long num15 = 0L;
								int num16 = 0;
								int num17 = (int)Mathf.Clamp((float)(i - 1), 0f, this.gridSize.x - 1f);
								int num18 = (int)Mathf.Clamp((float)(j - 1), 0f, this.gridSize.y - 1f);
								int num19 = (int)Mathf.Clamp((float)(k - 1), 0f, this.gridSize.z - 1f);
								int num20 = (int)Mathf.Clamp((float)(i + 1), 0f, this.gridSize.x - 1f);
								int num21 = (int)Mathf.Clamp((float)(j + 1), 0f, this.gridSize.y - 1f);
								int num22 = (int)Mathf.Clamp((float)(k + 1), 0f, this.gridSize.z - 1f);
								for (int l = num17; l <= num20; l++)
								{
									for (int m = num18; m <= num21; m++)
									{
										for (int n = num19; n <= num22; n++)
										{
											num15 += (long)this.data[l][m][n];
											num16++;
										}
									}
								}
								short val = (short)Mathf.Clamp(Mathf.Lerp((float)this.data[i][j][k], (float)(num15 / (long)num16), e.opacity * num8), -32768f, 32767f);
								this.edits.Add(new VoxelGrid.GridEdit(i, j, k, val));
								break;
							}
							}
						}
						break;
					}
					case BrushShape.Cube:
						switch (e.action)
						{
						case BrushAction.Subtract:
							if (32767 > this.data[i][j][k])
							{
								this.edits.Add(new VoxelGrid.GridEdit(i, j, k, short.MaxValue));
							}
							break;
						case BrushAction.Add:
							if (-32768 < this.data[i][j][k])
							{
								this.edits.Add(new VoxelGrid.GridEdit(i, j, k, short.MinValue));
							}
							break;
						case BrushAction.Decrease:
						{
							ushort num23 = (ushort)(65535f * e.opacity);
							this.edits.Add(new VoxelGrid.GridEdit(i, j, k, (short)Mathf.Clamp((int)(this.data[i][j][k] - (short)num23), -32768, 32767)));
							break;
						}
						case BrushAction.Increase:
						{
							ushort num24 = (ushort)(65535f * e.opacity);
							this.edits.Add(new VoxelGrid.GridEdit(i, j, k, this.data[i][j][k] = (short)Mathf.Clamp((int)(this.data[i][j][k] + (short)num24), -32768, 32767)));
							break;
						}
						}
						break;
					case BrushShape.Cylinder:
						if (e.action == BrushAction.Subtract)
						{
							Vector3 vector = e.pos - new Vector3((float)i, (float)j, (float)k) * this.voxelSize;
							vector.z = 0f;
							float num25 = vector.magnitude / (e.size / 2f);
							if (num25 <= 1f)
							{
								short num26 = (short)(32767f - 65534f * num25);
								if (num26 > this.data[i][j][k])
								{
									this.edits.Add(new VoxelGrid.GridEdit(i, j, k, num26));
								}
							}
						}
						break;
					}
				}
			}
		}
		short[][][] obj = this.data;
		lock (obj)
		{
			for (int num27 = 0; num27 < this.edits.Count; num27++)
			{
				this.data[this.edits[num27].x][this.edits[num27].y][this.edits[num27].z] = this.edits[num27].val;
			}
		}
		if (!skipBuild)
		{
			this.UpdateChunks(num2, num3, num4, num5, num6, num7, false);
		}
		return this.edits.Count > 0;
	}

	// Token: 0x060024D0 RID: 9424 RVA: 0x0001A64E File Offset: 0x0001884E
	public void UpdateAllChunks(bool forceColliderUpdate)
	{
		this.UpdateChunks(0, (int)this.size.x, 0, (int)this.size.y, 0, (int)this.size.z, forceColliderUpdate);
	}

	// Token: 0x060024D1 RID: 9425 RVA: 0x000DFD10 File Offset: 0x000DDF10
	public void UpdateChunks(int minX, int maxX, int minY, int maxY, int minZ, int maxZ, bool forceColliderUpdate = false)
	{
		int chunkMinX = Mathf.Clamp(Mathf.Min(minX / this.detailPerChunk, (minX - 2) / this.detailPerChunk), 0, this.chunkSize.x);
		int chunkMaxX = Mathf.Clamp(maxX / this.detailPerChunk, -1, this.chunkSize.x - 1);
		int chunkMinY = Mathf.Clamp(Mathf.Min(minY / this.detailPerChunk, (minY - 2) / this.detailPerChunk), 0, this.chunkSize.y);
		int chunkMaxY = Mathf.Clamp(maxY / this.detailPerChunk, -1, this.chunkSize.y - 1);
		int chunkMinZ = Mathf.Clamp(Mathf.Min(minZ / this.detailPerChunk, (minZ - 2) / this.detailPerChunk), 0, this.chunkSize.z);
		int chunkMaxZ = Mathf.Clamp(maxZ / this.detailPerChunk, -1, this.chunkSize.z - 1);
		this.UpdateChunkRange(chunkMinX, chunkMaxX, chunkMinY, chunkMaxY, chunkMinZ, chunkMaxZ, forceColliderUpdate);
	}

	// Token: 0x060024D2 RID: 9426 RVA: 0x000DFE04 File Offset: 0x000DE004
	private void UpdateChunkRange(int chunkMinX, int chunkMaxX, int chunkMinY, int chunkMaxY, int chunkMinZ, int chunkMaxZ, bool forceColliderUpdate)
	{
		for (int i = chunkMinX; i <= chunkMaxX; i++)
		{
			for (int j = chunkMinY; j <= chunkMaxY; j++)
			{
				for (int k = chunkMinZ; k <= chunkMaxZ; k++)
				{
					this.chunks[i, j, k].forceColliderUpdate = forceColliderUpdate;
					ThreadManager.Enqueue(new ChunkJob(this.chunks[i, j, k], this), null, true);
				}
			}
		}
	}

	// Token: 0x060024D3 RID: 9427 RVA: 0x0001A67E File Offset: 0x0001887E
	public Vector3 GridPointToWorldSpace(Vector3 p)
	{
		return base.transform.position + (p - (this.gridSize - Vector3.one) / 2f) * this.voxelSize;
	}

	// Token: 0x060024D4 RID: 9428 RVA: 0x000DFE68 File Offset: 0x000DE068
	public Vector3i WorldPositionToGridPoint(Vector3 p)
	{
		return (p - base.transform.position) / this.voxelSize + (this.gridSize - Vector3.one) / 2f;
	}

	// Token: 0x060024D5 RID: 9429 RVA: 0x0001A6BB File Offset: 0x000188BB
	public Vector3 WorldPositionToLocalPosition(Vector3 p)
	{
		return p - base.transform.position + (this.gridSize - Vector3.one) * this.voxelSize / 2f;
	}

	// Token: 0x060024D6 RID: 9430 RVA: 0x0001A6F8 File Offset: 0x000188F8
	public bool Filled(Vector3i gridPos)
	{
		return this.data[gridPos.x][gridPos.y][gridPos.z] < 0;
	}

	// Token: 0x17000459 RID: 1113
	// (get) Token: 0x060024D7 RID: 9431 RVA: 0x0001A718 File Offset: 0x00018918
	// (set) Token: 0x060024D8 RID: 9432 RVA: 0x000DFEB8 File Offset: 0x000DE0B8
	public VoxelGridRenderingMode RenderMode
	{
		get
		{
			return this.renderMode;
		}
		set
		{
			this.renderMode = value;
			VoxelGridRenderingMode voxelGridRenderingMode = this.renderMode;
			if (voxelGridRenderingMode == VoxelGridRenderingMode.Normal)
			{
				this.SetMaterial(this.chunkMat);
				return;
			}
			if (voxelGridRenderingMode != VoxelGridRenderingMode.Wireframe)
			{
				return;
			}
			this.SetMaterial(this.wireFrameMat);
		}
	}

	// Token: 0x060024D9 RID: 9433 RVA: 0x000DFEF4 File Offset: 0x000DE0F4
	private void SetMaterial(Material m)
	{
		for (int i = 0; i < this.chunkSize.x; i++)
		{
			for (int j = 0; j < this.chunkSize.y; j++)
			{
				for (int k = 0; k < this.chunkSize.z; k++)
				{
					if (this.chunks[i, j, k].gameObject != null)
					{
						this.chunks[i, j, k].meshRenderer.sharedMaterial = m;
					}
				}
			}
		}
	}

	// Token: 0x060024DA RID: 9434 RVA: 0x000DFF78 File Offset: 0x000DE178
	public void OnDestroy()
	{
		if (this.chunks != null)
		{
			for (int i = 0; i < this.chunkSize.x; i++)
			{
				for (int j = 0; j < this.chunkSize.y; j++)
				{
					for (int k = 0; k < this.chunkSize.z; k++)
					{
						if (this.chunks[i, j, k].mesh != null)
						{
							UnityEngine.Object.Destroy(this.chunks[i, j, k].mesh);
						}
					}
				}
			}
		}
	}

	// Token: 0x04002840 RID: 10304
	public Vector3 size;

	// Token: 0x04002841 RID: 10305
	public TextAsset gridBinaryFile;

	// Token: 0x04002842 RID: 10306
	public Material chunkMat;

	// Token: 0x04002843 RID: 10307
	public Material wireFrameMat;

	// Token: 0x04002844 RID: 10308
	public PhysicMaterial physicMaterial;

	// Token: 0x04002845 RID: 10309
	public int layer;

	// Token: 0x04002846 RID: 10310
	public float voxelSize = 0.1f;

	// Token: 0x04002847 RID: 10311
	public int detailPerChunk = 20;

	// Token: 0x04002848 RID: 10312
	public bool buildOnStart;

	// Token: 0x04002849 RID: 10313
	public bool isStatic;

	// Token: 0x0400284A RID: 10314
	public Gradient colorGradient;

	// Token: 0x0400284B RID: 10315
	public ShadowCastingMode shadowCastingMode = ShadowCastingMode.On;

	// Token: 0x0400284C RID: 10316
	public Vector3 gridSize;

	// Token: 0x0400284D RID: 10317
	public Chunk[,,] chunks;

	// Token: 0x0400284F RID: 10319
	public ChunkBuilder[] builders;

	// Token: 0x04002850 RID: 10320
	public short[][][] data;

	// Token: 0x04002851 RID: 10321
	public Vector3i chunkSize;

	// Token: 0x04002852 RID: 10322
	public Vector3 position;

	// Token: 0x04002853 RID: 10323
	[HideInInspector]
	public bool UseNavMesh;

	// Token: 0x04002854 RID: 10324
	public bool buildCompleted;

	// Token: 0x04002855 RID: 10325
	private List<ChunkUpdate> chunkUpdates = new List<ChunkUpdate>();

	// Token: 0x04002856 RID: 10326
	private List<VoxelGrid.GridEdit> edits = new List<VoxelGrid.GridEdit>();

	// Token: 0x04002857 RID: 10327
	private bool buildStarted;

	// Token: 0x04002858 RID: 10328
	private List<ChunkUpdate> test = new List<ChunkUpdate>();

	// Token: 0x04002859 RID: 10329
	private VoxelGridRenderingMode renderMode;

	// Token: 0x02000584 RID: 1412
	private struct GridEdit
	{
		// Token: 0x060024DC RID: 9436 RVA: 0x0001A720 File Offset: 0x00018920
		public GridEdit(int x, int y, int z, short val)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.val = val;
		}

		// Token: 0x0400285A RID: 10330
		public int x;

		// Token: 0x0400285B RID: 10331
		public int y;

		// Token: 0x0400285C RID: 10332
		public int z;

		// Token: 0x0400285D RID: 10333
		public short val;
	}
}
