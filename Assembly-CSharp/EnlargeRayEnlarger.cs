using System;
using UnityEngine;

// Token: 0x02000057 RID: 87
public class EnlargeRayEnlarger : MonoBehaviour
{
	// Token: 0x0600018A RID: 394 RVA: 0x000048B7 File Offset: 0x00002AB7
	private void Start()
	{
		this.startTime = Time.time;
		this.startScale = base.transform.localScale;
	}

	// Token: 0x0600018B RID: 395 RVA: 0x00032708 File Offset: 0x00030908
	public void LateUpdate()
	{
		float time = Mathf.Clamp01((Time.time - this.startTime) / this.scaleTime);
		base.transform.localScale = Vector3.Lerp(this.startScale, this.maxScale, this.scaleCurve.Evaluate(time));
	}

	// Token: 0x040001E2 RID: 482
	public AnimationCurve scaleCurve;

	// Token: 0x040001E3 RID: 483
	public Vector3 maxScale = new Vector3(5f, 5f, 5f);

	// Token: 0x040001E4 RID: 484
	public float scaleTime = 1f;

	// Token: 0x040001E5 RID: 485
	private float startTime;

	// Token: 0x040001E6 RID: 486
	private Vector3 startScale;
}
