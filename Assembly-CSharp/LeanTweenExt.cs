using System;
using UnityEngine;

// Token: 0x0200010A RID: 266
public static class LeanTweenExt
{
	// Token: 0x060007B5 RID: 1973 RVA: 0x0000950E File Offset: 0x0000770E
	public static LTDescr LeanAlpha(this GameObject gameObject, float to, float time)
	{
		return LeanTween.alpha(gameObject, to, time);
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x00009518 File Offset: 0x00007718
	public static LTDescr LeanAlphaVertex(this GameObject gameObject, float to, float time)
	{
		return LeanTween.alphaVertex(gameObject, to, time);
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x00009522 File Offset: 0x00007722
	public static LTDescr LeanAlpha(this RectTransform rectTransform, float to, float time)
	{
		return LeanTween.alpha(rectTransform, to, time);
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x0000952C File Offset: 0x0000772C
	public static LTDescr LeanAlpha(this CanvasGroup canvas, float to, float time)
	{
		return LeanTween.alphaCanvas(canvas, to, time);
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x00009536 File Offset: 0x00007736
	public static LTDescr LeanAlphaText(this RectTransform rectTransform, float to, float time)
	{
		return LeanTween.alphaText(rectTransform, to, time);
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x00009540 File Offset: 0x00007740
	public static void LeanCancel(this GameObject gameObject)
	{
		LeanTween.cancel(gameObject);
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x00009548 File Offset: 0x00007748
	public static void LeanCancel(this GameObject gameObject, bool callOnComplete)
	{
		LeanTween.cancel(gameObject, callOnComplete);
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x00009551 File Offset: 0x00007751
	public static void LeanCancel(this GameObject gameObject, int uniqueId, bool callOnComplete = false)
	{
		LeanTween.cancel(gameObject, uniqueId, callOnComplete);
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x0000955B File Offset: 0x0000775B
	public static void LeanCancel(this RectTransform rectTransform)
	{
		LeanTween.cancel(rectTransform);
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x00009563 File Offset: 0x00007763
	public static LTDescr LeanColor(this GameObject gameObject, Color to, float time)
	{
		return LeanTween.color(gameObject, to, time);
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x0000956D File Offset: 0x0000776D
	public static LTDescr LeanColorText(this RectTransform rectTransform, Color to, float time)
	{
		return LeanTween.colorText(rectTransform, to, time);
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x00009577 File Offset: 0x00007777
	public static LTDescr LeanDelayedCall(this GameObject gameObject, float delayTime, Action callback)
	{
		return LeanTween.delayedCall(gameObject, delayTime, callback);
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x00009581 File Offset: 0x00007781
	public static LTDescr LeanDelayedCall(this GameObject gameObject, float delayTime, Action<object> callback)
	{
		return LeanTween.delayedCall(gameObject, delayTime, callback);
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x0000958B File Offset: 0x0000778B
	public static bool LeanIsPaused(this GameObject gameObject)
	{
		return LeanTween.isPaused(gameObject);
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x00009593 File Offset: 0x00007793
	public static bool LeanIsPaused(this RectTransform rectTransform)
	{
		return LeanTween.isPaused(rectTransform);
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x0000959B File Offset: 0x0000779B
	public static bool LeanIsTweening(this GameObject gameObject)
	{
		return LeanTween.isTweening(gameObject);
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x000095A3 File Offset: 0x000077A3
	public static LTDescr LeanMove(this GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.move(gameObject, to, time);
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x000095AD File Offset: 0x000077AD
	public static LTDescr LeanMove(this Transform transform, Vector3 to, float time)
	{
		return LeanTween.move(transform.gameObject, to, time);
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x000095BC File Offset: 0x000077BC
	public static LTDescr LeanMove(this RectTransform rectTransform, Vector3 to, float time)
	{
		return LeanTween.move(rectTransform, to, time);
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x000095C6 File Offset: 0x000077C6
	public static LTDescr LeanMove(this GameObject gameObject, Vector2 to, float time)
	{
		return LeanTween.move(gameObject, to, time);
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x000095D0 File Offset: 0x000077D0
	public static LTDescr LeanMove(this Transform transform, Vector2 to, float time)
	{
		return LeanTween.move(transform.gameObject, to, time);
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x000095DF File Offset: 0x000077DF
	public static LTDescr LeanMove(this GameObject gameObject, Vector3[] to, float time)
	{
		return LeanTween.move(gameObject, to, time);
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x000095E9 File Offset: 0x000077E9
	public static LTDescr LeanMove(this GameObject gameObject, LTBezierPath to, float time)
	{
		return LeanTween.move(gameObject, to, time);
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x000095F3 File Offset: 0x000077F3
	public static LTDescr LeanMove(this GameObject gameObject, LTSpline to, float time)
	{
		return LeanTween.move(gameObject, to, time);
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x000095FD File Offset: 0x000077FD
	public static LTDescr LeanMove(this Transform transform, Vector3[] to, float time)
	{
		return LeanTween.move(transform.gameObject, to, time);
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x0000960C File Offset: 0x0000780C
	public static LTDescr LeanMove(this Transform transform, LTBezierPath to, float time)
	{
		return LeanTween.move(transform.gameObject, to, time);
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x0000961B File Offset: 0x0000781B
	public static LTDescr LeanMove(this Transform transform, LTSpline to, float time)
	{
		return LeanTween.move(transform.gameObject, to, time);
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x0000962A File Offset: 0x0000782A
	public static LTDescr LeanMoveLocal(this GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.moveLocal(gameObject, to, time);
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x00009634 File Offset: 0x00007834
	public static LTDescr LeanMoveLocal(this GameObject gameObject, LTBezierPath to, float time)
	{
		return LeanTween.moveLocal(gameObject, to, time);
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x0000963E File Offset: 0x0000783E
	public static LTDescr LeanMoveLocal(this GameObject gameObject, LTSpline to, float time)
	{
		return LeanTween.moveLocal(gameObject, to, time);
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x00009648 File Offset: 0x00007848
	public static LTDescr LeanMoveLocal(this Transform transform, Vector3 to, float time)
	{
		return LeanTween.moveLocal(transform.gameObject, to, time);
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x00009657 File Offset: 0x00007857
	public static LTDescr LeanMoveLocal(this Transform transform, LTBezierPath to, float time)
	{
		return LeanTween.moveLocal(transform.gameObject, to, time);
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x00009666 File Offset: 0x00007866
	public static LTDescr LeanMoveLocal(this Transform transform, LTSpline to, float time)
	{
		return LeanTween.moveLocal(transform.gameObject, to, time);
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x00009675 File Offset: 0x00007875
	public static LTDescr LeanMoveLocalX(this GameObject gameObject, float to, float time)
	{
		return LeanTween.moveLocalX(gameObject, to, time);
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x0000967F File Offset: 0x0000787F
	public static LTDescr LeanMoveLocalY(this GameObject gameObject, float to, float time)
	{
		return LeanTween.moveLocalY(gameObject, to, time);
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x00009689 File Offset: 0x00007889
	public static LTDescr LeanMoveLocalZ(this GameObject gameObject, float to, float time)
	{
		return LeanTween.moveLocalZ(gameObject, to, time);
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x00009693 File Offset: 0x00007893
	public static LTDescr LeanMoveLocalX(this Transform transform, float to, float time)
	{
		return LeanTween.moveLocalX(transform.gameObject, to, time);
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x000096A2 File Offset: 0x000078A2
	public static LTDescr LeanMoveLocalY(this Transform transform, float to, float time)
	{
		return LeanTween.moveLocalY(transform.gameObject, to, time);
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x000096B1 File Offset: 0x000078B1
	public static LTDescr LeanMoveLocalZ(this Transform transform, float to, float time)
	{
		return LeanTween.moveLocalZ(transform.gameObject, to, time);
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x000096C0 File Offset: 0x000078C0
	public static LTDescr LeanMoveSpline(this GameObject gameObject, Vector3[] to, float time)
	{
		return LeanTween.moveSpline(gameObject, to, time);
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x000096CA File Offset: 0x000078CA
	public static LTDescr LeanMoveSpline(this GameObject gameObject, LTSpline to, float time)
	{
		return LeanTween.moveSpline(gameObject, to, time);
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x000096D4 File Offset: 0x000078D4
	public static LTDescr LeanMoveSpline(this Transform transform, Vector3[] to, float time)
	{
		return LeanTween.moveSpline(transform.gameObject, to, time);
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x000096E3 File Offset: 0x000078E3
	public static LTDescr LeanMoveSpline(this Transform transform, LTSpline to, float time)
	{
		return LeanTween.moveSpline(transform.gameObject, to, time);
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x000096F2 File Offset: 0x000078F2
	public static LTDescr LeanMoveSplineLocal(this GameObject gameObject, Vector3[] to, float time)
	{
		return LeanTween.moveSplineLocal(gameObject, to, time);
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x000096FC File Offset: 0x000078FC
	public static LTDescr LeanMoveSplineLocal(this Transform transform, Vector3[] to, float time)
	{
		return LeanTween.moveSplineLocal(transform.gameObject, to, time);
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x0000970B File Offset: 0x0000790B
	public static LTDescr LeanMoveX(this GameObject gameObject, float to, float time)
	{
		return LeanTween.moveX(gameObject, to, time);
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x00009715 File Offset: 0x00007915
	public static LTDescr LeanMoveX(this Transform transform, float to, float time)
	{
		return LeanTween.moveX(transform.gameObject, to, time);
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x00009724 File Offset: 0x00007924
	public static LTDescr LeanMoveX(this RectTransform rectTransform, float to, float time)
	{
		return LeanTween.moveX(rectTransform, to, time);
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x0000972E File Offset: 0x0000792E
	public static LTDescr LeanMoveY(this GameObject gameObject, float to, float time)
	{
		return LeanTween.moveY(gameObject, to, time);
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x00009738 File Offset: 0x00007938
	public static LTDescr LeanMoveY(this Transform transform, float to, float time)
	{
		return LeanTween.moveY(transform.gameObject, to, time);
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x00009747 File Offset: 0x00007947
	public static LTDescr LeanMoveY(this RectTransform rectTransform, float to, float time)
	{
		return LeanTween.moveY(rectTransform, to, time);
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x00009751 File Offset: 0x00007951
	public static LTDescr LeanMoveZ(this GameObject gameObject, float to, float time)
	{
		return LeanTween.moveZ(gameObject, to, time);
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x0000975B File Offset: 0x0000795B
	public static LTDescr LeanMoveZ(this Transform transform, float to, float time)
	{
		return LeanTween.moveZ(transform.gameObject, to, time);
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x0000976A File Offset: 0x0000796A
	public static LTDescr LeanMoveZ(this RectTransform rectTransform, float to, float time)
	{
		return LeanTween.moveZ(rectTransform, to, time);
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x00009774 File Offset: 0x00007974
	public static void LeanPause(this GameObject gameObject)
	{
		LeanTween.pause(gameObject);
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x0000977C File Offset: 0x0000797C
	public static LTDescr LeanPlay(this RectTransform rectTransform, Sprite[] sprites)
	{
		return LeanTween.play(rectTransform, sprites);
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x00009785 File Offset: 0x00007985
	public static void LeanResume(this GameObject gameObject)
	{
		LeanTween.resume(gameObject);
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x0000978D File Offset: 0x0000798D
	public static LTDescr LeanRotate(this GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.rotate(gameObject, to, time);
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x00009797 File Offset: 0x00007997
	public static LTDescr LeanRotate(this Transform transform, Vector3 to, float time)
	{
		return LeanTween.rotate(transform.gameObject, to, time);
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x000097A6 File Offset: 0x000079A6
	public static LTDescr LeanRotate(this RectTransform rectTransform, Vector3 to, float time)
	{
		return LeanTween.rotate(rectTransform, to, time);
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x000097B0 File Offset: 0x000079B0
	public static LTDescr LeanRotateAround(this GameObject gameObject, Vector3 axis, float add, float time)
	{
		return LeanTween.rotateAround(gameObject, axis, add, time);
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x000097BB File Offset: 0x000079BB
	public static LTDescr LeanRotateAround(this Transform transform, Vector3 axis, float add, float time)
	{
		return LeanTween.rotateAround(transform.gameObject, axis, add, time);
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x000097CB File Offset: 0x000079CB
	public static LTDescr LeanRotateAround(this RectTransform rectTransform, Vector3 axis, float add, float time)
	{
		return LeanTween.rotateAround(rectTransform, axis, add, time);
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x000097D6 File Offset: 0x000079D6
	public static LTDescr LeanRotateAroundLocal(this GameObject gameObject, Vector3 axis, float add, float time)
	{
		return LeanTween.rotateAroundLocal(gameObject, axis, add, time);
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x000097E1 File Offset: 0x000079E1
	public static LTDescr LeanRotateAroundLocal(this Transform transform, Vector3 axis, float add, float time)
	{
		return LeanTween.rotateAroundLocal(transform.gameObject, axis, add, time);
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x000097F1 File Offset: 0x000079F1
	public static LTDescr LeanRotateAroundLocal(this RectTransform rectTransform, Vector3 axis, float add, float time)
	{
		return LeanTween.rotateAroundLocal(rectTransform, axis, add, time);
	}

	// Token: 0x060007F7 RID: 2039 RVA: 0x000097FC File Offset: 0x000079FC
	public static LTDescr LeanRotateX(this GameObject gameObject, float to, float time)
	{
		return LeanTween.rotateX(gameObject, to, time);
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x00009806 File Offset: 0x00007A06
	public static LTDescr LeanRotateX(this Transform transform, float to, float time)
	{
		return LeanTween.rotateX(transform.gameObject, to, time);
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x00009815 File Offset: 0x00007A15
	public static LTDescr LeanRotateY(this GameObject gameObject, float to, float time)
	{
		return LeanTween.rotateY(gameObject, to, time);
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x0000981F File Offset: 0x00007A1F
	public static LTDescr LeanRotateY(this Transform transform, float to, float time)
	{
		return LeanTween.rotateY(transform.gameObject, to, time);
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x0000982E File Offset: 0x00007A2E
	public static LTDescr LeanRotateZ(this GameObject gameObject, float to, float time)
	{
		return LeanTween.rotateZ(gameObject, to, time);
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x00009838 File Offset: 0x00007A38
	public static LTDescr LeanRotateZ(this Transform transform, float to, float time)
	{
		return LeanTween.rotateZ(transform.gameObject, to, time);
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x00009847 File Offset: 0x00007A47
	public static LTDescr LeanScale(this GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.scale(gameObject, to, time);
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x00009851 File Offset: 0x00007A51
	public static LTDescr LeanScale(this Transform transform, Vector3 to, float time)
	{
		return LeanTween.scale(transform.gameObject, to, time);
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x00009860 File Offset: 0x00007A60
	public static LTDescr LeanScale(this RectTransform rectTransform, Vector3 to, float time)
	{
		return LeanTween.scale(rectTransform, to, time);
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x0000986A File Offset: 0x00007A6A
	public static LTDescr LeanScaleX(this GameObject gameObject, float to, float time)
	{
		return LeanTween.scaleX(gameObject, to, time);
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x00009874 File Offset: 0x00007A74
	public static LTDescr LeanScaleX(this Transform transform, float to, float time)
	{
		return LeanTween.scaleX(transform.gameObject, to, time);
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x00009883 File Offset: 0x00007A83
	public static LTDescr LeanScaleY(this GameObject gameObject, float to, float time)
	{
		return LeanTween.scaleY(gameObject, to, time);
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x0000988D File Offset: 0x00007A8D
	public static LTDescr LeanScaleY(this Transform transform, float to, float time)
	{
		return LeanTween.scaleY(transform.gameObject, to, time);
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x0000989C File Offset: 0x00007A9C
	public static LTDescr LeanScaleZ(this GameObject gameObject, float to, float time)
	{
		return LeanTween.scaleZ(gameObject, to, time);
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x000098A6 File Offset: 0x00007AA6
	public static LTDescr LeanScaleZ(this Transform transform, float to, float time)
	{
		return LeanTween.scaleZ(transform.gameObject, to, time);
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x000098B5 File Offset: 0x00007AB5
	public static LTDescr LeanSize(this RectTransform rectTransform, Vector2 to, float time)
	{
		return LeanTween.size(rectTransform, to, time);
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x000098BF File Offset: 0x00007ABF
	public static LTDescr LeanValue(this GameObject gameObject, Color from, Color to, float time)
	{
		return LeanTween.value(gameObject, from, to, time);
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x000098CA File Offset: 0x00007ACA
	public static LTDescr LeanValue(this GameObject gameObject, float from, float to, float time)
	{
		return LeanTween.value(gameObject, from, to, time);
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x000098D5 File Offset: 0x00007AD5
	public static LTDescr LeanValue(this GameObject gameObject, Vector2 from, Vector2 to, float time)
	{
		return LeanTween.value(gameObject, from, to, time);
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x000098E0 File Offset: 0x00007AE0
	public static LTDescr LeanValue(this GameObject gameObject, Vector3 from, Vector3 to, float time)
	{
		return LeanTween.value(gameObject, from, to, time);
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x000098EB File Offset: 0x00007AEB
	public static LTDescr LeanValue(this GameObject gameObject, Action<float> callOnUpdate, float from, float to, float time)
	{
		return LeanTween.value(gameObject, callOnUpdate, from, to, time);
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x000098F8 File Offset: 0x00007AF8
	public static LTDescr LeanValue(this GameObject gameObject, Action<float, float> callOnUpdate, float from, float to, float time)
	{
		return LeanTween.value(gameObject, callOnUpdate, from, to, time);
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x00009905 File Offset: 0x00007B05
	public static LTDescr LeanValue(this GameObject gameObject, Action<float, object> callOnUpdate, float from, float to, float time)
	{
		return LeanTween.value(gameObject, callOnUpdate, from, to, time);
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x00009912 File Offset: 0x00007B12
	public static LTDescr LeanValue(this GameObject gameObject, Action<Color> callOnUpdate, Color from, Color to, float time)
	{
		return LeanTween.value(gameObject, callOnUpdate, from, to, time);
	}

	// Token: 0x0600080F RID: 2063 RVA: 0x0000991F File Offset: 0x00007B1F
	public static LTDescr LeanValue(this GameObject gameObject, Action<Vector2> callOnUpdate, Vector2 from, Vector2 to, float time)
	{
		return LeanTween.value(gameObject, callOnUpdate, from, to, time);
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x0000992C File Offset: 0x00007B2C
	public static LTDescr LeanValue(this GameObject gameObject, Action<Vector3> callOnUpdate, Vector3 from, Vector3 to, float time)
	{
		return LeanTween.value(gameObject, callOnUpdate, from, to, time);
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x00009939 File Offset: 0x00007B39
	public static void LeanSetPosX(this Transform transform, float val)
	{
		transform.position = new Vector3(val, transform.position.y, transform.position.z);
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x0000995D File Offset: 0x00007B5D
	public static void LeanSetPosY(this Transform transform, float val)
	{
		transform.position = new Vector3(transform.position.x, val, transform.position.z);
	}

	// Token: 0x06000813 RID: 2067 RVA: 0x00009981 File Offset: 0x00007B81
	public static void LeanSetPosZ(this Transform transform, float val)
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, val);
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x000099A5 File Offset: 0x00007BA5
	public static void LeanSetLocalPosX(this Transform transform, float val)
	{
		transform.localPosition = new Vector3(val, transform.localPosition.y, transform.localPosition.z);
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x000099C9 File Offset: 0x00007BC9
	public static void LeanSetLocalPosY(this Transform transform, float val)
	{
		transform.localPosition = new Vector3(transform.localPosition.x, val, transform.localPosition.z);
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x000099ED File Offset: 0x00007BED
	public static void LeanSetLocalPosZ(this Transform transform, float val)
	{
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, val);
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x00009A11 File Offset: 0x00007C11
	public static Color LeanColor(this Transform transform)
	{
		return transform.GetComponent<Renderer>().material.color;
	}
}
