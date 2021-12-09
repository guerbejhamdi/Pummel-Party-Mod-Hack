using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000889 RID: 2185
	public class ModifierManager : MonoBehaviour
	{
		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06003E41 RID: 15937 RVA: 0x00029E16 File Offset: 0x00028016
		public static bool Initialized
		{
			get
			{
				return ModifierManager.singleton != null;
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06003E42 RID: 15938 RVA: 0x00029E23 File Offset: 0x00028023
		public static ModifierManager Singleton
		{
			get
			{
				if (ModifierManager.singleton == null)
				{
					new GameObject("Dynamic Decals")
					{
						hideFlags = (HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild)
					}.AddComponent<ModifierManager>();
				}
				return ModifierManager.singleton;
			}
		}

		// Token: 0x06003E43 RID: 15939 RVA: 0x000281AB File Offset: 0x000263AB
		private void Start()
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		// Token: 0x06003E44 RID: 15940 RVA: 0x0013304C File Offset: 0x0013124C
		private void OnEnable()
		{
			if (ModifierManager.singleton == null)
			{
				ModifierManager.singleton = this;
			}
			else if (ModifierManager.singleton != this)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(base.gameObject);
					return;
				}
				UnityEngine.Object.DestroyImmediate(base.gameObject, true);
				return;
			}
			this.perFrameModifiers = new List<Modifier>();
			this.tenModifiers = new List<Modifier>();
			this.oneModifiers = new List<Modifier>();
			base.StartCoroutine(this.TenTimesPerSecond());
			base.StartCoroutine(this.OncePerSecond());
		}

		// Token: 0x06003E45 RID: 15941 RVA: 0x00029E4F File Offset: 0x0002804F
		private void OnDisable()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x06003E46 RID: 15942 RVA: 0x00029E57 File Offset: 0x00028057
		private static List<Modifier> GetModifiers(Frequency p_Frequency)
		{
			switch (p_Frequency)
			{
			case Frequency.PerFrame:
				return ModifierManager.Singleton.perFrameModifiers;
			case Frequency.TenPerSec:
				return ModifierManager.Singleton.tenModifiers;
			case Frequency.OncePerSec:
				return ModifierManager.Singleton.oneModifiers;
			default:
				return null;
			}
		}

		// Token: 0x06003E47 RID: 15943 RVA: 0x001330D8 File Offset: 0x001312D8
		public static void Register(Modifier p_Modifier)
		{
			List<Modifier> modifiers = ModifierManager.GetModifiers(p_Modifier.Frequency);
			if (!modifiers.Contains(p_Modifier))
			{
				modifiers.Add(p_Modifier);
			}
		}

		// Token: 0x06003E48 RID: 15944 RVA: 0x00029E8F File Offset: 0x0002808F
		public static void Deregister(Modifier p_Modifier)
		{
			ModifierManager.GetModifiers(p_Modifier.Frequency).Remove(p_Modifier);
		}

		// Token: 0x06003E49 RID: 15945 RVA: 0x00133104 File Offset: 0x00131304
		private void Update()
		{
			for (int i = 0; i < this.perFrameModifiers.Count; i++)
			{
				this.perFrameModifiers[i].Perform();
			}
		}

		// Token: 0x06003E4A RID: 15946 RVA: 0x00029EA3 File Offset: 0x000280A3
		private IEnumerator TenTimesPerSecond()
		{
			for (;;)
			{
				for (int i = 0; i < this.tenModifiers.Count; i++)
				{
					this.tenModifiers[i].Perform();
				}
				yield return this.tenthOfASecond;
			}
			yield break;
		}

		// Token: 0x06003E4B RID: 15947 RVA: 0x00029EB2 File Offset: 0x000280B2
		private IEnumerator OncePerSecond()
		{
			for (;;)
			{
				for (int i = 0; i < this.oneModifiers.Count; i++)
				{
					this.oneModifiers[i].Perform();
				}
				yield return this.second;
			}
			yield break;
		}

		// Token: 0x04003A77 RID: 14967
		private static ModifierManager singleton;

		// Token: 0x04003A78 RID: 14968
		private List<Modifier> perFrameModifiers;

		// Token: 0x04003A79 RID: 14969
		private List<Modifier> tenModifiers;

		// Token: 0x04003A7A RID: 14970
		private List<Modifier> oneModifiers;

		// Token: 0x04003A7B RID: 14971
		private WaitForSeconds tenthOfASecond = new WaitForSeconds(0.1f);

		// Token: 0x04003A7C RID: 14972
		private WaitForSeconds second = new WaitForSeconds(1f);
	}
}
