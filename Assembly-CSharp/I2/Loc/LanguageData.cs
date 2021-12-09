using System;

namespace I2.Loc
{
	// Token: 0x02000805 RID: 2053
	[Serializable]
	public class LanguageData
	{
		// Token: 0x06003A19 RID: 14873 RVA: 0x00027576 File Offset: 0x00025776
		public bool IsEnabled()
		{
			return (this.Flags & 1) == 0;
		}

		// Token: 0x06003A1A RID: 14874 RVA: 0x00027583 File Offset: 0x00025783
		public void SetEnabled(bool bEnabled)
		{
			if (bEnabled)
			{
				this.Flags = (byte)((int)this.Flags & -2);
				return;
			}
			this.Flags |= 1;
		}

		// Token: 0x06003A1B RID: 14875 RVA: 0x000275A8 File Offset: 0x000257A8
		public bool IsLoaded()
		{
			return (this.Flags & 4) == 0;
		}

		// Token: 0x06003A1C RID: 14876 RVA: 0x000275B5 File Offset: 0x000257B5
		public bool CanBeUnloaded()
		{
			return (this.Flags & 2) == 0;
		}

		// Token: 0x06003A1D RID: 14877 RVA: 0x000275C2 File Offset: 0x000257C2
		public void SetLoaded(bool loaded)
		{
			if (loaded)
			{
				this.Flags = (byte)((int)this.Flags & -5);
				return;
			}
			this.Flags |= 4;
		}

		// Token: 0x06003A1E RID: 14878 RVA: 0x000275E7 File Offset: 0x000257E7
		public void SetCanBeUnLoaded(bool allowUnloading)
		{
			if (allowUnloading)
			{
				this.Flags = (byte)((int)this.Flags & -3);
				return;
			}
			this.Flags |= 2;
		}

		// Token: 0x04003853 RID: 14419
		public string Name;

		// Token: 0x04003854 RID: 14420
		public string Code;

		// Token: 0x04003855 RID: 14421
		public byte Flags;

		// Token: 0x04003856 RID: 14422
		[NonSerialized]
		public bool Compressed;
	}
}
