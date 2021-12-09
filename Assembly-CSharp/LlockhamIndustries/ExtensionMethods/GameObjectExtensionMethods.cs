using System;
using System.Reflection;
using UnityEngine;

namespace LlockhamIndustries.ExtensionMethods
{
	// Token: 0x02000861 RID: 2145
	public static class GameObjectExtensionMethods
	{
		// Token: 0x06003CD9 RID: 15577 RVA: 0x000289A7 File Offset: 0x00026BA7
		public static T AddComponent<T>(this GameObject GameObject, T Source) where T : MonoBehaviour
		{
			return GameObject.AddComponent<T>().GetCopyOf(Source);
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x001301E8 File Offset: 0x0012E3E8
		public static MonoBehaviour AddComponent(this GameObject GameObject, MonoBehaviour Source)
		{
			Type type = Source.GetType();
			if (type.IsSubclassOf(typeof(MonoBehaviour)))
			{
				return ((MonoBehaviour)GameObject.AddComponent(type)).GetCopyOf(Source);
			}
			return null;
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x00130224 File Offset: 0x0012E424
		public static T GetCopyOf<T>(this MonoBehaviour Monobehaviour, T Source) where T : MonoBehaviour
		{
			Type type = Monobehaviour.GetType();
			if (type != Source.GetType())
			{
				return default(T);
			}
			BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			while (type != typeof(MonoBehaviour))
			{
				foreach (FieldInfo fieldInfo in type.GetFields(bindingAttr))
				{
					fieldInfo.SetValue(Monobehaviour, fieldInfo.GetValue(Source));
				}
				type = type.BaseType;
			}
			return Monobehaviour as T;
		}
	}
}
