using System;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000867 RID: 2151
	[Serializable]
	public class PoolInstance
	{
		// Token: 0x06003CFA RID: 15610 RVA: 0x0013068C File Offset: 0x0012E88C
		public PoolInstance(string Title, PoolInstance[] CurrentInstances)
		{
			this.id = this.UniqueID(CurrentInstances);
			this.title = Title;
			this.limits = new int[15];
			for (int i = 0; i < this.limits.Length; i++)
			{
				this.limits[i] = (i + 1) * 400;
			}
		}

		// Token: 0x06003CFB RID: 15611 RVA: 0x001306E4 File Offset: 0x0012E8E4
		private int UniqueID(PoolInstance[] CurrentInstances)
		{
			int num = 0;
			bool flag = false;
			if (CurrentInstances != null)
			{
				while (!flag)
				{
					num++;
					flag = true;
					for (int i = 0; i < CurrentInstances.Length; i++)
					{
						if (CurrentInstances[i] != null && num == CurrentInstances[i].id)
						{
							flag = false;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x040039DB RID: 14811
		public int id;

		// Token: 0x040039DC RID: 14812
		public string title;

		// Token: 0x040039DD RID: 14813
		public int[] limits;
	}
}
