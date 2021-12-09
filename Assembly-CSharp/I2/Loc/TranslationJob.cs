using System;

namespace I2.Loc
{
	// Token: 0x020007FC RID: 2044
	public class TranslationJob : IDisposable
	{
		// Token: 0x060039FF RID: 14847 RVA: 0x00027445 File Offset: 0x00025645
		public virtual TranslationJob.eJobState GetState()
		{
			return this.mJobState;
		}

		// Token: 0x06003A00 RID: 14848 RVA: 0x0000398C File Offset: 0x00001B8C
		public virtual void Dispose()
		{
		}

		// Token: 0x04003832 RID: 14386
		public TranslationJob.eJobState mJobState;

		// Token: 0x020007FD RID: 2045
		public enum eJobState
		{
			// Token: 0x04003834 RID: 14388
			Running,
			// Token: 0x04003835 RID: 14389
			Succeeded,
			// Token: 0x04003836 RID: 14390
			Failed
		}
	}
}
