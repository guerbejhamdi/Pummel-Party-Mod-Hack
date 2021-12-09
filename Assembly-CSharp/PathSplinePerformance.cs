using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E1 RID: 225
public class PathSplinePerformance : MonoBehaviour
{
	// Token: 0x0600046B RID: 1131 RVA: 0x0003FE88 File Offset: 0x0003E088
	private void Start()
	{
		Application.targetFrameRate = 240;
		List<Vector3> list = new List<Vector3>();
		float num = 0f;
		int num2 = this.trackNodes + 1;
		for (int i = 0; i < num2; i++)
		{
			float x = Mathf.Cos(num * 0.017453292f) * this.circleLength + UnityEngine.Random.Range(0f, this.randomRange);
			float z = Mathf.Sin(num * 0.017453292f) * this.circleLength + UnityEngine.Random.Range(0f, this.randomRange);
			list.Add(new Vector3(x, 1f, z));
			num += 360f / (float)this.trackNodes;
		}
		list[0] = list[list.Count - 1];
		list.Add(list[1]);
		list.Add(list[2]);
		this.track = new LTSpline(list.ToArray());
		this.carAdd = this.carSpeed / this.track.distance;
		this.tracerSpeed = this.track.distance / (this.carSpeed * 1.2f);
		LeanTween.moveSpline(this.trackTrailRenderers, this.track, this.tracerSpeed).setOrientToPath(true).setRepeat(-1);
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x0003FFCC File Offset: 0x0003E1CC
	private void Update()
	{
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
		this.track.place(this.car.transform, this.trackPosition);
		this.trackPosition += Time.deltaTime * this.carAdd;
		if (this.trackPosition > 1f)
		{
			this.trackPosition = 0f;
		}
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x000067A3 File Offset: 0x000049A3
	private void OnDrawGizmos()
	{
		if (this.track != null)
		{
			this.track.drawGizmo(Color.red);
		}
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x0003FD3C File Offset: 0x0003DF3C
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

	// Token: 0x040004D6 RID: 1238
	public GameObject trackTrailRenderers;

	// Token: 0x040004D7 RID: 1239
	public GameObject car;

	// Token: 0x040004D8 RID: 1240
	public GameObject carInternal;

	// Token: 0x040004D9 RID: 1241
	public float circleLength = 10f;

	// Token: 0x040004DA RID: 1242
	public float randomRange = 1f;

	// Token: 0x040004DB RID: 1243
	public int trackNodes = 30;

	// Token: 0x040004DC RID: 1244
	public float carSpeed = 30f;

	// Token: 0x040004DD RID: 1245
	public float tracerSpeed = 2f;

	// Token: 0x040004DE RID: 1246
	private LTSpline track;

	// Token: 0x040004DF RID: 1247
	private int trackIter = 1;

	// Token: 0x040004E0 RID: 1248
	private float carAdd;

	// Token: 0x040004E1 RID: 1249
	private float trackPosition;
}
