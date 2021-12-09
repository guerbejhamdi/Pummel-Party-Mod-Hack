using System;
using System.Collections;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200083A RID: 2106
	public class CoroutineManager : MonoBehaviour
	{
		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x06003BD0 RID: 15312 RVA: 0x0012C82C File Offset: 0x0012AA2C
		private static CoroutineManager pInstance
		{
			get
			{
				if (CoroutineManager.mInstance == null)
				{
					GameObject gameObject = new GameObject("_Coroutiner");
					gameObject.hideFlags = HideFlags.HideAndDontSave;
					CoroutineManager.mInstance = gameObject.AddComponent<CoroutineManager>();
					if (Application.isPlaying)
					{
						UnityEngine.Object.DontDestroyOnLoad(gameObject);
					}
				}
				return CoroutineManager.mInstance;
			}
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x000281AB File Offset: 0x000263AB
		private void Awake()
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x000281BF File Offset: 0x000263BF
		public static Coroutine Start(IEnumerator coroutine)
		{
			return CoroutineManager.pInstance.StartCoroutine(coroutine);
		}

		// Token: 0x04003925 RID: 14629
		private static CoroutineManager mInstance;
	}
}
