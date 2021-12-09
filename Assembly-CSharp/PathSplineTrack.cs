using System;
using UnityEngine;

// Token: 0x020000E2 RID: 226
public class PathSplineTrack : MonoBehaviour
{
	// Token: 0x06000470 RID: 1136 RVA: 0x000400FC File Offset: 0x0003E2FC
	private void Start()
	{
		this.track = new LTSpline(new Vector3[]
		{
			this.trackOnePoints[0].position,
			this.trackOnePoints[1].position,
			this.trackOnePoints[2].position,
			this.trackOnePoints[3].position,
			this.trackOnePoints[4].position,
			this.trackOnePoints[5].position,
			this.trackOnePoints[6].position
		});
		LeanTween.moveSpline(this.trackTrailRenderers, this.track, 2f).setOrientToPath(true).setRepeat(-1);
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x000401CC File Offset: 0x0003E3CC
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
		this.trackPosition += Time.deltaTime * 0.03f;
		if (this.trackPosition < 0f)
		{
			this.trackPosition = 1f;
			return;
		}
		if (this.trackPosition > 1f)
		{
			this.trackPosition = 0f;
		}
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x000067BD File Offset: 0x000049BD
	private void OnDrawGizmos()
	{
		LTSpline.drawGizmo(this.trackOnePoints, Color.red);
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x0003FD3C File Offset: 0x0003DF3C
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

	// Token: 0x040004E2 RID: 1250
	public GameObject car;

	// Token: 0x040004E3 RID: 1251
	public GameObject carInternal;

	// Token: 0x040004E4 RID: 1252
	public GameObject trackTrailRenderers;

	// Token: 0x040004E5 RID: 1253
	public Transform[] trackOnePoints;

	// Token: 0x040004E6 RID: 1254
	private LTSpline track;

	// Token: 0x040004E7 RID: 1255
	private int trackIter = 1;

	// Token: 0x040004E8 RID: 1256
	private float trackPosition;
}
