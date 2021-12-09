using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000E8 RID: 232
public class TestingZLegacyExt : MonoBehaviour
{
	// Token: 0x060004A0 RID: 1184 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Awake()
	{
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x0000697A File Offset: 0x00004B7A
	private void Start()
	{
		this.ltLogo = GameObject.Find("LeanTweenLogo").transform;
		LeanTween.delayedCall(1f, new Action(this.cycleThroughExamples));
		this.origin = this.ltLogo.position;
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x00006881 File Offset: 0x00004A81
	private void pauseNow()
	{
		Time.timeScale = 0f;
		Debug.Log("pausing");
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x00040DCC File Offset: 0x0003EFCC
	private void OnGUI()
	{
		string text = this.useEstimatedTime ? "useEstimatedTime" : ("timeScale:" + Time.timeScale.ToString());
		GUI.Label(new Rect(0.03f * (float)Screen.width, 0.03f * (float)Screen.height, 0.5f * (float)Screen.width, 0.3f * (float)Screen.height), text);
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x00006897 File Offset: 0x00004A97
	private void endlessCallback()
	{
		Debug.Log("endless");
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x00040E3C File Offset: 0x0003F03C
	private void cycleThroughExamples()
	{
		if (this.exampleIter == 0)
		{
			int num = (int)(this.timingType + 1);
			if (num > 4)
			{
				num = 0;
			}
			this.timingType = (TestingZLegacyExt.TimingType)num;
			this.useEstimatedTime = (this.timingType == TestingZLegacyExt.TimingType.IgnoreTimeScale);
			Time.timeScale = (this.useEstimatedTime ? 0f : 1f);
			if (this.timingType == TestingZLegacyExt.TimingType.HalfTimeScale)
			{
				Time.timeScale = 0.5f;
			}
			if (this.timingType == TestingZLegacyExt.TimingType.VariableTimeScale)
			{
				this.descrTimeScaleChangeId = base.gameObject.LeanValue(0.01f, 10f, 3f).setOnUpdate(delegate(float val)
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
		base.gameObject.LeanDelayedCall(delayTime, new Action(this.cycleThroughExamples)).setUseEstimatedTime(this.useEstimatedTime);
		this.exampleIter = ((this.exampleIter + 1 >= this.exampleFunctions.Length) ? 0 : (this.exampleIter + 1));
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x00040F88 File Offset: 0x0003F188
	public void updateValue3Example()
	{
		Debug.Log("updateValue3Example Time:" + Time.time.ToString());
		base.gameObject.LeanValue(new Action<Vector3>(this.updateValue3ExampleCallback), new Vector3(0f, 270f, 0f), new Vector3(30f, 270f, 180f), 0.5f).setEase(LeanTweenType.easeInBounce).setRepeat(2).setLoopPingPong().setOnUpdateVector3(new Action<Vector3>(this.updateValue3ExampleUpdate)).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x0000398C File Offset: 0x00001B8C
	public void updateValue3ExampleUpdate(Vector3 val)
	{
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x000069B9 File Offset: 0x00004BB9
	public void updateValue3ExampleCallback(Vector3 val)
	{
		this.ltLogo.transform.eulerAngles = val;
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x00041024 File Offset: 0x0003F224
	public void loopTestClamp()
	{
		Debug.Log("loopTestClamp Time:" + Time.time.ToString());
		Transform transform = GameObject.Find("Cube1").transform;
		transform.localScale = new Vector3(1f, 1f, 1f);
		transform.LeanScaleZ(4f, 1f).setEase(LeanTweenType.easeOutElastic).setRepeat(7).setLoopClamp().setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x000410A4 File Offset: 0x0003F2A4
	public void loopTestPingPong()
	{
		Debug.Log("loopTestPingPong Time:" + Time.time.ToString());
		Transform transform = GameObject.Find("Cube2").transform;
		transform.localScale = new Vector3(1f, 1f, 1f);
		transform.LeanScaleY(4f, 1f).setEase(LeanTweenType.easeOutQuad).setLoopPingPong(4).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x00041120 File Offset: 0x0003F320
	public void colorExample()
	{
		GameObject.Find("LCharacter").LeanColor(new Color(1f, 0f, 0f, 0.5f), 0.5f).setEase(LeanTweenType.easeOutBounce).setRepeat(2).setLoopPingPong().setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x00041178 File Offset: 0x0003F378
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
		this.ltLogo.LeanMove(to, 1f).setEase(LeanTweenType.easeOutQuad).setOrientToPath(true).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x00041258 File Offset: 0x0003F458
	public void customTweenExample()
	{
		string str = "customTweenExample starting pos:";
		string str2 = this.ltLogo.position.ToString();
		string str3 = " origin:";
		Vector3 vector = this.origin;
		Debug.Log(str + str2 + str3 + vector.ToString());
		this.ltLogo.LeanMoveX(-10f, 0.5f).setEase(this.customAnimationCurve).setUseEstimatedTime(this.useEstimatedTime);
		this.ltLogo.LeanMoveX(0f, 0.5f).setEase(this.customAnimationCurve).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x00041308 File Offset: 0x0003F508
	public void moveExample()
	{
		Debug.Log("moveExample");
		this.ltLogo.LeanMove(new Vector3(-2f, -1f, 0f), 0.5f).setUseEstimatedTime(this.useEstimatedTime);
		this.ltLogo.LeanMove(this.origin, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x0004137C File Offset: 0x0003F57C
	public void rotateExample()
	{
		Debug.Log("rotateExample");
		Hashtable hashtable = new Hashtable();
		hashtable.Add("yo", 5.0);
		this.ltLogo.LeanRotate(new Vector3(0f, 360f, 0f), 1f).setEase(LeanTweenType.easeOutQuad).setOnComplete(new Action<object>(this.rotateFinished)).setOnCompleteParam(hashtable).setOnUpdate(new Action<float>(this.rotateOnUpdate)).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x0000398C File Offset: 0x00001B8C
	public void rotateOnUpdate(float val)
	{
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x00040A78 File Offset: 0x0003EC78
	public void rotateFinished(object hash)
	{
		Hashtable hashtable = hash as Hashtable;
		string str = "rotateFinished hash:";
		object obj = hashtable["yo"];
		Debug.Log(str + ((obj != null) ? obj.ToString() : null));
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x00041410 File Offset: 0x0003F610
	public void scaleExample()
	{
		Debug.Log("scaleExample");
		Vector3 localScale = this.ltLogo.localScale;
		this.ltLogo.LeanScale(new Vector3(localScale.x + 0.2f, localScale.y + 0.2f, localScale.z + 0.2f), 1f).setEase(LeanTweenType.easeOutBounce).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x00041480 File Offset: 0x0003F680
	public void updateValueExample()
	{
		Debug.Log("updateValueExample");
		Hashtable hashtable = new Hashtable();
		hashtable.Add("message", "hi");
		base.gameObject.LeanValue(new Action<float, object>(this.updateValueExampleCallback), this.ltLogo.eulerAngles.y, 270f, 1f).setEase(LeanTweenType.easeOutElastic).setOnUpdateParam(hashtable).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x000414F8 File Offset: 0x0003F6F8
	public void updateValueExampleCallback(float val, object hash)
	{
		Vector3 eulerAngles = this.ltLogo.eulerAngles;
		eulerAngles.y = val;
		this.ltLogo.transform.eulerAngles = eulerAngles;
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x000069CC File Offset: 0x00004BCC
	public void delayedCallExample()
	{
		Debug.Log("delayedCallExample");
		LeanTween.delayedCall(0.5f, new Action(this.delayedCallExampleCallback)).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x0004152C File Offset: 0x0003F72C
	public void delayedCallExampleCallback()
	{
		Debug.Log("Delayed function was called");
		Vector3 localScale = this.ltLogo.localScale;
		this.ltLogo.LeanScale(new Vector3(localScale.x - 0.2f, localScale.y - 0.2f, localScale.z - 0.2f), 0.5f).setEase(LeanTweenType.easeInOutCirc).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x0004159C File Offset: 0x0003F79C
	public void alphaExample()
	{
		Debug.Log("alphaExample");
		GameObject gameObject = GameObject.Find("LCharacter");
		gameObject.LeanAlpha(0f, 0.5f).setUseEstimatedTime(this.useEstimatedTime);
		gameObject.LeanAlpha(1f, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x00041600 File Offset: 0x0003F800
	public void moveLocalExample()
	{
		Debug.Log("moveLocalExample");
		GameObject gameObject = GameObject.Find("LCharacter");
		Vector3 localPosition = gameObject.transform.localPosition;
		gameObject.LeanMoveLocal(new Vector3(0f, 2f, 0f), 0.5f).setUseEstimatedTime(this.useEstimatedTime);
		gameObject.LeanMoveLocal(localPosition, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x000069FA File Offset: 0x00004BFA
	public void rotateAroundExample()
	{
		Debug.Log("rotateAroundExample");
		GameObject.Find("LCharacter").LeanRotateAround(Vector3.up, 360f, 1f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x00006A30 File Offset: 0x00004C30
	public void loopPause()
	{
		GameObject.Find("Cube1").LeanPause();
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x00006A41 File Offset: 0x00004C41
	public void loopResume()
	{
		GameObject.Find("Cube1").LeanResume();
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x00006A52 File Offset: 0x00004C52
	public void punchTest()
	{
		this.ltLogo.LeanMoveX(7f, 1f).setEase(LeanTweenType.punch).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x04000502 RID: 1282
	public AnimationCurve customAnimationCurve;

	// Token: 0x04000503 RID: 1283
	public Transform pt1;

	// Token: 0x04000504 RID: 1284
	public Transform pt2;

	// Token: 0x04000505 RID: 1285
	public Transform pt3;

	// Token: 0x04000506 RID: 1286
	public Transform pt4;

	// Token: 0x04000507 RID: 1287
	public Transform pt5;

	// Token: 0x04000508 RID: 1288
	private int exampleIter;

	// Token: 0x04000509 RID: 1289
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

	// Token: 0x0400050A RID: 1290
	public bool useEstimatedTime = true;

	// Token: 0x0400050B RID: 1291
	private Transform ltLogo;

	// Token: 0x0400050C RID: 1292
	private TestingZLegacyExt.TimingType timingType;

	// Token: 0x0400050D RID: 1293
	private int descrTimeScaleChangeId;

	// Token: 0x0400050E RID: 1294
	private Vector3 origin;

	// Token: 0x020000E9 RID: 233
	// (Invoke) Token: 0x060004BF RID: 1215
	public delegate void NextFunc();

	// Token: 0x020000EA RID: 234
	public enum TimingType
	{
		// Token: 0x04000510 RID: 1296
		SteadyNormalTime,
		// Token: 0x04000511 RID: 1297
		IgnoreTimeScale,
		// Token: 0x04000512 RID: 1298
		HalfTimeScale,
		// Token: 0x04000513 RID: 1299
		VariableTimeScale,
		// Token: 0x04000514 RID: 1300
		Length
	}
}
