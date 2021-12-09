using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000FD RID: 253
public class LeanTween : MonoBehaviour
{
	// Token: 0x06000681 RID: 1665 RVA: 0x0000869A File Offset: 0x0000689A
	public static void init()
	{
		LeanTween.init(LeanTween.maxTweens);
	}

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x06000682 RID: 1666 RVA: 0x000086A6 File Offset: 0x000068A6
	public static int maxSearch
	{
		get
		{
			return LeanTween.tweenMaxSearch;
		}
	}

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x06000683 RID: 1667 RVA: 0x000086AD File Offset: 0x000068AD
	public static int maxSimulataneousTweens
	{
		get
		{
			return LeanTween.maxTweens;
		}
	}

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x06000684 RID: 1668 RVA: 0x00046AAC File Offset: 0x00044CAC
	public static int tweensRunning
	{
		get
		{
			int num = 0;
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (LeanTween.tweens[i].toggle)
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x000086B4 File Offset: 0x000068B4
	public static void init(int maxSimultaneousTweens)
	{
		LeanTween.init(maxSimultaneousTweens, LeanTween.maxSequences);
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x00046AE0 File Offset: 0x00044CE0
	public static void init(int maxSimultaneousTweens, int maxSimultaneousSequences)
	{
		if (LeanTween.tweens == null)
		{
			LeanTween.maxTweens = maxSimultaneousTweens;
			LeanTween.tweens = new LTDescr[LeanTween.maxTweens];
			LeanTween.tweensFinished = new int[LeanTween.maxTweens];
			LeanTween.tweensFinishedIds = new int[LeanTween.maxTweens];
			LeanTween._tweenEmpty = new GameObject();
			LeanTween._tweenEmpty.name = "~LeanTween";
			LeanTween._tweenEmpty.AddComponent(typeof(LeanTween));
			LeanTween._tweenEmpty.isStatic = true;
			LeanTween._tweenEmpty.hideFlags = HideFlags.HideAndDontSave;
			UnityEngine.Object.DontDestroyOnLoad(LeanTween._tweenEmpty);
			for (int i = 0; i < LeanTween.maxTweens; i++)
			{
				LeanTween.tweens[i] = new LTDescr();
			}
			SceneManager.sceneLoaded += LeanTween.onLevelWasLoaded54;
			LeanTween.sequences = new LTSeq[maxSimultaneousSequences];
			for (int j = 0; j < maxSimultaneousSequences; j++)
			{
				LeanTween.sequences[j] = new LTSeq();
			}
		}
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x00046BCC File Offset: 0x00044DCC
	public static void reset()
	{
		if (LeanTween.tweens != null)
		{
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (LeanTween.tweens[i] != null)
				{
					LeanTween.tweens[i].toggle = false;
				}
			}
		}
		LeanTween.tweens = null;
		UnityEngine.Object.Destroy(LeanTween._tweenEmpty);
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x000086C1 File Offset: 0x000068C1
	public void Update()
	{
		LeanTween.update();
	}

	// Token: 0x06000689 RID: 1673 RVA: 0x000086C8 File Offset: 0x000068C8
	private static void onLevelWasLoaded54(Scene scene, LoadSceneMode mode)
	{
		LeanTween.internalOnLevelWasLoaded(scene.buildIndex);
	}

	// Token: 0x0600068A RID: 1674 RVA: 0x000086D6 File Offset: 0x000068D6
	private static void internalOnLevelWasLoaded(int lvl)
	{
		LTGUI.reset();
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x00046C18 File Offset: 0x00044E18
	public static void update()
	{
		if (LeanTween.frameRendered != Time.frameCount)
		{
			LeanTween.init();
			LeanTween.dtEstimated = ((LeanTween.dtEstimated < 0f) ? 0f : (LeanTween.dtEstimated = Time.unscaledDeltaTime));
			LeanTween.dtActual = Time.deltaTime;
			LeanTween.maxTweenReached = 0;
			LeanTween.finishedCnt = 0;
			int num = 0;
			while (num <= LeanTween.tweenMaxSearch && num < LeanTween.maxTweens)
			{
				LeanTween.tween = LeanTween.tweens[num];
				if (LeanTween.tween.toggle)
				{
					LeanTween.maxTweenReached = num;
					if (LeanTween.tween.updateInternal())
					{
						LeanTween.tweensFinished[LeanTween.finishedCnt] = num;
						LeanTween.tweensFinishedIds[LeanTween.finishedCnt] = LeanTween.tweens[num].id;
						LeanTween.finishedCnt++;
					}
				}
				num++;
			}
			LeanTween.tweenMaxSearch = LeanTween.maxTweenReached;
			LeanTween.frameRendered = Time.frameCount;
			for (int i = 0; i < LeanTween.finishedCnt; i++)
			{
				LeanTween.j = LeanTween.tweensFinished[i];
				LeanTween.tween = LeanTween.tweens[LeanTween.j];
				if (LeanTween.tween.id == LeanTween.tweensFinishedIds[i])
				{
					LeanTween.removeTween(LeanTween.j);
					if (LeanTween.tween.hasExtraOnCompletes && LeanTween.tween.trans != null)
					{
						LeanTween.tween.callOnCompletes();
					}
				}
			}
		}
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x000086DD File Offset: 0x000068DD
	public static void removeTween(int i, int uniqueId)
	{
		if (LeanTween.tweens[i].uniqueId == uniqueId)
		{
			LeanTween.removeTween(i);
		}
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x00046D68 File Offset: 0x00044F68
	public static void removeTween(int i)
	{
		if (LeanTween.tweens[i].toggle)
		{
			LeanTween.tweens[i].toggle = false;
			LeanTween.tweens[i].counter = uint.MaxValue;
			if (LeanTween.tweens[i].destroyOnComplete)
			{
				if (LeanTween.tweens[i]._optional.ltRect != null)
				{
					LTGUI.destroy(LeanTween.tweens[i]._optional.ltRect.id);
				}
				else if (LeanTween.tweens[i].trans != null && LeanTween.tweens[i].trans.gameObject != LeanTween._tweenEmpty)
				{
					UnityEngine.Object.Destroy(LeanTween.tweens[i].trans.gameObject);
				}
			}
			LeanTween.startSearch = i;
			if (i + 1 >= LeanTween.tweenMaxSearch)
			{
				LeanTween.startSearch = 0;
			}
		}
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x00046E3C File Offset: 0x0004503C
	public static Vector3[] add(Vector3[] a, Vector3 b)
	{
		Vector3[] array = new Vector3[a.Length];
		LeanTween.i = 0;
		while (LeanTween.i < a.Length)
		{
			array[LeanTween.i] = a[LeanTween.i] + b;
			LeanTween.i++;
		}
		return array;
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x00046E90 File Offset: 0x00045090
	public static float closestRot(float from, float to)
	{
		float num = 0f - (360f - to);
		float num2 = 360f + to;
		float num3 = Mathf.Abs(to - from);
		float num4 = Mathf.Abs(num - from);
		float num5 = Mathf.Abs(num2 - from);
		if (num3 < num4 && num3 < num5)
		{
			return to;
		}
		if (num4 < num5)
		{
			return num;
		}
		return num2;
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x000086F4 File Offset: 0x000068F4
	public static void cancelAll()
	{
		LeanTween.cancelAll(false);
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x00046EE4 File Offset: 0x000450E4
	public static void cancelAll(bool callComplete)
	{
		LeanTween.init();
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].trans != null)
			{
				if (callComplete && LeanTween.tweens[i].optional.onComplete != null)
				{
					LeanTween.tweens[i].optional.onComplete();
				}
				LeanTween.removeTween(i);
			}
		}
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x000086FC File Offset: 0x000068FC
	public static void cancel(GameObject gameObject)
	{
		LeanTween.cancel(gameObject, false);
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x00046F4C File Offset: 0x0004514C
	public static void cancel(GameObject gameObject, bool callOnComplete)
	{
		LeanTween.init();
		Transform transform = gameObject.transform;
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			LTDescr ltdescr = LeanTween.tweens[i];
			if (ltdescr != null && ltdescr.toggle && ltdescr.trans == transform)
			{
				if (callOnComplete && ltdescr.optional.onComplete != null)
				{
					ltdescr.optional.onComplete();
				}
				LeanTween.removeTween(i);
			}
		}
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x00008705 File Offset: 0x00006905
	public static void cancel(RectTransform rect)
	{
		LeanTween.cancel(rect.gameObject, false);
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x00046FBC File Offset: 0x000451BC
	public static void cancel(GameObject gameObject, int uniqueId, bool callOnComplete = false)
	{
		if (uniqueId >= 0)
		{
			LeanTween.init();
			int num = uniqueId & 65535;
			int num2 = uniqueId >> 16;
			if (LeanTween.tweens[num].trans == null || (LeanTween.tweens[num].trans.gameObject == gameObject && (ulong)LeanTween.tweens[num].counter == (ulong)((long)num2)))
			{
				if (callOnComplete && LeanTween.tweens[num].optional.onComplete != null)
				{
					LeanTween.tweens[num].optional.onComplete();
				}
				LeanTween.removeTween(num);
			}
		}
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x00047054 File Offset: 0x00045254
	public static void cancel(LTRect ltRect, int uniqueId)
	{
		if (uniqueId >= 0)
		{
			LeanTween.init();
			int num = uniqueId & 65535;
			int num2 = uniqueId >> 16;
			if (LeanTween.tweens[num]._optional.ltRect == ltRect && (ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
			{
				LeanTween.removeTween(num);
			}
		}
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x00008713 File Offset: 0x00006913
	public static void cancel(int uniqueId)
	{
		LeanTween.cancel(uniqueId, false);
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x000470A4 File Offset: 0x000452A4
	public static void cancel(int uniqueId, bool callOnComplete)
	{
		if (uniqueId >= 0)
		{
			LeanTween.init();
			int num = uniqueId & 65535;
			int num2 = uniqueId >> 16;
			if (num > LeanTween.tweens.Length - 1)
			{
				int num3 = num - LeanTween.tweens.Length;
				LTSeq ltseq = LeanTween.sequences[num3];
				for (int i = 0; i < LeanTween.maxSequences; i++)
				{
					if (ltseq.current.tween != null)
					{
						LeanTween.removeTween(ltseq.current.tween.uniqueId & 65535);
					}
					if (ltseq.current.previous == null)
					{
						return;
					}
					ltseq.current = ltseq.current.previous;
				}
				return;
			}
			if ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
			{
				if (callOnComplete && LeanTween.tweens[num].optional.onComplete != null)
				{
					LeanTween.tweens[num].optional.onComplete();
				}
				LeanTween.removeTween(num);
			}
		}
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x00047188 File Offset: 0x00045388
	public static LTDescr descr(int uniqueId)
	{
		LeanTween.init();
		int num = uniqueId & 65535;
		int num2 = uniqueId >> 16;
		if (LeanTween.tweens[num] != null && LeanTween.tweens[num].uniqueId == uniqueId && (ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
		{
			return LeanTween.tweens[num];
		}
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].uniqueId == uniqueId && (ulong)LeanTween.tweens[i].counter == (ulong)((long)num2))
			{
				return LeanTween.tweens[i];
			}
		}
		return null;
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x0000871C File Offset: 0x0000691C
	public static LTDescr description(int uniqueId)
	{
		return LeanTween.descr(uniqueId);
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x00047214 File Offset: 0x00045414
	public static LTDescr[] descriptions(GameObject gameObject = null)
	{
		if (gameObject == null)
		{
			return null;
		}
		List<LTDescr> list = new List<LTDescr>();
		Transform transform = gameObject.transform;
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].toggle && LeanTween.tweens[i].trans == transform)
			{
				list.Add(LeanTween.tweens[i]);
			}
		}
		return list.ToArray();
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x00008724 File Offset: 0x00006924
	[Obsolete("Use 'pause( id )' instead")]
	public static void pause(GameObject gameObject, int uniqueId)
	{
		LeanTween.pause(uniqueId);
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x00047280 File Offset: 0x00045480
	public static void pause(int uniqueId)
	{
		int num = uniqueId & 65535;
		int num2 = uniqueId >> 16;
		if ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
		{
			LeanTween.tweens[num].pause();
		}
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x000472B8 File Offset: 0x000454B8
	public static void pause(GameObject gameObject)
	{
		Transform transform = gameObject.transform;
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].trans == transform)
			{
				LeanTween.tweens[i].pause();
			}
		}
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x00047300 File Offset: 0x00045500
	public static void pauseAll()
	{
		LeanTween.init();
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			LeanTween.tweens[i].pause();
		}
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x00047330 File Offset: 0x00045530
	public static void resumeAll()
	{
		LeanTween.init();
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			LeanTween.tweens[i].resume();
		}
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x0000872C File Offset: 0x0000692C
	[Obsolete("Use 'resume( id )' instead")]
	public static void resume(GameObject gameObject, int uniqueId)
	{
		LeanTween.resume(uniqueId);
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x00047360 File Offset: 0x00045560
	public static void resume(int uniqueId)
	{
		int num = uniqueId & 65535;
		int num2 = uniqueId >> 16;
		if ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
		{
			LeanTween.tweens[num].resume();
		}
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x00047398 File Offset: 0x00045598
	public static void resume(GameObject gameObject)
	{
		Transform transform = gameObject.transform;
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].trans == transform)
			{
				LeanTween.tweens[i].resume();
			}
		}
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x000473E0 File Offset: 0x000455E0
	public static bool isPaused(GameObject gameObject = null)
	{
		if (gameObject == null)
		{
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (object.Equals(LeanTween.tweens[i].direction, 0f))
				{
					return true;
				}
			}
			return false;
		}
		Transform transform = gameObject.transform;
		for (int j = 0; j <= LeanTween.tweenMaxSearch; j++)
		{
			if (object.Equals(LeanTween.tweens[j].direction, 0f) && LeanTween.tweens[j].trans == transform)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x00008734 File Offset: 0x00006934
	public static bool isPaused(RectTransform rect)
	{
		return LeanTween.isTweening(rect.gameObject);
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x0004747C File Offset: 0x0004567C
	public static bool isPaused(int uniqueId)
	{
		int num = uniqueId & 65535;
		int num2 = uniqueId >> 16;
		return num >= 0 && num < LeanTween.maxTweens && ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2) && object.Equals(LeanTween.tweens[LeanTween.i].direction, 0f));
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x000474E0 File Offset: 0x000456E0
	public static bool isTweening(GameObject gameObject = null)
	{
		if (gameObject == null)
		{
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (LeanTween.tweens[i].toggle)
				{
					return true;
				}
			}
			return false;
		}
		Transform transform = gameObject.transform;
		for (int j = 0; j <= LeanTween.tweenMaxSearch; j++)
		{
			if (LeanTween.tweens[j].toggle && LeanTween.tweens[j].trans == transform)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x00008734 File Offset: 0x00006934
	public static bool isTweening(RectTransform rect)
	{
		return LeanTween.isTweening(rect.gameObject);
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x00047554 File Offset: 0x00045754
	public static bool isTweening(int uniqueId)
	{
		int num = uniqueId & 65535;
		int num2 = uniqueId >> 16;
		return num >= 0 && num < LeanTween.maxTweens && ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2) && LeanTween.tweens[num].toggle);
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x000475A0 File Offset: 0x000457A0
	public static bool isTweening(LTRect ltRect)
	{
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].toggle && LeanTween.tweens[i]._optional.ltRect == ltRect)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x000475E4 File Offset: 0x000457E4
	public static void drawBezierPath(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float arrowSize = 0f, Transform arrowTransform = null)
	{
		Vector3 vector = a;
		Vector3 a2 = -a + 3f * (b - c) + d;
		Vector3 b2 = 3f * (a + c) - 6f * b;
		Vector3 b3 = 3f * (b - a);
		if (arrowSize > 0f)
		{
			Vector3 position = arrowTransform.position;
			Quaternion rotation = arrowTransform.rotation;
			float num = 0f;
			for (float num2 = 1f; num2 <= 120f; num2 += 1f)
			{
				float num3 = num2 / 120f;
				Vector3 vector2 = ((a2 * num3 + b2) * num3 + b3) * num3 + a;
				Gizmos.DrawLine(vector, vector2);
				num += (vector2 - vector).magnitude;
				if (num > 1f)
				{
					num -= 1f;
					arrowTransform.position = vector2;
					arrowTransform.LookAt(vector, Vector3.forward);
					Vector3 a3 = arrowTransform.TransformDirection(Vector3.right);
					Vector3 normalized = (vector - vector2).normalized;
					Gizmos.DrawLine(vector2, vector2 + (a3 + normalized) * arrowSize);
					a3 = arrowTransform.TransformDirection(-Vector3.right);
					Gizmos.DrawLine(vector2, vector2 + (a3 + normalized) * arrowSize);
				}
				vector = vector2;
			}
			arrowTransform.position = position;
			arrowTransform.rotation = rotation;
			return;
		}
		for (float num4 = 1f; num4 <= 30f; num4 += 1f)
		{
			float num3 = num4 / 30f;
			Vector3 vector2 = ((a2 * num3 + b2) * num3 + b3) * num3 + a;
			Gizmos.DrawLine(vector, vector2);
			vector = vector2;
		}
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x00008741 File Offset: 0x00006941
	public static object logError(string error)
	{
		if (LeanTween.throwErrors)
		{
			Debug.LogError(error);
		}
		else
		{
			Debug.Log(error);
		}
		return null;
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x00008759 File Offset: 0x00006959
	public static LTDescr options(LTDescr seed)
	{
		Debug.LogError("error this function is no longer used");
		return null;
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x000477E8 File Offset: 0x000459E8
	public static LTDescr options()
	{
		LeanTween.init();
		bool flag = false;
		LeanTween.j = 0;
		LeanTween.i = LeanTween.startSearch;
		while (LeanTween.j <= LeanTween.maxTweens)
		{
			if (LeanTween.j >= LeanTween.maxTweens)
			{
				return LeanTween.logError("LeanTween - You have run out of available spaces for tweening. To avoid this error increase the number of spaces to available for tweening when you initialize the LeanTween class ex: LeanTween.init( " + (LeanTween.maxTweens * 2).ToString() + " );") as LTDescr;
			}
			if (LeanTween.i >= LeanTween.maxTweens)
			{
				LeanTween.i = 0;
			}
			if (!LeanTween.tweens[LeanTween.i].toggle)
			{
				if (LeanTween.i + 1 > LeanTween.tweenMaxSearch && LeanTween.i + 1 < LeanTween.maxTweens)
				{
					LeanTween.tweenMaxSearch = LeanTween.i + 1;
				}
				LeanTween.startSearch = LeanTween.i + 1;
				flag = true;
				break;
			}
			LeanTween.j++;
			LeanTween.i++;
		}
		if (!flag)
		{
			LeanTween.logError("no available tween found!");
		}
		LeanTween.tweens[LeanTween.i].reset();
		LeanTween.global_counter += 1U;
		if (LeanTween.global_counter > 32768U)
		{
			LeanTween.global_counter = 0U;
		}
		LeanTween.tweens[LeanTween.i].setId((uint)LeanTween.i, LeanTween.global_counter);
		return LeanTween.tweens[LeanTween.i];
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x060006AF RID: 1711 RVA: 0x00008766 File Offset: 0x00006966
	public static GameObject tweenEmpty
	{
		get
		{
			LeanTween.init(LeanTween.maxTweens);
			return LeanTween._tweenEmpty;
		}
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x0004792C File Offset: 0x00045B2C
	private static LTDescr pushNewTween(GameObject gameObject, Vector3 to, float time, LTDescr tween)
	{
		LeanTween.init(LeanTween.maxTweens);
		if (gameObject == null || tween == null)
		{
			return null;
		}
		tween.trans = gameObject.transform;
		tween.to = to;
		tween.time = time;
		if (tween.time <= 0f)
		{
			tween.updateInternal();
		}
		return tween;
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x00047980 File Offset: 0x00045B80
	public static LTDescr play(RectTransform rectTransform, Sprite[] sprites)
	{
		float time = 0.25f * (float)sprites.Length;
		return LeanTween.pushNewTween(rectTransform.gameObject, new Vector3((float)sprites.Length - 1f, 0f, 0f), time, LeanTween.options().setCanvasPlaySprite().setSprites(sprites).setRepeat(-1));
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x000479D4 File Offset: 0x00045BD4
	public static LTSeq sequence(bool initSequence = true)
	{
		LeanTween.init(LeanTween.maxTweens);
		for (int i = 0; i < LeanTween.sequences.Length; i++)
		{
			if ((LeanTween.sequences[i].tween == null || !LeanTween.sequences[i].tween.toggle) && !LeanTween.sequences[i].toggle)
			{
				LTSeq ltseq = LeanTween.sequences[i];
				if (initSequence)
				{
					ltseq.init((uint)(i + LeanTween.tweens.Length), LeanTween.global_counter);
					LeanTween.global_counter += 1U;
					if (LeanTween.global_counter > 32768U)
					{
						LeanTween.global_counter = 0U;
					}
				}
				else
				{
					ltseq.reset();
				}
				return ltseq;
			}
		}
		return null;
	}

	// Token: 0x060006B3 RID: 1715 RVA: 0x00047A78 File Offset: 0x00045C78
	public static LTDescr alpha(GameObject gameObject, float to, float time)
	{
		LTDescr ltdescr = LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setAlpha());
		SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
		ltdescr.spriteRen = component;
		return ltdescr;
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x00008777 File Offset: 0x00006977
	public static LTDescr alpha(LTRect ltRect, float to, float time)
	{
		ltRect.alphaEnabled = true;
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, new Vector3(to, 0f, 0f), time, LeanTween.options().setGUIAlpha().setRect(ltRect));
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x000087AB File Offset: 0x000069AB
	public static LTDescr textAlpha(RectTransform rectTransform, float to, float time)
	{
		return LeanTween.pushNewTween(rectTransform.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setTextAlpha());
	}

	// Token: 0x060006B6 RID: 1718 RVA: 0x000087AB File Offset: 0x000069AB
	public static LTDescr alphaText(RectTransform rectTransform, float to, float time)
	{
		return LeanTween.pushNewTween(rectTransform.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setTextAlpha());
	}

	// Token: 0x060006B7 RID: 1719 RVA: 0x000087D3 File Offset: 0x000069D3
	public static LTDescr alphaCanvas(CanvasGroup canvasGroup, float to, float time)
	{
		return LeanTween.pushNewTween(canvasGroup.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasGroupAlpha());
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x000087FB File Offset: 0x000069FB
	public static LTDescr alphaVertex(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setAlphaVertex());
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x00047AB4 File Offset: 0x00045CB4
	public static LTDescr color(GameObject gameObject, Color to, float time)
	{
		LTDescr ltdescr = LeanTween.pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setColor().setPoint(new Vector3(to.r, to.g, to.b)));
		SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
		ltdescr.spriteRen = component;
		return ltdescr;
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x00047B14 File Offset: 0x00045D14
	public static LTDescr textColor(RectTransform rectTransform, Color to, float time)
	{
		return LeanTween.pushNewTween(rectTransform.gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setTextColor().setPoint(new Vector3(to.r, to.g, to.b)));
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x00047B14 File Offset: 0x00045D14
	public static LTDescr colorText(RectTransform rectTransform, Color to, float time)
	{
		return LeanTween.pushNewTween(rectTransform.gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setTextColor().setPoint(new Vector3(to.r, to.g, to.b)));
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x0000881E File Offset: 0x00006A1E
	public static LTDescr delayedCall(float delayTime, Action callback)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, Vector3.zero, delayTime, LeanTween.options().setCallback().setOnComplete(callback));
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x00008840 File Offset: 0x00006A40
	public static LTDescr delayedCall(float delayTime, Action<object> callback)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, Vector3.zero, delayTime, LeanTween.options().setCallback().setOnComplete(callback));
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x00008862 File Offset: 0x00006A62
	public static LTDescr delayedCall(GameObject gameObject, float delayTime, Action callback)
	{
		return LeanTween.pushNewTween(gameObject, Vector3.zero, delayTime, LeanTween.options().setCallback().setOnComplete(callback));
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x00008880 File Offset: 0x00006A80
	public static LTDescr delayedCall(GameObject gameObject, float delayTime, Action<object> callback)
	{
		return LeanTween.pushNewTween(gameObject, Vector3.zero, delayTime, LeanTween.options().setCallback().setOnComplete(callback));
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x0000889E File Offset: 0x00006A9E
	public static LTDescr destroyAfter(LTRect rect, float delayTime)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, Vector3.zero, delayTime, LeanTween.options().setCallback().setRect(rect).setDestroyOnComplete(true));
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x000088C6 File Offset: 0x00006AC6
	public static LTDescr move(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setMove());
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x000088DA File Offset: 0x00006ADA
	public static LTDescr move(GameObject gameObject, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to.x, to.y, gameObject.transform.position.z), time, LeanTween.options().setMove());
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x00047B68 File Offset: 0x00045D68
	public static LTDescr move(GameObject gameObject, Vector3[] to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveCurved();
		if (LeanTween.d.optional.path == null)
		{
			LeanTween.d.optional.path = new LTBezierPath(to);
		}
		else
		{
			LeanTween.d.optional.path.setPoints(to);
		}
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x00047BE4 File Offset: 0x00045DE4
	public static LTDescr move(GameObject gameObject, LTBezierPath to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveCurved();
		LeanTween.d.optional.path = to;
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x00047C30 File Offset: 0x00045E30
	public static LTDescr move(GameObject gameObject, LTSpline to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveSpline();
		LeanTween.d.optional.spline = to;
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x00047C7C File Offset: 0x00045E7C
	public static LTDescr moveSpline(GameObject gameObject, Vector3[] to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveSpline();
		LeanTween.d.optional.spline = new LTSpline(to);
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x00047C30 File Offset: 0x00045E30
	public static LTDescr moveSpline(GameObject gameObject, LTSpline to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveSpline();
		LeanTween.d.optional.spline = to;
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x060006C8 RID: 1736 RVA: 0x00047CD0 File Offset: 0x00045ED0
	public static LTDescr moveSplineLocal(GameObject gameObject, Vector3[] to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveSplineLocal();
		LeanTween.d.optional.spline = new LTSpline(to);
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x0000890E File Offset: 0x00006B0E
	public static LTDescr move(LTRect ltRect, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, to, time, LeanTween.options().setGUIMove().setRect(ltRect));
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x00008931 File Offset: 0x00006B31
	public static LTDescr moveMargin(LTRect ltRect, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, to, time, LeanTween.options().setGUIMoveMargin().setRect(ltRect));
	}

	// Token: 0x060006CB RID: 1739 RVA: 0x00008954 File Offset: 0x00006B54
	public static LTDescr moveX(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setMoveX());
	}

	// Token: 0x060006CC RID: 1740 RVA: 0x00008977 File Offset: 0x00006B77
	public static LTDescr moveY(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setMoveY());
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x0000899A File Offset: 0x00006B9A
	public static LTDescr moveZ(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setMoveZ());
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x000089BD File Offset: 0x00006BBD
	public static LTDescr moveLocal(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setMoveLocal());
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x00047D24 File Offset: 0x00045F24
	public static LTDescr moveLocal(GameObject gameObject, Vector3[] to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveCurvedLocal();
		if (LeanTween.d.optional.path == null)
		{
			LeanTween.d.optional.path = new LTBezierPath(to);
		}
		else
		{
			LeanTween.d.optional.path.setPoints(to);
		}
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x000089D1 File Offset: 0x00006BD1
	public static LTDescr moveLocalX(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setMoveLocalX());
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x000089F4 File Offset: 0x00006BF4
	public static LTDescr moveLocalY(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setMoveLocalY());
	}

	// Token: 0x060006D2 RID: 1746 RVA: 0x00008A17 File Offset: 0x00006C17
	public static LTDescr moveLocalZ(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setMoveLocalZ());
	}

	// Token: 0x060006D3 RID: 1747 RVA: 0x00047DA0 File Offset: 0x00045FA0
	public static LTDescr moveLocal(GameObject gameObject, LTBezierPath to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveCurvedLocal();
		LeanTween.d.optional.path = to;
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x00047DEC File Offset: 0x00045FEC
	public static LTDescr moveLocal(GameObject gameObject, LTSpline to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveSplineLocal();
		LeanTween.d.optional.spline = to;
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x00008A3A File Offset: 0x00006C3A
	public static LTDescr move(GameObject gameObject, Transform to, float time)
	{
		return LeanTween.pushNewTween(gameObject, Vector3.zero, time, LeanTween.options().setTo(to).setMoveToTransform());
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x00008A58 File Offset: 0x00006C58
	public static LTDescr rotate(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setRotate());
	}

	// Token: 0x060006D7 RID: 1751 RVA: 0x00008A6C File Offset: 0x00006C6C
	public static LTDescr rotate(LTRect ltRect, float to, float time)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, new Vector3(to, 0f, 0f), time, LeanTween.options().setGUIRotate().setRect(ltRect));
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x00008A99 File Offset: 0x00006C99
	public static LTDescr rotateLocal(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setRotateLocal());
	}

	// Token: 0x060006D9 RID: 1753 RVA: 0x00008AAD File Offset: 0x00006CAD
	public static LTDescr rotateX(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setRotateX());
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x00008AD0 File Offset: 0x00006CD0
	public static LTDescr rotateY(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setRotateY());
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x00008AF3 File Offset: 0x00006CF3
	public static LTDescr rotateZ(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setRotateZ());
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x00008B16 File Offset: 0x00006D16
	public static LTDescr rotateAround(GameObject gameObject, Vector3 axis, float add, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(add, 0f, 0f), time, LeanTween.options().setAxis(axis).setRotateAround());
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x00008B3F File Offset: 0x00006D3F
	public static LTDescr rotateAroundLocal(GameObject gameObject, Vector3 axis, float add, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(add, 0f, 0f), time, LeanTween.options().setRotateAroundLocal().setAxis(axis));
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x00008B68 File Offset: 0x00006D68
	public static LTDescr scale(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setScale());
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x00008B7C File Offset: 0x00006D7C
	public static LTDescr scale(LTRect ltRect, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, to, time, LeanTween.options().setGUIScale().setRect(ltRect));
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x00008B9F File Offset: 0x00006D9F
	public static LTDescr scaleX(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setScaleX());
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x00008BC2 File Offset: 0x00006DC2
	public static LTDescr scaleY(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setScaleY());
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x00008BE5 File Offset: 0x00006DE5
	public static LTDescr scaleZ(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setScaleZ());
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x00008C08 File Offset: 0x00006E08
	public static LTDescr value(GameObject gameObject, float from, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCallback().setFrom(new Vector3(from, 0f, 0f)));
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x00008C40 File Offset: 0x00006E40
	public static LTDescr value(float from, float to, float time)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, new Vector3(to, 0f, 0f), time, LeanTween.options().setCallback().setFrom(new Vector3(from, 0f, 0f)));
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x00047E38 File Offset: 0x00046038
	public static LTDescr value(GameObject gameObject, Vector2 from, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to.x, to.y, 0f), time, LeanTween.options().setValue3().setTo(new Vector3(to.x, to.y, 0f)).setFrom(new Vector3(from.x, from.y, 0f)));
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x00008C7C File Offset: 0x00006E7C
	public static LTDescr value(GameObject gameObject, Vector3 from, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setValue3().setFrom(from));
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x00047EA4 File Offset: 0x000460A4
	public static LTDescr value(GameObject gameObject, Color from, Color to, float time)
	{
		LTDescr ltdescr = LeanTween.pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setCallbackColor().setPoint(new Vector3(to.r, to.g, to.b)).setFromColor(from).setHasInitialized(false));
		SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
		ltdescr.spriteRen = component;
		return ltdescr;
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x00047F10 File Offset: 0x00046110
	public static LTDescr value(GameObject gameObject, Action<float> callOnUpdate, float from, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCallback().setTo(new Vector3(to, 0f, 0f)).setFrom(new Vector3(from, 0f, 0f)).setOnUpdate(callOnUpdate));
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x00047F70 File Offset: 0x00046170
	public static LTDescr value(GameObject gameObject, Action<float, float> callOnUpdateRatio, float from, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCallback().setTo(new Vector3(to, 0f, 0f)).setFrom(new Vector3(from, 0f, 0f)).setOnUpdateRatio(callOnUpdateRatio));
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x00047FD0 File Offset: 0x000461D0
	public static LTDescr value(GameObject gameObject, Action<Color> callOnUpdate, Color from, Color to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setCallbackColor().setPoint(new Vector3(to.r, to.g, to.b)).setAxis(new Vector3(from.r, from.g, from.b)).setFrom(new Vector3(0f, from.a, 0f)).setHasInitialized(false).setOnUpdateColor(callOnUpdate));
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x00048064 File Offset: 0x00046264
	public static LTDescr value(GameObject gameObject, Action<Color, object> callOnUpdate, Color from, Color to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setCallbackColor().setPoint(new Vector3(to.r, to.g, to.b)).setAxis(new Vector3(from.r, from.g, from.b)).setFrom(new Vector3(0f, from.a, 0f)).setHasInitialized(false).setOnUpdateColor(callOnUpdate));
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x000480F8 File Offset: 0x000462F8
	public static LTDescr value(GameObject gameObject, Action<Vector2> callOnUpdate, Vector2 from, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to.x, to.y, 0f), time, LeanTween.options().setValue3().setTo(new Vector3(to.x, to.y, 0f)).setFrom(new Vector3(from.x, from.y, 0f)).setOnUpdateVector2(callOnUpdate));
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x00008C96 File Offset: 0x00006E96
	public static LTDescr value(GameObject gameObject, Action<Vector3> callOnUpdate, Vector3 from, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setValue3().setTo(to).setFrom(from).setOnUpdateVector3(callOnUpdate));
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x0004816C File Offset: 0x0004636C
	public static LTDescr value(GameObject gameObject, Action<float, object> callOnUpdate, float from, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCallback().setTo(new Vector3(to, 0f, 0f)).setFrom(new Vector3(from, 0f, 0f)).setOnUpdate(callOnUpdate, gameObject));
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x00008CBD File Offset: 0x00006EBD
	public static LTDescr delayedSound(AudioClip audio, Vector3 pos, float volume)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, pos, 0f, LeanTween.options().setDelayedSound().setTo(pos).setFrom(new Vector3(volume, 0f, 0f)).setAudio(audio));
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x00008CFA File Offset: 0x00006EFA
	public static LTDescr delayedSound(GameObject gameObject, AudioClip audio, Vector3 pos, float volume)
	{
		return LeanTween.pushNewTween(gameObject, pos, 0f, LeanTween.options().setDelayedSound().setTo(pos).setFrom(new Vector3(volume, 0f, 0f)).setAudio(audio));
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x00008D33 File Offset: 0x00006F33
	public static LTDescr move(RectTransform rectTrans, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, to, time, LeanTween.options().setCanvasMove().setRect(rectTrans));
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x00008D52 File Offset: 0x00006F52
	public static LTDescr moveX(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasMoveX().setRect(rectTrans));
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x00008D80 File Offset: 0x00006F80
	public static LTDescr moveY(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasMoveY().setRect(rectTrans));
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x00008DAE File Offset: 0x00006FAE
	public static LTDescr moveZ(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasMoveZ().setRect(rectTrans));
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x00008DDC File Offset: 0x00006FDC
	public static LTDescr rotate(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasRotateAround().setRect(rectTrans).setAxis(Vector3.forward));
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x00008E14 File Offset: 0x00007014
	public static LTDescr rotate(RectTransform rectTrans, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, to, time, LeanTween.options().setCanvasRotateAround().setRect(rectTrans).setAxis(Vector3.forward));
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x00008E3D File Offset: 0x0000703D
	public static LTDescr rotateAround(RectTransform rectTrans, Vector3 axis, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasRotateAround().setRect(rectTrans).setAxis(axis));
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x00008E71 File Offset: 0x00007071
	public static LTDescr rotateAroundLocal(RectTransform rectTrans, Vector3 axis, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasRotateAroundLocal().setRect(rectTrans).setAxis(axis));
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x00008EA5 File Offset: 0x000070A5
	public static LTDescr scale(RectTransform rectTrans, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, to, time, LeanTween.options().setCanvasScale().setRect(rectTrans));
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x00008EC4 File Offset: 0x000070C4
	public static LTDescr size(RectTransform rectTrans, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, to, time, LeanTween.options().setCanvasSizeDelta().setRect(rectTrans));
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x00008EE8 File Offset: 0x000070E8
	public static LTDescr alpha(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasAlpha().setRect(rectTrans));
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x000481CC File Offset: 0x000463CC
	public static LTDescr color(RectTransform rectTrans, Color to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setCanvasColor().setRect(rectTrans).setPoint(new Vector3(to.r, to.g, to.b)));
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x00008F16 File Offset: 0x00007116
	public static float tweenOnCurve(LTDescr tweenDescr, float ratioPassed)
	{
		return tweenDescr.from.x + tweenDescr.diff.x * tweenDescr.optional.animationCurve.Evaluate(ratioPassed);
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x00048228 File Offset: 0x00046428
	public static Vector3 tweenOnCurveVector(LTDescr tweenDescr, float ratioPassed)
	{
		return new Vector3(tweenDescr.from.x + tweenDescr.diff.x * tweenDescr.optional.animationCurve.Evaluate(ratioPassed), tweenDescr.from.y + tweenDescr.diff.y * tweenDescr.optional.animationCurve.Evaluate(ratioPassed), tweenDescr.from.z + tweenDescr.diff.z * tweenDescr.optional.animationCurve.Evaluate(ratioPassed));
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x00008F41 File Offset: 0x00007141
	public static float easeOutQuadOpt(float start, float diff, float ratioPassed)
	{
		return -diff * ratioPassed * (ratioPassed - 2f) + start;
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x00008F51 File Offset: 0x00007151
	public static float easeInQuadOpt(float start, float diff, float ratioPassed)
	{
		return diff * ratioPassed * ratioPassed + start;
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x000482B8 File Offset: 0x000464B8
	public static float easeInOutQuadOpt(float start, float diff, float ratioPassed)
	{
		ratioPassed /= 0.5f;
		if (ratioPassed < 1f)
		{
			return diff / 2f * ratioPassed * ratioPassed + start;
		}
		ratioPassed -= 1f;
		return -diff / 2f * (ratioPassed * (ratioPassed - 2f) - 1f) + start;
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x00048308 File Offset: 0x00046508
	public static Vector3 easeInOutQuadOpt(Vector3 start, Vector3 diff, float ratioPassed)
	{
		ratioPassed /= 0.5f;
		if (ratioPassed < 1f)
		{
			return diff / 2f * ratioPassed * ratioPassed + start;
		}
		ratioPassed -= 1f;
		return -diff / 2f * (ratioPassed * (ratioPassed - 2f) - 1f) + start;
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x00008F5A File Offset: 0x0000715A
	public static float linear(float start, float end, float val)
	{
		return Mathf.Lerp(start, end, val);
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x00048378 File Offset: 0x00046578
	public static float clerp(float start, float end, float val)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		float result;
		if (end - start < -num3)
		{
			float num4 = (num2 - start + end) * val;
			result = start + num4;
		}
		else if (end - start > num3)
		{
			float num4 = -(num2 - end + start) * val;
			result = start + num4;
		}
		else
		{
			result = start + (end - start) * val;
		}
		return result;
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x000483E4 File Offset: 0x000465E4
	public static float spring(float start, float end, float val)
	{
		val = Mathf.Clamp01(val);
		val = (Mathf.Sin(val * 3.1415927f * (0.2f + 2.5f * val * val * val)) * Mathf.Pow(1f - val, 2.2f) + val) * (1f + 1.2f * (1f - val));
		return start + (end - start) * val;
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x00008F64 File Offset: 0x00007164
	public static float easeInQuad(float start, float end, float val)
	{
		end -= start;
		return end * val * val + start;
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x00008F72 File Offset: 0x00007172
	public static float easeOutQuad(float start, float end, float val)
	{
		end -= start;
		return -end * val * (val - 2f) + start;
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x00048448 File Offset: 0x00046648
	public static float easeInOutQuad(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * val * val + start;
		}
		val -= 1f;
		return -end / 2f * (val * (val - 2f) - 1f) + start;
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x00008F87 File Offset: 0x00007187
	public static float easeInOutQuadOpt2(float start, float diffBy2, float val, float val2)
	{
		val /= 0.5f;
		if (val < 1f)
		{
			return diffBy2 * val2 + start;
		}
		val -= 1f;
		return -diffBy2 * (val2 - 2f - 1f) + start;
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x00008FBB File Offset: 0x000071BB
	public static float easeInCubic(float start, float end, float val)
	{
		end -= start;
		return end * val * val * val + start;
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x00008FCB File Offset: 0x000071CB
	public static float easeOutCubic(float start, float end, float val)
	{
		val -= 1f;
		end -= start;
		return end * (val * val * val + 1f) + start;
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x0004849C File Offset: 0x0004669C
	public static float easeInOutCubic(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * val * val * val + start;
		}
		val -= 2f;
		return end / 2f * (val * val * val + 2f) + start;
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x00008FEA File Offset: 0x000071EA
	public static float easeInQuart(float start, float end, float val)
	{
		end -= start;
		return end * val * val * val * val + start;
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x00008FFC File Offset: 0x000071FC
	public static float easeOutQuart(float start, float end, float val)
	{
		val -= 1f;
		end -= start;
		return -end * (val * val * val * val - 1f) + start;
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x000484F0 File Offset: 0x000466F0
	public static float easeInOutQuart(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * val * val * val * val + start;
		}
		val -= 2f;
		return -end / 2f * (val * val * val * val - 2f) + start;
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x0000901E File Offset: 0x0000721E
	public static float easeInQuint(float start, float end, float val)
	{
		end -= start;
		return end * val * val * val * val * val + start;
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x00009032 File Offset: 0x00007232
	public static float easeOutQuint(float start, float end, float val)
	{
		val -= 1f;
		end -= start;
		return end * (val * val * val * val * val + 1f) + start;
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x00048548 File Offset: 0x00046748
	public static float easeInOutQuint(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * val * val * val * val * val + start;
		}
		val -= 2f;
		return end / 2f * (val * val * val * val * val + 2f) + start;
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x00009055 File Offset: 0x00007255
	public static float easeInSine(float start, float end, float val)
	{
		end -= start;
		return -end * Mathf.Cos(val / 1f * 1.5707964f) + end + start;
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x00009075 File Offset: 0x00007275
	public static float easeOutSine(float start, float end, float val)
	{
		end -= start;
		return end * Mathf.Sin(val / 1f * 1.5707964f) + start;
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x00009092 File Offset: 0x00007292
	public static float easeInOutSine(float start, float end, float val)
	{
		end -= start;
		return -end / 2f * (Mathf.Cos(3.1415927f * val / 1f) - 1f) + start;
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x000090BC File Offset: 0x000072BC
	public static float easeInExpo(float start, float end, float val)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (val / 1f - 1f)) + start;
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x000090E4 File Offset: 0x000072E4
	public static float easeOutExpo(float start, float end, float val)
	{
		end -= start;
		return end * (-Mathf.Pow(2f, -10f * val / 1f) + 1f) + start;
	}

	// Token: 0x06000718 RID: 1816 RVA: 0x000485A4 File Offset: 0x000467A4
	public static float easeInOutExpo(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * Mathf.Pow(2f, 10f * (val - 1f)) + start;
		}
		val -= 1f;
		return end / 2f * (-Mathf.Pow(2f, -10f * val) + 2f) + start;
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x0000910D File Offset: 0x0000730D
	public static float easeInCirc(float start, float end, float val)
	{
		end -= start;
		return -end * (Mathf.Sqrt(1f - val * val) - 1f) + start;
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x0000912D File Offset: 0x0000732D
	public static float easeOutCirc(float start, float end, float val)
	{
		val -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - val * val) + start;
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x00048614 File Offset: 0x00046814
	public static float easeInOutCirc(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return -end / 2f * (Mathf.Sqrt(1f - val * val) - 1f) + start;
		}
		val -= 2f;
		return end / 2f * (Mathf.Sqrt(1f - val * val) + 1f) + start;
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x00048680 File Offset: 0x00046880
	public static float easeInBounce(float start, float end, float val)
	{
		end -= start;
		float num = 1f;
		return end - LeanTween.easeOutBounce(0f, end, num - val) + start;
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x000486AC File Offset: 0x000468AC
	public static float easeOutBounce(float start, float end, float val)
	{
		val /= 1f;
		end -= start;
		if (val < 0.36363637f)
		{
			return end * (7.5625f * val * val) + start;
		}
		if (val < 0.72727275f)
		{
			val -= 0.54545456f;
			return end * (7.5625f * val * val + 0.75f) + start;
		}
		if ((double)val < 0.9090909090909091)
		{
			val -= 0.8181818f;
			return end * (7.5625f * val * val + 0.9375f) + start;
		}
		val -= 0.95454544f;
		return end * (7.5625f * val * val + 0.984375f) + start;
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x00048748 File Offset: 0x00046948
	public static float easeInOutBounce(float start, float end, float val)
	{
		end -= start;
		float num = 1f;
		if (val < num / 2f)
		{
			return LeanTween.easeInBounce(0f, end, val * 2f) * 0.5f + start;
		}
		return LeanTween.easeOutBounce(0f, end, val * 2f - num) * 0.5f + end * 0.5f + start;
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x000487AC File Offset: 0x000469AC
	public static float easeInBack(float start, float end, float val, float overshoot = 1f)
	{
		end -= start;
		val /= 1f;
		float num = 1.70158f * overshoot;
		return end * val * val * ((num + 1f) * val - num) + start;
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x000487E4 File Offset: 0x000469E4
	public static float easeOutBack(float start, float end, float val, float overshoot = 1f)
	{
		float num = 1.70158f * overshoot;
		end -= start;
		val = val / 1f - 1f;
		return end * (val * val * ((num + 1f) * val + num) + 1f) + start;
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x00048828 File Offset: 0x00046A28
	public static float easeInOutBack(float start, float end, float val, float overshoot = 1f)
	{
		float num = 1.70158f * overshoot;
		end -= start;
		val /= 0.5f;
		if (val < 1f)
		{
			num *= 1.525f * overshoot;
			return end / 2f * (val * val * ((num + 1f) * val - num)) + start;
		}
		val -= 2f;
		num *= 1.525f * overshoot;
		return end / 2f * (val * val * ((num + 1f) * val + num) + 2f) + start;
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x000488AC File Offset: 0x00046AAC
	public static float easeInElastic(float start, float end, float val, float overshoot = 1f, float period = 0.3f)
	{
		end -= start;
		float num = 0f;
		if (val == 0f)
		{
			return start;
		}
		if (val == 1f)
		{
			return start + end;
		}
		float num2;
		if (num == 0f || num < Mathf.Abs(end))
		{
			num = end;
			num2 = period / 4f;
		}
		else
		{
			num2 = period / 6.2831855f * Mathf.Asin(end / num);
		}
		if (overshoot > 1f && val > 0.6f)
		{
			overshoot = 1f + (1f - val) / 0.4f * (overshoot - 1f);
		}
		val -= 1f;
		return start - num * Mathf.Pow(2f, 10f * val) * Mathf.Sin((val - num2) * 6.2831855f / period) * overshoot;
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x00048970 File Offset: 0x00046B70
	public static float easeOutElastic(float start, float end, float val, float overshoot = 1f, float period = 0.3f)
	{
		end -= start;
		float num = 0f;
		if (val == 0f)
		{
			return start;
		}
		if (val == 1f)
		{
			return start + end;
		}
		float num2;
		if (num == 0f || num < Mathf.Abs(end))
		{
			num = end;
			num2 = period / 4f;
		}
		else
		{
			num2 = period / 6.2831855f * Mathf.Asin(end / num);
		}
		if (overshoot > 1f && val < 0.4f)
		{
			overshoot = 1f + val / 0.4f * (overshoot - 1f);
		}
		return start + end + num * Mathf.Pow(2f, -10f * val) * Mathf.Sin((val - num2) * 6.2831855f / period) * overshoot;
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x00048A28 File Offset: 0x00046C28
	public static float easeInOutElastic(float start, float end, float val, float overshoot = 1f, float period = 0.3f)
	{
		end -= start;
		float num = 0f;
		if (val == 0f)
		{
			return start;
		}
		val /= 0.5f;
		if (val == 2f)
		{
			return start + end;
		}
		float num2;
		if (num == 0f || num < Mathf.Abs(end))
		{
			num = end;
			num2 = period / 4f;
		}
		else
		{
			num2 = period / 6.2831855f * Mathf.Asin(end / num);
		}
		if (overshoot > 1f)
		{
			if (val < 0.2f)
			{
				overshoot = 1f + val / 0.2f * (overshoot - 1f);
			}
			else if (val > 0.8f)
			{
				overshoot = 1f + (1f - val) / 0.2f * (overshoot - 1f);
			}
		}
		if (val < 1f)
		{
			val -= 1f;
			return start - 0.5f * (num * Mathf.Pow(2f, 10f * val) * Mathf.Sin((val - num2) * 6.2831855f / period)) * overshoot;
		}
		val -= 1f;
		return end + start + num * Mathf.Pow(2f, -10f * val) * Mathf.Sin((val - num2) * 6.2831855f / period) * 0.5f * overshoot;
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x00048B60 File Offset: 0x00046D60
	public static LTDescr followDamp(Transform trans, Transform target, LeanProp prop, float smoothTime, float maxSpeed = -1f)
	{
		LTDescr d = LeanTween.pushNewTween(trans.gameObject, Vector3.zero, float.MaxValue, LeanTween.options().setFollow().setTarget(target));
		switch (prop)
		{
		case LeanProp.position:
			d.diff = d.trans.position;
			d.easeInternal = delegate()
			{
				d.optional.axis = LeanSmooth.damp(d.optional.axis, d.toTrans.position, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime);
				d.trans.position = d.optional.axis + d.toInternal;
			};
			break;
		case LeanProp.localPosition:
			d.optional.axis = d.trans.localPosition;
			d.easeInternal = delegate()
			{
				d.optional.axis = LeanSmooth.damp(d.optional.axis, d.toTrans.localPosition, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime);
				d.trans.localPosition = d.optional.axis + d.toInternal;
			};
			break;
		case LeanProp.x:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosX(LeanSmooth.damp(d.trans.position.x, d.toTrans.position.x, ref d.fromInternal.x, smoothTime, maxSpeed, Time.deltaTime));
			};
			break;
		case LeanProp.y:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosY(LeanSmooth.damp(d.trans.position.y, d.toTrans.position.y, ref d.fromInternal.y, smoothTime, maxSpeed, Time.deltaTime));
			};
			break;
		case LeanProp.z:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosZ(LeanSmooth.damp(d.trans.position.z, d.toTrans.position.z, ref d.fromInternal.z, smoothTime, maxSpeed, Time.deltaTime));
			};
			break;
		case LeanProp.localX:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosX(LeanSmooth.damp(d.trans.localPosition.x, d.toTrans.localPosition.x, ref d.fromInternal.x, smoothTime, maxSpeed, Time.deltaTime));
			};
			break;
		case LeanProp.localY:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosY(LeanSmooth.damp(d.trans.localPosition.y, d.toTrans.localPosition.y, ref d.fromInternal.y, smoothTime, maxSpeed, Time.deltaTime));
			};
			break;
		case LeanProp.localZ:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosZ(LeanSmooth.damp(d.trans.localPosition.z, d.toTrans.localPosition.z, ref d.fromInternal.z, smoothTime, maxSpeed, Time.deltaTime));
			};
			break;
		case LeanProp.scale:
			d.easeInternal = delegate()
			{
				d.trans.localScale = LeanSmooth.damp(d.trans.localScale, d.toTrans.localScale, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime);
			};
			break;
		case LeanProp.color:
			d.easeInternal = delegate()
			{
				Color color = LeanSmooth.damp(d.trans.LeanColor(), d.toTrans.LeanColor(), ref d.optional.color, smoothTime, maxSpeed, Time.deltaTime);
				d.trans.GetComponent<Renderer>().material.color = color;
			};
			break;
		}
		return d;
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x00048D28 File Offset: 0x00046F28
	public static LTDescr followSpring(Transform trans, Transform target, LeanProp prop, float smoothTime, float maxSpeed = -1f, float friction = 2f, float accelRate = 0.5f)
	{
		LTDescr d = LeanTween.pushNewTween(trans.gameObject, Vector3.zero, float.MaxValue, LeanTween.options().setFollow().setTarget(target));
		switch (prop)
		{
		case LeanProp.position:
			d.diff = d.trans.position;
			d.easeInternal = delegate()
			{
				d.diff = LeanSmooth.spring(d.diff, d.toTrans.position, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate);
				d.trans.position = d.diff;
			};
			break;
		case LeanProp.localPosition:
			d.optional.axis = d.trans.localPosition;
			d.easeInternal = delegate()
			{
				d.optional.axis = LeanSmooth.spring(d.optional.axis, d.toTrans.localPosition, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate);
				d.trans.localPosition = d.optional.axis + d.toInternal;
			};
			break;
		case LeanProp.x:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosX(LeanSmooth.spring(d.trans.position.x, d.toTrans.position.x, ref d.fromInternal.x, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate));
			};
			break;
		case LeanProp.y:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosY(LeanSmooth.spring(d.trans.position.y, d.toTrans.position.y, ref d.fromInternal.y, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate));
			};
			break;
		case LeanProp.z:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosZ(LeanSmooth.spring(d.trans.position.z, d.toTrans.position.z, ref d.fromInternal.z, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate));
			};
			break;
		case LeanProp.localX:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosX(LeanSmooth.spring(d.trans.localPosition.x, d.toTrans.localPosition.x, ref d.fromInternal.x, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate));
			};
			break;
		case LeanProp.localY:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosY(LeanSmooth.spring(d.trans.localPosition.y, d.toTrans.localPosition.y, ref d.fromInternal.y, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate));
			};
			break;
		case LeanProp.localZ:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosZ(LeanSmooth.spring(d.trans.localPosition.z, d.toTrans.localPosition.z, ref d.fromInternal.z, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate));
			};
			break;
		case LeanProp.scale:
			d.easeInternal = delegate()
			{
				d.trans.localScale = LeanSmooth.spring(d.trans.localScale, d.toTrans.localScale, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate);
			};
			break;
		case LeanProp.color:
			d.easeInternal = delegate()
			{
				Color color = LeanSmooth.spring(d.trans.LeanColor(), d.toTrans.LeanColor(), ref d.optional.color, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate);
				d.trans.GetComponent<Renderer>().material.color = color;
			};
			break;
		}
		return d;
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x00048F00 File Offset: 0x00047100
	public static LTDescr followBounceOut(Transform trans, Transform target, LeanProp prop, float smoothTime, float maxSpeed = -1f, float friction = 2f, float accelRate = 0.5f, float hitDamping = 0.9f)
	{
		LTDescr d = LeanTween.pushNewTween(trans.gameObject, Vector3.zero, float.MaxValue, LeanTween.options().setFollow().setTarget(target));
		switch (prop)
		{
		case LeanProp.position:
			d.easeInternal = delegate()
			{
				d.optional.axis = LeanSmooth.bounceOut(d.optional.axis, d.toTrans.position, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping);
				d.trans.position = d.optional.axis + d.toInternal;
			};
			break;
		case LeanProp.localPosition:
			d.optional.axis = d.trans.localPosition;
			d.easeInternal = delegate()
			{
				d.optional.axis = LeanSmooth.bounceOut(d.optional.axis, d.toTrans.localPosition, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping);
				d.trans.localPosition = d.optional.axis + d.toInternal;
			};
			break;
		case LeanProp.x:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosX(LeanSmooth.bounceOut(d.trans.position.x, d.toTrans.position.x, ref d.fromInternal.x, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping));
			};
			break;
		case LeanProp.y:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosY(LeanSmooth.bounceOut(d.trans.position.y, d.toTrans.position.y, ref d.fromInternal.y, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping));
			};
			break;
		case LeanProp.z:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosZ(LeanSmooth.bounceOut(d.trans.position.z, d.toTrans.position.z, ref d.fromInternal.z, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping));
			};
			break;
		case LeanProp.localX:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosX(LeanSmooth.bounceOut(d.trans.localPosition.x, d.toTrans.localPosition.x, ref d.fromInternal.x, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping));
			};
			break;
		case LeanProp.localY:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosY(LeanSmooth.bounceOut(d.trans.localPosition.y, d.toTrans.localPosition.y, ref d.fromInternal.y, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping));
			};
			break;
		case LeanProp.localZ:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosZ(LeanSmooth.bounceOut(d.trans.localPosition.z, d.toTrans.localPosition.z, ref d.fromInternal.z, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping));
			};
			break;
		case LeanProp.scale:
			d.easeInternal = delegate()
			{
				d.trans.localScale = LeanSmooth.bounceOut(d.trans.localScale, d.toTrans.localScale, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping);
			};
			break;
		case LeanProp.color:
			d.easeInternal = delegate()
			{
				Color color = LeanSmooth.bounceOut(d.trans.LeanColor(), d.toTrans.LeanColor(), ref d.optional.color, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping);
				d.trans.GetComponent<Renderer>().material.color = color;
			};
			break;
		}
		return d;
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x000490C4 File Offset: 0x000472C4
	public static LTDescr followLinear(Transform trans, Transform target, LeanProp prop, float moveSpeed)
	{
		LTDescr d = LeanTween.pushNewTween(trans.gameObject, Vector3.zero, float.MaxValue, LeanTween.options().setFollow().setTarget(target));
		switch (prop)
		{
		case LeanProp.position:
			d.easeInternal = delegate()
			{
				d.trans.position = LeanSmooth.linear(d.trans.position, d.toTrans.position, moveSpeed, -1f);
			};
			break;
		case LeanProp.localPosition:
			d.optional.axis = d.trans.localPosition;
			d.easeInternal = delegate()
			{
				d.optional.axis = LeanSmooth.linear(d.optional.axis, d.toTrans.localPosition, moveSpeed, -1f);
				d.trans.localPosition = d.optional.axis + d.toInternal;
			};
			break;
		case LeanProp.x:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosX(LeanSmooth.linear(d.trans.position.x, d.toTrans.position.x, moveSpeed, -1f));
			};
			break;
		case LeanProp.y:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosY(LeanSmooth.linear(d.trans.position.y, d.toTrans.position.y, moveSpeed, -1f));
			};
			break;
		case LeanProp.z:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosZ(LeanSmooth.linear(d.trans.position.z, d.toTrans.position.z, moveSpeed, -1f));
			};
			break;
		case LeanProp.localX:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosX(LeanSmooth.linear(d.trans.localPosition.x, d.toTrans.localPosition.x, moveSpeed, -1f));
			};
			break;
		case LeanProp.localY:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosY(LeanSmooth.linear(d.trans.localPosition.y, d.toTrans.localPosition.y, moveSpeed, -1f));
			};
			break;
		case LeanProp.localZ:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosZ(LeanSmooth.linear(d.trans.localPosition.z, d.toTrans.localPosition.z, moveSpeed, -1f));
			};
			break;
		case LeanProp.scale:
			d.easeInternal = delegate()
			{
				d.trans.localScale = LeanSmooth.linear(d.trans.localScale, d.toTrans.localScale, moveSpeed, -1f);
			};
			break;
		case LeanProp.color:
			d.easeInternal = delegate()
			{
				Color color = LeanSmooth.linear(d.trans.LeanColor(), d.toTrans.LeanColor(), moveSpeed);
				d.trans.GetComponent<Renderer>().material.color = color;
			};
			break;
		}
		return d;
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x0000914F File Offset: 0x0000734F
	public static void addListener(int eventId, Action<LTEvent> callback)
	{
		LeanTween.addListener(LeanTween.tweenEmpty, eventId, callback);
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x00049268 File Offset: 0x00047468
	public static void addListener(GameObject caller, int eventId, Action<LTEvent> callback)
	{
		if (LeanTween.eventListeners == null)
		{
			LeanTween.INIT_LISTENERS_MAX = LeanTween.LISTENERS_MAX;
			LeanTween.eventListeners = new Action<LTEvent>[LeanTween.EVENTS_MAX * LeanTween.LISTENERS_MAX];
			LeanTween.goListeners = new GameObject[LeanTween.EVENTS_MAX * LeanTween.LISTENERS_MAX];
		}
		LeanTween.i = 0;
		while (LeanTween.i < LeanTween.INIT_LISTENERS_MAX)
		{
			int num = eventId * LeanTween.INIT_LISTENERS_MAX + LeanTween.i;
			if (LeanTween.goListeners[num] == null || LeanTween.eventListeners[num] == null)
			{
				LeanTween.eventListeners[num] = callback;
				LeanTween.goListeners[num] = caller;
				if (LeanTween.i >= LeanTween.eventsMaxSearch)
				{
					LeanTween.eventsMaxSearch = LeanTween.i + 1;
				}
				return;
			}
			if (LeanTween.goListeners[num] == caller && object.Equals(LeanTween.eventListeners[num], callback))
			{
				return;
			}
			LeanTween.i++;
		}
		Debug.LogError("You ran out of areas to add listeners, consider increasing LISTENERS_MAX, ex: LeanTween.LISTENERS_MAX = " + (LeanTween.LISTENERS_MAX * 2).ToString());
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x0000915D File Offset: 0x0000735D
	public static bool removeListener(int eventId, Action<LTEvent> callback)
	{
		return LeanTween.removeListener(LeanTween.tweenEmpty, eventId, callback);
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x00049360 File Offset: 0x00047560
	public static bool removeListener(int eventId)
	{
		int num = eventId * LeanTween.INIT_LISTENERS_MAX + LeanTween.i;
		LeanTween.eventListeners[num] = null;
		LeanTween.goListeners[num] = null;
		return true;
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x0004938C File Offset: 0x0004758C
	public static bool removeListener(GameObject caller, int eventId, Action<LTEvent> callback)
	{
		LeanTween.i = 0;
		while (LeanTween.i < LeanTween.eventsMaxSearch)
		{
			int num = eventId * LeanTween.INIT_LISTENERS_MAX + LeanTween.i;
			if (LeanTween.goListeners[num] == caller && object.Equals(LeanTween.eventListeners[num], callback))
			{
				LeanTween.eventListeners[num] = null;
				LeanTween.goListeners[num] = null;
				return true;
			}
			LeanTween.i++;
		}
		return false;
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x0000916B File Offset: 0x0000736B
	public static void dispatchEvent(int eventId)
	{
		LeanTween.dispatchEvent(eventId, null);
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x000493F8 File Offset: 0x000475F8
	public static void dispatchEvent(int eventId, object data)
	{
		for (int i = 0; i < LeanTween.eventsMaxSearch; i++)
		{
			int num = eventId * LeanTween.INIT_LISTENERS_MAX + i;
			if (LeanTween.eventListeners[num] != null)
			{
				if (LeanTween.goListeners[num])
				{
					LeanTween.eventListeners[num](new LTEvent(eventId, data));
				}
				else
				{
					LeanTween.eventListeners[num] = null;
				}
			}
		}
	}

	// Token: 0x040005F2 RID: 1522
	public static bool throwErrors = true;

	// Token: 0x040005F3 RID: 1523
	public static float tau = 6.2831855f;

	// Token: 0x040005F4 RID: 1524
	public static float PI_DIV2 = 1.5707964f;

	// Token: 0x040005F5 RID: 1525
	private static LTSeq[] sequences;

	// Token: 0x040005F6 RID: 1526
	private static LTDescr[] tweens;

	// Token: 0x040005F7 RID: 1527
	private static int[] tweensFinished;

	// Token: 0x040005F8 RID: 1528
	private static int[] tweensFinishedIds;

	// Token: 0x040005F9 RID: 1529
	private static LTDescr tween;

	// Token: 0x040005FA RID: 1530
	private static int tweenMaxSearch = -1;

	// Token: 0x040005FB RID: 1531
	private static int maxTweens = 1300;

	// Token: 0x040005FC RID: 1532
	private static int maxSequences = 400;

	// Token: 0x040005FD RID: 1533
	private static int frameRendered = -1;

	// Token: 0x040005FE RID: 1534
	private static GameObject _tweenEmpty;

	// Token: 0x040005FF RID: 1535
	public static float dtEstimated = -1f;

	// Token: 0x04000600 RID: 1536
	public static float dtManual;

	// Token: 0x04000601 RID: 1537
	public static float dtActual;

	// Token: 0x04000602 RID: 1538
	private static uint global_counter = 0U;

	// Token: 0x04000603 RID: 1539
	private static int i;

	// Token: 0x04000604 RID: 1540
	private static int j;

	// Token: 0x04000605 RID: 1541
	private static int finishedCnt;

	// Token: 0x04000606 RID: 1542
	public static AnimationCurve punch = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.112586f, 0.9976035f),
		new Keyframe(0.3120486f, -0.1720615f),
		new Keyframe(0.4316337f, 0.07030682f),
		new Keyframe(0.5524869f, -0.03141804f),
		new Keyframe(0.6549395f, 0.003909959f),
		new Keyframe(0.770987f, -0.009817753f),
		new Keyframe(0.8838775f, 0.001939224f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x04000607 RID: 1543
	public static AnimationCurve shake = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.25f, 1f),
		new Keyframe(0.75f, -1f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x04000608 RID: 1544
	private static int maxTweenReached;

	// Token: 0x04000609 RID: 1545
	public static int startSearch = 0;

	// Token: 0x0400060A RID: 1546
	public static LTDescr d;

	// Token: 0x0400060B RID: 1547
	private static Action<LTEvent>[] eventListeners;

	// Token: 0x0400060C RID: 1548
	private static GameObject[] goListeners;

	// Token: 0x0400060D RID: 1549
	private static int eventsMaxSearch = 0;

	// Token: 0x0400060E RID: 1550
	public static int EVENTS_MAX = 10;

	// Token: 0x0400060F RID: 1551
	public static int LISTENERS_MAX = 10;

	// Token: 0x04000610 RID: 1552
	private static int INIT_LISTENERS_MAX = LeanTween.LISTENERS_MAX;
}
