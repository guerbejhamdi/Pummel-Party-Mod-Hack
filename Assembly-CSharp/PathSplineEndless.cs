using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E0 RID: 224
public class PathSplineEndless : MonoBehaviour
{
	// Token: 0x06000463 RID: 1123 RVA: 0x0003FA28 File Offset: 0x0003DC28
	private void Start()
	{
		for (int i = 0; i < 4; i++)
		{
			this.addRandomTrackPoint();
		}
		this.refreshSpline();
		LeanTween.value(base.gameObject, 0f, 0.3f, 2f).setOnUpdate(delegate(float val)
		{
			this.pushTrackAhead = val;
		});
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x0003FA7C File Offset: 0x0003DC7C
	private void Update()
	{
		if (this.trackPts[this.trackPts.Count - 1].z - base.transform.position.z < 200f)
		{
			this.addRandomTrackPoint();
			this.refreshSpline();
		}
		this.track.place(this.car.transform, this.carIter);
		this.carIter += this.carAdd * Time.deltaTime;
		this.track.place(this.trackTrailRenderers.transform, this.carIter + this.pushTrackAhead);
		float axis = Input.GetAxis("Horizontal");
		if (Input.anyKeyDown)
		{
			if (axis < 0f && this.trackIter > 0)
			{
				this.trackIter--;
				this.playSwish();
			}
			else if (axis > 0f && this.trackIter < 2)
			{
				this.trackIter++;
				this.playSwish();
			}
			LeanTween.moveLocalX(this.carInternal, (float)(this.trackIter - 1) * 6f, 0.3f).setEase(LeanTweenType.easeOutBack);
		}
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x0000672F File Offset: 0x0000492F
	private GameObject objectQueue(GameObject[] arr, ref int lastIter)
	{
		lastIter = ((lastIter >= arr.Length - 1) ? 0 : (lastIter + 1));
		arr[lastIter].transform.localScale = Vector3.one;
		arr[lastIter].transform.rotation = Quaternion.identity;
		return arr[lastIter];
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x0003FBA8 File Offset: 0x0003DDA8
	private void addRandomTrackPoint()
	{
		float num = Mathf.PerlinNoise(0f, this.randomIter);
		this.randomIter += this.randomIterWidth;
		Vector3 vector = new Vector3((num - 0.5f) * 20f, 0f, (float)this.zIter * 40f);
		this.objectQueue(this.cubes, ref this.cubesIter).transform.position = vector;
		GameObject gameObject = this.objectQueue(this.trees, ref this.treesIter);
		float num2 = (this.zIter % 2 == 0) ? -15f : 15f;
		gameObject.transform.position = new Vector3(vector.x + num2, 0f, (float)this.zIter * 40f);
		LeanTween.rotateAround(gameObject, Vector3.forward, 0f, 1f).setFrom((this.zIter % 2 == 0) ? 180f : -180f).setEase(LeanTweenType.easeOutBack);
		this.trackPts.Add(vector);
		if (this.trackPts.Count > this.trackMaxItems)
		{
			this.trackPts.RemoveAt(0);
		}
		this.zIter++;
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x0003FCE0 File Offset: 0x0003DEE0
	private void refreshSpline()
	{
		this.track = new LTSpline(this.trackPts.ToArray());
		this.carIter = this.track.ratioAtPoint(this.car.transform.position);
		this.carAdd = 40f / this.track.distance;
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x0003FD3C File Offset: 0x0003DF3C
	private void playSwish()
	{
		AnimationCurve volume = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0.005464481f, 1.83897f, 0f),
			new Keyframe(0.1114856f, 2.281785f, 0f, 0f),
			new Keyframe(0.2482903f, 2.271654f, 0f, 0f),
			new Keyframe(0.3f, 0.01670286f, 0f, 0f)
		});
		AnimationCurve frequency = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0.00136725f, 0f, 0f),
			new Keyframe(0.1482391f, 0.005405405f, 0f, 0f),
			new Keyframe(0.2650336f, 0.002480127f, 0f, 0f)
		});
		LeanAudio.play(LeanAudio.createAudio(volume, frequency, LeanAudio.options().setVibrato(new Vector3[]
		{
			new Vector3(0.2f, 0.5f, 0f)
		}).setWaveNoise().setWaveNoiseScale(1000f)));
	}

	// Token: 0x040004C5 RID: 1221
	public GameObject trackTrailRenderers;

	// Token: 0x040004C6 RID: 1222
	public GameObject car;

	// Token: 0x040004C7 RID: 1223
	public GameObject carInternal;

	// Token: 0x040004C8 RID: 1224
	public GameObject[] cubes;

	// Token: 0x040004C9 RID: 1225
	private int cubesIter;

	// Token: 0x040004CA RID: 1226
	public GameObject[] trees;

	// Token: 0x040004CB RID: 1227
	private int treesIter;

	// Token: 0x040004CC RID: 1228
	public float randomIterWidth = 0.1f;

	// Token: 0x040004CD RID: 1229
	private LTSpline track;

	// Token: 0x040004CE RID: 1230
	private List<Vector3> trackPts = new List<Vector3>();

	// Token: 0x040004CF RID: 1231
	private int zIter;

	// Token: 0x040004D0 RID: 1232
	private float carIter;

	// Token: 0x040004D1 RID: 1233
	private float carAdd;

	// Token: 0x040004D2 RID: 1234
	private int trackMaxItems = 15;

	// Token: 0x040004D3 RID: 1235
	private int trackIter = 1;

	// Token: 0x040004D4 RID: 1236
	private float pushTrackAhead;

	// Token: 0x040004D5 RID: 1237
	private float randomIter;
}
