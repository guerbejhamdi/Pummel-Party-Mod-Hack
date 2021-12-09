using System;

namespace I2.Loc
{
	// Token: 0x020007EC RID: 2028
	public static class PersistentStorage
	{
		// Token: 0x06003998 RID: 14744 RVA: 0x00027192 File Offset: 0x00025392
		public static void SetSetting_String(string key, string value)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			PersistentStorage.mStorage.SetSetting_String(key, value);
		}

		// Token: 0x06003999 RID: 14745 RVA: 0x000271B1 File Offset: 0x000253B1
		public static string GetSetting_String(string key, string defaultValue)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.GetSetting_String(key, defaultValue);
		}

		// Token: 0x0600399A RID: 14746 RVA: 0x000271D0 File Offset: 0x000253D0
		public static void DeleteSetting(string key)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			PersistentStorage.mStorage.DeleteSetting(key);
		}

		// Token: 0x0600399B RID: 14747 RVA: 0x000271EE File Offset: 0x000253EE
		public static bool HasSetting(string key)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.HasSetting(key);
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x0002720C File Offset: 0x0002540C
		public static void ForceSaveSettings()
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			PersistentStorage.mStorage.ForceSaveSettings();
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x00027229 File Offset: 0x00025429
		public static bool CanAccessFiles()
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.CanAccessFiles();
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x00027246 File Offset: 0x00025446
		public static bool SaveFile(PersistentStorage.eFileType fileType, string fileName, string data, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.SaveFile(fileType, fileName, data, logExceptions);
		}

		// Token: 0x0600399F RID: 14751 RVA: 0x00027267 File Offset: 0x00025467
		public static string LoadFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.LoadFile(fileType, fileName, logExceptions);
		}

		// Token: 0x060039A0 RID: 14752 RVA: 0x00027287 File Offset: 0x00025487
		public static bool DeleteFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.DeleteFile(fileType, fileName, logExceptions);
		}

		// Token: 0x060039A1 RID: 14753 RVA: 0x000272A7 File Offset: 0x000254A7
		public static bool HasFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.HasFile(fileType, fileName, logExceptions);
		}

		// Token: 0x0400380D RID: 14349
		private static I2CustomPersistentStorage mStorage;

		// Token: 0x020007ED RID: 2029
		public enum eFileType
		{
			// Token: 0x0400380F RID: 14351
			Raw,
			// Token: 0x04003810 RID: 14352
			Persistent,
			// Token: 0x04003811 RID: 14353
			Temporal,
			// Token: 0x04003812 RID: 14354
			Streaming
		}
	}
}
