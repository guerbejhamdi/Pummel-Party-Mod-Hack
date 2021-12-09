using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020007EA RID: 2026
	public class RegisterBundlesManager : MonoBehaviour, IResourceManager_Bundles
	{
		// Token: 0x06003991 RID: 14737 RVA: 0x00027149 File Offset: 0x00025349
		public void OnEnable()
		{
			if (!ResourceManager.pInstance.mBundleManagers.Contains(this))
			{
				ResourceManager.pInstance.mBundleManagers.Add(this);
			}
		}

		// Token: 0x06003992 RID: 14738 RVA: 0x0002716D File Offset: 0x0002536D
		public void OnDisable()
		{
			ResourceManager.pInstance.mBundleManagers.Remove(this);
		}

		// Token: 0x06003993 RID: 14739 RVA: 0x000053AE File Offset: 0x000035AE
		public virtual UnityEngine.Object LoadFromBundle(string path, Type assetType)
		{
			return null;
		}
	}
}
