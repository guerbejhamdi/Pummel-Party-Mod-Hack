using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000E4 RID: 228
public class TestingZLegacy : MonoBehaviour
{
	// Token: 0x0600047B RID: 1147 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Awake()
	{
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x00006842 File Offset: 0x00004A42
	private void Start()
	{
		this.ltLogo = GameObject.Find("LeanTweenLogo");
		LeanTween.delayedCall(1f, new Action(this.cycleThroughExamples));
		this.origin = this.ltLogo.transform.position;
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x00006881 File Offset: 0x00004A81
	private void pauseNow()
	{
		Time.timeScale = 0f;
		Debug.Log("pausing");
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x0004042C File Offset: 0x0003E62C
	private void OnGUI()
	{
		string text = this.useEstimatedTime ? "useEstimatedTime" : ("timeScale:" + Time.timeScale.ToString());
		GUI.Label(new Rect(0.03f * (float)Screen.width, 0.03f * (float)Screen.height, 0.5f * (float)Screen.width, 0.3f * (float)Screen.height), text);
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x00006897 File Offset: 0x00004A97
	private void endlessCallback()
	{
		Debug.Log("endless");
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x0004049C File Offset: 0x0003E69C
	private void cycleThroughExamples()
	{
		if (this.exampleIter == 0)
		{
			int num = (int)(this.timingType + 1);
			if (num > 4)
			{
				num = 0;
			}
			this.timingType = (TestingZLegacy.TimingType)num;
			this.useEstimatedTime = (this.timingType == TestingZLegacy.TimingType.IgnoreTimeScale);
			Time.timeScale = (this.useEstimatedTime ? 0f : 1f);
			if (this.timingType == TestingZLegacy.TimingType.HalfTimeScale)
			{
				Time.timeScale = 0.5f;
			}
			if (this.timingType == TestingZLegacy.TimingType.VariableTimeScale)
			{
				this.descrTimeScaleChangeId = LeanTween.value(base.gameObject, 0.01f, 10f, 3f).setOnUpdate(delegate(float val)
				{
					Time.timeScale = val;
				}).setEase(LeanTweenType.easeInQuad).setUseEstimatedTime(true).setRepeat(-1).id;
			}
			else
			{
				Debug.Log("cancel variable time");
				LeanTween.cancel(this.descrTimeScaleChangeId);
			}
		}
		base.gameObject.BroadcastMessage(this.exampleFunctions[this.exampleIter]);
		float delayTime = 1.1f;
		LeanTween.delayedCall(base.gameObject, delayTime, new Action(this.cycleThroughExamples)).setUseEstimatedTime(this.useEstimatedTime);
		this.exampleIter = ((this.exampleIter + 1 >= this.exampleFunctions.Length) ? 0 : (this.exampleIter + 1));
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x000405E8 File Offset: 0x0003E7E8
	public void updateValue3Example()
	{
		Debug.Log("updateValue3Example Time:" + Time.time.ToString());
		LeanTween.value(base.gameObject, new Action<Vector3>(this.updateValue3ExampleCallback), new Vector3(0f, 270f, 0f), new Vector3(30f, 270f, 180f), 0.5f).setEase(LeanTweenType.easeInBounce).setRepeat(2).setLoopPingPong().setOnUpdateVector3(new Action<Vector3>(this.updateValue3ExampleUpdate)).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x0000398C File Offset: 0x00001B8C
	public void updateValue3ExampleUpdate(Vector3 val)
	{
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x000068A3 File Offset: 0x00004AA3
	public void updateValue3ExampleCallback(Vector3 val)
	{
		this.ltLogo.transform.eulerAngles = val;
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x00040684 File Offset: 0x0003E884
	public void loopTestClamp()
	{
		Debug.Log("loopTestClamp Time:" + Time.time.ToString());
		GameObject gameObject = GameObject.Find("Cube1");
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		LeanTween.scaleZ(gameObject, 4f, 1f).setEase(LeanTweenType.easeOutElastic).setRepeat(7).setLoopClamp().setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x00040704 File Offset: 0x0003E904
	public void loopTestPingPong()
	{
		Debug.Log("loopTestPingPong Time:" + Time.time.ToString());
		GameObject gameObject = GameObject.Find("Cube2");
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		LeanTween.scaleY(gameObject, 4f, 1f).setEase(LeanTweenType.easeOutQuad).setLoopPingPong(4).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x00040780 File Offset: 0x0003E980
	public void colorExample()
	{
		LeanTween.color(GameObject.Find("LCharacter"), new Color(1f, 0f, 0f, 0.5f), 0.5f).setEase(LeanTweenType.easeOutBounce).setRepeat(2).setLoopPingPong().setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x000407D8 File Offset: 0x0003E9D8
	public void moveOnACurveExample()
	{
		Debug.Log("moveOnACurveExample Time:" + Time.time.ToString());
		Vector3[] to = new Vector3[]
		{
			this.origin,
			this.pt1.position,
			this.pt2.position,
			this.pt3.position,
			this.pt3.position,
			this.pt4.position,
			this.pt5.position,
			this.origin
		};
		LeanTween.move(this.ltLogo, to, 1f).setEase(LeanTweenType.easeOutQuad).setOrientToPath(true).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x000408B8 File Offset: 0x0003EAB8
	public void customTweenExample()
	{
		string str = "customTweenExample starting pos:";
		string str2 = this.ltLogo.transform.position.ToString();
		string str3 = " origin:";
		Vector3 vector = this.origin;
		Debug.Log(str + str2 + str3 + vector.ToString());
		LeanTween.moveX(this.ltLogo, -10f, 0.5f).setEase(this.customAnimationCurve).setUseEstimatedTime(this.useEstimatedTime);
		LeanTween.moveX(this.ltLogo, 0f, 0.5f).setEase(this.customAnimationCurve).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x00040970 File Offset: 0x0003EB70
	public void moveExample()
	{
		Debug.Log("moveExample");
		LeanTween.move(this.ltLogo, new Vector3(-2f, -1f, 0f), 0.5f).setUseEstimatedTime(this.useEstimatedTime);
		LeanTween.move(this.ltLogo, this.origin, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x000409E4 File Offset: 0x0003EBE4
	public void rotateExample()
	{
		Debug.Log("rotateExample");
		Hashtable hashtable = new Hashtable();
		hashtable.Add("yo", 5.0);
		LeanTween.rotate(this.ltLogo, new Vector3(0f, 360f, 0f), 1f).setEase(LeanTweenType.easeOutQuad).setOnComplete(new Action<object>(this.rotateFinished)).setOnCompleteParam(hashtable).setOnUpdate(new Action<float>(this.rotateOnUpdate)).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x0000398C File Offset: 0x00001B8C
	public void rotateOnUpdate(float val)
	{
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x00040A78 File Offset: 0x0003EC78
	public void rotateFinished(object hash)
	{
		Hashtable hashtable = hash as Hashtable;
		string str = "rotateFinished hash:";
		object obj = hashtable["yo"];
		Debug.Log(str + ((obj != null) ? obj.ToString() : null));
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x00040AB4 File Offset: 0x0003ECB4
	public void scaleExample()
	{
		Debug.Log("scaleExample");
		Vector3 localScale = this.ltLogo.transform.localScale;
		LeanTween.scale(this.ltLogo, new Vector3(localScale.x + 0.2f, localScale.y + 0.2f, localScale.z + 0.2f), 1f).setEase(LeanTweenType.easeOutBounce).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x00040B28 File Offset: 0x0003ED28
	public void updateValueExample()
	{
		Debug.Log("updateValueExample");
		Hashtable hashtable = new Hashtable();
		hashtable.Add("message", "hi");
		LeanTween.value(base.gameObject, new Action<float, object>(this.updateValueExampleCallback), this.ltLogo.transform.eulerAngles.y, 270f, 1f).setEase(LeanTweenType.easeOutElastic).setOnUpdateParam(hashtable).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x00040BA4 File Offset: 0x0003EDA4
	public void updateValueExampleCallback(float val, object hash)
	{
		Vector3 eulerAngles = this.ltLogo.transform.eulerAngles;
		eulerAngles.y = val;
		this.ltLogo.transform.eulerAngles = eulerAngles;
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x000068B6 File Offset: 0x00004AB6
	public void delayedCallExample()
	{
		Debug.Log("delayedCallExample");
		LeanTween.delayedCall(0.5f, new Action(this.delayedCallExampleCallback)).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x00040BDC File Offset: 0x0003EDDC
	public void delayedCallExampleCallback()
	{
		Debug.Log("Delayed function was called");
		Vector3 localScale = this.ltLogo.transform.localScale;
		LeanTween.scale(this.ltLogo, new Vector3(localScale.x - 0.2f, localScale.y - 0.2f, localScale.z - 0.2f), 0.5f).setEase(LeanTweenType.easeInOutCirc).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x00040C50 File Offset: 0x0003EE50
	public void alphaExample()
	{
		Debug.Log("alphaExample");
		GameObject gameObject = GameObject.Find("LCharacter");
		LeanTween.alpha(gameObject, 0f, 0.5f).setUseEstimatedTime(this.useEstimatedTime);
		LeanTween.alpha(gameObject, 1f, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x00040CB4 File Offset: 0x0003EEB4
	public void moveLocalExample()
	{
		Debug.Log("moveLocalExample");
		GameObject gameObject = GameObject.Find("LCharacter");
		Vector3 localPosition = gameObject.transform.localPosition;
		LeanTween.moveLocal(gameObject, new Vector3(0f, 2f, 0f), 0.5f).setUseEstimatedTime(this.useEstimatedTime);
		LeanTween.moveLocal(gameObject, localPosition, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x000068E4 File Offset: 0x00004AE4
	public void rotateAroundExample()
	{
		Debug.Log("rotateAroundExample");
		LeanTween.rotateAround(GameObject.Find("LCharacter"), Vector3.up, 360f, 1f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x0000691A File Offset: 0x00004B1A
	public void loopPause()
	{
		LeanTween.pause(GameObject.Find("Cube1"));
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x0000692B File Offset: 0x00004B2B
	public void loopResume()
	{
		LeanTween.resume(GameObject.Find("Cube1"));
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x0000693C File Offset: 0x00004B3C
	public void punchTest()
	{
		LeanTween.moveX(this.ltLogo, 7f, 1f).setEase(LeanTweenType.punch).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x040004ED RID: 1261
	public AnimationCurve customAnimationCurve;

	// Token: 0x040004EE RID: 1262
	public Transform pt1;

	// Token: 0x040004EF RID: 1263
	public Transform pt2;

	// Token: 0x040004F0 RID: 1264
	public Transform pt3;

	// Token: 0x040004F1 RID: 1265
	public Transform pt4;

	// Token: 0x040004F2 RID: 1266
	public Transform pt5;

	// Token: 0x040004F3 RID: 1267
	private int exampleIter;

	// Token: 0x040004F4 RID: 1268
	private string[] exampleFunctions = new string[]
	{
		"updateValue3Example",
		"loopTestClamp",
		"loopTestPingPong",
		"moveOnACurveExample",
		"customTweenExample",
		"moveExample",
		"rotateExample",
		"scaleExample",
		"updateValueExample",
		"delayedCallExample",
		"alphaExample",
		"moveLocalExample",
		"rotateAroundExample",
		"colorExample"
	};

	// Token: 0x040004F5 RID: 1269
	public bool useEstimatedTime = true;

	// Token: 0x040004F6 RID: 1270
	private GameObject ltLogo;

	// Token: 0x040004F7 RID: 1271
	private TestingZLegacy.TimingType timingType;

	// Token: 0x040004F8 RID: 1272
	private int descrTimeScaleChangeId;

	// Token: 0x040004F9 RID: 1273
	private Vector3 origin;

	// Token: 0x020000E5 RID: 229
	// (Invoke) Token: 0x0600049A RID: 1178
	public delegate void NextFunc();

	// Token: 0x020000E6 RID: 230
	public enum TimingType
	{
		// Token: 0x040004FB RID: 1275
		SteadyNormalTime,
		// Token: 0x040004FC RID: 1276
		IgnoreTimeScale,
		// Token: 0x040004FD RID: 1277
		HalfTimeScale,
		// Token: 0x040004FE RID: 1278
		VariableTimeScale,
		// Token: 0x040004FF RID: 1279
		Length
	}
}
