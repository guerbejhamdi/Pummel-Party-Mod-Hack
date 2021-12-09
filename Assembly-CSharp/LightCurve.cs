using System;
using UnityEngine;

// Token: 0x0200045B RID: 1115
public class LightCurve : MonoBehaviour
{
	// Token: 0x06001E7B RID: 7803 RVA: 0x000C548C File Offset: 0x000C368C
	private void Start()
	{
		this.light_target = base.GetComponent<Light>();
		if (this.light_target != null)
		{
			if (this.curve_type == LightCurveType.Range || this.curve_type == LightCurveType.RangeAndIntensity)
			{
				this.start_range = this.light_target.range;
				this.light_target.range = this.start_range * this.range_curve.Evaluate(this.counter / this.animation_time);
			}
			if (this.curve_type == LightCurveType.Intensity || this.curve_type == LightCurveType.RangeAndIntensity)
			{
				this.start_intensity = this.light_target.intensity;
				this.light_target.intensity = this.start_intensity * this.intensity_curve.Evaluate(this.counter / this.animation_time);
			}
		}
	}

	// Token: 0x06001E7C RID: 7804 RVA: 0x000C5550 File Offset: 0x000C3750
	private void Update()
	{
		if (this.light_target == null)
		{
			return;
		}
		this.counter += Time.deltaTime;
		if (this.curve_type == LightCurveType.Range || this.curve_type == LightCurveType.RangeAndIntensity)
		{
			this.light_target.range = this.start_range * this.range_curve.Evaluate(this.counter / this.animation_time);
		}
		if (this.curve_type == LightCurveType.Intensity || this.curve_type == LightCurveType.RangeAndIntensity)
		{
			this.light_target.intensity = this.start_intensity * this.intensity_curve.Evaluate(this.counter / this.animation_time);
		}
	}

	// Token: 0x04002163 RID: 8547
	public AnimationCurve range_curve;

	// Token: 0x04002164 RID: 8548
	public AnimationCurve intensity_curve;

	// Token: 0x04002165 RID: 8549
	public LightCurveType curve_type;

	// Token: 0x04002166 RID: 8550
	public float animation_time;

	// Token: 0x04002167 RID: 8551
	private float counter;

	// Token: 0x04002168 RID: 8552
	private Light light_target;

	// Token: 0x04002169 RID: 8553
	private float start_range;

	// Token: 0x0400216A RID: 8554
	private float start_intensity;
}
