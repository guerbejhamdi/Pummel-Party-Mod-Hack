using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Utility;

// Token: 0x0200057C RID: 1404
public class ChunkBuilder
{
	// Token: 0x060024A8 RID: 9384 RVA: 0x000DBE48 File Offset: 0x000DA048
	public ChunkBuilder(VoxelGrid voxelGrid)
	{
		this.voxelGrid = voxelGrid;
		this.size = voxelGrid.detailPerChunk;
		this.gridSize = this.size + 3;
		this.reuseGridSize = this.size + 1;
		this.voxels = ArrayUtility.allocate_3d_array<short>(this.gridSize, this.gridSize, this.gridSize);
		this.reuseGrid = ArrayUtility.allocate_4d_array<int>(this.reuseGridSize, this.reuseGridSize, this.reuseGridSize, 4);
	}

	// Token: 0x060024A9 RID: 9385 RVA: 0x000DBFF8 File Offset: 0x000DA1F8
	public void BuildChunk(Chunk c)
	{
		c.building = true;
		int num = (int)c.voxelPosition.x;
		int num2 = (int)c.voxelPosition.y;
		Vector3 voxelPosition = c.voxelPosition;
		short[][][] data = this.voxelGrid.data;
		lock (data)
		{
			for (int i = 0; i < this.gridSize; i++)
			{
				int num3 = i + num;
				for (int j = 0; j < this.gridSize; j++)
				{
					int num4 = j + num2;
					Array.Copy(this.voxelGrid.data[num3][num4], (int)c.voxelPosition.z, this.voxels[i][j], 0, this.gridSize);
				}
			}
		}
		this.tempIndexer.Clear();
		this.tempVertices.Clear();
		this.tempNormals.Clear();
		this.tempIndices.Clear();
		this.tempColors.Clear();
		byte b = 0;
		Vector3 a = Vector3.zero;
		for (int k = 1; k <= this.size; k++)
		{
			for (int l = 1; l <= this.size; l++)
			{
				bool flag2 = true;
				for (int m = 1; m <= this.size; m++)
				{
					this.corner[0] = this.voxels[m][l][k];
					this.corner[1] = this.voxels[m + 1][l][k];
					this.corner[2] = this.voxels[m][l + 1][k];
					this.corner[3] = this.voxels[m + 1][l + 1][k];
					this.corner[4] = this.voxels[m][l][k + 1];
					this.corner[5] = this.voxels[m + 1][l][k + 1];
					this.corner[6] = this.voxels[m][l + 1][k + 1];
					this.corner[7] = this.voxels[m + 1][l + 1][k + 1];
					uint num5 = (uint)((this.corner[0] >> 15 & 1) | (this.corner[1] >> 14 & 2) | (this.corner[2] >> 13 & 4) | (this.corner[3] >> 12 & 8) | (this.corner[4] >> 11 & 16) | (this.corner[5] >> 10 & 32) | (this.corner[6] >> 9 & 64) | (this.corner[7] >> 8 & 128));
					if (((ulong)num5 ^ (ulong)((long)(this.corner[7] >> 15 & 65535))) != 0UL)
					{
						if (flag2)
						{
							this.cornerNormal[0].x = (float)(this.corner[1] - this.voxels[m - 1][l][k]) * 0.5f;
							this.cornerNormal[0].y = (float)(this.corner[2] - this.voxels[m][l - 1][k]) * 0.5f;
							this.cornerNormal[0].z = (float)(this.corner[4] - this.voxels[m][l][k - 1]) * 0.5f;
							this.cornerNormal[0].Normalize();
							this.cornerNormal[2].x = (float)(this.corner[3] - this.voxels[m - 1][l + 1][k]) * 0.5f;
							this.cornerNormal[2].y = (float)(this.voxels[m][l + 2][k] - this.corner[0]) * 0.5f;
							this.cornerNormal[2].z = (float)(this.corner[6] - this.voxels[m][l + 1][k - 1]) * 0.5f;
							this.cornerNormal[2].Normalize();
							this.cornerNormal[4].x = (float)(this.corner[5] - this.voxels[m - 1][l][k + 1]) * 0.5f;
							this.cornerNormal[4].y = (float)(this.corner[6] - this.voxels[m][l - 1][k + 1]) * 0.5f;
							this.cornerNormal[4].z = (float)(this.voxels[m][l][k + 2] - this.corner[0]) * 0.5f;
							this.cornerNormal[4].Normalize();
							this.cornerNormal[6].x = (float)(this.corner[7] - this.voxels[m - 1][l + 1][k + 1]) * 0.5f;
							this.cornerNormal[6].y = (float)(this.voxels[m][l + 2][k + 1] - this.corner[4]) * 0.5f;
							this.cornerNormal[6].z = (float)(this.voxels[m][l + 1][k + 2] - this.corner[2]) * 0.5f;
							this.cornerNormal[6].Normalize();
							flag2 = false;
						}
						else
						{
							this.cornerNormal[0] = this.cornerNormal[1];
							this.cornerNormal[2] = this.cornerNormal[3];
							this.cornerNormal[4] = this.cornerNormal[5];
							this.cornerNormal[6] = this.cornerNormal[7];
						}
						this.cornerNormal[1].x = (float)(this.voxels[m + 2][l][k] - this.corner[0]) * 0.5f;
						this.cornerNormal[1].y = (float)(this.corner[3] - this.voxels[m + 1][l - 1][k]) * 0.5f;
						this.cornerNormal[1].z = (float)(this.corner[5] - this.voxels[m + 1][l][k - 1]) * 0.5f;
						this.cornerNormal[1].Normalize();
						this.cornerNormal[3].x = (float)(this.voxels[m + 2][l + 1][k] - this.corner[2]) * 0.5f;
						this.cornerNormal[3].y = (float)(this.voxels[m + 1][l + 2][k] - this.corner[1]) * 0.5f;
						this.cornerNormal[3].z = (float)(this.corner[7] - this.voxels[m + 1][l + 1][k - 1]) * 0.5f;
						this.cornerNormal[3].Normalize();
						this.cornerNormal[5].x = (float)(this.voxels[m + 2][l][k + 1] - this.corner[4]) * 0.5f;
						this.cornerNormal[5].y = (float)(this.corner[7] - this.voxels[m + 1][l - 1][k + 1]) * 0.5f;
						this.cornerNormal[5].z = (float)(this.voxels[m + 1][l][k + 2] - this.corner[1]) * 0.5f;
						this.cornerNormal[5].Normalize();
						this.cornerNormal[7].x = (float)(this.voxels[m + 2][l + 1][k + 1] - this.corner[6]) * 0.5f;
						this.cornerNormal[7].y = (float)(this.voxels[m + 1][l + 2][k + 1] - this.corner[5]) * 0.5f;
						this.cornerNormal[7].z = (float)(this.voxels[m + 1][l + 1][k + 2] - this.corner[3]) * 0.5f;
						this.cornerNormal[7].Normalize();
						byte b2 = Voxel.regularCellClass[(int)num5];
						RegularCellData regularCellData = Voxel.regularCellData[(int)b2];
						long vertex_count = regularCellData.vertex_count;
						long triangle_count = regularCellData.triangle_count;
						Vector3 b3 = new Vector3((float)(m - 1), (float)(l - 1), (float)(k - 1));
						int num6 = 0;
						while ((long)num6 < vertex_count)
						{
							ushort num7 = Voxel.regularVertexData[(int)num5][num6];
							byte b4 = (byte)(num7 >> 8 & 255);
							byte b5 = (byte)(num7 & 255);
							byte b6 = (byte)(b4 >> 4 & 15);
							byte b7 = b4 & 15;
							ushort num8 = (ushort)(b5 >> 4 & 15);
							ushort num9 = (ushort)(b5 & 15);
							long num10 = (long)this.corner[(int)num8];
							long num11 = (long)this.corner[(int)num9];
							long num12 = (num11 << 16) / (num11 - num10);
							int num13 = -1;
							if (num9 != 7 && (b6 & b) == b6)
							{
								int num14 = (int)(b6 & 1);
								int num15 = b6 >> 1 & 1;
								int num16 = b6 >> 2 & 1;
								num13 = this.reuseGrid[m - num14][l - num15][k - num16][(int)b7];
							}
							if (num13 == -1)
							{
								num13 = this.tempVertices.Count;
								long num17 = 65536L - num12;
								float d = 1.5258789E-05f;
								double num18 = (double)((float)num12) / 65536.0;
								double num19 = (double)((float)num17) / 65536.0;
								a = (float)num12 * this.cornerNormalPosition[(int)num8] + (float)num17 * this.cornerNormalPosition[(int)num9];
								a *= d;
								this.tempVertices.Add((a + b3) * this.voxelGrid.voxelSize + c.localPosition);
								this.tempColors.Add(Color.clear);
								this.tempNormals.Add((float)num18 * this.cornerNormal[(int)num8] + (float)num19 * this.cornerNormal[(int)num9]);
							}
							if ((b6 & 8) != 0)
							{
								this.reuseGrid[m][l][k][(int)b7] = this.tempVertices.Count - 1;
							}
							this.tempIndexer.Add(num13);
							num6++;
						}
						int num20 = 0;
						while ((long)num20 < triangle_count * 3L)
						{
							int[] array = new int[]
							{
								this.tempIndexer[(int)regularCellData.vertexIndex[num20]],
								this.tempIndexer[(int)regularCellData.vertexIndex[num20 + 1]],
								this.tempIndexer[(int)regularCellData.vertexIndex[num20 + 2]]
							};
							Vector3 b8 = this.tempVertices[array[0]];
							Vector3 a2 = this.tempVertices[array[1]];
							Vector3 a3 = this.tempVertices[array[2]];
							float time = (Vector3.Normalize(Vector3.Cross(a2 - b8, a3 - b8)).y * -1f + 1f) / 2f;
							Color32 value = this.voxelGrid.colorGradient.Evaluate(time);
							for (int n = 0; n < 3; n++)
							{
								this.tempColors[array[n]] = value;
							}
							num20 += 3;
						}
						int num21 = 0;
						while ((long)num21 < triangle_count * 3L)
						{
							this.tempIndices.Add(this.tempIndexer[(int)regularCellData.vertexIndex[num21]]);
							num21++;
						}
						this.tempIndexer.Clear();
					}
					else
					{
						flag2 = true;
					}
					b |= 1;
				}
				b &= 14;
				b |= 2;
			}
			b &= 13;
			b |= 4;
		}
		Vector3[] array2 = new Vector3[this.tempIndices.Count];
		int[] array3 = new int[this.tempIndices.Count];
		Color32[] array4 = new Color32[this.tempIndices.Count];
		for (int num22 = 0; num22 < this.tempIndices.Count; num22++)
		{
			array2[num22] = this.tempVertices[this.tempIndices[num22]];
			array4[num22] = this.tempColors[this.tempIndices[num22]];
			array3[num22] = num22;
		}
		ChunkUpdate chunkUpdate = new ChunkUpdate(c, this.tempVertices.ToArray(), this.tempNormals.ToArray(), this.tempIndices.ToArray(), this.tempColors.ToArray());
		c.lastUpdate = chunkUpdate;
		c.building = false;
		this.voxelGrid.AddChunkUpdates(chunkUpdate);
	}

	// Token: 0x0400281E RID: 10270
	private readonly Vector3[] cornerNormalPosition = new Vector3[]
	{
		new Vector3(0f, 0f, 0f),
		new Vector3(1f, 0f, 0f),
		new Vector3(0f, 1f, 0f),
		new Vector3(1f, 1f, 0f),
		new Vector3(0f, 0f, 1f),
		new Vector3(1f, 0f, 1f),
		new Vector3(0f, 1f, 1f),
		new Vector3(1f, 1f, 1f)
	};

	// Token: 0x0400281F RID: 10271
	private const int reuseGridBase = -1;

	// Token: 0x04002820 RID: 10272
	private Vector3[] cornerNormal = new Vector3[8];

	// Token: 0x04002821 RID: 10273
	private short[] corner = new short[8];

	// Token: 0x04002822 RID: 10274
	private List<int> tempIndexer = new List<int>();

	// Token: 0x04002823 RID: 10275
	private List<int> tempIndices = new List<int>();

	// Token: 0x04002824 RID: 10276
	private List<Vector3> tempVertices = new List<Vector3>();

	// Token: 0x04002825 RID: 10277
	private List<Vector3> tempNormals = new List<Vector3>();

	// Token: 0x04002826 RID: 10278
	private List<Color32> tempColors = new List<Color32>();

	// Token: 0x04002827 RID: 10279
	private short[][][] voxels;

	// Token: 0x04002828 RID: 10280
	private int[][][][] reuseGrid;

	// Token: 0x04002829 RID: 10281
	private int size;

	// Token: 0x0400282A RID: 10282
	private int gridSize;

	// Token: 0x0400282B RID: 10283
	private int reuseGridSize;

	// Token: 0x0400282C RID: 10284
	private VoxelGrid voxelGrid;
}
