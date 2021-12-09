using System;
using System.Collections.Generic;

// Token: 0x02000493 RID: 1171
public static class ListExtensions
{
	// Token: 0x06001F6C RID: 8044 RVA: 0x000170C6 File Offset: 0x000152C6
	public static void Shuffle<T>(this IList<T> list, int seed)
	{
		ListExtensions.rng = new Random(seed);
		list.Shuffle<T>();
	}

	// Token: 0x06001F6D RID: 8045 RVA: 0x000170D9 File Offset: 0x000152D9
	public static void Shuffle<T>(this IList<T> list)
	{
		list.DoShuffle<T>();
	}

	// Token: 0x06001F6E RID: 8046 RVA: 0x000C7D70 File Offset: 0x000C5F70
	private static void DoShuffle<T>(this IList<T> list)
	{
		int i = list.Count;
		while (i > 1)
		{
			i--;
			int index = ListExtensions.rng.Next(i + 1);
			T value = list[index];
			list[index] = list[i];
			list[i] = value;
		}
	}

	// Token: 0x0400224D RID: 8781
	private static Random rng = new Random();
}
