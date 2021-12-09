using System;
using UnityEngine;

// Token: 0x020000DE RID: 222
public class ExampleSpline : MonoBehaviour
{
	// Token: 0x0600045C RID: 1116 RVA: 0x0003F7A8 File Offset: 0x0003D9A8
	private void Start()
	{
		this.spline = new LTSpline(new Vector3[]
		{
			this.trans[0].position,
			this.trans[1].position,
			this.trans[2].position,
			this.trans[3].position,
			this.trans[4].position
		});
		this.ltLogo = GameObject.Find("LeanTweenLogo1");
		this.ltLogo2 = GameObject.Find("LeanTweenLogo2");
		LeanTween.moveSpline(this.ltLogo2, this.spline.pts, 1f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong().setOrientToPath(true);
		LeanTween.moveSpline(this.ltLogo2, new Vector3[]
		{
			Vector3.zero,
			Vector3.zero,
			new Vector3(1f, 1f, 1f),
			new Vector3(2f, 1f, 1f),
			new Vector3(2f, 1f, 1f)
		}, 1.5f).setUseEstimatedTime(true);
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x0003F900 File Offset: 0x0003DB00
	private void Update()
	{
		this.ltLogo.transform.position = this.spline.point(this.iter);
		this.iter += Time.deltaTime * 0.1f;
		if (this.iter > 1f)
		{
			this.iter = 0f;
		}
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x000066F1 File Offset: 0x000048F1
	private void OnDrawGizmos()
	{
		if (this.spline != null)
		{
			this.spline.gizmoDraw(-1f);
		}
	}

	// Token: 0x040004BC RID: 1212
	public Transform[] trans;

	// Token: 0x040004BD RID: 1213
	private LTSpline spline;

	// Token: 0x040004BE RID: 1214
	private GameObject ltLogo;

	// Token: 0x040004BF RID: 1215
	private GameObject ltLogo2;

	// Token: 0x040004C0 RID: 1216
	private float iter;
}
