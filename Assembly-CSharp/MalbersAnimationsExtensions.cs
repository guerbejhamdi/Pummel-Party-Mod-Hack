using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

// Token: 0x0200012E RID: 302
public static class MalbersAnimationsExtensions
{
	// Token: 0x060008BD RID: 2237 RVA: 0x0005067C File Offset: 0x0004E87C
	public static Transform FindGrandChild(this Transform aParent, string aName)
	{
		Transform transform = aParent.Find(aName);
		if (transform != null)
		{
			return transform;
		}
		foreach (object obj in aParent)
		{
			transform = ((Transform)obj).FindGrandChild(aName);
			if (transform != null)
			{
				return transform;
			}
		}
		return null;
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x000506F4 File Offset: 0x0004E8F4
	public static Vector3 DeltaPositionFromRotate(this Transform transform, Vector3 point, Vector3 axis, float deltaAngle)
	{
		Vector3 vector = transform.position;
		Vector3 vector2 = vector - point;
		vector2 = Quaternion.AngleAxis(deltaAngle, axis) * vector2;
		vector = point + vector2 - vector;
		vector.y = 0f;
		return vector;
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x0005073C File Offset: 0x0004E93C
	public static void InvokeWithParams(this MonoBehaviour sender, string method, object args)
	{
		Type type = null;
		if (args != null)
		{
			type = args.GetType();
		}
		MethodInfo method2;
		if (type != null)
		{
			method2 = sender.GetType().GetMethod(method, new Type[]
			{
				type
			});
		}
		else
		{
			method2 = sender.GetType().GetMethod(method);
		}
		if (!(method2 != null))
		{
			PropertyInfo property = sender.GetType().GetProperty(method);
			if (property != null)
			{
				property.SetValue(sender, args, null);
			}
			return;
		}
		if (args != null)
		{
			object[] parameters = new object[]
			{
				args
			};
			method2.Invoke(sender, parameters);
			return;
		}
		method2.Invoke(sender, null);
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x00009E67 File Offset: 0x00008067
	public static void InvokeDelay(this MonoBehaviour behaviour, string method, object options, YieldInstruction wait)
	{
		behaviour.StartCoroutine(behaviour._invoke(method, wait, options));
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x00009E79 File Offset: 0x00008079
	private static IEnumerator _invoke(this MonoBehaviour behaviour, string method, YieldInstruction wait, object options)
	{
		yield return wait;
		behaviour.GetType().GetMethod(method).Invoke(behaviour, new object[]
		{
			options
		});
		yield return null;
		yield break;
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x000507D0 File Offset: 0x0004E9D0
	public static void Invoke(this ScriptableObject sender, string method, object args)
	{
		MethodInfo method2 = sender.GetType().GetMethod(method);
		if (method2 != null)
		{
			if (args != null)
			{
				object[] parameters = new object[]
				{
					args
				};
				method2.Invoke(sender, parameters);
				return;
			}
			method2.Invoke(sender, null);
		}
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x00050814 File Offset: 0x0004EA14
	public static void SetLayer(this GameObject parent, int layer, bool includeChildren = true)
	{
		parent.layer = layer;
		if (includeChildren)
		{
			Transform[] componentsInChildren = parent.transform.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = layer;
			}
		}
	}
}
