using System;
using System.Runtime.InteropServices;

namespace ZP.Utility
{
	// Token: 0x020005EA RID: 1514
	public class ArrayUtility
	{
		// Token: 0x060026B9 RID: 9913 RVA: 0x000E9E60 File Offset: 0x000E8060
		public static T[][][] unflatten_byte_array_3d<T>(byte[] arr, int width, int height, int depth)
		{
			int num = Marshal.SizeOf(typeof(T));
			T[][][] array = ArrayUtility.allocate_3d_array<T>(width, height, depth);
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					Buffer.BlockCopy(arr, (i * (height * depth) + j * depth) * num, array[i][j], 0, depth * num);
				}
			}
			return array;
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x000E9EBC File Offset: 0x000E80BC
		public static byte[] flatten_to_byte_array_3d<T>(T[][][] arr, int width, int height, int depth)
		{
			int num = Marshal.SizeOf(typeof(T));
			byte[] array = new byte[width * height * depth * num];
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					Buffer.BlockCopy(arr[i][j], 0, array, (i * (height * depth) + j * depth) * num, depth * num);
				}
			}
			return array;
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x000E9F1C File Offset: 0x000E811C
		public static T[][][] copy_3d_array<T>(T[][][] arr, int width, int height, int depth)
		{
			T[][][] array = ArrayUtility.allocate_3d_array<T>(width, height, depth);
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					for (int k = 0; k < depth; k++)
					{
						array[i][j][k] = arr[i][j][k];
					}
				}
			}
			return array;
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x000E9F70 File Offset: 0x000E8170
		public static T[][][][] allocate_4d_array<T>(int width, int height, int depth, int size)
		{
			T[][][][] array = new T[width][][][];
			for (int i = 0; i < width; i++)
			{
				array[i] = new T[height][][];
				for (int j = 0; j < height; j++)
				{
					array[i][j] = new T[depth][];
					for (int k = 0; k < depth; k++)
					{
						array[i][j][k] = new T[size];
					}
				}
			}
			return array;
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x000E9FCC File Offset: 0x000E81CC
		public static T[][][] allocate_3d_array<T>(int width, int height, int depth)
		{
			T[][][] array = new T[width][][];
			for (int i = 0; i < width; i++)
			{
				array[i] = new T[height][];
				for (int j = 0; j < height; j++)
				{
					array[i][j] = new T[depth];
				}
			}
			return array;
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x000EA010 File Offset: 0x000E8210
		public static T[][] allocate_2d_array<T>(int width, int height)
		{
			T[][] array = new T[width][];
			for (int i = 0; i < width; i++)
			{
				array[i] = new T[height];
			}
			return array;
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x000EA03C File Offset: 0x000E823C
		public static T[][][] allocate_3d_array<T>(int width, int height, int depth, T val)
		{
			T[][][] array = ArrayUtility.allocate_3d_array<T>(width, height, depth);
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					for (int k = 0; k < depth; k++)
					{
						array[i][j][k] = val;
					}
				}
			}
			return array;
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x000EA084 File Offset: 0x000E8284
		public static T[] flatten_3d_array<T>(T[][][] original, int width, int height, int depth)
		{
			T[] array = new T[width * height * depth];
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					for (int k = 0; k < depth; k++)
					{
						array[i + width * (j + height * k)] = original[i][j][k];
					}
				}
			}
			return array;
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x000EA0DC File Offset: 0x000E82DC
		public static T[][][] unflatten_1d_array<T>(T[] flat, T[][][] arr_3d, int width, int height, int depth)
		{
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					for (int k = 0; k < depth; k++)
					{
						arr_3d[i][j][k] = flat[i + width * (j + height * k)];
					}
				}
			}
			return arr_3d;
		}
	}
}
