using System;

namespace ZP.Utility
{
	// Token: 0x020005ED RID: 1517
	public class Pair<T, U>
	{
		// Token: 0x060026EB RID: 9963 RVA: 0x00004023 File Offset: 0x00002223
		public Pair()
		{
		}

		// Token: 0x060026EC RID: 9964 RVA: 0x0001BB3D File Offset: 0x00019D3D
		public Pair(T first, U second)
		{
			this.First = first;
			this.Second = second;
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060026ED RID: 9965 RVA: 0x0001BB53 File Offset: 0x00019D53
		// (set) Token: 0x060026EE RID: 9966 RVA: 0x0001BB5B File Offset: 0x00019D5B
		public T First { get; set; }

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060026EF RID: 9967 RVA: 0x0001BB64 File Offset: 0x00019D64
		// (set) Token: 0x060026F0 RID: 9968 RVA: 0x0001BB6C File Offset: 0x00019D6C
		public U Second { get; set; }
	}
}
