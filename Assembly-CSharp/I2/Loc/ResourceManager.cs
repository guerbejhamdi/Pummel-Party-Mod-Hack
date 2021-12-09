using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
	// Token: 0x0200084E RID: 2126
	public class ResourceManager : MonoBehaviour
	{
		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06003C17 RID: 15383 RVA: 0x0012E544 File Offset: 0x0012C744
		public static ResourceManager pInstance
		{
			get
			{
				bool flag = ResourceManager.mInstance == null;
				if (ResourceManager.mInstance == null)
				{
					ResourceManager.mInstance = (ResourceManager)UnityEngine.Object.FindObjectOfType(typeof(ResourceManager));
				}
				if (ResourceManager.mInstance == null)
				{
					GameObject gameObject = new GameObject("I2ResourceManager", new Type[]
					{
						typeof(ResourceManager)
					});
					gameObject.hideFlags |= HideFlags.HideAndDontSave;
					ResourceManager.mInstance = gameObject.GetComponent<ResourceManager>();
					SceneManager.sceneLoaded += ResourceManager.MyOnLevelWasLoaded;
				}
				if (flag && Application.isPlaying)
				{
					UnityEngine.Object.DontDestroyOnLoad(ResourceManager.mInstance.gameObject);
				}
				return ResourceManager.mInstance;
			}
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x000283A1 File Offset: 0x000265A1
		public static void MyOnLevelWasLoaded(Scene scene, LoadSceneMode mode)
		{
			ResourceManager.pInstance.CleanResourceCache();
			LocalizationManager.UpdateSources();
		}

		// Token: 0x06003C19 RID: 15385 RVA: 0x0012E5F8 File Offset: 0x0012C7F8
		public T GetAsset<T>(string Name) where T : UnityEngine.Object
		{
			T t = this.FindAsset(Name) as T;
			if (t != null)
			{
				return t;
			}
			return this.LoadFromResources<T>(Name);
		}

		// Token: 0x06003C1A RID: 15386 RVA: 0x0012E630 File Offset: 0x0012C830
		private UnityEngine.Object FindAsset(string Name)
		{
			if (this.Assets != null)
			{
				int i = 0;
				int num = this.Assets.Length;
				while (i < num)
				{
					if (this.Assets[i] != null && this.Assets[i].name == Name)
					{
						return this.Assets[i];
					}
					i++;
				}
			}
			return null;
		}

		// Token: 0x06003C1B RID: 15387 RVA: 0x000283B3 File Offset: 0x000265B3
		public bool HasAsset(UnityEngine.Object Obj)
		{
			return this.Assets != null && Array.IndexOf<UnityEngine.Object>(this.Assets, Obj) >= 0;
		}

		// Token: 0x06003C1C RID: 15388 RVA: 0x0012E68C File Offset: 0x0012C88C
		public T LoadFromResources<T>(string Path) where T : UnityEngine.Object
		{
			T t;
			try
			{
				UnityEngine.Object @object;
				if (string.IsNullOrEmpty(Path))
				{
					t = default(T);
					t = t;
				}
				else if (this.mResourcesCache.TryGetValue(Path, out @object) && @object != null)
				{
					t = (@object as T);
				}
				else
				{
					T t2 = default(T);
					if (Path.EndsWith("]", StringComparison.OrdinalIgnoreCase))
					{
						int num = Path.LastIndexOf("[", StringComparison.OrdinalIgnoreCase);
						int length = Path.Length - num - 2;
						string value = Path.Substring(num + 1, length);
						Path = Path.Substring(0, num);
						T[] array = Resources.LoadAll<T>(Path);
						int i = 0;
						int num2 = array.Length;
						while (i < num2)
						{
							if (array[i].name.Equals(value))
							{
								t2 = array[i];
								break;
							}
							i++;
						}
					}
					else
					{
						t2 = (Resources.Load(Path, typeof(T)) as T);
					}
					if (t2 == null)
					{
						t2 = this.LoadFromBundle<T>(Path);
					}
					if (t2 != null)
					{
						this.mResourcesCache[Path] = t2;
					}
					t = t2;
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Unable to load {0} '{1}'\nERROR: {2}", new object[]
				{
					typeof(T),
					Path,
					ex.ToString()
				});
				t = default(T);
			}
			return t;
		}

		// Token: 0x06003C1D RID: 15389 RVA: 0x0012E818 File Offset: 0x0012CA18
		public T LoadFromBundle<T>(string path) where T : UnityEngine.Object
		{
			int i = 0;
			int count = this.mBundleManagers.Count;
			while (i < count)
			{
				if (this.mBundleManagers[i] != null)
				{
					T t = this.mBundleManagers[i].LoadFromBundle(path, typeof(T)) as T;
					if (t != null)
					{
						return t;
					}
				}
				i++;
			}
			return default(T);
		}

		// Token: 0x06003C1E RID: 15390 RVA: 0x000283D1 File Offset: 0x000265D1
		public void CleanResourceCache()
		{
			this.mResourcesCache.Clear();
			Resources.UnloadUnusedAssets();
			base.CancelInvoke();
		}

		// Token: 0x04003993 RID: 14739
		private static ResourceManager mInstance;

		// Token: 0x04003994 RID: 14740
		public List<IResourceManager_Bundles> mBundleManagers = new List<IResourceManager_Bundles>();

		// Token: 0x04003995 RID: 14741
		public UnityEngine.Object[] Assets;

		// Token: 0x04003996 RID: 14742
		private readonly Dictionary<string, UnityEngine.Object> mResourcesCache = new Dictionary<string, UnityEngine.Object>(StringComparer.Ordinal);
	}
}
