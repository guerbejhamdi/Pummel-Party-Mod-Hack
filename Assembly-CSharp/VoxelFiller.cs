using System;
using SimplexNoise;
using UnityEngine;
using ZP.Utility;

// Token: 0x02000580 RID: 1408
public static class VoxelFiller
{
	// Token: 0x060024AE RID: 9390 RVA: 0x0001A572 File Offset: 0x00018772
	public static short[][][] FillCylinder(int width, int height, int depth)
	{
		short[][][] array = ArrayUtility.allocate_3d_array<short>(width, height, depth);
		VoxelFiller.FillCylinder(array);
		return array;
	}

	// Token: 0x060024AF RID: 9391 RVA: 0x000DE5D8 File Offset: 0x000DC7D8
	public static void FillCylinder(short[][][] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			for (int j = 0; j < data[i].Length; j++)
			{
				for (int k = 0; k < data[i][j].Length; k++)
				{
					int num = 0;
					int num2 = 3;
					if (j >= data[i].Length - (2 + num) || j <= 1 + num2)
					{
						float num3 = Vector2.Distance(new Vector2((float)i, (float)k), (new Vector2((float)data.Length, (float)data[i][j].Length) - Vector2.one) / 2f);
						float num4 = (float)(1 + num2 - j);
						num4 = 0f;
						num3 /= ((float)data.Length - 4f) / 2f;
						data[i][j][k] = (short)(-32768f + 32777f * num3);
						if (data[i][j][k] < 0)
						{
							data[i][j][k] = (short)Mathf.Clamp((float)(-(float)data[i][j][k]) + 9000f * num4, -32768f, 32767f);
						}
					}
					else
					{
						float num5 = Vector2.Distance(new Vector2((float)i, (float)k), (new Vector2((float)data.Length, (float)data[i][j].Length) - Vector2.one) / 2f);
						num5 /= ((float)data.Length - 4f) / 2f;
						data[i][j][k] = (short)(-32768f + 32777f * num5);
					}
				}
			}
		}
	}

	// Token: 0x060024B0 RID: 9392 RVA: 0x0001A582 File Offset: 0x00018782
	public static short[][][] FillGrid(int width, int height, int depth)
	{
		short[][][] array = ArrayUtility.allocate_3d_array<short>(width, height, depth);
		VoxelFiller.FillGrid(array);
		return array;
	}

	// Token: 0x060024B1 RID: 9393 RVA: 0x000DE74C File Offset: 0x000DC94C
	public static void FillGrid(short[][][] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			for (int j = 0; j < data[i].Length; j++)
			{
				for (int k = 0; k < data[i][j].Length; k++)
				{
					if (i <= 1 || j <= 1 || k <= 1 || i >= data.Length - 2 || j >= data[i].Length - 2 || k >= data[i][j].Length - 2)
					{
						data[i][j][k] = short.MaxValue;
					}
					else
					{
						data[i][j][k] = short.MinValue;
					}
				}
			}
		}
	}

	// Token: 0x060024B2 RID: 9394 RVA: 0x000DE7D0 File Offset: 0x000DC9D0
	public static short[][][] FillToY(int width, int height, int depth, float y_point)
	{
		short[][][] array = ArrayUtility.allocate_3d_array<short>(width, height, depth);
		VoxelFiller.FillToY(y_point, array);
		return array;
	}

	// Token: 0x060024B3 RID: 9395 RVA: 0x000DE7F0 File Offset: 0x000DC9F0
	public static void FillToY(float y_point, short[][][] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			for (int j = 0; j < data[i].Length; j++)
			{
				for (int k = 0; k < data[i][j].Length; k++)
				{
					if ((float)j > (float)data[i].Length * y_point)
					{
						data[i][j][k] = short.MaxValue;
					}
					else
					{
						data[i][j][k] = short.MinValue;
					}
				}
			}
		}
	}

	// Token: 0x060024B4 RID: 9396 RVA: 0x000DE854 File Offset: 0x000DCA54
	public static short[][][] PerlinNoise(int width, int height, int depth, float scale)
	{
		short[][][] array = ArrayUtility.allocate_3d_array<short>(width, height, depth);
		VoxelFiller.PerlinNoise(scale, array);
		return array;
	}

	// Token: 0x060024B5 RID: 9397 RVA: 0x000DE874 File Offset: 0x000DCA74
	public static void PerlinNoise(float scale, short[][][] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			for (int j = 0; j < data[i].Length; j++)
			{
				for (int k = 0; k < data[i][j].Length; k++)
				{
					float num = Noise.Generate((float)i * scale, (float)j * scale, (float)k * scale);
					data[i][j][k] = (short)(32767f * num);
				}
			}
		}
	}

	// Token: 0x060024B6 RID: 9398 RVA: 0x000DE8D4 File Offset: 0x000DCAD4
	public static float Torus(Vector3 p, Vector2 t)
	{
		Vector2 vector = new Vector2(p.x, p.z);
		Vector2 vector2 = new Vector2(vector.magnitude - t.x, p.y);
		return vector2.magnitude - t.y;
	}

	// Token: 0x060024B7 RID: 9399 RVA: 0x000DE920 File Offset: 0x000DCB20
	public static float Box(Vector3 p, Vector3 b)
	{
		return Vector3.Max(new Vector3(Mathf.Abs(p.x), Mathf.Abs(p.y), Mathf.Abs(p.z)) - b, Vector3.zero).magnitude;
	}

	// Token: 0x060024B8 RID: 9400 RVA: 0x000DE96C File Offset: 0x000DCB6C
	public static void Ring(float radius, float ringSize, float noiseScale, short[][][] data)
	{
		Debug.Log("Radius: " + radius.ToString() + " Radius2: " + ringSize.ToString());
		for (int i = 0; i < data.Length; i++)
		{
			for (int j = 0; j < data[i].Length; j++)
			{
				for (int k = 0; k < data[i][j].Length; k++)
				{
					Vector3 b = (new Vector3((float)data.Length, (float)data[i].Length, (float)data[i][j].Length) - Vector3.one) / 2f;
					Vector3 vector = new Vector3((float)i, (float)j, (float)k) - b;
					vector = new Vector3(vector.x / 2f, vector.y, vector.z / 2f);
					float num = Mathf.Clamp(VoxelFiller.Torus(vector, new Vector2(radius, ringSize)), -1f, 1f);
					float num2 = Noise.Generate((float)i * noiseScale, (float)j * noiseScale, (float)k * noiseScale);
					float num3 = Noise.Generate((float)i * (noiseScale / 3f), (float)j * (noiseScale / 3f), (float)k * (noiseScale / 3f));
					num2 = num2 * 0.5f + num3 * 0.5f;
					num = num * 0.55f + num2 * 0.45f;
					data[i][j][k] = (short)(32767f * num);
				}
			}
		}
	}
}
