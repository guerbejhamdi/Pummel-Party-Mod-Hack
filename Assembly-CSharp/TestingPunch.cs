using System;
using UnityEngine;

// Token: 0x020000C4 RID: 196
public class TestingPunch : MonoBehaviour
{
	// Token: 0x060003FD RID: 1021 RVA: 0x00006327 File Offset: 0x00004527
	private void Start()
	{
		Debug.Log("exported curve:" + this.curveToString(this.exportCurve));
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x0003C2CC File Offset: 0x0003A4CC
	private void Update()
	{
		LeanTween.dtManual = Time.deltaTime;
		if (Input.GetKeyDown(KeyCode.Q))
		{
			LeanTween.moveLocalX(base.gameObject, 5f, 1f).setOnComplete(delegate()
			{
				Debug.Log("on complete move local X");
			}).setOnCompleteOnStart(true);
			GameObject gameObject = GameObject.Find("DirectionalLight");
			Light lt = gameObject.GetComponent<Light>();
			LeanTween.value(lt.gameObject, lt.intensity, 0f, 1.5f).setEase(LeanTweenType.linear).setLoopPingPong().setRepeat(-1).setOnUpdate(delegate(float val)
			{
				lt.intensity = val;
			});
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			MonoBehaviour.print("scale punch!");
			TestingPunch.tweenStatically(base.gameObject);
			LeanTween.scale(base.gameObject, new Vector3(1.15f, 1.15f, 1.15f), 0.6f);
			LeanTween.rotateAround(base.gameObject, Vector3.forward, -360f, 0.3f).setOnComplete(delegate()
			{
				LeanTween.rotateAround(base.gameObject, Vector3.forward, -360f, 0.4f).setOnComplete(delegate()
				{
					LeanTween.scale(base.gameObject, new Vector3(1f, 1f, 1f), 0.1f);
					LeanTween.value(base.gameObject, delegate(float v)
					{
					}, 0f, 1f, 0.3f).setDelay(1f);
				});
			});
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
			Vector3[] to = new Vector3[]
			{
				new Vector3(-1f, 0f, 0f),
				new Vector3(0f, 0f, 0f),
				new Vector3(4f, 0f, 0f),
				new Vector3(20f, 0f, 0f)
			};
			this.descr = LeanTween.move(base.gameObject, to, 15f).setOrientToPath(true).setDirection(1f).setOnComplete(delegate()
			{
				Debug.Log("move path finished");
			});
		}
		if (Input.GetKeyDown(KeyCode.Y))
		{
			this.descr.setDirection(-this.descr.direction);
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			LeanTween.rotateAroundLocal(base.gameObject, base.transform.forward, -80f, 5f).setPoint(new Vector3(1.25f, 0f, 0f));
			MonoBehaviour.print("rotate punch!");
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			MonoBehaviour.print("move punch!");
			Time.timeScale = 0.25f;
			float start = Time.realtimeSinceStartup;
			LeanTween.moveX(base.gameObject, 1f, 1f).setOnComplete(new Action<object>(this.destroyOnComp)).setOnCompleteParam(base.gameObject).setOnComplete(delegate()
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				float num = realtimeSinceStartup - start;
				Debug.Log(string.Concat(new string[]
				{
					"start:",
					start.ToString(),
					" end:",
					realtimeSinceStartup.ToString(),
					" diff:",
					num.ToString(),
					" x:",
					this.gameObject.transform.position.x.ToString()
				}));
			}).setEase(LeanTweenType.easeInBack).setOvershoot(this.overShootValue).setPeriod(0.3f);
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			LeanTween.color(base.gameObject, new Color(1f, 0f, 0f, 0.5f), 1f);
			Color to2 = new Color(UnityEngine.Random.Range(0f, 1f), 0f, UnityEngine.Random.Range(0f, 1f), 0f);
			LeanTween.color(GameObject.Find("LCharacter"), to2, 4f).setLoopPingPong(1).setEase(LeanTweenType.easeOutBounce);
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			LeanTween.delayedCall(base.gameObject, 0.3f, new Action<object>(this.delayedMethod)).setRepeat(4).setOnCompleteOnRepeat(true).setOnCompleteParam("hi");
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			LeanTween.value(base.gameObject, new Action<Color>(this.updateColor), new Color(1f, 0f, 0f, 1f), Color.blue, 4f);
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			LeanTween.delayedCall(0.05f, new Action<object>(this.enterMiniGameStart)).setOnCompleteParam(new object[]
			{
				5.ToString() ?? ""
			});
		}
		if (Input.GetKeyDown(KeyCode.U))
		{
			LeanTween.value(base.gameObject, delegate(Vector2 val)
			{
				base.transform.position = new Vector3(val.x, base.transform.position.y, base.transform.position.z);
			}, new Vector2(0f, 0f), new Vector2(5f, 100f), 1f).setEase(LeanTweenType.easeOutBounce);
			GameObject l = GameObject.Find("LCharacter");
			string str = "x:";
			Vector3 position = l.transform.position;
			string str2 = position.x.ToString();
			string str3 = " y:";
			position = l.transform.position;
			Debug.Log(str + str2 + str3 + position.y.ToString());
			LeanTween.value(l, new Vector2(l.transform.position.x, l.transform.position.y), new Vector2(l.transform.position.x, l.transform.position.y + 5f), 1f).setOnUpdate(delegate(Vector2 val)
			{
				string str4 = "tweening vec2 val:";
				Vector2 vector = val;
				Debug.Log(str4 + vector.ToString());
				l.transform.position = new Vector3(val.x, val.y, this.transform.position.z);
			}, null);
		}
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x0003C868 File Offset: 0x0003AA68
	private static void tweenStatically(GameObject gameObject)
	{
		Debug.Log("Starting to tween...");
		LeanTween.value(gameObject, delegate(float val)
		{
			Debug.Log("tweening val:" + val.ToString());
		}, 0f, 1f, 1f);
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x0003C8B4 File Offset: 0x0003AAB4
	private void enterMiniGameStart(object val)
	{
		Debug.Log("level:" + int.Parse((string)((object[])val)[0]).ToString());
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x00006344 File Offset: 0x00004544
	private void updateColor(Color c)
	{
		GameObject.Find("LCharacter").GetComponent<Renderer>().material.color = c;
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x0003C8EC File Offset: 0x0003AAEC
	private void delayedMethod(object myVal)
	{
		string str = myVal as string;
		Debug.Log("delayed call:" + Time.time.ToString() + " myVal:" + str);
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x00006360 File Offset: 0x00004560
	private void destroyOnComp(object p)
	{
		UnityEngine.Object.Destroy((GameObject)p);
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x0003C924 File Offset: 0x0003AB24
	private string curveToString(AnimationCurve curve)
	{
		string text = "";
		for (int i = 0; i < curve.length; i++)
		{
			text = string.Concat(new string[]
			{
				text,
				"new Keyframe(",
				curve[i].time.ToString(),
				"f, ",
				curve[i].value.ToString(),
				"f)"
			});
			if (i < curve.length - 1)
			{
				text += ", ";
			}
		}
		return "new AnimationCurve( " + text + " )";
	}

	// Token: 0x04000458 RID: 1112
	public AnimationCurve exportCurve;

	// Token: 0x04000459 RID: 1113
	public float overShootValue = 1f;

	// Token: 0x0400045A RID: 1114
	private LTDescr descr;
}
