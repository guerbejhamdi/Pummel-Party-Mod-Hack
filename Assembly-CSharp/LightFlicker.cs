using System;
using UnityEngine;

// Token: 0x0200045D RID: 1117
public class LightFlicker : MonoBehaviour
{
	// Token: 0x06001E7E RID: 7806 RVA: 0x000167DF File Offset: 0x000149DF
	private void Start()
	{
		this.target_light = base.GetComponent<Light>();
		this.original_color = this.target_light.color;
	}

	// Token: 0x06001E7F RID: 7807 RVA: 0x000167FE File Offset: 0x000149FE
	private void Update()
	{
		this.target_light.color = this.original_color * this.EvalWave();
	}

	// Token: 0x06001E80 RID: 7808 RVA: 0x000C55F8 File Offset: 0x000C37F8
	private float EvalWave()
	{
		float num = (Time.time + this.phase) * this.frequency;
		num -= Mathf.Floor(num);
		float num2;
		switch (this.flicker_function)
		{
		case FlickerFunction.Sin:
			num2 = Mathf.Sin(num * 2f * 3.1415927f);
			break;
		case FlickerFunction.Triangle:
			if (num < 0.5f)
			{
				num2 = 4f * num - 1f;
			}
			else
			{
				num2 = -4f * num + 3f;
			}
			break;
		case FlickerFunction.Square:
			if (num < 0.5f)
			{
				num2 = 1f;
			}
			else
			{
				num2 = -1f;
			}
			break;
		case FlickerFunction.Saw:
			num2 = num;
			break;
		case FlickerFunction.InvertedSaw:
			num2 = 1f - num;
			break;
		case FlickerFunction.Noise:
			num2 = 1f - UnityEngine.Random.value * 2f;
			break;
		case FlickerFunction.LerpNoise:
		{
			this.lerp_counter += Time.deltaTime;
			if (this.lerp_counter < this.lerp_time)
			{
				return Mathf.Lerp(this.start_amplitude, this.goal_amplitude, this.lerp_counter / this.lerp_time);
			}
			float result = Mathf.Lerp(this.start_amplitude, this.goal_amplitude, this.lerp_counter / this.lerp_time);
			this.lerp_counter = 0f;
			this.goal_amplitude = UnityEngine.Random.Range(this.min_noise, this.max_noise);
			this.start_amplitude = result;
			return result;
		}
		default:
			num2 = 1f;
			break;
		}
		return (1f + (num2 * this.amplitude + this.base_start)) / 2f;
	}

	// Token: 0x04002173 RID: 8563
	public FlickerFunction flicker_function;

	// Token: 0x04002174 RID: 8564
	public float base_start;

	// Token: 0x04002175 RID: 8565
	public float amplitude = 1f;

	// Token: 0x04002176 RID: 8566
	public float phase;

	// Token: 0x04002177 RID: 8567
	public float frequency = 0.5f;

	// Token: 0x04002178 RID: 8568
	public float lerp_time = 0.25f;

	// Token: 0x04002179 RID: 8569
	public float min_noise;

	// Token: 0x0400217A RID: 8570
	public float max_noise = 1f;

	// Token: 0x0400217B RID: 8571
	private Color original_color;

	// Token: 0x0400217C RID: 8572
	private Light target_light;

	// Token: 0x0400217D RID: 8573
	private float lerp_counter;

	// Token: 0x0400217E RID: 8574
	private float start_amplitude = 1f;

	// Token: 0x0400217F RID: 8575
	private float goal_amplitude = 1f;
}
