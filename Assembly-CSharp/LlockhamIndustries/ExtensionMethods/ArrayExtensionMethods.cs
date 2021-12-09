using System;
using UnityEngine;

namespace LlockhamIndustries.ExtensionMethods
{
	// Token: 0x02000860 RID: 2144
	public static class ArrayExtensionMethods
	{
		// Token: 0x06003CD3 RID: 15571 RVA: 0x0012FFFC File Offset: 0x0012E1FC
		public static T[] Insert<T>(this T[] Array, T Item, int Index)
		{
			if (Item != null)
			{
				if (Array != null)
				{
					Index = Mathf.Clamp(Index, 0, Array.Length);
					T[] array = Array;
					Array = new T[Array.Length + 1];
					int num = 0;
					for (int i = 0; i < Array.Length; i++)
					{
						if (i != Index)
						{
							Array[i] = array[num];
							num++;
						}
						else
						{
							Array[i] = Item;
						}
					}
				}
				else
				{
					Array = new T[]
					{
						Item
					};
				}
			}
			return Array;
		}

		// Token: 0x06003CD4 RID: 15572 RVA: 0x0002897B File Offset: 0x00026B7B
		public static T[] Add<T>(this T[] Array, T Item)
		{
			if (Item != null)
			{
				if (Array != null)
				{
					Array = Array.Insert(Item, Array.Length);
				}
				else
				{
					Array = new T[]
					{
						Item
					};
				}
			}
			return Array;
		}

		// Token: 0x06003CD5 RID: 15573 RVA: 0x00130074 File Offset: 0x0012E274
		public static bool Contains<T>(this T[] Array, T Item)
		{
			if (Array != null && Item != null && Array.Length != 0)
			{
				for (int i = 0; i < Array.Length; i++)
				{
					if (Array[i].Equals(Item))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x001300C0 File Offset: 0x0012E2C0
		public static T[] Remove<T>(this T[] Array, T Item)
		{
			if (Array == null || Item == null || Array.Length == 0)
			{
				return Array;
			}
			T[] array = new T[Array.Length - 1];
			bool flag = false;
			for (int i = 0; i < Array.Length; i++)
			{
				if (!flag && Array[i] != null && Array[i].Equals(Item))
				{
					flag = true;
				}
				else
				{
					array[flag ? (i - 1) : i] = Array[i];
				}
			}
			if (!flag)
			{
				return Array;
			}
			return array;
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x00130148 File Offset: 0x0012E348
		public static T[] RemoveAt<T>(this T[] Array, int Index)
		{
			if (Array != null && Array.Length != 0)
			{
				if (Index >= 0 && Index < Array.Length)
				{
					T[] array = Array;
					Array = new T[Array.Length - 1];
					int num = 0;
					for (int i = 0; i < array.Length; i++)
					{
						if (i != Index)
						{
							Array[num] = array[i];
							num++;
						}
					}
				}
				else
				{
					Debug.LogError("Index out of Bounds");
				}
			}
			return Array;
		}

		// Token: 0x06003CD8 RID: 15576 RVA: 0x001301A8 File Offset: 0x0012E3A8
		public static T[] Resize<T>(this T[] Array, int Size)
		{
			if (Array != null)
			{
				T[] array = Array;
				Array = new T[Size];
				for (int i = 0; i < Mathf.Min(Array.Length, array.Length); i++)
				{
					Array[i] = array[i];
				}
			}
			return Array;
		}
	}
}
